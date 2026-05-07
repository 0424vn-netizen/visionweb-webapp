using AS.Common;
using AS.Common.DBManager;
using AS.Web.Business;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Threading;

public partial class UserControls_DashBoardChart : System.Web.UI.UserControl, AS.Common.WebUI.IChart
{
    #region CONST STRING
    private const string XVALUE = "XValue";
    private const string MONTHS = "Months";
    private const string YEAR_MONTH = "YearMonth";
    private const string SALE_AMOUNT = "SaleAmount";
    private const string NET_AMOUNT = "NetAmount";
    private const string NET_AMOUNT_PREVIOUS = "NetAmountPrevious";
    private const string SALE_AMOUNT_PREVIOUS = "SaleAmountPrevious";
    private const string TOOLTIP_CURRENT = "TooltipCurrent";
    private const string TOOLTIP_PREVIOUS = "TooltipPrevious";
    private const string REPORT_DATE = "ReportDate";
    #endregion

    public enum DashBoardChartType
    {
        MTD = 1,
        YTD = 2,
        Last12Months = 3
    }

    #region FIELDS
    protected DataTable _dtYOYSummary;
    protected FilterParameterCollection _parameter;
    protected int _beginCurrentYearMonth, _endCurrentYearMonth;
    protected int _beginPreviousYearMonth, _endPreviousYearMonth;
    //protected string _netVolumeSumCurrent = "$0", _netVolumeSumNinetyDay = "$0";
    protected decimal _grossSalesCurrent = 0, _grossSalesPrevious = 0;
    private string Mode = string.Empty;
    public DashBoardChartType ChartType;
    protected string _textSalesCurrent, _textSalesPrevious;
    #endregion

    #region EVENTS
    protected object uxNetVolumeChart_NeedDataSource(aperia.controls.KendoChart sender)
    {
        DateTime reportDate;
        DataTable dtCurrentYear = GetGrossSaleData(_beginCurrentYearMonth, _endCurrentYearMonth, Mode);
        DataTable dtPreviousYear = GetGrossPreviousSaleData(_beginPreviousYearMonth, _endPreviousYearMonth, Mode);

        dtCurrentYear.Columns.Add(SALE_AMOUNT_PREVIOUS, System.Type.GetType("System.Double"));
        dtCurrentYear.Columns.Add(XVALUE, System.Type.GetType("System.String"));
        dtCurrentYear.Columns.Add(TOOLTIP_PREVIOUS, System.Type.GetType("System.String"));
        dtCurrentYear.Columns.Add(TOOLTIP_CURRENT, System.Type.GetType("System.String"));

        if (dtCurrentYear.Rows.Count > dtPreviousYear.Rows.Count)
        {
            int addingDays = dtCurrentYear.Rows.Count - dtPreviousYear.Rows.Count;
            int currentDaysOfPreviousYear = dtPreviousYear.Rows.Count;

            for (int j = currentDaysOfPreviousYear; j < dtCurrentYear.Rows.Count; j++)
            {
                dtPreviousYear.Rows.Add(dtPreviousYear.NewRow());
                dtPreviousYear.Rows[j][REPORT_DATE] = long.Parse(dtPreviousYear.Rows[j - 1][REPORT_DATE].ToString()) + 1;
            }
        }
        else if (dtCurrentYear.Rows.Count < dtPreviousYear.Rows.Count)
        {
            int addingDays = dtPreviousYear.Rows.Count - dtCurrentYear.Rows.Count;
            int daysOfCurrentYear = dtCurrentYear.Rows.Count;

            for (int j = daysOfCurrentYear; j < dtPreviousYear.Rows.Count; j++)
            {
                dtCurrentYear.Rows.Add(dtCurrentYear.NewRow());
                dtCurrentYear.Rows[j][REPORT_DATE] = long.Parse(dtCurrentYear.Rows[j - 1][REPORT_DATE].ToString()) + 1;
            }
        }

