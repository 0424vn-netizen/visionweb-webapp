using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_MTDChart : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void BindChart()
    {
        uxMTDChart.ChartType = UserControls_DashBoardChart.DashBoardChartType.MTD;
        uxMTDChart.BindChart();
    }
}