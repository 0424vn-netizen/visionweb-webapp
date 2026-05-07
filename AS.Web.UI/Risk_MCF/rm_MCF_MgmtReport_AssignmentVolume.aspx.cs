using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Global;
using AS.Controls.Pages;
using System;
using System.Data;
using System.Web;
using Telerik.Web.UI;

[PagePermission("RskManAssVol,MSRskManAssVol")]
public partial class rm_MCF_MgmtReport_AssignmentVolume : NonReportPage
{
    #region Enums
    enum DataBindAction
    {
        BindReportGrid,
        BindDetailGrid,
    }
    enum PostBackAction
    {
        ExportExcel,
        ExportCSV,
        DoFilterAction,
    }
    enum ViewMode
    {
        Date = 0,
        Detail = 1,
    }
    #endregion
    #region Properties

    private FilterParameterCollection _parameters = null;
    private string _OrderBy = string.Empty;

    #endregion
    private void BuildBaseParameters()
    {
        _parameters = new FilterParameterCollection();
        _parameters.AddMgmtLoggedInUser();

        _parameters.Add(new FilterParameter("@DateFilterMode", (int)RiskSessionManager.RiskMgmtReportFilter.DateType, DbType.Int32));
        DateTime beginDate = RiskSessionManager.RiskMgmtReportFilter.FromDate;
        DateTime endDate = RiskSessionManager.RiskMgmtReportFilter.ToDate;
        GeneralFuncsLib.GetRealDateRange(RiskSessionManager.RiskMgmtReportFilter.DateType, ref beginDate, ref endDate);

        _parameters.Add(new FilterParameter("@BeginDate", beginDate, DbType.DateTime));
        _parameters.Add(new FilterParameter("@EndDate", endDate, DbType.DateTime));
    }

