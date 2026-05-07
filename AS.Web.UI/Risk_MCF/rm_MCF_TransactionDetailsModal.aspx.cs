using System;
using System.Drawing;
using System.Data;
using System.Web.UI;

using Telerik.Web.UI;
using AS.Controls.Grid;
using AS.Controls.Exporter;
using AS.Common.DBManager;
using AS.Common;
using AS.Controls.Pages;
using AS.Web.Business;
using AS.Controls.UserControls;
using AS.Common.Formater;
using AS.Utilities;

[PagePermission("RskQueue,RskAdhoc,MSRskQueue,MSRskAdhoc")]
public partial class rm_MCF_TransactionDetailsModal : ReportPage
{
    #region Enums

    enum DataBindAction
    {
        BindTransactionDetails
    }

    #endregion Enums

    #region Constants

    private const string MERCHANT_NUMBER = "MerchantNumber";
    private const string REPORT_DATE = "ReportDate";
    private const string TRANSACTION_AMOUNT = "TransactionAmount";

    private const string NO_MATCH_FLAG = "NO MATCH";
    private const string MECHANT_VERIFIED_MATCH_FLAG = "MERCHANT VERIFIED";
    private const string PARTIAL_MATCH_FLAG = "PARTIAL";
    private const string FULL_MATCH_FLAG = "FULL";
    // SPAs
    private const string SPA_GET_TRANSACTION_DETAIL = "spa_RM_MCF_GetTransactionDetail";
    // Columns
    private const string ACCOUNT_NR_COL = "FullAccountNumber";
    private const string PARTIAL_ACCOUNT_NR_COL = "AccountNumber";
    private const string RECORD_ID_COL = "RecordId";
    private const string ISSUE_BANK_COL = "IssuingBank";
    private const string REPORT_DATE_COL = "ReportDate";
    private const string MERCHANT_NR_COL = "MerchantNumber";
    private const string MATCHED_FLAG_COL = "MatchedFlag";

    private const string MATCHED_CODE = "MatchCode";
    private const string MATCHED_NAME = "MatchName";


    private const string COLOR_URL_TEMPLATE = "<span style='border-bottom: 2px {0} solid !important'>{1}</span>";
    private const string COLOR_URL_TEMPLATE_TRANS = "<span style='border-bottom: 2px {0} solid !important; color: {2} !important'>{1}</span>";
    #endregion Constants

    #region Fields

    private int index = 0;
    private string _currentSortExpr = string.Empty;
    private string _currentSortOrder = string.Empty;
    private string _currentSortControls = string.Empty;
    private string _merchantNumber = string.Empty;
    private DateTime _reportDate;
    public int TotalRows;
    private string _partialCardSearchIntruderQuery = string.Empty;
    private string _authIntruderQuery = string.Empty;
    private bool _isExporting = false;

    #endregion Fields

    #region Properties

    private string PartialCardSearchIntruderQuery
    {
        get
        {

            if (_partialCardSearchIntruderQuery.IsNullOrEmpty())
            {
                _partialCardSearchIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(
                    uxTransactionDetails.ID, new string[] { "AccountNumber" });
            }
            return _partialCardSearchIntruderQuery;
        }
    }


    protected string AuthIntruderQuery
    {
        get
        {
            if (_authIntruderQuery.IsNullOrEmpty())
            {
                _authIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(
                    uxTransactionDetails.ID, new string[] { "AuthorizationNumber" });
            }
            return _authIntruderQuery;
        }
    }

    protected string GridTitle
    {
        get
        {
            return string.Format(GetLocalResourceObject("rm_TransactionDetailsModal_aspx_cs_MerchantNumber").ToString(), _merchantNumber,
                _reportDate.Month, _reportDate.Day, _reportDate.Year);
        }
    }

    #endregion Properties

    #region Methods

