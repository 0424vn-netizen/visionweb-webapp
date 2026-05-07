using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using AS.Security.WS.Entities;

namespace AS.Security.WS.Data
{
    public class HierarchyDao : BaseSecDao
    {
        ///Description for BindHierarchy
        ///Author:
        ///Date:  

        public HierarchyDao(string connString) : base(connString) { }

        public Hierarchy BindHierarchy(IDataReader reader)
        {
            List<string> columnsName = new List<string>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                columnsName.Add(reader.GetName(i));
            }

            Hierarchy item = new Hierarchy();
            if (!reader.IsDBNull(reader.GetOrdinal("HierarchyID")))
                item.HierarchyID = (int)reader["HierarchyID"];
            if (!reader.IsDBNull(reader.GetOrdinal("SystemId"))) 
                item.SystemId = (int)reader["SystemId"];
            if (!reader.IsDBNull(reader.GetOrdinal("HierarchyName"))) 
                item.HierarchyName = (string)reader["HierarchyName"];
            if (!reader.IsDBNull(reader.GetOrdinal("HierarchyParent")))
                item.HierarchyParent = (int)reader["HierarchyParent"];
            if (!reader.IsDBNull(reader.GetOrdinal("HierarchyDescription"))) 
                item.HierarchyDescription = (string)reader["HierarchyDescription"];
            if (!reader.IsDBNull(reader.GetOrdinal("ActvStatus"))) 
                item.ActvStatus = (string)reader["ActvStatus"];

            if (columnsName.IndexOf("UserCount") >= 0
                && !reader.IsDBNull(reader.GetOrdinal("UserCount")))
            {
                item.UserCount = (int)reader["UserCount"];
            }

            if (columnsName.IndexOf("UserRoleType") >= 0
                && !reader.IsDBNull(reader.GetOrdinal("UserRoleType")))
            {
                item.UserRoleType = (string)reader["UserRoleType"];
            }

            if (columnsName.IndexOf("ClientId") >= 0
                && !reader.IsDBNull(reader.GetOrdinal("ClientId")))
            {
                item.ClientId = (int)reader["ClientId"];
            }

            if (columnsName.IndexOf("HierarchyCode") >= 0
                && !reader.IsDBNull(reader.GetOrdinal("HierarchyCode")))
            {
                item.HierarchyCode = (string)reader["HierarchyCode"];
            }
            if (columnsName.IndexOf("HierarchyLevel") >= 0
                && !reader.IsDBNull(reader.GetOrdinal("HierarchyLevel")))
            {
                item.HierarchyLevel = (string)reader["HierarchyLevel"];
            }
            if (columnsName.IndexOf("IsPreDefined") >= 0
                && !reader.IsDBNull(reader.GetOrdinal("IsPreDefined")))
            {
                item.IsPredefined = reader["IsPreDefined"].ToString() == "1";
            }
            if (columnsName.IndexOf("CreatedDate") >= 0
                && !reader.IsDBNull(reader.GetOrdinal("CreatedDate")))
            {
                DateTime dt;
                if (DateTime.TryParse(reader["CreatedDate"].ToString(), out dt))
                    item.CreatedDate = dt;
            }
            return item;
        }

        public void UpdateAssignableHierarchy(AssignableHierarchyModel assignableHierarchyModel)
        {
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_UpdAssignableHierarchy");
            SecurityDatabase.AddInParameter(command, "@HierarchyId", DbType.Int32, assignableHierarchyModel.HierarchyId);
            SecurityDatabase.AddInParameter(command, "@AssignableHierarchyId", DbType.Int32, assignableHierarchyModel.AssignHierarchyId);
            SecurityDatabase.AddInParameter(command, "@IsRemove", DbType.Boolean, assignableHierarchyModel.IsRemove);
            SecurityDatabase.AddInParameter(command, "@UserID", DbType.AnsiString, assignableHierarchyModel.UserId);
            SecurityDatabase.AddInParameter(command, "@ASClient", DbType.Int32, assignableHierarchyModel.AsClientId);
            SecurityDatabase.AddInParameter(command, "@SiteID", DbType.Int32, assignableHierarchyModel.SiteId);
            SecurityDatabase.AddInParameter(command, "@ChangedByRecID", DbType.Guid, assignableHierarchyModel.ChangedByRecId);
            SecurityDatabase.AddInParameter(command, "@IsUpdate", DbType.Boolean, assignableHierarchyModel.IsUpdate);
            base.ExecuteNonQuery(command);
        }

