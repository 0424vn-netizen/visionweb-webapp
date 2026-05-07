using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using AS.Security.WS.Entities;

namespace AS.Security.WS.Data
{
    public class MembershipDao : BaseSecDao
    {
        ///Description for BindUser
        ///Author:
        ///Date:  
        public MembershipDao(string connString) : base(connString) { }

        public User BindUser(IDataReader reader)
        {
            List<string> columnsName = new List<string>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                columnsName.Add(reader.GetName(i));
            }

            User item = new User();

            //user info
            GetUserInfo(reader, columnsName, item);

            //security
            GetUserSecurityInfo(reader, columnsName, item);

            //role
            GetUserRoleInfo(reader, columnsName, item);

            //login info
            GetLoginInfo(reader, columnsName, item);

            //addition info
            GetUserAdditionInfo(reader, columnsName, item);

            return item;
        }

        public void GetUserInfo(IDataReader reader, List<string> columnsName, User item)
        {
            if (columnsName != null && item != null)
            {
                // user info
                if (columnsName.IndexOf("RecId") >= 0 && !reader.IsDBNull(reader.GetOrdinal("RecId")))
                    item.RecId = (Guid)reader["RecId"];

                if (columnsName.IndexOf("ASClient") >= 0 && !reader.IsDBNull(reader.GetOrdinal("ASClient")))
                    item.ASClient = (int)reader["ASClient"];

                if (columnsName.IndexOf("SiteID") >= 0 && !reader.IsDBNull(reader.GetOrdinal("SiteID")))
                    item.SiteID = (int)reader["SiteID"];

                if (columnsName.IndexOf("UserID") >= 0 && !reader.IsDBNull(reader.GetOrdinal("UserID")))
                    item.UserID = (string)reader["UserID"];

                if (columnsName.IndexOf("OriginalUserID") >= 0 && !reader.IsDBNull(reader.GetOrdinal("OriginalUserID")))
                    item.OriginalUserID = (string)reader["OriginalUserID"];

                if (columnsName.IndexOf("UserNameFirst") >= 0 && !reader.IsDBNull(reader.GetOrdinal("UserNameFirst")))
                    item.UserNameFirst = (string)reader["UserNameFirst"];

                GetUserInfoDetail(reader, columnsName, item);
            }
        }
        private void GetUserInfoDetail(IDataReader reader, List<string> columnsName, User item)
        {
            if (columnsName != null && item != null)
            {
                if (columnsName.IndexOf("UserNameLast") >= 0 && !reader.IsDBNull(reader.GetOrdinal("UserNameLast")))
                    item.UserNameLast = (string)reader["UserNameLast"];

                if (columnsName.IndexOf("UserNameFull") >= 0 && !reader.IsDBNull(reader.GetOrdinal("UserNameFull")))
                    item.UserNameFull = (string)reader["UserNameFull"];

                if (columnsName.IndexOf("CreatedDTS") >= 0 && !reader.IsDBNull(reader.GetOrdinal("CreatedDTS")))
                    item.CreatedDTS = (DateTime)reader["CreatedDTS"];

                if (columnsName.IndexOf("UpdatedDTS") >= 0 && !reader.IsDBNull(reader.GetOrdinal("UpdatedDTS")))
                    item.UpdatedDTS = (DateTime)reader["UpdatedDTS"];

                if (columnsName.IndexOf("email") >= 0 && !reader.IsDBNull(reader.GetOrdinal("email")))
                    item.Email = (string)reader["email"];

                if (columnsName.IndexOf("PhoneNumber") >= 0 && !reader.IsDBNull(reader.GetOrdinal("PhoneNumber")))
                    item.PhoneForSMS = (string)reader["PhoneNumber"];

                if (columnsName.IndexOf("NotificationEmail") >= 0 && !reader.IsDBNull(reader.GetOrdinal("NotificationEmail")))
                    item.ContactEmail = (string)reader["NotificationEmail"];
            }
        }
        public void GetUserSecurityInfo(IDataReader reader, List<string> columnsName, User item)
        {
            if (columnsName != null && item != null)
            {
                if (columnsName.IndexOf("UserPassword") >= 0 && !reader.IsDBNull(reader.GetOrdinal("UserPassword")))
                    item.UserPassword = (string)reader["UserPassword"];

                if (columnsName.IndexOf("UserPasswordType") >= 0 && !reader.IsDBNull(reader.GetOrdinal("UserPasswordType")))
                    item.UserPasswordType = (int)reader["UserPasswordType"];

                if (columnsName.IndexOf("UserPasswordDTS") >= 0 && !reader.IsDBNull(reader.GetOrdinal("UserPasswordDTS")))
                    item.UserPasswordDTS = (DateTime)reader["UserPasswordDTS"];

                if (columnsName.IndexOf("UserPasswordTmp") >= 0 && !reader.IsDBNull(reader.GetOrdinal("UserPasswordTmp")))
                    item.UserPasswordTmp = (string)reader["UserPasswordTmp"];

                if (columnsName.IndexOf("UserPasswordTmpType") >= 0 && !reader.IsDBNull(reader.GetOrdinal("UserPasswordTmpType")))
                    item.UserPasswordTmpType = (int)reader["UserPasswordTmpType"];

                if (columnsName.IndexOf("UserPasswordTmpDTS") >= 0 && !reader.IsDBNull(reader.GetOrdinal("UserPasswordTmpDTS")))
                    item.UserPasswordTmpDTS = (DateTime)reader["UserPasswordTmpDTS"];

                GetUserSecurityInfoDetail(reader, columnsName, item);
            }
        }
        private void GetUserSecurityInfoDetail(IDataReader reader, List<string> columnsName, User item)
        {
            if (columnsName != null && item != null)
            {
                if (columnsName.IndexOf("LoginAttempts") >= 0 && !reader.IsDBNull(reader.GetOrdinal("LoginAttempts")))
                    item.LoginAttempts = (int)reader["LoginAttempts"];

                if (columnsName.IndexOf("LastLoginDTS") >= 0 && !reader.IsDBNull(reader.GetOrdinal("LastLoginDTS")))
                    item.LastLoginDTS = (DateTime)reader["LastLoginDTS"];

                if (columnsName.IndexOf("PrevLoginDTS") >= 0 && !reader.IsDBNull(reader.GetOrdinal("PrevLoginDTS")))
                    item.PrevLoginDTS = (DateTime)reader["PrevLoginDTS"];

                if (columnsName.IndexOf("LoginQuestionIndex") >= 0 && !reader.IsDBNull(reader.GetOrdinal("LoginQuestionIndex")))
                    item.LoginQuestionIndex = (int)reader["LoginQuestionIndex"];

                if (columnsName.IndexOf("LoginQuestionAnswer") >= 0 && !reader.IsDBNull(reader.GetOrdinal("LoginQuestionAnswer")))
                    item.LoginQuestionAnswer = (string)reader["LoginQuestionAnswer"];
            }
        }
        public void GetUserRoleInfo(IDataReader reader, List<string> columnsName, User item)
        {
            if (columnsName != null && item != null)
            {
                if (columnsName.IndexOf("UserSecRole") >= 0 && !reader.IsDBNull(reader.GetOrdinal("UserSecRole")))
                    item.UserSecRole = (string)reader["UserSecRole"];

                if (columnsName.IndexOf("SuperUser") >= 0 && !reader.IsDBNull(reader.GetOrdinal("SuperUser")))
                    item.SuperUser = (string)reader["SuperUser"];

                if (columnsName.IndexOf("PCISuperUser") >= 0 && !reader.IsDBNull(reader.GetOrdinal("PCISuperUser")))
                    item.PCISuperUser = (string)reader["PCISuperUser"];

                if (columnsName.IndexOf("ASCSUser") >= 0 && !reader.IsDBNull(reader.GetOrdinal("ASCSUser")))
                    item.ASCSUser = (string)reader["ASCSUser"];

                if (columnsName.IndexOf("UserAccsType") >= 0 && !reader.IsDBNull(reader.GetOrdinal("UserAccsType")))
                    item.UserAccsType = (string)reader["UserAccsType"];

                if (columnsName.IndexOf("UserAccsLevel") >= 0 && !reader.IsDBNull(reader.GetOrdinal("UserAccsLevel")))
                    item.UserAccsLevel = (string)reader["UserAccsLevel"];

                if (columnsName.IndexOf("ActvStat") >= 0 && !reader.IsDBNull(reader.GetOrdinal("ActvStat")))
                    item.ActvStat = (string)reader["ActvStat"];

                GetUserRoleInfoDetail(reader, columnsName, item);
            }
        }
        public void GetUserRoleInfoDetail(IDataReader reader, List<string> columnsName, User item)
        {
            if (columnsName != null && item != null)
            {
                if (columnsName.IndexOf("EntityID") >= 0 && !reader.IsDBNull(reader.GetOrdinal("EntityID")))
                    item.EntityID = (string)reader["EntityID"];

                if (columnsName.IndexOf("EntityType") >= 0 && !reader.IsDBNull(reader.GetOrdinal("EntityType")))
                    item.EntityType = (int)reader["EntityType"];

                if (columnsName.IndexOf("InitialID") >= 0 && reader.IsDBNull(reader.GetOrdinal("InitialID")))
                    item.InitialID = (string)reader["InitialID"];

                if (columnsName.IndexOf("UserType") >= 0 && !reader.IsDBNull(reader.GetOrdinal("UserType")))
                    item.UserType = (int)reader["UserType"];

                if (columnsName.IndexOf("SalesRepCode") >= 0 && !reader.IsDBNull(reader.GetOrdinal("SalesRepCode")))
                    item.SalesRepCode = reader["SalesRepCode"].ToString();
            }
        }
        public void GetLoginInfo(IDataReader reader, List<string> columnsName, User item)
        {
            if (columnsName != null && item != null)
            {
                if (columnsName.IndexOf("LoginsM01") >= 0 && !reader.IsDBNull(reader.GetOrdinal("LoginsM01")))
                    item.LoginsM01 = (int)reader["LoginsM01"];

                if (columnsName.IndexOf("LoginsM02") >= 0 && !reader.IsDBNull(reader.GetOrdinal("LoginsM02")))
                    item.LoginsM02 = (int)reader["LoginsM02"];

                if (columnsName.IndexOf("LoginsM03") >= 0 && !reader.IsDBNull(reader.GetOrdinal("LoginsM03")))
                    item.LoginsM03 = (int)reader["LoginsM03"];

                if (columnsName.IndexOf("LoginsM04") >= 0 && !reader.IsDBNull(reader.GetOrdinal("LoginsM04")))
                    item.LoginsM04 = (int)reader["LoginsM04"];

                if (columnsName.IndexOf("LoginsM05") >= 0 && !reader.IsDBNull(reader.GetOrdinal("LoginsM05")))
                    item.LoginsM05 = (int)reader["LoginsM05"];

                if (columnsName.IndexOf("LoginsM06") >= 0 && !reader.IsDBNull(reader.GetOrdinal("LoginsM06")))
                    item.LoginsM06 = (int)reader["LoginsM06"];

                if (columnsName.IndexOf("LoginsM07") >= 0 && !reader.IsDBNull(reader.GetOrdinal("LoginsM07")))
                    item.LoginsM07 = (int)reader["LoginsM07"];

                GetLoginInfoDetail(reader, columnsName, item);
            }
        }
        public void GetLoginInfoDetail(IDataReader reader, List<string> columnsName, User item)
        {
            if (columnsName != null && item != null)
            {
                if (columnsName.IndexOf("LoginsM08") >= 0 && !reader.IsDBNull(reader.GetOrdinal("LoginsM08")))
                    item.LoginsM08 = (int)reader["LoginsM08"];

                if (columnsName.IndexOf("LoginsM09") >= 0 && !reader.IsDBNull(reader.GetOrdinal("LoginsM09")))
                    item.LoginsM09 = (int)reader["LoginsM09"];

                if (columnsName.IndexOf("LoginsM10") >= 0 && !reader.IsDBNull(reader.GetOrdinal("LoginsM10")))
                    item.LoginsM10 = (int)reader["LoginsM10"];

                if (columnsName.IndexOf("LoginsM11") >= 0 && !reader.IsDBNull(reader.GetOrdinal("LoginsM11")))
                    item.LoginsM11 = (int)reader["LoginsM11"];

                if (columnsName.IndexOf("LoginsM12") >= 0 && !reader.IsDBNull(reader.GetOrdinal("LoginsM12")))
                    item.LoginsM12 = (int)reader["LoginsM12"];

                if (columnsName.IndexOf("LoginsYTD") >= 0 && !reader.IsDBNull(reader.GetOrdinal("LoginsYTD")))
                    item.LoginsYTD = (int)reader["LoginsYTD"];

                if (columnsName.IndexOf("LoginsPriorYear") >= 0 && !reader.IsDBNull(reader.GetOrdinal("LoginsPriorYear")))
                    item.LoginsPriorYear = (int)reader["LoginsPriorYear"];
            }
        }
        public void GetUserAdditionInfo(IDataReader reader, List<string> columnsName, User item)
        {
            if (columnsName != null && item != null)
            {
                if (columnsName.IndexOf("ClientIPAddr") >= 0 && !reader.IsDBNull(reader.GetOrdinal("ClientIPAddr")))
                    item.ClientIPAddr = (string)reader["ClientIPAddr"];

                if (columnsName.IndexOf("HostIPAddr") >= 0 && !reader.IsDBNull(reader.GetOrdinal("HostIPAddr")))
                    item.HostIPAddr = (string)reader["HostIPAddr"];

                if (columnsName.IndexOf("BrowserType") >= 0 && !reader.IsDBNull(reader.GetOrdinal("BrowserType")))
                    item.BrowserType = (string)reader["BrowserType"];

                if (columnsName.IndexOf("ActvStatDTS") >= 0 && !reader.IsDBNull(reader.GetOrdinal("ActvStatDTS")))
                    item.ActvStatDTS = (DateTime)reader["ActvStatDTS"];

                GetUserAdditionInfoDetail(reader, columnsName, item);
            }
        }
        public void GetUserAdditionInfoDetail(IDataReader reader, List<string> columnsName, User item)
        {
            if (columnsName != null && item != null)
            {
                if (columnsName.IndexOf("fto_status") >= 0 && !reader.IsDBNull(reader.GetOrdinal("fto_status")))
                    item.Fto_status = (byte)reader["fto_status"];

                if (columnsName.IndexOf("fto_CreateDate") >= 0 && !reader.IsDBNull(reader.GetOrdinal("fto_CreateDate")))
                    item.Fto_CreateDate = (DateTime)reader["fto_CreateDate"];

                if (columnsName.IndexOf("TandCStat") >= 0 && !reader.IsDBNull(reader.GetOrdinal("TandCStat")))
                    item.TandCStat = (string)reader["TandCStat"];

                if (columnsName.IndexOf("TandCStatDTS") >= 0 && !reader.IsDBNull(reader.GetOrdinal("TandCStatDTS")))
                    item.TandCStatDTS = (DateTime)reader["TandCStatDTS"];
            }
        }
        public int ValidateUserID(ValidateUserModel validateUserModel)
        {
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_ValidateASUserAndLog");
            SecurityDatabase.AddInParameter(command, "@ASClient", DbType.Int32, validateUserModel.ClientId);
            SecurityDatabase.AddInParameter(command, "@UserName", DbType.AnsiString, validateUserModel.UserName);
            SecurityDatabase.AddInParameter(command, "@UserPassword", DbType.AnsiString, validateUserModel.BarePassword);
            SecurityDatabase.AddInParameter(command, "@ASPLog_ClientIPAddr", DbType.AnsiString, validateUserModel.RemoteIp);
            SecurityDatabase.AddInParameter(command, "@ASPLog_HostIPAddr", DbType.AnsiString, validateUserModel.HostIp);
            SecurityDatabase.AddInParameter(command, "@ASPLog_BrowserType", DbType.AnsiString, validateUserModel.BrowserType);
            SecurityDatabase.AddInParameter(command, "@LoginSessionID", DbType.AnsiString, validateUserModel.LogSessionID);
            SecurityDatabase.AddInParameter(command, "@AspSessionID", DbType.AnsiString, validateUserModel.Sessionid);

            return int.Parse(base.ExecuteScalar(command).ToString());
        }
        public int ChangePassword(int clientid, string userName, string oldBarePassword, string newBarePassword, string logSessionID)
        {

            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_ChangeUserPassword");
            SecurityDatabase.AddInParameter(command, "@ASClient", DbType.AnsiString, clientid);
            SecurityDatabase.AddInParameter(command, "@UserName", DbType.AnsiString, userName);
            SecurityDatabase.AddInParameter(command, "@OldPassword", DbType.AnsiString, oldBarePassword);
            SecurityDatabase.AddInParameter(command, "@NewPassword", DbType.AnsiString, newBarePassword);
            SecurityDatabase.AddInParameter(command, "@LogSessionID", DbType.String, logSessionID);

            DataSet _dts = base.ExecuteDataSet(command);
            if (_dts != null && _dts.Tables.Count > 0)
            {
                return Int32.Parse(_dts.Tables[0].Rows[0]["ErrorFlag"].ToString());
            }
            return 0;
        }

