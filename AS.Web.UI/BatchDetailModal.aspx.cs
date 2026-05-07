using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Exporter;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Controls.UserControls;
using AS.Web.Business;
using System;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class BatchDetailModal : ReportPage
{
    #region Enums

    enum DataBindAction
    {
        BindReportGrid,
        BindMerchantCard,
        BindTransactionGrid,
        BindTerminalComboBox,
    }

    #endregion Enums

    #region Constants

    private const string IMAGE =
        "<a href=\"#\" style=\"cursor:pointer\" onclick=\" return parent.ShowPopupModalChild({0}, '{1}','auto');\"><img src='res/images/information.gif' border=\"0\"/></a>&nbsp; &nbsp;";
    private const string TERMINAL_NUMBER_FIELD = "TerminalText";
    private const string TERMINAL_ID_FIELD = "TerminalNumber";
    private string ALL_TERMINAL_TEXT = string.Empty;
    private const string TERMINAL_COL_CARD_SUMMARY = "TerminalNr";

    private const string SPA_GET_TERMINAL_NUMBER = "spa_GetTerminalNumber";
    private const string SPA_GET_BATCH_DETAIL = "spa_GetBatchDetail";
    private const string SPA_GET_VOID_REJECT_DETAIL = "spa_GetVoidRejectDetail";
    private const string SPA_MS_GET_CARD_SUMMARY = "spa_ms_GetCardSummary";

    private string VOIDED_REJECTED_TRANS_TITLE = string.Empty;
    private string CARD_SUMMARY_TITLE = string.Empty;

    // Column name
    private const string ACCOUNT_NUMBER = "AccountNumber";
    private const string PARTIAL_CARD_NUMBER = "PartialCardNumber";
    private const string RECORD_ID = "RecordId";
    private const string ISSUE_BANK = "IssueBank";
    private const string REPORT_DATE = "ReportDate";
    private const string AUTHORIZATION_NUMBER = "AuthorizationNumber";
    private const string VOUCHER = "Voucher";
    private const string CARD_DESC = "CardDescription";
    private const string CARD_TYPE = "CardType";
    private const string EXP_DATE = "ExpirationDate";
    private const string TRANSACTION_CODE = "TransactionCode";

    private const string TRANSACTION_ID = "TransactionID";
    private const string TRANS_ACCOUNT_NUMBER = "AccountNumber";
    private const string TRANS_PARTIAL_CARD_NUMBER = "PartialCardNumber";
    private const string TRANS_RECORD_ID = "RecordId";
    private const string TRANS_ISSUE_BANK = "IssueBank";
    private const string TRANS_REPORT_DATE = "ReportDate";
    private const string TRANS_AUTHORIZATION_NUMBER = "AuthorizationNumber";
    private const string TRANS_CARD_DESC = "CardDescription";
    private const string TRANS_CARD_TYPE = "CardType";
    private const string TRANS_RC_DESC = "ReasonCodeDescription";
    private const string TRANS_RC = "ReasonCode";
    private const string KEYED_ENTRY = "KeyedEntry";
    private const string ENTRY_MODE_DESCRIPTION = "EntryModeDescription";

    #endregion Constants

    #region Fields

    private string _batchNumber = string.Empty;
    private bool _parentIsRisk = false;
    private string _terminalNumber = string.Empty;
    private DateTime _reportDate = DateTime.Today;
    private string _sourceName = string.Empty;
    private string _keyName = string.Empty;
    private string _merchantNumber = string.Empty;
    private string _target = "_parent";
    private int _curIdx = 0;
    private bool _isExporting = false;

    #endregion Fields

    #region Properties

    private string _GridTitle
    {
        get
        {
            return GeneralFuncsLib.GetFullGridTitleName(ReportFilter) + " " + GeneralFuncsLib.GetDateFilterText(ReportFilter);
        }
    }

    private bool EnableTerminalComboBox
    {
        get
        {
            // 57499: show Terminal ComboBox for all clients
            return true;
        }
    }

    private string SelectedTerminalNumber
    {
        get
        {
            return uxComboTerminalPlaceHolder.Visible && uxComboTerminal.SelectedIndex > 0
                ? uxComboTerminal.SelectedValue : string.Empty;
        }
    }

    private bool IsNotInMif
    {
        get
        {
            if (SecureQueryString["isNotInMif"].IsNotNullData() && SecureQueryString["isNotInMif"].ToLower().Equals("true"))
                return true;
            return false;
        }
    }

    private bool IsShowRoutingAccount
    {
        get
        {
            if (ViewState["ShowRoutingAccount"] != null)
                return (bool)(ViewState["ShowRoutingAccount"]);
            else
            {
                ViewState["ShowRoutingAccount"] = GeneralFuncsLib.Show_RoutingAccountNumber;
                return (bool)ViewState["ShowRoutingAccount"];
            }
        }
        set
        {
            ViewState["ShowRoutingAccount"] = value;
        }
    }
    #endregion Properties

    #region Methods

    #region Protected Methods

    protected override void PageInitialize()
    {
        //grids
        this.GridIDs.Add("uxTransaction");
        this.GridIDs.Add("uxCardMerchantGrid");
        //exporters
        this.ExporterIDs.Add("uxExportTransaction");
        this.ExporterIDs.Add("uxExportCardSummaryTop");

        this.IsBindDataOnLoad = true;
        base.PageInitialize();
    }

    protected override void DoSwitchView()
    {
        if (GeneralFuncsLib.CheckCSViewFullCard((SecurePage)this.Page))
        {
            uxReportGrid.Columns.FindByUniqueName(ACCOUNT_NUMBER).Visible = true;
            uxReportGrid.Columns.FindByUniqueName(PARTIAL_CARD_NUMBER).Visible = false;
            uxTransaction.Columns.FindByUniqueName(TRANS_ACCOUNT_NUMBER).Visible = true;
            uxTransaction.Columns.FindByUniqueName(TRANS_PARTIAL_CARD_NUMBER).Visible = false;
            if (IsShowRoutingAccount)
            {
                uxReportGrid.Columns.FindByUniqueName("RoutingAccountNumber").Visible = true;
                uxReportGrid.Columns.FindByUniqueName("PartialRoutingACC").Visible = false;
            }
        }
        else
        {
            uxReportGrid.Columns.FindByUniqueName(ACCOUNT_NUMBER).Visible = false;
            uxReportGrid.Columns.FindByUniqueName(PARTIAL_CARD_NUMBER).Visible = true;
            uxTransaction.Columns.FindByUniqueName(TRANS_ACCOUNT_NUMBER).Visible = false;
            uxTransaction.Columns.FindByUniqueName(TRANS_PARTIAL_CARD_NUMBER).Visible = true;
            if (IsShowRoutingAccount)
            {
                uxReportGrid.Columns.FindByUniqueName("RoutingAccountNumber").Visible = false;
                uxReportGrid.Columns.FindByUniqueName("PartialRoutingACC").Visible = true;
            }
        }

        HideVoiRejectSection();

        // TK26080: Hide the Transaction Time and Exp Date columns under Batch Detail for WRFC view on VW MS
        string[] hiddenColumns = GeneralFuncsLib.GetHiddenColumnsOfBatchDetailModal();
        for (int i = 0; i < hiddenColumns.Length; i++)
        {
            if (uxReportGrid.Columns.FindByUniqueName(hiddenColumns[i]) != null)
            {
                uxReportGrid.Columns.FindByUniqueName(hiddenColumns[i]).Visible = false;
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;
        CARD_SUMMARY_TITLE = GetLocalResourceObject("BatchDetailModal_aspx_cs_Text_BarchDetailsCardSum").ToString();
        VOIDED_REJECTED_TRANS_TITLE = GetLocalResourceObject("BatchDetailModal_aspx_cs_Text_VoidedRejectedTransaction").ToString();
        ALL_TERMINAL_TEXT = GetLocalResourceObject("BatchDetailModal_aspx_cs_Text_AllTerminals").ToString();
        if (SessionManager.ClientFrameInfo != string.Empty)
        {
            _target = SessionManager.ClientFrameInfo;
        }
        if (IsIntruderDetected)
        {
            return;
        }
        ProcessQueryString();
        IsBindDataOnLoad = true;
        if (!IsPostBack)
        {
            ltrMerchantInfor.Text = VeraCodeSolution.DoVeraCode(GetLocalResourceObject("BatchDetailModal_aspx_cs_Merchant").ToString() + " " + GridTitle());
            uxComboTerminalPlaceHolder.Visible = EnableTerminalComboBox;
            if (EnableTerminalComboBox)
            {
                ltrMerInfoCardSummary.Text = VeraCodeSolution.DoVeraCode(GetLocalResourceObject("BatchDetailModal_aspx_cs_Merchant").ToString() + " " + GridTitle());
                OnDataBindControls(DataBindAction.BindTerminalComboBox);
            }
        }
        uxPrinterpnl.Visible = GeneralFuncsLib.GetDataOfExtendedSetting("SHOW_PRINTER_LINK") == "true";
        //46652 - AW Multi-currency Transaction Display
        GeneralFuncsLib.ShowHideAWTransactionDetail(uxReportGrid);
        GeneralFuncsLib.ShowHideAWTransactionDetail(uxTransaction);
    }

    protected override void DoGridNeedDataSource(AS.Controls.Grid.ASGrid sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if (sender == uxReportGrid && uxReportGrid.Visible)
        {
            OnDataBindControls(DataBindAction.BindReportGrid, sender);
        }
        else if (sender == uxCardMerchantGrid && uxCardMerchantGrid.Visible)
        {
            OnDataBindControls(DataBindAction.BindMerchantCard, sender);
        }
        else if (sender == uxTransaction && uxTransaction.Visible)
        {
            OnDataBindControls(DataBindAction.BindTransactionGrid, sender);
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();
        parameters.Add("@HierarchyFilterMode", HierarchyMode.MERCHANT_NR, DbType.AnsiString);
        parameters.Add("@HierarchyFilterValue", _merchantNumber, System.Data.DbType.String);
        parameters.Add("@DateFilterMode", this.SavedReportFilterValue.DateOption, DbType.Int32);
        parameters.Add("@BeginDate", _reportDate, DbType.DateTime);
        parameters.Add("@EndDate", _reportDate, DbType.DateTime);
        parameters.Add("@BatchNumber", _batchNumber, DbType.String);
        _batchNumber = SecureQueryString["BatchNumber"];
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindReportGrid:
                {
                    parameters.AddLoggedInUserPrimaryUserID();
                    parameters.Add("@RiskClient", false, DbType.Boolean);
                    parameters.Add("@TerminalNumber", SelectedTerminalNumber, DbType.String);
                    parameters.Add("@IsNotInMif", IsNotInMif, DbType.Boolean);
                    if (GeneralFuncsLib.CheckCSViewFullCard((SecurePage)this.Page))
                    {
                        parameters.AddDecryptDataParams("AccountNumber", _isExporting);
                        if (IsShowRoutingAccount)
                            parameters.AddDecryptDataParams("RoutingAccountNumber", _isExporting);
                    }
                    parameters.AddLanguageID();
                    (sender as ASGrid).DataSourceInvoker = new ASFuncInvoker(
                        WebServices.CsReportServices,
                        WebSiteConstants.GET_REPORT_METHOD_NAME,
                        new object[] { SPA_GET_BATCH_DETAIL, ReportServices.ConvertToFilterParamWSArray(parameters) });
                    uxExporter.GridSubTitle = VeraCodeSolution.ValidateResponseData(GetGridSubTitle(false));
                }
                break;
            case DataBindAction.BindTransactionGrid:
                {
                    parameters.AddLoggedInUserPrimaryUserID();
                    parameters.Add("@RiskClient", false, DbType.Boolean);
                    if (GeneralFuncsLib.CheckCSViewFullCard((SecurePage)this.Page))
                    {
                        parameters.AddDecryptDataParams("AccountNumber", _isExporting);
                    }
                    parameters.AddLanguageID();
                    (sender as ASGrid).DataSourceInvoker = new ASFuncInvoker(
                        WebServices.CsReportServices,
                        WebSiteConstants.GET_REPORT_METHOD_NAME,
                        new object[] { SPA_GET_VOID_REJECT_DETAIL,
                                       ReportServices.ConvertToFilterParamWSArray(parameters) });
                    uxExportTransaction.GridTitle = VeraCodeSolution.ValidateResponseData(
                        VOIDED_REJECTED_TRANS_TITLE);
                }
                break;
            case DataBindAction.BindMerchantCard:
                {
                    parameters.Add("@TerminalNumber", SelectedTerminalNumber, DbType.String);
                    parameters.Add("@IsNotInMif", IsNotInMif, DbType.Boolean);
                    // TK26080: WRFC: Set order of Card Type by configuration
                    parameters.Add("@CardTypeOrder", GeneralFuncsLib.GetCardTypeOrder(), DbType.String);
                    parameters.AddLanguageID();
                    (sender as ASGrid).DataSourceInvoker = new ASFuncInvoker(
                        WebServices.CsReportServices,
                        WebSiteConstants.GET_REPORT_METHOD_NAME,
                        new object[] { SPA_MS_GET_CARD_SUMMARY, ReportServices.ConvertToFilterParamWSArray(parameters) });

                    uxExportCardSummaryTop.GridTitle = VeraCodeSolution.ValidateResponseData(
                        CARD_SUMMARY_TITLE);
                    uxExportCardSummaryTop.GridSubTitle = VeraCodeSolution.ValidateResponseData(
                        GetGridSubTitle(true));
                    SetVisibleCardSummaryColumns();

                    // Set header for Card Summary Grid
                    string newHeader = EnableTerminalComboBox ? GetGridHeader(true)
                        : string.Format(GetLocalResourceObject("BatchDetailModal_aspx_cs_BDCSB").ToString(), _batchNumber);
                    uxExportCardSummaryTop.GridHeader
                        = VeraCodeSolution.ValidateResponseData(newHeader);

                    // TK#24622: Set visibility of "Terminal #" column
                    uxCardMerchantGrid.Columns.FindByUniqueName(TERMINAL_COL_CARD_SUMMARY).Visible = !SelectedTerminalNumber.IsNullOrEmpty();
                }
                break;
            case DataBindAction.BindTerminalComboBox:
                {
                    DataBindComboTerminal();
                }
                break;
        }
    }

    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, AS.Controls.Exporter.ExportConfig exportConfig)
    {
        base.DoNeedExportConfig(sender, exportConfig);
        _isExporting = true;
        switch (sender.GridID)
        {
            case "uxReportGrid":
                {
                    SetVisibleBatchDetailColumnWhenExporting();
                    //46652 - AW Multi-currency Transaction Display
                    GeneralFuncsLib.ShowHideAWTransactionDetail(uxReportGrid);

                    SetHeaderExportFile(sender, exportConfig, GetLocalResourceObject("BatchDetailModal_aspx_cs_BDs").ToString(), GetLocalResourceObject("BatchDetailModal_aspx_cs_bd").ToString(), true);
                    break;
                }
            case "uxTransaction":
                {
                    //46652 - AW Multi-currency Transaction Display
                    GeneralFuncsLib.ShowHideAWTransactionDetail(uxTransaction);
                    SetVisibleTransactionColumnWhenExporting();
                    sender.GridHeader = VeraCodeSolution.ValidateResponseData(GetCardSummaryHeader());
                    exportConfig.FileName = GeneralFuncsLib.GetFileName(uxExportTransaction.GridHeader);
                    break;
                }
            case "uxCardMerchantGrid":
                {
                    SetVisibleCardSummaryColumns();
                    if (EnableTerminalComboBox)
                    {
                        SetHeaderExportFile(sender, exportConfig, GetLocalResourceObject("BatchDetailModal_aspx_cs_BDCS").ToString(), GetLocalResourceObject("BatchDetailModal_aspx_cs_BDCS1").ToString(), true);
                    }
                    else
                    {
                        exportConfig.ReportHeader = VeraCodeSolution.ValidateResponseData(GetCardSummaryHeader());
                        exportConfig.FileName = GeneralFuncsLib.GetFileName(GetCardSummaryHeader());
                    }
                    break;
                }
        }
    }

    protected override void DoItemDataBound(ASGrid sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView dataRow = e.Item.DataItem as DataRowView;
            if (sender == uxReportGrid && sender.Visible)
            {
                //handle for CardType
                string cardDesc = dataRow[CARD_DESC].ToString();
                dataItem[CARD_TYPE].ToolTip = cardDesc.IsNullOrEmpty() ? string.Empty : cardDesc;

                // Handle for Account Number
                if (GeneralFuncsLib.CheckCSViewFullCard((SecurePage)this.Page)
                    && (!dataItem[ACCOUNT_NUMBER].Text.Equals(GeneralFuncsLib.NBSP)
                    || !dataItem["RoutingAccountNumber"].Text.Equals(GeneralFuncsLib.NBSP)))
                {
                    dataItem[ACCOUNT_NUMBER].Text = VeraCodeSolution.GetOutputHtmlString(GeneralFuncsLib.BuildUrlForNotInMifCardNumber(
                        (SecurePage)this.Page, dataRow[PARTIAL_CARD_NUMBER], dataRow[ACCOUNT_NUMBER],
                        _merchantNumber, dataItem[ACCOUNT_NUMBER].Text, IsNotInMif));

                    if(IsShowRoutingAccount)
                        dataItem["RoutingAccountNumber"].Text = VeraCodeSolution.GetOutputHtmlString(GeneralFuncsLib.BuildUrlForNotInMifCardNumber(
                            (SecurePage)this.Page, dataRow["PartialRoutingACC"], dataRow["RoutingAccountNumber"],
                            _merchantNumber, dataItem["RoutingAccountNumber"].Text, IsNotInMif));
                }
                else
                {
                    if (!dataItem[PARTIAL_CARD_NUMBER].Text.Equals(GeneralFuncsLib.NBSP) || !dataItem["PartialRoutingACC"].Text.Equals(GeneralFuncsLib.NBSP))
                    {
                        if (GeneralFuncsLib.HasIPForFullCard(this))
                        {
                            dataItem[PARTIAL_CARD_NUMBER].Text = GeneralFuncsLib.BuildUrlForNotInMifFullCard(
                                (SecurePage)this.Page, ReportType.TRANSACTION_DETAIL,
                                dataRow[RECORD_ID].ToString(), dataRow[ISSUE_BANK].ToString(),
                                dataRow[REPORT_DATE].ToString(), dataRow[PARTIAL_CARD_NUMBER],
                                dataRow[ACCOUNT_NUMBER], _merchantNumber, dataRow[PARTIAL_CARD_NUMBER].ToString(),
                                _curIdx + 1, true, IsNotInMif);

                            if (IsShowRoutingAccount)
                                dataItem["PartialRoutingACC"].Text = GeneralFuncsLib.BuildUrlForNotInMifFullCard(
                                (SecurePage)this.Page, ReportType.TRANSACTION_DETAIL,
                                dataRow[RECORD_ID].ToString(), dataRow[ISSUE_BANK].ToString(),
                                dataRow[REPORT_DATE].ToString(), dataRow["PartialRoutingACC"],
                                dataRow["RoutingAccountNumber"], _merchantNumber, dataRow["PartialRoutingACC"].ToString(),
                                _curIdx + 1, true, IsNotInMif);
                        }
                        else
                        {
                            dataItem[PARTIAL_CARD_NUMBER].Text = GeneralFuncsLib.BuildUrlForNotInMifCardNumber(
                                (SecurePage)this.Page, dataRow[PARTIAL_CARD_NUMBER],
                                dataRow[ACCOUNT_NUMBER], _merchantNumber, dataItem[PARTIAL_CARD_NUMBER].Text, IsNotInMif);

                            if (IsShowRoutingAccount)
                                dataItem["PartialRoutingACC"].Text = GeneralFuncsLib.BuildUrlForNotInMifCardNumber(
                                (SecurePage)this.Page, dataRow["PartialRoutingACC"],
                                dataRow["RoutingAccountNumber"], _merchantNumber, dataItem["PartialRoutingACC"].Text, IsNotInMif);
                        }
                    }
                }

                //handle for Auth
                if (!dataRow[AUTHORIZATION_NUMBER].ToString().Trim().IsNullOrEmpty())
                {
                    dataItem[AUTHORIZATION_NUMBER].Text = GeneralFuncsLib.BuildAuthUrlPopupModalChild(
                        (SecurePage)Page, dataRow[AUTHORIZATION_NUMBER], _merchantNumber,
                        _curIdx + 1, AuthIntruderQuery(uxReportGrid),
                        dataItem[AUTHORIZATION_NUMBER].Text, true, IsNotInMif);
                }

                //handle for Voucher
                if (!dataRow[TRANSACTION_ID].ToString().Trim().IsNullOrEmpty())
                {
                    dataItem[VOUCHER].Text = GeneralFuncsLib.BuildVoucherUrl(
                        (SecurePage)Page, dataRow[TRANSACTION_ID], _merchantNumber,
                        _curIdx + 1, _reportDate, string.Empty, _parentIsRisk,
                        dataItem[VOUCHER].Text, IsNotInMif);
                }

                dataItem[EXP_DATE].ToolTip = "MM/YY";
                dataItem[TRANSACTION_CODE].ToolTip = VeraCodeSolution.DoVeraCode(dataRow[TRANSACTION_CODE].ToString());
                dataItem[KEYED_ENTRY].ToolTip = VeraCodeSolution.DoVeraCode(dataRow[ENTRY_MODE_DESCRIPTION].ToString());
            }
            else if (sender == uxTransaction && sender.Visible)
            {
                // Card Type
                string cardDesc = dataRow[TRANS_CARD_DESC].ToString();
                dataItem[TRANS_CARD_TYPE].ToolTip = cardDesc.IsNullOrEmpty() ? string.Empty : cardDesc;

                if (GeneralFuncsLib.CheckCSViewFullCard((SecurePage)this.Page))
                {
                    dataItem[TRANS_ACCOUNT_NUMBER].Text = GeneralFuncsLib.BuildUrlForCardNumber(
                        (SecurePage)this.Page, dataRow[TRANS_PARTIAL_CARD_NUMBER],
                        dataRow[TRANS_ACCOUNT_NUMBER], _merchantNumber, dataItem[TRANS_ACCOUNT_NUMBER].Text);
                }
                else
                {
                    if (GeneralFuncsLib.HasIPForFullCard(this))
                    {
                        dataItem[TRANS_PARTIAL_CARD_NUMBER].Text = GeneralFuncsLib.BuildUrlForNotInMifFullCard(
                        (SecurePage)this.Page, ReportType.TRANSACTION_DETAIL,
                        dataRow[TRANS_RECORD_ID].ToString(), dataRow[TRANS_ISSUE_BANK].ToString(),
                        dataRow[TRANS_REPORT_DATE].ToString(), dataRow[TRANS_PARTIAL_CARD_NUMBER],
                        dataRow[TRANS_ACCOUNT_NUMBER], _merchantNumber, dataRow[TRANS_PARTIAL_CARD_NUMBER].ToString(),
                        _curIdx + 1, true, IsNotInMif);
                    }
                    else
                    {
                        dataItem[TRANS_PARTIAL_CARD_NUMBER].Text = GeneralFuncsLib.BuildUrlForNotInMifCardNumber(
                        (SecurePage)this.Page, dataRow[TRANS_PARTIAL_CARD_NUMBER],
                        dataRow[TRANS_ACCOUNT_NUMBER], _merchantNumber, dataItem[TRANS_PARTIAL_CARD_NUMBER].Text, IsNotInMif);
                    }
                }

                //handle for Auth
                if (!dataRow[TRANS_AUTHORIZATION_NUMBER].ToString().Trim().IsNullOrEmpty())
                {
                    dataItem[TRANS_AUTHORIZATION_NUMBER].Text = GeneralFuncsLib.BuildAuthUrlPopupModalChild(
                        (SecurePage)Page, dataRow[TRANS_AUTHORIZATION_NUMBER],
                        _merchantNumber, _curIdx + 1, AuthIntruderQuery(uxTransaction),
                        dataItem[AUTHORIZATION_NUMBER].Text, false, IsNotInMif);
                }

                //handle for Reason Code
                string reasonDesc = dataRow[TRANS_RC_DESC].ToString();
                dataItem[TRANS_RC].ToolTip = reasonDesc.IsNullOrEmpty() ? string.Empty : reasonDesc;
                dataItem[KEYED_ENTRY].ToolTip = VeraCodeSolution.DoVeraCode(dataRow[ENTRY_MODE_DESCRIPTION].ToString());
            }
        }
    }



    protected void uxComboTerminal_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        // Re-bind data
        uxReportGrid.Rebind();
        uxTransaction.Rebind();
        uxCardMerchantGrid.Rebind();
    }

    #endregion Protected Methods

    #region Private Methods

    private string AuthIntruderQuery(ASGrid grid)
    {
        return GeneralFuncsLib.BuildIntruderInfoForQueryStr(grid.ID, new string[] { "AuthorizationNumber" });
    }

    private string GridTitle()
    {
        string hierarchyValue = _merchantNumber.IsNullOrEmpty()
            ? this.SavedReportFilterValue.Value : _merchantNumber;
        string merName = GeneralFuncsLib.GetMerchantName(hierarchyValue, IsNotInMif);
        string gridTitle = merName.IsNullOrEmpty() ? hierarchyValue : string.Format("{0} - {1}", hierarchyValue, merName);
        return gridTitle;
    }

    private string HandleHeader(string hierarchyMode, string entityNumber)
    {
        return string.Format(GetLocalResourceObject("BatchDetailModal_aspx_cs_ReportPeriod").ToString(),
            this.SavedReportFilterValue.DateOptionValue.From.ToShortDateString(),
            this.SavedReportFilterValue.DateOptionValue.To.ToShortDateString());
    }

    private void DataBindComboTerminal()
    {
        uxComboTerminal.DataTextField = TERMINAL_NUMBER_FIELD;
        uxComboTerminal.DataValueField = TERMINAL_ID_FIELD;

        // Load data from database
        var parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();
        parameters.Add("@DateFilterMode", this.SavedReportFilterValue.DateOption, DbType.Int32);
        parameters.Add("@BeginDate", _reportDate, DbType.DateTime);
        parameters.Add("@EndDate", _reportDate, DbType.DateTime);
        parameters.Add("@BatchNumber", _batchNumber, DbType.String);
        parameters.Add("@MerchantNumber", _merchantNumber, DbType.String);
        parameters.Add("@IsNotInMif", IsNotInMif, DbType.Boolean);
        uxComboTerminal.DataSource = WebServices.CsReportServices.GetReports(
            SPA_GET_TERMINAL_NUMBER, parameters);
        uxComboTerminal.DataBind();

        // Add default item
        AS.Controls.Global.RadComboBoxItem AllTerminalItem = new AS.Controls.Global.RadComboBoxItem()
        {
            Text = ALL_TERMINAL_TEXT,
            Value = "0"
        };

        uxComboTerminal.Items.Insert(0, AllTerminalItem);
        if(string.IsNullOrEmpty(_terminalNumber))
        {
            uxComboTerminal.SelectedIndex = 0;
        }
        else
        {
            uxComboTerminal.SelectedValue = _terminalNumber;
        }
    }

    private void ProcessQueryString()
    {
        _merchantNumber = SecureQueryString["merchantnumber"];
        if (string.IsNullOrEmpty(_merchantNumber))
        {
            _merchantNumber = this.SavedReportFilterValue.Value.Trim();
        }

        _batchNumber = SecureQueryString["BatchNumber"];
        _terminalNumber = SecureQueryString["TerminalNumber"];
        if (SecureQueryString["ParentIsRisk"] != null)
        {
            _parentIsRisk = bool.Parse(SecureQueryString["ParentIsRisk"]);
        }
        _sourceName = SecureQueryString[WebSiteConstants.INTRUDER_SOURCE_PARAM_NAME];
        _keyName = SecureQueryString[WebSiteConstants.INTRUDER_KEY_PARAM_NAME];
        string Temp = this.SecureQueryString["ReportDate"];
        if (!DateTime.TryParse(Temp, out _reportDate))
        {
            IsIntruderDetected = true;
            return;
        }
    }

    private void SetHeaderExportFile(UxExport sender, ExportConfig exportConfig,
        string pageTitle, string prefixFileName, bool includeTerminalNumber)
    {
        string strHeader = string.Format("{0}:\r\n{1}\r\n{2}",
            pageTitle,
            ltrMerchantInfor.Text,
            GetGridSubTitle(includeTerminalNumber));

        // File Name
        string strFileName = ltrMerchantInfor.Text + "_" + GetGridSubTitle(includeTerminalNumber);
        strFileName = strFileName.Replace("<br/>", "");
        strFileName = strFileName.Replace("<strong>", "");
        strFileName = strFileName.Replace("</strong>", "");
        exportConfig.FileName = GeneralFuncsLib.GetFileName(string.Format("{0}_{1}", prefixFileName, strFileName));

        // Report Header
        if (sender.ExportButtonType == UxExport.ExportType.Excel)
        {
            exportConfig.ReportHeader = strHeader;
        }
        else
        {
            string strCSVHeader = strHeader;
            strCSVHeader = strCSVHeader.Replace("\r\n", Environment.NewLine);
            strCSVHeader = strCSVHeader.Replace("<strong>", "");
            strCSVHeader = strCSVHeader.Replace("</strong>", "");
            exportConfig.ReportHeader = strCSVHeader;
        }
    }

    private string GetCardSummaryHeader()
    {
        return string.Format(GetLocalResourceObject("BatchDetailModal_aspx_cs_Batch").ToString(), CARD_SUMMARY_TITLE, _batchNumber);
    }

    private string GetGridSubTitle(bool includeTerminalNumber)
    {
        string newHeader = String.Empty;
        FilterParameterCollection paras = new FilterParameterCollection();
        if (IsSecureQueryString)
        {
            string batchNumber = this.SecureQueryString["BatchNumber"];
            string entityNumber = this.SecureQueryString["Entity"];
            string hierarchyMode = this.SecureQueryString["HierarchyMode"];
            string terminalNumber = this.SecureQueryString["TerminalNumber"];
            if (!String.IsNullOrEmpty(batchNumber))
            {
                string batchDetails = string.Format(GetLocalResourceObject("BatchDetailModal_aspx_cs_BatchReportDate").ToString(), batchNumber, _reportDate.ToShortDateString());
                newHeader = batchDetails;
            }
            if (!String.IsNullOrEmpty(entityNumber) && !String.IsNullOrEmpty(hierarchyMode))
            {
                newHeader = HandleHeader(hierarchyMode, entityNumber);
            }

            // TK#24622: Add "Terminal Number" into GridHeader
            var textTerminal = uxComboTerminal.SelectedIndex == 0 ? GetLocalResourceObject("BatchDetailModal_aspx_cs_Text_All").ToString() : uxComboTerminal.SelectedValue;
            newHeader += EnableTerminalComboBox && includeTerminalNumber
                ? string.Format(" " + GetLocalResourceObject("BatchDetailModal_aspx_cs_TerminalNumber").ToString(), textTerminal)
                : string.Empty;
        }
        else
        {
            newHeader = HandleHeader(HierarchyMode.MERCHANT_NR, _merchantNumber);
        }

        return newHeader;
    }

    private void SetVisibleCardSummaryColumns()
    {
        // TK#24622: Set visibility of "Terminal #" column
        uxCardMerchantGrid.Columns.FindByUniqueName(TERMINAL_COL_CARD_SUMMARY).Visible
            = !SelectedTerminalNumber.IsNullOrEmpty();
    }

    private void SetVisibleBatchDetailColumnWhenExporting()
    {
        uxReportGrid.Columns.FindByUniqueName(ACCOUNT_NUMBER).Visible = false;
        uxReportGrid.Columns.FindByUniqueName(PARTIAL_CARD_NUMBER).Visible = true;
        uxReportGrid.Columns.FindByUniqueName(VOUCHER).Visible = false;
        if (IsShowRoutingAccount)
        {
            uxReportGrid.Columns.FindByUniqueName("RoutingAccountNumber").Visible = false;
            uxReportGrid.Columns.FindByUniqueName("PartialRoutingACC").Visible = true;
        }
    }

    private void SetVisibleTransactionColumnWhenExporting()
    {
        uxTransaction.Columns.FindByUniqueName(TRANS_ACCOUNT_NUMBER).Visible = false;
        uxTransaction.Columns.FindByUniqueName(TRANS_PARTIAL_CARD_NUMBER).Visible = true;
    }

    protected string GetGridHeader(bool includeTerminalNumber)
    {
        string newHeader = String.Empty;
        FilterParameterCollection paras = new FilterParameterCollection();
        if (IsSecureQueryString)
        {
            string batchNumber = this.SecureQueryString["BatchNumber"];
            string entityNumber = this.SecureQueryString["Entity"];
            string hierarchyMode = this.SecureQueryString["HierarchyMode"];
            string terminalNumber = this.SecureQueryString["TerminalNumber"];
            //string reportDate = this.SecureQueryString["ReportDate"];
            if (!String.IsNullOrEmpty(batchNumber))
            {
                //DateTime newreportDate = Convert.ToDateTime(reportDate);
                string batchDetails = GetLocalResourceObject("ASGridBoundColumnResource21.HeaderText").ToString() + ": " + batchNumber + "  " + GetLocalResourceObject("ASGridBoundColumnResource20.HeaderText").ToString() + ": " + _reportDate.ToShortDateString();
                newHeader = batchDetails;
            }
            if (!String.IsNullOrEmpty(entityNumber) && !String.IsNullOrEmpty(hierarchyMode))
            {
                newHeader = HandleHeader(hierarchyMode, entityNumber);
            }

            // TK#24622: Add "Terminal Number" into GridHeader
            var textTerminal = uxComboTerminal.SelectedIndex == 0 ? GetLocalResourceObject("BatchDetailModal_aspx_cs_Text_All").ToString() : uxComboTerminal.SelectedValue;
            newHeader += EnableTerminalComboBox && includeTerminalNumber
                ? string.Format(" " + GetLocalResourceObject("BatchDetailModal_aspx_cs_TerminalNumber").ToString(), textTerminal)
                : string.Empty;
        }
        else
        {
            newHeader = HandleHeader("MERCHANTNUMBER", _merchantNumber);
        }

        return newHeader;
    }
    private void HideVoiRejectSection()
    {
        var config = GeneralFuncsLib.GetDataOfExtendedSetting("HIDE_VOIREJECTTRANS_BATCHHISTORY");
        if (!string.IsNullOrEmpty(config))
        {
            uxTransaction.Visible = !config.Equals("true", StringComparison.OrdinalIgnoreCase);
        }
        else
        {
            uxTransaction.Visible = IsUserWithPermission("VoiRejDecRpt") || IsUserWithPermission("MSVoiRejDecRpt");
        }
    }

    #endregion Private Methods

    #endregion Methods
}
