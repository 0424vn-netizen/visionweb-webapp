using AS.Common.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using Telerik.Web.UI;
using AS.VW.Entities;
using AS.Controls.UserControls;
using System.Text.RegularExpressions;

namespace AS.Controls.Global
{
    public class ASGridBoundColumn : AS.Controls.Grid.ASGridBoundColumn
    {
        public override GridColumn Clone()
        {
            ASGridBoundColumn requiredGridBoundColumn = new ASGridBoundColumn();
            requiredGridBoundColumn.ASFormat = this.ASFormat;
            requiredGridBoundColumn.ASDefaultNullValue = this.ASDefaultNullValue;
            //you should override CopyBaseProperties if you have some column specific properties 
            requiredGridBoundColumn.CopyBaseProperties(this);
            return requiredGridBoundColumn;
        }
    }

    public class ASGridTemplateColumn : AS.Controls.Grid.ASGridTemplateColumn
    {
        public override GridColumn Clone()
        {
            ASGridTemplateColumn requiredGridTemplateColumn = new ASGridTemplateColumn();
            requiredGridTemplateColumn.CopyBaseProperties(this);
            return requiredGridTemplateColumn;
        }
    }

    public class ASGridClientSelectColumn : AS.Controls.Grid.ASGridClientSelectColumn { }

    public class ASGrid : AS.Controls.Grid.ASGrid
    {
        private const string DEFAULT_OUTER_DIV_CSS = "col-md-12";
        private const string FREEZE_TABLE_CSS = "freeze-table";
        private const string PRINT_SUPPORT_CSS = "print-supported-table";
        private const int MAX_NUMBER_OF_ROWS_NO_SCROLLING = 20;

        [Category("Behavior"), DefaultValue("col-md-12"), Description("The css for the second outer div of current grid"), NotifyParentProperty(true),]
        public string SecondOuterDivCss { get; set; }

        [Category("Behavior"), DefaultValue("false"), Description("The css for the grid use CSS overflow"), NotifyParentProperty(true),]
        public bool CSSOverflowable { get; set; }

        [Category("Behavior"), DefaultValue("false"), Description("Handle overflow for x-coordiator"), NotifyParentProperty(true),]
        public bool XOverFlowable { get; set; }

        [Category("Behavior"), DefaultValue("true"), Description("Handle Add temp column when setting scroll bar"), NotifyParentProperty(true),]
        public bool InsertTempColumnAtTheEnd { get; set; }

        [Category("Behavior"), DefaultValue("false"), Description("Append Header for printer"), NotifyParentProperty(true),]
        public bool AppendHeaderforPrinter { get; set; }


        public ASGrid()
        {
            this.CustomPagingTemplateSource = "~/UserControls/ASCustomPager.ascx";
            this.SecondOuterDivCss = DEFAULT_OUTER_DIV_CSS;
            this.ShowPageTotal = false;
            this.XOverFlowable = true;
            this.CSSOverflowable = false;
            //this.MasterTableView.TableLayout = GridTableLayout.Fixed;
            this.CssClass = "in";
            this.SortingSettings.EnableSkinSortStyles = false;
            this.InsertTempColumnAtTheEnd = true;
            this.AppendHeaderforPrinter = false;
            //46652 - AW Multi-currency Transaction Display
            this.CurrencyFormat = SessionManager.CurrencyFortmat;
            this.CurrencySymbol = SessionManager.CurrencySymbol;
            this.IsCacheTemplateFile = false;
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.InsertTempColumnAtTheEnd && this.XOverFlowable)
            {
                //Add temp column when setting scroll bar
                ASGridBoundColumn columnTemp = new ASGridBoundColumn();
                columnTemp.HeaderStyle.Width = 1;
                columnTemp.ItemStyle.Width = 0;
                columnTemp.HeaderStyle.CssClass = "no-padding";
                columnTemp.ItemStyle.CssClass = "no-padding";
                columnTemp.UniqueName = WebSiteConstants.GRID_COLUMN_TEMP;
                columnTemp.AllowFiltering = false;   //To display none the filtering at end column
                columnTemp.Display = false;  //To display none the end column

                this.MasterTableView.Columns.Add(columnTemp);
            }

            base.OnInit(e);
        }

