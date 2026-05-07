using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Global;
using AS.Controls.Pages;
using System;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

[PagePermission("RskRP,MSRskRP")]
public partial class rm_MCF_ManageCustomViewsModal : ReportPage
{
    const string COL_RESULT = "Result";
    const string COL_ASSIGNMENT_LIST = "AssignmentList";
    const string COL_USER_LIST = "UserList";
    public string CustomViewID
    {
        get
        {
            if (SecureQueryString["CustomViewID"] != null)
            {
                return SecureQueryString["CustomViewID"];
            }
            else
            {
                return string.Empty;
            }
        }
    }

    public string CusViewDeleteId { get; set; }

    public WebSiteEnums.PageModeEnums PageMode
    {
        get
        {
            if (SecureQueryString["PageMode"] != null)
            {

                return (WebSiteEnums.PageModeEnums)Enum.Parse(typeof(WebSiteEnums.PageModeEnums), SecureQueryString["PageMode"]);
            }
            else
            {
                return WebSiteEnums.PageModeEnums.RiskReport;
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;
        string queryString = BuildSecureQueryString("PageMode=" + PageMode);
        btnCreateCustomView.OnClientClick = "return doOpenSubPopup('" + "rm_MCF_CreateCustomViewModal.aspx?" + queryString + "')";
        if (!IsSecureQueryString)
        {
            return;
        }
        if (!Page.IsPostBack)
        {
            GetData();
        }
    }

    protected override void PageInitialize()
    {
        this.GridIDs.Add("uxCustomViewsGrid");
        base.PageInitialize();
        IsBindDataOnLoad = true;
        this.IsSecureCSRF = true;
    }
    protected override void DoGridNeedDataSource(AS.Controls.Grid.ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        if (sender == uxCustomViewsGrid)
        {
            if (IsIntruderDetected) return;
            GetData();
        }
    }
    private DataTable GetCustomViewsData()
    {
        var parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(false);
        parameters.AddLanguageID();
        parameters.Add(new FilterParameter("@UserID", SessionManager.CurrentUser, DbType.String));
        parameters.Add(new FilterParameter("@ViewMode", 1, DbType.Int32));
        parameters.Add(new FilterParameter("@UserRecID", SessionManager.CurrentUser.RecId, DbType.Guid));
        if (uxDisplayViewType.Visible)
        {
            int viewType = rdPublicView.Checked ? 1 : rdPrivateView.Checked ? 2 : -1;
            parameters.Add(new FilterParameter("@ViewType", viewType, DbType.Int32));
        }
        else
            parameters.Add(new FilterParameter("@ViewType", -1, DbType.Int32));
        return WebServices.RiskServices.GetReports("spa_RM_MCF_GetCustomViewList", parameters);
    }

    private void GetData()
    {
        if (string.IsNullOrEmpty(uxCustomViewsGrid.AS_SortExpression))
            uxCustomViewsGrid.AS_SortExpression = "ViewName ASC";

        uxCustomViewsGrid.DataSource = GetCustomViewsData();
        uxCustomViewsGrid.AllowSorting = true;

    }
    protected void uxCustomViewsGrid_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView dataRow = e.Item.DataItem as DataRowView;
            bool canManagePublicView = IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RSK_MNG_PUBLIC_VIEW_MS) || IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RSK_MNG_PUBLIC_VIEW);
            bool canManagePrivate = canManagePublicView || IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RSK_MNG_PRIVATE_VIEW) || IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RSK_MNG_PRIVATE_VIEW_MS);
            // Full view
            if (dataRow["ViewType"].Equals((int)WebSiteEnums.ManageCustomView.FullView) && canManagePublicView)
            {
                string queryString = BuildSecureQueryString("CustomViewID=" + dataRow["CustomViewID"] + "&ViewName=" + dataRow["ViewName"] + "&ViewType=" + dataRow["ViewType"] +
                    "&PageMode=" + PageMode.ToString());
                string urlViewName = "rm_MCF_EditCustomViewModal.aspx?" + queryString;
                string url = "<a href=\"#\" style=\"cursor:pointer\" onclick=\"return doOpenEditFullViewPopup('" + urlViewName + "');\">";
                dataItem["ViewName"].Text = VeraCodeSolution.DoVeraCode(url + dataItem["ViewName"].Text + "</a>");
            }
            else if (dataRow["ViewType"].Equals((int)WebSiteEnums.ManageCustomView.Public) && canManagePublicView) // Public view
            {
                string queryString = BuildSecureQueryString("CustomViewID=" + dataRow["CustomViewID"] + "&ViewName=" + dataRow["ViewName"] + "&ViewType=" + dataRow["ViewType"] +
                    "&PageMode=" + PageMode.ToString() + "&customViewMessageResourceTypeMode=" + GetCustomViewMessageResourceTypeMode.ToString());
                string urlViewName = "rm_MCF_CustomColumnsModal.aspx?" + queryString;
                string url = "<a href=\"#\" style=\"cursor:pointer\" onclick=\"return doOpenEditSubPopup('" + urlViewName + "');\">";
                dataItem["ViewName"].Text = VeraCodeSolution.DoVeraCode(url + dataItem["ViewName"].Text + "</a>");
            }
            else if (dataRow["ViewType"].Equals((int)WebSiteEnums.ManageCustomView.Private) && canManagePrivate) // Public private view
            {
                string queryString = BuildSecureQueryString("CustomViewID=" + dataRow["CustomViewID"] + "&ViewName=" + dataRow["ViewName"] + "&ViewType=" + dataRow["ViewType"] +
                    "&PageMode=" + PageMode.ToString() + "&customViewMessageResourceTypeMode=" + GetCustomViewMessageResourceTypeMode.ToString());
                string urlViewName = "rm_MCF_CustomColumnsModal.aspx?" + queryString;
                string url = "<a href=\"#\" style=\"cursor:pointer\" onclick=\"return doOpenEditSubPopup('" + urlViewName + "');\">";
                dataItem["ViewName"].Text = VeraCodeSolution.DoVeraCode(url + dataItem["ViewName"].Text + "</a>");
            }
            LinkButton btnDelete = (LinkButton)e.Item.FindControl("btnDelete");
            if (dataRow["ViewType"].Equals((int)WebSiteEnums.ManageCustomView.FullView) ||
                (dataRow["ViewType"].Equals((int)WebSiteEnums.ManageCustomView.Public) && (!IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RSK_MNG_PUBLIC_VIEW_MS) && !IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RSK_MNG_PUBLIC_VIEW))))
            {
                btnDelete.Visible = false;
            }
            else
            {
                btnDelete.Visible = true;
            }

            RadioButton rbDisplayType = (RadioButton)e.Item.FindControl("rbDisplayType");
            rbDisplayType.Checked = (dataRow["IsDefault"].ToInt() == (int)WebSiteEnums.ManageCustomView.Public ? true : false);
            rbDisplayType.Attributes.Add("onclick", "UpdateDefaultCustomView('" + CryptorServices.Current.EncryptText(dataRow["CustomViewID"].ToString()) + "',this)");
        }
    }


    [System.Web.Services.WebMethod()]
    public static void UpdateDefaultCustomView(string viewId)
    {
        int customViewID = int.Parse(CryptorServices.Current.DecryptText(viewId));
        var parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(false);
        parameters.Add(new FilterParameter("@UserID", SessionManager.CurrentUser, DbType.String));
        parameters.Add(new FilterParameter("@CustomViewID", customViewID, DbType.Int32));
        parameters.Add(new FilterParameter("@UserRecID", SessionManager.CurrentUser.RecId, DbType.Guid));
        WebServices.RiskServices.GetReports("spa_RM_MCF_UpdateDefaultCustomView", parameters);
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        LinkButton rb = (LinkButton)sender;
        GridDataItem item = (GridDataItem)rb.NamingContainer;

        DataRowView dataRow = item.DataItem as DataRowView;
        HiddenField ctl = item.FindControl("CustomViewID") as HiddenField;

        int customViewID = int.Parse(CryptorServices.Current.DecryptText(ctl.Value));
        string action = string.Empty;
        action = "doOpenEditMessagePopup('rm_MCF_MessageCustomModal.aspx?"
                     + BuildSecureQueryString("customViewMessageResourceTypeMode=" + GetCustomViewMessageResourceTypeMode.ToString() + "&Message=" + GetLocalResourceObject("MessageConfirmDelete").ToString()) + "','" + customViewID.ToString() + "')";
        ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript(action);


    }
    protected void uxRebindCustomViewsGrid_Click(object sender, EventArgs e)
    {
        uxCustomViewsGrid.Rebind();
        AjaxAddResponseScript("AdjustModalSize();");
    }

    protected void uxDeleteCustomView_Click(object sender, EventArgs e)
    {
        int customViewID = int.Parse(hddDeleteValue.Value);
        var parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(false);
        parameters.Add(new FilterParameter("@CustomViewID", customViewID, DbType.Int32));
        WebServices.RiskServices.GetReports("spa_RM_MCF_DeleteCustomView", parameters);

        uxCustomViewsGrid.Rebind();
        AjaxAddResponseScript("AdjustModalSize();");
    }
}