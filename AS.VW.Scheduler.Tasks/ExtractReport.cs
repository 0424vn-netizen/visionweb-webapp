using AS.Common.Logger;
using AS.Common.WebServiceExport;
using AS.Framework.Services;
using AS.VW.Scheduler.Tasks.Codes.Models;
using AS.VW.Scheduler.Tasks.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Scheduler.Tasks
{
    public class ExtractReport : BaseTask
    {
        private readonly string _serverId;
        private readonly ITaskManager _taskManager;
        private const string CATEGORY_NAME = "VW Scheduler tasks";
        private readonly List<Task> listTaskCreateTemplate = new List<Task>();
        private readonly Dictionary<string, string> _listTemplate = new Dictionary<string, string>();

        public ExtractReport()
        {
            LoggerManager.Debug("App init.\n");
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
                LoggerManager.Debug("Process Export Data Extract Start");
                //Get list rows in queue, DB will handle update the status for these rows = 1: Processing
                List<Processer> dataProcess = _taskManager.GetDataProcessInQueue(AppConfigurations.ExtractMode);
                dataProcess.ForEach(item => item.ExtractMode = AppConfigurations.ExtractMode);
                List<Task> listTask = new List<Task>();
                int numberTask = 0;
                foreach (Processer item in dataProcess)
                {
                    while (numberTask >= AppConfigurations.MaxThread)
                    {
                        Task.WaitAll(listTask.ToArray());
                        numberTask = 0; // Reset task list
                    }

                    numberTask++;
                    var task = new Task(() =>
                    {
                        ExportDataExtract(item);
                    });

                    listTask.Add(task);
                    task.Start();
                }

                Task.WaitAll(listTask.ToArray());
            }
            catch (Exception ex)
            {
                AS.Common.Logger.LoggerManager.Error("Get list rows in queue : Failed.\n" + ex.ToString());
            }
        }

        private void ExportDataExtract(Processer pro)
        {
            try
            {
                // Get the total numnber of rows data 
                DataSet dataSet = _taskManager.GetExtractReport(pro, true);
                if (dataSet.Tables.Count > 0)
                {
                    DataTable getTotalRow = dataSet.Tables[0];
                    DataTable dataFilter = dataSet.Tables[1];
                    int totalRow = (getTotalRow != null) ? int.Parse(getTotalRow.Rows[0]["TotalRows"].ToString()) : -1;
                    LoggerManager.Debug(string.Format("LogID: {0} - Total Row: {1}", pro.LogID, totalRow));

                    string _filePath = AppConfigurations.ExportTempFolderReport;
                    // Check switch Export
                    var exportType = dataFilter.AsEnumerable().Where(x => x.Field<string>("CategoryKey").Equals("Export")).FirstOrDefault();
                    FileNameExtension.FileType fileType = totalRow >= AppConfigurations.NumberOfRowToSwitch || (exportType != null && exportType["StrFilterKey"].ToString().Trim().ToLower().Equals(FileNameExtension.FileType.csv.ToString(), StringComparison.OrdinalIgnoreCase))
                        ? FileNameExtension.FileType.csv
                        : FileNameExtension.FileType.xlsx;

                    string reportType = dataFilter.Rows[0]["StrFilterKey"].ToString();
                    string reportName = dataFilter.Rows[0]["StrFilterText"].ToString();
                    ExportTemplate exTemplate = GeneralFuncLibraries.GetExportTempate(reportType);
                    List<string> fileNameGenerates = new List<string>();

                    if (fileType == FileNameExtension.FileType.xlsx)
                    {
                        // Prepare data for export
                        PrepareNewTemplate(dataFilter, exTemplate, reportName);
                        WSExportParameter parameters = PrepareParameters(dataFilter, reportName, exTemplate.Path);
                        fileNameGenerates.Add(ExportExcel(pro, parameters, _filePath, exTemplate, dataFilter));
                    }
                    else if (fileType == FileNameExtension.FileType.csv)
                    {
                        fileNameGenerates = ExportCSV(pro, dataFilter, _filePath, exTemplate, totalRow);
                    }
                    else
                    {
                        LoggerManager.Error(string.Format("LogID: {0} - The extension ({1}) dont support at the time.", pro.LogID, fileType.ToString()));
                    }

                    if (fileNameGenerates.Count > 0)
                    {
                        CallThreadExportDone(pro, fileNameGenerates, fileType, _filePath);
                    }
                    else
                    {
                        LoggerManager.Error(string.Format("LogID: {0} - Failed while exporting file.", pro.LogID));
                        _taskManager.UpdateStatus(pro, "-1", ProcessStatus.Fail, string.Empty, AppConfigurations.ExtractMode);
                    }
                }
            }
            catch (Exception ex)
            {
                // Export Fail
                LoggerManager.Error(string.Format("LogID: {0} - Exporting file: Failed: {1}", pro.LogID, ex.ToString()));
                _taskManager.UpdateStatus(pro, "-1", ProcessStatus.Fail, string.Empty, AppConfigurations.ExtractMode);
            }
        }

        private void GetInfoDataFilter(DataTable dataFilter, Dictionary<string, string> dicInfoFile, Dictionary<string, string> dicFilter)
        {
            foreach (DataRow item in dataFilter.Rows)
            {
                string categoryKey = item["CategoryKey"].ToString();
                string strFilterText = item["StrFilterText"].ToString();
                string strFilterKey = item["StrFilterKey"].ToString();
                if (categoryKey.Equals("ReportType", StringComparison.OrdinalIgnoreCase))
                {
                    dicInfoFile.Add(strFilterKey, strFilterText);
                }
                else
                {
                    dicFilter.Add(categoryKey, strFilterText);
                }
            }
        }

        private WSExportParameter PrepareParametersForFilterSheet(DataTable dataFilter, ExportTemplate exTemplate, string sheetName)
        {
            Dictionary<string, object> extra = new Dictionary<string, object>();
            Dictionary<string, string> _dicInfoFile = new Dictionary<string, string>();
            Dictionary<string, string> _dicFilter = new Dictionary<string, string>();

            bool hasTemplate = false;
            GetInfoDataFilter(dataFilter, _dicInfoFile, _dicFilter);

            extra.Add("{[SubTable]}", _dicFilter);
            if (!extra.ContainsKey("{[REPORT_TITLE]}"))
                extra.Add("{[REPORT_TITLE]}", sheetName);

            WSExportParameter param = new WSExportParameter()
            {
                ColumnFormatter = string.Empty,
                ExportColumnFormatsForAuto = string.Empty,
                ExportColumnHeaders = string.Empty,
                ExportColumnNames = string.Empty,
                ExportTotalColumnNames = string.Empty,
                PrefixTemplatePath = "SchedulerExtractReport",
                IsAutoExportTemplate = !hasTemplate,
                IsCacheTemplateFile = !hasTemplate,
                IsUsedExtraValues = !hasTemplate,
                ExtraValues = extra,
                ReportHeader = sheetName,
                StoredProcName = string.Empty,
                TemplateExcel = exTemplate.Path
            };

            return param;
        }

        private void PrepareNewTemplate(DataTable dataFilter, ExportTemplate exTemplate, string sheetName)
        {
            Dictionary<string, object> extra = new Dictionary<string, object>();
            Dictionary<string, string> _dicInfoFile = new Dictionary<string, string>();
            Dictionary<string, string> _dicFilter = new Dictionary<string, string>();
            StringBuilder columnFormatter = new StringBuilder();
            StringBuilder exportColumnFormatsForAuto = new StringBuilder();
            StringBuilder exportColumnHeaders = new StringBuilder();
            StringBuilder exportColumnNames = new StringBuilder();

            var filterSheet = exTemplate.Sheets.FirstOrDefault(x => x.Name.Equals(sheetName));
            filterSheet = filterSheet == null ? exTemplate.Sheets.First() : filterSheet;

            bool hasTemplate = false;
            string templateExcel = string.Empty;

            GetInfoDataFilter(dataFilter, _dicInfoFile, _dicFilter);
            extra.Add("{[SubTable]}", _dicFilter);
            templateExcel = exTemplate.Path;

            if (exTemplate.IsCreateNewTemplate && !_listTemplate.ContainsKey(templateExcel)
                && !_listTemplate.ContainsKey("Delete_template_" + templateExcel))
            {
                _listTemplate.Add("Delete_template_" + templateExcel, string.Empty);
                var task = new Task(() =>
                {
                    DeleteTemplate(templateExcel);
                });
                task.Start();
                listTaskCreateTemplate.Add(task);
            }
            Task.WaitAll(listTaskCreateTemplate.ToArray());

            hasTemplate = ExistsTemplate(templateExcel);

            if (!hasTemplate)
            {
                foreach (ColumnTemplate item in filterSheet.Columns)
                {
                    columnFormatter.AppendFormat("{0}@{1}@,", item.DataKey, item.ASFormat);
                    exportColumnFormatsForAuto.AppendFormat("{0},", item.ASFormat);
                    exportColumnHeaders.AppendFormat("{0},", item.Name);
                    exportColumnNames.AppendFormat("{0},", item.DataKey);
                }

                WSExportParameter param = new WSExportParameter()
                {
                    ColumnFormatter = columnFormatter.Length > 0 ? columnFormatter.ToString().Remove(columnFormatter.Length - 1) : string.Empty,
                    ExportColumnFormatsForAuto = exportColumnFormatsForAuto.Length > 0 ? exportColumnFormatsForAuto.ToString().Remove(exportColumnFormatsForAuto.Length - 1) : string.Empty,
                    ExportColumnHeaders = exportColumnHeaders.Length > 0 ? exportColumnHeaders.ToString().Remove(exportColumnHeaders.Length - 1) : string.Empty,
                    ExportColumnNames = exportColumnNames.Length > 0 ? exportColumnNames.ToString().Remove(exportColumnNames.Length - 1) : string.Empty,
                    ExportTotalColumnNames = string.Empty,
                    PrefixTemplatePath = "SchedulerExtractReport",
                    IsAutoExportTemplate = !hasTemplate,
                    IsCacheTemplateFile = !hasTemplate,
                    IsUsedExtraValues = !hasTemplate,
                    ExtraValues = extra,
                    ReportHeader = sheetName,
                    StoredProcName = string.Empty,
                    TemplateExcel = templateExcel
                };

                // create new Template
                if (!_listTemplate.ContainsKey(templateExcel))
                {
                    var task = new Task(() =>
                    {
                        _listTemplate.Add(templateExcel, string.Empty);
                        GeneralFuncLibraries.CreateExcel2007Template("Data", "REPORT_TITLE", param, exTemplate, string.Format("{0}App_Data", AppConfigurations.BaseDirectory) + templateExcel);
                    });
                    task.Start();
                    listTaskCreateTemplate.Add(task);
                }
            }

            Task.WaitAll(listTaskCreateTemplate.ToArray());
        }

        private WSExportParameter PrepareParameters(DataTable dataFilter, string sheetName, string templateExcel)
        {
            Dictionary<string, object> extra = new Dictionary<string, object>();
            Dictionary<string, string> _dicInfoFile = new Dictionary<string, string>();
            Dictionary<string, string> _dicFilter = new Dictionary<string, string>();

            GetInfoDataFilter(dataFilter, _dicInfoFile, _dicFilter);
            extra.Add("{[SubTable]}", _dicFilter);
            WSExportParameter wsExportParameter = new WSExportParameter()
            {
                ExportColumnHeaders = string.Empty,
                ExportColumnNames = string.Empty,
                ExportTotalColumnNames = string.Empty,
                ExportColumnFormatsForAuto = string.Empty,
                PrefixTemplatePath = "SchedulerExtractReport",
                IsAutoExportTemplate = false,
                IsCacheTemplateFile = false,
                IsUsedExtraValues = false,
                ExtraValues = extra,
                ReportHeader = sheetName,
                StoredProcName = string.Empty,
                TemplateExcel = templateExcel
            };
            return wsExportParameter;
        }

        private string ExportExcel(Processer pro, WSExportParameter wsExportParameter, string filePath, ExportTemplate exTemplate, DataTable dataFilter)
        {
            WSExportCore wSExportCore = new WSExportCore();
            string fileName = pro.FileName;
            IDataReader dataResult = _taskManager.GetExtractReport(pro);
            wsExportParameter.FileName = fileName;
            string fileResourse = string.Format("{0}App_Data", AppConfigurations.BaseDirectory);
            LoggerManager.Debug(string.Format("LogID: {0} - Process Export Excel File: {1}", pro.LogID, fileName));

            if (dataResult != null)
            {
                if (!string.IsNullOrEmpty(exTemplate.AssemblyType) && !string.IsNullOrEmpty(exTemplate.AssemblyName))
                {
                    IExtension extension = (IExtension)Activator.CreateInstance(exTemplate.AssemblyName, exTemplate.AssemblyType).Unwrap();
                    DataTable data = extension.Excute(dataResult) as DataTable;
                    dataResult = data.CreateDataReader();
                    data.Dispose();
                }

                string templateFile = string.Empty;

                if (exTemplate.HasFilterSheet)
                {
                    templateFile = filePath + "\\" + Guid.NewGuid() + ".xlsx";
                    File.Copy(fileResourse + wsExportParameter.TemplateExcel.ToString(), templateFile, true);
                    var parameterFilterSheet = PrepareParametersForFilterSheet(dataFilter, exTemplate, "Filters");
                    GeneralFuncLibraries.CreateFilterSheet2007(templateFile, parameterFilterSheet);
                    wsExportParameter.TemplateExcel = templateFile;
                    fileResourse = string.Empty;
                }

                string exportedFileName = wSExportCore.ExportToExcel2007Extra(dataResult, wsExportParameter, null, filePath, fileResourse);

                if (exTemplate.HasFilterSheet)
                {
                    File.Delete(templateFile);
                }

                dataResult.Dispose();
                return exportedFileName;
            }

            return string.Empty;
        }

        private List<string> ExportCSV(Processer pro, DataTable dataFilter, string filePath, ExportTemplate exTemplate, int totalRows)
        {
            if (pro.ExtractMode == ExtractMode.MERCHANT_ALERT_WORKED && totalRows > AppConfigurations.NumberOfRowPerFile)
            {
                return ExportCSVMultiFile(pro, dataFilter, filePath, exTemplate, totalRows);
            }
            else
            {
                return ExportCSVSingleFile(pro, dataFilter, filePath, exTemplate);
            }
        }

        private void CallThreadExportDone(Processer pro, List<string> fileNameGenerates, FileNameExtension.FileType fileType, string filePath)
        {
            /* Get file's size, if it is greater than the threshold, the app will zip it.*/
            var fileName = GeneralFuncLibraries.ConvertFileNameExtension(pro.FileName, fileType.ToString());

            // Rename file
            if (fileType == FileNameExtension.FileType.xlsx && fileNameGenerates.Count > 0)
            {
                bool isRenameSuccess = RenameFile(filePath, fileNameGenerates[0], fileName);
                if (!isRenameSuccess)
                {
                    LoggerManager.Error(string.Format("LogID: {0} - Can not rename file: {1} to {2}", pro.LogID, fileNameGenerates.ToString(), fileName));
                    _taskManager.UpdateStatus(pro, "-1", ProcessStatus.Fail, AppConfigurations.ExtractMode, string.Empty);
                    return;
                }
            }

            // Zip file
            var isZipFile = false;
            if (fileNameGenerates.Count > 1)
            {
                isZipFile = true;
            }
            else
            {
                FileInfo fi = new FileInfo(string.Format("{0}\\{1}", filePath, fileName));
                int fileSize = (int)(fi.Length / (1024 * 1024)); //B -> KB -> = MB
                isZipFile = fileSize >= AppConfigurations.ThresholdToZip || fileNameGenerates.Count > 1;
            }

            if (isZipFile)
            {
                fileType = FileNameExtension.FileType.zip;
                string fileSource = string.Format("{0}\\{1}", filePath, fileName);
                LoggerManager.Debug(string.Format("LogID: {0} - Process Zip File: {1}", pro.LogID, fileSource));

                // Update filename
                fileName = GeneralFuncLibraries.ConvertFileNameExtension(fileName, "zip");
                string fileDestination = GeneralFuncLibraries.ConvertFileNameExtension(fileSource, "zip");
                var zipFileSuccess = false;
                if (fileNameGenerates.Count == 1)
                {
                    zipFileSuccess = GeneralFuncLibraries.SevenZipFile(fileSource, fileDestination);
                }
                else
                {
                    List<string> fileSources = new List<string>();
                    foreach (var file in fileNameGenerates)
                    {
                        fileSources.Add(string.Format("{0}\\{1}", filePath, file));
                    }
                    zipFileSuccess = ZipFiles(fileSources, fileDestination);
                }
                if (!zipFileSuccess)
                {
                    LoggerManager.Error(string.Format("Fail to zip file. LogID:{0}, file name:{1}, Client ID:{2}\n", pro.LogID, fileSource, pro.ASClientID));
                    _taskManager.UpdateStatus(pro, "-1", ProcessStatus.Fail, AppConfigurations.ExtractMode, string.Empty);
                    return;
                }

                LoggerManager.Debug(string.Format("LogID: {0} - Process Zip File END: {1}", pro.LogID, fileName));
            }

            // Upload to Doc server
            UploadFile(pro, fileName, fileType, filePath);
        }

        private void UploadFile(Processer pro, string fileName, FileNameExtension.FileType fileType, string filePath)
        {
            List<long> uploadedParts;
            bool uploadFileSuccess = GeneralFuncLibraries.UploadFileToDocServer(pro.ASClientID, pro.CreatedBy, filePath, fileName, _serverId, CATEGORY_NAME, out uploadedParts, AppConfigurations.ChunkSizeInKB);
            if (uploadFileSuccess)
            {
                // Update status
                _taskManager.UpdateStatus(pro, string.Join(",", uploadedParts), ProcessStatus.Success, fileType.ToString(), AppConfigurations.ExtractMode);
                LoggerManager.Debug(string.Format("LogID: {0} - Upload file success DocID: {1}", pro.LogID, string.Join(",", uploadedParts)));

                // Delete file at physical folder
                if (AppConfigurations.IsDeleteUploadFile && Directory.Exists(filePath))
                {
                    DirectoryInfo dir = new DirectoryInfo(filePath);
                    foreach (FileInfo file in dir.GetFiles())
                    {
                        file.Delete();
                    }

                    Directory.Delete(filePath);
                }
            }
            else
            {
                // Upload Fail
                _taskManager.UpdateStatus(pro, "-1", ProcessStatus.Fail, string.Empty, AppConfigurations.ExtractMode);
                LoggerManager.Error(string.Format("LogID: {0} - Upload file {1} to doc server: Failed", pro.LogID, fileName));
            }
        }

        private bool RenameFile(string filePath, string fileNameGenerate, string fileNameReal)
        {
            string pathGenerate = string.Format("{0}\\{1}", filePath, fileNameGenerate);
            string pathReal = string.Format("{0}\\{1}", filePath, fileNameReal);
            if (!string.IsNullOrEmpty(pathGenerate) && File.Exists(pathGenerate))
            {
                try
                {
                    File.Move(pathGenerate, pathReal);
                    return true;
                }
                catch (Exception ex)
                {
                    LoggerManager.Error("Rename File: Failed\n" + ex.ToString());
                    return false;
                }
            }
            return true;
        }

        private bool ExistsTemplate(string filePath)
        {
            string fileResourse = string.Format("{0}App_Data", AppConfigurations.BaseDirectory);
            return File.Exists(fileResourse + filePath);
        }

        private void DeleteTemplate(string filePath)
        {
            if (ExistsTemplate(filePath))
            {
                string fileResourse = string.Format("{0}App_Data", AppConfigurations.BaseDirectory);
                File.Delete(fileResourse + filePath);
            }
        }

        private List<string> ExportCSVSingleFile(Processer pro, DataTable dataFilter, string filePath, ExportTemplate exTemplate)
        {
            List<string> result = new List<string>();
            Dictionary<string, string> _dicInfoFile = new Dictionary<string, string>();
            Dictionary<string, string> _dicFilter = new Dictionary<string, string>();

            string filename = GeneralFuncLibraries.ConvertFileNameExtension(pro.FileName, FileNameExtension.FileType.csv.ToString());

            IDataReader dataResult = _taskManager.GetExtractReport(pro);
            DataTable dataTable = dataResult.GetSchemaTable();

            List<string> listColumnName = dataTable.AsEnumerable().Select(c => c.Field<string>("ColumnName")).ToList();
            List<string> listColumnKey = dataTable.AsEnumerable().Select(c => c.Field<string>("ColumnName")).ToList();

            if (exTemplate.Sheets.First().Columns.Count > 0)
            {
                listColumnName = exTemplate.Sheets.First().Columns.Select(x => x.Name).ToList();
                listColumnKey = exTemplate.Sheets.First().Columns.Select(x => x.DataKey).ToList();
            }

            // Get from DataFilter.
            GetInfoDataFilter(dataFilter, _dicInfoFile, _dicFilter);

            uint currNumOfRows = 1;

            string filePathReal = string.Format("{0}\\{1}", filePath, filename);

            using (FileStream file = new FileStream(filePathReal, FileMode.CreateNew))
            {
                using (StreamWriter writeFile = new StreamWriter(file))
                {
                    StringBuilder strContent = new StringBuilder();

                    // Write file
                    writeFile.Write(strContent.ToString());
                    strContent = new StringBuilder();

                    foreach (var item in _dicFilter)
                    {
                        strContent.Append("\"" + item.Key.ToString() + "\"" + ",");
                        strContent.Append("\"" + item.Value.ToString() + "\"" + ",");
                    }

                    currNumOfRows++;
                    strContent.Append(System.Environment.NewLine);
                    strContent.Append(System.Environment.NewLine);
                    strContent.Append(string.Join(",", listColumnName) + System.Environment.NewLine);

                    while (dataResult.Read())
                    {
                        foreach (var item in listColumnKey)
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

                    // flush the last of them if it still has data in it.
                    if (strContent.Length > 0)
                    {
                        writeFile.Write(strContent.ToString());
                        writeFile.Flush();
                    }

                    LoggerManager.Debug(string.Format("LogID: {0} - Done writing to file.", pro.LogID));
                }
            }

            dataResult.Dispose();
            result.Add(filename);
            return result;
        }

        public List<string> ExportCSVMultiFile(Processer pro, DataTable dataFilter, string filePath, ExportTemplate exTemplate, int totalRows)
        {
            Dictionary<string, string> _dicInfoFile = new Dictionary<string, string>();
            Dictionary<string, string> _dicFilter = new Dictionary<string, string>();

            string baseFileName = Path.GetFileNameWithoutExtension(pro.FileName);
            string fileExtension = ".csv";

            DataSet dataSet = _taskManager.GetExtractReport(pro, true);
            DataTable dataTable = dataSet.Tables[2];

            List<string> listColumnName = dataTable.Columns.Cast<DataColumn>().Select(col => col.ColumnName).ToList();
            List<string> listColumnKey = new List<string>(listColumnName);

            if (exTemplate.Sheets.First().Columns.Count > 0)
            {
                listColumnName = exTemplate.Sheets.First().Columns.Select(x => x.Name).ToList();
                listColumnKey = exTemplate.Sheets.First().Columns.Select(x => x.DataKey).ToList();
            }

            GetInfoDataFilter(dataFilter, _dicInfoFile, _dicFilter);

            var reportDate = _dicFilter.FirstOrDefault(f => f.Key == "Report Date");
            string[] parts = reportDate.Value.Split('-');
            DateTime fromDate = DateTime.ParseExact(parts[0].Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            DateTime toDate = fromDate;
            if (parts.Length > 1)
            {
                toDate = DateTime.ParseExact(parts[1].Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            }

            int pageSize = AppConfigurations.NumberOfRowPerFile;
            uint flushThreshold = AppConfigurations.NumOfRowsToFlush;
            int flushCounter = 0;
            int totalRowCounter = 0;
            int currentPage = 1;

            List<string> generatedFiles = new List<string>();
            StreamWriter writer = null;
            string currentFileName = null;

            try
            {
                for (DateTime date = fromDate; date <= toDate; date = date.AddDays(1))
                {
                    IDataReader reader = _taskManager.GetExtractReport(pro, date);

                    while (reader != null && reader.Read())
                    {
                        if (writer == null || totalRowCounter >= pageSize)
                        {
                            SafeCloseWriter(writer, currentFileName, pro.LogID);
                            writer = null;
                            currentFileName = null;

                            string suffix = currentPage.ToString("D2");
                            currentFileName = $"{baseFileName}_{suffix}{fileExtension}";
                            string fullPath = Path.Combine(filePath, currentFileName);

                            try
                            {
                                writer = new StreamWriter(fullPath, false, new UTF8Encoding(false));

                                StringBuilder filterLine = new StringBuilder();
                                foreach (var item in _dicFilter)
                                {
                                    filterLine.Append("\"" + item.Key.ToString() + "\"" + ",");
                                    filterLine.Append("\"" + item.Value.ToString() + "\"" + ",");
                                }
                                writer.WriteLine(filterLine.ToString());
                                writer.WriteLine();
                                writer.WriteLine(string.Join(",", listColumnName));

                                generatedFiles.Add(currentFileName);
                                LoggerManager.Debug($"LogID: {pro.LogID} - Created file {currentFileName}");
                            }
                            catch (Exception ex)
                            {
                                LoggerManager.Debug($"LogID: {pro.LogID} - Cannot open file {fullPath} for writing: {ex}");
                                continue;
                            }

                            totalRowCounter = 0;
                            flushCounter = 0;
                            currentPage++;
                        }

                        string row = string.Join(",", listColumnKey.Select(k =>
                        {
                            object val = reader[k.Trim()];
                            return $"\"{val?.ToString().Replace("\"", "\"\"")}\"";
                        }));

                        writer.WriteLine(row);
                        flushCounter++;
                        totalRowCounter++;

                        if (flushCounter >= flushThreshold)
                        {
                            writer.Flush();
                            flushCounter = 0;
                        }
                    }

                    reader?.Dispose();
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Debug($"LogID: {pro.LogID} - Unexpected error in ExportCSVMultiFile: {ex}");
            }
            finally
            {
                SafeCloseWriter(writer, currentFileName, pro.LogID);
            }

            return generatedFiles;
        }

        public bool ZipFiles(List<string> fileSources, string fileDestination)
        {
            try
            {
                using (FileStream zipToCreate = new FileStream(fileDestination, FileMode.Create))
                using (ZipArchive archive = new ZipArchive(zipToCreate, ZipArchiveMode.Create))
                {
                    foreach (string file in fileSources)
                    {
                        FileInfo fileInfo = new FileInfo(file);
                        archive.CreateEntryFromFile(file, fileInfo.Name);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void SafeCloseWriter(StreamWriter writer, string fileName, int logId)
        {
            if (writer == null) return;

            try
            {
                writer.Flush();
                writer.Dispose();
            }
            catch (Exception ex)
            {
                LoggerManager.Debug($"LogID: {logId} - Error closing file {fileName}: {ex}");
            }
        }
    }
}
