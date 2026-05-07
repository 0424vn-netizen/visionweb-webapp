using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AS.Controls.Pages;

[PagePermission("ManRole")]
public partial class _mps_EditMSRole_Modal : NonReportPage
{
    #region Methods

    protected override void PageInitialize()
    {
        base.PageInitialize();
        this.IsSecureCSRF = true;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected)
        {
            return;
        }
        PageType = SecurePageType.Modal;
        SessionManager.UserRoleType = WebSiteEnums.UserType.MS.ToString();
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }
    #endregion Methods
}
