using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AS.Security.WS.Entities;
using System.Collections.Generic;
using AS.Common;
using Telerik.Web.UI;
using AS.Common.DBManager;
using AS.Controls.Pages;
using AS.Tax.Security.Web.Services;
using System.Text.RegularExpressions;
using System.Text;
using AS.Common.DataProtection;
using VW.PCI.Api.Client;
using System.Reflection;
using AS.Tax.Security.Web.Services.Model;
using System.Web.UI.HtmlControls;

namespace As.VisionWeb.Web
{
    public partial class ManageUserControl : GlobalUserControl
    {
        private bool _isOk = true;
        bool _isRoleWithRiskPermission = false;
        private bool _IsRebindRiskGroupForUser = false;
        private const string SALES_REP = "Sales Rep";
        private const string ApiPermissionCode = "PMAPIAccess";
        private const string DISABLED_UPDATE_USER = "DisabledUpdateUser";

        private PermissionCollection permissionList = null;
        private const string PERMISSION_ID = "PermissionID";
        private const string PERMISSION_CODE = "PermissionCode";
        private const string PERMISSION_DESC = "Description";
        private const string IS_SECONDARY_USER = "isSecondaryUser";
        private const string PERMISSION_1099K = "SiteJump1099K";
        private const string SponsorAddDecisionsPermission = "SponsorAddDecisions";
        protected bool userSignOn = GeneralFuncsLib.GetDataOfExtendedSetting("IS_CHANGE_USERNAME_MS_SITE").ToLower().Equals("true");
        protected string PreDefineRole = GeneralFuncsLib.GetDataOfExtendedSetting("PREDIFINED_ROLE").ToString();
        private Hierarchy _hierarchy;
        private string ApiUserID = string.Empty;
        private SecurePage _page = null;
        private string ApiPermissionID
        {
            get
            {
                return (string)ViewState["ApiPermissionID"];
            }
            set
            {
                ViewState["ApiPermissionID"] = value;
            }
        }

        private string ClientEmail
        {
            get
            {
                return (string)ViewState["ClientEmail"];
            }
            set
            {
                ViewState["ClientEmail"] = value;
            }
        }
        private string ClientName
        {
            get
            {
                return (string)ViewState["ClientName"];
            }
            set
            {
                ViewState["ClientName"] = value;
            }
        }

        private bool IsActivePwd
        {
            get
            {
                return GeneralFuncsLib.GetDataOfExtendedSetting("IS_ACTIVE_PWD").ToLower().Equals("true");
            }
        }
        private int AddDecisionsPermissionId
        {
            get
            {
                var permissionId = 0;
                if (ViewState["AddDecisionsPermissionId"] != null
                    && !string.IsNullOrEmpty(ViewState["AddDecisionsPermissionId"].ToString())
                    && int.TryParse(ViewState["AddDecisionsPermissionId"].ToString(), out permissionId))
                {
                    return permissionId;
                }
                return 0;
            }
            set
            {
                ViewState["AddDecisionsPermissionId"] = value;
            }
        }
        protected bool IsFromWindowPopup
        {
            get
            {
                return _parentPage.IsSecureQueryString && _parentPage.SecureQueryString["IsPopup"] == "1";
            }
        }

        protected PermissionCollection accessPermissions = new PermissionCollection();
        protected int UserNameMaxLength = GeneralFuncsLib.GetUserNameMaxLength(SessionManager.CurrentClient);
        enum DataBindAction
        {
            BindUserDetail,
            BindRoleList,
            BindPermissionList,
            BindClientEmail
        }
        enum PostBackAction
        {
            SaveUserDetailClicked,
            RoleChanged,
            RiskGroupChanged,
            SendEmail,
        }

        enum PreDefineRoleAction
        {
            RoleChanged,
            BindUserDetail
        }
        private User _user
        {
            get { return (User)ViewState["EditedUser"]; }
            set { ViewState["EditedUser"] = value; }
        }

