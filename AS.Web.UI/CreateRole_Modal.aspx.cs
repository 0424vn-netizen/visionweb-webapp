using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using AS.Controls.Pages;

[PagePermission("ManRole")]
public partial class _mps_CreateRole_Modal : NonReportPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected)
            return;
        PageType = SecurePageType.Modal;
        Page.Title = GeneralFuncsLib.HasMSUserManagementFeature
            ? GetLocalResourceObject("CreateRole_Modal_aspx_cs_CreateCSRole").ToString() : GetLocalResourceObject("CreateRole_Modal_aspx_cs_CreateRole").ToString();
        SessionManager.UserRoleType = WebSiteEnums.UserType.CS.ToString();
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }

    protected override void PageInitialize()
    {
        base.PageInitialize();
        this.IsSecureCSRF = true;
    }
}
