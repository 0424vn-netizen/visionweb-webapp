using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using AS.Security.WS.Entities;
using AS.Common.DBManager;

namespace AS.Security.WS.Data
{
    public class PermissionDao : BaseSecDao
    {
        ///Description for BindPermission
        ///Author:
        ///Date: 

        public PermissionDao(string connString) : base(connString) { }

        public Permission BindPermission(IDataReader reader)
        {
            Permission item = new Permission();
            if (!reader.IsDBNull(reader.GetOrdinal("PermissionId"))) item.PermissionId = (int)reader["PermissionId"];
            if (!reader.IsDBNull(reader.GetOrdinal("SystemId"))) item.SystemId = (int)reader["SystemId"];
            if (!reader.IsDBNull(reader.GetOrdinal("PermissionCode"))) item.PermissionCode = (string)reader["PermissionCode"];
            if (!reader.IsDBNull(reader.GetOrdinal("Description"))) item.Description = (string)reader["Description"];
            if (!reader.IsDBNull(reader.GetOrdinal("Group"))) item.Group = (string)reader["Group"];
            if (!reader.IsDBNull(reader.GetOrdinal("Type"))) item.Type = (string)reader["Type"];
            if (!reader.IsDBNull(reader.GetOrdinal("DateCreated"))) item.DateCreated = (DateTime)reader["DateCreated"];
            if (!reader.IsDBNull(reader.GetOrdinal("GroupFuncName"))) item.GroupFuncName = (string)reader["GroupFuncName"];
            if (!reader.IsDBNull(reader.GetOrdinal("NodeOrder"))) item.NodeOrder = (int)reader["NodeOrder"];
            if (!reader.IsDBNull(reader.GetOrdinal("PermissionCodesRequire"))) item.PermissionCodesRequire = (string)reader["PermissionCodesRequire"];
            return item;
        }

        public PermissionCollection GetPermission(int hierarchyId)
        {

            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_GetPermissionsInHierarchy");
            SecurityDatabase.AddInParameter(command, "@HierarchyId", DbType.Int32, hierarchyId);
            PermissionCollection permissionCollection = new PermissionCollection();
            using (IDataReader reader = base.ExecuteReader(command))
            {
                while (reader.Read())
                {
                    Permission permission = BindPermission(reader);
                    permissionCollection.Add(permission);
                }
                reader.Close();
            }
            return permissionCollection;
        }

        public PermissionCollection GetPermissionsForUser(int clientId, string userName)
        {
            PermissionCollection permissions = null;

            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_GetPermissionsForUser");
            SecurityDatabase.AddInParameter(command, "@ASClient", DbType.Int32, clientId);
            SecurityDatabase.AddInParameter(command, "@UserName", DbType.String, userName);
            SecurityDatabase.AddInParameter(command, "@IsReskin", DbType.Boolean, true);

            permissions = new PermissionCollection();
            using (IDataReader reader = base.ExecuteReader(command))
            {
                while (reader.Read())
                {
                    Permission permission = BindPermission(reader);
                    permissions.Add(permission);
                }
                reader.Close();
            }
            return permissions;
        }

        public PermissionCollection GetPermissionsForUserWithSSO(int clientId, string userName, int ssoLevel)
        {
            PermissionCollection permissions = null;

            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_GetPermissionsForUser");
            SecurityDatabase.AddInParameter(command, "@ASClient", DbType.Int32, clientId);
            SecurityDatabase.AddInParameter(command, "@UserName", DbType.String, userName);
            SecurityDatabase.AddInParameter(command, "@SSOSecurityLevelID", DbType.Int32, ssoLevel);
            SecurityDatabase.AddInParameter(command, "@IsReskin", DbType.Boolean, true);

            permissions = new PermissionCollection();
            using (IDataReader reader = base.ExecuteReader(command))
            {
                while (reader.Read())
                {
                    Permission permission = BindPermission(reader);
                    permissions.Add(permission);
                }
                reader.Close();
            }
            return permissions;
        }

        public PermissionCollection GetPermissionsByUserGroupType(int clientId, string userName, int systemId, string group, string type, int languageId)
        {
            PermissionCollection permissions = null;

            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_GetPermissionsByUserGroupType");
            SecurityDatabase.AddInParameter(command, "@ASClient", DbType.Int32, clientId);
            SecurityDatabase.AddInParameter(command, "@UserName", DbType.String, userName);
            SecurityDatabase.AddInParameter(command, "@SystemId", DbType.Int32, systemId);
            SecurityDatabase.AddInParameter(command, "@Group", DbType.String, group);
            SecurityDatabase.AddInParameter(command, "@Type", DbType.String, type);
            SecurityDatabase.AddInParameter(command, "@LanguageId", DbType.Int32, languageId);
            SecurityDatabase.AddInParameter(command, "@IsReskin", DbType.Boolean, true);
            permissions = new PermissionCollection();
            using (IDataReader reader = base.ExecuteReader(command))
            {
                while (reader.Read())
                {
                    Permission permission = BindPermission(reader);
                    permissions.Add(permission);
                }
                reader.Close();
            }
            return permissions;
        }

