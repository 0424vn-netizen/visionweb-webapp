using AS.Controls.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class freeaccess_Redirect : NonReportPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
        if (User.Identity.IsAuthenticated && SessionManager.IsLoggedIn && IsSecureQueryString)
        {
            string Url = SecureQueryString["Url"];
            if (!string.IsNullOrEmpty(Url))
                HttpContext.Current.Response.Redirect(Url);
        }
        // this page must be have the param URL
        RaiseIntruderEvent(IntruderType.QueryString);
        return;

    }
}