using System;
using System.Collections.Generic;
using System.Text;

using AS.Security.WS.Entities;
using AS.Security.WS.Data;
using System.Data;

namespace AS.Security.WS.Business
{
    public class PermissionHierarchyServices
    {
        readonly PermissionDao _PermissionDAO;
        readonly HierarchyDao _HierarchyDAO;
        public PermissionHierarchyServices(string connString)
        {
            _PermissionDAO = new PermissionDao(connString);
            _HierarchyDAO = new HierarchyDao(connString);
        }
        #region Permission Methods

        public PermissionCollection GetPermissionsInHierarchy(int hierarchyId)
        {
            return _PermissionDAO.GetPermission(hierarchyId);
        }

        public PermissionCollection GetPermissionOfUser(int clientId, string userName)
        {
            return _PermissionDAO.GetPermissionsForUser(clientId, userName);
        }

        public PermissionCollection GetPermissionsForUserWithSSO(int clientId, string userName, int ssoLevel)
        {
            return _PermissionDAO.GetPermissionsForUserWithSSO(clientId, userName, ssoLevel);
        }

        public PermissionCollection GetPermissionsByUserGroupType(int clientId, string userName, int systemId, string group, string type, int languageId)
        {
            return _PermissionDAO.GetPermissionsByUserGroupType(clientId, userName, systemId, group, type, languageId);
        }
        public PermissionCollection GetMSPermissionsByType(int clientId, string type, string hierarchyLevel, string hierarchyCode)
        {
            return _PermissionDAO.GetMSPermissionsByType(clientId, type, hierarchyLevel, hierarchyCode);
        }
        public void InsertExcludePermissionForUser(int clientID, string username, int permissionId, DateTime startDate, DateTime endDate, Guid updatedBy, bool isLogged)
        {
            _PermissionDAO.InsertExcludePermissionForUser(clientID, username, permissionId, startDate, endDate, updatedBy, isLogged);
        }
        public void DeleteExcludePermissionForUser(int clientID, string username, int permissionId, Guid updatedBy, bool isLogged)
        {
            _PermissionDAO.DeleteExcludePermissionForUser(clientID, username, permissionId, updatedBy, isLogged);
        }
        public PermissionCollection GetExcludePermissionsForUser(int clientID, string username)
        {
            return _PermissionDAO.GetExcludePermissionsForUser(clientID, username);
        }

        public PermissionCollection GetPermissionsByASUserGroupType(int clientId, string userName, int systemId, bool isViewAll = false, bool isExculdeAccessPer = false)
        {
            return _PermissionDAO.GetPermissionsByASUserGroupType(clientId, userName, systemId, isViewAll, isExculdeAccessPer);
        }

        #endregion

        #region Hierarchy Methods
        public HierarchyCollection GetAssignableHierarchy(int hierarchyId, string userType)
        {
            return _HierarchyDAO.GetAssignableHierarchy(hierarchyId, userType);
        }
        public HierarchyCollection GetMSAssignableHierarchy(int clientId,
            string userId, string hierarchyLevel, string hierarchyCode, int systemId)
        {
            return _HierarchyDAO.GetMSAssignableHierarchy(
                clientId, userId, hierarchyLevel, hierarchyCode, systemId);
        }
        public System.Data.DataTable GetHierarchyAssignableListByHierarchy(int hierarchyId)
        {

            return _HierarchyDAO.GetHierarchyAssignableListByHierarchy(hierarchyId);
        }
        public void UpdateAssignableHierarchy(AssignableHierarchyModel assignableHierarchyModel)
        {

            _HierarchyDAO.UpdateAssignableHierarchy(assignableHierarchyModel);
        }
        public void UpdateChildHierarchyPermission(int hierarchyID)
        {

            _HierarchyDAO.UpdateChildHierarchyPermission(hierarchyID);
        }
        public HierarchyCollection GetHierarchyTreeForUser(int clientId, string username, int systemId)
        {

            return _HierarchyDAO.GetHierarchyTreeForUser(clientId, username, systemId);
        }

