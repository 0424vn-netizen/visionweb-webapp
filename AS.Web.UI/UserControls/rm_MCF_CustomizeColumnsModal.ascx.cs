using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Pages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControls_rm_MCF_CustomizeColumnsModal : GlobalUserControl
{
    const string COL_RESULT = "Result";
    const string COL_ASSIGNMENT_LIST = "AssignmentList";
    const string COL_USER_LIST = "UserList";
    private IEnumerable<ColumnDisplayedConfigurationItem> ColumnConfiguration { get; set; }
    private String ViewData { get; set; }
    protected const string FULL_VIEW = "full view";

    public string CustomViewID
    {
        set { ViewState["CustomViewID"] = value; }
        get
        {
            return (ViewState["CustomViewID"]) != null ? ViewState["CustomViewID"].ToString() : string.Empty;
        }
    }
    public string ViewName { get; set; }
    public int ViewType { get; set; }
    public bool IsCreate
    {
        set { ViewState["IsCreate"] = value; }
        get
        {
            return (ViewState["IsCreate"]).ToBoolean();
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            uxLeftGrid.DataBind();
            uxRightGrid.DataBind();
            DisplayViewType();
        }
    }

    protected void uxSubmit_Click(object sender, EventArgs e)
    {
        if (!ValidateData())
            return;
        var displayedItems = uxRightGrid.Items.Cast<RadListBoxItem>().Select(x => x.Value).ToList();
        ViewData = string.Join(",", displayedItems);
        if (IsCreate)
        {
            AddCustomView();
        }
        else
        {
            if (!ViewType.Equals((int)WebSiteEnums.ManageCustomView.Private) && GetViewType().Equals((int)WebSiteEnums.ManageCustomView.Private))
            {
                DataTable warningData = GeneralFuncsLib.GetWarningEditCustomView(CustomViewID.ToInt());
                if (warningData.Rows.Count > 0 && warningData.Rows[0][COL_RESULT].ToString().Equals("0"))
                {
                    string assignmentList = warningData.Rows[0][COL_ASSIGNMENT_LIST].ToString();
                    string userList = warningData.Rows[0][COL_USER_LIST].ToString();
                    string queryString = Page.BuildSecureQueryString("Mode=2&Data=" + assignmentList + "," + userList);
                    string action = "CustomizeColumnModal.doOpenEditMessagePopup('rm_MCF_MessageCustomModal.aspx?" + queryString + "')";
                    ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript(action);
                    return;
                }
                else
                {
                    UpdateCustomView();
                }
            }
            else
                UpdateCustomView();
        }
        ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript(string.Format(@"window.CustomizeColumnModal.SubmitColumnConfigurationModal();"));
    }

    private void DisplayViewType()
    {
        bool isPublic = Page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RSK_MNG_PUBLIC_VIEW) || Page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RSK_MNG_PUBLIC_VIEW_MS);
        bool isPrivate = isPublic || Page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RSK_MNG_PRIVATE_VIEW) || Page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RSK_MNG_PRIVATE_VIEW_MS);
        rdPublicView.Visible = isPublic;
        rdPrivateView.Visible = isPrivate;
        if (IsCreate)
        {
            rdPublicView.Checked = rdPublicView.Visible;
            rdPrivateView.Checked = !rdPublicView.Checked;
        }
        else
        {
            rdPublicView.Checked = !(rdPrivateView.Checked = ViewType.Equals((int)WebSiteEnums.ManageCustomView.Private));
        }
    }
    private bool ValidateData()
    {
        if (!CheckCustomViewName().Equals(1))
        {
            ShowMessageError(GetLocalResourceObject("ValidationMessages_Require").ToString());
            return false;
        }
        else
            txtNameErrMsg.Message = string.Empty;
        string strRegex = WebSiteConstants.REG_SPECIAL_CHARACTERS;
        Regex re = new Regex(strRegex);
        string customView = txtNameCustomView.Text;
        if (!re.IsMatch(customView))
        {
            ShowMessageError(GetLocalResourceObject("ValidationMessages_V1.Message").ToString());
            return false;
        }
        if (FULL_VIEW.Equals(customView.Trim().ToLower()))
        {
            ShowMessageError(GetLocalResourceObject("ValidationMessages_DuplicateFullView.Message").ToString());
            return false;
        }
        return true;
    }
    private void ShowMessageError(string message)
    {
        txtNameErrMsg.Message = VeraCodeSolution.DoVeraCode(message);
        txtNameErrMsg.ShowOnLoad = true;
        lbtNameCustomView.CssClass = "control-label label-error";
    }
    /// <summary>
    /// Get Unused Columns
    /// </summary>
    private void GetUnusedColumn()
    {
        var data = new List<ColumnDisplayedConfigurationItem>();
        var displayColumns = GenerateTransactionVolumeAnalysis();
        if (ColumnConfiguration != null)
        {
            foreach (var col in ColumnConfiguration)
            {
                if (!displayColumns.Contains(col.ColumnName))
                {
                    data.Add(col);
                }
            }
        }
        uxLeftGrid.DataSource = data.OrderBy(o => o.OrderIndex).ToList();
        uxLeftGrid.DataBind();
    }

    /// <summary>
    /// Get Displayed Columns
    /// </summary>
    private void GetDisplayedColumn()
    {
        var displayColumns = GenerateTransactionVolumeAnalysis();
        var data = new List<ColumnDisplayedConfigurationItem>();
        List<ColumnDisplayedConfigurationItem> items = new List<ColumnDisplayedConfigurationItem>();
        foreach (string item in displayColumns)
        {
            if (ColumnConfiguration != null)
            {
                var col = ColumnConfiguration.FirstOrDefault(a => a.ColumnName == item);
                if (col != null)
                {
                    data.Add(col);
                }
            }
        }
        uxRightGrid.DataSource = data.ToList();
        uxRightGrid.DataBind();
    }

    /// <summary>
    /// Get Data Columns
    /// </summary>
    public void GetData()
    {
        ColumnConfiguration = RiskSessionManager.RiskReportTransactionVolumeColumn;
        GetUnusedColumn();
        GetDisplayedColumn();
    }

    public void RebindData()
    {
        uxRightGrid.DataBind();
    }
    private void AddCustomView()
    {
        var parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(false);
        parameters.Add(new FilterParameter("@UserID", SessionManager.CurrentUser, DbType.String));
        parameters.Add(new FilterParameter("@UserRecID", SessionManager.CurrentUser.RecId, DbType.Guid));
        parameters.Add(new FilterParameter("@ViewName", txtNameCustomView.Text.Trim(), DbType.String));
        parameters.Add(new FilterParameter("@ViewType", GetViewType(), DbType.Int32));
        parameters.Add(new FilterParameter("@ViewData", ViewData, DbType.String));
        WebServices.RiskServices.GetReports("spa_RM_MCF_AddCustomView", parameters);
    }
    private int GetViewType()
    {
        int viewType = (int)WebSiteEnums.ManageCustomView.Public;
        if (!rdPublicView.Checked)
        {
            viewType = (int)WebSiteEnums.ManageCustomView.Private;
        }
        return viewType;
    }
    private int CheckCustomViewName()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        FilterParameterCollection outParameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(false);
        parameters.Add(new FilterParameter("@UserID", SessionManager.CurrentUser, DbType.String));
        parameters.Add(new FilterParameter("@UserRecID", SessionManager.CurrentUser.RecId, DbType.Guid));
        parameters.Add(new FilterParameter("@ViewName", txtNameCustomView.Text.Trim(), DbType.String));
        parameters.Add(new FilterParameter("@ViewType", GetViewType(), DbType.Int32));
        if (!IsCreate)
        {
            parameters.Add(new FilterParameter("@CustomViewID", CustomViewID.ToInt(), DbType.Int32));
        }
        DataTable result = WebServices.RiskServices.GetReports("spa_RM_MCF_CheckCustomViewName", parameters);
        return result.Rows[0].Field<int>(0);
    }
    private DataTable GetCustomDisplayedColumns()
    {
        var parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(false);
        parameters.Add(new FilterParameter("@UserID", SessionManager.CurrentUser, DbType.String));
        parameters.Add(new FilterParameter("@CustomViewID", CustomViewID.ToInt(), DbType.Int32));
        parameters.Add(new FilterParameter("@UserRecID", SessionManager.CurrentUser.RecId, DbType.Guid));
        return WebServices.RiskServices.GetReports("spa_RM_MCF_Get_CustomView", parameters);
    }
    protected void UpdateCustomView()
    {
        var parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(false);
        parameters.Add(new FilterParameter("@UserID", SessionManager.CurrentUser, DbType.String));
        parameters.Add(new FilterParameter("@UserRecID", SessionManager.CurrentUser.RecId, DbType.Guid));
        parameters.Add(new FilterParameter("@ViewName", txtNameCustomView.Text.Trim(), DbType.String));
        parameters.Add(new FilterParameter("@ViewType", GetViewType(), DbType.Int32));
        parameters.Add(new FilterParameter("@ViewData", ViewData, DbType.String));
        parameters.Add(new FilterParameter("@CustomViewID", CustomViewID, DbType.Int32));
        WebServices.RiskServices.GetReports("spa_RM_MCF_UpdateCustomView", parameters);
    }
    private List<string> GenerateTransactionVolumeAnalysis()
    {
        DataTable tb = new DataTable();
        if (!string.IsNullOrEmpty(CustomViewID))
        {
            tb = GetCustomDisplayedColumns();
            string listColumns = tb.Rows[0]["ViewData"].ToString();
            ViewName = tb.Rows[0]["ViewName"].ToString();
            ViewType = tb.Rows[0]["ViewType"].ToInt();
            txtOldNameCustomView.Value = ViewName;
            txtNameCustomView.Text = ViewName;
            string[] columns = listColumns.Split(',');
            for (int i = 0; i < columns.Length; i++)
            {
                columns[i] = columns[i].Trim();
            }
            return columns.ToList<string>();
        }
        else return new List<string>();
    }

    protected void uxSaveEdit_Click(object sender, EventArgs e)
    {
        var displayedItems = uxRightGrid.Items.Cast<RadListBoxItem>().Where(x => string.IsNullOrEmpty(x.Attributes["IsDefault"]))
          .Select(x => x.Value).ToList();
        ViewData = string.Join(",", displayedItems);
        UpdateCustomView();
        ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript(string.Format(@"window.CustomizeColumnModal.SubmitColumnConfigurationModal();"));
    }
}
