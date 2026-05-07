using System.Linq;
using AS.Common;
using AS.Common.DBManager;
using AS.Common.WebUI;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Web.Business;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Web;

[PagePermission("BatchRpt,MSBatchRpt")]
public partial class gen_BatchHistory : ReportPage
{
    enum DataBindAction
    {
        BindGridDrillDown,
        BindReportGridBatch,
        BindReportGridVoidedReject,
        BindReportGridTransQualification,
        BindReportGridCardSummary
    }

    enum PostBackAction
    {
        BatchDetailClick
    }

    #region Properties
    private string _Target = "_parent";

    private string _GridTitle
    {
        get
        {
            return GeneralFuncsLib.GetFullGridTitleName(ReportFilter) + " " + GeneralFuncsLib.GetDateFilterText(ReportFilter);
        }
    }

    private string _GridDrillDownHeader
    {
        get
        {
            return GeneralFuncsLib.GetGridDrilldownColumnName(ReportFilter.CurrentValue.HierarchyMode, ReportFilter.CurrentValue.Value);
        }
    }
    string _CardSummaryIntruderQuery = string.Empty;
    private string CardSummaryIntruderQuery
    {
        get
        {

            if (_CardSummaryIntruderQuery == string.Empty)
            {
                if (!GeneralFuncsLib.IsMerchantMode(ReportFilter.CurrentValue.HierarchyMode) || ReportFilter.CurrentValue.Value.IsNullOrEmpty())
                {
                    _CardSummaryIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(uxDrilldownGrid.ID, new string[] { "Entity" });
                }
                else
                {
                    _CardSummaryIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(uxBatchMerchantGrid.ID, new string[] { "BatchNumber", "TerminalNumber" });
                }

            }
            return _CardSummaryIntruderQuery;
        }
    }
    string _BatchDetailIntruderQuery = string.Empty;
    private string BatchDetailIntruderQuery
    {
        get
        {
            if (_BatchDetailIntruderQuery == string.Empty) _BatchDetailIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(uxBatchMerchantGrid.ID, new string[] { "BatchNumber", "TerminalNumber" });
            return _BatchDetailIntruderQuery;
        }
    }
    string _AuthIntruderQuery = string.Empty;
    private string AuthIntruderQuery
    {
        get
        {

            if (_AuthIntruderQuery == string.Empty)
            {
                _AuthIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(uxTranQualificationGrid.ID, new string[] { "AuthorizationNumber" });
            }
            return _AuthIntruderQuery;
        }
    }
    string _AuthIntruderQuery_Qual = string.Empty;
    private string AuthIntruderQuery_Qual
    {
        get
        {

            if (_AuthIntruderQuery_Qual == string.Empty)
            {
                _AuthIntruderQuery_Qual = GeneralFuncsLib.BuildIntruderInfoForQueryStr(uxTranQualificationGrid.ID, new string[] { "AuthorizationNumber" });
            }
            return _AuthIntruderQuery_Qual;
        }
    }
    string _CardSearchIntruderQuery = string.Empty;
    private string CardSearchIntruderQuery
    {
        get
        {

            if (_CardSearchIntruderQuery == string.Empty)
            {
                _CardSearchIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(uxVoidedRejectedTransGrid.ID, new string[] { "AccountNumber" });
            }
            return _CardSearchIntruderQuery;
        }
    }
    string _CardSearchIntruderQuery_Qual = string.Empty;
    private string CardSearchIntruderQuery_Qual
    {
        get
        {

            if (_CardSearchIntruderQuery_Qual == string.Empty)
            {
                _CardSearchIntruderQuery_Qual = GeneralFuncsLib.BuildIntruderInfoForQueryStr(uxTranQualificationGrid.ID, new string[] { "AccountNumber" });
            }
            return _CardSearchIntruderQuery_Qual;
        }
    }
    private bool CheckCSViewFullCard()
    {
        if ((SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.AS
            || SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.CS)
            && IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_CC))
        {
            return true;
        }
        return false;
    }

    private int _HierarchySelected
    {
        get
        {
            if (IsSecureQueryString && SecureQueryString["FilterMode"] != null)
            {
                return SecureQueryString["FilterMode"].ToInt();
            }
            return -1;
        }
    }

    private string _FilterValue
    {
        get
        {
            if (IsSecureQueryString && SecureQueryString["FilterValue"] != null)
            {
                return SecureQueryString["FilterValue"].ToString();
            }
            return string.Empty;
        }
    }
    #endregion
    private const string IMAGE = "<a class=\"image-link\" href=\"#\" style=\"cursor:pointer\" onclick=\" return ShowPopupModal('{0}','auto');\"><img src='res/img/information.png' border=\"0\"/></a>&nbsp; &nbsp;";


    protected override void PageInitialize()
    {
        this.GridIDs.Add("uxVoidedRejectedTransGrid");
        this.GridIDs.Add("uxBatchMerchantGrid");
        this.GridIDs.Add("uxTranQualificationGrid");
        this.GridIDs.Add("uxCardMerchantGrid");
        this.ExporterIDs.Add("uxExporterBatchMerchantGridTop");
        this.ExporterIDs.Add("uxExporterCardMerchantGridTop");
        this.ExporterIDs.Add("uxExportTranQualificationTop");
        this.ExporterIDs.Add("uxExportCardSummaryTop");

        ReportChart = (IChart)uxChart;
        base.PageInitialize();
    }


    private string GetActivity(bool isMerchant)
    {
        string value = GeneralFuncsLib.GetValueForActivityFunc(isMerchant, ReportFilter);
        string dateText = GeneralFuncsLib.GetDateTextForActivityFunc(isMerchant, ReportFilter);

        return string.Format(GetLocalResourceObject("BatchHistory_aspx_cs_BatchHistory").ToString(), value, dateText);

    }

    protected override void DoReportFilterAction(AS.Web.UI.Controls.ReportFilterEventArgs e)
    {
        if (e.ActionType == AS.Web.UI.Controls.ReportFilterEventType.Submit)
        {
            if (GeneralFuncsLib.IsMerchantMode(e.HierachyValue.HierarchyMode))
            {
                string merchantName = GeneralFuncsLib.GetMerchantName(e.HierachyValue.Value);
                string activity = GetActivity(!string.IsNullOrEmpty(merchantName));
                string merchantNumber = string.IsNullOrEmpty(merchantName) ? "" : e.HierachyValue.Value;
                GeneralFuncsLib.SaveUserActivity(merchantNumber, activity);
            }
        }
        base.DoReportFilterAction(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (aperia.controls.KendoChart.IsDataRequest) return;
        if (SessionManager.ClientFrameInfo != string.Empty) _Target = SessionManager.ClientFrameInfo;
        if (IsSecureQueryString && !IsPostBack)
        {
            string _date = String.Empty;
            _date = SecureQueryString["date"];
            string isMobileRedirect = SecureQueryString["mb"];
            string _merchantnumber = SecureQueryString["merchantnumber"];
            if (!_date.IsNullOrEmpty() && !_merchantnumber.IsNullOrEmpty())
            {
                ReportFilter.CurrentValue.DateOption = AS.Web.UI.Controls.DateOptionMode.Daily;
                ReportFilter.CurrentValue.DateOptionValue.From = this.ReportFilter.CurrentValue.DateOptionValue.To = new DateTime(long.Parse(_date));
                ReportFilter.CurrentValue.HierarchyMode = GeneralFuncsLib.GetMerchantHierarchyInfo().HierarchyMode;
                ReportFilter.CurrentValue.ID = GeneralFuncsLib.GetMerchantHierarchyInfo().HierarchyID;
                ReportFilter.CurrentValue.Value = _merchantnumber;
            }
            else if (!_date.IsNullOrEmpty() && !_FilterValue.IsNullOrEmpty() && _HierarchySelected != -1)
            {
                ReportFilter.CurrentValue.DateOption = AS.Web.UI.Controls.DateOptionMode.Daily;
                ReportFilter.CurrentValue.DateOptionValue.From = this.ReportFilter.CurrentValue.DateOptionValue.To = new DateTime(long.Parse(_date));
                ReportFilter.CurrentValue.HierarchyMode = GeneralFuncsLib.GetHierarchyInfo(_HierarchySelected).HierarchyMode.ToString();
                ReportFilter.CurrentValue.ID = GeneralFuncsLib.GetHierarchyInfo(_HierarchySelected).HierarchyID;
                ReportFilter.CurrentValue.Value = _FilterValue;
            }
            // Redirect from mobile
            if (!String.IsNullOrEmpty(isMobileRedirect))
            {
                string fromDate = SecureQueryString["BeginDate"];
                string toDate = SecureQueryString["EndDate"];
                if (!String.IsNullOrEmpty(fromDate))
                {
                    this.ReportFilter.CurrentValue.DateOptionValue.From = DateTime.Parse(fromDate);
                }
                if (!String.IsNullOrEmpty(toDate))
                {
                    this.ReportFilter.CurrentValue.DateOptionValue.To = DateTime.Parse(toDate);
                }
                if (!String.IsNullOrEmpty(_merchantnumber))
                {
                    this.ReportFilter.CurrentValue.Value = _merchantnumber;
                    this.ReportFilter.CurrentValue.HierarchyMode = HierarchyMode.MERCHANT_NR;
                    string hid = string.Empty;
                    DataRow dataRow = SessionManager.HierarchyFilter.Select("HierarchyMode = '" + HierarchyMode.MERCHANT_NR + "'").FirstOrDefault();
                    if (dataRow != null)
                    {
                        hid = dataRow[0].ToString();
                    }
                    this.ReportFilter.CurrentValue.ID = hid;
                }

            }
            SavedReportFilterValue = ReportFilter.CurrentValue;
        }
        IsBindDataOnLoad = true;
        uxChart.Reportpage = this;
        uxChart.BeginDate = this.ReportFilter.CurrentValue.DateOptionValue.From;
        uxChart.EndDate = this.ReportFilter.CurrentValue.DateOptionValue.To;
        this.uxToolTipMana.TargetControls.Clear();

        if (IsIntruderDetected) return;
        // insert a breadcrumb as Risk's request
        if (IsSecureQueryString)
        {
            if (SecureQueryString["returl"] != null && SecureQueryString["retlabel"] != null)
            {
                uxReturnUrl.Visible = true;
                uxReturnUrl.NavigateUrl = VeraCodeSolution.DoVeraCode(SecureQueryString["returl"].ToString());
                uxReturnUrl.Text = VeraCodeSolution.DoVeraCode(SecureQueryString["retlabel"].ToString());
            }
            else uxReturnUrl.Visible = false;
        }
        //Ticket #12112 - 28149 - TOTAL - Remove Non-qualifying report from Merchant Access (MS Site)
        if (SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.Merchant)
        {
            uxTranQualificationGrid.VisibleGrid(false);
        }

    }

    protected override void OnPreRender(EventArgs e)
    {
        if (uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderText == GetLocalResourceObject("BatchHistory_aspx_cs_MerchantNumber").ToString() ||
            uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderText == GetLocalResourceObject("BatchHistory_aspx_cs_Agent").ToString() ||
            uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderText == GetLocalResourceObject("BatchHistory_aspx_cs_Mertchant").ToString())
        {
            uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderStyle.Width = 120;
        }
        uxDrilldownGrid.Columns.FindByUniqueName("EntityName").Visible = GeneralFuncsLib.SHOW_PROCESSINGDATA_ENTITYNAME();
        base.OnPreRender(e);
    }

    protected override void DoSwitchView()
    {
        uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderText = VeraCodeSolution.DoVeraCode(_GridDrillDownHeader);
        uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderTooltip = VeraCodeSolution.DoVeraCode(_GridDrillDownHeader);
        GeneralFuncsLib.VisibleHierarchyNameByFilteringMode(ReportFilter.CurrentValue.HierarchyMode, ReportFilter.CurrentValue.Value, uxDrilldownGrid, uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderText);
        if (GeneralFuncsLib.IsMerchantMode(ReportFilter.CurrentValue.HierarchyMode))
        {
            if (!string.IsNullOrEmpty(ReportFilter.CurrentValue.Value))
            {
                uxPanelMerchantDetail.Visible = true;
                uxPanelHierarchy.Visible = false;
                uxBatchMerchantGrid.Columns.FindByUniqueName("ExBatchNumber").Visible = false;
                HideVoiRejectSection();
                HideTransQualSection();
                uxCardMerchantGrid.Visible = true;
                uxBatchMerchantGrid.Visible = true;

                if (CheckCSViewFullCard())
                {
                    uxTranQualificationGrid.Columns.FindByUniqueName("AccountNumber").Visible = true;
                    uxTranQualificationGrid.Columns.FindByUniqueName("PartialCardNumber").Visible = false;
                    uxVoidedRejectedTransGrid.Columns.FindByUniqueName("AccountNumber").Visible = true;
                    uxVoidedRejectedTransGrid.Columns.FindByUniqueName("PartialCardNumber").Visible = false;
                }
                else
                {
                    uxTranQualificationGrid.Columns.FindByUniqueName("AccountNumber").Visible = false;
                    uxTranQualificationGrid.Columns.FindByUniqueName("PartialCardNumber").Visible = true;
                    uxVoidedRejectedTransGrid.Columns.FindByUniqueName("AccountNumber").Visible = false;
                    uxVoidedRejectedTransGrid.Columns.FindByUniqueName("PartialCardNumber").Visible = true;
                }
                // TK26080: Hide the File Source columns under Batch History for WRFC view on VW MS
                string[] hiddenColumns = GeneralFuncsLib.GetHiddenColumnsOfBatchHistory();
                for (int i = 0; i < hiddenColumns.Length; i++)
                {
                    if (uxBatchMerchantGrid.Columns.FindByUniqueName(hiddenColumns[i]) != null)
                    {
                        uxBatchMerchantGrid.Columns.FindByUniqueName(hiddenColumns[i]).Visible = false;
                    }
                }
            }
            else
            {
                uxPanelMerchantDetail.Visible = false;
                uxPanelHierarchy.Visible = true;
                uxVoidedRejectedTransGrid.Visible = false;
                uxTranQualificationGrid.Visible = false;
                uxBatchMerchantGrid.Visible = false;
                uxCardMerchantGrid.Visible = false;
            }
        }
        else
        {
            uxPanelMerchantDetail.Visible = false;
            uxPanelHierarchy.Visible = true;
            uxVoidedRejectedTransGrid.Visible = false;
            uxTranQualificationGrid.Visible = false;
            uxBatchMerchantGrid.Visible = false;
            uxCardMerchantGrid.Visible = false;
        }

    }

    protected override void DoGridNeedDataSource(ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        if (sender == uxBatchMerchantGrid && uxPanelMerchantDetail.Visible)
            OnDataBindControls(DataBindAction.BindReportGridBatch, sender);
        else if (sender == uxVoidedRejectedTransGrid && uxPanelMerchantDetail.Visible)
            OnDataBindControls(DataBindAction.BindReportGridVoidedReject, sender);
        else if (sender == uxTranQualificationGrid && uxPanelMerchantDetail.Visible)
            OnDataBindControls(DataBindAction.BindReportGridTransQualification, sender);
        else if (sender == uxCardMerchantGrid && uxCardMerchantGrid.Visible)
            OnDataBindControls(DataBindAction.BindReportGridCardSummary, sender);
        else
            OnDataBindControls(DataBindAction.BindGridDrillDown, sender);
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddHierarchyFilterParams(this);
        parameters.AddLoggedInUserReportingParams();

        string spaName = string.Empty;
        bool AllowFullCC = IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_CC);
        FilterParameter PermissionCodeParam = new FilterParameter(WebSiteConstants.SPA_PERMISSION_PARAM_NAME, WebSiteConstants.SEC_PERMISSION_CC, DbType.String);

        switch ((DataBindAction)type)
        {
            case DataBindAction.BindGridDrillDown:
                {
                    parameters.Add("@ReportType", "BatchHistory", DbType.String);
                    spaName = WebSiteConstants.GET_REPORT_SPA_NAME;
                    (sender as ASGrid).DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(parameters) });

                    //set grif title
                    uxExporter.GridHeader = VeraCodeSolution.ValidateResponseData(GetLocalResourceObject("BatchHistory_aspx_cs_BatchSummary").ToString() + " " + _GridTitle);
                    uxExporter.GridTitle = VeraCodeSolution.ValidateResponseData(GetLocalResourceObject("BatchHistory_aspx_cs_BatchSummary").ToString() + " ");
                    uxExporter.GridSubTitle = VeraCodeSolution.ValidateResponseData(_GridTitle);
                    GeneralFuncsLib.VisibleHierarchyNameByFilteringMode(ReportFilter.CurrentValue.HierarchyMode, ReportFilter.CurrentValue.Value, uxDrilldownGrid, uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderText);
                }
                break;

            case DataBindAction.BindReportGridBatch:
                {
                    ASGrid grid = (ASGrid)sender;
                    spaName = "spa_GetBatchSummary";
                    string merchantNumber = this.ReportFilter.CurrentValue.Value.ToString();
                    (sender as ASGrid).DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(parameters) });

                    uxExporterBatchMerchantGridTop.GridHeader = VeraCodeSolution.ValidateResponseData(GetLocalResourceObject("BatchHistory_aspx_cs_BatchSummary").ToString() + " " + _GridTitle);
                    uxExporterBatchMerchantGridTop.GridTitle = VeraCodeSolution.DoVeraCode(GetLocalResourceObject("BatchHistory_aspx_cs_BatchSummary").ToString() + " ");
                    uxExporterBatchMerchantGridTop.GridSubTitle = VeraCodeSolution.ValidateResponseData(_GridTitle);
                }
                break;

            case DataBindAction.BindReportGridVoidedReject:
                {
                    parameters.AddLoggedInUserPrimaryUserID();
                    ASGrid grid = (ASGrid)sender;
                    spaName = "spa_GetVoidRejectDetail";
                    string merchantNumber = this.ReportFilter.CurrentValue.Value.ToString();
                    if (CheckCSViewFullCard() || GeneralFuncsLib.HasIPForFullCard((SecurePage)this))
                    {
                        parameters.AddDecryptDataParams("AccountNumber", _isExporting);
                    }
                    parameters.AddLanguageID();
                    (sender as ASGrid).DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(parameters) });
                    uxExporterCardMerchantGridTop.GridHeader = VeraCodeSolution.ValidateResponseData(GetLocalResourceObject("BatchHistory_aspx_cs_VoidedRejectedTransactions").ToString() + " " + _GridTitle);
                    uxExporterCardMerchantGridTop.GridTitle = GetLocalResourceObject("BatchHistory_aspx_cs_VoidedRejectedTransactions").ToString() + " ";
                    uxExporterCardMerchantGridTop.GridSubTitle = VeraCodeSolution.ValidateResponseData(_GridTitle);
                }
                break;
            case DataBindAction.BindReportGridTransQualification:
                {
                    parameters.AddLoggedInUserPrimaryUserID();
                    ASGrid grid = (ASGrid)sender;
                    spaName = "spa_GetTransQualificationDetail";
                    string merchantNumber = this.ReportFilter.CurrentValue.Value.ToString();
                    if (CheckCSViewFullCard() || GeneralFuncsLib.HasIPForFullCard((SecurePage)this))
                    {
                        parameters.AddDecryptDataParams("AccountNumber", _isExporting);
                    }
                    parameters.AddLanguageID();
                    (sender as ASGrid).DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(parameters) });
                    uxExportTranQualificationTop.GridHeader = VeraCodeSolution.ValidateResponseData(GetLocalResourceObject("BatchHistory_aspx_cs_NonQualifyingTransactions").ToString() + " " + _GridTitle);
                    uxExportTranQualificationTop.GridTitle = GetLocalResourceObject("BatchHistory_aspx_cs_NonQualifyingTransactions").ToString() + " ";
                    uxExportTranQualificationTop.GridSubTitle = VeraCodeSolution.ValidateResponseData(_GridTitle);
                }
                break;
            case DataBindAction.BindReportGridCardSummary:
                {
                    parameters.AddLanguageID();
                    ASGrid grid = (ASGrid)sender;
                    spaName = "spa_ms_GetCardSummary";
                    (sender as ASGrid).DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(parameters) });
                    uxExportCardSummaryTop.GridHeader = VeraCodeSolution.ValidateResponseData(GetLocalResourceObject("BatchHistory_aspx_cs_CardSummary").ToString() + " " + _GridTitle);
                    uxExportCardSummaryTop.GridTitle = GetLocalResourceObject("BatchHistory_aspx_cs_CardSummary").ToString() + " ";
                    uxExportCardSummaryTop.GridSubTitle = VeraCodeSolution.ValidateResponseData(_GridTitle);
                }
                break;
        }
    }
    protected override void DoItemDataBound(ASGrid sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView rowView = e.Item.DataItem as DataRowView;
            if (sender == uxDrilldownGrid && sender.Visible)
            {
                string queryString = BuildSecureQueryString("Entity=" + HttpUtility.UrlEncode(rowView["Entity"].ToString()) + "&HierarchyMode=" + ReportFilter.CurrentValue.HierarchyMode + CardSummaryIntruderQuery);
                string urlNetDetail = "CardSummaryModal.aspx?" + queryString;
                string url = "<a href=\"#\" style=\"cursor:pointer\" onclick=\"return ShowPopupModal('" + urlNetDetail + "','auto');\">";
                dataItem["NetAmount"].Text = VeraCodeSolution.DoVeraCode(url + dataItem["NetAmount"].Text + "</a>");
                if (!GeneralFuncsLib.SHOW_PROCESSINGDATA_ENTITYNAME())
                {
                    if (GeneralFuncsLib.IsMerchantModeToSetTooltip(ReportFilter) && rowView.DataView.Table.Columns["EntityName"] != null)
                    {
                        dataItem["DrilldownColumn"].ToolTip = VeraCodeSolution.DoVeraCode(rowView["EntityName"].ToString());
                    }
                }
                dataItem["EntityName"].Text = VeraCodeSolution.DoVeraCode(rowView["EntityName"].ToString());
            }
            if (sender == uxBatchMerchantGrid && sender.Visible)
            {
                //handle for Net Amount
                string CardSummary_queryString = BuildSecureQueryString("BatchNumber=" + rowView["BatchNumber"] + "&TerminalNumber=" + rowView["TerminalNumber"] + "&HierarchyMode=" + ReportFilter.CurrentValue.HierarchyMode + "&HierarchyValue=" + ReportFilter.CurrentValue.Value + "&ReportDate=" + ((DateTime)rowView["ReportDate"]).ToShortDateString() + CardSummaryIntruderQuery);
                string urlNetAmountDetail = "CardSummaryModal.aspx?" + CardSummary_queryString;
                string urlNetAmount = "<a href=\"#\" style=\"cursor:pointer\" onclick=\"return ShowPopupModal('" + urlNetAmountDetail + "','auto');\">";
                dataItem["NetAmount"].Text = VeraCodeSolution.DoVeraCode(urlNetAmount + dataItem["NetAmount"].Text + "</a>");
            }
            if (sender == uxVoidedRejectedTransGrid && sender.Visible)
            {
                string queryString = String.Empty;

                queryString = BuildSecureQueryString("cn=" + rowView["PartialCardNumber"] + "&cnf=" + rowView["AccountNumber"] + "&merch=" + ReportFilter.CurrentValue.Value);
                string urlCardDetail = "CardHistoryModal.aspx?" + queryString;
                string urlCardSearch = "<a href=\"javascript:void(0)\" onclick=\"openPopupWindow('" + urlCardDetail + "','CardHistoryWindow'); return false;\">";

                if (CheckCSViewFullCard())
                {
                    if (dataItem["AccountNumber"].Text != "&nbsp;")
                    {
                        dataItem["AccountNumber"].Text = VeraCodeSolution.DoVeraCode(urlCardSearch + dataItem["AccountNumber"].Text + "</a>");
                    }
                }
                else
                {
                    if (dataItem["PartialCardNumber"].Text != "&nbsp;")
                    {
                        if (GeneralFuncsLib.HasIPForFullCard(this))
                        {
                            dataItem["PartialCardNumber"].Text = VeraCodeSolution.DoVeraCode(String.Format(IMAGE, BuildUrlForFullCard("VoidRejectDetail", rowView["RecordId"].ToString(), rowView["IssueBank"].ToString(), rowView["ReportDate"].ToString())) + urlCardSearch + rowView["PartialCardNumber"] + "</a>");
                        }
                        else
                        {
                            dataItem["PartialCardNumber"].Text = VeraCodeSolution.DoVeraCode(urlCardSearch + dataItem["PartialCardNumber"].Text + "</a>");
                        }
                    }
                }
                // }
                //handle for Auth Number
                if (!String.IsNullOrEmpty(rowView["AuthorizationNumber"].ToString()))
                {
                    dataItem["AuthNumber"].Text = VeraCodeSolution.DoVeraCode(
                        GeneralFuncsLib.BuildAuthUrlPopupModal(
                        (SecurePage)Page, dataItem["AuthNumber"].Text,
                        rowView["AuthorizationNumber"], AuthIntruderQuery)
                        );
                }
                //handle for Reason Code
                dataItem["CardType"].ToolTip = VeraCodeSolution.DoVeraCode(rowView["CardDescription"].ToString());
                dataItem["ReasonCode"].ToolTip = VeraCodeSolution.DoVeraCode(rowView["ReasonCodeDescription"].ToString());
                dataItem["TransactionCode"].ToolTip = VeraCodeSolution.DoVeraCode(rowView["TransactionCode"].ToString());
                dataItem["KeyedEntry"].ToolTip = VeraCodeSolution.DoVeraCode(rowView["EntryModeDescription"].ToString());

            }
            if (sender == uxTranQualificationGrid && sender.Visible)
            {
                string queryString = String.Empty;
                //handle for Auth Number
                if (!String.IsNullOrEmpty(rowView["AuthorizationNumber"].ToString()))
                {
                    dataItem["AuthNumber"].Text = VeraCodeSolution.DoVeraCode(
                        GeneralFuncsLib.BuildAuthUrlPopupModal(
                        (SecurePage)Page, dataItem["AuthNumber"].Text,
                        rowView["AuthorizationNumber"], AuthIntruderQuery_Qual)
                        );
                }

                queryString = BuildSecureQueryString("cn=" + rowView["PartialCardNumber"] + "&cnf=" + rowView["AccountNumber"] + "&merch=" + ReportFilter.CurrentValue.Value);
                string urlCardDetail = "CardHistoryModal.aspx?" + queryString;
                string urlCardSearch = "<a href=\"javascript:void(0)\" onclick=\"openPopupWindow('" + urlCardDetail + "','CardHistoryWindow'); return false;\">";

                if (CheckCSViewFullCard())
                {
                    if (dataItem["AccountNumber"].Text != "&nbsp;")
                    {
                        dataItem["AccountNumber"].Text = VeraCodeSolution.DoVeraCode(urlCardSearch + dataItem["AccountNumber"].Text + "</a>");
                    }
                }
                else
                {
                    if (dataItem["PartialCardNumber"].Text != "&nbsp;")
                    {
                        if (GeneralFuncsLib.HasIPForFullCard(this))
                        {
                            dataItem["PartialCardNumber"].Text = VeraCodeSolution.DoVeraCode(String.Format(IMAGE, BuildUrlForFullCard("TransQualificationDetail", rowView["RecordId"].ToString(), rowView["IssueBank"].ToString(), rowView["ReportDate"].ToString())) + urlCardSearch + rowView["PartialCardNumber"] + "</a>");
                        }
                        else
                        {
                            dataItem["PartialCardNumber"].Text = VeraCodeSolution.DoVeraCode(urlCardSearch + dataItem["PartialCardNumber"].Text + "</a>");
                        }
                    }
                }
                //handle for Reason Code
                dataItem["TransactionCode"].ToolTip = VeraCodeSolution.DoVeraCode(rowView["TransactionCode"].ToString().Trim());
                dataItem["QualificationCode"].ToolTip = VeraCodeSolution.DoVeraCode(rowView["QualificationDescription"].ToString());
                dataItem["FeeRate"].ToolTip = VeraCodeSolution.DoVeraCode(rowView["FeeRateDescription"].ToString());
                dataItem["ReasonCode"].ToolTip = VeraCodeSolution.DoVeraCode(rowView["ReasonCodeDescription"].ToString());

                uxToolTipMana.TargetControls.Add(dataItem["ReasonCode"].ClientID, rowView["ReasonCodeDescription"].ToString(), true);
            }
        }
    }

    protected override void DoGridDataSourceReady(ASGrid sender, EventArgs e)
    {
        base.DoGridDataSourceReady(sender, e);
        if (sender == uxDrilldownGrid && sender.Visible && sender.AS_TotalRecords > 0)
        {
            GridColumn column = null;
            decimal divideByZero = 0;
            //Calculate Key Percent per Page
            column = sender.Columns.FindByUniqueNameSafe("KeyEntryPercent");
            if (column != null)
            {
                divideByZero = Convert.ToDecimal(sender.AS_TotalPerPage["TransactionCount"]);
                if (divideByZero != 0)
                    sender.AS_TotalPerPage["KeyEntryPercent"] = Convert.ToDecimal(sender.AS_TotalPerPage["KeyCount"]) / divideByZero * 100;
            }
        }
    }

    protected void uxLinkButton_Command(object sender, CommandEventArgs e)
    {
        OnPostBackActions(PostBackAction.BatchDetailClick, sender);
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.BatchDetailClick:
                LinkButton link = (LinkButton)sender;
                string[] param = link.CommandArgument.ToString().Split(';');

                string activityText = string.Format(GetLocalResourceObject("BatchHistory_aspx_cs_BatchDetailBatchReportDate").ToString(), param[0], param[1]);
                GeneralFuncsLib.SaveUserActivity(this.ReportFilter.CurrentValue.Value, activityText);

                string queryString = "BatchDetailModal.aspx?" + this.BuildSecureQueryString("MerchantNumber=" + this.ReportFilter.CurrentValue.Value + "&ReportDate=" + param[1] + "&BatchNumber=" + param[0] + "&TerminalNumber=" + param[2] + "&Type=batchhistory" + BatchDetailIntruderQuery);
                this.AjaxAddResponseScript(string.Format("ShowPopupModal('{0}','auto');", queryString));
                break;
        }
    }
    bool _isExporting = false;
    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, AS.Controls.Exporter.ExportConfig exportConfig)
    {
        _isExporting = true;
        uxVoidedRejectedTransGrid.Columns.FindByUniqueName("AccountNumber").Visible = false;
        uxVoidedRejectedTransGrid.Columns.FindByUniqueName("PartialCardNumber").Visible = true;
        uxVoidedRejectedTransGrid.Columns.FindByUniqueName("ReasonCode").Visible = false;
        uxVoidedRejectedTransGrid.Columns.FindByUniqueName("ReasonCodeDescription").Visible = true;

        uxTranQualificationGrid.Columns.FindByUniqueName("AccountNumber").Visible = false;
        uxTranQualificationGrid.Columns.FindByUniqueName("PartialCardNumber").Visible = true;
        if (uxPanelHierarchy.Visible)
        {
            for (int i = 4; i < uxDrilldownGrid.Columns.Count; i++)
            {
                uxDrilldownGrid.Columns[i].HeaderText = uxDrilldownGrid.Columns[i].HeaderTooltip;
            }
                uxDrilldownGrid.DisplayEntityNameWhenExport();
                if (uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderText == GetLocalResourceObject("BatchHistory_aspx_cs_MerchantNumber").ToString()
                || uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderText == GetLocalResourceObject("BatchHistory_aspx_cs_Mertchant").ToString()
                || GeneralFuncsLib.IsMerchantMode(ReportFilter.CurrentValue.HierarchyMode))
                {
                    uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderStyle.Width = 130;
                    uxDrilldownGrid.Columns.FindByUniqueName("EntityName").HeaderText = GetLocalResourceObject("BatchHistory_aspx_cs_MerchantName").ToString();
                }
        }
        else
        {
            if ((uxPanelMerchantDetail.Visible) && (sender == uxExporterBatchMerchantGridTop))
            {
                uxBatchMerchantGrid.Columns.FindByUniqueName("BatchNumber").Visible = false;
                uxBatchMerchantGrid.Columns.FindByUniqueName("ExBatchNumber").Visible = true;

                for (int i = 7; i < uxBatchMerchantGrid.Columns.Count; i++)
                {
                    uxBatchMerchantGrid.Columns[i].HeaderText = uxBatchMerchantGrid.Columns[i].HeaderTooltip;
                }
            }

        }
        base.DoNeedExportConfig(sender, exportConfig);

        exportConfig.FileName = GeneralFuncsLib.FormatFileName(((AS.Controls.UserControls.UxExport)sender).GridHeader.ToString());
    }
    private string BuildUrlForFullCard(string reportType, string encryptedCC, string issueBank, string ReportDate)
    {
        string queryString = BuildSecureQueryString("rt=" + reportType + "&cn=" + Server.UrlEncode(encryptedCC) + "&issue=" + issueBank + "&reportdate=" + ReportDate);
        queryString = "FullCC.aspx?" + queryString;
        return queryString;
    }

    protected int getBatchMerchantGridColumns(int cols)
    {
        cols = cols + (uxDrilldownGrid.Columns.FindByUniqueName("EntityName").Visible == true ? 1 : 0);
        return cols;
    }
    private void HideVoiRejectSection()
    {
        var config = GeneralFuncsLib.GetDataOfExtendedSetting("HIDE_VOIREJECTTRANS_BATCHHISTORY");
        if (!string.IsNullOrEmpty(config))
        {
            uxVoidedRejectedTransGrid.Visible = !config.Equals("true", StringComparison.OrdinalIgnoreCase);
        }
        else
        {
            uxVoidedRejectedTransGrid.Visible = IsUserWithPermission("VoiRejDecRpt") || IsUserWithPermission("MSVoiRejDecRpt");       
        }
    }
    private void HideTransQualSection()
    {
        var config = GeneralFuncsLib.GetDataOfExtendedSetting("HIDE_TRANSQUAL_BATCHHISTORY");
        if (!string.IsNullOrEmpty(config))
        {
            uxTranQualificationGrid.Visible = !config.Equals("true", StringComparison.OrdinalIgnoreCase);
        }
        else
        {
            uxTranQualificationGrid.Visible = IsUserWithPermission("NonTransRpt") || IsUserWithPermission("MSNonTransRpt");
        }
    }
}
