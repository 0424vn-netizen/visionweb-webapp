using AS.Common;
using AS.Common.DBManager;
using AS.Common.Formater;
using AS.Controls.Exporter;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Controls.UserControls;
using AS.Web.Business;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using BusinessGeneralFuncsLib = AS.Web.Business.General.GeneralFuncsLib;

[PagePermission("RskQueue,RskAdhoc,MSRskQueue,MSRskAdhoc")]
public partial class rm_MCF_ParameterViolationDetailsModal : ReportPage
{
    #region Enums

    enum DataBindAction
    {
        BindParameterViolationDetails,
        BindTitleInfo
    }
    enum PostBackAction
    {
        MerchantNumberClick,
    }

    #endregion Enums

    #region Constants

    private const string MERCHANT_NUMBER = "MerchantNumber";
    private const string ASSIGNMENT_ID = "AssignmentID";
    private const string PARAMETER_KEY = "ParameterKey";
    private const string PARAMETER_NAME = "ParameterName";
    private const string REPORT_DATE = "ReportDate";
    private const string TRANSACTION_AMOUNT = "TransactionAmount";
    private const string PARTIAL_ACCOUNT_NUMBER = "PartialAccountNumber";
    private const string PREFIX_SENSITIVE_COL = "Partial";

    // SPAs
    private const string SPA_GET_TRANSACTION_DETAIL = "spa_RM_MCF_Get_ParameterViolationDetail";
    private const string SPA_GET_PARAMETER_HEADER_CONFIG = "spa_RM_MCF_Get_ParameterHeaderConfig";

    // Columns
    private const string COLUMN_CONFIG_COLUMN_NAME = "ColumnName";
    private const string COLUMN_CONFIG_HEADER_TEXT = "Headertext";
    private const string COLUMN_CONFIG_IS_EXPORTABLE = "IsExportable";
    private const string COLUMN_CONFIG_IS_VISIBLE_ON_UI = "IsVisibleOnUI";
    private const string COLUMN_CONFIG_AS_FORMAT = "ASFormat";

    private const string COLOR_URL_TEMPLATE = "<span style='border-bottom: 2px {0} solid !important'>{1}</span>";
    private const string ACCOUNT_NUMBER_COL = "AccountNumber";

    #endregion Constants

    #region Fields

    private string _merchantNumber = string.Empty;
    private string _assignmentId = string.Empty;
    private string _parameterKey = string.Empty;
    private string _parameterName = string.Empty;
    private DateTime _reportDate;
    private string _partialCardSearchIntruderQuery = string.Empty;
    private bool _isExporting = false;
    private string _merchantProfileIntruderQuery = string.Empty;
    private DataTable _headers;

    #endregion Fields

    #region Properties

    private string MerchantProfileIntruderQuery
    {
        get
        {
            if (_merchantProfileIntruderQuery.Length == 0)
            {
                _merchantProfileIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(
                    this.ID, new string[] { "MerchantNumber" });
            }
            return _merchantProfileIntruderQuery;
        }
    }

    #endregion Properties

    #region Methods

    protected override void PageInitialize()
    {
        _merchantNumber = SecureQueryString[MERCHANT_NUMBER];
        _assignmentId = SecureQueryString[ASSIGNMENT_ID];
        _parameterKey = SecureQueryString[PARAMETER_KEY];
        _reportDate = DateTime.Parse(SecureQueryString[REPORT_DATE]);
        _parameterName = SecureQueryString[PARAMETER_NAME];
        GenerateGridColumns();
        this.GridIDs.Add("uxParameterViolationDetails");
        this.ExporterIDs.Add("uxExportTop");
        base.PageInitialize();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;
        IsBindDataOnLoad = true;
        uxParameterViolationDetails.IsIntruder = true;
        uxParameterViolationDetails.IntruderSourceName = GeneralFuncsLib.GetRequestFileName() + uxParameterViolationDetails.ID;

        OnDataBindControls(DataBindAction.BindTitleInfo);
    }

