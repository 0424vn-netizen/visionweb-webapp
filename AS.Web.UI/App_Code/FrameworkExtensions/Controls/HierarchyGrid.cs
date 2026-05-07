using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using AS.Controls.Grid;
using System.Drawing;
using System.Configuration;
using Telerik.Web.UI;

namespace AS.Controls.Global
{
    public class HierarchyGrid : RadGrid
    {
        public HierarchyGrid()
        {
            this.MasterTableView.NoMasterRecordsText = Resources.LanguageResource.AS_HierarchyGrid_NoRecord;
            this.MasterTableView.NoDetailRecordsText = Resources.LanguageResource.AS_HierarchyGrid_NoRecord;
        }

        protected override void OnItemEvent(GridItemEventArgs e)
        {
            if (e.EventInfo is GridInitializePagerItem)
            {
                GridInitializePagerItem info = e.EventInfo as GridInitializePagerItem;
                e.Canceled = true;
                var pager = e.Item as GridPagerItem;
               
                var customPager = (CustomPager)Page.LoadControl("~/UserControls/ASCustomPager.ascx");
                customPager.SetGrid(e.Item.OwnerTableView, info.PagingManager.DataSourceCount);
                pager.PagerContentCell.Controls.Add(customPager);
              
            }
            base.OnItemEvent(e);
        }

        protected override void OnItemCommand(GridCommandEventArgs e)
        {
            if (e.CommandName == RadGrid.ExpandCollapseCommandName)
            {
                if (e.Item.Expanded)
                {
                    e.Item.OwnerTableView.PagerStyle.AlwaysVisible = e.Item.OwnerTableView.Items.Count > 0;
                }
            }
            base.OnItemCommand(e);
        }

        protected override bool OnNeedDataSource(GridNeedDataSourceEventArgs e)
        {
            bool ret = base.OnNeedDataSource(e);
            this.MasterTableView.PagerStyle.AlwaysVisible = (this.VirtualItemCount > 0);
            return ret;
        }

        protected override void OnPreRender(EventArgs e)
        {
            VisiblePager(this.MasterTableView);
            base.OnPreRender(e);
        }
        protected override void OnDetailTableDataBind(GridDetailTableDataBindEventArgs e)
        {
            base.OnDetailTableDataBind(e);
            e.DetailTableView.PagerStyle.AlwaysVisible = (e.DetailTableView.VirtualItemCount > 0 || e.DetailTableView.Items.Count > 0);
            e.DetailTableView.ParentItem.OwnerTableView.PagerStyle.AlwaysVisible = e.DetailTableView.ParentItem.OwnerTableView.Items.Count > 0;
            e.DetailTableView.NoDetailRecordsText = Resources.LanguageResource.AS_HierarchyGrid_NoRecord;
        }

        private void VisiblePager(GridTableView tableView)
        {
            tableView.PagerStyle.AlwaysVisible = tableView.ExpandCollapseColumn.Display = tableView.VirtualItemCount > 0 || tableView.Items.Count > 0;
            if (tableView.Items.Count > 0)
            {
                foreach (GridDataItem item in tableView.Items)
                {
                    if (item.HasChildItems)
                    {
                        GridTableView childTableView = item.ChildItem.NestedTableViews[0];
                        childTableView.PagerStyle.AlwaysVisible = childTableView.ExpandCollapseColumn.Display = childTableView.Items.Count > 0 || childTableView.VirtualItemCount > 0;
                        VisiblePager(childTableView);
                    }
                }
            }
        }
        public static string SortExpression(GridSortExpressionCollection items)
        {
            if (items.Count > 0)
            {
                GridSortExpression ex = items[0];
                string col = ex.FieldName;
                string order = ex.SortOrder.ToString();
                order = order == "Descending" ? "DESC" :
                        order == "None" ? "" : "ASC";
                return order == "" ? "" : col + " " + order;
            }
            return string.Empty;
        }
    }

    public class HierarchyBoundColumn : GridBoundColumn
    {
        FormatType _ASDefaultFormat = FormatType.Auto;
        string _ASFormat = "";
        /// <summary>
        /// Gets or sets the format to display data on column
        /// </summary>
        /// <value>The AS format (FormatType).</value>
        public FormatType ASFormat
        {
            get
            {
                return _ASDefaultFormat;
            }
            set
            {
                _ASDefaultFormat = value;
            }
        }
        public HierarchyBoundColumn() : base()
        {
            this.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        }

