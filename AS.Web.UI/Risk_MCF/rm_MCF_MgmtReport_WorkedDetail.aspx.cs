using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Global;
using AS.Controls.Pages;
using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

[PagePermission("RskManWrk,MSRskManWrk")]
public partial class rm_MCF_MgmtReport_WorkedDetail : NonReportPage
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
        MerchantNumberClick,
        MerchantNameClick,
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

    private const string SESSION_HIERARCHYGRID = "Session_MgmtReport_WorkedDetail_PageIndex";
    private const string SESSION_EXPANDEDSTATES_AGENT = "Session_MgmtReport_WorkedDetail_uxReportAgent_ExpandedStates";
    private const string SESSION_EXPANDEDSTATES_GROUP = "Session_MgmtReport_WorkedDetail_uxReportGroup_ExpandedStates";
    private const string SESSION_GRIDSTATES_AGENT = "Session_MgmtReport_WorkedDetail_uxReportAgent_GridStates";
    private const string SESSION_GRIDSTATES_GROUP = "Session_MgmtReport_WorkedDetail_uxReportGroup_GridStates";
    string TOTAL = string.Empty;

    #endregion

    #region Fields

    private string _merchantNumber;
    private string _orderBy = string.Empty;
    private FilterParameterCollection _parameters = null;
    private string _merchantProfileIntruderQuery = string.Empty;

    #endregion Fields

    #region Properties

    private string MerchantProfileIntruderQuery
    {
        get
        {
            if (_merchantProfileIntruderQuery.Length == 0)
                _merchantProfileIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(
                    this.ID, new string[] { "MerchantNumber" });
            return _merchantProfileIntruderQuery;
        }
    }

    private Hashtable AgentExpandedStates
    {
        get
        {
            if (Session[SESSION_EXPANDEDSTATES_AGENT] == null)
            {
                Session[SESSION_EXPANDEDSTATES_AGENT] = new Hashtable();
            }
            return Session[SESSION_EXPANDEDSTATES_AGENT] as Hashtable;
        }
        set
        {
            Session[SESSION_EXPANDEDSTATES_AGENT] = value;
        }
    }

    private Hashtable AgentGridStates
    {
        get
        {
            if (Session[SESSION_GRIDSTATES_AGENT] == null)
            {
                Session[SESSION_GRIDSTATES_AGENT] = new Hashtable();
            }
            return Session[SESSION_GRIDSTATES_AGENT] as Hashtable;
        }
        set
        {
            Session[SESSION_GRIDSTATES_AGENT] = value;
        }
    }

    private Hashtable GroupExpandedStates
    {
        get
        {
            if (Session[SESSION_EXPANDEDSTATES_GROUP] == null)
            {
                Session[SESSION_EXPANDEDSTATES_GROUP] = new Hashtable();
            }
            return Session[SESSION_EXPANDEDSTATES_GROUP] as Hashtable;
        }
        set
        {
            Session[SESSION_EXPANDEDSTATES_GROUP] = value;
        }
    }

    private Hashtable GroupGridStates
    {
        get
        {
            if (Session[SESSION_GRIDSTATES_GROUP] == null)
            {
                Session[SESSION_GRIDSTATES_GROUP] = new Hashtable();
            }
            return Session[SESSION_GRIDSTATES_GROUP] as Hashtable;
        }
        set
        {
            Session[SESSION_GRIDSTATES_GROUP] = value;
        }
    }

    #endregion Properties

    #region Methods

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
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        TOTAL = GetLocalResourceObject("rm_MgmtReport_WorkedDetail_aspx_cs_TotalWorked").ToString() + " ";
        Session["FirstInitWS"] = null;
        if (!IsPostBack && Request["fromwd"] == null)
        {
            Session["FirstInitWD"] = null;
        }
        if (!IsPostBack && Session["FirstInitWD"] == null)
        {
            RiskSessionManager.RiskMgmtReportFilter.IsAgentSearch = true;
            RiskSessionManager.RiskMgmtReportFilter.IsGroupSearch = false;
            Session["FirstInitWD"] = 1;
            Session["FirstInitWS"] = null;
        }

        DoSwitchView();
        if (!IsPostBack)
        {
            if (!RiskSessionManager.RiskMgmtReportFilter.KeepSession)
                ResetStates();

            if (uxReportAgent.Visible)
                uxGridTitle.Attributes.Add("data-target", "#" + uxReportAgent.ClientID);
            if (uxReportGroup.Visible)
                uxGridTitle.Attributes.Add("data-target", "#" + uxReportGroup.ClientID);
        }
    }

    private void ResetStates()
    {
        GroupExpandedStates = AgentExpandedStates = null;
        AgentGridStates = GroupGridStates = null;
    }

    private void ClearGroupExpandedChildren(string parentHierarchicalIndex)
    {
        string[] indexes = new string[this.GroupExpandedStates.Keys.Count];
        this.GroupExpandedStates.Keys.CopyTo(indexes, 0);
        foreach (string index in indexes)
        {
            //all indexes of child items
            if (index.StartsWith(parentHierarchicalIndex + "_") ||
                index.StartsWith(parentHierarchicalIndex + ":"))
            {
                this.GroupExpandedStates.Remove(index);
            }
        }
        this.GroupExpandedStates.Remove(parentHierarchicalIndex);
    }

    private void ClearAgentExpandedChildren(string parentHierarchicalIndex)
    {
        string[] indexes = new string[this.AgentExpandedStates.Keys.Count];
        this.AgentExpandedStates.Keys.CopyTo(indexes, 0);
        foreach (string index in indexes)
        {
            //all indexes of child items
            if (index.StartsWith(parentHierarchicalIndex + "_") ||
                index.StartsWith(parentHierarchicalIndex + ":"))
            {
                this.AgentExpandedStates.Remove(index);
            }
        }
        this.AgentExpandedStates.Remove(parentHierarchicalIndex);
    }

    private void ClearAgentGridStatesChildren(string parentHierarchicalIndex)
    {
        string[] indexes = new string[this.AgentGridStates.Keys.Count];
        this.AgentGridStates.Keys.CopyTo(indexes, 0);
        foreach (string index in indexes)
        {
            //all indexes of child items
            if (index.StartsWith(parentHierarchicalIndex + "_") ||
                index.StartsWith(parentHierarchicalIndex + ":"))
            {
                this.AgentGridStates.Remove(index);
            }
        }
        this.AgentGridStates.Remove(parentHierarchicalIndex);
    }

    private void ClearGroupGridStatesChildren(string parentHierarchicalIndex)
    {
        string[] indexes = new string[this.GroupGridStates.Keys.Count];
        this.GroupGridStates.Keys.CopyTo(indexes, 0);
        foreach (string index in indexes)
        {
            //all indexes of child items
            if (index.StartsWith(parentHierarchicalIndex + "_") ||
                index.StartsWith(parentHierarchicalIndex + ":"))
            {
                this.GroupGridStates.Remove(index);
            }
        }
        this.GroupGridStates.Remove(parentHierarchicalIndex);
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
        string spName = "spa_RM_MCF_Mgmt_GetWorkedDetail";
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindAgentGrid:
                {
                    HierarchyGrid grid = (HierarchyGrid)sender;
                    BuildBaseParameters();
                    _orderBy = HierarchyGrid.SortExpression(grid.MasterTableView.SortExpressions);
                    _parameters.Add(new FilterParameter("@ViewMode", (int)ViewMode.User, DbType.Int32));
                    _parameters.Add(new FilterParameter("@AgentList", uxReportFilter.AgentFilterValue, DbType.AnsiString));

                    _parameters.AddPagingParameters(true, false, grid.MasterTableView.CurrentPageIndex + 1, grid.MasterTableView.PageSize, _orderBy, string.Empty);
                    grid.DataSource = WebServices.RiskServices.GetReports(spName, _parameters);

                    _parameters.ClearPagingParameters();
                    _parameters.AddPagingParameters(false, true, grid.MasterTableView.CurrentPageIndex + 1, grid.MasterTableView.PageSize, _orderBy, string.Empty);
                    DataTable totalRows = WebServices.RiskServices.GetReports(spName, _parameters);
                    grid.VirtualItemCount = totalRows == null || totalRows.Rows.Count == 0 ? 0 : int.Parse(totalRows.Rows[0][0].ToString());

                    divExport.Visible = grid.VirtualItemCount > 0;
                    RiskSessionManager.RiskMgmtReportFilter.IsAgentSearch = true;
                    RiskSessionManager.RiskMgmtReportFilter.IsGroupSearch = false;
                    litGridTitle.Text = VeraCodeSolution.ValidateResponseData(GetLocalResourceObject("rm_MgmtReport_WorkedDetail_aspx_cs_WorkedDetail1").ToString());
                    litGridSubTitle.Text = VeraCodeSolution.ValidateResponseData(uxReportFilter.HeaderText);
                }
                break;
            case DataBindAction.BindGroupGrid:
                {
                    HierarchyGrid grid = (HierarchyGrid)sender;
                    BuildBaseParameters();
                    _orderBy = HierarchyGrid.SortExpression(grid.MasterTableView.SortExpressions);
                    _parameters.Add(new FilterParameter("@ViewMode", (int)ViewMode.Group, DbType.Int32));
                    _parameters.Add(new FilterParameter("@GroupList", uxReportFilter.GroupFilterValue, DbType.AnsiString));

                    _parameters.AddPagingParameters(true, false, grid.MasterTableView.CurrentPageIndex + 1, grid.MasterTableView.PageSize, _orderBy, string.Empty);
                    grid.DataSource = WebServices.RiskServices.GetReports(spName, _parameters);


                    _parameters.ClearPagingParameters();
                    _parameters.AddPagingParameters(false, true, grid.MasterTableView.CurrentPageIndex + 1, grid.MasterTableView.PageSize, _orderBy, string.Empty);
                    DataTable totalRows = WebServices.RiskServices.GetReports(spName, _parameters);
                    grid.VirtualItemCount = totalRows == null || totalRows.Rows.Count == 0 ? 0 : int.Parse(totalRows.Rows[0][0].ToString());

                    divExport.Visible = grid.VirtualItemCount > 0;
                    RiskSessionManager.RiskMgmtReportFilter.IsAgentSearch = false;
                    RiskSessionManager.RiskMgmtReportFilter.IsGroupSearch = true;
                    litGridTitle.Text = VeraCodeSolution.ValidateResponseData(GetLocalResourceObject("rm_MgmtReport_WorkedDetail_aspx_cs_WorkedDetail1").ToString());
                    litGridSubTitle.Text = VeraCodeSolution.ValidateResponseData(uxReportFilter.HeaderText);
                }
                break;
            case DataBindAction.BindDetailGrid:
                {
                    GridTableView detailView = sender as GridTableView;
                    _orderBy = HierarchyGrid.SortExpression(detailView.SortExpressions);
                    _parameters.AddPagingParameters(true, false, detailView.CurrentPageIndex + 1, detailView.PageSize, _orderBy, string.Empty);
                    detailView.DataSource = WebServices.RiskServices.GetReports(spName, _parameters);

                    _parameters.ClearPagingParameters();
                    _parameters.AddPagingParameters(false, true, detailView.CurrentPageIndex + 1, detailView.PageSize, _orderBy, string.Empty);
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
                RiskExportAs.DownloadExportedFile( string.Format(GetLocalResourceObject("rm_MgmtReport_WorkedDetail_aspx_cs_WorkedDetail").ToString(), uxReportFilter.HeaderText),
                    string.Format(GetLocalResourceObject("rm_MgmtReport_WorkedDetail_aspx_cs_WorkedDetail").ToString(), uxReportFilter.HeaderText),
                    WebSiteEnums.MgmtReportType.WorkedDetail.ToString(), AS.Controls.UserControls.UxExport.ExportType.Excel.ToString().ToLower(), _parameters, true);
                break;
            case PostBackAction.ExportCSV:
                this.IsNoCache = true;
                BuildExportParameters();
                RiskExportAs.DownloadExportedFile(string.Format(GetLocalResourceObject("rm_MgmtReport_WorkedDetail_aspx_cs_WorkedDetail").ToString(), uxReportFilter.HeaderText),
                    string.Format(GetLocalResourceObject("rm_MgmtReport_WorkedDetail_aspx_cs_WorkedDetail").ToString(), uxReportFilter.HeaderText), 
                    WebSiteEnums.MgmtReportType.WorkedDetail.ToString(), AS.Controls.UserControls.UxExport.ExportType.CSV.ToString().ToLower(), _parameters, true);
                break;
            case PostBackAction.MerchantNameClick:
                string url = "rm_MCF_RiskReport.aspx?" + this.BuildSecureQueryString(
                    string.Format("merchantnumber={0}&IsPopup={1}{2}",
                                   _merchantNumber,
                                   true,
                                   MerchantProfileIntruderQuery));
                AjaxAddResponseScript("openPopupWindow('" + url + "','RiskReport');");

                break;
            case PostBackAction.MerchantNumberClick:
                RiskSessionManager.RiskReportReferrer = "WorkedDetail";
                RiskSessionManager.RiskReportReferrerInfo = new ReferrerInfo(_merchantNumber, ResolveUrl("~/risk_MCF/") + "rm_MCF_MgmtReport_WorkedDetail.aspx?fromwd=1", "Worked Detail");
                url = this.BuildSecureQueryString(string.Format("merchantnumber={0}&fromwd=1", _merchantNumber));
                url = ResolveUrl("~/") + "MerchantProfile.aspx?" + url;
                Response.Redirect(url, true);
                break;
            case PostBackAction.DoFilterAction:
                ResetStates();
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

    protected void uxReportAgent_OnPageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        string pageindex = e.NewPageIndex.ToString();

    }

    protected void DoGridNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        if (!e.IsFromDetailTable)
        {
            if (sender == uxReportAgent)
            {
                if (!RiskSessionManager.RiskMgmtReportFilter.KeepSession)
                {
                    Session[SESSION_HIERARCHYGRID] = uxReportAgent.MasterTableView.CurrentPageIndex.ToString();
                    AgentExpandedStates = AgentGridStates = null;
                }
                else
                {
                    int tempInt = 0;
                    Int32.TryParse(GeneralFuncsLib.NvlString(Session[SESSION_HIERARCHYGRID]), out tempInt);
                    //uxReportAgent.MasterTableView.CurrentPageIndex = tempInt;
                }
                OnDataBindControls(DataBindAction.BindAgentGrid, sender);
            }
            else if (sender == uxReportGroup)
            {
                if (!RiskSessionManager.RiskMgmtReportFilter.KeepSession)
                {
                    Session[SESSION_HIERARCHYGRID] = uxReportGroup.MasterTableView.CurrentPageIndex.ToString();
                    GroupExpandedStates = GroupGridStates = null;
                }
                else
                {
                    int tempInt = 0;
                    Int32.TryParse(GeneralFuncsLib.NvlString(Session[SESSION_HIERARCHYGRID]), out tempInt);
                    //uxReportGroup.MasterTableView.CurrentPageIndex = tempInt;
                }
                OnDataBindControls(DataBindAction.BindGroupGrid, sender);
            }
        }
    }

    protected void DoDetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
    {
        GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;
        if (RiskSessionManager.RiskMgmtReportFilter.KeepSession)
        {
            int tempInt = 0;
            if (sender == uxReportAgent)
            {
                Int32.TryParse(GeneralFuncsLib.NvlString(this.AgentGridStates[e.DetailTableView.Name + "_" + dataItem.ItemIndexHierarchical]), out tempInt);
            }
            else
            {
                Int32.TryParse(GeneralFuncsLib.NvlString(this.GroupGridStates[e.DetailTableView.Name + "_" + dataItem.ItemIndexHierarchical]), out tempInt);
            }
            //e.DetailTableView.CurrentPageIndex = tempInt;
        }
        else
        {
            if (sender == uxReportAgent)
            {
                ClearAgentGridStatesChildren(e.DetailTableView.Name + "_" + dataItem.ItemIndexHierarchical);
                AgentGridStates[e.DetailTableView.Name + "_" + dataItem.ItemIndexHierarchical] = e.DetailTableView.CurrentPageIndex.ToString();
            }
            else
            {
                ClearGroupGridStatesChildren(e.DetailTableView.Name + "_" + dataItem.ItemIndexHierarchical);
                GroupGridStates[e.DetailTableView.Name + "_" + dataItem.ItemIndexHierarchical] = e.DetailTableView.CurrentPageIndex.ToString();
            }
        }
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

    protected void DoItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView dataRow = e.Item.DataItem as DataRowView;
            switch (e.Item.OwnerTableView.Name)
            {
                case "Group":
                    {
                        string totalWorked = dataRow["TotalWorked"] == null || dataRow["TotalWorked"] == DBNull.Value ? string.Empty : TOTAL + dataRow["TotalWorked"].ToString();
                        dataItem["TotalWorked"].Text = VeraCodeSolution.ValidateResponseData(totalWorked);
                    }
                    break;
                case "Agent":
                    {
                        string totalWorked = dataRow["TotalWorked"] == null || dataRow["TotalWorked"] == DBNull.Value ? string.Empty : TOTAL + dataRow["TotalWorked"].ToString();
                        dataItem["TotalWorked"].Text = VeraCodeSolution.ValidateResponseData(totalWorked);
                    }
                    break;
            }
        }
    }

    protected void DoItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == RadGrid.ExpandCollapseCommandName)
        {
            if (!e.Item.Expanded)
            {
                if (sender == uxReportAgent)
                {
                    AgentExpandedStates[e.Item.ItemIndexHierarchical] = true;
                }
                else
                {
                    GroupExpandedStates[e.Item.ItemIndexHierarchical] = true;
                }
            }
            else
            {
                if (sender == uxReportAgent)
                {
                    ClearAgentExpandedChildren(e.Item.ItemIndexHierarchical);
                    ClearAgentGridStatesChildren(e.Item.ItemIndexHierarchical);
                }
                else
                {
                    ClearGroupExpandedChildren(e.Item.ItemIndexHierarchical);
                    ClearGroupGridStatesChildren(e.Item.ItemIndexHierarchical);
                }
            }
        }
    }

    protected void DoDataBound(object sender, EventArgs e)
    {
        if (sender == uxReportAgent)
        {
            string[] indexes = new string[this.AgentExpandedStates.Keys.Count];
            this.AgentExpandedStates.Keys.CopyTo(indexes, 0);

            ArrayList arr = new ArrayList(indexes);
            arr.Sort();

            HierarchyGrid grid = sender as HierarchyGrid;

            foreach (string key in arr)
            {
                bool value = (bool)this.AgentExpandedStates[key];
                if (value)
                {
                    grid.Items[key].Expanded = true;
                }
            }
        }
        else
        {
            string[] indexes = new string[this.GroupExpandedStates.Keys.Count];
            this.GroupExpandedStates.Keys.CopyTo(indexes, 0);

            ArrayList arr = new ArrayList(indexes);
            arr.Sort();

            HierarchyGrid grid = sender as HierarchyGrid;

            foreach (string key in arr)
            {
                bool value = (bool)this.GroupExpandedStates[key];
                if (value)
                {
                    grid.Items[key].Expanded = true;
                }
            }
        }
    }

    protected void uxMerchant_Command(object sender, CommandEventArgs e)
    {
        RiskSessionManager.RiskMgmtReportFilter.KeepSession = true;
        _merchantNumber = e.CommandArgument != null ? e.CommandArgument.ToString() : string.Empty;
        if (e.CommandName == "MerchantNameClick")
        {
            OnPostBackActions(PostBackAction.MerchantNameClick);
        }
        else if (e.CommandName == "MerchantNumberClick")
        {
            OnPostBackActions(PostBackAction.MerchantNumberClick);
        }
    }

    protected void OnSearchEvent(object sender, EventArgs e)
    {
        DoSwitchView();
        OnPostBackActions(PostBackAction.DoFilterAction);
        if (uxReportAgent.Visible)
            uxGridTitle.Attributes.Add("data-target", "#" + uxReportAgent.ClientID);
        if (uxReportGroup.Visible)
            uxGridTitle.Attributes.Add("data-target", "#" + uxReportGroup.ClientID);
    }

    protected override void OnPreRender(EventArgs e)
    {
        RiskSessionManager.RiskMgmtReportFilter.KeepSession = false;
        base.OnPreRender(e);
    }
    
    #endregion Methods
}
