using AS.Common.DBManager;
using AS.Controls.Exporter;
using AS.Controls.Grid;
using AS.Web.Business;
using AS.Web.UI.AppCode.General;
using AS.Web.UI.Controls;
using System;
using System.Data;
using Telerik.Web.UI;
using CardTypes = WebSiteEnums.CardTypes;

public partial class UserControls_rm_MCF_MerchantFraudReport : GlobalUserControl
{    
    private HierarchyFilterValue _reportValue = null;

    protected HierarchyFilterValue SavedReportFilterValue
    {
        get
        {
            return SessionManager.CurrentReportFilter;
        }
        set
        {
            SessionManager.CurrentReportFilter = value;
        }
    }   

    private bool IsSearch = false;   

    enum PostBackAction
    {
        DoSearching,
    }

    public CardTypes CardType { get; set; }

    private string OrderDefault
    {
        get
        {
            return ViewState["OrderDefault"].ToString();
        }
        set
        {
            ViewState["OrderDefault"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SavedReportFilterSyncManager.RestoreBrandFraudReportFilter(new UCBrandFraudReportFilterControl {                 
                UxDaily = uxDaily,
                UxMonthly = uxMonthly,
                UxRange = uxRange,
                UxDate = uxDate,    
                UxEndDate = uxEndDate,
                UxFromDate = uxFromDate,
                UxAcquirerList = uxAcquirerList                 
            }, CardType, ref _reportValue);

            if (CardType == CardTypes.Visa)
            {
                uxExport.GridTitle = string.Format(GetLocalResourceObject("GridVisaTitle.Text").ToString());
            }
            else
            {
                uxExport.GridTitle = string.Format(GetLocalResourceObject("GridMasterCardTitle.Text").ToString());
            }

            uxExport.GridSubTitle = GetSubTitle();
            uxDate.MaxDate = DateTime.Now;
            uxFromDate.MaxDate = DateTime.Now;
            uxEndDate.MaxDate = DateTime.Now;

            DataBindComboPaymentEntitieTypes();
            GetMerchantNumberFilter();
            OrderDefault = "FraudAmount DESC, TransactionCount DESC";
        }
    }

    private void GetMerchantNumberFilter()
    {
        if (_reportValue.HierarchyMode.Equals(HierarchyMode.MERCHNUMBER_NR_PARTIAL, StringComparison.OrdinalIgnoreCase))
        {
            uxPaymentEntitieTypes.SelectedValue = WebSiteConstants.PaymentEntitieTypeMerchantIDPartial;
            uxMerchantNumber.Text = _reportValue.Value;
        }
        else if(_reportValue.HierarchyMode.Equals(HierarchyMode.MERCHANT_NR, StringComparison.OrdinalIgnoreCase))
        {
            uxPaymentEntitieTypes.SelectedValue = WebSiteConstants.PaymentEntitieTypeMerchantIDFull;
            uxMerchantList.Text = _reportValue.Value;
            uxMerchantList.SelectedValue = _reportValue.Value;
        }
    }

    private void DataBindComboPaymentEntitieTypes()
    {
        uxPaymentEntitieTypes.DataTextField = "DataText";
        uxPaymentEntitieTypes.DataValueField = "DataKey";
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        uxPaymentEntitieTypes.DataSource = WebServices.RiskServices.GetReports("spa_RM_RCF_GetMerchantFraudReportFilter", parameters);
        uxPaymentEntitieTypes.DataBind();
    }

    public void uxReportGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        SavedReportFilterSyncManager.SaveBrandFraudReportFilter(new UCBrandFraudReportFilterControl
        {            
            UxDaily = uxDaily,
            UxMonthly = uxMonthly,
            UxRange = uxRange,
            UxDate = uxDate,
            UxEndDate = uxEndDate,
            UxFromDate = uxFromDate,
            UxPaymentEntitieTypes = uxPaymentEntitieTypes,
            UxAcquirerList = uxAcquirerList,    
            UxMerchantList = uxMerchantList,    
            UxMerchantNumber = uxMerchantNumber            
        }, UCBrandFraudReportFilterMode.MerchantFraudReport, ref _reportValue, IsSearch);

        FilterParameterCollection _parames = new FilterParameterCollection();
        _parames.AddLoggedInUserReportingParams(true);        

        if (uxDaily.Checked)
        {
            _parames.Add(new FilterParameter("@ToDate", _reportValue.DateOptionValue.From, DbType.Date));
        }
        else if (uxMonthly.Checked)
        {
            _parames.Add(new FilterParameter("@ToDate", _reportValue.DateOptionValue.From, DbType.Date));
            _parames.Add(new FilterParameter("@DateRange", 30, DbType.Int32));
        }
        else if (uxRange.Checked)
        {
            _parames.Add(new FilterParameter("@FromDate", _reportValue.DateOptionValue.From, DbType.Date));
            _parames.Add(new FilterParameter("@ToDate", _reportValue.DateOptionValue.To, DbType.Date));
        }

        uxExport.GridSubTitle = GetSubTitle();

        string cardTypeCode = CardType == CardTypes.Visa ? "VI" : "MC";
        _parames.Add(new FilterParameter("@CardTypeCode", cardTypeCode, DbType.String));

        if (!string.IsNullOrEmpty(OrderDefault))
        {
            uxReportGrid.AS_SortExpression = OrderDefault;
            uxReportGrid.MasterTableView.SortExpressions.AddSortExpression("FraudAmount DESC");
        }

        _parames.Add(new FilterParameter("@SearchMode", uxPaymentEntitieTypes.SelectedValue, DbType.String));
        _parames.Add(new FilterParameter("@SearchValue", GetSearchValue(), DbType.String));

        uxReportGrid.DataSourceInvoker = new ASFuncInvoker(WebServices.RiskServices, WebSiteConstants.GET_REPORT_METHOD_NAME,
                            new object[] { "spa_RM_RCF_GetMerchantFraudReport", ReportServices.ConvertToFilterParamWSArray(_parames) });
    }

