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
using AS.Security.WS.Entities;

public partial class freeaccess_testJumpSite : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    protected void ctrlOK_Click(object sender, EventArgs e)
    {
        User user = WebServices.SecurityServices.GetUser(WebSiteSettings.DefaultClient,ctrlUserName.Text);
        if (user != null)
        {

            string key = WebServices.SecurityServices.CreateJumpSiteTicket(user.RecId, Request.UserHostAddress, WebSiteSettings.DefaultSystem, Guid.Empty);
            string url = ResolveUrl("~/freeaccess/gate.aspx");
            url = string.Format(url + "?u={0}&k={1}&j={2}", HttpUtility.UrlEncode(WebServices.SecurityServices.EncryptText(user.UserID)), HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(WebServices.SecurityServices.EncryptText(Guid.Empty.ToString())));
            ClientScript.RegisterStartupScript(GetType(), "startup", string.Format("window.open('{0}');", url), true);
        }
    }
}
