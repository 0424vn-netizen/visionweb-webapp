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

[PagePermission("SendMsg,MSSendMsg,MSViewMsg")]
public partial class Message_HierarchyFilterModal : NonReportPage
{
    #region Enums
    enum DataBindAction { }
    enum PostBackAction
    {
        Close
    }
    #endregion

    public string HierarchyFilterText { get; set; }
    public string ClientIDbtn { get; set; }
    public string HierarchyFilterMode { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;

        HierarchyFilterMode = SecureQueryString["hierarchyMode"];

        HierarchyFilterText = GeneralFuncsLib.GetMessageHierarchyFilterText(HierarchyFilterMode);
        ClientIDbtn = SecureQueryString["clientID"];

        uxMessage_HierarchyFilter.HierarchyFilterText = HierarchyFilterText;
        uxMessage_HierarchyFilter.HierarchyFilterMode = HierarchyFilterMode;
        uxMessage_HierarchyFilter.MessageID = Convert.ToInt32(SecureQueryString["MessageID"]);

        //uxHierarchy.PrimaryID = SecureQueryString["primaryid"];
        //int mode = Int16.Parse(SecureQueryString["mode"]);
        //uxHierarchy.Mode = (WebSiteEnums.ParamFilterMode)mode;
        //uxHierarchy.ParamID = SecureQueryString["paramid"];

        this.Title = string.Format(GetLocalResourceObject("Message_HierarchyFilterModal_aspx_cs_Select").ToString(),HierarchyFilterText);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }
}