    #region Grid events

    protected override void DoGridDataSourceReady(ASGrid sender, EventArgs e)
    {
        if (sender == uxParameterViolationDetails && uxParameterViolationDetails.AS_DataSource.Rows.Count == 0)
        {
            uxParameterViolationDetails.AllowSorting = false;
        }
    }

    protected override void DoGridNeedDataSource(ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        if (sender == uxParameterViolationDetails)
        {
            OnDataBindControls(DataBindAction.BindParameterViolationDetails, sender);
        }
    }

    protected override void DoItemDataBound(ASGrid sender, GridItemEventArgs e)
    {
        if (IsIntruderDetected)
        {
            return;
        }

        if (sender == uxParameterViolationDetails)
        {
            switch (e.Item.ItemType)
            {
                case GridItemType.AlternatingItem:
                case GridItemType.Item:
                    {
                        GridDataItem dataItem = e.Item as GridDataItem;
                        DataRowView dataRow = e.Item.DataItem as DataRowView;
                        var rowItem = (e.Item.DataItem as DataRowView).Row;
                        foreach (DataRow header in _headers.Rows)
                        {
                            var columnName = BusinessGeneralFuncsLib.GetValue<string>(header, COLUMN_CONFIG_COLUMN_NAME);
                            var columnNameTooltip = columnName + "Tooltip";
                            if (rowItem.Table.Columns.Contains(columnNameTooltip))
                            {
                                dataItem[columnName].ToolTip = rowItem[columnNameTooltip].ToString();
                            }
                        }
                    }
                    break;
            }
        }

        if (e.Item is GridFooterItem)
        {
            GridFooterItem footerItem = e.Item as GridFooterItem;

            var transactionAmoutCol = sender.Columns.FindByDataFieldSafe(TRANSACTION_AMOUNT);
            if (transactionAmoutCol != null && uxParameterViolationDetails.AS_Total != null)
            {
                string transactionAmount = FormatData.FormatCurrency((decimal)uxParameterViolationDetails.AS_Total[TRANSACTION_AMOUNT], SessionManager.CurrencyFortmat);
                footerItem[TRANSACTION_AMOUNT].Text = transactionAmount;
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
            case DataBindAction.BindParameterViolationDetails:
                {
                    ASGrid grid = (ASGrid)sender;
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parameters.Add(new FilterParameter("@MerchantNumber", _merchantNumber, DbType.String));
                    parameters.Add(new FilterParameter("@ParameterKey", _parameterKey, DbType.String));
                    parameters.Add(new FilterParameter("@ReportDate", _reportDate, DbType.DateTime));
                    parameters.Add(new FilterParameter("@AssignmentID", _assignmentId, DbType.Int32));
                    parameters.AddLanguageID();
                    if (_isExporting)
                    {
                        parameters.Add(new FilterParameter("@IsPaging", false, DbType.Int32));
                    }

                    SetDecryptDataParams(_headers, parameters);

                    grid.DataSourceInvoker = new ASFuncInvoker(
                        WebServices.RiskServices,
                        WebSiteConstants.GET_REPORT_METHOD_NAME,
                        new object[] { SPA_GET_TRANSACTION_DETAIL, ReportServices.ConvertToFilterParamWSArray(parameters) });
                }
                break;

            case DataBindAction.BindTitleInfo:
                {
                    var merchantName = GeneralFuncsLib.GetMerchantName(_merchantNumber);
                    string riskReportIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(this.ID, new string[] { "MerchantNumber" });
                    string url = "rm_MCF_RiskReport.aspx?" + BuildSecureQueryString(string.Format("merchantnumber={0}&IsPopup={1}{2}", _merchantNumber, true, riskReportIntruderQuery));
                    var merchantNumberLink = "<a class=\"image-link\" href=\"#\" style=\"cursor:pointer\" onclick=\"openPopupWindowOnMenu(this,'" + url + "','RiskReport'); return false;\">" + _merchantNumber + " </a>";

                    ltrMerchantInfor.Text = VeraCodeSolution.DoVeraCode(string.Format("{0} <div class='font-12'>{1} </div>", VeraCodeSolution.ValidateResponseData(merchantName), merchantNumberLink));

                    uxExportTop.GridSubTitle = string.Format("<div class='font-12'>{0} {1}</div><div class='font-12'>Parameter: {2} - {3}</div>",
                        GetLocalResourceObject("rm_MCF_ParameterViolationDetailsModal_ReportDate").ToString(), _reportDate.ToString(WebSiteConstants.DATE_FORMAT), _parameterKey, _parameterName);
                }
                break;
        }
    }
    protected void uxMerchantName_Command(object sender, CommandEventArgs e)
    {
        RiskSessionManager.RiskMgmtReportFilter.KeepSession = true;
        _merchantNumber = e.CommandArgument != null ? e.CommandArgument.ToString() : string.Empty;

        if (e.CommandName == "MerchantNumberClick")
        {
            OnPostBackActions(PostBackAction.MerchantNumberClick);
        }
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.MerchantNumberClick:
                {
                    string url = "rm_MCF_RiskReport.aspx?" + this.BuildSecureQueryString(
                    string.Format("merchantnumber={0}&IsPopup={1}{2}",
                                  _merchantNumber,
                                  true,
                                  MerchantProfileIntruderQuery));
                    AjaxAddResponseScript("openPopupWindow('" + url + "','RiskReport');");
                    break;
                }
        }
    }

