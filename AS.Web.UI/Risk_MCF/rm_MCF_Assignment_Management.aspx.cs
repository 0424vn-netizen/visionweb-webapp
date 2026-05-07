using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AS.Common.DBManager;
using AS.Common.Utilities;
using AS.Controls.Exporter;
using AS.Controls.Global;
using Telerik.Web.UI;
using AS.Common;
using System.Text.RegularExpressions;
using AS.Common.Formater;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using AS.Controls.UserControls;
using AS.Controls.Pages;
using AS.Threading;
using AS.Security.WS.Entities;
using System.Threading;
using System.Web.UI.HtmlControls;
using BusinessGeneralFuncsLib = AS.Web.Business.General.GeneralFuncsLib;
using AS.Web.Business.Shared.Constants;

[PagePermission("RskManAss,MSRskManAss")]
public partial class rm_MCF_Assignment_Management : ReportPage
{
    #region Enums
    enum DataBindAction
    {
        AssignmentGrid,
        UserGroupGrid,
        ComboUser,
        ComboGroup,
        ComboAssignment
    }
    enum PostBackAction
    {
        Submit,
        CreateAssignment,
        EditAssignment,
        DeleteAssignment,
        ChangedExpirationTime,
        ExportGridUserGroup,
        ChangeStartDateTime
    }

    enum FilteringType
    {
        All = 0,
        User = 1,
        Group = 2,
        Assignment = 3,
        SubsiteAssignment = 4
    }


    #endregion

    #region properties
    // columns in database
    private const string ASSIGNMENT_ID_DB = "AssignmentID";
    private const string ASSIGNMENT_NAME_DB = "AssignmentName";
    private const string ASSIGNMENT_TYPE_DB = "AssignmentType";
    private const string GROUP_ID_DB = "GroupID";
    private const string GROUP_NAME_DB = "GroupName";
    private const string USER_ID_DB = "UserID";
    private const string USER_NAME_DB = "UserName";
    private const string ALERT_MERCH_COUNT_DB = "AlertMerchantCount";
    private const string TOTAL_MERCH_COUNT_DB = "TotalMerchantCount";
    private const string TOTAL_MERCH_VOLUME_DB = "AlertMerchantVolume";
    private const string WIP_MERCH_COUNT_DB = "WIPCount";
    private const string WIP_MERCH_VOLUME_DB = "WIPVolume";
    private const string WORKED_MERCH_COUNT_DB = "WorkedCount";
    private const string WORKED_MERCH_VOLUME_DB = "WorkedVolume";
    private const string REQUEUED_COUNT_DB = "RequeueCount";
    private const string REQUEUED_VOLUME_DB = "RequeueVolume";
    private const string GROUP_COUNT_DB = "GroupCount";
    private const string USER_COUNT_DB = "UserCount";
    private const string COMPLETE_PER_DB = "CompletePercent";
    private const string EXPIRATION_DATE_DB = "ExpirationDate";
    private const string EXPIRATION_DATE_EXP_DB = "ExpirationDateExport";
    private const string IS_REASSIGN_DB = "IsReAssign";
    private const string PROCESS_STATUS_CODE_DB = "ProcessingStatusCode";
    private const string PROCESS_STATUS_DB = "ProcessingStatus";
    private const string PROCESS_STATUS_NOTE_DB = "ProcessingStatusNotes";
    private const string PROCESS_STATUS_DATE_DB = "ProcessingStatusDate";
    private const string WK_MERCH_COUNT_DB = "WKCount";
    private const string WK_MERCH_VOLUME_DB = "WKVolume";

    private const string GROUP_ID = "GroupID";
    private const string GROUP_NAME = "GroupName";
    private const string USER_ID = "UserID";
    private const string USER_NAME = "UserName";
    private const string MERCHANT_COUNT = "MerchantCount";
    private const string WORKED = "Worked";
    private const string ASSIGNMENT_ID = "AssignmentID";
    private const string ASSIGNMENT_NAME = "AssignmentName";
    private const string ALL = "ALL";
    private const string EXPORT_FILE_HEADER = "";
    private const string FEATURE_MODE = "FeatureMode";
    private const string REVIEW_MODE = "ReviewMode";
    private const string DUPLICATE = "Duplicate";

    private string _exportFileName = string.Empty;

    // column name in gridview
    private const string ASSIGNMENT_TYPE_COL = "AssignmentType";
    private const string REQUEUED_COUNT_COL = "RequeueCount";
    private const string REQUEUED_VOLUME_COL = "RequeueVolume";
    private const string DELETE_COL = "Delete";

    // new columns for exporting
    private const string ASSIGNMENT_TYPE_ABBR = "AssignmentTypeAbbr";
    private const string REQUEUED_COUNT_NEW = "RequeuedAsCountNew";
    private const string REQUEUED_VOLUME_NEW = "RequeuedAsVolumeNew";

    // header for exporting
    private string _requeuedCountHeaderExp = string.Empty;
    private string _requeuedVolumeHeaderExp = string.Empty;

    private string _sttMode = string.Empty;
    private static string i_valueNA = string.Empty;
    private string _neverExpire = string.Empty;

    private DataTable _assignmentsTable = null;
    private string _filteredUserID = string.Empty;
    private int _filteredGroupID = 0;
    private int _filteredAssignmentID = 0;
    private int _filteredMode = 0;
    private string _groupBy = "Assignment";
    private List<DataTable> _expTableList = new List<DataTable>();
    private string _currentSortExpr = string.Empty;
    private string _currentSortOrder = string.Empty;
    private bool _isFromGroupByCombo = false;
    private bool _iSort = false;
    protected DateTime _filterDate = DateTime.Now;
    private string _currencyFortmat = string.Empty;
    private User _currentUser = new User();
    private string _userMode = string.Empty;
    private int _clientId;
    private bool _isThreadDone;
    private bool _loadDataParallel;
    private Dictionary<string, string> _assignmentResources = new Dictionary<string, string>();

    public bool EnabledByReadOnly
    {
        get
        {
            return GeneralFuncsLib.ReadOnlyRiskManagementEnableStatus((SecurePage)Page);
        }
    }

    public bool HasQueuingMechanism
    {
        get
        {
            return GeneralFuncsLib.HasQueuingMechanismFeature;
        }
    }

    #endregion

    protected override void PageInitialize()
    {
        if (IsIntruderDetected) return;
        this.GridIDs.Add("uxAssignmentGrid");
        this.GridIDs.Add("uxUserGroupGrid");

        IsBindDataOnLoad = true;
        base.PageInitialize();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        _requeuedCountHeaderExp = GetLocalResourceObject("rm_Assignment_Management_cs_HeaderExp_RequeuedCount").ToString();
        _requeuedVolumeHeaderExp = GetLocalResourceObject("rm_Assignment_Management_cs_HeaderExp_RequeuedVolume").ToString();
        _neverExpire = GetLocalResourceObject("rm_Assignment_Management_cs_HeaderExp_NeverExpire").ToString();
        _exportFileName = GetLocalResourceObject("rm_Assignment_Management_cs_exportfileName").ToString();
        i_valueNA = GetLocalResourceObject("rm_Assignment_Management_cs_Na_Value").ToString();
        if (this.IsIntruderDetected) return;
        if (!IsPostBack)
        {
            InitializeFilterType();
            OnDataBindControls(DataBindAction.ComboUser);
            OnDataBindControls(DataBindAction.ComboGroup);
            OnDataBindControls(DataBindAction.ComboAssignment);
            SetFilterDefault();
            SaveCurrentFilter();
            RefreshFilterValues();
            LoadAssigmentSummaryAsync();
        }

        SwitchView();
        plhActionPanel.Visible = GeneralFuncsLib.ReadOnlyRiskManagementEnableStatus((SecurePage)this.Page);

        // 38605: Advanced Filter
        uxAdvancedFilter.FilterPage = FilterPageEnums.ManageAssignments;
        if (IsPostBack)
        {
            var eventtarget = this.Request.Form["__EVENTTARGET"];
            if (eventtarget.EndsWith(uxRealSubmit.ID))
            {
                hddApplyFilterId.Value = "";
                uxAdvancedFilter.IsBindData = true;
            }
        }
        // End - 38605
    }

    private bool _isExporting = false;
    protected override void DoNeedExportConfig(UxExport sender, ExportConfig exportConfig)
    {
        if (IsIntruderDetected) return;

        _isExporting = true;
        uxAssignmentGrid.Columns.FindByUniqueName("AssignmentID").Visible = false;
        uxAssignmentGrid.Columns.FindByUniqueName("Delete").Visible = false;
        uxAssignmentGrid.Columns.FindByUniqueName("Duplicate").Visible = false;
        GridColumn clWorkCount = uxAssignmentGrid.Columns.FindByUniqueName("WKCount");
        clWorkCount.Visible = true;
        clWorkCount.HeaderText = GetLocalResourceObject("rm_Assignment_Management_aspx_cs_WkCount").ToString();// "Ready to work Count";

        GridColumn clWorkVolume = uxAssignmentGrid.Columns.FindByUniqueName("WKVolume");
        clWorkVolume.Visible = true;
        clWorkVolume.HeaderText = GetLocalResourceObject("rm_Assignment_Management_aspx_cs_WkVolume").ToString();// "Ready to work Volume";

        GridColumn clWipCount = uxAssignmentGrid.Columns.FindByUniqueName("WIPCount");
        clWipCount.Visible = true;
        clWipCount.HeaderText = GetLocalResourceObject("rm_Assignment_Management_aspx_cs_WIPCount").ToString();// "WIP Count";

        GridColumn clWipVolume = uxAssignmentGrid.Columns.FindByUniqueName("WIPVolume");
        clWipVolume.Visible = true;
        clWipVolume.HeaderText = GetLocalResourceObject("rm_Assignment_Management_aspx_cs_WIPVolume").ToString();// "Worked Volume";

        GridColumn clWorkedCount = uxAssignmentGrid.Columns.FindByUniqueName("WorkedCount");
        clWorkedCount.Visible = true;
        clWorkedCount.HeaderText = GetLocalResourceObject("rm_Assignment_Management_aspx_cs_WorkedCount").ToString();// "Worked Count";

        GridColumn clWorkedVolume = uxAssignmentGrid.Columns.FindByUniqueName("WorkedVolume");
        clWorkedVolume.Visible = true;
        clWorkedVolume.HeaderText = GetLocalResourceObject("rm_Assignment_Management_aspx_cs_WorkedVolume").ToString();// "Worked Volume";

        GridColumn clTotalMerchantCount = uxAssignmentGrid.Columns.FindByUniqueName("TotalMerchantCount");
        clTotalMerchantCount.Visible = true;
        clTotalMerchantCount.HeaderText = GetLocalResourceObject("rm_Assignment_Management_aspx_cs_MerchantCountTotalMerch").ToString(); //"Merchant Count: Total Merch";

        GridColumn clAlertMerchantCount = uxAssignmentGrid.Columns.FindByUniqueName("AlertMerchantCount");
        clAlertMerchantCount.Visible = true;
        clAlertMerchantCount.HeaderText = GetLocalResourceObject("rm_Assignment_Management_aspx_cs_MerchantCountAlert").ToString();// "Merchant Count: Alert";

        GridColumn clVolumeMerchantCount = uxAssignmentGrid.Columns.FindByUniqueName("AlertMerchantVolume");
        clVolumeMerchantCount.Visible = true;
        clVolumeMerchantCount.HeaderText = GetLocalResourceObject("rm_Assignment_Management_aspx_cs_Volume").ToString();// "Merchant Volume: Alert";

        GridColumn clAssignmentType = uxAssignmentGrid.Columns.FindByUniqueName(ASSIGNMENT_TYPE_COL);
        clAssignmentType.Visible = HasQueuingMechanism;

        GridColumn clRequeuedAssCount = uxAssignmentGrid.Columns.FindByUniqueName(REQUEUED_COUNT_COL);
        clRequeuedAssCount.Visible = HasQueuingMechanism;
        clRequeuedAssCount.HeaderText = _requeuedCountHeaderExp;

        GridColumn clRequeuedAssAmount = uxAssignmentGrid.Columns.FindByUniqueName(REQUEUED_VOLUME_COL);
        clRequeuedAssAmount.Visible = HasQueuingMechanism;
        clRequeuedAssAmount.HeaderText = _requeuedVolumeHeaderExp;


        GridColumn StartDateTemplate = uxAssignmentGrid.Columns.FindByUniqueName("StartDateTemplate");
        StartDateTemplate.Visible = false;

        GridColumn startDate = uxAssignmentGrid.Columns.FindByUniqueName(ManageAssignmentConstanst.COLUMN_NAME_START_DATE);
        startDate.Visible = false;
        GridColumn startDateExport = uxAssignmentGrid.Columns.FindByUniqueName(ManageAssignmentConstanst.COLUMN_NAME_START_DATE_EXPORT);
        startDateExport.Visible = true;
        GridColumn futureStartDate = uxAssignmentGrid.Columns.FindByUniqueName(ManageAssignmentConstanst.COLUMN_NAME_FUTURE_START_DATE);
        futureStartDate.Visible = true;

        GridColumn ExpirationDateTemplate = uxAssignmentGrid.Columns.FindByUniqueName("ExpirationDateTemplate");
        ExpirationDateTemplate.Visible = false;

        GridColumn cluxExpirationDateNever = uxAssignmentGrid.Columns.FindByUniqueName("ExpirationDateExport");
        cluxExpirationDateNever.Visible = true;


        if (IsIntruderDetected) return;
        UxExport exporter = (UxExport)sender;
        exportConfig.AllowHtmlEncoded = true;
        exportConfig.PageDirection = PageDirection.Landscape;

        base.DoNeedExportConfig(sender, exportConfig);
        exportConfig.FileName = _exportFileName;
        exportConfig.ReportHeader = _exportFileName;
    }

