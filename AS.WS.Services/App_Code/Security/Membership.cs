using System;
using System.Configuration;
using System.Linq;
using System.Web.Services;
using AS.Security.WS.Entities;
using AS.Common.Logger;
using AS.ApiClient.UnderWriting.Models;
using AS.ApiClient.UnderWriting;

/// <summary>
/// Summary description for Membership
/// </summary>
public partial class SecurityService
{
    [WebMethod(Description = "Create a AS user")]
    public void CreateUser(int clientid, string userName, string userNameFirst, string userNameLast, string userNameFull, string userPassword, int userPasswordType,
        string email, int loginQuestionIndex, string loginQuestionAnswer, string status, string[] hierarchyIds, string usersecrole, int userType, Guid createdBy, string salesRepCode, string organizations)
    {
        try
        {
            var userInfo = new UserInfoModel()
            {
                ClientId = clientid,
                UserName = userName,
                UserNameFirst = userNameFirst,
                UserNameLast = userNameLast,
                UserNameFull = userNameFull,
                UserPassword = userPassword,
                UserPasswordType = userPasswordType,
                Email = email,
                LoginQuestionIndex = loginQuestionIndex,
                LoginQuestionAnswer = loginQuestionAnswer,
                Status = status,
                HierarchyIds = hierarchyIds,
                UserSecRole = usersecrole,
                CreatedBy = createdBy,
                UserType = userType,
                SalesRepCode = salesRepCode,
                Organizations = organizations,
            };
            _MembershipService.InsertUser(userInfo);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("CreateUser:\n" + ex.ToString());
        }
    }

    [WebMethod(Description = "Create a MS user")]
    public void CreateMSUser(int clientId, string userName, string userNameFirst,
        string userNameLast, string userNameFull, string userPassword,
        int userPasswordType, string email, int loginQuestionIndex,
        string loginQuestionAnswer, string status, string[] hierarchyIds,
        string usersecrole, Guid createdBy, int siteId, string entityID,
        string userTypeMode)
    {
        try
        {
            var userInfo = new UserInfoModel() {
                ClientId = clientId,
                UserName = userName,
                UserNameFirst = userNameFirst,
                UserNameLast = userNameLast,
                UserNameFull = userNameFull,
                UserPassword = userPassword,
                UserPasswordType = userPasswordType,
                Email = email,
                LoginQuestionIndex = loginQuestionIndex,
                LoginQuestionAnswer = loginQuestionAnswer,
                Status = status,
                HierarchyIds = hierarchyIds,
                UserSecRole = usersecrole,
                CreatedBy = createdBy,
                SiteId = siteId,
                EntityId = entityID,
                UserTypeMode = userTypeMode,
            };
            _MembershipService.InsertMSUser(userInfo);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("CreateMSUser:\n" + ex.ToString());
        }
    }

    [WebMethod(Description = "Update a AS user")]
    public void UpdateUser(int clientid, int SystemID, string originalUserID, string userName, string userNameFirst, string userNameLast, string userNameFull, string email,
        int loginQuestionIndex, string loginQuestionAnswer, string actvStat, string[] hierarchyIds, Guid recID, int userType, Guid updatedBy,
        string loginQuestionAnswerOriginalValue, string loginQuestionAnswerNewValue, string salesRepCode, string organizations, string phoneForSMS, string contactEmail)
    {
        try
        {
            string hasSyncDataUW = BaseService.ClientExtendSettingSingleton.ShareInstance.GetDataOfExtendedSetting("SyncDataForShadowUnderwritings");
            var userInfo = new UpdateUserInfoModel()
            {
                ClientId = clientid,
                SystemId = SystemID,
                OriginalUserId = originalUserID,
                UserName = userName,
                UserNameFirst = userNameFirst,
                UserNameLast = userNameLast,
                UserNameFull = userNameFull,
                Email = email,
                LoginQuestionIndex = loginQuestionIndex,
                LoginQuestionAnswer = loginQuestionAnswer,
                Status = actvStat,
                HierarchyIds = hierarchyIds,
                RecId = recID,
                UserType = userType,
                UpdatedBy = updatedBy,
                LoginQuestionAnswerOriginalValue = loginQuestionAnswerOriginalValue,
                LoginQuestionAnswerNewValue = loginQuestionAnswerNewValue,
                SalesRepCode = salesRepCode,
                Organizations = organizations,
                PhoneForSMS = phoneForSMS,
                ContactEmail = contactEmail,
                HasSyncDataUw = hasSyncDataUW,
            };

            _MembershipService.UpdateUser(userInfo);
            SyncUserToUnderwriting(clientid, userName, userNameFull);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("UpdateUser:\n" + ex.ToString());
        }
    }

