using System;
using System.Data;
using System.Configuration;
using System.Web;
using AS.Common.DBManager;
using AS.Security.WS.Entities;
using AS.Common;
using AS.Common.Logger;
using AS.Web.SharedSession;
using System.Web.UI.WebControls;

public partial class Page_Login : NonReportPage
{

    const int LOGIN_SUCCESSFULL = 1;
    const int LOGIN_PASS_EXPIRED = 9;
    const int LOGIN_WITH_TEMP = 100;
    const int LOGIN_PASS_NEARLY_EXPIRED = 10;
    const int LOGIN_ATTEMPTS_EXCEEDED = 6;
    const int LOGIN_FAIL_EXCEEDED = 11;
    const string QA_MODE_ENABLED = "1";
    private const string LOGOUT_OPT = "logout_opt";
    protected string _loginResFolder = "";
    private bool _IsShowContactInfo = true;
    private string _ClientContactInfo = "ClientInformation";
    const string FORGOT_REFTBLNAME = "ForgotInformation";
    private string _ForgotInformation = "ForgotInformation";
    protected bool IsTMSSiteJump = false;
    protected bool MultiLanguage
    {
        get
        {
            return GeneralFuncsLib.GetDataOfExtendedSetting("MultiLanguage").ToLower().Equals("true") ? true : false;
        }
    }
    protected string ClientEmail
    {
        get
        {
            if (ViewState["ContactEmail"] != null)
                return ViewState["ContactEmail"].ToString();
            else
                return string.Empty;
        }
        set { ViewState["ContactEmail"] = value; }
    }

    private void BindClientInfo()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.Add(new FilterParameter("@ASClient", SessionManager.CurrentClient, System.Data.DbType.Int32));
        parameters.Add(new FilterParameter("@ContactName", _ClientContactInfo, System.Data.DbType.AnsiString));
        DataTable clientInfo = WebServices.SecurityServices.GetReports("spa_GetContactUsInfo", parameters);

