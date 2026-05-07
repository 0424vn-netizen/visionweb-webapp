using AS.Common;
using AS.Common.DBManager;
using AS.Security.WS.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Collections;

public partial class ViewRoleInfo : ReportPage
{
    private const string EXCLUDE_PERMISSIONS_BY_HIERARCHY = "EXCLUDE_PERMISSIONS_BY_HIERARCHY";
    public int HierarchyId { get; set; }
    public int? SystemId { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected)
            return;
        PageType = SecurePageType.Modal;
        if (SessionManager.CurrentSystem == 1)
            SessionManager.UserRoleType = WebSiteEnums.UserType.CS.ToString();
        else
            SessionManager.UserRoleType = WebSiteEnums.UserType.MS.ToString();
        if (IsSecureQueryString)
        {
            HierarchyId = SecureQueryString["id"].ToInt();
            SystemId = SecureQueryString["sysId"].ToInt();
        }

        Hierarchy hierarchy = WebServices.SecurityServices.GetHierarchyById(HierarchyId);
        PermissionCollection permissions = WebServices.SecurityServices
                   .GetPermissionsInHierarchy(hierarchy.HierarchyID);

        if (hierarchy == null)
        {
            Response.Redirect(WebSiteConstants.PAGE_NOT_FOUND_NAME);
            return;
        }
        BindTreeMenu();

        //Access Function
        if (SystemId == 1)
            BindCSAccessFunction(permissions);
        else
            BindMSAccessFunction(permissions, hierarchy.HierarchyLevel, hierarchy.HierarchyCode);
        uxPlAccessFunction.Visible = !(uxAccessFuncList.Items.Count == 0);
        //End Access Function
        doCheckPermissionForMenu(uxTreeMenuCS.Nodes, permissions);
        //Bind 1099K
        if (GeneralFuncsLib.IsCompliassureRoleSynch())
        {
            int role1099K = GetHierarchyIn1099KByHierarchyID(HierarchyId);
            doCheckPermissionSiteJump1099(uxTreeMenuCS.Nodes);
            if (uxPlc1099KRole.Visible)
            {
                Bind1099KRoleList();
            }
        }
        else
        {
            uxPlc1099KRole.Visible = false;
        }
        // PCI Admin Role
        if (GeneralFuncsLib.IsSynchPCIRole())
        {
            int rolePCI = GetHierarchyInPCIByHierarchyID(HierarchyId);
            doCheckPermissionSiteJumpPCI(uxTreeMenuCS.Nodes);
            if (uxPlcPCIRole.Visible)
            {
                BindPCIRoleList();
            }
        }
        else
        {
            uxPlcPCIRole.Visible = false;
        }

        if (uxPlc1099KRole.Visible && uxPlcPCIRole.Visible)
        {
            uxheight1.Attributes.Add("style", "display:block;");
            //uxPlcPCIRole_table.Attributes.Add("class", "max-width");
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

    string currentGroup = string.Empty;
    protected void uxAccessFuncList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Permission p = e.Item.DataItem as Permission;
        HtmlGenericControl divParent = e.Item.FindControl("uxPnlGroupLabel") as HtmlGenericControl;
        HtmlGenericControl divChild = e.Item.FindControl("uxPnlGroupChecbox") as HtmlGenericControl;

        Literal label = e.Item.FindControl("uxGroupLabel") as Literal;
        AS.Controls.Global.CheckBox chkbox = e.Item.FindControl("uxPermission") as AS.Controls.Global.CheckBox;
        if (label != null && chkbox != null && divParent != null)
        {
            if (p.GroupFuncName.Equals(currentGroup) || string.IsNullOrEmpty(p.GroupFuncName))
            {
                divParent.Visible = false;
            }
            else
            {
                currentGroup = p.GroupFuncName;
                string text = GetLocalResourceObject(string.Format("AccessFuntionControlGroupFuncLabel_{0}", p.GroupFuncName)) != null ? GetLocalResourceObject(string.Format("AccessFuntionControlGroupFuncLabel_{0}", p.GroupFuncName)).ToString() : string.Empty;
                label.Text = VeraCodeSolution.DoVeraCode(text);
                divParent.Attributes["data-target"] = p.GroupFuncName;
                divParent.Visible = true;

            }
            divChild.Attributes["data-target"] = p.GroupFuncName;
            chkbox.Text = VeraCodeSolution.DoVeraCode(p.Description);
            chkbox.Value = VeraCodeSolution.DoVeraCode(p.PermissionCode);
        }
    }

    #region Private Methods

