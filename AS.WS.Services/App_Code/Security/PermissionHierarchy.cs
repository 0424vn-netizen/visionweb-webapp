using System;
using System.Data;
using System.Web.Services;
using AS.Security.WS.Entities;
using AS.Common.Logger;

/// <summary>
/// Summary description for PermissionHierarchy
/// </summary>
public partial class SecurityService
{
    #region Permission

    [WebMethod(Description = "Get permissions ")]
    public PermissionCollection GetPermissionsByUserGroupType(int clientId, string userName, int systemId, string group, string type, int languageId)
    {
        try
        {
            return _PermHierService.GetPermissionsByUserGroupType(clientId, userName, systemId, group, type, languageId);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("GetPermissionsByUserGroupType:\n" + ex.ToString());
            return new PermissionCollection();
        }
    }

    [WebMethod(Description = "Get MS permissions by type ")]
    public PermissionCollection GetMSPermissionsByType(int clientId, string type, string hierarchyLevel, string hierarchyCode)
    {
        try
        {

            return _PermHierService.GetMSPermissionsByType(clientId, type, hierarchyLevel, hierarchyCode);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("GetMSPermissionsByType:\n" + ex.ToString());
            return new PermissionCollection();
        }
    }

    [WebMethod(Description = "Get all permissions of a hierarchy.")]
    public PermissionCollection GetPermissionsInHierarchy(int hierarchyId)
    {
        try
        {
            
            return _PermHierService.GetPermissionsInHierarchy(hierarchyId);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("GetPermissionsInHierarchy:\n" + ex.ToString());
            return new PermissionCollection();
        }
    }

    [WebMethod(Description = "Get all permissions of an user.")]
    public PermissionCollection GetPermissionsForUser(int clientId, string userName)
    {
        try
        {
            
            return _PermHierService.GetPermissionOfUser(clientId, userName);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("GetPermissionsForUser:\n" + ex.ToString());
            return new PermissionCollection();
        }
    }

    [WebMethod(Description = "Get all permissions of an user with SSO.")]
    public PermissionCollection GetPermissionsForUserWithSSO(int clientId, string userName, int ssoLevel)
    {
        try
        {

            return _PermHierService.GetPermissionsForUserWithSSO(clientId, userName, ssoLevel);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("GetPermissionsForUserWithSSO:\n" + ex.ToString());
            return new PermissionCollection();
        }
    }

    [WebMethod]
    public void InsertExcludePermissionForUser(int clientID, string username, int permissionId, DateTime startDate, DateTime endDate, Guid updatedBy, bool isLogged)
    {


        _PermHierService.InsertExcludePermissionForUser(clientID, username, permissionId, startDate, endDate, updatedBy, isLogged);
    }
    [WebMethod]
    public void DeleteExcludePermissionForUser(int clientID, string username, int permissionId, Guid updatedBy, bool isLogged)
    {


        _PermHierService.DeleteExcludePermissionForUser(clientID, username, permissionId, updatedBy, isLogged);

    }
    [WebMethod]
    public PermissionCollection GetExcludePermissionsForUser(int clientID, string username)
    {
        
        return _PermHierService.GetExcludePermissionsForUser(clientID, username);
    }

    [WebMethod(Description = "Get AS permissions ")]
    public PermissionCollection GetPermissionsByASUserGroupType(int clientId, string userName, int systemId, bool isViewAll = false, bool isExculdeAccessPer = false)
    {
        try
        {
            return _PermHierService.GetPermissionsByASUserGroupType(clientId, userName, systemId, isViewAll, isExculdeAccessPer);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("GetPermissionsByASUserGroupType:\n" + ex.ToString());
            return new PermissionCollection();
        }
    }

    #endregion

    #region Hierarchy

    [WebMethod(Description = "Get all assignable hierarchy by hierarchy")]
    public HierarchyCollection GetAssignableHierarchy(int hierarchyId, string userType)
    {
        try
        {
            
            return _PermHierService.GetAssignableHierarchy(hierarchyId, userType);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("GetAssignableHierarchy:\n" + ex.ToString());
            return new HierarchyCollection();
        }
    }

    [WebMethod(Description = "Get all MS assignable hierarchy by hierarchy or user id")]
    public HierarchyCollection GetMSAssignableHierarchy(int clientId,
        string userId, string hierarchyLevel, string hierarchyCode, int systemId)
    {
        try
        {
            return _PermHierService.GetMSAssignableHierarchy(clientId,
                userId, hierarchyLevel, hierarchyCode, systemId);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("GetMSAssignableHierarchy:\n" + ex.ToString());
            return new HierarchyCollection();
        }
    }

    [WebMethod(Description = "Get all assignable hierarchy list by hierarchy")]
    public DataTable GetHierarchyAssignableListByHierarchy(int hierarchyId)
    {
        return (_PermHierService).GetHierarchyAssignableListByHierarchy(hierarchyId);
    }
    [WebMethod(Description = "Update assignable hierarchy")]
    public void UpdateAssignableHierarchy(AssignableHierarchyModel assignableHierarchyModel)
    {
        try
        {
            _PermHierService.UpdateAssignableHierarchy(assignableHierarchyModel);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("UpdateAssignableHierarchy:\n" + ex.ToString());
        }
    }

