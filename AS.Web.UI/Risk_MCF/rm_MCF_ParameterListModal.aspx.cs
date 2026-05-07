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

[PagePermission("RskManAss,RskAdhoc,MSRskManAss,MSRskAdhoc")]
public partial class rm_MCF_ParameterListModal : NonReportPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;
        if (IsIntruderDetected) return;
        ucParameterList.Mode = (WebSiteEnums.ParamFilterMode)(int.Parse(SecureQueryString["Mode"]));
        ucParameterList.PrimaryID = SecureQueryString["PrimaryID"];
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        ucParameterList.Save();
        ClientScript.RegisterStartupScript(GetType(), "startupscript", "callFuncFormParent();", true);
    }
}
