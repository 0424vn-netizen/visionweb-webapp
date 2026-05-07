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

public partial class UserControls_Lead_LeadUserProfile : UserProfileBaseUserControl
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
        trBranchs.Visible = IsMSUser;
        if (!Page.IsPostBack)
        {
            if (IsMSUser)
            {
                BindDropDownBranch();
            }
        }
    }

    protected override bool SubmitUser(int clientID, string userId)
    {
        userMaintBusiness = new UserMaintBusiness(this.InitUserInfoObject);

        if (IsMSUser)
        {
            userMaintBusiness.SaveDefaultBranchForUser(uxListBranch.SelectedValue);
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

    private void BindDropDownBranch()
    {
        userMaintBusiness = new UserMaintBusiness(this.InitUserInfoObject);

        SelectedBranch = string.Empty;
        ListItemCollection lstBranchs = new ListItemCollection();
        
        //Bind First Item
        lstBranchs.Add(new ListItem(GetLocalResourceObject("uxListValidQuestionResource1.EmptyMessage").ToString(), ""));

        DataTable tbl = userMaintBusiness.GetBranchByUser().Tables[0];

        if (tbl.HasData())
        {
            for (int i = 0; i < tbl.Rows.Count; i++)
            {
             
                ListItem listItem = new ListItem(tbl.Rows[i]["BranchName"].ToString(), tbl.Rows[i]["BranchId"].ToString());
                lstBranchs.Add(listItem);

                if(tbl.Rows[i]["IsDefaultBranch"].ToString().ToInt() == 1)
                {
                  SelectedBranch = tbl.Rows[i]["BranchId"].ToString();
                }
            }
        }

        uxListBranch.DataSource = lstBranchs;
        uxListBranch.DataBind();

        uxListBranch.SelectedValue = SelectedBranch;
        
    }


    protected override void ReloadUser(int clientID, string userId)
    {
        
    }

    protected override bool ValidateUser(object extraInfo)
    {
        return true;
    }
}