    private void SyncUserToUnderwriting(int clientId, string userId, string fullName)
    {
        var isDisable = ConfigurationManager.AppSettings["UnderWritingApiDisable"] == "true";
        string hasSyncDataUW = BaseService.ClientExtendSettingSingleton.ShareInstance.GetDataOfExtendedSetting("SyncDataForShadowUnderwritings");
        bool isSync = false;
        
        if (!string.IsNullOrEmpty(hasSyncDataUW))
        {
            isSync = hasSyncDataUW.Split(',').Any(x => x.Equals(clientId.ToString()));
        }

        if (!isDisable && isSync)
        {
            var headerRequest = new UpdateFullNameForAPRequest()
            {
                Header = new Header()
                {
                    AsclientId = clientId,
                    UserId = userId,
                    LanguageId = "1"
                },
                Data = new MemberItem()
                {
                    MemberId = userId,
                    MemberFullname = fullName,
                }
            };

            var client = new UnderWritingClient();
            client.UpdateFullNameForApproverGroup(headerRequest, "UpdateFullNameForUW");
        }        
    }

    [WebMethod(Description = "Update a AS user for change username")]
    public void UpdateUserForceUpdateUserName(int clientid, int SystemID, string originalUserID, string userName, string userNameFirst, string userNameLast, string userNameFull, string email,
        int loginQuestionIndex, string loginQuestionAnswer, string actvStat, string[] hierarchyIds, Guid recID, int userType, Guid updatedBy,
        string loginQuestionAnswerOriginalValue, string loginQuestionAnswerNewValue, string salesRepCode, string organizations, string phoneForSMS, string contactEmail, bool isUpdateUserName)
    {
        try
        {
            var userInfo = new UpdateUserInfoModel()
            {
                ClientId = clientid,
                SystemId = SystemID,
                OriginalUserId = originalUserID,
                UserName = userName,
                UserNameFirst = userNameFirst,
                UserNameLast = userNameLast,
                UserNameFull = userNameFull,
                Email = email,
                LoginQuestionIndex = loginQuestionIndex,
                LoginQuestionAnswer = loginQuestionAnswer,
                Status = actvStat,
                HierarchyIds = hierarchyIds,
                RecId = recID,
                UserType = userType,
                UpdatedBy = updatedBy,
                LoginQuestionAnswerOriginalValue = loginQuestionAnswerOriginalValue,
                LoginQuestionAnswerNewValue = loginQuestionAnswerNewValue,
                SalesRepCode = salesRepCode,
                Organizations = organizations,
                PhoneForSMS = phoneForSMS,
                ContactEmail = contactEmail,
                IsUpdateUserName = isUpdateUserName,
            };

            _MembershipService.UpdateUserForceUpdateUserName(userInfo);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("UpdateUserForceUpdateUserName:\n" + ex.ToString());
        }
    }

    [WebMethod(Description = "check existing of a user")]
    public bool IsExistedUserName(int clientId, string username)
    {
        return (_MembershipService).IsExistedUserName(clientId, username);
    }
    [WebMethod(Description = "Get secondary users.")]
    public UserCollection GetSecondaryUsers(string userName, int clientId)
    {
        UserCollection Users = null;
        try
        {

            Users = _MembershipService.GetSecondaryUsers(userName, clientId);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("GetSecondaryUsers:\n" + ex.ToString());
        }
        return Users;
    }
    [WebMethod(Description = "Get user detail")]
    public User GetUser(int clientid, string username)
    {
        AS.Security.WS.Entities.User user = null;
        try
        {

            user = _MembershipService.GetUserID(clientid, username);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("GetUser:\n" + ex.ToString());
        }
        return user;
    }
    [WebMethod(Description = "Update user language code")]
    public bool UpdateUserLanguageCode(Guid recID, int asClient, int siteID, int languageID)
    {
        try
        {
            return _MembershipService.UpdateUserLanguageCode(recID, asClient, siteID, languageID);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("UpdateUserLanguageCode:\n" + ex.ToString());
        }
        return false;
    }
    [WebMethod(Description = "Get user language code")]
    public int GetUserLanguageCode(Guid recID, int asClient, int siteID)
    {
        int languageID = 1;
        try
        {

            languageID = _MembershipService.GetUserLanguageCode(recID, asClient, siteID);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("GetUserLanguageCode:\n" + ex.ToString());
        }
        return languageID;
    }
    [WebMethod(Description = "check is existed user of hierarchy")]
    public bool HasUserOfHierarchy(int hierarchyID)
    {

        return _MembershipService.HasUserOfHierarchy(hierarchyID);
    }

