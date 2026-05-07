using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AS.Tax.Security.Web.Services;
using System.Configuration;
using AS.Common.DBManager;
using System.Data;
using AS.Tax.Security.Web.Services.SecService;
using AS.Tax.Security.Web.Services.Model;
using System.Text;

public partial class UserControls_Jump2Tin : GlobalUserControl
{
    SecurityService _TaxSecurityService = new SecurityService(SessionManager.CurrentClient);

    protected void Page_Load(object sender, EventArgs e)
    {
        string gateUrl = WebSiteSettings.TaxGate;
        User user = _TaxSecurityService.GetUser(SessionManager.CurrentClient, SessionManager.CurrentUser.UserID);

        int hierarchy1099K = GetHierarchyIn1099KByHierarchyID(SessionManager.CurrentHierarchyId);

        string warningMessage = "<h1>" + GetLocalResourceObject("Jum2TinCS_InvalidUsers").ToString() + "</h1>";

        if (user != null && hierarchy1099K > 0) //User does not exist in TIN
        {
            JumpTo1099K(gateUrl, user, ref warningMessage, hierarchy1099K);
        }
        else
        {
            if (CheckUserType() && hierarchy1099K > 0)
            {
                AS.Security.WS.Entities.User currentUser = WebServices.SecurityServices.GetUser(SessionManager.CurrentClient, SessionManager.CurrentUser.UserID);
                if (currentUser != null)
                {
                    int accountStatus = Convert.ToInt32(currentUser.ActvStat);

                    int themeID = 0;
                    DataTable themeInfo = _TaxSecurityService.GetThemeInfoByUser(SessionManager.CurrentClient, SessionManager.CurrentUser.EntityID);

                    if (themeInfo.Rows.Count > 0)
                    {
                        themeID = Convert.ToInt32(themeInfo.Rows[0]["ThemeId"]);
                    }

                    var insertUser = new UserModel()
                    {
                        ClientID = SessionManager.CurrentClient,
                        UserName = currentUser.UserID,
                        FirstName = currentUser.UserNameFirst,
                        LastName = currentUser.UserNameLast,
                        Email = currentUser.Email,
                        ActiveStatus = accountStatus,
                        HierarchyIDs = hierarchy1099K.ToString(),
                        ThemeID = themeID,
                        CreatedBy = currentUser.CreatedBy,
                        UserType = currentUser.UserType
                    };

                    _TaxSecurityService.InsertUser(insertUser);

                    int isViewFullTIN = 0;
                    if (Page.IsUserWithPermission("TaxView") || Page.IsUserWithPermission("MSTaxView"))
                    {
                        isViewFullTIN = 1;
                    }

                    //  Get TAX user
                    user = _TaxSecurityService.GetUser(SessionManager.CurrentClient, SessionManager.CurrentUser.UserID);

                    //  Sync permission to TIN
                    this.Synch1099KPermission(user, hierarchy1099K, isViewFullTIN == 1);

                    if (user != null)
                    {
                        JumpTo1099K(gateUrl, user, ref warningMessage, hierarchy1099K);
                        
                        AS.Common.Logger.LoggerManager.Error(string.Format("Jump2Tin : Create user succesfully on TIN -- [CurrentClient] {0} [CurrentUserType] {1} [CurrentHierarchyId] {2} [UserID] {3}",
                        SessionManager.CurrentUser.ASClient, SessionManager.CurrentUserType, SessionManager.CurrentHierarchyId, SessionManager.CurrentUser.UserID));
                    }
                }
            }
            else
            {
                AS.Common.Logger.LoggerManager.Error(string.Format("Jump2Tin : [CurrentClient] {0} [CurrentUserType] {1} [CurrentHierarchyId] {2} [UserID] {3}",
                    SessionManager.CurrentUser.ASClient, SessionManager.CurrentUserType, SessionManager.CurrentHierarchyId, SessionManager.CurrentUser.UserID));
            }
        }

        uxMsg.Text = warningMessage;
    }

