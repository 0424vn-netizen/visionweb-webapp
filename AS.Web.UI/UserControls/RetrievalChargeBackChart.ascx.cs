using AS.Common.DBManager;
using AS.Common.WebUI;
using System;
using System.Collections.Generic;
using System.Data;

public partial class UserControls_RetrievalChargeBackChart : GlobalUserControl, IChart
{

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    public void BindChart()
    {
        if (aperia.controls.KendoChart.IsDataRequest) return;
        uxRTCBChart.DataBind();
    }

    protected object uxRTCBChart_NeedDataSource(aperia.controls.KendoChart sender)
    {
        
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();
        parameters.AddHierarchyFilterParams((ReportPage)this.Page);
        parameters.Add("@ReportType", "RetrievalsChargebacksChart", DbType.String);
        DataTable dt = WebServices.CsReportServices.GetReports("spa_ms_Config_GetReport_Chart", parameters);
        double Amount;
        List<Item> lst = new List<Item>();
        for (int i = 0; i < dt.Columns.Count; i++)
        {
            Amount = Convert.ToDouble(dt.Rows.Count == 0 ? 0 : (dt.Rows[0][i] == DBNull.Value ? 0 : dt.Rows[0][i]));
            if (Amount < 0)
                Amount = 0 - Amount;
            switch (dt.Columns[i].ColumnName)
            {
                case "ChargebackAmount":
                    lst.Add(new Item { label = GetLocalResourceObject("RetrievalChargeBackChartASCX_Text_ChargeBack").ToString(), value = Amount, color = "#d31f38" });
                    break;
                case "RetrievalAmount":
                    lst.Add(new Item { label = GetLocalResourceObject("RetrievalChargeBackChartASCX_Text_Retrievals").ToString(), value = Amount, color = "#032E12" });
                    break;
                case "SaleAmount":
                    lst.Add(new Item { label = GetLocalResourceObject("RetrievalChargeBackChartASCX_Text_Sales").ToString(), value = Amount, color = "#3692E3" });
                    break;
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

