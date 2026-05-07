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
public partial class rm_MCF_Filter_State_Modal : NonReportPage
{
    #region Enums
    enum DataBindAction { }
    enum PostBackAction
    {
        Close
    }
    #endregion

    protected string StateLabel = "State";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
        PageType = SecurePageType.Modal;
        uxFilterState.Mode = (WebSiteEnums.ParamFilterMode)(int.Parse(SecureQueryString["mode"]));
        uxFilterState.PrimaryID = SecureQueryString["primaryid"];
        uxFilterState.ParamID = SecureQueryString["paramid"];


        StateLabel = GeneralFuncsLib.GetDataOfExtendedSetting("STATE_LABEL_TEXT_PLURAL") == string.Empty ? GetLocalResourceObject("Filter_State_Modal_aspx_cs_States").ToString() : GeneralFuncsLib.GetDataOfExtendedSetting("STATE_LABEL_TEXT_PLURAL");

        this.Page.Title = GetLocalResourceObject("Filter_State_Modal_aspx_cs_Select").ToString() + " " + (GeneralFuncsLib.GetDataOfExtendedSetting("STATE_LABEL_TEXT") == string.Empty ? GetLocalResourceObject("Filter_State_Modal_aspx_cs_State").ToString() : GeneralFuncsLib.GetDataOfExtendedSetting("STATE_LABEL_TEXT"));

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
                uxFilterState.Save();
                ClientScript.RegisterStartupScript(GetType(), "startupscript", "parent.ClosePopupModal(1);", true);
                break;
        }
    }

    protected void uxClose_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.Close);
    }
}
