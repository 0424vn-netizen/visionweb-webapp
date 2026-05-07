using AS.Common;
using AS.Common.DBManager;
using AS.Common.Logger;
using AS.Controls.Exporter;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Utilities;
using AS.VW.Entities;
using AS.Web.Business;
using AS.Web.UI.AppCode.General;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Xml;
using Telerik.Web.UI;

public partial class UserControls_rm_MCF_NewRiskReportTransactionHistory : GlobalUserControl
{
    private const string TRANS_TYPE = "TransType";
    private const string MERCHANT_NUMBER = "MerchantNumber";
    private const string REPORT_DATE = "ReportDate";
    private const string TRANSACTION_AMOUNT = "TransactionAmount";
    private const string IMAGE = "<a class=\"image-link\" href=\"#\"  onclick=\" return ShowPopupModalChild({0},'{1}','auto');\"><img src='{2}res/img/information.png' border=\"0\"/></a>&nbsp; &nbsp;";
    private const string MATCHED_CODE = "MatchCode";
    private const string MATCHED_NAME = "MatchName";
    private const string SPA_GET_TRANS_HISTORY = "spa_RM_MCF_RiskReport_GetTransactionHistory";
    private const string SPA_GET_CUSTOMVIEW_LIST = "spa_RM_MCF_GetCustomViewList";
    enum DataBindAction
    {
        BindTransactionDetails,
        BindViewColumn
    }
    private int index = 0;
    private string m_currentSortExpr = string.Empty;
    private string m_currentSortOrder = string.Empty;
    private string _CurrentSortControls = string.Empty;
    DataTable dataGetTransactionDetails = null;
    private List<string> _cachedDisplayedColumns;
    public int TotalRows;
    public string MerchantNumber
    {
        get
        {
            if (ViewState["MerchantNum"] != null) return ViewState["MerchantNum"].ToString();
            else return string.Empty;
        }
        set { ViewState["MerchantNum"] = value; }
    }
    public DateTime ReportDate
    {
        get
        {
            if (ViewState["ReportDate"] != null) return DateTime.Parse(ViewState["ReportDate"].ToString());
            else return DateTime.Now.Date;
        }
        set { ViewState["ReportDate"] = value; }
    }
    public int FlagLink
    {
        get
        {
            if (ViewState["FlagLink"] != null)
                return Int32.Parse(ViewState["FlagLink"].ToString());
            else
                return 0;
        }
        set
        {
            ViewState["FlagLink"] = value;
        }
    }
    private bool _isBuidLink = true;
    public bool IsBuildLink { get { return _isBuidLink; } set { _isBuidLink = value; } }




    string _PartialCardSearchIntruderQuery = string.Empty;

