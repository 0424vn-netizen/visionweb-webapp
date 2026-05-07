using AS.Common;
using AS.Common.DBManager;
using AS.Common.Logger;
using AS.Controls.Exporter;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Controls.UserControls;
using AS.Web.Business;
using AS.Web.UI.AppCode.General;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;


[PagePermission("RskRP,MSRskRP")]
public partial class rm_MCF_RiskReport : ReportPage
{

    public enum DataBindAction
    {
        BindBatchHistoryGrid,
        BindEscalationHistory,
        BindEscalationStatus,
        BindEscalationUser,
        BindResolution,
        BindRiskMerchantComment,
        BindReasonList,
        BindReasonRequired,
        BindChargeBack
    }

    enum PostBackAction
    {
        SearchMerchantEvent,
        InvestigateSubmitEvent,
        ProcessEvent,
        AddNote,
        AccountLink
    }

    enum DataBindActionInUC
    {
        BindSourceList,
        BindRoleList,
        BindAddedByList,
        BindGridMerchantNote,
    }

    #region Fields

    #region Sales Grid

    private const string CURRENCY_FORMAT = "<span style =\"padding-right:0px;\" class=\"ReportTotal\">{0:C}</span>";
    private const string CURRENCY_FORMAT_NEG = "<span style =\"padding-right:0px;\" class=\"ReportTotal\"><font color='red'>{0:C}</font></span>";
    private const string PERCENT_FORMAT_4 = "<span style =\"padding-right:0px;\" class=\"ReportTotal\">{0:#,0.0000}%</span>";
    private const string AVERAGE_SALES = "AverageSales";
    private const string PERCENT_RETURN_COUNT = "PercentReturnCount";
    private const string PERCENT_RETURN_AMOUNT = "PercentReturnAmount";
    private const string AVERAGE_RETURNS = "AverageReturns";
    private const string PERCENT_CHARGE_BACK_COUNT = "PercentChargebackCount";
    private const string PERCENT_CHARGE_BACK_AMOUNT = "PercentChargebackAmount";
    private const string AVERAGE_CHARGE_BACK = "AverageChargeback";
    private const string PERCENT_FOREIGN_CARD_COUNT = "PercentForeignCardCount";
    private const string PERCENT_FOREIGN_CARD_AMOUNT = "PercentForeignCardAmount";

    private int _CountReturns = 0;
    private double _TotalReturns = 0.0;
    private int _CountSales = 0;
    private double _TotalSales = 0.0;
    private int _CountChargeback = 0;
    private double _TotalChargebackAmount = 0.0;
    private int _CountForeignCard = 0;
    private double _TotalForeignCardAmount = 0.0;

    #endregion Sales Grid

    #region SwipeKeyRate

    private const string PERCENT_SWIPED_COUNT = "PercentSwipedCount";
    private const string PERCENT_SWIPED_AMOUNT = "PercentSwipedAmount";
    private const string PERCENT_KEYED_COUNT = "PercentKeyedCount";
    private const string PERCENT_KEYED_AMOUNT = "PercentKeyedAmount";

    private int _SwipedCount = 0;
    private double _SwipedAmount = 0.0;
    private int _KeyedCount = 0;
    private double _KeyedAmount = 0.0;

    #endregion SwipeKeyRate

    #region Authorization

    private const string PERCENT_APPROVED_AUTHORIZATION_COUNT = "PercentApprovedAuthorizationCount";
    private const string PERCENT_APPROVED_AUTHORIZATION_AMOUNT = "PercentApprovedAuthorizationAmount";
    private const string PERCENT_DECLINED_AUTHORIZATION_AMOUNT = "PercentDeclinedAuthorizationAmount";
    private const string PERCENT_DECLINED_AUTHORIZATION_COUNT = "PercentDeclinedAuthorizationCount";

    private double _DeclinedAuthorizationAmount = 0.0;
    private int _DeclinedAuthorizationCount = 0;
    private int _AuthorizationCount = 0;
    private double _AuthorizationAmount = 0.0;
    private int _ApprovedAuthorizationCount = 0;
    private double _ApprovedAuthorizationAmount = 0.0;

    #endregion Authorization

    #region BatchHistory

    private const string ITEM_NUMBER_FORMAT = "{0:#,0}";
    private const string ITEM_NUMBER_FORMAT_NEG = "<font color='red'>{0:#,0}</font>";
    private const string ITEM_CURRENCY_FORMAT = "{0:C}";
    private const string ITEM_CURRENCY_FORMAT_NEG = "<font color='red'>{0:C}</font>";
    private const string FOOTER_NUMBER_FORMAT = "<span class=\"ReportTotal\">{0:#,0}</span>";
    private const string FOOTER_NUMBER_FORMAT_NEG = "<span class=\"ReportTotal\"><font color='red'>{0:#,0}</font></span>";
    private const string FOOTER_CURRENCY_FORMAT = "<span class=\"ReportTotal\">{0:C}</span>";
    private const string FOOTER_CURRENCY_FORMAT_NEG = "<span class=\"ReportTotal\"><font color='red'>{0:C}</font></span>";

    private const string BATCH_SUMMARY = "BatchSummary";
    private const string BANK_CARD = "BankCard";
    private const string NON_BANK_CARD = "NonBankCard";
    private const string TOTAL_BANK_CARD_SALES_AMOUNT = "TotalBankCardSalesAmount";
    private const string TOTAL_BANK_CARD_RETURNS_AMOUNT = "TotalBankCardReturnsAmount";
    private const string TOTAL_BANK_CARD_NET_AMOUNT = "TotalBankCardNetAmount";
    private const string TOTAL_NON_BANK_CARD_SALES_AMOUNT = "TotalNonBankCardSalesAmount";
    private const string TOTAL_NON_BANK_CARD_RETURNS_AMOUNT = "TotalNonBankCardReturnsAmount";
    private const string TOTAL_NON_BANK_CARD_NET_AMOUNT = "TotalNonBankCardNetAmount";
    private const string TOTAL_TRANSACTION_COUNT = "TotalTransactionCount";
    private const string TOTAL_KEYED_ENTRY_COUNT = "TotalKeyedEntryCount";
    private const string TOTAL_SALE_AMOUNT = "TotalSaleAmount";
    private const string TOTAL_RETURN_AMOUNT = "TotalReturnAmount";
    private const string TOTAL_NET_AMOUNT = "TotalNetAmount";

    private const string UX_BANK_CARD_SALES = "uxBankCardSales";
    private const string UX_BANK_CARD_RETURNS = "uxBankCardReturns";
    private const string UX_BANK_CARD_NET = "uxBankCardNet";
    private const string UX_NON_BANK_CARD_SALES = "uxNonBankCardSales";
    private const string UX_NON_BANK_CARD_RETURNS = "uxNonBankCardReturns";
    private const string UX_NON_BANK_CARD_NET = "uxNonBankCardNet";

    private string _CurrentSortExpr = string.Empty;
    private string _CurrentSortOrder = string.Empty;
    private string _CurrentSortControls = string.Empty;
    private string _BatchDetailIntruderQuery = string.Empty;
    private string _IntruderQuery = string.Empty;

    protected string CurrentFollowUpdateDate = string.Empty;
    protected string IsCheckFollowUpdate = "false";
    private bool IsExport;
    private string BatchDetailIntruderQuery
    {
        get
        {
            if (_BatchDetailIntruderQuery == string.Empty) _BatchDetailIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(uxBatchHistoryGrid.ID, new string[] { "BatchNumber", "TerminalNumber" });
            return _BatchDetailIntruderQuery;
        }
    }

    private string IntruderQuery
    {
        get
        {
            if (_IntruderQuery == string.Empty) _IntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(uxBatchHistoryGrid.IntruderSourceName, new string[] { "BatchNumber" });
            return _IntruderQuery;
        }
    }

    protected bool IsRequiredReason
    {
        get;
        set;

    }
    private bool IsCSViewFullCard
    {
        get
        {
            return GeneralFuncsLib.CheckCSViewFullCard((SecurePage)Page);
        }
    }
    #endregion BatchHistory

    #endregion Fields

    #region Constants