        protected override bool OnNeedDataSource(GridNeedDataSourceEventArgs e)
        {
            //Format column
            foreach (GridColumn col in this.Columns)
            {
                if (col.HeaderText.ToLower() == "merchant id" || col.HeaderText.ToLower() == "merchant id")
                {
                    col.HeaderStyle.CssClass = "merchant-number";
                }
            }
            
            // Refresh filter width
            if (this.Page is ReportPage)
            {
                if (this.AllowFilteringByColumn)
                    ((ReportPage)this.Page).AjaxAddResponseScript("setFilterWidth();");
            }
            else if (this.Page is NonReportPage)
            {
                if (this.AllowFilteringByColumn)
                    ((NonReportPage)this.Page).AjaxAddResponseScript("setFilterWidth();");
            }

            this.AS_FilterExpression = EscapeLikeValue(this.AS_FilterExpression);

            bool ret = base.OnNeedDataSource(e);

            if (this.DataSource == null ||
                !(this.DataSource is System.Data.DataTable) ||
                (this.DataSource as System.Data.DataTable).Rows.Count == 0)
                this.AllowSorting = false;
            else
                this.AllowSorting = true;
            return ret;
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (this.XOverFlowable)
            {

                this.ClientSettings.Scrolling.UseStaticHeaders = true;
                this.ClientSettings.Scrolling.AllowScroll = true;

                this.ClientSettings.ClientEvents.OnTableCreated =
                  string.Format("addScrollForGidData('{0}', {1})", this.ClientID, MAX_NUMBER_OF_ROWS_NO_SCROLLING);

                if (AppendHeaderforPrinter)
                {
                    this.ClientSettings.ClientEvents.OnGridCreated = "appendHeaderforPrinter";
                    this.CssClass += " " + PRINT_SUPPORT_CSS;
                }
                if (string.IsNullOrEmpty(this.CssClass))
                {
                    this.CssClass = FREEZE_TABLE_CSS;
                }
                if (!this.CssClass.Contains(FREEZE_TABLE_CSS))
                    this.CssClass += " " + FREEZE_TABLE_CSS;

            }

            if (AllowFilteringByColumn || MasterTableView.AllowFilteringByColumn)
            {
                var ajax = AS.Controls.Global.RadAjaxManager.GetCurrent(Page);
                ajax.ResponseScripts.Add("addValidateForFilterBox();");
            }

            base.OnPreRender(e);
            //Multi-language for Menu Filter
            GridFilterMenu menu = this.FilterMenu;
            foreach (RadMenuItem item in menu.Items)
            {
                var result = string.Empty;
                switch (item.Value)
                {
                    case RadGridFilterMenuItemValues.NoFilter:
                        result = Resources.LanguageResource.AS_ASGrid_FilterMenu_NoFilter;
                        break;
                    case RadGridFilterMenuItemValues.Contains:
                        result = Resources.LanguageResource.AS_ASGrid_FilterMenu_Contains;
                        break;
                    case RadGridFilterMenuItemValues.DoesNotContain:
                        result = Resources.LanguageResource.AS_ASGrid_FilterMenu_DoesNotContain;
                        break;
                    case RadGridFilterMenuItemValues.StartsWith:
                        result = Resources.LanguageResource.AS_ASGrid_FilterMenu_StartsWith;
                        break;
                    case RadGridFilterMenuItemValues.EndsWith:
                        result = Resources.LanguageResource.AS_ASGrid_FilterMenu_EndsWith;
                        break;
                    case RadGridFilterMenuItemValues.EqualTo:
                        result = Resources.LanguageResource.AS_ASGrid_FilterMenu_EqualTo;
                        break;
                    case RadGridFilterMenuItemValues.NotEqualTo:
                        result = Resources.LanguageResource.AS_ASGrid_FilterMenu_NotEqualTo;
                        break;
                    case RadGridFilterMenuItemValues.GreaterThan:
                        result = Resources.LanguageResource.AS_ASGrid_FilterMenu_GreaterThan;
                        break;
                    case RadGridFilterMenuItemValues.LessThan:
                        result = Resources.LanguageResource.AS_ASGrid_FilterMenu_LessThan;
                        break;
                    case RadGridFilterMenuItemValues.GreaterThanOrEqualTo:
                        result = Resources.LanguageResource.AS_ASGrid_FilterMenu_GreaterThanOrEqualTo;
                        break;
                    case RadGridFilterMenuItemValues.LessThanOrEqualTo:
                        result = Resources.LanguageResource.AS_ASGrid_FilterMenu_LessThanOrEqualTo;
                        break;
                    case RadGridFilterMenuItemValues.IsNull:
                        result = Resources.LanguageResource.AS_ASGrid_FilterMenu_IsNull;
                        break;
                    case RadGridFilterMenuItemValues.NotIsNull:
                        result = Resources.LanguageResource.AS_ASGrid_FilterMenu_NotIsNull;
                        break;
                }
                item.Text = result;
            }
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            writer.Write("<div class=\"row\">");
            writer.Write(String.Format("<div class=\"{0}\">", this.SecondOuterDivCss));
            if (CSSOverflowable)
            {
                writer.Write("<div class=\"table-responsive\">");
                base.Render(writer);
                writer.Write("</div>");
            }
            else
            {
                base.Render(writer);
            }
            writer.Write("</div></div>");


        }

