using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Exporter;
using AS.Controls.Grid;
using AS.Web.Business;
using AS.Web.UI.AppCode.General;
using System;
using System.Data;
using System.Web;
using Telerik.Web.UI;

public partial class rm_MCF_DetectionManageCustomViewsModal : ReportPage
{
    private DateTime _reportDate;
    private int _dateRange;
    private string _cardTypeCode;
    private string _hashedAccountNumber;
    private string _cardNumber;

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
            OrderDefault = "ReportDate DESC";
        }
    }

    void ProcessQueryString()
    {
        _reportDate = DateTime.Parse(SecureQueryString["ReportDate"]);
        _dateRange = int.Parse(SecureQueryString["DateRange"]);
        _cardTypeCode = SecureQueryString["CardTypeCode"].ToString();
        _hashedAccountNumber = SecureQueryString["HashedAccountNumber"].ToString();
        _cardNumber = SecureQueryString["CardNumber"].ToString();
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

                    string spaName = "spa_RM_RCF_GetNegativeOptionReport_Detail";
                    FilterParameterCollection parameters = new FilterParameterCollection();

                    parameters.AddLoggedInUserReportingParams();
                    parameters.Add(new FilterParameter("@ReportDate", _reportDate, DbType.Date));
                    parameters.Add(new FilterParameter("@DateRange", _dateRange, DbType.Int32));
                    parameters.Add(new FilterParameter("@CardTypeCode", _cardTypeCode, DbType.AnsiString));
                    parameters.Add(new FilterParameter("@HashedAccountNumber", _hashedAccountNumber, DbType.AnsiString));

                    if (!string.IsNullOrEmpty(OrderDefault))
                    {
                        uxReportGrid.AS_SortExpression = OrderDefault;
                        uxReportGrid.MasterTableView.SortExpressions.AddSortExpression(OrderDefault);
                    }

                    grid.DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(parameters) });

                    uxExport.GridTitle = VeraCodeSolution.ValidateResponseData("Detailed Transactions History");
                    var subTitle = GetSubTitle(_cardNumber, _reportDate);
                    uxExport.GridSubTitle = VeraCodeSolution.ValidateResponseData(subTitle);
                    break;
                }
        }
    }  

    private string GetSubTitle(string cardNumber, DateTime? dateTime)
    {
        string result = "Card Number: ";
        if (dateTime.HasValue)
        {
            result += cardNumber + " (" + Formatter.ToMMddyyyy(dateTime.Value.AddDays(-59)) + " - " + Formatter.ToMMddyyyy(dateTime) + ")";
        }
        return result;
    }


    protected void uxReportGrid_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView dataRow = e.Item.DataItem as DataRowView;

            dataItem["ISONumber"].ToolTip = dataRow["ISOName"].ToString();

        }
    }

    protected void uxReportGrid_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        OrderDefault = string.Empty;
    }

    protected void uxExport_OnNeedExportConfig(object sender, ExportConfig exportConfig)
    {   
        string fileName = "DetailedTransactionHistory_" + DateTime.Now.ToString("yyyy-MM-dd_hhmmss");
        exportConfig.FileName = HttpUtility.UrlEncode(fileName);
        var subTitle = GetSubTitle(_cardNumber, _reportDate);
        exportConfig.ReportHeader = "Detailed Transaction History\n" + subTitle;

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
    }

    protected void uxReportGrid_DataSourceReady(object sender, EventArgs e)
    {
        ASRadControlHelper.ShowHidePagingControl(uxReportGrid);
    }
}