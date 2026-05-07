using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using Microsoft.Web.Services3.Security.Tokens;
using FilterParamWS = AS.Security.Web.SecurityServices.SecService.FilterParameter;
using AS.Security.WS.Entities;
using ASUser = AS.Security.WS.Entities.User;
using ASUserCollection = AS.Security.WS.Entities.UserCollection;
using AS.Common.WebUI;
using AS.Common.DBManager;
using AS.Common.DataProtection;
using AS.Security.WS.Entities.Utility;

namespace AS.Security.Web.SecurityServices
{
    public class SecurityService : ASReportBusiness, ICryptor
    {
        private readonly SecService.SecurityService _Service = new SecService.SecurityService();
        readonly string _url = null;
        public int ClientId { get; set; }

        public SecurityService()
        {
            string userName = Cryptophy.DecryptText(System.Configuration.ConfigurationManager.AppSettings["SEC_WS_Token1"]);
            string passWord = Cryptophy.DecryptText(System.Configuration.ConfigurationManager.AppSettings["SEC_WS_Token2"]);

            UsernameToken token = new UsernameToken(userName, passWord, PasswordOption.SendPlainText);
            if (_url == null)
            {
                _url = System.Configuration.ConfigurationManager.AppSettings["SEC_WS_URL"];
            }
            _Service.Url = _url;
            _Service.SetClientCredential(token);
            _Service.SetPolicy("ClientPolicy");

            this.ReportWS = _Service;
        }
        public SecurityService(string url_KeyName, string token1_KeyName, string token2_KeyName)
        {
            string userName = Cryptophy.DecryptText(System.Configuration.ConfigurationManager.AppSettings[token1_KeyName]);
            string passWord = Cryptophy.DecryptText(System.Configuration.ConfigurationManager.AppSettings[token2_KeyName]);
            UsernameToken token = new UsernameToken(userName, passWord, PasswordOption.SendPlainText);

            _Service.Url = System.Configuration.ConfigurationManager.AppSettings[url_KeyName];
            _Service.SetClientCredential(token);
            _Service.SetPolicy("ClientPolicy");
            base.ReportWS = _Service;
        }
        protected string RemoteIpAddress
        {
            get
            {
                string clientIPAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(clientIPAddress)) clientIPAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                return clientIPAddress;
            }
        }

