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
public partial class rm_MCF_Filter_MarketData_Modal : NonReportPage
{
    #region Enums
    enum DataBindAction { }
    enum PostBackAction
    {
        Close
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;

        if (SecureQueryString != null && SecureQueryString["mode"] != null)
        {
            int mode = Int16.Parse(SecureQueryString["mode"]);
            uxMarketDataFilter.Mode = (WebSiteEnums.ParamFilterMode)mode;
        }
        if (SecureQueryString != null && SecureQueryString["primaryid"] != null)
            uxMarketDataFilter.PrimaryID = SecureQueryString["primaryid"];
        if (SecureQueryString != null && SecureQueryString["codeset"] != null)
            uxMarketDataFilter.MarketDataType = SecureQueryString["codeset"];
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.Close:
                int error = uxMarketDataFilter.Save();
                if (error != 0)
                    return;
                ClientScript.RegisterStartupScript(GetType(), "startupscript", "parent.ClosePopupModal(1);", true);
                break;
        }
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.Close);
    }
}
