//----------------------------------------------------------------------------
// <copyright file="gen_ManageProfile.aspx.cs" company="Data Delivery Services, Inc.">
//     Copyright (c) Data Delivery Services, Inc. All rights reserved.
// </copyright>
// <author>Hung Ho</author>
// <summary>Manage User ( Create, Edit & Reset password ) </summary>
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
using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Web.Business;
using AS.Controls.Exporter;
using Telerik.Web.UI;
using AS.Controls.Pages;
using AS.Security.WS.Entities;
using AS.Common;

[PagePermission("ManUser,MSManUser")]
public partial class _mps_ManageUsers : ReportPage
{
    #region Constants

    private const string FNAME_USER_TYPE = "HierarchyName";
    private const string FNAME_FULL_USER_TYPE = "FullUserType";
    private const string FNAME_USER_ID = "UserID";
    private const string FNAME_RESET_PWD = "ResetPassword";
    private const string FNAME_ACTIVE_STATUS = "ActiveStatus";
    private const string FNAME_SITE_ACCESS = "SiteAccess";
    private const string FNAME_HIERARCHY_CODE = "HierarchyCode";
    private const string FNAME_ACTIVE_STATUS_DESC = "ActiveStatusDescription";

    private string RESET_PWD_TEXT = string.Empty;

    private const string SPA_GET_USERLIST_BY_HIERARCHYID = "spa_SEC_GetUserListByHierarchyID";
    private const string SPA_GET_CS_USERLIST_BY_HIERARCHYID = "spa_SEC_GetCSUserListByHierarchyID";
    private const string SPA_GET_USERS_BELONG_TO_USER = "spa_SEC_GetUsersBelongToUser";
    private const string SPA_CHECK_USER_SITE_JUMP = "spa_SEC_CheckUserSiteJump";

    private string MESSAGE_INVALID_USER_ACCOUNT = string.Empty;
    private string MESSAGE_MERCHANT_OPTED_OUT = string.Empty;
    private string MESSAGE_USER_OPTED_OUT = string.Empty;

    private const string ENTITY_TYPE = "EntityType";
    private const string SYSTEM_ID = "SystemId";
    private const string IS_SECONDARY_USER = "IsSecondaryUser";
    private const string IS_SSO_USER = "IsSSOUser";
    private const string DISABLE_RESETPASSWORD = "DisableResetPasswordForInactiveUser";
    private const string SALES_REP_CODE = "SalesRepCode";

    #endregion Constants

    #region Fields

    private string key = string.Empty;
    private bool? _hasMSUserManagementFeature = null;
    // Because we use ASFuncInvoker to load UserList in case the client doesn't have MS User Management,
    // we need to re-filter data when datasoure is ready.
    private bool _needReFilterData = false;

    #endregion Fields

    #region Enums

    enum DataBindAction
    {
        BindUserListGrid,
        ShowHideCreateUserHyperlink

    }

    enum PostBackAction
    {
        RebindUserListClick,
        ActiveStatusClick,
        DoActivateDeactivate
    }

    #endregion Enums

    #region Properties

    private bool HasMSUserManagementFeature
    {
        get
        {
            _hasMSUserManagementFeature =
                _hasMSUserManagementFeature ?? GeneralFuncsLib.HasMSUserManagementFeature;
            return _hasMSUserManagementFeature.Value;
        }
    }

    private string DisableResetPasswordForInactiveUser
    {
        get
        {
            if (ViewState[DISABLE_RESETPASSWORD] == null)
                return GeneralFuncsLib.GetDataOfExtendedSetting(DISABLE_RESETPASSWORD);
            else return ViewState[DISABLE_RESETPASSWORD].ToString();
        }
    }

    #endregion Properties

    #region Methods

