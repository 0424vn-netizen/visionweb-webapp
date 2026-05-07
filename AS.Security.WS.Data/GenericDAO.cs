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
    public class GenericDao : BaseSecDao
    {
        public GenericDao(string connString) : base(connString) { }

        public DataTable GetReports(string spName, FilterParameterCollection paramaters)
        {
            DbCommand command = SecurityDatabase.GetStoredProcCommand(spName);


            foreach (FilterParameter param in paramaters)
            {
                SecurityDatabase.AddInParameter(command, param.ParameterName, (DbType)Enum.ToObject(typeof(DbType), param.ParameterType), param.ParameterValue);
            }

            DataSet dtsResult = base.ExecuteDataSet(command);
            if (dtsResult != null && dtsResult.Tables.Count > 0) return dtsResult.Tables[0];
            return null;
        }
        public RefTableValue BindRefTableValues(IDataReader reader)
        {
            RefTableValue item = new RefTableValue();
            if (!reader.IsDBNull(reader.GetOrdinal("ASClientID"))) item.ASClient = (int)reader["ASClientID"];
            if (!reader.IsDBNull(reader.GetOrdinal("RefTblName"))) item.RefTblName = (string)reader["RefTblName"];
            if (!reader.IsDBNull(reader.GetOrdinal("RefTblKey"))) item.RefTblKey = (string)reader["RefTblKey"];
            if (!reader.IsDBNull(reader.GetOrdinal("RefTblKey1"))) item.RefTblKey1 = (string)reader["RefTblKey1"];
            if (!reader.IsDBNull(reader.GetOrdinal("RefTblLang"))) item.RefTblLang = (string)reader["RefTblLang"];
            if (!reader.IsDBNull(reader.GetOrdinal("EffStartDTS"))) item.EffStartDTS = (DateTime)reader["EffStartDTS"];
            if (!reader.IsDBNull(reader.GetOrdinal("EffEndDTS"))) item.EffEndDTS = (DateTime)reader["EffEndDTS"];
            if (!reader.IsDBNull(reader.GetOrdinal("ActvStat"))) item.ActvStatus = (string)reader["ActvStat"];
            for (int i = 0; i < 9; i++)
            {
                if (!reader.IsDBNull(reader.GetOrdinal("RefTblCol" + (i + 1).ToString()))) item.RefTblCols[i] = (string)reader["RefTblCol" + (i + 1).ToString()];
            }            
            return item;
        }

        public RefTableValueCollection GetRefTableValues(int clientId, string refTableName, string refTableLang, int LanguageID)
        {
            
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_GetRefTableValues");
            SecurityDatabase.AddInParameter(command, "@ASClientID", DbType.Int32, clientId);
            SecurityDatabase.AddInParameter(command, "@RefTableName", DbType.String, refTableName);
            SecurityDatabase.AddInParameter(command, "@RefTblLang", DbType.String, refTableLang);
            SecurityDatabase.AddInParameter(command, "@LanguageID", DbType.Int32, LanguageID);
            RefTableValueCollection refValues = new RefTableValueCollection();
            RefTableValue refValue = null;
            using (IDataReader reader = base.ExecuteReader(command))
            {
                while (reader.Read())
                {
                    refValue = BindRefTableValues(reader);
                    refValues.Add(refValue);
                }
                reader.Close();
            }

            return refValues;
        }

        public ASTheme BindASThemeObject(IDataReader reader)
        {
            ASTheme item = new ASTheme();
            if (!reader.IsDBNull(reader.GetOrdinal("ThemeId"))) item.ThemeId = (int)reader["ThemeId"];
            if (!reader.IsDBNull(reader.GetOrdinal("ThemeName"))) item.ThemeName = (string)reader["ThemeName"];
            if (!reader.IsDBNull(reader.GetOrdinal("ThemeImage"))) item.ThemeImage = (string)reader["ThemeImage"];
            if (!reader.IsDBNull(reader.GetOrdinal("Description"))) item.Description = (string)reader["Description"];            
            if (!reader.IsDBNull(reader.GetOrdinal("DateCreated"))) item.DateCreated = (DateTime)reader["DateCreated"];            
            if (!reader.IsDBNull(reader.GetOrdinal("ActvStatus"))) item.ActvStatus = (string)reader["ActvStatus"];
            
            return item;
        }


        public ASThemeCollection GetASThemesByUser(int clientID, string username, int hierarchyid)
        {
            
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_GetThemeOfUser");
            SecurityDatabase.AddInParameter(command, "@ASClient", DbType.Int32, clientID);
            SecurityDatabase.AddInParameter(command, "@UserName", DbType.AnsiString,username);
            SecurityDatabase.AddInParameter(command, "@HierarchyId", DbType.Int32,hierarchyid);
            ASThemeCollection Themes = new ASThemeCollection();
            ASTheme theme = null;
            using (IDataReader reader = base.ExecuteReader(command))
            {
                while (reader.Read())
                {
                    theme = BindASThemeObject(reader);
                    Themes.Add(theme);
                }
                reader.Close();
            }

            return Themes;
        }
        public void InsertUserInTheme(int clientID, string userName, int hierarchyId, int themeId)
        {

            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_InsUserInTheme");
            SecurityDatabase.AddInParameter(command, "@ASClient", DbType.Int32, clientID);
            SecurityDatabase.AddInParameter(command, "@UserName", DbType.AnsiString, userName);
            SecurityDatabase.AddInParameter(command, "@HierarchyId", DbType.Int32, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@ThemeId", DbType.Int32, themeId);

            base.ExecuteNonQuery(command);
        }
        public void InsertLOG_WebServerSessionStats(string webSiteIPAddress, string webSiteClient, string webSiteType, string webSiteGroup, string webSiteDNS)
        {
            
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_LogWebServerSessionUpdates");

            SecurityDatabase.AddInParameter(command, "@mode", DbType.AnsiString, "iss");
            SecurityDatabase.AddInParameter(command, "@SessionEndDTS", DbType.DateTime, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@SessionID", DbType.AnsiString, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@UserIPAddress", DbType.AnsiString, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@WebSiteActiveUsers", DbType.Int32, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@WebSiteClient", DbType.AnsiString, webSiteClient);
            SecurityDatabase.AddInParameter(command, "@WebSiteDNS", DbType.AnsiString, webSiteDNS);
            SecurityDatabase.AddInParameter(command, "@WebSiteGroup", DbType.AnsiString, webSiteGroup);
            SecurityDatabase.AddInParameter(command, "@WebSiteIPAddress", DbType.AnsiString, webSiteIPAddress);
            SecurityDatabase.AddInParameter(command, "@WebSiteMaxUsers", DbType.Int32, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@WebSiteType", DbType.AnsiString, webSiteType);
            SecurityDatabase.AddInParameter(command, "@UserID", DbType.AnsiString, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@BrowserType", DbType.AnsiString, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@HttpCookie", DbType.AnsiString, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@PrintSQL", DbType.Boolean, false);


            

            base.ExecuteNonQuery(command);
        }
        public void UpdateLOG_WebServerSessionStats(string webSiteIPAddress, int webSiteMaxUsers, int webSiteActiveUsers)
        {
            
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_LogWebServerSessionUpdates");
            SecurityDatabase.AddInParameter(command, "@mode", DbType.AnsiString, "uss");
            SecurityDatabase.AddInParameter(command, "@SessionEndDTS", DbType.DateTime, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@SessionID", DbType.AnsiString, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@UserIPAddress", DbType.AnsiString, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@WebSiteActiveUsers", DbType.Int32, webSiteActiveUsers);
            SecurityDatabase.AddInParameter(command, "@WebSiteClient", DbType.AnsiString, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@WebSiteDNS", DbType.AnsiString, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@WebSiteGroup", DbType.AnsiString, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@WebSiteIPAddress", DbType.AnsiString, webSiteIPAddress);
            SecurityDatabase.AddInParameter(command, "@WebSiteMaxUsers", DbType.Int32, webSiteMaxUsers);
            SecurityDatabase.AddInParameter(command, "@WebSiteType", DbType.AnsiString, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@UserID", DbType.AnsiString, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@BrowserType", DbType.AnsiString, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@HttpCookie", DbType.AnsiString, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@PrintSQL", DbType.Boolean, false);





            base.ExecuteNonQuery(command);
        }
        public void DeleteLOG_WebServerSessionStats(string webSiteIPAddress)
        {
            
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_LogWebServerSessionUpdates");

            SecurityDatabase.AddInParameter(command, "@mode", DbType.AnsiString, "dss");
            SecurityDatabase.AddInParameter(command, "@SessionEndDTS", DbType.DateTime, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@SessionID", DbType.AnsiString, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@UserIPAddress", DbType.AnsiString, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@WebSiteActiveUsers", DbType.Int32, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@WebSiteClient", DbType.AnsiString, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@WebSiteDNS", DbType.AnsiString, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@WebSiteGroup", DbType.AnsiString, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@WebSiteIPAddress", DbType.AnsiString, webSiteIPAddress);
            SecurityDatabase.AddInParameter(command, "@WebSiteMaxUsers", DbType.Int32, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@WebSiteType", DbType.AnsiString, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@UserID", DbType.AnsiString, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@BrowserType", DbType.AnsiString, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@HttpCookie", DbType.AnsiString, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@PrintSQL", DbType.Boolean, false);
            base.ExecuteNonQuery(command);
        }

        public void DeleteLOG_WebServerSessionLog(string webSiteIPAddress, string sessionID)
        {
            
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_LogWebServerSessionUpdates");
            SecurityDatabase.AddInParameter(command, "@mode", DbType.AnsiString, "dsl");
            SecurityDatabase.AddInParameter(command, "@SessionEndDTS", DbType.DateTime, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@SessionID", DbType.AnsiString, sessionID);
            SecurityDatabase.AddInParameter(command, "@UserIPAddress", DbType.AnsiString, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@WebSiteActiveUsers", DbType.Int32, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@WebSiteClient", DbType.AnsiString, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@WebSiteDNS", DbType.AnsiString, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@WebSiteGroup", DbType.AnsiString, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@WebSiteIPAddress", DbType.AnsiString, webSiteIPAddress);
            SecurityDatabase.AddInParameter(command, "@WebSiteMaxUsers", DbType.Int32, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@WebSiteType", DbType.AnsiString, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@UserID", DbType.AnsiString, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@BrowserType", DbType.AnsiString, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@HttpCookie", DbType.AnsiString, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@PrintSQL", DbType.Boolean, false);




            base.ExecuteNonQuery(command);
        }

        public void InsertLOG_WebServerSessionLog(string webSiteIPAddress, string sessionID, string userIPAddress, string browserType, string httpCookie)
        {
            
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_LogWebServerSessionUpdates");
            SecurityDatabase.AddInParameter(command, "@mode", DbType.AnsiString, "isl");
            SecurityDatabase.AddInParameter(command, "@SessionEndDTS", DbType.DateTime, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@SessionID", DbType.AnsiString, sessionID);
            SecurityDatabase.AddInParameter(command, "@UserIPAddress", DbType.AnsiString, userIPAddress);
            SecurityDatabase.AddInParameter(command, "@WebSiteActiveUsers", DbType.Int32, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@WebSiteClient", DbType.AnsiString, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@WebSiteDNS", DbType.AnsiString, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@WebSiteGroup", DbType.AnsiString, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@WebSiteIPAddress", DbType.AnsiString, webSiteIPAddress);
            SecurityDatabase.AddInParameter(command, "@WebSiteMaxUsers", DbType.Int32, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@WebSiteType", DbType.AnsiString, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@UserID", DbType.AnsiString, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@BrowserType", DbType.AnsiString, browserType);
            SecurityDatabase.AddInParameter(command, "@HttpCookie", DbType.AnsiString, httpCookie);
            SecurityDatabase.AddInParameter(command, "@PrintSQL", DbType.Boolean, false);




            base.ExecuteNonQuery(command);
        }

        public void UpdateLOG_WebServerSessionLog(string webSiteIPAddress, string sessionID, string userIPAddress, string browserType, string httpCookie)
        {
            
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_LogWebServerSessionUpdates");
            SecurityDatabase.AddInParameter(command, "@mode", DbType.AnsiString, "uslu");
            SecurityDatabase.AddInParameter(command, "@SessionEndDTS", DbType.DateTime, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@SessionID", DbType.AnsiString, sessionID);
            SecurityDatabase.AddInParameter(command, "@UserIPAddress", DbType.AnsiString, userIPAddress);
            SecurityDatabase.AddInParameter(command, "@WebSiteActiveUsers", DbType.Int32, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@WebSiteClient", DbType.AnsiString, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@WebSiteDNS", DbType.AnsiString, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@WebSiteGroup", DbType.AnsiString, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@WebSiteIPAddress", DbType.AnsiString, webSiteIPAddress);
            SecurityDatabase.AddInParameter(command, "@WebSiteMaxUsers", DbType.Int32, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@WebSiteType", DbType.AnsiString, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@UserID", DbType.AnsiString, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@BrowserType", DbType.AnsiString, browserType);
            SecurityDatabase.AddInParameter(command, "@HttpCookie", DbType.AnsiString, httpCookie);
            SecurityDatabase.AddInParameter(command, "@PrintSQL", DbType.Boolean, false);




            base.ExecuteNonQuery(command);
        }

        public void UpdateLOG_WebServerSessionLogEnd(string sessionID)
        {
            
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_LogWebServerSessionUpdates");
            SecurityDatabase.AddInParameter(command, "@mode", DbType.AnsiString, "usle");
            SecurityDatabase.AddInParameter(command, "@SessionEndDTS", DbType.DateTime, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@SessionID", DbType.AnsiString, sessionID);
            SecurityDatabase.AddInParameter(command, "@UserIPAddress", DbType.AnsiString, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@WebSiteActiveUsers", DbType.Int32, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@WebSiteClient", DbType.AnsiString, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@WebSiteDNS", DbType.AnsiString, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@WebSiteGroup", DbType.AnsiString, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@WebSiteIPAddress", DbType.AnsiString, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@WebSiteMaxUsers", DbType.Int32, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@WebSiteType", DbType.AnsiString, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@UserID", DbType.AnsiString, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@BrowserType", DbType.AnsiString, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@HttpCookie", DbType.AnsiString, DBNull.Value);
            SecurityDatabase.AddInParameter(command, "@PrintSQL", DbType.Boolean, false);




            base.ExecuteNonQuery(command);
        }
        
    }
}
