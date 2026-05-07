using AS.Framework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AS.Common.DBManager;
using System.Data;
using System.IO;
using System.Collections;
using System.Threading;
using System.Reflection;
using System.Configuration;
using System.IO.Compression;
using AS.Archive;
using AS.VW.Scheduler.Tasks.Codes.Models;

namespace AS.VW.Scheduler.Tasks
{
    public class ExtractMerchant : BaseTask
    {
        private readonly string _filePath;
        private readonly string _serverId;
        private readonly ITaskManager _taskManager;
        private const string CATEGORY_NAME = "VW Scheduler tasks";

        public ExtractMerchant()
        {
            _filePath = AppConfigurations.ExportTempFolder;
            _taskManager = new TaskManager();

            //Get server ID
            System.Net.IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(Environment.MachineName);
            if (ipEntry.AddressList.Length > 1)
            {
                _serverId = ipEntry.AddressList[1].ToString();
            }
        }

        public override void Execute()
        {
            DoGenerateFiles();
        }

        public void DoGenerateFiles()
        {

            try
            {
                //Get list rows in queue, DB will handle update the status for these rows = 1: Processing
                List<Processer> dataProcess = _taskManager.GetDataProcessInQueue();

                foreach (Processer item in dataProcess)
                {
                    ExportDataExtractMerchant(item);
                }
            }
            catch (Exception ex)
            {

                AS.Common.Logger.LoggerManager.Error("Get list rows in queue : Failed.\n" + ex.ToString());
            }

        }

        private void ExportDataExtractMerchant(Processer pro)
        {
            try
            {
                //Get data
                IDataReader dataResult = _taskManager.GetExtractReport(pro);
                DataTable dataTable = dataResult.GetSchemaTable();

                List<string> listColumns = dataTable.AsEnumerable().Select(c => c.Field<string>("ColumnName")).ToList();
                string filename = pro.FileName;
                uint currNumOfRows = 1;

                //Check file exist
                string filePath =Path.Combine(_filePath, filename);
                if (File.Exists(filePath))
                    File.Delete(filePath);

                using (FileStream file = new FileStream(filePath, FileMode.CreateNew))
                {
                    using (StreamWriter writeFile = new StreamWriter(file))
                    {
                        StringBuilder strContent = new StringBuilder();
                        
                        //Write file
                        if (!string.IsNullOrEmpty(pro.TitleText))
                        {
                            strContent.Append("\"" + pro.TitleText + "\"");
                            strContent.Append(System.Environment.NewLine);
                        }
                        
                        strContent.Append(string.Join(",", listColumns) + System.Environment.NewLine);
                        writeFile.Write(strContent.ToString());
                        strContent = new StringBuilder();
                        while (dataResult.Read())
                        {
                            foreach (var item in listColumns)
                            {
                                strContent.Append("\"" + dataResult[item.Trim()].ToString() + "\"" + ",");
                            }
                            strContent.Append(System.Environment.NewLine);

                            if ((currNumOfRows % AppConfigurations.NumOfRowsToFlush) == 0)
                            {
                                writeFile.Write(strContent.ToString());
                                writeFile.Flush();
                                strContent = new StringBuilder();
                            }

                            currNumOfRows++;
                        }

                        //flush the last of them if it still has data in it.
                        if (strContent.Length > 0)
                        {
                            writeFile.Write(strContent.ToString());
                            writeFile.Flush();
                        }
                        AS.Common.Logger.LoggerManager.Debug(string.Format("LogID: {0} - Done writing to file.", pro.LogID));
                        //End write file
                    }
                }

                dataResult.Dispose();

                /* Get file's size, if it is greater than the threshold, the app will zip it.*/
                FileInfo fi = new FileInfo(filePath);
                int fileSize = (int)(fi.Length / (1024 * 1024)); //B -> KB -> = MB
                bool isZippingSuccess = false;
                AS.Common.Logger.LoggerManager.Debug(string.Format("LogID: {0} - File size: {1} MB.", pro.LogID, fileSize));

                /*Zipping files*/
                //thresholdToZip <= 0 -> not zip - normal mode
                if (AppConfigurations.ThresholdToZip > 0 && (fileSize >= AppConfigurations.ThresholdToZip))
                {
                    AS.Common.Logger.LoggerManager.Debug(string.Format("LogID: {0} - Start zipping file.", pro.LogID));
                    //check file size: only zip if the file is greate than the threshold
                    isZippingSuccess = ZipFile(filePath, filePath.Replace(".csv", ".zip"));
                    if (isZippingSuccess)
                    {
                        //update filename, and update db
                        filename = filename.Replace(".csv", ".zip");
                    }
                    else
                    {
                        //do nothing - just log
                        AS.Common.Logger.LoggerManager.Error(string.Format("Fail to zip file. LogID:{0}, file name:{1}, Client ID:{2}\n", pro.LogID, filename, pro.ASClientID));
                    }
                    AS.Common.Logger.LoggerManager.Debug(string.Format("LogID: {0} - End zipping file.", pro.LogID));
                }

                //Generating file is complete, Upload to Doc server
                long docID;
                bool success = UploadFileToDocServer(pro.ASClientID, pro.CreatedBy, _filePath, filename, _serverId, out docID);
                if (success)
                {
                    //Delete file at physical folder     
                    DeleteFile(filePath);                       //delete csv file
                    DeleteFile(Path.Combine(_filePath, filename));    //delete zip file

                    // Success
                    // Update status
                    _taskManager.UpdateStatus(pro, docID.ToString(), ProcessStatus.Success, (isZippingSuccess ? FileNameExtension.FileType.zip.ToString() : FileNameExtension.FileType.csv.ToString()));
                }
                else
                {
                    // Upload Fail
                    // Update status
                    _taskManager.UpdateStatus(pro, "-1", ProcessStatus.Fail, string.Empty);
                    AS.Common.Logger.LoggerManager.Error(string.Format("Upload file to doc server: failed. LogID:{0}, file name:{1}, Client ID:{2}\n", pro.LogID, filename, pro.ASClientID));
                }
            }
            catch (Exception ex)
            {
                // Export Fail
                // Update status
                _taskManager.UpdateStatus(pro, "-1", ProcessStatus.Fail, string.Empty);
                AS.Common.Logger.LoggerManager.Error("Export file in folder temp: failed");
                AS.Common.Logger.LoggerManager.Error("Exporting file:\n" + ex.ToString());
            }

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

        private bool UploadFileToDocServer(int clientID, string entityID, string filePath, string fileName, string serverIP, out long docID)
        {
            docID = -1;
            try
            {
                using (FileStream fs = new FileStream(Path.Combine(filePath, fileName), FileMode.Open, FileAccess.Read))
                {
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, (int)fs.Length);
                    docID = WebServices.DocServices.UploadDoc(clientID, entityID, buffer, fileName, serverIP, CATEGORY_NAME);
                }
            }
            catch (Exception ex)
            {
                AS.Common.Logger.LoggerManager.Error("Upload file: Can't upload file to document server.\n" + ex.ToString());
            }

            return docID != -1;
        }

        public bool ZipFile(string orgFileName, string zipFileName)
        {
            try
            {
                SevenZip.SevenZipDllPath = Path.Combine(AppConfigurations.BaseDirectory, "7z.dll");
                SevenZip.Pack(ArchiveType.Zip, zipFileName, new string[] { orgFileName }, null);

                return true;
            }
            catch (Exception e)
            {
                AS.Common.Logger.LoggerManager.Error("Error within zipping file, error msg: " + e.Message + ", at: " + DateTime.Now);
                return false;
            }
        }
    }
}
