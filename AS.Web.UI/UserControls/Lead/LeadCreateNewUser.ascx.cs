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
using System.Reflection;
using System.Linq;

public partial class UserControls_Lead_LeadCreateNewUser : CreateNewUserBaseUserControl
{
    #region Properties
    private static bool isFirstLoad = true;
    private bool IsAssigneBankBranch = false;//to show hide hyperlink View Bank or Add Bank
    private bool IsGetAllBankBranchData = false;//to show hide hyperlink View Bank or Add Bank
    private bool IsMSUser = SessionManager.CurrentSystem == WebSiteConstants.AS_SYSTEM_MS;
    private static string secondaryUserRecId = "00000000-0000-0000-0000-000000000000";
    private UserMaintBusiness userMaintBusiness = null;
    private int CurrentSystemId = SessionManager.CurrentSystem;
    #endregion

    #region Base function
    const string PERMISSION_ACCESS_FUNC_DO_REPO = "DocumentAccess";
    const string PERMISSION_ACCESS_FUNC_LEAD = "LeadsAccess";
    const string PERMISSION_ACCESS_ALL_BANK = "TIBAccessAllBanks";
    const string PERMISSION_ACCESS_ALL_BRANCH = "TIBMSAccessAllBranchs";
    const string TYPE_BANK = "bank";
    const string TYPE_BRANCH = "branch";

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

    protected override void LoadUser(int clientID, string userId, bool isUpdateMode, string[] permissionCodes, PermissionCollection accessPermissions)
    {
        this.IsUpdateMode = isUpdateMode;

        isFirstLoad = !IsPostBack;

        IsGetAllBankBranchData = IsGetAllBankBranch();

        userMaintBusiness = new UserMaintBusiness(this.InitUserInfoObject);


        //Switch System mode in CS page
        if (GeneralFuncsLib.HasMSUserManagementFeature)
        {
            SecurePage _parentPage = (SecurePage)this.Page;
            if (_parentPage != null)
            {
                if (isUpdateMode && !IsMSUser)
                {
                    // Get system of this user when the Current System is CS
                    int systemId = 0;
                    int.TryParse(_parentPage.SecureQueryString["systemId"], out systemId);
                    CurrentSystemId = systemId;
                    IsMSUser = systemId == WebSiteConstants.AS_SYSTEM_MS;
                }
            }
        }

        if (!isUpdateMode)
        {
            secondaryUserRecId = "00000000-0000-0000-0000-000000000000";
        }
        else
        {
            secondaryUserRecId = userId;
        }

       

        InitBankData(IsPostBack);

        if (!IsMSUser)//CS user
        {
            bool isAssigneBankBranch = !IsGetAllBankBranchData;

            uxAddBank.Visible = isAssigneBankBranch;
            uxViewBank.Visible = !isAssigneBankBranch;
            //Disable branch 
            Validator_Bank.Visible = true;
            Validator_Branch.Visible = false;
            uxAddBank.OnClientClick = "return ShowPopupModal('ManageBank_Modal.aspx?" + BuildSecureQueryString(TYPE_BANK, false) + "','auto')";
            uxViewBank.OnClientClick = "return ShowPopupModal('ManageBank_Modal.aspx?" + BuildSecureQueryString(TYPE_BANK, true) + "','auto')";
        }
        else
        {
            //todo: add resource
            uxViewBank.Visible = IsGetAllBankBranchData;
            uxAddBank.Visible = !IsGetAllBankBranchData;

            uxAddBank.OnClientClick = "return ShowPopupModal('ManageBank_Modal.aspx?" + BuildSecureQueryString(TYPE_BRANCH, false) + "','auto')";
            uxAddBank.Text = GetLocalResourceObject("Hyperlink_Add_Branch").ToString();

            if (IsGetAllBankBranchData)
            {
                uxViewBank.OnClientClick = "return ShowPopupModal('ManageBank_Modal.aspx?" + BuildSecureQueryString(TYPE_BRANCH, true) + "','auto')";
                uxViewBank.Text = GetLocalResourceObject("Hyperlink_View_Branch.Text").ToString();
            }


            Validator_Branch.Visible = true;
            Validator_Bank.Visible = false;
        }

        this.RegisterClientValidationFunction(this.Controls);

        if (!IsPostBack)
        {
            //Bind Lead Access Function
            var per_DocRepo = (from Permission a in accessPermissions.Cast<Permission>() where a.GroupFuncName.Equals(PERMISSION_ACCESS_FUNC_DO_REPO) select a).ToList();
            var per_Lead = (from Permission a in accessPermissions.Cast<Permission>() where a.GroupFuncName.Equals(PERMISSION_ACCESS_FUNC_LEAD) select a).ToList();

            if (per_DocRepo.Count > 0)
            {
                //43745: Load Exclude Access Permission for Aperia User.
                if (!string.IsNullOrEmpty(SessionManager.ExcludeAccessPermission))
                {
                    string[] excludeAccessPermissions = SessionManager.ExcludeAccessPermission.Split(',');
                    foreach (string perAccess in excludeAccessPermissions)
                    {
                        if (string.IsNullOrEmpty(perAccess)) continue;
                        foreach (Permission permission in per_DocRepo)
                        {
                            if (permission.PermissionCode.Equals(perAccess))
                            {
                                per_DocRepo.Remove(permission);
                                break;
                            }
                        }
                    }
                }

                uxAFDocRepoAccess.DataSource = per_DocRepo;
                uxAFDocRepoAccess.DataBind();

                if (per_DocRepo.Count <= 0)
                    uxtrDocRepoAccess.Attributes["class"] = uxtrDocRepoAccess.Attributes["class"] + " hide";
                else
                    uxtrDocRepoAccess.Attributes["class"] = uxtrDocRepoAccess.Attributes["class"] != null ? uxtrDocRepoAccess.Attributes["class"].Replace("hide", "") : "";
            }
            else
            {
                uxtrDocRepoAccess.Attributes["class"] = uxtrDocRepoAccess.Attributes["class"] + " hide";
            }

            if (per_Lead.Count > 0)
            {
                uxAFLeadAccess.DataSource = per_Lead;
                uxAFLeadAccess.DataBind();
                uxtrLeadAccess.Attributes["class"] = uxtrLeadAccess.Attributes["class"] != null ? uxtrLeadAccess.Attributes["class"].Replace("hide", "") : "";
            }
            else
            {
                uxtrLeadAccess.Attributes["class"] = uxtrLeadAccess.Attributes["class"] + " hide";
            }

            SetEnableCheckListPer();

            if (isUpdateMode)
            {
                //Bind Permission 
                if (SessionManager.UserGroupPermissions != null)
                {
                    foreach (Permission a in SessionManager.UserGroupPermissions.Cast<Permission>())
                    {
                        SetSelectItem(a.PermissionId, uxAFDocRepoAccess.Items);
                        SetSelectItem(a.PermissionId, uxAFLeadAccess.Items);
                    }
                }
            }

           
        }
    }