    private bool CheckUserType()
    {
        //Hierachy Secondary only
        return SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.Hierarchy
            && (SessionManager.CurrentUser.EntityID != SessionManager.CurrentUser.UserID);
    }

    private void JumpTo1099K(string gateUrl, User user, ref string warningMessage, int hierarchy1099K)
    {
        if (SessionManager.CurrentSystem == WebSiteConstants.AS_SYSTEM_MS && (string.IsNullOrEmpty(user.EntityID) || string.IsNullOrEmpty(user.EntityType.ToString())))
            return;

        int isViewFullTIN = 0;
        if (Page.IsUserWithPermission("TaxView") || Page.IsUserWithPermission("MSTaxView"))
        {
            isViewFullTIN = 1;
        }
        if (SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.CS)
        {
            this.SyncRoleOfUserWith1099K(user, hierarchy1099K, isViewFullTIN == 1);
        }

        DataRow drHierarchy = GetHierarchyIn1099(SessionManager.CurrentUser.UserID);
        if (drHierarchy != null && drHierarchy["SystemId"] != DBNull.Value)
        {
            string k = _TaxSecurityService.CreateJumpSiteTicket(user.RecId, Request.UserHostAddress, (int)drHierarchy["SystemId"], SessionManager.CurrentUser.RecId);//1: CS sys
            if (k != null)
            {
                var excludeAccessPermissions = new StringBuilder();
                if (!string.IsNullOrEmpty(SessionManager.ExcludeAccessPermission))
                {
                    DataTable dtAccessPermission = this.Get1099KMapping("UserAccessPermission", -1);
                    string[] excludeAccess = SessionManager.ExcludeAccessPermission.Split(',');
                    if (dtAccessPermission != null && dtAccessPermission.Rows.Count > 0)
                    {
                        foreach (string per in excludeAccess)
                        {
                            if (string.IsNullOrEmpty(per)) continue;

                            bool isMap = false;
                            foreach (DataRow dr in dtAccessPermission.Rows)
                            {
                                string per1099k = dr["ValueIn1099K"].ToString();
                                string perCurrent = dr["ValueInGeneric"].ToString();
                                if (per.Equals(perCurrent))
                                {
                                    excludeAccessPermissions.Append("," + per1099k);
                                    isMap = true;
                                }
                            }
                            if (!isMap)
                            {
                                excludeAccessPermissions.Append("," + per);
                            }
                        }
                    }
                }

                gateUrl = gateUrl + "?" + (string.Format("c={0}&u={1}&j={2}&k={3}&hid={4}&ft={5}&eap={6}"
                    , Server.UrlEncode(_TaxSecurityService.EncryptText(user.ASClient.ToString()))
                    , Server.UrlEncode(_TaxSecurityService.EncryptText(user.UserID))
                    , Server.UrlEncode(_TaxSecurityService.EncryptText(SessionManager.CurrentUser.RecId.ToString()))
                    , k
                    , Server.UrlEncode(_TaxSecurityService.EncryptText(hierarchy1099K.ToString()))
                    , isViewFullTIN
                    , Server.UrlEncode(_TaxSecurityService.EncryptText(excludeAccessPermissions.ToString()))
                    ));

                warningMessage = "<h4>" + GetLocalResourceObject("Jum2TinCS_JumpedCompliassure").ToString() + "</h4>";
                Page.ClientScript.RegisterStartupScript(GetType(), "openwindow", "window.open('" + gateUrl + "');", true);
            }
        }
    }

    private DataTable Get1099KMapping(string type, int? asClientID = null)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        asClientID = asClientID != null ? asClientID : SessionManager.CurrentClient;
        parameters.Add("@UserId", SessionManager.CurrentUser.RecId, DbType.Guid);
        parameters.Add("@ASClientID", asClientID, DbType.Int32);
        parameters.Add("@Type", type, DbType.String);

