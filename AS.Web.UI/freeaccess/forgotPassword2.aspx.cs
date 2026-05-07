//----------------------------------------------------------------------------
// <copyright file="gen_forgotPassword1.aspx.cs" company="Data Delivery Services, Inc.">
//     Copyright (c) Data Delivery Services, Inc. All rights reserved.
// </copyright>
// <author>Hung Ho</author>
// <summary>Handle forgot password function</summary>
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
using System.Net;

using AS.Security.WS.Entities;
using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Pages;

public partial class _mps_forgotpassword2 : NonReportPage
{
    private const string CLIENT_CONTACT_REF = "ClientInformation";
    private const string PASSWORD_ADMIN_RESET_EXPIRE_DAYS = "PasswordAdminResetExpiredDays";

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

    private SecurePage _page = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
        PageType = SecurePageType.Modal;
        if (SessionManager.ForgetPasswordUser == null)
        {
            Response.Redirect("forgotPassword1.aspx");
            return;
        }

        if (!IsPostBack)
        {
            RefTableValueCollection refs = WebServices.SecurityServices.GetRefTableValues(
                SessionManager.CurrentClient, "LoginQuestions", "ENG", SessionManager.CurrentLanguage);
            ListItemCollection lstQuestion = new ListItemCollection();
            int idx = 0;
            int currentQuestionIdx = 0;

            foreach (RefTableValue item in refs)
            {

                string questionText = item.RefTblCols[0];
                string questionValue = item.RefTblKey;

                lstQuestion.Add(new ListItem(questionText, questionValue));

                if (questionValue == SessionManager.ForgetPasswordUser.LoginQuestionIndex.ToString())
                    currentQuestionIdx = idx;

                idx++;
            }

            uxQuestion.Text = VeraCodeSolution.ValidateResponseData(lstQuestion[currentQuestionIdx].Text);
        }

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }


    public string GetLocalIPs(string hostname)
    {

        IPHostEntry ips = Dns.GetHostByName(hostname);
        if (ips.AddressList.Length > 0)
        {
            return ips.AddressList[0].ToString();
        }
        return "";
    }

    protected void uxContinue_Click(object sender, EventArgs e)
    {
        if (this.IsIntruderDetected) return;
        if (WebServices.SecurityServices.IsForgotLockedOut(SessionManager.CurrentClient, SessionManager.ForgetPasswordUser.UserID))
        {
            ShowMessagePopup("FLocked");
            return;
        }
        //Case : Valid Answer
        if (WebServices.SecurityServices.DecryptText(SessionManager.ForgetPasswordUser.LoginQuestionAnswer).ToLower() == uxAnswer.Text.ToLower())
        {
            // Update SuccessForgotPwdAttempts
            WebServices.SecurityServices.UpdateForgotPassword(SessionManager.CurrentClient, SessionManager.ForgetPasswordUser.UserID, true);

            //Generay password             
            string newPassword = WebServices.SecurityServices.GenPwd();

            this.ASPXTrackingLog.LogData5 = "RESET PASSWORD:\t" + SessionManager.ForgetPasswordUser.UserID + "\t" + newPassword;

            WebServices.SecurityServices.ResetUserPassword(SessionManager.CurrentClient,
                SessionManager.ForgetPasswordUser.UserID, 11, newPassword);

            //send email  
            string emailFrom = string.Empty;
            emailFrom = GetNoReplyEmail();

            //load mail template
            System.IO.StreamReader mailTemplate;

            if (IsActivePwd)
            {
                mailTemplate = new System.IO.StreamReader(MapPath(@"~\App_Data\tpl_ForgotPassword_ActivePwd.htm"));
            }
            else
            {
                mailTemplate = new System.IO.StreamReader(MapPath(@"~\App_Data\tpl_ForgotPassword.htm"));
            }

            bool isCsSite = false;
            isCsSite = SessionManager.ForgetPasswordUser.UserSecRole.Equals("CSUSERS") || SessionManager.ForgetPasswordUser.UserSecRole.Equals("SYSADMIN");

            string mail_body = mailTemplate.ReadToEnd();
            mailTemplate.Close();
            mail_body = mail_body.Replace("[tpl_EmailForgotPassword_htm_Title]", Resources.Template.tpl_EmailForgotPassword_htm_Title);
            mail_body = mail_body.Replace("[tpl_ForgotPassword_htm_Message1]", Resources.Template.tpl_ForgotPassword_htm_Message1);
            mail_body = mail_body.Replace("[tpl_ForgotPassword_htm_Message2]", Resources.Template.tpl_ForgotPassword_htm_Message2);

            mail_body = mail_body.Replace("[ReserPasswordUserName]", GeneralFuncsLib.GetProductnameOfClient(SessionManager.CurrentClient));
            mail_body = mail_body.Replace("[password]", newPassword);

            if (IsActivePwd)
            {
                mail_body = mail_body.Replace("[tpl_ResetUserPassword_htm_Message1_ActivePwd]", Resources.Template.tpl_ResetUserPassword_htm_Message1_ActivePwd);
                mail_body = mail_body.Replace("[tpl_ResetUserPassword_htm_Message2_ActivePwd]", Resources.Template.tpl_ResetUserPassword_htm_Message2_ActivePwd);
                mail_body = mail_body.Replace("[tpl_ResetUserPassword_htm_Link]", IsActivePwd ? BuildLinkUpdate(SessionManager.CurrentClient, SessionManager.ForgetPasswordUser.UserID, newPassword, isCsSite) : string.Empty);

            }

            int value = 72;
            if (!string.IsNullOrEmpty(((Validation)WebSiteSettings.PwdValidationRule.Get(PASSWORD_ADMIN_RESET_EXPIRE_DAYS)).Key))
            {
                value = ((Validation)WebSiteSettings.PwdValidationRule.Get(PASSWORD_ADMIN_RESET_EXPIRE_DAYS)).Value * 24;
            }
            string unit = value > 1 ? "hours" : "hour";
            mail_body = mail_body.Replace("[Time]", value.ToString() + " " + unit);

            AS.Common.Mail.SmtpMail.SendEmail(emailFrom, SessionManager.ForgetPasswordUser.Email,
                "New Password Notification", mail_body, WebSiteSettings.MailSettings);

            this.ASPXTrackingLog.LogTxt1 = "MAIL BODY:\r\n" + mail_body;

            //WebServices.SecurityServices.InsertLogUserLoginTransaction(2, 1, SessionManager.ForgetPasswordUser.RecId,
            //   Request.UserHostAddress, GetLocalIPs(Server.MachineName), Request.UserAgent);

            ShowMessagePopup("SuMes");
        }
        else
        {
            //Case :Invalid Answer
            //Set the number user answer wrong
            WebServices.SecurityServices.UpdateForgotPassword(SessionManager.CurrentClient, SessionManager.ForgetPasswordUser.UserID, false);
            ShowMessagePopup("AnNM");
            uxAnswer.Text = string.Empty;
        }
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

    private string GetNoReplyEmail()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.Add(new FilterParameter("@ASClient", SessionManager.CurrentClient, System.Data.DbType.Int32));
        parameters.Add(new FilterParameter("@ContactName", CLIENT_CONTACT_REF, System.Data.DbType.AnsiString));
        DataTable clientInfo = WebServices.SecurityServices.GetReports("spa_GetContactUsInfo", parameters);
        if (clientInfo != null && clientInfo.Rows.Count > 0)
        {
            return clientInfo.Rows[0]["RefTblCol9"].ToString();
        }
        return WebSiteSettings.NoReplyEmail;
    }
    private void ShowMessagePopup(string messageKey)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "OpenMessageWnd",
                string.Format("OpenForgotWndMsg('{0}');",
                "forgotPasswordMsg.aspx?" + BuildSecureQueryString("c=" + messageKey)), true);


    }
}