        public Hierarchy GetHierarchyById(int hierarchyId)
        {

            return _HierarchyDAO.GetHierarchyByHierarchyID(hierarchyId);
        }

        public HierarchyCollection GetHierarchyByName(string hierarchyName, int clientId, int systemId)
        {
            return _HierarchyDAO.GetHierarchyByHierarchyName(
                hierarchyName, clientId, systemId);
        }

        public HierarchyCollection GetHierarchysOfUser(int clientId, string userName, int systemId)
        {

            return _HierarchyDAO.GetHierarchy(clientId, userName, systemId);
        }

        public int CreateHierarchy(HierarchyModel hierarchyModel)
        {
            return _HierarchyDAO.InsertHierarchy(hierarchyModel);
        }

        public void DeleteHierarchy(int hierarchyId)
        {

            _HierarchyDAO.DeleteHierarchy(hierarchyId);
        }

        public int UpdateHierarchy(HierarchyModel hierarchyModel)
        {
            return _HierarchyDAO.UpdateHierarchy(hierarchyModel);
        }

        public void RemovePermissionFromHierarchy(int permissionId, int hierarchyId)
        {

            _HierarchyDAO.DeletePermissionFromHierarchy(permissionId, hierarchyId);
        }

        public void RemovePermissionFromHierarchy(string permissionCode, int hierarchyId, int clientId, Guid updatedBy, bool isLogged)
        {

            _HierarchyDAO.DeletePermissionFromHierarchy(permissionCode, hierarchyId, clientId, updatedBy, isLogged);
        }

        public void AddPermissionIntoHierarchy(int permissionId, int hierarchyId)
        {

            _HierarchyDAO.InsertPermissionIntoHierarchy(permissionId, hierarchyId);
        }
        public void AddPermissionIntoHierarchy(string permissionCode, int hierarchyId, Guid updatedBy, bool isLogged)
        {

            _HierarchyDAO.InsertPermissionIntoHierarchy(permissionCode, hierarchyId, updatedBy, isLogged);
        }
        public bool CheckUniqueHierarchyForUpdate(int clientId, string hierarchyName, int hierarchyId)
        {

            return _HierarchyDAO.CheckUniqueHierarchyForUpdate(clientId, hierarchyName, hierarchyId);

        }
        public bool CheckUniqueHierarchyForCreate(int clientId, string hierarchyName)
        {

            return _HierarchyDAO.CheckUniqueHierarchyForCreate(clientId, hierarchyName);

        }
        public void CreateUpdateRiskGroupForUser(int clientId, Guid userid, int groupid)
        {

            _HierarchyDAO.CreateUpdateRiskGroupForUser(clientId, userid, groupid);
        }
        public int GetRiskGroupForUser(int clientId, Guid userid)
        {

            return _HierarchyDAO.GetRiskGroupForUser(clientId, userid);
        }

        public HierarchyAccessCollection GetSiteJumpAccessByHierarchy(int hierarchyId, string type)
        {

            return _HierarchyDAO.GetSiteJumpAccessByHierarchy(hierarchyId, type);
        }
        public void UpdateSiteJumpAccess(int hierarchyId, string login, string createdBy, string type, bool isRemove)
        {

            _HierarchyDAO.UpdateSiteJumpAccess(hierarchyId, login, createdBy, type, isRemove);
        }
        public void UpdateHierarchyAccess(int hierarchyId, string entityNumber, string createdBy, string type, bool isRemove)
        {

            _HierarchyDAO.UpdateHierarchyAccess(hierarchyId, entityNumber, createdBy, type, isRemove);
        }
        public HierarchyAccessCollection GetHierarchyAccessByHierarchy(int hierarchyId, string type)
        {

            return _HierarchyDAO.GetHierarchyAccessByHierarchy(hierarchyId, type);
        }
        #endregion
    }
}