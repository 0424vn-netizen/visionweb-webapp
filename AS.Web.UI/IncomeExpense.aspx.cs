using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Common;
using System.Data;
using AS.Web.Business;
using Telerik.Web.UI;
using AS.Web.UI.Controls;
using AS.Controls.Pages;

[PagePermission("ViewIncomeExpense,MSViewIncomeExpense")]
public partial class IncomeExpense : ReportPage
{
    #region CONST STRING
    private const string OPEN_DATE = "OpenDate";
    private const string SIC_CODE = "SICcode";
    private const string STATE = "State";
    private const string ZIP = "Zip";

    #endregion
    enum DataBindAction
    {
        BindGridIncomeExpense
    }


    string FilteringMode
    {
        get
        {
            if (ViewState["FilteringMode"] == null)
                return "IncomeExpenseByEntity";
            else
                return ViewState["FilteringMode"].ToString();
        }
        set
        {
            ViewState["FilteringMode"] = value;
        }

    }
    bool IsNextMerchantMode { get; set; }

    protected override void PageInitialize()
    {
        base.PageInitialize();
        this.IsBindDataOnLoad = true;
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        OnRegisterEvent();

    }
    protected void OnRegisterEvent()
    {
        uxTabView.TabClick += (s, e) =>
        {

            switch (uxTabView.SelectedTab.Value)
            {
                case "ByEntity":
                    FilteringMode = "IncomeExpenseByEntity";
                    break;
                case "ByPeriod":
                    FilteringMode = "IncomeExpenseByPeriod";
                    break;
                case "ByMerchant":
                    FilteringMode = "IncomeExpenseByMerchant";
                    break;
            }
            uxDrilldownGrid.AS_SortExpression = string.Empty;
            uxDrilldownGrid.CurrentPageIndex = 0;
            uxDrilldownGrid.Rebind();

        };
        uxDrilldownGrid.ItemDataBound += (s, e) =>
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = e.Item as GridDataItem;
                DataRowView rowView = e.Item.DataItem as DataRowView;
                if (FilteringMode == "IncomeExpenseByMerchant" || IsNextMerchantMode)
                {
                    dataItem["DrilldownColumn"].Text = VeraCodeSolution.DoVeraCode(rowView["Entity"].ToString());
                }
                if (FilteringMode == "IncomeExpenseByPeriod")
                {
                    dataItem["Month"].Text = rowView["Entity"].DateFormatMMMYY();
                }
            }
        };
    }

    protected void DoNeedExportConfig(object sender, AS.Controls.Exporter.ExportConfig exportConfig)
    {
        string reportheader = uxExporterTop.GridHeader;
        if (FilteringMode == "IncomeExpenseByPeriod")
        {
            reportheader = reportheader + " - " + GetLocalResourceObject("RadTabResource2.Text").ToString();
        }
        else if (FilteringMode == "IncomeExpenseByEntity")
        {
            reportheader = reportheader + " - " + GetLocalResourceObject("RadTabResource1.Text").ToString();
        }
        else
        {
            reportheader = reportheader + " - " + GetLocalResourceObject("RadTabResource3.Text").ToString();
        }

        exportConfig.ReportHeader = reportheader;
        exportConfig.FileName = GeneralFuncsLib.GetFileName(reportheader);
        if (FilteringMode != "IncomeExpenseByPeriod")
        {
            if (((AS.Controls.UserControls.UxExport)sender).ExportButtonType == AS.Controls.UserControls.UxExport.ExportType.Excel)
            {
                uxDrilldownGrid.Columns.FindByUniqueName("EntityName").Visible = true;
            }
        }
        else
        {
            uxDrilldownGrid.Columns.FindByUniqueName("EntityName").Visible = false;
            uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").Visible = false;
            uxDrilldownGrid.Columns.FindByUniqueName("Month").Visible = true;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack && GeneralFuncsLib.IsMerchantMode(ReportFilter.CurrentValue.HierarchyMode)
          && !string.IsNullOrEmpty(ReportFilter.CurrentValue.Value))
        {
            FilteringMode = "IncomeExpenseByMerchant";
            uxTabView.FindTabByValue("ByMerchant").Selected = true;
        }
        //46652 - AW - Multi-Currency Transaction Display
        uxDrilldownGrid.Columns.FindByUniqueName("ChargeBacksAmount").HeaderText =
            uxDrilldownGrid.Columns.FindByUniqueName("ChargeBacksAmount").HeaderText.ToCurrencySymbol();
        uxDrilldownGrid.Columns.FindByUniqueName("ChargeBackPercent").HeaderText =
            uxDrilldownGrid.Columns.FindByUniqueName("ChargeBackPercent").HeaderText.ToCurrencySymbol();
        uxDrilldownGrid.Columns.FindByUniqueName("SalesAmount").HeaderText =
            uxDrilldownGrid.Columns.FindByUniqueName("SalesAmount").HeaderText.ToCurrencySymbol();
        uxDrilldownGrid.Columns.FindByUniqueName("CreditsAmount").HeaderText =
            uxDrilldownGrid.Columns.FindByUniqueName("CreditsAmount").HeaderText.ToCurrencySymbol();
        uxDrilldownGrid.Columns.FindByUniqueName("CreditPercent").HeaderText =
            uxDrilldownGrid.Columns.FindByUniqueName("CreditPercent").HeaderText.ToCurrencySymbol();
    }

    protected override void DoSwitchView()
    {
        DataTable HierarchyFilterLevel = SessionManager.HierarchyFilterDrillDown;
        DataRow currentHierarchyRow = HierarchyFilterLevel.FindObject("CurrentHierarchyMode", ReportFilter.CurrentValue.HierarchyMode);

        if (ReportFilter.CurrentValue.Value != "" && ReportFilter.CurrentValue.Value != SessionManager.AllHierarchyFilter.FindObject("HierarchyMode", currentHierarchyRow["CurrentHierarchyMode"].ToString())["HierarchyPrefix"].ToString())
        {
            if (currentHierarchyRow["NextHierarchyMode"].ToString().Equals("MERCHANTNUMBER", StringComparison.OrdinalIgnoreCase))
                IsNextMerchantMode = true;
        }
        else
        {
            IsNextMerchantMode = false;
        }
        if (uxTabView.FindTabByValue("ByMerchant").Selected == true)
        {
            uxReportFiltering.SetVisibleProfile(true);
            uxDrilldownGrid.Columns.FindByUniqueName(OPEN_DATE).Visible = true;
            uxDrilldownGrid.Columns.FindByUniqueName(SIC_CODE).Visible = true;
            uxDrilldownGrid.Columns.FindByUniqueName(STATE).Visible = true;
            uxDrilldownGrid.Columns.FindByUniqueName(ZIP).Visible = true;
        }
        else
        {
            uxReportFiltering.SetVisibleProfile(false);
            uxDrilldownGrid.Columns.FindByUniqueName(OPEN_DATE).Visible = false;
            uxDrilldownGrid.Columns.FindByUniqueName(OPEN_DATE).Visible = false;
            uxDrilldownGrid.Columns.FindByUniqueName(SIC_CODE).Visible = false;
            uxDrilldownGrid.Columns.FindByUniqueName(STATE).Visible = false;
            uxDrilldownGrid.Columns.FindByUniqueName(ZIP).Visible = false;
        }
        base.DoSwitchView();
    }

    protected override void DoGridNeedDataSource(ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        if (sender == uxDrilldownGrid)
        {
            OnDataBindControls(DataBindAction.BindGridIncomeExpense);
        }
    }


    public string GetIncomeDateFilterText()
    {
        string dateText = string.Empty;
        string beginDateText = string.Empty;
        string endDateText = string.Empty;
        IncomeFilterOptions filter = uxReportFiltering.IncomeFilterOption;

        switch (filter.DateFilterMode)
        {
            case (int)AS.Web.UI.Controls.DateOptionMode.DateRange:
                {
                    beginDateText = filter.FromDate.Value.ToGenericDateString();
                    endDateText = filter.ToDate.Value.ToGenericDateString();
                }
                dateText = string.Format("{0} - {1}", beginDateText, endDateText);
                break;
            case (int)AS.Web.UI.Controls.DateOptionMode.TrailingTwelveMonths:
                {
                    beginDateText = filter.FromDate.Value.ToGenericDateString();
                    endDateText = filter.FromDate.Value.AddMonths(-12).ToGenericDateString();
                }
                dateText = string.Format("{0} - {1}", beginDateText, endDateText);
                break;

            case (int)AS.Web.UI.Controls.DateOptionMode.Yearly:
                {
                    beginDateText = new DateTime(DateTime.Now.Year, 1, 1).ToGenericDateString();
                    endDateText = DateTime.Now.ToGenericDateString();
                }
                dateText = string.Format("{0} - {1}", beginDateText, endDateText);
                break;
        }

        return "(" + dateText + ")";
    }

    private string _GridTitle
    {
        get
        {
            return GeneralFuncsLib.GetFullGridTitleName(ReportFilter) + " " + GetIncomeDateFilterText();
        }
    }
    protected override void OnDataBindControls(Enum type, object sender)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();
        IncomeFilterOptions filter = uxReportFiltering.IncomeFilterOption;

        switch ((DataBindAction)type)
        {
            case DataBindAction.BindGridIncomeExpense:
                {
                    parameters.Add(new FilterParameter("@ReportType", FilteringMode, DbType.String));
                    if (filter.NetProfit.ToUpper() == "ALL"
                        || ((filter.NetProfit.ToUpper() == "GREATERTHAN"
                        || filter.NetProfit.ToUpper() == "LESSTHAN")
                        && filter.FromNetProfit == null)
                        || (filter.NetProfit.ToUpper() == "BETWEEN"
                        && filter.FromNetProfit == null
                        && filter.ToNetProfit == null)
                        )
                    {
                        parameters.Add(new FilterParameter("@NetProfitFilterMode", null, DbType.String));
                    }
                    else
                    {
                        parameters.Add(new FilterParameter("@NetProfitFilterMode", filter.NetProfit, DbType.String));
                    }

                    parameters.Add(new FilterParameter("@NetProfitFrom", filter.FromNetProfit, DbType.Currency));
                    parameters.Add(new FilterParameter("@NetProfitTo", filter.ToNetProfit, DbType.Currency));
                    if (FilteringMode == "IncomeExpenseByMerchant")
                        parameters.Add(new FilterParameter("@ProfileID", filter.ProfileID, DbType.Int32));

                    if (SessionManager.CurrentUser.ASClient == 29) // Orion
                    {
                        parameters.AddHierarchyFilterParamsForORION(this);
                    }
                    else
                    {
                        parameters.AddHierarchyFilterParamsWithoutDate(this);
                    }

                    parameters.Add(new AS.Common.DBManager.FilterParameter("@DateFilterMode", (int)DateOptionMode.DateRange, DbType.Int32));
                    if (filter.DateFilterMode == (int)DateOptionMode.DateRange)
                    {
                        parameters.Add(new AS.Common.DBManager.FilterParameter("@BeginDate", filter.FromDate.Value, DbType.DateTime));
                        parameters.Add(new AS.Common.DBManager.FilterParameter("@EndDate", filter.ToDate.Value, DbType.DateTime));

                    }
                    else if (filter.DateFilterMode == (int)DateOptionMode.TrailingTwelveMonths)
                    {
                        parameters.Add(new AS.Common.DBManager.FilterParameter("@BeginDate", DateTime.Now.AddMonths(-12).Date, DbType.DateTime));
                        parameters.Add(new AS.Common.DBManager.FilterParameter("@EndDate", DateTime.Now.Date, DbType.DateTime));
                    }
                    else
                    {
                        parameters.Add(new AS.Common.DBManager.FilterParameter("@BeginDate", new DateTime(DateTime.Now.Year, 1, 1), DbType.DateTime));
                        parameters.Add(new AS.Common.DBManager.FilterParameter("@EndDate", DateTime.Now.Date, DbType.DateTime));
                    }

                    if (FilteringMode == "IncomeExpenseByPeriod")
                    {
                        uxDrilldownGrid.Columns.FindByUniqueName("Month").Visible = true;
                        uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").Visible = false;
                    }
                    else
                    {
                        uxDrilldownGrid.Columns.FindByUniqueName("Month").Visible = false;
                        uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").Visible = true;
                    }
                    string spaName = "spa_GetIncomeExpense";
                    uxDrilldownGrid.DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(parameters) });

                    uxExporterTop.GridHeader = GetLocalResourceObject("IncomeExpense_aspx_cs_IncomeExpenseReport").ToString() + " -" + _GridTitle;
                    uxExporterTop.GridTitle = GetLocalResourceObject("IncomeExpense_aspx_cs_IncomeExpenseReport").ToString() + " - ";
                    uxExporterTop.GridSubTitle = _GridTitle;
                }
                break;
        }
    }
}
