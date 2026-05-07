using AS.Archive;
using AS.Common.Logger;
using AS.Common.Utilities;
using AS.Common.WebServiceExport;
using AS.VW.Scheduler.Tasks.Codes.Models;
using AS.VW.Scheduler.Tasks.Entities;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace AS.VW.Scheduler.Tasks
{
    public static class GeneralFuncLibraries
    {
        const int ROW_TITLE_INDEX = 1;
        const int ROW_TITLE_INDEX_2 = 2;
        const int ROW_HEADER_INDEX = 3;
        const int ROW_DATA_INDEX = 4;
        const int ROW_TOTAL_INDEX = 5;
        const string KEY_TOTAL_FOOTER_NAME = "{[Total";

        #region xml tag
        private static XName TEMPLATE_XML_TAG = "template";
        private static XName KEY_XML_TAG = "Key";
        private static XName SHEETS_XML_TAG = "sheets";
        private static XName SHEET_XML_TAG = "sheet";
        private static XName COLUMNNAME_XML_TAG = "ColumnName";
        private static XName PATH_XML_TAG = "Path";
        private static XName ISDEFAUL_XML_TAG = "IsDefaul";
        private static XName EXTENSIONMETHOD_XML_TAG = "extensionMethod";
        private static XName METHOD_XML_TAG = "method";
        private static XName ASSEMBLYNAME_XML_TAG = "dllName";
        private static XName TYPE_XML_TAG = "type";

        #endregion

        public static string NvlString(object val)
        {
            return (val == null || val == DBNull.Value ? string.Empty : val.ToString());
        }

        public static DataTable GetDataByXML(string xmlFilePath)
        {
            DataSet serversList = new DataSet();
            serversList.ReadXml(AppConfigurations.BaseDirectory + xmlFilePath);
            return serversList.Tables[0];
        }

        public static XElement GetXMLElement(string xmlFilePath)
        {
            return XElement.Load(AppConfigurations.BaseDirectory + xmlFilePath);
        }

        public static List<T> DataTableToList<T>(DataTable dt)
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            var columnNames = dt.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();
            var objectPropeties = typeof(T).GetProperties(flags);
            var targetList = dt.AsEnumerable().Select(dataRow =>
            {
                var instanceOfT = Activator.CreateInstance<T>();
                foreach (var properties in objectPropeties.Where(properties => columnNames.Contains(properties.Name) && dataRow[properties.Name] != DBNull.Value))
                {
                    var value = NvlString(dataRow[properties.Name]);
                    if (properties.PropertyType.Name == "Boolean")
                        properties.SetValue(instanceOfT, (value == "true" ? true : false), null);
                    else if (properties.PropertyType.Name == "Int32")
                        properties.SetValue(instanceOfT, Convert.ToInt32(string.IsNullOrEmpty(value) ? "0" : value), null);
                    else
                        properties.SetValue(instanceOfT, value, null);
                }
                return instanceOfT;
            }).ToList();
            return targetList;
        }

        public static Dictionary<string, StatisticReportColumn> GetStatisticColumns()
        {
            var dt = GetDataByXML("/App_Data/MgmtReporting/StatisticReportColumns.xml");
            Dictionary<string, StatisticReportColumn> result = new Dictionary<string, StatisticReportColumn>();
            if (dt.Rows.Count > 0)
            {
                //convert data to list
                return DataTableToList<StatisticReportColumn>(dt).ToDictionary(pair => pair.Key);
            }

            return result;
        }

        public static string GetExportTempatePath(string mode)
        {
            DataTable xmlValues = GetDataByXML("/App_Data/ExportTemplateConfigurations.xml");
            List<ExportTemplate> templates = new List<ExportTemplate>();
            if (xmlValues.Rows.Count > 0)
            {
                templates = DataTableToList<ExportTemplate>(xmlValues);
            }
            ExportTemplate template = templates.Where(item => item.Key == mode).Count() > 0 ? templates.Where(item => item.Key == mode).FirstOrDefault() : templates.Where(item => item.IsDefault).FirstOrDefault();
            return template.Path;
        }

        public static ExportTemplate GetExportTempate(string mode)
        {
            XElement root = GetXMLElement("/App_Data/ExportTemplateConfigurations.xml");

            var template = (from el in root.Elements(TEMPLATE_XML_TAG)
                            where (string)el.Attribute(KEY_XML_TAG) == mode
                            select el).FirstOrDefault();

            template = template != null ? template : (from el in root.Elements(TEMPLATE_XML_TAG)
                                                      where el.Attribute(ISDEFAUL_XML_TAG) != null && (bool)el.Attribute(ISDEFAUL_XML_TAG) == true
                                                      select el).FirstOrDefault();
            List<DataSheet> listSheet = new List<DataSheet>();
            var extensionMenthod = template.Element(EXTENSIONMETHOD_XML_TAG);
            var sheets = template.Element(SHEETS_XML_TAG);

            string assemblyName = extensionMenthod == null ? string.Empty : extensionMenthod.Element(METHOD_XML_TAG).Attributes(ASSEMBLYNAME_XML_TAG).FirstOrDefault().Value;
            string assemblyType = extensionMenthod == null ? string.Empty : extensionMenthod.Element(METHOD_XML_TAG).Attributes(TYPE_XML_TAG).FirstOrDefault().Value;

            List<XElement> xmlSheet = sheets == null ? new List<XElement>() : sheets.Elements(SHEET_XML_TAG).ToList();

            foreach (XElement node in xmlSheet)
            {
                DataSheet item = new DataSheet();

                item.Name = node.Attribute("Name") != null ? node.Attribute("Name").Value : string.Empty;

                List<XElement> columnXML = node.Elements("Columns").First().Elements(COLUMNNAME_XML_TAG).ToList();
                foreach (XElement column in columnXML)
                {
                    ColumnTemplate columnTemPlate = new ColumnTemplate
                    {
                        DataKey = column.Attribute("DataKey") == null ? string.Empty : column.Attribute("DataKey").Value,
                        Name = column.Attribute("Name") == null ? string.Empty : column.Attribute("Name").Value,
                        ASFormat = column.Attribute("ASFormat") != null ? column.Attribute("ASFormat").Value : "auto",
                        PartialColumn = column.Attribute("PartialColumn") != null ? column.Attribute("PartialColumn").Value : string.Empty,
                    };

                    item.MaxWidthColumns.Add(column.Attribute("Width") != null ? double.Parse(column.Attribute("Width").Value) : 0);
                    item.Columns.Add(columnTemPlate);
                }
                listSheet.Add(item);
            }

            ExportTemplate exportTemplate = new ExportTemplate();
            exportTemplate.Path = template.Attribute(PATH_XML_TAG).Value;
            exportTemplate.IsCreateNewTemplate = (template.Attribute("IsCreateNewTemplate") == null) || (string.IsNullOrEmpty(template.Attribute("IsCreateNewTemplate").Value)) ? false : bool.Parse(template.Attribute("IsCreateNewTemplate").Value);
            exportTemplate.AssemblyName = assemblyName;
            exportTemplate.AssemblyType = assemblyType;
            exportTemplate.HasFilterSheet = xmlSheet.Count > 1;
            exportTemplate.Sheets = listSheet;
            return exportTemplate;
        }

        public static bool CreateZipFile(string fileName, IList<string> files)
        {
            try
            {
                var zip = ZipFile.Open(fileName, System.IO.Compression.ZipArchiveMode.Create);

                foreach (string f in files)
                {
                    zip.CreateEntryFromFile(f, Path.GetFileName(f), CompressionLevel.Optimal);

                }

                zip.Dispose();
                return true;
            }
            catch (Exception e)
            {
                LoggerManager.Error("Error within zipping file, error msg: " + e.Message + ", at: " + DateTime.Now);
                return false;
            }
        }

        public static List<T> ConvertDataTable<T>(this DataTable dtTable)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dtTable.Rows)
            {
                T item = row.ConvertDataRow<T>();
                data.Add(item);
            }
            return data;
        }

        public static T ConvertDataRow<T>(this DataRow dr)
        {
            T item = (T)GetItem(typeof(T), dr);
            return item;
        }
        public static object GetItem(Type t, DataRow dr, string prefix = "")
        {
            object obj = Activator.CreateInstance(t);
            foreach (PropertyInfo pro in t.GetProperties())
            {
                if (!pro.CanWrite) continue;
                foreach (DataColumn column in dr.Table.Columns)
                {
                    if ((prefix + pro.Name).ToLower() == (column.ColumnName).ToLower())
                    {

                        if (pro.PropertyType.BaseType.FullName == "System.Enum")
                        {

                            if (dr[column.ColumnName] == System.DBNull.Value)
                            {
                                pro.SetValue(obj, Enum.Parse(pro.PropertyType, default(int).ToString(), true), null);
                            }
                            else
                            {
                                pro.SetValue(obj, Enum.Parse(pro.PropertyType, dr[column.ColumnName].ToString(), true), null);
                            }
                        }
                        else if (pro.PropertyType.FullName == "System.DateTime" && dr[column.ColumnName] == System.DBNull.Value)
                        {

                            pro.SetValue(obj, Convert.ChangeType(default(DateTime), pro.PropertyType), null);
                        }
                        else
                        {
                            pro.SetValue(obj, Convert.ChangeType(dr[column.ColumnName], pro.PropertyType), null);
                        }

                        break;
                    }
                }
            }

            return obj;
        }

        public static bool SevenZipFiles(string zipFile, ICollection<string> files, string password)
        {
            try
            {
                SevenZip.SevenZipDllPath = Path.Combine(AppConfigurations.BaseDirectory, "7z.dll");
                SevenZip.Pack(ArchiveType.Zip, zipFile, files, password);

                return true;
            }
            catch (Exception e)
            {
                AS.Common.Logger.LoggerManager.Error("Error within zipping file, error msg: " + e.Message + ", at: " + DateTime.Now);
                return false;
            }
        }

        public static bool SevenZipFile(string fileSource, string zipFileName)
        {
            try
            {
                SevenZip.SevenZipDllPath = Path.Combine(AppConfigurations.BaseDirectory, "7z.dll");
                SevenZip.Pack(ArchiveType.Zip, zipFileName, new string[] { fileSource }, null);

                return true;
            }
            catch (Exception e)
            {
                AS.Common.Logger.LoggerManager.Error("Error within zipping file, error msg: " + e.Message + ", at: " + DateTime.Now);
                return false;
            }
        }

        public static void DeleteFile(string filePath)
        {
            if (filePath != string.Empty && File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                }
                catch (Exception ex)
                {
                    LoggerManager.Error("Delete File: Failed\n" + ex.ToString());
                }
            }
        }

        public static void DeleteFiles(IList<string> listFile)
        {
            foreach (string item in listFile)
            {
                if (item != string.Empty && File.Exists(item))
                {
                    try
                    {
                        File.Delete(item);
                    }
                    catch (Exception ex)
                    {
                        LoggerManager.Debug("Delete File: Failed\n" + ex.ToString());
                    }
                }
            }
        }

        public static bool UploadFileToDocServer(int clientID, string entityID, string filePath, string fileName, string serverIP, string categoryName, out List<long> uploadedDocIds, int chunkSizeInKB = 102400)
        {
            uploadedDocIds = new List<long>();
            string fullPath = Path.Combine(filePath, fileName);
            const int bufferSize = 1024 * 1024;

            try
            {
                long fileSize = new FileInfo(fullPath).Length;
                long chunkSize = chunkSizeInKB * 1024L;

                using (FileStream fs = File.OpenRead(fullPath))
                {
                    int partIndex = 0;
                    byte[] buffer = new byte[bufferSize];

                    while (fs.Position < fileSize)
                    {
                        using (MemoryStream chunkStream = new MemoryStream())
                        {
                            long bytesRemaining = chunkSize;

                            while (bytesRemaining > 0 && fs.Position < fileSize)
                            {
                                int bytesToRead = (int)Math.Min(bufferSize, bytesRemaining);
                                int bytesRead = fs.Read(buffer, 0, bytesToRead);
                                if (bytesRead == 0) break;

                                chunkStream.Write(buffer, 0, bytesRead);
                                bytesRemaining -= bytesRead;
                            }

                            string chunkFileName = $"{fileName}.part{partIndex:D3}";
                            byte[] chunkData = chunkStream.ToArray();

                            long docID = WebServices.DocServices.UploadDoc(clientID, entityID, chunkData, chunkFileName, serverIP, categoryName);

                            if (docID == -1)
                            {
                                LoggerManager.Error($"Upload chunk {chunkFileName} failed.");
                                return false;
                            }

                            uploadedDocIds.Add(docID);
                            LoggerManager.Info($"Uploaded {chunkFileName} (size: {chunkData.Length} bytes), DocID: {docID}");

                            partIndex++;
                        }
                    }
                }

                return uploadedDocIds.Count > 0;
            }
            catch (Exception ex)
            {
                LoggerManager.Error("Upload file: Exception occurred.\n" + ex);
                return false;
            }
        }

        public static string ConvertFileNameExtension(string fileName, string extension)
        {
            var array = fileName.Split('.');
            if (array.Count() == 1)
            {
                return string.Format("{0}.{1}", fileName, extension);
            }
            string originalFileNameExtension = array[array.Length - 1];
            return fileName.Replace("." + originalFileNameExtension, "." + extension);
        }

        private static void GenerateWorkbookPartContent(WorkbookPart workbookPart, ExportTemplate expTemplate)
        {
            Workbook workbook1 = new Workbook();
            Sheets sheets1 = new Sheets();
            for (int i = 1; i <= expTemplate.Sheets.Count(); i++)
            {
                string sheetName = expTemplate.Sheets.ElementAt(i - 1).Name;
                Sheet sheet = new Sheet()
                {
                    Name = (!string.IsNullOrEmpty(sheetName) ? sheetName.TrumcateAtWord(27) : "Sheet" + i),
                    SheetId = Convert.ToUInt32(i),
                    Id = "rId" + i
                };
                sheets1.Append(sheet);
            }

            workbook1.Append(sheets1);
            workbookPart.Workbook = workbook1;
        }

        private static bool IsLongText(string value)
        {
            DoubleValue width = Math.Truncate(((double)value.Length * 7 + 5) / 7 * 256) / 256; //for font Calibri           
            return width > 55;
        }

        public static bool CheckColumnExists(IDataReader reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }

            return false;
        }

        private static double[] UpdateMaxColumnWidths(double[] maxColumnWidths, int index, string value, string formatType)
        {
            DoubleValue width = Math.Truncate(((double)value.Length * 7 + 5) / 7 * 256) / 256; //for font Calibri

            // Set Width for DateTime
            switch (formatType.ToLower())
            {
                case "date":
                    width = 12;
                    break;
                case "dateandtime":
                    width = 20;
                    break;
                case "dateandtime12hours":
                case "datetimeshorttime":
                    width = 22;
                    break;
            }

            if (index < maxColumnWidths.Count() && (maxColumnWidths[index] == 0 || maxColumnWidths[index] < width))
                maxColumnWidths[index] = width > 50 ? 50 : width < 7 ? 7 : width;

            return maxColumnWidths;
        }

        static string GetSharedStringCellValue(WorkbookPart workbookPart, Cell cell)
        {
            SharedStringTablePart sstPart = workbookPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
            SharedStringTable ssTable = sstPart.SharedStringTable;
            return ssTable.ChildElements[Convert.ToInt32(cell.CellValue.Text)].InnerText;

        }

        private static object RemoveInvalidXmlChars(object value)
        {
            if (value is string && value.ToString().Length > 0)
            {
                StringBuilder strBuilder = new StringBuilder();
                foreach (char ch in value.ToString())
                {
                    if (XmlConvert.IsXmlChar(ch))
                        strBuilder.Append(ch);
                }
                return strBuilder.ToString();
            }
            else
                return value;
        }

        static void FormatCell(Cell cell, object dataItem, string formatString, string defaultEmptyString = "")
        {
            if (string.IsNullOrEmpty(formatString))
            {
                if (dataItem is DateTime)
                {
                    cell.DataType = null;
                    cell.CellValue = new CellValue(((DateTime)dataItem).ToOADate().ToString());
                }
                else if (
                        dataItem is decimal
                        || dataItem is float
                        || dataItem is Single
                        || dataItem is int
                        || dataItem is long
                        || dataItem is short
                        || dataItem is byte
                    )
                {
                    cell.DataType = CellValues.Number;
                    cell.CellValue = new CellValue(dataItem.ToString());
                }
                else
                {
                    cell.DataType = CellValues.InlineString;
                    cell.InlineString = new InlineString() { Text = new Text(dataItem.ToString()) };
                    cell.CellValue = new CellValue(defaultEmptyString);
                }
            }
            else
            {
                cell.DataType = CellValues.InlineString;
                cell.InlineString = new InlineString() { Text = new Text(string.Format(formatString, dataItem)) };
                cell.CellValue = new CellValue(defaultEmptyString); ;
            }
        }

        static string FormatCellWithASFormat(Cell cell, object dataItem, string asFormatType, bool isUsedStyleIndex = true, string defaultEmptyString = "")
        {
            object cellValue = RemoveInvalidXmlChars(dataItem);

            uint styleIndex = 0;
            Regex regLineBreak = new Regex(@"[\r\n]");
            switch (asFormatType.ToLower())
            {
                case "date":
                case "dateandtime":
                case "dateandtime12hours":
                case "datetimeshorttime":
                    if (dataItem.ToString().Length > 0)
                    {
                        cell.DataType = null;
                        DateTime date = DateTime.Now;
                        if (DateTime.TryParse(dataItem.ToString(), out date))
                        {
                            string defaultDate = "1/1/1900";
                            if (date == DateTime.Parse(defaultDate))
                            {
                                cell.DataType = CellValues.InlineString;
                                cell.CellValue = null;
                                styleIndex = 3;
                            }
                            else
                            {
                                cellValue = date.ToOADate();
                                cell.CellValue = new CellValue(cellValue.ToString());
                                styleIndex = asFormatType.ToLower() == "date" ? (uint)7 : asFormatType.ToLower() == "dateandtime" ? (uint)8
                                    : asFormatType.ToLower() == "dateandtime12hours" ? (uint)13 : (uint)17;
                            }
                        }
                        else
                        {
                            cellValue = date.ToString();
                            cell.CellValue = new CellValue(cellValue.ToString());
                            styleIndex = 3;
                        }

                        break;
                    }
                    else
                    {
                        cell.DataType = CellValues.InlineString;
                        cell.InlineString = new InlineString() { Text = new Text(cellValue.ToString()) };
                        cell.CellValue = null;
                        styleIndex = 3;
                    }
                    break;

                case "currency":
                    cell.DataType = CellValues.Number;
                    cell.CellValue = new CellValue(cellValue.ToString());
                    styleIndex = 11;
                    break;

                case "number":
                case "integer":
                    cell.DataType = CellValues.Number;
                    cell.CellValue = new CellValue(cellValue.ToString());
                    styleIndex = 4;
                    break;
                case "number1digit":
                    cell.DataType = CellValues.Number;
                    cell.CellValue = new CellValue(cellValue.ToString());
                    styleIndex = 15;
                    break;
                case "number2digit":
                    cell.DataType = CellValues.Number;
                    cell.CellValue = new CellValue(cellValue.ToString());
                    styleIndex = 16;
                    break;
                case "currency4digits":
                    cell.DataType = CellValues.Number;
                    cell.CellValue = new CellValue(cellValue.ToString());
                    styleIndex = 12;
                    break;

                case "number4digits":
                    cell.DataType = CellValues.Number;
                    cell.CellValue = new CellValue(cellValue.ToString());
                    styleIndex = 6;

                    break;

                case "percentage":
                    cell.DataType = CellValues.Number;
                    decimal percent = 0;
                    if (decimal.TryParse(dataItem.ToString(), out percent))
                    {
                        cellValue = (percent / 100).ToString();
                        cell.CellValue = new CellValue(cellValue.ToString());
                        styleIndex = 9;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(defaultEmptyString))
                        {
                            cell.InlineString = new InlineString() { Text = new Text(defaultEmptyString) };
                        }
                        cell.CellValue = null;
                        styleIndex = 3;
                    }

                    break;

                case "percentage0digits":
                    cell.DataType = CellValues.Number;
                    decimal percentage0digits = 0;
                    if (decimal.TryParse(dataItem.ToString(), out percentage0digits))
                    {
                        cellValue = (percentage0digits / 100).ToString();
                        cell.CellValue = new CellValue(cellValue.ToString());
                        styleIndex = 14;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(defaultEmptyString))
                        {
                            cell.InlineString = new InlineString() { Text = new Text(defaultEmptyString) };
                        }
                        // cell.CellValue = new CellValue(defaultEmptyString);
                        styleIndex = 3;
                    }
                    break;

                case "phone":
                case "fax":
                    Regex regPhoneNumber = new Regex(@"^\D?(\d{3})\D?\D?(\d{3})\D?(\d{4})$");
                    string phoneValue = dataItem.ToString();
                    if (regPhoneNumber.IsMatch(dataItem.ToString()))
                        phoneValue = regPhoneNumber.Replace(dataItem.ToString(), "($1) $2-$3");

                    cellValue = phoneValue;
                    cell.DataType = CellValues.InlineString;
                    cell.InlineString = new InlineString() { Text = new Text(cellValue.ToString()) };
                    cell.CellValue = null;
                    styleIndex = 3;
                    break;

                case "auto":
                    if (dataItem is decimal
                     || dataItem is float
                     || dataItem is Single
                     || dataItem is int
                     || dataItem is long
                     || dataItem is short
                     || dataItem is byte)
                    {
                        cell.DataType = CellValues.Number;
                        cell.CellValue = new CellValue(cellValue.ToString());
                        styleIndex = 4;
                    }
                    else if (dataItem is DateTime)
                    {
                        cellValue = ((DateTime)dataItem).ToOADate().ToString();
                        cell.DataType = null;
                        cell.CellValue = new CellValue(cellValue.ToString());
                        styleIndex = 7;
                    }
                    else
                    {
                        if (regLineBreak.IsMatch(dataItem.ToString()) || IsLongText(dataItem.ToString()))
                        {
                            cell.DataType = CellValues.String;
                            cell.CellValue = new CellValue(cellValue.ToString());
                            styleIndex = 10; // Wrap Text
                        }
                        else
                        {
                            cell.DataType = CellValues.InlineString;
                            cell.InlineString = new InlineString() { Text = new Text(cellValue.ToString()) };
                            cell.CellValue = null;
                            styleIndex = 3;
                        }
                    }
                    break;

                case "staticstring":
                case "dynamicstring":
                case "none":
                case "truefalse":
                    if (regLineBreak.IsMatch(dataItem.ToString()) || IsLongText(dataItem.ToString()))
                    {
                        cell.DataType = CellValues.String;
                        cell.CellValue = new CellValue(cellValue.ToString());
                        styleIndex = 10; // Wrap Text
                    }
                    else
                    {
                        cell.DataType = CellValues.InlineString;
                        cell.InlineString = new InlineString() { Text = new Text(cellValue.ToString()) };
                        cell.CellValue = null;
                        styleIndex = 3;
                    }
                    break;

                default:
                    if (regLineBreak.IsMatch(dataItem.ToString()) || IsLongText(dataItem.ToString()))
                    {
                        cell.DataType = CellValues.String;
                        cell.CellValue = new CellValue(cellValue.ToString());
                        styleIndex = 10; // Wrap Text
                    }
                    else
                    {
                        cell.DataType = CellValues.InlineString;
                        cell.InlineString = new InlineString() { Text = new Text(cellValue.ToString()) };
                        cell.CellValue = null;
                        styleIndex = 3;
                    }
                    break;
            }

            if (isUsedStyleIndex)
                cell.StyleIndex = styleIndex;

            if ((dataItem == null || string.IsNullOrEmpty(dataItem.ToString())))
            {
                cell.DataType = CellValues.InlineString;
                cell.InlineString = new InlineString() { Text = new Text(defaultEmptyString) };
            }
            return cellValue.ToString();
        }

        private static void GenerateStylesheet(WorkbookStylesPart workbookStylesPart, string currencySymbol = "$")
        {
            Stylesheet stylesheet = new Stylesheet();
            NumberingFormats numberingFormats = new NumberingFormats();
            NumberingFormat numberingFormat1 = new NumberingFormat() { NumberFormatId = (UInt32Value)167U, FormatCode = "mm/dd/yyyy" };
            NumberingFormat numberingFormat2 = new NumberingFormat() { NumberFormatId = (UInt32Value)168U, FormatCode = "mm/dd/yyyy\\ hh:mm:ss\\ AM/PM" };
            NumberingFormat numberingFormat3 = new NumberingFormat() { NumberFormatId = (UInt32Value)169U, FormatCode = "#,##0.0000" };
            NumberingFormat numberingFormat4 = new NumberingFormat() { NumberFormatId = (UInt32Value)170U, FormatCode = "[<=9999999]###\\-####;\\(###\\)\\ ###\\-####" };
            NumberingFormat numberingFormat5 = new NumberingFormat() { NumberFormatId = (UInt32Value)171U, FormatCode = ("\"" + currencySymbol + "\"#,##0.00") };
            NumberingFormat numberingFormat6 = new NumberingFormat() { NumberFormatId = (UInt32Value)172U, FormatCode = ("\"" + currencySymbol + "\"#,##0.0000") };
            NumberingFormat numberingFormat7 = new NumberingFormat() { NumberFormatId = (UInt32Value)173U, FormatCode = "mm/dd/yyyy\\ hh:mm:ss" };

            NumberingFormat numberingFormat8 = new NumberingFormat() { NumberFormatId = (UInt32Value)174U, FormatCode = "#,##0.0" };
            NumberingFormat numberingFormat9 = new NumberingFormat() { NumberFormatId = (UInt32Value)175U, FormatCode = "#,##0.00" };
            NumberingFormat numberingFormat10 = new NumberingFormat() { NumberFormatId = (UInt32Value)176U, FormatCode = "mm/dd/yyyy\\ hh:mm\\ AM/PM" };


            numberingFormats.Append(numberingFormat1);
            numberingFormats.Append(numberingFormat2);
            numberingFormats.Append(numberingFormat3);
            numberingFormats.Append(numberingFormat4);
            numberingFormats.Append(numberingFormat5);
            numberingFormats.Append(numberingFormat6);
            numberingFormats.Append(numberingFormat7);
            numberingFormats.Append(numberingFormat8);
            numberingFormats.Append(numberingFormat9);
            numberingFormats.Append(numberingFormat10);

            Fonts fonts = new Fonts() { KnownFonts = true };

            Font font1 = new Font();
            FontSize fontSize1 = new FontSize() { Val = 10D };
            FontName fontName1 = new FontName() { Val = "Calibri" };
            font1.Append(fontSize1);
            font1.Append(fontName1);

            Font font2 = new Font();
            Bold bold1 = new Bold();
            FontSize fontSize2 = new FontSize() { Val = 12D };
            FontName fontName2 = new FontName() { Val = "Calibri" };
            font2.Append(bold1);
            font2.Append(fontSize2);
            font2.Append(fontName2);

            Font font3 = new Font();
            Bold bold2 = new Bold();
            FontSize fontSize3 = new FontSize() { Val = 10D };
            FontName fontName3 = new FontName() { Val = "Calibri" };
            font3.Append(bold2);
            font3.Append(fontSize3);
            font3.Append(fontName3);

            fonts.Append(font1);
            fonts.Append(font2);
            fonts.Append(font3);

            Fills fills = new Fills();
            Fill fill1 = new Fill();
            PatternFill patternFill1 = new PatternFill() { PatternType = PatternValues.None };
            fill1.Append(patternFill1);
            fills.Append(fill1);

            Borders borders = new Borders();
            Border border1 = new Border();
            LeftBorder leftBorder1 = new LeftBorder();
            RightBorder rightBorder1 = new RightBorder();
            TopBorder topBorder1 = new TopBorder();
            BottomBorder bottomBorder1 = new BottomBorder();
            DiagonalBorder diagonalBorder1 = new DiagonalBorder();

            border1.Append(leftBorder1);
            border1.Append(rightBorder1);
            border1.Append(topBorder1);
            border1.Append(bottomBorder1);
            border1.Append(diagonalBorder1);

            Border border2 = new Border();
            LeftBorder leftBorder2 = new LeftBorder() { Style = BorderStyleValues.Thin };
            Color color1 = new Color() { Auto = true };
            leftBorder2.Append(color1);
            RightBorder rightBorder2 = new RightBorder() { Style = BorderStyleValues.Thin };
            Color color2 = new Color() { Auto = true };
            rightBorder2.Append(color2);
            TopBorder topBorder2 = new TopBorder() { Style = BorderStyleValues.Thin };
            Color color3 = new Color() { Auto = true };
            topBorder2.Append(color3);
            BottomBorder bottomBorder2 = new BottomBorder() { Style = BorderStyleValues.Thin };
            Color color4 = new Color() { Auto = true };
            bottomBorder2.Append(color4);
            DiagonalBorder diagonalBorder2 = new DiagonalBorder();

            border2.Append(leftBorder2);
            border2.Append(rightBorder2);
            border2.Append(topBorder2);
            border2.Append(bottomBorder2);
            border2.Append(diagonalBorder2);
            borders.Append(border1);
            borders.Append(border2);

            CellStyleFormats cellStyleFormats = new CellStyleFormats();
            CellFormat cellFormatS1 = new CellFormat()
            {
                NumberFormatId = (UInt32Value)0U,
                FormatId = (UInt32Value)0U,
                FontId = (UInt32Value)0U,
                FillId = (UInt32Value)0U,
                BorderId = (UInt32Value)0U
            };

            CellFormat cellFormatS2 = new CellFormat()
            {
                NumberFormatId = (UInt32Value)9U,
                FontId = (UInt32Value)0U,
                FillId = (UInt32Value)0U,
                BorderId = (UInt32Value)0U,
                ApplyFont = false,
                ApplyFill = false,
                ApplyBorder = false,
                ApplyAlignment = false,
                ApplyProtection = false
            };

            cellStyleFormats.Append(cellFormatS1);
            cellStyleFormats.Append(cellFormatS2);

            CellFormats cellFormats = new CellFormats();
            CellFormat cellFormat0 = new CellFormat()
            {
                NumberFormatId = (UInt32Value)0U,
                FontId = (UInt32Value)0U,
                FillId = (UInt32Value)0U,
                BorderId = (UInt32Value)0U,
                FormatId = (UInt32Value)0U
            };

            // Title
            CellFormat cellFormat1 = new CellFormat()
            {
                NumberFormatId = (UInt32Value)49U,
                FontId = (UInt32Value)1U,
                FillId = (UInt32Value)0U,
                BorderId = (UInt32Value)0U,
                FormatId = (UInt32Value)49U,
                ApplyFont = true,
                ApplyAlignment = true,
                Alignment = new Alignment() { Horizontal = HorizontalAlignmentValues.Left, Vertical = VerticalAlignmentValues.Center, WrapText = true }
            };

            // Header
            CellFormat cellFormat2 = new CellFormat()
            {
                NumberFormatId = (UInt32Value)49U,
                FontId = (UInt32Value)2U,
                FillId = (UInt32Value)0U,
                BorderId = (UInt32Value)1U,
                FormatId = (UInt32Value)49U,
                ApplyBorder = true,
                ApplyAlignment = true,
                Alignment = new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center, WrapText = true }
            };

            // Template Cell
            CellFormat cellFormat3 = new CellFormat()
            {
                NumberFormatId = (UInt32Value)49U,
                FontId = (UInt32Value)0U,
                FillId = (UInt32Value)0U,
                BorderId = (UInt32Value)1U,
                FormatId = (UInt32Value)49U,
                ApplyFont = true,
                ApplyBorder = true,
                ApplyAlignment = true,
                Alignment = new Alignment() { Vertical = VerticalAlignmentValues.Center, WrapText = true }
            };

            // Number 
            CellFormat cellFormat4 = new CellFormat()
            {
                NumberFormatId = (UInt32Value)3U,
                FontId = (UInt32Value)0U,
                FillId = (UInt32Value)0U,
                BorderId = (UInt32Value)1U,
                FormatId = (UInt32Value)0U,
                ApplyNumberFormat = true,
                ApplyFont = true,
                ApplyBorder = true,
                ApplyAlignment = true,
                Alignment = new Alignment() { Horizontal = HorizontalAlignmentValues.Right, Vertical = VerticalAlignmentValues.Center, WrapText = true }
            };

            // Number 2 Decimals
            CellFormat cellFormat5 = new CellFormat()
            {
                NumberFormatId = (UInt32Value)4U,
                FontId = (UInt32Value)0U,
                FillId = (UInt32Value)0U,
                BorderId = (UInt32Value)1U,
                FormatId = (UInt32Value)0U,
                ApplyNumberFormat = true,
                ApplyFont = true,
                ApplyBorder = true,
                ApplyAlignment = true,
                Alignment = new Alignment() { Horizontal = HorizontalAlignmentValues.Right, Vertical = VerticalAlignmentValues.Center, WrapText = true }
            };

            // 4 Decimals
            CellFormat cellFormat6 = new CellFormat()
            {
                NumberFormatId = numberingFormat3.NumberFormatId,
                FontId = (UInt32Value)0U,
                FillId = (UInt32Value)0U,
                BorderId = (UInt32Value)1U,
                FormatId = (UInt32Value)0U,
                ApplyNumberFormat = true,
                ApplyBorder = true,
                ApplyAlignment = true,
                Alignment = new Alignment() { Horizontal = HorizontalAlignmentValues.Right, Vertical = VerticalAlignmentValues.Center, WrapText = true }
            };

            // Date
            CellFormat cellFormat7 = new CellFormat()
            {
                NumberFormatId = numberingFormat1.NumberFormatId,
                FontId = (UInt32Value)0U,
                FillId = (UInt32Value)0U,
                BorderId = (UInt32Value)1U,
                FormatId = (UInt32Value)0U,
                ApplyNumberFormat = true,
                ApplyBorder = true,
                ApplyAlignment = true,
                Alignment = new Alignment() { Horizontal = HorizontalAlignmentValues.Left, Vertical = VerticalAlignmentValues.Center, WrapText = true }
            };

            // DateTime
            CellFormat cellFormat8 = new CellFormat()
            {
                NumberFormatId = numberingFormat7.NumberFormatId,
                FontId = (UInt32Value)0U,
                FillId = (UInt32Value)0U,
                BorderId = (UInt32Value)1U,
                FormatId = (UInt32Value)0U,
                ApplyNumberFormat = true,
                ApplyBorder = true,
                ApplyAlignment = true,
                Alignment = new Alignment() { Horizontal = HorizontalAlignmentValues.Left, Vertical = VerticalAlignmentValues.Center, WrapText = true }
            };

            // Percent 2 Decimals
            CellFormat cellFormat9 = new CellFormat()
            {
                NumberFormatId = (UInt32Value)10U,
                FontId = (UInt32Value)0U,
                FillId = (UInt32Value)0U,
                BorderId = (UInt32Value)1U,
                FormatId = (UInt32Value)1U,
                ApplyNumberFormat = true,
                ApplyFont = true,
                ApplyBorder = true,
                ApplyAlignment = true,
                Alignment = new Alignment() { Horizontal = HorizontalAlignmentValues.Right, Vertical = VerticalAlignmentValues.Center, WrapText = true }
            };
            // Percent 0 Decimals
            CellFormat cellFormat14 = new CellFormat()
            {
                NumberFormatId = (UInt32Value)9U,
                FontId = (UInt32Value)0U,
                FillId = (UInt32Value)0U,
                BorderId = (UInt32Value)1U,
                FormatId = (UInt32Value)1U,
                ApplyNumberFormat = true,
                ApplyFont = true,
                ApplyBorder = true,
                ApplyAlignment = true,
                Alignment = new Alignment() { Horizontal = HorizontalAlignmentValues.Right, Vertical = VerticalAlignmentValues.Center, WrapText = true }
            };

            // Wrap Text 
            CellFormat cellFormat10 = new CellFormat()
            {
                FontId = (UInt32Value)0U,
                FillId = (UInt32Value)0U,
                BorderId = (UInt32Value)1U,
                FormatId = (UInt32Value)0U,
                ApplyBorder = true,
                ApplyAlignment = true,
                Alignment = new Alignment() { Horizontal = HorizontalAlignmentValues.Left, Vertical = VerticalAlignmentValues.Center, WrapText = true }
            };

            // Curency 2 Decimals
            CellFormat cellFormat11 = new CellFormat()
            {
                NumberFormatId = numberingFormat5.NumberFormatId,
                FontId = (UInt32Value)0U,
                FillId = (UInt32Value)0U,
                BorderId = (UInt32Value)1U,
                FormatId = (UInt32Value)0U,
                ApplyNumberFormat = true,
                ApplyBorder = true,
                ApplyAlignment = true,
                Alignment = new Alignment() { Horizontal = HorizontalAlignmentValues.Right, Vertical = VerticalAlignmentValues.Center, WrapText = true }
            };

            // Curency 4 Decimals
            CellFormat cellFormat12 = new CellFormat()
            {
                NumberFormatId = numberingFormat6.NumberFormatId,
                FontId = (UInt32Value)0U,
                FillId = (UInt32Value)0U,
                BorderId = (UInt32Value)1U,
                FormatId = (UInt32Value)0U,
                ApplyNumberFormat = true,
                ApplyBorder = true,
                ApplyAlignment = true,
                Alignment = new Alignment() { Horizontal = HorizontalAlignmentValues.Right, Vertical = VerticalAlignmentValues.Center, WrapText = true }
            };

            // DateTime 12Hours
            CellFormat cellFormat13 = new CellFormat()
            {
                NumberFormatId = numberingFormat2.NumberFormatId,
                FontId = (UInt32Value)0U,
                FillId = (UInt32Value)0U,
                BorderId = (UInt32Value)1U,
                FormatId = (UInt32Value)0U,
                ApplyNumberFormat = true,
                ApplyBorder = true,
                ApplyAlignment = true,
                Alignment = new Alignment() { Horizontal = HorizontalAlignmentValues.Left, Vertical = VerticalAlignmentValues.Center, WrapText = true }
            };

            // Number with 0 Decimals
            CellFormat cellFormat15 = new CellFormat()
            {
                NumberFormatId = numberingFormat8.NumberFormatId,
                FontId = (UInt32Value)0U,
                FillId = (UInt32Value)0U,
                BorderId = (UInt32Value)1U,
                FormatId = (UInt32Value)1U,
                ApplyNumberFormat = true,
                ApplyFont = true,
                ApplyBorder = true,
                ApplyAlignment = true,
                Alignment = new Alignment() { Horizontal = HorizontalAlignmentValues.Right, Vertical = VerticalAlignmentValues.Center, WrapText = true }
            };

            // Number with 2 Decimals
            CellFormat cellFormat16 = new CellFormat()
            {
                NumberFormatId = numberingFormat9.NumberFormatId,
                FontId = (UInt32Value)0U,
                FillId = (UInt32Value)0U,
                BorderId = (UInt32Value)1U,
                FormatId = (UInt32Value)1U,
                ApplyNumberFormat = true,
                ApplyFont = true,
                ApplyBorder = true,
                ApplyAlignment = true,
                Alignment = new Alignment() { Horizontal = HorizontalAlignmentValues.Right, Vertical = VerticalAlignmentValues.Center, WrapText = true }
            };

            // DateTime Shorttime
            CellFormat cellFormat17 = new CellFormat()
            {
                NumberFormatId = numberingFormat10.NumberFormatId,
                FontId = (UInt32Value)0U,
                FillId = (UInt32Value)0U,
                BorderId = (UInt32Value)1U,
                FormatId = (UInt32Value)0U,
                ApplyNumberFormat = true,
                ApplyBorder = true,
                ApplyAlignment = true,
                Alignment = new Alignment() { Horizontal = HorizontalAlignmentValues.Left, Vertical = VerticalAlignmentValues.Center, WrapText = true }
            };

            cellFormats.Append(cellFormat0);
            cellFormats.Append(cellFormat1);
            cellFormats.Append(cellFormat2);
            cellFormats.Append(cellFormat3);
            cellFormats.Append(cellFormat4);
            cellFormats.Append(cellFormat5);
            cellFormats.Append(cellFormat6);
            cellFormats.Append(cellFormat7);
            cellFormats.Append(cellFormat8);
            cellFormats.Append(cellFormat9);
            cellFormats.Append(cellFormat10);
            cellFormats.Append(cellFormat11);
            cellFormats.Append(cellFormat12);
            cellFormats.Append(cellFormat13);
            cellFormats.Append(cellFormat14);
            cellFormats.Append(cellFormat15);
            cellFormats.Append(cellFormat16);
            cellFormats.Append(cellFormat17);

            CellStyles cellStyles = new CellStyles();
            CellStyle cellStyle1 = new CellStyle() { Name = "Normal", FormatId = (UInt32Value)0U, BuiltinId = (UInt32Value)0U };
            CellStyle cellStyle2 = new CellStyle() { Name = "Percent", FormatId = (UInt32Value)1U, BuiltinId = (UInt32Value)5U };
            cellStyles.Append(cellStyle1);
            cellStyles.Append(cellStyle2);

            DifferentialFormats differentialFormats = new DifferentialFormats();
            TableStyles tableStyles1 = new TableStyles() { Count = (UInt32Value)0U, DefaultTableStyle = "TableStyleMedium2", DefaultPivotStyle = "PivotStyleLight16" };

            numberingFormats.Count = UInt32Value.FromUInt32((uint)numberingFormats.ChildElements.Count);
            fonts.Count = UInt32Value.FromUInt32((uint)fonts.ChildElements.Count);
            fills.Count = UInt32Value.FromUInt32((uint)fills.ChildElements.Count);
            borders.Count = UInt32Value.FromUInt32((uint)borders.ChildElements.Count);
            cellStyleFormats.Count = UInt32Value.FromUInt32((uint)cellStyleFormats.ChildElements.Count);
            cellFormats.Count = UInt32Value.FromUInt32((uint)cellFormats.ChildElements.Count);
            cellStyles.Count = UInt32Value.FromUInt32((uint)cellStyles.ChildElements.Count);
            differentialFormats.Count = UInt32Value.FromUInt32((uint)differentialFormats.ChildElements.Count);
            tableStyles1.Count = UInt32Value.FromUInt32((uint)tableStyles1.ChildElements.Count);

            stylesheet.Append(numberingFormats);
            stylesheet.Append(fonts);
            stylesheet.Append(fills);
            stylesheet.Append(borders);
            stylesheet.Append(cellStyleFormats);
            stylesheet.Append(cellFormats);
            stylesheet.Append(cellStyles);
            stylesheet.Append(differentialFormats);
            stylesheet.Append(tableStyles1);

            workbookStylesPart.Stylesheet = stylesheet;
        }

        private static void FormatCellForNewTemplate(Cell cell, string asFormatType, bool isUsedStyleIndex = true)
        {
            uint styleIndex = 0;
            switch (asFormatType.ToLower())
            {
                case "date":
                case "dateandtime":
                case "dateandtime12hours":
                case "datetimeshorttime":
                    cell.DataType = null;
                    styleIndex = asFormatType.ToLower() == "date" ? (uint)7 : asFormatType.ToLower() == "dateandtime" ? (uint)8
                                : asFormatType.ToLower() == "dateandtime12hours" ? (uint)13 : (uint)17;
                    break;
                case "currency":
                    cell.DataType = CellValues.Number;
                    styleIndex = 11;
                    break;

                case "number":
                case "integer":
                    cell.DataType = CellValues.Number;
                    styleIndex = 4;
                    break;
                case "number1digit":
                    cell.DataType = CellValues.Number;
                    styleIndex = 15;
                    break;
                case "number2digit":
                    cell.DataType = CellValues.Number;
                    styleIndex = 16;
                    break;

                case "currency4digits":
                    cell.DataType = CellValues.Number;
                    styleIndex = 12;
                    break;

                case "number4digits":
                    cell.DataType = CellValues.Number;
                    styleIndex = 6;
                    break;

                case "percentage":
                    cell.DataType = CellValues.Number;
                    styleIndex = 9;
                    break;
                case "percentage0digits":
                    cell.DataType = CellValues.Number;
                    styleIndex = 14;
                    break;

                case "phone":
                case "fax":
                case "staticstring":
                case "dynamicstring":
                case "none":
                case "truefalse":
                    cell.DataType = CellValues.InlineString;
                    styleIndex = 3;
                    break;

                default:
                    cell.DataType = CellValues.InlineString;
                    styleIndex = 3;
                    break;
            }

            if (isUsedStyleIndex)
                cell.StyleIndex = styleIndex;
        }

        private static void GenerateWorksheetPartFilter(WorksheetPart worksheetPart, string keyReportTitle, ExportTemplate expTemplate, List<double> maxColumnWidths)
        {
            const string SUBTABLE_KEY = "SUBTABLE_KEY";

            Worksheet worksheet1 = new Worksheet();
            SheetViews sheetViews = new SheetViews();
            SheetView sheetView1 = new SheetView() { WorkbookViewId = (UInt32Value)0U };
            sheetViews.Append(sheetView1);

            // Used for set Default Row Height & for Merge Cell 
            SheetFormatProperties sheetFormatProperties1 = new SheetFormatProperties() { DefaultRowHeight = 15D, DyDescent = 0.2D };

            SheetData sheetData = new SheetData();
            List<string> arrFieldFormatDefaultValue = new List<string>() { };
            string formaterNew = string.Empty;
            int subTableIndex = 0;

            // Title Row
            int rowIndex = ROW_TITLE_INDEX;
            Row rowTitle = new Row() { RowIndex = (UInt32Value)1U };
            Cell cellTitle = new Cell() { CellReference = ExcelAgent.GetExcelColumnName(0) + rowIndex.ToString(), DataType = CellValues.String, StyleIndex = 1 };
            CellValue cellValueTitle = new CellValue();
            cellValueTitle.Text = "{[" + keyReportTitle + "]}";
            cellTitle.Append(cellValueTitle);
            rowTitle.Append(cellTitle);
            rowTitle.CustomHeight = true;
            rowTitle.Height = 17;
            sheetData.Append(rowTitle);

            // Blank Row
            rowIndex = ROW_TITLE_INDEX_2;
            rowTitle = new Row() { RowIndex = (UInt32Value)(uint)rowIndex };
            cellTitle = new Cell() { CellReference = ExcelAgent.GetExcelColumnName(0) + rowIndex.ToString(), DataType = CellValues.String, StyleIndex = 1 };
            cellValueTitle = new CellValue();
            cellValueTitle.Text = string.Empty;
            cellTitle.Append(cellValueTitle);
            rowTitle.CustomHeight = true;
            rowTitle.Height = 17;
            rowTitle.Append(cellTitle);
            sheetData.Append(rowTitle);

            // Blank Row
            rowIndex = ROW_HEADER_INDEX;
            rowTitle = new Row() { RowIndex = (UInt32Value)(uint)rowIndex };
            cellTitle = new Cell() { CellReference = ExcelAgent.GetExcelColumnName(0) + rowIndex.ToString(), DataType = CellValues.String, StyleIndex = 1 };
            cellValueTitle = new CellValue();
            cellValueTitle.Text = string.Empty;
            cellTitle.Append(cellValueTitle);
            rowTitle.CustomHeight = true;
            rowTitle.Height = 17;
            rowTitle.Append(cellTitle);
            sheetData.Append(rowTitle);

            subTableIndex++;
            Row rowSubTable = new Row() { RowIndex = (UInt32Value)(uint)(ROW_HEADER_INDEX + subTableIndex) };
            Cell cellSubKey = new Cell() { CellReference = ExcelAgent.GetExcelColumnName(0) + (ROW_HEADER_INDEX + subTableIndex).ToString(), DataType = CellValues.String, StyleIndex = 2 };
            CellValue cellSubKeyText = new CellValue();
            cellSubKeyText.Text = "{[" + SUBTABLE_KEY + "]}";
            cellSubKey.Append(cellSubKeyText);

            rowSubTable.Append(cellSubKey);
            sheetData.Append(rowSubTable);
            subTableIndex++;

            worksheet1.Append(sheetViews);
            worksheet1.Append(sheetFormatProperties1);
            worksheet1.Append(sheetData);

            MergeCells mergeCells = new MergeCells() { Count = (UInt32Value)1U };
            MergeCell mergeCell1 = new MergeCell() { Reference = string.Format("{0}{1}:{2}{3}", "A", ROW_TITLE_INDEX, "B", ROW_TITLE_INDEX_2) };
            mergeCells.Append(mergeCell1);
            worksheet1.Append(mergeCells);

            // set column width

            for (int i = 0; i < maxColumnWidths.Count; i++)
            {
                SetColumnWidth(worksheet1, (uint)i, maxColumnWidths[i]);
            }

            worksheetPart.Worksheet = worksheet1;
        }

        private static void GenerateWorksheetPartContent(WorksheetPart worksheetPart, WSExportParameter expParam, ExportTemplate expTemplate, string keyName, string keyReportTitle, List<double> maxColumnWidths)
        {
            const string SUBTABLE_KEY = "SUBTABLE_KEY";

            Worksheet worksheet1 = new Worksheet();
            SheetViews sheetViews = new SheetViews();
            SheetView sheetView1 = new SheetView() { WorkbookViewId = (UInt32Value)0U };
            sheetViews.Append(sheetView1);

            // Used for set Default Row Height & for Merge Cell 
            SheetFormatProperties sheetFormatProperties1 = new SheetFormatProperties() { DefaultRowHeight = 15D, DyDescent = 0.2D };

            SheetData sheetData = new SheetData();
            string[] arrFieldName = expParam.ExportColumnNames.Split(',');
            string[] arrHeaderName = expParam.ExportColumnHeaders.Split(',');
            string[] arrFieldFormatValue = expParam.ExportColumnFormatsForAuto.Split(',');
            List<string> arrFieldFormatDefaultValue = new List<string>() { };
            string formaterOri = expParam.ColumnFormatter;
            string formaterNew = string.Empty;
            int subTableIndex = 0;

            foreach (string item in formaterOri.Split(','))
            {
                if (!item.Contains("_total"))
                {
                    formaterNew += item + ",";
                }
            }
            formaterNew.TrimEnd(',');


            foreach (string formatItem in formaterNew.Split(','))
            {
                if (formatItem.Contains("@") && formatItem.Split('@').Count() > 2)
                {
                    arrFieldFormatDefaultValue.Add(formatItem.Split('@')[2]);
                }
                else
                {
                    arrFieldFormatDefaultValue.Add(string.Empty);
                }
            }

            int numberOfColumns = arrFieldName.Length;
            string[] excelColumnNames = new string[numberOfColumns];
            for (int n = 0; n < numberOfColumns; n++)
                excelColumnNames[n] = ExcelAgent.GetExcelColumnName(n);

            // Title Row
            int rowIndex = ROW_TITLE_INDEX;
            Row rowTitle = new Row() { RowIndex = (UInt32Value)1U };
            Cell cellTitle = new Cell() { CellReference = ExcelAgent.GetExcelColumnName(0) + rowIndex.ToString(), DataType = CellValues.String, StyleIndex = 1 };
            CellValue cellValueTitle = new CellValue();
            cellValueTitle.Text = "{[" + keyReportTitle + "]}";
            cellTitle.Append(cellValueTitle);
            rowTitle.Append(cellTitle);
            rowTitle.CustomHeight = true;
            rowTitle.Height = 17;
            sheetData.Append(rowTitle);

            // Blank Row
            rowIndex = ROW_TITLE_INDEX_2;
            rowTitle = new Row() { RowIndex = (UInt32Value)(uint)rowIndex };
            cellTitle = new Cell() { CellReference = ExcelAgent.GetExcelColumnName(0) + rowIndex.ToString(), DataType = CellValues.String, StyleIndex = 1 };
            cellValueTitle = new CellValue();
            cellValueTitle.Text = string.Empty;
            cellTitle.Append(cellValueTitle);
            rowTitle.CustomHeight = true;
            rowTitle.Height = 17;
            rowTitle.Append(cellTitle);
            sheetData.Append(rowTitle);

            if (expParam.ExtraValues.ContainsKey("{[SubTable]}"))
            {
                var subTable = expParam.ExtraValues["{[SubTable]}"] as Dictionary<string, string>;
                if (subTable.Count > 0)
                {
                    // Blank Row
                    rowIndex = ROW_HEADER_INDEX;
                    rowTitle = new Row() { RowIndex = (UInt32Value)(uint)rowIndex };
                    cellTitle = new Cell() { CellReference = ExcelAgent.GetExcelColumnName(0) + rowIndex.ToString(), DataType = CellValues.String, StyleIndex = 1 };
                    cellValueTitle = new CellValue();
                    cellValueTitle.Text = string.Empty;
                    cellTitle.Append(cellValueTitle);
                    rowTitle.CustomHeight = true;
                    rowTitle.Height = 17;
                    rowTitle.Append(cellTitle);
                    sheetData.Append(rowTitle);

                    subTableIndex++;
                    Row rowSubTable = new Row() { RowIndex = (UInt32Value)(uint)(ROW_HEADER_INDEX + subTableIndex) };
                    Cell cellSubKey = new Cell() { CellReference = ExcelAgent.GetExcelColumnName(0) + (ROW_HEADER_INDEX + subTableIndex).ToString(), DataType = CellValues.String, StyleIndex = 2 };
                    CellValue cellSubKeyText = new CellValue();
                    cellSubKeyText.Text = "{[" + SUBTABLE_KEY + "]}";
                    cellSubKey.Append(cellSubKeyText);

                    rowSubTable.Append(cellSubKey);
                    sheetData.Append(rowSubTable);
                    subTableIndex++;
                }
            }
            else
            {
                // Blank Row
                rowIndex = ROW_HEADER_INDEX;
                rowTitle = new Row() { RowIndex = (UInt32Value)(uint)rowIndex };
                cellTitle = new Cell() { CellReference = ExcelAgent.GetExcelColumnName(0) + rowIndex.ToString(), DataType = CellValues.String, StyleIndex = 1 };
                cellValueTitle = new CellValue();
                cellValueTitle.Text = string.Empty;
                cellTitle.Append(cellValueTitle);
                rowTitle.CustomHeight = true;
                rowTitle.Height = 17;
                rowTitle.Append(cellTitle);
                sheetData.Append(rowTitle);
                subTableIndex++;
            }

            rowIndex = ROW_HEADER_INDEX + subTableIndex;
            Row rowHeader = new Row() { RowIndex = (UInt32Value)(uint)(ROW_HEADER_INDEX + subTableIndex) };
            Row rowTemplate = new Row() { RowIndex = (UInt32Value)(uint)(ROW_DATA_INDEX + subTableIndex) };
            // VW-44617: Show Total row
            Row rowTotalFooter = new Row() { RowIndex = (UInt32Value)(uint)(ROW_TOTAL_INDEX + subTableIndex) };
            for (int colInx = 0; colInx < numberOfColumns; colInx++)
            {
                //Add a new Excel Cell to Header Row from A3, B3, ...
                Cell cell = new Cell() { CellReference = excelColumnNames[colInx] + (ROW_HEADER_INDEX + subTableIndex).ToString(), DataType = CellValues.String, StyleIndex = 2 };
                CellValue cellValue = new CellValue();
                cellValue.Text = arrHeaderName[colInx];
                cell.Append(cellValue);
                rowHeader.Append(cell);

                //Add a new Excel Cell to Template Row from A4, B4, ...
                Cell cellTemplate = new Cell() { CellReference = excelColumnNames[colInx] + (ROW_DATA_INDEX + subTableIndex).ToString(), DataType = CellValues.String, StyleIndex = 3 };
                CellValue cellValueTemplate = new CellValue();
                cellValueTemplate.Text = string.Format("{{{0}:[{1}|{2}|{3}]}}", keyName, arrFieldName[colInx], arrFieldFormatValue[colInx], arrFieldFormatDefaultValue[colInx]);
                FormatCellForNewTemplate(cellTemplate, arrFieldFormatValue[colInx], true);
                cellTemplate.Append(cellValueTemplate);
                rowTemplate.Append(cellTemplate);

                if (!string.IsNullOrEmpty(expParam.ExportTotalColumnNames))
                {
                    if (colInx == 0)
                    {
                        // Add Total Text: Report Total
                        Cell cellTotal = new Cell() { CellReference = excelColumnNames[colInx] + (ROW_TOTAL_INDEX + subTableIndex).ToString(), DataType = CellValues.String, StyleIndex = 3 };
                        CellValue cellTotalTemplate = new CellValue();
                        cellTotalTemplate.Text = expParam.ReportTotalText;
                        cellTotal.Append(cellTotalTemplate);
                        rowTotalFooter.Append(cellTotal);
                    }
                    else
                    {
                        if (expParam.ExportTotalColumnNames.Split(',').Where(col => col.Equals(arrFieldName[colInx])).Count() > 0)
                        {
                            // Add Total Footer
                            Cell cellTotal = new Cell() { CellReference = excelColumnNames[colInx] + (ROW_TOTAL_INDEX + subTableIndex).ToString(), DataType = CellValues.String, StyleIndex = 3 };
                            CellValue cellTotalTemplate = new CellValue();
                            cellTotalTemplate.Text = string.Format("{0}:{1}|{2}|{3}]}}", KEY_TOTAL_FOOTER_NAME, arrFieldName[colInx], arrFieldFormatValue[colInx], arrFieldFormatDefaultValue[colInx].Trim('#'));
                            FormatCellForNewTemplate(cellTotal, arrFieldFormatValue[colInx], true);
                            cellTotal.Append(cellTotalTemplate);
                            rowTotalFooter.Append(cellTotal);
                        }
                        else
                        {
                            // Add Cell Empty
                            Cell cellTotal = new Cell() { CellReference = excelColumnNames[colInx] + (ROW_TOTAL_INDEX + subTableIndex).ToString(), DataType = CellValues.String, StyleIndex = 3 };
                            CellValue cellTotalTemplate = new CellValue();
                            cellTotal.Append(cellTotalTemplate);
                            rowTotalFooter.Append(cellTotal);
                        }
                    }
                }
            }

            sheetData.Append(rowHeader);
            sheetData.Append(rowTemplate);
            sheetData.Append(rowTotalFooter);

            worksheet1.Append(sheetViews);
            worksheet1.Append(sheetFormatProperties1);
            worksheet1.Append(sheetData);

            if (numberOfColumns > 0)
            {
                MergeCells mergeCells = new MergeCells() { Count = (UInt32Value)1U };
                MergeCell mergeCell1 = new MergeCell() { Reference = string.Format("{0}{1}:{2}{3}", excelColumnNames[0], ROW_TITLE_INDEX, excelColumnNames[numberOfColumns - 1], ROW_TITLE_INDEX_2) };
                mergeCells.Append(mergeCell1);
                worksheet1.Append(mergeCells);
            }

            // set column width
            if (numberOfColumns == maxColumnWidths.Count)
            {
                for (int i = 0; i < numberOfColumns; i++)
                {
                    SetColumnWidth(worksheet1, (uint)i, maxColumnWidths[i]);
                }
            }

            worksheetPart.Worksheet = worksheet1;
        }

        private static void SetColumnWidth(Worksheet worksheet, uint index, DoubleValue dWidth)
        {
            Columns columns = worksheet.GetFirstChild<Columns>();
            uint colWidthIndex = index + 1; // Width of column begin with 1 (not 0)

            if (columns != null)
            {
                try
                {
                    IEnumerable<Column> colList = columns.Elements<Column>().Where(r => r.Min == colWidthIndex).Where(r => r.Max == colWidthIndex);
                    if (colList.Count() > 0)
                    {
                        Column col = colList.First();
                        col.Width = dWidth;
                    }
                    else
                    {
                        Column col = new Column() { Min = colWidthIndex, Max = colWidthIndex, Width = dWidth, CustomWidth = true };
                        columns.Append(col);
                    }
                }
                catch (Exception ex)
                {
                    AS.Common.Logger.LoggerManager.Debug("[ExportExcel2007] SetColumnWidth  case columns null Failed" + ex.Message);
                    AS.Common.Logger.LoggerManager.Debug("[ExportExcel2007] colWidthIndex: " + colWidthIndex);
                }
            }
            else
            {
                try
                {
                    columns = new Columns();
                    Column col = new Column() { Min = colWidthIndex, Max = colWidthIndex, Width = dWidth, CustomWidth = true };
                    columns.Append(col);
                    worksheet.InsertAfter(columns, worksheet.GetFirstChild<SheetFormatProperties>());
                }
                catch (Exception ex)
                {
                    AS.Common.Logger.LoggerManager.Debug("[ExportExcel2007] SetColumnWidth  case columns !null Failed" + ex.Message);
                    AS.Common.Logger.LoggerManager.Debug("[ExportExcel2007] colWidthIndex: " + colWidthIndex);
                }
            }
        }


        public static bool CreateExcel2007Template(string keyName, string keyReportTitle, WSExportParameter expParam, ExportTemplate expTemplate, string outputFile)
        {
            try
            {
                AS.Common.Logger.LoggerManager.Debug("[ExportExcel2007AutoTemplate] Begin Create Template: " + outputFile);
                string directoryPath = Path.GetDirectoryName(outputFile);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }   
                using (SpreadsheetDocument document = SpreadsheetDocument.Create(outputFile, SpreadsheetDocumentType.Workbook))
                {
                    WorkbookPart workbookPart1 = document.AddWorkbookPart();
                    GenerateWorkbookPartContent(workbookPart1, expTemplate);

                    WorkbookStylesPart workbookStylesPart1 = workbookPart1.AddNewPart<WorkbookStylesPart>("rsId1");

                    GenerateStylesheet(workbookStylesPart1, expParam.CurrencySymbol);

                    WorksheetPart worksheetPart1 = workbookPart1.AddNewPart<WorksheetPart>("rId1");
                    GenerateWorksheetPartContent(worksheetPart1, expParam, expTemplate, keyName, keyReportTitle, expTemplate.Sheets.First().MaxWidthColumns);
                    if (expTemplate.HasFilterSheet)
                    {
                        WorksheetPart worksheetPart2 = workbookPart1.AddNewPart<WorksheetPart>("rId2");
                        GenerateWorksheetPartFilter(worksheetPart2, keyReportTitle, expTemplate, expTemplate.Sheets.Last().MaxWidthColumns);
                    }
                }
                AS.Common.Logger.LoggerManager.Debug("[ExportExcel2007AutoTemplate] End Create Template: " + outputFile);
                return true;
            }
            catch (Exception ex)
            {
                AS.Common.Logger.LoggerManager.Debug("[ExportExcel2007AutoTemplate] " + ex.StackTrace);
                AS.Common.Logger.LoggerManager.Debug("[ExportExcel2007AutoTemplate] " + ex.InnerException);
                AS.Common.Logger.LoggerManager.Debug("[ExportExcel2007AutoTemplate] Failed, exception thrown: " + ex.Message);
                return false;
            }
        }

        public static string TrumcateAtWord(this string input, int length)
        {
            if (input == null || input.Length <= length)
                return input;
            return string.Format("{0}...", input.Substring(0, length).Trim());
        }

        public static void CreateFilterSheet2007(string filePath, WSExportParameter expParam)
        {
            using (SpreadsheetDocument myDoc = SpreadsheetDocument.Open(filePath, true))
            {
                const string REPORT_TITLE = "{[REPORT_TITLE]}";
                const string SUBTABLE_KEY = "{[SUBTABLE_KEY]}";
                const string SUBTABLE_VALUE = "{[SUBTABLE_VALUE]}";

                WorkbookPart workbookPart = myDoc.WorkbookPart;
                WorksheetPart worksheetPart = workbookPart.WorksheetParts.Last();

                string origninalSheetId = workbookPart.GetIdOfPart(worksheetPart);

                //Take advantage of AddPart for deep cloning
                SpreadsheetDocument tempSheet = SpreadsheetDocument.Create(new MemoryStream(), myDoc.DocumentType);
                WorkbookPart tempWorkbookPart = tempSheet.AddWorkbookPart();
                WorksheetPart tempWorksheetPart = tempWorkbookPart.AddPart<WorksheetPart>(worksheetPart);

                //Add cloned sheet and all associated parts to workbook
                WorksheetPart replacementPart = workbookPart.AddPart<WorksheetPart>(tempWorksheetPart);

                //WorksheetPart replacementPart = workbookPart.AddNewPart<WorksheetPart>();
                string replacementPartId = workbookPart.GetIdOfPart(replacementPart);

                OpenXmlReader reader = OpenXmlReader.Create(worksheetPart);
                OpenXmlWriter writer = OpenXmlWriter.Create(replacementPart);
                uint currentRowIndex = 0;
                while (reader.Read())
                {
                    if (reader.IsStartElement)
                    {
                        if (reader.ElementType == typeof(Row))
                        {
                            Row row = (Row)reader.LoadCurrentElement();

                            //check if this row is template row
                            IEnumerable<Cell> cells = row.Elements<Cell>();

                            foreach (var cell in cells)
                            {
                                string cellTemplate = "";
                                if (cell.DataType != null && cell.DataType == CellValues.SharedString)
                                    cellTemplate = GetSharedStringCellValue(myDoc.WorkbookPart, cell);
                                else
                                    cellTemplate = cell.InnerText;

                            }
                            currentRowIndex++;
                            int colIndex = 0;
                            foreach (var cell in row.Elements<Cell>())
                            {
                                if (currentRowIndex != row.RowIndex) cell.CellReference = new StringValue(cell.CellReference.Value.Replace(row.RowIndex.ToString(), currentRowIndex.ToString()));
                                string cellText = "";
                                if (cell.DataType != null && cell.DataType == CellValues.SharedString)
                                    cellText = GetSharedStringCellValue(myDoc.WorkbookPart, cell);
                                else
                                    cellText = cell.InnerText;

                                // Replace Extra Values
                                if (expParam.ExtraValues != null && cellText == REPORT_TITLE
                                    || cellText.StartsWith(KEY_TOTAL_FOOTER_NAME) || cellText.StartsWith(SUBTABLE_KEY) || cellText.StartsWith(SUBTABLE_VALUE))
                                {
                                    foreach (KeyValuePair<string, object> extraField in expParam.ExtraValues)
                                    {
                                        if (cellText.StartsWith(KEY_TOTAL_FOOTER_NAME))
                                        {
                                            string[] dataTotal = cellText.Split('|');
                                            string dataText = string.Format("{0}]}}", dataTotal[0]);

                                            if (dataText == extraField.Key && extraField.Value != null)
                                            {
                                                FormatCellWithASFormat(cell, extraField.Value, dataTotal[1].Replace("]}", ""), false);
                                                break;
                                            }
                                        }
                                        else if (cellText == extraField.Key)
                                        {
                                            FormatCell(cell, extraField.Value, null);
                                            //Fix height of title
                                            if (Regex.Matches(extraField.Value.ToString(), "\r\n").Count >= 2 && cellText == REPORT_TITLE)
                                                row.Height = 40;
                                            break;
                                        }
                                        else if (cellText.Contains(extraField.Key))
                                        {
                                            cell.DataType = CellValues.InlineString;
                                            cell.InlineString = new InlineString() { Text = new Text(cellText.Replace(extraField.Key, extraField.Value.ToString())) };
                                            cell.CellValue = null;
                                            break;
                                        }
                                        else if (cellText.Contains(SUBTABLE_KEY))
                                        {
                                            if (expParam.ExtraValues.ContainsKey("{[SubTable]}"))
                                            {
                                                var subTable = expParam.ExtraValues["{[SubTable]}"] as Dictionary<string, string>;
                                                if (subTable.Count > 0)
                                                {
                                                    int coutCell = 0;
                                                    foreach (var item in subTable)
                                                    {
                                                        Row rowSub = new Row() { RowIndex = (UInt32Value)(uint)(currentRowIndex) };
                                                        coutCell = 0;
                                                        for (var i = 0; i < 2; i++)
                                                        {
                                                            int styleIndex = (coutCell < 1 ? 2 : 3);
                                                            var cellSub = new Cell() { CellReference = ExcelAgent.GetExcelColumnName(coutCell) + (currentRowIndex).ToString(), DataType = CellValues.String, StyleIndex = Convert.ToUInt32(styleIndex) };
                                                            var cellSubValue = new CellValue();
                                                            cellSubValue.Text = (coutCell < 1 ? item.Key : item.Value);
                                                            cellSub.Append(cellSubValue);
                                                            rowSub.Append(cellSub);
                                                            coutCell++;
                                                            cell.CellValue.Text = null;
                                                        }
                                                        writer.WriteElement(rowSub);
                                                        currentRowIndex++;
                                                    }
                                                }
                                            }
                                            cell.CellReference = new StringValue(cell.CellReference.Value.Replace(row.RowIndex.ToString(), currentRowIndex.ToString()));
                                            row.RowIndex = currentRowIndex;
                                            cell.StyleIndex = (UInt32Value)1U;
                                            cell.CellValue.Text = null;
                                            cellText = string.Empty;
                                            break;
                                        }
                                    }
                                }

                                colIndex++;
                            }

                            row.RowIndex = currentRowIndex;//must after assign CellReference value
                            writer.WriteElement(row);

                        }
                        else
                            writer.WriteStartElement(reader);
                    }
                    else if (reader.IsEndElement)
                    {
                        if (reader.ElementType == typeof(Row))
                        {
                        }
                        else
                            writer.WriteEndElement();
                    }
                }
                reader.Close();
                writer.Close();

                Sheet sheet = workbookPart.Workbook.Descendants<Sheet>().Where(s => s.Id.Value.Equals(origninalSheetId)).First();
                sheet.Id.Value = replacementPartId;
                workbookPart.DeletePart(worksheetPart);
                workbookPart.AddPart(replacementPart);
                workbookPart.Workbook.Save();
                myDoc.Close();
            }
        }

        public static ExportReportFilterModel ExportReportFilterModelParse(string xml)
        {
            var doc = XDocument.Parse(xml);
            var root = doc.Root ?? throw new System.FormatException("XML invalid!");

            // ParamInputs
            var paramInputs = root
                .Element("ParamInputs")?
                .Elements("ParamInput")
                .Select(x => new ParamInput
                {
                    Key = (string)x.Attribute("Key"),
                    Value = (string)x.Attribute("Value")
                })
                .ToList() ?? new List<ParamInput>();

            // FilterItem (Value-only)
            var filterItem = root.Element("FilterItem") != null
                ? new ValueItem { Value = (string)root.Element("FilterItem")?.Attribute("Value") }
                : null;

            // SortItem (Value-only)
            var sortItem = root.Element("SortItem") != null
                ? new ValueItem { Value = (string)root.Element("SortItem")?.Attribute("Value") }
                : null;

            var customView = root.Element("CustomView") != null
                ? new ValueItem { Value = (string)root.Element("CustomView")?.Attribute("Value") }
                : null;

            var reportHeader = root.Element("ReportHeader") != null
                ? new ValueItem { Value = (string)root.Element("ReportHeader")?.Attribute("Value") }
                : null;

            return new ExportReportFilterModel()
            {
                ParamInputs = paramInputs,
                FilterItem = filterItem,
                SortItem = sortItem,
                CustomView = customView,
                ReportHeader = reportHeader
            };
        }

    }
}