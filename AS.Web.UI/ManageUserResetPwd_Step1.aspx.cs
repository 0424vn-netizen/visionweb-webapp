//----------------------------------------------------------------------------
// <copyright file="gen_ResetPasswordForCS1.aspx.cs" company="Data Delivery Services, Inc.">
//     Copyright (c) Data Delivery Services, Inc. All rights reserved.
// </copyright>
// <author>Hung Ho</author>
// <summary>Reset password function</summary>
//----------------------------------------------------------------------------

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
using AS.Security.WS.Entities;
using AS.Common.DBManager;
using AS.Common;

[PagePermission("ManUser,MSManUser,RstMerPwd,ASLandingPage,ResetSubHierarchyPassword")]
public partial class _mps_ResetPasswordForCS1 : NonReportPage
{
    private string RESET_PWD_TITLE = string.Empty;
    private string MESSAGE_INVALID_USER_ACCOUNT = string.Empty;
    private const string ALERT_SCRIPT = "setTimeout(\"alert('{0}');\", 500);";
    private SecurePage _page = null;
    private bool IsActivePwd
    {
        get
        {
            var IsActivePWDConfig = GeneralFuncsLib.GetDataOfExtendedSetting("IS_ACTIVE_PWD");
            if (!IsActivePWDConfig.IsNullOrEmpty())
                return IsActivePWDConfig.ToLower().Equals("true") ? true : false;

            return false;
        }
    }
    private string EmailNoReply
    {
        get { return (string)ViewState["EmailNoReply"]; }
        set { ViewState["EmailNoReply"] = value; }
    }

