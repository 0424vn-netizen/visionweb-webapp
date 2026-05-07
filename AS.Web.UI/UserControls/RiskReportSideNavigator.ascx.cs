using AS.Controls.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_RiskReportSideNavigator : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //for SNET Readonly
        plhInvestigation.Visible = GeneralFuncsLib.ReadOnlyRiskManagementEnableStatus((SecurePage)Page);
    }
}