        public bool ResetPassword(int clientid, string userName, int passwordType, string newBarePassword)
        {

            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_ResetUserPassword");
            SecurityDatabase.AddInParameter(command, "@ASClient", DbType.AnsiString, clientid);
            SecurityDatabase.AddInParameter(command, "@UserName", DbType.AnsiString, userName);
            SecurityDatabase.AddInParameter(command, "@UserPasswordType", DbType.Int32, passwordType);
            SecurityDatabase.AddInParameter(command, "@NewPassword", DbType.AnsiString, newBarePassword);
            return base.ExecuteNonQuery(command) > 0;

        }

        public int GetUserLanguageID(Guid recID, int asClient, int siteID)
        {
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_GetUserLanguageID");
            SecurityDatabase.AddInParameter(command, "@RecId", DbType.Guid, recID);
            SecurityDatabase.AddInParameter(command, "@ASClient", DbType.Int32, asClient);
            SecurityDatabase.AddInParameter(command, "@SiteID", DbType.Int32, siteID);
            int languageID = 1;

            object ret = base.ExecuteScalar(command);
            if (!(ret is DBNull) && (ret != null) && !string.IsNullOrEmpty(ret.ToString()))
                languageID = int.Parse(ret.ToString());
            return languageID;
        }

        public bool UpdateUserLanguageID(Guid recID, int asClient, int siteID, int languageID)
        {
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_UpdateUserLanguageID");
            SecurityDatabase.AddInParameter(command, "@RecId", DbType.Guid, recID);
            SecurityDatabase.AddInParameter(command, "@ASClient", DbType.Int32, asClient);
            SecurityDatabase.AddInParameter(command, "@SiteID", DbType.Int32, siteID);
            SecurityDatabase.AddInParameter(command, "@languageID", DbType.Int32, languageID);
            return base.ExecuteNonQuery(command) > 0;
        }