    [WebMethod(Description = "Get all hierarchys.")]
    public HierarchyCollection GetHierarchysOfUser(int clientId, string userName, int systemId)
    {
        try
        {
            
            return _PermHierService.GetHierarchysOfUser(clientId, userName, systemId);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("GetHierarchysOfUser:\n" + ex.ToString());
            return new HierarchyCollection();
        }
    }

    [WebMethod(Description = "Get all hierarchys of a system.")]
    public HierarchyCollection GetHierarchyTreeForUser(int clientId, string username, int systemId)
    {
        try
        {
            return _PermHierService.GetHierarchyTreeForUser(clientId, username, systemId);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("GetHierarchyTreeForUser:\n" + ex.ToString());
            return new HierarchyCollection();
        }
    }

    [WebMethod(Description = "Get hierarchy by id.")]
    public Hierarchy GetHierarchyById(int hierarchyId)
    {
        try
        {
            
            return _PermHierService.GetHierarchyById(hierarchyId);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("GetHierarchyById:\n" + ex.ToString());
            return null;
        }
    }

    [WebMethod(Description = "Get hierarchy by name.")]
    public HierarchyCollection GetHierarchyByName(string hierarchyName, int clientId, int systemId)
    {
        try
        {
            return _PermHierService.GetHierarchyByName(hierarchyName, clientId, systemId);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("GetHierarchyByName:\n" + ex.ToString());
            return new HierarchyCollection();
        }
    }

    [WebMethod(Description = "Create new hierarchy for a system.")]
    public int CreateHierarchyWithCreatedUser(int systemId, string hierarchyName,
        int parentHierarchy, string hierarchyDesc, Guid createdBy,
        int clientId, string hierarchyCode, string hierarchyLevel)
    {
        try
        {
            var hierarchyModel = new HierarchyModel()
            {
                SystemId = systemId,
                HierarchyId = 0,
                HierarchyName = hierarchyName,
                ParentHierarchy = parentHierarchy,
                HierarchyDesc = hierarchyDesc,
                Status = "1",
                CreatedBy = createdBy,
                ClientId = clientId,
                HierarchyCode = hierarchyCode,
                HierarchyLevel = hierarchyLevel,
            };
            return _PermHierService.CreateHierarchy(hierarchyModel);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("CreateHierarchyWithCreatedUser:\n" + ex.ToString());
            return 0;
        }
    }
    [WebMethod(Description = "Delete a hierarchy.")]
    public bool DeleteHierarchy(int hierarchyId)
    {
        try
        {

            _PermHierService.DeleteHierarchy(hierarchyId);
            return true;
        }
        catch (Exception ex)
        {

            LoggerManager.Error("DeleteHierarchy:\n" + ex.ToString());
            return false;
        }
    }

    [WebMethod(Description = "Update a hierarchy.")]
    public int UpdateHierarchy(int systemId, int hierarchyID, string hierarchyName,
        int parentHierarchy, string hierarchyDescription, string status,
        int clientId, string hierarchyCode, string hierarchyLevel, Guid updatedBy)
    {
        try
        {
            var hierarchyModel = new HierarchyModel()
            {
                SystemId = systemId,
                HierarchyId = hierarchyID,
                HierarchyName = hierarchyName,
                ParentHierarchy = parentHierarchy,
                HierarchyDesc = hierarchyDescription,
                Status = status,
                ClientId = clientId,
                HierarchyCode = hierarchyCode,
                HierarchyLevel = hierarchyLevel,
                UpdatedBy = updatedBy,
            };
            return _PermHierService.UpdateHierarchy(hierarchyModel);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("UpdateHierarchy:\n" + ex.ToString());
            return -3;
        }
    }

    [WebMethod(Description = "Get all hierarchys.")]
    public void UpdateChildHierarchyPermission(int hierarchyID)
    {
        try
        {

            _PermHierService.UpdateChildHierarchyPermission(hierarchyID);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("UpdateChildHierarchyPermission:\n" + ex.ToString());
        }
    }

    [WebMethod(Description = "Remove a permission out of a hierarchy.")]
    public void RemovePermissionFromHierarchy(int permissionId, int hierarchyId)
    {
        try
        {

            _PermHierService.RemovePermissionFromHierarchy(permissionId, hierarchyId);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("RemovePermissionFromHierarchy:\n" + ex.ToString());
        }
    }

    [WebMethod(Description = "Remove a permission code out of a hierarchy.")]
    public void RemovePermissionCodeFromHierarchy(string permissionCode, int hierarchyId, int clientId, Guid updatedBy, bool isLogged)
    {
        try
        {

            _PermHierService.RemovePermissionFromHierarchy(permissionCode, hierarchyId, clientId, updatedBy, isLogged);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("RemovePermissionCodeFromHierarchy:\n" + ex.ToString());
        }
    }


