using AS.Common;
using AS.Common.Utilities;
using AS.Common.WebServiceExport;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

/// <summary>
/// Summary description for ExportHelper
/// </summary>
public static class ExportHelper
{
    const int ROW_TITLE_INDEX = 1;
    const int ROW_TITLE_INDEX_2 = 2;
    const int ROW_HEADER_INDEX = 3;

    public static void CreateExcelMultipleSheets(string excelFilename, List<MultipleSheet> mulSheets)
    {
        try
        {
            string filePath = System.Web.HttpContext.Current.Server.MapPath(WebSiteSettings.ExportTempFolder);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            var fileName = string.Format("{0}{1}.xlsx", filePath, Guid.NewGuid().ToString());

            using (SpreadsheetDocument document = SpreadsheetDocument.Create(fileName, SpreadsheetDocumentType.Workbook))
            {
                CreateParts(document, mulSheets);
            }

            TransferFileToClient(fileName, excelFilename);
        }
        catch (Exception ex)
        {
            AS.Common.Logger.LoggerManager.Debug("[ExcelMultipleSheets] fail" + ex.Message);
        }
    }

    private static void CreateParts(SpreadsheetDocument document, List<MultipleSheet> mulSheets)
    {
        WorkbookPart workbookPart = document.AddWorkbookPart();
        Workbook workbook = new Workbook();
        workbookPart.Workbook = workbook;

        WorkbookStylesPart workbookStylesPart1 = workbookPart.AddNewPart<WorkbookStylesPart>("rsId1");
        GenerateStylesheet(workbookStylesPart1);

        Sheets sheets = new Sheets();

        //  Loop through each of the DataTables in our DataSet, and create a new Excel Worksheet for each.
        uint worksheetNumber = 1;
        foreach (var item in mulSheets)
        {
            //  For each worksheet you want to create
            string workSheetID = "rId" + worksheetNumber.ToString();
            string worksheetName = item.SheetName;

            WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>(workSheetID);
            WriteDataTableToExcelWorksheet(worksheetPart, item);

            Sheet sheet = new Sheet() { Name = worksheetName, SheetId = (UInt32Value)worksheetNumber, Id = workSheetID };
            sheets.Append(sheet);

            worksheetNumber++;
        }

        workbook.Append(sheets);
    }
    private static void WriteDataTableToExcelWorksheet(WorksheetPart worksheetPart1, MultipleSheet sheet)
    {
        Worksheet worksheet = new Worksheet();
        SheetViews sheetViews = new SheetViews();

        SheetView sheetView = new SheetView() { TabSelected = true, WorkbookViewId = (UInt32Value)0U };
        sheetViews.Append(sheetView);

        // Used for set Default Row Height & for Merge Cell 
        SheetFormatProperties sheetFormatProperties1 = new SheetFormatProperties() { DefaultRowHeight = 15D, DyDescent = 0.2D };

        SheetData sheetData1 = new SheetData();

        string[] arrFieldName = sheet.ExportColumnNames.Split(',');
        string[] arrHeaderName = sheet.ExportColumnHeaders.Split(',');
        string[] arrFieldFormatValue = sheet.ExportColumnFormatsForAuto.Split(',');

        int numberOfColumns = arrFieldName.Count();
        string[] excelColumnNames = new string[numberOfColumns];
        for (int n = 0; n < numberOfColumns; n++)
            excelColumnNames[n] = GetExcelColumnName(n);

        int rowIndex = ROW_TITLE_INDEX;
        Row rowTitle = new Row() { RowIndex = (UInt32Value)1U };
        Cell cellTitle = new Cell() { CellReference = GetExcelColumnName(0) + rowIndex.ToString(), DataType = CellValues.String, StyleIndex = 1 };
        CellValue cellValueTitle = new CellValue();
        cellValueTitle.Text = sheet.ReportTitle;
        cellTitle.Append(cellValueTitle);
        rowTitle.Append(cellTitle);
        rowTitle.CustomHeight = true;
        rowTitle.Height = 17;
        sheetData1.Append(rowTitle);

        // Blank Row
        rowIndex = ROW_TITLE_INDEX_2;
        rowTitle = new Row() { RowIndex = (UInt32Value)(uint)rowIndex };
        cellTitle = new Cell() { CellReference = GetExcelColumnName(0) + rowIndex.ToString(), DataType = CellValues.String, StyleIndex = 1 };
        cellValueTitle = new CellValue();
        cellValueTitle.Text = string.Empty;
        cellTitle.Append(cellValueTitle);
        rowTitle.CustomHeight = true;
        rowTitle.Height = 17;
        rowTitle.Append(cellTitle);
        sheetData1.Append(rowTitle);

        //
        //  Create the Header row in our Excel Worksheet
        //
        rowIndex = ROW_HEADER_INDEX;
        Row row1 = new Row() { RowIndex = (UInt32Value)(uint)rowIndex };
        for (int colInx = 0; colInx < numberOfColumns; colInx++)
        {
            Cell cell = new Cell() { CellReference = excelColumnNames[colInx] + rowIndex, DataType = CellValues.String, StyleIndex = 2 };
            CellValue cellValue1 = new CellValue();
            cellValue1.Text = arrHeaderName[colInx];
            cell.Append(cellValue1);
            row1.Append(cell);
        }
        sheetData1.Append(row1);

        worksheet.Append(sheetViews);
        worksheet.Append(sheetFormatProperties1);
        worksheet.Append(sheetData1);

        //
        //  Now, step through each row of data in our DataTable...
        //
        double[] maxColumnWidths = new double[arrHeaderName.Count()];
        foreach (DataRow dsrow in sheet.Data.Rows)
        {
            ++rowIndex;
            Row newRow = new Row() { RowIndex = (UInt32Value)(uint)rowIndex };
            for (int colInx = 0; colInx < numberOfColumns; colInx++)
            {
                Cell cell = new Cell() { CellReference = excelColumnNames[colInx] + rowIndex.ToString() };
                cell.DataType = CellValues.String;
                cell.StyleIndex = 1;
                var colName = arrFieldName[colInx];
                var asFormatType = arrFieldFormatValue[colInx];
                if (dsrow.Table.Columns.Contains(colName))
                {
                    var data = dsrow[colName];
                    object text = data;
                    var valueFormated = FormatCellWithASFormat(cell, text, asFormatType);
                    maxColumnWidths = UpdateMaxColumnWidths(maxColumnWidths, colInx, valueFormated, asFormatType);

                    newRow.Append(cell);
                }
            }

            sheetData1.Append(newRow);
        }

        if (numberOfColumns > 0)
        {
            MergeCells mergeCells = new MergeCells() { Count = (UInt32Value)1U };
            MergeCell mergeCell1 = new MergeCell() { Reference = string.Format("{0}{1}:{2}{3}", excelColumnNames[0], ROW_TITLE_INDEX, excelColumnNames[numberOfColumns - 1], ROW_TITLE_INDEX_2) };
            mergeCells.Append(mergeCell1);
            worksheet.Append(mergeCells);
        }

        // set column width
        if (numberOfColumns == maxColumnWidths.Count())
        {
            for (int i = 0; i < numberOfColumns; i++)
            {
                SetColumnWidth(worksheet, (uint)i, maxColumnWidths[i]);
            }
        }

        worksheetPart1.Worksheet = worksheet;
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
                AS.Common.Logger.LoggerManager.Error("[ExcelMultipleSheets] SetColumnWidth  case columns null Failed" + ex.Message);
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
                AS.Common.Logger.LoggerManager.Debug("[ExcelMultipleSheets] SetColumnWidth  case columns !null Failed" + ex.Message);
            }
        }
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
    private static string FormatCellWithASFormat(Cell cell, object dataItem, string asFormatType, bool isUsedStyleIndex = true, string defaultEmptyString = "")
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
    private static bool IsLongText(string value)
    {
        DoubleValue width = Math.Truncate(((double)value.Length * 7 + 5) / 7 * 256) / 256; //for font Calibri           
        return width > 55;
    }
    private static string GetExcelColumnName(int columnIndex)
    {
        //  Convert a zero-based column index into an Excel column reference (A, B, C.. Y, Y, AA, AB, AC... AY, AZ, B1, B2..)
        //  Each Excel cell we write must have the cell name stored with it.
        //
        if (columnIndex < 26)
            return ((char)('A' + columnIndex)).ToString();

        char firstChar = (char)('A' + (columnIndex / 26) - 1);
        char secondChar = (char)('A' + (columnIndex % 26));

        return string.Format("{0}{1}", firstChar, secondChar);
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

    public static void TransferFileToClient(string fileName, string exportedName)
    {
        HttpResponse response = HttpContext.Current.Response;
        response.Clear();
        try
        {

            response.ContentType = "application/octet-stream";
            string header = "attachment; filename=" + exportedName;
            //TODO: remember to do veracode VeraCodeSolution.RemoveCRLF(header)
            response.AddHeader("content-disposition", VeraCodeSolution.RemoveCRLF(header));
            response.TransmitFile(fileName);
            response.Flush();
        }
        catch
        {
            //TODO: write log here.
        }
        finally
        {

            if (File.Exists(fileName))
                File.Delete(fileName);
            response.End();
        }
    }
}

public class MultipleSheet
{
    public string ReportTitle { get; set; }
    public string SheetName { get; set; }
    public string ColumnFormatter
    {
        get;
        set;
    }
    public string ExportColumnHeaders
    {
        get;
        set;
    }
    public string ExportColumnFormatsForAuto { get; set; }
    public string ExportColumnNames
    {
        get;
        set;
    }
    public DataTable Data { get; set; }
}