    private const string PERCENT_FORMAT =
        "<span style =\"padding-right:0px;\" class=\"ReportTotal\">{0:#,0.00}%</span>";
    private const string ESCALATION_ID = "EscalationID";
    private const string MERCHANT_NUMBER = "MerchantNumber";
    private const string CS_ADD_MANAGEMENT_COMMENT_PERMISSION_CODE = "AddManagementComment";
    private const string MS_ADD_MANAGEMENT_COMMENT_PERMISSION_CODE = "MSAddManagementComment";
    private const string CS_VIEW_MANAGEMENT_COMMENT_PERMISSION_CODE = "ViewManagementComment";
    private const string MS_VIEW_MANAGEMENT_COMMENT_PERMISSION_CODE = "MSViewManagementComment";
    private const int VIEW_ALL_RISK_COMMENT = 0;
    private const int VIEW_RISK_MANAGEMENT_COMMENT = 1;
    private const int VIEW_RISK_NORMAL_COMMENT = 2;

    private const string BACK_END_PROCESSOR = "BackEndProcessor";
    private const string SPA_GET_RESOLUTION = "spa_RM_MCF_GetResolution";
    private const string SPA_GET_ESCALATION_REASON = "spa_RM_MCF_GetEscalationReason";
    private const string SPA_GET_ESCALATION_CONFIG = "spa_RM_MCF_GetEscalationReasonConfig";
    private const string SPA_GET_ESCALATION_USERS = "spa_RM_MCF_GetEscalationUsers";
    private const string SPA_GET_ESCALATION_STATUS = "spa_RM_MCF_GetEscalationStatus";
    private const string SPA_GET_BATCH_HISTORY = "spa_RM_MCF_RiskReport_GetBatchHistory";
    private const string SPA_GET_ESCALATION_HISTORY = "spa_RM_MCF_GetEscalationHistory";
    private const string MERCHANT_PROFILE_PAGE = "MerchantProfile.aspx";
    private const string RISK_REPORT_PAGE = "RiskReport.aspx";
    #endregion Constants

    #region Fields

    private string _EscalationHistoryIntruderQuery = string.Empty;

    #endregion Fields

    #region Properties

    private string EscalationHistoryIntruderQuery
    {
        get
        {
            if (_EscalationHistoryIntruderQuery.Length == 0)
                _EscalationHistoryIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(this.ID, new string[] { ESCALATION_ID });
            return _EscalationHistoryIntruderQuery;
        }
    }

    public string MerchantNumber
    {
        get
        {
            return GeneralFuncsLib.NvlString(ViewState[MERCHANT_NUMBER]);
        }
        set
        {
            ViewState[MERCHANT_NUMBER] = value;
        }
    }


    public List<DataSourceParallelResponse> RiskReportDataSource { get; set; }

    #endregion Properties

    #region Methods

    #region Base overrides

    protected override void PageInitialize()
    {
        if (IsIntruderDetected) return;
        this.GridIDs.Add("uxSalesDataGrid");
        this.ExporterIDs.Add("uxExportSales");
        this.GridIDs.Add("uxSwipeKeyRateDataGrid");
        this.ExporterIDs.Add("uxExportSwipeKeyRate");
        this.GridIDs.Add("uxAuthorizationDataGrid");
        this.ExporterIDs.Add("uxExportAuthorization");
        this.GridIDs.Add("uxBatchHistoryGrid");
        this.ExporterIDs.Add("uxExportBatchHistory");
        this.GridIDs.Add("uxEscHistoryGrid");
        this.ExporterIDs.Add("uxExportEscHistory");
        //TK39919 - Remove uxRiskComments
        this.ExporterIDs.Add("uxRiskCommentsExport");

        this.GridIDs.Add("uxChargebacks");

        base.PageInitialize();
    }

    protected override void DoIntruderDetected(IntruderType type)
    {
        var langugeControl = this.Master.FindControl("drpLanguage");
        if (langugeControl != null)
        {
            if (Request.Form[langugeControl.UniqueID] != null)
            {
                // Ignore Intruder
            }
            else
            {
                base.DoIntruderDetected(type);
            }
        }
    }

    protected int isScroll = 0;
    #endregion

    #region Grid events