    #region Protected

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        RegisterEvent();
        ((ReportPage)this.Page).IsBindDataOnLoad = true;
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindUserListGrid:
                {
                    if (SessionManager.CurrentSystem == WebSiteConstants.AS_SYSTEM_CS)
                    {
                        LoadUserList();
                    }
                    else
                    {
                        FilterParameterCollection parameters = new FilterParameterCollection
                        {
                            new FilterParameter("@ASClient", SessionManager.CurrentUser.ASClient, DbType.Int32),
                            new FilterParameter("@UserID", SessionManager.CurrentUser.UserID, DbType.String)
                        };
                        parameters.AddLanguageID();
                        DataTable dt = WebServices.SecurityServices.GetReports(SPA_GET_USERS_BELONG_TO_USER, parameters);
                        dt = DoFilterTable(dt);
                        uxReportGrid.DataSource = dt;
                        if (dt.Rows.Count == 0 && string.IsNullOrEmpty(uxReportGrid.AS_FilterExpression))
                        {
                            uxReportGrid.AllowFilteringByColumn = false;
                        }
                        else
                        {
                            uxReportGrid.AllowFilteringByColumn = true;
                        }
                    }
                }
                break;
            case DataBindAction.ShowHideCreateUserHyperlink:
                uxCreateNewUser.Attributes.Add("onclick", "return false;");
                if (SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.Headquarter
                    || SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.Merchant)
                {
                    //get current scecondary user
                    //*
                    FilterParameterCollection paramList = new FilterParameterCollection();
                    paramList.AddLoggedInUserReportingParams(false);
                    paramList.Add(new FilterParameter("@CheckedUserID", SessionManager.CurrentUser.UserID, DbType.String));
                    DataTable returnDT = WebServices.SecurityServices.GetReports("spa_ms_CheckMaxSecCountForUser", paramList);
                    //*/               
                    int curUser = (int)returnDT.Rows[0]["CurSecCount"];
                    int maxUser = (int)returnDT.Rows[0]["MaxSecCount"];
                    if (maxUser <= curUser)
                    {
                        uxCreateNewUser.Disabled = true;
                        uxCreateNewUser.Title = VeraCodeSolution.DoVeraCode(
                             GetLocalResourceObject("ManageUsers_aspx_cs_MaximumNumberOfSecondaryUser").ToString());
                    }
                    else
                    {
                        uxCreateNewUser.Attributes.Add("onclick",
                             "return ShowPopupModal('CreateNewUser.aspx','auto');");
                        uxCreateNewUser.Title = VeraCodeSolution.DoVeraCode(
                            string.Format(GetLocalResourceObject("ManageUsers_aspx_cs_AllowedAdditionalUser").ToString(),
                                          maxUser - curUser));
                    }
                }
                else
                {
                    uxCreateNewUser.Attributes.Add("onclick",
                        "return ShowPopupModal('CreateNewUser.aspx','auto');");
                }
                break;
        }
    }

    protected override void OnPostBackActions(Enum type, object param)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.RebindUserListClick:
                {
                    uxReportGrid.MasterTableView.Rebind();
                    OnDataBindControls(DataBindAction.ShowHideCreateUserHyperlink);
                }
                break;
            case PostBackAction.ActiveStatusClick:
                {
                    this.AjaxAddResponseScript(string.Format("doSetStatus('{0}','Y','Y');", hddActiveUserID.Value));
                }
                break;
            case PostBackAction.DoActivateDeactivate:
                string userid = WebServices.SecurityServices.DecryptText(HttpUtility.UrlDecode(hddActiveUserID.Value), key);
                User user = WebServices.SecurityServices.GetUser(SessionManager.CurrentUser.ASClient, userid);
                if (user != null)
                {
                    user.Status = user.Status == "1" ? "0" : "1";
                }
                else
                {
                    return;
                }
                user.UpdatedBy = SessionManager.CurrentUser.RecId;
                WebServices.SecurityServices.UpdateUser(user, 0, "".Split('.'), string.Empty, string.Empty);

                int systemId = WebSiteConstants.AS_SYSTEM_CS;
                Int32.TryParse(hddSystemId.Value, out systemId);
                bool isSecondaryUser = false;
                bool.TryParse(hddIsSecondaryUser.Value, out isSecondaryUser);
                // For CS only
                if (GeneralFuncsLib.IsSynchPCIUser() && (systemId == WebSiteConstants.AS_SYSTEM_CS || (systemId == WebSiteConstants.AS_SYSTEM_MS && isSecondaryUser) || SessionManager.CurrentSystem == WebSiteConstants.AS_SYSTEM_MS))
                {
                    GeneralFuncsLib.SyncUserWithPCIForLockAccount(
                        SessionManager.CurrentUser.ASClient,
                        userid,
                        user.Status == "1");
                }

                uxManageUsersMasterUserControl.SubmitUser(SessionManager.CurrentClient, user.RecId.ToString(), systemId, user.Status);

                uxReportGrid.MasterTableView.Rebind();
                break;
        }
    }

    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, ExportConfig exportConfig)
    {
        if (IsIntruderDetected)
        {
            return;
        }
        base.DoNeedExportConfig(sender, exportConfig);
        exportConfig.ExportMethod = ExportMethod.BY_FILESTREAM;
        exportConfig.FileName = GeneralFuncsLib.FormatFileName(GetLocalResourceObject("ManageUsers_aspx_cs_UserListFileName").ToString());

        exportConfig.ReportHeader = GetLocalResourceObject("ManageUsers_aspx_cs_UserListFileName").ToString();

        this.uxExporter.Formatter = new System.Collections.Generic.Dictionary<string, Func<object, string>>();
        this.uxExporter.FormatterWS = new System.Collections.Generic.Dictionary<string, string>();

        SetVisibleColumns(true);
    }

    protected override void PageInitialize()
    {
        base.PageInitialize();
        this.IsSecureCSRF = true;
        this.IsBindDataOnLoad = true;
        uxPlaceHolderReportFilter.Visible = EnableMSUserManagement();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected)
        {
            return;
        }
        RESET_PWD_TEXT = GetLocalResourceObject("ManageUsers_aspx_cs_ResetPwd").ToString();
        MESSAGE_MERCHANT_OPTED_OUT = GetLocalResourceObject("ManageUsers_aspx_cs_MerchantOptedOut").ToString();
        MESSAGE_USER_OPTED_OUT = GetLocalResourceObject("ManageUsers_aspx_cs_UserOptedOut").ToString();
        MESSAGE_INVALID_USER_ACCOUNT = GetLocalResourceObject("ManageUsers_aspx_cs_InvalidUserAccount").ToString();
        key = Session.SessionID.Substring(0, 8);
        if (!IsPostBack)
        {
            // With Client having MS User Management, the UserList result will not be shown
            // until the user click on "Search" button
            uxReportGrid.Visible = !EnableMSUserManagement();
            uxReportGrid.ASPagingMethod = EnableMSUserManagement()
                ? ASGrid.PagingMethods.SPASingleMethod : ASGrid.PagingMethods.None;

            OnDataBindControls(DataBindAction.ShowHideCreateUserHyperlink);
        }
        else
        {
            uxReportGrid.Visible = true;
        }

        var hasMSUserManagementFeature = GeneralFuncsLib.HasMSUserManagementFeature;
        uxReportTitle.HasFilteringOption = hasMSUserManagementFeature;

        SetVisibleColumns(false);
        RemoveFilterMenuItem();

        if (hasMSUserManagementFeature)
        {
            uxReportGrid.MasterTableView.NoDetailRecordsText = GetLocalResourceObject("NoResultsFound").ToString();
            uxReportGrid.MasterTableView.NoMasterRecordsText = GetLocalResourceObject("NoResultsFound").ToString();
        }
    }

    protected override void DoItemDataBound(AS.Controls.Grid.ASGrid sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (sender == uxReportGrid && sender.Visible && e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView rowView = ((System.Data.DataRowView)(e.Item.DataItem));
            string userID = rowView[FNAME_USER_ID].ToString();

            int systemId = WebSiteConstants.AS_SYSTEM_CS;
            int.TryParse(rowView[SYSTEM_ID].ToString(), out systemId);
            bool isSecondaryUser = false;
            if (rowView.Row.Table.Columns.Contains(IS_SECONDARY_USER))
                bool.TryParse(rowView[IS_SECONDARY_USER].ToString(), out isSecondaryUser);

            bool isSSOUser = false;
            if (rowView.Row.Table.Columns.Contains(IS_SSO_USER))
                bool.TryParse(rowView[IS_SSO_USER].ToString(), out isSSOUser);

            if (isSSOUser)
            {
                dataItem[FNAME_RESET_PWD].Text = string.Empty;
                dataItem[FNAME_SITE_ACCESS].Text = string.Empty;
                dataItem[FNAME_ACTIVE_STATUS_DESC].Text = rowView[FNAME_ACTIVE_STATUS_DESC].ToString();

            }
            else
            {
                if (rowView[FNAME_ACTIVE_STATUS].ToString().CompareTo("N") == 0
                    && rowView.Row.Table.Columns.Contains(ENTITY_TYPE)
                    && GeneralFuncsLib.DisableResetPasswordForInactiveUser(rowView[ENTITY_TYPE].ToString(), DisableResetPasswordForInactiveUser))
                {
                    dataItem[FNAME_RESET_PWD].Text = string.Empty;
                }
                else
                {
                    // Reset Password Field   
                    dataItem[FNAME_RESET_PWD].Text = VeraCodeSolution.DoVeraCode(
                        BuildOpenModalLink("ManageUserResetPwd_Step1.aspx?"
                                                + BuildSecureQueryString(string.Format("u={0}&reload={1}", userID, 1)),
                                           RESET_PWD_TEXT));
                }
                bool disabledUpdateUser = DisabledUpdateUser(rowView["PermissionCodeList"].ToString());
                // User Name Hyperlink
                HiddenField uxHierarchyCode = ((HiddenField)dataItem[FNAME_ACTIVE_STATUS_DESC].FindControl("uxHierarchyCode"));
                string updateUserQuery = BuildUserNameHyperlink(userID, systemId, isSecondaryUser, rowView[FNAME_HIERARCHY_CODE].ToString(), disabledUpdateUser);
                dataItem[FNAME_USER_ID].Text = VeraCodeSolution.DoVeraCode(
                    BuildOpenModalLink("UpdateUser.aspx?" + BuildSecureQueryString(updateUserQuery),
                                       userID));

                // Site Jump
                if ((EnableMSUserManagement()
                   && int.Parse(rowView[SYSTEM_ID].ToString()) == WebSiteConstants.AS_SYSTEM_CS) || (int.Parse(rowView[SYSTEM_ID].ToString()) == WebSiteConstants.AS_SYSTEM_MS && rowView.Row.Table.Columns.Contains(ENTITY_TYPE) && IsEntityTypeExcludedForSiteJump(rowView[ENTITY_TYPE].ToString())))
                {
                    dataItem[FNAME_SITE_ACCESS].Text = string.Empty;
                }
                else
                {
                    LinkButton lnkSJ = dataItem[FNAME_SITE_ACCESS].FindControl("uxSiteJumpLinkBtn") as LinkButton;
                    if (lnkSJ != null)
                        lnkSJ.Visible = true;
                }

                bool isSalesRep = !string.IsNullOrEmpty(rowView[SALES_REP_CODE].ToString());
                //45177 - Relabel Fields and Column Headings
                if (isSalesRep || disabledUpdateUser)
                {
                    dataItem[FNAME_ACTIVE_STATUS_DESC].Text = rowView[FNAME_ACTIVE_STATUS_DESC].ToString();
                }
                else
                {
                    // Active Status Hyperlink
                    ((LinkButton)dataItem[FNAME_ACTIVE_STATUS_DESC].FindControl("uxActiveStatus")).OnClientClick =
                        string.Format("return doSetStatus('{0}','{1}','{2}', '{3}')",
                                      Server.UrlEncode(WebServices.SecurityServices.EncryptText((userID), key)),
                                      rowView[FNAME_ACTIVE_STATUS].ToString(), systemId, isSecondaryUser);
                }
            }
        }
    }

    private bool DisabledUpdateUser(string permissionCodes)
    {
        string ms = string.Format(",{0},", WebSiteConstants.SEC_PERMISSION_DISABLEDUPDATEUSER_MS);
        string cs = string.Format(",{0},", WebSiteConstants.SEC_PERMISSION_DISABLEDUPDATEUSER);
        permissionCodes = string.Format(",{0},", permissionCodes);
        if (permissionCodes.Contains(cs) || permissionCodes.Contains(ms))
            return true;
        return false;
    }

    protected override void DoGridNeedDataSource(ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        if (sender == uxReportGrid && sender.Visible)
        {
            OnDataBindControls(DataBindAction.BindUserListGrid);
            SetVisibleColumns(false);
        }
    }

    protected override void DoGridDataSourceReady(ASGrid sender, EventArgs e)
    {
        base.DoGridDataSourceReady(sender, e);
        if (sender == uxReportGrid && sender.Visible)
        {
            // Re-filter data in case the Client doesn't have MS User Management
            if (_needReFilterData && sender.DataSource != null
                && ((DataTable)sender.DataSource).Rows.Count > 0)
            {
                sender.DataSource = FilterUserList((DataTable)sender.DataSource);
            }

            // Reset to avoid calling FilterUserList in other cases
            _needReFilterData = false;

            if (uxReportGrid.AS_FilterExpression.IsNullOrEmpty()
                && (sender.DataSource == null || ((DataTable)sender.DataSource).Rows.Count == 0))
            {
                uxReportGrid.AllowFilteringByColumn = false;
                uxExporter.Visible = false;
            }
            else
            {
                uxReportGrid.AllowFilteringByColumn = true;
                uxExporter.Visible = sender.DataSource != null && ((DataTable)sender.DataSource).Rows.Count != 0;
            }
            
        }
    }

    protected void uxRebinData_Click(object poster, EventArgs e)
    {
        if (IsIntruderDetected)
            return;
        OnPostBackActions(PostBackAction.RebindUserListClick);
    }

    protected void ActivateDeactivateCommand(object sender, CommandEventArgs e)
    {
        OnPostBackActions(PostBackAction.ActiveStatusClick);
    }

    protected void uxActiveDecactive_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.DoActivateDeactivate);
    }

    protected void RegisterEvent()
    {
        uxReportFiltering.Filtering += (s, e) =>
        {
            uxReportGrid.CurrentPageIndex = 0;
            uxReportGrid.Rebind();
        };
    }

    protected void uxSiteAccess_Click(object sender, CommandEventArgs e)
    {
        if (IsIntruderDetected)
        {
            return;
        }
        string userId = e.CommandArgument.ToString().Trim();

        this.ASPXTrackingLog.LogData1 = "gen_log_JumpSite.aspx";
        this.ASPXTrackingLog.LogData3 = "";

        User user = null;
        if (!CheckValidToSiteJump(userId, ref user))
        {
            return;
        }

        string keyJump = WebServices.SecurityServices.CreateJumpSiteTicket(
            user.RecId,
            Request.UserHostAddress,
            2,
            SessionManager.CurrentUser.RecId);
        string url = string.Format(WebSiteSettings.MsGate + "?u={0}&k={1}&j={2}&c={3}&f={4}&lan={5}",
            HttpUtility.UrlEncode(WebServices.SecurityServices.EncryptText(user.UserID)),
            HttpUtility.UrlEncode(keyJump),
            HttpUtility.UrlEncode(WebServices.SecurityServices.EncryptText(SessionManager.CurrentUser.RecId.ToString())),
            HttpUtility.UrlEncode(AS.Common.DataProtection.Cryptophy.EncryptText(user.ASClient.ToString())),
            HttpUtility.UrlEncode(WebServices.SecurityServices.EncryptText("CS")),
            SessionManager.CurrentLanguage
            );

        this.AjaxAddResponseScript(string.Format("window.open('{0}');", url));
    }

    #endregion Protected

    #region Private methods
    private string BuildOpenModalLink(string url, string text)
    {
        return string.Format("<a href=\"#\" onclick=\"return ShowPopupModal('{0}','auto');\">{1}</a>", url, text);
    }

    private void RemoveFilterMenuItem()
    {
        //show filter menu
        var grids = new RadGrid[] { uxReportGrid };
        var removedItems = new string[] { 
           "GreaterThan", "LessThan", "GreaterThanOrEqualTo", "LessThanOrEqualTo",
           "Between", "NotBetween", "IsEmpty", "NotIsEmpty", "IsNull", "NotIsNull"
        };

        foreach (var grid in grids)
        {
            for (int i = 0; i < removedItems.Length; i++)
            {
                var mi = grid.FilterMenu.Items.FindItemByText(removedItems[i]);
                if (mi != null)
                    grid.FilterMenu.Items.Remove(mi);
            }
        }
    }


    private DataTable DoFilterTable(DataTable dt)
    {
        if (!string.IsNullOrEmpty(uxReportGrid.AS_FilterExpression))
        {
            DataTable dtFiltered = new DataTable();
            foreach (DataColumn c in dt.Columns)
            {
                dtFiltered.Columns.Add(c.ColumnName, c.DataType);
            }
            DataRow[] filtered = dt.Select(uxReportGrid.AS_FilterExpression);
            foreach (DataRow dr in filtered)
            {
                dtFiltered.ImportRow(dr);
            }
            return dtFiltered;
        }
        return dt;
    }

    private void SetVisibleColumns(bool isExporting)
    {
        if (isExporting)
        {
            uxReportGrid.Columns.FindByUniqueName(FNAME_RESET_PWD).Visible = false;
            uxReportGrid.Columns.FindByUniqueName(FNAME_SITE_ACCESS).Visible = false;
        }
        else
        {
            uxReportGrid.Columns.FindByUniqueName(FNAME_RESET_PWD).Visible =
                IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RESET_PWD_MS)
                || IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RESET_PWD);
            uxReportGrid.Columns.FindByUniqueName(FNAME_SITE_ACCESS).Visible = EnableMSUserManagement();
        }

        uxReportGrid.Columns.FindByUniqueName(FNAME_FULL_USER_TYPE).Visible = EnableMSUserManagement();
        uxReportGrid.Columns.FindByUniqueName(FNAME_USER_TYPE).HeaderText =
            EnableMSUserManagement() ? GetLocalResourceObject("ManageUsers_aspx_cs_UserRole").ToString() : GetLocalResourceObject("ManageUsers_aspx_cs_UserType").ToString();
    }

    private void LoadUserList()
    {
        if (EnableMSUserManagement())
        {
            LoadUserListByFilter();
        }
        else
        {
            LoadCSUserList();
        }
    }

    private void LoadUserListByFilter()
    {
        FilterParameterCollection param = new FilterParameterCollection();
        param.AddLoggedInUserReportingParams();
        param.Add(new FilterParameter("@FilterType",
                                      uxReportFiltering.ManageUserFilterOption.SearchType,
                                      DbType.String));
        param.Add(new FilterParameter("@FilterMode",
                                      uxReportFiltering.ManageUserFilterOption.FilterMode,
                                      DbType.Int32));
        param.Add(new FilterParameter("@FilterVal",
                                      uxReportFiltering.ManageUserFilterOption.SearchValue,
                                      DbType.String));
        param.Add(new FilterParameter("@RoleId",
                                      uxReportFiltering.ManageUserFilterOption.RoleId,
                                      DbType.Int32));
        param.Add(new FilterParameter("@HierarchyID",
                                      SessionManager.CurrentUserRoles[0].HierarchyID,
                                      DbType.Int32));
        param.AddLanguageID();

        uxReportGrid.DataSourceInvoker =
            new ASFuncInvoker(
                WebServices.SecurityServices,
                WebSiteConstants.GET_REPORT_METHOD_NAME,
                new object[] {
                    SPA_GET_USERLIST_BY_HIERARCHYID,
                    AS.Security.Web.SecurityServices.SecurityService.ConvertToFilterParamWSArray(param)
                });

        _needReFilterData = true;
    }

    private void LoadCSUserList()
    {
        FilterParameterCollection param = new FilterParameterCollection
        {
            new FilterParameter("@HierarchyID",
                                      SessionManager.CurrentUserRoles[0].HierarchyID,
                                      DbType.Int32),
            new FilterParameter("@UserID", SessionManager.CurrentUser.UserID, DbType.String)
        };
        param.AddLanguageID();
        DataTable dt = WebServices.SecurityServices.GetReports(SPA_GET_CS_USERLIST_BY_HIERARCHYID, param);
        uxReportGrid.DataSource = FilterUserList(dt);
    }

    private DataTable FilterUserList(DataTable dt)
    {
        dt = DoFilterTable(dt);
        DataTable tempDt = dt.Clone();
        foreach (DataRow row in dt.Rows)
        {
            DataRow tempRow = tempDt.NewRow();
            tempRow.ItemArray = row.ItemArray;
            if (row["UserID"].ToString() != SessionManager.CurrentUser.UserID
                && (Int32.Parse(row["SiteID"].ToString()) == SessionManager.CurrentUser.SiteID
                    || EnableMSUserManagement())
                && Int32.Parse(row["ASClient"].ToString()) == SessionManager.CurrentUser.ASClient)
            {
                if (SessionManager.CurrentUser.UserType == 1)  // AS User
                {
                    tempDt.Rows.Add(tempRow);
                }
                else
                {
                    if (row["UserType"].ToString() != "1")  // add other Users
                    {
                        tempDt.Rows.Add(tempRow);
                    }
                }
            }
        }
        return tempDt;
    }

    private bool IsEntityTypeExcludedForSiteJump(string entityTypeId)
    {
        bool result = false;
        string excludeHierachies = GeneralFuncsLib.GetDataOfExtendedSetting("DisableSiteJumpByEntityTypeIDs");
        if (!string.IsNullOrEmpty(excludeHierachies))
        {
            excludeHierachies = string.Format(",{0},", excludeHierachies);
            if (excludeHierachies.Contains(string.Format(",{0},", entityTypeId)))
            {
                result = true;
            }
        }
        return result;
    }

    private bool CheckValidToSiteJump(string userId, ref User user)
    {
        bool isValidToSiteJump = true;
        string alertScript = "setTimeout(\"alert('{0}');\", 500);";

        if (GeneralFuncsLib.IsSSOUser(SessionManager.CurrentUser.ASClient, userId))
        {
            AjaxAddResponseScript(string.Format(alertScript, MESSAGE_INVALID_USER_ACCOUNT));
            return false;
        }

        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();
        parameters.Add(new FilterParameter("@UserHierarchyCode", SessionManager.CurrentUserRoles[0].HierarchyCode, DbType.String));
        parameters.Add(new FilterParameter("@UserSiteJump", userId, DbType.String));
        parameters.Add(new FilterParameter("@UserEntityType", SessionManager.CurrentUser.EntityType, DbType.Int32));
        parameters.Add(new FilterParameter("@SystemId", SessionManager.CurrentSystem, DbType.Int32));
        DataTable dt = WebServices.SecurityServices.GetReports(SPA_CHECK_USER_SITE_JUMP, parameters);

        if (dt != null && dt.Rows.Count > 0)
        {
            switch (Int32.Parse(dt.Rows[0][0].ToString()))
            {
                case 0:
                    // Success
                    isValidToSiteJump = true;
                    break;
                case 1:
                // Invalid user                                   
                case 2:
                    // Entity not belongs to the current user
                    AjaxAddResponseScript(string.Format(alertScript, MESSAGE_INVALID_USER_ACCOUNT));
                    isValidToSiteJump = false;
                    break;
                case 3:
                    // Merchant is opted out
                    AjaxAddResponseScript(string.Format(alertScript, MESSAGE_MERCHANT_OPTED_OUT));
                    isValidToSiteJump = false;
                    break;
                case 4:
                    // User is opted out
                    AjaxAddResponseScript(string.Format(alertScript, MESSAGE_USER_OPTED_OUT));
                    isValidToSiteJump = false;
                    break;
            }
        }

        if (isValidToSiteJump)
        {
            user = WebServices.SecurityServices.GetUser(SessionManager.CurrentClient, userId);

            bool isEntityExcluded = IsEntityTypeExcludedForSiteJump(user.EntityType.ToSafeString());

            if (isEntityExcluded || (SessionManager.CurrentSystem == WebSiteConstants.AS_SYSTEM_CS && user.SiteID == 0))
            {
                AjaxAddResponseScript(string.Format(alertScript, MESSAGE_INVALID_USER_ACCOUNT));
                isValidToSiteJump = false;
            }
        }

        return isValidToSiteJump;
    }

    private string BuildUserNameHyperlink(string userID, int systemId, bool isSecondaryUser, string hierarchyCode, bool disabledUpdateUser)
    {
        string userIdIntruderInfo =
            GeneralFuncsLib.BuildIntruderInfoForQueryStr(
                uxReportGrid.ID,
                new string[] { "UserID" });
        string updateUserQuery = string.Empty;
        if (EnableMSUserManagement() || SessionManager.CurrentSystem == WebSiteConstants.AS_SYSTEM_MS)
        {
            updateUserQuery = string.Format("u={0}&systemId={1}&isSecondaryUser={2}{3}&hierarchyCode={4}&DisabledUpdateUser={5}",
                userID, systemId, isSecondaryUser, userIdIntruderInfo, hierarchyCode, disabledUpdateUser);
        }
        else
        {
            updateUserQuery = string.Format("u={0}{1}&DisabledUpdateUser={2}", userID, userIdIntruderInfo, disabledUpdateUser);
        }
        return updateUserQuery;
    }

    private bool EnableMSUserManagement()
    {
        return HasMSUserManagementFeature
           && SessionManager.CurrentSystem == WebSiteConstants.AS_SYSTEM_CS;
    }

    #endregion Private method

    #endregion Methods
}
