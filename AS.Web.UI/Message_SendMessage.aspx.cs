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
using AS.Controls.Grid;
using Telerik.Web.UI;
using AS.Common.DBManager;
using AS.Web.Business;
using AS.Controls.Pages;
using AS.Common;

[PagePermission("SendMsg")]
public partial class gen_Message_SendMessage : ReportPage
{
    enum PostBackAction
    {
        SendMessage,
        RefreshEvent
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack) {
            uxGridMessage.Rebind(MessageGrid.ViewMessageType.TodayMessage, DateTime.Now, DateTime.Now);
        }
    }
    
    protected void uxBtnSendMessage_Click(object sender, EventArgs e)
    {
        SendMessages();
    }
    protected override void OnPostBackActions(Enum type, object sender)
    {
        base.OnPostBackActions(type, sender);
        switch ((PostBackAction)type)
        {
            case PostBackAction.SendMessage:
                if (this.IsIntruderDetected)
                {
                    return;
                }
                else
                {
                    SendMessages();
                }
                break;
            case PostBackAction.RefreshEvent:
                uxGridMessage.Rebind(MessageGrid.ViewMessageType.TodayMessage, DateTime.Now, DateTime.Now);
                break;
        }

    }

    protected void SendMessages()
    {
        this.InsertMessage();
        //Clear Message
        uxMessageEditor.Message = string.Empty;
        OnPostBackActions(PostBackAction.RefreshEvent, this);
    }

    protected void InsertMessage()
    {
        FilterParameterCollection inParams = new FilterParameterCollection();
        FilterParameterCollection outParams = new FilterParameterCollection();
        inParams.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        inParams.Add(new FilterParameter("@Message", uxMessageEditor.Message, DbType.AnsiString));
        WebServices.CsReportServices.ExecuteNonQueryCommand("spa_InsertMessage", inParams, out outParams);
    }
}
