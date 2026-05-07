using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_Last12MonthsChart : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void BindChart()
    {
        uxLast12MonthsChart.ChartType = UserControls_DashBoardChart.DashBoardChartType.Last12Months;
        uxLast12MonthsChart.BindChart();
    }
}