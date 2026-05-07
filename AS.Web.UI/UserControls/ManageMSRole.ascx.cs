using AS.Common;
using AS.Common.DBManager;
using AS.Security.WS.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Telerik.Web.UI;

public partial class UserControls_ManageMSRole : GlobalUserControl
{
    #region Constants

    private const string HUSER_PRIMARY = "HUSERS";
    private const string HUSER_SECOND = "HUSERS_SEC";
    private const string MUSER_PRIMARY = "MUSERS";
    private const string MUSER_SECOND = "MUSERS_SEC";
    private const string EXCLUDE_PERMISSIONS_BY_HIERARCHY = "EXCLUDE_PERMISSIONS_BY_HIERARCHY";

    private const string HLEVEL_BANK = "BANK";

    // SPAs
    private const string SPA_GET_HIERARCHY_LEVEL = "spa_SEC_GetHierarchyLevelForMSRole";
    private const string SPA_GET_1099K_LIST = "spa_SEC_Get1099KHierarchiesList";
    private const string SPA_GET_PCI_ROLE_LIST = "spa_SEC_GetPCIHierarchiesList";
    private const string SPA_GET_HIERARCHY_IN_1099K_BY_ID = "spa_SEC_GetHierarchyIn1099KById";
    private const string SPA_GET_HIERARCHY_IN_PCI_BY_ID = "spa_SEC_GetHierarchyInPCIById";

    // Assign User Role
    private const string ROLE_HIERARCHY_NAME = "HierarchyName";
    private const string ROLE_HIERARCHY_ID = "HierarchyID";
    private string ROLE_ALL_VALUE = string.Empty;
    private const string ROLE_ALL_ID = "-1";
    private string NEW_ROLE_VALUE = string.Empty;
    private const string NEW_ROLE_ID = "0";

    // Hierarchy Level
    private const string HL_NAME = "DataText";
    private const string HL_ID = "DataKey";
    protected string HL_SELECT_HIERARCHY_VALUE = string.Empty;
    protected const string HL_SELECT_HIERARCHY_ID = "0";
    protected string HL_ALL_VALUE = "All";

    private const string PAGE_NOT_FOUND = "404.htm";

    #endregion Constants

    #region Enums

    #endregion Enums

    #region Fields

    private int _hierarchyId = 0;
    private bool _checkAllMenu = true;
    private string[] RISK_PERS = 
    {
        WebSiteConstants.SEC_PERMISSION_RSK_MAN_AS_MS,
        WebSiteConstants.SEC_PERMISSION_RSK_ADHOC_MS,
        WebSiteConstants.SEC_PERMISSION_RSK_PARAMS_MS,
        WebSiteConstants.SEC_PERMISSION_RSK_SCORE_MS,
        WebSiteConstants.SEC_PERMISSION_RSK_QUEUE_MS,
        WebSiteConstants.SEC_PERMISSION_RSK_RETCB_MS,
        WebSiteConstants.SEC_PERMISSION_RSK_PORT_MS,
        WebSiteConstants.SEC_PERMISSION_RSK_ESC_QUEUE_MS,
        WebSiteConstants.SEC_PERMISSION_RSK_RP_MS,
        WebSiteConstants.SEC_PERMISSION_RSK_WATCH_MS,
        WebSiteConstants.SEC_PERMISSION_RSK_GROUP_MS,
        WebSiteConstants.SEC_PERMISSION_RSK_ESCA_MS,
        WebSiteConstants.SEC_PERMISSION_RSK_RESO_MS,
        WebSiteConstants.SEC_PERMISSION_RSK_MAR_DATA_MS,
        WebSiteConstants.SEC_PERMISSION_AUTO_QUEUE
    };
    private IList<string> _notificationPermissions;
    private bool _hasNotification;
    #endregion Fields

    #region Properties

    protected bool MoveSecondStep
    {
        get;
        set;
    }

    protected bool MoveFourthStep
    {
        get;
        set;
    }

    public bool IsEditMode
    {
        get;
        set;
    }

    private string HierarchyLevel
    {
        get
        {
            return uxHierarchyLevel.SelectedValue.IsNullOrEmpty()
                ? null : uxHierarchyLevel.SelectedValue;
        }
    }

    private string HierarchyUserType
    {
        get
        {
            string userType = string.Empty;
            if (HierarchyLevel != null && HierarchyLevel.Equals("MERCHANT"))
            {
                userType = uxRadPrimaryUser.Checked ? MUSER_PRIMARY : MUSER_SECOND;
            }
            else
            {
                userType = uxRadPrimaryUser.Checked ? HUSER_PRIMARY : HUSER_SECOND;
            }
            return userType;
        }
    }