    protected override void DoGridNeedDataSource(AS.Controls.Grid.ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        if (IsIntruderDetected) return;
        if (sender == uxAssignmentGrid && uxAssignmentGrid.Visible)
        {
            OnDataBindControls(DataBindAction.AssignmentGrid, sender);
        }
        else if (sender == uxUserGroupGrid && uxUserGroupGrid.Visible)
        {
            OnDataBindControls(DataBindAction.UserGroupGrid, sender);
        }
    }

    protected override void DoItemDataBound(AS.Controls.Grid.ASGrid sender, GridItemEventArgs e)
    {
        if (IsIntruderDetected) return;
        if (sender == uxUserGroupGrid)
        {
            if (e.Item is GridDataItem && e.Item.OwnerTableView.Name == "Detail")
            {
                var dataItem = e.Item as GridDataItem;
                DataRowView dataRow = e.Item.DataItem as DataRowView;
                if (!this.uxListOfGridId.Value.Contains(e.Item.OwnerTableView.ClientID))
                {
                    if (!string.IsNullOrEmpty(this.uxListOfGridId.Value))
                    {
                        this.uxListOfGridId.Value += ";";
                    }
                    this.uxListOfGridId.Value += VeraCodeSolution.DoVeraCode(e.Item.OwnerTableView.ClientID);
                }

                if (_iSort)
                {
                    string[] parts = GeneralFuncsLib.NvlString(hddFilter.Value).Split('|');
                    string selectedGroupBy = parts[2];
                    if (selectedGroupBy == "User")
                        ReplaceGroupSortList(dataItem["UserID"].Text);
                    else
                        ReplaceGroupSortList(dataItem["GroupID"].Text);
                }
                _iSort = false;
                BuildDataItems(sender, dataItem, e.Item.DataItem as DataRowView, dataRow[ASSIGNMENT_ID].ToString());

                if (dataItem["WorkedVolume"].Text.Contains('('))
                {
                    dataItem["WorkedVolume"].Text = VeraCodeSolution.DoVeraCode(string.Format("<font color='red'>{0}</font>", dataItem["WorkedVolume"].Text));
                }
            }
        }
        else if (sender == uxAssignmentGrid && e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView dataRow = dataItem.DataItem as DataRowView;
            string assignmentID = dataRow["AssignmentID"].ToString();
            BuildDataItems(sender, dataItem, dataRow, assignmentID);
            if (dataItem["WorkedVolume"].Text.Contains('('))
            {
                dataItem["WorkedVolume"].Text = VeraCodeSolution.DoVeraCode(string.Format("<font color='red'>{0}</font>", dataItem["WorkedVolume"].Text));
            }
            dataItem["ExpirationDateExport"].Text = BuildExpirationDate(dataRow[EXPIRATION_DATE_DB]);
            dataItem[ManageAssignmentConstanst.COLUMN_NAME_START_DATE_EXPORT].Text = GetStartDateByDataRowView(dataRow);
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        if (IsIntruderDetected) return;
        switch ((DataBindAction)type)
        {
            case DataBindAction.AssignmentGrid:
                SetVisibleForRequeuedAsColumns();
                uxAssignmentGrid_NeedDataSource();
                break;
            case DataBindAction.UserGroupGrid:
                SetVisibleForRequeuedUserGroupColumns();
                uxUserGroupGrid_NeedDataSource();
                break;
            case DataBindAction.ComboUser:
                if (!IsPostBack)
                    DataBindComboUser(true);
                else
                    DataBindComboUser(false);
                break;
            case DataBindAction.ComboGroup:
                if (!IsPostBack)
                    DataBindComboGroup(true);
                else
                    DataBindComboGroup(false);
                break;
            case DataBindAction.ComboAssignment:
                if (!IsPostBack)
                    DataBindComboAssignment(true);
                else
                    DataBindComboAssignment(false);
                break;
        }
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        if (this.IsIntruderDetected) return;
        int assignmentID = 0;
        FilterParameterCollection parameters = new FilterParameterCollection();
        FilterParameterCollection outparameters;
        switch ((PostBackAction)type)
        {
            case PostBackAction.CreateAssignment:
                assignmentID = Save();
                string queryString = BuildSecureQueryString(string.Format("{0}={1}{2}&IsCreateMode={3}", ASSIGNMENT_ID, assignmentID, AssignmentIntruderQuery, (int)WebSiteEnums.FeatureMode.Create));
                if (RiskSessionManager.IsUsingMarketData)
                    AjaxAddResponseScript("ShowPopupModal('rm_MCF_Assignment_CreateNew_MarketData.aspx?" + queryString + "','auto');");
                else
                    AjaxAddResponseScript("ShowPopupModal('rm_MCF_Assignment_CreateNew.aspx?" + queryString + "','auto');");
                break;
            case PostBackAction.Submit:
                SessionManager.AssignementFilteringOptions = string.Format("{0};{1};{2};{3};{4}", (int)this.FilterType, this.FilterValue,
                    uxGroupBy.SelectedValue, (int)this.ActiveType, uxReportDate.SelectedDate.Value);
                _isFromGroupByCombo = true;

                RefreshFilterValues();

                RefreshFilterMerchant();

                LoadAssigmentSummaryAsync();

                RebindGridData();
                break;
            case PostBackAction.DeleteAssignment:
                assignmentID = Convert.ToInt32(hddAssignmentID.Value);
                parameters.Clear();
                parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                parameters.Add(new FilterParameter("@AssignmentID", assignmentID, DbType.Int32));
                parameters.Add(new FilterParameter("@ReturnValue", 0, DbType.Int32));
                WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_DeleteAssignment", parameters, out outparameters);
                RebindGridData();

                break;
            case PostBackAction.ChangedExpirationTime:
                assignmentID = Convert.ToInt32(hddAssignmentID.Value);
                DateTime expirationDate = Convert.ToDateTime(hddExtendDate.Value);
                parameters.Clear();
                parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                parameters.Add(new FilterParameter("@AssignmentID", assignmentID, DbType.Int32));
                parameters.Add(new FilterParameter("@ExtendedDate", expirationDate, DbType.DateTime));
                parameters.Add(new FilterParameter("@ReturnValue", 0, DbType.Int32));
                WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_ExtendAssignment", parameters, out outparameters);
                RebindGridData();
                break;
            case PostBackAction.ChangeStartDateTime:
                assignmentID = Convert.ToInt32(hddAssignmentID.Value);
                DateTime startDate = Convert.ToDateTime(hddExtendDate.Value);
                parameters.Clear();
                parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                parameters.Add(new FilterParameter("@AssignmentID", assignmentID, DbType.Int32));
                parameters.Add(new FilterParameter("@StartDate", startDate, DbType.DateTime));
                parameters.Add(new FilterParameter("@ReturnValue", 0, DbType.Int32));
                WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_ExtendAssignment", parameters, out outparameters);
                RebindGridData();
                break;
            case PostBackAction.ExportGridUserGroup:
                DoExportingGridUserGroup(sender);
                break;
            case PostBackAction.EditAssignment:
                assignmentID = RM_MCF_GeneralFuncsLib.MoveDataFromFinalTables(int.Parse((sender as CommandEventArgs).CommandArgument.ToString()));
                int featureMode = !GeneralFuncsLib.ReadOnlyRiskManagementEnableStatus((SecurePage)Page) ? 1 : 0;
                string queryString1 = BuildSecureQueryString(string.Format("{0}={1}&{2}={3}{4}", ASSIGNMENT_ID, assignmentID, FEATURE_MODE, featureMode, AssignmentIntruderQuery));

                AjaxAddResponseScript("ShowPopupModal('rm_MCF_Assignment_CreateNew.aspx?" + queryString1 + "','auto');");

                break;
        }
    }

    private void LoadAssigmentSummaryAsync()
    {
        _loadDataParallel = true;
        // Prepare parameters before load data
        _currencyFortmat = SessionManager.CurrencyFortmat;
        _currentUser = SessionManager.CurrentUser;
        _userMode = GeneralFuncsLib.GetUserMode();
        _clientId = SessionManager.CurrentClient;

        _assignmentResources.Add("EligibleResource1Tooltip", GetLocalResourceObject("EligibleResource1Tooltip").ToString());
        _assignmentResources.Add("AlertedResource1ToolTip", GetLocalResourceObject("AlertedResource1ToolTip").ToString());
        _assignmentResources.Add("WorkedResource1ToolTip", GetLocalResourceObject("WorkedResource1ToolTip").ToString());
        _assignmentResources.Add("RemainingResource1ToolTip", GetLocalResourceObject("RemainingResource1ToolTip").ToString());
        _assignmentResources.Add("AssignmentHelpText", GetLocalResourceObject("AssignmentHelpText.Text").ToString());

        // Paralle load data to reduce excution time
        Thread thread = new Thread(() =>
        {
            LoadAssigmentSummary();
            _isThreadDone = true;
        });
        thread.Start();

    }

    private void DoCallback(CallbackArgs args)
    {
        _isThreadDone = true;
    }

    private void DataBindComboUser(bool isFirstLoad)
    {
        uxComboUser.DataValueField = "UserID";
        uxComboUser.DataTextField = "UserText";
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        uxComboUser.DataSource = WebServices.RiskServices.GetReports("spa_RM_MCF_GetRiskUsers", parameters);
        uxComboUser.DataBind();
        AS.Controls.Global.RadComboBoxItem EmptyItem = new AS.Controls.Global.RadComboBoxItem();
        EmptyItem.Height = Unit.Pixel(12);
        uxComboUser.Items.Insert(0, EmptyItem);

        if (isFirstLoad) uxComboUser.SelectedIndex = 0;
    }

    private void DataBindComboGroup(bool isFirstLoad)
    {
        uxComboGroup.DataTextField = GROUP_NAME;
        uxComboGroup.DataValueField = GROUP_ID;
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@IsActive", 1, DbType.Int32));
        parameters.Add(new FilterParameter("@SortOrder", "GroupName ASC", DbType.AnsiString));
        uxComboGroup.DataSource = WebServices.RiskServices.GetReports("spa_RM_MCF_GetAllGroups", parameters);
        uxComboGroup.DataBind();
        AS.Controls.Global.RadComboBoxItem EmptyItem = new AS.Controls.Global.RadComboBoxItem();
        EmptyItem.Height = Unit.Pixel(12);
        uxComboGroup.Items.Insert(0, EmptyItem);

        if (isFirstLoad)
            uxComboGroup.SelectedIndex = 0;
    }

    private void DataBindComboAssignment(bool isFirstLoad)
    {
        uxComboAssignment.Items.Clear();

        uxComboAssignment.DataTextField = ASSIGNMENT_NAME;
        uxComboAssignment.DataValueField = ASSIGNMENT_ID;
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);

        var assignmentType = uxFilterType.SelectedValue.ToInt();
        if (assignmentType == (int)FilteringType.SubsiteAssignment)
        {
            parameters.Add(new FilterParameter("@AssignmentType", (int)WebSiteEnums.AssignmentType.Subsite, DbType.Int32));
        }
        else
        {
            parameters.Add(new FilterParameter("@AssignmentType", (int)WebSiteEnums.AssignmentType.All, DbType.Int32));
        }

        parameters.Add(new FilterParameter("@IsReskinSite", true, DbType.Boolean));
        parameters.Add(new FilterParameter("@ReportDate", uxReportDate.SelectedDate, DbType.DateTime));
        uxComboAssignment.DataSource = WebServices.RiskServices.GetReports("spa_RM_MCF_GetAssignmentList", parameters);
        uxComboAssignment.DataBind();
        AS.Controls.Global.RadComboBoxItem EmptyItem = new AS.Controls.Global.RadComboBoxItem();
        EmptyItem.Height = Unit.Pixel(12);
        uxComboAssignment.Items.Insert(0, EmptyItem);

        if (isFirstLoad)
            uxComboAssignment.SelectedIndex = 0;
    }