        public HierarchyCollection GetAssignableHierarchy(int hierarchyId, string userType)
        {
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_GetHierarchyAssignableByHierarchy");
            SecurityDatabase.AddInParameter(command, "@HierarchyId", DbType.Int32, hierarchyId);
            SecurityDatabase.AddInParameter(command, "@UserType", DbType.String, userType);
            HierarchyCollection hierarchyCollection = new HierarchyCollection();
            using (IDataReader reader = base.ExecuteReader(command))
            {
                while (reader.Read())
                {
                    Hierarchy hierarchy = BindHierarchy(reader);
                    hierarchyCollection.Add(hierarchy);
                }
                reader.Close();
            }
            return hierarchyCollection;

        }

        public HierarchyCollection GetMSAssignableHierarchy(int clientId,
            string userId, string hierarchyLevel, string hierarchyCode, int systemId)
        {
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_GetMSHierarchyAssignableByHierarchy");
            SecurityDatabase.AddInParameter(command, "@ASClient", DbType.Int32, clientId);
            SecurityDatabase.AddInParameter(command, "@UserId", DbType.String, userId);
            SecurityDatabase.AddInParameter(command, "@HierarchyLevel", DbType.String, hierarchyLevel);
            SecurityDatabase.AddInParameter(command, "@HierarchyCode", DbType.String, hierarchyCode);
            SecurityDatabase.AddInParameter(command, "@SystemId", DbType.Int32, systemId);
            HierarchyCollection hierarchyCollection = new HierarchyCollection();
            using (IDataReader reader = base.ExecuteReader(command))
            {
                while (reader.Read())
                {
                    Hierarchy hierarchy = BindHierarchy(reader);
                    hierarchyCollection.Add(hierarchy);
                }
                reader.Close();
            }
            return hierarchyCollection;
        }

        public DataTable GetHierarchyAssignableListByHierarchy(int hierarchyId)
        {
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_GetHierarchyAssignableListByHierarchy");
            SecurityDatabase.AddInParameter(command, "@HierarchyId", DbType.Int32, hierarchyId);
            return base.ExecuteDataSet(command).Tables[0];
        }
        public HierarchyCollection GetHierarchy(int clientId, string username, int systemId)
        {
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_GetHierarchysForUser");
            SecurityDatabase.AddInParameter(command, "@ASClient", DbType.Int32, clientId);
            SecurityDatabase.AddInParameter(command, "@UserName", DbType.AnsiString, username);
            SecurityDatabase.AddInParameter(command, "@SystemId", DbType.Int32, systemId);
            HierarchyCollection hierarchyCollection = new HierarchyCollection();
            using (IDataReader reader = base.ExecuteReader(command))
            {
                while (reader.Read())
                {
                    Hierarchy hierarchy = BindHierarchy(reader);
                    hierarchyCollection.Add(hierarchy);
                }
                reader.Close();
            }
            return hierarchyCollection;

        }

        ///Description for GetHierarchyByHierarchyID
        ///Author:
        ///Date: 
        public Hierarchy GetHierarchyByHierarchyID(int hierarchyID)
        {
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_GetHierarchyById");
            SecurityDatabase.AddInParameter(command, "@HierarchyID", DbType.Int32, hierarchyID);
            Hierarchy hierarchy = null;
            using (IDataReader reader = base.ExecuteReader(command))
            {
                if (reader.Read())
                {
                    hierarchy = BindHierarchy(reader);
                    reader.Close();
                }
            }
            return hierarchy;
        }