        public User GetUserID(int ddsClient, string userName)
        {


            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_GetUsers");
            SecurityDatabase.AddInParameter(command, "@ASClient", DbType.Int32, ddsClient);
            SecurityDatabase.AddInParameter(command, "@UserName", DbType.AnsiString, userName);
            User userIDs = null;
            using (IDataReader reader = base.ExecuteReader(command))
            {
                if (reader.Read())
                {
                    userIDs = BindUser(reader);

                }
                reader.Close();
            }
            return userIDs;

        }

        ///Description for InsertMSUser
        ///Author:
        ///Date: 
        public void InsertMSUser(UserInfoModel userInfo)
        {
            if (userInfo != null)
            {
                DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_CreateMSUser");
                GetCreateUserParams(userInfo, command);
                SecurityDatabase.AddInParameter(command, "@CreatedBy", DbType.Guid, userInfo.CreatedBy);
                SecurityDatabase.AddInParameter(command, "@SiteID", DbType.Int32, userInfo.SiteId);
                SecurityDatabase.AddInParameter(command, "@EntityID", DbType.String, userInfo.EntityId);
                SecurityDatabase.AddInParameter(command, "@UserTypeMode", DbType.AnsiString, userInfo.UserTypeMode);
                base.ExecuteNonQuery(command);
            }            
        }