    protected override void DoGridNeedDataSource(ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        if (sender == uxBatchHistoryGrid)
        {
            OnDataBindControls(DataBindAction.BindBatchHistoryGrid, sender);

        }
        else if (sender == uxEscHistoryGrid)
        {
            OnDataBindControls(DataBindAction.BindEscalationHistory, sender);
        }
    }
    protected override void DoItemDataBound(ASGrid sender, GridItemEventArgs e)
    {
        if (sender == uxBatchHistoryGrid)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = e.Item as GridDataItem;
                DataRowView dataRow = e.Item.DataItem as DataRowView;
                if (!SessionManager.CurrentUser.ASClient.Equals(WebSiteConstants.IPMT_CLIENT))
                {
                    dataItem["BatchNumber"].Text = "<a class=\"link\" href=\"javascript:void(0)\" onclick=\"getLinkBatchHistory('" + dataRow["TerminalNumber"].ToString()
                                                    + "','" + ((DateTime)dataRow["ReportDate"]).ToShortDateString() + "',this);\">" + dataRow["BatchNumber"].ToString() + "</a>";
                }
            }
        }
        else if (sender == uxEscHistoryGrid && e.Item is GridDataItem)
        {
            DataTable dt = (DataTable)uxEscHistoryGrid.DataSource;
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView dataRow = e.Item.DataItem as DataRowView;
            string escalationID = GeneralFuncsLib.NvlString(dataRow[ESCALATION_ID]);
            string prevEscalationID = null;

            string url = string.Format("<a class=\"link\" href=\"#\" style=\"cursor:pointer\" onclick=\"escalation_Click('{0}');\">{0}</a>", dataRow[ESCALATION_ID]);
            dataItem[ESCALATION_ID].Text = VeraCodeSolution.GetOutputHtmlString(url);


            if (dataItem.ItemIndex > 0)
            {
                prevEscalationID = GeneralFuncsLib.NvlString(dt.Rows[dataItem.ItemIndex - 1][ESCALATION_ID]);
                if (escalationID == prevEscalationID)
                {
                    dataItem[ESCALATION_ID].Text = "&nbsp;";
                    dataItem["EscalationDate"].Text = "&nbsp;";
                    dataItem["Closed"].Text = "&nbsp;";
                }
            }
        }
    }
    #endregion

    protected override void DoNeedExportConfig(UxExport sender, ExportConfig exportConfig)
    {
        if (sender == uxExportBatchHistory)
        {
            if (string.IsNullOrEmpty(uxBatchHistoryGrid.AS_SortExpression))
                uxBatchHistoryGrid.AS_SortExpression = "ReportDate DESC";

            for (int i = 6; i < uxBatchHistoryGrid.Columns.Count; i++)
            {
                uxBatchHistoryGrid.Columns[i].HeaderText = uxBatchHistoryGrid.Columns[i].HeaderTooltip;
            }
            base.DoNeedExportConfig(sender, exportConfig);
            string fileName = GeneralFuncsLib.FormatFileName(uxBatchHistoryGrid.GridName);
            exportConfig.FileName = HttpUtility.UrlEncode(fileName);

            IsExport = true;
        }
        else if (sender == uxExportEscHistory)
        {
            base.DoNeedExportConfig(sender, exportConfig);
            exportConfig.FileName = GeneralFuncsLib.GetLegalFileName(uxExportEscHistory.FileName);
        }
    }

    protected override void OnPostBackActions(Enum type, object param)
    {
        if (this.IsIntruderDetected) return;

        switch ((PostBackAction)type)
        {
            case PostBackAction.SearchMerchantEvent:
                {
                    if (uxMerchantNumber.Text.Trim().Length > 0)
                    {
                        //Clear drop downlist if user paste into textbox
                        if (uxMerchantList.SelectedValue != uxMerchantNumber.Text.Trim())
                        {
                            uxMerchantList.Items.Clear();
                            uxMerchantList.Text = string.Empty;
                        }

                        this.MerchantNumber = VeraCodeSolution.DoVeraCode(uxMerchantNumber.Text.Trim());

                        InitSettings();
                    }
                }
                break;
            case PostBackAction.ProcessEvent:
                {
                    string[] parms = hddProcessData.Value.Split(';');
                    if (parms.Length > 1 && string.Compare(parms[0], "escalation") == 0)
                    {
                        string escalationNumber = parms[1];
                        bool isPopup = false;
                        if (IsSecureQueryString)
                        {
                            Boolean.TryParse(base.SecureQueryString["IsPopup"], out isPopup);
                        }
                        else
                        {
                            RiskSessionManager.EscalationQueueReferrer = "RiskReport";
                            RiskSessionManager.EscalationQueueReferrerInfo =
                                new ReferrerInfo(escalationNumber, "~/risk_MCF/rm_MCF_RiskReport.aspx", GetLocalResourceObject("PageResource1.Title").ToString());
                        }
                        string queryString = BuildSecureQueryString(
                            string.Format("{0}={1}&{2}={3}{4}",
                            ESCALATION_ID, escalationNumber,
                            "IsPopup", isPopup,
                            this.EscalationHistoryIntruderQuery));
                        Response.Redirect("~/risk_MCF/rm_MCF_EscalationHistory.aspx?" + queryString, true);
                    }
                }
                break;
            case PostBackAction.InvestigateSubmitEvent:
                {
                    int escalationID = 0;
                    string merchantNumber = this.MerchantNumber;
                    int status = 0;
                    int resolution = 0;
                    int reasonid = 0;

                    bool isValidation = true;

                    if (uxIsFollowupDate.Checked && !uxCloseInvestigation.Checked
                        && uxFollowUpdate.SelectedDate < DateTime.Now.Date)
                    {
                        isValidation = false;
                        uxAjaxpnl.ResponseScripts.Add("alert('" + GetLocalResourceObject("rm_RiskReport_aspx_cs_FollowUpDate").ToString() + "');");
                    }

                    if (isValidation)
                    {
                        if (!Int32.TryParse(uxStatusList.SelectedValue, out status)) return;

                        Int32.TryParse(GeneralFuncsLib.NvlString(ViewState[ESCALATION_ID]), out escalationID);

                        int.TryParse(uxReasonList.SelectedValue, out reasonid);

                        string activityText = "Risk Report: Ticket {0}, Status: {1}, Assigned to: {2}";
                        if (escalationID > 0)
                        {
                            Int32.TryParse(uxResolutionList.SelectedValue, out resolution);

                            UpdateEscalation(escalationID, uxCloseInvestigation.Checked,
                                uxAssignedToList.SelectedValue, status, resolution, VeraCodeSolution.DoVeraCode(uxComments.Text), reasonid, uxFollowUpdate.SelectedDate, uxSavingsLoss.Value);
                            activityText = string.Format(activityText, "changed", uxStatusList.SelectedItem.Text, uxAssignedToList.SelectedItem.Text) + (uxCloseInvestigation.Checked ? ", Closed" : "");
                        }
                        else
                        {
                            if (merchantNumber.Length == 0) return;
                            AddEscalation(SessionManager.CurrentUser.ASClient, merchantNumber,
                                uxAssignedToList.SelectedValue, status, VeraCodeSolution.DoVeraCode(uxComments.Text),
                                SessionManager.CurrentUser.UserID.ToString(), reasonid, uxFollowUpdate.SelectedDate, uxSavingsLoss.Value);
                            activityText = string.Format(activityText, "created", uxStatusList.SelectedItem.Text, uxAssignedToList.SelectedItem.Text);
                        }
                        // Save User Activity
                        GeneralFuncsLib.SaveUserActivity(this.MerchantNumber, activityText, true);
                        RiskSessionManager.IsSCroll = 2;
                        uxComments.Text = String.Empty;
                        uxEscHistoryGrid.Rebind();
                        BindInvestigationForm();
                    }
                }
                break;
            case PostBackAction.AddNote:

                FilterParameterCollection parameters = new FilterParameterCollection();
                FilterParameterCollection outParameters = new FilterParameterCollection();
                parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                parameters.Add(new FilterParameter("@NoteSourceId", NoteSourceDefaultForIns, DbType.Int32));
                parameters.Add(new FilterParameter("@MerchantNumber", MerchantNumber, DbType.AnsiString));
                parameters.Add(new FilterParameter("@Comment", EncryptComment(uxComment.Content), DbType.AnsiString));
                parameters.Add(new FilterParameter("@CommentPlainText", EncryptComment(uxComment.Text.Replace("\n", "")), DbType.String));
                parameters.Add(new FilterParameter("@CommentType", null, DbType.AnsiString));

                WebServices.CsReportServices.ExecuteNonQueryCommand("spa_MerchantNotes_InsNote", parameters, out outParameters);
                uxComment.Content = string.Empty;

                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "rebindRoleAndUser", "rebindNoteFromRiskReport()", true);
                break;
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        if (this.IsIntruderDetected || string.IsNullOrEmpty(this.MerchantNumber)) return;
        FilterParameterCollection parameters = new FilterParameterCollection();

        switch ((DataBindAction)type)
        {
            case DataBindAction.BindBatchHistoryGrid:
                {
                    ASGrid grid = (ASGrid)sender;
                    if (IsPostBack)
                    {
                        this.AjaxAddResponseScript("doAdjustHeader();");
                    }

                    DataTable bindBatchHistory = null;
                    if (RiskReportDataSource.IsNotNullData())
                    {
                        var data = RiskReportDataSource.SingleOrDefault(m => m.FeatureName == DataBindAction.BindBatchHistoryGrid.ToString());
                        if (data.IsNotNullData())
                            bindBatchHistory = data.DataSource;
                    }

                    if (bindBatchHistory.IsNotNullData() && !IsExport)
                    {
                        DataTable dtHistoryGrid = bindBatchHistory;

                        if (dtHistoryGrid != null && dtHistoryGrid.Rows.Count > 0 && dtHistoryGrid.Columns.Contains("TotalRows"))
                            grid.VirtualItemCount = Convert.ToInt32(dtHistoryGrid.Rows[0]["TotalRows"].ToString());

                        grid.MasterTableView.AllowCustomPaging = true;

                        if (dtHistoryGrid != null && dtHistoryGrid.Rows.Count > 0)
                            grid.MasterTableView.ShowFooter = true;
                        grid.DataSource = dtHistoryGrid;
                    }
                    else
                    {
                        parameters.AddLoggedInUserReportingParams(false);
                        parameters.Add(new FilterParameter("@ReportDate", DateTime.Today, DbType.Date));
                        parameters.Add(new FilterParameter("@MerchantNumber", this.MerchantNumber, DbType.String));
                        parameters.Add(new FilterParameter("@DateRange", RiskSessionManager.BatchDateRangeModal, DbType.Int32));
                        grid.DataSourceInvoker = new ASFuncInvoker(WebServices.RiskServices, "GetReports", new object[] { SPA_GET_BATCH_HISTORY, ReportServices.ConvertToFilterParamWSArray(parameters) });
                    }
                }
                break;
            case DataBindAction.BindEscalationHistory:
                {
                    DataTable table;
                    DataTable bindEscalation = null;
                    if (RiskReportDataSource.IsNotNullData())
                    {
                        var data = RiskReportDataSource.SingleOrDefault(m => m.FeatureName == DataBindAction.BindEscalationHistory.ToString());
                        if (data.IsNotNullData())
                            bindEscalation = data.DataSource;
                    }

                    if (bindEscalation.IsNotNullData())
                    {
                        table = bindEscalation;
                    }
                    else
                    {
                        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                        parameters.Add(new FilterParameter("@EscalationID", 0, DbType.Int32));
                        parameters.Add(new FilterParameter("@MerchantNumber", this.MerchantNumber, DbType.String));
                        table = WebServices.RiskServices.GetReports(SPA_GET_ESCALATION_HISTORY, parameters);
                    }
                    uxEscHistoryGrid.DataSource = table;

                }
                break;
            case DataBindAction.BindRiskMerchantComment:
                {
                    int commentType = VIEW_RISK_NORMAL_COMMENT;
                    if (IsUserWithPermission(CS_ADD_MANAGEMENT_COMMENT_PERMISSION_CODE) || IsUserWithPermission(MS_ADD_MANAGEMENT_COMMENT_PERMISSION_CODE) || IsUserWithPermission(CS_VIEW_MANAGEMENT_COMMENT_PERMISSION_CODE) || IsUserWithPermission(MS_VIEW_MANAGEMENT_COMMENT_PERMISSION_CODE))
                    {
                        commentType = VIEW_ALL_RISK_COMMENT;
                    }
                    ASGrid grid = (ASGrid)sender;
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    if (SessionManager.CurrentUser.EntityType > 0 && !string.IsNullOrEmpty(SessionManager.CurrentUser.EntityID))
                    {
                        parameters.Add(new FilterParameter("@EntityType", SessionManager.CurrentUser.EntityType, DbType.Int32));
                        parameters.Add(new FilterParameter("@EntityNumber", SessionManager.CurrentUser.EntityID, DbType.AnsiString));
                    }
                    parameters.Add(new FilterParameter("@CommentType", commentType, DbType.Int32));
                    parameters.Add(new FilterParameter("@MerchantNumber", this.MerchantNumber, DbType.String));
                    grid.DataSourceInvoker = new ASFuncInvoker(WebServices.RiskServices, "GetReports", new object[] { "spa_RM_MCF_GetCommentsOfMerchant", ReportServices.ConvertToFilterParamWSArray(parameters) });
                }
                break;
            case DataBindAction.BindEscalationStatus:
                {
                    RadComboBox cbx = (RadComboBox)sender;

                    RadComboBoxItem item = new RadComboBoxItem(string.Empty);
                    item.Height = Unit.Pixel(12);

                    DataTable table;
                    DataTable bindEscalationStatus = null;

                    if (RiskReportDataSource.IsNotNullData())
                    {
                        var data = RiskReportDataSource.SingleOrDefault(m => m.FeatureName == DataBindAction.BindEscalationStatus.ToString());
                        if (data.IsNotNullData())
                            bindEscalationStatus = data.DataSource;
                    }

                    if (bindEscalationStatus.IsNotNullData())
                    {
                        table = bindEscalationStatus;
                    }
                    else
                    {
                        //Call EscalationStatus
                        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                        parameters.Add(new FilterParameter("@IsActive", 1, DbType.Int32));
                        parameters.Add(new FilterParameter("@SortOrder", " Status ASC", DbType.String));
                        table = WebServices.RiskServices.GetReports(SPA_GET_ESCALATION_STATUS, parameters);
                    }
                    cbx.DataSource = table;
                    cbx.DataBind();
                    cbx.Items.Insert(0, item);
                }
                break;
            case DataBindAction.BindEscalationUser:
                {
                    RadComboBox cbx = (RadComboBox)sender;

                    RadComboBoxItem item = new RadComboBoxItem(string.Empty);
                    item.Height = Unit.Pixel(12);

                    DataTable table;
                    DataTable bindEscalationUser = null;
                    if (RiskReportDataSource.IsNotNullData())
                    {
                        var data = RiskReportDataSource.SingleOrDefault(m => m.FeatureName == DataBindAction.BindEscalationUser.ToString());
                        if (data.IsNotNullData())
                            bindEscalationUser = data.DataSource;
                    }

                    if (bindEscalationUser.IsNotNullData())
                    {
                        table = bindEscalationUser;
                    }
                    else
                    {
                        parameters.Clear();
                        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                        parameters.Add(new FilterParameter("@Mode", 0, DbType.Int32));
                        table = WebServices.RiskServices.GetReports(SPA_GET_ESCALATION_USERS, parameters);
                    }

                    cbx.DataSource = GetRefValue(table);
                    if (table != null && table.Rows.Count > 10)
                    {
                        cbx.Height = Unit.Pixel(200);
                        cbx.MaxHeight = Unit.Pixel(200);
                    }

                    cbx.DataBind();
                    cbx.Items.Insert(0, item);
                }
                break;
            case DataBindAction.BindResolution:
                {
                    RadComboBox cbx = (RadComboBox)sender;

                    RadComboBoxItem item = new RadComboBoxItem(string.Empty);
                    item.Height = Unit.Pixel(12);
                    DataTable table;
                    DataTable bindResolution = null;
                    if (RiskReportDataSource.IsNotNullData())
                    {
                        var data = RiskReportDataSource.SingleOrDefault(m => m.FeatureName == DataBindAction.BindResolution.ToString());
                        if (data.IsNotNullData())
                            bindResolution = data.DataSource;
                    }

                    if (bindResolution.IsNotNullData())
                    {
                        table = bindResolution;
                    }
                    else
                    {
                        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                        parameters.Add(new FilterParameter("@IsActive", 1, DbType.Int32));
                        parameters.Add(new FilterParameter("@SortOrder", " Resolution ASC", DbType.String));
                        table = WebServices.RiskServices.GetReports(SPA_GET_RESOLUTION, parameters);
                    }
                    cbx.DataSource = table;
                    cbx.DataBind();
                    cbx.Items.Insert(0, item);
                }
                break;
            case DataBindAction.BindReasonList:
                {
                    DataTable table;
                    DataTable bindReasonList = null;
                    if (RiskReportDataSource.IsNotNullData())
                    {
                        var data = RiskReportDataSource.SingleOrDefault(m => m.FeatureName == DataBindAction.BindReasonList.ToString());
                        if (data.IsNotNullData())
                            bindReasonList = data.DataSource;
                    }

                    if (bindReasonList.IsNotNullData())
                    {
                        table = bindReasonList;
                    }
                    else
                    {
                        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                        parameters.Add(new FilterParameter("@IsActive", 1, DbType.Int32));
                        table = WebServices.RiskServices.GetReports(SPA_GET_ESCALATION_REASON, parameters);
                    }
                    uxReasonList.DataSource = table;
                    uxReasonList.DataBind();
                    uxReasonList.Items.Insert(0, new RadComboBoxItem("\u00A0", "0"));

                }
                break;
            case DataBindAction.BindReasonRequired:
                {
                    DataTable table;
                    DataTable bindReason = null;
                    if (RiskReportDataSource.IsNotNullData())
                    {
                        var data = RiskReportDataSource.SingleOrDefault(m => m.FeatureName == DataBindAction.BindReasonRequired.ToString());
                        if (data.IsNotNullData())
                            bindReason = data.DataSource;
                    }

                    if (bindReason.IsNotNullData())
                    {
                        table = bindReason;
                    }
                    else
                    {
                        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                        table = WebServices.RiskServices.GetReports(SPA_GET_ESCALATION_CONFIG, parameters);
                    }
                    if (table == null || table.Rows.Count == 0)
                        IsRequiredReason = false;
                    else
                    {
                        IsRequiredReason = table.Rows[0][0].ToBoolean();
                    }
                    break;
                }
        }
    }

    protected void uxIsFollowupDate_Onchange(object sender, EventArgs e)
    {
        if (uxIsFollowupDate.Checked)
            uxFollowUpdate.Visible = true;
        else
            uxFollowUpdate.Visible = false;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            IsBindDataOnLoad = true;

            if (IsSecureQueryString)
            {
                bool isPopup = false;
                Boolean.TryParse(base.SecureQueryString["IsPopup"], out isPopup);
                if (isPopup)
                {
                    ((MasterPageNormal)Page.Master).HideHeaderMenu = true;
                }
            }

            bool isShowSavingLossCol = GeneralFuncsLib.GetDataOfExtendedSetting("GainLossInvestigationAmount").ToLower().Equals("true");
            uxSavingsLossInvestigation.Visible = isShowSavingLossCol;
            if (((ReportPage)Page).IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_CM_OPEN_CASE))
            {
                uxbtnOpenNewCase3.Visible = true;
                string url = ResolveUrl("~/JumpToCase.aspx?") + string.Format("type=6");
                uxbtnOpenNewCase3.Attributes.Add("onclick", string.Format("parent.openPopupWindowOnMenu(event,'{0}','{1}'); return false;", url, "OpenNewCase"));
            }
            if (isShowSavingLossCol)
            {
                GridColumn colSavingLoss = uxEscHistoryGrid.MasterTableView.GetColumnSafe("SavingLoss");
                if (colSavingLoss != null)
                {
                    colSavingLoss.Visible = true;
                }
            }

            if (!Page.IsPostBack)
            {
                if (SavedReportFilterValue == null)
                {
                    SavedReportFilterValue = new AS.Web.UI.Controls.HierarchyFilterValue();
                    SavedReportFilterValue.Value = string.Empty;
                    SavedReportFilterValue.ID = GeneralFuncsLib.GetMerchantHierarchyInfo().HierarchyID;
                    SavedReportFilterValue.HierarchyMode = GeneralFuncsLib.GetMerchantHierarchyInfo().HierarchyMode;
                    SavedReportFilterValue.DateOption = AS.Web.UI.Controls.DateOptionMode.DateRange;
                    SavedReportFilterValue.DateOptionValue.From = SavedReportFilterValue.DateOptionValue.To = DateTime.Now;
                }

                if (IsSecureQueryString)
                {
                    this.MerchantNumber = base.SecureQueryString[MERCHANT_NUMBER];
                }
                else
                {
                    SavedReportFilterSyncManager.SyncMerchantNumberToRiskReport();

                    if (SessionManager.ShareMerchantNumber != null)
                    {
                        this.MerchantNumber = SessionManager.ShareMerchantNumber;
                    }
                    else if (GeneralFuncsLib.IsMerchantMode(SavedReportFilterValue.HierarchyMode))
                        this.MerchantNumber = SavedReportFilterValue.Value;
                }

                if (this.MerchantNumber.Length == 0 && GeneralFuncsLib.IsMerchantMode(SavedReportFilterValue.HierarchyMode))
                {
                    this.MerchantNumber = SavedReportFilterValue.Value;
                }

                InitSettings();
            }

            RiskSessionManager.IsSCroll--;
        }
        catch (ViewStateException)
        {
            LoggerManager.Error(string.Format("RiskReport.aspx_Page_Load - Request={0}; SessionID={1}", Context.Request.Url, Session.SessionID));
            throw;
        }
    }

    private bool HasAttributeRiskScore()
    {
        if (GeneralFuncsLib.CheckOnOffModule(WebSiteConstants.AttributeRiskScoreModuleName))
        {
            return (((ReportPage)Page).IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_MERCHANTPROFILE) || ((ReportPage)Page).IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_MERCHANTPROFILE_MS)) && (((ReportPage)Page).IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RISK_INFO) || ((ReportPage)Page).IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_MSRISK_INFO));

        }
        else
        {
            return false;
        }
    }

    public void uxMerchantList_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        if (e.Text.Length > 2)
        {
            string merchantName = e.Text.Replace("%", "[%]").Replace(",", "[,]").Replace("^", "[^]").Replace("_", "[_]");
            FilterParameterCollection parameters = new FilterParameterCollection();
            parameters.AddLoggedInUserReportingParams(false);
            parameters.Add(new FilterParameter("@MerchantName", merchantName, DbType.String));
            DataTable list = WebServices.RiskServices.GetReports("spa_RM_MCF_GetMerchantList", parameters);
            uxMerchantList.DataSource = list;
            uxMerchantList.DataBind();
        }
        else
        {
            uxMerchantList.Items.Clear();
        }
    }

    protected void uxSearchButton_OnClick(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.SearchMerchantEvent);
    }

    protected void btnProcess_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.ProcessEvent);
    }

    protected void uxInvestigate_OnClick(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.InvestigateSubmitEvent);
    }

    protected int UpdateEscalation(int escalationID, bool closed, string assignedTo,
        int status, int resolution, string notes, int reasonid, DateTime? followupdate, double? savingLoss)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        FilterParameterCollection outValue;
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@EscalationID", escalationID, DbType.Int32));
        parameters.Add(new FilterParameter("@Closed", closed, DbType.Boolean));
        parameters.Add(new FilterParameter("@AssignedTo", assignedTo, DbType.String));
        parameters.Add(new FilterParameter("@Status", status, DbType.Int32));
        parameters.Add(new FilterParameter("@Resolution", resolution, DbType.Int32));
        parameters.Add(new FilterParameter("@Notes", notes, DbType.String));
        parameters.Add(new FilterParameter("@ReasonID", reasonid, DbType.Int32));
        parameters.Add(new FilterParameter("@FollowupDate", followupdate, DbType.Date));
        parameters.Add(new FilterParameter("@Followed", uxIsFollowupDate.Checked, DbType.Boolean));
        parameters.Add(new FilterParameter("@SavingsLossAmt", savingLoss, DbType.Currency));
        return WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_UpdateEscalation", parameters, out outValue);

    }

    protected int AddEscalation(int siteID, string merchantNumber, string assignedTo,
        int status, string notes, string userID, int reasonid, DateTime? followupdate, double? savingLoss)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        FilterParameterCollection outValue;
        parameters.Add(new FilterParameter("@MerchantNumber", merchantNumber, DbType.String));
        parameters.Add(new FilterParameter("@AssignedTo", assignedTo, DbType.String));
        parameters.Add(new FilterParameter("@Status", status, DbType.Int32));
        parameters.Add(new FilterParameter("@Notes", notes, DbType.String));
        parameters.Add(new FilterParameter("@ReasonID", reasonid, DbType.Int32));
        parameters.Add(new FilterParameter("@FollowupDate", followupdate, DbType.Date));
        parameters.Add(new FilterParameter("@Followed", uxIsFollowupDate.Checked, DbType.Boolean));
        parameters.Add(new FilterParameter("@SavingsLossAmt", savingLoss, DbType.Currency));
        return WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_AddEscalation", parameters, out outValue);
    }

    private void InitSettings()
    {
        try
        {
            if (Regex.Match(this.MerchantNumber, "[0-9]{1,20}").Success)
            {
                HierarchyDetail merchDetail = GeneralFuncsLib.GetMerchantHierarchyInfo();
                if (merchDetail != null)
                {
                    SavedReportFilterValue.ID = merchDetail.HierarchyID;
                    SavedReportFilterValue.HierarchyMode = merchDetail.HierarchyMode;
                }
                else
                {
                    LoggerManager.Warn("InitSettings HierarchyDetail is null");
                }

                SavedReportFilterValue.Value = this.MerchantNumber;

                SessionManager.CurrentMerchantNumber = this.MerchantNumber;
                ucMerchantNote.MerchantNumber = this.MerchantNumber;
                uxMerchantNumber.Text = VeraCodeSolution.ValidateResponseData(this.MerchantNumber.Trim());
                ucCaseHistory.MerchantNumber = this.MerchantNumber;
                SessionManager.ShareMerchantNumber = this.MerchantNumber;

                GetData();
            }
            else
            {
                this.MerchantNumber = string.Empty;
                uxRiskReportMerchantInformation.MerchantNumber = this.MerchantNumber;
                uxChargebacks.ReportDate = DateTime.Now;
                GetChargebackWithIsCSViewFullCard();
            }
        }
        catch (Exception ex)
        {
            LoggerManager.Error("InitSettings error", ex);
            throw;
        }
        //for SNET Readonly
        //TK39919 - remove uxNewRiskCommentLink
    }

    private void GetData()
    {
        uxRiskReportMerchantInformation.MerchantNumber = this.MerchantNumber;

        //TK39919 - remove uxRiskComment
        uxACHReturnDetail.MerchantNumber = this.MerchantNumber;
        uxACHReturnDetail.ReportDate = DateTime.Now.Date;

        uxTransactionHistory.MerchantNumber = this.MerchantNumber;
        uxTransactionHistory.ReportDate = DateTime.Now.Date;
        uxTransactionHistory.FlagLink = 1;

        if (GeneralFuncsLib.GetDataOfExtendedSetting("EnableACHReturnDetail").ToLower().Equals("true"))
        {
            uxACHReturnDetail.Visible = true;
        }

        uxPanelCaseHistory.Visible = ((ReportPage)Page).IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_CM_OPEN_CASE) || ((ReportPage)Page).IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_CM_SEARCH_CASE);


        // 44814 - remove menu of all Escalation page
        addInvestigation.Visible = GeneralFuncsLib.ReadOnlyRiskManagementEnableStatus((SecurePage)Page);
        uxInvestigationPanel.Visible = uxInvestigationGridPanel.Visible = GeneralFuncsLib.CheckOnOffModule(WebSiteConstants.RskEscalationModuleName);

        uxRelatedMerchants.MerchantNumber = this.MerchantNumber;

        uxForwardDelivery.MerchantNumber = this.MerchantNumber;

        uxChargebacks.MerchantList = this.MerchantNumber;
        uxChargebacks.ReportDate = DateTime.UtcNow.Date;
        uxChargebacks.IsEnableExport = true;
        GetChargebackWithIsCSViewFullCard();

        GeneralFuncsLib.GetDefaultDaysOfRiskReport();

        BindBatchDateRange();

        //Bind data use multi-thread
        BindDataToAllUserControl();
    }

    private void BindInvestigationForm()
    {
        DataTable escHistory = uxEscHistoryGrid.DataSource as DataTable;
        DataRow[] escHistoryNotClosedRows = null;
        if (escHistory != null && escHistory.Rows.Count > 0)
        {
            escHistoryNotClosedRows = escHistory.Select("Closed = 'No'");

            if (escHistoryNotClosedRows.Length > 0)
            {
                ViewState[ESCALATION_ID] = GeneralFuncsLib.NvlString(escHistoryNotClosedRows[0][ESCALATION_ID]);

                uxInvStartedDateInput.Visible = true;
                uxInvCloseInput.Visible = true;
                uxInvResolution.Visible = true;
                uxStartedDate.Text = VeraCodeSolution.DoVeraCode(GeneralFuncsLib.FormatDate(escHistoryNotClosedRows[0]["EscalationDate"]));
                uxStatusList.SelectedValue = GeneralFuncsLib.NvlString(escHistoryNotClosedRows[0]["StatusID"]);
                uxAssignedToList.SelectedValue = GeneralFuncsLib.NvlString(escHistoryNotClosedRows[0]["AssignedToID"]);
                uxResolutionList.SelectedValue = GeneralFuncsLib.NvlString(escHistoryNotClosedRows[0]["ResolutionID"]);

                uxReasonList.SelectedValue = GeneralFuncsLib.NvlString(escHistoryNotClosedRows[0]["ReasonID"]);
                if (escHistoryNotClosedRows[0]["SavingsLossAmt"] != null)
                {
                    uxSavingsLoss.Text = VeraCodeSolution.DoVeraCode(escHistoryNotClosedRows[0]["SavingsLossAmt"].ToString());
                }
                if (escHistory.Rows[0]["FollowupDate"] != DBNull.Value)
                {
                    uxFollowUpdate.SelectedDate = ((DateTime)escHistory.Rows[0]["FollowupDate"]).Date;

                    CurrentFollowUpdateDate = ((DateTime)escHistory.Rows[0]["FollowupDate"]).Date.ToString();

                    if (escHistory.Rows[0]["Followed"] != DBNull.Value)
                    {

                        uxIsFollowupDate.Checked = escHistory.Rows[0]["Followed"].ToString().ToLower() == "yes";
                        uxFollowUpdate.Enabled = escHistory.Rows[0]["Followed"].ToString().ToLower() == "yes";
                        IsCheckFollowUpdate = escHistory.Rows[0]["Followed"].ToString().ToLower() == "yes" ? "true" : "false";
                    }
                }
                else
                {
                    uxFollowUpdate.Enabled = false;
                }


                uxInvestigate.Text = GetLocalResourceObject("rm_RiskReport_aspx_cs_SaveInvestigation").ToString();
            }
            else
            {
                RefreshInvestigationForm();
            }
        }
        else
        {
            RefreshInvestigationForm();
        }
    }

    private void RefreshInvestigationForm()
    {
        uxInvStartedDateInput.Visible = false;
        uxInvCloseInput.Visible = false;
        uxInvResolution.Visible = false;
        uxStatusList.SelectedIndex = 0;
        uxAssignedToList.SelectedIndex = 0;
        uxReasonList.SelectedIndex = 0;
        uxResolutionList.SelectedIndex = 0;
        uxFollowUpdate.Enabled = false;
        uxFollowUpdate.SelectedDate = null;
        uxIsFollowupDate.Checked = false;
        ViewState[ESCALATION_ID] = 0;
        uxCloseInvestigation.Checked = false;
        uxSavingsLoss.Text = string.Empty;
    }

    protected string BuildURL(string merchantnumber, string batchnumber, string terminalnumber,
        string reportdate, string network)
    {
        return "<a href=\"#\" onclick=\"return " + jsOpenPopupURL(merchantnumber, batchnumber, terminalnumber, reportdate, network) + "\">" + batchnumber + "</a>";
    }

    protected string jsOpenPopupURL(string merchantnumber, string batchnumber, string terminalnumber,
        string reportdate, string network)
    {
        string queryString = this.BuildSecureQueryString("merchantnumber=" + merchantnumber + "&BatchNumber=" + batchnumber + "&TerminalNumber=" + terminalnumber + "&ReportDate=" + reportdate + "&network=" + network + "&ParentIsRisk=true" + IntruderQuery + "&type=1");
        string urlBatchDetail = ResolveUrl("~/BatchDetailModal.aspx?") + queryString;
        return "ShowPopupModal('" + urlBatchDetail + "','auto');";
    }

    protected string RiskNoteUrl()
    {
        string queryString = this.BuildSecureQueryString("MerchantNumber=" + MerchantNumber);
        return ResolveUrl("rm_MCF_RiskNotesModal_MCPS.aspx?") + queryString;
    }

    protected DataTable GetRefValue(DataTable value)
    {
        DataTable result = value;

        for (int i = 0; i < result.Rows.Count; i++)
        {
            result.Rows[i]["DataText"] = VeraCodeSolution.ValidateResponseData(result.Rows[i]["DataText"].ToString());
        }

        return result;
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        ((MasterPageNormal)this.Master).ShowLeftNaviControl = uxInfo.Visible;
    }
    //TK39919 - remove uxRiskComments

    private void BindBatchDateRange()
    {
        uxBatchDateRange.Items.Clear();
        List<RadComboBoxItem> items = new List<RadComboBoxItem>()
        {
            new RadComboBoxItem("1", "1"),
            new RadComboBoxItem("2", "2"),
            new RadComboBoxItem("3", "3"),
            new RadComboBoxItem("5", "5"),
            new RadComboBoxItem("7", "7"),
            new RadComboBoxItem("14", "14"),
            new RadComboBoxItem("30", "30"),
            new RadComboBoxItem("60", "60"),
            new RadComboBoxItem("90", "90")
        };
        uxBatchDateRange.Items.AddRange(items);
        string defaultDays = RiskSessionManager.BatchDateRangeModal.ToString();
        RadComboBoxItem item = items.Find(i => i.Value == defaultDays);
        item.Selected = true;
    }

    protected void uxBatchDateRange_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        RiskSessionManager.BatchDateRangeModal = int.Parse(uxBatchDateRange.SelectedValue);
        uxBatchHistoryGrid.MasterTableView.CurrentPageIndex = 0;
        uxBatchHistoryGrid.Rebind();
    }

    protected void btnClickLink_Click(object sender, EventArgs e)
    {
        string[] value = hddValueLink.Value.Split(';');
        string action = string.Empty;
        Telerik.Web.UI.RadAjaxManager ajax = Telerik.Web.UI.RadAjaxManager.GetCurrent(this.Page);
        if (value[0].Equals("B"))
        {
            action = jsOpenPopupURL(MerchantNumber, value[1], value[2], value[3], string.Empty);
            ajax.ResponseScripts.Add(action);
        }
        else if (value[0].Equals("T"))
        {
            string fullCC = String.Empty;
            FilterParameterCollection parameters_FullCard = new FilterParameterCollection();

            parameters_FullCard.Add(new FilterParameter("@UserID", SessionManager.CurrentUser.UserID, DbType.AnsiString));
            parameters_FullCard.Add(new FilterParameter("@ASClient", SessionManager.CurrentUser.ASClient, DbType.Int32));
            parameters_FullCard.Add(new FilterParameter("@RecordID", Convert.ToInt32(value[2]), System.Data.DbType.Int32));
            parameters_FullCard.Add(new FilterParameter("@ReportType", value[4], System.Data.DbType.AnsiString));
            parameters_FullCard.Add(new FilterParameter("@ReportDate", Convert.ToDateTime(value[3]), System.Data.DbType.Date));

            if (GeneralFuncsLib.CheckCSViewFullCard((SecurePage)this.Page))
            {
                parameters_FullCard.AddDecryptDataParams("AccountNumber");
            }

            DataTable dt_FullCard = WebServices.CsReportServices.GetReports("spa_cs_GetFullCardNumber", parameters_FullCard);
            if (dt_FullCard != null && dt_FullCard.Rows.Count > 0)
            {
                fullCC = dt_FullCard.Rows[0][0].ToString();
            }

            string queryString = this.BuildSecureQueryString("cn=" + value[1] + "&cnf=" + fullCC + "&merch=" + MerchantNumber + "&isRisk=1");
            string urlCardDetail = ResolveUrl("~/") + "CardHistoryModal.aspx?" + queryString;
            action = "openPopupWindow('" + urlCardDetail + "', 'CardHistoryWindow')";
            ajax.ResponseScripts.Add(action);
        }
    }

    protected void uxSubmit_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.AddNote, sender);
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string[] CheckSensitiveData(string comment)
    {
        return GeneralFuncsLib.DetectSesitiveData(comment);
    }

    //39919 - Performance PROP Issue
    [WebMethod]
    public static object[] GetRoleAndAddedBy(string sources)
    {
        UserControls_rm_MCF_MerchantNote merchantNote = new UserControls_rm_MCF_MerchantNote();
        DataTable roles = merchantNote.GetRoles(sources);
        DataTable users = merchantNote.GetAddedBy(sources, string.Empty);
        object[] results = new object[2];
        results[0] = GeneralFuncsLib.DataTableToJson(roles);
        results[1] = GeneralFuncsLib.DataTableToJson(users);
        return results;
    }

    [WebMethod]
    public static object GetAddedBy(string sources, string roles)
    {
        UserControls_rm_MCF_MerchantNote merchantNote = new UserControls_rm_MCF_MerchantNote();
        DataTable users = merchantNote.GetAddedBy(sources, roles);
        return GeneralFuncsLib.DataTableToJson(users);
    }

    [WebMethod]
    public static object GetSourceAndRole()
    {
        UserControls_rm_MCF_MerchantNote merchantNote = new UserControls_rm_MCF_MerchantNote();
        string defaultSources = PersonalDataHelper.GetJSONConfig<string>(UserConfigNames.CONFIG_RISK_REPORT_SOURCE_DEFAULT_SETTING);
        object[] results = new object[4];

        results[0] = defaultSources;
        results[1] = GeneralFuncsLib.DataTableToJson(merchantNote.GetSources());
        results[2] = GeneralFuncsLib.DataTableToJson(merchantNote.GetRoles(defaultSources));
        results[3] = GeneralFuncsLib.DataTableToJson(merchantNote.GetAddedBy(defaultSources, string.Empty));

        return results;
    }

    [WebMethod]
    public static object GetRoles(string sources)
    {
        UserControls_rm_MCF_MerchantNote merchantNote = new UserControls_rm_MCF_MerchantNote();
        DataTable roles = merchantNote.GetRoles(sources);

        return GeneralFuncsLib.DataTableToJson(roles);
    }

    #region Enhance Risk Report Performance
    public List<SpaInfo> Thread1 { get; set; }
    public List<SpaInfo> Thread2 { get; set; }
    public List<SpaInfo> Thread3 { get; set; }
    public List<SpaInfo> Thread4 { get; set; }

    //Bind data use multi-thread
    public void BindDataToAllUserControl()
    {
        List<DataSourceParallelRequest> threads = BuildThreads();
        List<DataSourceParallelResponse> dataSources;

        RiskReportBusiness riskReportBusiness = new RiskReportBusiness();
        dataSources = riskReportBusiness.GetDataSourceParallel(threads);

        // Save UserActivity
        if (!string.IsNullOrEmpty(GeneralFuncsLib.GetMerchantName(this.MerchantNumber)))
            GeneralFuncsLib.SaveUserActivity(this.MerchantNumber, string.Format("Risk Report: View {0}", this.MerchantNumber), true);

        //bind UC Merchant Infomation
        uxRiskReportMerchantInformation.BindDataFromThread(dataSources);
        //bind uc ReportTransactionVolumeAnalysis
        uxRiskReportTransactionVolumeAnalysis.BindDataFromThread(dataSources);
        //bind uc uxTransactionHistory
        uxTransactionHistory.BindDataFromThread(dataSources);
        //bind ucCaseHistory
        ucCaseHistory.BindDataFromThread(null);

        //bind uc uxACHReturnDetail
        uxACHReturnDetail.BindDataFromThread(dataSources);

        //bind ucMerchantNote
        ucMerchantNote.BindMerchantNoteGirdInitiateFromRiskReport();

        //bind uc ucRelatedMerchants
        uxRelatedMerchants.BindDataFromThread(dataSources);

        //bind uc ucRelatedMerchants
        uxForwardDelivery.BindDataFromThread(dataSources);

        //bind uc ucChargeBacks
        uxChargebacks.BindDataFromThread(dataSources);

        DataTable generalInfo = uxRiskReportMerchantInformation.MerchantInfo;
        if (generalInfo == null || generalInfo.Rows.Count == 0)
        {
            uxInfo.Visible = false;
            return;
        }
        uxInfo.Visible = (generalInfo.Rows.Count > 0);

        //Bind data for all controls on this page
        this.RiskReportDataSource = dataSources;

        uxBatchHistoryGrid.Rebind();
        uxEscHistoryGrid.Rebind();

        //Call EscalationStatus
        OnDataBindControls(DataBindAction.BindEscalationStatus, uxStatusList);
        //Call EscalationUser
        OnDataBindControls(DataBindAction.BindEscalationUser, uxAssignedToList);
        OnDataBindControls(DataBindAction.BindResolution, uxResolutionList);
        OnDataBindControls(DataBindAction.BindReasonList, uxResolutionList);
        OnDataBindControls(DataBindAction.BindReasonRequired);

        BindInvestigationForm();

        //select 1st tab as default
        var tab = uxTabView.FindTabByText("Transactions");
        tab.Selected = true;

        var FDtab = uxTabView.FindTabByText("Forward Delivery");
        FDtab.Visible = false;
        uxForwardDelivery.Visible = false;
        if (SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.CS || SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.AS)
        {
            var fdSetting = GeneralFuncsLib.GetDataOfExtendedSetting("ENABLE_FORWARD_DELIVERY");
            if ("true".Equals(fdSetting, StringComparison.OrdinalIgnoreCase))
            {
                FDtab.Visible = true;
                uxForwardDelivery.Visible = true;
            }
        }

    }

    private List<DataSourceParallelRequest> BuildThreads()
    {
        List<DataSourceParallelRequest> threads = new List<DataSourceParallelRequest>();
        //Thread 1
        Thread1 = new List<SpaInfo>();
        Thread1.Add(uxTransactionHistory.SpaGetTransactionHistory);
        threads.Add(new DataSourceParallelRequest()
        {
            Thread = 1,
            SpasInfo = Thread1
        });
        //Thread 2
        Thread2 = new List<SpaInfo>();
        Thread2.Add(uxRiskReportMerchantInformation.SpaGetMerchantInfo);
        Thread2.Add(this.SpaGetEscalationUsers);
        Thread2.Add(this.SpaGetEscalationReason);
        Thread2.Add(this.SpaGetEscalationReasonConfig);
        threads.Add(new DataSourceParallelRequest()
        {
            Thread = 2,
            SpasInfo = Thread2
        });
        //Thread 3
        Thread3 = new List<SpaInfo>();
        Thread3.Add(uxRiskReportTransactionVolumeAnalysis.SpaGetTransactionVolumeAnalysis);
        Thread3.Add(uxRiskReportMerchantInformation.SpaGetMerchantHierarchyForRiskReport);
        Thread3.Add(this.SpaGetEscalationHistory);
        Thread3.Add(this.SpaGetResolution);

        threads.Add(new DataSourceParallelRequest()
        {
            Thread = 3,
            SpasInfo = Thread3
        });

        //Thread 4
        Thread4 = new List<SpaInfo>();
        Thread4.Add(this.SpaGetBatchHistory);
        Thread4.Add(uxRiskReportMerchantInformation.SpaGetMIFClassificationByMerchant);
        Thread4.Add(uxACHReturnDetail.SpaGetACHHistory);
        Thread4.Add(uxRiskReportTransactionVolumeAnalysis.SpaGetCustomViewList);
        Thread4.Add(uxRiskReportMerchantInformation.SpaGetProfileList);
        Thread4.Add(this.SpaGetEscalationStatus);
        Thread4.Add(uxRelatedMerchants.SpaGetRelatedMerchants);
        Thread4.Add(uxChargebacks.SpaGetChargeBacks90Days);
        threads.Add(new DataSourceParallelRequest()
        {
            Thread = 4,
            SpasInfo = Thread4
        });

        return threads;
    }
    #endregion

    #region SPA INFO
    public SpaInfo SpaGetBatchHistory
    {
        get
        {
            return new SpaInfo()
            {
                FeatureName = DataBindAction.BindBatchHistoryGrid.ToString(),
                SpaName = SPA_GET_BATCH_HISTORY,
                Parameters = GetRiskReportParams(DataBindAction.BindBatchHistoryGrid)
            };
        }
    }
    public SpaInfo SpaGetEscalationHistory
    {
        get
        {
            return new SpaInfo()
            {
                FeatureName = DataBindAction.BindEscalationHistory.ToString(),
                SpaName = SPA_GET_ESCALATION_HISTORY,
                Parameters = GetRiskReportParams(DataBindAction.BindEscalationHistory)
            };
        }
    }

    public SpaInfo SpaGetEscalationStatus
    {
        get
        {
            return new SpaInfo()
            {
                FeatureName = DataBindAction.BindEscalationStatus.ToString(),
                SpaName = SPA_GET_ESCALATION_STATUS,
                Parameters = GetRiskReportParams(DataBindAction.BindEscalationStatus)
            };
        }
    }

    public SpaInfo SpaGetEscalationUsers
    {
        get
        {
            return new SpaInfo()
            {
                FeatureName = DataBindAction.BindEscalationUser.ToString(),
                SpaName = SPA_GET_ESCALATION_USERS,
                Parameters = GetRiskReportParams(DataBindAction.BindEscalationUser)
            };
        }
    }

    public SpaInfo SpaGetResolution
    {
        get
        {
            return new SpaInfo()
            {
                FeatureName = DataBindAction.BindResolution.ToString(),
                SpaName = SPA_GET_RESOLUTION,
                Parameters = GetRiskReportParams(DataBindAction.BindResolution)
            };
        }
    }

    public SpaInfo SpaGetEscalationReason
    {
        get
        {
            return new SpaInfo()
            {
                FeatureName = DataBindAction.BindReasonList.ToString(),
                SpaName = SPA_GET_ESCALATION_REASON,
                Parameters = GetRiskReportParams(DataBindAction.BindReasonList)
            };
        }
    }

    public SpaInfo SpaGetEscalationReasonConfig
    {
        get
        {
            return new SpaInfo()
            {
                FeatureName = DataBindAction.BindReasonRequired.ToString(),
                SpaName = SPA_GET_ESCALATION_CONFIG,
                Parameters = GetRiskReportParams(DataBindAction.BindReasonRequired)
            };
        }
    }

    #endregion
    private FilterParameterCollection GetRiskReportParams(DataBindAction action)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        switch (action)
        {
            case DataBindAction.BindBatchHistoryGrid:
                {
                    parameters.AddLoggedInUserReportingParams(false);
                    parameters.Add(new FilterParameter("@ReportDate", DateTime.Today, DbType.Date));
                    parameters.Add(new FilterParameter("@MerchantNumber", this.MerchantNumber, DbType.String));
                    parameters.Add(new FilterParameter("@DateRange", RiskSessionManager.BatchDateRangeModal, DbType.Int32));

                    parameters.Add(new FilterParameter("@IsPaging", true, DbType.Boolean));
                    parameters.Add(new FilterParameter("@IsCountPageTotal", false, DbType.Boolean));
                    parameters.Add(new FilterParameter("@PageNo", 1, DbType.Int16));
                    parameters.Add(new FilterParameter("@PageSize", 10, DbType.Int16));
                    parameters.Add(new FilterParameter("@stOrder", "", DbType.String));
                    parameters.Add(new FilterParameter("@stFilter", "", DbType.String));
                    break;
                }
            case DataBindAction.BindEscalationHistory:
                {
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parameters.Add(new FilterParameter("@EscalationID", 0, DbType.Int32));
                    parameters.Add(new FilterParameter("@MerchantNumber", this.MerchantNumber, DbType.String));

                    break;
                }
            case DataBindAction.BindEscalationStatus:
                {
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parameters.Add(new FilterParameter("@IsActive", 1, DbType.Int32));
                    parameters.Add(new FilterParameter("@SortOrder", " Status ASC", DbType.String));
                    break;
                }
            case DataBindAction.BindEscalationUser:
                {
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parameters.Add(new FilterParameter("@Mode", 0, DbType.Int32));
                    break;
                }
            case DataBindAction.BindResolution:
                {
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parameters.Add(new FilterParameter("@IsActive", 1, DbType.Int32));
                    parameters.Add(new FilterParameter("@SortOrder", " Resolution ASC", DbType.String));
                    break;
                }
            case DataBindAction.BindReasonList:
                {
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parameters.Add(new FilterParameter("@IsActive", 1, DbType.Int32));
                    break;
                }
            case DataBindAction.BindReasonRequired:
                {
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    break;
                }
            case DataBindAction.BindChargeBack:
                {
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    break;
                }
        }

        return parameters;
    }

    private string EncryptComment(string comment)
    {
        string lstCard = hdCardDetected.Value.TrimEnd(',');
        if (!string.IsNullOrEmpty(lstCard))
        {
            foreach (var item in lstCard.Split(','))
            {
                string realItem = item.Replace(" ", "").Replace("-", "");
                string replaceItem = realItem.Substring(0, 6) + "xxxxxx" + realItem.Substring(realItem.Length - 4, 4);
                comment = comment.Replace(item, replaceItem);
            }
        }

        return comment;
    }
    public int NoteSourceDefaultForIns
    {
        get
        {
            var url = Request.Url.AbsoluteUri.ToLower();
            if (url.Contains(MERCHANT_PROFILE_PAGE.ToLower()) || url.Contains("MerchantInformation.aspx".ToLower()))
                return 1;
            if (url.Contains(RISK_REPORT_PAGE.ToLower()))
                return 2;
            return 0;
        }
    }
    #endregion Methods

    #region Chargebacks90Days
    protected void uxAccountNumberClick_Click(object sender, EventArgs e)
    {
        string[] arg = uxHiddenAccountNumberClick.Value.Split(';');
        string reportType = arg[1];
        string recordID = arg[2];
        string partialCardNum = arg[3];
        DateTime rpDate;
        if (string.IsNullOrEmpty(arg[4]) || !DateTime.TryParse(arg[4], CultureInfo.InvariantCulture, DateTimeStyles.None, out rpDate))
        {
            rpDate = DateTime.Now;
        }
        string merchantNumber = arg[5];

        string fullCC = string.Empty;
        FilterParameterCollection parameters_FullCard = new FilterParameterCollection
        {
            new FilterParameter("@UserID", SessionManager.CurrentUser.UserID, DbType.AnsiString),
            new FilterParameter("@ASClient", SessionManager.CurrentUser.ASClient, DbType.Int32),
            new FilterParameter("@RecordID", Convert.ToInt32(recordID), DbType.Int32),
            new FilterParameter("@ReportType", reportType, DbType.AnsiString),
            new FilterParameter("@ReportDate", rpDate, DbType.DateTime)
        };

        if (IsCSViewFullCard)
        {
            parameters_FullCard.AddDecryptDataParams("AccountNumber");
        }

        DataTable dt_FullCard = WebServices.CsReportServices.GetReports("spa_cs_GetFullCardNumber", parameters_FullCard);
        if (dt_FullCard != null && dt_FullCard.Rows.Count > 0)
        {
            fullCC = dt_FullCard.Rows[0][0].ToString();
        }

        string queryString = BuildSecureQueryString("cn=" + partialCardNum + "&cnf=" + fullCC + "&merch=" + merchantNumber + "&isRisk=1");
        string urlCardDetail = ResolveUrl("~/") + "CardHistoryModal.aspx?" + queryString;
        var action = "openPopupWindow('" + urlCardDetail + "', 'CardHistoryWindow')";

        Telerik.Web.UI.RadAjaxManager ajax = Telerik.Web.UI.RadAjaxManager.GetCurrent(this.Page);
        ajax.ResponseScripts.Add(action);
    }

    private void GetChargebackWithIsCSViewFullCard()
    {
        uxChargebacks.IsCSViewFullCard = IsCSViewFullCard;
    }
    #endregion
}
