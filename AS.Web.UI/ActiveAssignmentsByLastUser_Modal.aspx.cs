using System;
using System.Web.UI;
using AS.Controls.Pages;
using AS.Common;

//[PagePermission("ManageUsers")]
public partial class _mps_ActiveAssignmentsByLastUser_Modal : NonReportPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;
        string messageKey = SecureQueryString["mode"] != "active" ? "Message_Selected" : "Message_Inactive";
        uxMessage.Text = VeraCodeSolution.DoVeraCode(string.Format(GetLocalResourceObject(messageKey).ToString(), SecureQueryString["UserIDFilter"]));
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }

}
