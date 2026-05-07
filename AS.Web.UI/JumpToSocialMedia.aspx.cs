
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
using AS.Security.Web.SecurityServices;
using AS.Common.DBManager;
using AS.Common;
using AS.Common.DataProtection;
using System.Net;
using System.IO;
using System.Text;
using AS.Web.UI.Controls;

//[PagePermission("")]
public partial class JumpToSocialMedia : NonReportPage
{
    enum JumpType
    {
        NotFound = 0,
        Signup = 1,
        Dashboard = 2, 
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["GetUserQuery"] == "1")
        {
            string username = Request["username"].Trim();

            string queryString = this.Request.Url.Query;
            string root = this.Request.Url.AbsoluteUri.ToLower();
            root = root.Substring(0, root.Length - queryString.Length);
            root = root.Substring(0, root.Length - "JumpToCase.aspx".Length).Trim('/').Trim('\\');
            Response.ContentType = "text/javascript";
            string sQueryString = BuildSecureQueryString("u=" + username + "&IsPopup=1");
            Response.Write(string.Format("responseFromNPC('{0}/UpdateUser.aspx?{1}');", root, sQueryString));
            Response.End();
        }
        else if (Request["gethash"] == "1")
        {
            Response.ContentType = "text/javascript";
            Response.Write(string.Format("responseFromNPC('{0}');", BuildSecureQueryString(Request["text"].Replace("EQUALMARK", "=") + "&FromExCM=1")));
            Response.End();
            return;
        }
        PageType = SecurePageType.None;
        uxScript.Text = string.Empty;
        if (!IsPostBack)
        {
            if (SessionManager.SocialMediaSiteJumpTicket == null || SessionManager.SocialMediaSiteJumpTicket == Guid.Empty)
            {
                CreateJumpSiteTicket(); 
            }
            string script = string.Format("<script type=\"text/javascript\" src=\"{0}\"></script>", BuildGateUrl(true));
            uxScript.Text = VeraCodeSolution.DoVeraCode(script);
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }


    protected void uxRePost_Click(object sender, EventArgs e)
    {
        if (uxResponseValue.Value.Trim() != "1")
        {
            CreateJumpSiteTicket();
        }
        Response.Redirect(BuildGateUrl(false));
    }

    void CreateJumpSiteTicket()
    { 
        SessionManager.SocialMediaSiteJumpTicket = Guid.NewGuid();
        SessionManager.SocialMediaSiteJumpPassword = WebServices.SecurityServices.GenPwd(); 
        WebServices.SMApiServices.CreateJumpSiteTicket(
             SessionManager.CurrentClient,
             SessionManager.CurrentUser.SiteID,
             SessionManager.CurrentUser.RecId,
             Request.UserHostAddress,
             SessionManager.SocialMediaSiteJumpPassword,
             SessionManager.SocialMediaSiteJumpTicket,
             SessionManager.CurrentUser.EntityID,
             SessionManager.CurrentUser.EntityType,
             SessionManager.CurrentUserPermissions,
             SessionManager.CurrentUserThemeName);
    }

    void ExecuteClientScript(string key, string script)
    {
        ClientScript.RegisterStartupScript(typeof(JumpToSocialMedia), key, script, true);
    }

    protected string BuildGateUrl(bool isCheckSession)
    {
        string gateUrl = WebSiteSettings.SocialMediaGateUrl + "?" + (string.Format("u={0}&k={1}&j={2}&c={3}&dt={4}&UMode={5}&check={6}&uid={7}&ufullname={8}"
                    , Server.UrlEncode(EncryptText(SessionManager.CurrentUser.RecId.ToString()))
                    , Server.UrlEncode(EncryptText(SessionManager.SocialMediaSiteJumpPassword))
                    , Server.UrlEncode(EncryptText(SessionManager.SocialMediaSiteJumpTicket.ToString()))
                    , Server.UrlEncode(EncryptText(SessionManager.CurrentClient.ToString()))
                    , Server.UrlEncode(EncryptText(string.IsNullOrEmpty(Request["type"]) ? ((int)JumpType.NotFound).ToString() : Request["type"]))
                    , Server.UrlEncode(EncryptText(GeneralFuncsLib.GetHierarchyInfo(SessionManager.CurrentUser.EntityType).UserMode))
                    , Server.UrlEncode(EncryptText(isCheckSession ? "1" : "0"))
                    , Server.UrlEncode(EncryptText(SessionManager.CurrentUser.UserID))
                    , Server.UrlEncode(EncryptText(SessionManager.CurrentUser.UserNameFull))
                    ));

        HierarchyFilterValue filter = SessionManager.CurrentReportFilter; 
        if (filter != null && GeneralFuncsLib.IsMerchantMode(filter.HierarchyMode)
            && !string.IsNullOrEmpty(filter.Value))
        {
            gateUrl += "&m=" + Server.UrlEncode(EncryptText(filter.Value));
        }
        else if (SessionManager.CurrentUser.EntityType == 12 )
        {
            gateUrl += "&m=" + Server.UrlEncode(EncryptText(SessionManager.CurrentUser.EntityID));
        } 
        return gateUrl;
    } 
    string EncryptText(string plainText)
    {
        return WebServices.SMApiServices.SMEncryptText(plainText);
    }
}