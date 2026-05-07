using AS.Core.Common.VeraCode;
using AS.Security.WS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class ViewASUserDetail : NonReportPage
{
    private const string POPUP_ATAG = "<a href='javascript:void(0)' style='cursor:pointer' onclick=\"ShowPopupModal('{0}','auto');return false;\">{1}</a>";
    private const string FNAME_ATag = "<a href='{0}' style='cursor:pointer' >{1}</a>";
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
            if (IsSecureQueryString && !string.IsNullOrEmpty(SecureQueryString["u"]))
                return true;
            return false;
        }
    }

    string EditUserName
    {
        get
        {
            if (IsSecureQueryString && !string.IsNullOrEmpty(SecureQueryString["u"]))
                return SecureQueryString["u"].Trim();
            else
                return uxUsername.Text.Trim();
        }
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            OnDataBindControls(DataBindAction.BindPermissions);
            OnDataBindControls(DataBindAction.BindUserDetail);
        }
        pnlbtnEditUser.Visible = IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_EDITASUSERS);
        pnlbtnDeleteUser.Visible = IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_DELETEASUSERS);
        btnEditUser.NavigateUrl = "CreateNewASUser.aspx?" + BuildSecureQueryString("u=" + EditUserName);
        btnDeleteUser.OnClientClick = "return ShowPopupModal('EditASUserConfirmModal.aspx?" + BuildSecureQueryString("u=" + EditUserName + "&action=" + WebSiteEnums.ASUserAction.Delete) + "','auto');";
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

        }
    }


    protected void BindPermissions()
    {
        PermissionCollection accessPermissions = WebServices.SecurityServices.GetPermissionsByASUserGroupType(
               SessionManager.CurrentUser.ASClient,
               SessionManager.CurrentUser.UserID,
                WebSiteConstants.AS_SYSTEM_CS, false);

        uxAccessFunctionPermissions.DataSource = accessPermissions;
        uxAccessFunctionPermissions.DataBind();
    }

    protected void BindUserDetail()
    {
        User user = WebServices.SecurityServices.GetUser(SessionManager.CurrentUser.ASClient, SecureQueryString["u"]);
        uxEmail.Text = VeraCodeSolution.DoVeraCode(user.Email);
        uxStatus.Text = (user.StatusChar == 'Y') ? GetLocalResourceObject("uxActiveResource1.Text").ToString() : GetLocalResourceObject("uxInactiveResource1.Text").ToString();
        uxFirstName.Text = VeraCodeSolution.DoVeraCode(user.UserNameFirst);
        uxLastName.Text = VeraCodeSolution.DoVeraCode(user.UserNameLast);
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
    protected void btnDodeleteUser_Click(object sender, EventArgs e)
    {
        WebServices.SecurityServices.DeleteASUser(SessionManager.CurrentUser.ASClient, EditUserName);

        Response.Redirect("ManageASUsers.aspx");

    }
}