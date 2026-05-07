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
using AS.Security.WS.Entities;

public partial class page_ExpiredSession : NonReportPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string themeName = string.Empty;
        if (SessionManager.CurrentUserTheme != null)
        {
            themeName = SessionManager.CurrentUserTheme.ThemeName;
        }
        int clientId = SessionManager.CurrentClient;
        GeneralFuncsLib.UpdateDataWhenLogout();
        Session.Clear();
        SessionManager.CurrentClient = clientId;
        FormsAuthentication.SignOut();
        if (!string.IsNullOrEmpty(themeName))
        {
            SessionManager.CurrentUserTheme = new ASTheme(1, themeName, "", "", DateTime.Now, "1");
        }
    }
}