        ///Description for GetHierarchyByHierarchyName
        ///Author:
        ///Date: 
        public HierarchyCollection GetHierarchyByHierarchyName(string hierarchyName, int clientId, int systemId)
        {
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_GetHierarchyByName");
            SecurityDatabase.AddInParameter(command, "@HierarchyName", DbType.AnsiString, hierarchyName);
            SecurityDatabase.AddInParameter(command, "@ClientId", DbType.Int32, clientId);
            SecurityDatabase.AddInParameter(command, "@SystemId", DbType.Int32, systemId);
            HierarchyCollection hierarchyCollection = new HierarchyCollection();
            using (IDataReader reader = base.ExecuteReader(command))
            {
                if (reader.Read())
                {
                    Hierarchy hierarchy = BindHierarchy(reader);
                    hierarchyCollection.Add(hierarchy);
                    reader.Close();
                }
            }
            return hierarchyCollection;
        }

        ///Description for DeleteHierarchy
        ///Author:
        ///Date: 
        public void DeleteHierarchy(int hierarchyID)
        {


            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_DelHierarchy");
            SecurityDatabase.AddInParameter(command, "@HierarchyID", DbType.Int32, hierarchyID);
            base.ExecuteNonQuery(command);
        }

        public int InsertHierarchy(HierarchyModel hierarchyModel)
        {
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_InsHierarchy");
            SecurityDatabase.AddInParameter(command, "@SystemId", DbType.Int32, hierarchyModel.SystemId);
            SecurityDatabase.AddInParameter(command, "@HierarchyName", DbType.AnsiString, hierarchyModel.HierarchyName);
            SecurityDatabase.AddInParameter(command, "@HierarchyParent", DbType.Int32, hierarchyModel.ParentHierarchy);
            SecurityDatabase.AddInParameter(command, "@HierarchyDescription", DbType.AnsiString, hierarchyModel.HierarchyDesc);
            SecurityDatabase.AddInParameter(command, "@ActvStatus", DbType.AnsiString, hierarchyModel.Status);
            SecurityDatabase.AddInParameter(command, "@CreatedBy", DbType.Guid, hierarchyModel.CreatedBy);
            SecurityDatabase.AddInParameter(command, "@ClientId", DbType.Int32, hierarchyModel.ClientId);
            SecurityDatabase.AddInParameter(command, "@HierarchyCode", DbType.AnsiString, hierarchyModel.HierarchyCode);
            SecurityDatabase.AddInParameter(command, "@HierarchyLevel", DbType.AnsiString, hierarchyModel.HierarchyLevel);

            DbParameter param = command.CreateParameter();
            param.Direction = ParameterDirection.InputOutput;
            param.Value = hierarchyModel.HierarchyId;
            param.DbType = DbType.Int32;
            param.ParameterName = "@HierarchyID";
            command.Parameters.Add(param);
            base.ExecuteNonQuery(command);
            return (int)SecurityDatabase.GetParameterValue(command, "@HierarchyID");
        }

        public int UpdateHierarchy(HierarchyModel hierarchyModel)
        {
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_UpdHierarchy");
            SecurityDatabase.AddInParameter(command, "@SystemId", DbType.Int32, hierarchyModel.SystemId);
            SecurityDatabase.AddInParameter(command, "@HierarchyID", DbType.Int32, hierarchyModel.HierarchyId);
            SecurityDatabase.AddInParameter(command, "@HierarchyName", DbType.AnsiString, hierarchyModel.HierarchyName);
            SecurityDatabase.AddInParameter(command, "@HierarchyParent", DbType.Int32, hierarchyModel.ParentHierarchy);
            SecurityDatabase.AddInParameter(command, "@HierarchyDescription", DbType.AnsiString, hierarchyModel.HierarchyDesc);
            SecurityDatabase.AddInParameter(command, "@ActvStatus", DbType.AnsiString, hierarchyModel.Status);
            SecurityDatabase.AddInParameter(command, "@ClientId", DbType.Int32, hierarchyModel.ClientId);
            SecurityDatabase.AddInParameter(command, "@HierarchyCode", DbType.AnsiString, hierarchyModel.HierarchyCode);
            SecurityDatabase.AddInParameter(command, "@HierarchyLevel", DbType.AnsiString, hierarchyModel.HierarchyLevel);
            SecurityDatabase.AddInParameter(command, "@UpdatedBy", DbType.Guid, hierarchyModel.UpdatedBy);
            SecurityDatabase.AddParameter(command, "@RETURN_VALUE", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Default, null);
            base.ExecuteNonQuery(command);
            return (int)SecurityDatabase.GetParameterValue(command, "@RETURN_VALUE");
        }