    [WebMethod(Description = "Get all hierarchy of the user")]
    public HierarchyCollection GetHierarchiesOfUser(int clientId, string username, int systemId)
    {
        HierarchyCollection Hierarchys = null;
        try
        {
            Hierarchys = _PermHierService.GetHierarchysOfUser(clientId, username, systemId);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("GetHierarchiesOfUser:\n" + ex.ToString());
        }
        return Hierarchys;
    }

    [WebMethod(Description = "Validate login information of AS's user.")]
    public int ValidateUser(int clientid, string userName, string barePassword, string remoteIp, string hostIp, string browserType, string logSessionID, string sessionID)
    {
        int ret = 0;
        try
        {
            var validateUserModel = new ValidateUserModel()
            {
                ClientId = clientid,
                UserName = userName,
                BarePassword = barePassword,
                RemoteIp = remoteIp,
                HostIp = hostIp,
                BrowserType = browserType,
                LogSessionID = logSessionID,
                Sessionid = sessionID,
            };
            ret = _MembershipService.ValidateUserID(validateUserModel);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("ValidateUser:\n" + ex.ToString());
        }
        return ret;
    }

    [WebMethod(Description = "Change user's password.")]
    public int ChangeUserPassword(int clientid, string userID, string oldBarePassword, string newBarePassword, string logSessionID)
    {
        int errorFlag = 0;
        try
        {

            errorFlag = _MembershipService.ChangePassword(clientid, userID, oldBarePassword, newBarePassword, logSessionID);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("ChangeUserPassword:\n" + ex.ToString());
        }
        return errorFlag;
    }
    [WebMethod(Description = "Reset user's password.")]
    public bool ResetUserPassword(int clientid, string userID, int passwordType, string newBarePassword)
    {
        bool IsChanged = false;
        try
        {

            IsChanged = _MembershipService.ResetPassword(clientid, userID, passwordType, newBarePassword);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("ResetUserPassword:\n" + ex.ToString());
        }
        return IsChanged;
    }

    ///Description for BindCSUser
    ///Author:HoangDinh
    ///Date:12/1/2008
    [WebMethod(Description = "Get CS's User List By Hierarchy Access.")]
    public UserCollection GetCSUserListByHierarchyAccess(int hierarchyID)
    {
        UserCollection Users = null;
        try
        {

            Users = _MembershipService.GetCSUserListByHierarchyAccess(hierarchyID);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("GetCSUserListByHierarchyAccess:\n" + ex.ToString());
        }
        return Users;
    }
    [WebMethod(Description = "Delete AS User")]
    public bool DeleteASUser(int clientid, string userID)
    { 
        return  _MembershipService.DeleteASUser(clientid, userID);
        
    } 


    #region Email And Forgot Password (used for Email Enrollment requirement)
    [WebMethod]
    public bool UpdateEmailAndConfirm(string userName, string email, string valCode)
    {
        return (_MembershipService).UpdateEmailAndConfirm(userName, email, valCode);

    }
    [WebMethod]
    public byte GetCurrentEmailStatus(string userName)
    {
        return (_MembershipService).GetCurrentEmailStatus(userName);

    }
    [WebMethod]
    public int CheckEmailConfirm(string userName, string valCode)
    {
        return (_MembershipService).CheckEmailConfirm(userName, valCode);

    }
    [WebMethod]
    public bool CreateForgotPwdTicket(string userName, string valCode)
    {
        return (_MembershipService).CreateForgotPwdTicket(userName, valCode);
    }
    [WebMethod]
    public int CheckForgotPwdTicket(string userName, string valCode)
    {
        return (_MembershipService).CheckForgotPwdTicket(userName, valCode);

    }
    [WebMethod]
    public bool LockOutUser(int clientID, string userName, string logSessionID)
    {
        return (_MembershipService).LockOutUser(clientID, userName, logSessionID);
    }
    [WebMethod]
    public bool IsUserLockedOut(string userName)
    {
        return (_MembershipService).IsUserLockedOut(userName);
    }
    [WebMethod]
    public void UpdateForgotPassword(int clientId, string userName, bool isSuccess)
    {
        try
        {
            (_MembershipService).UpdateForgotPassword(clientId, userName, isSuccess);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("Update ForgotPassword:\n" + ex.ToString());
        }
    }

    [WebMethod]
    public bool IsForgotLockedOut(int clientId, string userName)
    {
        return (_MembershipService).IsForgotLockedOut(clientId, userName);
    }
    #endregion

}