        protected override void OnDataBindControls(Enum type, object sender)
        {
            switch ((DataBindAction)type)
            {
                case DataBindAction.BindUserDetail:
                    {
                        BindUserDetail();
                    }
                    break;
                case DataBindAction.BindRoleList:
                    {
                        BindRoleList();
                        CheckSelectedRoleHasRiskPermission();
                        BindRiskGroup();
                    }
                    break;
                case DataBindAction.BindPermissionList:
                    {
                        BindPermissionList();
                    }
                    break;
                case DataBindAction.BindClientEmail:
                    {
                        FilterParameterCollection parameters = new FilterParameterCollection();
                        parameters.Add(new FilterParameter("@ASClient", SessionManager.CurrentClient, System.Data.DbType.Int32));
                        DataTable clientInfo = WebServices.SecurityServices.GetReports("spa_GetContactUsInfo", parameters);
                        if (clientInfo != null && clientInfo.Rows.Count > 0)
                        {
                            this.ClientEmail = clientInfo.Rows[0]["RefTblCol9"].ToString();
                            this.ClientName = clientInfo.Rows[0]["RefTblCol1"].ToString();
                        }
                    }
                    break;
            }
        }
        protected override void OnPostBackActions(Enum type, object sender)
        {
            switch ((PostBackAction)type)
            {
                case PostBackAction.SaveUserDetailClicked:
                    {
                        bool isEdit = true;

                        if (!ValidateData())
                        {
                            return;
                        }

                        string groupUserID = string.Empty;
                        if (IsUpdateMode)//modify use
                        {
                            UpdateUserProfile();
                            groupUserID = uxUsernameOld.Text;
                        }
                        else//create new secondary user
                        {
                            CreateNewSecondaryUser();
                            if (userSignOn && IsMSUser)
                            {
                                groupUserID = uxUsernameSignOn.Text;
                            }
                            else
                            {
                                groupUserID = uxUsername.Text;
                            }
                            isEdit = false;
                        }

                        if (_isOk) // Create/Update succesfully, add/remove or update Group for user, 0-remove, >0 add/update
                        {

                            FilterParameterCollection parameters = new FilterParameterCollection();
                            FilterParameterCollection outParameters;
                            if (uxRiskGroup.SelectedValue != string.Empty && uxRiskManagement.Checked)
                            {
                                parameters.Add(new FilterParameter("@GroupID", Int32.Parse(uxRiskGroup.SelectedValue), DbType.Int32));
                            }
                            else
                            {
                                parameters.Add(new FilterParameter("@GroupID", 0, DbType.Int32));
                            }
                            CheckSelectedRoleHasRiskPermission();
                            parameters.Add(new FilterParameter("@ASClient", SessionManager.CurrentUser.ASClient, DbType.Int32));
                            parameters.Add(new FilterParameter("@SiteID", SessionManager.CurrentUser.SiteID, DbType.Int32));
                            parameters.Add(new FilterParameter("@RiskUser", _isRoleWithRiskPermission, DbType.Boolean));
                            parameters.Add(new FilterParameter("@GroupUserID", groupUserID, DbType.String));
                            parameters.Add(new FilterParameter("@UpdatedBy", SessionManager.CurrentUser.RecId, DbType.Guid));
                            parameters.Add(new FilterParameter("@IsLogged", isEdit, DbType.Boolean));
                            WebServices.RiskServices.ExecuteNonQueryCommand("spa_rm_cs_AddUserGroup", parameters, out outParameters);

                            if (IsUpdateMode)//not use for create mode
                            {
                                parameters.Clear();
                                outParameters.Clear();

                                parameters.Add(new FilterParameter("@ASClient", SessionManager.CurrentUser.ASClient, DbType.Int32));
                                parameters.Add(new FilterParameter("@SiteID", SessionManager.CurrentUser.SiteID, DbType.Int32));
                                parameters.Add(new FilterParameter("UserID", groupUserID, DbType.String));
                                parameters.Add(new FilterParameter("@OldHierarchyID", int.Parse(ViewState["OldRole"].ToString()), DbType.Int32));
                                WebServices.SecurityServices.GetReports("spa_SEC_OptInOutRiskAccessForEntity", parameters);
                            }

                            uxMasterUserControl.SubmitUser(SessionManager.CurrentUser.ASClient, groupUserID, null);

                        }
                        if (IsFromWindowPopup)
                        {
                            ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript("closeMe()");
                        }
                    }
                    break;
                case PostBackAction.RoleChanged:
                    {
                        ViewState["RiskRole"] = uxRole.SelectedValue;
                        _IsRebindRiskGroupForUser = false;
                        uxRiskGroup.Items.Clear();
                        CheckSelectedRoleHasRiskPermission();

                        if (_isRoleWithRiskPermission)
                        {
                            uxRiskManagement.Visible = true;
                            uxRiskManagement.Checked = false;
                            ViewState["IsUserWithRiskPermission"] = false;
                        }
                        BindRiskGroup();
                        SetEnable1099KGroup();
                        SetEnableCheckListPer();

                        if (!string.IsNullOrEmpty(uxRole.SelectedValue) && !IsRoleHasCaseManagement(int.Parse(uxRole.SelectedValue.Split('/')[0])))
                        {
                            uxOwnershipGroupDefault.SelectedValue = "0";
                            ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript("ShowHideOwnershipGroup('none','true')");
                        }
                        else
                        {
                            ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript("ShowHideOwnershipGroup('','true')");
                        }

                        // Get data for Permission Group
                        string[] hierachyArg = uxRole.SelectedValue.Split('/');
                        SessionManager.RoleGroupPermissions = WebServices.SecurityServices.GetPermissionsInHierarchy(Convert.ToInt32(hierachyArg[0])) ?? new PermissionCollection();
                        SessionManager.UserGroupPermissions = null;
                        SessionManager.SelectedGroupPermission = null;

                        //36296 - Climate Control
                        PreDefineRoleSelected(PreDefineRoleAction.RoleChanged);
                        getAPIAccessPassword();

                        uxMasterUserControl.ReloadUser(SessionManager.CurrentUser.ASClient, null, null);

                        ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript("addCheckSpecialCharacters();");

                        ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript("hideUWApproverGroup();");
                    }
                    break;
                case PostBackAction.RiskGroupChanged:
                    ViewState["RiskGroup"] = uxRiskGroup.SelectedValue;
                    if (IsUpdateMode)
                    {
                        CheckSelectedRoleHasRiskPermission();
                        BindRiskGroup();
                    }
                    break;

                case PostBackAction.SendEmail:
                    bool isAlert = GeneralFuncsLib.GetDataOfExtendedSetting("IS_ALERT").Equals("true");
                    if (isAlert)
                    {
                        FilterParameterCollection _parames = new FilterParameterCollection();
                        _parames.AddLoggedInUserReportingParams();
                        _parames.Add(new FilterParameter("@ChangedByRecID", SessionManager.CurrentUser.RecId.ToString(), DbType.AnsiString));
                        if (uxUsernameSignOn.Visible)
                        {
                            _parames.Add(new FilterParameter("@ChangedUserID", uxUsernameSignOn.Text.Trim(), DbType.AnsiString));
                        }
                        else
                        {
                            if (IsMSUser)
                                _parames.Add(new FilterParameter("@ChangedUserID", uxuserNamePrefix.Text.Trim() + uxUsername.Text.Trim(), DbType.AnsiString));
                            else
                                _parames.Add(new FilterParameter("@ChangedUserID", uxUsername.Text.Trim(), DbType.AnsiString));
                        }
                        _parames.AddLanguageID();
                        DataTable data = WebServices.SecurityServices.GetReports("spa_GetAuditUserChangesOfUser", _parames);
                        if (uxEmail.Text.Trim() != "" && data != null && data.Rows.Count > 0)
                        {
                            //load mail template
                            bool isAlerDefault = GeneralFuncsLib.GetDataOfExtendedSetting("ALERT_DEFAULT") != "false";
                            System.IO.StreamReader mailTemplate;
                            string bradingBankName = GeneralFuncsLib.GetDataOfExtendedSetting("BRANDING_EMAIL_ALERT_UPDATE_USER");
                            string bradingTemplateAll = GeneralFuncsLib.GetDataOfExtendedSetting("BRANDING_EMAIL_ALERT_UPDATE_CS_MS_USER");
                            string isActivePwd = GeneralFuncsLib.GetDataOfExtendedSetting("IS_ACTIVE_PWD");

                            const int BANK_ENTITY_TYPE = 1;

                            if (!isActivePwd.IsNullOrEmpty() && !IsUpdateMode)
                            {
                                mailTemplate = new System.IO.StreamReader(MapPath(@"~\App_Data\tpl_AlertEmail_ActivePwd.htm"));
                            }
                            else if (!bradingTemplateAll.IsNullOrEmpty())
                            {
                                mailTemplate = new System.IO.StreamReader(MapPath(@"~\App_Data\tpl_AlertEmail_" + bradingTemplateAll + ".htm"));
                            }
                            else
                            {
                                if (((IsMSUser ? WebSiteConstants.AS_SYSTEM_MS : SessionManager.CurrentSystem) == WebSiteConstants.AS_SYSTEM_MS
                                    || IsMSUser) && !bradingBankName.IsNullOrEmpty()
                                    && GetParentEntityNumber(BANK_ENTITY_TYPE).Equals(bradingBankName, StringComparison.OrdinalIgnoreCase))
                                {
                                    mailTemplate = new System.IO.StreamReader(MapPath(@"~\App_Data\tpl_AlertEmail_" + bradingBankName + ".htm"));
                                }
                                else if (!isAlerDefault)
                                {
                                    mailTemplate = new System.IO.StreamReader(MapPath(@"~\App_Data\tpl_AlertEmail_EmailContact.htm"));
                                }
                                else
                                {
                                    mailTemplate = new System.IO.StreamReader(MapPath(@"~\App_Data\tpl_AlertEmail.htm"));
                                }
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

                            if (!IsUpdateMode)
                            {
                                mail_body = mail_body.Replace("[tpl_AlertEmail_ActivePwd_htm_ContactService]", Resources.Template.tpl_AlertEmail_ActivePwd_htm_ContactService);
                                mail_body = mail_body.Replace("[tpl_AlertEmail_ActivePwd_htm_CreateUser]", string.Format(Resources.Template.tpl_AlertEmail_ActivePwd_htm_CreateUser, SessionManager.ResetPasswordUser.UserID));
                                mail_body = mail_body.Replace("[tpl_AlertEmail_ActivePwd_htm_Link]", IsActivePwd ? BuildLinkUpdate(SessionManager.CurrentClient, SessionManager.ResetPasswordUser.UserID, SessionManager.ResetPasswordUser.UserPassword) : string.Empty);

                            }

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
                            AS.Common.Mail.SmtpMail.SendEmail(this.ClientEmail, uxEmail.Text.Trim(), GetLocalResourceObject("ManageUserCS_Text_UserChangeNotification").ToString(),
                                mail_body, WebSiteSettings.MailSettings);
                        }
                    }
                    break;
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
        private string _editUserName = "";
        private SecurePage _parentPage = null;
        const string USER_REC_ID = "recId";

        #region Properties
        public bool IsUpdateMode { get; set; }

        public bool IsMSUser
        {
            get;
            set;
        }
        public bool IsSecondaryUser
        {
            get;
            set;
        }
        #endregion

        private void CheckSelectedRoleHasRiskPermission()
        {
            if (HasRole())
            {
                int hierarchyID = Int32.Parse(uxRole.SelectedValue.Split('/')[0]);
                _isRoleWithRiskPermission =
                    IsRoleHasPermission(hierarchyID, WebSiteConstants.SEC_PERMISSION_RSK_MGMT)
                    || IsRoleHasPermission(hierarchyID, WebSiteConstants.SEC_PERMISSION_RSK_MGMT_MS);
            }
            else
            {
                _isRoleWithRiskPermission = false;
            }
        }
        private bool HasRole()
        {
            return !uxRole.SelectedValue.IsNullOrEmpty()
                && uxRole.SelectedValue.Split('/').Length > 0;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _parentPage = Page;
            if (_parentPage.IsIntruderDetected)
                return;

            uxUsernameOld.Visible = IsUpdateMode;

            if (IsUpdateMode)
                _editUserName = _parentPage.SecureQueryString["u"];

            // If this client doesn't have MSUsermanagement feature, leave it as it is
            IsMSUser = SessionManager.CurrentSystem == WebSiteConstants.AS_SYSTEM_MS;
            IsSecondaryUser = false;
            if (!IsUpdateMode && userSignOn && IsMSUser)
            {
                uxUsernameSignOn.Visible = true;
            }
            else
            {
                uxUsername.Visible = true;
            }
            if (GeneralFuncsLib.HasMSUserManagementFeature)
            {
                if (IsUpdateMode && !IsMSUser)
                {
                    // Get system of this user when the Current System is CS
                    int systemId = 0;
                    int.TryParse(_parentPage.SecureQueryString["systemId"], out systemId);
                    IsMSUser = systemId == WebSiteConstants.AS_SYSTEM_MS;
                }
                if (IsMSUser)
                {
                    bool isSecondary = true;
                    if (_parentPage.IsSecureQueryString && _parentPage.SecureQueryString[IS_SECONDARY_USER] != null)
                        bool.TryParse(_parentPage.SecureQueryString[IS_SECONDARY_USER], out isSecondary);
                    IsSecondaryUser = isSecondary;
                }
            }

            // TK26080 - WRFC - New Client Implementation - Change request
            // Add a few quick tips to the MS User Maintenance – Manage Users functionality for creating secondary users.
            bool isMsSytem = SessionManager.CurrentSystem == WebSiteConstants.AS_SYSTEM_MS;
            uxUserNameTips.Visible = isMsSytem && !IsUpdateMode;
            uxCreateUserTip.Visible = isMsSytem && !IsUpdateMode;

            if (!IsPostBack)
            {
                if (userSignOn && IsMSUser)
                {
                    IsUserSignOn.Visible = true;
                }
                else
                {
                    IsNotUserSignOn.Visible = true;
                }

                if (IsUpdateMode)
                {
                    if (!IsUserSignOn.Visible)
                    {
                        uxPrefix.Visible = false;
                    }
                    OnDataBindControls(DataBindAction.BindUserDetail);
                    BindOwnershipGroupList();

                    if (uxRole.SelectedValue != string.Empty)
                    {
                        //Processing for Case Management
                        if (IsRoleHasCaseManagement(int.Parse(uxRole.SelectedValue.Split('/')[0])))
                        {
                            ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript("ShowHideOwnershipGroup('','false')");
                        }
                        else
                            ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript("ShowHideOwnershipGroup('none','false')");

                    }
                }
                else
                {
                    BindOwnershipGroupList();
                    if (!IsUserSignOn.Visible)
                    {
                        BindPrimaryPrefix();
                    }
                    ViewState["IsUserWithRiskPermission"] = false;
                    OnDataBindControls(DataBindAction.BindRoleList);
                    if (!string.IsNullOrEmpty(uxRole.SelectedValue) && !IsRoleHasCaseManagement(int.Parse(uxRole.SelectedValue.Split('/')[0])))
                    {
                        uxOwnershipGroupDefault.SelectedValue = "0";
                        ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript("ShowHideOwnershipGroup('none','false')");
                    }
                    else
                    {
                        ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript("ShowHideOwnershipGroup('','false')");
                    }
                }
                SetEnable1099KGroup();
                OnDataBindControls(DataBindAction.BindPermissionList);
                OnDataBindControls(DataBindAction.BindClientEmail);

                if (IsMSUser)
                {
                    LinkButton assignActivityGroup = this.FindControl("uxLnkAssignActivityGroups") as LinkButton;
                    assignActivityGroup.CssClass += " display-none";
                }

                string currentRecId = _user == null ? string.Empty : _user.RecId.ToString();
                btnAddOrg.Attributes["OnClick"] = "parent.ShowPopupModalChild(1,'AddOrganizationModal.aspx?" + this.Page.BuildSecureQueryString("recId=" + currentRecId) + "','auto'); return false;";

                bool disabledUpdateUser = false;
                if (_parentPage.IsSecureQueryString && _parentPage.SecureQueryString[DISABLED_UPDATE_USER] != null)
                    bool.TryParse(_parentPage.SecureQueryString[DISABLED_UPDATE_USER], out disabledUpdateUser);
                if (disabledUpdateUser)
                {
                    DisbleAllControl(this.Page);
                    DisableClientControl();
                }

                BindApproveGroup();
            }
            string NewUserRecId = ViewState[USER_REC_ID] == null ? string.Empty : ViewState[USER_REC_ID].ToString();
            uxMasterUserControl.LoadUser(SessionManager.CurrentUser.ASClient, NewUserRecId, IsUpdateMode, null, accessPermissions);
            uxUsername.Attributes["MaxLength"] = UserNameMaxLength.ToString();
            uxUsernameSignOn.Attributes["MaxLength"] = UserNameMaxLength.ToString();
            lbUserNameDescription.Text = string.Format(GetLocalResourceObject("LiteralResource1.Text").ToString(), UserNameMaxLength);
        }
        private void UpdateOwnershipGroup(bool IsUpdate, User user)
        {
            string spName = "spa_CM_AddEditGroupUser";
            FilterParameterCollection paras = new FilterParameterCollection();
            paras.AddCaseLoggedInUserNoSite();
            paras.Add(new FilterParameter("@UserGroupID", user.RecId, DbType.Guid));
            paras.Add(new FilterParameter("@OwnerGroup", txtOwnershipGroup.Text.Trim(), DbType.String));
            paras.Add(new FilterParameter("@OwnerGroupDefault", !string.IsNullOrEmpty(uxOwnershipGroupDefault.SelectedValue) ? uxOwnershipGroupDefault.SelectedValue.ToInt() : 0, DbType.Int32));
            paras.Add(new FilterParameter("@CreatedBy", SessionManager.CurrentUser.RecId, DbType.Guid));
            paras.Add(new FilterParameter("@IsUpdate", IsUpdate, DbType.Boolean));
            paras.Add(new FilterParameter("@SiteID", user.SiteID, DbType.Int32));
            //user.SiteID

            WebServices.RiskServices.ExecuteNonQueryCommand(spName, paras, out paras);

        }

        /// <summary>
        /// BindOwnershipGroupList into uxOwnershipGroup control
        /// </summary>
        private void BindOwnershipGroupList()
        {
            if (!IsPostBack)
            {
                string spName = "spa_CM_GetOwnershipGroups";
                FilterParameterCollection _params = new FilterParameterCollection();
                if (SessionManager.CurrentClient == 22
                        && IsMSUser
                        && SessionManager.CurrentSystem == WebSiteConstants.AS_SYSTEM_CS) // For FIS only 
                {
                    _params.Add(new AS.Common.DBManager.FilterParameter("@UserID", _user.RecId, DbType.Guid));
                    _params.Add(new AS.Common.DBManager.FilterParameter("@ASClientID", SessionManager.CurrentClient, DbType.Int32));
                    if (!string.IsNullOrEmpty(_user.EntityID))
                    {
                        _params.Add(new FilterParameter("@EntityTypeID", _user.EntityType, DbType.Int32));
                        _params.Add(new FilterParameter("@EntityNumber", _user.EntityID, DbType.String));
                    }
                    _params.Add(new FilterParameter("@SiteID", _user.SiteID, DbType.Int32));

                }
                else
                {
                    Guid userRec = IsUpdateMode ? _user.RecId : SessionManager.CurrentUser.RecId;
                    _params.Add(new FilterParameter("@UserID", userRec, DbType.Guid));
                    _params.AddCaseLoggedInUser();
                }
                _params.Add(new FilterParameter("@IsUpdated", IsUpdateMode, DbType.Boolean));
                _params.Add(new FilterParameter("@Mode", 1, DbType.Boolean));

                DataTable dt = WebServices.CsReportServices.GetReports(spName, _params);
                if (dt.Rows.Count > 0)
                {
                    uxOwnershipGroupList.DataSource = dt;
                    uxOwnershipGroupDefault.DataSource = dt;
                    uxOwnershipGroupList.DataBind();
                    uxOwnershipGroupDefault.DataBind();
                }
            }

        }
        private void BindApproveGroup()
        {
            if (GeneralFuncsLib.HasUserPermission(SponsorAddDecisionsPermission))
            {
                var userName = IsUpdateMode ? _user.UserID : string.Empty;
                DataTable data = WebServices.CsReportServices.GetApproveGroup(userName);
                uxApproveGroupList.DataSource = data;
                uxApproveGroupList.DataBind();
            }
        }
        private void BindPrimaryPrefix()
        {
            if (SessionManager.UsePrimaryPrefix)
            {
                uxPrefix.Visible = true;
                string PrefixName = SessionManager.CurrentUser.EntityID + "-";
                //For SNET only
                if (SessionManager.CurrentUser.ASClient == WebSiteConstants.SNET_CLIENT
                    && SessionManager.CurrentUserType.ToString() == WebSiteEnums.UserHierarchyMode.Hierarchy.ToString())
                {
                    string[] userid = (SessionManager.CurrentUser.UserID + "-").Split('-');
                    PrefixName = userid[0] + "-";
                }
                uxuserNamePrefix.Text = PrefixName;
            }
            else
            {
                uxPrefix.Visible = false;
            }
        }
        private void BindUserDetail()
        {
            uxCreateMode.Visible = !IsUpdateMode;
            uxUsernameOld.Visible = IsUpdateMode;
            ViewState["IsUserWithRiskPermission"] = false;
            if (IsUpdateMode)
            {
                if (_parentPage.SecureQueryString["u"] != null)
                {
                    _user = WebServices.SecurityServices.GetUser(SessionManager.CurrentUser.ASClient, _editUserName);
                    ViewState[USER_REC_ID] = _user.RecId;
                    if (_user != null)
                    {
                        uxEmail.Text = VeraCodeSolution.DoVeraCode(_user.Email);
                        uxActive.Checked = _user.StatusChar == 'Y';
                        uxInactive.Checked = _user.StatusChar != 'Y';

                        uxFirstName.Text = VeraCodeSolution.DoVeraCode(_user.UserNameFirst);
                        uxLastName.Text = VeraCodeSolution.DoVeraCode(_user.UserNameLast);
                        uxUsernameOld.Text = VeraCodeSolution.DoVeraCode(_user.UserID);
                        uxSaleRepCode.Text = VeraCodeSolution.DoVeraCode(_user.SalesRepCode);
                        if (uxUsernameSignOn.Visible)
                        {
                            uxUsernameSignOn.Text = VeraCodeSolution.DoVeraCode(_user.UserID);
                        }
                        else
                        {
                            uxUsername.Text = VeraCodeSolution.DoVeraCode(_user.UserID);
                        }

                        uxHddUrlParameter.Value = "ActiveAssignmentsByLastUser_Modal.aspx?" + this.Page.BuildSecureQueryString("mode=active&UserIDFilter=" + uxUsernameOld.Text);
                        if (CheckWhetherCanBeMoved(string.Empty))
                        {
                            uxHddAllowToSetStatus.Value = "Y";
                        }
                        _IsRebindRiskGroupForUser = true;
                        BindRoleList();
                        SetEnable1099KGroup();

                        HierarchyCollection roles = WebServices.SecurityServices
                            .GetHierarchiesOfUser(
                                SessionManager.CurrentUser.ASClient,
                                _editUserName,
                                IsMSUser ? WebSiteConstants.AS_SYSTEM_MS : SessionManager.CurrentUserRoles[0].SystemId);

                        this.uxRole.SelectedValue = roles[0].HierarchyID + "/" + roles[0].HierarchyCode;
                        ViewState["RiskRole"] = roles[0].HierarchyID + "/" + roles[0].HierarchyCode;
                        ViewState["OldRole"] = roles[0].HierarchyID;
                        permissionList = WebServices.SecurityServices.GetPermissionsForUser(SessionManager.CurrentUser.ASClient, _editUserName);

                        if (permissionList != null && permissionList.Count > 0)
                        {
                            var hasRiskPermission = permissionList.Cast<Permission>().Select(x => x.PermissionCode).ToList();
                            ViewState["IsUserWithRiskPermission"] = HasRiskManagementPermission(hasRiskPermission);
                        }

                        CheckSelectedRoleHasRiskPermission();
                        BindRiskGroup();

                        if (GeneralFuncsLib.IsDisableChangeMSRoleByEntities(_user.EntityType.ToString()))
                        {
                            uxRole.Enabled = false;
                        }

                        //36296 - Climate Control                   
                        PreDefineRoleSelected(PreDefineRoleAction.BindUserDetail);
                        BindSelectedOrganization();
                        //TK: 41877 - ALICE PH2 - Climate Control Updates
                        bool isSalesRep = !string.IsNullOrEmpty(_user.SalesRepCode);
                        uxPnlStatus.Enabled = !isSalesRep;
                        uxFirstName.Enabled = !isSalesRep;
                        uxLastName.Enabled = !isSalesRep;
                        uxEmail.Enabled = !isSalesRep;
                    }
                    else
                    {
                        //intruder here
                        _parentPage.IsIntruderDetected = true;
                        _parentPage.ASPXTrackingLog.LogData4 += GetLocalResourceObject("ManageUserCS_Text_RequestedUserNotFound").ToString();

                    }
                }
                else
                {
                    //intruder here
                    _parentPage.IsIntruderDetected = true;
                    _parentPage.ASPXTrackingLog.LogData4 += GetLocalResourceObject("ManageUserCS_Text_RequireParam").ToString() + "[u]";
                }
            }
        }
        protected void uxAccessListFunction_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (permissionList != null)
                {
                    string permissionCode = ((DataRowView)e.Item.DataItem)["PermissionCode"].ToString();
                    foreach (Permission permission in permissionList)
                    {
                        if (permission.PermissionCode == permissionCode)
                        {
                            ((CheckBox)e.Item.FindControl("uxAccessFunction")).Checked = true;
                            if (HasRiskManagementPermission(permission.PermissionCode))
                            {
                                ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript("ShowHideRiskGroupRow('');");
                            }
                            break;
                        }
                    }
                }
                ((CheckBox)e.Item.FindControl("uxAccessFunction")).Attributes["permissionId"] = ((DataRowView)e.Item.DataItem)["PermissionId"].ToString();
                ((CheckBox)e.Item.FindControl("uxAccessFunction")).Attributes["permissionCode"] = ((DataRowView)e.Item.DataItem)["PermissionCode"].ToString();
            }
        }
        protected void UserControls_RiskManagement_CheckedChanged(object sender, EventArgs e)
        {
            string display = GeneralFuncsLib.CSS_DISPLAY_NONE;
            CheckBox chkPermission = (CheckBox)sender;
            if (HasRiskManagementPermission(chkPermission.Attributes["permissionCode"].ToString())
                && ((CheckBox)sender).Checked)
            {
                display = string.Empty;
                ViewState["IsUserWithRiskPermission"] = true;
                // Reset selected Risk group
                uxRiskGroup.SelectedIndex = 0;
            }

            ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript("ShowHideRiskGroupRow('" + display + "');");
        }
        private void BindRiskGroup()
        {
            string display = GeneralFuncsLib.CSS_DISPLAY_NONE;
            if (_isRoleWithRiskPermission)
            {
                if (!IsUpdateMode)
                {
                    display = GeneralFuncsLib.CSS_DISPLAY_NONE;

                }
                else
                {
                    if ((bool)ViewState["IsUserWithRiskPermission"])
                    {
                        display = string.Empty;
                    }
                }

                FilterParameterCollection parameters = new FilterParameterCollection();
                if (SessionManager.CurrentClient == 22
                        && IsMSUser
                        && SessionManager.CurrentSystem == WebSiteConstants.AS_SYSTEM_CS
                        && _user != null) // For FIS only 
                {
                    string userMode = GeneralFuncsLib.GetHierarchyInfo(_user.EntityType).UserMode;

                    parameters.Add(new FilterParameter("@UserMode", userMode, DbType.AnsiString));
                    parameters.Add(new FilterParameter("@UserID", _user.UserID, DbType.AnsiString));
                    parameters.Add(new FilterParameter("@ASClient", _user.ASClient, DbType.Int32));
                    parameters.Add(new FilterParameter("@SiteID", _user.SiteID, DbType.Int32));

                }
                else
                {
                    parameters.AddLoggedInUserReportingParams();
                }
                parameters.Add(new FilterParameter("@IsActive", 1, DbType.Int32));
                uxRiskGroup.DataSource = WebServices.RiskServices.GetReports("spa_rm_cs_GetAllGroups", parameters);
                uxRiskGroup.DataBind();
                RadComboBoxItem item = new RadComboBoxItem("\u00A0", "0");
                uxRiskGroup.Items.Insert(0, item);
                if (IsUpdateMode && _IsRebindRiskGroupForUser)
                {
                    parameters = new FilterParameterCollection();
                    parameters.Add(new FilterParameter("@ASClient", SessionManager.CurrentUser.ASClient, DbType.Int32));
                    parameters.Add(new FilterParameter("@GroupUserID", uxUsernameOld.Text.Trim(), DbType.String));
                    DataTable dt = WebServices.RiskServices.GetReports("spa_rm_cs_GetGroupByUser", parameters);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        uxRiskGroup.SelectedValue = dt.Rows[0]["GroupID"].ToString();

                    }
                    ViewState["RiskGroup"] = uxRiskGroup.SelectedValue;
                }
                else
                {
                    if (ViewState["RiskGroup"] != null)
                    {
                        uxRiskGroup.SelectedValue = ViewState["RiskGroup"].ToString();
                    }
                }

            }
            else
            {
                uxRiskManagement.Checked = uxRiskManagement.Visible = false;

            }
            ViewState["DisplayRiskGroup"] = display;
            if (IsPostBack) // for ajax postback
            {
                ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript("ShowHideRiskGroupRow('" + display + "');");
            }
            else
            {
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "ShowHideRiskGroupRow('" + display + "');", true);
            }

        }