        public void UpdateChildHierarchyPermission(int hierarchyID)
        {

            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_UpdatePermissionsForChildHierarchy");
            SecurityDatabase.AddInParameter(command, "@HierarchyId", DbType.Int32, hierarchyID);
            base.ExecuteNonQuery(command);
        }

        public HierarchyCollection GetHierarchyTreeForUser(int clientId, string username, int systemId)
        {
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_GetAvailableHierarchy");
            SecurityDatabase.AddInParameter(command, "@ASClient", DbType.Int32, clientId);
            SecurityDatabase.AddInParameter(command, "@UserName", DbType.AnsiString, username);
            SecurityDatabase.AddInParameter(command, "@SystemId", DbType.Int32, systemId);
            SecurityDatabase.AddInParameter(command, "@IsEnvironment", DbType.Int32, 2);

            HierarchyCollection hierarchyCollection = new HierarchyCollection();
            using (IDataReader reader = base.ExecuteReader(command))
            {
                while (reader.Read())
                {
                    Hierarchy hierarchy = BindHierarchy(reader);
                    hierarchyCollection.Add(hierarchy);
                }
                reader.Close();
            }
            return hierarchyCollection;
        }

        public HierarchyAccessCollection GetSiteJumpAccessByHierarchy(int hierarchyId, string type)
        {
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_GetSiteJumpAccessByHierarchy");
            SecurityDatabase.AddInParameter(command, "@HierarchyId", DbType.Int32, hierarchyId);
            SecurityDatabase.AddInParameter(command, "@Type", DbType.AnsiString, type);

            HierarchyAccessCollection hierarchyAccessCollection = new HierarchyAccessCollection();

            using (IDataReader reader = base.ExecuteReader(command))
            {
                while (reader.Read())
                {
                    HierarchyAccess hierarchyAccess = BindHierarchyAccess(reader);
                    hierarchyAccessCollection.Add(hierarchyAccess);
                }
                reader.Close();
            }
            return hierarchyAccessCollection;

        }

        public void UpdateSiteJumpAccess(int hierarchyId, string login, string createdBy, string type, bool isRemove)
        {
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_UpdSiteJumpAccess");
            SecurityDatabase.AddInParameter(command, "@HierarchyId", DbType.Int32, hierarchyId);

            SecurityDatabase.AddInParameter(command, "@Login", DbType.AnsiString, login);
            SecurityDatabase.AddInParameter(command, "@Type", DbType.AnsiString, type);
            SecurityDatabase.AddInParameter(command, "@CreatedBy", DbType.AnsiString, createdBy);
            SecurityDatabase.AddInParameter(command, "@IsRemove", DbType.Boolean, isRemove);
            base.ExecuteNonQuery(command);

        }

        /// <summary>
        /// Delete permission out of PermissionInHierarchy table.
        /// </summary>
        /// <param name="permissionId"></param>
        /// <param name="hierarchyID"></param>
        public void DeletePermissionFromHierarchy(int permissionId, int hierarchyID)
        {

            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_DelPermissionFromHierarchy");
            SecurityDatabase.AddInParameter(command, "@PermissionId", DbType.Int32, permissionId);
            SecurityDatabase.AddInParameter(command, "@HierarchyId", DbType.Int32, hierarchyID);
            base.ExecuteNonQuery(command);
        }

