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
using Telerik.Web.UI;

[PagePermission("RskBrandFraudReports")]
public partial class rm_MCF_VisaMerchantFraudDetail_ViewModal : ReportPage
{
    private const int _DateRange = 30;
    private DateTime _toDate;    
    private string _acquirerId;
    private string _merchantNumber;
    private string _dateTimeSubTitle;

    enum DataBindAction
    {
        BindReportGrid
    }

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
        if (IsIntruderDetected)
            return;

        PageType = SecurePageType.Modal;
        IsBindDataOnLoad = true;
        ProcessQueryString();

        if (!IsPostBack)
        {
            OrderDefault = "FraudAmount DESC";            
        }
    }

    void ProcessQueryString()
    {
        _toDate = DateTime.Parse(SecureQueryString["ToDate"]);        
        _acquirerId = SecureQueryString["AcquirerID"].ToString();
        _merchantNumber = SecureQueryString["MerchantNumber"].ToString();
        _dateTimeSubTitle = SecureQueryString["DateTimeSubTitle"].ToString();
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
                    
                    FilterParameterCollection parameters = new FilterParameterCollection();

                    parameters.AddLoggedInUserReportingParams();
                    parameters.Add(new FilterParameter("@DateRange", _DateRange, DbType.Int32));
                    parameters.Add(new FilterParameter("@ToDate", _toDate, DbType.Date));                    
                    parameters.Add(new FilterParameter("@AcquirerID", _acquirerId, DbType.AnsiString));

                    if (string.IsNullOrEmpty(_merchantNumber)){
                        parameters.Add(new FilterParameter("@MerchantNumber", null, DbType.AnsiString));
                    }
                    else {
                        parameters.Add(new FilterParameter("@MerchantNumber", _merchantNumber, DbType.AnsiString));
                    }

                    if (!string.IsNullOrEmpty(OrderDefault))
                    {
                        uxReportGrid.AS_SortExpression = OrderDefault;
                        uxReportGrid.MasterTableView.SortExpressions.AddSortExpression(OrderDefault);
                    }

                    grid.DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { "spa_RM_RCF_GetVisaFraudDetail", ReportServices.ConvertToFilterParamWSArray(parameters) });

                    uxExport.GridTitle = GetLocalResourceObject("GridTitle.Text").ToString();                    
                    uxExport.GridSubTitle = GetSubTitle();
                    break;
                }
        }
    }

    private string GetSubTitle()
    {
        string result = string.Format("({0})", _dateTimeSubTitle);     
        return result;
    }


    protected void uxReportGrid_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView dataRow = e.Item.DataItem as DataRowView;

            dataItem["AcquirerID"].ToolTip = dataRow["AcquirerName"].ToString();
            dataItem["FraudType"].ToolTip = dataRow["FraudTypeDescription"].ToString();
            dataItem["ECIMOTO"].ToolTip = dataRow["ECIMOTODescription"].ToString();
            dataItem["POSEntryMode"].ToolTip = dataRow["POSEntryDescription"].ToString();
            dataItem["POSTerminalCapability"].ToolTip = dataRow["POSTerminalDescription"].ToString();

            //MerchantNumber
            var merchantNumber = dataRow["MerchantNumber"].ToString();
            string merchantNumberQueryString = this.BuildSecureQueryString("MerchantNumber=" + merchantNumber);
            string urlMerchant = "rm_MCF_RiskReport.aspx?" + merchantNumberQueryString;
            var linkMerchant = "<a class=\"link\" href=\"javascript:void(0)\" onclick=\"openPopupWindow('" + urlMerchant + "','RiskReportWindow'); return false;\">";
            dataItem["MerchantNumber"].Text = VeraCodeSolution.DoVeraCode(linkMerchant + merchantNumber + "</a>");
        }
    }

    protected void uxReportGrid_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        OrderDefault = string.Empty;
    }

    protected void uxExport_OnNeedExportConfig(object sender, ExportConfig exportConfig)
    {        
        string fileName = "VisaMerchantFraudDetail_" + DateTime.Now.ToString("yyyy-MM-dd_hhmmss");        
        exportConfig.FileName = HttpUtility.UrlEncode(fileName);        
        exportConfig.ReportHeader = uxExport.GridTitle + "\n" + uxExport.GridSubTitle;
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