        #region InUsed Methods
        public int ValidateUser(int clientid, string userName, string barePassword, string logSessionID, string sessionID)
        {
            return _Service.ValidateUser(clientid, userName, barePassword, RemoteIpAddress, GetLocalIPs(HttpContext.Current.Server.MachineName), HttpContext.Current.Request.UserAgent, logSessionID, sessionID);
        }
        public ASUser GetUser(int clientId, string userName)
        {
            return SecEntityConverter.ConvertASUser(_Service.GetUser(clientId, userName));
        }
        public bool UpdateUserLanguageID(Guid recID, int asClient, int siteID, int languageID)
        {
            return _Service.UpdateUserLanguageCode(recID, asClient, siteID, languageID);
        }
        public int GetUserLanguageID(Guid recID, int asClient, int siteID)
        {
            return _Service.GetUserLanguageCode(recID, asClient, siteID);
        }
        public PermissionCollection GetPermissionsForUser(int clientId, string userName)
        {
            return SecEntityConverter.BuildPermissionCollection(_Service.GetPermissionsForUser(clientId, userName));
        }
        public PermissionCollection GetPermissionsForUserSSO(int clientId, string userName, int ssoLevel)
        {
            return SecEntityConverter.BuildPermissionCollection(_Service.GetPermissionsForUserWithSSO(clientId, userName, ssoLevel));
        }
        public HierarchyCollection GetHierarchysOfUser(int clientId, string userName, int systemId)
        {
            return SecEntityConverter.BuildHierarchyCollection(_Service.GetHierarchiesOfUser(clientId, userName, systemId));
        }
        public bool LockOutUser(int clientId, string userName, string logSessionID)
        {
            return _Service.LockOutUser(clientId, userName, logSessionID);
        }
        public bool IsUserLockedOut(string userName)
        {
            return _Service.IsUserLockedOut(userName);
        }
        public int ChangeUserPassword(int clientid, string userID, string oldBarePassword, string newBarePassword, string logSessionID)
        {
            return _Service.ChangeUserPassword(clientid, userID, oldBarePassword, newBarePassword, logSessionID);
        }
        public void UpdateUser(ASUser user, int SystemID, string[] hierarchyIdList, string loginQuestionAnswerOriginalValue, string loginQuestionAnswerNewValue)
        {
            _Service.UpdateUser(user.ASClient, SystemID, user.OriginalUserID, user.UserID, user.UserNameFirst, user.UserNameLast, user.UserNameFull, user.Email,
                user.LoginQuestionIndex, user.LoginQuestionAnswer, user.Status, hierarchyIdList, Guid.Empty, user.UserType, user.UpdatedBy, loginQuestionAnswerOriginalValue,
                loginQuestionAnswerNewValue, user.SalesRepCode, user.Organizations, user.PhoneForSMS, user.ContactEmail);
        }
        public void UpdateUserForceUpdateUserName(ASUser user, int SystemID, string[] hierarchyIdList, string loginQuestionAnswerOriginalValue, string loginQuestionAnswerNewValue, bool isUpdateUserName)
        {
            _Service.UpdateUserForceUpdateUserName(user.ASClient, SystemID, user.OriginalUserID, user.UserID, user.UserNameFirst, user.UserNameLast, user.UserNameFull, user.Email,
                user.LoginQuestionIndex, user.LoginQuestionAnswer, user.Status, hierarchyIdList, Guid.Empty, user.UserType, user.UpdatedBy, loginQuestionAnswerOriginalValue,
                loginQuestionAnswerNewValue, user.SalesRepCode, user.Organizations, user.PhoneForSMS, user.ContactEmail, isUpdateUserName);
        }
        public void CreateUser(ASUser user, string[] hierarchyIdList)
        {
            _Service.CreateUser(user.ASClient, user.UserID, user.UserNameFirst, user.UserNameLast, user.UserNameFull, user.UserPassword, user.UserPasswordType,
                user.Email, user.LoginQuestionIndex, user.LoginQuestionAnswer, user.Status, hierarchyIdList, user.UserSecRole, user.UserType, user.CreatedBy, user.SalesRepCode, user.Organizations);
        }
        public void CreateMSUser(User user, string[] hierarchyIdList, string userTypeMode)
        {
            _Service.CreateMSUser(user.ASClient, user.UserID, user.UserNameFirst,
                user.UserNameLast, user.UserNameFull, user.UserPassword,
                user.UserPasswordType, user.Email, user.LoginQuestionIndex,
                user.LoginQuestionAnswer, user.Status, hierarchyIdList,
                user.UserSecRole, user.CreatedBy, user.SiteID, user.EntityID,
                userTypeMode);
        }
        public void AddThemeForUser(int clientid, string username, int hierarchyid, int themeId)
        {
            _Service.AddThemeForUser(clientid, username, hierarchyid, themeId);
        }
        public bool ResetUserPassword(int clientid, string userID, int passwordType, string newBarePassword)
        {
            return _Service.ResetUserPassword(clientid, userID, passwordType, newBarePassword);
        }
        public ASThemeCollection GetASThemsByUser(int clientid, string username, int hierarchyid)
        {
            return SecEntityConverter.BuildASThemesCollection(_Service.GetASThemesByUser(clientid, username, hierarchyid));
        }
        public bool DeleteASUser(int clientid, string userID)
        {
            return _Service.DeleteASUser(clientid, userID);
        }

        public PermissionCollection GetPermissionsInHierarchy(int hierarchyId)
        {
            return SecEntityConverter.BuildPermissionCollection(_Service.GetPermissionsInHierarchy(hierarchyId));
        }
        public bool DeleteHierarchy(Hierarchy deletedHierarchy)
        {
            return _Service.DeleteHierarchy(deletedHierarchy.HierarchyID);
        }
        public DataTable GetHierarchyAssignableListByHierarchy(int hierarchyId)
        {
            return _Service.GetHierarchyAssignableListByHierarchy(hierarchyId);
        }
        public HierarchyCollection GetAssignableHierarchy(int hierarchyId, string userType)
        {
            return SecEntityConverter.BuildHierarchyCollection(_Service.GetAssignableHierarchy(hierarchyId, userType));
        }

