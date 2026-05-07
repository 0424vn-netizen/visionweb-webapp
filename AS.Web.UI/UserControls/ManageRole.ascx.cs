using System.Linq;
using AS.Common;
using AS.Common.DBManager;
using AS.Security.WS.Entities;
using System;
using System.Data;
using Telerik.Web.UI;
using System.Collections.Generic;
using AS.Controls.Pages;

public partial class UserControls_ManageRole : GlobalUserControl
{
    #region Constants

    /// <summary>
    /// "ShowHideAltRole({0}, {1});"
    /// </summary>
    private const string SHOW_HIDE_ALT_ROLE = "ShowHideAltRole({0}, {1});";

    private const string HIERARCHY_NAME_FIELD = "HierarchyName";
    private const string HIERARCHY_ID_FIELD = "HierarchyID";

    private string ALL_HIERARCHY_TEXT = string.Empty;
    private const string ALL_HIERARCHY_VALUE = "-1";

    private string NEW_HIERARCHY_TEXT = string.Empty;
    private const string NEW_HIERARCHY_VALUE = "0";

    /// <summary>
    /// Define User Role
    /// </summary>
    private string PAGE_TITLE = string.Empty;
    /// <summary>
    /// Define CS User Role
    /// </summary>
    private string PAGE_TITLE_HAS_MS_USER_MGMT = string.Empty;

    #endregion Constants

    #region Fields

    private int hierarchyId = 0;
    private IList<string> _notificationPermissions;
    private bool _hasNotification;
    private string createdDate = string.Empty;
    #endregion Fields

    #region Methods

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionManager.UserGroupPermissions = null;

        ALL_HIERARCHY_TEXT = GetLocalResourceObject("ManageRole_ascx_cs_ALL").ToString();
        NEW_HIERARCHY_TEXT = GetLocalResourceObject("ManageRole_ascx_cs_ThisNewRole").ToString();
        PAGE_TITLE = GetLocalResourceObject("ManageRole_ascx_cs_DefineUserRole").ToString();
        PAGE_TITLE_HAS_MS_USER_MGMT = GetLocalResourceObject("ManageRole_ascx_cs_DefineCSUserRole").ToString();
        if (this.Page.IsSecureQueryString)
        {
            int.TryParse(this.Page.SecureQueryString["id"], out hierarchyId);
            createdDate = this.Page.SecureQueryString["createdDate"].ToString();
        }