    protected override void PageInitialize()
    {
        this.GridIDs.Add("uxTransactionDetails");
        this.ExporterIDs.Add("uxExportTop");
        base.PageInitialize();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        IsBindDataOnLoad = true;
        uxTransactionDetails.IsIntruder = true;
        uxTransactionDetails.IntruderSourceName = GeneralFuncsLib.GetRequestFileName() + uxTransactionDetails.ID;

        _merchantNumber = SecureQueryString[MERCHANT_NUMBER];
        _reportDate = DateTime.Parse(SecureQueryString[REPORT_DATE]);

        // Set Header, title of Grid
        uxExportTop.GridHeader = VeraCodeSolution.ValidateResponseData(GetLocalResourceObject("rm_TransactionDetailsModal_aspx_cs_TransactionDetails").ToString() + " - " + GridTitle);
        uxExportTop.GridTitle = VeraCodeSolution.ValidateResponseData(GetLocalResourceObject("rm_TransactionDetailsModal_aspx_cs_TransactionDetails").ToString() + " - ");
        uxExportTop.GridSubTitle = VeraCodeSolution.ValidateResponseData(GridTitle);

        // Set title of page
        string merchantName = GeneralFuncsLib.GetMerchantName(_merchantNumber);
        Title += string.Format(" - {0} - {1}", _merchantNumber, merchantName);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }

    #region Grid events

    protected override void DoGridDataSourceReady(AS.Controls.Grid.ASGrid sender, System.EventArgs e)
    {
        if (sender == uxTransactionDetails)
        {
            if (uxTransactionDetails.AS_DataSource.Rows.Count == 0)
            {
                uxTransactionDetails.AllowSorting = false;
            }
        }
    }

    protected override void DoGridNeedDataSource(ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        if (sender == uxTransactionDetails)
        {
            OnDataBindControls(DataBindAction.BindTransactionDetails, sender);
        }

    }