        public HierarchyCollection GetMSAssignableHierarchy(int clientId,
            string userId, string hierarchyLevel, string hierarchyCode, int systemId)
        {
            return SecEntityConverter.BuildHierarchyCollection(
                _Service.GetMSAssignableHierarchy(
                    clientId,
                    userId,
                    hierarchyLevel,
                    hierarchyCode,
                    systemId));
        }

        public DataTable GetCSUserListByHierarchyAccess(int hierarchyID)
        {
            try
            {
                SecService.User[] userArray = _Service.GetCSUserListByHierarchyAccess(hierarchyID);
                DataTable dtUserList = new DataTable();
                dtUserList.Columns.Add("UserID", System.Type.GetType("System.String"));
                dtUserList.Columns.Add("UserNameFirst", System.Type.GetType("System.String"));
                dtUserList.Columns.Add("UserNameLast", System.Type.GetType("System.String"));
                dtUserList.Columns.Add("Email", System.Type.GetType("System.String"));
                dtUserList.Columns.Add("HierarchyName", System.Type.GetType("System.String"));
                dtUserList.Columns.Add("StatusChar", System.Type.GetType("System.String"));
                dtUserList.Columns.Add("UserType", System.Type.GetType("System.String"));
                dtUserList.Columns.Add("SiteID", System.Type.GetType("System.Int32"));
                dtUserList.Columns.Add("ASClient", System.Type.GetType("System.Int32"));

                foreach (SecService.User c in userArray)
                {
                    DataRow row = dtUserList.NewRow();
                    row["UserID"] = c.UserID;
                    row["UserNameFirst"] = c.UserNameFirst;
                    row["UserNameLast"] = c.UserNameLast;
                    row["Email"] = c.Email;
                    row["HierarchyName"] = c.HierarchyName;
                    row["SiteID"] = c.SiteID;
                    row["ASClient"] = c.ASClient;

                    if (c.UserType == 1)
                    {
                        row["UserType"] = "Y";
                    }
                    else
                    {
                        row["UserType"] = "N";
                    }

                    if (c.ActvStat == "1")
                    {
                        row["StatusChar"] = "Y";
                    }
                    else
                    {
                        row["StatusChar"] = "N";
                    }

                    dtUserList.Rows.Add(row);
                }
                return dtUserList;
            }
            catch (System.Web.Services.Protocols.SoapHeaderException ex)
            {
                throw new HttpException(700, "Function: GetAllStatement", ex);
            }

        }
        public SecMenuItemCollection GetMenuItemForCreateRole(int clientID, string username, int systemId, string group)
        {
            return SecEntityConverter.BuildSECMenuCollection(_Service.GetMenuItemForCreateRole(clientID, username, systemId, group));
        }

        public SecMenuItemCollection GetMenuItemsByUser(int clientid, string username, int hierarchyid, int languageId, bool manageMode)
        {
            var menus = _Service.GetMenuItemsByUser(clientid, username, hierarchyid, languageId, manageMode);
            return SecEntityConverter.BuildSECMenuCollection(menus);
        }

        public SecMenuItemCollection GetMenuItemsByUserSSO(int clientid, string username, int hierarchyid, int ssoLevel, int languageId, bool manageMode)
        {
            return SecEntityConverter.BuildSECMenuCollection(_Service.GetMenuItemsByUserSSO(clientid, username, hierarchyid, ssoLevel, languageId, manageMode));
        }

        public SecMenuItemCollection GetMSMenuItems(int clientId, string hierarchyLevel,
            string hierarchyCode, int languageId)
        {
            return SecEntityConverter.BuildSECMenuCollection(
                _Service.GetMSMenuItems(clientId, hierarchyLevel, hierarchyCode, languageId));
        }

        public Hierarchy GetHierarchyById(int hierarchyId)
        {
            return SecEntityConverter.ConvertHierarchy(_Service.GetHierarchyById(hierarchyId));
        }

