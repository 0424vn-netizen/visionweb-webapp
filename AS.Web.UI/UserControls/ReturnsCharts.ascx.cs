using AS.Common.DBManager;
using System;
using System.Collections.Generic;
using System.Data;

public partial class UserControls_ReturnsCharts : System.Web.UI.UserControl, AS.Common.WebUI.IChart
{
    public void BindChart()
    {
        if (aperia.controls.KendoChart.IsDataRequest) return;
        uxSalesReturnsChart.DataBind();
        uxMatchedReturnsChart.DataBind();
    }
    protected object uxSalesReturnsChart_NeedDataSource(aperia.controls.KendoChart sender)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();
        parameters.AddHierarchyFilterParams((ReportPage)this.Page);
        parameters.Add(new FilterParameter("@ReportType", "ReturnSalesReturnsChart", DbType.AnsiString));
        DataTable dtSalesReturnsData = WebServices.MsReportServices.GetReports(WebSiteConstants.GET_CHART_SPA_NAME, parameters);

        List<Item> lst = new List<Item>();

        if (dtSalesReturnsData.Rows.Count > 0)
        {
            for (int i = 0; i < dtSalesReturnsData.Columns.Count; i++)
            {
                switch (dtSalesReturnsData.Columns[i].ColumnName)
                {
                    case "SalePercent":
                        lst.Add(new Item { label = GetLocalResourceObject("ReturnsCharts_ascx_cs_Sales").ToString(), value = Convert.ToDouble(dtSalesReturnsData.Rows[0][i]), color = "#3692E3" });
                        break;
                    case "ReturnPercent":
                        lst.Add(new Item { label = GetLocalResourceObject("ReturnsCharts_ascx_cs_Returns").ToString(), value = Convert.ToDouble(dtSalesReturnsData.Rows[0][i]), color = "#d31f38" });
                        break;
                }
            }
        }
        return lst;
    }
    protected object uxMatchedReturnsChart_NeedDataSource(aperia.controls.KendoChart sender)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();
        parameters.AddHierarchyFilterParams((ReportPage)this.Page);
        parameters.Add(new FilterParameter("@ReportType", "ReturnMatchedReturnsChart", DbType.AnsiString));

        DataTable dtMatchedReturnsData = WebServices.MsReportServices.GetReports(WebSiteConstants.GET_CHART_SPA_NAME, parameters);
        List<Item> lst = new List<Item>();

        if (dtMatchedReturnsData.Rows.Count > 0)
        {
            for (int i = 0; i < dtMatchedReturnsData.Columns.Count; i++)
            {
                switch (dtMatchedReturnsData.Columns[i].ColumnName)
                {
                    case "PartialPercent":
                        lst.Add(new Item { label = GetLocalResourceObject("ReturnsCharts_ascx_cs_PartialMatch").ToString(), value = Convert.ToDouble(dtMatchedReturnsData.Rows[0][i]), color = "#FFFF00" });
                        break;
                    case "FullPercent":
                        lst.Add(new Item { label = GetLocalResourceObject("ReturnsCharts_ascx_cs_FullMatch").ToString(), value = Convert.ToDouble(dtMatchedReturnsData.Rows[0][i]), color = "#00FF00" });
                        break;
                    case "NoMatch":
                        lst.Add(new Item { label = GetLocalResourceObject("ReturnsCharts_ascx_cs_NoMatch").ToString(), value = Convert.ToDouble(dtMatchedReturnsData.Rows[0][i]), color = "#FF0000" });
                        break;
                }
            }
        }
        return lst;
    }

    [Serializable]
    public class Item
    {
        public string label { set; get; }
        public double value { set; get; }
        public string color { set; get; }
    }
}
