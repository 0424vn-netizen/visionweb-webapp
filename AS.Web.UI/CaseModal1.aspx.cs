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

[PagePermission("ManCase,MSManCase")]
public partial class CaseModal1 : NonReportPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
        PageType = SecurePageType.Modal;

        if (SecureQueryString["type"] == "create")
            uxMessage.Text = GetLocalResourceObject("CaseModal1_aspx_cs_MsgAddSuccessfully").ToString();
        else if (SecureQueryString["type"] == "close")
            uxMessage.Text = GetLocalResourceObject("CaseModal1_aspx_cs_MsgClosed").ToString();
        else
            uxMessage.Text = GetLocalResourceObject("CaseModal1_aspx_cs_MsgUpdateSuccessfully").ToString();

        if (SecureQueryString["mode"] == "redirect")
            uxClose.OnClientClick = "doClose(\"CaseManagement.aspx?" + BuildSecureQueryString("isMe=yes&TicketNumber=" + SecureQueryString["TicketNumber"] + "&MerchantNumber=" + SecureQueryString["MerchantNumber"] + "&SiteID=" + SecureQueryString["SiteID"]) + "\")";
        else
            uxClose.OnClientClick = "parent.HidePopupModal();";
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }
}