        public HierarchyCollection GetHierarchyByName(string hierarchyName, int clientId, int systemId)
        {
            return SecEntityConverter.BuildHierarchyCollection(_Service.GetHierarchyByName(
                hierarchyName, clientId, systemId));
        }


        public PermissionCollection GetPermissionsByASUserGroupType(int clientId, string userName, int systemId, bool isViewAll = false, bool isGetUnChecked = false)
        {
            return SecEntityConverter.BuildPermissionCollection(_Service.GetPermissionsByASUserGroupType(clientId, userName, systemId, isViewAll, isGetUnChecked));
        }

        public PermissionCollection GetPermissionsByUserGroupType(int clientId, string userName, int systemId, string group, string type, int languageId)
        {
            return SecEntityConverter.BuildPermissionCollection(_Service.GetPermissionsByUserGroupType(clientId, userName, systemId, group, type, languageId));
        }

        public PermissionCollection GetMSPermissionsByType(int clientId, string type, string hierarchyLevel, string hierarchyCode)
        {
            return SecEntityConverter.BuildPermissionCollection(
                _Service.GetMSPermissionsByType(clientId, type, hierarchyLevel, hierarchyCode));
        }
        public HierarchyCollection GetHierarchyTreeForUser(int clientId, string username, int systemId)
        {
            return SecEntityConverter.BuildHierarchyCollection(_Service.GetHierarchyTreeForUser(clientId, username, systemId));
        }
        public int CreateHierarchy(Hierarchy newHierarchy)
        {
            return _Service.CreateHierarchyWithCreatedUser(
                newHierarchy.SystemId, newHierarchy.HierarchyName,
                newHierarchy.HierarchyParent, newHierarchy.HierarchyDescription,
                newHierarchy.CreatedBy, newHierarchy.ClientId,
                newHierarchy.HierarchyCode, newHierarchy.HierarchyLevel);
        }

        public void UpdateHierarchy(Hierarchy updatedHierarchy, Guid updatedBy)
        {
            _Service.UpdateHierarchy(updatedHierarchy.SystemId, updatedHierarchy.HierarchyID,
                updatedHierarchy.HierarchyName, updatedHierarchy.HierarchyParent,
                updatedHierarchy.HierarchyDescription, updatedHierarchy.ActvStatus,
                updatedHierarchy.ClientId, updatedHierarchy.HierarchyCode,
                updatedHierarchy.HierarchyLevel, updatedBy);
        }
        public void AddPermissionIntoHierarchy(int permissionId, int hierarchyId)
        {
            _Service.AddPermissionIntoHierarchy(permissionId, hierarchyId);
        }
        public void AddPermissionIntoHierarchy(string permissionCode, int hierarchyId, Guid updatedBy, bool isLogged)
        {
            _Service.AddPermissionCodeIntoHierarchy(permissionCode, hierarchyId, updatedBy, isLogged);
        }
        public void AddPermissionsIntoHierarchy(int[] permissionIds, int hierarchyId)
        {
            _Service.AddPermissionsIntoHierarchy(permissionIds, hierarchyId);
        }
        public void RemovePermissionFromHierarchy(int permissionId, int hierarchyId)
        {
            _Service.RemovePermissionFromHierarchy(permissionId, hierarchyId);
        }
        public void RemovePermissionFromHierarchy(string permissionCode, int hierarchyId, int clientId, Guid updatedBy, bool isLogged)
        {
            _Service.RemovePermissionCodeFromHierarchy(permissionCode, hierarchyId, clientId, updatedBy, isLogged);
        }
        public void RemovePermissionsFromHierarchy(int[] permissionIds, int hierarchyId)
        {
            _Service.RemovePermissionsFromHierarchy(permissionIds, hierarchyId);
        }
        public void UpdateAssignableHierarchy(AssignableHierarchyModel assignableHierarchyModel)
        {
            var updateModel = new SecService.AssignableHierarchyModel()
            {
                AsClientId = assignableHierarchyModel.AsClientId,
                SiteId = assignableHierarchyModel.SiteId,
                UserId = assignableHierarchyModel.UserId,
                ChangedByRecId = assignableHierarchyModel.ChangedByRecId,
                IsUpdate = assignableHierarchyModel.IsUpdate,
                HierarchyId = assignableHierarchyModel.HierarchyId,
                AssignHierarchyId = assignableHierarchyModel.AssignHierarchyId,
                IsRemove = assignableHierarchyModel.IsRemove
            };
            _Service.UpdateAssignableHierarchy(updateModel);
        }
        public void UpdateChildHierarchyPermission(int hierarchyId)
        {
            _Service.UpdateChildHierarchyPermission(hierarchyId);
        }
        public HierarchyAccessCollection GetSiteJumpAccessByHierarchy(int hierarchyId, string type)
        {

            return SecEntityConverter.BuildHierarchyAccessCollection(_Service.GetSiteJumpAccessByHierarchy(hierarchyId, type));
        }