    protected void uxBtnSubmit_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.Submit, sender);
    }

    protected void btnDeleteAssignment_Click(object sender, EventArgs e)
    {
        DataTable tb = CheckDeleteAutoQueue(hddAssignmentID.Value.ToLong());
        if (tb != null && tb.Rows.Count > 0)
            return;

        OnPostBackActions(PostBackAction.DeleteAssignment, sender);
        OnDataBindControls(DataBindAction.ComboAssignment);
    }

    protected void btnRebind_Click(object sender, EventArgs e)
    {
        if (this.uxAssignmentGrid.Visible == true)
            uxAssignmentGrid.Rebind();
        else if (this.uxUserGroupGrid.Visible == true)
            uxUserGroupGrid.Rebind();
        OnDataBindControls(DataBindAction.ComboAssignment);
        uxComboAssignment.SelectedValue = _filteredAssignmentID.ToString();
    }

    protected void uxLnkCreateAssignemnt_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.CreateAssignment, sender);
    }

    private void DoExportingGridUserGroup(object sender)
    {
        var id = (sender as AS.Controls.Global.LinkButton).ID;
        string[] parts = GeneralFuncsLib.NvlString(hddFilter.Value).Split('|');
        string selectedGroupBy = parts[2];
        string exportColumns = string.Empty;
        string exportNewColumns = string.Empty;
        if (HasQueuingMechanism)
        {
            exportColumns = string.Join(",", new string[]{
                "AssignmentName", ManageAssignmentConstanst.COLUMN_NAME_FUTURE_START_DATE,  ASSIGNMENT_TYPE_COL, "TotalMerchantCount",
                "AlertMerchantCount","AlertMerchantVolume","WKCount","WKVolume",
                "WIPCount",  "WIPVolume", "WorkedCount",
                "WorkedVolume", REQUEUED_COUNT_COL, REQUEUED_VOLUME_COL,
                "CompletePercent", ManageAssignmentConstanst.COLUMN_NAME_START_DATE, "ExpirationDate",
                "ProcessingStatus", "ProcessingStatusDate"});
            exportNewColumns = string.Join(",", new string[]{
                GetLocalResourceObject("rm_Assignment_Management_aspx_cs_AssignmentName").ToString()
            , GetLocalResourceObject("rm_Assignment_Management_aspx_cs_FutureStartDate").ToString()
            , GetLocalResourceObject("rm_Assignment_Management_aspx_cs_Type").ToString()
            , GetLocalResourceObject("rm_Assignment_Management_aspx_cs_MerchantCountTotalMerch").ToString()
            ,GetLocalResourceObject("rm_Assignment_Management_aspx_cs_MerchantCountAlert").ToString()
            , GetLocalResourceObject("rm_Assignment_Management_aspx_cs_Volume").ToString(),
            GetLocalResourceObject("rm_Assignment_Management_aspx_cs_WkCount").ToString(),
            GetLocalResourceObject("rm_Assignment_Management_aspx_cs_WkVolume").ToString(),
            GetLocalResourceObject("rm_Assignment_Management_aspx_cs_WIPCount").ToString()
            , GetLocalResourceObject("rm_Assignment_Management_aspx_cs_WIPVolume").ToString()
            , GetLocalResourceObject("rm_Assignment_Management_aspx_cs_WorkedCount").ToString()
            , GetLocalResourceObject("rm_Assignment_Management_aspx_cs_WorkedVolume").ToString()
            , _requeuedCountHeaderExp, _requeuedVolumeHeaderExp
            ,GetLocalResourceObject("rm_Assignment_Management_aspx_cs_Complete").ToString()
            ,GetLocalResourceObject("rm_Assignment_Management_aspx_cs_StartDate").ToString()
            , GetLocalResourceObject("rm_Assignment_Management_aspx_cs_ExpirationDate").ToString()
            , GetLocalResourceObject("rm_Assignment_Management_aspx_cs_ProcStatus").ToString()
            ,GetLocalResourceObject("rm_Assignment_Management_aspx_cs_LastProcStatusDate").ToString()
});
        }
        else
        {
            exportColumns = string.Join(",", new string[]{
                "AssignmentName", ManageAssignmentConstanst.COLUMN_NAME_FUTURE_START_DATE, "TotalMerchantCount", "AlertMerchantCount",
                "AlertMerchantVolume","WKCount","WKVolume", "WIPCount",
                "WIPVolume", "WorkedCount", "WorkedVolume",
                "CompletePercent", ManageAssignmentConstanst.COLUMN_NAME_START_DATE, "ExpirationDate",
                "ProcessingStatus", "ProcessingStatusDate"});
            exportNewColumns = string.Join(",", new string[]{
                  GetLocalResourceObject("rm_Assignment_Management_aspx_cs_AssignmentName").ToString(),
                  GetLocalResourceObject("rm_Assignment_Management_aspx_cs_FutureStartDate").ToString(),
                 GetLocalResourceObject("rm_Assignment_Management_aspx_cs_MerchantCountTotalMerch").ToString(),
                 GetLocalResourceObject("rm_Assignment_Management_aspx_cs_MerchantCountAlert").ToString(),
                GetLocalResourceObject("rm_Assignment_Management_aspx_cs_Volume").ToString(),
                GetLocalResourceObject("rm_Assignment_Management_aspx_cs_WkCount").ToString(),
                GetLocalResourceObject("rm_Assignment_Management_aspx_cs_WkVolume").ToString(),
                 GetLocalResourceObject("rm_Assignment_Management_aspx_cs_WIPCount").ToString(),
                 GetLocalResourceObject("rm_Assignment_Management_aspx_cs_WIPVolume").ToString(),
                 GetLocalResourceObject("rm_Assignment_Management_aspx_cs_WorkedCount").ToString(),
                GetLocalResourceObject("rm_Assignment_Management_aspx_cs_WorkedVolume").ToString(),
                GetLocalResourceObject("rm_Assignment_Management_aspx_cs_Complete").ToString()
                ,GetLocalResourceObject("rm_Assignment_Management_aspx_cs_StartDate").ToString()
                , GetLocalResourceObject("rm_Assignment_Management_aspx_cs_ExpirationDate").ToString()
                , GetLocalResourceObject("rm_Assignment_Management_aspx_cs_ProcStatus").ToString()
                ,GetLocalResourceObject("rm_Assignment_Management_aspx_cs_LastProcStatusDate").ToString()});
        }
        List<string> headers = GetHeaders();
        List<string> ids = GetIDs(selectedGroupBy);
        List<DataTable> tables = new List<DataTable>();

        foreach (var i in ids)
        {
            var table = selectedGroupBy == "User" ? GetAssignmentsByUserID(i.Trim(), true) :
                GetAssignmentsByGroupID(i.Trim(), true);
            tables.Add(BuildDataExport(table));
        }

        string hasQueuingQuery = string.Format("&hasQueuing={0}", HasQueuingMechanism);

        if (id == "imgCSV" || id == "imgCSVBottom")
        {
            StringBuilder sbContent = RiskExporterAs.GetContentCSV(headers, tables,
                exportColumns, exportNewColumns);
            SessionManager.DataCSV = sbContent;
            Response.Redirect("rm_MCF_ExportAssignment.aspx?" + this.BuildSecureQueryString("exp=c" + hasQueuingQuery));
        }
        else if (id == "imgExcel" || id == "imgExcelBottom")
        {
            SessionManager.AssignmentGroupTables = tables;
            SessionManager.HeaderList = headers;
            Response.Redirect("rm_MCF_ExportAssignment.aspx?" + this.BuildSecureQueryString("exp=e" + hasQueuingQuery));
        }
        else
        {
            SessionManager.AssignmentGroupTables = tables;
            SessionManager.HeaderList = headers;
            Response.Redirect("rm_MCF_ExportAssignment.aspx?" + this.BuildSecureQueryString("exp=p" + hasQueuingQuery));
        }
    }

    protected void ExportData(object sender, EventArgs ea)
    {
        OnPostBackActions(PostBackAction.ExportGridUserGroup, sender);
    }

    #region uxAssignmentGrid

    protected void lnkEditAssignment_Command(object sender, CommandEventArgs e)
    {
        OnPostBackActions(PostBackAction.EditAssignment, e);
    }

    protected void uxAssignmentGrid_DoReportHeader(object sender, LineArgs e)
    {
        e.ReportHeader = _exportFileName;
    }


    protected void uxAssignmentGrid_OnSortCommand(object source, GridSortCommandEventArgs e)
    {
        if (IsIntruderDetected) return;
        if (e.Item is GridHeaderItem)
        {
            _currentSortOrder = "";
            _currentSortExpr = "";
            GridTableView tableView = e.Item.OwnerTableView;
            if (tableView.SortExpressions.Count > 0)
            {
                _currentSortExpr = tableView.SortExpressions[0].FieldName;
                switch (tableView.SortExpressions[0].SortOrder)
                {
                    case GridSortOrder.Ascending:
                        _currentSortOrder = "ASC";
                        break;
                    case GridSortOrder.Descending:
                        _currentSortOrder = "DESC";
                        break;
                    case GridSortOrder.None:
                        _currentSortOrder = "";
                        _currentSortExpr = "";
                        break;
                }
            }
        }
    }

    private void uxAssignmentGrid_NeedDataSource()
    {
        divPagerTop.Visible = false;
        if (IsIntruderDetected) return;
        DataTable source = LoadAssignmentData();
        source.Columns.Add("ExpirationDateExport");
        foreach (DataRow row in source.Rows)
        {
            row["ExpirationDateExport"] = ((DateTime)row["ExpirationDate"]).Year == 2999 ? GetLocalResourceObject("rm_Assignment_Management_cs_HeaderExp_NeverExpire").ToString() : ((DateTime)row["ExpirationDate"]).ToString("MM/dd/yyyy");
        }

        uxExporter.Visible = (source.Rows.Count < 1) ? false : true;

        if (_isExporting)
        {
            var dataSource = BuildDataExport(AssignmentsTable);
            if (!string.IsNullOrEmpty(uxAssignmentGrid.AS_SortExpression))
            {
                dataSource.DefaultView.Sort = uxAssignmentGrid.AS_SortExpression;
                dataSource = dataSource.DefaultView.ToTable();
            }

            uxAssignmentGrid.DataSource = dataSource;
        }
        else
        {
            uxAssignmentGrid.DataSource = source;
        }

        if (source.Rows.Count == 0)
            uxAssignmentGrid.AllowSorting = false;
        else
            uxAssignmentGrid.AllowSorting = true;
    }

    private void uxAssignmentGrid_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (IsIntruderDetected) return;
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView dataRow = dataItem.DataItem as DataRowView;
            string assignmentID = dataRow["AssignmentID"].ToString();
            BuildDataItems(sender, dataItem, dataRow, assignmentID);
            if (dataItem["WorkedVolume"].Text.Contains('('))
            {
                dataItem["WorkedVolume"].Text = VeraCodeSolution.DoVeraCode(string.Format("<font color='red'>{0}</font>", dataItem["WorkedVolume"].Text));
            }
        }
    }

    private void SetVisibleForRequeuedAsColumns()
    {
        // Visibility of 3 columns are based on HasQueuingMechanism
        // Assignment Type, Re-queued Assignment count, Re-queued Assignment Volume
        uxAssignmentGrid.Columns.FindByUniqueName(ASSIGNMENT_TYPE_COL).Visible =
            HasQueuingMechanism;
        uxAssignmentGrid.Columns.FindByUniqueName(REQUEUED_COUNT_COL).Visible =
            HasQueuingMechanism;
        uxAssignmentGrid.Columns.FindByUniqueName(REQUEUED_VOLUME_COL).Visible =
            HasQueuingMechanism;
        //for snet readonly 
        uxAssignmentGrid.Columns.FindByUniqueName(DELETE_COL).Visible = GeneralFuncsLib.ReadOnlyRiskManagementEnableStatus((SecurePage)Page);
    }

    #endregion uxAssignmentGrid

    #region uxUserGroupGrid

    protected void lnkEditUserGroup_Command(object sender, CommandEventArgs e)
    {
        OnPostBackActions(PostBackAction.EditAssignment, e);
    }

    private void uxUserGroupGrid_NeedDataSource()
    {
        if (IsIntruderDetected) return;
        if (!_iSort) UserGroupSortList = string.Empty;
        string[] parts = GeneralFuncsLib.NvlString(hddFilter.Value).Split('|');
        string selectedGroupBy = parts[2];
        bool DisplayPaging = false;

        if (selectedGroupBy.ToUpper() == "USER")
        {
            uxUserGroupGrid.DataSource = UserGridDataSource;
            DisplayPaging = UserGridDataSource.Rows.Count > 0;
            exportGroup.Visible = UserGridDataSource.Rows.Count > 0;
            if (_isFromGroupByCombo)
                this.uxUserGroupGrid.MasterTableView.CurrentPageIndex = 0;

            if (UserGridDataSource.Rows.Count == 0)
                _sttMode = GetLocalResourceObject("rm_Assignment_Management_aspx_cs_nodata").ToString();
            else if (UserGridDataSource.Rows.Count == 1)
                _sttMode = GetLocalResourceObject("rm_Assignment_Management_aspx_cs_onlyuser").ToString();
            else
                _sttMode = GetLocalResourceObject("rm_Assignment_Management_aspx_cs_users").ToString();
        }
        else if (selectedGroupBy.ToUpper() == "GROUP")
        {
            uxUserGroupGrid.DataSource = GroupGridDataSource;
            DisplayPaging = GroupGridDataSource.Rows.Count > 0;
            exportGroup.Visible = GroupGridDataSource.Rows.Count > 0;
            if (_isFromGroupByCombo)
                uxUserGroupGrid.MasterTableView.CurrentPageIndex = 0;

            if (GroupGridDataSource.Rows.Count == 0)
                _sttMode = GetLocalResourceObject("rm_Assignment_Management_aspx_cs_nodata").ToString();
            else if (GroupGridDataSource.Rows.Count == 1)
                _sttMode = GetLocalResourceObject("rm_Assignment_Management_aspx_cs_onlygroup").ToString();
            else
                _sttMode = GetLocalResourceObject("rm_Assignment_Management_aspx_cs_groups").ToString();
        }
        else
        {
            return;
        }



        DisplayGridPaging(uxUserGroupGrid as RadGrid, DisplayPaging);


        if (uxUserGroupGrid.DataSource == null)
            divPagerTop.Visible = false;
        else
        {
            divPagerTop.Visible = true;
            int rowCount = (selectedGroupBy.ToUpper() == "USER" ? UserGridDataSource.Rows.Count : GroupGridDataSource.Rows.Count);
            int pageIndex = this.uxUserGroupGrid.MasterTableView.CurrentPageIndex;
            double value = ((double)rowCount / (double)this.uxUserGroupGrid.MasterTableView.PageSize);
            int pageCount = (int)Math.Ceiling(value);

            if ((rowCount != null) && (rowCount > 0))
                this.litPager.Text = VeraCodeSolution.DoVeraCode(string.Format("<span style=\"color: #3e3e3e\"><strong>{0} </strong>{1}</span>", rowCount, _sttMode));
            else
            {
                uxUserGroupGrid.Visible = false;
                this.litPager.Text = VeraCodeSolution.DoVeraCode(string.Format("<span style=\"color: #3e3e3e\">{0}</span>", _sttMode));
            }
        }
    }

    protected void uxUserGroupGrid_DetailTableDataBind(object source, GridDetailTableDataBindEventArgs e)
    {
        if (IsIntruderDetected) return;
        GridDataItem ParentItem = e.DetailTableView.ParentItem as GridDataItem;
        string[] parts = GeneralFuncsLib.NvlString(hddFilter.Value).Split('|');
        string selectedGroupBy = parts[2];
        uxUserGroupGrid.AllowSorting = true;
        if (selectedGroupBy.ToUpper() == "USER")
        {
            e.DetailTableView.DataSource = GetAssignmentsByUserID((string)ParentItem.GetDataKeyValue(USER_ID), false);
            this._expTableList.Add(e.DetailTableView.DataSource as DataTable);
        }
        else if (selectedGroupBy.ToUpper() == "GROUP")
        {
            e.DetailTableView.DataSource = GetAssignmentsByGroupID(((int)ParentItem.GetDataKeyValue(GROUP_ID)).ToString(), false);
            this._expTableList.Add(e.DetailTableView.DataSource as DataTable);
        }
        else return;
    }

    protected void uxUserGroupGrid_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (IsIntruderDetected) return;
        if (e.Item is GridDataItem && e.Item.OwnerTableView.Name == "Detail")
        {
            var dataItem = e.Item as GridDataItem;
            if (!this.uxListOfGridId.Value.Contains(e.Item.OwnerTableView.ClientID))
            {
                if (!string.IsNullOrEmpty(this.uxListOfGridId.Value))
                {
                    this.uxListOfGridId.Value += ";";
                }
                this.uxListOfGridId.Value += VeraCodeSolution.DoVeraCode(e.Item.OwnerTableView.ClientID);
            }

            if (_iSort)
            {
                string[] parts = GeneralFuncsLib.NvlString(hddFilter.Value).Split('|');
                string selectedGroupBy = parts[2];
                if (selectedGroupBy == "User")
                    ReplaceGroupSortList(dataItem["UserID"].Text);
                else
                    ReplaceGroupSortList(dataItem["GroupID"].Text);
            }
            _iSort = false;
            BuildDataItems(sender, dataItem, e.Item.DataItem as DataRowView, dataItem[ASSIGNMENT_ID].Text.Trim());

            if (dataItem["WorkedVolume"].Text.Contains('('))
            {
                dataItem["WorkedVolume"].Text = VeraCodeSolution.DoVeraCode(string.Format("<font color='red'>{0}</font>", dataItem["WorkedVolume"].Text));
            }
        }
    }

    protected void uxUserGroupGrid_SortCommand(object source, GridSortCommandEventArgs e)
    {
        _iSort = true;
        if (e.Item is GridHeaderItem && e.Item.OwnerTableView.Name == "Detail")
        {
            _currentSortOrder = string.Empty;
            _currentSortExpr = string.Empty;
            GridTableView tableView = e.Item.OwnerTableView;
            if (tableView.SortExpressions.Count > 0)
            {
                _currentSortExpr = tableView.SortExpressions[0].FieldName;
                switch (tableView.SortExpressions[0].SortOrder)
                {
                    case GridSortOrder.Ascending:
                        _currentSortOrder = "ASC";
                        break;
                    case GridSortOrder.Descending:
                        _currentSortOrder = "DESC";
                        break;
                    case GridSortOrder.None:
                        _currentSortOrder = string.Empty;
                        _currentSortExpr = string.Empty;
                        break;
                }
            }
        }
    }

    protected void uxUserGroupGrid_ItemCreated(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridNestedViewItem)
        {
            GridNestedViewItem nestedItem = e.Item as GridNestedViewItem;
            nestedItem.NestedViewCell.PreRender += new EventHandler(NestedViewCell_PreRender);
        }
    }

    protected void uxUserGroupGrid_ItemEvent(object sender, GridItemEventArgs e)
    {
        if (e.EventInfo is GridInitializePagerItem)
        {
            GridInitializePagerItem info = e.EventInfo as GridInitializePagerItem;
            e.Canceled = true;
        }
    }

    protected void uxUserGroupGrid_PreRender(object sender, EventArgs e)
    {
        uxUserGroupGrid.PagerStyle.AlwaysVisible = uxUserGroupGrid.MasterTableView.PagerStyle.AlwaysVisible = false;
        uxUserGroupGrid.ShowFooter = false;
    }

    private void SetVisibleForRequeuedUserGroupColumns()
    {
        // Visibility of 3 columns are based on HasQueuingMechanism
        // Assignment Type, Re-queued Assignment count, Re-queued Assignment Volume
        uxUserGroupGrid.MasterTableView.DetailTables[0].Columns.
            FindByUniqueName(ASSIGNMENT_TYPE_COL).Visible = HasQueuingMechanism;
        uxUserGroupGrid.MasterTableView.DetailTables[0].Columns.
            FindByUniqueName(REQUEUED_COUNT_COL).Visible = HasQueuingMechanism;
        uxUserGroupGrid.MasterTableView.DetailTables[0].Columns.
            FindByUniqueName(REQUEUED_VOLUME_COL).Visible = HasQueuingMechanism;
        //for snet readonly 
        uxUserGroupGrid.MasterTableView.DetailTables[0].Columns.
            FindByUniqueName(DELETE_COL).Visible = GeneralFuncsLib.ReadOnlyRiskManagementEnableStatus((SecurePage)Page);
    }

    #endregion uxUserGroupGrid

    #region util

    private List<string> GetHeaders()
    {
        List<string> headers = new List<string>();
        string[] parts = GeneralFuncsLib.NvlString(hddFilter.Value).Split('|');
        string selectedGroupBy = parts[2];

        if (selectedGroupBy.ToUpper() == "USER")
        { //user view
            for (int i = 0; i < UserGridDataSource.Rows.Count;
                headers.Add(UserGridDataSource.Rows[i++][1].ToString())) ;
        }
        else
        {
            for (int i = 0; i < GroupGridDataSource.Rows.Count;
                headers.Add(GroupGridDataSource.Rows[i++][1].ToString())) ;
        }
        return headers;
    }

    private List<string> GetIDs(string selectedGroupBy)
    {
        List<string> ids = new List<string>();
        if (selectedGroupBy.ToUpper() == "USER")
        {
            for (int i = 0; i < UserGridDataSource.Rows.Count;
                ids.Add(UserGridDataSource.Rows[i++][0].ToString())) ;
        }
        else
        {
            for (int i = 0; i < GroupGridDataSource.Rows.Count;
                ids.Add(GroupGridDataSource.Rows[i++][0].ToString())) ;
        }
        return ids;
    }

    private DataTable GetAssignmentsByGroupID(string groupId, bool isExport)
    {
        DataTable TmpTable = null;

        AssignmentsTable.DefaultView.RowFilter = string.Format("GroupID = '{0}'", groupId);
        AssignmentsTable.DefaultView.Sort = "AssignmentName ASC";
        if (isExport)
        {
            string sortName = GetSortFieldOnExport(groupId);
            if (sortName != string.Empty) AssignmentsTable.DefaultView.Sort = sortName;
        }
        TmpTable = AssignmentsTable.DefaultView.ToTable("Assignments");

        return TmpTable;
    }

    private DataTable GetAssignmentsByUserID(string userId, bool isExport)
    {
        DataTable TmpTable = null;
        AssignmentsTable.DefaultView.RowFilter = string.Format("UserID = '{0}'", userId);
        AssignmentsTable.DefaultView.Sort = "AssignmentName ASC";
        if (isExport)
        {
            string sortName = GetSortFieldOnExport(userId);
            if (sortName != string.Empty) AssignmentsTable.DefaultView.Sort = sortName;
        }
        TmpTable = AssignmentsTable.DefaultView.ToTable("Assignments");
        return TmpTable;
    }

    private string GetSortFieldOnExport(string usergroupid)
    {
        Regex reg = new Regex(usergroupid + "[^;]*");
        string[] result = Regex.Match(UserGroupSortList, usergroupid + "[^;]*").ToString().Split(',');
        if (result.Length > 1)
            return result[1];
        else return string.Empty;
    }

    private void RebindGridData()
    {
        if (this.uxGroupBy.SelectedValue == null) return;
        string[] parts = GeneralFuncsLib.NvlString(hddFilter.Value).Split('|');
        string selectedGroupBy = string.Empty;

        selectedGroupBy = parts[2];

        switch (selectedGroupBy.ToUpper())
        {
            case "ASSIGNMENT": // Assignment View
                uxAssignmentGrid.MasterTableView.Rebind();
                break;
            case "USER": // User View
                uxUserGroupGrid.MasterTableView.Rebind();
                break;
            case "GROUP": // Group View
                uxUserGroupGrid.MasterTableView.Rebind();
                break;
            default: // Assignment View
                uxAssignmentGrid.MasterTableView.Rebind();
                break;
        }
    }

    /// <summary>
    /// set default value to control
    /// </summary>
    private void SetFilterDefault()
    {
        if (!string.IsNullOrEmpty(SessionManager.AssignementFilteringOptions))
        {
            string[] parts = GeneralFuncsLib.NvlString(SessionManager.AssignementFilteringOptions).Split(';');

            if (parts.Length == 5)
            {
                int filterType = 0;
                int activeType = 0;

                if (Int32.TryParse(parts[0], out filterType))
                    this.FilterType = (WebSiteEnums.AssignmentFilterTypes)filterType;

                this.FilterValue = parts[1];
                uxGroupBy.SelectedValue = parts[2];

                if (Int32.TryParse(parts[3], out activeType))
                    this.ActiveType = (WebSiteEnums.AssignmentActiveTypes)activeType;

                uxReportDate.SelectedDate = DateTime.Parse(parts[4]);
            }
        }
        else
        {
            uxReportDate.SelectedDate = uxReportDate.MaxDate = DateTime.Today;
        }
    }

    /// <summary>
    /// saved current filter in control into hidden field control
    /// </summary>
    private void SaveCurrentFilter()
    {
        string filterMode = string.Empty;
        string filterValue = string.Empty;
        string GroupBy = string.Empty;

        int selectedFilter = int.Parse(uxFilterType.SelectedValue);

        if (selectedFilter == (int)FilteringType.All)
            filterMode = "All";
        if (selectedFilter == (int)FilteringType.User)
        {
            filterMode = "User";
            filterValue = uxComboUser.SelectedValue;
        }
        if (selectedFilter == (int)FilteringType.Group)
        {
            filterMode = "Group";
            filterValue = uxComboGroup.SelectedValue;
        }
        if (selectedFilter == (int)FilteringType.Assignment)
        {
            filterMode = "Assignment";
            filterValue = uxComboAssignment.SelectedValue;
        }
        if (selectedFilter == (int)FilteringType.SubsiteAssignment)
        {
            filterMode = "Subsite";
            filterValue = uxComboAssignment.SelectedValue;
        }

        hddFilter.Value = filterMode + "|" + filterValue + "|" + uxGroupBy.SelectedValue + "|" + uxReportDate.SelectedDate.Value.ToString();
    }

    /// <summary>
    /// Swith display between 2 grid
    /// </summary>
    private void SwitchView()
    {
        if (this.uxGroupBy.SelectedValue == null) return;
        string[] parts = GeneralFuncsLib.NvlString(hddFilter.Value).Split('|');
        string selectedGroupBy = string.Empty;
        selectedGroupBy = parts[2];
        switch (selectedGroupBy.ToUpper())
        {
            case "ASSIGNMENT": // Assignment View
                uxAssignmentGrid.Visible = true;
                uxUserGroupGrid.Visible = this.exportGroup.Visible = uxGroupByGridPadding.Visible = false;
                break;
            case "USER": // User View
                uxAssignmentGrid.Visible = false;
                uxUserGroupGrid.Visible = true;
                uxGroupByGridPadding.Visible = true;
                uxUserGroupGrid.MasterTableView.DataKeyNames = new string[1] { USER_ID };
                uxUserGroupGrid.MasterTableView.DetailTables[0].ParentTableRelation[0].MasterKeyField = USER_ID;
                uxUserGroupGrid.MasterTableView.DetailTables[0].ParentTableRelation[0].DetailKeyField = USER_ID;
                // Change data of master table                 
                (uxUserGroupGrid.MasterTableView.Columns[0] as GridBoundColumn).DataField = USER_NAME;
                (uxUserGroupGrid.MasterTableView.Columns[0] as GridBoundColumn).UniqueName = USER_NAME;

                break;
            case "GROUP": // Group View
                uxAssignmentGrid.Visible = false;
                uxUserGroupGrid.Visible = true;
                uxGroupByGridPadding.Visible = true;
                // Change Relationship
                uxUserGroupGrid.MasterTableView.DataKeyNames = new string[1] { GROUP_ID };
                uxUserGroupGrid.MasterTableView.DetailTables[0].ParentTableRelation[0].MasterKeyField = GROUP_ID;
                uxUserGroupGrid.MasterTableView.DetailTables[0].ParentTableRelation[0].DetailKeyField = GROUP_ID;
                // Change data of master table                 
                (uxUserGroupGrid.MasterTableView.Columns[0] as GridBoundColumn).DataField = GROUP_NAME;
                (uxUserGroupGrid.MasterTableView.Columns[0] as GridBoundColumn).UniqueName = GROUP_NAME;

                break;
            default: // Assignment View
                uxAssignmentGrid.Visible = true;
                uxUserGroupGrid.Visible = this.exportGroup.Visible = false;
                break;
        }
        uxExporter.Visible = uxAssignmentGrid.Visible;
        if (uxAssignmentGrid.Visible == false)
        {
            AjaxAddResponseScript("refreshUIElements(false);");
        }
        else
        {
            AjaxAddResponseScript("refreshUIElements(true);");
        }
    }

    /// <summary>
    /// create new assignment in temporary table
    /// </summary>
    /// <returns>assignmentID in temporary table</returns>
    public int Save()
    {
        int AssignmentID = 0;
        FilterParameterCollection paramsIn = new FilterParameterCollection();
        paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramsIn.Add(new FilterParameter("@AssignmentID", 0, DbType.Int32, true));
        paramsIn.Add(new FilterParameter("@AssignmentName", string.Empty, DbType.AnsiString));
        paramsIn.Add(new FilterParameter("@ExpirationDate", DateTime.Now.AddDays(7), DbType.DateTime));
        FilterParameterCollection paramsOut;
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_AddAssignment", paramsIn, out paramsOut);
        AssignmentID = Int32.Parse(paramsOut[0].ParameterValue.ToString());
        return AssignmentID;
    }

    /// <summary>
    /// Bound data into grid
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="dataItem">Item data row</param>
    /// <param name="assignmentID">AssignmentID</param>
    private void BuildDataItems(object sender, GridDataItem dataItem, DataRowView rowData, string assignmentID)
    {
        BuildStartDate(dataItem, rowData);
        GetIconFutureStartDate(dataItem, rowData);
        var dateTime = GeneralFuncsLib.GetDateTimeValue(rowData["ExpirationDate"].ToString());
        var radDatePicker = (System.Web.UI.WebControls.TextBox)dataItem["ExpirationDateTemplate"].FindControl("uxExpirationDate");
        var uxCalendar = (AS.Controls.Global.PlaceHolder)dataItem["ExpirationDateTemplate"].FindControl("uxCalendar");
        bool hasSubsiteAssignmentPer = ((SecurePage)this.Page).IsUserWithPermission("SubsiteAssignment");
        var currentUserType = SessionManager.CurrentUserType;
        var isMSUser = currentUserType != WebSiteEnums.UserHierarchyMode.CS && currentUserType != WebSiteEnums.UserHierarchyMode.AS;
        var LiteralDatePicker = (System.Web.UI.WebControls.Literal)dataItem["ExpirationDateTemplate"].FindControl("uxExpirationDateNever");

        if (rowData["ExpirationDate"] == null || rowData["ExpirationDate"] == DBNull.Value || Convert.ToDateTime(rowData["ExpirationDate"]).Year == 1 || Convert.ToDateTime(rowData["ExpirationDate"]).Year == 2999)
        {
            LiteralDatePicker.Visible = true;
            radDatePicker.Visible = false;
            uxCalendar.Visible = false;
        }
        else
        {
            LiteralDatePicker.Visible = false;
            radDatePicker.Visible = true;
            uxCalendar.Visible = true;
            radDatePicker.ReadOnly = true;
            radDatePicker.Text = VeraCodeSolution.DoVeraCode(dateTime.ToString("MM/dd/yyyy"));
            LiteralDatePicker.Text = VeraCodeSolution.DoVeraCode(dateTime.ToString("MM/dd/yyyy"));
        }

        if (dateTime < DateTime.Today)
            radDatePicker.CssClass = radDatePicker.CssClass + " red";

        radDatePicker.Enabled = GeneralFuncsLib.ReadOnlyRiskManagementEnableStatus((SecurePage)Page);

        if (GeneralFuncsLib.NvlString(rowData["ProcessingStatusCode"]) == "4")
        {
            dataItem["ProcessingStatus"].ForeColor = Color.Red;
            if (string.Compare(SessionManager.CurrentUser.UserSecRole, "FDUSER") == 0)
                dataItem["ProcessingStatus"].ToolTip = VeraCodeSolution.DoVeraCode(GeneralFuncsLib.NvlString(rowData["ProcessingStatusNotes"]));
        }

        // Format AssignmentType
        dataItem[ASSIGNMENT_TYPE_COL].Text = GeneralFuncsLib.BuildAssignmentTypeAbbr(rowData[ASSIGNMENT_TYPE_DB]);

        // We only delete/ edit Assignment if it is not a Distinct Detection Queue Assignment.
        if (dataItem[ASSIGNMENT_TYPE_COL].Text.Equals(GeneralFuncsLib.DISTINCT_DETECTION_QUEUE_AS_VALUE))
        {
            dataItem[ASSIGNMENT_NAME].Text = VeraCodeSolution.DoVeraCode(
                string.Format("<a class=\"link\" href=\"#\" style=\"cursor:pointer\" onclick=\"alert('{0}'); return false;\">{1}</a>",
                Resources.RiskMessageManager.Risk_CannotEditDTAssignment,
                rowData[ASSIGNMENT_NAME_DB])
                );
        }
        else if ((WebSiteEnums.AssignmentType)(rowData["AssignmentType"].ToInt()) == WebSiteEnums.AssignmentType.AggregateQueue)
        {
            dataItem[ASSIGNMENT_NAME].Text = VeraCodeSolution.DoVeraCode(rowData[ASSIGNMENT_NAME].ToString());
        }
        else
        {
            var lbel = (AS.Controls.Global.Literal)dataItem["Delete"].FindControl("lbDelete");
            lbel.Text = VeraCodeSolution.GetOutputHtmlString(
                string.Format("<a href='#' onclick=\"return DeleteAssignment('{0}')\">{1}</a>",
                              assignmentID, GetLocalResourceObject("rm_Assignment_Management_aspx_cs_TextDelete").ToString()));
            if ((WebSiteEnums.AssignmentType)rowData["AssignmentType"].ToInt() == WebSiteEnums.AssignmentType.DetectionQueue
                || ((WebSiteEnums.AssignmentType)rowData["AssignmentType"].ToInt() == WebSiteEnums.AssignmentType.Subsite) && hasSubsiteAssignmentPer && !isMSUser)
            {
                var lbelDuplicate = (AS.Controls.Global.Literal)dataItem["Duplicate"].FindControl("lbDuplicate");
                lbelDuplicate.Text = VeraCodeSolution.GetOutputHtmlString(
                    string.Format("<a title=\"{0}\" href='#' onclick=\"openDuplicateAssignment('{1}');return false;\">{2}</a>", GetLocalResourceObject("DuplicateAssignmentResource").ToString(),
                                  assignmentID, "<span class=\"duplicate-icon\"><img src=\"../res/img/duplicateIcon.png\" /></span>"));
            }
        }


        //process for subsite MS
        if ((WebSiteEnums.AssignmentType)(rowData["AssignmentType"].ToInt()) == WebSiteEnums.AssignmentType.Subsite && (!hasSubsiteAssignmentPer || isMSUser))
        {
            uxCalendar.Visible = false;
            LiteralDatePicker.Visible = true;
            var lbDelete = (AS.Controls.Global.Literal)dataItem["Delete"].FindControl("lbDelete");
            lbDelete.Text = string.Empty;
        }

        var progressBar = dataItem["CompleteTemplate"].FindControl("progressBar") as ProgressBar;
        if (string.IsNullOrEmpty(rowData["CompletePercent"].ToString()))
        {
            progressBar.Visible = false;
            dataItem["CompleteTemplate"].Text = "—";
        }
        else
        {
            progressBar.Visible = true;
            progressBar.Value = decimal.Parse(rowData["CompletePercent"].ToString());
        }
    }


    protected void btnExtendAssignment_Click(object sender, EventArgs e)
    {
        var postBackAction = GetPostBackActionChangeDate();
        OnPostBackActions(postBackAction);
    }

    protected void uxExpirationDate_SelectedDataChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
    {
        GridDataItem dataItem = (sender as AS.Controls.Global.RadDatePicker).Parent.Parent as GridDataItem;
        hddAssignmentID.Value = dataItem["AssignmentID"].Text;
        if ((sender as AS.Controls.Global.RadDatePicker).SelectedDate.Value < DateTime.Today)
        {
            AjaxAddResponseScript("alert(" + GetLocalResourceObject("rm_Assignment_Management_aspx_cs_Expiration").ToString() + ");");
            return;
        }
        hddExtendDate.Value = (sender as AS.Controls.Global.RadDatePicker).SelectedDate.Value.ToString();
        var postBackAction = GetPostBackActionChangeDate();
        OnPostBackActions(postBackAction);
    }

    /// <summary>
    /// Refresh values filter
    /// </summary>
    private void RefreshFilterValues()
    {
        string[] parts = GeneralFuncsLib.NvlString(hddFilter.Value).Split('|');
        if (parts[0] == "User")
            _filteredUserID = parts[1];
        else
            _filteredUserID = string.Empty;

        if (parts[0] == "Group")
            int.TryParse(parts[1], out _filteredGroupID);
        else
            _filteredGroupID = 0;

        if (parts[0] == "Assignment" || parts[0] == "Subsite")
            int.TryParse(parts[1], out _filteredAssignmentID);
        else
            _filteredAssignmentID = 0;

        //Active or Expire
        if (uxAll.Checked)
            _filteredMode = 0;
        else if (uxActive.Checked)
            _filteredMode = 1;
        else if (uxExpire.Checked)
            _filteredMode = 2;

        //Groupby
        _groupBy = parts[2];

        //Date
        _filterDate = DateTime.Parse(parts[3]);
    }

    /// <summary>
    /// Load data
    /// </summary>
    /// <returns>Datatable</returns>
    private DataTable LoadAssignmentData()
    {
        if (uxGroupBy.SelectedValue == null) return null;
        RefreshFilterValues();
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@UserIDFilter", _filteredUserID, DbType.AnsiString));
        parameters.Add(new FilterParameter("@GroupID", _filteredGroupID, DbType.Int32));
        parameters.Add(new FilterParameter("@AssignmentID", _filteredAssignmentID, DbType.Int32));
        parameters.Add(new FilterParameter("@Mode", _filteredMode, DbType.Int32));
        parameters.Add(new FilterParameter("@GroupBy", _groupBy, DbType.AnsiString));
        parameters.Add(new FilterParameter("@ReportDate", _filterDate, DbType.Date));
        parameters.Add(new FilterParameter("@ApplyFilterId", hddApplyFilterId.Value, DbType.String));
        parameters.AddLanguageID();
        _assignmentsTable = WebServices.RiskServices.GetReports("spa_RM_MCF_Get_Assignment", parameters);
        return _assignmentsTable;
    }

    private string UserGroupSortList
    {
        get
        {
            return SessionManager.UserGroupSort != null ? SessionManager.UserGroupSort : null;
        }
        set
        {
            SessionManager.UserGroupSort = value;
        }
    }

    private void ReplaceGroupSortList(string ID)
    {
        if (UserGroupSortList.IndexOf(ID) != -1)
        {
            Regex reg = new Regex(ID + "[^;]*;");
            if (_currentSortExpr != string.Empty)
            {

                UserGroupSortList = reg.Replace(UserGroupSortList, ID + "," + _currentSortExpr + " " + _currentSortOrder + ";");
            }
            else
            {
                UserGroupSortList = reg.Replace(UserGroupSortList, "");
            }
        }
        else
        {
            UserGroupSortList += ID + "," + _currentSortExpr + " " + _currentSortOrder + ";";
        }
    }
    protected override void OnPreRender(EventArgs e)
    {
        // Wait for until prepare the data is done.
        while (_loadDataParallel && !_isThreadDone)
        {
            Thread.Sleep(100);
        }

        if (uxUserGroupGrid.Visible && uxUserGroupGrid.Items.Count == 0)
        {
            uxNoDataPadding.CssClass = "height-18";
        }
        else
        {
            uxNoDataPadding.CssClass = "height-15";
        }
        base.OnPreRender(e);
    }
    private void DisplayGridPaging(RadGrid radGrid, bool DisplayPaging)
    {
        radGrid.MasterTableView.AllowPaging = DisplayPaging;
        radGrid.MasterTableView.PagerStyle.AlwaysVisible = DisplayPaging;
    }

    private void NestedViewCell_PreRender(object sender, EventArgs e)
    {
        ((Control)sender).Controls[0].SetRenderMethodDelegate(new RenderMethod(this.NestedViewTable_Render));
    }

    protected void NestedViewTable_Render(HtmlTextWriter writer, Control control)
    {
        control.SetRenderMethodDelegate(null);
        if (control.Controls.Count > 0)
        {
            int count = control.Controls[0].Controls.Count / 2;
            if (count > 6)
            {
                writer.Write(string.Format("<div style='height: {0}px; overflow: scroll;'>", 220));
                control.RenderControl(writer);
                writer.Write("</div>");
            }
            else
                control.RenderControl(writer);
        }
        else
            control.RenderControl(writer);
    }

    #endregion

    protected DataTable AssignmentsTable
    {
        get
        {
            if (_assignmentsTable == null) LoadAssignmentData();
            return _assignmentsTable;
        }
    }

    protected DataTable UserGridDataSource
    {
        get
        {
            DataTable UsersTable = null;
            if (AssignmentsTable != null)
            {
                UsersTable = AssignmentsTable.DefaultView.ToTable("Users", true, new string[2] { USER_ID, USER_NAME });
                for (int i = 0; i < UsersTable.Rows.Count; i++)
                    UsersTable.Rows[i][USER_NAME] = HttpUtility.HtmlEncode(UsersTable.Rows[i][USER_NAME].ToString());
            }
            else
            {
                UsersTable = new DataTable();
                UsersTable.Columns.Add(new DataColumn(USER_ID, typeof(string)));
                UsersTable.Columns.Add(new DataColumn(USER_NAME, typeof(string)));
            }
            return UsersTable;
        }
    }

    protected DataTable GroupGridDataSource
    {
        get
        {
            DataTable GroupsTable = null;

            if (AssignmentsTable != null)
            {
                GroupsTable = AssignmentsTable.DefaultView.ToTable("Groups", true, new string[2] { GROUP_ID, GROUP_NAME });
                for (int i = 0; i < GroupsTable.Rows.Count; i++)
                    GroupsTable.Rows[i][GROUP_NAME] = HttpUtility.HtmlEncode(GroupsTable.Rows[i][GROUP_NAME].ToString());
            }
            else
            {
                GroupsTable = new DataTable();
                GroupsTable.Columns.Add(new DataColumn(GROUP_ID, typeof(string)));
                GroupsTable.Columns.Add(new DataColumn(GROUP_NAME, typeof(string)));
            }

            return GroupsTable;
        }
    }


    private WebSiteEnums.AssignmentActiveTypes ActiveType
    {
        get
        {
            WebSiteEnums.AssignmentActiveTypes activeType = WebSiteEnums.AssignmentActiveTypes.All;
            int activeTypeValue = 0;

            foreach (Control ctl in uxAssignmentActiveTypeRadioButtons.Controls)
            {
                if (ctl is AS.Controls.Global.RadioButton)
                {
                    AS.Controls.Global.RadioButton radioButton = ctl as AS.Controls.Global.RadioButton;

                    if (radioButton.Checked)
                    {
                        if (Int32.TryParse(radioButton.Attributes["xValue"], out activeTypeValue))
                        {
                            activeType = (WebSiteEnums.AssignmentActiveTypes)activeTypeValue;
                        }

                        break;
                    }
                }
            }

            return activeType;
        }

        set
        {
            WebSiteEnums.AssignmentActiveTypes activeType = WebSiteEnums.AssignmentActiveTypes.All;
            int activeTypeValue = 0;

            foreach (Control ctl in uxAssignmentActiveTypeRadioButtons.Controls)
            {
                if (ctl is AS.Controls.Global.RadioButton)
                {
                    AS.Controls.Global.RadioButton radioButton = ctl as AS.Controls.Global.RadioButton;

                    if (Int32.TryParse(radioButton.Attributes["xValue"], out activeTypeValue))
                    {
                        activeType = (WebSiteEnums.AssignmentActiveTypes)activeTypeValue;
                        if (activeType == value)
                            radioButton.Checked = true;
                        else
                            radioButton.Checked = false;
                    }
                }
            }
        }
    }

    public string FilterValue
    {
        get
        {
            WebSiteEnums.AssignmentFilterTypes filterType = this.FilterType;
            if (filterType == WebSiteEnums.AssignmentFilterTypes.User)
                return uxComboUser.SelectedValue;
            else if (filterType == WebSiteEnums.AssignmentFilterTypes.Group)
                return uxComboGroup.SelectedValue;
            else if (filterType == WebSiteEnums.AssignmentFilterTypes.Assignment || filterType == WebSiteEnums.AssignmentFilterTypes.Subsite)
                return uxComboAssignment.SelectedValue;

            return string.Empty;
        }
        set
        {
            WebSiteEnums.AssignmentFilterTypes filterType = this.FilterType;

            if (filterType == WebSiteEnums.AssignmentFilterTypes.User)
                uxComboUser.SelectedValue = value;
            else if (filterType == WebSiteEnums.AssignmentFilterTypes.Group)
                uxComboGroup.SelectedValue = value;
            else if (filterType == WebSiteEnums.AssignmentFilterTypes.Assignment || filterType == WebSiteEnums.AssignmentFilterTypes.Subsite)
                uxComboAssignment.SelectedValue = value;
        }
    }

    private WebSiteEnums.AssignmentFilterTypes FilterType
    {
        get
        {
            int selectedFilterType = int.Parse(uxFilterType.SelectedValue);

            int filterTypeValue = selectedFilterType;
            WebSiteEnums.AssignmentFilterTypes filterType = WebSiteEnums.AssignmentFilterTypes.All;

            filterType = (WebSiteEnums.AssignmentFilterTypes)filterTypeValue;

            return filterType;
        }
        set
        {
            WebSiteEnums.AssignmentFilterTypes filterType = WebSiteEnums.AssignmentFilterTypes.All;

            filterType = (WebSiteEnums.AssignmentFilterTypes)(value);
            uxFilterType.SelectedValue = ((int)filterType).ToString();
        }
    }

    string _AssignmentIntruderQuery = string.Empty;
    private string AssignmentIntruderQuery
    {
        get
        {
            if (_AssignmentIntruderQuery.Length == 0)
                _AssignmentIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(this.ID, new string[] { ASSIGNMENT_ID });
            return _AssignmentIntruderQuery;
        }
    }

    private void LoadAssigmentSummary()
    {
        AS.Web.Business.ReportServices service = WebServices.RiskServices; //

        DataTable dataSource = new DataTable();
        FilterParameterCollection parameters = new FilterParameterCollection();

        parameters.AddLoggedInUserParams(_currentUser, _userMode);
        parameters.Add(new FilterParameter("@UserIDFilter", _filteredUserID, DbType.AnsiString));
        parameters.Add(new FilterParameter("@GroupID", _filteredGroupID, DbType.Int32));
        parameters.Add(new FilterParameter("@AssignmentID", _filteredAssignmentID, DbType.Int32));
        parameters.Add(new FilterParameter("@GroupBy", _groupBy, DbType.AnsiString));
        parameters.Add(new FilterParameter("@ReportDate", _filterDate, DbType.Date));
        parameters.Add(new FilterParameter("@ApplyFilterId", hddApplyFilterId.Value, DbType.String));

        service.AddRequestHeader("ClientId", _clientId.ToString());

        dataSource = service.GetReports("spa_RM_MCF_GetManageAssignmentSummary", parameters);

        if (dataSource != null && dataSource.Rows.Count > 0)
        {
            DataRow dr = dataSource.Rows[0];

            this.uxCountTotal.Text = string.Format("<span>{0}</span>", VeraCodeSolution.DoVeraCode(FormatData.FormatInteger(Formatter.FormatDataToMDash(dr["TotalCount"]))));
            this.uxCountAlerted.Text = string.Format("<span>{0}</span>", VeraCodeSolution.DoVeraCode(FormatData.FormatInteger(Formatter.FormatDataToMDash(dr["AlertedCount"]))));
            this.uxCountWorked.Text = string.Format("<span>{0}</span>", VeraCodeSolution.DoVeraCode(FormatData.FormatInteger(Formatter.FormatDataToMDash(dr["WorkedCount"]))));
            this.uxCountRemaining.Text = string.Format("<span>{0}</span>", VeraCodeSolution.DoVeraCode(FormatData.FormatInteger(Formatter.FormatDataToMDash(dr["RemainingCount"]))));

            var volumnTotal = dr["TotalVolume"].ToString();
            volumnTotal = !string.IsNullOrEmpty(volumnTotal) ? FormatData.FormatCurrency(volumnTotal) : WebSiteConstants.HTML_EM_DASH;
            this.uxVolumeTotal.Text = string.Format("<span>{0}</span>", VeraCodeSolution.DoVeraCode(volumnTotal));

            var volumnAlerted = dr["AlertedVolume"].ToString();
            volumnAlerted = !string.IsNullOrEmpty(volumnAlerted) ? FormatData.FormatCurrency(volumnAlerted) : WebSiteConstants.HTML_EM_DASH;
            this.uxVolumeAlerted.Text = string.Format("<span>{0}</span>", VeraCodeSolution.DoVeraCode(volumnAlerted));

            var volumWorked = dr["WorkedVolume"].ToString();
            volumWorked = !string.IsNullOrEmpty(volumWorked) ? FormatData.FormatCurrency(volumWorked) : WebSiteConstants.HTML_EM_DASH;
            this.uxVolumeWorked.Text = string.Format("<span>{0}</span>", VeraCodeSolution.DoVeraCode(volumWorked));

            var volumRemaining = dr["RemainingVolume"].ToString();
            volumRemaining = !string.IsNullOrEmpty(volumRemaining) ? FormatData.FormatCurrency(volumRemaining) : WebSiteConstants.HTML_EM_DASH;
            this.uxVolumeRemaining.Text = string.Format("<span>{0}</span>", VeraCodeSolution.DoVeraCode(volumRemaining));


            uxEligibleToolTip.Text = string.Format("<span class='radtooltip-content'>{0}</span>", _assignmentResources["EligibleResource1Tooltip"]);
            uxAlertedToolTip.Text = string.Format("<span class='radtooltip-content'>{0}</span>", _assignmentResources["AlertedResource1ToolTip"]);
            uxWorkedToolTip.Text = string.Format("<span class='radtooltip-content'>{0}</span>", _assignmentResources["WorkedResource1ToolTip"]);
            uxRemainingToolTip.Text = string.Format("<span class='radtooltip-content'>{0}</span>", _assignmentResources["RemainingResource1ToolTip"]);
            uxAssignmentSummaryInfo.Text = string.Format("<span class='radtooltip-content'>{0}</span>", _assignmentResources["AssignmentHelpText"]);

        }
    }

    private static string BuildNumberColumn(object obj)
    {
        return obj == DBNull.Value || obj.ToString().IsNullOrEmpty() ? i_valueNA : FormatData.FormatInteger(obj.ToString());
    }

    private static string BuildCurrencyColumn(object obj)
    {
        return obj == DBNull.Value || obj.ToString().IsNullOrEmpty() ?
                    i_valueNA : FormatData.FormatCurrency(obj.ToString(), SessionManager.CurrencyFortmat);
    }

    private DataTable BuildDataExport(DataTable table)
    {
        DataTable newAssignmentTbl = table.Clone();

        // Change DataType of "AssignmentType", "TotalMerchantCount"
        // "RequeueCount" & "RequeueVolume" Columns
        newAssignmentTbl.Columns[ASSIGNMENT_TYPE_DB].DataType = typeof(string);
        newAssignmentTbl.Columns[TOTAL_MERCH_COUNT_DB].DataType = typeof(string);
        newAssignmentTbl.Columns[REQUEUED_COUNT_DB].DataType = typeof(string);
        newAssignmentTbl.Columns[REQUEUED_VOLUME_DB].DataType = typeof(string);

        // add new column "FutureStartDate"
        if (!newAssignmentTbl.Columns.Contains(ManageAssignmentConstanst.COLUMN_NAME_FUTURE_START_DATE))
            newAssignmentTbl.Columns.Add(ManageAssignmentConstanst.COLUMN_NAME_FUTURE_START_DATE, typeof(string));

        // add new column "StartDateExport"
        if (!newAssignmentTbl.Columns.Contains(ManageAssignmentConstanst.COLUMN_NAME_START_DATE_EXPORT))
            newAssignmentTbl.Columns.Add(ManageAssignmentConstanst.COLUMN_NAME_START_DATE_EXPORT, typeof(string));

        // add new column "ExpirationDateExport"
        if (!newAssignmentTbl.Columns.Contains(EXPIRATION_DATE_EXP_DB))
            newAssignmentTbl.Columns.Add(EXPIRATION_DATE_EXP_DB, typeof(string));

        for (int i = 0; i < table.Rows.Count; i++)
        {
            DataRow row = table.Rows[i];
            DataRow newRow = newAssignmentTbl.NewRow();
            newRow[ASSIGNMENT_NAME_DB] = row[ASSIGNMENT_NAME_DB];
            newRow[ASSIGNMENT_TYPE_DB] = GeneralFuncsLib.BuildAssignmentTypeAbbr(row[ASSIGNMENT_TYPE_DB]);

            // Merchant Count
            newRow[TOTAL_MERCH_COUNT_DB] = (row[TOTAL_MERCH_COUNT_DB]);
            newRow[ALERT_MERCH_COUNT_DB] = row[ALERT_MERCH_COUNT_DB];
            newRow[TOTAL_MERCH_VOLUME_DB] = row[TOTAL_MERCH_VOLUME_DB];

            //Ready to work
            newRow[WK_MERCH_COUNT_DB] = row[WK_MERCH_COUNT_DB];
            newRow[WK_MERCH_VOLUME_DB] = row[WK_MERCH_VOLUME_DB];

            // Wip
            newRow[WIP_MERCH_COUNT_DB] = row[WIP_MERCH_COUNT_DB];
            newRow[WIP_MERCH_VOLUME_DB] = row[WIP_MERCH_VOLUME_DB];

            // Worked
            newRow[WORKED_MERCH_COUNT_DB] = row[WORKED_MERCH_COUNT_DB];
            newRow[WORKED_MERCH_VOLUME_DB] = row[WORKED_MERCH_VOLUME_DB];

            //Re-queued
            newRow[REQUEUED_COUNT_DB] = (row[REQUEUED_COUNT_DB]);
            newRow[REQUEUED_VOLUME_DB] = (row[REQUEUED_VOLUME_DB]);

            newRow[COMPLETE_PER_DB] = row[COMPLETE_PER_DB];
            newRow[ManageAssignmentConstanst.COLUMN_NAME_START_DATE_EXPORT] = GetStartDateToExport(row);
            newRow[EXPIRATION_DATE_DB] = row[EXPIRATION_DATE_DB];
            newRow[EXPIRATION_DATE_EXP_DB] = BuildExpirationDate(row[EXPIRATION_DATE_DB]);
            newRow[PROCESS_STATUS_DB] = row[PROCESS_STATUS_DB];
            newRow[PROCESS_STATUS_DATE_DB] = row[PROCESS_STATUS_DATE_DB];
            newRow[ManageAssignmentConstanst.COLUMN_NAME_FUTURE_START_DATE] = GetFutureStartDateToExport(row);
            newAssignmentTbl.Rows.Add(newRow);
        }
        return newAssignmentTbl;
    }

    private void BuildStartDate(GridDataItem dataItem, DataRowView rowData)
    {
        var dateTime = GetStartDateByDataRowView(rowData);
        var radDatePicker = (System.Web.UI.WebControls.TextBox)dataItem["StartDateTemplate"].FindControl("uxStartDate");
        var uxCalendar = (AS.Controls.Global.PlaceHolder)dataItem["StartDateTemplate"].FindControl("uxStartDateCalendar");

        var isStartDate = IsStartDateByAssignmentType(rowData);
        radDatePicker.Visible = true;
        uxCalendar.Visible = isStartDate;
        radDatePicker.ReadOnly = true;
        radDatePicker.Text = isStartDate ? dateTime : string.Empty;

        radDatePicker.Enabled = GeneralFuncsLib.ReadOnlyRiskManagementEnableStatus((SecurePage)Page);
    }
    private static string GetStartDateByDataRowView(DataRowView dataRow)
    {
        var startDateValue = GetValueInfoToDate(dataRow, ManageAssignmentConstanst.COLUMN_NAME_START_DATE);
        if (startDateValue == DateTime.MinValue)
        {
            return null;
        }
        return startDateValue.ToString(ManageAssignmentConstanst.FORMAT_DATE_MMDDYYYY);
    }
    private string BuildExpirationDate(object expDate)
    {
        string expirationDate = string.Empty;
        if (((DateTime)expDate).Year == 2999)
        {
            expirationDate = _neverExpire;
        }
        else
        {
            expirationDate = ((DateTime)expDate).ToString(ManageAssignmentConstanst.FORMAT_DATE_MMDDYYYY);
        }
        return expirationDate;
    }

    private DataTable CheckDeleteAutoQueue(long assignmentId)
    {
        var paramIns = new FilterParameterCollection();

        paramIns.AddLoggedInUserParamsWithRecId();
        paramIns.Add(new FilterParameter("@AssignmentID", assignmentId, DbType.Int64));
        paramIns.Add(new FilterParameter("@IsDeletedAvailable", true, DbType.Boolean));
        DataTable dt = WebServices.RiskServices.GetReports("spa_RM_MCF_Assignment_Check_Request", paramIns);

        return dt;
    }

    private IEnumerable<string> GetListAutoQueueByAssignmentName(DataRowCollection rows)
    {
        foreach (DataRow r in rows)
        {
            yield return r["AutoQueueName"].ToString();
        }
    }

    protected void uxReportDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
    {
        if (uxFilterType.SelectedValue.ToInt() == (int)FilteringType.Assignment || uxFilterType.SelectedValue.ToInt() == (int)FilteringType.SubsiteAssignment)
        {
            OnDataBindControls(DataBindAction.ComboAssignment);
        }
    }
    protected void uxFilterType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        var assignmentType = uxFilterType.SelectedValue.ToInt();
        if (assignmentType == (int)FilteringType.Assignment || assignmentType == (int)FilteringType.SubsiteAssignment)
        {
            OnDataBindControls(DataBindAction.ComboAssignment);
        }
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.Submit, sender);
    }

    protected void uxBtnAssignmentFilter_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.Submit, sender);
    }

    protected void uxOpenEditAssignment_Click(object sender, EventArgs e)
    {
        int assignmentID = RM_MCF_GeneralFuncsLib.MoveDataFromFinalTables(int.Parse(hddOpenEditAssignmentID.Value));
        int featureMode = !GeneralFuncsLib.ReadOnlyRiskManagementEnableStatus((SecurePage)Page) ? 1 : 0;
        bool isSubsiteAssignment = hddOpenEditAssignmentType.Value == ((int)FilteringType.SubsiteAssignment).ToString();
        var currentUserType = SessionManager.CurrentUserType;
        var isMSUser = currentUserType != WebSiteEnums.UserHierarchyMode.CS && currentUserType != WebSiteEnums.UserHierarchyMode.AS;
        if (isSubsiteAssignment && !((SecurePage)this.Page).IsUserWithPermission("SubsiteAssignment") && !isMSUser)
        {
            featureMode = 1;
        }
        string queryString1 = BuildSecureQueryString(string.Format("{0}={1}&{2}={3}{4}", ASSIGNMENT_ID, assignmentID, FEATURE_MODE, featureMode, AssignmentIntruderQuery));
        AjaxAddResponseScript("ShowPopupModal('rm_MCF_Assignment_CreateNew.aspx?" + queryString1 + "','auto');");
    }

    private void RefreshFilterMerchant()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@Force", 0, DbType.Byte));
        parameters.Add(new FilterParameter("@ReportDate", _filterDate, DbType.Date));
        parameters.Add(new FilterParameter("@ApplyFilterId", hddApplyFilterId.Value, DbType.String));
        WebServices.RiskServices.GetReports("spa_RM_MCF_RefreshFilterMerchant", parameters);
    }

    protected void uxOpenDuplicateAssignment_Click(object sender, EventArgs e)
    {
        int assignmentID = RM_MCF_GeneralFuncsLib.MoveDataFromFinalTables(int.Parse(hddOpenEditAssignmentID.Value), 2);
        int featureMode = GeneralFuncsLib.ReadOnlyRiskManagementEnableStatus((SecurePage)Page) == false ? 1 : 0;
        string queryString1 = BuildSecureQueryString(string.Format("{0}={1}&{2}&{3}={4}", ASSIGNMENT_ID, assignmentID, AssignmentIntruderQuery, DUPLICATE, 1));
        AjaxAddResponseScript("ShowPopupModal('rm_MCF_Assignment_CreateNew.aspx?" + queryString1 + "','auto');");
    }

    protected void InitializeFilterType()
    {
        uxFilterType.Items.Add(new AS.Controls.Global.RadComboBoxItem()
        {
            Value = ((int)FilteringType.All).ToString(),
            Text = GetLocalResourceObject("ASRadComboBoxItemResource1.Text").ToString()
        });

        uxFilterType.Items.Add(new AS.Controls.Global.RadComboBoxItem()
        {
            Value = ((int)FilteringType.User).ToString(),
            Text = GetLocalResourceObject("ASRadComboBoxItemResource2.Text").ToString()
        });

        uxFilterType.Items.Add(new AS.Controls.Global.RadComboBoxItem()
        {
            Value = ((int)FilteringType.Group).ToString(),
            Text = GetLocalResourceObject("ASRadComboBoxItemResource3.Text").ToString()
        });

        uxFilterType.Items.Add(new AS.Controls.Global.RadComboBoxItem()
        {
            Value = ((int)FilteringType.Assignment).ToString(),
            Text = GetLocalResourceObject("ASRadComboBoxItemResource4.Text").ToString()
        });

        if (GeneralFuncsLib.HasAssignmentSubsite())
        {
            uxFilterType.Items.Add(new AS.Controls.Global.RadComboBoxItem()
            {
                Value = ((int)FilteringType.SubsiteAssignment).ToString(),
                Text = GetLocalResourceObject("SubAssignment.Text").ToString()
            });
        }

        uxFilterType.DataBind();

        if (!string.IsNullOrEmpty(SessionManager.AssignementFilteringOptions))
        {
            string[] parts = GeneralFuncsLib.NvlString(SessionManager.AssignementFilteringOptions).Split(';');
            var filterType = 0;
            if (parts.Length == 5 && Int32.TryParse(parts[0], out filterType))
            {
                uxFilterType.SelectedValue = filterType.ToString();
            }
        }
        else
        {
            uxFilterType.SelectedIndex = 0;
        }
    }


    private PostBackAction GetPostBackActionChangeDate()
    {
        return hddFieldTypeOfDateChange.Value == ManageAssignmentConstanst.EVENT_FIELD_START_DATE ? PostBackAction.ChangeStartDateTime : PostBackAction.ChangedExpirationTime;
    }

    private static void GetIconFutureStartDate(GridDataItem dataItem, DataRowView rowData)
    {
        var placeHolderStartDate = (System.Web.UI.WebControls.PlaceHolder)dataItem[ManageAssignmentConstanst.COLUMN_NAME_ASSIGNMENT_NAME].FindControl("uxIconFutureStartDate");
        placeHolderStartDate.Visible = ShowIconStartDate(rowData);
    }

    private static string GetFutureStartDateToExport(DataRow rowData)
    {
        var isShow = IsStartDateByAssignmentType(rowData);
        if (!isShow)
        {
            return string.Empty;
        }
        var startDateVal = GetValueInfoToDate(rowData, ManageAssignmentConstanst.COLUMN_NAME_START_DATE);
        return startDateVal != DateTime.MinValue && startDateVal > GetDateToNow() ? ManageAssignmentConstanst.VALUE_YES : ManageAssignmentConstanst.VALUE_NO;
    }

    private static string GetStartDateToExport(DataRow dataRow)
    {
        var isStartDate = IsStartDateByAssignmentType(dataRow);
        if (!isStartDate)
        {
            return null;
        }
        var startDateValue = GetValueInfoToDate(dataRow, ManageAssignmentConstanst.COLUMN_NAME_START_DATE);
        if (startDateValue == DateTime.MinValue)
        {
            return null;
        }
        return startDateValue.ToString(ManageAssignmentConstanst.FORMAT_DATE_MMDDYYYY);
    }

    private static bool ShowIconStartDate(DataRowView rowData)
    {
        var isShow = IsStartDateByAssignmentType(rowData);
        if (!isShow)
        {
            return false;
        }
        var startDateVal = GetValueInfoToDate(rowData, ManageAssignmentConstanst.COLUMN_NAME_START_DATE);
        return startDateVal != DateTime.MinValue && startDateVal > GetDateToNow();
    }
    private static DateTime GetValueInfoToDate(DataRowView rowView, string columnName)
    {
        var result = BusinessGeneralFuncsLib.GetValueDataRowView(rowView, columnName);
        if (string.IsNullOrEmpty(result))
        {
            return DateTime.MinValue;
        }
        return Convert.ToDateTime(result);
    }
    private static bool IsStartDateByAssignmentType(DataRow dataRow)
    {
        return GeneralFuncsLib.IsFieldStartDateOfRiskMgnt(dataRow[ManageAssignmentConstanst.COLUMN_NAME_ASSIGNMENT_TYPE].ToString());
    }
    private static bool IsStartDateByAssignmentType(DataRowView dataRow)
    {
        return GeneralFuncsLib.IsFieldStartDateOfRiskMgnt(dataRow[ManageAssignmentConstanst.COLUMN_NAME_ASSIGNMENT_TYPE].ToString());
    }
    private static DateTime GetValueInfoToDate(DataRow row, string columnName)
    {
        var isColumn = row.Table.Columns.Contains(columnName);
        if (!isColumn)
        {
            return DateTime.MinValue;
        }
        var result = BusinessGeneralFuncsLib.GetValueDataRow(row, columnName);
        if (string.IsNullOrEmpty(result))
        {
            return DateTime.MinValue;
        }
        return Convert.ToDateTime(result);
    }
    protected static DateTime GetDateToNow()
    {
        return BusinessGeneralFuncsLib.GetDateToNow().Date;
    }

    protected void btnOpenConfirmModal_Click(object sender, EventArgs e)
    {
        string hddAssignmentID = hddAssigmentDelete.Value;
        long assignmentID = 0;
        long.TryParse(hddAssignmentID, out assignmentID);
        DataTable tb = CheckDeleteAutoQueue(assignmentID);
        string msg = string.Empty;
        bool isDelete = false;
        if (tb != null && tb.Rows.Count > 0)
        {
            var rows = tb.Rows;
            string[] autoQueues = GetListAutoQueueByAssignmentName(rows).ToArray();
            string assignmentName = rows[0]["AssignmentName"].ToString();
            int assignmentType = rows[0]["AssignmentType"].ToInt();
            string templateMsg = assignmentType == 0
                ? GetLocalResourceObject("rm_Assignment_Management_js_DeleteAssignment_Confirm_Assignment").ToString()
                : GetLocalResourceObject("rm_Assignment_Management_js_DeleteAssignment_Confirm_WQ").ToString();
            msg = string.Format(templateMsg, assignmentName, string.Join(", ", autoQueues));
        }
        else
        {
            msg = GetLocalResourceObject("DeleteAssignmentConfirm").ToString();
            isDelete = true;
        }

        string queryString = BuildSecureQueryString(string.Format("MessageDelete={0}&isDelete={1}", msg, isDelete));
        AjaxAddResponseScript("ShowPopupModal('DeleteAssignmentModal.aspx?" + queryString + "','auto');");
    }
}


