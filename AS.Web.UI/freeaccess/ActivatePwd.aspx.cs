using AS.Common.DBManager;
using AS.Controls.Pages;
using AS.Security.WS.Entities;
using AS.Web.SharedSession;
using System;
using System.Data;
using System.Web;
using System.Web.Security;


public partial class ActivatePwd : NonReportPage
{

    const int LOGIN_SUCCESSFULL = 1;
    const int LOGIN_PASS_EXPIRED = 9;
    const int LOGIN_WITH_TEMP = 100;
    const int LOGIN_PASS_NEARLY_EXPIRED = 10;
    public SecurePage _page;
    protected string _loginResFolder = "";
    protected bool MultiLanguage
    {
        get
        {
            return GeneralFuncsLib.GetDataOfExtendedSetting("MultiLanguage").ToLower().Equals("true") ? true : false;
        }
    }

    protected string ClienLoginPage
    {
        get
        {
            return GeneralFuncsLib.GetDataOfExtendedSetting("LOGIN_PAGE");
        }
    }

    protected string userName
    {
        get
        {
            _page = (SecurePage)Page;
            if (!string.IsNullOrEmpty(_page.SecureQueryString["userName"]))
            {
                return _page.SecureQueryString["userName"];
            }
            else
            {
                return string.Empty;
            }
        }

    }
    protected string passWord
    {
        get
        {
            _page = (SecurePage)Page;
            if (!string.IsNullOrEmpty(_page.SecureQueryString["passWord"]))
            {
                return _page.SecureQueryString["passWord"];
            }
            else
            {
                return string.Empty;
            }
        }
    }

    protected int clientId
    {
        get
        {
            _page = (SecurePage)Page;
            if (!string.IsNullOrEmpty(_page.SecureQueryString["clientId"]))
            {
                return Convert.ToInt32(_page.SecureQueryString["clientId"]);
            }
            else
            {
                return -1;
            }
        }
    }

    protected override void DoPagePreInit()
    {
        //LogOut();
        base.DoPagePreInit();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
        }

    }

    private void LogOut()
    {
        Session.Clear();
        SessionManager.CurrentClient = clientId;
        Session.Abandon();
        FormsAuthentication.SignOut();
    }
    private void Login()
    {
        _page = (SecurePage)Page;
        Session.Clear();
        Response.Cookies.Clear();

        if (clientId != -1)
        {
            SessionManager.CurrentClient = clientId;
        }
        else
        {
            return;
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

        User user = WebServices.SecurityServices.GetUser(SessionManager.CurrentClient, userName);
        //get user type
        if (user == null)
        {
            //uxError.Text = ERROR_MSG;
            return;
        }

        SessionManager.UniqueSessionID = Guid.NewGuid().ToString();

        int loginResult = WebServices.SecurityServices.ValidateUser(SessionManager.CurrentClient, userName, passWord, SessionManager.UniqueSessionID, Session.SessionID);

        loginResult = loginResult >> 24;

        if ((loginResult == LOGIN_SUCCESSFULL) || (loginResult == LOGIN_PASS_NEARLY_EXPIRED) || (loginResult == LOGIN_WITH_TEMP))
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

                this.LoginUrl = ClienLoginPage;

                string redirectUrl = string.Empty;

                SessionManager.PasswordTemporary = passWord;
                SessionManager.PasswordExpired = ((loginResult == LOGIN_PASS_EXPIRED));
                SessionManager.ForceChangePassword = true;
                SharedSessionManager.PasswordExpired = SessionManager.ForceChangePassword;

                redirectUrl = "~/ManageProfile.aspx";

                Response.Redirect(redirectUrl);
            }
            else
            {
                uxProgress.Visible = false;
                Session.Clear();
            }
        }
        else
        {
            uxProgress.Visible = false;

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

    protected void uxLogin_Click(object sender, EventArgs e)
    {
        Login();
    }

    protected void uxLogOut_Click(object sender, EventArgs e)
    {
        LogOut();
    }

}