        public HierarchyAccessCollection GetHierarchyAccessByHierarchy(int hierarchyId, string type)
        {
            return SecEntityConverter.BuildHierarchyAccessCollection(_Service.GetHierarchyAccessByHierarchy(hierarchyId, type));
        }
        public void UpdateHierarchyAccess(HierarchyAccess item, bool isRemove)
        {
            _Service.UpdateHierarchyAccess(item.HierarchyId, item.EntityNumber, item.UserIDCreate, item.Type, isRemove);
        }
        public void UpdateSiteJumpAccess(HierarchyAccess item, bool isRemove)
        {
            _Service.UpdateSiteJumpAccess(item.HierarchyId, item.Login, item.UserIDCreate, item.Type, isRemove);
        }
        public PermissionCollection GetExcludePermissionsForUser(int clientID, string username)
        {
            return SecEntityConverter.BuildPermissionCollection(_Service.GetExcludePermissionsForUser(clientID, username));
        }
        public HierarchyCollection GetHierarchiesOfUser(int clientId, string username, int systemId)
        {
            return SecEntityConverter.BuildHierarchyCollection(_Service.GetHierarchiesOfUser(clientId, username, systemId));
        }
        public void InsertExcludePermissionForUser(int clientID, string username, int permissionId, DateTime startDate, DateTime endDate, Guid updatedBy, bool isLogged)
        {
            _Service.InsertExcludePermissionForUser(clientID, username, permissionId, startDate, endDate, updatedBy, isLogged);
        }
        public void DeleteExcludePermissionForUser(int clientID, string username, int permissionId, Guid updatedBy, bool isLogged)
        {
            _Service.DeleteExcludePermissionForUser(clientID, username, permissionId, updatedBy, isLogged);
        }
        public bool IsExistedUserName(int clientId, string username)
        {
            return _Service.IsExistedUserName(clientId, username);
        }

        public bool CheckUniqueHierarchyForUpdate(int clientId, string hierarchyName, int hierarchyId)
        {
            return _Service.CheckUniqueHierarchyForUpdate(clientId, hierarchyName, hierarchyId);

        }
        public bool CheckUniqueHierarchyForCreate(int clientId, string hierarchyName)
        {
            return _Service.CheckUniqueHierarchyForCreate(clientId, hierarchyName);

        }
        public void CreateUpdateRiskGroupForUser(int clientId, Guid userid, int groupid)
        {
            _Service.CreateUpdateRiskGroupForUser(clientId, userid, groupid);

        }
        public int GetRiskGroupForUser(int clientId, Guid userid)
        {
            return _Service.GetRiskGroupForUser(clientId, userid);

        }
        public ASUserCollection GetSecondaryUsers(int clientId, string userName)
        {

            return SecEntityConverter.BuildASUserCollection(_Service.GetSecondaryUsers(userName, clientId));
        }