        protected override void ProcessFooterItem(GridFooterItem footerItem)
        {
            AllowSorting = Items.Count > 0;
            base.ProcessFooterItem(footerItem);
            GridColumn col = this.Columns.Cast<GridColumn>().First(c => c.Visible);
            string firstUniqueName = col.UniqueName;

            // If grid have group column, it can be visible but have no footer cell
            try
            {
                if (footerItem[firstUniqueName].Text == "Report Total") footerItem[firstUniqueName].Text = String.Empty;
            }
            catch { };
        }

        protected override void OnSortCommand(GridSortCommandEventArgs e)
        {
            GridTableView tableView = e.Item.OwnerTableView;
            e.Canceled = true;
            GridSortExpression expression = new GridSortExpression();
            expression.FieldName = e.SortExpression;

            if (tableView.SortExpressions.Count == 0 || tableView.SortExpressions[0].FieldName != expression.FieldName)
            {
                AS_SortExpression = "";
                tableView.SortExpressions.Clear();
                expression.SortOrder = GridSortOrder.Descending;
                AS_SortExpression = string.Format("[{0}]", expression.FieldName) + " DESC";
            }
            else if (tableView.SortExpressions[0].SortOrder == GridSortOrder.Descending)
            {
                expression.SortOrder = GridSortOrder.Ascending;
                AS_SortExpression = string.Format("[{0}]", expression.FieldName) + " ASC";
            }
            else
            {
                expression.SortOrder = GridSortOrder.None;
            }

            if (expression.SortOrder == GridSortOrder.None)
            {
                AS_SortExpression = "";
                tableView.SortExpressions.Clear();
            }
            else
            {
                tableView.SortExpressions.AddSortExpression(expression);
            }

            if (this.EnableSortItemsPersistence)
            {
                this.SaveSortItem(expression);
            }

            base.OnSortCommand(e);
            tableView.Rebind();
        }
        protected override void CalculateTotalForNonPaging()
        {
            base.CalculateTotalForNonPaging();

            if (AS_Total.Count > 0 && AS_DataSource != null && AS_DataSource.Rows.Count > 0)
            {
                var uniqueCols = this.AS_TotalColumns.Replace(" ", "").Split(',').ToList();
                var headers = AS_DataSource.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();
                var totalKeys = new List<string>() { "Sum_", "Total_" };

                foreach (var item in uniqueCols)
                {
                    foreach (var key in totalKeys)
                    {
                        CalculateTotalItem(item, key + item, headers);
                    }
                }
            }
        }

        private void CalculateTotalItem(string key, string totalKey, List<string> headers)
        {
            if (headers.Any(x => x.Equals(totalKey, StringComparison.OrdinalIgnoreCase))
                        && AS_Total.ContainsKey(key)
                        && (AS_DataSource.Columns[totalKey].DataType == TypeCollection.IntType ||
                            AS_DataSource.Columns[totalKey].DataType == TypeCollection.FloatType ||
                            AS_DataSource.Columns[totalKey].DataType == TypeCollection.DoubleType ||
                            AS_DataSource.Columns[totalKey].DataType == TypeCollection.DecimalType ||
                            AS_DataSource.Columns[totalKey].DataType == TypeCollection.LongType))
            {
                AS_Total[key] = Convert.ToDecimal(AS_DataSource.Rows[0][totalKey].ToString());
            }
        }

        private string EscapeLikeValue(string expression)
        {
            var result = Regex.Replace(expression, @"[\*]", m => string.Format("[{0}]", m.Value));
            return result;
        }
    }
}