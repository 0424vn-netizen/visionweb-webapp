using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Exporter;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Controls.UserControls;
using AS.Web.Business;
using AS.Web.UI.Controls;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using Telerik.Web.UI;

[PagePermission("RskPort,MSRskPort")]
public partial class rm_MCF_BatchDetailsModal : ReportPage
{
    #region Enums

    enum DataBindAction
    {
        BindBatchDetailGrid,
        BindTerminalComboBox,
        BindMerchantCard
    }

    #endregion Enums

    #region Fields

    private string _batchNumber = string.Empty;
    private string _terminalNumber = string.Empty;
    private DateTime _reportDate = DateTime.Today;
    private string _merchantNumber = string.Empty;
    private string _merchantName = string.Empty;
    private int _index = 0;
    private bool _isExporting = false;
    private string _voucherDetailIntruderQuery = string.Empty;

    #endregion Fields

    #region Constants

    // Terminal Combobox
    private const string TERMINAL_NUMBER_FIELD = "TerminalText";
    private const string TERMINAL_ID_FIELD = "TerminalNumber";
    private const string ALL_TERMINAL_TEXT = "All Terminals";
    // Title
    private string CARD_SUMMARY_TITLE = string.Empty;
    // SPAs
    private const string SPA_GET_BATCH_DETAILS = "spa_RM_MCF_GetBatchDetails";
    private const string SPA_GET_CARD_SUMMARY = "spa_ms_GetCardSummary";
    private const string SPA_GET_TERMINAL_NUMBER = "spa_GetTerminalNumber";
    // Columns
    private const string TERMINAL_COL_CARD_SUMMARY = "TerminalNr";
    private const string TERMINAL_COL_BATCH_DETAIL = "TerminalNr";
    private const string VOUCHER_COL = "Voucher";
    private const string ACCOUNT_NR_COL = "AccountNumber";
    private const string PARTIAL_ACCOUNT_NR_COL = "PartialAccountNumber";
    private const string RECORD_ID_COL = "RecordId";
    private const string ISSUE_BANK_COL = "IssuingBank";
    private const string REPORT_DATE_COL = "ReportDate";
    private const string AUTH_NR_COL = "AuthorizationNumber";
    private const string TRANS_DATE_COL = "TransactionDate";
    private const string TRANS_ID_COL = "TransactionID";

    #endregion Constants

    #region Properties