        //Check permission Risk Management
        private void CheckRiskManagement()
        {
            PermissionCollection riskPermissions = WebServices.SecurityServices
                .GetPermissionsByUserGroupType(
                    SessionManager.CurrentUser.ASClient,
                    SessionManager.CurrentUser.UserID,
                    IsMSUser ? WebSiteConstants.AS_SYSTEM_MS : SessionManager.CurrentSystem,
                    WebSiteConstants.PERMISSION_GROUP_RISK,
                    WebSiteConstants.PERMISSION_TYPE_MENU,
                    SessionManager.CurrentLanguage);
            uxPlcRiskMng.Visible = riskPermissions.Count > 0;
        }
        private void BindPermissionList()
        {
            // Load Access function from DB
            DataTable dt = BuildAccessPermissionTable();
            PermissionCollection pers = BuildAccessPermission();

            var per_Merch = (from Permission a in pers.Cast<Permission>() where a.GroupFuncName.Equals(WebSiteConstants.PERMISSION_ACCESS_FUNC_MERCH) select a).ToList();
            var per_Risk = (from Permission a in pers.Cast<Permission>() where a.GroupFuncName.Equals(WebSiteConstants.PERMISSION_ACCESS_FUNC_RISK) select a).ToList();
            var per_Security = (from Permission a in pers.Cast<Permission>() where a.GroupFuncName.Equals(WebSiteConstants.PERMISSION_ACCESS_FUNC_SECURITY) select a).ToList();
            var per_Alice = (from Permission a in pers.Cast<Permission>() where a.GroupFuncName.Equals(WebSiteConstants.PERMISSION_ACCESS_FUNC_ALICE) select a).ToList();
            var per_External = (from Permission a in pers.Cast<Permission>() where a.GroupFuncName.Equals(WebSiteConstants.PERMISSION_ACCESS_FUNC_EXTERNAL) select a).ToList();
            var per_Notification = (from Permission a in pers.Cast<Permission>() where a.GroupFuncName.Equals(WebSiteConstants.PERMISSION_ACCESS_FUNC_NOTIFICATION) select a).ToList();
            var per_ProcessingData = (from Permission a in pers.Cast<Permission>() where a.GroupFuncName.Equals(WebSiteConstants.PERMISSION_ACCESS_FUNC_PROCESSINGDATA) select a).ToList();
            var per_DocumentType = (from Permission a in pers.Cast<Permission>() where a.GroupFuncName.Equals(WebSiteConstants.PERMISSION_ACCESS_FUNC_DOCUMENTTYPE) select a).ToList();
            //Binding TIB access function

            //43745: Load Exclude Access Permission for Aperia User.
            var excludedPermission = SessionManager.ExcludeAccessPermission;

            if (!string.IsNullOrEmpty(excludedPermission))
            {
                var excludeAccessPermissions = excludedPermission.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                per_Security = per_Security.Where(x => !excludeAccessPermissions.Contains(x.PermissionCode)).ToList();
                per_Alice = per_Alice.Where(x => !excludeAccessPermissions.Contains(x.PermissionCode)).ToList();
            }

            if (per_Merch.Count > 0)
            {
                uxAFMerchantProfileAccess.DataSource = per_Merch;
                uxAFMerchantProfileAccess.DataBind();
                uxtrMerchantProfileAccess.Attributes["class"] = uxtrMerchantProfileAccess.Attributes["class"] != null ? uxtrMerchantProfileAccess.Attributes["class"].Replace("hide", "") : "";
            }
            else
            {
                uxtrMerchantProfileAccess.Attributes["class"] = uxtrMerchantProfileAccess.Attributes["class"] + " hide";
            }
            if (per_Risk.Count > 0)
            {
                uxAFRiskAccess.DataSource = per_Risk;
                uxAFRiskAccess.DataBind();
                uxtrRiskAccess.Attributes["class"] = uxtrRiskAccess.Attributes["class"] != null ? uxtrRiskAccess.Attributes["class"].Replace("hide", "") : "";
            }
            else
            {
                uxtrRiskAccess.Attributes["class"] = uxtrRiskAccess.Attributes["class"] + " hide";
            }
            if (per_Security.Count > 0)
            {
                uxAFSecurityAccess.DataSource = per_Security;
                uxAFSecurityAccess.DataBind();
                uxtrSecurityAccess.Attributes["class"] = uxtrSecurityAccess.Attributes["class"] != null ? uxtrSecurityAccess.Attributes["class"].Replace("hide", "") : "";
            }
            else
            {
                uxtrSecurityAccess.Attributes["class"] = uxtrSecurityAccess.Attributes["class"] + " hide";
            }
            if (per_Alice.Count > 0)
            {
                uxAFAliceAccess.DataSource = per_Alice;
                uxAFAliceAccess.DataBind();
                uxtrAliceAccess.Attributes["class"] = uxtrAliceAccess.Attributes["class"] != null ? uxtrAliceAccess.Attributes["class"].Replace("hide", "") : "";
            }
            else
            {
                uxtrAliceAccess.Attributes["class"] = uxtrAliceAccess.Attributes["class"] + " hide";
            }
            if (per_External.Count > 0)
            {
                uxAFExternalAccess.DataSource = per_External;
                uxAFExternalAccess.DataBind();
                Permission item = per_External.FirstOrDefault(p => p.PermissionCode.Equals(ApiPermissionCode)) ?? new Permission();
                ApiPermissionID = item.PermissionId.ToString();
                uxtrExternalAccess.Attributes["class"] = uxtrExternalAccess.Attributes["class"] != null ? uxtrExternalAccess.Attributes["class"].Replace("hide", "") : "";
            }
            else
            {
                uxtrExternalAccess.Attributes["class"] = uxtrExternalAccess.Attributes["class"] + " hide";
            }
            //41945 - Notifications - Enhancements to Notifications Admin settings.
            if (per_Notification.Count > 0)
            {
                uxNotificationAccess.DataSource = per_Notification;
                uxNotificationAccess.DataBind();
                uxTrNotification.Attributes["class"] = uxTrNotification.Attributes["class"] != null ? uxTrNotification.Attributes["class"].Replace("hide", "") : "";
            }
            else
            {
                uxTrNotification.Attributes["class"] = uxTrNotification.Attributes["class"] + " hide";
            }
            //43534 - Document Types 
            if (per_DocumentType.Count > 0)
            {
                uxManageDocumentTypesAccess.DataSource = per_DocumentType;
                uxManageDocumentTypesAccess.DataBind();
                uxTrDocumentType.Attributes["class"] = uxTrDocumentType.Attributes["class"] != null ? uxTrDocumentType.Attributes["class"].Replace("hide", "") : "";
            }
            else
            {
                uxTrDocumentType.Attributes["class"] = uxTrDocumentType.Attributes["class"] + " hide";
            }

            //45535 - FIS - Activation Report
            if (per_ProcessingData.Count > 0)
            {
                uxProcessingDateAccess.DataSource = per_ProcessingData;
                uxProcessingDateAccess.DataBind();
                uxtrProcessingDataAccess.Attributes["class"] = uxtrProcessingDataAccess.Attributes["class"] != null ? uxtrProcessingDataAccess.Attributes["class"].Replace("hide", "") : "";
            }
            else
            {
                uxtrProcessingDataAccess.Attributes["class"] = uxtrProcessingDataAccess.Attributes["class"] + " hide";
            }

            //56972-KeyBank Shadow Underwriting 
            BindSponsorAccessPermission();

            //Set Enable Check Permition list
            SetEnableCheckListPer();

            //Set permission have been selected
            if (IsUpdateMode)
            {
                var permissionCollection = WebServices.SecurityServices.GetPermissionsForUser(SessionManager.CurrentUser.ASClient, _editUserName);

                foreach (Permission item in permissionCollection)
                {
                    SetSelectItem(item.PermissionId, uxAFMerchantProfileAccess.Items);
                    SetSelectItem(item.PermissionId, uxAFRiskAccess.Items);
                    SetSelectItem(item.PermissionId, uxAFSecurityAccess.Items);
                    SetSelectItem(item.PermissionId, uxAFAliceAccess.Items);
                    SetSelectItem(item.PermissionId, uxAFExternalAccess.Items);
                    SetSelectItem(item.PermissionId, uxNotificationAccess.Items);
                    SetSelectItem(item.PermissionId, uxManageDocumentTypesAccess.Items);
                    SetSelectItem(item.PermissionId, uxProcessingDateAccess.Items);
                    SetSelectItem(item.PermissionId, uxSponsorAccess.Items);

                    if (item.PermissionCode == SponsorAddDecisionsPermission)
                        uxApproveGroupRow.Attributes.Remove("class");
                }

                SessionManager.UserGroupPermissions = permissionCollection;
                SessionManager.SelectedGroupPermission = permissionCollection;
            }
            else
            {
                SessionManager.UserGroupPermissions = null;
                SessionManager.SelectedGroupPermission = null;
            }

            CheckRiskManagement();

            bool haveRiskManagement = false;
            int hierarchyID = 0;
            Int32.TryParse(uxRole.SelectedValue.Split('/')[0], out hierarchyID);
            var currentSystem = IsMSUser ? WebSiteConstants.AS_SYSTEM_MS : SessionManager.CurrentSystem;

            uxRiskManagement.Attributes["PermissionCode"] =
                currentSystem == WebSiteConstants.AS_SYSTEM_CS
                ? WebSiteConstants.SEC_PERMISSION_RSK_MGMT
                : WebSiteConstants.SEC_PERMISSION_RSK_MGMT_MS;

            if (IsRoleHasPermission(hierarchyID, WebSiteConstants.SEC_PERMISSION_RSK_MGMT)
                || IsRoleHasPermission(hierarchyID, WebSiteConstants.SEC_PERMISSION_RSK_MGMT_MS))
            {
                haveRiskManagement = true;
                uxRiskManagement.Checked = (bool)ViewState["IsUserWithRiskPermission"];
            }
            else
            {
                uxRiskManagement.Visible = haveRiskManagement = false;
            }

            if (dt == null || dt.Rows.Count == 0 && !haveRiskManagement || dt.Rows.Count == 0 && !uxPlcRiskMng.Visible)
            {
                uxPhdAccessFunction.CssClass = "hide";
            }

            getAPIAccessPassword();
        }
        private void BindSponsorAccessPermission()
        {
            var accessFunctions = GeneralFuncsLib.GetAdditionAccessFunctions();
            if (accessFunctions.Any())
            {
                uxSponsorAccess.DataSource = accessFunctions;
                uxSponsorAccess.DataBind();
            }
            else
            {
                uxRowSponsorAccessFunction.Attributes["class"] = "hide";
                uxApproveGroupRow.Visible = false;
            }
        }
        private void SetSelectItem(int p, ListItemCollection listItemCollection)
        {
            var t = listItemCollection.FindByValue(p.ToString());
            if (t != null)
                t.Selected = true;
        }
        private PermissionCollection BuildAccessPermission()
        {
            var currentSystem = IsMSUser ? WebSiteConstants.AS_SYSTEM_MS : SessionManager.CurrentSystem;
            string group = currentSystem == WebSiteConstants.AS_SYSTEM_CS
              ? WebSiteConstants.PERMISSION_GROUP_CS : WebSiteConstants.PERMISSION_GROUP_MS;

            // Get permission from Database
            accessPermissions = WebServices.SecurityServices
                .GetPermissionsByUserGroupType(
                    SessionManager.CurrentUser.ASClient,
                    SessionManager.CurrentUser.UserID,
                    (IsMSUser ? WebSiteConstants.AS_SYSTEM_MS : SessionManager.CurrentSystem),
                    group,
                    WebSiteConstants.PERMISSION_TYPE_ACCESS_FUNC,
                    SessionManager.CurrentLanguage);

            PermissionCollection accessPermissions2 = WebServices.SecurityServices
                .GetPermissionsByUserGroupType(
                    SessionManager.CurrentUser.ASClient,
                    SessionManager.CurrentUser.UserID,
                    (IsMSUser ? WebSiteConstants.AS_SYSTEM_MS : SessionManager.CurrentSystem),
                    WebSiteConstants.PERMISSION_GROUP_BOARDING,
                    WebSiteConstants.PERMISSION_TYPE_ACCESS_FUNC,
                    SessionManager.CurrentLanguage);

            PermissionCollection accessPermissionsRisk = WebServices.SecurityServices
                .GetPermissionsByUserGroupType(
                    SessionManager.CurrentUser.ASClient,
                    SessionManager.CurrentUser.UserID,
                    (IsMSUser ? WebSiteConstants.AS_SYSTEM_MS : SessionManager.CurrentSystem),
                    WebSiteConstants.PERMISSION_GROUP_RISK,
                    WebSiteConstants.PERMISSION_TYPE_ACCESS_FUNC,
                    SessionManager.CurrentLanguage);

            foreach (Permission per in accessPermissions2)
            {
                accessPermissions.Add(per);
            }
            foreach (Permission per in accessPermissionsRisk)
            {
                accessPermissions.Add(per);
            }
            return accessPermissions;
        }
        private DataTable BuildAccessPermissionTable()
        {
            var currentSystem = IsMSUser ? WebSiteConstants.AS_SYSTEM_MS : SessionManager.CurrentSystem;
            string group = currentSystem == WebSiteConstants.AS_SYSTEM_CS
                ? WebSiteConstants.PERMISSION_GROUP_CS : WebSiteConstants.PERMISSION_GROUP_MS;

            // Get permission from Database
            PermissionCollection permissions = WebServices.SecurityServices
                .GetPermissionsByUserGroupType(
                    SessionManager.CurrentUser.ASClient,
                    SessionManager.CurrentUser.UserID,
                    (IsMSUser ? WebSiteConstants.AS_SYSTEM_MS : SessionManager.CurrentSystem),
                    group,
                    WebSiteConstants.PERMISSION_TYPE_ACCESS_FUNC,
                    SessionManager.CurrentLanguage);

            PermissionCollection boardingPermissions = WebServices.SecurityServices
                .GetPermissionsByUserGroupType(
                    SessionManager.CurrentUser.ASClient,
                    SessionManager.CurrentUser.UserID,
                    (IsMSUser ? WebSiteConstants.AS_SYSTEM_MS : SessionManager.CurrentSystem),
                    WebSiteConstants.PERMISSION_GROUP_BOARDING,
                    WebSiteConstants.PERMISSION_TYPE_ACCESS_FUNC,
                    SessionManager.CurrentLanguage);

            DataTable dt = new DataTable();
            dt.Columns.Add(PERMISSION_ID, typeof(string));
            dt.Columns.Add(PERMISSION_CODE, typeof(string));
            dt.Columns.Add(PERMISSION_DESC, typeof(string));

            foreach (Permission per in permissions)
            {
                dt.Rows.Add(per.PermissionId.ToString(), per.PermissionCode, per.Description);
            }

            foreach (Permission per in boardingPermissions)
            {
                dt.Rows.Add(per.PermissionId.ToString(), per.PermissionCode, per.Description);
            }

            return dt;
        }