        ///Description for InsertUser
        ///Author:
        ///Date: 
        public void InsertUser(UserInfoModel userInfo)
        {
            if (userInfo != null)
            {
                DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_CreateASUser");
                GetCreateUserParams(userInfo, command);
                SecurityDatabase.AddInParameter(command, "@SalesRepCode", DbType.String, userInfo.SalesRepCode);
                SecurityDatabase.AddInParameter(command, "@OrganizationIDs ", DbType.String, userInfo.Organizations);
                
                object intValue = null;
                if (userInfo.UserType >= 0) 
                    intValue = userInfo.UserType; 
                else 
                    intValue = DBNull.Value;

                SecurityDatabase.AddInParameter(command, "@UserType", DbType.Int32, intValue);
                SecurityDatabase.AddInParameter(command, "@CreatedBy", DbType.Guid, userInfo.CreatedBy);
                base.ExecuteNonQuery(command);
            }            
        }
        private string GetHierarchyInput(string[] hierarchyIds)
        {
            if (hierarchyIds != null && hierarchyIds.Length > 0)
                return string.Join(",", hierarchyIds);

            return string.Empty;
        }
        private void GetCreateUserParams(UserInfoModel userInfo, DbCommand command)
        {
            var hierarchyIds = GetHierarchyInput(userInfo.HierarchyIds);
            SecurityDatabase.AddInParameter(command, "@ASClient", DbType.Int32, userInfo.ClientId);
            SecurityDatabase.AddOutParameter(command, "@UserId", DbType.Guid, 50);
            SecurityDatabase.AddInParameter(command, "@UserName", DbType.AnsiString, userInfo.UserName);
            SecurityDatabase.AddInParameter(command, "@UserNameFirst", DbType.AnsiString, userInfo.UserNameFirst);
            SecurityDatabase.AddInParameter(command, "@UserNameLast", DbType.AnsiString, userInfo.UserNameLast);
            SecurityDatabase.AddInParameter(command, "@UserNameFull", DbType.AnsiString, userInfo.UserNameFull);
            SecurityDatabase.AddInParameter(command, "@UserPassword", DbType.AnsiString, userInfo.UserPassword);
            SecurityDatabase.AddInParameter(command, "@UserPasswordType", DbType.Int32, userInfo.UserPasswordType);

            SecurityDatabase.AddInParameter(command, "@Email", DbType.AnsiString, userInfo.Email);
            SecurityDatabase.AddInParameter(command, "@LoginQuestionIndex", DbType.Int32, userInfo.LoginQuestionIndex);
            SecurityDatabase.AddInParameter(command, "@LoginQuestionAnswer", DbType.AnsiString, userInfo.LoginQuestionAnswer);
            SecurityDatabase.AddInParameter(command, "@ActiveStatus", DbType.AnsiString, userInfo.Status);
            SecurityDatabase.AddInParameter(command, "@HierarchyIds", DbType.String, hierarchyIds);
            SecurityDatabase.AddInParameter(command, "@UserSecRole", DbType.String, userInfo.UserSecRole);
        }