    protected override void DoItemDataBound(ASGrid sender, GridItemEventArgs e)
    {
        if (this.IsIntruderDetected)
            return;
        if (sender == uxTransactionDetails)
        {
            switch (e.Item.ItemType)
            {
                case GridItemType.AlternatingItem:
                case GridItemType.Item:
                    {

                        GridDataItem dataItem = e.Item as GridDataItem;
                        DataRowView dataRow = e.Item.DataItem as DataRowView;
                        var rowItem = (e.Item.DataItem as DataRowView).Row;
                        dataItem["CardTypeCode"].ToolTip = VeraCodeSolution.DoVeraCode(rowItem["CardDescription"].ToString());
                        dataItem["TransactionCode"].ToolTip = VeraCodeSolution.DoVeraCode(rowItem["TransactionCode"].ToString());
                        dataItem["CountryCode"].ToolTip = VeraCodeSolution.DoVeraCode(rowItem["CountryName"].ToString());
                        dataItem["KeyedEntry"].ToolTip = VeraCodeSolution.DoVeraCode(rowItem["EntryModeDescription"].ToString());
                        var dupe = dataItem["DupeCount"];
                        var dupeToolTip = VeraCodeSolution.DoVeraCode(string.Format("{0:C}", rowItem["DupeAmount"]));
                        dupe.ToolTip = dupeToolTip;
                        if (GeneralFuncsLib.NvlString(rowItem["DuplicateCard"]).Equals("1"))
                        {
                            foreach (ASGridBoundColumn col in uxTransactionDetails.Columns)
                            {
                                dupe.ToolTip = dupeToolTip;
                                dataItem[col.UniqueName].ToolTip = VeraCodeSolution.DoVeraCode(GetLocalResourceObject("rm_TransactionDetailsModal_aspx_cs_DuplicateAN").ToString());
                            }
                        }
                        //Dupe Column

                        // TransactionAmount column
                        if (GeneralFuncsLib.NvlString(rowItem["TopAmount"]).Equals("1"))
                        {
                            var transactionAmount = dataItem["TransactionAmount"];
                            string transactionAmountVal = string.Empty;
                            string textColor = "#3e3e3e";
                            if (!rowItem["TransactionAmount"].IsNullOrEmpty())
                            {
                                decimal tempVal;
                                decimal.TryParse(rowItem["TransactionAmount"].ToString(), out tempVal);
                                transactionAmountVal = tempVal < 0 ? string.Format("{0}", GeneralFuncsLib.FormatCurrency(tempVal)) : GeneralFuncsLib.FormatCurrency(tempVal);
                                if (tempVal < 0) textColor = "red";
                            }
                            transactionAmount.ToolTip = VeraCodeSolution.DoVeraCode(GetLocalResourceObject("rm_TransactionDetailsModal_aspx_cs_MaximumTicketAmount").ToString());
                            transactionAmount.Text = string.Format(COLOR_URL_TEMPLATE_TRANS, "green", VeraCodeSolution.ValidateResponseData(transactionAmountVal), textColor);
                        }

                        // MatchedFlag column
                        //dataItem[MATCHED_FLAG_COL].ToolTip = VeraCodeSolution.DoVeraCode(rowItem["MatchToSalesDescription"].ToString());
                        //if (GeneralFuncsLib.NvlString(rowItem[MATCHED_FLAG_COL]).Equals(NO_MATCH_FLAG))
                        //{
                        //    dataItem[MATCHED_FLAG_COL].Text = string.Format(COLOR_URL_TEMPLATE, "red", rowItem[MATCHED_FLAG_COL].ToString());
                        //}
                        //else if (GeneralFuncsLib.NvlString(rowItem[MATCHED_FLAG_COL]).Equals(PARTIAL_MATCH_FLAG))
                        //{
                        //    //dataItem[MATCHED_FLAG_COL].ForeColor = Color.White;
                        //    dataItem[MATCHED_FLAG_COL].Text = string.Format(COLOR_URL_TEMPLATE, "blue", rowItem[MATCHED_FLAG_COL].ToString());
                        //}

                        // 46807
                        dataItem[MATCHED_FLAG_COL].ToolTip = VeraCodeSolution.DoVeraCode(rowItem[MATCHED_NAME].ToString());

                        string matchedCode = rowItem[MATCHED_CODE].ToString();

                        if (matchedCode.Equals("P", StringComparison.OrdinalIgnoreCase) || matchedCode.Equals("CP", StringComparison.OrdinalIgnoreCase))
                        {
                            dataItem[MATCHED_FLAG_COL].Text = GeneralFuncsLib.FormatBorderText(matchedCode, Color.Blue);
                        }
                        else if (matchedCode.Equals("U", StringComparison.OrdinalIgnoreCase) || matchedCode.Equals("SC", StringComparison.OrdinalIgnoreCase))
                        {
                            dataItem[MATCHED_FLAG_COL].Text = GeneralFuncsLib.FormatBorderText(matchedCode, Color.Red);
                        }
                        else if (matchedCode.Equals("M", StringComparison.OrdinalIgnoreCase) || matchedCode.Equals("C", StringComparison.OrdinalIgnoreCase))
                        {
                            dataItem[MATCHED_FLAG_COL].Text = GeneralFuncsLib.FormatBorderText(matchedCode, "#3fbf00".ToColor());
                        }

                        // Card # Column
                        if (GeneralFuncsLib.CheckCSViewFullCard((SecurePage)this.Page))
                        {

                            dataItem[ACCOUNT_NR_COL].Text = string.Format(COLOR_URL_TEMPLATE, "yellow", VeraCodeSolution.DoVeraCode(
                                GeneralFuncsLib.BuildRiskUrlForCardNumber(
                                (SecurePage)this.Page,
                                dataRow[PARTIAL_ACCOUNT_NR_COL],
                                dataRow[ACCOUNT_NR_COL],
                                dataRow[MERCHANT_NR_COL].ToString(),
                                dataRow[ACCOUNT_NR_COL].ToString())
                                ));
                        }
                        else
                        {
                            if (GeneralFuncsLib.HasIPForFullCard((SecurePage)this))
                            {
                                dataItem[PARTIAL_ACCOUNT_NR_COL].Text = string.Format(COLOR_URL_TEMPLATE, "yellow", VeraCodeSolution.DoVeraCode(
                                    GeneralFuncsLib.BuildRiskUrlForFullCard(
                                    (SecurePage)this.Page,
                                    ReportType.TRANSACTION_DETAIL,
                                    dataRow[RECORD_ID_COL].ToString(),
                                    dataRow[ISSUE_BANK_COL].ToString(),
                                    dataRow[REPORT_DATE_COL].ToString(),
                                    dataRow[PARTIAL_ACCOUNT_NR_COL],
                                    dataRow[ACCOUNT_NR_COL],
                                    dataRow[MERCHANT_NR_COL].ToString(),
                                    dataRow[PARTIAL_ACCOUNT_NR_COL].ToString(),
                                    index + 1,
                                    true)
                                    ));
                            }
                            else
                            {
                                dataItem[PARTIAL_ACCOUNT_NR_COL].Text = string.Format(COLOR_URL_TEMPLATE, "yellow", VeraCodeSolution.DoVeraCode(
                                    GeneralFuncsLib.BuildRiskUrlForCardNumber(
                                    (SecurePage)this.Page,
                                    dataRow[PARTIAL_ACCOUNT_NR_COL],
                                    dataRow[ACCOUNT_NR_COL],
                                    dataRow[MERCHANT_NR_COL].ToString(),
                                    dataRow[PARTIAL_ACCOUNT_NR_COL].ToString())
                                    ));
                            }
                        }

                    }
                    break;
            }
        }
    }

