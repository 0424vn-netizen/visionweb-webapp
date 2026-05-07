using AS.Framework.Services;
using AS.VW.Common;
using AS.VW.Entities;
using AS.VW.PDFStatementCreator;
using AS.VW.Repository;
using System;
using System.Collections.Generic;
using System.IO;

namespace AS.VW.Scheduler.Tasks
{
    public class JoinStatementPdf:BaseTask
    {
        private readonly string _filePath;
        private readonly string _serverId ="VW_FIS";
        private readonly int _ClientID;
        private readonly IStatementRepository _statementRepository;
              
        public JoinStatementPdf()
        {
            _filePath = AppConfigurations.ExportTempFolder;
            _statementRepository = RepositoryFactory.Create<IStatementRepository>();
            _ClientID = int.Parse(AppConfigurations.ClientIdDefault);

            //Get server ID
            System.Net.IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(Environment.MachineName);
            if (ipEntry.AddressList.Length > 1)
            {
                _serverId = ipEntry.AddressList[1].ToString();
            }           
        }
        
        public override void Execute()
        {
            try
            {
                List<DownLoadItem> dataProcess = _statementRepository.GetPendingDownloadItems();

                foreach (DownLoadItem item in dataProcess)
                {
                    DoGenerateReport(item);
                }
            }
            catch (Exception ex)
            {
                AS.Common.Logger.LoggerManager.Error("Processer failed.\n" + ex.ToString());
            }
        }

        private void DoGenerateReport(DownLoadItem itemProcesser)
        {
            if (itemProcesser==null) return;        
            long docID=0;

            // Update status fail
            _statementRepository.UpdateStatusDownloadItems(docID, itemProcesser.RecordID, StatementStatus.Processing);
            AS.Common.Logger.LoggerManager.Error(string.Format("Start processer item. ID:{0}, file name:{1}\n", itemProcesser.RecordID, itemProcesser.FileName));

            List<PdfStatementCreatorResult> pdfResults = new List<PdfStatementCreatorResult>();
            var statements = _statementRepository.GetStatementListByDownLoadItemId(itemProcesser.RecordID);
            foreach (var statement in statements)
            {
                IPdfStatementCreator creator = PdfStatementCreatorBase.CreateNewInstance(statement.StatementType, itemProcesser.Language,PdfStatementResultType.Path);
                pdfResults.Add(creator.Execute(statement.ParameterContent, 
                    itemProcesser.ASClientID, itemProcesser.SiteID,
                    itemProcesser.CreatedBy, itemProcesser.UserMode,
                    itemProcesser.ReportDate));
            }
            string fileName = itemProcesser.FileName + ".pdf";
            MergeFromFile(pdfResults, fileName);
            
            bool success = UploadFileToDocServer(_ClientID, itemProcesser.CreatedBy, _filePath, fileName, _serverId, out docID);
            if (success)
            {
                // Delete file in temp folder
                var fullpathFile = $"{_filePath}\\{fileName}";
                DeleteFile(fullpathFile);

                // Update status success
                _statementRepository.UpdateStatusDownloadItems(docID, itemProcesser.RecordID, StatementStatus.Success);
            }
            else
            {
                // Update status fail
                _statementRepository.UpdateStatusDownloadItems(docID, itemProcesser.RecordID, StatementStatus.Fail);
                AS.Common.Logger.LoggerManager.Error(string.Format("Upload file to doc server: failed. ID:{0}, file name:{1}\n", itemProcesser.RecordID, itemProcesser.FileName));
            }
            
        }

        private void MergeFromFile(List<PdfStatementCreatorResult> pdfResults,string fileName)
        {
            var doc = new AbcpdfDoc();
            foreach (PdfStatementCreatorResult item in pdfResults)
            {
                try
                {
                    var tempDoc = new AbcpdfDoc();
                    switch(item.ResultType)
                    {
                        case PdfStatementResultType.Byte:
                            tempDoc.Read(item.Content as byte[]);
                            break;
                        case PdfStatementResultType.Path:
                            tempDoc.Read(FileHandler.ReadFile(item.Content as string));
                            DeleteFile(item.Content as string);
                            break;
                    }

                    doc.Append(tempDoc);
                }
                catch (Exception ex)
                {
                    AS.Common.Logger.LoggerManager.Error("Read file: ABC pdf can not read the file .\n" + ex.ToString());  
                }              
            }

            var pdfReportName = _filePath + fileName;
            try
            {
                doc.Save(pdfReportName);
            }
            catch (Exception ex)
            {
                AS.Common.Logger.LoggerManager.Error("Upload file: ABC pdf can not generate the file .\n" + ex.ToString());
            }

        }

        private bool UploadFileToDocServer(int clientID, string userId, string filePath, string fileName, string serverIP, out long docID)
        {
            docID = -1;
            Stream ReadingStream = null;
            FileInfo UploadedFileInfo = null;
            try
            {                
                UploadedFileInfo = new FileInfo($"{filePath}\\{fileName}");
                ReadingStream = UploadedFileInfo.OpenRead();
                byte[] FileBuffer = new byte[UploadedFileInfo.Length + 1];

                ReadingStream.Read(FileBuffer, 0, Convert.ToInt32(UploadedFileInfo.Length));
                ReadingStream.Close();

                docID = WebServices.DocServices.UploadDoc(clientID, userId, FileBuffer, fileName, serverIP);
            }
            catch (Exception ex)
            {
                AS.Common.Logger.LoggerManager.Error("Upload file: Can't upload file to document server.\n" + ex.ToString());
            }

            return docID != -1;
        }

        private void DeleteFile(string filePath)
        {
            if (filePath != string.Empty && File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                }
                catch (Exception ex)
                {
                    AS.Common.Logger.LoggerManager.Error("Delete File: Failed\n" + ex.ToString());
                }
            }
        }       
    }

}