        protected override string FormatDataValue(object dataValue, GridItem item)
        {
            base.FormatDataValue(dataValue, item);
            if (_ASFormat != "")
            {
                string ret = "";
                string[] replace_cases = _ASFormat.Split('|');
                string[] workingCase = null;
                ret = dataValue.ToString();
                for (int i = 0; i < replace_cases.Length; i++)
                {
                    workingCase = replace_cases[i].Split(',');
                    if (workingCase.Length == 1)
                    {
                        if (i == 0)
                        {
                            ret = DataBinder.Eval(item.DataItem, workingCase[0]).ToString() + ret;
                        }
                        else if (i == replace_cases.Length - 1)
                        {
                            ret = ret + DataBinder.Eval(item.DataItem, workingCase[0]).ToString();
                        }
                    }
                    else
                    {
                        workingCase[0] = workingCase[0].Replace("[0x2C]", ",").Replace("[0x22]", "\"");
                        workingCase[1] = workingCase[1].Replace("[0x2C]", ",").Replace("[0x22]", "\"");
                        ret = ret.Replace(workingCase[0], workingCase[1]);
                    }

                }
                ret = CustomFormatDataValue(ret, item);
                return ret;
            }
            else
            {
                return CustomFormatDataValue(dataValue, item);
            }
        }
        string _DefaultNullValue = "N/A";
        public string ASDefaultNullValue
        {
            get
            {
                if (ConfigurationManager.AppSettings["ASGrid_NullValueText"] != null) _DefaultNullValue = ConfigurationManager.AppSettings["ASGrid_NullValueText"];
                return _DefaultNullValue;
            }
            set
            {
                _DefaultNullValue = value;
            }
        }
        string CustomFormatDataValue(object dataValue, GridItem item)
        {
            if (dataValue is DBNull)
            {
                return ASDefaultNullValue;
            }
            try
            {
                decimal TempValue = 0M;
                bool autoAlign = this.ItemStyle.HorizontalAlign == HorizontalAlign.NotSet;
                switch (_ASDefaultFormat)
                {
                    case FormatType.Auto:
                        {
                            if (dataValue is Int16
                                || dataValue is Int32
                                || dataValue is Int64
                                || dataValue is uint
                                || dataValue is sbyte
                                || dataValue is byte
                                || dataValue is ushort
                                || dataValue is ulong

                                )
                            {
                                if (autoAlign)
                                    this.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                                if (item is GridDataItem)
                                    (item as GridDataItem)[this.UniqueName].HorizontalAlign = this.ItemStyle.HorizontalAlign;

                                return FormatConvertResolver.FormatInteger(dataValue);
                            }

                            if (dataValue is float
                                || dataValue is Single
                                || dataValue is Double

                                )
                            {
                                if (autoAlign)
                                    this.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                                if (item is GridDataItem)
                                    (item as GridDataItem)[this.UniqueName].HorizontalAlign = this.ItemStyle.HorizontalAlign;

                                return FormatConvertResolver.FormatNumber(dataValue);
                            }
                            if (dataValue is Decimal)
                            {
                                TempValue = decimal.Parse(dataValue.ToString());
                                if (autoAlign)

                                    this.ItemStyle.HorizontalAlign = HorizontalAlign.Right;

                                if (item is GridDataItem)
                                {
                                    (item as GridDataItem)[this.UniqueName].HorizontalAlign = this.ItemStyle.HorizontalAlign;

                                    if (TempValue < 0) (item as GridDataItem)[this.UniqueName].ForeColor = Color.Red;
                                }

                                return FormatConvertResolver.FormatCurrency(TempValue);
                            }
                            if (dataValue is DateTime)
                            {
                                if (autoAlign)
                                    this.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                                if (item is GridDataItem)
                                    (item as GridDataItem)[this.UniqueName].HorizontalAlign = this.ItemStyle.HorizontalAlign;

                                return FormatConvertResolver.FormatDateTime((DateTime)dataValue);
                            }
                            if (dataValue is String)
                            {
                                if (autoAlign)
                                    this.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                                if (item is GridDataItem)
                                    (item as GridDataItem)[this.UniqueName].HorizontalAlign = this.ItemStyle.HorizontalAlign;

                                return FormatConvertResolver.FormatString(dataValue.ToString());
                            }
                            break;
                        }
                    case FormatType.DateAndTime:
                        {
                            if (autoAlign)
                                this.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                            if (item is GridDataItem)
                                (item as GridDataItem)[this.UniqueName].HorizontalAlign = this.ItemStyle.HorizontalAlign;

                            return FormatConvertResolver.FormatDateTime((DateTime)dataValue);
                        }
                    case FormatType.DateAndTime12Hours:
                        {
                            if (autoAlign)
                                this.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                            if (item is GridDataItem)
                                (item as GridDataItem)[this.UniqueName].HorizontalAlign = this.ItemStyle.HorizontalAlign;


                            return FormatConvertResolver.FormatDateTime12Hours((DateTime)dataValue);
                        }
                    case FormatType.Date:
                        {
                            if (autoAlign)
                                this.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                            if (item is GridDataItem)
                                (item as GridDataItem)[this.UniqueName].HorizontalAlign = this.ItemStyle.HorizontalAlign;
                            return FormatConvertResolver.FormatDateOnly((DateTime)dataValue);
                        }
                    case FormatType.Currency:
                        {
                            TempValue = decimal.Parse(dataValue.ToString());
                            if (autoAlign)
                                this.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                            if (item is GridDataItem)
                                (item as GridDataItem)[this.UniqueName].HorizontalAlign = this.ItemStyle.HorizontalAlign;
                            if (TempValue < 0)
                            {
                                if (item is GridDataItem)
                                    (item as GridDataItem)[this.UniqueName].ForeColor = Color.Red;
                            }
                            return FormatConvertResolver.FormatCurrency(TempValue);
                        }
                    case FormatType.Currency4Digits:
                        {
                            TempValue = decimal.Parse(dataValue.ToString());
                            if (autoAlign)
                                this.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                            if (item is GridDataItem)
                                (item as GridDataItem)[this.UniqueName].HorizontalAlign = this.ItemStyle.HorizontalAlign;
                            if (TempValue < 0)
                            {
                                if (item is GridDataItem)
                                    (item as GridDataItem)[this.UniqueName].ForeColor = Color.Red;
                            }
                            return FormatConvertResolver.FormatCurrency4Digits(TempValue);
                        }
                    case FormatType.Percentage:
                        {
                            if (autoAlign)
                                this.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                            if (item is GridDataItem)
                                (item as GridDataItem)[this.UniqueName].HorizontalAlign = this.ItemStyle.HorizontalAlign;


                            return FormatConvertResolver.FormatPercent(decimal.Parse(dataValue.ToString()));
                        }
                    case FormatType.Number:
                        {
                            if (autoAlign)
                                this.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                            if (item is GridDataItem)
                                (item as GridDataItem)[this.UniqueName].HorizontalAlign = this.ItemStyle.HorizontalAlign;


                            return FormatConvertResolver.FormatNumber(dataValue);
                        }
                    case FormatType.Integer:
                        {
                            if (autoAlign)
                                this.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                            if (item is GridDataItem)
                                (item as GridDataItem)[this.UniqueName].HorizontalAlign = this.ItemStyle.HorizontalAlign;

                            return FormatConvertResolver.FormatInteger(dataValue);
                        }
                    case FormatType.DynamicString:
                        {
                            if (autoAlign)
                                this.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                            if (item is GridDataItem)
                                (item as GridDataItem)[this.UniqueName].HorizontalAlign = this.ItemStyle.HorizontalAlign;

                            return FormatConvertResolver.FormatString(dataValue.ToString());
                        }

                    case FormatType.StaticString:
                        {
                            if (autoAlign)
                                this.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                            if (item is GridDataItem)
                                (item as GridDataItem)[this.UniqueName].HorizontalAlign = this.ItemStyle.HorizontalAlign;

                            return FormatConvertResolver.FormatString(dataValue.ToString());
                        }
                    case FormatType.None:
                        {
                            return base.FormatDataValue(dataValue, item);
                        }
                }
                return "";
            }
            catch
            {
                return "AS System: Invalid Data Type";
            }

        }
        
