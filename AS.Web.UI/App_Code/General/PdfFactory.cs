using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.IO;
using WebSupergoo.ABCpdf9;

/// <summary>
/// Summary description for PdfFactory
/// </summary>
namespace AS.Common.Utilities
{

    public class PdfFactory
    {

        public static void CreatePdfTemplate(object[] dataSources, object[] replaceStrings, string templateFile, Stream outputStream)
        {
            //analyse template file
            System.IO.StreamReader templateReader = new System.IO.StreamReader(templateFile);
            string template = templateReader.ReadToEnd();
            templateReader.Close();//href=""
            string header = GetFullString(template, "<header", "</header>");
            string footer = GetFullString(template, "<footer", "</footer>");
            string content = GetString(template, "<content>", "</content>");
            string[] allTables = GetFullStrings(content, "<table ", "</table>");
            string[] contents = null;
            TableSettings[] tableSettings = null;
            ColumnSettings[][] colSettings = null;
            //replace string here
            string attStr = null;
            if (allTables != null)
            {
                foreach (string table in allTables) content = content.Replace(table, "\0");
                if (replaceStrings != null) content = string.Format(content, replaceStrings);
                content = content.Replace("[[", "{").Replace("]]", "}");

                contents = content.Split('\0');
                tableSettings = new TableSettings[allTables.Length];
                colSettings = new ColumnSettings[allTables.Length][];
                for (int k = 0; k < allTables.Length; k++)
                {
                    string tableAttributes = GetString(allTables[k], "<table ", ">");
                    tableSettings[k] = new TableSettings();
                    tableSettings[k].RowColor = GetAttribute(tableAttributes, "rowColor");
                    TableSettings.BorderType borderType = TableSettings.BorderType.None;
                    attStr = GetAttribute(tableAttributes, "border");
                    if (attStr != null)
                    {
                        switch (attStr.ToLower())
                        {
                            case "table":
                                borderType = TableSettings.BorderType.Table;
                                break;
                            case "column":
                                borderType = TableSettings.BorderType.Column;
                                break;
                            case "cell":
                                borderType = TableSettings.BorderType.Cell;
                                break;
                        }
                    }
                    tableSettings[k].Border = borderType;


                    string[] columns = GetStrings(allTables[k], "<col ", "/>");
                    colSettings[k] = new ColumnSettings[columns.Length];
                    for (int i = 0; i < columns.Length; i++)
                    {
                        colSettings[k][i] = new ColumnSettings();
                        colSettings[k][i].DataField = GetAttribute(columns[i], "dataField");
                        colSettings[k][i].HeaderText = GetAttribute(columns[i], "headerText");
                        colSettings[k][i].FormatString = GetAttribute(columns[i], "formatString");
                        colSettings[k][i].NegativeNumberFormatString = GetAttribute(columns[i], "negativeNumberFormatString");
                        colSettings[k][i].HeaderBgColor = GetAttribute(columns[i], "headerBgColor");
                        attStr = GetAttribute(columns[i], "width");
                        if (attStr != null) colSettings[k][i].Width = double.Parse(attStr);
                        else throw new Exception("Please set width for the columns");

                    }

                }
            }
            else
            {
                if (replaceStrings != null) content = string.Format(content, replaceStrings);
                content = content.Replace("[[", "{").Replace("]]", "}");
            }
            PdfFactory pdfCreator = new PdfFactory();
            if (header != null)
            {
                string headerAttributes = GetString(header, "<header", ">");
                attStr = GetAttribute(headerAttributes, "height");
                if (attStr != null) pdfCreator.SetHeaderSize(double.Parse(attStr));
                header = GetString(header, ">", "</header>");
                pdfCreator.CreateHeader += new PdfFactoryHanlder(delegate(PdfFactory.PdfAgent agent, XRect rect)
                {
                    agent.AddHtml(header.Replace("{[PageNumber]}", agent.PdfDoc.PageNumber.ToString()).Replace("{[TotalPage]}", agent.PdfDoc.PageCount.ToString()));
                });
            }
            else
            {
                pdfCreator.SetHeaderSize(0);
            }
            pdfCreator.CreateContent += new PdfFactoryHanlder(delegate(PdfFactory.PdfAgent agent, XRect rect)
            {
                if (allTables == null)
                {
                    agent.AddHtml(content);
                }
                else
                {
                    for (int i = 0; i < contents.Length; i++)
                    {
                        agent.AddHtml(contents[i]);
                        if (i < allTables.Length) agent.AddTable(tableSettings[i], colSettings[i], dataSources[i]);
                    }
                }
            });
            if (footer != null)
            {
                string footerAttributes = GetString(footer, "<footer", ">");
                attStr = GetAttribute(footerAttributes, "height");
                if (attStr != null) pdfCreator.SetFooterSize(double.Parse(attStr));
                footer = GetString(footer, ">", "</footer>");
                pdfCreator.CreateFooter += new PdfFactoryHanlder(delegate(PdfFactory.PdfAgent agent, XRect rect)
                {
                    agent.AddHtml(footer.Replace("{[PageNumber]}", agent.PdfDoc.PageNumber.ToString()).Replace("{[TotalPage]}", agent.PdfDoc.PageCount.ToString()));
                });

            }
            else
            {
                pdfCreator.SetFooterSize(0);
            }
            pdfCreator.Create(outputStream);
        }
        static string GetAttribute(string txt, string attr)
        {
            txt = txt.Replace("\\\"", "['']");

            string ret = GetString(txt, attr + "=\"", "\"");
            if (ret != null) ret = ret.Replace("['']", "\"");
            return ret;
        }
        static string GetString(string txt, string begin, string end)
        {
            string[] ret = GetStrings(txt, begin, end);
            if (ret != null && ret.Length > 0) return ret[0];
            return null;
        }
        static string[] GetStrings(string txt, string begin, string end)
        {
            txt = txt.Replace("\r", "{[r]}").Replace("\n", "{[n]}");
            Regex reg = new Regex(begin + "(?<data>.*?)" + end);
            MatchCollection matches = reg.Matches(txt);
            if (matches == null) return null;
            string[] ret = new string[matches.Count];
            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = matches[i].Groups[1].Value.Replace("{[r]}", "\r").Replace("{[n]}", "\n");
            }
            return ret;
        }
        static string GetFullString(string txt, string begin, string end)
        {
            string[] ret = GetFullStrings(txt, begin, end);
            if (ret != null && ret.Length > 0) return ret[0];
            return null;
        }
        static string[] GetFullStrings(string txt, string begin, string end)
        {
            txt = txt.Replace("\r", "{[r]}").Replace("\n", "{[n]}");
            Regex reg = new Regex(begin + "(?<data>.*?)" + end);
            MatchCollection matches = reg.Matches(txt);
            if (matches == null || matches.Count == 0) return null;
            string[] ret = new string[matches.Count];
            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = matches[i].Value.Replace("{[r]}", "\r").Replace("{[n]}", "\n");
            }
            return ret;
        }
        public class TableSettings
        {
            public string Name { get; set; }
            public enum BorderType
            {
                None,
                Table,
                Column,
                Cell,
                HeaderBottom,
                HeaderTop,
                Empty
            }
            public string RowColor { get; set; }
            public string AltRowColor { get; set; }
            public BorderType Border { get; set; }

        }
        public class ColumnSettings
        {
            public string Name { get; set; }
            public string HeaderText { get; set; }
            public string HeaderBgColor { get; set; }
            public string DataField { get; set; }
            public string FormatString { get; set; }
            public string NegativeNumberFormatString { get; set; }
            public double Width { get; set; }
        }

        public class PdfAgent
        {
            Doc _doc;
            XRect _container;
            PdfFactory _factory;
            bool _isNoBreakPage;
            internal PdfAgent(PdfFactory factory, bool isNoBreakPage)
            {
                _factory = factory;
                _doc = factory.PdfDoc;
                _container = factory.Container;
                _isNoBreakPage = isNoBreakPage;
            }
            public Doc PdfDoc
            {
                get
                {
                    return _doc;
                }
            }
            public void NewPage()
            {
                _doc.Page = _doc.AddPage();
            }
            public void NewLine()
            {
                _doc.AddHtml("<BR>");
            }
            public void NewLine(int lines)
            {
                for (int i = 0; i < lines; i++)
                    _doc.AddHtml("<BR>");
            }
            void CheckBreakPage()
            {
                if (_isNoBreakPage) return;
                if (_doc.Pos.Y <= _container.Bottom + _doc.FontSize) NewPage();
            }
            static object GetDataValue(object container, string propName)
            {
                if (container == null)
                {
                    throw new ArgumentNullException("container");
                }
                if (string.IsNullOrEmpty(propName))
                {
                    throw new ArgumentNullException("propName");
                }
                PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(container).Find(propName, true);
                if (propertyDescriptor != null)
                {
                    return propertyDescriptor.GetValue(container);
                }
                return null;
            }
            public void AddTable(TableSettings tableSettings, ColumnSettings[] colSettings, object dataSource)
            {
                AddTable(tableSettings, colSettings, dataSource, null);
            }
            public void AddTable(TableSettings tableSettings, ColumnSettings[] colSettings, object dataSource, PdfCellFormatHanlder formatCell)
            {
                CheckBreakPage();
                string oldRect = _doc.Rect.String;

                int colCount = colSettings.Length;


                double val = 0;
                PdfTable table = new PdfTable(_doc, colCount, 0, _doc.Pos.Y);
                table.Name = tableSettings.Name;
                double[] widthSettings = new double[colCount];
                string[] headers = new string[colCount];
                string[] bgColorHeaders = new string[colCount];

                for (int i = 0; i < colCount; i++)
                {
                    headers[i] = colSettings[i].HeaderText;
                    widthSettings[i] = colSettings[i].Width;
                    bgColorHeaders[i] = colSettings[i].HeaderBgColor;
                }
                table.SetColumnWidths(widthSettings);
                table.CellPadding = 5;
                table.RepeatHeader = true;

                if (dataSource is DataTable)
                {
                    dataSource = ((DataTable)dataSource).DefaultView;
                }
                if (dataSource is IEnumerable)
                {
                    int page = 1;
                    bool isAltRow = false;

                    table.SetHeader(headers, bgColorHeaders, headers);
                    isAltRow = !isAltRow;
                    int dataIndex = 0;
                    foreach (object dataItem in (IEnumerable)dataSource)
                    {
                        dataIndex++;
                        table.NextRow();
                        string[] rowDatas = new string[colCount];
                        for (int i = 0; i < colCount; i++)
                        {
                            object dataCell = GetDataValue(dataItem, colSettings[i].DataField);
                            if (string.IsNullOrEmpty(colSettings[i].FormatString))
                            {
                                rowDatas[i] = dataCell.ToString();
                            }
                            else
                            {
                                rowDatas[i] = string.Format(colSettings[i].FormatString, dataCell);
                            }
                            if (formatCell != null) rowDatas[i] = formatCell(tableSettings.Name, table.Row, i, colSettings[i].Name, rowDatas[i], dataCell, dataIndex);
                            else if (_factory.FormatCell != null) rowDatas[i] = _factory.FormatCell(tableSettings.Name, table.Row, i, colSettings[i].Name, rowDatas[i], dataCell, dataIndex);
                        }
                        table.AddHtml(rowDatas);
                        if (_doc.PageNumber > page)
                        {
                            page = _doc.PageNumber;
                            isAltRow = true;
                        }
                        if (!isAltRow)
                            if (!string.IsNullOrEmpty(tableSettings.AltRowColor)) table.FillRow(tableSettings.AltRowColor, table.Row);

                            else
                                if (!string.IsNullOrEmpty(tableSettings.RowColor)) table.FillRow(tableSettings.RowColor, table.Row);
                        isAltRow = !isAltRow;

                    }
                    val = table.LastPosY;
                }
                switch (tableSettings.Border)
                {
                    case TableSettings.BorderType.Table:
                        table.Frame();
                        break;
                    case TableSettings.BorderType.Column:
                        table.FrameColumns();
                        table.Frame();
                        break;
                    case TableSettings.BorderType.Cell:
                        table.FrameCells();
                        break;
                }

                _doc.Rect.SetRect(new XRect(oldRect));
                _doc.Pos.Y = val - 5;
            }

            /// <summary>
            /// Add table to pdf document.
            /// </summary>
            /// <param name="tableSettings"></param>
            /// <param name="colSettings"></param>
            /// <param name="dataSource"></param>
            /// <param name="formatCell"></param>
            /// <param name="padding"></param>
            /// <param name="isTotalTable"></param>
            public void AddTable(TableSettings tableSettings, ColumnSettings[] colSettings, object dataSource, PdfCellFormatHanlder formatCell, double padding, bool isTotalTable)
            {
                CheckBreakPage();
                string oldRect = _doc.Rect.String;

                int colCount = colSettings.Length;

                double val = 0;
                PdfTable table = new PdfTable(_doc, colCount, padding, _doc.Pos.Y);

                table.Name = tableSettings.Name;
                double[] widthSettings = new double[colCount];
                string[] headers = new string[colCount];
                string[] bgColorHeaders = new string[colCount];

                for (int i = 0; i < colCount; i++)
                {
                    headers[i] = colSettings[i].HeaderText;
                    widthSettings[i] = colSettings[i].Width;
                    bgColorHeaders[i] = colSettings[i].HeaderBgColor;
                }
                table.SetColumnWidths(widthSettings);
                table.RepeatHeader = true;

                if (tableSettings.Border == TableSettings.BorderType.HeaderTop)
                {
                    table.HeaderLinePosition = 1;
                }
                else if (tableSettings.Border == TableSettings.BorderType.HeaderBottom)
                {
                    table.HeaderLinePosition = 2;
                }
                else if (tableSettings.Border == TableSettings.BorderType.Empty)
                {
                    table.HeaderLinePosition = 3;
                }
                if (dataSource is DataTable)
                {
                    dataSource = ((DataTable)dataSource).DefaultView;
                }
                if (dataSource is IEnumerable)
                {
                    int page = 1;
                    bool isAltRow = false;

                    table.SetHeader(headers, bgColorHeaders, headers);
                    isAltRow = !isAltRow;
                    int dataIndex = 0;
                    foreach (object dataItem in (IEnumerable)dataSource)
                    {
                        dataIndex++;
                        table.NextRow();
                        string[] rowDatas = new string[colCount];
                        for (int i = 0; i < colCount; i++)
                        {
                            object dataCell = GetDataValue(dataItem, colSettings[i].DataField);
                            if (string.IsNullOrEmpty(colSettings[i].FormatString))
                            {
                                rowDatas[i] = dataCell.ToString();
                            }
                            else
                            {
                                rowDatas[i] = string.Format(colSettings[i].FormatString, dataCell);
                            }

                            if (!string.IsNullOrEmpty(colSettings[i].NegativeNumberFormatString))
                            {
                                if (dataCell is double && Convert.ToDouble(dataCell) < 0)
                                {
                                    rowDatas[i] = string.Format(colSettings[i].NegativeNumberFormatString, 0 - Convert.ToDouble(dataCell));
                                }
                                else if (dataCell is int && Convert.ToInt32(dataCell) < 0)
                                {
                                    rowDatas[i] = string.Format(colSettings[i].NegativeNumberFormatString, 0 - Convert.ToInt32(dataCell));
                                }
                            }


                            if (formatCell != null) rowDatas[i] = formatCell(tableSettings.Name, table.Row, i, colSettings[i].Name, rowDatas[i], dataCell, dataIndex);
                            else if (_factory.FormatCell != null) rowDatas[i] = _factory.FormatCell(tableSettings.Name, table.Row, i, colSettings[i].Name, rowDatas[i], dataCell, dataIndex);
                        }
                        table.AddHtml(rowDatas);
                        if (_doc.PageNumber > page)
                        {
                            page = _doc.PageNumber;
                            isAltRow = true;
                        }
                        if (!isAltRow)
                            if (!string.IsNullOrEmpty(tableSettings.AltRowColor)) table.FillRow(tableSettings.AltRowColor, table.Row);

                            else
                                if (!string.IsNullOrEmpty(tableSettings.RowColor)) table.FillRow(tableSettings.RowColor, table.Row);
                        isAltRow = !isAltRow;

                    }

                    // Grid view have no record
                    if (dataIndex == 0 && !isTotalTable)
                    {
                        table.NextRow();
                        table.AddHtml("No data found.");
                    }

                    val = table.LastPosY;
                }
                switch (tableSettings.Border)
                {
                    case TableSettings.BorderType.Table:
                        table.Frame();
                        break;
                    case TableSettings.BorderType.Column:
                        table.FrameColumns();
                        table.Frame();
                        break;
                    case TableSettings.BorderType.Cell:
                        table.FrameCells();
                        break;
                }

                _doc.Rect.SetRect(new XRect(oldRect));
                _doc.Pos.Y = val - 5;
            }
            public void AddHtml(string html)
            {
                CheckBreakPage();
                int id = _doc.AddHtml(html);
                while (_doc.Chainable(id))
                {
                    _doc.Page = _doc.AddPage();
                    id = _doc.AddHtml("", id);

                }
            }
            public void AddHtml(string html, string color, int padding)
            {
                CheckBreakPage();
                string oldRect = _doc.Rect.String;
                double oldVal = _doc.Pos.Y;
                string oldColor = _doc.Color.String;
                

                //fill
                _doc.Color.String = color;
                XRect imgRect = new XRect();
                imgRect.Move(_doc.Pos.X, oldVal - _doc.FontSize - padding + 1);
                imgRect.Resize(_doc.Rect.Width, _doc.FontSize + 2 * padding);
                _doc.Rect.SetRect(imgRect);
                _doc.FillRect();
                //_doc.Color.String = "0 0 0";
                //_doc.FrameRect();
                //restore
                _doc.Color.String = oldColor;
                _doc.Rect.SetRect(new XRect(oldRect));
                _doc.Pos.Y = oldVal;
                //add html
                _doc.AddHtml(html);
                
                
                
            }
            public void AddTextTitle(string html, string color, int padding)
            {
                CheckBreakPage();
                string oldRect = _doc.Rect.String;
                double oldVal = _doc.Pos.Y;
                string oldColor = _doc.Color.String;


                //restore - add html to get position Y.
                _doc.Rect.SetRect(new XRect(oldRect));
                _doc.Pos.Y = oldVal;
                //add html
                _doc.AddHtml(html);

                //fill background
                _doc.Color.String = color;
                XRect imgRect = new XRect();
                imgRect.Move(_doc.Pos.X, _doc.Pos.Y - padding);
                imgRect.Resize(_doc.Rect.Width, oldVal - _doc.Pos.Y + 2 * padding);
                _doc.Rect.SetRect(imgRect);
                _doc.FillRect();

                // re add html to display on background
                _doc.Color.String = oldColor;
                _doc.Rect.SetRect(new XRect(oldRect));
                _doc.Pos.Y = oldVal;
                //add html
                _doc.AddHtml(html);

                _doc.Pos.Y = _doc.Pos.Y - padding;


            }
            public void AddImage(XImage img, double width, double height)
            {
                if (_doc.Pos.Y - height < _container.Bottom) NewPage();
                string oldRect = _doc.Rect.String;
                double val = _doc.Pos.Y;
                _doc.Pos.Y -= 3;
                XRect imgRect = new XRect();
                imgRect.Move(_doc.Pos.X, _doc.Pos.Y - height);
                imgRect.Resize(width, height);
                _doc.Rect.SetRect(imgRect);


                _doc.AddImageObject(img);
                _doc.Rect.SetRect(new XRect(oldRect));
                _doc.Pos.Y = val - height - 3;
            }
            public void AddText(string text)
            {
                CheckBreakPage();
                int id = _doc.AddText(text);
                while (_doc.Chainable(id))
                {
                    _doc.Page = _doc.AddPage();
                    id = _doc.AddHtml("", id);

                }
            }
            public void AddUrl(string url)
            {
                CheckBreakPage();
                int id = _doc.AddImageUrl(url, true, _container.Rectangle.Width, true);
                while (_doc.Chainable(id))
                {
                    _doc.Page = _doc.AddPage();
                    id = _doc.AddImageToChain(id);
                }
            }
            public void AddBrowserHtml(string html)
            {
                CheckBreakPage();
                int id = _doc.AddImageHtml(html, true, _container.Rectangle.Width, true);
                while (_doc.Chainable(id))
                {
                    _doc.Page = _doc.AddPage();
                    id = _doc.AddImageToChain(id);
                }
            }

            /// <summary>
            /// Add a box html
            /// </summary>
            /// <param name="html"></param>
            /// <param name="color"></param>
            /// <param name="height"></param>
            /// <param name="width"></param>
            /// <param name="marginLeft"></param>
            /// <param name="marginTop"></param>
            public void AddBoxHtml(string html, string color, double height, double width, double marginLeft, double marginTop)
            {
                CheckBreakPage();
                string oldRect = _doc.Rect.String;
                double oldVal = _doc.Pos.Y;
                string oldColor = _doc.Color.String;

                //fill
                _doc.Color.String = color;
                XRect imgRect = new XRect();
                imgRect.Move(_doc.Pos.X + marginLeft, oldVal - marginTop);
                imgRect.Resize(width, height);
                _doc.Rect.SetRect(imgRect);
                _doc.FillRect();

                //restore
                _doc.Color.String = oldColor;
                _doc.Rect.SetRect(new XRect(oldRect));
                _doc.Pos.Y = oldVal;
                //add html
                _doc.AddHtml(html);
            }

        }


        public delegate void PdfFactoryHanlder(PdfFactory.PdfAgent agent, XRect rect);
        public delegate string PdfCellFormatHanlder(string tableName, int rowIndex, int colIndex, string colName, string text, object dataCell, int dataIndex);
        public event PdfFactoryHanlder CreateHeader;
        public event PdfFactoryHanlder CreateContent;
        public event PdfFactoryHanlder CreateFooter;
        public event PdfCellFormatHanlder FormatCell;
        Doc _doc;
        XRect _header, _footer, _container;
        const double ZONE_SPACE = 10;
        int _count;
        double _margin, _pageWidth, _pageHeight, _headerHeight, _footerHeight;

        public PdfFactory()
        {
            _doc = new Doc();

#if PDF7

#else
            _doc.HtmlOptions.Engine = EngineType.Gecko;
#endif
            _doc.Width = 0.1;
            _header = new XRect();
            _footer = new XRect();
            _container = new XRect();
            _margin = 15;
            _pageWidth = 550;
            _pageHeight = 690;
            _headerHeight = 20;
            _footerHeight = 20;
        }

        public void SetPageMargin(double margin)
        {
            _margin = margin;

        }
        public void SetPageSize(double width, double height)
        {
            _pageWidth = width;
            _pageHeight = height;

        }
        public void SetHeaderSize(double height)
        {
            _headerHeight = height;
        }
        public void SetFooterSize(double height)
        {
            _footerHeight = height;
        }
        Doc PdfDoc { get { return _doc; } }
        XRect Container { get { return _container; } }
        protected void OnCreateHeader(XRect rect)
        {
            if (CreateHeader != null)
            {
                CreateHeader(new PdfAgent(this, true), new XRect(rect.String));
            }
        }
        protected void OnCreateContent(XRect rect)
        {
            if (CreateContent != null)
            {
                CreateContent(new PdfAgent(this, false), new XRect(rect.String));
            }
        }
        protected void OnCreateFooter(XRect rect)
        {
            if (CreateFooter != null)
            {
                CreateFooter(new PdfAgent(this, true), new XRect(rect.String));
            }
        }
        void ApplyConfig()
        {
            _doc.MediaBox.SetRect(0, 0, _pageWidth, _pageHeight);
            _header.SetRect(_doc.MediaBox.Left + _margin, _doc.MediaBox.Height - _margin - _headerHeight, _doc.MediaBox.Width - 2 * _margin, _headerHeight);
            _footer.SetRect(_doc.MediaBox.Left + _margin, _margin, _doc.MediaBox.Width - 2 * _margin, _footerHeight);
            _container.SetRect(_doc.MediaBox.Left + _margin, _footer.Top + ZONE_SPACE, _doc.MediaBox.Width - 2 * _margin, _header.Bottom - _footer.Top - 2 * ZONE_SPACE);
        }

        public void Create(System.IO.Stream stream)
        {
            ApplyConfig();
            _doc.Rect.SetRect(_container);

            OnCreateContent(_container);
            _count = _doc.PageCount;
            _doc.Rect.SetRect(_header);
            for (int i = 1; i <= _count; i++)
            {
                _doc.PageNumber = i;
                OnCreateHeader(_header);
            }
            _doc.Rect.SetRect(_footer);
            for (int i = 1; i <= _count; i++)
            {
                _doc.PageNumber = i;
                _doc.VPos = 1;
                OnCreateFooter(_footer);
            }
            for (int i = 1; i <= _count; i++)
            {
                _doc.PageNumber = i;
                _doc.Flatten();
            }
            _doc.Save(stream);
            _doc.Clear();
        }
    }
}
