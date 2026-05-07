using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using AS.Security.WS.Entities;
using AS.Common;
using Telerik.Web.UI;
using AS.Common.DBManager;
using AS.Controls.Pages;

using AS.Security.WS.Entities;
using System.Collections.Generic;
using AS.Controls.Pages;
using AS.Tax.Security.Web.Services;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Text;
using AS.Common.DataProtection;
using System.Reflection;

public partial class UserControls_ManageASUser : GlobalUserControl
{
    enum DataBindAction
    {
        BindUserDetail,
        BindPermissions,
    }
    enum PostBackAction
    {
        SaveUserDetailClicked
    }

    #region Properties
    public bool IsUpdateMode
    {
        get
        {
            if (Page.IsSecureQueryString && !string.IsNullOrEmpty(Page.SecureQueryString["u"]))
                return true;
            return false;
        }
    }

    public string EditUserName
    {
        get
        {
            if (Page.IsSecureQueryString && !string.IsNullOrEmpty(Page.SecureQueryString["u"]))
                return Page.SecureQueryString["u"].Trim();
            else
                return uxUsername.Text.Trim();
        }
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        uxUsername.Visible = !IsUpdateMode;
        uxUsernameOld.Visible = IsUpdateMode;
        uxSaveEdit.Visible = IsUpdateMode;
        uxSave.Visible = !IsUpdateMode;

        if (!IsPostBack)
        {
            OnDataBindControls(DataBindAction.BindPermissions);
            if (IsUpdateMode)
            {
                OnDataBindControls(DataBindAction.BindUserDetail);

            }
        }
        uxActive.Enabled = uxInactive.Enabled = Page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_ACTIVATEDEACTIVATEASUSERS);

    }
    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindUserDetail:
                BindUserDetail();
                break;
            case DataBindAction.BindPermissions:
                BindPermissions();
                break;
        }
    }
    protected override void OnPostBackActions(Enum type, object param)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.SaveUserDetailClicked:
                if (IsUpdateMode)//modify use
                    UpdateUserProfile();
                else //create new secondary user
                    CreateNewSecondaryUser();
                break;
        }
    }


    protected void BindPermissions()
    {
        PermissionCollection accessPermissions = WebServices.SecurityServices.GetPermissionsByASUserGroupType(
               SessionManager.CurrentUser.ASClient,
               SessionManager.CurrentUser.UserID,
                WebSiteConstants.AS_SYSTEM_CS);

        uxAccessFunctionPermissions.DataSource = accessPermissions;
        uxAccessFunctionPermissions.DataBind();


    }

    protected void BindUserDetail()
    {
        uxCreateMode.Visible = !IsUpdateMode;
        uxUsernameOld.Visible = IsUpdateMode;
        User user = WebServices.SecurityServices.GetUser(SessionManager.CurrentUser.ASClient, Page.SecureQueryString["u"]);
        uxEmail.Text = VeraCodeSolution.DoVeraCode(user.Email);
        uxActive.Checked = (user.StatusChar == 'Y') ? true : false;
        uxInactive.Checked = (user.StatusChar != 'Y') ? true : false;
        uxFirstName.Text = VeraCodeSolution.DoVeraCode(user.UserNameFirst);
        uxLastName.Text = VeraCodeSolution.DoVeraCode(user.UserNameLast);
        uxUsernameOld.Text = VeraCodeSolution.DoVeraCode(user.UserID);
        uxUsername.Text = VeraCodeSolution.DoVeraCode(user.UserID);
        //
        if (IsUpdateMode)
        {
            PermissionCollection ex_pers = new PermissionCollection();
            ex_pers = WebServices.SecurityServices.GetPermissionsForUser(SessionManager.CurrentUser.ASClient, EditUserName);

            foreach (Permission a in ex_pers)
            {
                SetSelectItem(a.PermissionId, uxAccessFunctionPermissions.Items);
            }
        }
    }
    private void SetSelectItem(int p, ListItemCollection listItemCollection)
    {
        var t = listItemCollection.FindByValue(p.ToString());
        if (t != null)
            t.Selected = true;
    }
    protected void UpdateUserProfile()
    {
        User user = WebServices.SecurityServices.GetUser(SessionManager.CurrentUser.ASClient, Page.SecureQueryString["u"]);
        user.UserNameFirst = this.uxFirstName.Text.Trim();
        user.UserNameLast = this.uxLastName.Text.Trim();
        user.UserNameFull = user.UserNameFirst + " " + user.UserNameLast;
        user.Email = this.uxEmail.Text.Trim();
        user.Status = uxActive.Checked ? "1" : "0";
        user.SiteID = SessionManager.CurrentUser.SiteID;
        string[] roles = new string[] { SessionManager.CurrentUserRoles[0].HierarchyID.ToString() };
        WebServices.SecurityServices.UpdateUser(user, SessionManager.CurrentSystem, roles, string.Empty, string.Empty);
        InsertOrDeletePermissionForUser(user, false, uxAccessFunctionPermissions);
        Response.Redirect("ManageASUsers.aspx");
    }

    protected void CreateNewSecondaryUser()
    {
        bool isExistUserName = WebServices.SecurityServices.IsExistedUserName(SessionManager.CurrentUser.ASClient, uxUsername.Text.Trim());
        ShowMessageBox(isExistUserName);
        if (isExistUserName) return;

        string[] roles = new string[] { SessionManager.CurrentUserRoles[0].HierarchyID.ToString() };

        //create new user
        User user = new User();
        //assigne value
        user.UserID = uxUsername.Text.Trim();
        user.UserNameFirst = this.uxFirstName.Text.Trim();
        user.UserNameLast = this.uxLastName.Text.Trim();
        user.UserNameFull = uxFirstName.Text.Trim() + " " + uxLastName.Text.Trim();
        user.Email = this.uxEmail.Text.Trim();
        user.Status = uxActive.Checked ? "1" : "0";
        user.UserPassword = WebServices.SecurityServices.GenPwd();
        user.UserPasswordType = 10;
        user.LoginQuestionIndex = 1;
        user.UserSecRole = SessionManager.CurrentUser.UserSecRole;

        //User Sec Role
        user.CreatedBy = SessionManager.CurrentUser.RecId;

        user.ASClient = SessionManager.CurrentUser.ASClient;
        user.SiteID = SessionManager.CurrentUser.SiteID;

        WebServices.SecurityServices.CreateUser(user, roles);
        //General password
        SessionManager.ResetPasswordUser = user;
        InsertOrDeletePermissionForUser(user, false, uxAccessFunctionPermissions);
        ((BaseMasterPage)Page.Master).AjaxAddResponseScript("ShowPopupModal('CreateNewUser2.aspx', 'auto');");

    }

    private void InsertOrDeletePermissionForUser(User user, bool isLogged, CheckBoxList ckbList)
    {
        for (int i = 0; i < ckbList.Items.Count; i++)
        {
            if (!ckbList.Items[i].Selected)
            {
                WebServices.SecurityServices.InsertExcludePermissionForUser(user.ASClient, user.UserID, Int32.Parse(ckbList.Items[i].Value), DateTime.Now, DateTime.Now, SessionManager.CurrentUser.RecId, isLogged);
            }
            else
            {
                WebServices.SecurityServices.DeleteExcludePermissionForUser(user.ASClient, user.UserID, Int32.Parse(ckbList.Items[i].Value), SessionManager.CurrentUser.RecId, isLogged);
            }
        }
    }

    protected void uxSave_Click(object sender, EventArgs e)
    {
        if (Page.IsIntruderDetected) return;
        OnPostBackActions(PostBackAction.SaveUserDetailClicked);
    }

    protected void ShowMessageBox(bool isClear)
    {
        if (!isClear)
        {
            txtUserNameErrMsg.Message = string.Empty;
            txtUsernameLabel.CssClass = "control-label";
        }
        else
        {
            txtUserNameErrMsg.Message = AS.Common.VeraCodeSolution.DoVeraCode(GetLocalResourceObject("ManageASUserASCX_Text_RequiredAndUnique.Message").ToString());
            txtUserNameErrMsg.ShowOnLoad = true;
            txtUsernameLabel.CssClass = "control-label label-error";
        }
    }
}
