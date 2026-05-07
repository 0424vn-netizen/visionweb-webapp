using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Exporter;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Web.Business;
using AS.Web.UI.AppCode.General;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using Telerik.Web.UI;

[PagePermission("RskNegativeOptionReport")]
public partial class rm_MCF_NegativeOptionReport : ReportPage
{
    private const int _DayRolling = 60;
    private bool _isExporting = false;

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

    private DateTime ReportDateValue
    {
        get
        {
            return (DateTime)this.ViewState["ReportDateValue"];
        }
        set
        {
            ViewState["ReportDateValue"] = value;
        }
    }

    private bool HasViewFullCard
    {
        get
        {
            return (bool)this.ViewState["HasViewFullCard"];
        }
        set
        {
            ViewState["HasViewFullCard"] = value;
        }
    }

    enum DataBindAction
    {
        BindReportGrid
    }
    enum PostBackAction
    {
        DoSearching        
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) 
            return;

        IsBindDataOnLoad = true;
        HasViewFullCard = PermissionManager.CheckCSViewFullCard(((SecurePage)this.Page));
        if (!IsPostBack)
        {
            OrderDefault = "TransactionCount DESC";
            DataBindComboCardType();
            SetFilterDefault();
        }
    }

    private void DataBindComboCardType()
    {
        uxCardType.DataTextField = "Description";
        uxCardType.DataValueField = "CardTypeCode";
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        uxCardType.DataSource = WebServices.RiskServices.GetReports("spa_RM_RCF_GetCardTypeList", parameters);
        uxCardType.DataBind();
    }

    private void SetFilterDefault()
    {
        uxCardType.SelectedValue = "VI";
        uxReportDate.SelectedDate = ReportDateValue = uxReportDate.MaxDate = DateTime.Today;
    }

    protected void uxBtnSubmit_Click(object sender, EventArgs e)
    {
        UpdateFilterValues();
        OnPostBackActions(PostBackAction.DoSearching, sender);
    }

    private void UpdateFilterValues()
    {
        ReportDateValue = uxReportDate.SelectedDate.Value;
    }

    protected override void DoGridNeedDataSource(ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.BindReportGrid, sender);
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindReportGrid:
                {
                    ASGrid grid = sender as ASGrid;

                    string spaName = "spa_RM_RCF_GetNegativeOptionReport";
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    
                    parameters.AddLoggedInUserReportingParams();
                    parameters.Add(new FilterParameter("@ReportDate", ReportDateValue, DbType.Date));
                    parameters.Add(new FilterParameter("@DateRange", _DayRolling, DbType.Int32));
                    parameters.Add(new FilterParameter("@CardTypeCode", uxCardType.SelectedValue, DbType.AnsiString));

                    if (!string.IsNullOrEmpty(OrderDefault))
                    {
                        uxReportGrid.AS_SortExpression = OrderDefault;
                        uxReportGrid.MasterTableView.SortExpressions.AddSortExpression(OrderDefault);
                    }
                    
                    string decryptDataParams = GetDecryptDataParams();

                    if (!string.IsNullOrEmpty(decryptDataParams))
                    {
                        parameters.AddDecryptDataParams(decryptDataParams, _isExporting);
                    }
                    grid.DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(parameters) });

                    uxExport.GridTitle = VeraCodeSolution.ValidateResponseData("Negative Option Report");
                    var subTitle = GetSubTitle(uxCardType.SelectedItem.Text, ReportDateValue);
                    uxExport.GridSubTitle = VeraCodeSolution.ValidateResponseData(subTitle);
                    break;
                }
        }
    }

    private string GetDecryptDataParams()
    {
        string decryptDataParams = string.Empty;
        if (HasViewFullCard)        
        {
            decryptDataParams = "CardNumber";
        }

        return decryptDataParams;
    }

    private string GetSubTitle(string cardType, DateTime? dateTime)
    {
        string result = "Card Type: ";
        if (dateTime.HasValue)
        {
            result += cardType + " (" + Formatter.ToMMddyyyy(dateTime.Value.AddDays(-59)) + " - " + Formatter.ToMMddyyyy(dateTime) + ")";
        }
        return result;
    }    

    protected override void OnPostBackActions(Enum type, object sender)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.DoSearching:
                {
                    this.uxReportGrid.Rebind();
                    break;
                }
        }

    }

    protected void uxReportGrid_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView dataRow = e.Item.DataItem as DataRowView;

            var partialCard = dataRow["PartialCardNumber"].ToString();
            var fullCard = dataRow["CardNumber"].ToString();

            var cardTypeCode = uxCardType.SelectedValue;
            var hashedAccountNumber = string.Empty;

            if (HasViewFullCard)
            {
                hashedAccountNumber = dataRow["CardNumber_Original"].ToString();
            }
            else
            {
                hashedAccountNumber = dataRow["CardNumber"].ToString();
            }

            var cardNumber = string.Empty;

            var cardQueryString = this.BuildSecureQueryString("cn=" + partialCard + "&cnf=" + fullCard + "&isRisk=1");
            var urlCardDetail = ResolveUrl("~/") + "CardHistoryModal.aspx?" + cardQueryString;
            var link = "<a class=\"link\" href=\"javascript:void(0)\" onclick=\"openPopupWindow('" + urlCardDetail + "','CardHistoryWindow'); return false;\">";


            if (HasViewFullCard)
            {
                dataItem["CardNumber"].Text = VeraCodeSolution.DoVeraCode(link + dataRow["CardNumber"].ToString() + "</a>");
                cardNumber = dataRow["CardNumber"].ToString();
            }
            else
            {
                dataItem["PartialCardNumber"].Text = VeraCodeSolution.DoVeraCode(link + dataRow["PartialCardNumber"].ToString() + "</a>");
                cardNumber = dataRow["PartialCardNumber"].ToString();
            }


            string detailQueryString = this.BuildSecureQueryString(string.Format("ReportDate={0}&DateRange={1}&CardTypeCode={2}&HashedAccountNumber={3}&CardNumber={4}", ReportDateValue, _DayRolling, cardTypeCode, hashedAccountNumber, cardNumber));
            string urlDetail = "rm_MCF_NegativeOptionReportDetailModal.aspx?" + detailQueryString;
            dataItem["Detail"].Text = string.Format("<a href=\"#\" style=\"cursor:pointer\" onclick=\"return ShowPopupModal('{0}','auto');\">{1}</a>", urlDetail, "Detail");

        }
    }

    private void ShowColumnsByPermissions()
    {
        if (HasViewFullCard)
        {
            uxReportGrid.MasterTableView.Columns.FindByUniqueName("CardNumber").Visible = true;
            uxReportGrid.MasterTableView.Columns.FindByUniqueName("PartialCardNumber").Visible = false;
        }
        else
        {
            uxReportGrid.MasterTableView.Columns.FindByUniqueName("CardNumber").Visible = false;
            uxReportGrid.MasterTableView.Columns.FindByUniqueName("PartialCardNumber").Visible = true;
        }
    }

    protected void uxReportGrid_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        OrderDefault = string.Empty;
    }

    protected void uxReportGrid_PreRender(object sender, EventArgs e)
    {        
        ShowColumnsByPermissions();

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
    }

    protected void uxExport_OnNeedExportConfig(object sender, ExportConfig exportConfig)
    {
        _isExporting = true;
        uxReportGrid.Columns.FindByUniqueName("CardNumber").Visible = false;
        uxReportGrid.Columns.FindByUniqueName("PartialCardNumber").Visible = true;
        uxReportGrid.Columns.FindByUniqueName("Detail").Visible = false;

        string fileName = "NegativeOptionReport_" + DateTime.Now.ToString("yyyy-MM-dd_hhmmss");
        exportConfig.FileName = HttpUtility.UrlEncode(fileName);
        var subTitle = GetSubTitle(uxCardType.SelectedItem.Text, ReportDateValue);
        exportConfig.ReportHeader = "Risk Management - Risk Analysis - Negative Option Report\n" + subTitle;

    }

    protected void uxReportGrid_DataSourceReady(object sender, EventArgs e)
    {
        ASRadControlHelper.ShowHidePagingControl(uxReportGrid);
    }
}