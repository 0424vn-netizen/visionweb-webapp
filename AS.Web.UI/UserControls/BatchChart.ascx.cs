using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Dundas.Charting.WebControl;
using AS.Common.WebUI;
using AS.Common.DBManager;
using AS.Threading;
using AS.Common.Logger;
using AS.Web.Business;
using System.Collections.Generic;
using AS.Web.UI.Controls;
using System.Threading;
using System.Globalization;

public partial class UserControls_BatchChart : GlobalUserControl, IChart
{
    #region CONST
    private const string DAY = "Day";
    private const string COLOR = "Color";
    private const string NET_VOLUME = "NetVolume";
    private const string REPORT_DATE = "ReportDate";
    private const string OLD_COLOR = "#AAAAAA";
    private const string NEW_COLOR = "#385487";

    private const int DATE_RANGE = 30;
    #endregion

    #region FIELDS
    protected bool _IsHasNagativeData = false;

    public ReportPage Reportpage
    {
        get;
        set;
    }

    public DateTime BeginDate
    {
        get;
        set;
    }

    public DateTime EndDate
    {
        get;
        set;
    }
    #endregion

    #region EVENTS
    protected object uxVolumeCardTypeChart_NeedDataSource(aperia.controls.KendoChart sender)
    {
        ReportServices services = WebServices.CsReportServices;
        FilterParameterCollection paras = new FilterParameterCollection();
        paras.AddLoggedInUserReportingParams();
        paras.AddHierarchyFilterParams(this.Reportpage);
        paras.Add("@ReportType", "VolumeByCardTypeChart", DbType.String);
        DataTable dtChart = services.GetReports("spa_ms_Config_GetReport_Chart", paras);
        List<Item> lst = new List<Item>();
        string[] colors = new string[7] { "#009ada", "#063579", "#ff6a00", "#00b60d", "#9900cc", "#9e9e9e", "#FBBC00" };
        for (int i = 0; i < dtChart.Columns.Count; i++)
        {
            double tempValue = Convert.ToDouble(dtChart.Rows[0][i]);
            double tempDisplayValue = tempValue < 0 ? tempValue * -1 : tempValue;
            switch (dtChart.Columns[i].ColumnName)
            {
                case "VIPercent":
                    lst.Add(new Item { label = "Visa", value = tempValue, color = colors[i], displayValue = tempDisplayValue });
                    break;
                case "AEPercent":
                    lst.Add(new Item { label = "AMEX", value = tempValue, color = colors[i], displayValue = tempDisplayValue });
                    break;
                case "DIPercent":
                    lst.Add(new Item { label = "Discover", value = tempValue, color = colors[i], displayValue = tempDisplayValue });
                    break;
                case "MCPercent":
                    lst.Add(new Item { label = "Mastercard", value = tempValue, color = colors[i], displayValue = tempDisplayValue });
                    break;
                case "DBPercent":
                    lst.Add(new Item
                    {
                        label = GetLocalResourceObject("BatchChart_Text_Debit").ToString(),
                        value = tempValue,
                        color = colors[i],
                        displayValue = tempDisplayValue
                    });
                    break;
                case "OtherCardsPercent":
                    lst.Add(new Item { label = GetLocalResourceObject("BatchChart_Text_Other").ToString(), value = tempValue, color = colors[i], displayValue = tempDisplayValue });
                    break;
                case "ACHCardsPercent":
                    lst.Add(new Item { label = "ACH", value = tempValue, color = colors[i], displayValue = tempDisplayValue });
                    break;
            }
        }
        lst.Sort((x, y) => y.value.CompareTo(x.value));
        return lst;
    }

    protected object uxDailyVolumeChart_NeedDataSource(aperia.controls.KendoChart sender)
    {
        DateTime fromDate;
        DateTime beginDate = DateTime.Now;
        DateTime endDate = DateTime.Now;
        ReportPage page = this.Reportpage;

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

        ReportServices services = WebServices.CsReportServices;
        FilterParameterCollection paras = new FilterParameterCollection();
        if (SessionManager.CurrentUser.ASClient == 29) // Orion        
            paras = FilterParameterExtension.AddHierarchyFilterParamsForORION(paras, page);
        else
            paras = FilterParameterExtension.AddHierarchyFilterParamsWithoutDate(paras, page);

