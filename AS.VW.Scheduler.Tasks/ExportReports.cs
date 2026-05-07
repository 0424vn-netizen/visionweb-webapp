using AS.Common.Logger;
using AS.Common.WebServiceExport;
using AS.Framework.Services;
using AS.VW.Scheduler.Tasks.Codes.Models;
using AS.VW.Scheduler.Tasks.Entities;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Scheduler.Tasks
{
    public class ExportReports : BaseTask
    {
        private readonly string _serverId;
        private readonly ITaskManager _taskManager;
        private const string CATEGORY_NAME = "VW Scheduler tasks";
        private bool isDebug = AppConfigurations.IsDebug;

        public ExportReports()
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
                    ExportReportConfigResponse reportConfig = _taskManager.GetReportConfig(new ExportReportConfigRequest
                    {
                        AsClientId = item.ASClientID,
                        CategoryCode = item.CategoryCode,
                        SubCategoryCode = item.SubCategoryCode
                    });

                    item.ReportConfig = reportConfig;

                    if (isDebug)
                    {
                        if (item.SubCategoryCode == "1300")// SubCategoryCode want to debug
                            ExportDataExtract(item);
                        else continue;
                        break;  
                    }

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

                    if (!isDebug)
                    {
                        task.Start();
                    }
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
            string templateExcel = string.Empty;
            try
            {
                LoggerManager.Debug(string.Format("ProcessLogID: {0} - Start exporting", pro.ProcessLogID));

                // Determine export type from dataExport
                var exportType = pro.FileType.ToString().Equals(FileNameExtension.FileType.csv.ToString(), StringComparison.OrdinalIgnoreCase)
                    ? FileNameExtension.FileType.csv
                    : FileNameExtension.FileType.xlsx;                

                FileNameExtension.FileType fileType = exportType;

                // Create folder with name from Processer.FileName
                string baseFolderPath = Path.Combine(AppConfigurations.ExportTempFolderReport,
                    Path.GetFileNameWithoutExtension(pro.FileName));

                if (!Directory.Exists(baseFolderPath))
                {
                    Directory.CreateDirectory(baseFolderPath);
                }

                List<string> fileNameGenerates = new List<string>();

                DataTable dataFilter = new DataTable();
                dataFilter.Columns.Add("CategoryKey", typeof(string));
                dataFilter.Columns.Add("StrFilterKey", typeof(string));
                dataFilter.Columns.Add("StrFilterText", typeof(string));
                DataRow row = dataFilter.NewRow();
                row["CategoryKey"] = "ReportType";
                row["StrFilterKey"] = pro.SubCategoryCode;
                row["StrFilterText"] = pro.SubCategoryDescription;
                dataFilter.Rows.Add(row);

                string reportType = dataFilter.Rows[0]["StrFilterKey"].ToString();
                string reportHeader = pro.ReportConfig?.ExportReportFilter?.ReportHeader?.Value;

                ExportTemplate exTemplate = GeneralFuncLibraries.GetExportTempate(reportType);

                string customView = pro.ReportConfig.ExportReportFilter?.CustomView?.Value;
                
                if (fileType == FileNameExtension.FileType.xlsx)
                {
                    LoggerManager.Debug(string.Format("ProcessLogID: {0} - Export as Excel", pro.ProcessLogID));
                    // Prepare data for export
                    templateExcel = PrepareNewTemplate(dataFilter, exTemplate, string.Empty, customView);
                    WSExportParameter parameters = PrepareParameters(dataFilter, reportHeader, exTemplate.Path);
                    parameters.TemplateExcel = templateExcel;
                    LoggerManager.Debug(string.Format("ProcessLogID: {0} - Export as Excel", pro.ProcessLogID));

                    // Get list of generated files
                    fileNameGenerates = ExportExcelPaginated(pro, parameters, baseFolderPath);
                }
                else if (fileType == FileNameExtension.FileType.csv)
                {
                    LoggerManager.Debug(string.Format("ProcessLogID: {0} - Export as CSV", pro.ProcessLogID));

                    // Get list of generated CSV files
                    fileNameGenerates = ExportCSVPaginated(pro, baseFolderPath, exTemplate, reportHeader);
                }
                else
                {
                    LoggerManager.Error(string.Format("ProcessLogID: {0} - The extension ({1}) is not supported.", pro.ProcessLogID, fileType.ToString()));
                    _taskManager.UpdateStatus(pro, "-1", ProcessStatus.Fail, string.Empty, AppConfigurations.ExtractMode);
                }

                if (fileNameGenerates.Count > 0)
                {
                    CallThreadExportDone(pro, fileNameGenerates, fileType, baseFolderPath);
                }
                else
                {
                    LoggerManager.Error(string.Format("ProcessLogID: {0} - Failed while exporting file.", pro.ProcessLogID));
                    _taskManager.UpdateStatus(pro, "-1", ProcessStatus.Fail, string.Empty, AppConfigurations.ExtractMode);
                    CleanupFolder(baseFolderPath);
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Error(string.Format("ProcessLogID: {0} - Exporting file failed: {1}", pro.ProcessLogID, ex.ToString()));
                _taskManager.UpdateStatus(pro, "-1", ProcessStatus.Fail, string.Empty, AppConfigurations.ExtractMode);
            }
            finally
            {
                // Delete template
                try
                {
                    string fileResource = Path.Combine(AppConfigurations.BaseDirectory, "App_Data");
                    string fullPath = fileResource + templateExcel;
                    if (File.Exists(fullPath))
                    {
                        File.Delete(fullPath);
                    }
                }
                catch (Exception ex)
                {
                    LoggerManager.Error("Error when Delete template", ex);
                }
            }
        }

        private string PrepareNewTemplate(DataTable dataFilter, ExportTemplate exTemplate, string sheetName, string customView)
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
            // Get file name and extension
            string fileName = Path.GetFileNameWithoutExtension(exTemplate.Path);
            string extension = Path.GetExtension(exTemplate.Path);
            string directory = Path.GetDirectoryName(exTemplate.Path);
            // Create new filename with guideId
            string newFileName = $"{fileName}_{Guid.NewGuid().ToString()}{extension}";
            // Combine back to full path
            string newPath = Path.Combine(directory, newFileName);
            templateExcel = string.Format("\\{0}", newPath);
          

            if (!string.IsNullOrEmpty(customView))
            {
                string[] arrCustomColumns = customView.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string col in arrCustomColumns)
                {
                    foreach (ColumnTemplate item in filterSheet.Columns)
                    {
                        if (item.DataKey.Equals(col.Trim(), StringComparison.OrdinalIgnoreCase))
                        {
                            string dataKey = !string.IsNullOrEmpty(item.PartialColumn) ? item.PartialColumn : item.DataKey;
                            columnFormatter.AppendFormat("{0}@{1}@,", dataKey, item.ASFormat);
                            exportColumnFormatsForAuto.AppendFormat("{0},", item.ASFormat);
                            exportColumnHeaders.AppendFormat("{0},", item.Name);
                            exportColumnNames.AppendFormat("{0},", dataKey);
                            break;
                        }
                    }
                }
            }
            else
            {
                foreach (ColumnTemplate item in filterSheet.Columns)
                {
                    string dataKey = !string.IsNullOrEmpty(item.PartialColumn) ? item.PartialColumn : item.DataKey;
                    columnFormatter.AppendFormat("{0}@{1}@,", dataKey, item.ASFormat);
                    exportColumnFormatsForAuto.AppendFormat("{0},", item.ASFormat);
                    exportColumnHeaders.AppendFormat("{0},", item.Name);
                    exportColumnNames.AppendFormat("{0},", dataKey);
                }
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
            string outputFile = string.Format("{0}App_Data", AppConfigurations.BaseDirectory) + templateExcel;
            GeneralFuncLibraries.CreateExcel2007Template("Data", "REPORT_TITLE", param, exTemplate, outputFile);
            Debug.WriteLine(string.Format("Finish PrepareNewTemplate for {0}", templateExcel));
            return templateExcel;
        }

        private WSExportParameter PrepareParameters(DataTable dataFilter, string reportHeader, string templateExcel)
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
                ReportHeader = reportHeader,
                StoredProcName = string.Empty,
                TemplateExcel = templateExcel
            };
            return wsExportParameter;
        }            

        private List<string> ExportExcelPaginated(Processer pro, WSExportParameter wsExportParameter, string filePath)
        {
            List<string> fileNameGenerates = new List<string>();

            WSExportCore wSExportCore = new WSExportCore();
            string fileName = Path.GetFileNameWithoutExtension(pro.FileName);
            wsExportParameter.FileName = fileName;
            string fileResourse = string.Format("{0}App_Data", AppConfigurations.BaseDirectory);
            LoggerManager.Debug(string.Format("ProcessLogID: {0} - Process Export Excel File: {1}", pro.ProcessLogID, fileName));

            try
            {
                // Get total rows from database
                DataTable getTotalRow = _taskManager.GetExtractReportAutoBindingParameters(pro, true);
                int totalRows = (getTotalRow != null && getTotalRow.Rows.Count > 0)
                    ? int.Parse(getTotalRow.Rows[0]["TotalRows"].ToString())
                    : 0;

                LoggerManager.Debug(string.Format("ProcessLogID: {0} - Total Row: {1}", pro.ProcessLogID, totalRows));

                int pageSize = AppConfigurations.EXPORTREPORTS_PAGESIZE;
                int pagesPerFile = AppConfigurations.EXPORTREPORTS_PAGES_PER_FILE;

                // If no data, create empty file with headers only
                if (totalRows == 0)
                {
                    LoggerManager.Debug(string.Format("ProcessLogID: {0} - No data to export, creating empty file with headers", pro.ProcessLogID));

                    // Create empty DataTable with column structure
                    DataTable emptyData = new DataTable();

                    // Try to get column structure from first page (will be empty but has schema)
                    IDataReader schemaReader = _taskManager.GetExtractReportAutoBindingParameters(pro, 1, 1);
                    if (schemaReader != null)
                    {
                        DataTable schemaTable = schemaReader.GetSchemaTable();
                        if (schemaTable != null)
                        {
                            List<string> columnNames = schemaTable.AsEnumerable()
                                .Select(c => c.Field<string>("ColumnName")).ToList();

                            foreach (string colName in columnNames)
                            {
                                emptyData.Columns.Add(colName);
                            }
                        }
                        schemaReader.Dispose();
                    }
                    
                    // Single file - no number suffix
                    wsExportParameter.FileName = fileName;
                    
                    string exportedFileName = wSExportCore.ExportToExcel2007Extra(
                        emptyData.CreateDataReader(), wsExportParameter, null, filePath, fileResourse);

                    emptyData.Dispose();
                    LoggerManager.Debug(string.Format("ProcessLogID: {0} - Empty Excel file created: {1}", pro.ProcessLogID, exportedFileName));

                    if (!string.IsNullOrEmpty(exportedFileName))
                    {
                        fileNameGenerates.Add(exportedFileName);
                    }
                    return fileNameGenerates;
                }

                // Calculate total pages needed based on totalRows
                int totalPages = (int)Math.Ceiling((double)totalRows / pageSize);
                
                // Calculate total files that will be generated
                int totalFiles = (int)Math.Ceiling((double)totalPages / pagesPerFile);
                bool isSingleFile = totalFiles == 1;
                
                LoggerManager.Debug(string.Format("ProcessLogID: {0} - Total rows: {1}, Page size: {2}, Total pages: {3}, Total files: {4}", 
                    pro.ProcessLogID, totalRows, pageSize, totalPages, totalFiles));

                int fileNumber = 1;
                int pageNo = 1;
                int pageCount = 0;
                int rowsProcessed = 0;
                DataTable accumulatedData = null;
                List<string> listColumnName = null;

                // Loop with boundary based on totalPages and totalRows
                while (pageNo <= totalPages && rowsProcessed < totalRows)
                {
                    IDataReader pageReader = _taskManager.GetExtractReportAutoBindingParameters(pro, pageSize, pageNo);

                    if (pageReader == null)
                    {
                        LoggerManager.Debug(string.Format("ProcessLogID: {0} - pageReader is null at page {1}", pro.ProcessLogID, pageNo));
                        break;
                    }

                    // Get column information from first page reader (only once)
                    if (listColumnName == null)
                    {
                        DataTable schemaTable = pageReader.GetSchemaTable();
                        listColumnName = schemaTable.AsEnumerable()
                            .Select(c => c.Field<string>("ColumnName")).ToList();
                    }

                    // Initialize accumulated data table on first page
                    if (accumulatedData == null)
                    {
                        accumulatedData = new DataTable();
                        foreach (string colName in listColumnName)
                        {
                            accumulatedData.Columns.Add(colName);
                        }
                    }

                    // Accumulate data from this page
                    int rowsInThisPage = 0;
                    while (pageReader.Read() && rowsProcessed < totalRows)
                    {
                        DataRow newRow = accumulatedData.NewRow();
                        for (int i = 0; i < listColumnName.Count; i++)
                        {
                            try
                            {
                                newRow[listColumnName[i]] = pageReader[listColumnName[i]] ?? DBNull.Value;
                            }
                            catch
                            {
                                newRow[listColumnName[i]] = DBNull.Value;
                            }
                        }
                        accumulatedData.Rows.Add(newRow);
                        rowsInThisPage++;
                        rowsProcessed++;
                    }

                    pageReader.Dispose();

                    LoggerManager.Debug(string.Format("ProcessLogID: {0} - Page {1}/{2}: Read {3} rows, Total processed: {4}/{5}",
                        pro.ProcessLogID, pageNo, totalPages, rowsInThisPage, rowsProcessed, totalRows));

                    pageCount++;

                    // Write to file when accumulated pagesPerFile or reached end
                    if (pageCount >= pagesPerFile || pageNo >= totalPages || rowsProcessed >= totalRows)
                    {
                        if (accumulatedData != null && accumulatedData.Rows.Count > 0)
                        {
                            // If only 1 file will be generated, don't add number suffix
                            // Otherwise add _01, _02, etc.
                            string fileNameForExport;
                            if (isSingleFile)
                            {
                                fileNameForExport = fileName;
                            }
                            else
                            {
                                fileNameForExport = $"{fileName}_{fileNumber:D2}";
                            }
                            
                            wsExportParameter.FileName = fileNameForExport;
                            
                            string exportedFileName = wSExportCore.ExportToExcel2007Extra(
                                accumulatedData.CreateDataReader(), wsExportParameter, null, filePath, fileResourse);

                            if (!string.IsNullOrEmpty(exportedFileName))
                            {
                                fileNameGenerates.Add(exportedFileName);
                            }

                            accumulatedData.Dispose();
                            accumulatedData = null;
                            pageCount = 0;
                            fileNumber++;

                            LoggerManager.Debug(string.Format("ProcessLogID: {0} - Excel file batch exported: {1}", pro.ProcessLogID, exportedFileName));
                        }
                    }

                    pageNo++;
                }

                LoggerManager.Debug(string.Format("ProcessLogID: {0} - Export completed. Processed {1}/{2} rows in {3} pages. Generated {4} files.",
                    pro.ProcessLogID, rowsProcessed, totalRows, pageNo - 1, fileNameGenerates.Count));

                return fileNameGenerates;
            }
            catch (Exception ex)
            {
                LoggerManager.Error(string.Format("ProcessLogID: {0} - ExportExcelPaginated failed: {1}", pro.ProcessLogID, ex.ToString()));
                return fileNameGenerates;
            }
        }

        private List<string> ExportCSVPaginated(Processer pro, string filePath, ExportTemplate exTemplate, string reportHeader)
        {
            List<string> fileNameGenerates = new List<string>();
            
            string fileName = Path.GetFileNameWithoutExtension(pro.FileName);
            LoggerManager.Debug(string.Format("ProcessLogID: {0} - Process Export CSV File: {1}", pro.ProcessLogID, fileName));

            try
            {
                // Get total rows from database
                DataTable getTotalRow = _taskManager.GetExtractReportAutoBindingParameters(pro, true);
                int totalRows = (getTotalRow != null && getTotalRow.Rows.Count > 0)
                    ? int.Parse(getTotalRow.Rows[0]["TotalRows"].ToString())
                    : 0;

                LoggerManager.Debug(string.Format("ProcessLogID: {0} - Total Row: {1}", pro.ProcessLogID, totalRows));

                int pageSize = AppConfigurations.EXPORTREPORTS_PAGESIZE;
                int pagesPerFile = AppConfigurations.EXPORTREPORTS_PAGES_PER_FILE;

                // Get column information from template (display names and data keys)
                var filterSheet = exTemplate.Sheets.FirstOrDefault() ?? exTemplate.Sheets.First();
                List<string> listColumnName = filterSheet.Columns.Select(x => x.Name).ToList();      // Display names for header
                List<string> listColumnKey = filterSheet.Columns.Select(x => x.DataKey).ToList();    // Data keys for reading from DB

                // If no data, create empty file with headers only
                if (totalRows == 0)
                {
                    LoggerManager.Debug(string.Format("ProcessLogID: {0} - No data to export, creating empty CSV file with headers", pro.ProcessLogID));

                    // If template doesn't have columns defined, try to get from DB schema
                    if (listColumnKey.Count == 0)
                    {
                        IDataReader schemaReader = _taskManager.GetExtractReportAutoBindingParameters(pro, 1, 1);
                        if (schemaReader != null)
                        {
                            DataTable schemaTable = schemaReader.GetSchemaTable();
                            if (schemaTable != null)
                            {
                                listColumnName = schemaTable.AsEnumerable()
                                    .Select(c => c.Field<string>("ColumnName")).ToList();
                                listColumnKey = new List<string>(listColumnName);
                            }
                            schemaReader.Dispose();
                        }
                    }

                    string emptyFileName = $"{fileName}.csv";
                    string emptyFilePath = Path.Combine(filePath, emptyFileName);

                    using (StreamWriter writer = new StreamWriter(emptyFilePath, false, new UTF8Encoding(false)))
                    {
                        // Write report header if provided
                        if (!string.IsNullOrEmpty(reportHeader))
                        {
                            writer.WriteLine(reportHeader);
                        }

                        // Write column headers
                        writer.WriteLine(string.Join(",", listColumnName));
                    }

                    fileNameGenerates.Add(emptyFileName);
                    LoggerManager.Debug(string.Format("ProcessLogID: {0} - Empty CSV file created: {1}", pro.ProcessLogID, emptyFileName));
                    return fileNameGenerates;
                }

                // Calculate total pages needed based on totalRows
                int totalPages = (int)Math.Ceiling((double)totalRows / pageSize);

                // Calculate total files that will be generated
                int totalFiles = (int)Math.Ceiling((double)totalPages / pagesPerFile);
                bool isSingleFile = totalFiles == 1;

                LoggerManager.Debug(string.Format("ProcessLogID: {0} - Total rows: {1}, Page size: {2}, Total pages: {3}, Total files: {4}",
                    pro.ProcessLogID, totalRows, pageSize, totalPages, totalFiles));

                int fileNumber = 1;
                int pageNo = 1;
                int pageCount = 0;
                int rowsProcessed = 0;
                List<string[]> accumulatedData = null;  // Accumulate rows as string arrays
                bool columnsInitialized = false;

                // Loop with boundary based on totalPages and totalRows
                while (pageNo <= totalPages && rowsProcessed < totalRows)
                {
                    IDataReader pageReader = _taskManager.GetExtractReportAutoBindingParameters(pro, pageSize, pageNo);

                    if (pageReader == null)
                    {
                        LoggerManager.Debug(string.Format("ProcessLogID: {0} - pageReader is null at page {1}", pro.ProcessLogID, pageNo));
                        break;
                    }

                    // Get column information from DB schema (only once) if template doesn't have columns
                    if (!columnsInitialized)
                    {
                        if (listColumnKey.Count == 0)
                        {
                            DataTable schemaTable = pageReader.GetSchemaTable();
                            listColumnName = schemaTable.AsEnumerable()
                                .Select(c => c.Field<string>("ColumnName")).ToList();
                            listColumnKey = new List<string>(listColumnName);
                        }
                        columnsInitialized = true;
                    }

                    // Initialize accumulated data list
                    if (accumulatedData == null)
                    {
                        accumulatedData = new List<string[]>();
                    }

                    // Accumulate data from this page
                    int rowsInThisPage = 0;
                    while (pageReader.Read() && rowsProcessed < totalRows)
                    {
                        string[] rowData = listColumnKey.Select(k =>
                        {
                            try
                            {
                                object val = pageReader[k.Trim()];
                                string strVal = val?.ToString() ?? string.Empty;
                                // Escape quotes in CSV
                                return $"\"{strVal.Replace("\"", "\"\"")}\"";
                            }
                            catch
                            {
                                return "\"\"";
                            }
                        }).ToArray();

                        accumulatedData.Add(rowData);
                        rowsInThisPage++;
                        rowsProcessed++;
                    }

                    pageReader.Dispose();

                    LoggerManager.Debug(string.Format("ProcessLogID: {0} - Page {1}/{2}: Read {3} rows, Total processed: {4}/{5}",
                        pro.ProcessLogID, pageNo, totalPages, rowsInThisPage, rowsProcessed, totalRows));

                    pageCount++;

                    // Write to file when accumulated pagesPerFile or reached end
                    if (pageCount >= pagesPerFile || pageNo >= totalPages || rowsProcessed >= totalRows)
                    {
                        if (accumulatedData != null && accumulatedData.Count > 0)
                        {
                            // Determine file name
                            string fileNameForExport;
                            if (isSingleFile)
                            {
                                fileNameForExport = $"{fileName}.csv";
                            }
                            else
                            {
                                fileNameForExport = $"{fileName}_{fileNumber:D2}.csv";
                            }

                            string fullPath = Path.Combine(filePath, fileNameForExport);

                            // Write accumulated data to file
                            using (StreamWriter writer = new StreamWriter(fullPath, false, new UTF8Encoding(false)))
                            {
                                // Write report header if provided
                                if (!string.IsNullOrEmpty(reportHeader))
                                {
                                    writer.WriteLine(reportHeader);
                                }

                                // Write column headers
                                writer.WriteLine(string.Join(",", listColumnName));

                                // Write all accumulated rows
                                foreach (var rowData in accumulatedData)
                                {
                                    writer.WriteLine(string.Join(",", rowData));
                                }
                            }

                            fileNameGenerates.Add(fileNameForExport);
                            LoggerManager.Debug(string.Format("ProcessLogID: {0} - CSV file batch exported: {1} ({2} rows)", 
                                pro.ProcessLogID, fileNameForExport, accumulatedData.Count));

                            // Reset for next file
                            accumulatedData.Clear();
                            accumulatedData = null;
                            pageCount = 0;
                            fileNumber++;
                        }
                    }

                    pageNo++;
                }

                LoggerManager.Debug(string.Format("ProcessLogID: {0} - Export CSV completed. Processed {1}/{2} rows in {3} pages. Generated {4} files.",
                    pro.ProcessLogID, rowsProcessed, totalRows, pageNo - 1, fileNameGenerates.Count));

                return fileNameGenerates;
            }
            catch (Exception ex)
            {
                LoggerManager.Error(string.Format("ProcessLogID: {0} - ExportCSVPaginated failed: {1}", pro.ProcessLogID, ex.ToString()));
                return fileNameGenerates;
            }
        }

        private void CallThreadExportDone(Processer pro, List<string> fileNameGenerates, FileNameExtension.FileType fileType, string filePath)
        {
            // Get base file name without extension
            string baseFileName = Path.GetFileNameWithoutExtension(pro.FileName);
            string extension = fileType.ToString();
            List<string> renamedFiles = new List<string>();

            LoggerManager.Debug(string.Format("ProcessLogID: {0} - CallThreadExportDone: baseFileName={1}, fileCount={2}",
                pro.ProcessLogID, baseFileName, fileNameGenerates.Count));

            // Rename all generated files by extracting the part from baseFileName to end
            // Example: "AS_f0b0b11a-6761-4902-a8f7-4f1f047e8cbe_MerchantList-AllISOs_01.xlsx" 
            //       -> "MerchantList-AllISOs_01.xlsx"
            if (fileNameGenerates.Count > 0)
            {
                for (int i = 0; i < fileNameGenerates.Count; i++)
                {
                    string originalFileName = fileNameGenerates[i];
                    string newFileName;

                    // Find the position of baseFileName in the original file name
                    int baseNameIndex = originalFileName.IndexOf(baseFileName, StringComparison.OrdinalIgnoreCase);

                    if (baseNameIndex >= 0)
                    {
                        // Extract from baseFileName to end (keeps the _01, _02 suffix and extension)
                        newFileName = originalFileName.Substring(baseNameIndex);
                    }
                    else
                    {
                        // Fallback: if baseFileName not found, keep original name
                        newFileName = originalFileName;
                        LoggerManager.Debug(string.Format("ProcessLogID: {0} - Warning: baseFileName '{1}' not found in '{2}', keeping original name",
                            pro.ProcessLogID, baseFileName, originalFileName));
                    }

                    LoggerManager.Debug(string.Format("ProcessLogID: {0} - Renaming: {1} -> {2}",
                        pro.ProcessLogID, originalFileName, newFileName));

                    bool isRenameSuccess = RenameFile(filePath, originalFileName, newFileName);
                    if (!isRenameSuccess)
                    {
                        LoggerManager.Error(string.Format("ProcessLogID: {0} - Can not rename file: {1} to {2}", pro.ProcessLogID, originalFileName, newFileName));
                        _taskManager.UpdateStatus(pro, "-1", ProcessStatus.Fail, AppConfigurations.ExtractMode, string.Empty);
                        return;
                    }

                    renamedFiles.Add(newFileName);
                    LoggerManager.Debug(string.Format("ProcessLogID: {0} - Renamed file successfully: {1} -> {2}", pro.ProcessLogID, originalFileName, newFileName));
                }
            }

            // Check if we need to zip
            var isZipFile = false;
            long totalSize = 0;

            foreach (var file in renamedFiles)
            {
                FileInfo fi = new FileInfo(Path.Combine(filePath, file));
                totalSize += fi.Length;
            }

            int totalSizeMB = (int)(totalSize / (1024 * 1024));
            isZipFile = renamedFiles.Count > 1 || totalSizeMB >= AppConfigurations.ThresholdToZip;

            List<string> filesToUpload = new List<string>();

            if (isZipFile)
            {
                fileType = FileNameExtension.FileType.zip;
                LoggerManager.Debug(string.Format("ProcessLogID: {0} - Process Zip Files. Total size: {1} MB", pro.ProcessLogID, totalSizeMB));

                // Zip each file individually
                List<string> zippedFiles = ZipFilesIndividually(renamedFiles, filePath, pro.ProcessLogID);

                if (zippedFiles.Count == 0)
                {
                    LoggerManager.Error(string.Format("ProcessLogID: {0} - Failed to zip files", pro.ProcessLogID));
                    _taskManager.UpdateStatus(pro, "-1", ProcessStatus.Fail, AppConfigurations.ExtractMode, string.Empty);
                    return;
                }

                filesToUpload = zippedFiles;
                LoggerManager.Debug(string.Format("ProcessLogID: {0} - Process Zip Files END. Generated {1} zip files", pro.ProcessLogID, zippedFiles.Count));
            }
            else
            {
                // No zip needed, upload original files
                filesToUpload = renamedFiles;
            }

            // Upload files to Doc server
            UploadFiles(pro, filesToUpload, fileType, filePath);
        }

        /// <summary>
        /// Zip each file individually and return list of zipped file names
        /// Example: MerchantList-AllISOs_01.xlsx -> MerchantList-AllISOs_01.zip
        /// </summary>
        private List<string> ZipFilesIndividually(List<string> sourceFiles, string filePath, int logId)
        {
            List<string> zippedFiles = new List<string>();

            foreach (var sourceFile in sourceFiles)
            {
                string sourceFilePath = Path.Combine(filePath, sourceFile);
                string zipFileName = Path.GetFileNameWithoutExtension(sourceFile) + ".zip";
                string zipFilePath = Path.Combine(filePath, zipFileName);

                LoggerManager.Debug(string.Format("ProcessLogID: {0} - Zipping: {1} -> {2}", logId, sourceFile, zipFileName));

                bool zipSuccess = ZipFiles(sourceFilePath, zipFilePath);

                if (zipSuccess)
                {
                    zippedFiles.Add(zipFileName);
                    LoggerManager.Debug(string.Format("ProcessLogID: {0} - Zipped successfully: {1}", logId, zipFileName));
                }
                else
                {
                    LoggerManager.Error(string.Format("ProcessLogID: {0} - Failed to zip file: {1}", logId, sourceFile));
                    // Continue with other files even if one fails
                }
            }

            return zippedFiles;
        }

        private bool ZipFiles(string fileSources, string fileDestination)
        {
            try
            {
                using (FileStream zipToCreate = new FileStream(fileDestination, FileMode.Create))
                using (ZipArchive archive = new ZipArchive(zipToCreate, ZipArchiveMode.Create))
                {
                    FileInfo fileInfo = new FileInfo(fileSources);
                    archive.CreateEntryFromFile(fileSources, fileInfo.Name);

                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Upload multiple files to Doc server
        /// </summary>
        private void UploadFiles(Processer pro, List<string> fileNames, FileNameExtension.FileType fileType, string filePath)
        {
            List<long> allUploadedParts = new List<long>();
            bool allSuccess = true;

            foreach (var fileName in fileNames)
            {
                LoggerManager.Debug(string.Format("ProcessLogID: {0} - Uploading file: {1}", pro.ProcessLogID, fileName));

                List<long> uploadedParts;
                bool uploadFileSuccess = GeneralFuncLibraries.UploadFileToDocServer(
                    pro.ASClientID, pro.CreatedBy, filePath, fileName,
                    _serverId, CATEGORY_NAME, out uploadedParts, AppConfigurations.ChunkSizeInKB);

                if (uploadFileSuccess && uploadedParts != null)
                {
                    allUploadedParts.AddRange(uploadedParts);
                    LoggerManager.Debug(string.Format("ProcessLogID: {0} - Upload success for {1}. DocID: {2}",
                        pro.ProcessLogID, fileName, string.Join(",", uploadedParts)));
                }
                else
                {
                    allSuccess = false;
                    LoggerManager.Error(string.Format("ProcessLogID: {0} - Upload failed for file: {1}", pro.ProcessLogID, fileName));
                }
            }

            if (allSuccess && allUploadedParts.Count > 0)
            {
                // Update status with all DocIDs
                _taskManager.UpdateStatus(pro, string.Join(",", allUploadedParts), ProcessStatus.Success, fileType.ToString(), AppConfigurations.ExtractMode);
                LoggerManager.Debug(string.Format("ProcessLogID: {0} - All uploads completed. DocIDs: {1}",
                    pro.ProcessLogID, string.Join(",", allUploadedParts)));

                // Delete files at physical folder
                if (!isDebug)
                {
                    if (AppConfigurations.IsDeleteUploadFile && Directory.Exists(filePath))
                    {
                        try
                        {
                            DirectoryInfo dir = new DirectoryInfo(filePath);
                            foreach (FileInfo file in dir.GetFiles())
                            {
                                file.Delete();
                            }
                            Directory.Delete(filePath);
                            LoggerManager.Debug(string.Format("ProcessLogID: {0} - Deleted folder: {1}", pro.ProcessLogID, filePath));
                        }
                        catch (Exception ex)
                        {
                            LoggerManager.Debug(string.Format("ProcessLogID: {0} - Error deleting folder {1}: {2}",
                                pro.ProcessLogID, filePath, ex.ToString()));
                        }
                    }
                }
            }
            else
            {
                // Upload failed
                _taskManager.UpdateStatus(pro, "-1", ProcessStatus.Fail, string.Empty, AppConfigurations.ExtractMode);
                LoggerManager.Error(string.Format("ProcessLogID: {0} - Upload to doc server failed", pro.ProcessLogID));
            }
        }

        private bool RenameFile(string filePath, string fileNameGenerate, string fileNameReal)
        {
            string pathGenerate = Path.Combine(filePath, fileNameGenerate);
            string pathReal = Path.Combine(filePath, fileNameReal);
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

        private void CleanupFolder(string folderPath)
        {
            try
            {
                if (Directory.Exists(folderPath))
                {
                    Directory.Delete(folderPath, true);
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Debug(string.Format("Error cleaning up folder {0}: {1}", folderPath, ex.ToString()));
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
    }
}