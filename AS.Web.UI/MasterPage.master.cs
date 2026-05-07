using AS.Common;
using AS.Common.DBManager;
using AS.Controls.ASP.Net;
using AS.Controls.Pages;
using AS.Leads.UserMaintService;
using AS.Security.WS.Entities;
using AS.VW.Share.Models;
using AS.Web.SharedSession;
using AS.Web.UI.Controls;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;

public partial class MasterPage : MasterPageNormal
{
    protected string RootUrl = "";
    SecurePage _page;
    protected bool IsLandingPage
    {
        get
        {
            return this.Request.CurrentExecutionFilePath.EndsWith("LandingPage.aspx", StringComparison.OrdinalIgnoreCase);
        }
    }



    protected void Page_Load(object sender, EventArgs e)
    {
        uxWindowManager.Localization.Cancel = Resources.MessageManager.TextCancel;
        RootUrl = ResolveUrl("~/");
        _page = (SecurePage)Page;
        uxOpenNewCase.Attributes.Add("onclick", string.Format("return openPopupWindowOnMenu(event,\"{0}JumpToCase.aspx?type=6\",\"OpenNewCase\");", ResolveUrl("~/")));

        uxMyCase.Attributes.Add("onclick", string.Format("return openPopupWindowOnMenu(event,\"{0}JumpToCase.aspx?type=7\",\"MyCases\");", ResolveUrl("~/")));
        uxMPAList.Attributes.Add("href", BuildRedirectUrl("/boarding/taskmanager/mpa"));
        uxTaskList.Attributes.Add("href", BuildRedirectUrl("/boarding/taskmanager/tasklist"));

        uxUserProfile.NavigateUrl = ResolveUrl("~/ManageProfile.aspx");
        uxLandingPage.NavigateUrl = ResolveUrl("~/LandingPage.aspx");

        //45213 - [AW_Aperia] - Allied Wallet VW Implementation
        uxAddNewMerchant.Attributes.Add("href", BuildRedirectUrl("~/AddNewMerchant.aspx"));

        string usrName = "";
        User user = SessionManager.CurrentUser;
        if (user != null)
        {
            if (user.UserNameFull == "")
            {
                usrName += user.UserNameFirst + " " + user.UserNameLast;
            }
            else
            {
                usrName += user.UserNameFull;
            }
        }
        string websiteTitle = GeneralFuncsLib.GetDataOfExtendedSetting("WEBSITE_TITLE_" + WebSiteSettings.WebSiteType.ToUpper());

        // TK26930
        //if (websiteTitle == string.Empty)
        //{
        //    websiteTitle = SessionManager.ClientInfo.ClientName;
        //}
        //Page.Title = websiteTitle;

        uxUserInfoWithoutLink.Text = uxUserInfo.Text = VeraCodeSolution.ValidateResponseData(usrName.Length > 20 ? usrName.Substring(0, 20) + "..." : usrName);
        //Show tooltip when username over 20 chars
        if (usrName.Length > 20)
        {
            uxUserInfoWithoutLink.ToolTip = uxUserProfile.ToolTip = usrName;
        }
        if (GeneralFuncsLib.GetDataOfExtendedSetting("IS_QA_ENVIRONMENT").Equals("1"))
        {
            uxWelcomeBar.Attributes["class"] = uxWelcomeBar.Attributes["class"] + " qa";
            //uxQaEnvironment.Visible = true;
        }
        if (SessionManager.CurrentUser.PrevLoginDTS.Year > 1900)
        {
            uxLastLogin.Text = AS.Common.VeraCodeSolution.ValidateResponseData(GetLocalResourceObject("MasterPage_master_cs_LastLogin").ToString() + " " + SessionManager.CurrentUser.PrevLoginDTS.ToString("MM/dd/yyyy - hh:mm tt") + " | ");
        }
        else
        {
            uxLastLogin.Text = "";
        }
        if (_page.LoginUrl == "[jumpsite]" && !SessionManager.SingleSignOnMenuMod)
        {
            switch (SessionManager.JumpSource)
            {
                case "CS":
                    uxLogOffLink.Text = GetLocalResourceObject("MasterPage_master_cs_ReturnToCS").ToString();
                    break;
                default:
                    uxLogOffLink.Text = GetLocalResourceObject("MasterPage_master_cs_Return").ToString();
                    break;
            }
        }
        //  We apply this change for TNBCI only ClientID 49
        //if ((_page.LoginUrl == "[jumpsite]" || SessionManager.SingleSignOnMenuMod) && SessionManager.CurrentClient == 49)
        //{
        //uxLogOffLink.Text = "Home";
        //uxHomeLink.Text = "Dashboard";
        // divBannerLogo.Attributes.Add("onclick", string.Format("javascript:__doPostBack('{0}','')", uxLogOffLink.UniqueID));
        // divBannerLogo.Attributes.Add("style", "width: 280px; cursor:pointer");
        //}
        LoadMenuBySitemap();

        if (this.IsLandingPage)
        {
            uxPanelHome.Visible = false;
            uxPanelLanding.Visible = false;
        }
        else if (SessionManager.LandingCurrentUser != null || _page.IsUserWithPermission("ASLandingPage"))
        {
            uxPanelHome.Visible = true;
            uxPanelLanding.Visible = true;
        }
        else
        {
            uxPanelHome.Visible = true;
            uxPanelLanding.Visible = false;
        }

        if (_page.IsUserWithPermission("MSDashboard") || _page.IsUserWithPermission("Dashboard"))
        {
            uxHomeLink.NavigateUrl = ResolveUrl("~/Dashboard.aspx");
        }
        else if (_page.IsUserWithPermission("MerchProfile") || _page.IsUserWithPermission("MSMerchProfile"))
        {
            uxHomeLink.NavigateUrl = ResolveUrl("~/MerchantProfile.aspx");
        }
        else if (_page.IsUserWithPermission("TCFlatAuth"))
        {
            uxHomeLink.NavigateUrl = ResolveUrl("~/AuthorizationSearch.aspx");
        }
        else
        {
            uxHomeLink.NavigateUrl = ResolveUrl("~/Default.aspx");
        }

        if (GeneralFuncsLib.IsIFrameSupported())
        {
            uxLogOffLink.Visible = false;
        }
        ShowUnreadMessages();
        if (!Page.IsPostBack)
        {
            if (GeneralFuncsLib.HasMultiLanguageFeature)
            {
                plhdLanguage.Visible = true;
                BindDropdownListLanguage();
            }
            else
            {
                plhdLanguage.Visible = false;
            }
        }

        // TK 38053
        if (GeneralFuncsLib.ShowEnvironmentIndicator())
        {
            uxPanelEnvironment.Visible = true;
            uxEnvironment.Text = GeneralFuncsLib.GetCurrentEnvironment(false);
        }
        else
        {
            uxPanelEnvironment.Visible = false;
        }
        // TK 38053
    }

