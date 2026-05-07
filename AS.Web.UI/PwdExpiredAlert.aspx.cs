using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using AS.Common;
using AS.Web.SharedSession;

public partial class _mps_PwdExpiredAlert : NonReportPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.PageType = SecurePageType.Modal;
        uxDaysRemained.Text = SessionManager.DayRemainingPasswordExpired.ToString();

    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }
    protected void uxResetPwd_Click(object sender, EventArgs e)
    {
        SessionManager.ForceChangePassword = true;
        SharedSessionManager.PasswordExpired = SessionManager.ForceChangePassword;
        SessionManager.PasswordExpiredNearly = false;
        SessionManager.PasswordExpired = true;
        ClientScript.RegisterStartupScript(GetType(), "startup", VeraCodeSolution.DoVeraCode("parent.location.href='" + LoginUrl + "';"), true);
    }

    protected void uxContinue_Click(object sender, EventArgs e)
    {
        SessionManager.PasswordExpiredNearly = false;
        string defaultPage = "";
        if (IsUserWithPermission("Reports"))
        {
            defaultPage = ResolveUrl("~/gen_Home.aspx");
        }
        else
        {
            defaultPage = ResolveUrl("~/gen_Default.aspx");
        }
        ClientScript.RegisterStartupScript(GetType(), "startup", VeraCodeSolution.DoVeraCode("parent.location.href='" + defaultPage + "';"), true);
    }
}
