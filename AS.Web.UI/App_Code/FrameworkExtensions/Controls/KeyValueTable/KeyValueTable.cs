using AS.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for KeyValueTable
/// </summary>
/// 
namespace AS.Controls
{
        public class KeyValueTable : CompositeDataBoundControl, IExportKeyValueTable
        {
            private const string _EX_TAG_OPEN = "<Html><head><meta charset=3D'UTF-8'>"
                     +"<style type=3D'text/css'>"
                     + "td{border-style:solid;border-width:thin;}"
                     +"tr.borderBottom td{border-bottom-style:solid;border-bottom-width:thin;}"
                     +"tr.borderTop td{border-top-style:solid;border-top-width:thin;}"
                     +"</style>"
                     +"<meta http-equiv=3D'content-type' content=3D'application/xhtml+xml; charset=UTF-8' />"
                     +"</head>"
                     +"<Body><Table cellspacing='0' cellpadding='0' border='0'>";
            private const string _EX_TAG_CLOSE = "</Table></Body></Html>";

            private Table _Table;
            private List<KeyValueTableItem> _ItemCollection;
            private bool _IsBinded = false;
            private int _Count;

            public int IndexDataSource { get; set; }
            public Unit LeftWidth { get; set; }
            public string CssClassRow { get; set; }
            public string CssClassAltRow { get; set; }
            public string CssClassBorder { get; set; }
            public string CssClassHeading { get; set; }

            public event System.EventHandler ExportConfig;
            public event NeedDataSourceEvent NeedDataSource;
            public delegate void NeedDataSourceEvent(KeyValueTable sender);

            public event ItemDataBoundEvent ItemDataBound;
            public delegate void ItemDataBoundEvent(KeyValueTable sender, KeyValueTableItemEventArgs args);

            [Description("Rows of the KeyValueTable")]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(true)]
            [PersistenceMode(PersistenceMode.InnerProperty)]
            public FDCKeyValueTableRowCollection RowCollection { get; set; }

            public KeyValueTable()
            {
                IndexDataSource = 0;
                LeftWidth = Unit.Percentage(50.0);
                CssClassRow = "Row";
                CssClassAltRow = "AltRow";
                CssClassBorder = "ASTable";
                CssClassHeading = "heading";
            }

            public override void DataBind()
            {
                if (!_IsBinded)
                {
                    if (NeedDataSource != null)
                    {
                        NeedDataSource(this);
                    }
                    base.DataBind();

                    //Bind child control
                    if (RowCollection.Any(x => x is KeyValueTableTemplateRow))
                    {
                        DataBindChildren();
                    }
                    _IsBinded = true;
                }
            }

            protected override int CreateChildControls(IEnumerable dataSource, bool dataBinding)
            {
                _Count = 0;
                if (dataSource != null && dataSource is DataView)
                {
                    //Create Table
                    _Table = new Table()
                    {
                        CssClass = CssClassBorder,
                    };

                    _ItemCollection = new List<KeyValueTableItem>();
                    var dataSourceView = dataSource as DataView;
                    dataSourceView.AllowNew = dataSourceView.AllowEdit = dataSourceView.AllowDelete = false;
                    if (dataSourceView.Count > 0)
                    {
                        bool isFirstRow = true;
                        foreach (KeyValueTableRow row in RowCollection)
                        {
                            if (row.Visible)
                            {
                                _Count++;
                                KeyValueTableItem item = CreateItem(dataSourceView[IndexDataSource], row, dataBinding);
                                if (isFirstRow && LeftWidth != Unit.Percentage(0))
                                {
                                    item.LeftCell.Width = LeftWidth;
                                }
                                isFirstRow = false;
                                item.CssClass = _Count % 2 == 1 ? CssClassRow : CssClassAltRow;

                                _Table.Rows.Add(item);
                                _ItemCollection.Add(item);
                            }
                        }
                    }

                    Controls.Clear();
                    Controls.Add(_Table);
                }
                return _Count;
            }

            protected virtual KeyValueTableItem CreateItem(DataRowView dataRow, KeyValueTableRow row, bool useDataSource)
            {
                KeyValueTableItem item = new KeyValueTableItem();

                TableCell cellLeft = new TableCell()
                {
                    CssClass = CssClassHeading,
                    Text = row.CellTitle,
                    ToolTip = row.CellTitleTooltip
                };
                TableCell cellRight = new TableCell();
                item.UniqueName = row.UniqueName;
                if (useDataSource)
                {
                    item.DataItem = dataRow;

                    row.BuildCell(cellRight, dataRow);
                }
                item.Cells.Add(cellLeft);
                item.Cells.Add(cellRight);
                if (ItemDataBound != null && useDataSource)
                {
                    ItemDataBound(this, new KeyValueTableItemEventArgs(item));
                }
                return item;
            }

            protected override void Render(HtmlTextWriter writer)
            {
                writer.WriteBeginTag("div");
                writer.WriteAttribute("id", this.ClientID);
                writer.WriteAttribute("class", "in");
                writer.Write(HtmlTextWriter.TagRightChar);
                RenderContents(writer);
                writer.WriteEndTag("div");
            }

            public string ExportKeyValueTableContent(string reportTitle)
            {
                if (ExportConfig != null)
                {
                    ExportConfig(this, null);

                }
                string content = GetExportContent(reportTitle);
                if (!string.IsNullOrEmpty(content))
                {
                    HttpContext.Current.Response.BufferOutput = true;
                    HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                    HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=\"" + VeraCodeSolution.RemoveCRLF(GeneralFuncsLib.GetFileName(reportTitle)) + ".xls\"");
                    HttpContext.Current.Response.Write(content);
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.Response.End();
                }
                return string.Empty;
            }

            public string ExportForMulti(string reportTitle, string path)
            {
                return ExportMultiSections.WriteContentToFile(path, GeneralFuncsLib.GetFileName(reportTitle), "xls", GetExportContent(reportTitle));
            }

            public string GetExportContent(string reportTitle)
            {
                if (NeedDataSource != null)
                {
                    NeedDataSource(this);

                    var dataRow = (DataSource as DataTable).Rows[IndexDataSource];
                    StringBuilder stringContent = new StringBuilder();
                    stringContent.Append(_EX_TAG_OPEN);
                    stringContent.Append("<tr><th align='left' colspan=\"2\"><b>" + reportTitle + "</b></th></tr>");
                    for (var i = 0; i < RowCollection.Count; i++)
                    {
                        if (RowCollection[i].Visible)
                        {
                            stringContent.Append("<tr><td>" + RowCollection[i].CellTitle + "</td>");
                            stringContent.Append("<td>" + "&nbsp;" + FormatResolver.Format(RowCollection[i].FormatType,
                                RowCollection[i].DataField == null || RowCollection[i].DataField == string.Empty ? string.Empty : dataRow[RowCollection[i].DataField])
                                + "</td></tr>");
                        }
                    }
                    stringContent.Append(_EX_TAG_CLOSE);
                    return stringContent.ToString();
                }
                return string.Empty;
            }
        }
}