        public void AddRequestHeader(string name, string value)
        {
            _Service.AddRequestHeader(name, value);
        }
        #endregion
        #region Log
        public void InsertIntruderLog(ILogObject logObject)
        {
            _Service.InsertIntruderLog(logObject.LogWebServerDts, logObject.LogId1, logObject.LogId2, logObject.LogRecordCount, logObject.LogElapsedTime, logObject.LogMenuId, logObject.LogSubMenuId,
                logObject.LogSessionId, logObject.LogSessionCnt, logObject.LogLoggingMode, logObject.LogSpecialId, logObject.LogClientId, logObject.LogSystemId, logObject.LogFullName, logObject.LogWebSiteName,
                logObject.LogData1, logObject.LogData2, logObject.LogData3, logObject.LogData4, logObject.LogData5, logObject.LogData6, logObject.LogData7, logObject.LogData8, logObject.LogData9, logObject.LogData10,
                logObject.LogTxt1, logObject.LogTxt2, logObject.LogClientIPAddr, logObject.LogHostIPAddr, logObject.LogBrowserType);
        }
        public int InsertASPXTrackingLog(ILogObject logObject)
        {
            return _Service.InsertASPXTrackingLog(logObject.LogWebServerDts, logObject.LogId1, logObject.LogId2, logObject.LogRecordCount, logObject.LogElapsedTime, logObject.LogMenuId, logObject.LogSubMenuId,
                logObject.LogSessionId, logObject.LogSessionCnt, logObject.LogLoggingMode, logObject.LogSpecialId, logObject.LogClientId, logObject.LogSystemId, logObject.LogFullName, logObject.LogWebSiteName,
                logObject.LogData1, logObject.LogData2, logObject.LogData3, logObject.LogData4, logObject.LogData5, logObject.LogData6, logObject.LogData7, logObject.LogData8, logObject.LogData9, logObject.LogData10,
                logObject.LogTxt1, logObject.LogTxt2, logObject.LogClientIPAddr, logObject.LogHostIPAddr, logObject.LogBrowserType);
        }

        public void InsertLOG_WebServerSessionStats(string webSiteIPAddress, string webSiteClient, string webSiteType, string webSiteGroup, string webSiteDNS)
        {
            _Service.InsertLOG_WebServerSessionStats(webSiteIPAddress, webSiteClient, webSiteType, webSiteGroup, webSiteDNS);
        }
        public void UpdateLOG_WebServerSessionStats(string webSiteIPAddress, int webSiteMaxUsers, int webSiteActiveUsers)
        {
            _Service.UpdateLOG_WebServerSessionStats(webSiteIPAddress, webSiteMaxUsers, webSiteActiveUsers);
        }
        public void DeleteLOG_WebServerSessionStats(string webSiteIPAddress)
        {
            _Service.DeleteLOG_WebServerSessionStats(webSiteIPAddress);
        }

        public void DeleteLOG_WebServerSessionLog(string webSiteIPAddress, string sessionID)
        {
            _Service.DeleteLOG_WebServerSessionLog(webSiteIPAddress, sessionID);
        }

        public void InsertLOG_WebServerSessionLog(string webSiteIPAddress, string sessionID, string userIPAddress, string browserType, string httpCookie)
        {
            _Service.InsertLOG_WebServerSessionLog(webSiteIPAddress, sessionID, userIPAddress, browserType, httpCookie);
        }
        public void UpdateLOG_WebServerSessionLog(string webSiteIPAddress, string sessionID, string userIPAddress, string browserType, string httpCookie)
        {
            _Service.UpdateLOG_WebServerSessionLog(webSiteIPAddress, sessionID, userIPAddress, browserType, httpCookie);
        }
        public void UpdateLOG_WebServerSessionLogEnd(string sessionID)
        {
            _Service.UpdateLOG_WebServerSessionLogEnd(sessionID);
        }
        #endregion
        #region Generic Services
        public DataTable GetReports(string spName, FilterParameterCollection parameters)
        {
            try
            {
                FilterParamWS[] wsparams = new FilterParamWS[parameters.Count];
                for (int i = 0; i < wsparams.Length; i++)
                {
                    wsparams[i] = new FilterParamWS();
                    wsparams[i].ParameterName = parameters[i].ParameterName;
                    wsparams[i].ParameterValue = parameters[i].ParameterValue;
                    wsparams[i].ParameterType = parameters[i].ParameterType;

                }
                return _Service.GetReports(spName, wsparams);
            }
            catch (Exception ex)
            {
                LogHepler.WriteLogException("GetReports", spName, parameters, ex);
                throw;
            }
            
        }
        public string EncryptText(string str)
        {
            return _Service.EncryptText(str);
        }
        public string EncryptText(string str, string key)
        {
            return _Service.EncryptTextWithKey(str, key);
        }
        public string DecryptText(string str)
        {
            return _Service.DecryptText(str);
        }
        public string DecryptText(string str, string key)
        {
            return _Service.DecryptTextWithKey(str, key);
        }
        public int GenNum(int min, int max)
        {
            return _Service.GenNum(min, max);
        }
        public string GenPwd()
        {
            return _Service.GenPwd();
        }