        /// <summary>
        /// Delete permission out of PermissionInHierarchy table.
        /// </summary>
        /// <param name="permissionCode"></param>
        /// <param name="hierarchyID"></param>
        public void DeletePermissionFromHierarchy(string permissionCode, int hierarchyID, int clientID, Guid updatedBy, bool isLogged)
        {

            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_DelPermissionCodeFromHierarchy");
            SecurityDatabase.AddInParameter(command, "@PermissionCode", DbType.AnsiString, permissionCode);
            SecurityDatabase.AddInParameter(command, "@HierarchyId", DbType.Int32, hierarchyID);
            SecurityDatabase.AddInParameter(command, "@ClientID", DbType.Int32, clientID);
            SecurityDatabase.AddInParameter(command, "@UpdatedBy", DbType.Guid, updatedBy);
            SecurityDatabase.AddInParameter(command, "@IsLogged", DbType.Boolean, isLogged);
            base.ExecuteNonQuery(command);
        }
        /// <summary>
        /// Insert a permission into a Hierarchy (PermissionInHierarchy table).
        /// </summary>
        /// <param name="permissionId"></param>
        /// <param name="hierarchyID"></param>
        public void InsertPermissionIntoHierarchy(int permissionId, int hierarchyID)
        {
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_InsPermissionInHierarchy");
            SecurityDatabase.AddInParameter(command, "@PermissionId", DbType.Int32, permissionId);
            SecurityDatabase.AddInParameter(command, "@HierarchyId", DbType.Int32, hierarchyID);
            base.ExecuteNonQuery(command);

        }

        /// <summary>
        /// Insert a permission into a Hierarchy (PermissionInHierarchy table).
        /// </summary>
        /// <param name="permissionCode"></param>
        /// <param name="hierarchyID"></param>
        public void InsertPermissionIntoHierarchy(string permissionCode, int hierarchyID, Guid updatedBy, bool isLogged)
        {
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_InsPermissionCodeInHierarchy");
            SecurityDatabase.AddInParameter(command, "@PermissionCode", DbType.AnsiString, permissionCode);
            SecurityDatabase.AddInParameter(command, "@HierarchyId", DbType.Int32, hierarchyID);
            SecurityDatabase.AddInParameter(command, "@UpdatedBy", DbType.Guid, updatedBy);
            SecurityDatabase.AddInParameter(command, "@IsLogged", DbType.Boolean, isLogged);
            base.ExecuteNonQuery(command);

        }

        public bool CheckUniqueHierarchyForUpdate(int clientId, string hierarchyName, int hierarchyId)
        {
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_CheckUniqueHierarchyForUpdate");
            SecurityDatabase.AddInParameter(command, "@ClientId", DbType.Int32, clientId);
            SecurityDatabase.AddInParameter(command, "@HierarchyName", DbType.AnsiString, hierarchyName);
            SecurityDatabase.AddInParameter(command, "@HierarchyId", DbType.Int32, hierarchyId);

            SecurityDatabase.AddParameter(command, "@RETURN_VALUE", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Default, null);
            base.ExecuteNonQuery(command);
            return (int)SecurityDatabase.GetParameterValue(command, "@RETURN_VALUE") == 1;

        }
        public bool CheckUniqueHierarchyForCreate(int clientId, string hierarchyName)
        {
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_CheckUniqueHierarchyForCreate");
            SecurityDatabase.AddInParameter(command, "@ClientId", DbType.Int32, clientId);
            SecurityDatabase.AddInParameter(command, "@HierarchyName", DbType.AnsiString, hierarchyName);
            SecurityDatabase.AddParameter(command, "@RETURN_VALUE", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Default, null);
            base.ExecuteNonQuery(command);
            return (int)SecurityDatabase.GetParameterValue(command, "@RETURN_VALUE") == 1;

        }
        public void CreateUpdateRiskGroupForUser(int clientId, Guid userid, int groupid)
        {
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_CreateUpdateRiskGroupForUser");
            SecurityDatabase.AddInParameter(command, "@ASClientID", DbType.Int32, clientId);
            SecurityDatabase.AddInParameter(command, "@UserId", DbType.Guid, userid);
            SecurityDatabase.AddInParameter(command, "@GroupID", DbType.Int32, groupid);
            base.ExecuteNonQuery(command);
        }

        public int GetRiskGroupForUser(int clientId, Guid userid)
        {
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_GetRiskGroupForUser");
            SecurityDatabase.AddInParameter(command, "@ASClientID", DbType.Int32, clientId);
            SecurityDatabase.AddInParameter(command, "@UserId", DbType.Guid, userid);
            SecurityDatabase.AddParameter(command, "@RETURN_VALUE", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Default, null);
            base.ExecuteNonQuery(command);
            return (int)SecurityDatabase.GetParameterValue(command, "@RETURN_VALUE");
        }

