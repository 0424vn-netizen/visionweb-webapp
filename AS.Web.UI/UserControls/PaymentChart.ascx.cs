using AS.Common.DBManager;
using AS.Web.UI.Controls;
using System;
using System.Data;
using System.Globalization;
using System.Threading;

public partial class UserControls_PaymentChart : System.Web.UI.UserControl, AS.Common.WebUI.IChart
{
    #region CONST STRING
    private const string DAY = "Day";
    private const string COLOR = "Color";
    private const string NET_DEPOSIT_AMOUNT = "NetDepositAmount";
    private const string REPORT_DATE = "ReportDate";
    private const string OLD_COLOR = "#AAAAAA";
    private const string NEW_COLOR = "#385487";

    private const int DATE_RANGE = 30;
    #endregion

    #region FIELDS
    protected bool _IsHasNagativeData = false;
    #endregion

    #region EVENTS
    protected object uxDailyDepositsChart_NeedDataSource(aperia.controls.KendoChart sender)
    {
        DateTime fromDate;
        DateTime beginDate = DateTime.Now;
        DateTime endDate = DateTime.Now;
        ReportPage page = (ReportPage)this.Page;

        switch (page.ReportFilter.CurrentValue.DateOption)
        {
            case DateOptionMode.Daily:
                beginDate = endDate = page.ReportFilter.CurrentValue.DateOptionValue.From;
                break;
            case DateOptionMode.Monthly:
                beginDate = page.ReportFilter.CurrentValue.DateOptionValue.From.GetFirstDayOfMonth();
                endDate = page.ReportFilter.CurrentValue.DateOptionValue.From.GetLastDayOfMonth();
                break;
            case DateOptionMode.DateRange:
                beginDate = page.ReportFilter.CurrentValue.DateOptionValue.From;
                endDate = page.ReportFilter.CurrentValue.DateOptionValue.To;
                break;
        }
        fromDate = endDate.AddDays(1 - DATE_RANGE);

        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();
        if (SessionManager.CurrentUser.ASClient == 29) // Orion        
            parameters = FilterParameterExtension.AddHierarchyFilterParamsForORION(parameters, page);
        else
            parameters = FilterParameterExtension.AddHierarchyFilterParamsWithoutDate(parameters, page);

        parameters.Add(new AS.Common.DBManager.FilterParameter("@DateFilterMode", DateOptionMode.DateRange, DbType.Int32));
        parameters.Add(new AS.Common.DBManager.FilterParameter("@BeginDate", fromDate, DbType.DateTime));
        parameters.Add(new AS.Common.DBManager.FilterParameter("@EndDate", endDate, DbType.DateTime));
        parameters.Add(new FilterParameter("@ReportType", "DepositsChart", DbType.AnsiString));
        DataTable data = WebServices.CsReportServices.GetReports(WebSiteConstants.GET_CHART_SPA_NAME, parameters);

        DataTable dataTable = DailyVolumeDataTable();

        DateTime currentDate = DateTime.Now;

        for (int i = 0; i < 30; i++)
        {
            currentDate = fromDate.AddDays(i).Date;

            DataRow newRow = dataTable.NewRow();
            newRow[DAY] = FormatDisplayDate(i, currentDate);
            newRow[NET_DEPOSIT_AMOUNT] = 0;

            if (DateTime.Compare(currentDate, beginDate.Date) < 0)
                newRow[COLOR] = OLD_COLOR;
            else
                newRow[COLOR] = NEW_COLOR;

            foreach (DataRow item in data.Rows)
            {
                if (DateTime.Compare(((DateTime)item[REPORT_DATE]).Date, currentDate) == 0)
                {
                    newRow[NET_DEPOSIT_AMOUNT] = item[NET_DEPOSIT_AMOUNT];
                    break;
                }
            }
            dataTable.Rows.Add(newRow);
        }

        if (data.Rows.Count > 0)
            _IsHasNagativeData = Convert.ToDecimal(data.Compute("min(NetDepositAmount)", string.Empty)) < 0;
        return dataTable;
    }
    #endregion

    #region METHODS
    public void BindChart()
    {
        if (aperia.controls.KendoChart.IsDataRequest) return;
        uxDailyDepositsChart.DataBind();
    }

    private string FormatDisplayDate(int index, DateTime date)
    {
        if ((date.Day == 1) || (index == 0 && date.AddDays(1).Day != 1))
        {
            return date.ToString("MMM dd");
        }
        return date.ToString("dd");
    }

    private DataTable DailyVolumeDataTable()
    {
        DataTable tbl = new DataTable();
        tbl.Columns.Add(DAY, typeof(string));
        tbl.Columns.Add(NET_DEPOSIT_AMOUNT, typeof(decimal));
        tbl.Columns.Add(COLOR, typeof(string));
        return tbl;
    }
    #endregion
}