        #endregion
        #region JumpSite
        public string CreateJumpSiteTicket(Guid userId, string remoteIP, int desSysId, Guid jumper)
        {
            return _Service.CreateJumpSiteTicket(userId, remoteIP, desSysId, jumper);
        }

        public bool CheckJumpSiteTicket(Guid userId, Guid jumper, string tempPassword, int desSysId, int clientId)
        {
            return _Service.CheckJumpSiteTicket(userId, jumper, tempPassword, HttpContext.Current.Request.UserHostAddress, desSysId, clientId, GetLocalIPs(HttpContext.Current.Server.MachineName), HttpContext.Current.Request.UserAgent);
        }
        string GetLocalIPs(string hostname)
        {

            System.Net.IPHostEntry ips = System.Net.Dns.GetHostEntry(hostname);
            if (ips.AddressList.Length > 0)
            {
                return ips.AddressList[0].ToString();
            }
            return "";
        }
        #endregion
        #region Configs
        public List<AppConfig> GetAppConfigByAppCode(string appCode)
        {
            return SecEntityConverter.BuildConvertAppConfigCollection(_Service.GetAppConfigByAppCode(appCode));

        }
        public RefTableValueCollection GetRefTableValues(int clientId, string refTableName, string refTableLang, int LanguageID)
        {
            return SecEntityConverter.BuildReTableValueCollection(_Service.GetRefTableValues(clientId, refTableName, refTableLang, LanguageID));
        }
        #endregion

        public SecService.Partner GetPartners(string partnerName, string clientID)
        {
            return _Service.GetPartners(partnerName, clientID);
        }

        #region Email And Forgot Password (used for Email Enrollment requirement)
        public bool UpdateEmailAndConfirm(string userName, string email, string valCode)
        {
            return _Service.UpdateEmailAndConfirm(userName, email, valCode);

        }
        public byte GetCurrentEmailStatus(string userName)
        {
            return _Service.GetCurrentEmailStatus(userName);

        }
        public int CheckEmailConfirm(string userName, string valCode)
        {
            return _Service.CheckEmailConfirm(userName, valCode);

        }

        public bool CreateForgotPwdTicket(string userName, string valCode)
        {
            return _Service.CreateForgotPwdTicket(userName, valCode);
        }
        public int CheckForgotPwdTicket(string userName, string valCode)
        {
            return _Service.CheckForgotPwdTicket(userName, valCode);

        }

        public void UpdateForgotPassword(int clientId, string userName, bool isSuccess)
        {
            _Service.UpdateForgotPassword(clientId, userName, isSuccess);
        }

        public bool IsForgotLockedOut(int clientId, string userName)
        {
            return _Service.IsForgotLockedOut(clientId, userName);
        }
        #endregion

        public bool HasUserOfHierarchy(int hierarchy)
        {
            return _Service.HasUserOfHierarchy(hierarchy);
        }

        public static FilterParamWS[] ConvertToFilterParamWSArray(FilterParameterCollection filterParams)
        {
            if (filterParams == null)
                return new FilterParamWS[0];
            List<FilterParamWS> array = new List<FilterParamWS>();
            foreach (FilterParameter param in filterParams)
            {
                array.Add(ConvertToFilterParamWS(param));
            }
            return array.ToArray();
        }
        public static FilterParamWS ConvertToFilterParamWS(FilterParameter filterParam)
        {
            if (filterParam == null)
                return null;
            return new FilterParamWS
            {
                ParameterName = filterParam.ParameterName,
                ParameterType = filterParam.ParameterType,
                ParameterValue = filterParam.ParameterValue,
                IsOutParameter = filterParam.IsOutParameter
            };
        }
        public SecService.MobileUser GetMobileUser(int clientId, string userId)
        {
            return _Service.GetMobileUser(clientId, userId);
        }
    }
}
