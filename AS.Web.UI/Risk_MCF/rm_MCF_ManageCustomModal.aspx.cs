using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Global;
using AS.Controls.Pages;
using System;
using System.Data;
using Telerik.Web.UI;

[PagePermission("RskRP,MSRskRP")]
public partial class rm_MCF_ManageCustomModal : ReportPage
{
    const string COL_RESULT = "Result";
    const string COL_ASSIGNMENT_LIST = "AssignmentList";
    const string COL_USER_LIST = "UserList";
    enum PostBackAction
    {
        BindCustomViewGrid
    }

    public string AssignmentID
    {
        get
        {
            if (SecureQueryString["AssignmentID"] != null)
            {
                return SecureQueryString["AssignmentID"];
            }
            else
            {
                return string.Empty;
            }
        }
    }

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
                return WebSiteEnums.PageModeEnums.Assignment;
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsSecureQueryString) return;

        // Show hide Public or Private Mode
        uxDisplayViewType.Visible = PageMode == WebSiteEnums.PageModeEnums.DetectionQueue;
        string queryString = BuildSecureQueryString("PageMode=" + PageMode);
        btnCreateCustomView.OnClientClick = "return doOpenSubPopup('" + "rm_MCF_CreateManageCustomViewModal.aspx?" + queryString + "')";
        bool isPermissionCreate = PageMode == WebSiteEnums.PageModeEnums.Assignment ? this.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RSK_MNG_PUBLIC_VIEW_MS) || this.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RSK_MNG_PUBLIC_VIEW) :
            this.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RSK_MNG_PRIVATE_VIEW) || this.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RSK_MNG_PRIVATE_VIEW_MS) ||
        this.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RSK_MNG_PUBLIC_VIEW_MS) || this.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RSK_MNG_PUBLIC_VIEW);

        btnCreateCustomView.Visible = isPermissionCreate;

        if (!IsPostBack)
            hddCustomViewDefault.Value = CryptorServices.Current.EncryptText(CustomViewID);
    }

    protected override void PageInitialize()
    {
        PageType = SecurePageType.Modal;
        this.GridIDs.Add("uxCustomViewsGrid");
        IsBindDataOnLoad = true;
        base.PageInitialize();
    }

    protected override void DoGridNeedDataSource(AS.Controls.Grid.ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        if (sender == uxCustomViewsGrid)
        {
            OnPostBackActions(PostBackAction.BindCustomViewGrid);
        }
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        if (IsIntruderDetected) return;
        switch ((PostBackAction)type)
        {
            case PostBackAction.BindCustomViewGrid:
                DataTable data = GetCustomViewsData();
                uxCustomViewsGrid.DataSource = data;
                break;
        }
    }
    private DataTable GetCustomViewsData()
    {
        var parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParamsWithRecId();
        parameters.AddLanguageID();
        parameters.Add(new FilterParameter("@AssignmentID", AssignmentID, DbType.String));
        parameters.Add(new FilterParameter("@ViewMode", 4, DbType.Int32));
        parameters.Add(new FilterParameter("@PageType", "Assignment", System.Data.DbType.String));
        if (uxDisplayViewType.Visible)
        {
            int viewType = rdPublicView.Checked ? 1 : rdPrivateView.Checked ? 2 : -1;
            parameters.Add(new FilterParameter("@ViewType", viewType, DbType.Int32));
        }
        else
            parameters.Add(new FilterParameter("@ViewType", 1, DbType.Int32));

        return WebServices.RiskServices.GetReports("spa_RM_MCF_GetCustomViewList", parameters);
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
                string urlViewName = "rm_MCF_EditManageCustomModal.aspx?" + queryString;
                string url = "<a href=\"#\" style=\"cursor:pointer\" onclick=\"return doOpenEditFullViewPopup('" + urlViewName + "');\">";
                dataItem["ViewName"].Text = VeraCodeSolution.DoVeraCode(url + dataItem["ViewName"].Text + "</a>");
            }
            else if (dataRow["ViewType"].Equals((int)WebSiteEnums.ManageCustomView.Public) && canManagePublicView) // Public view
            {
                string queryString = BuildSecureQueryString("CustomViewID=" + dataRow["CustomViewID"] + "&ViewName=" + dataRow["ViewName"] + "&ViewType=" + dataRow["ViewType"] +
                    "&PageMode=" + PageMode.ToString());
                string urlViewName = "rm_MCF_CustomColumnsModal.aspx?" + queryString;
                string url = "<a href=\"#\" style=\"cursor:pointer\" onclick=\"return doOpenEditSubPopup('" + urlViewName + "');\">";
                dataItem["ViewName"].Text = VeraCodeSolution.DoVeraCode(url + dataItem["ViewName"].Text + "</a>");
            }
            else if (dataRow["ViewType"].Equals((int)WebSiteEnums.ManageCustomView.Private) && canManagePrivate) // Public private view
            {
                string queryString = BuildSecureQueryString("CustomViewID=" + dataRow["CustomViewID"] + "&ViewName=" + dataRow["ViewName"] + "&ViewType=" + dataRow["ViewType"] +
                    "&PageMode=" + PageMode.ToString());
                string urlViewName = "rm_MCF_CustomColumnsModal.aspx?" + queryString;
                string url = "<a href=\"#\" style=\"cursor:pointer\" onclick=\"return doOpenEditSubPopup('" + urlViewName + "');\">";
                dataItem["ViewName"].Text = VeraCodeSolution.DoVeraCode(url + dataItem["ViewName"].Text + "</a>");
            }

            LinkButton btnDelete = (LinkButton)e.Item.FindControl("btnDelete");
            if (dataRow["ViewType"].Equals((int)WebSiteEnums.ManageCustomView.FullView) ||
                (dataRow["ViewType"].Equals((int)WebSiteEnums.ManageCustomView.Public) &&
                (!IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RSK_MNG_PUBLIC_VIEW_MS)
                && !IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RSK_MNG_PUBLIC_VIEW))))
            {
                btnDelete.Visible = false;
            }
            else
            {
                btnDelete.Visible = true;
            }

            RadioButton rbDisplayType = (RadioButton)e.Item.FindControl("rbDisplayType");
            bool isDefaultCus = false;
            string customDefault = CryptorServices.Current.DecryptText(hddCustomViewDefault.Value);
            if (!string.IsNullOrEmpty(CusViewDeleteId))
            {
                if (customDefault == CusViewDeleteId)
                {
                    isDefaultCus = Convert.ToBoolean(Convert.ToInt32(dataRow["IsDefault"].ToString()));
                }
                else
                {
                    isDefaultCus = dataRow["CustomViewID"].ToString() == customDefault;
                }

                if (isDefaultCus)
                {
                    AjaxAddResponseScript(string.Format("parent.applyCustomView('{0}');", CryptorServices.Current.EncryptText(dataRow["CustomViewID"].ToString())));
                }
            }
            else if (PageMode == WebSiteEnums.PageModeEnums.DetectionQueue)
            {
                isDefaultCus = Convert.ToBoolean(Convert.ToInt32(dataRow["IsDefault"].ToString()));
                if (isDefaultCus && dataRow["CustomViewID"].ToString() != customDefault)
                {
                    AjaxAddResponseScript(string.Format("parent.applyCustomView('{0}');", CryptorServices.Current.EncryptText(dataRow["CustomViewID"].ToString())));
                }
            }
            else
            {
                isDefaultCus = dataRow["CustomViewID"].ToString() == customDefault;
            }

            rbDisplayType.Checked = isDefaultCus;
            if (isDefaultCus)
            {
                hddCustomViewDefault.Value = CryptorServices.Current.EncryptText(dataRow["CustomViewID"].ToString());
            }
            rbDisplayType.Attributes.Add("value", CryptorServices.Current.EncryptText(dataRow["CustomViewID"].ToString()));
            rbDisplayType.Attributes.Add("onclick", "UpdateDefaultCustomView(this, '" + CryptorServices.Current.EncryptText(dataRow["CustomViewID"].ToString()) +
                "','" + PageMode.ToString() + "','" + AssignmentID + "')");
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        LinkButton rb = (LinkButton)sender;
        GridDataItem item = (GridDataItem)rb.NamingContainer;
        HiddenField ctl = item.FindControl("CustomViewID") as HiddenField;

        int customViewID = int.Parse(CryptorServices.Current.DecryptText(ctl.Value));
        string action = string.Empty;
        DataTable warningData = GeneralFuncsLib.GetWarningEditCustomView(customViewID);
        if (warningData.Rows.Count > 0 && warningData.Rows[0][COL_RESULT].ToString().Equals("0"))
        {
            string assignmentList = warningData.Rows[0][COL_ASSIGNMENT_LIST].ToString();
            string userList = warningData.Rows[0][COL_USER_LIST].ToString();
            string queryString = BuildSecureQueryString("Mode=3&Data=" + assignmentList + "," + userList);
            action = "manageCustomModal.doOpenEditMessagePopup('rm_MCF_MessageCustomModal.aspx?" + queryString + "','" + customViewID.ToString() + "')";

        }
        else
        {
            action = "manageCustomModal.doOpenEditMessagePopup('rm_MCF_MessageCustomModal.aspx?"
                    + BuildSecureQueryString("Message=" + GetLocalResourceObject("MessageConfirmDelete").ToString()) + "','" + customViewID.ToString() + "')";
        }
         ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript(action);
    }

    protected void uxRebindCustomViewsGrid_Click(object sender, EventArgs e)
    {
        uxCustomViewsGrid.Rebind();
        AjaxAddResponseScript("parent.registerCloseCustomViewModalEvent();");
        AjaxAddResponseScript("AdjustModalSize();");
    }

    protected void uxDeleteCustomView_Click(object sender, EventArgs e)
    {
        int customViewID = int.Parse(hddDeleteValue.Value);
        CusViewDeleteId = customViewID.ToString();
        var parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(false);
        parameters.Add(new FilterParameter("@CustomViewID", customViewID, DbType.Int32));
        var result = WebServices.RiskServices.GetReports("spa_RM_MCF_DeleteCustomView", parameters);
        uxCustomViewsGrid.Rebind();
        AjaxAddResponseScript("parent.registerCloseCustomViewModalEvent();");
        AjaxAddResponseScript("AdjustModalSize();");
    }

    [System.Web.Services.WebMethod()]
    public static void UpdateDefaultCustomView(string viewId, string pageMode, string assignmentId)
    {
        int customViewID = int.Parse(CryptorServices.Current.DecryptText(viewId));
        var parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParamsWithRecId();
        parameters.Add(new FilterParameter("@AssignmentID", assignmentId, DbType.Int32));
        parameters.Add(new FilterParameter("@CustomViewID", customViewID, DbType.Int32));
        WebServices.RiskServices.GetReports("spa_RM_MCF_UpdateDefaultCustomView", parameters);
    }
}