    private bool IsMerchantLevel
    {
        get
        {
            if (HierarchyLevel != null && HierarchyLevel.Equals("MERCHANT", StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }
    }

    protected string DisableSecondaryUserRoleByEntities
    {
        get
        {
            string excludeHierachies = GeneralFuncsLib.GetDataOfExtendedSetting("DisableSecondaryUserRoleByEntities");
            if (!string.IsNullOrEmpty(excludeHierachies))
            {
                return string.Format(",{0},", excludeHierachies);
            }
            return string.Empty;
        }
    }

    protected bool EnableThisNewRoleOption
    {
        get
        {
            string value = GeneralFuncsLib.GetDataOfExtendedSetting("ENABLE_MS_THIS_NEW_ROLE_OPTION");
            if (value.IsNullOrEmpty())
                return false;

            return value.ToBoolean();
        }
    }

    protected bool DisableStepCreateMSRole
    {
        get
        {
            var disableStepCreateMSRole =
                GeneralFuncsLib.GetClientExtendedSetting("DisableStepCreateMSRole");
            return disableStepCreateMSRole.Data != null && disableStepCreateMSRole.Data.ToLower().Equals("true");
        }
    }

    #endregion Properties

    #region Methods

    #region Protected Methods

    protected void Page_Load(object sender, EventArgs e)
    {
        HL_ALL_VALUE = GetLocalResourceObject("ManageMSRoleCS_Text_All").ToString();
        HL_SELECT_HIERARCHY_VALUE = GetLocalResourceObject("ManageMSRole_ascx_cs_SelectHierarchy").ToString();
        ROLE_ALL_VALUE = GetLocalResourceObject("ManageMSRole_ascx_cs_ALL").ToString();
        NEW_ROLE_VALUE = GetLocalResourceObject("ManageMSRole_ascx_cs_ThisNewRole").ToString();
        if (this.Page.IsSecureQueryString)
        {
            int.TryParse(this.Page.SecureQueryString["id"], out _hierarchyId);
            IsEditMode = _hierarchyId > 0;
        }
        MoveSecondStep = false;
        MoveFourthStep = false;

        if (!IsPostBack)
        {
            //Re-register attribute for control
            uxRoleDes.Attributes.Add("maxlength", uxRoleDes.MaxLength.ToString());

            Hierarchy hierarchy = null;
            PermissionCollection permissions = null;

            // If the HierarchyId doesn't exist, redirect to "404.html" page
            if (!IsExistHierarchy(ref hierarchy, ref permissions))
            {
                return;
            }

            LoadHierarchyLevel();

            string hierarchyId = HierarchyLevel;
            // in case Edit mode
            if (IsEditMode && hierarchy != null)
            {
                hierarchyId = hierarchy.HierarchyID.ToString();
                LoadAccessFunction(permissions, hierarchy.HierarchyLevel, hierarchy.HierarchyCode);
                LoadMenu(hierarchy.HierarchyLevel, hierarchy.HierarchyCode);
                LoadAssignableHierarchy(hierarchy.HierarchyLevel);
                BindCurrentHierarcy(hierarchy, permissions);
                uxRadPrimaryUser.Enabled = uxRadSecondaryUser.Enabled
                    = uxHierarchyLevel.Enabled = false;
                //44648 - VW - Implement Change Log to review permission changes made to user roles
                string changelongInfo = GeneralFuncsLib.GetChangeLogLastestInfo(hierarchy.HierarchyID);
                if (!changelongInfo.IsNullOrEmpty())
                {
                    string queryString = Page.BuildSecureQueryString(string.Format("hierarchyId={0}", hierarchyId));
                    string url = string.Format("{0}?{1}", "ViewChangeLogModal.aspx", queryString);
                    uxViewLogTextStep1.Text = uxViewLogTextStep2.Text = uxViewLogTextStep3.Text = uxViewLogTextStep4.Text = uxViewLogTextStep5.Text = changelongInfo;
                    uxViewLogStep1.Text = uxViewLogStep2.Text = uxViewLogStep3.Text = uxViewLogStep4.Text = uxViewLogStep5.Text
                        = string.Format("<a href=\"#\" onclick=\" return parent.ShowPopupModalChild(1, '{0}', 'auto');\">{1}</a>", url, GetLocalResourceObject("ViewChangesLink").ToString());
                }
            }
            else
            {
                uxPlViewChangeLogStep1.Visible = uxPlViewChangeLogStep2.Visible = uxPlViewChangeLogStep3.Visible =
                    uxPlViewChangeLogStep4.Visible = uxPlViewChangeLogStep5.Visible = false;
            }
            //if (GeneralFuncsLib.IsDisableSecondaryUserRoleByEntities(hierarchyId))
            //    uxPnlSecondaryUser.Visible = false;
        }

    }
    protected void uxContinureStep2_Click(object sender, EventArgs e)
    {
        bool isUnique = false;

        // Check the unique role name
        if (_hierarchyId > 0)
        {
            // case: update Role
            isUnique = !WebServices.SecurityServices.CheckUniqueHierarchyForUpdate(
                SessionManager.CurrentUser.ASClient,
                uxRoleName.Text.Trim(),
                _hierarchyId);
        }
        else
        {
            // case: create role
            isUnique = !WebServices.SecurityServices.CheckUniqueHierarchyForCreate(
                SessionManager.CurrentUser.ASClient,
                uxRoleName.Text.Trim());
        }

        MoveSecondStep = isUnique;
        if (!MoveSecondStep)
        {
            Page.ClientScript.RegisterClientScriptBlock(
                this.GetType(),
                "CheckUnique",
                "alert('" + GetLocalResourceObject("ManageMSRoleCS_Text_RoleExist").ToString() + "');",
                true);
        }
        else if (DisableStepCreateMSRole)
        {
            MoveSecondStep = false;
            //Set default value for HierarchyLevel Dropdownlist
            uxHierarchyLevel.SelectedValue = HLEVEL_BANK;
            //Set default value for SecondaryUser Radibutton
            uxRadSecondaryUser.Checked = true;
            this.LoadDataStep4();
        }
    }

    protected void uxRoleListSelected_MovedData(object sender, EventArgs e)
    {
        DataTable destinationSource = uxRoleListSelected.DataSourceDestination;
        DataTable originalSource = uxRoleListSelected.DataSourceOrigination;

        if (destinationSource.Rows.Count >= 1)
        {
            // if we select row [ALL], clearing the DestinationSource & insert "ALL" row.
            if (destinationSource.Rows[0][ROLE_HIERARCHY_ID].ToString() == ROLE_ALL_ID
                || destinationSource.Rows[destinationSource.Rows.Count - 1][ROLE_HIERARCHY_ID].ToString() == ROLE_ALL_ID)
            {
                if (destinationSource.Rows[0][ROLE_HIERARCHY_ID].ToString() == ROLE_ALL_ID)
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
                    tempRow[ROLE_HIERARCHY_NAME] = row[ROLE_HIERARCHY_NAME].ToString();
                    tempRow[ROLE_HIERARCHY_ID] = row[ROLE_HIERARCHY_ID].ToString();
                    originalSource.Rows.Add(tempRow);
                }
                DataRow newRow = destinationSource.NewRow();
                destinationSource.Clear();
                newRow[ROLE_HIERARCHY_NAME] = ROLE_ALL_VALUE;
                newRow[ROLE_HIERARCHY_ID] = ROLE_ALL_ID;
                destinationSource.Rows.InsertAt(newRow, 0);
                uxRoleListSelected.DataSourceDestination = destinationSource;
                uxRoleListSelected.DataSourceOrigination = originalSource;
            }
            uxHddSelectedRole.Value = true.ToString();
        }
        else
        {
            uxHddSelectedRole.Value = false.ToString();
        }
    }

    protected void uxSave_Click(object sender, EventArgs e)
    {
        // Create new hierachy.
        Hierarchy hierarchy = new Hierarchy();

        if (!IsExistHierarchy(ref hierarchy))
        {
            return;
        }

        // Save new role into database.
        SaveMSRole(hierarchy);

        bool hasManUserPer = SearchForPermissionInTreeView(uxTreeMenuCS.Nodes,
            WebSiteConstants.SEC_PERMISSION_MAN_USER_MS);

        // Add or remove permission: Menu , Risk, User Profile,
        // Reset Pwd Permission and Access function.
        AddOrRemovePermissionForRole(hierarchy, hasManUserPer);

        // Save assign role.
        SaveAssignMSUserRole(hierarchy, hasManUserPer, IsEditMode);

        //
        doUpdateDefaultLandingPage(hierarchy.HierarchyID);

        // Only add when create new role
        if (!IsEditMode)
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
                IsUpdate = IsEditMode
            };
            WebServices.SecurityServices.UpdateAssignableHierarchy(assignableHierarchyModel);
        }

