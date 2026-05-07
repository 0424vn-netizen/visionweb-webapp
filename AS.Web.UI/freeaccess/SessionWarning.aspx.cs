using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using AS.Web.SharedSession;

public partial class page_SessionWarning : NonReportPage
{
    protected void uxButton_Click(object sender, EventArgs e)
    {
        if (SessionManager.IsLoggedIn)
        {
            this.ASPXTrackingLog.LogId1 = SessionManager.CurrentUser.UserID;
            GeneralFuncsLib.UpdateUserLogs("Logout");
            GeneralFuncsLib.UpdateDataWhenLogout();
            int clientId = SessionManager.CurrentClient;
            Session.Clear();
            SessionManager.CurrentClient = clientId;
            Session.Abandon();
            FormsAuthentication.SignOut();

            //Re-new SessionId support for boarding/ case management
            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
        }
        this.ASPXTrackingLog.LogData1 = "gen_log_SessionExpire.aspx";
        this.ASPXTrackingLog.LogData3 = "";
        if (LoginUrl == "[jumpsite]")
        {
            Response.Cookies.Add(new HttpCookie("logout_opt", "2"));//jumpsite time out
            LoginUrl = " ";
            //44594 - VW - Session time out issue - Short term
            SharedSessionManager.USER_LOGIN_URL = LoginUrl;

            ClientScript.RegisterStartupScript(GetType(), "return_login", "doReturnLoginPage('" + FormsAuthentication.LoginUrl + "');", true);
        }
        else
        {
            Response.Cookies.Add(new HttpCookie("logout_opt", "1"));//login time out
            ClientScript.RegisterStartupScript(GetType(), "return_login", "doReturnLoginPage('" + LoginUrl + "');", true);    
        }
        
    }
}