    [WebMethod(Description = "Remove list of permissions out of a hierarchy.")]
    public void RemovePermissionsFromHierarchy(int[] permissionIds, int hierarchyId)
    {
        try
        {

            foreach (int permissionId in permissionIds)
            {
                _PermHierService.RemovePermissionFromHierarchy(permissionId, hierarchyId);
            }
        }
        catch (Exception ex)
        {
            LoggerManager.Error("RemovePermissionFromHierarchy:\n" + ex.ToString());
        }
    }

    [WebMethod(Description = "Add a permission into a hierarchy.")]
    public void AddPermissionIntoHierarchy(int permissionId, int hierarchyId)
    {
        try
        {

            _PermHierService.AddPermissionIntoHierarchy(permissionId, hierarchyId);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("AddPermissionIntoHierarchy:\n" + ex.ToString());
        }
    }

    [WebMethod(Description = "Add a permission code into a hierarchy.")]
    public void AddPermissionCodeIntoHierarchy(string permissionCode, int hierarchyId, Guid updatedBy, bool isLogged)
    {
        try
        {

            _PermHierService.AddPermissionIntoHierarchy(permissionCode, hierarchyId, updatedBy, isLogged);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("AddPermissionCodeIntoHierarchy:\n" + ex.ToString());
        }
    }

    [WebMethod(Description = "Add list of permissions into a hierarchy.")]
    public void AddPermissionsIntoHierarchy(int[] permissionIds, int hierarchyId)
    {
        try
        {

            foreach (int PermissionId in permissionIds)
            {
                _PermHierService.AddPermissionIntoHierarchy(PermissionId, hierarchyId);
            }
        }
        catch (Exception ex)
        {
            LoggerManager.Error("AddPermissionsIntoHierarchy:\n" + ex.ToString());
        }
    }

    [WebMethod(Description = "")]
    public bool CheckUniqueHierarchyForUpdate(int clientId, string hierarchyName, int hierarchyId)
    {
        try
        {

            return _PermHierService.CheckUniqueHierarchyForUpdate(clientId, hierarchyName, hierarchyId);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("CheckUniqueHierarchyForUpdate:\n" + ex.ToString());
            return true;
        }


    }
    [WebMethod(Description = "")]
    public bool CheckUniqueHierarchyForCreate(int clientId, string hierarchyName)
    {
        try
        {

            return _PermHierService.CheckUniqueHierarchyForCreate(clientId, hierarchyName);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("CheckUniqueHierarchyForCreate:\n" + ex.ToString());
            return true;
        }
    }
    [WebMethod(Description = "")]
    public void CreateUpdateRiskGroupForUser(int clientId, Guid userid, int groupid)
    {
        try
        {

            _PermHierService.CreateUpdateRiskGroupForUser(clientId, userid, groupid);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("CreateUpdateRiskGroupForUser:\n" + ex.ToString());
        }
    }
    [WebMethod(Description = "")]
    public int GetRiskGroupForUser(int clientId, Guid userid)
    {
        try
        {

            return _PermHierService.GetRiskGroupForUser(clientId, userid);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("GetRiskGroupForUser:\n" + ex.ToString());
            return -1;
        }
    }

    [WebMethod(Description = "Get all hierarchy access by hierarchy")]
    public HierarchyAccessCollection GetSiteJumpAccessByHierarchy(int hierarchyId, string type)
    {
        try
        {

            return _PermHierService.GetSiteJumpAccessByHierarchy(hierarchyId, type);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("GetSiteJumpAccessByHierarchy:\n" + ex.ToString());
            return new HierarchyAccessCollection();
        }
    }

    [WebMethod(Description = "Update site jump access")]
    public void UpdateSiteJumpAccess(int hierarchyId, string login, string createdBy, string type, bool isRemove)
    {
        try
        {

            _PermHierService.UpdateSiteJumpAccess(hierarchyId, login, createdBy, type, isRemove);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("UpdateSiteJumpAccess:\n" + ex.ToString());
        }
    }

    [WebMethod(Description = "Update hierarchy access")]
    public void UpdateHierarchyAccess(int hierarchyId, string entityNumber, string createdBy, string type, bool isRemove)
    {
        try
        {

            _PermHierService.UpdateHierarchyAccess(hierarchyId, entityNumber, createdBy, type, isRemove);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("UpdateHierarchyAccess:\n" + ex.ToString());
        }
    }

    [WebMethod(Description = "Get all hierarchy access by hierarchy")]
    public HierarchyAccessCollection GetHierarchyAccessByHierarchy(int hierarchyId, string type)
    {
        try
        {

            return _PermHierService.GetHierarchyAccessByHierarchy(hierarchyId, type);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("GetHierarchyAccessByHierarchy:\n" + ex.ToString());
            return new HierarchyAccessCollection();
        }
    }
    #endregion
}