    public List<DataSourceParallelResponse> DataSources { get; set; }
    private string PartialCardSearchIntruderQuery
    {
        get
        {

            if (_PartialCardSearchIntruderQuery == string.Empty)
            {
                _PartialCardSearchIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(uxTransactionDetails.ID, new string[] { "AccountNumber" });
            }
            return _PartialCardSearchIntruderQuery;
        }
    }
    string _AuthIntruderQuery = string.Empty;
    protected string AuthIntruderQuery
    {
        get
        {
            if (this._AuthIntruderQuery == string.Empty) this._AuthIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(this.uxTransactionDetails.ID, new string[] { "AuthorizationNumber" });
            return this._AuthIntruderQuery;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            uxTransactionDetails.IsIntruder = true;
            uxTransactionDetails.IntruderSourceName = GeneralFuncsLib.GetPageUrlFileName() + uxTransactionDetails.ID;
            if (FlagLink == 1)
            {
                string queryString = this.Page.BuildSecureQueryString(string.Format("MerchantNumber={0}&ReportDate={1}",
                       MerchantNumber, ReportDate));
                string url = string.Format("<span class=\"dark-blue\"><a class=\"link-back\" href=\"#\" onclick=\"openPopupWindow('rm_MCF_NewRiskReport_TransactionHistoryModal.aspx?{0}','TransactionHistory'); return false;\">", queryString);
                uxTransHistoryHeader.Text = VeraCodeSolution.DoVeraCode(url + GetLocalResourceObject("RiskTransactionHistory_ascx_cs_NewWindow").ToString() + "</a></span>");
            }
            else
            {
                uxTransHistoryHeader.Text = "";
            }
            if (!IsPostBack)
            {
                if (FlagLink == 0)
                {
                    SetViewCombo();
                    if (!string.IsNullOrEmpty(SessionManager.CurrentMerchantNumber))
                    {
                        if (Page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RSK_MNG_PRIVATE_VIEW) || Page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RSK_MNG_PRIVATE_VIEW_MS) ||
                            Page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RSK_MNG_PUBLIC_VIEW_MS) || Page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RSK_MNG_PUBLIC_VIEW))
                        {
                            uxCustomizeColumnLink.Visible = true;
                            string encodeURL = string.Format("rm_MCF_ManageTransactionHistoryCustomViewsModal.aspx?" + this.Page.BuildSecureQueryString(string.Format("merchantNumber={0}&flagLink={1}&customViewMessageResourceTypeMode={2}", SessionManager.CurrentMerchantNumber, FlagLink, CustomViewMessageResourceType.CustomView.ToString())));
                            uxCustomizeColumnLink.OnClientClick = "return doOpenTransactionHistoryNewPopup('" + encodeURL + "')";
                        }
                        else
                        {
                            uxCustomizeColumnLink.Visible = false;
                        }
                    }
                }
                RiskSessionManager.TransactionHistorySortOrder = "ReportDate DESC, PartialCardNumber ASC";

                BindTransDateRange();
                uxTransactionDetails.Rebind();
            }
        }
        catch (ViewStateException)
        {
            LoggerManager.Error(string.Format("UserControls_rm_MCF_NewRiskReportTransactionHistory - ViewStateException - Request={0}; SessionID={1}", Context.Request.Url, Session.SessionID));
            throw;
        }
    }

    private bool _IsExporting = false;
    protected void uxExport_OnNeedExportConfig(object sender, ExportConfig exportConfig)
    {
        _IsExporting = true;
        uxTransactionDetails.Columns.FindByUniqueName("CardNumber").Visible = false;
        uxTransactionDetails.Columns.FindByUniqueName("PartialCardNumber").Visible = true;

        string fileName = GeneralFuncsLib.FormatFileName(exportConfig.FileName);
        exportConfig.FileName = HttpUtility.UrlEncode(fileName);
        exportConfig.ReportHeader = GetLocalResourceObject("RiskTransactionHistory_ascx_cs_TransactionHistory").ToString();

    }

    protected void uxTransactionDetails_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        RiskSessionManager.TransactionHistorySortOrder = string.Empty;
    }

    protected void uxTransactionDetails_DataSourceReady(object sender, EventArgs e)
    {
        if (uxTransactionDetails.AS_DataSource.Rows.Count == 0)
        {
            uxTransactionDetails.AllowSorting = false;
            uxTransactionDetails.AllowPaging = false;
        }
        else
        {
            uxTransactionDetails.AllowSorting = true;
            uxTransactionDetails.AllowPaging = true;
        }

    }
    protected void uxTransactionDetails_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        if (string.IsNullOrEmpty(MerchantNumber))
        {
            uxTransactionDetails.DataSource = new DataTable();
            return;
        }

        if (RiskSessionManager.TransactionHistorySortOrder != string.Empty)
        {
            uxTransactionDetails.AS_SortExpression = RiskSessionManager.TransactionHistorySortOrder;
        }
        if (GeneralFuncsLib.CheckCSViewFullCard(this.Page) && !_IsExporting)
        {
            uxTransactionDetails.Columns.FindByUniqueName("CardNumber").Visible = true;
            uxTransactionDetails.Columns.FindByUniqueName("PartialCardNumber").Visible = false;
        }
        else
        {
            uxTransactionDetails.Columns.FindByUniqueName("CardNumber").Visible = false;
            uxTransactionDetails.Columns.FindByUniqueName("PartialCardNumber").Visible = true;
        }
        if (dataGetTransactionDetails != null)
        {
            ASRadControlHelper.BindGridWithPaging(uxTransactionDetails, dataGetTransactionDetails);
        }
        else
        {
            var appconfig = WebSiteSettings.GetWebAppConfig();

            if (appconfig != null && appconfig.TransactionHistoryGrid != null && appconfig.TransactionHistoryGrid.EnhancePerformance)
            {
                var dt = GetTransHistoryData();
                ASRadControlHelper.BindGridWithPaging(uxTransactionDetails, dt);
            }
            else
            {
                FilterParameterCollection parameters = new FilterParameterCollection();
                parameters.AddLoggedInUserRiskParams();
                SecurePage securePage = HttpContext.Current.Handler as SecurePage;
                if (GeneralFuncsLib.CheckCSViewFullCard((SecurePage)this.Page))
                {
                    parameters.AddDecryptDataParams("CardNumber", _IsExporting);
                }
                parameters.Add(new FilterParameter("@MerchantNumber", MerchantNumber, DbType.String));
                parameters.Add(new FilterParameter("@ReportDate", ReportDate, DbType.DateTime));
                parameters.Add(new FilterParameter("@DateRange", RiskSessionManager.TransDateRangeModal, DbType.Int32));
                parameters.AddLanguageID();
                uxTransactionDetails.DataSourceInvoker = new ASFuncInvoker(WebServices.RiskServices,
                            WebSiteConstants.GET_REPORT_METHOD_NAME,
                            new object[] { SPA_GET_TRANS_HISTORY,
                ReportServices.ConvertToFilterParamWSArray(parameters) });
            }
        }
    }

    private DataTable GetTransHistoryData()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserRiskParams();
        SecurePage securePage = HttpContext.Current.Handler as SecurePage;
        if (GeneralFuncsLib.CheckCSViewFullCard((SecurePage)this.Page))
        {
            parameters.AddDecryptDataParams("CardNumber", _IsExporting);
        }
        parameters.Add(new FilterParameter("@MerchantNumber", MerchantNumber, DbType.String));
        parameters.Add(new FilterParameter("@ReportDate", ReportDate, DbType.DateTime));
        parameters.Add(new FilterParameter("@DateRange", RiskSessionManager.TransDateRangeModal, DbType.Int32));

        parameters.AddPagingParameters(!_IsExporting, false, uxTransactionDetails.CurrentPageIndex + 1, uxTransactionDetails.MasterTableView.PageSize, uxTransactionDetails.AS_SortExpression, uxTransactionDetails.GetCurrentFilterExpressions());

        parameters.AddLanguageID();

        Stopwatch st = new Stopwatch();
        st.Start();
        var dt = WebServices.RiskServices.GetReports(SPA_GET_TRANS_HISTORY, parameters);
        st.Stop();
        LoggerManager.Info("GetTransHistoryData takes: " + st.ElapsedMilliseconds + " ms");
        return dt;

    }

    protected void uxTransactionDetails_ItemDataBound(object sender, GridItemEventArgs e)
    {

        switch (e.Item.ItemType)
        {
            case GridItemType.AlternatingItem:
            case GridItemType.Item:
                {

                    GridDataItem dataItem = e.Item as GridDataItem;
                    var rowItem = (e.Item.DataItem as DataRowView).Row;
                    dataItem["CardType"].ToolTip = VeraCodeSolution.DoVeraCode(rowItem["CardDescription"].ToString());
                    dataItem["KeyedEntry"].ToolTip = VeraCodeSolution.DoVeraCode(rowItem["EntryModeDescription"].ToString());
                    dataItem["ADF"].ToolTip = VeraCodeSolution.DoVeraCode(rowItem["ADFDescription"].ToString());
                    dataItem["ResponseCode"].ToolTip = VeraCodeSolution.DoVeraCode(rowItem["AuthResponseDescription"].ToString());
                    dataItem["AVS"].ToolTip = VeraCodeSolution.DoVeraCode(rowItem["AVSDescription"].ToString());
                    dataItem["CVV"].ToolTip = VeraCodeSolution.DoVeraCode(rowItem["CVVDescription"].ToString());
                    dataItem["ExpirationDate"].ToolTip = "MM/YY";
                    dataItem["CountryCode"].ToolTip = VeraCodeSolution.DoVeraCode(rowItem["CountryName"].ToString());
                    dataItem["DupeCount"].ToolTip = VeraCodeSolution.DoVeraCode(string.Format("{0:C}", rowItem["DupeAmount"]));

                    if (GeneralFuncsLib.NvlString(rowItem["ADF"]).Equals("F", StringComparison.OrdinalIgnoreCase)
                        || GeneralFuncsLib.NvlString(rowItem["ADF"]).Equals("D", StringComparison.OrdinalIgnoreCase))
                    {
                        dataItem["ADF"].Text = VeraCodeSolution.DoVeraCode(GeneralFuncsLib.FormatBorderText(rowItem["ADF"].ToString(), Color.Red));
                    }


                    Color transColor = Color.White;
                    if (GeneralFuncsLib.NvlString(rowItem[TRANS_TYPE]).Equals("Sales") || GeneralFuncsLib.NvlString(rowItem[TRANS_TYPE]).Equals("Ventas"))
                    {
                        transColor = Color.Green;
                    }
                    else if (GeneralFuncsLib.NvlString(rowItem[TRANS_TYPE]).Equals("Auth") || GeneralFuncsLib.NvlString(rowItem[TRANS_TYPE]).Equals("Autorización"))
                    {
                        transColor = Color.LightBlue;
                    }
                    if (transColor != Color.White)
                    {
                        dataItem[TRANS_TYPE].Text = VeraCodeSolution.DoVeraCode(GeneralFuncsLib.FormatBorderText(rowItem[TRANS_TYPE].ToString(), transColor));
                    }

                    if (rowItem["TransDate"].ToString().Trim().Length > 10)
                    {
                        dataItem["TransDate"].Width = 90;
                    }

                    if (!GeneralFuncsLib.NvlString(rowItem[TRANS_TYPE]).Equals("Auth"))
                    {
                        dataItem["TransDate"].Text = VeraCodeSolution.ValidateResponseData(dataItem["TransDate"].Text.Split(' ')[0]);
                    }
                    if (!String.IsNullOrEmpty(rowItem["CardNumber"].ToString()))
                    {

                        string fullcard = VeraCodeSolution.ValidateResponseData(rowItem["CardNumber"].ToString());// WebServices.RiskServices.DecryptText(rowView["AccountNumber"].ToString(), SessionManager.CurrentUser.ASClient);
                        string urlCard = string.Empty;
                        if (_isBuidLink)
                        {
                            string queryString = Page.BuildSecureQueryString("cn=" + rowItem["PartialCardNumber"] + "&cnf=" + fullcard + "&merch=" + MerchantNumber + "&isRisk=1");
                            string urlCardDetail = ResolveUrl("~/") + "CardHistoryModal.aspx?" + queryString;
                            urlCard = "<a class=\"link\" href=\"javascript:void(0)\" onclick=\"openPopupWindow('" + urlCardDetail + "','CardHistoryWindow'); return false;\">";
                        }
                        else
                        {
                            urlCard = "<a class=\"link\" href=\"javascript:void(0)\" onclick=\"openTransactionHistoryModal('" + rowItem["PartialCardNumber"] + ";" + rowItem["RecordId"] + ";" + rowItem["ReportDate"].ToString() + ";" + rowItem["ReportType"].ToString() + "'); return false;\">";
                        }
                        if (_IsExporting)
                        {
                            dataItem["PartialCardNumber"].Text = VeraCodeSolution.DoVeraCode(rowItem["PartialCardNumber"].ToString());
                        }
                        else if (GeneralFuncsLib.CheckCSViewFullCard(this.Page))
                        {
                            dataItem["CardNumber"].Text = VeraCodeSolution.DoVeraCode(urlCard + fullcard + "</a>");
                        }
                        else
                        {
                            if (GeneralFuncsLib.HasIPForFullCard((SecurePage)this.Page))
                            {
                                dataItem["PartialCardNumber"].Text = VeraCodeSolution.DoVeraCode(
                                    String.Format(IMAGE, (index + 1), BuildUrlForFullCard(rowItem["ReportType"].ToString(), rowItem["RecordId"].ToString(), rowItem["IssuingBank"].ToString(), rowItem["ReportDate"].ToString()), ResolveUrl("~/")) + VeraCodeSolution.DoVeraCode(urlCard + rowItem["PartialCardNumber"].ToString() + "</a>")
                                    );
                            }
                            else
                            {
                                dataItem["PartialCardNumber"].Text = VeraCodeSolution.DoVeraCode(urlCard + rowItem["PartialCardNumber"].ToString() + "</a>");
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(rowItem["SettleType"].ToString()))
                    {
                        dataItem["SettleType"].Text = WebSiteConstants.HTML_EM_DASH_ENCODE;
                    }

                    // 46807 
                    dataItem[MATCHED_CODE].ToolTip = VeraCodeSolution.DoVeraCode(rowItem[MATCHED_NAME].ToString());

                    string matchedCode = rowItem[MATCHED_CODE].ToString();

                    if (matchedCode.Equals("P", StringComparison.OrdinalIgnoreCase) || matchedCode.Equals("CP", StringComparison.OrdinalIgnoreCase))
                    {
                        dataItem[MATCHED_CODE].Text = GeneralFuncsLib.FormatBorderText(matchedCode, Color.Blue);
                    }
                    else if (matchedCode.Equals("U", StringComparison.OrdinalIgnoreCase) || matchedCode.Equals("SC", StringComparison.OrdinalIgnoreCase))
                    {
                        dataItem[MATCHED_CODE].Text = GeneralFuncsLib.FormatBorderText(matchedCode, Color.Red);
                    }
                    else if (matchedCode.Equals("M", StringComparison.OrdinalIgnoreCase) || matchedCode.Equals("C", StringComparison.OrdinalIgnoreCase))
                    {
                        dataItem[MATCHED_CODE].Text = GeneralFuncsLib.FormatBorderText(matchedCode, "#3fbf00".ToColor());
                    }
                }
                break;
        }
    }


    private string BuildUrlForFullCard(string reportType, string encryptedCC, string issueBank, string ReportDate)
    {
        string queryString = Page.BuildSecureQueryString("rt=" + reportType + "&cn=" + Server.UrlEncode(encryptedCC) + "&issue=" + issueBank + "&reportdate=" + ReportDate + "&idx=" + (index + 1));
        queryString = ResolveUrl("~/") + "FullCC.aspx?" + queryString;
        return queryString;
    }

    private void BindTransDateRange()
    {
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
            new RadComboBoxItem("90", "90"),
            new RadComboBoxItem("120", "120"),
            new RadComboBoxItem("150", "150"),
            new RadComboBoxItem("180", "180"),
        };
        uxTransDateRange.Items.AddRange(items);

        string defaultDays = RiskSessionManager.TransDateRangeModal.ToString();
        RadComboBoxItem item = items.Find(i => i.Value == defaultDays);
        item.Selected = true;
    }

    protected void uxTransDateRange_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        RiskSessionManager.TransDateRangeModal = int.Parse(uxTransDateRange.SelectedValue);
        uxTransactionDetails.MasterTableView.CurrentPageIndex = 0;
        uxTransactionDetails.Rebind();
    }

    public bool isOntop { get; set; }

    private bool IsShowNewExport
    {
        get { return GeneralFuncsLib.GetDataOfExtendedSetting("SHOW_EXPORT_QUEUE").ToLower().Equals("true"); }
    }


    protected override void OnPreRender(EventArgs e)
    {
        uxExportTransactionHistory.Visible = !IsShowNewExport;
        uxExportQueueTransactionHistory.Visible = IsShowNewExport;

        if (IsShowNewExport)
        {
            uxExportQueueTransactionHistory.GridHeader = "Transaction History";
            uxExportQueueTransactionHistory.FilterParams = BuildExportFilterParams();
        }

        base.OnPreRender(e);
        if (isOntop)
            uxTitle.Attributes["class"] += " on-top";
    }

    private string BuildExportFilterParams()
    {
        string merchantNumber = MerchantNumber ?? string.Empty;
        string dateRange = RiskSessionManager.TransDateRangeModal.ToString();

        var doc = new XmlDocument();
        var root = doc.CreateElement("ExportFilter");
        doc.AppendChild(root);

        var reportTitleItem = doc.CreateElement("ReportHeader");
        reportTitleItem.SetAttribute("Value", string.Join(",", uxExportQueueTransactionHistory.GridHeader));
        root.AppendChild(reportTitleItem);

        var paramInputs = doc.CreateElement("ParamInputs");
        root.AppendChild(paramInputs);
        AppendParamInput(doc, paramInputs, "MerchantNumber", merchantNumber);
        AppendParamInput(doc, paramInputs, "DateRange", dateRange);
        AppendParamInput(doc, paramInputs, "ReportDate", DateTime.Now.ToString());
        AppendParamInput(doc, paramInputs, "UserID", SessionManager.CurrentUser.UserID);
        AppendParamInput(doc, paramInputs, "UserMode", GeneralFuncsLib.GetUserMode());

        string customViewId = string.IsNullOrEmpty(uxViewColumns.SelectedValue) ? "0" : uxViewColumns.SelectedValue;
        var customViewIdItem = doc.CreateElement("CustomViewId");
        customViewIdItem.SetAttribute("Value", customViewId);
        root.AppendChild(customViewIdItem);

        if (_cachedDisplayedColumns == null)
        {
            var displayedColumns = GetDisplayedColumnsByViewID();
            _cachedDisplayedColumns = displayedColumns != null ? displayedColumns.DisplayedColumns : new List<string>();
        }

        var customViewItem = doc.CreateElement("CustomView");
        customViewItem.SetAttribute("Value", string.Join(",", _cachedDisplayedColumns));
        root.AppendChild(customViewItem);

        var filterItem = doc.CreateElement("FilterItem");
        filterItem.SetAttribute("Value", uxTransactionDetails.AS_FilterExpression ?? string.Empty);
        root.AppendChild(filterItem);

        var sortItem = doc.CreateElement("SortItem");
        sortItem.SetAttribute("Value", uxTransactionDetails.AS_SortExpression ?? string.Empty);
        root.AppendChild(sortItem);

        return doc.OuterXml;
    }

    private static void AppendParamInput(XmlDocument doc, XmlElement parent, string key, string value)
    {
        var item = doc.CreateElement("ParamInput");
        if (!string.IsNullOrEmpty(key))
        {
            item.SetAttribute("Key", key);
        }
        item.SetAttribute("Value", value);
        parent.AppendChild(item);
    }

    protected void btnRebind_Click(object sender, EventArgs e)
    {
        if (FlagLink == 0) // new window
        {
            RiskSessionManager.TransactionHistoryNewWindowUserViewSelected = string.Empty;
        }
        else
        {
            RiskSessionManager.TransactionHistoryUserViewSelected = string.Empty;
        }
        SetViewCombo();
        base.OnPreRender(e);
        uxTransactionDetails.Rebind();
    }

    public void SetViewCombo()
    {
        DataTable tb = GetViewColumn();
        uxViewColumns.DataSource = tb;
        uxViewColumns.DataBind();

        DataRow[] defaultRow = tb.Rows.Count > 0 ? tb.Select("IsDefault = 1") : null;
        string viewSelected = FlagLink == 0 ? RiskSessionManager.TransactionHistoryNewWindowUserViewSelected : RiskSessionManager.TransactionHistoryUserViewSelected;

        if (!string.IsNullOrEmpty(viewSelected) && tb.AsEnumerable().FirstOrDefault(r => r.Field<int>("CustomViewID") == int.Parse(viewSelected)) != null)
        {
            uxViewColumns.SelectedValue = viewSelected;
        }
        else if (defaultRow != null)
        {
            uxViewColumns.SelectedValue = defaultRow[0]["CustomViewID"].ToString();
        }
    }

    private DataTable GetViewColumn()
    {
        DataTable bindData = null;
        if (DataSources.IsNotNullData())
        {
            var data = DataSources.SingleOrDefault(m => m.FeatureName == DataBindAction.BindViewColumn.ToString());
            if (data.IsNotNullData())
                bindData = data.DataSource;
        }

        if (bindData.IsNotNullData())
            return bindData;

        var parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(false);
        parameters.AddLanguageID();
        parameters.Add(new FilterParameter("@ViewMode", 0, System.Data.DbType.Int32));
        parameters.Add(new FilterParameter("@ASClient", SessionManager.CurrentClient, System.Data.DbType.Int32));
        parameters.Add(new FilterParameter("@UserRecID", SessionManager.CurrentUser.RecId, DbType.Guid));
        parameters.Add(new FilterParameter("@PageType", "RiskReport_TransactionHistory", DbType.String));
        if (FlagLink == 0) // in new window
        {
            parameters.Add(new FilterParameter("@PageName", "New Window", DbType.String));
        }
        return WebServices.RiskServices.GetReports(SPA_GET_CUSTOMVIEW_LIST, parameters);
    }

    protected void uxViewColumns_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if (FlagLink == 0) // new window
        {
            RiskSessionManager.TransactionHistoryNewWindowUserViewSelected = uxViewColumns.SelectedValue;
        }
        else
        {
            RiskSessionManager.TransactionHistoryUserViewSelected = uxViewColumns.SelectedValue;
        }
        var columns = GetDisplayedColumnsByViewID();
        if (columns == null) return;
        _cachedDisplayedColumns = columns.DisplayedColumns;
        uxTransactionDetails.Rebind();
    }

    protected TransVolumeUserDataItem GetDisplayedColumnsByViewID()
    {
        return GetDisplayedColumnsByViewID(false);
    }

    protected TransVolumeUserDataItem GetDisplayedColumnsByViewID(bool isFullView)
    {
        var dataSource = GetCustomDisplayedColumns(isFullView);
        string listColumns = string.Empty;
        if (dataSource.Rows.Count > 0)
        {
            listColumns = dataSource.Rows[0]["ViewData"].ToString();
        }
        else
        {
            SetViewCombo();
            LoggerManager.Error("GetDisplayedColumnsByViewID at rm_MCF_NewRiskReportTransactionHistory have no data (data issue) at" + DateTime.Now);
            return null;
        }
        string[] columns = listColumns.Split(',');
        for (int i = 0; i < columns.Length; i++)
        {
            columns[i] = columns[i].Trim();
        }
        List<string> items = columns.ToList<string>();
        var columnData = new TransVolumeUserDataItem
        {
            DisplayedColumns = items
        };
        return columnData;
    }

    private DataTable GetCustomDisplayedColumns(bool isFullView)
    {
        string fullViewID = string.Empty;
        if (isFullView)
        {
            fullViewID = GetCustomViewIDFullView();
        }
        string selectValue = string.IsNullOrEmpty(uxViewColumns.SelectedValue) ? "0" : uxViewColumns.SelectedValue;
        var parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(false);
        parameters.Add(new FilterParameter("@UserID", SessionManager.CurrentUser, DbType.String));
        parameters.Add(new FilterParameter("@CustomViewID", isFullView ? fullViewID : selectValue, DbType.Int32));
        parameters.Add(new FilterParameter("@UserRecID", SessionManager.CurrentUser.RecId, DbType.Guid));
        return WebServices.RiskServices.GetReports("spa_RM_MCF_Get_CustomView", parameters);
    }

    private string GetCustomViewIDFullView()
    {
        var parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(false);
        parameters.AddLanguageID();
        parameters.Add(new FilterParameter("@UserID", SessionManager.CurrentUser, DbType.String));
        parameters.Add(new FilterParameter("@ViewMode", 1, DbType.Int32));
        parameters.Add(new FilterParameter("@UserRecID", SessionManager.CurrentUser.RecId, DbType.Guid));
        parameters.Add(new FilterParameter("@PageType", "RiskReport_TransactionHistory", DbType.String));
        if (FlagLink == 0) // in new window
        {
            parameters.Add(new FilterParameter("@PageName", "New Window", DbType.String));
        }
        DataTable data = WebServices.RiskServices.GetReports(SPA_GET_CUSTOMVIEW_LIST, parameters);

        var fullViewID = data.AsEnumerable().Where(x => x.Field<int>("ViewType") == 0).CopyToDataTable();
        return fullViewID.Rows.Count > 0 ? fullViewID.Rows[0]["CustomViewID"].ToString() : "0";
    }

    protected void uxTransactionDetails_PreRender(object sender, EventArgs e)
    {
        var defaultColumnData = GetDisplayedColumnsByViewID();
        if (defaultColumnData == null)
        {
            return;
        }
        _cachedDisplayedColumns = defaultColumnData.DisplayedColumns;

        var listDisplayedColumns = _cachedDisplayedColumns;

        ShowHideColumnsAndBindHeaderTooltip(listDisplayedColumns, Page.IsPostBack);

        GenerateAllColumnsTransactionHistoryGrid();

        var rows = uxTransactionDetails.AS_DataSource != null ? uxTransactionDetails.AS_DataSource.Rows.Count : 0;
        if (rows == 0)
        {
            uxTransactionDetails.AllowPaging = false;
        }
        else
        {
            uxTransactionDetails.AllowPaging = true;
        }

        if (IsShowNewExport)
        {
            uxExportQueueTransactionHistory.HasData = rows > 0;
        }

        uxTransactionDetails.MasterTableView.Rebind();

        if (Page.IsPostBack)
        {
            if (uxTransactionDetails.EnableFilterItemsPersistence)
            {
                uxTransactionDetails.RestoreFilters();
            }

            if (uxTransactionDetails.EnableSortItemsPersistence)
            {
                uxTransactionDetails.RestoreSortItem();
            }

            ShowHideColumnsAndBindHeaderTooltip(listDisplayedColumns, Page.IsPostBack);
        }
    }

    private void ShowHideColumnsAndBindHeaderTooltip(List<string> listDisplayedColumns, bool isPostBack)
    {
        foreach (GridColumn col in uxTransactionDetails.MasterTableView.Columns)
        {
            if (col.Visible)
            {
                var uniqueName = col.UniqueName;
                if (col.UniqueName == "PartialCardNumber")
                {
                    uniqueName = "CardNumber";
                }
                var index = listDisplayedColumns.FindIndex(c => c.ToString() == uniqueName);
                if (index >= 0)
                {
                    col.Visible = true;
                    col.OrderIndex = (index + 1);

                    var tableCol = uxTransactionDetails.MasterTableView.Columns.FindByUniqueName(uniqueName);
                    var partialCardNumberCol = uxTransactionDetails.MasterTableView.Columns.FindByUniqueName("PartialCardNumber");
                    var colTooltip = string.Format("{0}.HeaderTooltip", uniqueName);
                    string tableColId = GetLocalResourceObject(colTooltip).ToString();

                    if (isPostBack)
                    {
                        GenerateTooltipHeaderByColumnId(uniqueName, tableCol, tableColId, partialCardNumberCol);
                    }
                    else
                    {
                        if (!tableCol.HeaderText.Contains(string.Format("id='{0}'", tableColId)))
                        {
                            GenerateTooltipHeaderByColumnId(uniqueName, tableCol, tableColId, partialCardNumberCol);
                        }
                    }
                }
                else
                {
                    col.Visible = false;
                }
            }
        }
    }

    private void GenerateTooltipHeaderByColumnId(string uniqueName, GridColumn tableCol, string tableColId, GridColumn partialCardNumberCol)
    {
        string headerTextColumn = GetLocalResourceObject(string.Format("{0}.HeaderText", uniqueName)).ToString();
        string columnToolTip = GetLocalResourceObject(string.Format("{0}Tooltip.HeaderDescription", uniqueName)).ToString();
        RM_MCF_GeneralFuncsLib.GenerateTooltipHeaderDefaulColumnID(tableCol, headerTextColumn, uxTransactionDetails, tableColId, columnToolTip);
        if (uniqueName == "CardNumber") // if col is account number, generate tooltip for hidden partial account number also
        {
            RM_MCF_GeneralFuncsLib.GenerateTooltipHeaderDefaulColumnID(partialCardNumberCol, headerTextColumn, uxTransactionDetails, tableColId, columnToolTip);
        }
        tableCol.HeaderTooltip = " ";
    }

    private void GenerateAllColumnsTransactionHistoryGrid()
    {
        var dataSource = this.dataGetTransactionDetails;
        if (dataSource != null)
        {
            uxTransactionDetails.DataSource = dataSource;
        }
        var listColumns = new List<ColumnDisplayedConfigurationItem>();
        var colGrid = uxTransactionDetails.MasterTableView.Columns;
        foreach (GridColumn col in colGrid)
        {
            if (col.UniqueName != WebSiteConstants.GRID_COLUMN_TEMP && GetLocalResourceObject(col.UniqueName + ".HeaderText") != null)
            {
                listColumns.Add(new ColumnDisplayedConfigurationItem
                {
                    ColumnName = col.UniqueName,
                    ColumnText = GetLocalResourceObject(col.UniqueName + ".HeaderText").ToString(),
                    IsDisplayed = false,
                    OrderIndex = col.OrderIndex
                });
            }
        }
        RiskSessionManager.RiskReportTransactionHistoryColumn = listColumns;
    }

    #region ---- Multi thread ----
    private FilterParameterCollection GetTransactionHistoryParameters(DataBindAction action)
    {
        //DataBindAction actiontemp = (DataBindAction)action; 
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserRiskParams();
        switch (action)
        {
            case DataBindAction.BindTransactionDetails:
                {
                    parameters.AddLoggedInUserRiskParams();
                    SecurePage securePage = HttpContext.Current.Handler as SecurePage;
                    if (GeneralFuncsLib.CheckCSViewFullCard((SecurePage)this.Page))
                    {
                        parameters.AddDecryptDataParams("CardNumber", _IsExporting);
                    }
                    parameters.Add(new FilterParameter("@MerchantNumber", MerchantNumber, DbType.String));
                    parameters.Add(new FilterParameter("@ReportDate", ReportDate, DbType.DateTime));
                    parameters.Add(new FilterParameter("@DateRange", RiskSessionManager.TransDateRangeModal, DbType.Int32));
                    parameters.Add(new FilterParameter("@PageNo", 1, DbType.Int32));
                    parameters.Add(new FilterParameter("@PageSize", 10, DbType.Int32));
                    parameters.Add(new FilterParameter("@IsPaging", true, DbType.Boolean));
                    parameters.Add(new FilterParameter("@stOrder", RiskSessionManager.TransactionHistorySortOrder, DbType.String));
                    parameters.AddLanguageID();
                    //spa_RM_MCF_RiskReport_GetTransactionHistory
                    break;
                }
            case DataBindAction.BindViewColumn:
                {
                    parameters.AddLanguageID();
                    parameters.Add(new FilterParameter("@ViewMode", 0, System.Data.DbType.Int32));
                    parameters.Add(new FilterParameter("@ASClient", SessionManager.CurrentClient, System.Data.DbType.Int32));
                    parameters.Add(new FilterParameter("@UserRecID", SessionManager.CurrentUser.RecId, DbType.Guid));
                    parameters.Add(new FilterParameter("@PageType", "RiskReport_TransactionHistory", DbType.String));
                    //return WebServices.RiskServices.GetReports("spa_RM_MCF_GetCustomViewList", parameters);
                    break;
                }

        }
        return parameters;
    }

    public void BindDataFromThread(List<DataSourceParallelResponse> dataSources)
    {
        string queryString = this.Page.BuildSecureQueryString(string.Format("MerchantNumber={0}&ReportDate={1}",
                   MerchantNumber, ReportDate));
        string url = string.Format("<span class=\"dark-blue\"><a class=\"link-back\" href=\"#\" onclick=\"OpenInstanceWindow('rm_MCF_NewRiskReport_TransactionHistoryModal.aspx?{0}','DQWindow'); return false;\">", queryString);
        uxTransHistoryHeader.Text = VeraCodeSolution.DoVeraCode(url + GetLocalResourceObject("RiskTransactionHistory_ascx_cs_NewWindow").ToString() + "</a></span>");
        if (dataSources.IsNotNullData())
        {
            var data = dataSources.SingleOrDefault(m => m.FeatureName == DataBindAction.BindTransactionDetails.ToString());
            if (data.IsNotNullData())
                dataGetTransactionDetails = data.DataSource;
        }

        if (!string.IsNullOrEmpty(SessionManager.CurrentMerchantNumber))
        {
            if (Page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RSK_MNG_PRIVATE_VIEW) || Page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RSK_MNG_PRIVATE_VIEW_MS) ||
                Page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RSK_MNG_PUBLIC_VIEW_MS) || Page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RSK_MNG_PUBLIC_VIEW))
            {
                uxCustomizeColumnLink.Visible = true;
                string encodeURL = string.Format("rm_MCF_ManageTransactionHistoryCustomViewsModal.aspx?" + this.Page.BuildSecureQueryString(string.Format("merchantNumber={0}&flagLink={1}&customViewMessageResourceTypeMode={2}", SessionManager.CurrentMerchantNumber, FlagLink, CustomViewMessageResourceType.CustomView.ToString())));
                uxCustomizeColumnLink.OnClientClick = "return doOpenTransactionHistoryNewPopup('" + encodeURL + "')";
            }
            else
            {
                uxCustomizeColumnLink.Visible = false;
            }
        }

        uxTransactionDetails.Rebind();
        SetViewCombo();
    }


    #region SPA INFO
    public SpaInfo SpaGetTransactionHistory
    {
        get
        {
            return new SpaInfo()
            {
                FeatureName = DataBindAction.BindTransactionDetails.ToString(),
                SpaName = SPA_GET_TRANS_HISTORY,
                Parameters = GetTransactionHistoryParameters(DataBindAction.BindTransactionDetails)
            };
        }
    }

    public SpaInfo SpaGetCustomViewList
    {
        get
        {
            return new SpaInfo()
            {
                FeatureName = DataBindAction.BindViewColumn.ToString(),
                SpaName = SPA_GET_CUSTOMVIEW_LIST,
                Parameters = GetTransactionHistoryParameters(DataBindAction.BindViewColumn)
            };
        }
    }
    #endregion
    #endregion ---- Multi thread ----    
}
