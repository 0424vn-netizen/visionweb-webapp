using AS.Common.DBManager;
using AS.Common.Logger;
using AS.StatementPDFReportCommon;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WebSupergoo.ABCpdf9;

namespace AS.StatementPDFReport
{
    public class ProcessStatement
    {
        public void Start(string tmpTable, string password, string exportPath)
        {
            LoggerManager.Debug("\n\n----- Start exporting... -----");

            // Get list statements
            List<StatementItem> lstStatement = GetStatementList(tmpTable, exportPath);

            if (lstStatement != null && lstStatement.Any())
            {
                try
                {
                    string folderPath = lstStatement[0].FilePath;
                    // Generate Folder Name
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    string filename = "";
                    string[] itemArray;
                    StringBuilder sbGood = new StringBuilder();
                    StringBuilder sbBad = new StringBuilder();

                    sbGood.AppendFormat("{0}DateTime: {1}{2}", Environment.NewLine, DateTime.Now.ToString(), Environment.NewLine);
                    sbBad.AppendFormat("{0}DateTime: {1}{2}", Environment.NewLine, DateTime.Now.ToString(), Environment.NewLine);

                    foreach (var item in lstStatement)
                    {
                        Doc mergeDoc = new Doc();
                        filename = folderPath + "\\" + item.FileNameReal;
                        try
                        {
                            byte[] doc = DocServices.DownloadDoc(int.Parse(item.DocId));
                            if (doc.Length != 0)
                            {
                                // Delete file exist
                                if (File.Exists(filename))
                                {
                                    //Skip generating pdf
                                    continue;
                                }

                                mergeDoc.Read(doc);
                                mergeDoc.Encryption.Type = 5;
                                mergeDoc.Encryption.SetCryptMethods(CryptMethodType.AESV3);
                                mergeDoc.Encryption.Password = password;
                                mergeDoc.Save(filename);
                                mergeDoc.Clear();

                                UpdateStatusExport(tmpTable, item.MerchantNumber, item.FileNameReal);
                                LoggerManager.Debug("\n\n----- Export success! Path: " + filename + "-----");
                                itemArray = filename.Split('\\');
                                sbGood.AppendFormat("{0} - {1} - Filename: {2}", item.MerchantNumber, item.DocId, itemArray[itemArray.Length - 1]);
                                sbGood.Append(Environment.NewLine);
                            }
                            else
                            {
                                LoggerManager.Debug("\n\n----- Document " + item.DocId + " not exists -----");
                                sbBad.AppendFormat("{0} -- {1} -- Filename: {2}", item.MerchantNumber, item.DocId, "ERROR - Cannot extract the statement file.");
                                sbBad.Append(Environment.NewLine);
                            }
                        }
                        catch (Exception ex)
                        {
                            LoggerManager.Error(ex.Message);
                            sbBad.AppendFormat("{0} - {1} - Filename: {2}", item.MerchantNumber, item.DocId, "ERROR - Cannot extract the statement file.");
                            sbBad.Append(Environment.NewLine);
                        }
                    }

                    File.AppendAllText(folderPath + "\\Info_Valid.txt", sbGood.ToString());
                    File.AppendAllText(folderPath + "\\Info_InValid.txt", sbBad.ToString());
                }
                catch (Exception ex)
                {
                    LoggerManager.Debug("\n\n----- Exception while exporting: " + ex + "-----");
                    Application.Exit();
                }
            }
            else
            {
                LoggerManager.Debug("\n\n----- No statement to export! ---");
                Application.Exit();
            }
        }

        public void StartOld(string tmpTable, string password, string exportPath)
        {
            LoggerManager.Debug("\n\n----- Start exporting... -----");

            // Get list statements
            List<StatementItem> lstStatement = GetStatementList(tmpTable, exportPath);

            if (lstStatement != null && lstStatement.Any())
            {
                try
                {
                    Doc mergeDoc = new Doc();
                    string prevMerchantNum = string.Empty;
                    string filename = lstStatement[0].FilePath;

                    foreach (var item in lstStatement)
                    {
                        string currMerchantNum = item.MerchantNumber;
                        byte[] doc = DocServices.DownloadDoc(int.Parse(item.DocId));
                        if (doc.Length != 0)
                        {
                            Doc currentDoc = new Doc();
                            currentDoc.Read(doc);
                            currentDoc.Bookmark.Clear();
                            if (currMerchantNum != prevMerchantNum)
                            {
                                currentDoc.Bookmark.Insert(0, currMerchantNum);
                                prevMerchantNum = currMerchantNum;
                            }
                            mergeDoc.Append(currentDoc);
                        }
                        else
                        {
                            LoggerManager.Debug("\n\n----- Document " + item.DocId + " not exists -----");
                        }
                    }

                    mergeDoc.Encryption.Type = 5;
                    mergeDoc.Encryption.SetCryptMethods(CryptMethodType.AESV3);
                    mergeDoc.Encryption.Password = password;
                    mergeDoc.Save(filename);
                    mergeDoc.Clear();

                    UpdateStatusExport(tmpTable, string.Empty, string.Empty);

                    LoggerManager.Debug("\n\n----- Export success! Path: " + filename + "-----");
                }
                catch (Exception ex)
                {
                    LoggerManager.Debug("\n\n----- Exception while exporting: " + ex + "-----");
                    Application.Exit();
                }
            }
            else
            {
                LoggerManager.Debug("\n\n----- No statement to export! ---");
                Application.Exit();
            }
        }

        // Get statement list
        private List<StatementItem> GetStatementList(string tmpTable, string exportPath)
        {
            Data _coreData = new Data();
            List<StatementItem> lstStatements = new List<StatementItem>();
            FilterParameterCollection _params = new FilterParameterCollection
            {
                new FilterParameter("@TmpTable", tmpTable, DbType.String)
            };
            DataTable dtbStatements = _coreData.GetReports("spp_fis_cs_SelectExtractStatement_ByHierarchy", _params);
            if (dtbStatements != null && dtbStatements.Rows.Count > 0)
            {
                // Generate Folder Name
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }

                foreach (DataRow row in dtbStatements.Rows)
                {
                    lstStatements.Add(new StatementItem
                    {
                        MerchantNumber = row["MerchantNumber"] != DBNull.Value ? row["MerchantNumber"].ToString() : string.Empty,
                        DocId = row["DocId"] != DBNull.Value ? row["DocId"].ToString() : string.Empty,
                        FilePath = exportPath + "\\" + row["OutputFolder"].ToString(),
                        FileNameReal = row["FileName"].ToString()
                    });
                }
            }

            return lstStatements;
        }

        // Update status export 
        private void UpdateStatusExport(string tmpTable, string merchantNumber, string fileName)
        {
            Data _coreData = new Data();
            FilterParameterCollection _params = new FilterParameterCollection
            {
                new FilterParameter("@TmpTable", tmpTable, DbType.String),
                new FilterParameter("@MerchantNumber", merchantNumber, DbType.String),
                new FilterParameter("@FileName", fileName, DbType.String)
            };
            _coreData.ExecuteNonQueryCommand("spp_fis_cs_UpdateStatusExtractStatement_ByHierarchy", _params);
        }
    }
}