    #endregion Grid events

    protected override void OnDataBindControls(Enum type, object sender)
    {
        if (this.IsIntruderDetected)
            return;

        switch ((DataBindAction)type)
        {
            case DataBindAction.BindTransactionDetails:
                {
                    ASGrid grid = (ASGrid)sender;
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    if (GeneralFuncsLib.CheckCSViewFullCard((SecurePage)this.Page))
                    {
                        parameters.AddDecryptDataParams("FullAccountNumber", _isExporting);
                    }
                    parameters.Add(new FilterParameter("@MerchantNumber", _merchantNumber, DbType.String));
                    parameters.Add(new FilterParameter("@ReportDate", _reportDate, DbType.DateTime));
                    parameters.AddLanguageID();
                    grid.DataSourceInvoker = new ASFuncInvoker(
                        WebServices.RiskServices,
                        WebSiteConstants.GET_REPORT_METHOD_NAME,
                        new object[] {
                            SPA_GET_TRANSACTION_DETAIL,
                            ReportServices.ConvertToFilterParamWSArray(parameters) });
                }
                break;
        }
    }

    # region Export

    protected override void DoNeedExportConfig(UxExport sender, ExportConfig exportConfig)
    {
        if (this.IsIntruderDetected)
            return;
        if (sender == uxExportTop)
        {
            _isExporting = true;
            uxTransactionDetails.Columns.FindByUniqueName(ACCOUNT_NR_COL).Visible = false;
            uxTransactionDetails.Columns.FindByUniqueName(PARTIAL_ACCOUNT_NR_COL).Visible = true;
            base.DoNeedExportConfig(sender, exportConfig);
            exportConfig.FileName = GeneralFuncsLib.FormatFileName(uxExportTop.GridHeader);
            exportConfig.ReportHeader = uxExportTop.GridHeader;
        }
    }

    protected override void DoSwitchView()
    {
        if (GeneralFuncsLib.CheckCSViewFullCard((SecurePage)this.Page))
        {
            uxTransactionDetails.Columns.FindByUniqueName(ACCOUNT_NR_COL).Visible = true;
            uxTransactionDetails.Columns.FindByUniqueName(PARTIAL_ACCOUNT_NR_COL).Visible = false;
        }
        else
        {
            uxTransactionDetails.Columns.FindByUniqueName(ACCOUNT_NR_COL).Visible = false;
            uxTransactionDetails.Columns.FindByUniqueName(PARTIAL_ACCOUNT_NR_COL).Visible = true;
        }
    }

    #endregion Protected Methods

    #endregion Methods
}