        // if edit role, must edit all child role.
        if (IsEditMode)
        {
            WebServices.SecurityServices.UpdateChildHierarchyPermission(hierarchy.HierarchyID);
        }

        this.Page.ClientScript.RegisterClientScriptBlock(
            GetType(),
            "closemode",
            "parent.HidePopupModal();parent.doRebindUserRole();",
            true);
    }

    protected void CheckPCIAnd1099K(object sender, EventArgs e)
    {
        //if (uxhdRolePCI.Value == "true" && GeneralFuncsLib.IsSynchPCIRole())
        //{
        //    uxPlcPCIRole.Visible = true;
        //    BindPCIRoleList();
        //}
        //else
        //{
        //    uxPlcPCIRole.Visible = false;
        //}

        if (uxhdRole1099.Value == "true")
        {
            uxPlc1099KRole.Visible = true;
            Bind1099KRoleList();
        }
        else
        {
            uxPlc1099KRole.Visible = false;
        }

        ((NonReportPage)this.Page).AjaxAddResponseScript("AdjustModalSize();");
    }

    //protected void CheckRolePCI(object sender, EventArgs e)
    //{
    //    if (uxhdRolePCI.Value == "true" && GeneralFuncsLib.IsSynchPCIRole())
    //    {
    //        uxPlcPCIRole.Visible = true;
    //        BindPCIRoleList();
    //    }
    //    else
    //    {
    //        uxPlcPCIRole.Visible = false;
    //    }
    //    ((NonReportPage)this.Page).AjaxAddResponseScript("AdjustModalSize();");
    //}

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

        ((NonReportPage)this.Page).AjaxAddResponseScript("AdjustModalSize();");
    }

    protected void LoadMenuAndAssignableHierarchy(object sender, EventArgs e)
    {
        this.LoadDataStep4();
    }

    #endregion Protected Methods

    #region Private Methods

    private void LoadDataStep4()
    {
        // If Hierarchy Level OR Hierarchy User Type change,
        // reload load menu & Assignable Hierarchy
        string selectedHierarchyLevel = HierarchyLevel.IsNullOrEmpty()
            ? string.Empty : HierarchyLevel;
        if (!uxOldHierarchyLevel.Value.ToString().Equals(selectedHierarchyLevel)
            || !uxOldUserType.Value.ToString().Equals(HierarchyUserType))
        {
            LoadAccessFunction(null, selectedHierarchyLevel, HierarchyUserType);
            LoadMenu(selectedHierarchyLevel, HierarchyUserType);
            // Reset 1099 & PCI
            Reset1099AndPCI();
            LoadAssignableHierarchy();
            PreserveHierarchyLevelAndUserType();
        }
        MoveFourthStep = true;
    }

    /// <summary>
    /// Step 2
    /// </summary>
    private void LoadHierarchyLevel()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.Add(new FilterParameter("@ASClient",
                                           SessionManager.CurrentUser.ASClient,
                                           DbType.Int32));
        parameters.Add(new FilterParameter("@LanguageID", SessionManager.CurrentLanguage, DbType.Int32));
        DataTable dtHierarchyLevel = WebServices.SecurityServices.GetReports(SPA_GET_HIERARCHY_LEVEL, parameters);

        DataRow newRow = dtHierarchyLevel.NewRow();
        newRow[HL_NAME] = HL_SELECT_HIERARCHY_VALUE;
        newRow[HL_ID] = HL_SELECT_HIERARCHY_ID;
        dtHierarchyLevel.Rows.InsertAt(newRow, 0);

        // Bind data
        uxHierarchyLevel.DataSource = dtHierarchyLevel;
        uxHierarchyLevel.DataValueField = HL_ID;
        uxHierarchyLevel.DataTextField = HL_NAME;
        uxHierarchyLevel.DataBind();
    }

    private void Reset1099AndPCI()
    {
        uxPlc1099KRole.Visible = false;
        //uxPlcPCIRole.Visible = false;
    }

    /// <summary>
    /// Step 5: This method will be called when we create role    
    /// </summary>
    private void LoadAssignableHierarchy()
    {
        LoadAssignableHierarchy(HierarchyLevel);
    }

    /// <summary>
    /// Step 5: This method will be called when we edit role
    /// We load Secondary hierarchy only regardless of User Type
    /// </summary>
    /// 
    private void LoadAssignableHierarchy(string hierarchyLevel)
    {
        DataTable orginalDataSource = WebServices.SecurityServices
            .GetMSAssignableHierarchy(
                SessionManager.CurrentClient,
                null,
                hierarchyLevel,
                HUSER_SECOND,
                0)
            .ToDataTable();
        // Add [ALL] Role to source
        DataRow allRoleRow = orginalDataSource.NewRow();
        allRoleRow[ROLE_HIERARCHY_NAME] = ROLE_ALL_VALUE;
        allRoleRow[ROLE_HIERARCHY_ID] = ROLE_ALL_ID;
        orginalDataSource.Rows.InsertAt(allRoleRow, 0);

        // Add [THIS NEW ROLE] Role to source
        if (EnableThisNewRoleOption && !IsEditMode)
        {
            DataRow thisRoleRow = orginalDataSource.NewRow();
            thisRoleRow[ROLE_HIERARCHY_NAME] = NEW_ROLE_VALUE;
            thisRoleRow[ROLE_HIERARCHY_ID] = NEW_ROLE_ID;
            orginalDataSource.Rows.Add(thisRoleRow);
        }

        DataTable desDataSource = WebServices.SecurityServices
               .GetAssignableHierarchy(_hierarchyId, WebSiteEnums.UserType.ALL.ToString())
               .ToDataTable();

        if (!IsEditMode)
        {
            desDataSource.Clear();
        }

        for (int i = 0; i < desDataSource.Rows.Count; i++)
        {
            for (int j = 0; j < orginalDataSource.Rows.Count; j++)
            {
                if (orginalDataSource.Rows[j][ROLE_HIERARCHY_ID].ToString()
                        .Equals(desDataSource.Rows[i][ROLE_HIERARCHY_ID].ToString()))
                {
                    orginalDataSource.Rows.RemoveAt(j);
                    break;
                }
            }
        }
        uxRoleListSelected.DataSourceOrigination = orginalDataSource;
        uxRoleListSelected.DataSourceDestination = desDataSource;
    }


    private void LoadAccessFunction(PermissionCollection permissions, string hierarchyLevel, string hierarchyCode)
    {
        PermissionCollection accessFuncCollection =
            WebServices.SecurityServices.GetMSPermissionsByType(
                SessionManager.CurrentUser.ASClient,
                WebSiteConstants.PERMISSION_TYPE_ACCESS_FUNC, hierarchyLevel, hierarchyCode);

        // Get only View Dashboard permission
        PermissionCollection newAccessFunc = new PermissionCollection();
        var dashboardPermision = (from Permission a in accessFuncCollection.Cast<Permission>()
                                  where a.PermissionCode == WebSiteConstants.SEC_PERMISSION_DASHBOARD_MS
                                  select a).ToList();
        if (dashboardPermision != null && dashboardPermision.Count > 0)
        {
            newAccessFunc.Add(dashboardPermision[0]);
        }

        // Get  CM permission 
        var cmPermisions = (from Permission a in accessFuncCollection.Cast<Permission>()
                            where a.Group == WebSiteConstants.PERMISSION_GROUP_CM
                            select a).ToList();
        if (cmPermisions != null && cmPermisions.Count > 0)
        {
            foreach (Permission p in cmPermisions)
            {
                newAccessFunc.Add(p);
            }
        }

        // Get  MS permission 
        var msPermisions = (from Permission a in accessFuncCollection.Cast<Permission>()
                            where a.Group == WebSiteConstants.PERMISSION_GROUP_MS
                            select a).ToList();
        if (msPermisions != null && msPermisions.Count > 0)
        {
            foreach (Permission p in msPermisions)
            {
                newAccessFunc.Add(p);
            }
        }

        PermissionCollection boardingPermissions = WebServices.SecurityServices.GetPermissionsByUserGroupType(
               SessionManager.CurrentUser.ASClient,
               SessionManager.CurrentUser.UserID,
               WebSiteConstants.AS_SYSTEM_MS,
               WebSiteConstants.PERMISSION_GROUP_BOARDING,
               WebSiteConstants.PERMISSION_TYPE_ACCESS_FUNC,
               SessionManager.CurrentLanguage);

        foreach (Permission p in boardingPermissions)
        {
            newAccessFunc.Add(p);
        }

        // Get  Risk permission 
        var riskPermisions = (from Permission a in accessFuncCollection.Cast<Permission>()
                            where a.Group == WebSiteConstants.PERMISSION_GROUP_RISK
                            select a).ToList();
        if (riskPermisions != null && riskPermisions.Count > 0)
        {
            foreach (Permission p in riskPermisions)
            {
                newAccessFunc.Add(p);
            }
        }

        // Get only View Dashboard permission
        var leadAccessFunc = (from Permission a in accessFuncCollection.Cast<Permission>()
                              where a.Group == WebSiteConstants.PERMISSION_GROUP_MS
                              && (a.GroupFuncName == WebSiteConstants.PERMISSION_GROUP_LEAD_DOCUMENT || a.GroupFuncName == WebSiteConstants.PERMISSION_GROUP_LEAD)
                              select a).ToList();
        if (leadAccessFunc != null && leadAccessFunc.Count > 0)
        {
            foreach (Permission item in leadAccessFunc)
            {
                newAccessFunc.Add(item);
            }
        }

        newAccessFunc = ExcludePermissionsByHierarchy(newAccessFunc) as PermissionCollection;

        //47441: Excluding Risk permissions. There is a specific business for Risk module
        // If the user who does not have "RiskMgmt" permission then excluding all permissions of Risk
        if (!SessionManager.CurrentUserPermissions.Contains(string.Format(",{0},", WebSiteConstants.SEC_PERMISSION_RSK_MGMT)))
        {
            List<Permission> per_Risk = (from Permission a in newAccessFunc.Cast<Permission>() where a.GroupFuncName.Equals(WebSiteConstants.PERMISSION_ACCESS_FUNC_RISK) select a).ToList();

            foreach (Permission permission in per_Risk)
            {
                newAccessFunc.Remove(permission);
            }
        }

        uxAccessFuncControl.DataSource = newAccessFunc;

        if (IsEditMode && permissions != null)
        {
            uxAccessFuncControl.SetSelectedByPermissions(permissions);
        }
    }

    private void LoadMenu(string hierarchyLevel, string hierarchyCode)
    {
        SecMenuItem defaultMenu = new SecMenuItem();
        uxTreeMenuCS.DataTextField = "Title";
        uxTreeMenuCS.DataValueField = "Permissions";
        uxTreeMenuCS.DataFieldID = "SiteMapId";
        uxTreeMenuCS.DataFieldParentID = "Parent";

        // Load menu items
        SecMenuItemCollection menuItems =
           WebServices.SecurityServices.GetMSMenuItems(
           SessionManager.CurrentUser.ASClient, hierarchyLevel, hierarchyCode, SessionManager.CurrentLanguage);
        menuItems = ExcludePermissionsByHierarchy(menuItems) as SecMenuItemCollection;

        var menus = menuItems.IsNotNullData() ? menuItems.Cast<SecMenuItem>() : new List<SecMenuItem>();
        for (int i = 0; i < menuItems.Count; i++)
        {
            var permissionCode = menuItems[i].Permissions;

            //Remove menu is config on SitemapSetting.xml
            SecMenuItem menu = menus.Where(m => m.SiteMapId == menuItems[i].Parent).SingleOrDefault();
            if (menu.IsNotNullData() && SitemapHandler.IsRemoveFromSitemaps(menu.IsNullData() ? "" : menu.Title, menuItems[i].Title))
            {
                menuItems.Remove(menuItems[i]);
                i--;
                continue;
            }
            // 43534 - remove document type
            if (WebSiteConstants.SEC_PERMISSION_MANAGE_DOCUMENT_TYPES.Trim().Equals(permissionCode.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                menuItems.Remove(menuItems[i]);
                i--;
                continue;
            }

            if (WebSiteConstants.SEC_PERMISSION_DASHBOARD.Equals(permissionCode, StringComparison.OrdinalIgnoreCase) ||
                WebSiteConstants.SEC_PERMISSION_DASHBOARD_MS.Equals(permissionCode, StringComparison.OrdinalIgnoreCase))
            {
                defaultMenu = menuItems[i];
            }
        }

        // Bind data
        uxTreeMenuCS.DataSource = menuItems;
        uxTreeMenuCS.DataBind();
        uxTreeMenuCS.ExpandAllNodes();

        if (!string.IsNullOrEmpty(defaultMenu.Permissions) && _hierarchyId == 0)
        {
            RadTreeNode node = uxTreeMenuCS.FindNodeByText(defaultMenu.Title);
            if (node != null)
                node.Checked = true;
        }

        #region Removes Case mgmt menu for Chain, MIC Chain, Merchant hierarchy level

        if (SessionManager.CurrentUser.ASClient == 22 && menuItems != null && menuItems.Count > 0 &&
            ("MPSCHAIN".Equals(hierarchyLevel, StringComparison.OrdinalIgnoreCase) || "CHAIN".Equals(hierarchyLevel, StringComparison.OrdinalIgnoreCase) || "MERCHANT".Equals(hierarchyLevel, StringComparison.OrdinalIgnoreCase)))
        {
            const string caseMgmtMenuTitle = "Case Mgmt";
            foreach (RadTreeNode node in this.uxTreeMenuCS.Nodes)
            {
                if (caseMgmtMenuTitle.Equals(node.Text, StringComparison.OrdinalIgnoreCase))
                {
                    node.Visible = false;
                    break;
                }
            }
        }

        #endregion

        // 35129
        initDefaultLandingPage(hierarchyLevel, hierarchyCode);


    }

    private void BindCurrentHierarcy(Hierarchy hierarchy, PermissionCollection permissions)
    {
        if (IsEditMode && hierarchy != null)
        {
            // Bind basic information
            uxRoleDes.Text = VeraCodeSolution.DoVeraCode(hierarchy.HierarchyDescription);
            uxRoleName.Text = VeraCodeSolution.DoVeraCode(hierarchy.HierarchyName);

            // Bind the level of Hierarchy
            SetHierarchyLevel(hierarchy.HierarchyLevel);

            // Bind Hierarchy user type
            bool isPrimaryUser = !hierarchy.HierarchyCode.IsNullOrEmpty()
                && (hierarchy.HierarchyCode == MUSER_PRIMARY
                    || hierarchy.HierarchyCode == HUSER_PRIMARY);
            uxRadSecondaryUser.Checked = !(uxRadPrimaryUser.Checked = isPrimaryUser);

            // Keep the old value of HierarchyLevel & HierarchyUserType
            PreserveHierarchyLevelAndUserType();

            // Bind Permission
            DoCheckPermissionForMenu(uxTreeMenuCS.Nodes, permissions);

            // Bind 1099K
            Bind1099KForCurrentHierarchy(hierarchy.HierarchyID);
            // Bind PCI
            //BindPCIForCurrentHierarchy(hierarchy.HierarchyID);
        }
    }

    private void SetHierarchyLevel(string hierarchyLevel)
    {
        foreach (Telerik.Web.UI.RadComboBoxItem item in uxHierarchyLevel.Items)
        {
            if ((item.Value.IsNullOrEmpty() & hierarchyLevel.IsNullOrEmpty())
                || (item.Value == hierarchyLevel))
            {
                item.Selected = true;
                break;
            }
        }
    }

    private void PreserveHierarchyLevelAndUserType()
    {
        uxOldHierarchyLevel.Value = HierarchyLevel;
        uxOldUserType.Value = HierarchyUserType;
    }

    private void DoCheckPermissionForMenu(Telerik.Web.UI.RadTreeNodeCollection nodes,
        PermissionCollection permissions)
    {
        if (nodes == null || nodes.Count == 0) return;
        for (int i = 0; i < nodes.Count; i++)
        {
            DoCheckPermissionForMenu(nodes[i].Nodes, permissions);
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

    private bool CheckPermissionIDInList(string strPermission, PermissionCollection permissions)
    {
        string[] listPermissions = (from Permission p in permissions
                                    select p.PermissionCode).ToArray();
        return listPermissions.Contains(strPermission);
    }

    private void Bind1099KRoleList()
    {
        ux1099KRole.Items.Clear();

        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(true);
        if (IsMerchantLevel)
            parameters.Add(new FilterParameter("@UserType", UserRoleType.MS_ROLE, DbType.AnsiString));
        else
            parameters.Add(new FilterParameter("@UserType", UserRoleType.CS_ROLE, DbType.AnsiString));
        DataTable roleList = WebServices.SecurityServices.GetReports(SPA_GET_1099K_LIST, parameters);

        ux1099KRole.DataSource = roleList;
        ux1099KRole.DataTextField = "HierarchyName";
        ux1099KRole.DataValueField = "HierarchyID";
        ux1099KRole.DataBind();
    }

    //private void BindPCIRoleList()
    //{
    //    uxPCIRole.Items.Clear();

    //    FilterParameterCollection parameters = new FilterParameterCollection();
    //    parameters.AddLoggedInUserReportingParams(true);
    //    DataTable roleList = WebServices.CsReportServices.GetReports(
    //        SPA_GET_PCI_ROLE_LIST, parameters);

    //    uxPCIRole.DataSource = roleList;
    //    uxPCIRole.DataTextField = "HierarchyName";
    //    uxPCIRole.DataValueField = "HierarchyID";
    //    uxPCIRole.DataBind();
    //}

    private bool IsExistHierarchy(ref Hierarchy hierarchy)
    {
        PermissionCollection permissions = null;
        return IsExistHierarchy(ref hierarchy, ref permissions);
    }

    private bool IsExistHierarchy(ref Hierarchy hierarchy, ref PermissionCollection permissions)
    {
        bool isExist = true;
        if (IsEditMode)
        {
            hierarchy = WebServices.SecurityServices.GetHierarchyById(_hierarchyId);
            if (hierarchy == null)
            {
                Response.Redirect(PAGE_NOT_FOUND);
                isExist = false;
            }
            permissions = WebServices.SecurityServices.GetPermissionsInHierarchy(hierarchy.HierarchyID);
        }
        return isExist;
    }

    private void DoUpdateAssignableHierarchy(int hierarchyId, DataTable listRoleSelected, bool isEditMote)
    {
        bool existAssignHierarchy = true;
        int assignId = 0;

        // get current hierarchy
        HierarchyCollection assignableHierarchyList =
            WebServices.SecurityServices.GetAssignableHierarchy(
                hierarchyId, WebSiteEnums.UserType.ALL.ToString());

        // Insert new assignable hierarchy if the hierarchy doesn't have it
        // in the current list (assignableHierarchyList)
        for (int i = 0; i < listRoleSelected.Rows.Count; i++)
        {
            existAssignHierarchy = false;
            foreach (Hierarchy assignableHierarchy in assignableHierarchyList)
            {
                if (listRoleSelected.Rows[i][ROLE_HIERARCHY_ID].ToString().Equals(
                    assignableHierarchy.HierarchyID.ToString()))
                {
                    existAssignHierarchy = true;
                    break;
                }
            }

            // no record is matched => new record
            if (!existAssignHierarchy)
            {
                assignId = int.Parse(listRoleSelected.Rows[i][ROLE_HIERARCHY_ID].ToString());

                if (EnableThisNewRoleOption && listRoleSelected.Rows[i][ROLE_HIERARCHY_ID].ToString() == NEW_ROLE_ID)  //[This new role]
                {
                    assignId = hierarchyId;
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
                    IsUpdate = isEditMote
                };
                WebServices.SecurityServices.UpdateAssignableHierarchy(assignableHierarchyModel);
            }
        }

        // Remove assignable hierarchy if list role selected doesn't contain it
        // and the assignable hierarchy list does.
        for (int i = 0; i < assignableHierarchyList.Count; i++)
        {
            existAssignHierarchy = false;
            for (int k = 0; k < listRoleSelected.Rows.Count; k++)
            {
                if (listRoleSelected.Rows[k][ROLE_HIERARCHY_ID].ToString().Equals(
                    assignableHierarchyList[i].HierarchyID.ToString()))
                {
                    existAssignHierarchy = true;
                    break;
                }
            }

            //no record is matched => need to be removed
            if (!existAssignHierarchy)
            {
                assignId = assignableHierarchyList[i].HierarchyID;

                //41907 - VW - Aperia - Enrich User Audit Report (Billable)
                var assignableHierarchyModel = new AssignableHierarchyModel()
                {
                    HierarchyId = hierarchyId,
                    AssignHierarchyId = assignId,
                    IsRemove = true,
                    AsClientId = SessionManager.CurrentClient,
                    SiteId = SessionManager.CurrentUser.SiteID,
                    UserId = SessionManager.CurrentUser.UserID,
                    ChangedByRecId = SessionManager.CurrentUser.RecId,
                    IsUpdate = isEditMote
                };
                WebServices.SecurityServices.UpdateAssignableHierarchy(assignableHierarchyModel);
            }
        }
    }

    private bool SearchForPermissionInTreeView(Telerik.Web.UI.RadTreeNodeCollection nodes,
        string permissionCode)
    {
        if (nodes == null || nodes.Count == 0)
        {
            return false;
        }
        for (int i = 0; i < nodes.Count; i++)
        {
            if (SearchForPermissionInTreeView(nodes[i].Nodes, permissionCode))
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

    private void SaveAssignMSUserRole(Hierarchy hierarchy, bool hasManUserPer, bool isEditMode)
    {
        if (!hasManUserPer)
        {
            uxRoleListSelected.DataSourceDestination.Clear();
        }

        // Add All role
        if (uxRoleListSelected.DataSourceDestination.Rows.Count == 1
            && uxRoleListSelected.DataSourceDestination.Rows[0][ROLE_HIERARCHY_ID].ToString() == ROLE_ALL_ID)
        {
            for (int i = 0; i < uxRoleListSelected.DataSourceOrigination.Rows.Count; i++)
            {
                bool isRemove = EnableThisNewRoleOption ? (uxRoleListSelected.DataSourceOrigination.Rows[i][ROLE_HIERARCHY_ID].ToString().Equals(NEW_ROLE_ID)
                    || uxRoleListSelected.DataSourceOrigination.Rows[i][ROLE_HIERARCHY_ID].ToString().Equals(ROLE_ALL_ID))
                    : uxRoleListSelected.DataSourceOrigination.Rows[i][ROLE_HIERARCHY_ID].ToString().Equals(ROLE_ALL_ID);
                if (isRemove)
                {
                    uxRoleListSelected.DataSourceOrigination.Rows.RemoveAt(i);
                }
            }
            DoUpdateAssignableHierarchy(hierarchy.HierarchyID, uxRoleListSelected.DataSourceOrigination, isEditMode);
        }
        else
        {
            DoUpdateAssignableHierarchy(hierarchy.HierarchyID, uxRoleListSelected.DataSourceDestination, isEditMode);
        }
    }

    private void SaveAccessFuncForHierarchy(Hierarchy hierarchy)
    {
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
                    IsEditMode);
            }
            else
            {
                WebServices.SecurityServices.RemovePermissionFromHierarchy(
                    permissionCode,
                    hierarchy.HierarchyID,
                    SessionManager.CurrentUser.ASClient,
                    SessionManager.CurrentUser.RecId,
                    IsEditMode);
            }
        }


        // Save 1099K
        if (GeneralFuncsLib.IsCompliassureRoleSynch())
        {
            int hierarchyID_1099K = 0;
            if (uxPlc1099KRole.Visible)
            {
                hierarchyID_1099K = Convert.ToInt32(ux1099KRole.SelectedValue);
            }
            InsertUpdateHierarchyIn1099K(hierarchy.HierarchyID, hierarchyID_1099K);
        }

        // Save PCI Role
        //if (GeneralFuncsLib.IsSynchPCIRole())
        //{
        //    int hierarchyID_PCI = 0;
        //    if (uxPlcPCIRole.Visible)
        //    {
        //        hierarchyID_PCI = Convert.ToInt32(uxPCIRole.SelectedValue);
        //    }
        //    InsertUpdateHierarchyInPCI(hierarchy.HierarchyID, hierarchyID_PCI);
        //}
    }


    private void InsertUpdateHierarchyIn1099K(int hierarchyID, int hierarchyIn1099K)
    {
        //41907 - VW - Aperia - Enrich User Audit Report (Billable)
        FilterParameterCollection parameters = new FilterParameterCollection();
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

    private void DoAddPermissionForMenu(Telerik.Web.UI.RadTreeNodeCollection nodes,
        int hierarchyId, bool isLogged)
    {
        _checkAllMenu = true;
        if (nodes == null || nodes.Count == 0)
        {
            return;
        }
        for (int i = 0; i < nodes.Count; i++)
        {
            DoAddPermissionForMenu(nodes[i].Nodes, hierarchyId, isLogged);
            if (nodes[i].Value.IndexOf(",") == -1)
            {
                if (nodes[i].Checked)
                {
                    WebServices.SecurityServices.AddPermissionIntoHierarchy(
                        nodes[i].Value,
                        hierarchyId,
                        SessionManager.CurrentUser.RecId,
                        isLogged);
                }
                else
                {
                    WebServices.SecurityServices.RemovePermissionFromHierarchy(
                        nodes[i].Value,
                        hierarchyId,
                        SessionManager.CurrentUser.ASClient,
                        SessionManager.CurrentUser.RecId,
                        isLogged);
                    _checkAllMenu = false;
                }
            }
        }
    }

    private bool HasRiskPermission()
    {
        bool hasRiskPermission = false;
        foreach (string per in RISK_PERS)
        {
            if (SearchForPermissionInTreeView(uxTreeMenuCS.Nodes, per))
            {
                hasRiskPermission = true;
                break;
            }
        }
        return hasRiskPermission;
    }

    private void AddOrRemovePermissionForRole(Hierarchy hierarchy, bool hasManUserPer)
    {
        _notificationPermissions = GetNotificationPermissions();
        _hasNotification = false;
        // Add/Remove Permission for menu and check if all menus are checked or not.
        DoAddPermissionForMenu(uxTreeMenuCS.Nodes, hierarchy.HierarchyID, IsEditMode);

        if (_hasNotification)
        {
            WebServices.SecurityServices.AddPermissionIntoHierarchy(WebSiteConstants.SEC_PERMISSION_NOTIFICATION_SETTING_MS, hierarchy.HierarchyID, SessionManager.CurrentUser.RecId, IsEditMode);
        }
        else
        {
            WebServices.SecurityServices.RemovePermissionFromHierarchy(WebSiteConstants.SEC_PERMISSION_NOTIFICATION_SETTING_MS, hierarchy.HierarchyID, SessionManager.CurrentUser.ASClient, SessionManager.CurrentUser.RecId, IsEditMode);
        }

        // Add/Remove Risk Permission
        AddOrRemoveRiskPermission(hierarchy);

        // Add/ Remove User Profile permission
        AddOrRemoveUserProfilePermission(hierarchy);

        // Add permission Reset User PWD
        AddOrRemoveResetPwdPermission(hierarchy, hasManUserPer);

        // Add access function for hierarchy
        SaveAccessFuncForHierarchy(hierarchy);

        #region Grants Notification Setting for all roles

        var menuPermissions = WebServices.SecurityServices.GetPermissionsByUserGroupType(SessionManager.CurrentUser.ASClient, SessionManager.CurrentUser.UserID, WebSiteConstants.AS_SYSTEM_MS,
                                                            WebSiteConstants.PERMISSION_GROUP_MS, WebSiteConstants.PERMISSION_TYPE_MENU, SessionManager.CurrentLanguage);
        if (menuPermissions != null && menuPermissions.Count > 0)
        {
            var hasPermission = menuPermissions.Cast<Permission>().Any(p => WebSiteConstants.SEC_PERMISSION_NOTIFICATION_SETTING_MS.Equals(p.PermissionCode, StringComparison.OrdinalIgnoreCase));
            if (hasPermission)
            {
                // If the notification setting feature is enabled, then grant notification setting for all roles
                WebServices.SecurityServices.AddPermissionIntoHierarchy(WebSiteConstants.SEC_PERMISSION_NOTIFICATION_SETTING_MS,
                                                                    hierarchy.HierarchyID, SessionManager.CurrentUser.RecId, IsEditMode);
            }
        }

        #endregion
    }

    private void AddOrRemoveResetPwdPermission(Hierarchy hierarchy, bool hasManUserPer)
    {
        if (hasManUserPer)
        {
            WebServices.SecurityServices.AddPermissionIntoHierarchy(
                WebSiteConstants.SEC_PERMISSION_RESET_PWD_MS,
                hierarchy.HierarchyID,
                SessionManager.CurrentUser.RecId,
                IsEditMode);
        }
        else
        {
            WebServices.SecurityServices.RemovePermissionFromHierarchy(
                WebSiteConstants.SEC_PERMISSION_RESET_PWD_MS,
                hierarchy.HierarchyID,
                SessionManager.CurrentUser.ASClient,
                SessionManager.CurrentUser.RecId,
                IsEditMode);
        }
    }

    private void AddOrRemoveUserProfilePermission(Hierarchy hierarchy)
    {
        WebServices.SecurityServices.AddPermissionIntoHierarchy(
            WebSiteConstants.SEC_PERMISSION_USER_PROF_MS,
            hierarchy.HierarchyID,
            SessionManager.CurrentUser.RecId,
            IsEditMode);
    }

    private void AddOrRemoveRiskPermission(Hierarchy hierarchy)
    {
        // Check if the role has any risk permission or not.
        bool hasRiskRole = HasRiskPermission();

        // Add or Remove RiskMgmt Permission
        if (hasRiskRole)
        {
            WebServices.SecurityServices.AddPermissionIntoHierarchy(
                WebSiteConstants.SEC_PERMISSION_RSK_MGMT_MS,
                hierarchy.HierarchyID,
                SessionManager.CurrentUser.RecId,
                IsEditMode);
        }
        else
        {
            WebServices.SecurityServices.RemovePermissionFromHierarchy(
                WebSiteConstants.SEC_PERMISSION_RSK_MGMT_MS,
                hierarchy.HierarchyID,
                SessionManager.CurrentUser.ASClient,
                SessionManager.CurrentUser.RecId,
                IsEditMode);
        }
    }

    private void SaveMSRole(Hierarchy hierarchy)
    {
        // Basic information
        hierarchy.ClientId = SessionManager.CurrentUser.ASClient;
        hierarchy.HierarchyDescription = uxRoleDes.Text;
        hierarchy.HierarchyName = uxRoleName.Text.Trim();

        // Hierarchy Level
        hierarchy.HierarchyLevel = uxHierarchyLevel.SelectedValue.IsNullOrEmpty()
            ? null : uxHierarchyLevel.SelectedValue;

        // User Type
        hierarchy.HierarchyCode = HierarchyUserType;

        // Save/Update information of Role
        if (IsEditMode)
        {
            WebServices.SecurityServices.UpdateHierarchy(hierarchy, SessionManager.CurrentUser.RecId);
        }
        else
        {
            hierarchy.CreatedBy = SessionManager.CurrentUser.RecId;
            hierarchy.ActvStatus = "1";
            // TODO:
            hierarchy.HierarchyParent = SessionManager.CurrentHierarchyId;
            hierarchy.SystemId = WebSiteConstants.AS_SYSTEM_MS;
            hierarchy.HierarchyID = WebServices.SecurityServices.CreateHierarchy(hierarchy);
        }
    }

    private int GetHierarchyIn1099KByHierarchyID(int hierarchyId)
    {
        int hierarchy1099KId = -1;
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.Add(new FilterParameter("@HierarchyId", hierarchyId, DbType.Int32));

        DataTable hierarchyIn1099KList = WebServices.SecurityServices.GetReports(SPA_GET_HIERARCHY_IN_1099K_BY_ID, parameters);
        if (hierarchyIn1099KList != null && hierarchyIn1099KList.Rows.Count > 0)
        {
            hierarchy1099KId = Convert.ToInt32(hierarchyIn1099KList.Rows[0][0]);
        }
        return hierarchy1099KId;
    }

    private void DoCheckPermissionSiteJump1099(Telerik.Web.UI.RadTreeNodeCollection nodes)
    {
        if (nodes == null || nodes.Count == 0) return;
        for (int i = 0; i < nodes.Count; i++)
        {
            DoCheckPermissionSiteJump1099(nodes[i].Nodes);
            if (nodes[i].Value == WebSiteConstants.SEC_PERMISSION_SITE_JUMP_1099K
                && nodes[i].Checked)
            {
                uxPlc1099KRole.Visible = true;
                break;
            }
        }
    }

    private int GetHierarchyInPCIByHierarchyID(int hierarchyId)
    {
        int hierarchyPCIId = -1;
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.Add(new FilterParameter("@HierarchyId", hierarchyId, DbType.Int32));
        parameters.Add(new FilterParameter("@ASClientID", SessionManager.CurrentClient, DbType.Int32));
        DataTable hierarchyInPCIList = WebServices.SecurityServices.GetReports(SPA_GET_HIERARCHY_IN_PCI_BY_ID, parameters);
        if (hierarchyInPCIList != null && hierarchyInPCIList.Rows.Count > 0)
        {
            hierarchyPCIId = Convert.ToInt32(hierarchyInPCIList.Rows[0][0]);
        }
        return hierarchyPCIId;
    }

    //private void DoCheckPermissionSiteJumpPCI(Telerik.Web.UI.RadTreeNodeCollection nodes)
    //{
    //    if (nodes == null || nodes.Count == 0) return;
    //    for (int i = 0; i < nodes.Count; i++)
    //    {
    //        DoCheckPermissionSiteJumpPCI(nodes[i].Nodes);
    //        if ((nodes[i].Value == WebSiteConstants.SEC_PERMISSION_HIERARCHY_SITE_ACCESS_PCI || nodes[i].Value == WebSiteConstants.SEC_PERMISSION_MERCHANT_SITE_ACCESS_PCI)
    //            && nodes[i].Checked)
    //        {
    //            uxPlcPCIRole.Visible = true;
    //            break;
    //        }
    //    }
    //}

    //private void BindPCIForCurrentHierarchy(int hierarchyID)
    //{
    //    if (GeneralFuncsLib.IsSynchPCIRole())
    //    {
    //        int rolePCI = GetHierarchyInPCIByHierarchyID(hierarchyID);
    //        DoCheckPermissionSiteJumpPCI(uxTreeMenuCS.Nodes);
    //        if (uxPlcPCIRole.Visible)
    //        {
    //            BindPCIRoleList();
    //            uxPCIRole.SelectedValue = rolePCI.ToString();
    //        }
    //    }
    //    else
    //    {
    //        uxPlcPCIRole.Visible = false;
    //    }
    //}

    private void Bind1099KForCurrentHierarchy(int hierarchyID)
    {
        if (GeneralFuncsLib.IsCompliassureRoleSynch())
        {
            int role1099K = GetHierarchyIn1099KByHierarchyID(hierarchyID);
            DoCheckPermissionSiteJump1099(uxTreeMenuCS.Nodes);
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
    }

    private void initDefaultLandingPage(string hierarchyLevel, string hierarchyCode)
    {
        SecMenuItemCollection menuItems =
           WebServices.SecurityServices.GetMSMenuItems(
           SessionManager.CurrentUser.ASClient, hierarchyLevel, hierarchyCode, SessionManager.CurrentLanguage);
        menuItems = ExcludePermissionsByHierarchy(menuItems) as SecMenuItemCollection;

        SecMenuItem defaultMenu = new SecMenuItem();

        string[] pes = WebSiteConstants.SEC_PERMISSION_CaseMgt.Split(',');
        var menus = menuItems.IsNotNullData() ? menuItems.Cast<SecMenuItem>() : new List<SecMenuItem>();
        for (int i = menuItems.Count - 1; i >= 0; i--)
        {
            var permissionCode = menuItems[i].Permissions.Trim();
            if (WebSiteConstants.SEC_PERMISSION_MAN_ROLE.Equals(permissionCode, StringComparison.OrdinalIgnoreCase)
                // || WebSiteConstants.SEC_PERMISSION_AUTO_QUEUE.Equals(permissionCode, StringComparison.OrdinalIgnoreCase)
                || WebSiteConstants.SEC_PERMISSION_HIERARCHY_SITE_ACCESS_PCI.Equals(permissionCode, StringComparison.OrdinalIgnoreCase)
                || WebSiteConstants.SEC_PERMISSION_MERCHANT_SITE_ACCESS_PCI.Equals(permissionCode, StringComparison.OrdinalIgnoreCase)
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
            if (menu.IsNotNullData() && SitemapHandler.IsRemoveFromSitemaps(menu.IsNullData() ? "" : menu.Title, menuItems[i].Title))
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

        if (_hierarchyId > 0)
        {
            DataTable td = GeneralFuncsLib.GetDefaultLandingPage(_hierarchyId);
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

    private CollectionBase ExcludePermissionsByHierarchy(CollectionBase collection)
    {
        string infos = GeneralFuncsLib.GetDataOfExtendedSetting(EXCLUDE_PERMISSIONS_BY_HIERARCHY);
        if (!infos.IsNullOrEmpty())
        {
            string[] arr = infos.Split(';');
            foreach (string val in arr)
            {
                string[] subArr = val.Split(':');
                if (subArr.Count() > 1)
                {
                    if (SessionManager.CurrentHierarchyId.ToString().Equals(subArr[0], StringComparison.OrdinalIgnoreCase))
                    {
                        string excludePermissions = string.Format(",{0},", subArr[1]);
                        for (int i = collection.Count - 1; i >= 0; i--)
                        {
                            string permissionCode = string.Empty;
                            if (collection is SecMenuItemCollection)
                            {
                                permissionCode = (collection as SecMenuItemCollection)[i].Permissions.Trim();
                            }
                            else if (collection is PermissionCollection)
                            {
                                permissionCode = (collection as PermissionCollection)[i].PermissionCode.Trim();
                            }

                            permissionCode = string.Format(",{0},", permissionCode);
                            if (excludePermissions.Contains(permissionCode))
                                collection.RemoveAt(i);
                        }
                        return collection;
                    }
                }
            }
        }
        return collection;
    }

    private bool CheckNotificationSetting(string permissionCode)
    {
        foreach (string per in _notificationPermissions)
        {
            if (per.Equals(permissionCode, StringComparison.OrdinalIgnoreCase))
                return true;
        }
        return false;
    }

    private IList<string> GetNotificationPermissions()
    {
        return GeneralFuncsLib.GetNotificationPermissionsOfSourceApp();
    }

    #endregion Private Methods

    #endregion Methods
}