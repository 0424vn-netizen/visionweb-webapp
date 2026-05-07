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

[PagePermission("CMStatusMaintenance,CMIssuesMaintenance,CMOwnershipGroupMaintenance,CMPriorityMaintenance,CMSearchCase,CMOpenCase,CMAdministration,CMActionMaintenance,CMOutcomesMaintenance")]
public partial class JumpToCase : NonReportPage
{
    enum JumpType
    {
        Status = 1,
        Issue = 2,
        OwnershipGroup = 3,
        Priority = 4,
        SearchCase = 5,
        ViewAddCase = 6,
        MyCase = 7,
        CaseHistory = 8,
        CaseSettings = 9,
        CaseHistoryOnly = 15,
        NotFound = 0
    }
    protected bool IsIframeSupported
    {
        get
        {
            return Request["type"] == ((int)JumpType.CaseHistory).ToString();
        }
    }

    protected string IsShowDefaultValue
    {
        get
        {
            if (Request.Params.AllKeys.Contains("IsShowDefaultValue"))
                return Request["IsShowDefaultValue"].ToString();
            return "0";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["GetUserQuery"] == "1")
        {
            string username = Request["username"].Trim();
             
            string queryString = this.Request.Url.Query;
            string root = this.Request.Url.AbsoluteUri.ToLower();
            if (ConfigurationManager.AppSettings["ForceHttpsOnCm"].ToString() == "1")
                root = root.Replace("http://", "https://").ToLower();
            root = root.Substring(0, root.Length - queryString.Length);
            root = root.Substring(0, root.Length - "JumpToCase.aspx".Length).Trim('/').Trim('\\'); 
            Response.ContentType = "text/javascript"; 
            string sQueryString = BuildSecureQueryString("u=" + username  + "&IsPopup=1");
            Response.Write(string.Format("responseFromNPC('{0}/UpdateUser.aspx?{1}');", root, sQueryString));
            Response.End();  
        }
        else if (Request["gethash"] == "1")
        {
            //39919 - Fixbugs OP #34331
            Response.ContentType = "text/javascript";
            Response.Write(string.Format("responseFromNPC('{0}');", BuildSecureQueryString(Request["text"].Replace("EQUALMARK", "=") + "&FromExCM=1")));
            Response.End();
            return;
        }
        PageType = SecurePageType.None;
        uxScript.Text = string.Empty;
        if (!IsPostBack)
        {
            if (SessionManager.CaseSiteJumpTicket == null || SessionManager.CaseSiteJumpTicket == Guid.Empty)
            {
                CreateJumpSiteTicket();
                //ExecuteClientScript("gotoCaseAtFirstTime", "response('1');");                
                //return;
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
        SessionManager.CaseSiteJumpTicket = Guid.NewGuid();
        SessionManager.CaseSiteJumpPassword = WebServices.SecurityServices.GenPwd();

        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.Add(new FilterParameter("@ASClientID", SessionManager.CurrentClient, DbType.Int32));
        parameters.Add(new FilterParameter("@UserID", SessionManager.CurrentUser.RecId, DbType.Guid));
        parameters.Add(new FilterParameter("@RemoteIP", Request.UserHostAddress, DbType.String));
        parameters.Add(new FilterParameter("@TempPass", SessionManager.CaseSiteJumpPassword, DbType.String));
        parameters.Add(new FilterParameter("@Jumper", SessionManager.CaseSiteJumpTicket, DbType.Guid));
        parameters.Add(new FilterParameter("@SiteID", SessionManager.CurrentUser.SiteID, DbType.String));
        parameters.Add(new FilterParameter("@EntityNumber", SessionManager.CurrentUser.EntityID, DbType.String));
        parameters.Add(new FilterParameter("@EntityTypeID", SessionManager.CurrentUser.EntityType, DbType.Int32));
        parameters.Add(new FilterParameter("@UserPermissions", SessionManager.CurrentUserPermissions, DbType.String));
        parameters.Add(new FilterParameter("@ThemeName", SessionManager.CurrentUserThemeName, DbType.String));
        WebServices.CsReportServices.ExecuteNonQueryCommand("spa_CM_InsertJumperInfo", parameters, out parameters);
    }

    void ExecuteClientScript(string key, string script)
    {
        ClientScript.RegisterStartupScript(typeof(JumpToCase), key, script, true);
    }


    protected string BuildGateUrl(bool isCheckSession)
    {

        var root = this.ResolveUrl("~");
        string gateUrlConfig = ConfigurationManager.AppSettings["CaseGate"].Trim().Trim('/');

        if (!gateUrlConfig.StartsWith("http"))
        {
            gateUrlConfig = Path.Combine(root, gateUrlConfig);
        }

        if (ConfigurationManager.AppSettings["ForceHttpsOnCm"].ToString() == "1")
            gateUrlConfig = gateUrlConfig.Replace("http://", "https://");

        string gateUrl = gateUrlConfig + "?" + (string.Format("u={0}&k={1}&j={2}&c={3}&dt={4}&check={5}&l={6}&lid={7}&opnewcase={8}&IsShowDefaultValue={9}"
                    , Server.UrlEncode(EncryptText(SessionManager.CurrentUser.RecId.ToString()))
                    , Server.UrlEncode(EncryptText(SessionManager.CaseSiteJumpPassword))
                    , Server.UrlEncode(EncryptText(SessionManager.CaseSiteJumpTicket.ToString()))
                    , Server.UrlEncode(EncryptText(SessionManager.CurrentClient.ToString()))
                    , Server.UrlEncode(EncryptText(string.IsNullOrEmpty(Request["type"]) ? ((int)JumpType.NotFound).ToString() : Request["type"]))
                    , Server.UrlEncode(EncryptText(isCheckSession ? "1" : "0"))
                    , Server.UrlEncode(EncryptText(GeneralFuncsLib.GetCurrentCulture()))
                    , Server.UrlEncode(EncryptText(SessionManager.CurrentLanguage.ToString()))
                    , Server.UrlEncode(EncryptText(((NonReportPage)Page).IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_CM_OPEN_CASE) ? "1" : "0"))
                    , Server.UrlEncode(EncryptText(IsShowDefaultValue))));

        HierarchyFilterValue filter = SessionManager.CurrentReportFilter;
        
        if (filter != null && GeneralFuncsLib.IsMerchantMode(filter.HierarchyMode)           
            && !string.IsNullOrEmpty(filter.Value))
        {
            gateUrl += "&m=" + Server.UrlEncode(EncryptText(filter.Value));
        }        

        if (Request["type"] == ((int)JumpType.CaseHistory).ToString())
        {
            gateUrl += "&from=" + Server.UrlEncode(EncryptText(Request["from"]));
        }

        if (Request["type"] == ((int)JumpType.CaseHistoryOnly).ToString())
        {
            gateUrl += "&p=" + Server.UrlEncode(EncryptText(Request["p"]));
        }

        if (!Request["cid"].IsNullOrEmpty())
        {
            gateUrl += "&cid=" + Server.UrlEncode(EncryptText(Request["cid"]));
        }

        if (!Request["FilterMode"].IsNullOrEmpty())
        {
            gateUrl += "&FilterMode=" + Server.UrlEncode(EncryptText(Request["FilterMode"]));
        }

        if (!Request["FilterValue"].IsNullOrEmpty())
        {
            gateUrl += "&FilterValue=" + Server.UrlEncode(EncryptText(Request["FilterValue"]));
        }

        if (!Request["xmlFilter"].IsNullOrEmpty())
        {
            gateUrl += "&xmlFilter=" + Server.UrlEncode(Request["xmlFilter"]);
        }
        if (!Request["m"].IsNullOrEmpty())
        {
            gateUrl += "&m=" + Server.UrlEncode(EncryptText(Request["m"]));
        }
        return gateUrl;

    }

    string EncryptText(string plainText)
    {
        return WebServices.ApiServices.EncryptText(plainText);
    }
}