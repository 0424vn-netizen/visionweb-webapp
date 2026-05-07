using System;
using AS.Controls.Pages;

public partial class MasterPagePopup : MasterPageNormal
{
    public bool IsModal
    {
        get
        {
            bool isModal = true;
            if (Page is ReportPage)
                isModal = ((ReportPage)Page).IsModal;
            else if (Page is NonReportPage)
                isModal = ((NonReportPage)Page).IsModal;
            return isModal;
        }
    }

    protected string RootUrl = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        RootUrl = ResolveUrl("~/");
        uxWindowManager.Localization.Cancel = Resources.MessageManager.TextCancel;
    }
    public static string IsIEBrowser
    {
        get
        {
            return GeneralFuncsLib.GetIEBrowserMode();
        }
    }
    protected void uxMasterScriptManager_AsyncPostBackError(object sender, System.Web.UI.AsyncPostBackErrorEventArgs e)
    {
        //through exception to Application_Error catch        
        throw e.Exception;
    }
}