    private void BuildExportParameters()
    {
        BuildBaseParameters();
        _parameters.Add(new FilterParameter("@ViewMode", (int)ViewMode.Date, DbType.Int32));
        _parameters.Add(new FilterParameter("@AssignmentList", uxReportFilter.AssignmentFilterValue, DbType.AnsiString));
    }
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        uxReportFilter.IsMonthlyDefault = true;

    }
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            OnPostBackActions(PostBackAction.DoFilterAction);
        }
    }
    protected override void OnDataBindControls(Enum type, object sender)
    {
        string spName = "spa_RM_MCF_Mgmt_GetAssignmentVolumeSummary";
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindReportGrid:
                {
                    HierarchyGrid grid = (HierarchyGrid)sender;
                    BuildBaseParameters();
                    _OrderBy = HierarchyGrid.SortExpression(grid.MasterTableView.SortExpressions);
                    _parameters.Add(new FilterParameter("@ViewMode", (int)ViewMode.Date, DbType.Int32));
                    _parameters.Add(new FilterParameter("@AssignmentList", uxReportFilter.AssignmentFilterValue, DbType.AnsiString));

                    _parameters.AddPagingParameters(true, false, grid.MasterTableView.CurrentPageIndex + 1, grid.MasterTableView.PageSize, string.Empty, string.Empty);
                    grid.DataSource = WebServices.RiskServices.GetReports(spName, _parameters);


                    _parameters.ClearPagingParameters();
                    _parameters.AddPagingParameters(false, true, grid.MasterTableView.CurrentPageIndex + 1, grid.MasterTableView.PageSize, string.Empty, string.Empty);
                    DataTable totalRows = WebServices.RiskServices.GetReports(spName, _parameters);
                    grid.VirtualItemCount = totalRows == null || totalRows.Rows.Count == 0 ? 0 : int.Parse(totalRows.Rows[0][0].ToString());

                    uxExporter.Visible = grid.VirtualItemCount > 0;
                    litGridHeader.Text = GetLocalResourceObject("rm_MgmtReport_AssignmentVolume_aspx_cs_GrigHeader").ToString();
                    litGridSubTitle.Text = VeraCodeSolution.ValidateResponseData(uxReportFilter.HeaderText);
                }
                break;
            case DataBindAction.BindDetailGrid:
                {
                    GridTableView detailView = sender as GridTableView;


                    _OrderBy = HierarchyGrid.SortExpression(detailView.SortExpressions);
                    _parameters.Add(new FilterParameter("@ViewMode", (int)ViewMode.Detail, DbType.Int32));
                    _parameters.Add(new FilterParameter("@AssignmentList", uxReportFilter.AssignmentFilterValue, DbType.AnsiString));

                    _parameters.AddPagingParameters(true, false, detailView.CurrentPageIndex + 1, detailView.PageSize, string.Empty, string.Empty);
                    detailView.DataSource = WebServices.RiskServices.GetReports(spName, _parameters);

                    _parameters.ClearPagingParameters();
                    _parameters.AddPagingParameters(false, true, detailView.CurrentPageIndex + 1, detailView.PageSize, string.Empty, string.Empty);
                    DataTable totalRows = WebServices.RiskServices.GetReports(spName, _parameters);
                    detailView.VirtualItemCount = totalRows == null || totalRows.Rows.Count == 0 ? 0 : int.Parse(totalRows.Rows[0][0].ToString());
                }
                break;
            default:
                break;
        }
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        var fileName = litGridHeader.Text + " - " + litGridSubTitle.Text;
        switch ((PostBackAction)type)
        {
            case PostBackAction.ExportExcel:
                this.IsNoCache = true;
                BuildExportParameters();
                RiskExportAs.DownloadExportedFile(fileName, fileName, WebSiteEnums.MgmtReportType.AssignmentVolumeSummary.ToString(),
                    AS.Controls.UserControls.UxExport.ExportType.Excel.ToString().ToLower(), _parameters, true);
                break;
            case PostBackAction.ExportCSV:
                this.IsNoCache = true;
                BuildExportParameters();
                RiskExportAs.DownloadExportedFile(fileName, fileName, WebSiteEnums.MgmtReportType.AssignmentVolumeSummary.ToString(),
                    AS.Controls.UserControls.UxExport.ExportType.CSV.ToString().ToLower(), _parameters, true);
                break;
            case PostBackAction.DoFilterAction:
                uxReportGrid.CurrentPageIndex = 0;
                uxReportGrid.Rebind();
                break;
        }
    }

    #region Export
    protected void ButtonExcel_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.ExportExcel);
    }
    protected void ButtonCSV_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.ExportCSV);
    }
    #endregion

    protected void DoGridNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        if (!e.IsFromDetailTable)
        {
            if (sender == uxReportGrid)
            {
                OnDataBindControls(DataBindAction.BindReportGrid, sender);
            }
        }
    }

    protected void DoDetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
    {
        GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;
        switch (e.DetailTableView.Name)
        {
            case "DetailView":
                {
                    BuildBaseParameters();
                    DateTime reportDate = (DateTime)dataItem.GetDataKeyValue("ReportDate");
                    _parameters.Add(new FilterParameter("@ReportDate", reportDate, DbType.DateTime));
                    OnDataBindControls(DataBindAction.BindDetailGrid, e.DetailTableView);
                    break;
                }
        }
    }

    protected void DoItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView dataRow = e.Item.DataItem as DataRowView;
            switch (e.Item.OwnerTableView.Name)
            {
                case "DateView":
                    {
                        dataItem["ReportDate"].Text = VeraCodeSolution.DoVeraCode(GetLocalResourceObject("rm_MgmtReport_AssignmentVolume_aspx_cs_Item1").ToString() + " <b>" + GeneralFuncsLib.FormatDate(dataRow["ReportDate"])) + "</b>";
                        dataItem["TotalAssigned"].Text = VeraCodeSolution.DoVeraCode(GetLocalResourceObject("rm_MgmtReport_AssignmentVolume_aspx_cs_Item2").ToString() + " <b>" + GeneralFuncsLib.FormatInteger(dataRow["TotalAssigned"])) + "</b>";
                        dataItem["PercentWorked"].Text = VeraCodeSolution.DoVeraCode(GetLocalResourceObject("rm_MgmtReport_AssignmentVolume_aspx_cs_Item3").ToString() + " <b>" + GeneralFuncsLib.FormatPercent(dataRow["PercentWorked"])) + "</b>";
                        dataItem["PercentNotWorked"].Text = VeraCodeSolution.DoVeraCode(GetLocalResourceObject("rm_MgmtReport_AssignmentVolume_aspx_cs_Item4").ToString() + " <b>" + GeneralFuncsLib.FormatPercent(dataRow["PercentNotWorked"])) + "</b>";
                    }
                    break;
            }
        }
    }

    protected void OnSearchEvent(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.DoFilterAction);
    }
}
