using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AS.Controls.Pages;
using AS.Common;
using AS.Common.DBManager;
using AS.Security;
using System.Text.RegularExpressions;

[PagePermission("MerchProfile,MSMerchProfile")]
public partial class MerchantProfileModal : ReportPage
{
    enum PostBackAction
    {
        SubmitOptInOut,
    }

    protected string MerchantNumber
    {
        get { return (string)ViewState["MerchantNumber"]; }
        set { ViewState["MerchantNumber"] = value; }
    }

    protected bool IsOptIn
    {
        get { return (bool)ViewState["IsOptIn"]; }
        set { ViewState["IsOptIn"] = value; }
    }

    private string Email
    {
        get { return (string)ViewState["Email"]; }
        set { ViewState["Email"] = value; }
    }

    private string EmailNoReply
    {
        get { return (string)ViewState["EmailNoReply"]; }
        set { ViewState["EmailNoReply"] = value; }
    }

    private void BindClientInfo()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.Add(new FilterParameter("@ASClient", SessionManager.CurrentUser.ASClient, System.Data.DbType.Int32));
        DataTable clientInfo = WebServices.MsReportServices.GetReports("spa_GetContactUsInfo", parameters);
        if (clientInfo != null && clientInfo.Rows.Count > 0)
        {
            this.EmailNoReply = clientInfo.Rows[0]["RefTblCol9"].ToString();
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.PageType = SecurePageType.Modal;
        if (!IsPostBack)
        {
            BindClientInfo();
            MerchantNumber = SecureQueryString["merchant"];
            Email = SecureQueryString["e"];
            IsOptIn = SecureQueryString["m"] + "" == "1" ? true : false;
            DoNeedDataForControls();
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }

    protected void DoNeedDataForControls()
    {
        idMerchantNum.Text = VeraCodeSolution.DoVeraCode(MerchantNumber);
        string headerText = string.Empty;
        if (!IsOptIn)
        {
            headerText = GetLocalResourceObject("MerchantProfileModal_aspx_cs_ResetPassword").ToString();
            if (string.IsNullOrEmpty(uxCommentText.Text.Trim()))
                uxCommentText.Text = VeraCodeSolution.DoVeraCode(GetLocalResourceObject("MerchantProfileModal_aspx_cs_MerchantOptedIn").ToString() + " - ");
            idMsg.Text = VeraCodeSolution.DoVeraCode(GetLocalResourceObject("MerchantProfileModal_aspx_cs_MerchantResetPassword").ToString());
            idEmail.Text = VeraCodeSolution.DoVeraCode(Email);
            FilterParameterCollection paramList = new FilterParameterCollection();
            paramList.AddLoggedInUserReportingParams();
            paramList.Add(new FilterParameter("@CheckedUserID", idMerchantNum.Text, DbType.String));
            DataTable returnDT = WebServices.MsReportServices.GetReports("spa_ms_CheckMaxSecCountForUser", paramList);
            //*/
            if (returnDT != null && returnDT.Rows.Count > 0)
            {
                uxMaxSec.Attributes.Add("curU", returnDT.Rows[0]["CurSecCount"].ToString());
                uxMaxSec.Text = VeraCodeSolution.ValidateResponseData(returnDT.Rows[0]["MaxSecCount"].ToString());
            }
            else
            {
                uxMaxSec.Text = string.Empty;
            }

        }
        else
        {
            headerText = GetLocalResourceObject("MerchantProfileModal_aspx_cs_MerchantOptOutMerchant").ToString();
            //idHeaderText.Text = "Opt Out Merchant";
            if (string.IsNullOrEmpty(uxCommentText.Text.Trim()))
                uxCommentText.Text = VeraCodeSolution.DoVeraCode(GetLocalResourceObject("MerchantProfileModal_aspx_cs_MerchantOptedOut").ToString() + " - ");
            idMsg.Text = VeraCodeSolution.DoVeraCode(GetLocalResourceObject("MerchantProfileModal_aspx_cs_OutTheMerchant").ToString());
            idEmailHolder.Visible = false;
        }
        uxTitle.Text = VeraCodeSolution.DoVeraCode(headerText);
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.SubmitOptInOut:
                this.SubmitOptInOut();
                break;
        }
    }

