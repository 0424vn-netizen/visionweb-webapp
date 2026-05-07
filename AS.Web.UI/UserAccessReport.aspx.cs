using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AS.Controls.Pages;
using AS.Controls.Grid;
using AS.Common.DBManager;
using System.Data;
using AS.Web.Business;
using Telerik.Web.UI;
using AS.Web.UI.Controls;
using AS.Common;
[PagePermission("UserAccessReport,MSUserAccessReport")]
public partial class UserAccessReport : ReportPage
{
    #region Enums
    enum DataBindAction
    {
        BindReportGrid,
    }
    enum PostBackAction
    {
        DoSearching,
        GoBackClick,
    }

    #endregion
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        RegisterEvent();
        InitializeGridColumn();
        ((ReportPage)this.Page).IsBindDataOnLoad = true;
    }

    protected void InitializeGridColumn()
    {

        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(false);
        parameters.AddLanguageID();
        DataTable info = WebServices.RiskServices.GetReports("spa_SEC_GetHierarchyInfoOnGridUserAccessReport", parameters);


        ////initialize cols

        foreach (DataRow row in info.Rows)
        {
            ASGridBoundColumn boundColumn = new ASGridBoundColumn();
            boundColumn.DataField = row["HierarchyGridDataField"].ToString();
            boundColumn.UniqueName = row["HierarchyGridDataField"].ToString();
            boundColumn.HeaderText = row["HierarchyGridHeaderText"].ToString();
            boundColumn.HeaderTooltip = row["HierarchyHeaderTooltip"].ToString();
            boundColumn.ASFormat = FormatType.StaticString;
            boundColumn.SortExpression = row["HierarchyGridDataField"].ToString();
            uxReportGrid.MasterTableView.Columns.Add(boundColumn);
        }

    }
    protected void RegisterEvent()
    {
        uxReportGrid.ItemDataBound += (s, e) =>
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = e.Item as GridDataItem;
                DataRowView dataRow = e.Item.DataItem as DataRowView;
                dataItem["LogInsCount"].Text = VeraCodeSolution.DoVeraCode(
                    string.Format("<a href=\"#\" onclick=\"ShowPopupModal('{0}', 'auto'); return false;\">{1}</a>", ResolveUrl("~/UserAccessDetail.aspx?" + BuildSecureQueryString(string.Format("UserID={0}", dataRow["RecId"]))), dataRow["LogInsCount"])
                    );

                dataItem["Status"].Text = dataRow["Status"].Equals("Y") ? GetLocalResourceObject("UserAccessReport_aspx_cs_Status").ToString() : dataRow["Status"].ToString();
            }
        };
        uxReportFiltering.Filtering += (s, e) =>
        {
            uxReportGrid.Rebind();
        };
    }

    protected void DoNeedExportConfig(object sender, AS.Controls.Exporter.ExportConfig exportConfig)
    {
        exportConfig.FileName = uxExporter.GridSubTitle.Replace(" ", "");
    }


    protected override void DoGridDataSourceReady(ASGrid sender, EventArgs e)
    {
        if ((sender.DataSource == null || ((DataTable)sender.DataSource).Rows.Count == 0) && sender.AS_FilterExpression == "")
        {
            sender.AllowFilteringByColumn = false;
        }
        else
        {
            sender.AllowFilteringByColumn = true;
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        RemoveFilterMenuItem();
    }
    protected override void DoGridNeedDataSource(ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.BindReportGrid, sender);
    }
    protected override void OnDataBindControls(Enum type, object sender)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindReportGrid:
                {
                    parameters.Add(new FilterParameter("@DateFilterMode", uxReportFiltering.UserAccessFilterOption.DateFilterMode, DbType.Int32));
                    parameters.Add(new FilterParameter("@FilterMode", uxReportFiltering.UserAccessFilterOption.FilterMode, DbType.Int32));
                    parameters.Add(new FilterParameter("@BeginDate", uxReportFiltering.UserAccessFilterOption.FromDate, DbType.Date));
                    parameters.Add(new FilterParameter("@EndDate", uxReportFiltering.UserAccessFilterOption.ToDate, DbType.Date));
                    parameters.Add(new FilterParameter("@FilterType", uxReportFiltering.UserAccessFilterOption.SearchType, DbType.String));
                    parameters.Add(new FilterParameter("@FilterVal", (uxReportFiltering.UserAccessFilterOption.SearchType.ToUpper().Trim() == "USERTYPE")? 
                        uxReportFiltering.UserAccessFilterOption.FilterValue : uxReportFiltering.UserAccessFilterOption.SearchValue, DbType.String));
                    parameters.Add(new FilterParameter("@HierarchyID", uxReportFiltering.UserAccessFilterOption.HierarchyID, DbType.Int32));
                    //44617: Export real excel
                    uxReportGrid.DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] 
                    { "spa_SEC_GetUserAccessLog", ReportServices.ConvertToFilterParamWSArray(parameters) });

                    uxExporter.GridSubTitle = GetLocalResourceObject("UserAccessReport_aspx_cs_ResultForAllUsers").ToString() + " " + GetFilterText();
                    uxExporter.GridHeader = GetLocalResourceObject("UserAccessReport_aspx_cs_ResultForAllUsers").ToString() + " " + GetFilterText();

                }
                break;
        }
    }

    public string GetFilterText()
    {
        string dateText = string.Empty;
        string beginDateText = string.Empty;
        string endDateText = string.Empty;
        switch ((DateOptionMode)uxReportFiltering.UserAccessFilterOption.DateFilterMode)
        {
            case DateOptionMode.Daily:
                {
                    beginDateText = uxReportFiltering.UserAccessFilterOption.FromDate.Value.ToGenericDateString();
                    dateText = string.Format("{0}", beginDateText);
                }
                break;
            case DateOptionMode.Monthly:
                {
                    beginDateText = uxReportFiltering.UserAccessFilterOption.FromDate.Value.GetFirstDayOfMonth().ToGenericDateString();
                    if (uxReportFiltering.UserAccessFilterOption.FromDate.Value.Year == DateTime.Today.Year && uxReportFiltering.UserAccessFilterOption.FromDate.Value.Month == DateTime.Today.Month)
                        endDateText = DateTime.Today.ToGenericDateString();
                    else
                        endDateText = uxReportFiltering.UserAccessFilterOption.FromDate.Value.GetLastDayOfMonth().ToGenericDateString();
                }
                dateText = string.Format("{0} - {1}", beginDateText, endDateText);
                break;
            case DateOptionMode.DateRange:
                {
                    beginDateText = uxReportFiltering.UserAccessFilterOption.FromDate.Value.ToGenericDateString();
                    endDateText = uxReportFiltering.UserAccessFilterOption.ToDate.Value.ToGenericDateString();
                }
                dateText = string.Format("{0} - {1}", beginDateText, endDateText);
                break;
        }

        return "(" + dateText + ")";
    }




    private void RemoveFilterMenuItem()
    {
        //show filter menu
        var grids = new RadGrid[] { uxReportGrid };
        var removedItems = new string[] {
           "GreaterThan",
            "LessThan", "GreaterThanOrEqualTo", "LessThanOrEqualTo", "Between", "NotBetween",
            "IsEmpty", "NotIsEmpty", "IsNull", "NotIsNull"
        };
        foreach (var grid in grids)
        {
            for (int i = 0; i < removedItems.Length; i++)
            {
                var mi = grid.FilterMenu.Items.FindItemByText(removedItems[i]);
                if (mi != null)
                    grid.FilterMenu.Items.Remove(mi);
            }
        }
    }


    protected void uxReportFiltering_Filtering(object sender, EventArgs e)
    {
        uxReportGrid.CurrentPageIndex = 0;
    }
}