        /// <summary>
        /// Assign user to group in 1099k
        /// </summary>
        /// <param name="userId"></param>
        private void AssignUserToGroup(string userId)
        {
            Int64 groupId = 0;
            Int64.TryParse(uxGroup.SelectedValue, out groupId);

            SecurityService taxService = new SecurityService(SessionManager.CurrentClient);
            FilterParameterCollection parameters = new FilterParameterCollection();
            FilterParameterCollection outParam;
            parameters.Add("@ASClientID", SessionManager.CurrentClient, DbType.Int32);
            parameters.Add("@UserID", userId, DbType.String);
            parameters.Add("@GroupID", groupId, DbType.Int64);
            taxService.ExecuteNonQueryCommand("spa_SEC_AssignUserToGroup", parameters, out outParam);
        }

        /// <summary>
        /// Check show/hide 1099k Group dropdown list
        /// </summary>
        /// <param name="userId"></param>
        private void SetEnable1099KGroup()
        {
            if (GeneralFuncsLib.GetDataOfExtendedSetting("SHOW_1099K_GROUP") == "true")
            {
                SecurityService taxService = new SecurityService(SessionManager.CurrentClient);
                var user1099K = taxService.GetUser(SessionManager.CurrentUser.ASClient, SessionManager.CurrentUser.UserID);
                PermissionCollection enPers = null;
                string[] hierachyArg = uxRole.SelectedValue.Split('/');
                ViewState["IsShowGroup1099K"] = false;

                if (user1099K != null
                    && SessionManager.CurrentUserPermissions.IndexOf(PERMISSION_1099K) != -1
                    && hierachyArg.Length > 1)
                {
                    enPers = WebServices.SecurityServices.GetPermissionsInHierarchy(Convert.ToInt32(hierachyArg[0])) ?? new PermissionCollection();
                    List<Permission> lstPer = enPers.OfType<Permission>().ToList();
                    if (lstPer.Exists(m => m.PermissionCode == PERMISSION_1099K))
                    {
                        uxGroup.SelectedValue = null;
                        FilterParameterCollection parameters = new FilterParameterCollection();
                        parameters.Add(new FilterParameter("@ASClientID", SessionManager.CurrentClient, DbType.Int16));
                        parameters.Add(new FilterParameter("@UserRecID", user1099K.RecId.ToString().ToUpper(), DbType.String));
                        parameters.Add(new FilterParameter("@Mode", "Active", DbType.String));
                        uxGroup.DataSource = taxService.GetReports("spa_TIN1099_ManageAssignment_GetGroupUserList", parameters);
                        uxGroup.DataBind();
                        string existGroup = null;
                        if (uxUsernameSignOn.Visible)
                        {
                            existGroup = ExistGroupUser(uxUsernameSignOn.Text);
                        }
                        else
                        {
                            existGroup = ExistGroupUser(uxUsername.Text);
                        }

                        if (existGroup != null)
                            uxGroup.SelectedValue = existGroup;
                        RadComboBoxItem item = new RadComboBoxItem("\u00A0", "0");
                        uxGroup.Items.Insert(0, item);
                        ViewState["IsShowGroup1099K"] = true;
                        ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript("ShowHideGroupRow('');");
                    }
                }

                if (!ViewState["IsShowGroup1099K"].ToBoolean())
                {
                    uxGroup.Items.Clear();
                    ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript("ShowHideGroupRow('none');");
                }
            }
        }
        private string ExistGroupUser(string userId)
        {
            SecurityService taxService = new SecurityService(SessionManager.CurrentClient);
            FilterParameterCollection parameters = new FilterParameterCollection();
            parameters.Add(new FilterParameter("@ASClientID", SessionManager.CurrentClient, DbType.Int16));
            parameters.Add("@UserID", userId, DbType.AnsiString);
            DataTable dt = taxService.GetReports("spa_SEC_GetGroupByUser", parameters);
            if (dt != null && dt.Rows.Count > 0)
                return dt.Rows[0]["GroupID"].ToString();
            return null;
        }
        private bool IsHQMerchant(string merchantNumber)
        {
            FilterParameterCollection parameters = new FilterParameterCollection();
            parameters.Add("@UserID", SessionManager.CurrentUser.UserID, DbType.AnsiString);
            parameters.Add("@ASClientID", SessionManager.CurrentClient, DbType.AnsiString);
            parameters.Add("@SiteID", SessionManager.CurrentUser.SiteID, DbType.AnsiString);
            parameters.Add("@MerchantNumber", merchantNumber, DbType.AnsiString);
            var dt = WebServices.SecurityServices.GetReports("spa_GetMerchantTypeByMID", parameters);
            if (dt != null && dt.Rows.Count > 0)
                return dt.Rows[0]["ISHQMerchant"].ToBoolean();
            return false;
        }
        private void SetEnableCheckListPer()
        {
            PermissionCollection enPers = null;
            string[] hierachyArg = uxRole.SelectedValue.Split('/');
            if (hierachyArg.Length > 1)
            {
                int hierarchyId = hierachyArg[0].ToInt();
                _hierarchy = WebServices.SecurityServices.GetHierarchyById(hierarchyId);
                enPers = WebServices.SecurityServices.GetPermissionsInHierarchy(hierarchyId);

                // Get permission for role
                SessionManager.RoleGroupPermissions = enPers;

                #region 36127 -  Clear And Set Enabel
                bool isSaleRepRole = false;
                bool isPredefine = IsPredefineRole(_hierarchy.HierarchyName, _hierarchy.HierarchyCode, out isSaleRepRole);

                List<ListItem> lstListItem = new List<ListItem>();

                foreach (ListItem litem in uxAFMerchantProfileAccess.Items)
                {
                    lstListItem.Add(litem);
                }
                foreach (ListItem litem in uxAFRiskAccess.Items)
                {
                    lstListItem.Add(litem);
                }
                foreach (ListItem litem in uxAFSecurityAccess.Items)
                {
                    lstListItem.Add(litem);
                }
                foreach (ListItem litem in uxAFAliceAccess.Items)
                {
                    lstListItem.Add(litem);
                }
                foreach (ListItem litem in uxAFExternalAccess.Items)
                {
                    lstListItem.Add(litem);
                }
                foreach (ListItem litem in uxNotificationAccess.Items)
                {
                    lstListItem.Add(litem);
                }
                foreach (ListItem litem in uxSponsorAccess.Items)
                {
                    lstListItem.Add(litem);
                }
                foreach (ListItem litem in uxManageDocumentTypesAccess.Items)
                {
                    lstListItem.Add(litem);
                }
                foreach (ListItem litem in uxProcessingDateAccess.Items)
                {
                    lstListItem.Add(litem);
                }

                // 36127
                for (int i = 0; i < lstListItem.Count; i++)
                {
                    lstListItem[i].Selected = false;
                }

                #endregion

                // Get PermissionId of AddDecisions
                AddDecisionsPermissionId = 0;
                for (int k = 0; k < enPers.Count; k++)
                {
                    if (enPers[k].PermissionCode.Equals(SponsorAddDecisionsPermission))
                    {
                        AddDecisionsPermissionId = enPers[k].PermissionId;
                        break;
                    }
                }

                //Set checked for Cheklistbox
                bool isEnable = false;
                for (int i = 0; i < lstListItem.Count; i++)
                {
                    isEnable = false;
                    for (int j = 0; j < enPers.Count; j++)
                    {
                        int _permissionId = int.Parse(lstListItem[i].Value);
                        if (_permissionId == enPers[j].PermissionId)
                        {
                            isEnable = true;
                            //TK 36296 - Climate Control
                            if (isPredefine)
                            {
                                isEnable = false;
                                lstListItem[i].Selected = true;
                            }
                            break;
                        }
                    }

                    lstListItem[i].Enabled = isEnable;

                    if (!IsUpdateMode && lstListItem[i].Enabled && lstListItem[i].Text.Equals("Add/Edit Relationship Manager", StringComparison.OrdinalIgnoreCase))
                    {
                        lstListItem[i].Selected = true;
                    }

                    if (AddDecisionsPermissionId.ToString() == lstListItem[i].Value)
                    {
                        lstListItem[i].Attributes.Add("class", "js-add-decisions");
                        lstListItem[i].Attributes.Add("onclick", "onSelectedAddDecisions(this)");
                    }
                }

                if (CheckPermissionIDInList(WebSiteConstants.SEC_PERMISSION_VIEW_TASK_LIST))
                    uxLnkAssignActivityGroups.CssClass = uxLnkAssignActivityGroups.CssClass.Replace("hide", "").Trim();
                else
                    uxLnkAssignActivityGroups.CssClass += " hide";
            }
        }
        private void BindRoleList()
        {
            HierarchyCollection roles;

            if (IsMSUser)
            {
                // MS System
                // Business Logic: in MS System, we can create/edit SECONDAY MS User.
                //  (a) Getting role list of EDITING user based on the Role of LOGING User
                //      Step 1: Get Role of loging user R1 (in SEC_UserInHierarchy)
                //      Step 2: Get HierarchyLevel of Login User HL1
                //      Step 3: Get Assignable of R1 -> A1 (in SEC_AssignableHierarchy)
                //      Step 4: Filter A1 based on HL1
                //  (b) Including 
                //      HierarchyLevel <> 'MERCHANT': 'Hierarchy Referral Role' AND 'Secondary Hierarchy Users'
                //      HierarchyLevel == 'MERCHANT': 'Secondary Merchant Users' AND 'Merchant Referral Role' 
                if (SessionManager.CurrentSystem == WebSiteConstants.AS_SYSTEM_MS)
                {
                    roles = WebServices.SecurityServices.GetMSAssignableHierarchy(
                        SessionManager.CurrentClient,
                        SessionManager.CurrentUser.UserID,
                        null, null, SessionManager.CurrentSystem);
                }
                else
                {
                    // CS System
                    // Business logic: in CS System, we only EDIT MS User (PRI OR SEC)
                    //  Step 1: Get HierarchyLevel & HierarchyCode  of Editing User R1
                    //  Step 2: Get all Hierarchies which are the same level or level is null & same code
                    roles = WebServices.SecurityServices.GetMSAssignableHierarchy(
                        _user.ASClient, _user.UserID,
                        null, null, SessionManager.CurrentSystem);
                }
            }
            else
            {
                roles = WebServices.SecurityServices
                    .GetAssignableHierarchy(
                        SessionManager.CurrentUserRoles[0].HierarchyID, WebSiteEnums.UserType.CS.ToString());
            }

            foreach (Hierarchy role in roles)
            {
                uxRole.Items.Add(new RadComboBoxItem(role.HierarchyName,
                   role.HierarchyID + "/" + role.HierarchyCode));
            }
            if (!IsUpdateMode)
            {
                uxRole.SelectedIndex = 0;
            }

        }
        private void UpdateUserProfile()
        {
            string[] hierachyArg = this.uxRole.SelectedValue.Split('/'); //this array contain HierachyID + '/' + HierachyCode
            User user = WebServices.SecurityServices.GetUser(SessionManager.CurrentUser.ASClient, _editUserName);
            // 36296 - Climate Control - Check "Sales Rep Code is unique"         
            // 41877 - ALICE PH2 - Climate Control Updates 
            user.UserNameFirst = this.uxFirstName.Text.Trim();
            user.UserNameLast = this.uxLastName.Text.Trim();
            user.UserNameFull = user.UserNameFirst + " " + user.UserNameLast;
            user.Email = this.uxEmail.Text.Trim();
            string isActive = uxActive.Checked ? "1" : "0";

            // 41877 - ALICE PH2 - Climate Control Updates 
            if (!string.IsNullOrEmpty(uxSaleRepCode.Text) && IsExistsSalesRepCode(uxSaleRepCode.Text, _editUserName, out isActive, user))
            {
                txtSalesRepCodeErrMsg.Message = GetLocalResourceObject("uxSaleRepCodeUniqueValidation.Message").ToString();
                txtSalesRepCodeErrMsg.ShowOnLoad = true;
                uxSaleRepCodelable.CssClass = "control-label label-error";

                _isOk = false;
                showHideSalesRepControl();
                return;
            }

            user.Status = isActive;
            user.UpdatedBy = SessionManager.CurrentUser.RecId;
            //User Sec Role

            string[] roles = new string[1] { hierachyArg[0] };
            user.SalesRepCode = uxSaleRepCode.Text;
            //36296 - Climate Control
            user.Organizations = SessionManager.Organizations;
            UpdateOwnershipGroup(true, user);

            WebServices.SecurityServices.UpdateUser(
                user,
                IsMSUser ? WebSiteConstants.AS_SYSTEM_MS : SessionManager.CurrentSystem,
                roles, string.Empty, string.Empty);
            AddRemovePermissionsForUser(user, true);

            //Sync PCI
            if (!IsMSUser || (IsMSUser && IsSecondaryUser) || SessionManager.CurrentSystem == WebSiteConstants.AS_SYSTEM_MS)
            {
                SyncUserWithPCI(user, "");
            }

            //Sync 1099K
            if (GeneralFuncsLib.IsCompliassureUserSynch())
            {
                var isSyn1099K = GeneralFuncsLib.GetClientExtendedSetting("COMPLIASSURE_SYNCH_CUSTOM").Data;
                if (isSyn1099K != null && isSyn1099K.Equals("true"))
                {
                    if (!IsMSUser || IsHQMerchant(user.UserID))
                    {
                        SyncUserWith1099K(user);
                        //Assign User to Group
                        if (ViewState["IsShowGroup1099K"].ToBoolean())
                        {
                            AssignUserToGroup(user.UserID);
                        }
                    }
                }
                else if (!IsMSUser || (IsMSUser && IsSecondaryUser) || SessionManager.CurrentSystem == WebSiteConstants.AS_SYSTEM_MS)
                {
                    //temporary comment
                    SyncUserWith1099K(user);
                    //Assign User to Group
                    if (ViewState["IsShowGroup1099K"].ToBoolean())
                    {
                        AssignUserToGroup(user.UserID);
                    }
                }
            }
            Page.ClientScript.RegisterStartupScript(this.GetType(), "RefreshList", "closeMe(true);", true);

        }
        private void CreateNewSecondaryUser()
        {
            if (WebServices.SecurityServices.IsExistedUserName(SessionManager.CurrentUser.ASClient, uxUsernameSignOn.Text.Trim()) ||
                WebServices.SecurityServices.IsExistedUserName(SessionManager.CurrentUser.ASClient, uxuserNamePrefix.Text.Trim() + uxUsername.Text.Trim()))
            {
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "ShowHideRiskGroupRow('" + (string)ViewState["DisplayRiskGroup"] + "');", true);
                txtUserNameErrMsg.Message = AS.Common.VeraCodeSolution.DoVeraCode(Resources.MessageManager.Field_RequireAndUnique);
                txtUserNameErrMsg.ShowOnLoad = true;
                txtUsernameLabel.CssClass = "control-label label-error";
                showHideSalesRepControl();
                _isOk = false;

                return;
            }
            //change to int array
            string[] hierachyArg = this.uxRole.SelectedValue.Split('/');
            string[] roles = new string[1] { hierachyArg[0] };
            // 36296 - Climate Control - Check "Sales Rep Code is unique"
            string isActive = uxActive.Checked ? "1" : "0";
            string userID = string.Empty;
            if (uxUsernameSignOn.Visible)
            {
                userID = uxUsernameSignOn.Text.Trim();
            }
            else
            {
                userID = uxuserNamePrefix.Text + uxUsername.Text.Trim();
            }
            // 41877 - FIS ALICE PH2 - Climate Control Updates
            //create new user
            User user = new User();
            //assigne value
            user.UserID = ApiUserID = userID;

