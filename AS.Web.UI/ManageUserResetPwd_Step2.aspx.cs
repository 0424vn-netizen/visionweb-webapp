
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
using AS.Controls.Pages;
using AS.Common;

[PagePermission("ManUser,MSManUser,RstMerPwd,ASLandingPage,ResetSubHierarchyPassword")]
public partial class _mps_ResetPasswordForCS2 : NonReportPage
{
    protected string timeToExpired = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
        PageType = SecurePageType.Modal;
        if (!IsPostBack)
        {
            if (SessionManager.ResetPasswordUser != null)
            {
                uxPassword.Text = VeraCodeSolution.DoVeraCode(SessionManager.ResetPasswordUser.UserPassword);

            }

            if (IsSecureQueryString)
            {
                // Reload Manage User Page after reseting password
                if (SecureQueryString["reload"] != null)
                {
                    uxHidReload.Value = SecureQueryString["reload"];
                }
            }
        }

        if (!string.IsNullOrEmpty(((Validation)WebSiteSettings.PwdValidationRule.Get("PasswordAdminResetExpiredDays")).Key))
        {
            int configValue = ((Validation)WebSiteSettings.PwdValidationRule.Get("PasswordAdminResetExpiredDays")).Value;

            timeToExpired = (configValue * 24) + " " + GetLocalResourceObject("ManageUserResetPwd_Step2_aspx_cs_Hours").ToString();
        }
        else
        {
            timeToExpired = GetLocalResourceObject("ManageUserResetPwd_Step2_aspx_cs_72Hours").ToString();
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }
}