    private void GenerateGridColumns()
    {
        uxParameterViolationDetails.Columns.Clear();
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.Add(new FilterParameter("@ParameterKey", _parameterKey, DbType.AnsiString));
        DataTable headers = WebServices.SecurityServices.GetReports(SPA_GET_PARAMETER_HEADER_CONFIG, parameters);

        if (headers != null && headers.Rows.Count > 0)
        {
            int orderno = 0;
            foreach (DataRow header in headers.Rows)
            {
                ASGridBoundColumn newColumn = new ASGridBoundColumn();
                newColumn.OrderIndex = orderno;
                newColumn.Visible = true;
                newColumn.HtmlEncode = false;

                var columnName = BusinessGeneralFuncsLib.GetValue<string>(header, COLUMN_CONFIG_COLUMN_NAME);
                var columnWidth = BusinessGeneralFuncsLib.GetValue<int>(header, "HeaderWidth");
                newColumn.HeaderStyle.Width = columnWidth != 0 ? columnWidth : 120;
                newColumn.DataField = columnName;
                newColumn.UniqueName = columnName;
                newColumn.HeaderText = BusinessGeneralFuncsLib.GetValue<string>(header, COLUMN_CONFIG_HEADER_TEXT);
                FormatType asFormat = RM_MCF_GeneralFuncsLib.GetASFormat(BusinessGeneralFuncsLib.GetValue<string>(header, COLUMN_CONFIG_AS_FORMAT));
                newColumn.ASFormat = asFormat;

                //if (string.Equals(columnName, TRANSACTION_AMOUNT, StringComparison.OrdinalIgnoreCase))
                //{
                //    newColumn.ASIsTotalColumn = true;
                //}

                if (string.Equals(columnName, ACCOUNT_NUMBER_COL, StringComparison.OrdinalIgnoreCase) || string.Equals(columnName, PARTIAL_ACCOUNT_NUMBER, StringComparison.OrdinalIgnoreCase))
                {
                    newColumn.AllowSorting = false;
                }

                uxParameterViolationDetails.Columns.Add(newColumn);
                uxParameterViolationDetails.Columns.FindByUniqueName(columnName).Visible = BusinessGeneralFuncsLib.GetValue<bool>(header, COLUMN_CONFIG_IS_VISIBLE_ON_UI);
                orderno++;
            }

            _headers = headers;
        }
    }