    private DateTime GetToDateValue()
    {
        DateTime toDate;
        if (uxDaily.Checked || uxMonthly.Checked)
        {
            toDate = uxDate.SelectedDate.Value;
        }
        else 
        {           
            toDate = uxEndDate.SelectedDate.Value;
        }

        return toDate;
    }

    private string GetSearchValue()
    {
        string result = string.Empty;

        if (uxPaymentEntitieTypes.SelectedValue == WebSiteConstants.PaymentEntitieTypeAcquirerID)
        {
            if (string.IsNullOrEmpty(uxAcquirerList.SelectedValue))
            {
                result = uxAcquirerList.Text;
            }
            else
            {
                result = uxAcquirerList.SelectedValue;
            }
        }
        else if (uxPaymentEntitieTypes.SelectedValue == WebSiteConstants.PaymentEntitieTypeMerchantIDFull)
        {
            if (string.IsNullOrEmpty(uxMerchantList.SelectedValue))
            {
                result = uxMerchantList.Text;
            }
            else
            {
                result = uxMerchantList.SelectedValue;
            }
        }
        else if (uxPaymentEntitieTypes.SelectedValue == WebSiteConstants.PaymentEntitieTypeMerchantIDPartial)
        {
            result = uxMerchantNumber.Text.Trim();
        }

        return result;
    }

    protected void uxReportGrid_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView dataRow = e.Item.DataItem as DataRowView;

            dataItem["AcquirerID"].Text = string.Format("<span title=\"{0}\">{1}</span>", dataRow["AcquirerName"], dataRow["AcquirerID"]);

            string merchantQueryString = Page.BuildSecureQueryString(string.Format("merchantnumber={0}", dataRow["MerchantNumber"].ToString()));
            string merchantUrlString = ResolveUrl("~/MerchantProfile.aspx?") + merchantQueryString;
            dataItem["MerchantNumber"].Text = string.Format("<a href=\"{0}\" style=\"cursor:pointer\">{1}</a>", merchantUrlString, dataRow["MerchantNumber"].ToString());

            string detailQueryString = Page.BuildSecureQueryString(string.Format("ToDate={0}&AcquirerID={1}&MerchantNumber={2}&DateTimeSubTitle={3}", GetToDateValue(), dataRow["AcquirerID"], dataRow["MerchantNumber"], GetDateTimeSubTitle()));

            string detailPage = "rm_MCF_VisaMerchantFraudDetail_ViewModal.aspx";

            if(CardType == CardTypes.MasterCard)
            {
                detailPage = "rm_MCF_MasterCardMerchantFraudDetail_ViewModal.aspx";
            }

