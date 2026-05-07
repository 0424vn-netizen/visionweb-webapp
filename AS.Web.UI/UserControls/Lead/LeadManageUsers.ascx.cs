using AS.Controls.Pages;
using AS.Core.Common;
using AS.Leads.UserMaintService;
using AS.Security.WS.Entities;
using AS.VW.Share.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using AS.Core.Common.Utilities;
using AS.Controls.Global;

public partial class UserControls_Lead_LeadManageUsers : ManageUsersBaseUserControl
{
    #region Properties
    private bool IsMSUser = SessionManager.CurrentSystem == WebSiteConstants.AS_SYSTEM_MS;

    private bool IsBankAdminRole = SessionManager.CurrentUserRoles[0].HierarchyCode == WebSiteConstants.BANK_ADMIN_HIERACHY_CODE;

    
    private UserMaintBusiness userMaintBusiness = null;
   
    private static string SelectedBranch = string.Empty;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        userMaintBusiness = new UserMaintBusiness(this.InitUserInfoObject);
    }

    protected override void LoadUser(int clientID, string userId)
    {
       
        
    }

    protected override bool SubmitUser(int clientID, string userId, int userType, string status)
    {
        if(userType ==  WebSiteConstants.AS_SYSTEM_MS && status.EqualTo("0"))
        {
            userMaintBusiness.UnAssignLeadForMSBankUser(userId);
        }

        return true;
    }
    
    private UserInfo InitUserInfoObject
    {
        get
        {
            UserInfo userInfo = new UserInfo();
            userInfo.AsClientId = SessionManager.CurrentUser.ASClient;
            userInfo.SiteId = SessionManager.CurrentUser.SiteID;
            userInfo.UserId = SessionManager.CurrentUser.UserID;
            userInfo.UserMode = String.Empty;
            userInfo.UserSessionId = SessionManager.UniqueSessionID;
            userInfo.RecId = SessionManager.CurrentUser.RecId;
            return userInfo;
        }
    }

    protected override void ReloadUser(int clientID, string userId)
    {
        
    }

    protected override bool ValidateUser(object extraInfo)
    {
        return true;
    }
}

