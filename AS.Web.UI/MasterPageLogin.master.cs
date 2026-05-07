using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPageLogin : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        radForceWindow.Behaviors = Telerik.Web.UI.WindowBehaviors.None;
        radForceWindow.VisibleOnPageLoad = SessionManager.ForceChangePassword;
        radPwdExpiredAlert.VisibleOnPageLoad = SessionManager.PasswordExpiredNearly;
        CheckLinkForgotPassword();
        ShowHideMaintenanceMode();
    }

    private void CheckLinkForgotPassword()
    {
        if (!string.IsNullOrEmpty(Request.QueryString["u"]) && Request.QueryString["a"] == AS.Common.DataProtection.Cryptophy.EncryptText("f"))
        {
            radForgetWindow.NavigateUrl = "freeaccess/forgotPassword1.aspx";
            radForgetWindow.VisibleOnPageLoad = !IsPostBack;
        }
    }

    /// <summary>
    /// Show/Hide Maintenance Mode
    /// </summary>
    private void ShowHideMaintenanceMode()
    { 
        if (Request.Params["MaintMode"] != null && Request.Params["MaintMode"].ToLower().Equals("on"))
        {
            uxBody.Attributes["class"] = "body-login";
        }
        else if (ConfigurationManager.AppSettings["MaintenanceMode"] != null && ConfigurationManager.AppSettings["MaintenanceMode"].ToLower().Equals("on"))
        {
            uxBody.Attributes["class"] = "body-maintenance";
        }

    }


}
