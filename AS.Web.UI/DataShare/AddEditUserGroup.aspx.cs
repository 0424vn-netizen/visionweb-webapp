using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Pages;
using System;
using System.Data;

[PagePermission("UserGroupMaint,MSUserGroupMaint")]
public partial class AddEditUserGroup : NonReportPage
{
    int UserGroupID
    {
        get
        {
            if (IsSecureQueryString && !string.IsNullOrEmpty(SecureQueryString["UserGroupID"]))
                return int.Parse(SecureQueryString["UserGroupID"]);
            else return 0;
        }
    }

    enum PostBackAction
    {
        Update
    }

    enum DataBindAction
    {
        ToForm
    }

    enum ResultUpdate
    {
        Successed = 1,
        ExistsName = 0,
        NotInactive = 2
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.PageType = SecurePageType.Modal;

        if (!IsPostBack)
        {
            if (this.UserGroupID > 0)
            {
                OnDataBindControls(DataBindAction.ToForm);
            }
        }

        this.Page.Title = this.UserGroupID <= 0 ? GetLocalResourceObject("AddEditUserGroup_aspx_cs_Title").ToString() :
                                                GetLocalResourceObject("AddEditUserGroup_aspx_cs_Title1").ToString();
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.Update:
                UpdateUserGroup();
                break;
        }
    }
    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.ToForm:
                {
                    BindDataForm(this.UserGroupID);
                }
                break;
        }
    }

    #region funcs

    void BindDataForm(int userGroupID)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@GroupID", userGroupID, DbType.Int32));
        DataTable tbGroup = WebServices.CsReportServices.GetReports("spa_cs_DataShare_GetUserGroupByGroupID", parameters);
        if (tbGroup.Rows.Count > 0)
        {
            uxUserGroup.Text = VeraCodeSolution.DoVeraCode(tbGroup.Rows[0]["GroupName"].ToString());
            uxDescription.Text = VeraCodeSolution.DoVeraCode(tbGroup.Rows[0]["Description"].ToString());
            if ((bool)tbGroup.Rows[0]["IsActive"])
                uxActive.Checked = true;
            else
                uxDeActive.Checked = true;
        }
    }

    void UpdateUserGroup()
    {

        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@GroupID", UserGroupID, DbType.Int32));
        parameters.Add(new FilterParameter("@GroupName", uxUserGroup.Text.Trim(), DbType.String));
        parameters.Add(new FilterParameter("@Description", uxDescription.Text.Trim(), DbType.String));
        parameters.Add(new FilterParameter("@IsActive", uxActive.Checked, DbType.Boolean));
        DataTable tbGroup = WebServices.CsReportServices.GetReports("spa_cs_DataShare_AddEditUserGroup", parameters);

        int result = -1;
        int.TryParse(tbGroup.Rows[0]["Result"].ToString(), out result);

        string documentList = tbGroup.Rows[0]["DocumentList"].ToString();
        if (result == (int)ResultUpdate.Successed)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "closemodal", "DoClose(); ", true);
        }
        else if (result == (int)ResultUpdate.ExistsName)
        {
            uxUserGroupMsg.Message = Resources.MessageManager.Field_RequireAndUnique;
            uxlbUserGroup.CssClass = "control-label label-error";
            uxUserGroupMsg.ShowOnLoad = true;
        }
        else if (result == (int)ResultUpdate.NotInactive)
        {
            string queryString = "ConfirmUserGroupModal.aspx?" + BuildSecureQueryString("DocumentList=" + documentList);

            Page.ClientScript.RegisterStartupScript(this.GetType(), "openValidateModal", "parent.ShowPopupModalChild(1,'" + queryString + "', 'auto');", true);
        }
    }

    #endregion

    protected void uxSubmit_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.Update);
    }
}