    private void ShowUnreadMessages()
    {
        if (!IsShowUnReadMessage())
            return;

        string url = Request.AppRelativeCurrentExecutionFilePath;
        if (SessionManager.ShowUnreadMessages == false && !url.Equals("~/ManageProfile.aspx", StringComparison.OrdinalIgnoreCase) && !url.Equals("~/header.aspx", StringComparison.OrdinalIgnoreCase))
        {
            SessionManager.ShowUnreadMessages = true;
            FilterParameterCollection parameters = new FilterParameterCollection();
            FilterParameterCollection parameterOut = new FilterParameterCollection();
            parameters.AddLoggedInUserReportingParams();
            parameters.Add(new FilterParameter("@DateFilterMode", (int)DateOptionMode.DateRange, DbType.Int32));
            parameters.Add(new FilterParameter("@BeginDate", DateTime.Now.GetFirstDayOfMonth(), DbType.DateTime));
            parameters.Add(new FilterParameter("@EndDate", DateTime.Now, DbType.DateTime));
            parameters.Add(new FilterParameter("@View", "N", DbType.AnsiString));
            string spa_name = string.Empty;
            if (SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.AS || SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.CS)
                spa_name = "spa_cs_GetMySrvMessages";
            else
                spa_name = "spa_GetMyMessages";
            DataTable tableUnreadMessage = WebServices.RiskServices.GetReports(spa_name, parameters);
            if (tableUnreadMessage == null || tableUnreadMessage.Rows.Count < 1)
                return;
            if (tableUnreadMessage.Rows.Count > 0)
                this.Page.RegisterStartupScript("New Messages Modal", "<script> setTimeout('showUnreadMessages()', 500); </script>");
        }
    }

