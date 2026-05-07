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

using AS.Common.DBManager;
using AS.Security.WS.Entities;
using AS.Controls.Pages;
using AS.Common.Logger;
using AS.Web.SharedSession;

public partial class gate : NonReportPage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        Response.AddHeader("P3P", "policyref=\"/w3c/p3p.xml\", CP=\"IDC DSP COR IVAi IVDi OUR TST\"");
        Session.Clear();
        SessionManager.CurrentClient = int.Parse(AS.Common.DataProtection.Cryptophy.DecryptText(Request["c"] + ""));
        string userName = WebServices.SecurityServices.DecryptText(Request["u"] + "");
        // mobile redirect
        string pageName = !String.IsNullOrEmpty(Request["p"]) ? Request["p"].ToLower() : string.Empty ;
        string isMobileRedirect = Request["mb"];
        string loginUrl = Request["l"];
        bool isRedirectLogin = !String.IsNullOrEmpty(loginUrl) && loginUrl.ToLower() == "true";

        if (!Request["f"].IsNullOrEmpty())
        {
            SessionManager.JumpSource = WebServices.SecurityServices.DecryptText(Request["f"]);
        }
        else
        {
            SessionManager.JumpSource = "Outside";
        }

        Guid jumper = Guid.Empty;
        try
        {
            jumper = new Guid(WebServices.SecurityServices.DecryptText(Request["j"] + ""));
        }
        catch (Exception we)
        {
            jumper = Guid.Empty;
        }
        bool failed = false;
        User user = WebServices.SecurityServices.GetUser(SessionManager.CurrentClient, userName);
        if (user != null)
        {

            //Get language default from jump site
            if (Request["lan"] != null)
            {
                SessionManager.CurrentLanguage = Request["lan"].ToInt();
            }

            if (WebServices.SecurityServices.CheckJumpSiteTicket(user.RecId, jumper, Request["k"], WebSiteSettings.DefaultSystem, SessionManager.CurrentClient))
            {
                LoginUrl = "[jumpsite]";
                //44594 - VW - Session time out issue - Short term
                SharedSessionManager.USER_LOGIN_URL = LoginUrl;
                if (!GeneralFuncsLib.SaveLoginUserData(user.UserID, this))
                {
                    failed = false;
                }
                else
                {
                    SessionManager.UsePrimaryPrefix = CheckUsePrimaryPrefix();
                    GetHierarchy();
                }
            }
            else
            {
                failed = true;
            }
        }
        else
        {
            failed = true;
        }

        if (failed)
        {
            string redirectLoginUrl = string.Format("~/{0}?_fs=mb", pageName);
            Response.Redirect(redirectLoginUrl);
        }
        else
        {
            //Save Timezone
            if (!GeneralFuncsLib.GetCookie("ClientTimezone").IsNullOrEmpty() && !GeneralFuncsLib.GetCookie("DayLightSaving").IsNullOrEmpty())
            {
                TimeZoneHandler.SaveTimeZone(null, string.Empty, GeneralFuncsLib.GetCookie("ClientTimezone"), GeneralFuncsLib.GetCookie("DayLightSaving").ToInt());
            }
            else
            {
                LoggerManager.Debug("Cookie clienttimezone is null!");
            }
            string redirectUrl = string.Empty;

            //view full site Vision Web Mobile
            if (!String.IsNullOrEmpty(isMobileRedirect) && isMobileRedirect == "true")
            {

                string fromDate = Request["dF"];
                string toDate = Request["dT"];
                string mid = Request["mid"];
                if (isRedirectLogin)
                {
                    string redirectLoginUrl = string.Format("~/{0}?_fs=mb", pageName);
                    Response.Redirect(redirectLoginUrl);
                }
                else
                {
                    redirectUrl = BuildMobileRedirectUrl(pageName, fromDate, toDate, mid);
                }
            }
            else
            {
                redirectUrl = GeneralFuncsLib.GetDefaultPageOfLoggedInUser((SecurePage)this.Page);
            }

            //Reload header menu 
            SessionManager.GetHeaderMenu();
            //Sprint 6 - 46652 - AW Multi-currency Transaction Display
            GeneralFuncsLib.SetDefaultCurrency();

            // Check sitejump from CS site
            if (String.IsNullOrEmpty(isMobileRedirect))
                SessionManager.IsSiteJump = jumper != Guid.Empty;

            Response.Redirect(redirectUrl);
        }
    }

    private string BuildMobileRedirectUrl(string pageName, string fromDate, string toDate, string mid)
    {
        string url = string.Empty;
        switch (pageName)
        {
            case WebSiteConstants.NOPERMISSION_PAGE:
                url = string.Format("~/{0}?_fs=mb", pageName);
                break;
            case WebSiteConstants.DEPOSIT_PAGE:
                url = "~/PaymentHistory.aspx?" + this.BuildSecureQueryString("MerchantNumber=" + mid +
                    "&BeginDate=" + fromDate + "&EndDate=" + toDate + "&mb=true&_fs=mb");
                break;
            case WebSiteConstants.RETRIEVAL_PAGE:
                url = "~/Retrievals.aspx?" + this.BuildSecureQueryString("MerchantNumber=" + mid +
                    "&BeginDate=" + fromDate + "&EndDate=" + toDate + "&mb=true&_fs=mb");
                break;
            case WebSiteConstants.DASHBOARD_PAGE:
                url = "~/Dashboard.aspx?_fs=mb";
                break;
            case WebSiteConstants.UPDATEMYPROFILE_PAGE:
                url = "~/ManageProfile.aspx";
                break;
            case WebSiteConstants.STATEMENT_PAGE:
                url = "~/Statement.aspx?" + this.BuildSecureQueryString("MerchantNumber=" + mid + "&mb=true&_fs=mb");
                break;
            case WebSiteConstants.CHARGEBACK_PAGE:
                url = "~/Chargebacks.aspx?" + this.BuildSecureQueryString("MerchantNumber=" + mid +
                    "&BeginDate=" + fromDate + "&EndDate=" + toDate + "&mb=true&_fs=mb");
                break;
            case WebSiteConstants.BATCH_PAGE:
                url = "~/BatchHistory.aspx?" + this.BuildSecureQueryString("MerchantNumber=" + mid +
                    "&BeginDate=" + fromDate + "&EndDate=" + toDate + "&mb=true&_fs=mb");
                break;

        }
        return url;
    }

    private bool CheckUsePrimaryPrefix()
    {
        FilterParameterCollection paras = new FilterParameterCollection();
        paras.Add(new FilterParameter("@ASClient", SessionManager.CurrentUser.ASClient, DbType.Int32));
        paras.Add(new FilterParameter("@UserName", SessionManager.CurrentUser.UserID, DbType.AnsiString));
        paras.Add(new FilterParameter("@SystemId", SessionManager.CurrentSystem, DbType.Int32));
        DataTable tb = WebServices.SecurityServices.GetReports("spa_SEC_GetHierarchysForUser", paras);
        if (tb != null && tb.Rows.Count > 0)
        {
            if (tb.Rows[0]["UsePrimaryPrefix"].ToString() == "True")
                return true;
            else
                return false;
        }
        else
            return false;
    }
    private void GetHierarchy()
    {
        FilterParameterCollection paras = new FilterParameterCollection();
        paras.AddLoggedInUserReportingParams(false);
        paras.Add(new FilterParameter("@EntityNumber", SessionManager.CurrentUser.EntityID, DbType.AnsiString));
        DataTable dt = WebServices.MsReportServices.GetReports("spa_ms_GetHierarchyName", paras);
        string hiearchy = SessionManager.CurrentUser.UserID;
        if (dt != null && dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["EntityName"] != DBNull.Value && !dt.Rows[0]["EntityName"].ToString().IsNullOrEmpty())
                SessionManager.HierarchyName = dt.Rows[0]["EntityName"].ToString();
        }
    }
}