    protected override void ReloadUser(int clientID, string userId, string[] permissionCodes)
    {
        if (!IsUpdateMode)
            secondaryUserRecId = "00000000-0000-0000-0000-000000000000";

        InitBankData(false);

        //Visible Bank Control
        bool bCheck = true;
        if (SessionManager.CurrentUser.UserSecRole == "MSUSERS")
        {
            bCheck = IsGetAllBankBranchData;
        }

        this.LoadResponseScript(bCheck);

        //Set Bank/Branch Hyper link data when role change;
        SetBankHyperLink();

        SetEnableCheckListPer();
    }

    protected override bool ValidateUser(object extraInfo)
    {
        bool isValid = false;
        if (!IsMSUser)//CS
        {
            if (!IsGetAllBankBranchData)
            {
                DataSet ds = userMaintBusiness.GetBanks(secondaryUserRecId, SessionManager.UniqueSessionID);
                isValid = ds.Tables[1].Rows.Count > 0;
            }
            else
            {
                isValid = true;
            }
        }
        else//MS
        {
            DataSet ds = userMaintBusiness.GetBranchs(secondaryUserRecId, SessionManager.UniqueSessionID);
            isValid = ds.Tables[1].Rows.Count > 0;
        }

        if (!isValid)
        {
            hdfSelectedBankErrMsg.Message = GetLocalResourceObject("Validator_Bank.Message").ToString();
            hdfSelectedBankErrMsg.ShowOnLoad = true;
            lblBankLabel.CssClass = "control-label label-error";
        }

        return isValid;
    }