        public PermissionCollection GetPermissionsByASUserGroupType(int clientId, string userName, int systemId, bool isViewAll = false, bool isExculdeAccessPer = false)
        {
            PermissionCollection permissions = new PermissionCollection();
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_GetPermissionsByASUserGroupType");
            SecurityDatabase.AddInParameter(command, "@ASClient", DbType.Int32, clientId);
            SecurityDatabase.AddInParameter(command, "@UserName", DbType.String, userName);
            SecurityDatabase.AddInParameter(command, "@SystemId", DbType.Int32, systemId);
            SecurityDatabase.AddInParameter(command, "@IsViewAll", DbType.Boolean, isViewAll);

            using (IDataReader reader = base.ExecuteReader(command))
            {
                while (reader.Read())
                {
                    if (isExculdeAccessPer)
                    {
                        if (reader["Checked"].ToString() == "0")
                        {
                            Permission permission = BindPermission(reader);
                            permissions.Add(permission);
                        }
                    }
                    else
                    {
                        Permission permission = BindPermission(reader);
                        permissions.Add(permission);
                    }
                }
                reader.Close();
            }
            return permissions;
        }

        public PermissionCollection GetMSPermissionsByType(int clientId, string type, string hierarchyLevel, string hierarchyCode)
        {
            PermissionCollection permissions = null;

            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_GetMSPermissionsByType");
            SecurityDatabase.AddInParameter(command, "@ASClient", DbType.Int32, clientId);
            SecurityDatabase.AddInParameter(command, "@Type", DbType.String, type);
            SecurityDatabase.AddInParameter(command, "@HierarchyLevel", DbType.String, hierarchyLevel);
            SecurityDatabase.AddInParameter(command, "@HierarchyCode", DbType.String, hierarchyCode);

            permissions = new PermissionCollection();
            using (IDataReader reader = base.ExecuteReader(command))
            {
                while (reader.Read())
                {
                    Permission permission = BindPermission(reader);
                    permissions.Add(permission);
                }
                reader.Close();
            }
            return permissions;
        }

        public void InsertExcludePermissionForUser(int clientID, string username, int permissionId, DateTime startDate, DateTime endDate, Guid updatedBy, bool isLogged)
        {

            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_InsExcludePermisionItem");
            SecurityDatabase.AddInParameter(command, "@ClientId", DbType.Int32, clientID);
            SecurityDatabase.AddInParameter(command, "@UserName", DbType.AnsiString, username);
            SecurityDatabase.AddInParameter(command, "@PermissionId", DbType.Int32, permissionId);
            SecurityDatabase.AddInParameter(command, "@EffStartDate", DbType.DateTime, startDate);
            SecurityDatabase.AddInParameter(command, "@EffEndDate", DbType.DateTime, endDate);
            SecurityDatabase.AddInParameter(command, "@UpdatedBy", DbType.Guid, updatedBy);
            SecurityDatabase.AddInParameter(command, "@IsLogged", DbType.Boolean, isLogged);
            base.ExecuteNonQuery(command);

        }
        public void DeleteExcludePermissionForUser(int clientID, string username, int permissionId, Guid updatedBy, bool isLogged)
        {

            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_DelExcludePermissionItem");
            SecurityDatabase.AddInParameter(command, "@ClientId", DbType.Int32, clientID);
            SecurityDatabase.AddInParameter(command, "@UserName", DbType.AnsiString, username);
            SecurityDatabase.AddInParameter(command, "@PermissionId", DbType.Int32, permissionId);
            SecurityDatabase.AddInParameter(command, "@UpdatedBy", DbType.Guid, updatedBy);
            SecurityDatabase.AddInParameter(command, "@IsLogged", DbType.Boolean, isLogged);
            base.ExecuteNonQuery(command);

        }
        public PermissionCollection GetExcludePermissionsForUser(int clientId, string userName)
        {
            PermissionCollection permissions = null;

            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_GetExcludePermisionList");
            SecurityDatabase.AddInParameter(command, "@ClientId", DbType.Int32, clientId);
            SecurityDatabase.AddInParameter(command, "@UserName", DbType.String, userName);

            permissions = new PermissionCollection();
            using (IDataReader reader = base.ExecuteReader(command))
            {
                while (reader.Read())
                {
                    Permission permission = BindPermission(reader);
                    permissions.Add(permission);
                }
                reader.Close();
            }
            return permissions;
        }

        public bool CheckUserPermission(int ddsClient, string userID, string permissionCode)
        {
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_CheckUserPermission");
            SecurityDatabase.AddInParameter(command, "@ASClient", DbType.Int16, ddsClient);
            SecurityDatabase.AddInParameter(command, "@UserID", DbType.AnsiString, userID);
            SecurityDatabase.AddInParameter(command, "@PermissionCode", DbType.AnsiString, permissionCode);
            SecurityDatabase.AddParameter(command, "@RETURN_VALUE", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Default, null);
            base.ExecuteNonQuery(command);
            return (int)SecurityDatabase.GetParameterValue(command, "@Return_Value") == 1;
        }


    }
}