        for (int i = 0; i < dtCurrentYear.Rows.Count; i++)
        {
            if (dtCurrentYear.Rows[i][SALE_AMOUNT] == DBNull.Value)
            {
                dtCurrentYear.Rows[i][SALE_AMOUNT] = "0";
            }
            else
            {
                _grossSalesCurrent += Decimal.Parse(dtCurrentYear.Rows[i][SALE_AMOUNT].ToString());
            }

            if (dtPreviousYear.Rows[i][SALE_AMOUNT] == DBNull.Value)
            {
                dtCurrentYear.Rows[i][SALE_AMOUNT_PREVIOUS] = "0";
            }
            else
            {
                dtCurrentYear.Rows[i][SALE_AMOUNT_PREVIOUS] = dtPreviousYear.Rows[i][SALE_AMOUNT];
                _grossSalesPrevious += Decimal.Parse(dtPreviousYear.Rows[i][SALE_AMOUNT].ToString());
            }
                        
            switch (ChartType)
            {
                    
                case DashBoardChartType.MTD:
                    dtCurrentYear.Rows[i][XVALUE] = (i == 0 ? string.Format("{0} {1}", DateTime.Now.ToString("MMM", CultureInfo.CurrentUICulture), dtCurrentYear.Rows[i][REPORT_DATE].ToString().Substring(6, 2)).ToUpper() : dtCurrentYear.Rows[i][REPORT_DATE].ToString().Substring(6, 2).ToUpper());
                    dtCurrentYear.Rows[i][TOOLTIP_CURRENT] = string.Format("{0} {1}<br/>{2}", DateTime.Now.ToString("MMM", CultureInfo.CurrentUICulture), dtCurrentYear.Rows[i][REPORT_DATE].ToString().Substring(0, 4), AS.Common.Formater.FormatData.FormatCurrency(dtCurrentYear.Rows[i][SALE_AMOUNT], SessionManager.CurrencyFortmat));
                    dtCurrentYear.Rows[i][TOOLTIP_PREVIOUS] = string.Format("{0} {1}<br/>{2}", DateTime.Now.ToString("MMM", CultureInfo.CurrentUICulture), dtPreviousYear.Rows[i][REPORT_DATE].ToString().Substring(0, 4), AS.Common.Formater.FormatData.FormatCurrency(dtPreviousYear.Rows[i][SALE_AMOUNT], SessionManager.CurrencyFortmat));
                    break;
                case DashBoardChartType.YTD:
                    
                    DateTime.TryParseExact(dtCurrentYear.Rows[i]["ReportDate"].ToString(), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out reportDate);
                    dtCurrentYear.Rows[i][XVALUE] = (i == 0 ? string.Format("{0} {1}", reportDate.ToString("MMM", CultureInfo.CurrentUICulture), dtCurrentYear.Rows[i][REPORT_DATE].ToString().Substring(2, 2)).ToUpper() : reportDate.ToString("MMM", CultureInfo.CurrentUICulture).ToUpper());
                    dtCurrentYear.Rows[i][TOOLTIP_CURRENT] = string.Format("{0} {1}<br/>{2}", reportDate.ToString("MMM", CultureInfo.CurrentUICulture), dtCurrentYear.Rows[i][REPORT_DATE].ToString().Substring(0, 4), AS.Common.Formater.FormatData.FormatCurrency(dtCurrentYear.Rows[i][SALE_AMOUNT], SessionManager.CurrencyFortmat));
                    dtCurrentYear.Rows[i][TOOLTIP_PREVIOUS] = string.Format("{0} {1}<br/>{2}", reportDate.ToString("MMM", CultureInfo.CurrentUICulture), dtPreviousYear.Rows[i][REPORT_DATE].ToString().Substring(0, 4), AS.Common.Formater.FormatData.FormatCurrency(dtPreviousYear.Rows[i][SALE_AMOUNT], SessionManager.CurrencyFortmat));
                    break;
                case DashBoardChartType.Last12Months:
                    DateTime.TryParseExact(dtCurrentYear.Rows[i]["ReportDate"].ToString(), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out reportDate);
                    string month = reportDate.ToString("MMM", CultureInfo.CurrentUICulture).ToUpper();
                    dtCurrentYear.Rows[i][XVALUE] = dtCurrentYear.Rows[i][REPORT_DATE].ToString().Substring(4, 2) == "01" ? string.Format("{0} {1}", month, dtCurrentYear.Rows[i][REPORT_DATE].ToString().Substring(2, 2)).ToUpper() : string.Format("{0}", month);
                    dtCurrentYear.Rows[i][TOOLTIP_CURRENT] = string.Format("{0} {1}<br/>{2}", month, dtCurrentYear.Rows[i][REPORT_DATE].ToString().Substring(0, 4), AS.Common.Formater.FormatData.FormatCurrency(dtCurrentYear.Rows[i][SALE_AMOUNT], SessionManager.CurrencyFortmat));
                    dtCurrentYear.Rows[i][TOOLTIP_PREVIOUS] = string.Format("{0} {1}<br/>{2}", month, dtPreviousYear.Rows[i][REPORT_DATE].ToString().Substring(0, 4), AS.Common.Formater.FormatData.FormatCurrency(dtPreviousYear.Rows[i][SALE_AMOUNT], SessionManager.CurrencyFortmat));
                    break;
                default:
                    break;
            }
        }