            string detailUrlString = string.Format("{0}?", detailPage) + detailQueryString;
            dataItem["Detail"].Text = string.Format("<a href=\"#\" style=\"cursor:pointer\" onclick=\"return ShowPopupModal('{0}','auto');\">{1}</a>", detailUrlString, "Details");
        }
    }

    private string GetDateTimeSubTitle()
    {
        if (uxDaily.Checked || uxMonthly.Checked)
        {
            return _reportValue.DateOptionValue.From.ToString("MM/dd/yyyy");
        }
        else if (uxRange.Checked)
        {
            return string.Format("{0} - {1}", _reportValue.DateOptionValue.From.ToString("MM/dd/yyyy"), _reportValue.DateOptionValue.To.ToString("MM/dd/yyyy"));
        }

        return string.Empty;
    }    

    protected void uxSearchButton_Click(object sender, EventArgs e)
    {
        IsSearch = true;
        OnPostBackActions(PostBackAction.DoSearching);
        IsSearch = false;
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.DoSearching:
                {
                    uxReportGrid.Rebind();
                }
                break;
        }
    }

    public void uxAcquirerList_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        if (e.Text.Length >= 1)
        {            
            DataTable dt = GeneralFuncsLib.GetAcquirers(CardType, e.Text);
            uxAcquirerList.DataSource = dt;
            uxAcquirerList.DataBind();
        }
        else
        {
            uxAcquirerList.Items.Clear();
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

    private string GetSubTitle()
    {
        string paymentEntitieType = string.Empty;
        string fromDateToDate = string.Empty;
        string result = string.Empty;

        if (uxPaymentEntitieTypes.SelectedValue == WebSiteConstants.PaymentEntitieTypeAcquirerID)
        {
            if (string.IsNullOrEmpty(uxAcquirerList.Text.Trim()))
            {
                paymentEntitieType = "All Acquirers";
            }
            else
            {
                if (string.IsNullOrEmpty(uxAcquirerList.SelectedValue))
                    paymentEntitieType = string.Format("Acquirer: {0}", uxAcquirerList.Text);
                else
                    paymentEntitieType = string.Format("Acquirer: {0}", uxAcquirerList.SelectedValue);
            }
        }
        else if (uxPaymentEntitieTypes.SelectedValue == WebSiteConstants.PaymentEntitieTypeMerchantIDFull)
        {
            if (string.IsNullOrEmpty(uxMerchantList.Text.Trim()))
            {
                paymentEntitieType = "All Merchants";
            }
            else
            {
                if (string.IsNullOrEmpty(uxMerchantList.SelectedValue))
                {
                    paymentEntitieType = string.Format("{0}: {1}", uxMerchantList.Text, GeneralFuncsLib.GetMerchantName(uxMerchantList.Text));
                }
                else
                {
                    paymentEntitieType = string.Format("{0}: {1}", uxMerchantList.SelectedValue, GeneralFuncsLib.GetMerchantName(uxMerchantList.SelectedValue));
                }
            }
        }
        else if (uxPaymentEntitieTypes.SelectedValue == WebSiteConstants.PaymentEntitieTypeMerchantIDPartial && !string.IsNullOrEmpty(uxMerchantNumber.Text.Trim()))
        {
            paymentEntitieType = string.Format("Multiple Merchants: {0}", uxMerchantNumber.Text.Trim());
        }

        if (uxDaily.Checked || uxMonthly.Checked)
        {
            fromDateToDate = _reportValue.DateOptionValue.From.ToString("MM/dd/yyyy");
        }
        else if (uxRange.Checked)
        {
            fromDateToDate = string.Format("{0} - {1}", _reportValue.DateOptionValue.From.ToString("MM/dd/yyyy"), _reportValue.DateOptionValue.To.ToString("MM/dd/yyyy"));
        }

        result = string.Format("{0} ({1})", paymentEntitieType, fromDateToDate);
        return result;
    }

    protected void uxExporter_NeedExportConfig(object sender, ExportConfig exportConfig)
    {        
        string filename = CardType == CardTypes.Visa ? "VisaMerchantFraudReport" : "MasterCardMerchantFraudReport";
        exportConfig.FileName = string.Format("{0}_{1}", filename, DateTime.Now.ToString("yyyy-MM-dd_hhmmss"));
        exportConfig.ReportHeader = string.Format("{0} \n{1}", uxExport.GridTitle, uxExport.GridSubTitle);
        uxReportGrid.Columns.FindByUniqueName("Detail").Visible = false;

    }

    protected void uxReportGrid_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        OrderDefault = string.Empty;
    }

    protected void uxReportGrid_PreRender(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
        {
            if (uxReportGrid.EnableFilterItemsPersistence)
            {
                uxReportGrid.RestoreFilters();
            }

            if (uxReportGrid.EnableSortItemsPersistence)
            {
                uxReportGrid.RestoreSortItem();
            }
        }
        ASRadControlHelper.ShowHidePagingControl(uxReportGrid);
    }

    protected void uxReportGrid_DataSourceReady(object sender, EventArgs e)
    {
        ASRadControlHelper.ShowHidePagingControl(uxReportGrid);
    }
}