        ///Description for UpdateUser
        ///Author:
        ///Date: 
        public void UpdateUser(UpdateUserInfoModel userInfo)
        {
            if (userInfo != null)
            {
                var command = GetUpdateUserParams(userInfo, false);
                base.ExecuteNonQuery(command);
            }                       
        }

        ///Description for UpdateUser: Update user force change UserID
        ///Author: QuocLe
        ///Date: 11/30/2017
        public void UpdateUserForceUpdateUserName(UpdateUserInfoModel userInfo)
        {
            if (userInfo != null)
            {
                var command = GetUpdateUserParams(userInfo, true);
                base.ExecuteNonQuery(command);
            }
        }
        private DbCommand GetUpdateUserParams(UpdateUserInfoModel userInfo, bool isForceUpdateUserName)
        {
            var hierarchy = GetHierarchyInput(userInfo.HierarchyIds);
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_UpdateASUser");
            SecurityDatabase.AddInParameter(command, "@ASClient", DbType.Int32, userInfo.ClientId);
            SecurityDatabase.AddInParameter(command, "@SystemID", DbType.Int32, userInfo.SystemId);
            SecurityDatabase.AddInParameter(command, "@UserName", DbType.AnsiString, userInfo.UserName);
            SecurityDatabase.AddInParameter(command, "@OriginalUserID", DbType.AnsiString, userInfo.OriginalUserId);

            SecurityDatabase.AddInParameter(command, "@UserNameFirst", DbType.AnsiString, userInfo.UserNameFirst);
            SecurityDatabase.AddInParameter(command, "@UserNameLast", DbType.AnsiString, userInfo.UserNameLast);
            SecurityDatabase.AddInParameter(command, "@UserNameFull", DbType.AnsiString, userInfo.UserNameFull);
            SecurityDatabase.AddInParameter(command, "@email", DbType.AnsiString, userInfo.Email);
            SecurityDatabase.AddInParameter(command, "@PhoneNumber", DbType.AnsiString, userInfo.PhoneForSMS);
            SecurityDatabase.AddInParameter(command, "@NotificationEmail", DbType.AnsiString, userInfo.ContactEmail);
            SecurityDatabase.AddInParameter(command, "@LoginQuestionIndex", DbType.Int32, userInfo.LoginQuestionIndex);
            SecurityDatabase.AddInParameter(command, "@LoginQuestionAnswer", DbType.AnsiString, userInfo.LoginQuestionAnswer);
            SecurityDatabase.AddInParameter(command, "@LoginQuestionAnswer_OriginalValue", DbType.AnsiString, userInfo.LoginQuestionAnswerOriginalValue);
            SecurityDatabase.AddInParameter(command, "@LoginQuestionAnswer_NewValue", DbType.AnsiString, userInfo.LoginQuestionAnswerNewValue);
            SecurityDatabase.AddInParameter(command, "@ActvStatus", DbType.String, userInfo.Status);
            SecurityDatabase.AddInParameter(command, "@UserType", DbType.Int32, userInfo.UserType);
            SecurityDatabase.AddInParameter(command, "@HierarchyIds", DbType.String, hierarchy);
            SecurityDatabase.AddInParameter(command, "@UpdatedBy", DbType.Guid, userInfo.UpdatedBy);
            SecurityDatabase.AddInParameter(command, "@SalesRepCode", DbType.String, userInfo.SalesRepCode);
            SecurityDatabase.AddInParameter(command, "@OrganizationIDs ", DbType.String, userInfo.Organizations);
            
            if (isForceUpdateUserName)
                SecurityDatabase.AddInParameter(command, "@IsUpdateUserName ", DbType.Boolean, userInfo.IsUpdateUserName);
            if (userInfo.RecId != Guid.Empty)
                SecurityDatabase.AddInParameter(command, "@UserId", DbType.Guid, userInfo.RecId);

            return command;
        }
        public bool IsExistedUserName(int clientId, string username)
        {

            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_CheckUserName");
            SecurityDatabase.AddInParameter(command, "@ClientID", DbType.Int32, clientId);
            SecurityDatabase.AddInParameter(command, "@UserName", DbType.AnsiString, username);
            using (IDataReader reader = base.ExecuteReader(command))
            {
                if (reader.Read())
                    return true;
                else return false;

            }
        }