        if (clientInfo != null && clientInfo.Rows.Count > 0)
        {
            var clientId = (int)clientInfo.Rows[0]["ASClientID"];

            uxCSHeader.Text = VeraCodeSolution.DoVeraCode(clientInfo.Rows[0]["RefTblCol1"].ToString());
            var isHeaderNote = GeneralFuncsLib.GetDataOfExtendedSetting("LOGIN_SHOW_CONTACT_MESSAGE_NOTE_HEADER").Contains(WebSiteSettings.WebSiteType.ToUpper());   
            if (isHeaderNote)
            {
                uxHeaderNote.Text = GeneralFuncsLib.GetDataOfExtendedSetting("LOGIN_CONTACT_MESSAGE_NOTE_HEADER");
                uxHeaderNotePanel.Visible = true;
            }
            uxCompanyName.Text = VeraCodeSolution.DoVeraCode(clientInfo.Rows[0]["RefTblCol10"].ToSafeString());
            uxPanelCompanyName.Visible = GeneralFuncsLib.GetDataOfExtendedSetting("LOGIN_SHOW_COMPANY_NAME").Contains(WebSiteSettings.WebSiteType.ToUpper());

            uxFax.Text = VeraCodeSolution.DoVeraCode(clientInfo.Rows[0]["RefTblCol6"].ToSafeString());
            uxPanelFax.Visible = GeneralFuncsLib.GetDataOfExtendedSetting("LOGIN_SHOW_FAX").Contains(WebSiteSettings.WebSiteType.ToUpper());

            bool isHideAddr01 = GeneralFuncsLib.GetDataOfExtendedSetting("CLIENT_INFO_HIDE_ADDRESS_1").Equals("true") ? true : false;
            bool isHideAddr02 = GeneralFuncsLib.GetDataOfExtendedSetting("CLIENT_INFO_HIDE_ADDRESS_2").Equals("true") ? true : false;
            bool isHideAddr03 = GeneralFuncsLib.GetDataOfExtendedSetting("CLIENT_INFO_HIDE_ADDRESS_3").Equals("true") ? true : false;
            bool isHideAddrMS = GeneralFuncsLib.GetDataOfExtendedSetting("CLIENT_INFO_HIDE_ADDRESS_MS").Equals("true") ? true : false;
            if (isHideAddrMS && WebSiteSettings.WebSiteType.ToLower() == "ms")
            {
                uxHideAddress.Visible = false;
            }
            else
            {
                uxCSAddr01.Text = VeraCodeSolution.DoVeraCode(clientInfo.Rows[0]["RefTblCol2"].ToString());
                uxCSAddr03.Text = VeraCodeSolution.DoVeraCode(clientInfo.Rows[0]["RefTblCol4"].ToString());
                uxCSPhone.Text = VeraCodeSolution.DoVeraCode(clientInfo.Rows[0]["RefTblCol5"].ToString());

                if (isHideAddr01)
                {
                    uxCSAddr01.Visible = false;
                }
                if (isHideAddr02)
                {
                    uxAdd02Info.Visible = false;
                }
                else
                {
                    uxAdd02Info.Visible = true;
                    uxCSAddr02.Text = VeraCodeSolution.DoVeraCode(clientInfo.Rows[0]["RefTblCol3"].ToString());
                }
                if (isHideAddr03)
                {
                    uxCSAddr03.Visible = false;
                }
            }
            // displaying additional contact numbers
            var isHidePhoneFinancial = GeneralFuncsLib.GetDataOfExtendedSetting("LOGIN_HIDE_CONTACT_PHONE_FINANCIAL").Equals("true") ? true : false;
            uxPhoneNumber.Visible = GeneralFuncsLib.GetDataOfExtendedSetting("LOGIN_SHOW_CONTACT_PHONE").Contains(WebSiteSettings.WebSiteType.ToUpper());           
            if (uxPhoneNumber.Visible && !isHidePhoneFinancial)
            {
                uxFinancialIntitutions.Text = VeraCodeSolution.DoVeraCode("Financial Institutions: " + clientInfo.Rows[0]["FinancialIntitutions"].ToString());
                uxDirectMerchants.Text = VeraCodeSolution.DoVeraCode("Direct Merchants: " + clientInfo.Rows[0]["DirectMerchants"].ToString());
            }

            uxContactMessage.Visible = GeneralFuncsLib.GetDataOfExtendedSetting("LOGIN_SHOW_CONTACT_MESSAGE").Contains(WebSiteSettings.WebSiteType.ToUpper());
            if (uxContactMessage.Visible)
            {
                string configMessage = GeneralFuncsLib.GetDataOfExtendedSetting("LOGIN_CONTACT_MESSAGE_" + WebSiteSettings.WebSiteType.ToUpper());
                if ((int)clientInfo.Rows[0]["ASClientID"] == WebSiteConstants.SPHERE_CLIENT && WebSiteSettings.WebSiteType.ToLower() == "ms")
                {
                    configMessage += GeneralFuncsLib.GetDataOfExtendedSetting("LOGIN_CONTACT_MESSAGE_NOTE");
                }
                this.uxMes.Text = VeraCodeSolution.DoVeraCode(string.Format(configMessage, clientInfo.Rows[0]["RefTblCol1"].ToString()));
            }

            uxContactEmail.Visible = GeneralFuncsLib.GetDataOfExtendedSetting("LOGIN_SHOW_CONTACT_EMAIL").Contains(WebSiteSettings.WebSiteType.ToUpper());
            if (uxContactEmail.Visible)
            {
                this.ClientEmail = VeraCodeSolution.DoVeraCode(clientInfo.Rows[0]["RefTblCol7"].ToString());
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        // #45589: Fix issue: redirect to right login url when logout/session timeout
        //-----------------
        // Request["readCookieLoginUrl"].ToBoolean():                                   : get login url from cookie
        // !this.LoginUrl.Contains(System.Web.Security.FormsAuthentication.LoginUrl)    : ignore case first access in to our system
        // !this.LoginUrl.Contains("[jumpsite]")                                        : ignore case access by SSO/Sitejump
        //--------------------

        bool isMaintenanceMode = false;
        ShowHideMaintenanceMode(ref isMaintenanceMode);

        if (isMaintenanceMode)
            return;


        bool readCookieLoginUrl = false;
        Boolean.TryParse(Request["readCookieLoginUrl"], out readCookieLoginUrl);

        if (readCookieLoginUrl
            && !string.IsNullOrEmpty(this.LoginUrl)
            && !this.LoginUrl.Contains(System.Web.Security.FormsAuthentication.LoginUrl)
            && !this.LoginUrl.Contains("[jumpsite]"))
        {
            Response.Redirect(this.LoginUrl);
        }

        PageType = SecurePageType.None;
        ERROR_MSG = GetLocalResourceObject("Login_aspx_cs_InvalidUsernamePassword").ToString();

        if (Request["id"].IsNullOrEmpty())
        {
            if (Request.Cookies[LOGOUT_OPT] != null && (Request.Cookies[LOGOUT_OPT].Value == "2" || Request.Cookies[LOGOUT_OPT].Value == "3"
                || Request.Cookies[LOGOUT_OPT].Value.Equals(((int)WebSiteEnums.LOGOUT_MODE.SSO).ToString(), StringComparison.OrdinalIgnoreCase)
                ))
            {
                if (Request.Cookies[LOGOUT_OPT].Value == "2"
                || Request.Cookies[LOGOUT_OPT].Value.Equals(((int)WebSiteEnums.LOGOUT_MODE.SSO).ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    string script = "$('body').append('<div class=\"loading-full-bg\">Loading...</div>')";
                    Page.ClientScript.RegisterStartupScript(GetType(), "showLoading", script, true);
                }
                SessionManager.CurrentClient = WebSiteSettings.DefaultClient;
            }
            else
            {
                Response.Redirect("~/freeaccess/404.htm");
            }
        }
        else
        {
            if (this.LoginUrl != "[jumpsite]")
            {
                SessionManager.CurrentClient = int.Parse(Request["id"]);
            }

            IsTMSSiteJump = SamlHelper.IsSsoMode(this);
        }

        var redirectURL = GeneralFuncsLib.GetDataOfExtendedSetting("SSO_Redirecting_To_URL");
        if (!string.IsNullOrEmpty(redirectURL))
        {
            Response.Redirect(redirectURL);
        }

        this.ForcePostbackValidation = false;
        if (!Request["ins"].IsNullOrEmpty())
        {
            uxInstructions.Visible = true;
            uxInstructions.ResourceKey = Request["ins"];
        }
        else
        {
            uxInstructions.Visible = false;
        }
        if (Request["theme"].IsNullOrEmpty())
        {
            _loginResFolder = "default";
        }
        else
        {
            _loginResFolder = Request["theme"];
        }

        ////Get Theme
        SessionManager.CurrentUserTheme = new ASTheme(1, _loginResFolder, "", "", DateTime.Now, "1");

        // get custom contact information
        if (Request["contact"].IsNullOrEmpty())
        {
            _ClientContactInfo = "ClientInformation";
        }
        else
        {
            _ClientContactInfo = "ClientInformation_" + Request["contact"];
        }
        if (Request["forgot"].IsNullOrEmpty())
        {
            _ForgotInformation = FORGOT_REFTBLNAME;
        }
        else
        {
            _ForgotInformation = FORGOT_REFTBLNAME + "_" + Request["forgot"];
        }
        this.txtUserName.Attributes["onkeypress"] = "EnterSignIn(event)";
        this.txtPassword.Attributes["onkeypress"] = "EnterSignIn(event)";


        _IsShowContactInfo = !GeneralFuncsLib.GetDataOfExtendedSetting("LOGIN_HIDE_CONTACT_INFO").Contains(WebSiteSettings.WebSiteType.ToUpper());
        uxContactInfoPanel.Visible = _IsShowContactInfo;
        HandleTotalLoginPages();
        BindClientInfo();

        if (GeneralFuncsLib.ShowEnvironmentIndicator(true))
        {
            uxPanelLoginEnvironment.Visible = true;
            uxCurrentEnvironment.Text = string.Format("{0} {1}", GeneralFuncsLib.GetCurrentEnvironment(), GetGlobalResourceObject("LanguageResource", "Login_Environment_Label").ToString());
        }
        else
        {
            uxPanelLoginEnvironment.Visible = false;
        }

    }

    string ERROR_MSG = string.Empty;
    protected void uxLogin_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Response.Cookies.Clear();
        if (!Request["contactus"].IsNullOrEmpty())
        {
            SessionManager.ClientContactInforKey = "ClientInformation_" + Request["contactus"];
        }
        else
        {
            SessionManager.ClientContactInforKey = "ClientInformation";
        }
        if (Request["id"].IsNullOrEmpty())
        {
            SessionManager.CurrentClient = WebSiteSettings.DefaultClient;
        }
        else
        {
            SessionManager.CurrentClient = int.Parse(Request["id"]);
        }
        if (Request["theme"].IsNullOrEmpty())
        {
            _loginResFolder = "default";
        }
        else
        {
            _loginResFolder = Request["theme"];
        }

        ////Get Theme
        SessionManager.CurrentUserTheme = new ASTheme(1, _loginResFolder, "", "", DateTime.Now, "1");

        User user = WebServices.SecurityServices.GetUser(SessionManager.CurrentClient, txtUserName.Text);
        //get user type
        if (user == null)
        {
            uxError.Text = ERROR_MSG;
            return;
        }

        SessionManager.UniqueSessionID = Guid.NewGuid().ToString();

        int loginResult = WebServices.SecurityServices.ValidateUser(SessionManager.CurrentClient, txtUserName.Text, txtPassword.Text, SessionManager.UniqueSessionID, Session.SessionID);

        int NumOfDayLeft = loginResult & 0xFFFF;
        loginResult = loginResult >> 24;

        if ((loginResult == LOGIN_SUCCESSFULL) || (loginResult == LOGIN_PASS_EXPIRED) || (loginResult == LOGIN_PASS_NEARLY_EXPIRED) || (loginResult == LOGIN_WITH_TEMP))
        {
            if (GeneralFuncsLib.SaveLoginUserData(user.UserID, this))
            {
                if (MultiLanguage)
                {
                    // Get Current language
                    SessionManager.CurrentLanguage = WebServices.SecurityServices.GetUserLanguageID(user.RecId, user.ASClient, user.SiteID);
                }

                //Check Use primary prefix
                SessionManager.UsePrimaryPrefix = CheckUsePrimaryPrefix();
                if (SessionManager.CurrentUserType.ToString() == WebSiteEnums.UserHierarchyMode.Hierarchy.ToString())
                {
                    GetHierarchy();
                }

                LoginUrl = Request.RawUrl;
                //44594 - VW - Session time out issue - Short term
                SharedSessionManager.USER_LOGIN_URL = LoginUrl;

                string redirectUrl = string.Empty;
                if ((loginResult == LOGIN_WITH_TEMP) || (loginResult == LOGIN_PASS_EXPIRED))
                {

                    SessionManager.PasswordExpired = ((loginResult == LOGIN_PASS_EXPIRED));
                    SessionManager.ForceChangePassword = true;
                    SharedSessionManager.PasswordExpired = SessionManager.ForceChangePassword;
                    GeneralFuncsLib.SetCookie("is_open_recurring_system_message", "False");
                    redirectUrl = "~/ManageProfile.aspx";
                }
                else
                {
                    if (loginResult == LOGIN_PASS_NEARLY_EXPIRED)
                    {
                        SessionManager.PasswordExpiredNearly = true;
                        SessionManager.DayRemainingPasswordExpired = NumOfDayLeft;
                        GeneralFuncsLib.SetCookie("is_open_recurring_system_message", "False");
                        redirectUrl = "~/ManageProfile.aspx";
                    }
                    else if (!Request["tos"].IsNullOrEmpty() && Request["tos"].ToString().Trim().Equals("1")
                        && (IsUserWithPermission("StatementRpt") || IsUserWithPermission("MSStatementRpt")))
                    {
                        SessionManager.OpenStatementDetailPopup = true;
                        redirectUrl = "~/Statement.aspx";
                    }
                    else
                    {
                        redirectUrl = GeneralFuncsLib.GetDefaultPageOfLoggedInUser((AS.Controls.Pages.SecurePage)this.Page);
                    }
                    if (!Request["sparam"].IsNullOrEmpty())
                    {
                        redirectUrl += "?" + Request.QueryString.ToSafeString();
                    }
                }

                //Save Timezone
                if (SessionManager.CurrentClient != WebSiteConstants.WRFC_CLIENT)
                {
                    HiddenField uxHddDayLightSaving = (HiddenField)Master.FindControl("uxHddDayLightSaving");
                    HiddenField uxHddClientTimezone = (HiddenField)Master.FindControl("uxHddClientTimezone");
                    if (!string.IsNullOrEmpty(uxHddClientTimezone.Value) && !string.IsNullOrEmpty(uxHddClientTimezone.Value))
                    {
                        TimeZoneHandler.SaveTimeZone(null, string.Empty, uxHddClientTimezone.Value, uxHddDayLightSaving.Value.ToInt());
                    }
                }

                //Reload header menu 
                SessionManager.GetHeaderMenu();
                SharedSessionManager.UserDefaultPage = redirectUrl;
                //Sprint 6 - 46652 - AW Multi-currency Transaction Display
                GeneralFuncsLib.SetDefaultCurrency();
                Response.Redirect(redirectUrl);
            }
            else
            {
                Session.Clear();
                uxError.Text = ERROR_MSG;
            }
        }
        else
        {
            var maxLoginAttempts = 3;
            if (loginResult == LOGIN_ATTEMPTS_EXCEEDED)
            {
                RefTableValueCollection refs = WebServices.SecurityServices.GetRefTableValues(user.ASClient, "MaxLoginAttempts", "END", 1);
                foreach (RefTableValue item in refs)
                {
                    Int32.TryParse(item.RefTblKey.ToString(), out maxLoginAttempts);
                    break;
                }
            }

            if (loginResult == LOGIN_FAIL_EXCEEDED || (loginResult == LOGIN_ATTEMPTS_EXCEEDED && user.LoginAttempts == maxLoginAttempts))
            {
                string urlParameter = "?u=" + Server.UrlEncode(WebServices.SecurityServices.EncryptText(user.UserID)) + "&a=" + HttpUtility.UrlEncode(AS.Common.DataProtection.Cryptophy.EncryptText("f"));
                string urlEmail = Request.Url.GetLeftPart(UriPartial.Authority) + Request.RawUrl + urlParameter;

                System.IO.StreamReader mailTemplate = new System.IO.StreamReader(MapPath(@"~\App_Data\tpl_EmailForgotPassword.htm"));
                string mail_body = mailTemplate.ReadToEnd();
                mailTemplate.Close();
                mail_body = mail_body.Replace("[tpl_EmailForgotPassword_htm_Title]", Resources.Template.tpl_EmailForgotPassword_htm_Title);
                mail_body = mail_body.Replace("[tpl_EmailForgotPassword_htm_Message1]", Resources.Template.tpl_EmailForgotPassword_htm_Message1);
                mail_body = mail_body.Replace("[tpl_EmailForgotPassword_htm_Message2]", Resources.Template.tpl_EmailForgotPassword_htm_Message2);
                mail_body = mail_body.Replace("[tpl_EmailForgotPassword_htm_Message3]", Resources.Template.tpl_EmailForgotPassword_htm_Message3);
                mail_body = mail_body.Replace("[LinkForgotPassword]", urlEmail);
                mail_body = string.Format(mail_body, GetMerchantServicesValue());

                string fromEmail = GetFromEmail();
                if (!string.IsNullOrEmpty(fromEmail))
                {
                    AS.Common.Mail.SmtpMail.SendEmail(fromEmail, user.Email, GetLocalResourceObject("Login_aspx_cs_MailSubject").ToString(), mail_body, WebSiteSettings.MailSettings);
                }
                else
                {
                    AS.Common.Logger.LoggerManager.Debug("From email is empty");
                }
            }
            else
            {
                uxError.Text = ERROR_MSG;
            }
        }
    }

    private string GetFromEmail()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.Add(new FilterParameter("@ASClient", SessionManager.CurrentClient, System.Data.DbType.Int32));
        parameters.Add(new FilterParameter("@ContactName", _ClientContactInfo, System.Data.DbType.AnsiString));
        DataTable clientInfo = WebServices.SecurityServices.GetReports("spa_GetContactUsInfo", parameters);
        if (clientInfo != null && clientInfo.Rows.Count > 0)
        {
            return clientInfo.Rows[0]["RefTblCol9"].ToString();
        }
        return WebSiteSettings.NoReplyEmail;
    }