    private void SetDecryptDataParams(DataTable headers, FilterParameterCollection parameters)
    {
        if (headers != null)
        {
            foreach (DataRow header in _headers.Rows)
            {
                var columnName = BusinessGeneralFuncsLib.GetValue<string>(header, COLUMN_CONFIG_COLUMN_NAME);
                if (columnName == ACCOUNT_NUMBER_COL)
                {
                    string decryptDataParams = string.Empty;
                    if (GeneralFuncsLib.CheckCSViewFullCard((SecurePage)this.Page))
                    {
                        parameters.AddDecryptDataParams(ACCOUNT_NUMBER_COL, _isExporting);
                    }
                }
            }
        }
    }

    # region Export

    protected override void DoNeedExportConfig(UxExport sender, ExportConfig exportConfig)
    {
        if (IsIntruderDetected)
        {
            return;
        }

        if (sender == uxExportTop)
        {
            _isExporting = true;

            if (_headers != null)
            {
                foreach (DataRow header in _headers.Rows)
                {
                    var columnName = BusinessGeneralFuncsLib.GetValue<string>(header, COLUMN_CONFIG_COLUMN_NAME);
                    var isExportable = BusinessGeneralFuncsLib.GetValue<bool>(header, COLUMN_CONFIG_IS_EXPORTABLE);
                    uxParameterViolationDetails.Columns.FindByUniqueName(columnName).Visible = isExportable;
                }
            }
            base.DoNeedExportConfig(sender, exportConfig);
            var reportDate = _reportDate.ToString(WebSiteConstants.DATE_FORMAT_EXPORT);
            var fileName = string.Format("{0}_{1}_{2}", GetLocalResourceObject("rm_ParameterViolationDetailsModal_aspx_cs_ParameterViolationDetails").ToString(), _parameterKey, reportDate);
            exportConfig.FileName = GeneralFuncsLib.FormatFileName(fileName);

            var merchantName = GeneralFuncsLib.GetMerchantName(_merchantNumber);
            var labelReportDate = string.Format("{0} {1}", GetLocalResourceObject("rm_MCF_ParameterViolationDetailsModal_ReportDate").ToString(), reportDate);
            var labelParameter = string.Format("{0} - {1}", _parameterKey, _parameterName);
            exportConfig.ReportHeader = merchantName + "\r\n" + _merchantNumber + "\r\n" + labelReportDate + "\r\n" + labelParameter + "\r\n";
        }
    }

    protected override void DoSwitchView()
    {
        if (_headers != null && _headers.Rows.Count > 0)
        {
            foreach (DataRow header in _headers.Rows)
            {
                var columnName = BusinessGeneralFuncsLib.GetValue<string>(header, COLUMN_CONFIG_COLUMN_NAME);
                var isVisibleOnUI = BusinessGeneralFuncsLib.GetValue<bool>(header, COLUMN_CONFIG_IS_VISIBLE_ON_UI);
                var isShowFullData = true;
                if (isVisibleOnUI)
                {
                    if (columnName == ACCOUNT_NUMBER_COL && !GeneralFuncsLib.CheckCSViewFullCard((SecurePage)this.Page))
                    {
                        isShowFullData = false;
                    }

                    if (isShowFullData)
                    {
                        SetColumnVisibility(uxParameterViolationDetails, columnName, true);
                        SetColumnVisibility(uxParameterViolationDetails, PREFIX_SENSITIVE_COL + columnName, false);
                    }
                    else
                    {
                        SetColumnVisibility(uxParameterViolationDetails, columnName, false);
                        SetColumnVisibility(uxParameterViolationDetails, PREFIX_SENSITIVE_COL + columnName, true);
                    }
                }
            }
        }
    }

    protected void SetColumnVisibility(ASGrid grid, string columnName, bool isVisible)
    {
        GridColumn gridColumn = grid.Columns.FindByUniqueNameSafe(columnName);
        if (gridColumn != null)
        {
            gridColumn.Visible = isVisible;
        }
    }

    #endregion Protected Methods

    #endregion Methods
}