        public UserCollection GetSecondaryUsers(string userName, int clientId)
        {
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_GetSecondaryUsers");
            SecurityDatabase.AddInParameter(command, "@ASClient", DbType.Int32, clientId);
            SecurityDatabase.AddInParameter(command, "@UserName", DbType.AnsiString, userName);
            UserCollection userIDsCollection = new UserCollection();
            using (IDataReader reader = base.ExecuteReader(command))
            {
                while (reader.Read())
                {
                    User userIDs = BindUser(reader);
                    userIDsCollection.Add(userIDs);
                }
                reader.Close();
            }
            return userIDsCollection;
        }

        ///Description for BindCSUser
        ///Author:HoangDinh
        ///Date:12/1/2008
        public User BindCSUser(IDataReader reader)
        {
            User item = new User();
            if (!reader.IsDBNull(reader.GetOrdinal("UserID"))) item.UserID = (string)reader["UserID"];
            if (!reader.IsDBNull(reader.GetOrdinal("UserNameFirst"))) item.UserNameFirst = (string)reader["UserNameFirst"];
            if (!reader.IsDBNull(reader.GetOrdinal("UserNameLast"))) item.UserNameLast = (string)reader["UserNameLast"];
            if (!reader.IsDBNull(reader.GetOrdinal("Email"))) item.Email = (string)reader["Email"];
            if (!reader.IsDBNull(reader.GetOrdinal("HierarchyName"))) item.HierarchyName = (string)reader["HierarchyName"];
            if (!reader.IsDBNull(reader.GetOrdinal("ActiveStatus"))) item.ActvStat = (string)reader["ActiveStatus"];
            if (!reader.IsDBNull(reader.GetOrdinal("UserType"))) item.UserType = (int)reader["UserType"];
            if (!reader.IsDBNull(reader.GetOrdinal("SiteID"))) item.SiteID = (int)reader["SiteID"];
            if (!reader.IsDBNull(reader.GetOrdinal("ASClient"))) item.ASClient = (int)reader["ASClient"];

            return item;
        }