    protected override bool SubmitUser(int clientID, string userId, string[] permissionCodes)
    {
        //Insert/Update permission for access function
        InsertOrDeletePermissionForUser(clientID, userId, IsAssigneBankBranch, uxAFDocRepoAccess);
        InsertOrDeletePermissionForUser(clientID, userId, IsAssigneBankBranch, uxAFLeadAccess);

        User newuser = WebServices.SecurityServices.GetUser(SessionManager.CurrentUser.ASClient, userId);

        if (!IsMSUser)//CS
        {
            if (!IsGetAllBankBranchData)
            {
                userMaintBusiness.SaveBanks(secondaryUserRecId, newuser.RecId.ToString(), SessionManager.UniqueSessionID);
            }
        }
        else //MS
        {
            if (newuser.Status.EqualTo("0"))
            {
                userMaintBusiness.UnAssignLeadForMSBankUser(newuser.RecId.ToString());
            }

            userMaintBusiness.SaveBranch(secondaryUserRecId, newuser.RecId.ToString(), SessionManager.UniqueSessionID);
        }
        return true;
    }

    #endregion

    #region Business function
    //Get parent page control (Manage User page)
    private Control parentControl
    {
        get
        {
            return this.Parent.Parent.Parent;
        }
    }

    private string BuildSecureQueryString(string type, bool IsViewMode)
    {
        string query = string.Empty;
        if (type == TYPE_BANK)
        {
            if (IsViewMode)
                query = "isAddMode=false&type=bank&all=" + IsGetAllBankBranchData + "&recId=" + secondaryUserRecId + "&systemId=" + CurrentSystemId;
            else
                query = "isAddMode=true&type=bank&all=" + IsGetAllBankBranchData + "&recId=" + secondaryUserRecId + "&systemId=" + CurrentSystemId;
        }
        else
        {
            if (IsViewMode)
                query = "isAddMode=false&type=branch&all=" + IsGetAllBankBranchData + "&recId=" + secondaryUserRecId + "&systemId=" + CurrentSystemId;
            else
                query = "isAddMode=true&type=branch&all=" + IsGetAllBankBranchData + "&recId=" + secondaryUserRecId + "&systemId=" + CurrentSystemId;
        }

        string queryString = string.Empty;
        SecurePage _parentPage = (SecurePage)this.Page;
        if (_parentPage != null)
        {
            queryString = _parentPage.BuildSecureQueryString(query);
        }
        return queryString;
    }

    private void SetEnableCheckListPer()
    {
        PermissionCollection enPers = SessionManager.RoleGroupPermissions;
        string[] hierachyArg = GetRoleSelected().Split('/');
        if (enPers != null && hierachyArg.Length > 1)
        {
            List<ListItem> lstListItem = new List<ListItem>();

            foreach (ListItem litem in uxAFDocRepoAccess.Items)
            {
                lstListItem.Add(litem);
            }
            foreach (ListItem litem in uxAFLeadAccess.Items)
            {
                lstListItem.Add(litem);
            }

            for (int i = 0; i < lstListItem.Count; i++)
            {
                lstListItem[i].Selected = false;
            }

            bool isEnable = false;

            for (int i = 0; i < lstListItem.Count; i++)
            {
                isEnable = false;
                for (int j = 0; j < enPers.Count; j++)
                {
                    int _permissionId = int.Parse(lstListItem[i].Value);
                    if (_permissionId == enPers[j].PermissionId)
                    {
                        isEnable = true;
                        break;
                    }
                }

                lstListItem[i].Enabled = isEnable;
            }
        }

    }

    private void LoadResponseScript(bool isShow)
    {
        ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript(string.Format("ShowHideTR_CC('"
            + this.TIB_TR_Bank.ClientID + "','" + isShow.ToString().ToLower() + "')"));
    }

    private string GetRoleSelected()
    {
        RadComboBox rcbCurrentRole = (RadComboBox)parentControl.FindControl("uxRole");
        if (rcbCurrentRole != null)
            return rcbCurrentRole.SelectedValue;
        return string.Empty;
    }

    //private bool GetAllRole()
    //{
    //    string selectedRole = GetRoleSelected().ToLower().Split('/')[0];
    //    string[] allRoles = ALL_ROLE.Split(',');
    //    return allRoles.Where(x => x.ToLower().Equals(selectedRole)).Count() > 0;
    //}

    private bool IsGetAllBankBranch()
    {
        string[] hierachyArg = GetRoleSelected().ToLower().Split('/');
        PermissionCollection persInHierarchy = WebServices.SecurityServices.GetPermissionsInHierarchy(Convert.ToInt32(hierachyArg[0])) ?? new PermissionCollection();
        var pers = persInHierarchy.Cast<Permission>();

        if (pers.Any(m => m.PermissionCode == PERMISSION_ACCESS_ALL_BANK) || pers.Any(m=>m.PermissionCode == PERMISSION_ACCESS_ALL_BRANCH))
            return true;
        return false;
    }