            user.UserNameFirst = this.uxFirstName.Text.Trim();
            user.UserNameLast = this.uxLastName.Text.Trim();
            user.UserNameFull = uxFirstName.Text.Trim() + " " + uxLastName.Text.Trim();
            user.Email = this.uxEmail.Text.Trim();

            if (!string.IsNullOrEmpty(uxSaleRepCode.Text) && IsExistsSalesRepCode(uxSaleRepCode.Text, userID, out isActive, user))
            {
                txtSalesRepCodeErrMsg.Message = GetLocalResourceObject("uxSaleRepCodeUniqueValidation.Message").ToString();
                txtSalesRepCodeErrMsg.ShowOnLoad = true;
                uxSaleRepCodelable.CssClass = "control-label label-error";

                _isOk = false;
                showHideSalesRepControl();
                return;
            }

            user.Status = isActive;
            user.UserPassword = GeneratePwdAPI();
            user.UserPasswordType = 10;
            user.LoginQuestionIndex = 1;
            //36296 - Climate Control
            user.SalesRepCode = uxSaleRepCode.Text;
            user.Organizations = SessionManager.Organizations;
            if (SessionManager.CurrentSystem == WebSiteConstants.AS_SYSTEM_MS)
            {
                user.UserSecRole = SessionManager.CurrentUser.UserSecRole.Replace("PRI", "SEC");
            }
            else
            {
                user.UserSecRole = SessionManager.CurrentUser.UserSecRole;
            }

            //User Sec Role
            user.CreatedBy = SessionManager.CurrentUser.RecId;
            user.ASClient = SessionManager.CurrentUser.ASClient;
            user.SiteID = SessionManager.CurrentUser.SiteID;
            WebServices.SecurityServices.CreateUser(user, roles);

            _parentPage.ASPXTrackingLog.LogData5 = GetLocalResourceObject("ManageUserCS_Text_NewPassword").ToString() + "\t" + user.UserID + "\t" + user.UserPassword;
            User newuser = WebServices.SecurityServices.GetUser(SessionManager.CurrentUser.ASClient, user.UserID);
            user.RecId = newuser.RecId;

            //41907 - VW - Aperia - Enrich User Audit Report (Billable)
            UpdateOwnershipGroup(false, newuser);
            AddRemovePermissionsForUser(user, false);

            //General password
            SessionManager.ResetPasswordUser = user;