    private string GetMerchantServicesValue()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.Add(new FilterParameter("@ASClient", SessionManager.CurrentClient, System.Data.DbType.Int32));
        parameters.Add(new FilterParameter("@ContactName", _ForgotInformation, System.Data.DbType.AnsiString));
        DataTable forgotInfo = WebServices.SecurityServices.GetReports("spa_GetContactUsInfo", parameters);
        if (forgotInfo != null && forgotInfo.Rows.Count > 0)
        {
            if (WebSiteSettings.WebSiteType.ToUpper() == "CS")
            {
                if (forgotInfo.Rows[0]["RefTblCol5"].ToString().ToLower() == "true") // include URL in the email as a regular text
                {
                    return forgotInfo.Rows[0]["RefTblCol1"].ToString() + (string.IsNullOrEmpty(forgotInfo.Rows[0]["RefTblCol2"].ToString()) ? string.Empty : " - " + forgotInfo.Rows[0]["RefTblCol2"].ToString());
                }
                else
                {
                    return string.Format("<a href=\"{0}\">{1}</a>", forgotInfo.Rows[0]["RefTblCol2"].ToString(), forgotInfo.Rows[0]["RefTblCol1"].ToString());
                }
            }
            else if (WebSiteSettings.WebSiteType.ToUpper() == "MS")
            {
                return string.Format("<a href=\"{0}\">{1}</a>", forgotInfo.Rows[0]["RefTblCol4"].ToString(), forgotInfo.Rows[0]["RefTblCol3"].ToString());
            }
            else
            {
                return string.Empty;
            }
        }
        return string.Empty;
    }

    /// <summary>
    /// Show/Hide Maintenance Mode
    /// </summary>
    private void ShowHideMaintenanceMode(ref bool isMaintenanceMode)
    {

        if (Request.Params["MaintMode"] != null && Request.Params["MaintMode"].ToLower().Equals("on"))
        {
            uxMaintenanceInfo.Visible = false;
            uxLoginboxContainer.Visible = true;
            isMaintenanceMode = false;
        }
        else if (ConfigurationManager.AppSettings["MaintenanceMode"] != null && ConfigurationManager.AppSettings["MaintenanceMode"].ToLower().Equals("on"))
        {
            uxMaintenanceInfo.Visible = true;
            uxLoginboxContainer.Visible = false;
            isMaintenanceMode = true;
        }
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

    /// <summary>
    /// Handle specified business for Total login pages
    /// </summary>
    private void HandleTotalLoginPages()
    {
        if (SessionManager.CurrentClient != WebSiteConstants.TOTAL_CLIENT)
            return;

        var isQaMode = GeneralFuncsLib.GetDataOfExtendedSetting("IS_QA_ENVIRONMENT").Equals(QA_MODE_ENABLED);
        if (isQaMode)
        {
            uxWelcomeBarPanel.Visible = true;
            uxLogo.Style.Add("margin-top", "43px");
        }

        var hideContactInfoModes = GeneralFuncsLib.GetDataOfExtendedSetting("LOGIN_HIDE_CONTACT_INFO_COLUMN");
        if (!string.IsNullOrWhiteSpace(hideContactInfoModes) && hideContactInfoModes.Contains(WebSiteSettings.WebSiteType.ToUpper()))
        {
            uxMsLink.Visible = true;
            uxContactInfoColumn.Visible = false;
            uxLoginboxPanel.CssClass = "login-box single-col total total-cs";
        }
        uxLoginHeader.Visible = WebSiteSettings.WebSiteType.Equals(WebSiteConstants.WEBSITE_TYPE_CS, StringComparison.OrdinalIgnoreCase);
    }

    protected void uxGoToAccount_Click(object sender, EventArgs e)
    {
        //Store some values before redirect
        StoreSSOInfo();

        SamlHelper.SendAuthRequest(HttpContext.Current);
    }

    private void StoreSSOInfo()
    {
        //Add a cookie to store a value which indicates user accesses directly to VW MS login page
        HttpCookie cookie = new HttpCookie("sso_ms_url", Request.RawUrl);
        cookie.HttpOnly = true;
        cookie.Secure = System.Web.Security.FormsAuthentication.RequireSSL;
        HttpContext.Current.Response.Cookies.Add(cookie);
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string[] CheckSSOUser(string userId)
    {
        bool result = true;

        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.Add(new FilterParameter("@ASClient", SessionManager.CurrentClient, System.Data.DbType.Int32));
        parameters.Add(new AS.Common.DBManager.FilterParameter("@UserId", userId, DbType.AnsiString));
        FilterParameterCollection outParam = new FilterParameterCollection();
        parameters.Add(new AS.Common.DBManager.FilterParameter("@ReturnValue", false, DbType.Boolean, true));
        DataTable data = WebServices.SecurityServices.GetReports("spa_SEC_IsSSOUser", parameters);
        // need to review
        if (data.HasData())
        {

            bool.TryParse(data.Rows[0][0].ToString(), out result);
        }

        return new string[] { result.ToString().ToLower() };
        //return result;
    }

    #region Mobile Redirect handling
    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);

        var clientId = Request["id"] != null ? Convert.ToInt32(Request["id"]) : 0;
        MobileRedirectHandler.ProcessMobileRedirect(clientId);
    }
    #endregion
}
