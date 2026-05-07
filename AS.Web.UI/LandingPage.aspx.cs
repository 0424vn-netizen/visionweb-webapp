using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AS.Common.DBManager;
using System.Data;
using AS.Security.WS.Entities;
using AS.Controls.Pages;
using AS.Common.Logger;
using AS.Web.SharedSession;

[PagePermission("ASLandingPage")]
public partial class LandingPage : NonReportPage
{
    enum DataBindAction
    {
        BindClientList
    }
    const string LOGO_FOLDER = "~/res/images/Landing/";
    const string LOGO_APERIA = "Aperia.png";

    protected override void OnPreInit(EventArgs e)
    {
        if (!IsPostBack)
        {
            if (SessionManager.LandingCurrentUser != null)
            {
                string landingUserID = SessionManager.LandingCurrentUser.UserID;
                int landingCurClient = SessionManager.LandingCurrentUser.ASClient;

                Session.Clear();
                Response.Cookies.Clear();

                SessionManager.CurrentClient = landingCurClient;
                User user = WebServices.SecurityServices.GetUser(landingCurClient, landingUserID);
                SessionManager.CurrentUser = user;
                GeneralFuncsLib.SaveLoginUserData(user.UserID, (SecurePage)this.Page);
            }
            else
            {
                if (!IsUserWithPermission("ASLandingPage"))
                {
                    this.IntruderLog.LogData3 += "Per=ASLandingPage";
                    RaiseIntruderEvent(IntruderType.Permission);
                    return;
                }
            }
        }
        base.OnPreInit(e);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            OnDataBindControls(DataBindAction.BindClientList);
        }
    }
    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindClientList:
                BindClientList();
                break;
        }
    }
    public string GetClientIDParam(object clientID)
    {
        return BuildSecureQueryString("id=" + clientID);
    }
    private void BindClientList()
    {
        string spName = "spa_SEC_GetAllClient";
        FilterParameterCollection parameters = new FilterParameterCollection();
        DataTable dt = WebServices.SecurityServices.GetReports(spName, parameters);

        int numOfClient = dt.Rows.Count;
        for (int i = 0; i < numOfClient; i++)
        {
            var logo = dt.Rows[i]["LandingLogo"].ToString();
            if (logo.IsNullOrEmpty())
                dt.Rows[i]["LandingLogo"] = ResolveUrl(LOGO_FOLDER) + LOGO_APERIA;
            else
                dt.Rows[i]["LandingLogo"] = ResolveUrl(LOGO_FOLDER) + logo;
        }

        uxClientList.DataSource = dt;
        uxClientList.DataBind();
    }

    protected void uxClient_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        switch (e.Item.ItemType)
        {
            case ListItemType.Item:
            case ListItemType.AlternatingItem:
                {
                    LinkButton lb = (LinkButton)e.Item.FindControl("uxSiteJump");
                    var rowItem = (e.Item.DataItem as DataRowView).Row;
                    var logo = rowItem["LandingLogo"];
                    lb.Style.Add("background-image", "url('" + logo.ToString() + "')");
                    lb.ToolTip = rowItem["ClientDescription"].ToString();
                }
                break;
        }
    }

    protected void uxReload(object sender, EventArgs e)
    {
        SessionManager.CurrentClient = SessionManager.CurrentUser.ASClient;
        BindClientList();
    }
    private void SiteJump(string client)
    {
        int clientID = Int32.Parse(client);
        FilterParameterCollection paras = new FilterParameterCollection();
        paras.Add(new FilterParameter("@ASClient", clientID, DbType.Int32));
        DataTable dt = WebServices.SecurityServices.GetReports("spa_SEC_GetUserNameByClientID", paras);

        // Save current AS user
        string landingUserID = SessionManager.CurrentUser.UserID;
        int landingCurClient = SessionManager.CurrentClient;

        string loginURL = SharedSessionManager.USER_LOGIN_URL;

        Session.Clear();
        Response.Cookies.Clear();

        //44594 - VW - Session time out issue - Short term
        // Save login url
        SharedSessionManager.USER_LOGIN_URL = loginURL;
        // Save current AS user
        SessionManager.CurrentClient = landingCurClient;
        User landingUser = WebServices.SecurityServices.GetUser(landingCurClient, landingUserID);
        SessionManager.LandingCurrentUser = landingUser;

        //43745: Load Exclude Access Permission for Aperia User.
        PermissionCollection excludeAccessPermission = WebServices.SecurityServices.GetPermissionsByASUserGroupType(
               SessionManager.LandingCurrentUser.ASClient,
               SessionManager.LandingCurrentUser.UserID,
                WebSiteConstants.AS_SYSTEM_CS, true, true);

        if (excludeAccessPermission != null && excludeAccessPermission.Count > 0)
        {
            string excludeAccess = string.Empty;
            foreach (Permission permission in excludeAccessPermission)
            {
                excludeAccess += permission.PermissionCode + ",";
            }

            SessionManager.ExcludeAccessPermission = excludeAccess;
        }

        // Load static user to landing
        SessionManager.CurrentClient = clientID;

        if (dt != null && dt.Rows.Count > 0)
        {
            User user = WebServices.SecurityServices.GetUser(SessionManager.CurrentClient, dt.Rows[0]["UserID"].ToString());

            if (GeneralFuncsLib.SaveLoginUserData(user.UserID, this))
            {
                //Save Timezone
                if (!GeneralFuncsLib.GetCookie("ClientTimezone").IsNullOrEmpty() && !GeneralFuncsLib.GetCookie("DayLightSaving").IsNullOrEmpty())
                {
                    TimeZoneHandler.SaveTimeZone(null, string.Empty, GeneralFuncsLib.GetCookie("ClientTimezone"), GeneralFuncsLib.GetCookie("DayLightSaving").ToInt());
                }
                else
                {                    
                    string clientTimezone = GeneralFuncsLib.GetCookie("ClientTimezone").IsNotNullData() ? GeneralFuncsLib.GetCookie("ClientTimezone") : "";
                    string dayLightSaving = GeneralFuncsLib.GetCookie("DayLightSaving").IsNotNullData() ? GeneralFuncsLib.GetCookie("DayLightSaving") : "";

                    LoggerManager.Warn(string.Format("Cookie clienttimezone is null! UserId={0}; ClientId={1}; ClientTimezone={2}; DayLightSaving={3}"
                    , SessionManager.CurrentUser.UserID, SessionManager.CurrentUser.ASClient, clientTimezone, dayLightSaving));

                }

                //Reload header menu 
                SessionManager.GetHeaderMenu();

                //Sprint 6 - 46652 - AW Multi-currency Transaction Display
                GeneralFuncsLib.SetDefaultCurrency();

                // 44634 - Aperia user not landing on PurePortal Dashboard
                string redirectUrl = GeneralFuncsLib.GetDefaultPageOfLoggedInUser((AS.Controls.Pages.SecurePage)this.Page);
                if (!redirectUrl.IsNullOrEmpty())
                {
                    if (!Request["sparam"].IsNullOrEmpty())
                    {
                        redirectUrl += "?" + Request.QueryString.ToSafeString();
                    }
                    Response.Redirect(redirectUrl);
                }
                else
                {
                    Response.Redirect("~/Default.aspx");
                }
            }
        }

    }
    protected void SiteJumpCommand(object sender, CommandEventArgs e)
    {
        if (!e.CommandArgument.ToString().IsNullOrEmpty())
        {
            SiteJump(e.CommandArgument.ToString());
        }
    }
}
