using System;
using System.Data;
using System.Linq;
using System.Web.Security;
using System.Web.UI.WebControls;
using AS.Security.WS.Entities;
using System.Collections.Generic;
using AS.Common;
using Telerik.Web.UI;
using AS.Common.DBManager;
using System.Text.RegularExpressions;
using AS.Tax.Security.Web.Services;
using System.Text;
using AS.Controls.Global;
using AS.Common.Logger;
using AS.Web.SharedSession;
using AS.Tax.Security.Web.Services.Model;
using AS.Web.Business.Shared.Constants;
using VW.PCI.Api.Client;

namespace As.VisionWeb.Web
{
    public partial class UserProfileControl : GlobalUserControl
    {
        #region Const
        private string MSG_OldPasswordNotCorrect = string.Empty;
        private string MSG_UpdateUnsuccessful = string.Empty;
        private string MSG_UpdateSuccessful = string.Empty;
        private string MSG_ConfirmPasswordNotMatch = string.Empty;
        private string MSG_PasswordLengthNotValid = string.Empty;
        private string MSG_PasswordMustHaveNoSpecialCharacter = string.Empty;
        private string MSG_PassMustHaveAtLeastOneAlphaCharAndOneNum = string.Empty;
        private string MSG_LengNotValid = string.Empty;
        private string MSG_InvalidEmail = string.Empty;
        private string MSG_InvalidFormat = string.Empty;
        private const int SYSTEM_CS = 1;
        private const int LOGIN_SUCCESSFULL = 1;
        private const int LOGIN_WITH_TEMP = 100;
        private const int LOGIN_PASS_NEARLY_EXPIRED = 10;
        private const string SITE_ACCESS_PCI_ADMIN_PERMISSIONCODE = "SiteAccessPCIAdmin";
        private const string HIERARCHY_SITE_ACCESS_PCI_ADMIN = "HierarchySiteAccessPCIAdmin";
        private const string MERCHANT_SITE_ACCESS_PCI_ADMIN = "MerchantSiteAccessPCIAdmin";
        protected bool userSignOn = GeneralFuncsLib.GetDataOfExtendedSetting("IS_CHANGE_USERNAME_MS_SITE").ToLower().Equals("true");
        protected bool isSecondaryUser = !string.IsNullOrEmpty(SessionManager.CurrentUser.UserSecRole) && (SessionManager.CurrentUser.UserSecRole.EndsWith("SEC") || SessionManager.CurrentUser.UserSecRole.EndsWith("_REF"));
        #endregion
        protected bool IsGRP_TOTAL = false;
        protected string _dayRemained = string.Empty;
        public bool IsMSUser
        {
            get;
            set;
        }

        private string ClientEmail
        {
            get { return (string)ViewState["ClientEmail"]; }
            set { ViewState["ClientEmail"] = value; }
        }
        private string ClientName
        {
            get { return (string)ViewState["ClientName"]; }
            set { ViewState["ClientName"] = value; }
        }

        public ManageSetting ConfigSetting
        {
            set { ViewState["ManageSetting"] = value; }
            get
            {
                if (ViewState["ManageSetting"] == null)
                    ViewState["ManageSetting"] = new ManageSetting();
                return (ManageSetting)ViewState["ManageSetting"];
            }
        }