            //Sync with 1099K
            if (GeneralFuncsLib.IsCompliassureUserSynch())
            {
                SyncUserWith1099K(newuser);
                //Assign User to Group
                if (ViewState["IsShowGroup1099K"].ToBoolean())
                {
                    AssignUserToGroup(user.UserID);
                }
            }
            //sync with PCI
            if (user != null && !string.IsNullOrEmpty(user.UserPassword))
            {
                SyncUserWithPCI(newuser, user.UserPassword);
            }
            Response.Redirect("CreateNewUser2.aspx", false);

        }
        private void AddRemovePermissionsForUser(User user, bool isLogged)
        {
            PermissionCollection permissionRisks = WebServices.SecurityServices.GetPermissionsByUserGroupType(
                                                    SessionManager.CurrentUser.ASClient,
                                                   (SessionManager.CurrentUser.UserID),
                                                    (IsMSUser ? WebSiteConstants.AS_SYSTEM_MS : SessionManager.CurrentSystem),
                                                    WebSiteConstants.PERMISSION_GROUP_RISK, "",
                                                    SessionManager.CurrentLanguage);
            // 41907
            // Remove permision in uxAFRiskAccess
            List<Permission> perList = new List<Permission>();
            if (permissionRisks != null)
            {
                foreach (Permission per in permissionRisks)
                {
                    if (uxAFRiskAccess.Items.FindByValue(per.PermissionId.ToString()) == null)
                    {
                        perList.Add(per);
                    }
                }
            }
            if (!uxRiskManagement.Checked) // Exclude all Risk permissions for this user
            {
                foreach (Permission per in perList)
                {
                    WebServices.SecurityServices.InsertExcludePermissionForUser(user.ASClient, user.UserID, per.PermissionId, DateTime.Now, DateTime.Now, SessionManager.CurrentUser.RecId, isLogged);
                }
            }
            else  // delete exclude all risk permissions for this user
            {
                foreach (Permission per in perList)
                {
                    WebServices.SecurityServices.DeleteExcludePermissionForUser(user.ASClient, user.UserID, per.PermissionId, SessionManager.CurrentUser.RecId, isLogged);
                }
            }
            #region 36127
            InsertOrDeletePermissionForUser(user, isLogged, uxAFMerchantProfileAccess);
            InsertOrDeletePermissionForUser(user, isLogged, uxAFRiskAccess);
            InsertOrDeletePermissionForUser(user, isLogged, uxAFSecurityAccess);
            InsertOrDeletePermissionForUser(user, isLogged, uxAFAliceAccess);
            InsertOrDeletePermissionForUser(user, isLogged, uxAFExternalAccess);
            InsertOrDeletePermissionForUser(user, isLogged, uxNotificationAccess);
            InsertOrDeletePermissionForUser(user, isLogged, uxProcessingDateAccess);
            InsertOrDeletePermissionForUser(user, isLogged, uxManageDocumentTypesAccess);
            InsertOrDeletePermissionForUser(user, isLogged, uxSponsorAccess);
            #endregion
            //47456 - ALICE - Issue with updating Assign to Self permission - PROD - FE
            SaveTaskPermission(user, isLogged);
            UpdateApiAccessPassword();
            UpdateApproveGroup(user);
        }
        private void UpdateApproveGroup(User user)
        {
            if (AddDecisionsPermissionId > 0 && uxSponsorAccess.Items != null && uxSponsorAccess.Items.Count > 0)
            {
                foreach (ListItem item in uxSponsorAccess.Items)
                {
                    if (item.Value == AddDecisionsPermissionId.ToString()
                        && item.Selected)
                    {
                        var selectedGroups = GetSelectedApproveGroup();
                        WebServices.CsReportServices.UpdateApproveGroup(user.UserID, user.UserNameFull, selectedGroups);
                    }
                }
            }
            else if(GeneralFuncsLib.HasShadowUnderwriting())
            {
                WebServices.CsReportServices.UpdateApproveGroup(user.UserID, user.UserNameFull, new List<int>());
            }
        }
        private List<int> GetSelectedApproveGroup()
        {
            var resultData = new List<int>();
            if (uxApproveGroupRow.Visible && uxApproveGroupList.Items.Count > 0)
            {
                int outputId;
                foreach (RepeaterItem item in uxApproveGroupList.Items)
                {
                    var group = item.FindControl("ckApproveGroup") as HtmlInputCheckBox;
                    if (group != null && group.Checked
                        && int.TryParse(group.Value, out outputId))
                    {
                        resultData.Add(outputId);
                    }
                }
            }
            return resultData;
        }
        private void SaveTaskPermission(User user, bool isLogged)
        {
            // Get permission from Database
            PermissionCollection allPermissions = WebServices.SecurityServices
                .GetPermissionsByUserGroupType(
                    SessionManager.CurrentUser.ASClient,
                    SessionManager.CurrentUser.UserID,
                    (IsMSUser ? WebSiteConstants.AS_SYSTEM_MS : SessionManager.CurrentSystem),
                    WebSiteConstants.PERMISSION_GROUP_BOARDING_TASK,
                    WebSiteConstants.PERMISSION_TYPE_ACCESS_FUNC,
                    SessionManager.CurrentLanguage);

            PermissionCollection accessPermissions2 = WebServices.SecurityServices
                .GetPermissionsByUserGroupType(
                    SessionManager.CurrentUser.ASClient,
                    SessionManager.CurrentUser.UserID,
                    (IsMSUser ? WebSiteConstants.AS_SYSTEM_MS : SessionManager.CurrentSystem),
                    WebSiteConstants.PERMISSION_GROUP_BOARDING_TASK,
                    WebSiteConstants.PERMISSION_TYPE_DATA,
                    SessionManager.CurrentLanguage);

            foreach (Permission item in accessPermissions2)
            {
                allPermissions.Add(item);
            }

            var groupPermissions = SessionManager.SelectedGroupPermission.Cast<Permission>();

            foreach (Permission p in allPermissions)
            {
                if (groupPermissions.Any(m => m.PermissionCode == p.PermissionCode))
                    WebServices.SecurityServices.DeleteExcludePermissionForUser(user.ASClient, user.UserID, p.PermissionId, SessionManager.CurrentUser.RecId, isLogged);
                else
                    WebServices.SecurityServices.InsertExcludePermissionForUser(user.ASClient, user.UserID, p.PermissionId, DateTime.Now, DateTime.Now, SessionManager.CurrentUser.RecId, isLogged);
            }
        }
        private void InsertOrDeletePermissionForUser(User user, bool isLogged, CheckBoxList ckbList)
        {
            for (int i = 0; i < ckbList.Items.Count; i++)
            {
                if (!ckbList.Items[i].Selected)
                {
                    WebServices.SecurityServices.InsertExcludePermissionForUser(user.ASClient, user.UserID, Int32.Parse(ckbList.Items[i].Value), DateTime.Now, DateTime.Now, SessionManager.CurrentUser.RecId, isLogged);
                }
                else
                {
                    WebServices.SecurityServices.DeleteExcludePermissionForUser(user.ASClient, user.UserID, Int32.Parse(ckbList.Items[i].Value), SessionManager.CurrentUser.RecId, isLogged);
                }
            }
        }
        private void SyncUserWithPCI(User user, string newPassword)
        {
            if (GeneralFuncsLib.IsSynchPCIUser())
            {
                int hierachyInPCI = 0;
                bool isHierarchyPCIAccess = GeneralFuncsLib.GetDataOfExtendedSetting("PCI_HIERARCHY_ACCESS").Equals("true");

                // FIS: Synch CS & Hierarchy Users
                // The others: Only synch CS Users
                FilterParameterCollection paras = new FilterParameterCollection();
                paras.Add(new FilterParameter("@HierarchyID", Convert.ToInt32(uxRole.SelectedValue.Split('/')[0]), DbType.Int32));
                paras.Add(new FilterParameter("@ASClientID", SessionManager.CurrentClient, DbType.Int32));

                DataTable hierarchyList = WebServices.SecurityServices.GetReports("spa_SEC_GetHierarchyInPCIById", paras);
                if (hierarchyList.Rows.Count > 0)
                {
                    int.TryParse(hierarchyList.Rows[0]["HierarchyID_PCI"].ToString(), out hierachyInPCI);
                }
                else if ((IsMSUser ? WebSiteConstants.AS_SYSTEM_MS : SessionManager.CurrentSystem) == WebSiteConstants.AS_SYSTEM_CS)
                {
                    // Old roles that does not exitsted in SEC_Hierarchy_HierarchyInPCI
                    // Because we have not updated that roles yet.
                    hierachyInPCI = int.Parse(GeneralFuncsLib.GetDataOfExtendedSetting("PCI_DEFAULT_HIERARCHY_ID_CS"));
                }
                else // MS Users
                {
                    if (isHierarchyPCIAccess && SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.Hierarchy)
                    {
                        var hierarchyUserResponse = PCIServiceClient.Instance.GetUsers(SessionManager.CurrentClient, new AS.VW.PCI.Api.Client.Models.Requests.GetUsersRequest
                        {
                            ASClient = SessionManager.CurrentClient,
                            UserName = SessionManager.CurrentUser.UserID
                        });
                        var hierarchyUser = hierarchyUserResponse != null ? hierarchyUserResponse.Data : null;
                        if (hierarchyUser != null)
                        {
                            hierachyInPCI = hierarchyUser.HierarchyId;
                        }
                    }
                    else
                    {
                        return;
                    }
                }

                var username = user.OriginalUserID;
                if (GeneralFuncsLib.GetDataOfExtendedSetting("PCI_Site_Jump_MappingPrimaryUser") == "true" && GeneralFuncsLib.GetEntityTypeMapping())
                {
                    FilterParameterCollection _param = new FilterParameterCollection();
                    _param.Add(new FilterParameter("@ASClient", SessionManager.CurrentUser.ASClient, DbType.Int32));
                    _param.Add(new FilterParameter("@UserID", user.UserID, DbType.AnsiString));
                    DataTable dtNewUserId = WebServices.SecurityServices.GetReports("spa_cs_GetSpecial_OriginalUserID ", _param);
                    if (dtNewUserId != null && dtNewUserId.Rows.Count > 0)
                    {
                        username = dtNewUserId.Rows[0]["OriginalUserID"].ToString();
                    }
                }

                FilterParameterCollection parameterIn = new FilterParameterCollection();
                FilterParameterCollection parameterOut = new FilterParameterCollection();
                var checkUserResponse = PCIServiceClient.Instance.GetUsers(SessionManager.CurrentClient, new AS.VW.PCI.Api.Client.Models.Requests.GetUsersRequest
                {
                    ASClient = user.ASClient,
                    UserName = username
                });
                var dtCheckUser = checkUserResponse != null ? checkUserResponse.Data : null;

                string actvStatus;
                if (uxActive.Checked)
                    actvStatus = "1";
                else
                    actvStatus = "0";

                if (dtCheckUser == null)
                {
                    user.RecId = Guid.Empty;
                    //Do create user
                    parameterIn.Clear();
                    parameterOut.Clear();
                    parameterIn.Add(new FilterParameter("@UserId", Guid.Empty, DbType.Guid));
                    parameterIn.Add(new FilterParameter("@ASClient", user.ASClient, System.Data.DbType.Int32));
                    parameterIn.Add(new FilterParameter("@UserName", username, System.Data.DbType.String));
                    parameterIn.Add(new FilterParameter("@UserNameFirst", user.UserNameFirst, System.Data.DbType.String));
                    parameterIn.Add(new FilterParameter("@UserNameLast", user.UserNameLast, System.Data.DbType.String));
                    parameterIn.Add(new FilterParameter("@UserNameFull", user.UserNameFull, System.Data.DbType.String));
                    parameterIn.Add(new FilterParameter("@UserPassword", newPassword, System.Data.DbType.String));
                    parameterIn.Add(new FilterParameter("@UserPasswordType", user.UserPasswordType, System.Data.DbType.String));
                    parameterIn.Add(new FilterParameter("@Email", null, System.Data.DbType.String));
                    parameterIn.Add(new FilterParameter("@LoginQuestionIndex", user.LoginQuestionIndex, System.Data.DbType.Int32));
                    parameterIn.Add(new FilterParameter("@LoginQuestionAnswer", user.LoginQuestionAnswer, System.Data.DbType.String));
                    parameterIn.Add(new FilterParameter("@ActiveStatus", actvStatus, System.Data.DbType.String));
                    parameterIn.Add(new FilterParameter("@HierarchyIds", hierachyInPCI.ToString(), System.Data.DbType.AnsiString));
                    parameterIn.Add(new FilterParameter("@CreatedBy", user.CreatedBy, System.Data.DbType.Guid));

                    parameterIn[0].IsOutParameter = true;
                    PciWebServices.PciReportServices.ExecuteNonQueryCommand("spa_SEC_CreateDDSUser", parameterIn, out parameterOut);
                    if (parameterOut.Count > 0)
                        user.RecId = new Guid(parameterOut[0].ParameterValue.ToString());
                }
                else
                {
                    user.RecId = new Guid(dtCheckUser.RecId);
                    //Do update user info
                    parameterIn.Clear();
                    parameterOut.Clear();
                    parameterIn.Add(new FilterParameter("@RecId", user.RecId, System.Data.DbType.Guid));
                    parameterIn.Add(new FilterParameter("@UserName", username, System.Data.DbType.String));
                    parameterIn.Add(new FilterParameter("@ASClient", user.ASClient, System.Data.DbType.Int32));
                    parameterIn.Add(new FilterParameter("@SystemID", 2, System.Data.DbType.Int32));
                    parameterIn.Add(new FilterParameter("@UserNameFirst", user.UserNameFirst, System.Data.DbType.String));
                    parameterIn.Add(new FilterParameter("@UserNameLast", user.UserNameLast, System.Data.DbType.String));
                    parameterIn.Add(new FilterParameter("@UserNameFull", user.UserNameFull, System.Data.DbType.String));
                    parameterIn.Add(new FilterParameter("@UserPasswordType", user.UserPasswordType, System.Data.DbType.String));
                    parameterIn.Add(new FilterParameter("@Email", null, System.Data.DbType.String));
                    parameterIn.Add(new FilterParameter("@LoginQuestionIndex", user.LoginQuestionIndex, System.Data.DbType.Int32));
                    parameterIn.Add(new FilterParameter("@LoginQuestionAnswer", user.LoginQuestionAnswer, System.Data.DbType.String));
                    parameterIn.Add(new FilterParameter("@ActvStatus", actvStatus, System.Data.DbType.String));
                    parameterIn.Add(new FilterParameter("@HierarchyIds", hierachyInPCI.ToString(), System.Data.DbType.AnsiString));

                    PciWebServices.PciReportServices.ExecuteNonQueryCommand("spa_SEC_UpdateDDSUser", parameterIn, out parameterOut);
                }

                if (user.RecId != Guid.Empty)
                {
                    //Do update user
                    //Do update sec roles
                    parameterIn.Clear();
                    parameterOut.Clear();
                    PCIServiceClient.Instance.UpdSecRoleByUserID(SessionManager.CurrentClient, new AS.VW.PCI.Api.Client.Models.Requests.UpdSecRoleByUserIDRequest
                    {
                        ASClient = user.ASClient.ToString(),
                        UserID = user.RecId.ToString(),
                        RoleID = hierachyInPCI.ToString()
                    });

                    //Do update theme          
                    int themeID = GetThemeOfPCIUser(SessionManager.CurrentUser.OriginalUserID, SessionManager.CurrentUserRoles[0].HierarchyID);
                    parameterIn.Clear();
                    parameterOut.Clear();
                    parameterIn.Add(new FilterParameter("@ASClient", user.ASClient, System.Data.DbType.Int32));
                    parameterIn.Add(new FilterParameter("@UserName", username, System.Data.DbType.String));
                    parameterIn.Add(new FilterParameter("@HierarchyId", null, System.Data.DbType.Int32));
                    parameterIn.Add(new FilterParameter("@ThemeId", themeID, System.Data.DbType.Int32));
                    PciWebServices.PciReportServices.ExecuteNonQueryCommand("spa_SEC_InsUserInTheme", parameterIn, out parameterOut);

                    ////Delete call position from exclude list
                    parameterIn.Clear();
                    parameterOut.Clear();
                    parameterIn.Add(new FilterParameter("@ClientId", user.ASClient, System.Data.DbType.Int32));
                    parameterIn.Add(new FilterParameter("@UserName", username, System.Data.DbType.String));
                    parameterIn.Add(new FilterParameter("@PermissionId", 33, System.Data.DbType.Int32)); //CallDisposition
                    PciWebServices.PciReportServices.ExecuteNonQueryCommand("spa_SEC_DelExcludePermissionItem", parameterIn, out parameterOut);

                    parameterIn.Clear();
                    parameterOut.Clear();
                    parameterIn.Add(new FilterParameter("@ClientId", user.ASClient, System.Data.DbType.Int32));
                    parameterIn.Add(new FilterParameter("@UserName", username, System.Data.DbType.String));
                    parameterIn.Add(new FilterParameter("@PermissionId", 37, System.Data.DbType.Int32)); //AdminCallDisposition
                    PciWebServices.PciReportServices.ExecuteNonQueryCommand("spa_SEC_DelExcludePermissionItem", parameterIn, out parameterOut);
                }
            }
        }
        protected void uxSave_Click(object sender, EventArgs e)
        {
            if (_parentPage.IsIntruderDetected)
                return;

            OnPostBackActions(PostBackAction.SaveUserDetailClicked, sender);

            if (_isOk)
            {
                OnPostBackActions(PostBackAction.SendEmail, sender);
            }
        }
        protected void uxRole_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            OnPostBackActions(PostBackAction.RoleChanged, sender);
        }
        protected void uxRiskGroup_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            OnPostBackActions(PostBackAction.RiskGroupChanged, sender);
        }
        private bool CheckWhetherCanBeMoved(string mode)
        {
            FilterParameterCollection parameters = new FilterParameterCollection();
            parameters.Add(new FilterParameter("UserId", uxUsernameOld.Text, DbType.String));
            if (mode != string.Empty)
            {
                parameters.Add(new FilterParameter("Mode", mode, DbType.String));
            }
            DataTable dt = WebServices.RiskServices.GetReports("spa_rm_cs_IsUserLastOnActiveAssignments", parameters);

            if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "0") // No assignment is assigned, can be moved RiskRole/RiskGroup
            {
                return true;
            }
            return false;
        }
        bool IsRoleHasPermission(int hierarchyID, string perCode)
        {
            PermissionCollection permissions = WebServices.SecurityServices.GetPermissionsInHierarchy(hierarchyID);
            foreach (Permission permission in permissions)
            {
                if (permission.PermissionCode == perCode)
                {
                    return true;
                }
            }
            return false;
        }

        #region SyncUserWith1099K
        private void SyncUserWith1099K(User visionWebUser)
        {
            SecurityService taxService = new SecurityService(SessionManager.CurrentClient);
            string hierarchyID = String.Empty;
            int actvStat = uxActive.Checked ? 1 : 0;

            string spaName = "spa_SEC_GetHierarchyIn1099KById";
            FilterParameterCollection paras = new FilterParameterCollection();
            paras.Add(new FilterParameter("@HierarchyID", Convert.ToInt32(uxRole.SelectedValue.Split('/')[0]), DbType.Int32));
            DataTable hierarchyList = WebServices.SecurityServices.GetReports(spaName, paras);
            if (hierarchyList.Rows.Count > 0)
            {
                hierarchyID = hierarchyList.Rows[0][0].ToString();
            }
            else
            {
                hierarchyID = "0";
            }
            bool isViewFullTIN = false;

            #region 36127
            for (int i = 0; i < uxAFMerchantProfileAccess.Items.Count; i++)
            {
                if (uxAFMerchantProfileAccess.Items[i].Selected
                    && uxAFMerchantProfileAccess.Items[i].Text.Equals("View Full Tax ID and SSN", StringComparison.OrdinalIgnoreCase))
                {
                    isViewFullTIN = true;
                    break;
                }
            }

            for (int i = 0; i < uxAFRiskAccess.Items.Count; i++)
            {
                if (uxAFRiskAccess.Items[i].Selected
                    && uxAFRiskAccess.Items[i].Text.Equals("View Full Tax ID and SSN", StringComparison.OrdinalIgnoreCase))
                {
                    isViewFullTIN = true;
                    break;
                }
            }

            for (int i = 0; i < uxAFSecurityAccess.Items.Count; i++)
            {
                if (uxAFSecurityAccess.Items[i].Selected
                    && uxAFSecurityAccess.Items[i].Text.Equals("View Full Tax ID and SSN", StringComparison.OrdinalIgnoreCase))
                {
                    isViewFullTIN = true;
                    break;
                }
            }

            for (int i = 0; i < uxAFAliceAccess.Items.Count; i++)
            {
                if (uxAFAliceAccess.Items[i].Selected
                    && uxAFAliceAccess.Items[i].Text.Equals("View Full Tax ID and SSN", StringComparison.OrdinalIgnoreCase))
                {
                    isViewFullTIN = true;
                    break;
                }
            }

            #endregion

            //GetThemeID        
            AS.Tax.Security.Web.Services.SecService.User taxUser = taxService.GetUser(SessionManager.CurrentClient, visionWebUser.UserID);

            if (taxUser == null)
            {
                AS.Tax.Security.Web.Services.SecService.User creator = taxService.GetUser(SessionManager.CurrentClient, SessionManager.CurrentUser.OriginalUserID);
                if (creator == null)
                    return;

                int themeID = this.GetThemeOf1099KUser(creator.UserID);
                if (themeID <= 0)
                {
                    DataTable themeInfo = taxService.GetThemeInfo(SessionManager.CurrentClient);
                    if (themeInfo.Rows.Count > 0)
                    {
                        themeID = Convert.ToInt32(themeInfo.Rows[0]["ThemeId"]);
                    }
                }
                var insertUser = new UserModel()
                {
                    ClientID = SessionManager.CurrentClient,
                    UserName = visionWebUser.UserID,
                    FirstName = visionWebUser.UserNameFirst,
                    LastName = visionWebUser.UserNameLast,
                    Email = visionWebUser.Email,
                    ActiveStatus = actvStat,
                    HierarchyIDs = hierarchyID,
                    ThemeID = themeID,
                    CreatedBy = creator.RecId,
                    UserType = 0
                };

                taxService.InsertUser(insertUser);
            }
            else
            {
                AS.Tax.Security.Web.Services.SecService.Hierarchy hierarchy = taxService.GetHierarchyOfUser(SessionManager.CurrentClient, taxUser.UserID);
                int systemId = (hierarchy == null) ? 1 : hierarchy.SystemId;

                var updatetUser = new UserModel()
                {
                    ClientID = SessionManager.CurrentClient,
                    UserName = visionWebUser.UserID,
                    FirstName = visionWebUser.UserNameFirst,
                    LastName = visionWebUser.UserNameLast,
                    Email = visionWebUser.Email,
                    ActiveStatus = actvStat,
                    HierarchyIDs = hierarchyID,
                    UserType = visionWebUser.UserType,
                    Answer = visionWebUser.LoginQuestionAnswer
                };

                taxService.UpdateUser(updatetUser, systemId);
            }
            Synch1099KPermission(visionWebUser, int.Parse(hierarchyID), isViewFullTIN);
        }
        protected void Synch1099KPermission(User visionWebUser, int hierarchyID, bool isViewFullTIN)
        {
            SecurityService taxService = new SecurityService(SessionManager.CurrentClient);
            FilterParameterCollection parameters = new FilterParameterCollection();
            FilterParameterCollection outParam;
            parameters.Add("@ASClientID", SessionManager.CurrentClient, DbType.Int32);
            parameters.Add("@UserName", visionWebUser.OriginalUserID, DbType.String);
            parameters.Add("@HiearchyID", hierarchyID, DbType.Int32);
            parameters.Add("@IsViewFullTIN", isViewFullTIN, DbType.Boolean);

            string userSecRole = string.Empty;
            DataTable dtSecRole = this.Get1099KMapping("UserSecRole");

            if (dtSecRole != null && dtSecRole.Rows.Count > 0)
            {
                var valueInGeneric = dtSecRole.Rows.Cast<DataRow>().FirstOrDefault(x => x["ValueInGeneric"].ToString().Equals(visionWebUser.UserSecRole, StringComparison.OrdinalIgnoreCase));
                userSecRole = valueInGeneric["ValueIn1099K"].ToString();
            }

            parameters.Add("@UserSecRole", userSecRole, DbType.String);
            taxService.ExecuteNonQueryCommand("spa_SEC_SynchUserFromVisionWeb", parameters, out outParam);
        }
        protected DataTable Get1099KMapping(string type)
        {
            FilterParameterCollection parameters = new FilterParameterCollection();

            parameters.Add("@UserId", SessionManager.CurrentUser.RecId, DbType.Guid);
            parameters.Add("@ASClientID", SessionManager.CurrentClient, DbType.Int32);
            parameters.Add("@Type", type, DbType.String);

            return WebServices.SecurityServices.GetReports("spa_SEC_GetMapping1099K", parameters);
        }
        protected int GetThemeOf1099KUser(string username)
        {
            int themeId = 0;
            SecurityService taxService = new SecurityService(SessionManager.CurrentClient);
            FilterParameterCollection parameters = new FilterParameterCollection();

            parameters.Add("@ASClient", SessionManager.CurrentClient, DbType.Int32);
            parameters.Add("@UserName", username, DbType.String);
            parameters.Add("@HierarchyId", 0, DbType.Int32);
            DataTable dt = taxService.GetReports("spa_SEC_GetThemeOfUser", parameters);
            if (dt.Rows.Count > 0)
            {
                themeId = int.Parse(dt.Rows[0]["ThemeID"].ToString());
            }
            return themeId;
        }
        protected int GetThemeOfPCIUser(string username, int hierarchyId)
        {
            int themeId = 0;
            FilterParameterCollection parameters = new FilterParameterCollection();

            parameters.Add("@ASClient", SessionManager.CurrentClient, DbType.Int32);
            parameters.Add("@UserName", username, DbType.String);
            parameters.Add("@HierarchyId", hierarchyId, DbType.Int32);
            DataTable dt = PciWebServices.PciReportServices.GetReports("spa_SEC_GetThemeOfUser", parameters);
            if (dt.Rows.Count > 0)
            {
                themeId = int.Parse(dt.Rows[0]["ThemeID"].ToString());
            }
            return themeId;
        }
        #endregion

        protected bool ValidateData()
        {
            if (uxRole.Text.Equals(SALES_REP) && string.IsNullOrEmpty(uxSaleRepCode.Text))
            {
                Page.IsIntruderDetected = true;
                Page.IntruderLog.LogData4 += "&uxRole=" + uxRole.SelectedValue;
                Page.RaiseIntruderEvent(IntruderType.Control);
                return false;
            }

            var regularExpression = "[a-zA-Z0-9_.-]{7," + UserNameMaxLength + "}";
            if (!string.IsNullOrEmpty(uxUsername.Text) && !IsUpdateMode && !Regex.IsMatch(uxUsername.Text, regularExpression))
            {
                Page.IsIntruderDetected = true;
                Page.IntruderLog.LogData4 += "&uxUsername=" + uxUsername.Text.Trim();
                Page.RaiseIntruderEvent(IntruderType.Control);
                return false;
            }
            if (!string.IsNullOrEmpty(uxUsernameSignOn.Text) && !IsUpdateMode && !Regex.IsMatch(uxUsernameSignOn.Text, regularExpression))
            {
                Page.IsIntruderDetected = true;
                Page.IntruderLog.LogData4 += "&uxUsername=" + uxUsernameSignOn.Text.Trim();
                Page.RaiseIntruderEvent(IntruderType.Control);
                return false;
            }
            else if (uxFirstName.Text.Trim().Length < 2)
            {
                Page.IsIntruderDetected = true;
                Page.IntruderLog.LogData4 += "&uxFirstName=" + uxFirstName.Text.Trim();
                Page.RaiseIntruderEvent(IntruderType.Control);
                return false;
            }
            else if (uxLastName.Text.Trim().Length < 2)
            {
                Page.IsIntruderDetected = true;
                Page.IntruderLog.LogData4 += "&uxLastName=" + uxLastName.Text.Trim();
                Page.RaiseIntruderEvent(IntruderType.Control);
                return false;
            }
            else if (uxEmail.Text.Trim().Length <= 0)
            {
                Page.IsIntruderDetected = true;
                Page.IntruderLog.LogData4 += "&uxEmail=" + uxEmail.Text.Trim();
                Page.RaiseIntruderEvent(IntruderType.Control);
                return false;
            }
            return true;
        }
        private bool IsRoleHasCaseManagement(int roleID)
        {
            FilterParameterCollection parameters = new FilterParameterCollection();
            FilterParameterCollection outparams = new FilterParameterCollection();
            parameters.AddCaseLoggedInUser();
            parameters.Add(new FilterParameter("@RoleID", roleID, DbType.Int32, false));
            parameters.Add(new FilterParameter("@ModifiedUser", _editUserName, DbType.AnsiString));
            parameters.Add(new FilterParameter("@HasPermission", false, DbType.Boolean, true));
            outparams.Add(new FilterParameter("@HasPermission", false, DbType.Boolean, true));
            DataTable data = WebServices.SecurityServices.GetReports("spa_CM_CheckRoleHasCasePermission", parameters);

            bool result = false;
            if (data.HasData())
            {
                bool.TryParse(data.Rows[0][0].ToString(), out result);
            }

            return result;
        }
        private static bool HasRiskManagementPermission(string permissionCode)
        {
            return permissionCode == WebSiteConstants.SEC_PERMISSION_RSK_MGMT
                || permissionCode == WebSiteConstants.SEC_PERMISSION_RSK_MGMT_MS;
        }
        private static bool HasRiskManagementPermission(List<string> permissions)
        {
            if (permissions != null && permissions.Any())
            {
                var hasRiskPermission = permissions.Any(x => x.Equals(WebSiteConstants.SEC_PERMISSION_RSK_MGMT) || x.Equals(WebSiteConstants.SEC_PERMISSION_RSK_MGMT_MS));
                return hasRiskPermission;
            }
            return false;
        }
        private bool CheckPermissionIDInList(string strPermission)
        {
            string[] hierachyArg = uxRole.SelectedValue.Split('/');
            PermissionCollection persInHierarchy = WebServices.SecurityServices.GetPermissionsInHierarchy(Convert.ToInt32(hierachyArg[0])) ?? new PermissionCollection();
            var pers = persInHierarchy.Cast<Permission>();
            if (pers.Any(m => m.PermissionCode == strPermission)
                && pers.Any(m => m.Group == WebSiteConstants.PERMISSION_GROUP_BOARDING_TASK))
                return true;
            return false;
        }

        #region 36296 - Climate Control

        private bool IsExistsSalesRepCode(string code, string userID, out string isActive, User user)
        {
            bool isExist = false;
            isActive = "0";
            string spaName = "spa_SEC_CheckSalesRep";
            FilterParameterCollection parametersInput = new FilterParameterCollection();
            parametersInput.Add("@ASClientID", SessionManager.CurrentClient, DbType.Int32);
            parametersInput.Add("@SiteID", SessionManager.CurrentUser.SiteID, DbType.Int32);
            parametersInput.Add("@LoginUserID", SessionManager.CurrentUser.RecId, DbType.Guid);
            parametersInput.Add("@UserID", userID, DbType.String);
            parametersInput.Add("@SalesRepCode", code, DbType.String);
            //41877 - FIS ALICE PH2 - Climate Control Updates (1st priority)
            parametersInput.Add("@SalesRepFirstName", user.UserNameFirst, DbType.String);
            parametersInput.Add("@SalesRepLastName", user.UserNameLast, DbType.String);
            parametersInput.Add("@SalesRepFullName", user.UserNameFull, DbType.String);
            parametersInput.Add("@Email", user.Email, DbType.String);

            DataTable dt = WebServices.SecurityServices.GetReports(spaName, parametersInput);

            if (dt.IsNotNullData())
            {
                isExist = dt.Rows[0]["IsExists"].ToBoolean();
                isActive = dt.Rows[0]["SalesRepStatus"].ToString();
                user.UserNameFirst = dt.Rows[0]["SalesRepFirstName"].ToString();
                user.UserNameLast = dt.Rows[0]["SalesRepLastName"].ToString();
                user.UserNameFull = dt.Rows[0]["SalesRepFullName"].ToString();
                user.Email = dt.Rows[0]["Email"].ToString();
            }
            return isExist;
        }

        private bool IsPredefineRole(string role, string hierarchyCode, out bool isSalesRepRole)
        {
            isSalesRepRole = false;
            if (!PreDefineRole.IsNullOrEmpty() && hierarchyCode == UserSecRole.CSUSERS)
            {
                string[] predefineRoles = PreDefineRole.Split(',');
                if (predefineRoles.Contains(role))
                    isSalesRepRole = role == SALES_REP;
                return predefineRoles.Contains(role);
            }
            return false;
        }

        protected void uxRebindOrgSelected_Click(object sender, EventArgs e)
        {
            BindSelectedOrganization();
        }

        private void BindSelectedOrganization()
        {
            if (!SessionManager.Organizations.IsNullOrEmpty())
                lbNumSelectedOrg.Text = string.Format("{0} {1}", SessionManager.Organizations.Split(',').Count().ToString(), GetLocalResourceObject("lbOrganizationAdded").ToString());
            else
                lbNumSelectedOrg.Text = string.Empty;
        }

        private void PreDefineRoleSelected(PreDefineRoleAction action)
        {
            switch (action)
            {
                case PreDefineRoleAction.RoleChanged:
                    {

                        SessionManager.Organizations = null;
                        uxSaleRepCode.Text = string.Empty;
                        lbNumSelectedOrg.Text = string.Empty;
                        //Reset SaleRepCode validation
                        uxSaleRepCodelable.CssClass = string.Empty;
                        txtSalesRepCodeErrMsg.ShowOnLoad = false;
                        txtSalesRepCodeErrMsg.Message = string.Empty;
                        uxPnlStatus.Enabled = true;
                        //TK: 41877 - ALICE PH2 - Climate Control Updates 
                        uxFirstName.Enabled = true;
                        uxLastName.Enabled = true;
                        uxEmail.Enabled = true;
                        break;
                    }
                case PreDefineRoleAction.BindUserDetail:
                    {
                        SessionManager.Organizations = getOrgList();
                        break;
                    }
            }
        }

        private void showHideSalesRepControl()
        {
            if (_hierarchy != null)
            {
                bool isSaleRepRole = false;
                bool isPredifineRole = IsPredefineRole(_hierarchy.HierarchyName, _hierarchy.HierarchyCode, out isSaleRepRole);
                uxSaleRepCode.Visible = isPredifineRole;

                ListItem itemKeyMerchant = uxAFMerchantProfileAccess.Items.FindByText(GetLocalResourceObject("txtKeyMerchantApplications").ToString());
                if (itemKeyMerchant == null)
                    itemKeyMerchant = uxAFRiskAccess.Items.FindByText(GetLocalResourceObject("txtKeyMerchantApplications").ToString());
                if (itemKeyMerchant == null)
                    itemKeyMerchant = uxAFSecurityAccess.Items.FindByText(GetLocalResourceObject("txtKeyMerchantApplications").ToString());
                if (itemKeyMerchant == null)
                    itemKeyMerchant = uxAFAliceAccess.Items.FindByText(GetLocalResourceObject("txtKeyMerchantApplications").ToString());
                if (itemKeyMerchant == null)
                    itemKeyMerchant = uxAFExternalAccess.Items.FindByText(GetLocalResourceObject("txtKeyMerchantApplications").ToString());
                if (itemKeyMerchant == null)
                    itemKeyMerchant = uxNotificationAccess.Items.FindByText(GetLocalResourceObject("txtKeyMerchantApplications").ToString());
                if (itemKeyMerchant == null)
                    itemKeyMerchant = uxProcessingDateAccess.Items.FindByText(GetLocalResourceObject("txtKeyMerchantApplications").ToString());
                if (itemKeyMerchant == null)
                    itemKeyMerchant = uxManageDocumentTypesAccess.Items.FindByText(GetLocalResourceObject("txtKeyMerchantApplications").ToString());

                if (itemKeyMerchant != null)
                {
                    ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript(string.Format("SetCheckboxIndexChanged({0},{1},{2})",
                        itemKeyMerchant.Selected ? "true" : "false",
                        isPredifineRole ? "true" : "false",
                        uxRole.Text.Equals(SALES_REP) ? "true" : "false"));
                    //clear arganization list
                    if (!itemKeyMerchant.Selected)
                        SessionManager.Organizations = null;
                }

                BindSelectedOrganization();
            }
        }
        #endregion

        protected void linkViewOrgazition_Click(object sender, EventArgs e)
        {
            string queryString = this.Page.BuildSecureQueryString(string.Format("SaleRepCode=" + uxSaleRepCode.Text));
            ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript("ShowPopupModalChild(1,'ViewOrganizationModal.aspx?" + queryString + "','auto'); return false;");
        }

        private string getOrgList()
        {
            if (_user != null)
            {
                DataTable OrganizationSeleted = GeneralFuncsLib.GetOrganizations(WebSiteEnums.OrganizationMode.BYUSER.ToString(), _user.RecId);
                var orgs = new StringBuilder();
                for (int i = 0; i < OrganizationSeleted.Rows.Count; i++)
                {
                    orgs.Append(OrganizationSeleted.Rows[i]["OrganizationID"] + ",");
                }
                return orgs.ToString().Trim(',');
            }
            return string.Empty;
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            showHideSalesRepControl();
        }
        protected void uxOwnershipGroupList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            DataRowView dataSourceRow = (DataRowView)e.Item.DataItem;
            var ownershipGroupID = dataSourceRow["OwnershipGroupID"];
            var ownershipGroupName = dataSourceRow["OwnershipGroup"];
            HyperLink linkOwnershipGroup = (HyperLink)e.Item.FindControl("uxLinkOwnershipGroup");
            string queryString = this.Page.BuildSecureQueryString(string.Format("OwnershipGroupID=" + ownershipGroupID + "&OwnershipGroupName=" + ownershipGroupName));
            linkOwnershipGroup.Attributes["onclick"] = string.Format("ShowPopupModalChild(1,'MemberList.aspx?" + queryString + "','auto'); return false;");
        }
        protected void uxOwnershipGroupDefault_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            DataRowView dataSourceRow = (DataRowView)e.Item.DataItem;
            var isChecked = (bool)dataSourceRow["IsChecked"];
            var isDefault = (bool)dataSourceRow["IsDefault"];

            e.Item.CssClass = isChecked ? "" : "hide";
            if (isChecked)
            {
                uxOwnershipGroupDefault.Enabled = true;
            }
            if (IsUpdateMode && isDefault)
            {
                uxOwnershipGroupDefault.SelectedValue = string.Format("{0}", dataSourceRow["OwnershipGroupID"].ToString());
            }
        }
        public string GetAssociatedOrg(string saleRepCode)
        {
            DataTable dtOrg = GeneralFuncsLib.GetListOrganizationsView(WebSiteEnums.OrganizationMode.BYSALESREP.ToString(), saleRepCode);
            if (dtOrg.HasData()) return dtOrg.Rows.Count.ToString();
            return "0";
        }

        protected void uxGetAssociatedOrg_CreateResponseData(AS.Controls.Global.ASCommandControl sender, string eventArgument)
        {
            sender.ResponseString = GetAssociatedOrg(eventArgument);
        }
        protected void getAPIAccessPassword()
        {
            if (checkApiAccessPermission())
            {
                FilterParameterCollection parameters = new FilterParameterCollection();
                string userID = IsUpdateMode ? _user.UserID : SessionManager.CurrentUser.UserID;
                parameters.Add("@UserID", userID, DbType.AnsiString);
                parameters.Add("@ASClientID", SessionManager.CurrentClient, DbType.AnsiString);
                parameters.Add("@SiteID", SessionManager.CurrentUser.SiteID, DbType.AnsiString);
                var dt = WebServices.SecurityServices.GetReports("spa_SEC_GetUsers_APIReportingExtend", parameters);
                if (dt != null && dt.Rows.Count > 0)
                {
                    txtAPIPassword.Text = Cryptophy.DecryptText(dt.Rows[0]["ApiPassword"].ToString());
                }
                else
                {
                    txtAPIPassword.Text = GeneratePwdAPI();
                }
                btnShowPassword.Visible = true;
                lblApiPasswordText.CssClass = "";
                ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript("ShowHideApiPassword('" + true + "');");
            }
            else
            {
                ShowHideApiPasswordSection(false);
            }
        }
        protected void UpdateApiAccessPassword()
        {
            if (checkApiAccessPermission() && SessionManager.CurrentUser.UserSecRole.Equals(UserSecRole.CSUSERS))
            {
                FilterParameterCollection parameters = new FilterParameterCollection();
                string userID = IsUpdateMode ? _user.UserID : ApiUserID;
                string apiPassword = Cryptophy.EncryptText(txtAPIPassword.Text.Trim());
                parameters.Add("@UserID", userID, DbType.AnsiString);
                parameters.Add("@UserMode", UserSecRole.CSUSERS, DbType.AnsiString);
                parameters.Add("@PMApiPassword", apiPassword, DbType.AnsiString);
                parameters.Add("@ASClientID", SessionManager.CurrentUser.ASClient, DbType.Int32);
                parameters.Add("@SiteID", SessionManager.CurrentUser.SiteID, DbType.Int32);
                WebServices.SecurityServices.GetReports("spa_SEC_Users_APIReportingExtend_UpdateUserPassword", parameters);
            }
        }
        protected bool checkApiAccessPermission()
        {
            var apiItem = uxAFExternalAccess.Items.FindByValue(ApiPermissionID);
            if (apiItem != null)
                return (apiItem.Selected && apiItem.Enabled);
            else
            {
                return false;
            }
        }
        string oldApiPassword = string.Empty;
        protected void uxAFExternalAccess_SelectedIndexChanged(object sender, EventArgs e)
        {
            var chks = (CheckBoxList)sender;
            var item = chks.Items.FindByValue(ApiPermissionID);
            if (item != null)
            {
                if (item.Selected && String.Compare(txtAPIPassword.Text.Trim(), oldApiPassword, false) == 0)
                {
                    ShowHideApiPasswordSection(true);
                }
                if (!item.Selected)
                {
                    ShowHideApiPasswordSection(false);
                }
            }
        }
        protected void ShowHideApiPasswordSection(bool isShow)
        {
            ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript("ShowHideApiPassword('" + isShow + "');");
            if (isShow)
            {
                txtAPIPassword.Text = GeneratePwdAPI();
                lblApiPasswordText.CssClass = "";
                btnShowPassword.Visible = true;
            }
            else
            {
                txtAPIPassword.Text = string.Empty;
                lblApiPasswordText.CssClass = "hide";
                btnShowPassword.Visible = false;
            }

        }
        private void DisableClientControl()
        {
            ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript("DisableClientContol()");
        }
        private void DisbleAllControl(Control baseControl)
        {
            ControlCollection controls = baseControl.Controls;
            foreach (Control control in controls)
            {
                PropertyInfo p = control.GetType().GetProperty("Enabled");
                if (p != null && p.CanWrite)
                {
                    bool isEnabled = control.ID.Contains("uxCancel");
                    p.SetValue(control, isEnabled);
                }

                if (control is AS.Controls.Global.Button && control.ID.Contains("uxSave"))
                {
                    PropertyInfo visible = control.GetType().GetProperty("Visible");
                    if (visible != null && visible.CanWrite)
                    {
                        visible.SetValue(control, false);
                    }
                }

                DisbleAllControl(control);
            }
        }
        private string GeneratePwdAPI()
        {
            string clientId = SessionManager.CurrentClient.ToString().PadLeft(4, '0');
            return WebServices.SecurityServices.GenPwd() + clientId;
        }
        private string BuildLinkUpdate(int clientId, string userName, string passWord)
        {
            _page = Page;
            string domain = IsMSUser ? GeneralFuncsLib.GetDataOfExtendedSetting("MS_DOMAIN") : GeneralFuncsLib.GetDataOfExtendedSetting("CS_DOMAIN");
            domain = string.IsNullOrEmpty(domain) ? WebSiteSettings.CurrentDomain : domain;
            string param = _page.BuildStaticQueryString(
                               string.Format("clientId={0}&userName={1}&passWord={2}", clientId, userName, passWord));

            var result = domain + (domain.EndsWith("/") ? string.Empty : "/") + "freeaccess/ActivatePwd.aspx?" + param;

            return result;
        }
        protected void uxApproveGroupList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

        }
    }
}