    private void BindCSAccessFunction(PermissionCollection rolePermissions)
    {
        PermissionCollection permissions = new PermissionCollection();

        List<Permission> rolePermissionTemp = rolePermissions.IsNotNullData() ?
            rolePermissions.Cast<Permission>().ToList()
            : new List<Permission>();

        PermissionCollection currentUserPermissions = WebServices.SecurityServices.GetPermissionsByUserGroupType(
                SessionManager.CurrentUser.ASClient,
                SessionManager.CurrentUser.UserID,
                SessionManager.CurrentSystem,
                SessionManager.UserRoleType.ToLower(),
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
            currentUserPermissions.Add(p);
        }

        foreach (Permission p in currentUserPermissions)
        {
            if (rolePermissionTemp.Any(m => m.PermissionCode == p.PermissionCode))
                permissions.Add(p);
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

        uxAccessFuncList.DataSource = permissions;
        uxAccessFuncList.DataBind();
        SetSelectedByPermissions(permissions);

    }

    private void BindMSAccessFunction(PermissionCollection rolePermissions, string hierarchyLevel, string hierarchyCode)
    {
        PermissionCollection newAccessFunc = new PermissionCollection();

        List<Permission> rolePermissionTemp = rolePermissions.IsNotNullData() ?
            rolePermissions.Cast<Permission>().ToList()
            : new List<Permission>();

        PermissionCollection msAccessFuncCollection =
            WebServices.SecurityServices.GetMSPermissionsByType(
                SessionManager.CurrentUser.ASClient,
                WebSiteConstants.PERMISSION_TYPE_ACCESS_FUNC, hierarchyLevel, hierarchyCode);

        foreach (Permission p in msAccessFuncCollection)
        {
            if (rolePermissionTemp.Any(m => m.PermissionCode == p.PermissionCode))
                newAccessFunc.Add(p);
        }

        newAccessFunc = ExcludePermissionsByHierarchy(newAccessFunc) as PermissionCollection;
        uxAccessFuncList.DataSource = newAccessFunc;
        uxAccessFuncList.DataBind();
        SetSelectedByPermissions(newAccessFunc);
    }

    private void BindTreeMenu()
    {
        SecMenuItem defaultMenu = new SecMenuItem();
        uxTreeMenuCS.DataTextField = "Title";
        uxTreeMenuCS.DataValueField = "Permissions";
        uxTreeMenuCS.DataFieldID = "SiteMapId";
        uxTreeMenuCS.DataFieldParentID = "Parent";

        Hierarchy hierarchy = WebServices.SecurityServices.GetHierarchyById(HierarchyId);
        SecMenuItemCollection menuItems = new SecMenuItemCollection();
        if (SystemId == 1)
        {
            menuItems = WebServices.SecurityServices.GetMenuItemsByUser(
                SessionManager.CurrentUser.ASClient,
                SessionManager.CurrentUser.UserID,
                HierarchyId,
                SessionManager.CurrentLanguage, true);
        }
        else
        {
            menuItems = WebServices.SecurityServices.GetMSMenuItems(
                SessionManager.CurrentUser.ASClient, hierarchy.HierarchyLevel, hierarchy.HierarchyCode, SessionManager.CurrentLanguage);
        }

        //var menus = menuItems.IsNotNullData() ? menuItems.Cast<SecMenuItem>().Where(m => m.Parent != 0).ToList() : new List<SecMenuItem>();
        var menus = menuItems.IsNotNullData() ? menuItems.Cast<SecMenuItem>() : new List<SecMenuItem>();
        for (int i = 0; i < menuItems.Count; i++)
        {
            var permissionCode = menuItems[i].Permissions;
            var title = menuItems[i].Title;

            if (WebSiteConstants.SEC_PERMISSION_DASHBOARD.Equals(permissionCode, StringComparison.OrdinalIgnoreCase) ||
                WebSiteConstants.SEC_PERMISSION_DASHBOARD_MS.Equals(permissionCode, StringComparison.OrdinalIgnoreCase))
            {
                defaultMenu = menuItems[i];
            }

            if (WebSiteConstants.SEC_PERMISSION_VIEW_NOTIFICATION_SETTING.Equals(permissionCode, StringComparison.OrdinalIgnoreCase))
            {
                menuItems[i].Permissions = WebSiteConstants.SEC_PERMISSION_NOTIFICATION_ADMIN_SETTING;
            }
        }

        for (int i = 0; i < menuItems.Count; i++)
        {
            var permissionCode = menuItems[i].Permissions;
            if (WebSiteConstants.SEC_PERMISSION_MANAGE_DOCUMENT_TYPES.Trim().Equals(permissionCode.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                menuItems.Remove(menuItems[i]);
                i--;
                continue;
            }

            SecMenuItem menu = menus.Where(m => m.SiteMapId == menuItems[i].Parent).SingleOrDefault();
            if (menu.IsNotNullData() && SitemapHandler.IsRemoveFromSitemaps(menu.IsNullData() ? "" : menu.Title, menuItems[i].Title))
            {
                menuItems.Remove(menuItems[i]);
                continue;
            }
        }

        uxTreeMenuCS.DataSource = menuItems;
        uxTreeMenuCS.DataBind();
        uxTreeMenuCS.ExpandAllNodes();

        if (!string.IsNullOrEmpty(defaultMenu.Permissions))
        {
            RadTreeNode node = uxTreeMenuCS.FindNodeByText(defaultMenu.Title);
            if (node != null)
                node.Checked = true;
        }

        if (SystemId == 2)
        {
            #region Removes Case mgmt menu for Chain, MIC Chain, Merchant hierarchy level

            if (SessionManager.CurrentUser.ASClient == 22 && menuItems != null && menuItems.Count > 0 &&
                ("MPSCHAIN".Equals(hierarchy.HierarchyLevel, StringComparison.OrdinalIgnoreCase)
                || "CHAIN".Equals(hierarchy.HierarchyLevel, StringComparison.OrdinalIgnoreCase)
                || "MERCHANT".Equals(hierarchy.HierarchyLevel, StringComparison.OrdinalIgnoreCase)))
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
        }

        //LadingPage
        DataTable td = GeneralFuncsLib.GetDefaultLandingPage(HierarchyId);
        if (td.HasData())
        {
            SecMenuItem menu = menus.FirstOrDefault(m => m.SiteMapId == td.Rows[0]["SiteMapId"].ToInt());
            uxDefaultLandingPage.Text = menu.IsNotNullData() ? menu.Title : string.Empty;
        }
        else
            PlaceHolder1.Visible = false;
    }

    protected void uxTreeMenuCS_NodeDataBound(object sender, RadTreeNodeEventArgs e)
    {
        SecMenuItem element = (SecMenuItem)e.Node.DataItem;
        e.Node.Attributes.Add("SiteMapId", element.SiteMapId.ToString());
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

    private bool CheckPermissionIDInList(string strPermission, List<Permission> permissions)
    {
        if (permissions.Any(m => m.PermissionCode == strPermission))
            return true;
        return false;
    }

    public void SetSelectedByPermissions(PermissionCollection permissions)
    {
        List<Permission> pers = permissions.IsNotNullData() ? permissions.Cast<Permission>().ToList() : new List<Permission>();
        foreach (RepeaterItem item in uxAccessFuncList.Items)
        {
            AS.Controls.Global.CheckBox chkbox = item.FindControl("uxPermission") as AS.Controls.Global.CheckBox;
            if (chkbox != null)
            {
                chkbox.Checked = CheckPermissionIDInList(chkbox.Value, pers);
            }
        }
    }

    private void Bind1099KRoleList()
    {
        int role1099K = GetHierarchyIn1099KByHierarchyID(HierarchyId);
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(true);
        parameters.Add(new FilterParameter("@UserType", SessionManager.UserRoleType.ToString(), DbType.AnsiString));
        DataTable roleList = WebServices.SecurityServices.GetReports("spa_SEC_Get1099KHierarchiesList", parameters);
        if (roleList.HasData())
        {
            DataRow[] drs = roleList.Select("HierarchyID = '" + role1099K + "' ");
            if (drs.IsNotNullData() && drs.Length > 0)
            {
                uxPlc1099KRole.Visible = true;
                lb1099K.Text = drs[0]["HierarchyName"].ToString();
            }
            else
                //uxPlc1099KRole.Visible = false;
                lb1099K.Text = string.Empty;
        }
    }

    private void BindPCIRoleList()
    {
        int rolePCI = GetHierarchyInPCIByHierarchyID(HierarchyId);
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(true);
        DataTable roleList = WebServices.SecurityServices.GetReports("spa_SEC_GetPCIHierarchiesList", parameters);
        if (roleList.HasData())
        {
            DataRow[] drs = roleList.Select("HierarchyID = '" + rolePCI + "' ");
            if (drs.IsNotNullData() && drs.Length > 0)
            {
                uxPlcPCIRole.Visible = true;
                lbPCIAdmin.Text = drs[0]["HierarchyName"].ToString();
            }
            else
            {
                uxPlcPCIRole.Visible = false;
                lbPCIAdmin.Text = string.Empty;
            }
        }
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

    #endregion Private Methods
}