    private bool IsShowUnReadMessage()
    {
        if (SessionManager.CurrentUserPermissions.Contains(",MSSendMsg,") || SessionManager.CurrentUserPermissions.Contains(",MSViewMsg,")
            || SessionManager.CurrentUserPermissions.Contains(",SendSrvMsg,") || SessionManager.CurrentUserPermissions.Contains(",ViewSrvMsg,"))
            return true;
        else
            return false;
    }


    protected void uxLogout_Click(object sender, EventArgs e)
    {
        bool _singleSignOnMenuMod = SessionManager.SingleSignOnMenuMod;
        GeneralFuncsLib.UpdateUserLogs("Logout");
        GeneralFuncsLib.UpdateDataWhenLogout();
        int clientId = SessionManager.CurrentClient;
        Session.Clear();
        SessionManager.CurrentClient = clientId;
        Session.Abandon();
        Response.Cookies.Clear();
        Response.Cookies["ClientTimezone"].Expires = DateTime.Now.AddDays(-1);
        FormsAuthentication.SignOut();

        //Re-new SessionId support for boarding/ case management
        Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));

        if (_singleSignOnMenuMod)
        {
            var ssoMsUrlCookie = Request.Cookies["sso_ms_url"];
            if (ssoMsUrlCookie != null)
            {
                //Clear sso_ms_url cookie
                ClearCookie("sso_ms_url");
                _page.LoginUrl = string.Empty;
                //44594 - VW - Session time out issue - Short term
                SharedSessionManager.USER_LOGIN_URL = _page.LoginUrl;

                Response.Redirect(ssoMsUrlCookie.Value);
            }
            else
            {
                Response.Cookies.Add(new HttpCookie("logout_opt", ((int)WebSiteEnums.LOGOUT_MODE.SSO).ToString()));
                Response.Redirect(FormsAuthentication.LoginUrl);
                //this.Page.RegisterStartupScript("SSOLogout", "<script>SSOLogout();</script>");
            }
        }
        else
        {
            if (_page.LoginUrl == "[jumpsite]")
            {
                _page.LoginUrl = string.Empty;
                //44594 - VW - Session time out issue - Short term
                SharedSessionManager.USER_LOGIN_URL = _page.LoginUrl;

                Response.Cookies.Add(new HttpCookie("logout_opt", "3"));
                Response.Redirect(FormsAuthentication.LoginUrl);
            }
            else
            {
                Response.Redirect(_page.LoginUrl);
            }
        }
    }

    private void ClearCookie(string cookieName)
    {
        HttpCookie cookie = new HttpCookie(cookieName, Request.RawUrl);
        cookie.HttpOnly = true;
        cookie.Secure = System.Web.Security.FormsAuthentication.RequireSSL;
        cookie.Expires = DateTime.Now.AddDays(-1);
        HttpContext.Current.Response.Cookies.Add(cookie);
    }

    protected void uxManageProfile_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/ManageProfile.aspx");
    }

    private void LoadMenuBySitemap()
    {
        if (SessionManager.CurrentMenuItems == null)
        {
            LoadMenuBarDataSource();
            // Call Keep Alive after login
            if (GeneralFuncsLib.IsMCFRisk())
            {
                RM_MCF_GeneralFuncsLib.KeepAliveWIP();
            }
        }
        ASMenu menu = new ASMenu(SessionManager.CurrentMenuItems, "MenuID", "MenuParentID", "MenuOrder", "MenuName", "MenuUrl");
        menu.IsMobileBrowser = AS.Web.SharedSession.General.IsMobileBrowser();

        menu.UrlHomeLogo = GeneralFuncsLib.GetDefaultPageOfLoggedInUser((SecurePage)Page);

        menu.OnFinishCreateLink += (a, r) =>
        {
            //DisplayType: 3: Jump to case management; 4:Redirect URL
            if (r["DisplayType"].ToString() == "3")
            {
                ((HtmlGenericControl)a).Attributes.Add("onclick", string.Format("return openPopupWindowOnMenu(event, '{0}', '{1}' )", ResolveUrl(r["MenuUrl"].ToSafeString()), r["MenuName"].ToString().Replace(" ", "")));
                ((HtmlGenericControl)a).Attributes.Add("href", "#");
            }
            else if (r["DisplayType"].ToString() == "4")
            {
                ((HtmlGenericControl)a).Attributes.Add("href", BuildRedirectUrl(r["MenuUrl"].ToSafeString()));
            }
        };

        uxMasterHeaderMenuControl.LoadMenu(menu);

        this.siteHeader.Controls.Add(menu);
        uxHeaderMenu.Visible = !base.HideHeaderMenu;

        //43724 – VW - Auto-refresh Cache of JavasScripts and CSS files 
        // share theme url to child module as CMS
        if (SharedSessionManager.ThemeUrlFull.IsNullOrEmpty())
        {
            string path = string.Empty;
            string enableBundle = ConfigurationManager.AppSettings["BundleCssJs"];

            if (!string.IsNullOrEmpty(enableBundle) && enableBundle.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    // get all css files in folder App_Themes
                    var bundles = BundleTable.Bundles.ToList();
                    var list = bundles.Where(i =>
                                            i.Path.Contains(string.Format("App_Themes/{0}/", SessionManager.CurrentUserThemeName))
                                            && i.Path.Contains("css"))
                                        .ToList();

                    for (int i = 0; i < list.Count; i++)
                    {
                        path += ";" + Scripts.Url(list[i].Path).ToASString();
                    }
                }
                catch (Exception ex)
                {
                    AS.Common.Logger.LoggerManager.Debug(string.Format("Load Menu By Sitemap: themename={0}; client={1}; UserId={2} "
                        , SessionManager.CurrentUserThemeName, SessionManager.CurrentUser.ASClient, SessionManager.CurrentUser.UserID), ex);
                }

            }
            else
            {
                path = string.Format("{0}App_Themes/{1}/branding_master.min.css", _page.ResolveUrl("~/"), SessionManager.CurrentUserThemeName);
            }

            AS.Web.SharedSession.SharedSessionManager.ThemeUrlFull = path.Trim(';');
        }
    }

    private string GetLogoDescription()
    {
        string logoText = string.Empty;
        string config = GeneralFuncsLib.GetDataOfExtendedSetting(WebSiteConstants.SHOW_LOGO_DESCRIPTION);

        if (SessionManager.CurrentUser != null)
        {
            if ((SessionManager.CurrentSystem == WebSiteConstants.AS_SYSTEM_MS && config.Contains("MS"))
                || SessionManager.CurrentSystem == WebSiteConstants.AS_SYSTEM_CS && config.Contains("CS"))
            {
                //Only Get Bank name for Lead Module
                if (SessionManager.CurrentUser.ASClient == 118)
                {
                    UserInfo userInfo = new UserInfo();
                    userInfo.AsClientId = SessionManager.CurrentUser.ASClient;
                    userInfo.SiteId = SessionManager.CurrentUser.SiteID;
                    userInfo.UserId = SessionManager.CurrentUser.UserID;
                    userInfo.UserMode = String.Empty;
                    userInfo.UserSessionId = SessionManager.UniqueSessionID;
                    userInfo.RecId = SessionManager.CurrentUser.RecId;

                    UserMaintBusiness userMaintBusiness = new UserMaintBusiness(userInfo);
                    DataSet ds = userMaintBusiness.GetAllBanks("", "");
                    if (ds.Tables.Count > 0 && ds.Tables[0].HasData() && ds.Tables[0].Columns.Contains("BankName"))
                    {
                        logoText = ds.Tables[0].Rows[0]["BankName"].ToString();
                    }
                }
            }
        }
        return logoText;
    }


    private void LoadMenuBarDataSource()
    {
        if ((SessionManager.CurrentMenuItems == null) || (SessionManager.SingleSignOnMenuMod == true))
        {
            SecMenuItemCollection MenuItems = new SecMenuItemCollection();
            if (SessionManager.SingleSignOnMenuMod)
            {
                MenuItems = WebServices.SecurityServices.GetMenuItemsByUserSSO(SessionManager.CurrentUser.ASClient, SessionManager.CurrentUser.UserID, SessionManager.CurrentUserRoles[0].HierarchyID, SessionManager.SSOSecurityLevel, SessionManager.CurrentLanguage, false);
                _page.LoginUrl = "[jumpsite]";
                //44594 - VW - Session time out issue - Short term
                SharedSessionManager.USER_LOGIN_URL = _page.LoginUrl;
            }
            else
            {
                MenuItems = WebServices.SecurityServices.GetMenuItemsByUser(SessionManager.CurrentUser.ASClient, SessionManager.CurrentUser.UserID, SessionManager.CurrentUserRoles[0].HierarchyID, SessionManager.CurrentLanguage, false);
            }

            //Remove PCI menu for user PCI not active
            if (!_page.IsUserWithPermission("SiteAccessPCIAdmin") && !_page.IsUserWithPermission("HierarchySiteAccessPCIAdmin")
                && !_page.IsUserWithPermission("MerchantSiteAccessPCIAdmin"))
            {
                foreach (SecMenuItem item in MenuItems)
                {
                    if (item.Permissions == "SiteAccessPCIAdmin" || item.Permissions == "HierarchySiteAccessPCIAdmin" || item.Permissions == "MerchantSiteAccessPCIAdmin")
                    {
                        MenuItems.Remove(item);
                        break;
                    }
                }
            }

            //End of remove PCI menu for user PCI not active

            //Start - manually add PCI redesign
            if (CheckUserCanSeePCIReskinMenu())
            {
                AddPCIReskinMenu(MenuItems);
            }
            //End - manually add PCI redesign

            //49947 - WRFC - Merchant has an error when trying to add a new user
            bool hasSyn1099K = GeneralFuncsLib.CheckPermissionSyn1099K(SessionManager.CurrentSystem == WebSiteConstants.AS_SYSTEM_MS, SessionManager.CurrentUser);
            if (!hasSyn1099K)
            {
                RemoveMenu1099(MenuItems);
            }

            var menuDataSource = new DataTable("Sitemap");
            menuDataSource.Columns.Add("MenuID");
            menuDataSource.Columns.Add("MenuName");
            menuDataSource.Columns.Add("MenuUrl");
            menuDataSource.Columns.Add("MenuParentID");
            menuDataSource.Columns.Add("MenuOrder");
            menuDataSource.Columns.Add("Description");
            menuDataSource.Columns.Add("Target");
            menuDataSource.Columns.Add("DisplayType");
            //42589 – VW – FIS - Password Reset and Expiration Issues
            menuDataSource.Columns.Add("MenuPermissions");

            string target = "_self";// _blank _self

            var itemsToRemove = new List<SecMenuItem>();
            foreach (SecMenuItem item in MenuItems)
            {
                if (item.Url != null && item.Url.Contains("rm_MCF_MgmtReport_Exports.aspx")
                    && !GeneralFuncsLib.GetDataOfExtendedSetting("SHOW_EXPORT_QUEUE").ToLower().Equals("true"))
                {
                    itemsToRemove.Add(item);
                    break;
                }
            }

            foreach (var item in itemsToRemove)
            {
                MenuItems.Remove(item);
            }
                
            foreach (SecMenuItem item in MenuItems)
            {
                if (item.Permissions.Equals(WebSiteConstants.SEC_PERMISSION_JSACCESS, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                else if (!item.Permissions.Equals("CMManageTheme", StringComparison.OrdinalIgnoreCase))
                {
                    target = item.DisplayType == "2" ? "_blank" : "_self";
                    if (item.Parent == 0)
                    {
                        menuDataSource.Rows.Add(new object[] { item.SiteMapId.ToString(), item.Title, item.Url, null, item.NodeOrder.ToString(), item.Description, target, item.DisplayType, item.Permissions });
                    }
                    else
                    {
                        menuDataSource.Rows.Add(new object[] { item.SiteMapId.ToString(), item.Title, item.Url, item.Parent, item.NodeOrder.ToString(), item.Description, target, item.DisplayType, item.Permissions });
                    }
                }
            }

            SessionManager.CurrentMenuItems = menuDataSource;
        }
    }

    const int maxSiteMapID = 10000;
    private void ProcessMCFRisk(SecMenuItemCollection MenuItems, bool isDuplicate)
    {
        string removeMenuItems = System.Configuration.ConfigurationManager.AppSettings["RiskMCFRemoveMenuItems"];
        string[] arrRemoveItems = string.IsNullOrEmpty(removeMenuItems) ? null : removeMenuItems.Split('|');
        SecMenuItemCollection removeMenu = new SecMenuItemCollection();
        int parentRemoveItem = 0;
        if (isDuplicate)
        {
            SecMenuItemCollection nrtRisk = new SecMenuItemCollection();
            int siteMapIdParent = 0;
            int itemNodeParent = 0;
            foreach (SecMenuItem item in MenuItems)
            {
                if (item.DisplayType == "5" && (arrRemoveItems != null && arrRemoveItems.Contains(item.Url)))
                {
                    parentRemoveItem = item.Parent + maxSiteMapID;
                }

                if (item.DisplayType == "5" && !(arrRemoveItems != null && arrRemoveItems.Contains(item.Url)))
                {
                    SecMenuItem newItem = new SecMenuItem
                    {
                        Parent = item.Parent,
                        SiteMapId = item.SiteMapId,
                        Description = item.Description,
                        Title = item.Title,
                        NodeOrder = item.NodeOrder,
                        Url = item.Url
                    };
                    if (item.Parent == 0)
                    {
                        siteMapIdParent = item.SiteMapId;
                        newItem.Title = GetLocalResourceObject("RiskMCFModule").ToString();
                        newItem.SiteMapId = siteMapIdParent + maxSiteMapID;
                        itemNodeParent = item.NodeOrder;
                    }
                    nrtRisk.Add(newItem);
                }
                else
                {
                    // Fix bug order MCF menu
                    if (item.NodeOrder == itemNodeParent && item.Parent == 0)
                    {
                        item.NodeOrder = item.NodeOrder + 1;
                    }
                }
            }

            FillNodeChild(siteMapIdParent, nrtRisk);
            MenuItems.Add(nrtRisk);
        }
        else
        {
            // Remove old risk menu

            foreach (SecMenuItem item in MenuItems)
            {
                if (item.DisplayType == "5")
                {
                    if (arrRemoveItems != null && arrRemoveItems.Contains(item.Url))
                    {
                        removeMenu.Add(item);
                        parentRemoveItem = item.Parent;
                    }
                    else
                    {
                        item.Url = item.Url.Replace(GeneralFuncsLib.RISK_OLD_URL_RULE, GeneralFuncsLib.RISK_MCF_URL_RULE);
                    }
                }
            }

            // Remove menu not used for MCF
            foreach (SecMenuItem removeItem in removeMenu)
            {
                MenuItems.Remove(removeItem);
            }
        }

        // Remove Parent item if have not child item.
        bool isRemoveParent = true;
        SecMenuItem itemParent = new SecMenuItem();
        foreach (SecMenuItem itemMenu in MenuItems)
        {
            if (itemMenu.Parent == parentRemoveItem) isRemoveParent = false;
            if (itemMenu.SiteMapId == parentRemoveItem)
            {
                itemParent = itemMenu;
            }
        }

        if (isRemoveParent)
        {
            MenuItems.Remove(itemParent);
        }

    }

    private void RemoveMenu1099(SecMenuItemCollection MenuItems)
    {
        SecMenuItem itemParent = new SecMenuItem();
        bool isRemove = false;
        foreach (SecMenuItem itemMenu in MenuItems)
        {
            if (itemMenu.Permissions.Equals(WebSiteConstants.SEC_PERMISSION_SITE_JUMP_1099K))
            {
                isRemove = true;
                itemParent = itemMenu;
                break;
            }
        }

        if (isRemove)
            MenuItems.Remove(itemParent);
    }

    private void FillNodeChild(int parent, SecMenuItemCollection nrtRisk)
    {
        foreach (SecMenuItem item in nrtRisk)
        {
            if (item.Parent == parent)
            {
                int siteMapId = item.SiteMapId;
                item.Parent = parent + maxSiteMapID;
                item.SiteMapId = item.SiteMapId + maxSiteMapID;
                item.Url = item.Url.Replace(GeneralFuncsLib.RISK_OLD_URL_RULE, GeneralFuncsLib.RISK_MCF_URL_RULE);
                FillNodeChild(siteMapId, nrtRisk);
            }
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        uxSiteAccessLink.Attributes["onclick"] = string.Format("javascript:ShowPopupModal('{0}', 500, 300);", ResolveUrl("~/SiteJump.aspx"));
        uxOpenNewCase.Visible = _page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_CM_OPEN_CASE);
        uxMyCase.Visible = _page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_CM_MY_CASES);
        uxPanelCase.Visible = uxOpenNewCase.Visible || uxMyCase.Visible;
        uxPlhMPAList.Visible = _page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_VIEW_MPA_LIST) || _page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_VIEW_MPA_LIST_MS);
        uxPlhTaskList.Visible = _page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_VIEW_TASK_LIST) || _page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_VIEW_TASK_LIST_MS);
        uxPlAddNewMerchant.Visible = _page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_ADDNEWMERCHANT);

        if (_page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_USER_PROF) || _page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_USER_PROF_MS))
        {
            uxUserInfoWithoutLink.Visible = false;
            uxUserProfile.Visible = true;
        }
        else
        {
            uxUserInfoWithoutLink.Visible = true;
            uxUserProfile.Visible = false;
        }

        uxPnlSiteAccess.Visible = _page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_JSACCESS);


        if (_page.IsStaticQueryString)
        {
            string queryString = _page.Cryptor.DecryptText(Request["sparam"], SecurePage.STATIC_KEY);
            string openType = Request["OpenType"].ToSafeString();
            string desUrl = _page.SecureQueryString["DesURL"];
            if (openType.Equals("OpenPopup", StringComparison.OrdinalIgnoreCase))
            {
                string script = string.Format("openPopupWindow('{0}&{1}', 'OpenNewCase');", desUrl, queryString);
                _page.ClientScript.RegisterStartupScript(GetType(), "openAlert", script, true);
            }
            else
            {
                Response.Redirect(desUrl + "&" + queryString);
            }
        }

    }

    protected void drpLanguage_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Context.Items["InHeaderLoop"] == null)// check to avoid infinite loop when execute ~/header.aspx in SessionManager.GetHeaderMenu()
        {
            GeneralFuncsLib.ResetSessionLanguage();
            SessionManager.CurrentLanguage = int.Parse(drpLanguage.SelectedValue);
            GeneralFuncsLib.ReloadSession();
            User user = SessionManager.CurrentUser;
            if (user != null)
            {
                WebServices.SecurityServices.UpdateUserLanguageID(user.RecId, user.ASClient, user.SiteID, int.Parse(drpLanguage.SelectedValue));
            }

            SessionManager.GetHeaderMenu();
            if (Request.RawUrl.Contains("/header.aspx"))
            {
                Response.Redirect(Request.UrlReferrer.OriginalString);
            }
            else
            {
                Response.Redirect(Request.RawUrl);
            }
        }


    }

    private void BindDropdownListLanguage()
    {
        string path = "~/App_Data/Country.xml";
        StreamReader sr = new StreamReader(Server.MapPath(path));
        drpLanguage.LoadXml(sr.ReadToEnd());
        drpLanguage.SelectedValue = SessionManager.CurrentLanguage.ToString();
    }

    private string BuildRedirectUrl(string url)
    {
        string path = ResolveUrl("~/freeaccess/Redirect.aspx");
        return string.Format("{0}?{1}", path, ((SecurePage)Page).BuildSecureQueryString("Url=" + url));
    }

    #region For PCI Reskin - Should removed when PCI Reskin done
    private static readonly string[] PCI_PERMISSIONS = new string[] { "SiteAccessPCIAdmin", "HierarchySiteAccessPCIAdmin", "MerchantSiteAccessPCIAdmin" };
    /// <summary>
    /// Add more PCI Reskin menu item if user has PCI access permissions
    /// </summary>
    /// <param name="menuItems"></param>
    private void AddPCIReskinMenu(SecMenuItemCollection menuItems)
    {
        if (menuItems == null || menuItems.Count == 0) return;

        foreach (SecMenuItem item in menuItems)
        {
            if (PCI_PERMISSIONS.Contains(item.Permissions, StringComparer.InvariantCultureIgnoreCase))
            {
                var newSitemapId = 9999;
                var newUrl = string.Format("{0}?{1}", item.Url, ((SecurePage)this.Page).BuildSecureQueryString("res=1"));
                var mewTitle = string.Format("{0} - Reskin", item.Title);
                SecMenuItem pciItem = new SecMenuItem(newSitemapId, item.SystemId, newUrl, mewTitle, item.Description, item.Permissions, item.Parent);
                pciItem.NodeOrder = item.NodeOrder;
                pciItem.DisplayType = item.DisplayType;
                menuItems.Add(pciItem);
                break;
            }
        }
    }

    /// <summary>
    /// Check if current user can see PCI Reskin menu
    /// </summary>
    /// <returns></returns>
    private bool CheckUserCanSeePCIReskinMenu()
    {
        var config = ConfigurationManager.AppSettings["Reskin_PCI_Sitejump_Users"];
        if (string.IsNullOrEmpty(config)) return false;

        //Allow all users having PCI permission can access PCI reskin url
        if (config.Equals("all")) return true;

        var users = config.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
        var current = string.Format("{0}-{1}", SessionManager.CurrentClient, SessionManager.CurrentUser.UserID);

        return users.Any(u => u.Equals(current, StringComparison.InvariantCultureIgnoreCase));
    }
    #endregion
}
