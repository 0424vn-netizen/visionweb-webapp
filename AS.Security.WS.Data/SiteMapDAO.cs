using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;

using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using AS.Security.WS.Entities;
using AS.Common.DBManager;

namespace AS.Security.WS.Data
{
    public class SiteMapDao : BaseSecDao
    {
        ///Description for BindSiteMap
        ///Author:
        ///Date:  
        public SiteMapDao(string connString) : base(connString) { }

        public SecMenuItem BindObject(IDataReader reader)
        {
            SecMenuItem item = new SecMenuItem();
            if (!reader.IsDBNull(reader.GetOrdinal("SiteMapId"))) item.SiteMapId = (int)reader["SiteMapId"];
            if (!reader.IsDBNull(reader.GetOrdinal("SystemId"))) item.SystemId = (int)reader["SystemId"];
            if (!reader.IsDBNull(reader.GetOrdinal("Url"))) item.Url = (string)reader["Url"];
            if (!reader.IsDBNull(reader.GetOrdinal("Title"))) item.Title = (string)reader["Title"];
            if (!reader.IsDBNull(reader.GetOrdinal("Description"))) item.Description = (string)reader["Description"];
            if (!reader.IsDBNull(reader.GetOrdinal("PermissionCodes"))) item.Permissions = (string)reader["PermissionCodes"];
            if (!reader.IsDBNull(reader.GetOrdinal("Parent"))) item.Parent = (int)reader["Parent"];
            if (!reader.IsDBNull(reader.GetOrdinal("NodeOrder"))) item.NodeOrder = (int)reader["NodeOrder"];
            if (!reader.IsDBNull(reader.GetOrdinal("DisplayType"))) item.DisplayType = (string)reader["DisplayType"];
            return item;
        }


        ///Description for GetSiteMap
        ///Author:
        ///Date: 
        public SecMenuItemCollection GetMenuItemsByUser(int clientID, string username, int hierarchyid, int languageID, bool manageMode)
        {


            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_GetMenuItemForUser");
            SecurityDatabase.AddInParameter(command, "@ClientId", DbType.Int32, clientID);
            SecurityDatabase.AddInParameter(command, "@UserName", DbType.AnsiString, username);
            SecurityDatabase.AddInParameter(command, "@HierarchyId", DbType.Int32, hierarchyid);
            SecurityDatabase.AddInParameter(command, "@LanguageId", DbType.Int32, languageID);
            SecurityDatabase.AddInParameter(command, "@IsReskin", DbType.Boolean, true);
            SecurityDatabase.AddInParameter(command, "@ManageMode", DbType.Boolean, manageMode);

            SecMenuItemCollection MenuItems = new SecMenuItemCollection();
            using (IDataReader reader = base.ExecuteReader(command))
            {
                while (reader.Read())
                {
                    SecMenuItem menuItem = BindObject(reader);
                    MenuItems.Add(menuItem);
                }
                reader.Close();
            }
            return MenuItems;

        }

        public SecMenuItemCollection GetMenuItemsByUserSSO(int clientID, string username, int hierarchyid, int ssoLevel, int languageID, bool manageMode)
        {
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_GetMenuItemForUser");
            SecurityDatabase.AddInParameter(command, "@ClientId", DbType.Int32, clientID);
            SecurityDatabase.AddInParameter(command, "@UserName", DbType.AnsiString, username);
            SecurityDatabase.AddInParameter(command, "@HierarchyId", DbType.Int32, hierarchyid);
            SecurityDatabase.AddInParameter(command, "@SSOSecurityLevelID", DbType.Int32, ssoLevel);
            SecurityDatabase.AddInParameter(command, "@LanguageId", DbType.Int32, languageID);
            SecurityDatabase.AddInParameter(command, "@IsReskin", DbType.Boolean, true);
            SecurityDatabase.AddInParameter(command, "@ManageMode", DbType.Boolean, manageMode);

            SecMenuItemCollection MenuItems = new SecMenuItemCollection();
            using (IDataReader reader = base.ExecuteReader(command))
            {
                while (reader.Read())
                {
                    SecMenuItem menuItem = BindObject(reader);
                    MenuItems.Add(menuItem);
                }
                reader.Close();
            }
            return MenuItems;
        }

        public SecMenuItemCollection GetMSMenuItems(int clientId, string hierarchyLevel,
            string hierarchyCode, int languageId)
        {
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_GetMSMenuItem");
            SecurityDatabase.AddInParameter(command, "@ClientId", DbType.Int32, clientId);
            SecurityDatabase.AddInParameter(
                command, "@HierarchyLevel", DbType.AnsiString, hierarchyLevel);
            SecurityDatabase.AddInParameter(
                command, "@HierarchyCode", DbType.AnsiString, hierarchyCode);
			SecurityDatabase.AddInParameter(command, "@LanguageId", DbType.Int32, languageId);
            SecurityDatabase.AddInParameter(command, "@IsReskin", DbType.Boolean, true);

            SecMenuItemCollection MenuItems = new SecMenuItemCollection();
            using (IDataReader reader = base.ExecuteReader(command))
            {
                while (reader.Read())
                {
                    SecMenuItem menuItem = BindObject(reader);
                    MenuItems.Add(menuItem);
                }
                reader.Close();
            }
            return MenuItems;
        }

        public SecMenuItemCollection GetMenuItemForCreateRole(int clientID, string username, int systemId, string group)
        {


            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_GetMenuItemForCreateRole");
            SecurityDatabase.AddInParameter(command, "@ClientId", DbType.Int32, clientID);
            SecurityDatabase.AddInParameter(command, "@UserName", DbType.AnsiString, username);
            SecurityDatabase.AddInParameter(command, "@SystemId", DbType.Int32, systemId);
            SecurityDatabase.AddInParameter(command, "@Group", DbType.AnsiString, group);
            SecMenuItemCollection MenuItems = new SecMenuItemCollection();
            using (IDataReader reader = base.ExecuteReader(command))
            {
                while (reader.Read())
                {
                    SecMenuItem menuItem = BindObject(reader);
                    MenuItems.Add(menuItem);
                }
                reader.Close();
            }
            return MenuItems;

        }

        public void InsertExcludeMenuForUser(int clientID, string username, int menuId, DateTime startDate, DateTime endDate)
        {

            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_InsExcludeMenuItem");
            SecurityDatabase.AddInParameter(command, "@ClientId", DbType.Int32, clientID);
            SecurityDatabase.AddInParameter(command, "@UserName", DbType.AnsiString, username);
            SecurityDatabase.AddInParameter(command, "@MenuId", DbType.Int32, menuId);
            SecurityDatabase.AddInParameter(command, "@EffStartDate", DbType.DateTime, startDate);
            SecurityDatabase.AddInParameter(command, "@EffEndDate", DbType.DateTime, endDate);
            base.ExecuteNonQuery(command);

        }
        public void DeleteExcludeMenuForUser(int clientID, string username, int menuId)
        {

            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_DelExcludeMenuItem");
            SecurityDatabase.AddInParameter(command, "@ClientId", DbType.Int32, clientID);
            SecurityDatabase.AddInParameter(command, "@UserName", DbType.AnsiString, username);
            SecurityDatabase.AddInParameter(command, "@MenuId", DbType.Int32, menuId);
            base.ExecuteNonQuery(command);

        }
    }
}