    private bool EnableTerminalComboBox
    {
        get
        {
            return GeneralFuncsLib.EnableTerminalCombobox();
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

    #endregion Properties

    #region Methods

    #region Protected Methods

    protected override void PageInitialize()
    {
        if (IsIntruderDetected)
            return;
        this.GridIDs.Add("uxBatchDetailGrid");
        this.GridIDs.Add("uxCardMerchantGrid");
        this.ExporterIDs.Add("uxExporterTop");
        this.ExporterIDs.Add("uxExportCardSummaryTop");
        base.PageInitialize();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        CARD_SUMMARY_TITLE = GetLocalResourceObject("rm_BatchDetailsModal_aspx_cs_CARD_SUMMARY_TITLE").ToString();
        PageType = SecurePageType.Modal;
        IsBindDataOnLoad = true;
        ProcessQueryString();
        _merchantName = GeneralFuncsLib.GetMerchantName(_merchantNumber);
        if (!IsPostBack)
        {
            // Set Title & SubTile for Batch Detail Grid. We set here 
            // because they don't change when Terminal Number changes)
            SetTitleForBatchDetailGrid();

            uxCardSummary.Visible = EnableTerminalComboBox;
            uxComboTerminalPlaceHolder.Visible = EnableTerminalComboBox;
            if (EnableTerminalComboBox)
            {
                OnDataBindControls(DataBindAction.BindTerminalComboBox);
            }
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }

    protected override void DoNeedExportConfig(UxExport sender, ExportConfig exportConfig)
    {
        _isExporting = true;
        base.DoNeedExportConfig(sender, exportConfig);
        switch (sender.GridID)
        {
            case "uxBatchDetailGrid":
                {
                    SetVisibleBatchDetailColumn();
                    SetHeaderExportFile(sender, exportConfig, false, GetLocalResourceObject("rm_BatchDetailsModal_aspx_cs_ExportTitle1").ToString());
                    exportConfig.FileName = "RiskManagement-RiskAnalysis-BatchDetails";
                    break;
                }
            case "uxCardMerchantGrid":
                {
                    SetVisibleCardSummaryColumn();
                    SetHeaderExportFile(sender, exportConfig, true, GetLocalResourceObject("rm_BatchDetailsModal_aspx_cs_ExportTitle2").ToString());
                    exportConfig.FileName = "RiskManagement-RiskAnalysis-BatchDetailsCardSummary";
                    break;
                }
        }

    }

    protected override void DoGridNeedDataSource(ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        if (sender == uxBatchDetailGrid)
        {
            OnDataBindControls(DataBindAction.BindBatchDetailGrid, sender);
        }
        else if (sender == uxCardMerchantGrid && uxCardMerchantGrid.Visible)
        {
            OnDataBindControls(DataBindAction.BindMerchantCard, sender);
        }
    }

    protected override void DoGridDataSourceReady(ASGrid sender, EventArgs e)
    {
        if (sender == uxBatchDetailGrid)
        {
            uxExporterTop.Visible = (uxBatchDetailGrid.DataSource as DataTable).Rows.Count > 0;
        }
        else if (sender == uxCardMerchantGrid)
        {
            uxExportCardSummaryTop.Visible = (uxCardMerchantGrid.DataSource as DataTable).Rows.Count > 0;
        }
    }

    protected override void DoSwitchView()
    {
        if (GeneralFuncsLib.CheckCSViewFullCard((SecurePage)this.Page))
        {
            uxBatchDetailGrid.Columns.FindByUniqueName(ACCOUNT_NR_COL).Visible = true;
            uxBatchDetailGrid.Columns.FindByUniqueName(PARTIAL_ACCOUNT_NR_COL).Visible = false;
        }
        else
        {
            uxBatchDetailGrid.Columns.FindByUniqueName(ACCOUNT_NR_COL).Visible = false;
            uxBatchDetailGrid.Columns.FindByUniqueName(PARTIAL_ACCOUNT_NR_COL).Visible = true;
        }
    }

    protected override void DoItemDataBound(ASGrid sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView dataRow = e.Item.DataItem as DataRowView;
            if (sender == uxBatchDetailGrid && sender.Visible)
            {
                dataItem["CardType"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["CardDescription"].ToString());
                dataItem["TransactionCode"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["TransactionCode"].ToString());
                //dataItem["Keyed"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["KeyedText"].ToString());
                dataItem["KeyedEntry"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["EntryModeDescription"].ToString());
                // Card Number
                if (GeneralFuncsLib.CheckCSViewFullCard((SecurePage)this.Page))
                {
                    dataItem[ACCOUNT_NR_COL].Text = VeraCodeSolution.DoVeraCode(
                        GeneralFuncsLib.BuildRiskUrlForCardNumber(
                        (SecurePage)this.Page, dataRow[PARTIAL_ACCOUNT_NR_COL],
                        dataRow[ACCOUNT_NR_COL], _merchantNumber, dataItem[ACCOUNT_NR_COL].Text)
                        );
                }
                else
                {

                    if (GeneralFuncsLib.HasIPForFullCard((SecurePage)this))
                    {
                        dataItem[PARTIAL_ACCOUNT_NR_COL].Text = VeraCodeSolution.DoVeraCode(
                            GeneralFuncsLib.BuildRiskUrlForFullCard(
                            (SecurePage)this.Page, ReportType.TRANSACTION_DETAIL,
                            dataRow[RECORD_ID_COL].ToString(), dataRow[ISSUE_BANK_COL].ToString(),
                            dataRow[REPORT_DATE_COL].ToString(), dataRow[PARTIAL_ACCOUNT_NR_COL],
                            dataRow[ACCOUNT_NR_COL], _merchantNumber, dataRow[PARTIAL_ACCOUNT_NR_COL].ToString(),
                            _index + 1, true)
                            );
                    }
                    else
                    {
                        dataItem[PARTIAL_ACCOUNT_NR_COL].Text = VeraCodeSolution.DoVeraCode(
                            GeneralFuncsLib.BuildRiskUrlForCardNumber(
                            (SecurePage)this.Page, dataRow[PARTIAL_ACCOUNT_NR_COL],
                            dataRow[ACCOUNT_NR_COL], _merchantNumber, dataItem[PARTIAL_ACCOUNT_NR_COL].Text)
                            );
                    }

                }

                // Auth column
                if (!dataRow[AUTH_NR_COL].ToString().Trim().IsNullOrEmpty())
                {
                    dataItem[AUTH_NR_COL].Text = VeraCodeSolution.DoVeraCode(
                        GeneralFuncsLib.BuildRskAuthUrlPopupModalChild(
                        (SecurePage)this.Page, dataRow[AUTH_NR_COL], _merchantNumber,
                        _merchantName, dataRow[TRANS_DATE_COL], _index + 1,
                        dataRow[AUTH_NR_COL].ToString(), true, true)
                        );
                }

                // Voucher column
                dataItem[VOUCHER_COL].Text = VeraCodeSolution.DoVeraCode(
                    GeneralFuncsLib.BuildVoucherUrl(
                    (SecurePage)this.Page, dataRow[TRANS_ID_COL], _merchantNumber,
                    (_index + 1), _reportDate, VoucherDetailIntruderQuery, true,
                    dataRow[VOUCHER_COL].ToString())
                    );
            }
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindBatchDetailGrid:
                {
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserReportingParams(false);
                    if (GeneralFuncsLib.CheckCSViewFullCard((SecurePage)this.Page))
                    {
                        parameters.AddDecryptDataParams("AccountNumber", _isExporting);
                    }
                    parameters.Add(new FilterParameter("@HierarchyFilterMode", GeneralFuncsLib.GetMerchantHierarchyInfo().HierarchyMode, DbType.AnsiString));
                    parameters.Add(new FilterParameter("@HierarchyFilterValue", _merchantNumber, DbType.AnsiString));
                    parameters.Add(new FilterParameter("@BatchNumber", _batchNumber, DbType.AnsiString));
                    parameters.Add(new FilterParameter("@ReportDate", _reportDate, DbType.DateTime));
                    parameters.Add(new FilterParameter("@TerminalNumber", SelectedTerminalNumber, DbType.AnsiString));
                    parameters.AddLanguageID();
                    ((ASGrid)sender).DataSourceInvoker = new ASFuncInvoker(
                        WebServices.RiskServices,
                        WebSiteConstants.GET_REPORT_METHOD_NAME,
                        new object[] { SPA_GET_BATCH_DETAILS, ReportServices.ConvertToFilterParamWSArray(parameters) });

                    // TK#24622: "Terminal #" column is visible when there is more than one Terminal Number.
                    SetVisibleBatchDetailColumn();
                }
                break;
            case DataBindAction.BindTerminalComboBox:
                {
                    DataBindComboTerminal();
                }
                break;
            case DataBindAction.BindMerchantCard:
                {
                    if (uxCardSummary.Visible)
                    {
                        FilterParameterCollection parameters = new FilterParameterCollection();
                        parameters.AddLoggedInUserReportingParams();
                        parameters.Add("@HierarchyFilterMode", HierarchyMode.MERCHANT_NR, DbType.AnsiString);
                        parameters.Add("@HierarchyFilterValue", _merchantNumber, System.Data.DbType.String);
                        parameters.Add("@DateFilterMode", GetDateModeForNoneReport(), DbType.Int32);
                        parameters.Add("@BeginDate", _reportDate, DbType.DateTime);
                        parameters.Add("@EndDate", _reportDate, DbType.DateTime);
                        parameters.Add("@BatchNumber", _batchNumber, DbType.String);
                        parameters.Add("@TerminalNumber", SelectedTerminalNumber, DbType.String);
                        parameters.AddLanguageID();
                        (sender as ASGrid).DataSourceInvoker = new ASFuncInvoker(
                            WebServices.CsReportServices,
                            WebSiteConstants.GET_REPORT_METHOD_NAME,
                            new object[] {
                                SPA_GET_CARD_SUMMARY,
                                ReportServices.ConvertToFilterParamWSArray(parameters)
                            }
                        );
                        SetTitleForCardSummaryGrid();
                        SetVisibleCardSummaryColumn();
                    }
                }
                break;
        }
    }

    protected void uxComboTerminal_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
    {
        // Re-bind data
        uxBatchDetailGrid.Rebind();
        uxCardMerchantGrid.Rebind();
    }

    #endregion Protected Methods

    #region Private Methods

    private void ProcessQueryString()
    {
        _merchantNumber = SecureQueryString["merch"];
        if (string.IsNullOrEmpty(_merchantNumber) && SessionManager.CurrentMerchantNumber != null)
        {
            _merchantNumber = SessionManager.CurrentMerchantNumber.Trim();
        }
        _batchNumber = SecureQueryString["BatchNumber"];
        _terminalNumber = SecureQueryString["TerminalNumber"];
        _reportDate = DateTime.Parse(SecureQueryString["ReportDate"]);

    }

    private string VoucherDetailIntruderQuery
    {
        get
        {
            if (_voucherDetailIntruderQuery == string.Empty)
                _voucherDetailIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(uxBatchDetailGrid.IntruderSourceName, new string[] { "TransactionID" });
            return _voucherDetailIntruderQuery;
        }
    }
    
    private void DataBindComboTerminal()
    {
        uxComboTerminal.DataTextField = TERMINAL_NUMBER_FIELD;
        uxComboTerminal.DataValueField = TERMINAL_ID_FIELD;

        // Load data from database
        var parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();
        parameters.Add("@DateFilterMode", GetDateModeForNoneReport(), DbType.Int32);
        parameters.Add("@BeginDate", _reportDate, DbType.DateTime);
        parameters.Add("@EndDate", _reportDate, DbType.DateTime);
        parameters.Add("@BatchNumber", _batchNumber, DbType.String);
        parameters.Add("@MerchantNumber", _merchantNumber, DbType.String);
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
        uxComboTerminal.SelectedIndex = 0;
    }

    private string GetBatchDetailGridTitle()
    {
        return _merchantNumber + (!_merchantName.IsNullOrEmpty() ? " - " + _merchantName : string.Empty);
    }

    private string GetGridSubTitle(bool includeTerminalNumber)
    {
        var gridHeader = string.Format(GetLocalResourceObject("rm_BatchDetailsModal_aspx_cs_Header1").ToString(),
            _batchNumber, _reportDate.ToShortDateString());

        // TK#24622: Add "Terminal Number" into GridHeader
        gridHeader += EnableTerminalComboBox && includeTerminalNumber
            ? string.Format(" " + GetLocalResourceObject("rm_BatchDetailsModal_aspx_cs_Header2").ToString(),
                            uxComboTerminal.SelectedIndex == 0 ? "ALL" : uxComboTerminal.SelectedValue)
            : string.Empty;
        return gridHeader;
    }

    private void SetVisibleBatchDetailColumn()
    {
        if (_isExporting)
        {
            uxBatchDetailGrid.Columns.FindByUniqueName(VOUCHER_COL).Visible = false;
            uxBatchDetailGrid.Columns.FindByUniqueName(ACCOUNT_NR_COL).Visible = false;
            uxBatchDetailGrid.Columns.FindByUniqueName(PARTIAL_ACCOUNT_NR_COL).Visible = true;
        }
        uxBatchDetailGrid.Columns.FindByUniqueName(TERMINAL_COL_BATCH_DETAIL).Visible =
                uxComboTerminalPlaceHolder.Visible && uxComboTerminal.Items.Count > 2;
    }

    private void SetVisibleCardSummaryColumn()
    {
        uxCardMerchantGrid.Columns.FindByUniqueName(TERMINAL_COL_CARD_SUMMARY).Visible =
            !SelectedTerminalNumber.IsNullOrEmpty();
    }

    private void SetTitleForBatchDetailGrid()
    {
        uxMerchantInfo.Text = VeraCodeSolution.DoVeraCode(GetBatchDetailGridTitle());
        uxExporterTop.GridSubTitle = VeraCodeSolution.ValidateResponseData(GetGridSubTitle(false));
    }

    private void SetHeaderExportFile(UxExport sender, ExportConfig exportConfig,
        bool includeTerminalNumber, string pageTitle)
    {
        // Set Terminal Header
        var terminalHeader = EnableTerminalComboBox && includeTerminalNumber
            ? string.Format(" " + GetLocalResourceObject("rm_BatchDetailsModal_aspx_cs_Header2").ToString(),
                            uxComboTerminal.SelectedIndex == 0 ? "ALL" : uxComboTerminal.SelectedValue)
            : string.Empty;

        if (sender.ExportButtonType == AS.Controls.UserControls.UxExport.ExportType.Excel)
        {
            exportConfig.ReportHeader =
                string.Format(GetLocalResourceObject("rm_BatchDetailsModal_aspx_cs_Header3").ToString(), pageTitle,
                uxMerchantInfo.Text, _batchNumber, _reportDate.ToShortDateString(), terminalHeader, "\r\n");
        }
        else
        {
            exportConfig.ReportHeader =
                string.Format(GetLocalResourceObject("rm_BatchDetailsModal_aspx_cs_Header4").ToString(), pageTitle +
                              Environment.NewLine, uxMerchantInfo.Text,
                              Environment.NewLine, _batchNumber,
                              _reportDate.ToShortDateString(),
                              terminalHeader);
        }
    }

    private void SetTitleForCardSummaryGrid()
    {
        uxExportCardSummaryTop.GridTitle = VeraCodeSolution.ValidateResponseData(CARD_SUMMARY_TITLE);
        uxExportCardSummaryTop.GridSubTitle = VeraCodeSolution.ValidateResponseData(GetGridSubTitle(true));
    }

    private int GetDateModeForNoneReport()
    {
        // Get DateMode
        int dateMode = (int)DateOptionMode.DateRange;
        if (this.SavedReportFilterValue != null)
        {
            dateMode = (int)this.SavedReportFilterValue.DateOption;
        }

        return dateMode;
    }
    #endregion Private Methods

    #endregion Methods
}