        public HierarchyAccess BindHierarchyAccess(IDataReader reader)
        {
            List<String> columnsName = new List<String>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                columnsName.Add(reader.GetName(i));
            }
            HierarchyAccess item = new HierarchyAccess();

            if (columnsName.IndexOf("Sys") >= 0 && !reader.IsDBNull(reader.GetOrdinal("Sys")))
                item.Sys = (string)reader["Sys"];

            if (columnsName.IndexOf("Prin") >= 0 && !reader.IsDBNull(reader.GetOrdinal("Prin")))
                item.Prin = (string)reader["Prin"];

            if (columnsName.IndexOf("AgentNumber") >= 0 && !reader.IsDBNull(reader.GetOrdinal("AgentNumber")))
                item.Agent = (string)reader["AgentNumber"];

            if (columnsName.IndexOf("EntityNumber") >= 0 && !reader.IsDBNull(reader.GetOrdinal("EntityNumber")))
                item.EntityNumber = (string)reader["EntityNumber"];

            if (columnsName.IndexOf("DisplayText") >= 0 && !reader.IsDBNull(reader.GetOrdinal("DisplayText")))
                item.DisplayText = (string)reader["DisplayText"];

            if (columnsName.IndexOf("Login") >= 0 && !reader.IsDBNull(reader.GetOrdinal("Login")))
                item.Login = (string)reader["Login"];

            if (columnsName.IndexOf("ClientName") >= 0 && !reader.IsDBNull(reader.GetOrdinal("ClientName")))
                item.ClientName = (string)reader["ClientName"];

            return item;
        }

        public HierarchyAccessCollection GetHierarchyAccessByHierarchy(int hierarchyId, string type)
        {
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_GetHierarchyAccessByHierarchy");
            SecurityDatabase.AddInParameter(command, "@HierarchyId", DbType.Int32, hierarchyId);
            SecurityDatabase.AddInParameter(command, "@Type", DbType.AnsiString, type);

            HierarchyAccessCollection hierarchyAccessCollection = new HierarchyAccessCollection();

            using (IDataReader reader = base.ExecuteReader(command))
            {
                while (reader.Read())
                {
                    HierarchyAccess hierarchyAccess = BindHierarchyAccess(reader);
                    hierarchyAccessCollection.Add(hierarchyAccess);
                }
                reader.Close();
            }
            return hierarchyAccessCollection;

        }
        public void UpdateHierarchyAccess(int hierarchyId, string entityNumber, string createdBy, string type, bool isRemove)
        {
            string[] numbers = new string[3];
            numbers[0] = numbers[1] = numbers[2] = "ALL";

            if (entityNumber != "ALL")
            {
                numbers = entityNumber.Split('-');
            }


            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_UpdHierarchyAccess");
            SecurityDatabase.AddInParameter(command, "@HierarchyId", DbType.Int32, hierarchyId);

            SecurityDatabase.AddInParameter(command, "@EntityNumber", DbType.AnsiString, entityNumber);
            SecurityDatabase.AddInParameter(command, "@Type", DbType.AnsiString, type);
            SecurityDatabase.AddInParameter(command, "@UserIDCreate", DbType.AnsiString, createdBy);


            if (numbers[1] != null)
            {
                SecurityDatabase.AddInParameter(command, "@Sys", DbType.AnsiString, numbers[0]);
            }
            else
            {
                SecurityDatabase.AddInParameter(command, "@Sys", DbType.AnsiString, DBNull.Value);
            }

            if (numbers[1] != null)
            {
                SecurityDatabase.AddInParameter(command, "@Prin", DbType.AnsiString, numbers[1]);
            }
            else
            {
                SecurityDatabase.AddInParameter(command, "@Prin", DbType.AnsiString, DBNull.Value);
            }

            if (numbers.Length == 3 && numbers[2] != null)
            {
                SecurityDatabase.AddInParameter(command, "@Agent", DbType.AnsiString, numbers[2]);
            }
            else
            {
                SecurityDatabase.AddInParameter(command, "@Agent", DbType.AnsiString, DBNull.Value);
            }
            SecurityDatabase.AddInParameter(command, "@IsRemove", DbType.Boolean, isRemove);
            base.ExecuteNonQuery(command);

        }

    }
}