        paras.Add(new AS.Common.DBManager.FilterParameter("@DateFilterMode", DateOptionMode.DateRange, DbType.Int32));
        paras.Add(new AS.Common.DBManager.FilterParameter("@BeginDate", fromDate, DbType.DateTime));
        paras.Add(new AS.Common.DBManager.FilterParameter("@EndDate", endDate, DbType.DateTime));

        paras.AddLoggedInUserReportingParams();
        paras.Add("@ReportType", "BatchDailyVolumneChart", DbType.String);
        DataTable dt = services.GetReports("spa_ms_Config_GetReport_Chart", paras);

        DataTable dataTable = DailyVolumeDataTable();

        DateTime currentDate = DateTime.Now;
        for (int i = 0; i < DATE_RANGE; i++)
        {
            currentDate = fromDate.AddDays(i).Date;

            DataRow newRow = dataTable.NewRow();
            newRow[DAY] = FormatDisplayDate(i, currentDate);
            newRow[NET_VOLUME] = 0;

            if (DateTime.Compare(currentDate, beginDate.Date) < 0)
                newRow[COLOR] = OLD_COLOR;
            else
                newRow[COLOR] = NEW_COLOR;

            foreach (DataRow item in dt.Rows)
            {
                if (DateTime.Compare(((DateTime)item[REPORT_DATE]).Date, currentDate) == 0)
                {
                    newRow[NET_VOLUME] = item[NET_VOLUME];
                    break;
                }
            }
            dataTable.Rows.Add(newRow);
        }

        if (dt.Rows.Count > 0)
            _IsHasNagativeData = Convert.ToDecimal(dt.Compute("min(NetVolume)", string.Empty)) < 0;

        return dataTable;
    }

    protected object uxKeyedSwipedChart_NeedDataSource(aperia.controls.KendoChart sender)
    {
        ReportServices services = WebServices.CsReportServices;
        FilterParameterCollection paras = new FilterParameterCollection();
        paras.AddLoggedInUserReportingParams();
        paras.AddHierarchyFilterParams(this.Reportpage);
        paras.Add("@ReportType", "KeyedVsSwipedChart", DbType.String);
        DataTable dt = services.GetReports("spa_ms_Config_GetReport_Chart", paras);
        List<Item> lst = new List<Item>();
        for (int i = 0; i < dt.Columns.Count; i++)
        {
            switch (dt.Columns[i].ColumnName)
            {
                case "KeyedPercent":
                    lst.Add(new Item { label = GetLocalResourceObject("BatchChart_Text_Keyed").ToString(), value = Convert.ToDouble(dt.Rows[0][i]), color = "#d31f38" });
                    break;
                case "SwipedPercent":
                    lst.Add(new Item { label = GetLocalResourceObject("BatchChart_Text_Swiped").ToString(), value = Convert.ToDouble(dt.Rows[0][i]), color = "#3692E3" });
                    break;

            }
        }
        return lst;
    }

    #endregion

    #region METHODS
    protected override void OnInit(EventArgs e)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo(WebSiteConstants.USCulture, false);
    }

    public void BindChart()
    {
        if (aperia.controls.KendoChart.IsDataRequest) return;
        uxVolumeCardTypeChart.DataBind();
        uxDailyVolumeChart.DataBind();

        //45658 - Add Keyed vs. Swiped graph to the CS and MS for all hierarchy levels-FE
        pnlKeyedSwipedChart.Visible = true;
        uxKeyedSwipedChart.Visible = true;
        uxKeyedSwipedChart.DataBind();
    }

    private string FormatDisplayDate(int index, DateTime date)
    {
        if ((date.Day == 1) || (index == 0 && date.AddDays(1).Day != 1))
        {
            string result = date.ToString("MMM dd");
            if (SessionManager.CurrentLanguage == (int)WebSiteEnums.LanguageCode.Spanish)
            {
                //format spanish
                Thread.CurrentThread.CurrentCulture = new CultureInfo("es-ES");
                result = date.ToString("MMM dd");

                //reset format
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            }
            return result;
        }
        return date.ToString("dd");
    }

    private DataTable DailyVolumeDataTable()
    {
        DataTable tbl = new DataTable();
        tbl.Columns.Add(DAY, typeof(string));
        tbl.Columns.Add(NET_VOLUME, typeof(decimal));
        tbl.Columns.Add(COLOR, typeof(string));
        return tbl;
    }
    #endregion

    #region CLASS
    [Serializable]
    public class Item
    {
        public string label { set; get; }
        public double value { set; get; }
        public double displayValue { set; get; }
        public string color { set; get; }
    }
    #endregion
}