        ///Description for GetCSUserListByHierarchyAccess
        ///Author:HoangDinh
        ///Date:12/1/2008
        public UserCollection GetCSUserListByHierarchyAccess(int hierarchyID)
        {
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_GetCSUserListByHierarchyID");
            SecurityDatabase.AddInParameter(command, "@HierarchyID", DbType.Int32, hierarchyID);
            UserCollection userCollection = new UserCollection();
            using (IDataReader reader = base.ExecuteReader(command))
            {
                while (reader.Read())
                {
                    User user = BindCSUser(reader);
                    userCollection.Add(user);
                }
                reader.Close();
            }
            return userCollection;

        }
        public UserCollection GetUsersForCS(string userList, string type)
        {
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_GetUsersForCS");
            SecurityDatabase.AddInParameter(command, "@UserList", DbType.AnsiString, userList);
            SecurityDatabase.AddInParameter(command, "@Type", DbType.AnsiString, type);
            UserCollection userCollection = new UserCollection();
            using (IDataReader reader = base.ExecuteReader(command))
            {
                while (reader.Read())
                {
                    User user = BindUser(reader);
                    userCollection.Add(user);
                }
                reader.Close();
            }
            return userCollection;

        }

        #region Email And Forgot Password FNMS
        public bool UpdateEmailAndConfirm(string userName, string email, string valCode)
        {
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_UpdateEmail");
            SecurityDatabase.AddInParameter(command, "@UserID", DbType.AnsiString, userName);
            SecurityDatabase.AddInParameter(command, "@Email", DbType.AnsiString, email);
            SecurityDatabase.AddInParameter(command, "@ValidateCode", DbType.AnsiString, valCode);
            SecurityDatabase.AddParameter(command, "@RETURN_VALUE", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Default, null);
            base.ExecuteNonQuery(command);
            return (int)SecurityDatabase.GetParameterValue(command, "@RETURN_VALUE") == 1;

        }
        public byte GetCurrentEmailStatus(string userName)
        {
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_GetCurrentEmailStatus");
            SecurityDatabase.AddInParameter(command, "@UserID", DbType.AnsiString, userName);
            object ret = base.ExecuteScalar(command);
            if (ret is DBNull) ret = (byte)0;
            return (byte)ret;

        }
        public int CheckEmailConfirm(string userName, string valCode)
        {
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_CheckEmailConfirm");
            SecurityDatabase.AddInParameter(command, "@UserID", DbType.AnsiString, userName);
            SecurityDatabase.AddInParameter(command, "@ValidateCode", DbType.AnsiString, valCode);
            SecurityDatabase.AddParameter(command, "@RETURN_VALUE", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Default, null);
            base.ExecuteNonQuery(command);
            return (int)SecurityDatabase.GetParameterValue(command, "@Return_Value");

        }

