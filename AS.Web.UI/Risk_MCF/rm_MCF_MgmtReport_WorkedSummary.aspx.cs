using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Global;
using AS.Controls.Pages;
using System;
using System.Data;
using System.Web;
using Telerik.Web.UI;

[PagePermission("RskManWrkSum,MSRskManWrkSum")]
public partial class rm_MCF_MgmtReport_WorkedSummary : NonReportPage
{
    #region Enums
    enum DataBindAction
    {
        BindAgentGrid,
        BindGroupGrid,
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
        Group = 0,
        User = 1,
        Detail = 2,
    }
    #endregion
    #region Consts

    string TOTAL = string.Empty;

    #endregion

    #region Properties

    private string _MerchantNumber;
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
        if (uxReportFilter.IsAgentSearch)
        {
            _parameters.Add(new FilterParameter("@ViewMode", (int)ViewMode.User, DbType.Int32));
            _parameters.Add(new FilterParameter("@AgentList", uxReportFilter.AgentFilterValue, DbType.AnsiString));
        }
        else
        {
            _parameters.Add(new FilterParameter("@ViewMode", (int)ViewMode.Group, DbType.Int32));
            _parameters.Add(new FilterParameter("@GroupList", uxReportFilter.GroupFilterValue, DbType.AnsiString));
        }
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        if (IsPostBack)
        {
            Session["FirstInitWS"] = null;
            Session["FirstInitWD"] = null;
        }
        if (!IsPostBack && Session["FirstInitWS"] == null)
        {
            RiskSessionManager.RiskMgmtReportFilter.IsAgentSearch = true;
            RiskSessionManager.RiskMgmtReportFilter.IsGroupSearch = false;
            Session["FirstInitWS"] = 1;
            Session["FirstInitWD"] = null;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        TOTAL = GetLocalResourceObject("rm_MgmtReport_WorkedSummary_aspx_cs_Total").ToString() +" ";
        if (!IsPostBack)
        {
            DoSwitchView();
            if (uxReportAgent.Visible) uxGridTitle.Attributes.Add("data-target", "#" + uxReportAgent.ClientID);
            if (uxReportGroup.Visible) uxGridTitle.Attributes.Add("data-target", "#" + uxReportGroup.ClientID);
        }
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        
    }
    protected void DoSwitchView()
    {
        if (uxReportFilter.IsAgentSearch)
        {
            uxReportAgent.Visible = true;
            uxReportGroup.Visible = false;
        }
        else if (uxReportFilter.IsGroupSearch)
        {
            uxReportAgent.Visible = false;
            uxReportGroup.Visible = true;
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        string spName = "spa_RM_MCF_Mgmt_GetWorkedSummary";
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindAgentGrid:
                {
                    HierarchyGrid grid = (HierarchyGrid)sender;
                    BuildBaseParameters();
                    _OrderBy = HierarchyGrid.SortExpression(grid.MasterTableView.SortExpressions);
                    _parameters.Add(new FilterParameter("@ViewMode", (int)ViewMode.User, DbType.Int32));
                    _parameters.Add(new FilterParameter("@AgentList", uxReportFilter.AgentFilterValue, DbType.AnsiString));

                    _parameters.AddPagingParameters(true, false, grid.MasterTableView.CurrentPageIndex + 1, grid.MasterTableView.PageSize, _OrderBy, string.Empty);
                    grid.DataSource = WebServices.RiskServices.GetReports(spName, _parameters);

                    _parameters.ClearPagingParameters();
                    _parameters.AddPagingParameters(false, true, grid.MasterTableView.CurrentPageIndex + 1, grid.MasterTableView.PageSize, _OrderBy, string.Empty);
                    DataTable totalRows = WebServices.RiskServices.GetReports(spName, _parameters);
                    grid.VirtualItemCount = totalRows == null || totalRows.Rows.Count == 0 ? 0 : int.Parse(totalRows.Rows[0][0].ToString());

                    divExport.Visible = grid.VirtualItemCount > 0;
                    RiskSessionManager.RiskMgmtReportFilter.IsAgentSearch = true;
                    RiskSessionManager.RiskMgmtReportFilter.IsGroupSearch = false;
                    litGridTitle.Text = VeraCodeSolution.ValidateResponseData(GetLocalResourceObject("rm_MgmtReport_WorkedSummary_aspx_cs_WorkedSummary").ToString());
                    litGridSubTitle.Text = VeraCodeSolution.ValidateResponseData(uxReportFilter.HeaderText);
                }
                break;
            case DataBindAction.BindGroupGrid:
                {
                    HierarchyGrid grid = (HierarchyGrid)sender;
                    BuildBaseParameters();
                    _OrderBy = HierarchyGrid.SortExpression(grid.MasterTableView.SortExpressions);
                    _parameters.Add(new FilterParameter("@ViewMode", (int)ViewMode.Group, DbType.Int32));
                    _parameters.Add(new FilterParameter("@GroupList", uxReportFilter.GroupFilterValue, DbType.AnsiString));

                    _parameters.AddPagingParameters(true, false, grid.MasterTableView.CurrentPageIndex + 1, grid.MasterTableView.PageSize, _OrderBy, string.Empty);
                    grid.DataSource = WebServices.RiskServices.GetReports(spName, _parameters);

                    _parameters.ClearPagingParameters();
                    _parameters.AddPagingParameters(false, true, grid.MasterTableView.CurrentPageIndex + 1, grid.MasterTableView.PageSize, _OrderBy, string.Empty);
                    DataTable totalRows = WebServices.RiskServices.GetReports(spName, _parameters);
                    grid.VirtualItemCount = totalRows == null || totalRows.Rows.Count == 0 ? 0 : int.Parse(totalRows.Rows[0][0].ToString());

                    divExport.Visible = grid.VirtualItemCount > 0;
                    RiskSessionManager.RiskMgmtReportFilter.IsAgentSearch = false;
                    RiskSessionManager.RiskMgmtReportFilter.IsGroupSearch = true;
                    litGridTitle.Text = VeraCodeSolution.ValidateResponseData(GetLocalResourceObject("rm_MgmtReport_WorkedSummary_aspx_cs_WorkedSummary").ToString());
                    litGridSubTitle.Text = VeraCodeSolution.ValidateResponseData(uxReportFilter.HeaderText);
                }
                break;
            case DataBindAction.BindDetailGrid:
                {
                    GridTableView detailView = sender as GridTableView;
                    _OrderBy = HierarchyGrid.SortExpression(detailView.SortExpressions);
                    _parameters.AddPagingParameters(true, false, detailView.CurrentPageIndex + 1, detailView.PageSize, _OrderBy, string.Empty);
                    detailView.DataSource = WebServices.RiskServices.GetReports(spName, _parameters);

                    _parameters.ClearPagingParameters();
                    _parameters.AddPagingParameters(false, true, detailView.CurrentPageIndex + 1, detailView.PageSize, _OrderBy, string.Empty);
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
        switch ((PostBackAction)type)
        {
            case PostBackAction.ExportExcel:
                this.IsNoCache = true;
                BuildExportParameters();
                RiskExportAs.DownloadExportedFile(string.Format(GetLocalResourceObject("rm_MgmtReport_WorkedSummary_aspx_cs_WorkedSummary1").ToString(), uxReportFilter.HeaderText), 
                    string.Format(GetLocalResourceObject("rm_MgmtReport_WorkedSummary_aspx_cs_WorkedSummary1").ToString(), uxReportFilter.HeaderText),
                    WebSiteEnums.MgmtReportType.WorkedSummary.ToString(), 
                    AS.Controls.UserControls.UxExport.ExportType.Excel.ToString().ToLower(), _parameters, true);
                break;
            case PostBackAction.ExportCSV:
                this.IsNoCache = true;
                BuildExportParameters();
                RiskExportAs.DownloadExportedFile(  string.Format(GetLocalResourceObject("rm_MgmtReport_WorkedSummary_aspx_cs_WorkedSummary1").ToString(), uxReportFilter.HeaderText),
                    string.Format(GetLocalResourceObject("rm_MgmtReport_WorkedSummary_aspx_cs_WorkedSummary1").ToString(), uxReportFilter.HeaderText),
                    WebSiteEnums.MgmtReportType.WorkedSummary.ToString(), AS.Controls.UserControls.UxExport.ExportType.CSV.ToString().ToLower(), _parameters, true);
                break;
            case PostBackAction.DoFilterAction:
                if (uxReportFilter.IsAgentSearch)
                {
                    uxReportAgent.CurrentPageIndex = 0;
                    uxReportAgent.Rebind();
                }
                else
                {
                    uxReportGroup.CurrentPageIndex = 0;
                    uxReportGroup.Rebind();

                }
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
            if (sender == uxReportAgent)
            {
                OnDataBindControls(DataBindAction.BindAgentGrid, sender);
            }
            else if (sender == uxReportGroup)
            {
                OnDataBindControls(DataBindAction.BindGroupGrid, sender);
            }
        }
    }

    protected void DoDetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
    {
        GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;
        switch (e.DetailTableView.Name)
        {
            case "Agent":
                {
                    int groupId = Convert.ToInt32(dataItem.GetDataKeyValue("GroupID"));
                    BuildBaseParameters();
                    _parameters.Add(new FilterParameter("@ViewMode", (int)ViewMode.User, DbType.Int32));
                    _parameters.Add(new FilterParameter("@GroupID", groupId, DbType.Int32));

                    OnDataBindControls(DataBindAction.BindDetailGrid, e.DetailTableView);
                    break;
                }

            case "WorkedDetail":
                {
                    string userId = dataItem.GetDataKeyValue("UserID").ToString();
                    BuildBaseParameters();
                    _parameters.Add(new FilterParameter("@ViewMode", (int)ViewMode.Detail, DbType.Int32));
                    _parameters.Add(new FilterParameter("@SelectedUserID", userId, DbType.AnsiString));
                    OnDataBindControls(DataBindAction.BindDetailGrid, e.DetailTableView);
                    break;
                }
        }
    }

    protected void OnSearchEvent(object sender, EventArgs e)
    {
        DoSwitchView();
        OnPostBackActions(PostBackAction.DoFilterAction);
        if (uxReportAgent.Visible) uxGridTitle.Attributes.Add("data-target", "#" + uxReportAgent.ClientID);
        if (uxReportGroup.Visible) uxGridTitle.Attributes.Add("data-target", "#" + uxReportGroup.ClientID);
    }
}