        DataTable orginalDataSource = null;
        if (!IsPostBack)
        {
            //Re-register attribute for control
            uxRoleDes.Attributes.Add("maxlength", uxRoleDes.MaxLength.ToString());

            BindTreeMenu();
            BindAccessFunction();

            orginalDataSource = WebServices.SecurityServices
                .GetAssignableHierarchy(SessionManager.CurrentHierarchyId, WebSiteEnums.UserType.ALL.ToString())
                .ToDataTable();

            if (hierarchyId > 0)
            {
                Hierarchy hierarchy = WebServices.SecurityServices.GetHierarchyById(hierarchyId);
                if (hierarchy == null)
                {
                    Response.Redirect(WebSiteConstants.PAGE_NOT_FOUND_NAME);
                    return;
                }
                PermissionCollection permissions = WebServices.SecurityServices
                    .GetPermissionsInHierarchy(hierarchy.HierarchyID);

                SessionManager.SelectedGroupPermission = SessionManager.RoleGroupPermissions = permissions;

                uxRoleDes.Text = VeraCodeSolution.DoVeraCode(hierarchy.HierarchyDescription);
                uxRoleName.Text = VeraCodeSolution.DoVeraCode(hierarchy.HierarchyName);

                doCheckPermissionForMenu(uxTreeMenuCS.Nodes, permissions);

                uxRoleListSelected.DataSourceDestination = WebServices.SecurityServices
                    .GetAssignableHierarchy(hierarchyId, WebSiteEnums.UserType.ALL.ToString()).ToDataTable();

                // check site jump permission here
                PermissionCollection hierarchy_permissions = WebServices.SecurityServices
                    .GetPermissionsInHierarchy(hierarchyId);


                // 36127 - Enhancement in Edit User Page - Ownership Group 

                uxAccessFuncControl.SetSelectedByPermissions(permissions);

                DataTable temp = uxRoleListSelected.DataSourceDestination;
                for (int i = 0; i < temp.Rows.Count; i++)
                {
                    for (int j = 0; j < orginalDataSource.Rows.Count; j++)
                    {
                        if (orginalDataSource.Rows[j][HIERARCHY_ID_FIELD].ToString().Equals(temp.Rows[i][HIERARCHY_ID_FIELD].ToString()))
                        {
                            orginalDataSource.Rows.RemoveAt(j);
                            break;
                        }
                    }
                }

                //44648 - VW - Implement Change Log to review permission changes made to user roles
                string changelongInfo = GeneralFuncsLib.GetChangeLogLastestInfo(hierarchyId);
                if (!changelongInfo.IsNullOrEmpty())
                {
                    string queryString = Page.BuildSecureQueryString(string.Format("hierarchyId={0}", hierarchyId));
                    string url = string.Format("{0}?{1}", "ViewChangeLogModal.aspx", queryString);
                    uxViewLogTextStep1.Text = uxViewLogTextStep2.Text = uxViewLogTextStep3.Text = changelongInfo;
                    uxViewLogStep1.Text = uxViewLogStep2.Text = uxViewLogStep3.Text
                        = string.Format("<a href=\"#\" onclick=\" return parent.ShowPopupModalChild(1, '{0}', 'auto');\">{1}</a>", url, GetLocalResourceObject("ViewChangesLink").ToString());
                }
            }
            else
            {
                SessionManager.SelectedGroupPermission = SessionManager.RoleGroupPermissions = new PermissionCollection();
                DataTable desDataSource = WebServices.SecurityServices
                    .GetAssignableHierarchy(hierarchyId, WebSiteEnums.UserType.ALL.ToString())
                    .ToDataTable();
                desDataSource.Clear();
                uxRoleListSelected.DataSourceDestination = desDataSource;
                DataRow newRow1 = orginalDataSource.NewRow();
                newRow1[HIERARCHY_NAME_FIELD] = NEW_HIERARCHY_TEXT;
                newRow1[HIERARCHY_ID_FIELD] = NEW_HIERARCHY_VALUE;
                orginalDataSource.Rows.InsertAt(newRow1, 0);

                // 36127 - Enhancement in Edit User Page - Ownership Group 
                uxAccessFuncControl.SetSelectedByPermissionText("Add/Edit Relationship Manager");

                //44648 - VW - Implement Change Log to review permission changes made to user roles
                uxPlViewChangeLogStep1.Visible = uxPlViewChangeLogStep2.Visible = uxPlViewChangeLogStep3.Visible = false;

            }

            DataRow newRow2 = orginalDataSource.NewRow();
            newRow2[HIERARCHY_NAME_FIELD] = ALL_HIERARCHY_TEXT;
            newRow2[HIERARCHY_ID_FIELD] = ALL_HIERARCHY_VALUE;
            orginalDataSource.Rows.InsertAt(newRow2, 0);
            uxRoleListSelected.DataSourceOrigination = orginalDataSource;
            if (GeneralFuncsLib.IsCompliassureRoleSynch())
            {
                int role1099K = GetHierarchyIn1099KByHierarchyID(hierarchyId);
                doCheckPermissionSiteJump1099(uxTreeMenuCS.Nodes);
                if (uxPlc1099KRole.Visible)
                {
                    Bind1099KRoleList();
                    ux1099KRole.SelectedValue = role1099K.ToString();
                }
            }
            else
            {
                uxPlc1099KRole.Visible = false;
            }

            // PIC Admin Role
            if (GeneralFuncsLib.IsSynchPCIRole())
            {
                int rolePCI = GetHierarchyInPCIByHierarchyID(hierarchyId);
                doCheckPermissionSiteJumpPCI(uxTreeMenuCS.Nodes);
                if (uxPlcPCIRole.Visible)
                {
                    BindPCIRoleList();
                    uxPCIRole.SelectedValue = rolePCI.ToString();
                }
            }
            else
            {
                uxPlcPCIRole.Visible = false;
            }

            if (uxPlc1099KRole.Visible && uxPlcPCIRole.Visible)
            {
                uxheight1.Attributes.Add("style", "display:block;");
                uxPlcPCIRole_table.Attributes.Add("class", "max-width");
            }
            else if ((!uxPlc1099KRole.Visible && uxPlcPCIRole.Visible)
                || (uxPlc1099KRole.Visible && !uxPlcPCIRole.Visible))
            {
                uxheight1.Attributes.Add("style", "display:none;");
            }
            else
            {
                uxheight1.Attributes.Add("style", "display:none;");
            }
        }
    }

    private void initDefaultLandingPage()
    {
        SecMenuItemCollection menuItems = WebServices.SecurityServices.GetMenuItemsByUser(
            SessionManager.CurrentUser.ASClient,
            SessionManager.CurrentUser.UserID,
            SessionManager.CurrentUserRoles[0].HierarchyID,
            SessionManager.CurrentLanguage, true);

        SecMenuItem defaultMenu = new SecMenuItem();

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
                || Array.Exists<string>(pes, (Predicate<string>)delegate(string s) { return permissionCode.IndexOf(s, StringComparison.OrdinalIgnoreCase) > -1; })
                || WebSiteConstants.SEC_PERMISSION_MANAGE_DOCUMENT_TYPES.Equals(permissionCode, StringComparison.OrdinalIgnoreCase)
                )
            {
                menuItems.RemoveAt(i);
                continue;
            }

            //Remove menu is config on SitemapSetting.xml
            SecMenuItem menu = menus.Where(m => m.SiteMapId == menuItems[i].Parent).SingleOrDefault();
            if (SitemapHandler.IsRemoveFromSitemaps(menu.IsNullData() ? "" : menu.Title, menuItems[i].Title))
            {
                menuItems.Remove(menuItems[i]);
                continue;
            }

            if (WebSiteConstants.SEC_PERMISSION_DASHBOARD.Equals(permissionCode, StringComparison.OrdinalIgnoreCase) ||
                WebSiteConstants.SEC_PERMISSION_DASHBOARD_MS.Equals(permissionCode, StringComparison.OrdinalIgnoreCase))
            {
                defaultMenu = menuItems[i];
            }
        }


        uxDefaultLandingPage.DataTextField = "Title";
        uxDefaultLandingPage.DataValueField = "Permissions";
        uxDefaultLandingPage.DataFieldID = "SiteMapId";
        uxDefaultLandingPage.DataFieldParentID = "Parent";

        uxDefaultLandingPage.DataSource = menuItems;
        uxDefaultLandingPage.DataBind();
        uxDefaultLandingPage.ExpandAllDropDownNodes();


        if (hierarchyId > 0)
        {
            DataTable td = GeneralFuncsLib.GetDefaultLandingPage(hierarchyId);
            if (td.HasData())
            {
                uxDefaultLandingPage.SelectedValue = string.Format("{0}_{1}", td.Rows[0]["PermissionCodes"], td.Rows[0]["SiteMapId"]);
            }
        }
        else
        {
            if (!string.IsNullOrEmpty(defaultMenu.Url))
                uxDefaultLandingPage.SelectedValue = string.Format("{0}_{1}", defaultMenu.Permissions, defaultMenu.SiteMapId);
        }
    }


    protected bool moveNextStep = false;
    /// <summary>
    /// check a permission ID in set of permissions
    /// </summary>
    /// <param name="strPermission">permission code</param>
    /// <param name="permissions">set of permission</param>
    /// <returns></returns>
    private bool CheckPermissionIDInList(string strPermission, PermissionCollection permissions)
    {
        foreach (Permission p in permissions)
        {
            if (string.Compare(p.PermissionCode, strPermission) == 0)
                return true;
        }
        return false;
    }

    private void BindAccessFunction()
    {
        PermissionCollection permissions = WebServices.SecurityServices.GetPermissionsByUserGroupType(
                SessionManager.CurrentUser.ASClient,
                SessionManager.CurrentUser.UserID,
                SessionManager.CurrentSystem,
                WebSiteConstants.PERMISSION_GROUP_CS,
                WebSiteConstants.PERMISSION_TYPE_ACCESS_FUNC,
                SessionManager.CurrentLanguage);

        PermissionCollection boardingPermissions = WebServices.SecurityServices.GetPermissionsByUserGroupType(
                SessionManager.CurrentUser.ASClient,
                SessionManager.CurrentUser.UserID,
                SessionManager.CurrentSystem,
                WebSiteConstants.PERMISSION_GROUP_BOARDING,
                WebSiteConstants.PERMISSION_TYPE_ACCESS_FUNC,
                SessionManager.CurrentLanguage);

        foreach (Permission p in boardingPermissions)
        {
            permissions.Add(p);
        }

        //43745: Exclude Permission: Login from Landing page
        permissions = GeneralFuncsLib.ExcludeAccessPermission(permissions);

        //47441: Excluding Risk permissions. There is a specific business for Risk module
        // If the user who does not have "RiskMgmt" permission then excluding all permissions of Risk
        if (!SessionManager.CurrentUserPermissions.Contains(string.Format(",{0},", WebSiteConstants.SEC_PERMISSION_RSK_MGMT)))
        {

            List<Permission> per_Risk = (from Permission a in permissions.Cast<Permission>() where a.GroupFuncName.Equals(WebSiteConstants.PERMISSION_ACCESS_FUNC_RISK) select a).ToList();

            foreach (Permission permission in per_Risk)
            {
                permissions.Remove(permission);
            }
        }

        //add addition access function
        var accessFunctions = GeneralFuncsLib.GetAdditionAccessFunctions();
        if (accessFunctions.Any())
        {
            foreach (var item in accessFunctions)
            {
                permissions.Add(item);
            }            
        }

        uxAccessFuncControl.DataSource = AS.Web.Business.General.GeneralFuncsLib.BuildPermissionCollectionWithOrderByGroupName(permissions);
    }   

    protected void uxCheckUnique_Click(object sender, EventArgs e)
    {
        //uxCheckUnique
        if (hierarchyId > 0)//update
        {
            if (!WebServices.SecurityServices.CheckUniqueHierarchyForUpdate(
                SessionManager.CurrentUser.ASClient, uxRoleName.Text.Trim(), hierarchyId))
            {
                moveNextStep = true;
            }
            else
            {
                moveNextStep = false;
                Page.ClientScript.RegisterClientScriptBlock(
                    this.GetType(),
                    "CheckUnique",
                    "alert('" + GetLocalResourceObject("ManageRoleCS_Text_RoleExist").ToString() + "');",
                    true);
            }
        }
        else//create
        {
            if (!WebServices.SecurityServices.CheckUniqueHierarchyForCreate(
                    SessionManager.CurrentUser.ASClient, uxRoleName.Text.Trim()))
            {
                moveNextStep = true;
            }
            else
            {
                moveNextStep = false;
                Page.ClientScript.RegisterClientScriptBlock(
                    this.GetType(),
                    "CheckUnique",
                    "alert('" + GetLocalResourceObject("ManageRoleCS_Text_RoleExist").ToString() + "');",
                    true);
            }
        }
    }

    Permission FindPermissionById(PermissionCollection cols, int id)
    {
        foreach (Permission item in cols)
        {
            if (item.PermissionId == id)
                return item;
        }
        return null;
    }

    DataTable SelectDistinct(DataTable SourceTable, string FieldName)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add(FieldName, SourceTable.Columns[FieldName].DataType);
        dt.Columns.Add("ClientName", SourceTable.Columns["ClientName"].DataType);

        object LastValue = null;
        foreach (DataRow dr in SourceTable.Select("", FieldName))
        {
            if (LastValue == null || !(ColumnEqual(LastValue, dr[FieldName])))
            {
                LastValue = dr[FieldName];
                dt.Rows.Add(new object[] { LastValue, dr["ClientName"] });
            }
        }

        return dt;
    }

    private bool ColumnEqual(object A, object B)
    {

        // Compares two values to see if they are equal. Also compares DBNULL.Value.
        // Note: If your DataTable contains object fields, then you must extend this
        // function to handle them in a meaningful way if you intend to group on them.

        if (A == DBNull.Value && B == DBNull.Value) //  both are DBNull.Value
            return true;
        if (A == DBNull.Value || B == DBNull.Value) //  only one is DBNull.Value
            return false;
        return (A.Equals(B));  // value type standard comparison
    }

    protected void uxSave_Click(object sender, EventArgs e)
    {

        //create new hierachy
        Hierarchy hierarchy = new Hierarchy();
        bool isUpdateMode = false;
        if (hierarchyId > 0)
        {
            hierarchy = WebServices.SecurityServices.GetHierarchyById(hierarchyId);
            if (hierarchy == null)
            {
                Response.Redirect(WebSiteConstants.PAGE_NOT_FOUND_NAME);
                return;
            }
        }
        hierarchy.ClientId = SessionManager.CurrentUser.ASClient;
        hierarchy.HierarchyDescription = uxRoleDes.Text;
        hierarchy.HierarchyName = uxRoleName.Text.Trim();
        hierarchy.HierarchyLevel = null;

        if (hierarchy.HierarchyID > 0)
        {
            isUpdateMode = true;
            WebServices.SecurityServices.UpdateHierarchy(hierarchy, SessionManager.CurrentUser.RecId);
        }
        else
        {
            hierarchy.CreatedBy = SessionManager.CurrentUser.RecId;
            hierarchy.ActvStatus = "1";
            hierarchy.HierarchyParent = SessionManager.CurrentHierarchyId;
            hierarchy.SystemId = SessionManager.CurrentSystem;
            hierarchy.HierarchyID = WebServices.SecurityServices.CreateHierarchy(hierarchy);
        }

        checkAllMenu = true;
        var isLogged = hierarchyId > 0;
        _notificationPermissions = GetNotificationPermissions();
        _hasNotification = false;
        doAddPermissionForMenu(uxTreeMenuCS.Nodes, hierarchy.HierarchyID, isLogged);

        if (_hasNotification)
        {
            WebServices.SecurityServices.AddPermissionIntoHierarchy(WebSiteConstants.SEC_PERMISSION_NOTIFICATION_SETTING, hierarchy.HierarchyID, SessionManager.CurrentUser.RecId, isLogged);
        }
        else
        {
            WebServices.SecurityServices.RemovePermissionFromHierarchy(WebSiteConstants.SEC_PERMISSION_NOTIFICATION_SETTING, hierarchy.HierarchyID, SessionManager.CurrentUser.ASClient, SessionManager.CurrentUser.RecId, isLogged);
        }


        // - TK 35129
        doUpdateDefaultLandingPage(hierarchy.HierarchyID);
        //
        string[] RISK_PERS =
        {
            WebSiteConstants.SEC_PERMISSION_RSK_MAN_AS,
            WebSiteConstants.SEC_PERMISSION_RSK_ADHOC,
            WebSiteConstants.SEC_PERMISSION_RSK_PARAMS,
            WebSiteConstants.SEC_PERMISSION_RSK_SCORE,
            WebSiteConstants.SEC_PERMISSION_RSK_QUEUE,
            WebSiteConstants.SEC_PERMISSION_RSK_RETCB,
            WebSiteConstants.SEC_PERMISSION_RSK_PORT,
            WebSiteConstants.SEC_PERMISSION_RSK_ESC_QUEUE,
            WebSiteConstants.SEC_PERMISSION_RSK_RP,
            WebSiteConstants.SEC_PERMISSION_RSK_WATCH,
            WebSiteConstants.SEC_PERMISSION_RSK_GROUP,
            WebSiteConstants.SEC_PERMISSION_RSK_ESCA,
            WebSiteConstants.SEC_PERMISSION_RSK_RESO,
            WebSiteConstants.SEC_PERMISSION_RSK_MAR_DATA,
            WebSiteConstants.SEC_PERMISSION_AUTO_QUEUE
        };

        //47441: There is a specific business for Risk module
        // If the user does not have "RiskMgmt" permission then nothing need to update

        if (SessionManager.CurrentUserPermissions.Contains(string.Format(",{0},", WebSiteConstants.SEC_PERMISSION_RSK_MGMT)))
        {
            bool isRiskRole = false;
            foreach (string per in RISK_PERS)
            {
                if (searchForPermissionInTreeView(uxTreeMenuCS.Nodes, per))//risk role
                {
                    isRiskRole = true;
                    break;
                }
            }
            if (isRiskRole)
            {
                WebServices.SecurityServices.AddPermissionIntoHierarchy(
                    WebSiteConstants.SEC_PERMISSION_RSK_MGMT,
                    hierarchy.HierarchyID,
                    SessionManager.CurrentUser.RecId,
                    hierarchyId > 0);
            }
            else
            {
                WebServices.SecurityServices.RemovePermissionFromHierarchy(
                    WebSiteConstants.SEC_PERMISSION_RSK_MGMT,
                    hierarchy.HierarchyID,
                    SessionManager.CurrentUser.ASClient,
                    SessionManager.CurrentUser.RecId,
                    hierarchyId > 0);
            }
        }


        if (SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.AS
            || SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.CS)
        {
            WebServices.SecurityServices.AddPermissionIntoHierarchy(
                WebSiteConstants.SEC_PERMISSION_USER_PROF,
                hierarchy.HierarchyID,
                SessionManager.CurrentUser.RecId,
                hierarchyId > 0);
        }
        else
        {
            WebServices.SecurityServices.AddPermissionIntoHierarchy(
                WebSiteConstants.SEC_PERMISSION_USER_PROF_MS,
                hierarchy.HierarchyID,
                SessionManager.CurrentUser.RecId,
                hierarchyId > 0);
        }

        //update assign role
        bool hasPer = false;
        hasPer = searchForPermissionInTreeView(uxTreeMenuCS.Nodes, WebSiteConstants.SEC_PERMISSION_MAN_USER);

        // Add permission Reset User PWD TK24092 
        if (hasPer)
        {
            WebServices.SecurityServices.AddPermissionIntoHierarchy(
                WebSiteConstants.SEC_PERMISSION_RESET_PWD,
                hierarchy.HierarchyID,
                SessionManager.CurrentUser.RecId,
                hierarchyId > 0);
        }
        else
        {
            WebServices.SecurityServices.RemovePermissionFromHierarchy(
                WebSiteConstants.SEC_PERMISSION_RESET_PWD,
                hierarchy.HierarchyID,
                SessionManager.CurrentUser.ASClient,
                SessionManager.CurrentUser.RecId,
                hierarchyId > 0);
        }

        if (!hasPer)
        {
            uxRoleListSelected.DataSourceDestination.Clear();
        }

        if (uxRoleListSelected.DataSourceDestination.Rows.Count == 1
            && uxRoleListSelected.DataSourceDestination.Rows[0][HIERARCHY_ID_FIELD].ToString() == ALL_HIERARCHY_VALUE)  // Add All 
        {
            for (int i = 0; i < uxRoleListSelected.DataSourceOrigination.Rows.Count; i++)
            {
                if (uxRoleListSelected.DataSourceOrigination.Rows[i][HIERARCHY_ID_FIELD].ToString().Equals(NEW_HIERARCHY_VALUE)
                    || uxRoleListSelected.DataSourceOrigination.Rows[i][HIERARCHY_ID_FIELD].ToString().Equals(ALL_HIERARCHY_VALUE))
                {
                    uxRoleListSelected.DataSourceOrigination.Rows.RemoveAt(i);
                }
            }

            doUpdateAssignableHierarchy(hierarchy.HierarchyID, uxRoleListSelected.DataSourceOrigination, isUpdateMode);
        }
        else
        {
            doUpdateAssignableHierarchy(hierarchy.HierarchyID, uxRoleListSelected.DataSourceDestination, isUpdateMode);
        }

        // Add MPS Admin to SEC_AssignableHierarchy with AssignalbeHierarchyID will created role
        Hierarchy curHierarchy = WebServices.SecurityServices.GetHierarchyById(SessionManager.CurrentHierarchyId);

        //Redmine TK16711 use hierarchyId variable and not SessionManager.CurrentHierarchyId variable
        if (curHierarchy == null)
            curHierarchy = WebServices.SecurityServices.GetHierarchyById(hierarchyId);

        if (!curHierarchy.HierarchyName.Equals("CS Users") && !curHierarchy.HierarchyName.Equals("AS Users"))
        {
            if (hierarchyId == 0) // Only add when create new role
            {
                //41907 - VW - Aperia - Enrich User Audit Report (Billable)
                var assignableHierarchyModel = new AssignableHierarchyModel()
                {
                    HierarchyId = SessionManager.CurrentHierarchyId,
                    AssignHierarchyId = hierarchy.HierarchyID,
                    IsRemove = false,
                    AsClientId = SessionManager.CurrentClient,
                    SiteId = SessionManager.CurrentUser.SiteID,
                    UserId = SessionManager.CurrentUser.UserID,
                    ChangedByRecId = SessionManager.CurrentUser.RecId,
                    IsUpdate = isUpdateMode
                };
                WebServices.SecurityServices.UpdateAssignableHierarchy(assignableHierarchyModel);
            }
        }

        //

        //  36127 - Enhancement in Edit User Page - Ownership Group
        foreach (AS.Controls.Global.CheckBox item in uxAccessFuncControl.Items)
        {
            string permissionCode = item.Value;
            bool ischecked = item.Checked;

            if (ischecked)
            {
                WebServices.SecurityServices.AddPermissionIntoHierarchy(
                    permissionCode,
                    hierarchy.HierarchyID,
                    SessionManager.CurrentUser.RecId,
                    hierarchyId > 0);
            }
            else
            {
                WebServices.SecurityServices.RemovePermissionFromHierarchy(
                    permissionCode,
                    hierarchy.HierarchyID,
                    SessionManager.CurrentUser.ASClient,
                    SessionManager.CurrentUser.RecId,
                    hierarchyId > 0);
            }
        }

        // Add access function for hierarchy
        //for (int i = 0; i < uxAccessFunctionList.Items.Count; i++)
        //{
        //    if (uxAccessFunctionList.Items[i].Selected)
        //    {
        //        WebServices.SecurityServices.AddPermissionIntoHierarchy(
        //            uxAccessFunctionList.Items[i].Value,
        //            hierarchy.HierarchyID,
        //            SessionManager.CurrentUser.RecId,
        //            hierarchyId > 0);
        //    }
        //    else
        //    {
        //        WebServices.SecurityServices.RemovePermissionFromHierarchy(
        //            uxAccessFunctionList.Items[i].Value,
        //            hierarchy.HierarchyID,
        //            SessionManager.CurrentUser.ASClient,
        //            SessionManager.CurrentUser.RecId,
        //            hierarchyId > 0);
        //    }
        //}

        //GroupPermissions
        //if (isUpdateMode)
        //{
        PermissionCollection permissions = WebServices.SecurityServices.GetPermissionsByUserGroupType(
                SessionManager.CurrentUser.ASClient,
                SessionManager.CurrentUser.UserID,
                SessionManager.CurrentSystem,
                WebSiteConstants.PERMISSION_GROUP_BOARDING_TASK,
                string.Empty,
                SessionManager.CurrentLanguage);
        bool hasTaskListpermission = searchForPermissionInTreeView(uxTreeMenuCS.Nodes, WebSiteConstants.SEC_PERMISSION_VIEW_TASK_LIST);
        var groupPermissions = SessionManager.SelectedGroupPermission.Cast<Permission>();

        foreach (Permission p in permissions)
        {
            //Step 1: If role has ViewTaskList permission then add all selected permissions into role  
            if (hasTaskListpermission)
            {
                if (groupPermissions.Any(m => m.PermissionCode == p.PermissionCode))
                {
                    WebServices.SecurityServices.AddPermissionIntoHierarchy(
                       p.PermissionCode,
                       hierarchy.HierarchyID,
                       SessionManager.CurrentUser.RecId,
                       hierarchyId > 0);
                }
                else
                {
                    WebServices.SecurityServices.RemovePermissionFromHierarchy(
                   p.PermissionCode,
                   hierarchy.HierarchyID,
                   SessionManager.CurrentUser.ASClient,
                   SessionManager.CurrentUser.RecId,
                   hierarchyId > 0);
                }

            }
            else if (isUpdateMode)//Step 2: Else if role does not has ViewTaskList permission then remove all boarding permissions
            {
                WebServices.SecurityServices.RemovePermissionFromHierarchy(
                    p.PermissionCode,
                    hierarchy.HierarchyID,
                    SessionManager.CurrentUser.ASClient,
                    SessionManager.CurrentUser.RecId,
                    hierarchyId > 0);
            }
        }
        //}

        //if (searchForPermissionInTreeView(uxTreeMenuCS.Nodes, WebSiteConstants.SEC_PERMISSION_VIEW_TASK_LIST))//ViewTaskList
        //{
        //    foreach (Permission permission in SessionManager.SelectedGroupPermission)
        //    {
        //        if (!permission.PermissionCode.IsNullOrEmpty())
        //        {
        //            WebServices.SecurityServices.AddPermissionIntoHierarchy(
        //                permission.PermissionCode,
        //                hierarchy.HierarchyID,
        //                SessionManager.CurrentUser.RecId,
        //                hierarchyId > 0);
        //        }
        //    }
        //}

        // if edit role edit all child role
        if (hierarchyId > 0)
        {
            WebServices.SecurityServices.UpdateChildHierarchyPermission(hierarchyId);
        }

        if (GeneralFuncsLib.IsCompliassureRoleSynch())
        {
            int hierarchyID_1099K = 0;
            if (uxPlc1099KRole.Visible)
            {
                hierarchyID_1099K = Convert.ToInt32(ux1099KRole.SelectedValue);
            }
            InsertUpdateHierarchyIn1099K(hierarchy.HierarchyID, hierarchyID_1099K);
        }

        if (GeneralFuncsLib.IsSynchPCIRole())
        {
            int hierarchyID_PCI = 0;
            if (uxPlcPCIRole.Visible)
            {
                hierarchyID_PCI = Convert.ToInt32(uxPCIRole.SelectedValue);
            }
            InsertUpdateHierarchyInPCI(hierarchy.HierarchyID, hierarchyID_PCI);
        }

        this.Page.ClientScript.RegisterClientScriptBlock(
            GetType(),
            "closemode",
            "parent.rebindRole()",
            true);
    }

    bool searchForPermissionInTreeView(Telerik.Web.UI.RadTreeNodeCollection nodes, string permissionCode)
    {
        if (nodes == null || nodes.Count == 0)
        {
            return false;
        }
        for (int i = 0; i < nodes.Count; i++)
        {
            if (searchForPermissionInTreeView(nodes[i].Nodes, permissionCode))
            {
                return true;
            }
            if (nodes[i].Value.IndexOf(",") == -1)
            {
                if (nodes[i].Checked && nodes[i].Value.ToLower() == permissionCode.ToLower())
                {
                    return true;
                }
            }
        }
        return false;
    }

    protected void uxRoleListSelected_MovedData(object sender, EventArgs e)
    {
        DataTable destinationSource = uxRoleListSelected.DataSourceDestination;
        DataTable originalSource = uxRoleListSelected.DataSourceOrigination;

        if (destinationSource.Rows.Count >= 1)
        {
            if (destinationSource.Rows[0][HIERARCHY_ID_FIELD].ToString() == ALL_HIERARCHY_VALUE
                || destinationSource.Rows[destinationSource.Rows.Count - 1][HIERARCHY_ID_FIELD].ToString() == ALL_HIERARCHY_VALUE) // ALL
            {
                if (destinationSource.Rows[0][HIERARCHY_ID_FIELD].ToString() == ALL_HIERARCHY_VALUE)
                {
                    destinationSource.Rows.RemoveAt(0);
                }
                else
                {
                    destinationSource.Rows.RemoveAt(destinationSource.Rows.Count - 1);
                }
                foreach (DataRow row in destinationSource.Rows)
                {
                    DataRow tempRow = originalSource.NewRow();
                    tempRow[HIERARCHY_NAME_FIELD] = row[HIERARCHY_NAME_FIELD].ToString();
                    tempRow[HIERARCHY_ID_FIELD] = row[HIERARCHY_ID_FIELD].ToString();
                    originalSource.Rows.Add(tempRow);
                }
                DataRow newRow = destinationSource.NewRow();
                destinationSource.Clear();
                newRow[HIERARCHY_NAME_FIELD] = ALL_HIERARCHY_TEXT;
                newRow[HIERARCHY_ID_FIELD] = ALL_HIERARCHY_VALUE;
                destinationSource.Rows.InsertAt(newRow, 0);
                uxRoleListSelected.DataSourceDestination = destinationSource;
                uxRoleListSelected.DataSourceOrigination = originalSource;
            }
            uxHddSelectedRole.Value = "true";
        }
        else
        {
            uxHddSelectedRole.Value = "false";
        }
    }

    void doUpdateAssignableHierarchy(int hierarchyId, DataTable list, bool isUpdateMode)
    {
        bool isOk = true;

        int assignId = 0;
        //get current hierarchy
        HierarchyCollection cols = WebServices.SecurityServices.GetAssignableHierarchy(hierarchyId, WebSiteEnums.UserType.ALL.ToString());
        //add new item

        for (int i = 0; i < list.Rows.Count; i++)
        {
            isOk = true;
            for (int k = 0; k < cols.Count; k++)
            {
                if (list.Rows[i][HIERARCHY_ID_FIELD].ToString() == cols[k].HierarchyID.ToString())
                {
                    isOk = false;
                    break;
                }
            }
            if (isOk)//no record is matched => new record
            {
                if (list.Rows[i][HIERARCHY_ID_FIELD].ToString() == NEW_HIERARCHY_VALUE)  //[This new role]
                {
                    assignId = hierarchyId;
                }
                else
                {
                    assignId = int.Parse(list.Rows[i][HIERARCHY_ID_FIELD].ToString());
                }
                //41907 - VW - Aperia - Enrich User Audit Report (Billable)
                var assignableHierarchyModel = new AssignableHierarchyModel()
                {
                    HierarchyId = hierarchyId,
                    AssignHierarchyId = assignId,
                    IsRemove = false,
                    AsClientId = SessionManager.CurrentClient,
                    SiteId = SessionManager.CurrentUser.SiteID,
                    UserId = SessionManager.CurrentUser.UserID,
                    ChangedByRecId = SessionManager.CurrentUser.RecId,
                    IsUpdate = isUpdateMode
                };
                WebServices.SecurityServices.UpdateAssignableHierarchy(assignableHierarchyModel);
            }
        }

        //remove item
        for (int i = 0; i < cols.Count; i++)
        {
            isOk = true;
            for (int k = 0; k < list.Rows.Count; k++)
            {
                if (list.Rows[k][HIERARCHY_ID_FIELD].ToString() == cols[i].HierarchyID.ToString())
                {
                    isOk = false;
                    break;
                }
            }
            if (isOk)//no record is matched => need to be removed
            {
                //41907 - VW - Aperia - Enrich User Audit Report (Billable)
                assignId = cols[i].HierarchyID;
                var assignableHierarchyModel = new AssignableHierarchyModel()
                {
                    HierarchyId = hierarchyId,
                    AssignHierarchyId = assignId,
                    IsRemove = true,
                    AsClientId = SessionManager.CurrentClient,
                    SiteId = SessionManager.CurrentUser.SiteID,
                    UserId = SessionManager.CurrentUser.UserID,
                    ChangedByRecId = SessionManager.CurrentUser.RecId,
                    IsUpdate = isUpdateMode
                };
                WebServices.SecurityServices.UpdateAssignableHierarchy(assignableHierarchyModel);
            }
        }
    }

    bool checkAllMenu = true;
    void doAddPermissionForMenu(Telerik.Web.UI.RadTreeNodeCollection nodes, int hierarchyId, bool isLogged)
    {
        if (nodes == null || nodes.Count == 0)
            return;
        for (int i = 0; i < nodes.Count; i++)
        {
            doAddPermissionForMenu(nodes[i].Nodes, hierarchyId, isLogged);
            if (nodes[i].Value.IndexOf(",") == -1)
            {
                if (nodes[i].Checked)
                {
                    if (!_hasNotification)
                        _hasNotification = CheckNotificationSetting(nodes[i].Value);
                    WebServices.SecurityServices.AddPermissionIntoHierarchy(
                        nodes[i].Value, hierarchyId, SessionManager.CurrentUser.RecId, isLogged);
                }
                else
                {
                    WebServices.SecurityServices.RemovePermissionFromHierarchy(
                        nodes[i].Value, hierarchyId, SessionManager.CurrentUser.ASClient,
                        SessionManager.CurrentUser.RecId, isLogged);
                    checkAllMenu = false;
                }
            }
        }
    }

    void doCheckPermissionForMenu(Telerik.Web.UI.RadTreeNodeCollection nodes, PermissionCollection permissions)
    {
        if (nodes == null || nodes.Count == 0)
            return;
        for (int i = 0; i < nodes.Count; i++)
        {
            doCheckPermissionForMenu(nodes[i].Nodes, permissions);
            for (int j = 0; j < permissions.Count; j++)
            {
                if (nodes[i].Value == permissions[j].PermissionCode)
                {
                    nodes[i].Checked = true;
                    break;
                }
            }
        }
    }

    void doCheckPermissionSiteJump1099(Telerik.Web.UI.RadTreeNodeCollection nodes)
    {
        if (nodes == null || nodes.Count == 0)
            return;
        for (int i = 0; i < nodes.Count; i++)
        {
            doCheckPermissionSiteJump1099(nodes[i].Nodes);
            if (nodes[i].Value == WebSiteConstants.SEC_PERMISSION_SITE_JUMP_1099K
                && nodes[i].Checked)
            {
                uxPlc1099KRole.Visible = true;
                break;
            }
        }
    }

    void doCheckPermissionSiteJumpPCI(Telerik.Web.UI.RadTreeNodeCollection nodes)
    {
        if (nodes == null || nodes.Count == 0)
            return;
        for (int i = 0; i < nodes.Count; i++)
        {
            doCheckPermissionSiteJumpPCI(nodes[i].Nodes);
            if (nodes[i].Value.ToLower() == "siteaccesspciadmin" && nodes[i].Checked)
            {
                uxPlcPCIRole.Visible = true;
                break;
            }
        }
    }


    void doUpdateDefaultLandingPage(int hierarchyID)
    {
        //41907 - VW - Aperia - Enrich User Audit Report (Billable)
        string seletedValue = !string.IsNullOrEmpty(uxDefaultLandingPage.SelectedValue.ToSafeString()) ? uxDefaultLandingPage.SelectedValue.ToSafeString() : uxDefaultLandingPageSelectedValue.Value;
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(true);
        //parameters.Add(new FilterParameter("@ASClient", SessionManager.CurrentUser.ASClient, DbType.Int32));
        parameters.Add(new FilterParameter("@SiteMapID", seletedValue.Split('_')[seletedValue.Split('_').Length - 1], DbType.Int32));
        parameters.Add(new FilterParameter("@HierarchyID", hierarchyID, DbType.Int32));
        parameters.Add(new FilterParameter("@ChangedByRecID", SessionManager.CurrentUser.RecId, DbType.Guid));

        WebServices.SecurityServices.GetReports("spa_SEC_UpdateDefaultLandingPage", parameters);
    }

    private void Bind1099KRoleList()
    {
        ux1099KRole.Items.Clear();
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(true);
        parameters.Add(new FilterParameter("@UserType", SessionManager.UserRoleType.ToString(), DbType.AnsiString));
        DataTable roleList = WebServices.SecurityServices.GetReports("spa_SEC_Get1099KHierarchiesList", parameters);
        ux1099KRole.DataSource = roleList;
        ux1099KRole.DataTextField = HIERARCHY_NAME_FIELD;
        ux1099KRole.DataValueField = HIERARCHY_ID_FIELD;
        ux1099KRole.DataBind();
    }

    private void BindPCIRoleList()
    {
        uxPCIRole.Items.Clear();
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(true);
        DataTable roleList = WebServices.SecurityServices.GetReports("spa_SEC_GetPCIHierarchiesList", parameters);
        uxPCIRole.DataSource = roleList;
        uxPCIRole.DataTextField = HIERARCHY_NAME_FIELD;
        uxPCIRole.DataValueField = HIERARCHY_ID_FIELD;
        uxPCIRole.DataBind();
    }

    private int GetHierarchyIn1099KByHierarchyID(int hierarchyId)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.Add(new FilterParameter("@HierarchyId", hierarchyId, DbType.Int32));

        DataTable hierachyIn1099KList = WebServices.SecurityServices.GetReports("spa_SEC_GetHierarchyIn1099KById", parameters);
        if (hierachyIn1099KList != null && hierachyIn1099KList.Rows.Count > 0)
        {
            return Convert.ToInt32(hierachyIn1099KList.Rows[0][0]);
        }
        else
        {
            return -1;
        }
    }

    private int GetHierarchyInPCIByHierarchyID(int hierarchyId)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.Add(new FilterParameter("@HierarchyId", hierarchyId, DbType.Int32));
        parameters.Add(new FilterParameter("@ASClientID", SessionManager.CurrentClient, DbType.Int32));
        DataTable hierachyInPCIList = WebServices.SecurityServices.GetReports("spa_SEC_GetHierarchyInPCIById", parameters);
        if (hierachyInPCIList != null && hierachyInPCIList.Rows.Count > 0)
        {
            return Convert.ToInt32(hierachyInPCIList.Rows[0][0]);
        }
        else
        {
            return -1;
        }
    }

    private void InsertUpdateHierarchyIn1099K(int hierarchyID, int hierarchyIn1099K)
    {
        //41907 - VW - Aperia - Enrich User Audit Report (Billable)
        FilterParameterCollection parameters = new FilterParameterCollection();
        FilterParameterCollection paramOuts = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(true);
        parameters.Add(new FilterParameter("@HierarchyID", hierarchyID, DbType.Int32));
        parameters.Add(new FilterParameter("@HierarchyID_1099K", hierarchyIn1099K, DbType.Int32));
        parameters.Add(new FilterParameter("@ChangedByRecID", SessionManager.CurrentUser.RecId, DbType.Guid));
        WebServices.SecurityServices.GetReports("spa_SEC_InsertUpdateHierarchyIn1099K", parameters);
    }

    private void InsertUpdateHierarchyInPCI(int hierarchyID, int hierarchyInPci)
    {
        //41907 - VW - Aperia - Enrich User Audit Report (Billable)
        FilterParameterCollection parameters = new FilterParameterCollection();
        FilterParameterCollection paramOuts = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(true);
        parameters.Add(new FilterParameter("@HierarchyID", hierarchyID, DbType.Int32));
        parameters.Add(new FilterParameter("@HierarchyID_PCI", hierarchyInPci, DbType.Int32));
        parameters.Add(new FilterParameter("@ChangedByRecID", SessionManager.CurrentUser.RecId, DbType.Guid));
        WebServices.SecurityServices.GetReports("spa_SEC_InsertUpdateHierarchyInPCI", parameters);
    }

    protected void CheckRole1099K(object sender, EventArgs e)
    {
        if (uxhdRole1099.Value == "true")
        {
            uxPlc1099KRole.Visible = true;
            Bind1099KRoleList();
        }
        else
        {
            uxPlc1099KRole.Visible = false;
        }

        if (uxPlc1099KRole.Visible || uxPlcPCIRole.Visible)
        {
            string op2 = uxPlc1099KRole.Visible ? "true" : "false";
            ((NonReportPage)this.Page).AjaxAddResponseScript(
                string.Format(SHOW_HIDE_ALT_ROLE, "true", op2));
        }
        else
        {
            ((NonReportPage)this.Page).AjaxAddResponseScript(
                string.Format(SHOW_HIDE_ALT_ROLE, "false", "false"));
        }

        ((NonReportPage)this.Page).AjaxAddResponseScript("AdjustModalSize();");
    }

    protected void CheckRolePCI(object sender, EventArgs e)
    {
        if (uxhdRolePCI.Value == "true" && GeneralFuncsLib.IsSynchPCIRole())
        {
            uxPlcPCIRole.Visible = true;
            BindPCIRoleList();
        }
        else
        {
            uxPlcPCIRole.Visible = false;
        }

        if (uxPlc1099KRole.Visible || uxPlcPCIRole.Visible)
        {
            string op2 = uxPlc1099KRole.Visible ? "true" : "false";
            ((NonReportPage)this.Page).AjaxAddResponseScript(
                string.Format(SHOW_HIDE_ALT_ROLE, "true", op2));
        }
        else
        {
            ((NonReportPage)this.Page).AjaxAddResponseScript(
                 string.Format(SHOW_HIDE_ALT_ROLE, "false", "false"));
        }
        ((NonReportPage)this.Page).AjaxAddResponseScript("AdjustModalSize();");
    }

    protected void CheckPCIAnd1099K(object sender, EventArgs e)
    {
        if (uxhdRolePCI.Value == "true" && GeneralFuncsLib.IsSynchPCIRole())
        {
            uxPlcPCIRole.Visible = true;
            BindPCIRoleList();
        }
        else
        {
            uxPlcPCIRole.Visible = false;
        }

        if (uxhdRole1099.Value == "true")
        {
            uxPlc1099KRole.Visible = true;
            Bind1099KRoleList();
        }
        else
        {
            uxPlc1099KRole.Visible = false;
        }

        ((NonReportPage)this.Page).AjaxAddResponseScript(string.Format(
            SHOW_HIDE_ALT_ROLE,
            uxPlc1099KRole.Visible ? "true" : "false",
            uxPlcPCIRole.Visible ? "true" : "false"));

        ((NonReportPage)this.Page).AjaxAddResponseScript("AdjustModalSize();");
    }

    #region Private Methods

    private void BindTreeMenu()
    {
        SecMenuItem defaultMenu = new SecMenuItem();
        uxTreeMenuCS.DataTextField = "Title";
        uxTreeMenuCS.DataValueField = "Permissions";
        uxTreeMenuCS.DataFieldID = "SiteMapId";
        uxTreeMenuCS.DataFieldParentID = "Parent";

        SecMenuItemCollection menuItems = WebServices.SecurityServices.GetMenuItemsByUser(
            SessionManager.CurrentUser.ASClient,
            SessionManager.CurrentUser.UserID,
            SessionManager.CurrentUserRoles[0].HierarchyID,
            SessionManager.CurrentLanguage, true);
        var menus = menuItems.IsNotNullData() ? menuItems.Cast<SecMenuItem>() : new List<SecMenuItem>();
        for (int i = 0; i < menuItems.Count; i++)
        {
            SecMenuItem menu = menus.Where(m => m.SiteMapId == menuItems[i].Parent).SingleOrDefault();
            if (SitemapHandler.IsRemoveFromSitemaps(menu.IsNullData() ? "" : menu.Title, menuItems[i].Title))
            {
                menuItems.Remove(menuItems[i]);
                i--;
                continue;
            }

            var permissionCode = menuItems[i].Permissions;
            var title = menuItems[i].Title;
            if (WebSiteConstants.SEC_PERMISSION_MANAGE_DOCUMENT_TYPES.Trim().Equals(permissionCode.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                menuItems.Remove(menuItems[i]);
                i--;
                continue;
            }
            //// temp fix for Manage Auto queue & Temporarily Exclude Work Queue 
            //if (WebSiteConstants.SEC_PERMISSION_MAN_ROLE.Equals(permissionCode)
            //    || title.Equals("Temporarily Exclude Work Queue", StringComparison.OrdinalIgnoreCase) 
            //    || title.Equals("Excluir la cola de trabajo temporal", StringComparison.OrdinalIgnoreCase)
            //    )
            //{
            //    menuItems.RemoveAt(i);
            //}

            //Remove menu is config on SitemapSetting.xml
            if (WebSiteConstants.SEC_PERMISSION_DASHBOARD.Equals(permissionCode, StringComparison.OrdinalIgnoreCase) ||
                WebSiteConstants.SEC_PERMISSION_DASHBOARD_MS.Equals(permissionCode, StringComparison.OrdinalIgnoreCase))
            {
                defaultMenu = menuItems[i];
            }

            if (WebSiteConstants.SEC_PERMISSION_VIEW_NOTIFICATION_SETTING.Equals(permissionCode, StringComparison.OrdinalIgnoreCase))
            {
                menuItems[i].Permissions = WebSiteConstants.SEC_PERMISSION_NOTIFICATION_ADMIN_SETTING;
            }
            // 43534 -  remove document type

        }

        uxTreeMenuCS.DataSource = menuItems;
        uxTreeMenuCS.DataBind();
        uxTreeMenuCS.ExpandAllNodes();

        if (!string.IsNullOrEmpty(defaultMenu.Permissions) && hierarchyId == 0)
        {
            RadTreeNode node = uxTreeMenuCS.FindNodeByText(defaultMenu.Title);
            if (node != null)
                node.Checked = true;
        }

        initDefaultLandingPage();

    }

    #endregion Private Methods

    #endregion Methods

    protected void uxTreeMenuCS_NodeDataBound(object sender, RadTreeNodeEventArgs e)
    {
        SecMenuItem element = (SecMenuItem)e.Node.DataItem;
        e.Node.Attributes.Add("SiteMapId", element.SiteMapId.ToString());
    }
    protected void uxDefaultLandingPage_NodeDataBound(object sender, DropDownTreeNodeDataBoundEventArguments e)
    {
        SecMenuItem element = (SecMenuItem)e.DropDownTreeNode.DataItem;
        e.DropDownTreeNode.Value = e.DropDownTreeNode.Value + "_" + element.SiteMapId.ToString();


    }

    //45213 - [AW_Aperia] - Allied Wallet VW Implementation
    //Allied Wallet has CM permissions but don't have NotificationSetting
    private bool CheckNotificationSetting(string permissionCode)
    {
        var isExcludeNotification = GeneralFuncsLib.GetClientExtendedSetting("ExcludeNotificationSettings");

        if (isExcludeNotification.Data.IsNotNullData()
            && isExcludeNotification.Data.ToLower().Equals("true"))
        {
            return false;
        }
        else
        {
            foreach (string per in _notificationPermissions)
            {
                if (per.Equals(permissionCode, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
        }
        return false;
    }

    private IList<string> GetNotificationPermissions()
    {
        return GeneralFuncsLib.GetNotificationPermissionsOfSourceApp();
    }
}
