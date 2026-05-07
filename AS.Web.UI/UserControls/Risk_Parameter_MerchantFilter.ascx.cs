using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AS.Common;

public partial class UserControls_Risk_Parameter_MerchantFilter : System.Web.UI.UserControl
{
    public static double? merchFilterFrom
    {
        get;
        set;
    }
    public static double? merchFilterTo
    {
        get;
        set;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        txtFrom.Text = VeraCodeSolution.DoVeraCode(Convert.ToInt32(merchFilterFrom).ToString());
        txtTo.Text = VeraCodeSolution.DoVeraCode(Convert.ToInt32(merchFilterTo).ToString());
    }
    protected void Save()
    {

    }
}