    private void InitBankData(bool isPostBack)
    {
        if (!isPostBack)
        {
            if (IsUpdateMode || (!IsUpdateMode && isFirstLoad))
            {
                if (!IsMSUser)
                {//CS user
                    if (!IsGetAllBankBranchData) //Manager/Admin
                    {
                        userMaintBusiness.TransferStagingBanks(secondaryUserRecId, SessionManager.UniqueSessionID);
                        isFirstLoad = false;
                    }
                }
                else
                {
                    userMaintBusiness.TransferStagingBranchs(secondaryUserRecId, SessionManager.UniqueSessionID);
                    isFirstLoad = false;
                }
            }
        }

        if (!IsMSUser)
        {//CS user
            int totalRows = 0;

            if (IsGetAllBankBranchData) //Manager/Admin
            {
                DataTable banks = LoadAllBank();
                totalRows = banks.Rows.Count;
                hdfSelectedBank.Value = "true";
            }
            else //User
            {

                DataSet ds = userMaintBusiness.GetBanks(secondaryUserRecId, SessionManager.UniqueSessionID);

                totalRows = ds.Tables[1].Rows.Count;
                hdfSelectedBank.Value = totalRows > 0 ? "true" : "false";
            }
            LiteralCountSelectedBank.Text = totalRows.ToString();
        }
        else
        {
            int totalRows = 0;
            if (IsGetAllBankBranchData) //Manager/Admin
            {
                DataTable branchs = LoadAllBranch();
                totalRows = branchs.Rows.Count;
            }
            else //User
            {
                DataSet ds = userMaintBusiness.GetBranchs(secondaryUserRecId, SessionManager.UniqueSessionID);
                totalRows = ds.Tables[1].Rows.Count;
            }

            hdfSelectedBank.Value = totalRows > 0 ? "true" : "false";
            LiteralCountSelectedBank.Text = totalRows.ToString();

            //Load resource for Branch
            LoadBranchResource();
        }

    }

    private void LoadBranchResource()
    {
        lblBankLabel.Text = GetLocalResourceObject("Label_Branch").ToString();
        lbCountSelectedBank.Text = GetLocalResourceObject("Lablel_Branch_Selected").ToString();
    }

    private DataTable LoadAllBank()
    {
        DataSet ds = userMaintBusiness.GetAllBanks(secondaryUserRecId, SessionManager.UniqueSessionID);

        return ds.Tables[0];
    }

    private DataTable LoadAllBranch()
    {
        DataSet ds = userMaintBusiness.GetAllBranchs(secondaryUserRecId);

        return ds.Tables[0];
    }

    private void InsertOrDeletePermissionForUser(int clientId, string userId, bool isLogged, CheckBoxList ckbList)
    {
        for (int i = 0; i < ckbList.Items.Count; i++)
        {
            if (!ckbList.Items[i].Selected)
            {
                WebServices.SecurityServices.InsertExcludePermissionForUser(clientId, userId, Int32.Parse(ckbList.Items[i].Value), DateTime.Now, DateTime.Now, SessionManager.CurrentUser.RecId, isLogged);
            }
            else
            {
                WebServices.SecurityServices.DeleteExcludePermissionForUser(clientId, userId, Int32.Parse(ckbList.Items[i].Value), SessionManager.CurrentUser.RecId, isLogged);
            }
        }
    }
    //End Access function region

    private void SetBankHyperLink()
    {
        if (!IsMSUser)//CS user
        {
            uxAddBank.OnClientClick = "return ShowPopupModal('ManageBank_Modal.aspx?" + BuildSecureQueryString(TYPE_BANK, false) + "','auto')";
            uxViewBank.OnClientClick = "return ShowPopupModal('ManageBank_Modal.aspx?" + BuildSecureQueryString(TYPE_BANK, true) + "','auto')";
        }
        else
        {
            uxAddBank.OnClientClick = "return ShowPopupModal('ManageBank_Modal.aspx?" + BuildSecureQueryString(TYPE_BRANCH, false) + "','auto')";
        }
    }

    private void SetSelectItem(int p, ListItemCollection listItemCollection)
    {
        var t = listItemCollection.FindByValue(p.ToString());
        if (t != null)
            t.Selected = true;
    }
    #endregion
}