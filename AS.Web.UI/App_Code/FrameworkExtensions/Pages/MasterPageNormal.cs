using AS.Controls.Pages;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for MasterPageNormal
/// </summary>
public class MasterPageNormal : BaseMasterPage
{
    public bool HideHeaderMenu { get; set; }
    public bool ShowLeftNaviControl { get; set; }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        HtmlGenericControl mainContent = this.FindControl("mainContent") as HtmlGenericControl;
        string containerCss = "container";

        if (this.Page is ReportPage) {
            if (((ReportPage)Page).IsModal)
            {
                containerCss = string.Empty;
            }
        }

        if (this.Page is NonReportPage)
        {
            if (((NonReportPage)Page).IsModal)
            {
                containerCss = string.Empty;
            }
        }

        if (ShowLeftNaviControl)
        {
            containerCss += " merchant-info";
        }
        else
        {
            containerCss += " hide-nav-left";
        }

        mainContent.Attributes.Add("class", containerCss);

        //Render setting
        if (SessionManager.IsLoggedIn)
            GeneralFuncsLib.RenderScriptSetting(this.Page);
    }
}