        return WebServices.SecurityServices.GetReports("spa_SEC_GetMapping1099K", parameters);
    }

    private void Synch1099KPermission(User visionWebUser, int hierarchyID, bool isViewFullTIN)
    {
        SecurityService taxService = new SecurityService(SessionManager.CurrentClient);
        FilterParameterCollection parameters = new FilterParameterCollection();
        FilterParameterCollection outParam;

        parameters.Add("@ASClientID", SessionManager.CurrentClient, DbType.Int32);
        parameters.Add("@UserName", visionWebUser.UserID, DbType.String);
        parameters.Add("@HiearchyID", hierarchyID, DbType.Int32);
        parameters.Add("@IsViewFullTIN", isViewFullTIN, DbType.Boolean);

        string userSecRole = string.Empty;
        DataTable dtSecRole = this.Get1099KMapping("UserSecRole");

        if (dtSecRole != null && dtSecRole.Rows.Count > 0)
        {
            dtSecRole.Rows.Cast<DataRow>().ToList().ForEach(x => {
                if (x["ValueInGeneric"].ToString().Equals(visionWebUser.UserSecRole, StringComparison.OrdinalIgnoreCase))
                {
                    userSecRole = x["ValueIn1099K"].ToString();
                }
            });
        }        
        
        parameters.Add("@UserSecRole", userSecRole, DbType.String);
        taxService.ExecuteNonQueryCommand("spa_SEC_SynchUserFromVisionWeb", parameters, out outParam);
    }
    private int GetHierarchyIn1099KByHierarchyID(int hierarchyId)
    {
        int hierarchyIDOf1099K = 0;
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.Add(new AS.Common.DBManager.FilterParameter("@HierarchyId", hierarchyId, DbType.Int32));
        DataTable hierachyIn1099KList = WebServices.CsReportServices.GetReports("spa_SEC_GetHierarchyIn1099KById", parameters);
        if (hierachyIn1099KList != null && hierachyIn1099KList.Rows.Count > 0)
        {
            hierarchyIDOf1099K = Convert.ToInt32(hierachyIn1099KList.Rows[0]["HierarchyID_1099K"]);
        }
        return hierarchyIDOf1099K;
    }
    private void SyncRoleOfUserWith1099K(User taxUser, int hierarchyID, bool isViewFullTIN)
    {
        SecurityService taxService = new SecurityService(SessionManager.CurrentClient);
        var updatetUser = new UserModel()
        {
            ClientID = SessionManager.CurrentClient,
            UserName = taxUser.UserID,
            FirstName = taxUser.UserNameFirst,
            LastName = taxUser.UserNameLast,
            Email = taxUser.Email,
            ActiveStatus = !string.IsNullOrEmpty(taxUser.ActvStat)? int.Parse(taxUser.ActvStat) : 0,
            HierarchyIDs = hierarchyID.ToString(),
            UserType = taxUser.UserType,
            Answer = taxUser.LoginQuestionAnswer
        };

        taxService.UpdateUser(updatetUser, SessionManager.CurrentUserRoles[0].SystemId);
        taxService.SynchPermission(SessionManager.CurrentClient, taxUser.UserID, hierarchyID, isViewFullTIN);
    }
    private DataRow GetHierarchyIn1099(string username)
    {
        SecurityService service = new SecurityService(SessionManager.CurrentClient);
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.Add(new AS.Common.DBManager.FilterParameter("@ASClient", SessionManager.CurrentClient, DbType.Int32));
        parameters.Add(new AS.Common.DBManager.FilterParameter("@UserName", username, DbType.String));
        parameters.Add(new AS.Common.DBManager.FilterParameter("@SystemId", 0, DbType.Int32));
        DataTable dt = service.GetReports("spa_SEC_GetHierarchysForUser", parameters);       
        if (dt == null || dt.Rows.Count == 0) return null;
        return dt.Rows[0];
    }
}
