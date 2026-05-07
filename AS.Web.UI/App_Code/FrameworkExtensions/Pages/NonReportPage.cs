using System;
using System.Web;
using System.Web.Security;
using AS.Controls.Pages;
using AS.Security.WS.Entities;
using AS.Controls.Global;
using System.Threading;
using System.Globalization;
using AS.Web.SharedSession;

/// <summary>
/// Summary description for NonReportPage
/// </summary>
public class NonReportPage : SecurePage
{
    public bool IsModal
    {
        get { return PageType == SecurePageType.Modal; }
    }

    public NonReportPage()
    {
        //
        // TODO: Add constructor logic here
        //
        this.ASPXTrackingLog = new AspxTracking();
        this.IntruderLog = new Intruders();
        this.DefaultTheme = "Default";
        this.ValidPagesForForceResetPassword = ",manageprofile.aspx,";
        this.ResetPasswordPage = "~/ManageProfile.aspx";
    }
    public void AjaxAddResponseScript(string script)
    {
        ((BaseMasterPage)Master).AjaxAddResponseScript(script);
    }
    public override AS.Common.WebUI.ICryptor Cryptor
    {
        get { return CryptorServices.Current; }
    }


    public void ShowServerErrorMessage(ValidatorMessage val, String customMessage = null)
    {
        if (customMessage != null)
        {
            val.Message = AS.Common.VeraCodeSolution.DoVeraCode(customMessage);
        }
        val.ShowOnLoad = true;
    }


    protected override void DoIntruderDetected(IntruderType type)
    {
        switch (type)
        {
            case IntruderType.Permission:
                if (type == IntruderType.Permission)
                {
                    if (User.Identity.IsAuthenticated && SessionManager.IsLoggedIn)
                    {
                        string rootURL = this.ResolveUrl("~");
                        Response.Redirect(rootURL + "403.aspx");
                    }
                    else
                    {
                        Session.Abandon();
                        Session.Clear();
                        
                        Response.Redirect(LoginUrl);
                    }
                }
                break;
            default:
                {


                    Session.Clear();
                    Session.Abandon();
                    HttpContext.Current.Response.Cookies.Clear();

                    FormsAuthentication.SignOut();

                    //Re-new SessionId support for boarding/ case management
                    HttpContext.Current.Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));

                    if (this.LoginUrl == "[jumpsite]")
                    {
                        HttpContext.Current.Response.Cookies.Add(new HttpCookie("logout_opt", "3"));
                        HttpContext.Current.Response.Redirect(FormsAuthentication.LoginUrl);
                    }
                    else
                    {
                        if(string.IsNullOrEmpty(this.LoginUrl))
                        {
                            HttpContext.Current.Response.Cookies.Add(new HttpCookie("logout_opt", "3"));
                            HttpContext.Current.Response.Redirect(FormsAuthentication.LoginUrl);
                        }
                        else
                        {
                            HttpContext.Current.Response.Redirect(this.LoginUrl);
                        }
                    }

                }
                break;
        }
    }

    protected override void OnLoad(EventArgs e)
    {
        this.Form.SetCssClass();
        SharedProvider.UpdateLastActive();
        base.OnLoad(e);
    }
    protected override void PageInitialize()
    {
        if (GeneralFuncsLib.IsIFrameSupported())
        //{
            PageType = SecurePageType.None;
        //}
    }

    public override void ProcessRequest(HttpContext context)
    {
        AS.Web.SharedSession.SharedProvider.RedirectLoginUrlWhenTimeout();

        base.ProcessRequest(context);
    }

    protected override void InitializeCulture()
    {
        string selectedLanguage = string.Empty;
        if (SessionManager.CurrentLanguage == (int)WebSiteEnums.LanguageCode.English)
        {
            selectedLanguage = WebSiteConstants.USCulture;
        }
        else if (SessionManager.CurrentLanguage == (int)WebSiteEnums.LanguageCode.Spanish)
        {
            selectedLanguage = WebSiteConstants.SpanishCulture;
        }
        else
        {
            // default is English
            selectedLanguage = WebSiteConstants.USCulture;
        }

        Thread.CurrentThread.CurrentUICulture = new CultureInfo(selectedLanguage);
        base.InitializeCulture();
    }
    protected override void OnAspxWriteLog()
    {

        if (User.Identity.IsAuthenticated && SessionManager.IsLoggedIn)
        {
            ASPXTrackingLog.LogClientId = SessionManager.CurrentClient;
            ASPXTrackingLog.LogSystemId = SessionManager.CurrentSystem;
            ASPXTrackingLog.LogFullName = SessionManager.CurrentUser.UserNameFull;
            ASPXTrackingLog.LogId1 = SessionManager.CurrentUser.UserID;
            ASPXTrackingLog.LogId2 = SessionManager.SiteJumper;
            ASPXTrackingLog.LogData4 = SessionManager.JumpFrom;


        }
        else
        {
            ASPXTrackingLog.LogClientId = WebSiteSettings.DefaultClient;
            ASPXTrackingLog.LogSystemId = WebSiteSettings.DefaultSystem;
        }

        
        int SuspectedBotCode = WebServices.LogServices.InsertASPXTrackingLog(ASPXTrackingLog);

        if (SuspectedBotCode == 1)
        {
            Session.Abandon();
            Session.Clear();
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage(LoginUrl);
        }
    }
    protected override void OnIntruderWriteLog()
    {
        if (User.Identity.IsAuthenticated && SessionManager.IsLoggedIn)
        {
            ASPXTrackingLog.LogClientId = SessionManager.CurrentClient;
            ASPXTrackingLog.LogSystemId = SessionManager.CurrentSystem;
            IntruderLog.LogFullName = SessionManager.CurrentUser.UserNameFull;
            IntruderLog.LogId1 = SessionManager.CurrentUser.UserID;
            IntruderLog.LogId2 = SessionManager.SiteJumper;
            ASPXTrackingLog.LogData4 = SessionManager.JumpFrom;
            
        }
        else
        {
            ASPXTrackingLog.LogClientId = WebSiteSettings.DefaultClient;
            ASPXTrackingLog.LogSystemId = WebSiteSettings.DefaultSystem;
        }
        WebServices.SecurityServices.InsertIntruderLog(IntruderLog);
    }
    #region Common functions

    protected virtual void OnDataBindControls(Enum type, object sender) { }
    protected void OnDataBindControls(Enum type) { OnDataBindControls(type, null); }
    protected virtual void OnPostBackActions(Enum type, object sender) { }
    protected void OnPostBackActions(Enum type) { OnPostBackActions(type, null); }
    protected virtual bool OnValidateInputs(Enum type, object sender) { return true; }
    protected bool OnValidateInputs(Enum type) { return OnValidateInputs(type, null); }

    #endregion

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        GeneralFuncsLib.RegisterHtmlMeta(this);        
    }
    protected override void OnPreRenderComplete(EventArgs e)
    {
        base.OnPreRenderComplete(e);
        bool isModal = (PageType == SecurePageType.Modal) ? true : false;
        GeneralFuncsLib.RegisterCssFile(this, isModal);
    }
    public SecurePage Page
    {
        get
        {
            return (SecurePage)base.Page;
        }
    }
}