        public override GridColumn Clone()
        {
            HierarchyBoundColumn hierarchyBoundColumn = new HierarchyBoundColumn();
            hierarchyBoundColumn.ASFormat = this.ASFormat;
            hierarchyBoundColumn.ASDefaultNullValue = this.ASDefaultNullValue;
            hierarchyBoundColumn.CopyBaseProperties(this);
            return hierarchyBoundColumn; 
        }
    }

    public class HierarchyTemplateColumn : GridTemplateColumn
    {
        public HierarchyTemplateColumn()
            : base()
        {
            this.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        }
        public override GridColumn Clone()
        {
            HierarchyTemplateColumn hierarchyTemplateColumn = new HierarchyTemplateColumn();
            hierarchyTemplateColumn.CopyBaseProperties(this);
            return hierarchyTemplateColumn;
        }
    }   
     public static class HierarchyGridExtension
    {
        public static string FirstSortOrder(this GridTableView tableView)
        {
            if (tableView.SortExpressions.Count > 0)
            {
                GridSortExpression ex = tableView.SortExpressions[0];
                string col = ex.FieldName;
                string order = ex.SortOrder.ToString();
                order = order == "Descending" ? "DESC" :
                        order == "None" ? "" : "ASC";
                return order == "" ? "" : col + " " + order;
            }
            return "";
        }
    }

}