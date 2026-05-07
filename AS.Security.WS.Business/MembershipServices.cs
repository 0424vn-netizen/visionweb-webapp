using System;
using System.Collections.Generic;
using System.Text;

using AS.Security.WS.Entities;
using AS.Security.WS.Data;
using System.Data;

namespace AS.Security.WS.Business
{
    public class MembershipServices
    {
        readonly MembershipDao _UserIDsDAO;
        public MembershipServices(string connString)
        {
            _UserIDsDAO = new MembershipDao(connString);
        }
        ///Description for GetUser
        ///Author:
        ///Date: 

        public int ValidateUserID(ValidateUserModel validateUserModel)
        {
            return _UserIDsDAO.ValidateUserID(validateUserModel);
        }
        public int ChangePassword(int clientid, string userID, string oldBarePassword, string newBarePassword, string logSessionID)
        {

            return _UserIDsDAO.ChangePassword(clientid, userID, oldBarePassword, newBarePassword, logSessionID);
        }
        public bool ResetPassword(int clientid, string userName, int passwordType, string newBarePassword)
        {

            return _UserIDsDAO.ResetPassword(clientid, userName, passwordType, newBarePassword);
        }
        public User GetUserID(int clientid, string userID)
        {

            return _UserIDsDAO.GetUserID(clientid, userID);
        }

        public int GetUserLanguageCode(Guid recID, int asClient, int siteID)
        {
            return _UserIDsDAO.GetUserLanguageID(recID, asClient, siteID);
        }

        public bool UpdateUserLanguageCode(Guid recID, int asClient, int siteID, int languageID)
        {
            return _UserIDsDAO.UpdateUserLanguageID(recID, asClient, siteID, languageID);
        }

        ///Description for InsertUser
        ///Author:
        ///Date: 
        public void InsertUser(UserInfoModel userInfo)
        {

            _UserIDsDAO.InsertUser(userInfo);
        }

        ///Description for InsertMSUser
        ///Author:
        ///Date: 
        public void InsertMSUser(UserInfoModel userInfo)
        {
            _UserIDsDAO.InsertMSUser(userInfo);
        }

        ///Description for UpdateUser
        ///Author:
        ///Date: 
        public void UpdateUser(UpdateUserInfoModel userInfo)
        {
            _UserIDsDAO.UpdateUser(userInfo);
        }

        ///Description for UpdateUser: Update user force change UserID
        ///Author: QuocLe
        ///Date: 11/30/2017
        public void UpdateUserForceUpdateUserName(UpdateUserInfoModel userInfo)
        {
            _UserIDsDAO.UpdateUserForceUpdateUserName(userInfo);
        }

        public bool IsExistedUserName(int clientId, string username)
        {
            return (_UserIDsDAO).IsExistedUserName(clientId, username);
        }
        public UserCollection GetSecondaryUsers(string userName, int clientId)
        {
            return (_UserIDsDAO).GetSecondaryUsers(userName, clientId);
        }
        public UserCollection GetUsersForCS(string userList, string type)
        {
            return (_UserIDsDAO).GetUsersForCS(userList, type);
        }
        ///Description for BindCSUser
        ///Author:HoangDinh
        ///Date:12/1/2008
        public UserCollection GetCSUserListByHierarchyAccess(int hierarchyID)
        {
            return (_UserIDsDAO).GetCSUserListByHierarchyAccess(hierarchyID);
        }
        public bool HasUserOfHierarchy(int hierarchyID)
        {
            return (_UserIDsDAO).HasUserOfHierarchy(hierarchyID);
        }
        public bool DeleteASUser(int clientid, string userID)
        {
            return (_UserIDsDAO).DeleteASUser( clientid,  userID);
        } 


        #region Email And Forgot Password (Email Enrollment)
        public bool UpdateEmailAndConfirm(string userName, string email, string valCode)
        {
            return (_UserIDsDAO).UpdateEmailAndConfirm(userName, email, valCode);

        }
        public byte GetCurrentEmailStatus(string userName)
        {
            return (_UserIDsDAO).GetCurrentEmailStatus(userName);

        }
        public int CheckEmailConfirm(string userName, string valCode)
        {
            return (_UserIDsDAO).CheckEmailConfirm(userName, valCode);

        }

        public bool CreateForgotPwdTicket(string userName, string valCode)
        {
            return (_UserIDsDAO).CreateForgotPwdTicket(userName, valCode);
        }
        public int CheckForgotPwdTicket(string userName, string valCode)
        {
            return (_UserIDsDAO).CheckForgotPwdTicket(userName, valCode);

        }
        public bool LockOutUser(int clientID, string userName, string logSessionID)
        {
            return (_UserIDsDAO).LockOutUser(clientID, userName, logSessionID);
        }
        public bool IsUserLockedOut(string userName)
        {
            return (_UserIDsDAO).IsUserLockedOut(userName);
        }
        public void UpdateForgotPassword(int clientId, string userName, bool isSuccess)
        {
            (_UserIDsDAO).UpdateForgotPassword(clientId, userName, isSuccess);
        }

        public bool IsForgotLockedOut(int clientId, string userName)
        {
            return (_UserIDsDAO).IsForgotLockedOut(clientId, userName);
        }
        #endregion
    }
}

