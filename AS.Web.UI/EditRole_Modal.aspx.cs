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
public partial class _mps_EditRole_Modal : NonReportPage
{
    protected override void PageInitialize()
    {
        base.PageInitialize();
        this.IsSecureCSRF = true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected)
            return;
        PageType = SecurePageType.Modal;
        Page.Title = GeneralFuncsLib.HasMSUserManagementFeature
            ? GetLocalResourceObject("EditRole_aspx_cs_CSRole").ToString() : GetLocalResourceObject("PageResource1.Title").ToString();
        SessionManager.UserRoleType = WebSiteEnums.UserType.CS.ToString();
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }
}