    protected void uxSubmit_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.SubmitOptInOut);
    }

    private void SubmitOptInOut()
    {
        FilterParameterCollection tempParameters = null;
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();
        parameters.Add(new FilterParameter("@MerchantNumber", MerchantNumber, DbType.AnsiString));
        parameters.Add(new FilterParameter("@FlagPermission", SessionManager.CurrentUserViewMode, DbType.Int32));
        string temp = IsOptIn == true ? "1" : "0";
        parameters.Add(new FilterParameter("@OptStatus", temp, DbType.AnsiString));
        WebServices.MsReportServices.ExecuteNonQueryCommand("spa_ms_SetMerchantOptStatus", parameters, out tempParameters);
        // save UserActivity
        string atv_Opt = string.Format(GetLocalResourceObject("MerchantProfileModal_aspx_cs_MerchantProfile").ToString(), IsOptIn ? GetLocalResourceObject("MerchantProfileModal_aspx_cs_OptOut").ToString() : GetLocalResourceObject("MerchantProfileModal_aspx_cs_OptIn").ToString(), MerchantNumber);
        GeneralFuncsLib.SaveUserActivity(MerchantNumber, atv_Opt);

        if (!temp.Equals("1"))
        {

            SessionManager.ResetPasswordUser = WebServices.SecurityServices.GetUser(SessionManager.CurrentUser.ASClient, MerchantNumber);
            if (SessionManager.ResetPasswordUser != null)
            {
                SessionManager.ResetPasswordUser.UserPassword = WebServices.SecurityServices.GenPwd();

                WebServices.SecurityServices.ResetUserPassword(SessionManager.CurrentUser.ASClient,
                SessionManager.ResetPasswordUser.UserID, 9, SessionManager.ResetPasswordUser.UserPassword);

                string validationCode = WebServices.SecurityServices.EncryptText(Guid.NewGuid().ToString() + DateTime.Now.Ticks);
                WebServices.SecurityServices.CreateForgotPwdTicket(SessionManager.ResetPasswordUser.UserID, validationCode);
                if (!string.IsNullOrEmpty(idEmail.Text))
                {
                    //send email here
                    string fromEmail = GetEmailFrom(MerchantNumber);

                    if (fromEmail == String.Empty)
                    {
                        fromEmail = this.EmailNoReply;
                    }

                    //load mail template
                    System.IO.StreamReader mailTemplate = new System.IO.StreamReader(MapPath(@"~\App_Data\tpl_ResetUserPassword.htm"));
                    string mail_body = mailTemplate.ReadToEnd();
                    mailTemplate.Close();
                    string[] emails = idEmail.Text.Trim().Replace(";", ",").Split(',');
                    for (int i = 0; i < emails.Length; i++)
                    {
                        if (isEmail(emails[i]))
                        {
                            string urlParameter = "?u=" + Server.UrlEncode(WebServices.SecurityServices.EncryptText(SessionManager.ResetPasswordUser.UserID)) + "&v=" + Server.UrlEncode(validationCode) + "&p=" + Server.UrlEncode(WebServices.SecurityServices.EncryptText(SessionManager.ResetPasswordUser.UserPassword)) + "&a=" + WebServices.SecurityServices.EncryptText("t");
                            string urlEmail = GetTargetDomain() + "/" + ("Login.aspx") + urlParameter;
                            string urlEmailText = urlEmail;
                            mail_body = mail_body.Replace("[tpl_ResetUserPassword_htm_Message1]", Resources.Template.tpl_ResetUserPassword_htm_Message1);
                            mail_body = mail_body.Replace("[tpl_ResetUserPassword_htm_Message2]", Resources.Template.tpl_ResetUserPassword_htm_Message2);
                            mail_body = mail_body.Replace("[tpl_EmailForgotPassword_htm_Title]", Resources.Template.tpl_EmailForgotPassword_htm_Title);
                            mail_body = mail_body.Replace("[ReserPasswordUserName]", GeneralFuncsLib.GetProductnameOfClient(SessionManager.CurrentClient));
                            mail_body = mail_body.Replace("[password]", SessionManager.ResetPasswordUser.UserPassword);
                            string timeToExpired = string.Empty;

                            if (!string.IsNullOrEmpty(((Validation)WebSiteSettings.PwdValidationRule.Get("PasswordAdminResetExpiredDays")).Key))
                            {
                                int configValue = ((Validation)WebSiteSettings.PwdValidationRule.Get("PasswordAdminResetExpiredDays")).Value;

                                string intervalString = configValue > 1 ? " " + GetLocalResourceObject("MerchantProfileModal_aspx_cs_Hours").ToString() : " " + GetLocalResourceObject("MerchantProfileModal_aspx_cs_Hour").ToString();

                                timeToExpired = (configValue * 24) + intervalString;
                            }
                            else
                            {
                                timeToExpired = GetLocalResourceObject("MerchantProfileModal_aspx_cs_72Hour").ToString();
                            }

                            mail_body = mail_body.Replace("[Time]", timeToExpired);


                            AS.Common.Mail.SmtpMail.SendEmail(fromEmail, emails[i], GetLocalResourceObject("MerchantProfileModal_aspx_cs_EmailSubject").ToString(),
                                mail_body, WebSiteSettings.MailSettings);
                        }
                    }
                }

                //update max secondary users
                //*

                FilterParameterCollection paramList = new FilterParameterCollection();
                paramList.AddLoggedInUserReportingParams();
                paramList.Add(new FilterParameter("@SetUserID", SessionManager.ResetPasswordUser.UserID, DbType.String));
                int maxSecCount = 0;
                Int32.TryParse(uxMaxSec.Text, out maxSecCount);
                paramList.Add(new FilterParameter("@MaxSecCount", maxSecCount, DbType.Int32));
                FilterParameterCollection outparamList = null;
                WebServices.MsReportServices.ExecuteNonQueryCommand("spa_ms_UpdateMaxSecCountForUser", paramList, out outparamList);
                //*/
                bool isPCISynchUser = GeneralFuncsLib.GetDataOfExtendedSetting("PCI_USER_SYNCH").Equals("true") ? true : false;
                if (isPCISynchUser && SessionManager.CurrentSystem == 1) // For CS only
                {
                    SyncWithPCIResetPassword();
                }
            }
            else
            {
                SessionManager.ResetPasswordUser = new AS.Security.WS.Entities.User();
            }

        }
        //insert a comment
        parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();
        parameters.Add(new FilterParameter("@MerchantNumber", MerchantNumber, DbType.AnsiString));

        if (!string.IsNullOrEmpty(uxCommentText.Text.Trim()))
        {
            parameters.Add(new FilterParameter("@Comment", uxCommentText.Text.Trim(), DbType.String));
            uxCommentText.Text = string.Empty;
        }
        else
        {
            parameters.Add(new FilterParameter("@Comment", (temp.Equals("1") ? (GetLocalResourceObject("MerchantProfileModal_aspx_cs_MerchantOptedOut").ToString() + " - ") : (GetLocalResourceObject("MerchantProfileModal_aspx_cs_MerchantOptedIn").ToString() + " - ")) + SessionManager.CurrentUser.UserID, DbType.String));
        }
        WebServices.MsReportServices.ExecuteNonQueryCommand("spa_cs_InsertCommentsOfMerchant", parameters, out tempParameters);

        bool isOutPCISynchUser = GeneralFuncsLib.GetDataOfExtendedSetting("PCI_USER_SYNCH").Equals("true") ? true : false;
        if (isOutPCISynchUser && SessionManager.CurrentSystem == 1) // For CS only
        {
            GeneralFuncsLib.SyncUserWithPCIForLockAccount(SessionManager.CurrentUser.ASClient, MerchantNumber, !IsOptIn);
        }


        if (IsPostBack)
        {
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "parent.UpdateResult();", true);
        }

        if (!temp.Equals("1"))
        {
            Response.Redirect("ManageUserResetPwd_Step2.aspx");
        }
    }


    private void SyncWithPCIResetPassword()
    {
        //Reset MS 
        FilterParameterCollection parameterIn = new FilterParameterCollection();
        FilterParameterCollection parameterOut = new FilterParameterCollection();
        parameterIn.Add(new FilterParameter("@ASClient", SessionManager.CurrentUser.ASClient, System.Data.DbType.Int32));
        parameterIn.Add(new FilterParameter("@UserName", MerchantNumber, System.Data.DbType.String));
        parameterIn.Add(new FilterParameter("@UserPasswordType", 9, System.Data.DbType.Int32));
        parameterIn.Add(new FilterParameter("@NewPassword", SessionManager.ResetPasswordUser.UserPassword, System.Data.DbType.AnsiString));
        PciWebServices.PciReportServices.ExecuteNonQueryCommand("spa_SEC_ResetUserPassword", parameterIn, out parameterOut);
    }
    private string GetEmailFrom(string merchantNum)
    {
        // get Customer Service email corresponding to the user who request a temporary pwd.
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();
        parameters.AddLoggedInUserPrimaryUserID();
        DataTable dtClientInfor = WebServices.MsReportServices.GetReports("spa_ms_GetContactUs", parameters);
        if (dtClientInfor != null && dtClientInfor.Rows.Count > 0 && !dtClientInfor.Rows[0].IsNull("EmailAddress"))
        {
            return dtClientInfor.Rows[0]["EmailAddress"].ToString();
        }

        return string.Empty;
    }


    public static bool isEmail(string inputEmail)
    {
        string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
              @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
              @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        Regex re = new Regex(strRegex);
        if (re.IsMatch(inputEmail))
            return (true);
        else
            return (false);

    }

    private void SubmitOptInOut_MPS()
    {
        FilterParameterCollection param = new FilterParameterCollection();
        FilterParameterCollection outParam = new FilterParameterCollection();
        param.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        param.Add(new FilterParameter("@MerchantNumber", MerchantNumber, DbType.AnsiString));
        param.Add(new FilterParameter("@FlagPermission", SessionManager.CurrentUserViewMode, DbType.Int32));
        param.Add(new FilterParameter("@OptStatus", IsOptIn, DbType.Boolean));
        WebServices.CsReportServices.ExecuteNonQueryCommand("spa_ms_SetMerchantOptStatus", param, out outParam);

        //  Add comment
        param.Clear();
        outParam.Clear();
        string comment = string.Empty;
        if (!IsOptIn)
            comment = GetLocalResourceObject("MerchantProfileModal_aspx_cs_MerchantOptedOut").ToString() + " - " + SessionManager.CurrentUser.UserID;
        else
            comment = GetLocalResourceObject("MerchantProfileModal_aspx_cs_MerchantOptedIn").ToString() + " - " + SessionManager.CurrentUser.UserID;
        param.AddLoggedInUserReportingParams();
        param.Add(new FilterParameter("@MerchantNumber", MerchantNumber, DbType.AnsiString));
        param.Add(new FilterParameter("@Comment", comment, DbType.AnsiString));
        FilterParameterCollection oParameters = new FilterParameterCollection();
        WebServices.CsReportServices.ExecuteNonQueryCommand("spa_cs_InsertCommentsOfMerchant", param, out outParam);

        // save UserActivity
        string atv_Opt = string.Format(GetLocalResourceObject("MerchantProfileModal_aspx_cs_MerchantProfile").ToString(), !IsOptIn ? GetLocalResourceObject("MerchantProfileModal_aspx_cs_OptOut").ToString() : GetLocalResourceObject("MerchantProfileModal_aspx_cs_OptIn").ToString(), MerchantNumber);
        GeneralFuncsLib.SaveUserActivity(MerchantNumber, atv_Opt);
    }

    private string GetTargetDomain()
    {
        string currentUrl = Request.Url.ToString();
        string gatewayUrl = WebSiteSettings.MsGate;
        int splitIndex = 0;
        string urldomain = currentUrl.Substring(0, splitIndex);
        if (SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.CS)
        {
            splitIndex = gatewayUrl.LastIndexOf('/');
            if (splitIndex > 0)
                return gatewayUrl.Substring(0, splitIndex);
            else
                return string.Empty;
        }
        else
        {
            splitIndex = currentUrl.LastIndexOf('/');
            if (splitIndex > 0)
                return currentUrl.Substring(0, splitIndex);
            else
                return string.Empty;
        }
    }
}