    private void BindClientInfo()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.Add(new FilterParameter("@ASClient", SessionManager.CurrentClient, System.Data.DbType.Int32));
        DataTable clientInfo = WebServices.SecurityServices.GetReports("spa_GetContactUsInfo", parameters);
        if (clientInfo != null && clientInfo.Rows.Count > 0)
        {
            this.EmailNoReply = clientInfo.Rows[0]["RefTblCol9"].ToString();
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
        PageType = SecurePageType.Modal;
        RESET_PWD_TITLE = GetLocalResourceObject("ManageUserResetPwd_Step1_aspx_cs_ResetPassword").ToString();
        MESSAGE_INVALID_USER_ACCOUNT = GetLocalResourceObject("ManageUserResetPwd_Step1_aspx_cs_InvalidUserAccount").ToString();
        if (!IsPostBack)
        {
            if (IsSecureQueryString)
            {
                Page.Title = RESET_PWD_TITLE;
                //Requeset from Manage User Page
                if (SecureQueryString["u"] != null)
                {
                    string userName = SecureQueryString["u"];
                    User user = WebServices.SecurityServices.GetUser(SessionManager.CurrentClient, userName);
                    if (user != null)
                    {
                        uxUsername.Text = VeraCodeSolution.DoVeraCode(user.UserID);
                        uxEmail.Text = VeraCodeSolution.DoVeraCode(user.Email);
                        SessionManager.ResetPasswordUser = user;
                    }
                }

                // Reload Manage User Page after reseting password
                if (SecureQueryString["reload"] != null)
                {
                    int reload;
                    if (int.TryParse(SecureQueryString["reload"], out reload))
                    {
                        ViewState["Reload"] = reload;
                    }
                }
            }
            else
            {
                Page.Title = RESET_PWD_TITLE;
                //Request from Reset MS password
                if (SessionManager.ResetPasswordUser != null)
                {
                    uxUsername.Text = VeraCodeSolution.DoVeraCode(SessionManager.ResetPasswordUser.UserID);
                    uxEmail.Text = VeraCodeSolution.DoVeraCode(SessionManager.ResetPasswordUser.Email);
                }
            }
            this.BindClientInfo();
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }

    protected string BuildLinkUpdate(int clientId, string userName, string passWord, bool isCsSite)
    {
        string result = string.Empty;
        _page = (SecurePage)this.Page;
        string domain = isCsSite ? GeneralFuncsLib.GetDataOfExtendedSetting("CS_DOMAIN") : GeneralFuncsLib.GetDataOfExtendedSetting("MS_DOMAIN");
        domain = string.IsNullOrEmpty(domain) ? WebSiteSettings.CurrentDomain : domain;
        string param = _page.BuildStaticQueryString(
                           string.Format("clientId={0}&userName={1}&passWord={2}", clientId, userName, passWord));

        result = domain + (domain.EndsWith("/") ? string.Empty : "/") + "freeaccess/ActivatePwd.aspx?" + param;



        return result;
    }

    protected void uxContinue_Click(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;

        if (SessionManager.CurrentUser.ASClient == 89 && GeneralFuncsLib.IsSSOUser(SessionManager.CurrentUser.ASClient, SessionManager.ResetPasswordUser.UserID) == true)
        {
            AjaxAddResponseScript(string.Format(ALERT_SCRIPT, MESSAGE_INVALID_USER_ACCOUNT));
            return;
        }

        //Generay PASSWORD
        SessionManager.ResetPasswordUser.UserPassword = WebServices.SecurityServices.GenPwd();

        WebServices.SecurityServices.ResetUserPassword(SessionManager.CurrentClient,
            SessionManager.ResetPasswordUser.UserID, 9, SessionManager.ResetPasswordUser.UserPassword);

        if (uxEmail.Text.Trim() != "")
        {
            //send email here
            string fromEmail = this.EmailNoReply;

            //load mail template
            System.IO.StreamReader mailTemplate;

            if (IsActivePwd)
            {
                mailTemplate = new System.IO.StreamReader(MapPath(@"~\App_Data\tpl_ResetUserPassword_ActivePwd.htm"));
            }
            else
            {
                mailTemplate = new System.IO.StreamReader(MapPath(@"~\App_Data\tpl_ResetUserPassword.htm"));
            }



            string mail_body = mailTemplate.ReadToEnd();

            bool isCsSite = false;

            isCsSite = SessionManager.ResetPasswordUser.UserSecRole.Equals("CSUSERS") || SessionManager.ResetPasswordUser.UserSecRole.Equals("SYSADMIN");

            mailTemplate.Close();

            mail_body = mail_body.Replace("[tpl_ResetUserPassword_htm_Message1]", Resources.Template.tpl_ResetUserPassword_htm_Message1);
            mail_body = mail_body.Replace("[tpl_ResetUserPassword_htm_Message2]", Resources.Template.tpl_ResetUserPassword_htm_Message2);
            mail_body = mail_body.Replace("[tpl_EmailForgotPassword_htm_Title]", Resources.Template.tpl_EmailForgotPassword_htm_Title);
            mail_body = mail_body.Replace("[ReserPasswordUserName]", GeneralFuncsLib.GetProductnameOfClient(SessionManager.CurrentClient));
            mail_body = mail_body.Replace("[password]", SessionManager.ResetPasswordUser.UserPassword);

            if (IsActivePwd)
            {
                mail_body = mail_body.Replace("[tpl_ResetUserPassword_htm_Message1_ActivePwd]", Resources.Template.tpl_ResetUserPassword_htm_Message1_ActivePwd);
                mail_body = mail_body.Replace("[tpl_ResetUserPassword_htm_Message2_ActivePwd]", Resources.Template.tpl_ResetUserPassword_htm_Message2_ActivePwd);
                mail_body = mail_body.Replace("[tpl_ResetUserPassword_htm_Link]", IsActivePwd ? BuildLinkUpdate(SessionManager.CurrentClient, SessionManager.ResetPasswordUser.UserID, SessionManager.ResetPasswordUser.UserPassword, isCsSite) : string.Empty);

            }
            string timeToExpired = string.Empty;

            if (!string.IsNullOrEmpty(((Validation)WebSiteSettings.PwdValidationRule.Get("PasswordAdminResetExpiredDays")).Key))
            {
                int configValue = ((Validation)WebSiteSettings.PwdValidationRule.Get("PasswordAdminResetExpiredDays")).Value;

                string intervalString = configValue > 1 ? (" " + GetLocalResourceObject("ManageUserResetPwd_Step1_aspx_cs_Hours").ToString()) : (" " + GetLocalResourceObject("ManageUserResetPwd_Step1_aspx_cs_Hour").ToString());

                timeToExpired = (configValue * 24) + intervalString;
            }
            else
            {
                timeToExpired = GetLocalResourceObject("ManageUserResetPwd_Step1_aspx_cs_72Hours").ToString();
            }

            mail_body = mail_body.Replace("[Time]", timeToExpired);

            AS.Common.Mail.SmtpMail.SendEmail(fromEmail, uxEmail.Text, GetLocalResourceObject("ManageUserResetPwd_Step1_aspx_cs_EmailSubject").ToString(),
                mail_body, WebSiteSettings.MailSettings);
        }

        // TK25270: call function doRebindUserList() after reseting Pwd 
        if (ViewState["Reload"] != null)
        {
            Response.Redirect("ManageUserResetPwd_Step2.aspx?"
                + BuildSecureQueryString(string.Format("reload={0}", ViewState["Reload"])));
        }
        else
        {
            Response.Redirect("ManageUserResetPwd_Step2.aspx");
        }
    }
}