        private void BindClientInfo()
        {
            FilterParameterCollection parameters = new FilterParameterCollection();
            parameters.Add(new FilterParameter("@ASClient", SessionManager.CurrentClient, DbType.Int32));
            DataTable clientInfo = WebServices.SecurityServices.GetReports("spa_GetContactUsInfo", parameters);
            if (clientInfo != null && clientInfo.Rows.Count > 0)
            {
                this.ClientEmail = clientInfo.Rows[0]["RefTblCol9"].ToString();
                this.ClientName = clientInfo.Rows[0]["RefTblCol1"].ToString();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            // Hide/show Environment indicator by the config in web.config
            uxPnlEnvironmentIndicator.Visible = !string.IsNullOrEmpty(GeneralFuncsLib.GetCurrentEnvironment(false)) && !GeneralFuncsLib.GetCurrentEnvironment(false).Equals("PROD", StringComparison.OrdinalIgnoreCase);

            MSG_InvalidFormat = GetLocalResourceObject("UserProfile_ascx_cs_MSG_InvalidFormat").ToString();
            MSG_InvalidEmail = GetLocalResourceObject("UserProfile_ascx_cs_MSG_InvalidEmailFormat").ToString();
            MSG_PassMustHaveAtLeastOneAlphaCharAndOneNum = GetLocalResourceObject("UserProfile_ascx_cs_MSG_LengthMinMax").ToString();
            MSG_PassMustHaveAtLeastOneAlphaCharAndOneNum = GetLocalResourceObject("UserProfile_ascx_cs_MSG_PasswordAtLeastCharacterNumber").ToString();
            MSG_PasswordMustHaveNoSpecialCharacter = GetLocalResourceObject("UserProfile_ascx_cs_MSG_PasswordHaveNoSpecialCharacter").ToString();
            MSG_PasswordLengthNotValid = GetLocalResourceObject("UserProfile_ascx_cs_MSG_PasswordLengthInValid").ToString();
            MSG_ConfirmPasswordNotMatch = GetLocalResourceObject("UserProfile_ascx_cs_MSG_ConfirmPasswordIsNotMatch").ToString();
            MSG_UpdateSuccessful = GetLocalResourceObject("UserProfile_ascx_cs_MSG_UpdateSuccessfully").ToString();
            MSG_UpdateUnsuccessful = GetLocalResourceObject("UserProfile_ascx_cs_MSG_UpdateUnSuccessfully").ToString();
            MSG_OldPasswordNotCorrect = GetLocalResourceObject("UserProfile_ascx_cs_MSG_OldPasswordNotCorrect").ToString();
            GetMessageUxPasswordRule();
            txtAnswerErrMsg.ShowOnLoad = false;
            txtConfirmPassErrMsg.ShowOnLoad = false;
            txtEmailErrMsg.ShowOnLoad = false;
            //Reset valid status
            txtEmailLabel.CssClass = "control-label";
            if (GeneralFuncsLib.IsAlertNotification())
            {
                txtEmailContactErrMsg.ShowOnLoad = false;
                txtEmailContactLabel.CssClass = "control-label";
                uxPhoneErrMsg.ShowOnLoad = false;
                txtPhoneLabel.CssClass = "control-label";
            }
            else
            {
                uxPhoneSMSContainer.Visible = uxEmailContactContainer.Visible = false;
            }

            txtFirstNameErrMsg.ShowOnLoad = false;
            txtLastNameErrMsg.ShowOnLoad = false;
            txtNewPassErrMsg.ShowOnLoad = false;
            txtOldPassErrMsg.ShowOnLoad = false;
            IsMSUser = SessionManager.CurrentSystem == WebSiteConstants.AS_SYSTEM_MS;
            uxPldQuestion.Visible = true;

            if (SessionManager.ForceChangePassword)
            {
                if (SessionManager.PasswordExpired)
                {
                    uxWarning1.Visible = true; //Show message: Your password has expired. Please change your password now.
                }
                else
                {
                    uxWarning2.Visible = true; // Show message: You are logging in with temporary password. Please change your password now.
                }

                uxHddChkPass.Value = "true";
                uxHddChkQuestion.Value = uxPldQuestion.Visible.ToString();
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "HideCheckbox", "$(document).ready(function(){setTimeout('HideCheckbox()',500);});", true);
            }
            else if (SessionManager.PasswordExpiredNearly)
            {
                uxWarning3.Visible = true; //Show message: Your password will expire in ... 
                _dayRemained = SessionManager.DayRemainingPasswordExpired.ToString();
            }

            if (!IsPostBack)
            {
                BindUserInfo();
                BindClientInfo();
                BindNote();
                BindManageSetting();
                initDefaultLandingPage();

                if (this.Page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_DISABLEDUPDATEUSER) || this.Page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_DISABLEDUPDATEUSER_MS))
                {
                    DisabledUpdateUser();
                }
                if (!this.Page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RSK_QUEUE) && !this.Page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RSK_QUEUE_MS))
                {
                    uxRiskDetectionQueueDefaultView.Visible = false;
                }

                uxRiskCaseTypeDefault.Visible = this.Page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_CM_RISK_CASE) ||
                    this.Page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_MS_CM_RISK_CASE);
            }
            BindMSChangeUserUI();

            if (SessionManager.CurrentUser.ASClient == 89 && SessionManager.CurrentUser.UserSecRole == UserSecRole.GRPPRI)
            {
                IsGRP_TOTAL = true;
                if (userSignOn && IsMSUser)
                {
                    uxOriginalUserName.Enabled = false;
                }
                uxUserName.Enabled = false;
                uxFirstName.Enabled = false;
                uxLastName.Enabled = false;
                uxEmail.Enabled = false;
                uxTMSAccount.Visible = true;
                uxPanelEmailNote.Visible = false;

                tdEmailDisabled.Visible = tdFirstNameDisabled.Visible = tdLastNameDisabled.Visible = true;
                tdEmail.Visible = tdFirstName.Visible = tdLastName.Visible = false;
                uxFirstNameDisabled.Text = uxFirstName.Text;
                uxLastNameDisabled.Text = uxLastName.Text;
                uxEmailDisabled.Text = uxEmail.Text;

                string urlAddress = GeneralFuncsLib.GetDataOfExtendedSetting("TMS_CONTACT_URL");
                string url = string.Format("<a href=\"javascript:void(0)\" onclick=\"openPopupWindow('{0}','Contact US'); return false;\">{1}</a>", urlAddress, "here");
                uxTMSNote.Text = VeraCodeSolution.DoVeraCode(string.Format(Resources.MessageManager.UserProfile_TMS_Note, url));
            }

            uxUserProfileMasterUserControl.LoadUser(SessionManager.CurrentClient, SessionManager.CurrentUser.UserID);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            bool isUpdate = UpdateProfile();

            if (isUpdate)
            {
                uxUserProfileMasterUserControl.SubmitUser(SessionManager.CurrentClient, SessionManager.CurrentUser.UserID);

                //Register action update header menu by client site, do not user server site
                Page.ClientScript.RegisterStartupScript(this.GetType(), "UpdateHeaderMenu", "PageMethods.UpdateHeaderMenu();", true);
            }
        }
        //Bind timezone
        protected void BindTimeZone()
        {
            drSysTimeZone.DataSource = TimeZoneHandler.GetTimeZones();
            drSysTimeZone.DataBind();
            bool isAutomaticTz = TimeZoneHandler.IsAutomaticTimeZone();
            ckTimeZone.Checked = isAutomaticTz;
            this.ConfigSetting.SetDefaultTimeZone = isAutomaticTz;
            //TK41161 - Selected timezone on drp when checkbox "save time zone automatic" is checked
            drSysTimeZone.SelectedValue = TimeZoneHandler.CurrentSettingKey();
            drSysTimeZone.Enabled = !isAutomaticTz;
        }

        // Update Timezone
        protected void UpdateTimeZone()
        {
            var isUpdateTimeZone = this.ConfigSetting.SetDefaultTimeZone != ckTimeZone.Checked
                       || !(drSysTimeZone.SelectedValue.Equals(TimeZoneHandler.CurrentSettingKey(), StringComparison.OrdinalIgnoreCase));

            if (ckTimeZone.Checked)
            {
                //Save Timezone
                if (!GeneralFuncsLib.GetCookie("ClientTimezone").IsNullOrEmpty() && !GeneralFuncsLib.GetCookie("DayLightSaving").IsNullOrEmpty())
                {
                    TimeZoneHandler.SaveTimeZone(true, string.Empty, GeneralFuncsLib.GetCookie("ClientTimezone"), GeneralFuncsLib.GetCookie("DayLightSaving").ToInt());
                }
                else
                {
                    string clientTimezone = GeneralFuncsLib.GetCookie("ClientTimezone").IsNotNullData() ? GeneralFuncsLib.GetCookie("ClientTimezone") : "";
                    string dayLightSaving = GeneralFuncsLib.GetCookie("DayLightSaving").IsNotNullData() ? GeneralFuncsLib.GetCookie("DayLightSaving") : "";

                    LoggerManager.Warn(string.Format("Cookie clienttimezone is null! UserId={0}; ClientId={1}; ClientTimezone={2}; DayLightSaving={3}"
                    , SessionManager.CurrentUser.UserID, SessionManager.CurrentUser.ASClient, clientTimezone, dayLightSaving));

                }
            }
            else
            {
                TimeZoneHandler.SaveTimeZone(false, drSysTimeZone.SelectedValue, string.Empty);
            }

            if (isUpdateTimeZone)
            {
                var user = WebServices.SecurityServices.GetUser(SessionManager.CurrentClient, SessionManager.CurrentUser.UserID);
                SessionManager.CurrentUser.PrevLoginDTS = user.PrevLoginDTS;
            }

            BindTimeZone();
        }

        protected void BindMSChangeUserUI()
        {
            if (userSignOn && IsMSUser && !isSecondaryUser)
            {
                var currentUser = SessionManager.CurrentUser;
                IsUserSignOn.Visible = true;
                txtNewUserNameErrMsg.ShowOnLoad = false;
                lbNewUserName.CssClass = "control-label js-lbNewUserName";
                if (currentUser.UserID.Equals(currentUser.OriginalUserID) && currentUser.UserPasswordType == 10
                    || (currentUser.UserPasswordType == 9 && string.IsNullOrEmpty(currentUser.LoginQuestionAnswer)))
                {
                    ckChangeUserName.Enabled = true;
                    ckChangeUserName.Checked = false;
                    uxNewUserName.Attributes.Add("readonly", "true");
                }
                else
                {
                    ckChangeUserName.Enabled = false;
                    ckChangeUserName.Checked = false;
                    uxNewUserName.ReadOnly = true;
                }

                uxNewUserName.Attributes["MaxLength"] = GetUserNameMaxLengthForUserProfile(currentUser.ASClient).ToString();
            }
            else
            {
                IsNotUserSignOn.Visible = true;

            }
        }

        private void SendAlertEmail()
        {
            bool isAlert = GeneralFuncsLib.GetDataOfExtendedSetting("IS_ALERT").Equals("true");
            if (isAlert)
            {
                //spa_GetAuditUserChanges
                FilterParameterCollection _parames = new FilterParameterCollection();
                _parames.AddLoggedInUserReportingParams();
                _parames.Add(new FilterParameter("@ChangedByRecID", SessionManager.CurrentUser.RecId.ToString(), DbType.AnsiString));
                if (uxNewUserName.Visible)
                {
                    _parames.Add(new FilterParameter("@ChangedUserID", uxNewUserName.Text, DbType.AnsiString));
                }
                else
                {
                    _parames.Add(new FilterParameter("@ChangedUserID", uxUserName.Text, DbType.AnsiString));
                }
                _parames.AddLanguageID();
                DataTable data = WebServices.SecurityServices.GetReports("spa_GetAuditUserChangesOfUser", _parames);

                if (uxEmail.Text.Trim() != "" && data != null && data.Rows.Count > 0)
                {
                    //load mail template
                    System.IO.StreamReader mailTemplate;
                    bool isAlerDefault = GeneralFuncsLib.GetDataOfExtendedSetting("ALERT_DEFAULT") != "false";

                    string bradingBankName = GeneralFuncsLib.GetDataOfExtendedSetting("BRANDING_EMAIL_ALERT_UPDATE_USER");
                    string bradingTemplateAll = GeneralFuncsLib.GetDataOfExtendedSetting("BRANDING_EMAIL_ALERT_UPDATE_CS_MS_USER");
                    const int BANK_ENTITY_TYPE = 1;
                    if (!bradingTemplateAll.IsNullOrEmpty())
                    {
                        mailTemplate = new System.IO.StreamReader(MapPath(@"~\App_Data\tpl_AlertEmail_" + bradingTemplateAll + ".htm"));
                    }
                    else
                    {
                        if (SessionManager.CurrentSystem == WebSiteConstants.AS_SYSTEM_MS && !bradingBankName.IsNullOrEmpty()
                            && GetParentEntityNumber(BANK_ENTITY_TYPE).Equals(bradingBankName, StringComparison.OrdinalIgnoreCase))
                        {
                            mailTemplate = new System.IO.StreamReader(MapPath(@"~\App_Data\tpl_AlertEmail_" + bradingBankName + ".htm"));
                        }
                        else if (!isAlerDefault)
                        {
                            mailTemplate = new System.IO.StreamReader(MapPath(@"~\App_Data\tpl_AlertEmail_EmailContact.htm"));
                        }
                        else
                            mailTemplate = new System.IO.StreamReader(MapPath(@"~\App_Data\tpl_AlertEmail.htm"));
                    }
                    string mail_body = mailTemplate.ReadToEnd();
                    mail_body = mail_body.Replace("[tpl_AlertEmail_htm_UpdatesMadeToUser]", Resources.Template.tpl_AlertEmail_htm_UpdatesMadeToUser);
                    mail_body = mail_body.Replace("[tpl_AlertEmail_htm_ChangeType]", Resources.Template.tpl_AlertEmail_htm_ChangeType);
                    mail_body = mail_body.Replace("[tpl_AlertEmail_htm_Field]", Resources.Template.tpl_AlertEmail_htm_Field);
                    mail_body = mail_body.Replace("[tpl_AlertEmail_htm_OldValue]", Resources.Template.tpl_AlertEmail_htm_OldValue);
                    mail_body = mail_body.Replace("[tpl_AlertEmail_htm_NewValue]", Resources.Template.tpl_AlertEmail_htm_NewValue);

                    /**************56897***********************/
                    string financialInstitutions = GeneralFuncsLib.GetFinancialInstitutions();
                    string directMerchants = GeneralFuncsLib.GetDirectMerchants();
                    if (string.IsNullOrEmpty(financialInstitutions)
                        && string.IsNullOrEmpty(directMerchants)
                        && !mail_body.Contains("[tpl_AlertEmail_BNK8714_htm_LocalPhone]")
                        && !mail_body.Contains("[tpl_AlertEmail_BNK8714_htm_TollFree]"))
                        mail_body = mail_body.Replace("[tpl_AlertEmail_htm_ContactService]", string.Empty);
                    else
                        mail_body = mail_body.Replace("[tpl_AlertEmail_htm_ContactService]", Resources.Template.tpl_AlertEmail_htm_ContactService);

                    // Remove Payline
                    if (string.IsNullOrEmpty(financialInstitutions))
                        mail_body = mail_body.Replace("[tpl_AlertEmail_htm_FinancialInstitutions]", string.Empty);
                    else
                        mail_body = mail_body.Replace("[tpl_AlertEmail_htm_FinancialInstitutions]", "<li>" + financialInstitutions + "</li>");

                    if (string.IsNullOrEmpty(directMerchants))
                        mail_body = mail_body.Replace("[tpl_AlertEmail_htm_DirectMerchants]", string.Empty);
                    else
                        mail_body = mail_body.Replace("[tpl_AlertEmail_htm_DirectMerchants]", "<li>" + directMerchants + "</li>");
                    /************** END 56897***********************/

                    mail_body = mail_body.Replace("[tpl_AlertEmail_BNK8714_htm_LocalPhone]", Resources.Template.tpl_AlertEmail_BNK8714_htm_LocalPhone);
                    mail_body = mail_body.Replace("[tpl_AlertEmail_BNK8714_htm_TollFree]", Resources.Template.tpl_AlertEmail_BNK8714_htm_TollFree);
                    mailTemplate.Close();

                    StringBuilder content = new StringBuilder();
                    foreach (DataRow row in data.Rows)
                    {
                        content.Append(string.Format(@"
                        <tr>
                            <td align='center'>{0}</td>
                            <td align='center'>{1}</td>
                            <td align='center'>{2}</td>
                            <td align='center'>{3}</td>
                        </tr>", row["ChangeType"], row["FieldName"], row["OldValue"], row["NewValue"]));
                    }
                    mail_body = mail_body.Replace("[CONTENT]", content.ToString());
                    AS.Common.Mail.SmtpMail.SendEmail(this.ClientEmail, uxEmail.Text.Trim(), GetLocalResourceObject("UserProfile_ascx_cs_UserChangeNotification").ToString(),
                        mail_body, WebSiteSettings.MailSettings);
                }

            }
        }
        protected string GetParentEntityNumber(int entityTypeID)
        {
            FilterParameterCollection _parames = new FilterParameterCollection();
            _parames.AddLoggedInUserReportingParams(true);
            _parames.Add("@ParentEntityType", entityTypeID, DbType.Int32);
            DataTable data = WebServices.SecurityServices.GetReports("spa_GetParentEntityNumber", _parames);
            if (data != null && data.Rows.Count > 0)
            {
                return data.Rows[0]["EntityNumber"].ToSafeString();
            }
            return string.Empty;
        }
        protected void BindUserInfo()
        {
            User userVo = SessionManager.CurrentUser;
            if (userVo != null)
            {
                uxNewUserName.Text = VeraCodeSolution.DoVeraCode(userVo.UserID);
                uxOriginalUserName.Text = VeraCodeSolution.DoVeraCode(userVo.OriginalUserID);
                uxUserName.Text = VeraCodeSolution.DoVeraCode(userVo.UserID);
                uxFirstName.Text = VeraCodeSolution.DoVeraCode(userVo.UserNameFirst);
                uxLastName.Text = VeraCodeSolution.DoVeraCode(userVo.UserNameLast);
                uxEmail.Text = VeraCodeSolution.DoVeraCode(userVo.Email);
                uxPhone.Text = VeraCodeSolution.DoVeraCode(userVo.PhoneForSMS);
                uxEmailContact.Text = VeraCodeSolution.DoVeraCode(userVo.ContactEmail);
                uxAnswer.Text = WebServices.SecurityServices.DecryptText(userVo.LoginQuestionAnswer);

                string passwordTemporary = SessionManager.PasswordTemporary;

                if (!string.IsNullOrEmpty(passwordTemporary))
                {
                    LoginByEmailLink(passwordTemporary, userVo.UserID);
                }
            }
            BindDropDownQuestion();
            BindTimeZone();
        }

        protected void LoginByEmailLink(string password, string userName)
        {
            int loginResult = WebServices.SecurityServices.ValidateUser(SessionManager.CurrentClient, userName, password, SessionManager.UniqueSessionID, Session.SessionID);
            loginResult = loginResult >> 24;
            if ((loginResult == LOGIN_SUCCESSFULL) || (loginResult == LOGIN_PASS_NEARLY_EXPIRED) || (loginResult == LOGIN_WITH_TEMP))
            {
                uxOldPass.Visible = false;
                uxOldPass.TextMode = TextBoxMode.SingleLine;
                uxOldTempPassword.Visible = true;
                uxOldPass.Text = password;
            }
            else
            {
                ShowMessageBox(GetLocalResourceObject("UserProfile_ascx_cs_InvalidOldPassword").ToString());
                uxFlagLoginLink.Value = "1";
            }
        }

        private void LogOut()
        {
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            Response.Redirect(this.Page.LoginUrl);
        }

        protected void BindManageSetting()
        {
            var filterSetting = PersonalDataHelper.GetJSONConfig<string>(UserConfigNames.CONFIG_USER_PROFILE_EXPAND_COLLAPSE_FILTERS);
            var graphSetting = PersonalDataHelper.GetJSONConfig<string>(UserConfigNames.CONFIG_USER_PROFILE_EXPAND_COLLAPSE_GRAPHS);
            var riskdetectionqueueviewSetting = PersonalDataHelper.GetJSONConfig<string>(UserConfigNames.CONFIG_USER_PROFILE_RISK_DETECTION_QUEUE_VIEW);
            var showEnvironmentIndicator = PersonalDataHelper.GetJSONConfig<string>(UserConfigNames.CONFIG_USER_PROFILE_SHOW_ENVIRONMENT_INDICATOR);

            this.ConfigSetting.FilterSetting = filterSetting ?? string.Empty;
            this.ConfigSetting.GraphSetting = graphSetting ?? string.Empty;
            this.ConfigSetting.RiskDQSetting = riskdetectionqueueviewSetting ?? string.Empty;
            this.ConfigSetting.DisplayEISetting = showEnvironmentIndicator ?? string.Empty;
            //39251 – VW - Add Default Landing Page On Update My Profile Page
            this.ConfigSetting.DefaultLandingPage = PersonalDataHelper.GetJSONConfig<SecMenuItem>(UserConfigNames.CONFIG_USER_PROFILE_DEFAULT_LANDING_PAGE) ?? new SecMenuItem();

            // 44758 - VW CMS Case Type Default Preference  
            var caseTypeDefault = PersonalDataHelper.GetJSONConfig<string>(UserConfigNames.CONFIG_USER_PROFILE_CASE_TYPE_DEFAULT);
            this.ConfigSetting.CaseTypeDefault = caseTypeDefault ?? string.Empty;

            if (!string.IsNullOrEmpty(filterSetting))
            {
                RadioButtonFilterDefault.SelectedValue = filterSetting;
            }

            if (!string.IsNullOrEmpty(graphSetting))
            {
                RadioButtonGraphDefault.SelectedValue = graphSetting;
            }

            if (!string.IsNullOrEmpty(caseTypeDefault))
            {
                RadioButtonCaseTypeDefault.SelectedValue = caseTypeDefault;
            }

            if (!string.IsNullOrEmpty(riskdetectionqueueviewSetting))
            {
                RadioButtonDetectionQueueDefault.SelectedValue = riskdetectionqueueviewSetting;
            }
            if (!string.IsNullOrEmpty(showEnvironmentIndicator))
            {
                uxRadioButtonListDisplayEnvironmentIndicator.SelectedValue = showEnvironmentIndicator;
            }
            else
            {
                uxRadioButtonListDisplayEnvironmentIndicator.SelectedValue = "Yes";
            }
        }

        protected void BindDropDownQuestion()
        {
            //Get table Question
            RefTableValueCollection refs = WebServices.SecurityServices.GetRefTableValues(
                SessionManager.CurrentClient, "LoginQuestions", "ENG", SessionManager.CurrentLanguage);

            ListItemCollection lstQuestions = new ListItemCollection();
            int idx = 0;
            int currentQuestionIdx = 0;

            foreach (RefTableValue item in refs)
            {
                string questionText = item.RefTblCols[0];
                string questionValue = item.RefTblKey;
                ListItem listItem = new ListItem(questionText, questionValue);
                lstQuestions.Add(listItem);
                if (questionValue == SessionManager.CurrentUser.LoginQuestionIndex.ToString())
                    currentQuestionIdx = idx;
                idx++;
            }

            uxListValidQuestion.DataSource = lstQuestions;
            uxListValidQuestion.DataBind();
            uxListValidQuestion.SelectedIndex = currentQuestionIdx;
        }

        protected bool UpdateProfile()
        {
            User userVo = SessionManager.CurrentUser;
            if (ckChangeUserName.Checked
                && IsUserSignOn.Visible && !uxOriginalUserName.Text.Trim().Equals(uxNewUserName.Text.Trim(), StringComparison.OrdinalIgnoreCase)
                && IsNotValidByUserType()
                && (userVo.UserPasswordType == 10 || (userVo.UserPasswordType == 9 && string.IsNullOrEmpty(userVo.LoginQuestionAnswer)))
                && WebServices.SecurityServices.IsExistedUserName(userVo.ASClient, uxNewUserName.Text.Trim()))
            {
                txtNewUserNameErrMsg.Message = VeraCodeSolution.DoVeraCode(Resources.MessageManager.Field_RequireAndUnique);
                txtNewUserNameErrMsg.ShowOnLoad = true;
                lbNewUserName.CssClass = "control-label label-error js-lbNewUserName";
                return false;
            }

            //validate user can change username
            if (IsUserSignOn.Visible && !string.IsNullOrEmpty(uxNewUserName.Text.Trim()))
            {
                var newUserName = uxNewUserName.Text.Trim();
                var maxLength = GetUserNameMaxLengthForUserProfile(userVo.ASClient);
                var regularExpression = "[a-zA-Z0-9_.-]{7," + maxLength + "}";

                if (!Regex.IsMatch(newUserName, regularExpression))
                {
                    txtNewUserNameErrMsg.Message = VeraCodeSolution.DoVeraCode(GetLocalResourceObject("ManageUserASCX_Text_InvalidUsername").ToString());
                    txtNewUserNameErrMsg.ShowOnLoad = true;
                    lbNewUserName.CssClass = "control-label label-error js-lbNewUserName";
                    return false;
                }
            }

            if (uxOriginalUserName.Text.Trim().Equals(uxNewUserName.Text.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                userVo.UserID = userVo.OriginalUserID;
            }

            bool hasAlertNotification = GeneralFuncsLib.IsAlertNotification();

            string selectedValue = !string.IsNullOrEmpty(uxDefaultLandingPage.SelectedValue.ToSafeString()) ? uxDefaultLandingPage.SelectedValue.ToSafeString() : uxDefaultLandingPageSelectedValue.Value;

            if (!string.IsNullOrEmpty(selectedValue))
            {
                DataRow selectedLandingPage = SessionManager.CurrentMenuItems.Select(string.Format("MenuID = '{0}'", selectedValue.Split('_').Last())).FirstOrDefault();
                this.ConfigSetting.DefaultLandingPage = PersonalDataHelper.GetJSONConfig<SecMenuItem>(UserConfigNames.CONFIG_USER_PROFILE_DEFAULT_LANDING_PAGE) ?? new SecMenuItem();

                //42589 – VW – FIS - Password Reset and Expiration Issues
                if (string.Compare(selectedLandingPage["MenuID"].ToString(), this.ConfigSetting.DefaultLandingPage.SiteMapId.ToString()) != 0
                    || string.Compare(selectedLandingPage["MenuUrl"].ToString(), this.ConfigSetting.DefaultLandingPage.Url, StringComparison.OrdinalIgnoreCase) != 0
                    || string.Compare(selectedLandingPage["MenuPermissions"].ToString(), this.ConfigSetting.DefaultLandingPage.Permissions, StringComparison.OrdinalIgnoreCase) != 0)
                {
                    this.ConfigSetting.DefaultLandingPage.SiteMapId = Convert.ToInt32(selectedLandingPage["MenuID"].ToString());
                    this.ConfigSetting.DefaultLandingPage.Url = selectedLandingPage["MenuUrl"].ToString();
                    this.ConfigSetting.DefaultLandingPage.Permissions = selectedLandingPage["MenuPermissions"].ToString();

                    PersonalDataHelper.SaveConfigAsJSON(UserConfigNames.CONFIG_USER_PROFILE_DEFAULT_LANDING_PAGE, this.ConfigSetting.DefaultLandingPage);
                }
            }

            var isEqualFirstName = string.Compare(uxFirstName.Text.Trim(), (string.IsNullOrEmpty(userVo.UserNameFirst) ? "" : userVo.UserNameFirst.Trim()), false) == 0;

            var isEqualLastName = string.Compare(uxLastName.Text.Trim(), (string.IsNullOrEmpty(userVo.UserNameLast) ? "" : userVo.UserNameLast.Trim()), false) == 0;

            var isEqualEmail = string.Compare(uxEmail.Text.Trim(), (string.IsNullOrEmpty(userVo.Email) ? "" : userVo.Email.Trim()), false) == 0;

            var isEqualPhone = !hasAlertNotification || (string.Compare(uxPhone.Text.Trim(), (string.IsNullOrEmpty(userVo.PhoneForSMS) ? "" : userVo.PhoneForSMS.Trim()), false) == 0);

            var isEqualContact = !hasAlertNotification || (string.Compare(uxEmailContact.Text.Trim(), (string.IsNullOrEmpty(userVo.ContactEmail) ? "" : userVo.ContactEmail.Trim()), false) == 0);

            var isEqualUserName = (string.Compare(uxNewUserName.Text.Trim(), (string.IsNullOrEmpty(userVo.UserID) ? "" : userVo.UserID.Trim()), false) == 0);

            if (isEqualFirstName && isEqualLastName && isEqualEmail && isEqualPhone && isEqualContact && isEqualUserName &&
                !uxCheckChangePass.Checked && !uxCheckChangeQuestion.Checked
                && this.ConfigSetting.FilterSetting.Equals(RadioButtonFilterDefault.SelectedValue, StringComparison.OrdinalIgnoreCase)
                && this.ConfigSetting.CaseTypeDefault.Equals(RadioButtonCaseTypeDefault.SelectedValue, StringComparison.OrdinalIgnoreCase)
                && this.ConfigSetting.GraphSetting.Equals(RadioButtonGraphDefault.SelectedValue, StringComparison.OrdinalIgnoreCase)
                && this.ConfigSetting.RiskDQSetting.Equals(RadioButtonDetectionQueueDefault.SelectedValue, StringComparison.OrdinalIgnoreCase)
                && this.ConfigSetting.DisplayEISetting.Equals(uxRadioButtonListDisplayEnvironmentIndicator.SelectedValue, StringComparison.OrdinalIgnoreCase)
                && this.ConfigSetting.SetDefaultTimeZone == ckTimeZone.Checked
                && (drSysTimeZone.SelectedValue.Equals(TimeZoneHandler.CurrentSettingKey(), StringComparison.OrdinalIgnoreCase))
                )
            {
                //If user don't change any thing ==> return           
                ShowMessageBox(MSG_UpdateSuccessful, true);
                return false;
            }

            if (!IsGRP_TOTAL && !ValidInputEmailFirstLast())
            {
                return false;
            }

            int errorFlag;
            //Update Password 
            if (uxCheckChangePass.Checked)
            {
                string msg = string.Empty;
                string breakLine = "</br>";
                if (!IsPwdValidationSubmit(UserProfileConstants.KEY_MIN_PASSWORD_LENGTH))
                {
                    msg += breakLine + GetPwdValidationRuleToMessage(UserProfileConstants.KEY_MIN_PASSWORD_LENGTH);
                }

                if (!IsPwdValidationSubmit(UserProfileConstants.KEY_MAX_PASSWORD_LENGTH))
                {
                    msg += breakLine + GetPwdValidationRuleToMessage(UserProfileConstants.KEY_MAX_PASSWORD_LENGTH);
                }

                if (!string.IsNullOrEmpty((WebSiteSettings.PwdValidationRule.Get("NumberOfNumberic")).Key)
                    && GeneralFuncsLib.LatestContaintNumberic(uxPassword.Text.Trim()) < (WebSiteSettings.PwdValidationRule.Get("NumberOfNumberic")).Value)
                {
                    msg += breakLine + (WebSiteSettings.PwdValidationRule.Get("NumberOfNumberic")).Msg;
                }

                if (!string.IsNullOrEmpty((WebSiteSettings.PwdValidationRule.Get("NumberOfUpperCaseCharacters")).Key)
                    && GeneralFuncsLib.LatestContaintUpperCase(uxPassword.Text.Trim()) < (WebSiteSettings.PwdValidationRule.Get("NumberOfUpperCaseCharacters")).Value)
                {
                    msg += breakLine + (WebSiteSettings.PwdValidationRule.Get("NumberOfUpperCaseCharacters")).Msg;
                }

                if (!string.IsNullOrEmpty((WebSiteSettings.PwdValidationRule.Get("NumberOfLowerCaseCharacters")).Key)
                    && GeneralFuncsLib.LatestContaintLowerCase(uxPassword.Text.Trim()) < (WebSiteSettings.PwdValidationRule.Get("NumberOfLowerCaseCharacters")).Value)
                {
                    msg += breakLine + (WebSiteSettings.PwdValidationRule.Get("NumberOfLowerCaseCharacters")).Msg;
                }

                if (!string.IsNullOrEmpty((WebSiteSettings.PwdValidationRule.Get("NumberOfSpecialCharacters")).Key))
                {
                    if ((WebSiteSettings.PwdValidationRule.Get("NumberOfSpecialCharacters")).Value == 0)
                    {
                        if (GeneralFuncsLib.LatestContaintSpecialChar(uxPassword.Text.Trim()) > (WebSiteSettings.PwdValidationRule.Get("NumberOfSpecialCharacters")).Value)
                        {
                            msg += breakLine + (WebSiteSettings.PwdValidationRule.Get("NumberOfSpecialCharacters")).Msg;
                        }
                    }
                    else
                    {
                        if (GeneralFuncsLib.LatestContaintSpecialChar(uxPassword.Text.Trim()) < (WebSiteSettings.PwdValidationRule.Get("NumberOfSpecialCharacters")).Value)
                        {
                            msg += breakLine + (WebSiteSettings.PwdValidationRule.Get("NumberOfSpecialCharacters")).Msg;
                        }

                    }
                }

                if (!string.IsNullOrEmpty(msg))
                {
                    errorPassword.InnerHtml = msg.Substring(5, msg.Length);
                    ShowMessageBox(msg, txtNewPassLabel, txtNewPassErrMsg);
                    return false;
                }

                errorFlag = WebServices.SecurityServices.ChangeUserPassword(
                       SessionManager.CurrentUser.ASClient, SessionManager.CurrentUser.UserID, uxOldPass.Text,
                       uxPassword.Text, SessionManager.UniqueSessionID);

                if (errorFlag == 1)
                {
                    if (!string.IsNullOrEmpty(SessionManager.PasswordTemporary))
                    {
                        uxFlagLoginLink.Value = "1";
                    }
                    ShowMessageBox(GetLocalResourceObject("UserProfile_ascx_cs_InvalidOldPassword").ToString());

                    return false;
                }
                else if (errorFlag == 3)
                {

                    ShowMessageBox((WebSiteSettings.PwdValidationRule.Get("MinimumPasswordAge")).Msg);
                    return false;
                }
                else
                {
                    if (errorFlag == 2)
                    {
                        if (!string.IsNullOrEmpty((WebSiteSettings.PwdValidationRule.Get("MinPasswordHistory")).Key))
                        {
                            ShowMessageBox((WebSiteSettings.PwdValidationRule.Get("MinPasswordHistory")).Msg);
                        }
                        else
                        {
                            ShowMessageBox(GetLocalResourceObject("UserProfile_ascx_cs_SameOldPassword").ToString());
                        }
                        return false;
                    }
                }
            }
            string encryptanswer = "";
            string originalAnswer = string.Empty;
            string newAnswer = string.Empty;
            if (uxCheckChangeQuestion.Checked)
            {
                if (!ValidInputAnswer())
                {
                    return false;
                }
                originalAnswer = WebServices.SecurityServices.DecryptText(userVo.LoginQuestionAnswer);
                newAnswer = uxAnswer.Text.Trim();
                encryptanswer = WebServices.SecurityServices.EncryptText(uxAnswer.Text.Trim());
                userVo.LoginQuestionAnswer = AS.Common.DataProtection.Cryptophy.EncryptText(uxAnswer.Text.Trim()); //for pci
                userVo.LoginQuestionIndex = Convert.ToInt32(uxListValidQuestion.SelectedValue);
            }
            userVo.UpdatedBy = SessionManager.CurrentUser.RecId;
            if (!IsGRP_TOTAL)
            {
                userVo.UserNameFirst = uxFirstName.Text;
                userVo.UserNameLast = uxLastName.Text;
                userVo.UserNameFull = uxFirstName.Text + " " + uxLastName.Text;
                userVo.Email = uxEmail.Text;
                if (GeneralFuncsLib.IsAlertNotification())
                {
                    userVo.PhoneForSMS = uxPhone.Text;
                    userVo.ContactEmail = uxEmailContact.Text;
                }
                else
                {
                    userVo.PhoneForSMS = null;
                    userVo.ContactEmail = null;
                }
                if (uxNewUserName.Visible)
                {
                    userVo.OriginalUserID = uxOriginalUserName.Text;
                    userVo.UserID = uxNewUserName.Text;
                }
            }

            userVo.SalesRepCode = null;
            userVo.Organizations = null;
            if (uxCheckChangeQuestion.Checked) userVo.LoginQuestionAnswer = encryptanswer;
            SessionManager.CurrentUser.UserPasswordType = 8;
            WebServices.SecurityServices.UpdateUser(userVo, 0, "".Split('.'), originalAnswer, newAnswer);

            SyncUserWithPCI(userVo);

            BindMSChangeUserUI();

            UpdateManagerSetting();

            SessionManager.CurrentUser = userVo;

            SharedSessionManager.UserNameFull = userVo.UserNameFull;

            //Update user at 1099K
            bool _IsSynchToCompliAssure = GeneralFuncsLib.GetDataOfExtendedSetting("COMPLIASSURE_USER_SYNCH").Equals("true");
            if (_IsSynchToCompliAssure)
            {
                SyncUserWith1099K(userVo);
            }

            //42589 – VW – FIS - Password Reset and Expiration Issues
            if (SessionManager.PasswordExpiredNearly && uxCheckChangePass.Checked)
            {
                SessionManager.PasswordExpiredNearly = false;
                uxWarning3.Visible = false;
            }

            uxCheckChangePass.Checked = false;
            uxCheckChangeQuestion.Checked = false;

            UpdateTimeZone();
            SendAlertEmail();

            SessionManager.PasswordTemporary = string.Empty;
            if (SessionManager.ForceChangePassword)
            {
                SessionManager.ForceChangePassword = false;
                SharedSessionManager.PasswordExpired = SessionManager.ForceChangePassword;
                //Reload header menu 
                SessionManager.GetHeaderMenu();

                string redirectUrl = GeneralFuncsLib.GetDefaultPageOfLoggedInUser(Page);
                Response.Redirect(redirectUrl);
            }

            ShowMessageBox(MSG_UpdateSuccessful, true);

            return true;
        }

        private bool IsNotValidByUserType()
        {
            bool isNotValid = isSecondaryUser;
            if (!isSecondaryUser)
            {
                isNotValid = GeneralFuncsLib.GetDataOfExtendedSetting("ENABLE_UPDATE_USER_PROFILE_MS_PRI").ToLower() == "true";
            }

            return !isNotValid;
        }
        private bool ValidInputAnswer()
        {
            if (uxListValidQuestion.SelectedValue.Length == 0)
            {
                return false;
            }

            if (uxAnswer.Text.Trim().Length < 1 ||
                uxAnswer.Text.Trim().Length > 50)
            {
                ShowMessageBox(MSG_LengNotValid, txtAnswerLabel, txtAnswerErrMsg);
                return false;
            }
            return true;
        }

        private bool ValidInputEmailFirstLast()
        {
            Regex reg = new Regex(@"^\w+([-+.']+\w+)*@\w+([-.]+\w+)*\.\w+([-.]\w+)*$");
            if (uxEmail.Text.Trim().Length > 0 && !checkExpression(reg, uxEmail.Text.Trim()))
            {
                ShowMessageBox(String.Format(Resources.UserMaintenanceMessage.InvalidFormat, GetLocalResourceObject("UserProfile_ascx_cs_Email").ToString()), txtEmailLabel, txtEmailErrMsg);
                return false;
            }

            if (uxEmailContactContainer.Visible && uxEmailContact.Text.Trim().Length > 0 && !checkExpression(reg, uxEmailContact.Text.Trim()))
            {
                ShowMessageBox(String.Format(Resources.UserMaintenanceMessage.InvalidFormat, GetLocalResourceObject("UserProfile_ascx_cs_Email").ToString()),
                    txtEmailContactLabel, txtEmailContactErrMsg);
                return false;
            }

            if ((uxFirstName.Enabled && (uxFirstName.Text.Trim().Length < 1 || uxFirstName.Text.Trim().Length > 50)) ||
                (uxLastName.Enabled && (uxLastName.Text.Trim().Length < 1 || uxLastName.Text.Trim().Length > 50)))
            {
                ShowMessageBox(string.Format(Resources.UserMaintenanceMessage.InvalidFormat, GetLocalResourceObject("UserProfile_ascx_cs_FirstName").ToString()), txtFirstNameLabel, txtFirstNameErrMsg);

                return false;
            }
            return true;
        }

        bool checkExpression(Regex reg, string str_value)
        {
            return (reg.IsMatch(str_value) && reg.Matches(str_value)[0].Length == str_value.Length);
        }

        private void ShowMessageBox(string message, bool reload = false)
        {
            if (reload)
                message = string.Format("alert('{0}');  window.location.href = window.location.href; ", message).ToString();
            else
                message = string.Format("alert('{0}');", message).ToString();
            Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowAlerMessage", message, true);
        }

        private void ShowMessageBox(string message, ValidatorLabel lbctrl, ValidatorMessage msgctrl)
        {

            ((NonReportPage)this.Page).ShowServerErrorMessage(msgctrl, message);
            msgctrl.ShowOnLoad = true;
            if (!string.IsNullOrEmpty(message))
            {
                lbctrl.CssClass = "control-label label-error";
            }
            else
            {
                lbctrl.CssClass = "control-label";
            }
        }

        private void SyncUserWithPCI(User user)
        {
            bool isSynchToPCI = GeneralFuncsLib.GetDataOfExtendedSetting("PCI_USER_SYNCH").Equals("true");
            if (isSynchToPCI && (SessionManager.CurrentUserPermissions.Contains(SITE_ACCESS_PCI_ADMIN_PERMISSIONCODE) ||
                SessionManager.CurrentUserPermissions.Contains(HIERARCHY_SITE_ACCESS_PCI_ADMIN) || SessionManager.CurrentUserPermissions.Contains(MERCHANT_SITE_ACCESS_PCI_ADMIN))) // For Orion CS and MS Pri only/ For SNET CS
            {
                FilterParameterCollection parameterIn;
                FilterParameterCollection parameterOut;
                //Check user name  
                var checkUserResponse = PCIServiceClient.Instance.GetUsers(SessionManager.CurrentClient, new AS.VW.PCI.Api.Client.Models.Requests.GetUsersRequest
                {
                    ASClient = user.ASClient,
                    UserName = user.OriginalUserID
                });
                var dtCheckUser = checkUserResponse != null ? checkUserResponse.Data : null;

                if (dtCheckUser != null)
                {
                    Guid userRecId = new Guid(dtCheckUser.RecId);
                    //Do update user info
                    parameterIn.Clear();
                    parameterOut.Clear();
                    parameterIn.Add(new FilterParameter("@RecId", userRecId, DbType.Guid));
                    parameterIn.Add(new FilterParameter("@UserName", user.OriginalUserID, DbType.String));
                    parameterIn.Add(new FilterParameter("@ASClient", user.ASClient, DbType.Int32));
                    parameterIn.Add(new FilterParameter("@SystemId", 0, DbType.Int32));
                    parameterIn.Add(new FilterParameter("@UserNameFirst", user.UserNameFirst, DbType.String));
                    parameterIn.Add(new FilterParameter("@UserNameLast", user.UserNameLast, DbType.String));
                    parameterIn.Add(new FilterParameter("@UserNameFull", user.UserNameFull, DbType.String));
                    parameterIn.Add(new FilterParameter("@UserPasswordType", user.UserPasswordType, DbType.String));
                    parameterIn.Add(new FilterParameter("@Email", null, DbType.String));
                    if (uxCheckChangeQuestion.Checked)
                    {
                        parameterIn.Add(new FilterParameter("@LoginQuestionIndex", user.LoginQuestionIndex, DbType.Int32));
                        parameterIn.Add(new FilterParameter("@LoginQuestionAnswer", user.LoginQuestionAnswer, DbType.String));
                    }
                    PciWebServices.PciReportServices.ExecuteNonQueryCommand("spa_SEC_UpdateDDSUser", parameterIn, out parameterOut);

                    if (uxCheckChangePass.Checked)
                    {
                        //Update user password
                        parameterIn.Clear();
                        parameterOut.Clear();
                        parameterIn.Add(new FilterParameter("@ASClient", user.ASClient, DbType.Int32));
                        parameterIn.Add(new FilterParameter("@UserName", user.OriginalUserID, DbType.String));
                        parameterIn.Add(new FilterParameter("@UserPasswordType", 8, DbType.Int32));
                        parameterIn.Add(new FilterParameter("@NewPassword", uxPassword.Text, DbType.AnsiString));
                        PciWebServices.PciReportServices.ExecuteNonQueryCommand("spa_SEC_ResetUserPassword", parameterIn, out parameterOut);
                    }
                }
            }

        }

        private void SyncUserWith1099K(User user)
        {
            SecurityService service = new SecurityService(SessionManager.CurrentClient);
            var updatetUser = new UserModel()
            {
                ClientID = SessionManager.CurrentClient,
                UserName = user.OriginalUserID,
                FirstName = user.UserNameFirst,
                LastName = user.UserNameLast,
                Email = user.Email,
                ActiveStatus = !string.IsNullOrEmpty(user.ActvStat) ? int.Parse(user.ActvStat) : 0,
                UserType = user.UserType,
                Answer = user.LoginQuestionAnswer
            };

            service.UpdateUser(updatetUser, 0);
        }
        private void BindNote()
        {
            string emailNote = GeneralFuncsLib.GetDataOfExtendedSetting("USER_PROFILE_EMAIL_NOTE");
            string confirmPassNote = GeneralFuncsLib.GetDataOfExtendedSetting("USER_PROFILE_CONFIRM_PASSWORD_NOTE");

            if (!emailNote.IsNullOrEmpty())
            {
                uxPanelEmailNote.Visible = true;
                uxEmailNote.Text = VeraCodeSolution.DoVeraCode(emailNote);
            }
            if (!confirmPassNote.IsNullOrEmpty())
            {
                uxPanelConfirmPassNote.Visible = true;
                uxConfirmPassNote.Text = VeraCodeSolution.DoVeraCode(confirmPassNote);
            }
        }

        private void UpdateManagerSetting()
        {
            PersonalDataHelper.SaveConfigAsJSON(UserConfigNames.CONFIG_USER_PROFILE_EXPAND_COLLAPSE_FILTERS, RadioButtonFilterDefault.SelectedValue);
            Response.Cookies["FilterCollapseState"].Value = RadioButtonFilterDefault.SelectedValue == "Expand" ? "0" : "1";
            PersonalDataHelper.SaveConfigAsJSON(UserConfigNames.CONFIG_USER_PROFILE_EXPAND_COLLAPSE_GRAPHS, RadioButtonGraphDefault.SelectedValue);
            PersonalDataHelper.SaveConfigAsJSON(UserConfigNames.CONFIG_USER_PROFILE_RISK_DETECTION_QUEUE_VIEW, RadioButtonDetectionQueueDefault.SelectedValue);
            PersonalDataHelper.SaveConfigAsJSON(UserConfigNames.CONFIG_USER_PROFILE_SHOW_ENVIRONMENT_INDICATOR, uxRadioButtonListDisplayEnvironmentIndicator.SelectedValue);
            Response.Cookies["ShowEnvironmentIndicator"].Value = uxRadioButtonListDisplayEnvironmentIndicator.SelectedValue;
            //  44758 - VW CMS Case Type Default Preference  
            PersonalDataHelper.SaveConfigAsJSON(UserConfigNames.CONFIG_USER_PROFILE_CASE_TYPE_DEFAULT, RadioButtonCaseTypeDefault.SelectedValue);

        }

        private void DisabledUpdateUser()
        {
            RemoveClientValidationRule("Validator1", "uxLastName");
            uxFirstName.Enabled = false;
            uxLastName.Enabled = false;
            uxEmail.Enabled = false;
            tdEmailDisabled.Visible = tdFirstNameDisabled.Visible = tdLastNameDisabled.Visible = true;

            uxFirstNameDisabled.Text = uxFirstName.Text;
            uxLastNameDisabled.Text = uxLastName.Text;
            uxEmailDisabled.Text = uxEmail.Text;

            tdEmail.Visible = tdFirstName.Visible = tdLastName.Visible = false;
            uxPhone.Enabled = uxEmailContact.Enabled = false;
        }

        private void RemoveClientValidationRule(string validatorID, string controlIds)
        {
            string[] controlIdList = controlIds.Split(',');
            foreach (string id in controlIdList)
            {
                Validator validator = (Validator)this.FindControl(validatorID);
                for (int i = 0; i < validator.Items.Count; i++)
                {
                    if (!(validator.Items[i] is BasicValidationItem))
                    {
                        continue;
                    }
                    BasicValidationItem basicValidationItem = (BasicValidationItem)validator.Items[i];
                    if (basicValidationItem.ControlToValidateID.Equals(id, StringComparison.OrdinalIgnoreCase))
                        validator.Items.Remove(basicValidationItem);
                }
            }
        }

        //39251 – VW - Add Default Landing Page On Update My Profile Page
        private void initDefaultLandingPage()
        {
            SecMenuItemCollection menuItems = WebServices.SecurityServices.GetMenuItemsByUser(
                SessionManager.CurrentUser.ASClient,
                SessionManager.CurrentUser.UserID,
                SessionManager.CurrentUserRoles[0].HierarchyID,
                SessionManager.CurrentLanguage, false);

            string[] pes = WebSiteConstants.SEC_PERMISSION_CaseMgt.Split(',');
            var menus = menuItems.IsNotNullData() ? menuItems.Cast<SecMenuItem>() : new List<SecMenuItem>();
            for (int i = menuItems.Count - 1; i >= 0; i--)
            {
                var permissionCode = menuItems[i].Permissions.Trim();

                if (WebSiteConstants.SEC_PERMISSION_VIEW_NOTIFICATION_SETTING.Equals(permissionCode, StringComparison.OrdinalIgnoreCase))
                {
                    menuItems[i].Permissions = WebSiteConstants.SEC_PERMISSION_NOTIFICATION_ADMIN_SETTING;
                    permissionCode = WebSiteConstants.SEC_PERMISSION_NOTIFICATION_ADMIN_SETTING;
                }

                if (WebSiteConstants.SEC_PERMISSION_MAN_ROLE.Equals(permissionCode, StringComparison.OrdinalIgnoreCase)
                    || WebSiteConstants.SEC_PERMISSION_SITE_ACCESS_PCI_ADMIN.Equals(permissionCode, StringComparison.OrdinalIgnoreCase)
                    || WebSiteConstants.SEC_PERMISSION_JSACCESS.Equals(permissionCode, StringComparison.OrdinalIgnoreCase)
                    || WebSiteConstants.SEC_PERMISSION_SITE_JUMP_1099K.Equals(permissionCode, StringComparison.OrdinalIgnoreCase)
                    || WebSiteConstants.SEC_PERMISSION_CM_MY_CASES.Equals(permissionCode, StringComparison.OrdinalIgnoreCase)
                    || WebSiteConstants.SEC_PERMISSION_CM_OPEN_CASE.Equals(permissionCode, StringComparison.OrdinalIgnoreCase)
                    || WebSiteConstants.SEC_PERMISSION_CM_SEARCH_CASE.Equals(permissionCode, StringComparison.OrdinalIgnoreCase)
                    || Array.Exists<string>(pes, (Predicate<string>)delegate (string s) { return permissionCode.IndexOf(s, StringComparison.OrdinalIgnoreCase) > -1; })
                      || WebSiteConstants.SEC_PERMISSION_MANAGE_DOCUMENT_TYPES.Equals(permissionCode, StringComparison.OrdinalIgnoreCase)
                    )
                {
                    menuItems.RemoveAt(i);
                    continue;
                }

                //Remove menu is config on SitemapSetting.xml 
                SecMenuItem menu = menus.SingleOrDefault(m => m.SiteMapId == menuItems[i].Parent);
                if (SitemapHandler.IsRemoveFromSitemaps(menu.IsNullData() ? "" : menu.Title, menuItems[i].Title))
                {
                    menuItems.Remove(menuItems[i]);
                }
            }


            uxDefaultLandingPage.DataTextField = "Title";
            uxDefaultLandingPage.DataValueField = "Permissions";
            uxDefaultLandingPage.DataFieldID = "SiteMapId";
            uxDefaultLandingPage.DataFieldParentID = "Parent";

            uxDefaultLandingPage.DataSource = menuItems;
            uxDefaultLandingPage.DataBind();
            uxDefaultLandingPage.ExpandAllDropDownNodes();

            //39251 – VW - Add Default Landing Page On Update My Profile Page
            SecMenuItem defaultLandingPage = GeneralFuncsLib.GetDefaultMenuItem(this.Page);

            uxDefaultLandingPage.SelectedValue = string.Format("{0}_{1}", defaultLandingPage.Permissions, defaultLandingPage.SiteMapId);
        }

        protected void uxDefaultLandingPage_NodeDataBound(object sender, DropDownTreeNodeDataBoundEventArguments e)
        {
            SecMenuItem element = (SecMenuItem)e.DropDownTreeNode.DataItem;
            e.DropDownTreeNode.Value = e.DropDownTreeNode.Value + "_" + element.SiteMapId.ToString();
        }

        [Serializable]
        public class ManageSetting
        {
            public string FilterSetting { get; set; }
            public string GraphSetting { get; set; }
            public string RiskDQSetting { get; set; }
            public string DisplayEISetting { get; set; }
            public bool SetDefaultTimeZone { get; set; }
            //39251 – VW - Add Default Landing Page On Update My Profile Page
            public SecMenuItem DefaultLandingPage { get; set; }

            public string CaseTypeDefault { get; set; }
        }

        protected void uxLogoutButton_Click(object sender, EventArgs e)
        {
            LogOut();
        }

        protected static int GetPwdValidationRuleToValue(string key)
        {
            switch (key)
            {
                case UserProfileConstants.KEY_MIN_PASSWORD_LENGTH:
                    if (!IsPwdValidationRuleByKey(key))
                    {
                        return UserProfileConstants.USER_PASSWORD_DEFAULT_MIN_LENGTH;
                    }
                    var minVal = WebSiteSettings.PwdValidationRule.Get(key).Value;
                    return minVal <= 0 ? UserProfileConstants.USER_PASSWORD_DEFAULT_MIN_LENGTH : minVal;
                case UserProfileConstants.KEY_MAX_PASSWORD_LENGTH:
                    if (!IsPwdValidationRuleByKey(key))
                    {
                        return UserProfileConstants.USER_PASSWORD_DEFAULT_MAX_LENGTH;
                    }
                    var maxVal = WebSiteSettings.PwdValidationRule.Get(key).Value;
                    return maxVal <= 0 ? UserProfileConstants.USER_PASSWORD_DEFAULT_MAX_LENGTH : maxVal;
                default:
                    break;
            }
            if (!IsPwdValidationRuleByKey(key))
            {
                return UserProfileConstants.PWD_VALIDATION_RULE_DEFAULT_VALUE;
            }
            return WebSiteSettings.PwdValidationRule.Get(key).Value;
        }
        protected string GetPwdValidationRuleToMessage(string key)
        {
            var txtValue = GetPwdValidationRuleToValue(key);
            switch (key)
            {
                case UserProfileConstants.KEY_MIN_PASSWORD_LENGTH:
                    if (!IsPwdValidationRuleByKey(key) || string.IsNullOrEmpty(WebSiteSettings.PwdValidationRule.Get(key).Msg))
                    {
                        var msg = GetLocalResourceObject("UserProfile_ascx_cs_MinimumCharacter").ToString();
                        return string.IsNullOrEmpty(msg) ? string.Empty : string.Format(msg, txtValue);
                    }
                    break;
                case UserProfileConstants.KEY_MAX_PASSWORD_LENGTH:
                    if (!IsPwdValidationRuleByKey(key) || string.IsNullOrEmpty(WebSiteSettings.PwdValidationRule.Get(key).Msg))
                    {
                        var msg = GetLocalResourceObject("UserProfile_ascx_cs_Maximumcharacter").ToString();
                        return string.IsNullOrEmpty(msg) ? string.Empty : string.Format(msg, txtValue);
                    }
                    break;
                default:
                    break;
            }
            if (!IsPwdValidationRuleByKey(key))
            {
                return string.Empty;
            }
            return WebSiteSettings.PwdValidationRule.Get(key).Msg;
        }

        protected int GetUserNameMaxLengthForUserProfile(int? asClientId = null)
        {
            var currentASClientId = asClientId == null ? SessionManager.CurrentUser.ASClient : (int)asClientId;
            var maxLengthUserName = GeneralFuncsLib.GetUserNameMaxLengthForUserProfile(currentASClientId);
            return maxLengthUserName <= 0 ? UserProfileConstants.CUSTOM_DEFAULT_MAXLENGTH : maxLengthUserName;
        }
        #region private      
        private static bool IsPwdValidationRuleByKey(string key)
        {
            if (WebSiteSettings.PwdValidationRule == null || WebSiteSettings.PwdValidationRule.Get(key) == null)
            {
                return false;
            }
            return true;
        }
        private bool IsPwdValidationSubmit(string key, bool isIgnoreEmpty = false)
        {
            if (isIgnoreEmpty && !IsPwdValidationRuleByKey(key))
            {
                return true;
            }
            bool result = true;
            var intValue = GetPwdValidationRuleToValue(key);
            switch (key)
            {
                case UserProfileConstants.KEY_MIN_PASSWORD_LENGTH:
                    result = intValue <= uxPassword.Text.Trim().Length;
                    break;
                case UserProfileConstants.KEY_MAX_PASSWORD_LENGTH:
                    result = intValue >= uxPassword.Text.Trim().Length;
                    break;
                default:
                    break;
            }
            return result;
        }
        private void GetMessageUxPasswordRule()
        {
            var msgRule = GetLocalResourceObject("uxpasswordruleResource1.Text").ToString();
            uxpasswordrule.Text = string.IsNullOrEmpty(msgRule) ? string.Empty : string.Format(msgRule, GetPwdValidationRuleToValue(UserProfileConstants.KEY_MIN_PASSWORD_LENGTH), GetPwdValidationRuleToValue(UserProfileConstants.KEY_MAX_PASSWORD_LENGTH));
        }
        #endregion
    }
}