        dtCurrentYear = dtCurrentYear.DefaultView.ToTable();
        return dtCurrentYear;

    }
    #endregion

    #region METHODS
    public void BindChart()
    {
        if (aperia.controls.KendoChart.IsDataRequest) return;
        DateTime now = DateTime.Now;
        switch (ChartType)
        {
            case DashBoardChartType.MTD:
                _beginCurrentYearMonth = Convert.ToInt32(now.ToString("yyyyMM"));
                _endCurrentYearMonth = _beginCurrentYearMonth;
                Mode = "MTD";
                _textSalesCurrent = GetLocalResourceObject("DashBoardChart_ascx_cs_CurrentMTD").ToString();
                _textSalesPrevious = string.Format(GetLocalResourceObject("DashBoardChart_ascx_cs_PreviousMTD").ToString(), DateTime.Now.Year - 1);
                break;
            case DashBoardChartType.YTD:
                _beginCurrentYearMonth = Convert.ToInt32(string.Format("{0}01", now.Year.ToString()));
                _endCurrentYearMonth = Convert.ToInt32(now.ToString("yyyyMM"));
                Mode = "YTD";
                _textSalesCurrent = GetLocalResourceObject("DashBoardChart_ascx_cs_CurrentYTD").ToString();
                _textSalesPrevious = GetLocalResourceObject("DashBoardChart_ascx_cs_PreviousYTD").ToString();
                break;
            case DashBoardChartType.Last12Months:
                _endCurrentYearMonth = Convert.ToInt32(now.AddMonths(-1).ToString("yyyyMM"));
                _beginCurrentYearMonth = Convert.ToInt32(now.AddMonths(-12).ToString("yyyyMM"));
                Mode = "Last12Months";
                _textSalesCurrent = GetLocalResourceObject("DashBoardChart_ascx_cs_Current12Months").ToString();
                _textSalesPrevious = GetLocalResourceObject("DashBoardChart_ascx_cs_Previous12Months").ToString();
                break;
            default:
                break;
        }
        _beginPreviousYearMonth = _beginCurrentYearMonth - 100;
        _endPreviousYearMonth = _endCurrentYearMonth - 100;
        uxNetVolumeChart.DataBind();
    }

    private DataTable GetGrossSaleData(int beginYearMonth, int endYearMonth, string mode)
    {
        FilterParameterCollection parameter = new FilterParameterCollection();
        parameter.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameter.Add(new FilterParameter("@BeginYearMonth", beginYearMonth, System.Data.DbType.Int32));
        parameter.Add(new FilterParameter("@EndYearMonth", endYearMonth, System.Data.DbType.Int32));
        parameter.Add(new FilterParameter("@Mode", mode, System.Data.DbType.AnsiString));
        return GetDataSource("spa_Reskin_GetDashBoardYOY_VolumeChart", parameter);
    }

    private DataTable GetGrossPreviousSaleData(int beginYearMonth, int endYearMonth, string mode)
    {
        FilterParameterCollection parameter = new FilterParameterCollection();
        parameter.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameter.Add(new FilterParameter("@BeginYearMonth", beginYearMonth, System.Data.DbType.Int32));
        parameter.Add(new FilterParameter("@EndYearMonth", endYearMonth, System.Data.DbType.Int32));
        parameter.Add(new FilterParameter("@Mode", mode, System.Data.DbType.AnsiString));
        parameter.Add(new FilterParameter("@ProcessDate", DateTime.Now.AddYears(-1).Date.ToString("yyyy-MM-dd"), System.Data.DbType.DateTime));
        return GetDataSource("spa_Reskin_GetDashBoardYOY_VolumeChart", parameter);
    }

    private DataTable GetDataSource(string spaName, FilterParameterCollection paras)
    {
        return WebServices.CsReportServices.GetReports(spaName, paras);
    }
    #endregion
}