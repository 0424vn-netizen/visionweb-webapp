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

[PagePermission("SendMsg")]
public partial class gen_Message_PastMessage : ReportPage
{
    enum PostBackAction
    {
        RefreshEvent
    }
    

    protected void Page_Load(object sender, EventArgs e)
    {
       this.PageType = SecurePageType.Modal;
       uxStarting.MaxDate = DateTime.Today;
       uxEnding.MaxDate = DateTime.Today;
       
       if (!Page.IsPostBack)
       {
           //Set default date range
           DateTime fisrtDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
           uxStarting.SelectedDate = fisrtDate;
           uxEnding.SelectedDate = DateTime.Now;
           uxMessageGrid.Rebind(MessageGrid.ViewMessageType.PastMessage, uxStarting.SelectedDate.Value, uxEnding.SelectedDate.Value);
       }
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }
    protected override void OnPostBackActions(Enum type, object sender)
    {
        base.OnPostBackActions(type, sender);
        switch ((PostBackAction)type)
        {
            case PostBackAction.RefreshEvent:
                if (this.IsIntruderDetected)
                {
                    return;
                }
                else
                    uxMessageGrid.Rebind(MessageGrid.ViewMessageType.PastMessage, uxStarting.SelectedDate.Value, uxEnding.SelectedDate.Value);
                break;
        }
    }
    protected void uxBtnSearch_Click(object sender, EventArgs e)
    {
        //Validate when search 
        OnPostBackActions(PostBackAction.RefreshEvent, sender);
        //uxMessageGrid.Rebind(MessageGrid.ViewMessageType.PastMessage, uxStarting.SelectedDate.Value, uxEnding.SelectedDate.Value);
        
    }

}