        public bool CreateForgotPwdTicket(string userName, string valCode)
        {
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_CreateForgotPwdTicket");
            SecurityDatabase.AddInParameter(command, "@UserID", DbType.AnsiString, userName);
            SecurityDatabase.AddInParameter(command, "@ValidateCode", DbType.AnsiString, valCode);
            SecurityDatabase.AddParameter(command, "@RETURN_VALUE", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Default, null);
            base.ExecuteNonQuery(command);
            return (int)SecurityDatabase.GetParameterValue(command, "@Return_Value") == 1;
        }
        public int CheckForgotPwdTicket(string userName, string valCode)
        {
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_CheckForgotPwdTicket");
            SecurityDatabase.AddInParameter(command, "@UserID", DbType.AnsiString, userName);
            SecurityDatabase.AddInParameter(command, "@ValidateCode", DbType.AnsiString, valCode);
            SecurityDatabase.AddParameter(command, "@RETURN_VALUE", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Default, null);
            base.ExecuteNonQuery(command);
            return (int)SecurityDatabase.GetParameterValue(command, "@Return_Value");

        }

        public bool IsUserLockedOut(string userName)
        {
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_IsUserLockedOut");
            SecurityDatabase.AddInParameter(command, "@UserID", DbType.AnsiString, userName);
            SecurityDatabase.AddParameter(command, "@RETURN_VALUE", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Default, null);
            base.ExecuteNonQuery(command);
            return (int)SecurityDatabase.GetParameterValue(command, "@Return_Value") == 1;
        }
        public bool LockOutUser(int clientId, string userName, string logSessionID)
        {
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_LockOutUser");
            SecurityDatabase.AddInParameter(command, "@ASClient", DbType.Int32, clientId);
            SecurityDatabase.AddInParameter(command, "@UserID", DbType.AnsiString, userName);
            SecurityDatabase.AddInParameter(command, "@LogSessionID", DbType.String, logSessionID);
            return base.ExecuteNonQuery(command) > 0;
        }

        public void UpdateForgotPassword(int clientId, string userName, bool isSuccess)
        {
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_UpdateForgotPassword");
            SecurityDatabase.AddInParameter(command, "@ASClient", DbType.Int32, clientId);
            SecurityDatabase.AddInParameter(command, "@UserName", DbType.AnsiString, userName);
            SecurityDatabase.AddInParameter(command, "@IsSuccess", DbType.Boolean, isSuccess);
            base.ExecuteNonQuery(command);
        }

        public bool IsForgotLockedOut(int clientId, string userName)
        {
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_IsForgotLockedOut");
            SecurityDatabase.AddInParameter(command, "@ASClient", DbType.Int32, clientId);
            SecurityDatabase.AddInParameter(command, "@UserID", DbType.AnsiString, userName);
            SecurityDatabase.AddParameter(command, "@RETURN_VALUE", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Default, null);
            base.ExecuteNonQuery(command);
            return (int)SecurityDatabase.GetParameterValue(command, "@Return_Value") == 1;
        }
        #endregion


        public bool HasUserOfHierarchy(int hierarchyID)
        {
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_HasUserOfHierarchy");
            SecurityDatabase.AddInParameter(command, "@hierarchyID", DbType.Int16, hierarchyID);
            SecurityDatabase.AddParameter(command, "@RETURN_VALUE", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Default, null);
            base.ExecuteNonQuery(command);
            return (int)SecurityDatabase.GetParameterValue(command, "@Return_Value") == 1;
        }

        public bool DeleteASUser(int clientId, string userId)
        {
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_DeleteASUser");
            SecurityDatabase.AddInParameter(command, "@ASClient", DbType.Int32, clientId);
            SecurityDatabase.AddInParameter(command, "@UserID", DbType.AnsiString, userId);
            SecurityDatabase.ExecuteNonQuery(command);
            return true;
        }



    }
}
