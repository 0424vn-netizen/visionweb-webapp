using AS.Common;
using AS.Common.DBManager;
using AS.Common.Export;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Web.Business;
using System;
using System.Data;
using System.Linq;
using Telerik.Web.UI;

[PagePermission("UserGroupMaint,MSUserGroupMaint")]
public partial class MemberListDataShare : ReportPage
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

    enum DataBindAction
    {
        ToGrid
    }
    
    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.ToGrid:
                {
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parameters.Add(new FilterParameter("@GroupID", UserGroupID, DbType.Int32));
                    parameters.Add(new FilterParameter("@Mode", true, DbType.Boolean));
                    parameters.Add(new FilterParameter("@PageName", "MemberList", DbType.String));
                    uxReportGrid.DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { "spa_cs_DataShare_GetUserList", ReportServices.ConvertToFilterParamWSArray(parameters) });
                }
                break;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.PageType = SecurePageType.Modal;
        this.IsBindDataOnLoad = true;
        if (!IsPostBack)
        {
            if (this.UserGroupID > 0)
            {
                OnDataBindControls(DataBindAction.ToGrid);
                phdManageUser.Visible = true;
                uxManageGroupUserList.Attributes["onclick"] = string.Format("openManageUserModal('{0}?{1}')", "ManageUserList.aspx", BuildSecureQueryString(String.Format("UserGroupID={0}", UserGroupID)));
            }
            else phdManageUser.Visible = false;
         }
    }
    
    protected void uxRefreshGrid_Click(object sender, EventArgs e)
    {
        uxReportGrid.Rebind();
    }

    protected void uxReportGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.ToGrid, sender);
    }
}