public class MyPager : WebControl
{
    public static string StrMode = string.Empty;

    private GridPagingManager paging;
    private GridTableView tableView;

    private AS.Controls.Global.RadNumericTextBox textBox;
    private AS.Controls.Global.LinkButton lbFirst;
    private AS.Controls.Global.LinkButton lbPrevious;
    private AS.Controls.Global.LinkButton lbNext;
    private AS.Controls.Global.LinkButton lbLast;
    private AS.Controls.Global.LinkButton lbGo;

    public MyPager(GridPagingManager paging, GridTableView tableView)
    {
        this.paging = paging;
        this.tableView = tableView;

        this.EnsureChildControls();
        this.Style.Add(HtmlTextWriterStyle.FontWeight, "bold");
    }
    protected override void CreateChildControls()
    {
        if (string.IsNullOrEmpty(MyPager.StrMode))
        {
            MyPager.StrMode = GetLocalResourceObject("rm_Assignment_Management_aspx_cs_users").ToString();
        }
        int rowCount = this.tableView.PagingManager.DataSourceCount;
        int pageCount = this.tableView.PageCount;
        int pageIndex = this.tableView.CurrentPageIndex;
        string First = GetLocalResourceObject("rm_Assignment_Management_aspx_cs_First").ToString();
        string Previous = GetLocalResourceObject("rm_Assignment_Management_aspx_cs_Previous").ToString();
        string Next = GetLocalResourceObject("rm_Assignment_Management_aspx_cs_Next").ToString();
        string Last = GetLocalResourceObject("rm_Assignment_Management_aspx_cs_Last").ToString();

        this.Controls.Add(new LiteralControl("<font color='black' size='2'>" + string.Format("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + GetLocalResourceObject("rm_Assignment_Management_aspx_cs_FoundPageOf").ToString() + "", rowCount, pageIndex + 1, pageCount, MyPager.StrMode) + "</font>"));

        this.Controls.Add(new LiteralControl("<br/>"));

        this.Controls.Add(new LiteralControl("<font color='black' size='2'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + GetLocalResourceObject("rm_Assignment_Management_aspx_cs_BrowserPages").ToString() + " </font>"));

        this.lbFirst = new AS.Controls.Global.LinkButton { ID = "glbFirst", Text = First, ForeColor = Color.Black };
        this.lbFirst.Font.Size = new FontUnit(10);
        this.lbFirst.Click += new EventHandler(LinkButton_Click);
        this.lbPrevious = new AS.Controls.Global.LinkButton { ID = "glbPrevious", Text = Previous, ForeColor = Color.Black };
        this.lbPrevious.Font.Size = new FontUnit(10);
        this.lbPrevious.Click += new EventHandler(LinkButton_Click);
        this.lbNext = new AS.Controls.Global.LinkButton { ID = "glbNext", Text = Next, ForeColor = Color.Black };
        this.lbNext.Font.Size = new FontUnit(10);
        this.lbNext.Click += new EventHandler(LinkButton_Click);
        this.lbLast = new AS.Controls.Global.LinkButton { ID = "glbLast", Text = Last, ForeColor = Color.Black };
        this.lbLast.Font.Size = new FontUnit(10);
        this.lbLast.Click += new EventHandler(LinkButton_Click);

        this.Controls.Add(this.lbFirst); this.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
        this.Controls.Add(this.lbPrevious); this.Controls.Add(new LiteralControl("&nbsp;&nbsp;&nbsp;&nbsp;"));

        this.textBox = new AS.Controls.Global.RadNumericTextBox
        {
            ID = "gtbGo",
            Text = VeraCodeSolution.DoVeraCode((pageIndex + 1).ToString()),
            MinValue = 1,
            MaxValue = pageCount,
            Width = Unit.Pixel(25)
        };
        textBox.ClientEvents.OnBlur = "textBox_Blur";
        textBox.Type = NumericType.Number;
        textBox.NumberFormat.DecimalDigits = 0;
        this.textBox.ClientEvents.OnKeyPress = "textBox_KeyPress";
        this.Controls.Add(textBox); this.Controls.Add(new LiteralControl("&nbsp;"));
        this.lbGo = new AS.Controls.Global.LinkButton { ID = "glbGo", Text = "Go", ForeColor = Color.Black };
        this.lbGo.Font.Size = new FontUnit(10);
        this.lbGo.Click += new EventHandler(this.LinkButton_Click);
        this.Controls.Add(lbGo); this.Controls.Add(new LiteralControl("&nbsp;&nbsp;&nbsp;&nbsp;"));

        this.Controls.Add(this.lbNext); this.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
        this.Controls.Add(this.lbLast);
    }
    private object GetLocalResourceObject(string p)
    {
        return HttpContext.GetLocalResourceObject(HttpContext.Current.Request.Path, p);
    }
    void textBox_TextChanged(object sender, EventArgs e)
    {
        if (this.textBox.Text.Trim().Length > 0)
            this.tableView.CurrentPageIndex = int.Parse(this.textBox.Text) - 1;
        this.tableView.Rebind();
    }
    void LinkButton_Click(object sender, EventArgs e)
    {
        AS.Controls.Global.LinkButton lb = (sender as AS.Controls.Global.LinkButton);
        switch (lb.ID)
        {
            case "glbFirst":
                this.tableView.CurrentPageIndex = 0;
                this.tableView.Rebind();
                break;
            case "glbPrevious":
                if (this.tableView.CurrentPageIndex > 0)
                    this.tableView.CurrentPageIndex--;
                this.tableView.Rebind();
                break;
            case "glbNext":
                if (this.tableView.CurrentPageIndex < this.tableView.PageCount - 1)
                    this.tableView.CurrentPageIndex++;
                this.tableView.Rebind();
                break;
            case "glbLast":
                this.tableView.CurrentPageIndex = this.tableView.PageCount - 1;
                this.tableView.Rebind();
                break;
            case "glbGo":
                if (this.textBox.Text.Trim().Length > 0)
                    this.tableView.CurrentPageIndex = int.Parse(this.textBox.Text) - 1;
                this.tableView.Rebind();
                break;
            default:
                break;
        }

    }
}
public static class RiskExporterAs
{
    public static StringBuilder GetContentCSV(List<string> headers, List<DataTable> tables,
    string exportColNames, string exportNewColNames)
    {

        StringBuilder content = new StringBuilder();
        //write content
        var exportCols = exportColNames.Split(',');
        for (int i = 0; i < exportCols.Length; i++)
            exportCols[i] = exportCols[i].Trim();

        for (int i = 0; i < tables.Count; i++)
        {
            var table = tables[i];
            //write header
            content.Append("\"" + HttpUtility.HtmlDecode(headers[i]) + "\"");
            content.Append(Environment.NewLine);
            //write header columns
            foreach (var col in exportNewColNames.Split(','))
                content.Append(col.Trim() + ",");
            content = content.Remove(content.Length - 1, 1);
            content.Append(Environment.NewLine);
            //write content
            foreach (DataRow r in table.Rows)
            {
                foreach (string colName in exportCols)
                {
                    DataColumn col = table.Columns[colName];
                    if (col.DataType == Type.GetType("System.Int32"))
                    {
                        ProcessIntegerType(content, r, col, true);
                    }
                    else if (col.DataType == Type.GetType("System.Decimal"))
                    {
                        ProcessDecimalType(content, r, col, true);
                    }
                    else if (col.DataType == Type.GetType("System.DateTime"))
                    {

                        if (((DateTime)r["ExpirationDate"]).Year == 2999 && colName.Equals("ExpirationDate"))
                        {
                            ProcessStringType(content, "Never Expire");
                        }
                        else
                        {
                            ProcessDateTimeType(content, r, col, true);
                        }

                    }
                    else if (col.DataType == Type.GetType("System.Double") || (col.DataType == Type.GetType("System.Single")))
                    {
                        ProcessFloatType(content, r, col, true);

                    }
                    else
                    {
                        ProcessStringType(content, r, col, true);
                    }
                }//end col process
                content = content.Remove(content.Length - 1, 1);
                content = content.Append(Environment.NewLine);
            }
            content.Append(Environment.NewLine);
        }

        return content;
    }
    /// <summary>
    /// Write result string to response stream 
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="htmlBuilder"></param>
    /// <param name="excelOrWord"></param>
    private static void ProcessStringType(StringBuilder content, DataRow r, DataColumn col, bool csvOrOffice)
    {
        try
        {
            content.Append("\"" + r[col.ColumnName].ToString() + "\",");
        }
        catch
        {
            content.Append("NULL,");
        }
    }

    private static void ProcessStringType(StringBuilder content, string data)
    {
        try
        {
            content.Append("\"" + data + "\",");
        }
        catch
        {
            content.Append("NULL,");
        }
    }

    private static void ProcessIntegerType(StringBuilder content, DataRow r, DataColumn col, bool csvOrOffice)
    {
        try
        {
            int val_int = int.Parse(r[col.ColumnName].ToString());
            content.Append("\"" + val_int.ToString("#,#0") + "\",");
        }
        catch
        {
            content.Append(",");
        }
    }
    private static void ProcessFloatType(StringBuilder content, DataRow r, DataColumn col, bool csvOrOffice)
    {
        try
        {
            content.Append("\"" + Convert.ToDouble(r[col.ColumnName].ToString()) + "\",");
        }
        catch
        {
            content.Append(",");
        }
    }
    private static void ProcessDateTimeType(StringBuilder content, DataRow r, DataColumn col, bool csvOrOffice)
    {
        try
        {
            if (r[col.ColumnName].ToString() == string.Empty)
            {
                content.Append(",");
            }
            else if (col.ColumnName == "ProcessingStatusDate")
                content.Append("\"" + Convert.ToDateTime(r[col.ColumnName].ToString()).ToString("MM/dd/yyyy hh:mm:ss tt") + "\",");
            else
            {
                content.Append("\"" + Convert.ToDateTime(r[col.ColumnName].ToString()).ToShortDateString() + "\",");
            }
        }
        catch
        {
            content.Append(",");
        }
    }
    private static void ProcessDecimalType(StringBuilder content, DataRow r, DataColumn col, bool csvOrOffice)
    {
        try
        {
            if (col.ColumnName == "CompletePercent")
            {
                content.Append("\"" + ((decimal)r[col.ColumnName]).ToString().Replace(",", "") + "%" + "\",");
            }
            else
            {
                content.Append("\"" + FormatData.FormatCurrency(r[col.ColumnName], SessionManager.CurrencyFortmat).Replace(",", "").Replace("(", "-").Replace(")", "") + "\",");
            }
        }
        catch
        {
            content.Append(",");
        }
    }
}