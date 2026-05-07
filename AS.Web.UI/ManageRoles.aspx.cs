//----------------------------------------------------------------------------
// <copyright file="gen_ManageProfile.aspx.cs" company="Data Delivery Services, Inc.">
//     Copyright (c) Data Delivery Services, Inc. All rights reserved.
// </copyright>
// <author>Hung Ho</author>
// <summary>Manage User ( Create, Edit & Reset password ) </summary>
//----------------------------------------------------------------------------

using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Web.Business;
using AS.Controls.Exporter;
using Telerik.Web.UI;
using AS.Controls.Pages;
using AS.Security.WS.Entities;
using AS.Common;
using AS.Controls.UserControls;

[PagePermission("ManRole")]
public partial class _mps_ManageRoles : ReportPage
{
    #region Constants

    private string USER_ROLE_TYPE = "UserRoleType";
    private string SYSTEM_ID = "SystemId";
    private string HIERARCHY_ID = "HierarchyID";
    private string HIERARCHY_NAME = "HierarchyName";
    private string COL_DELETE = "Delete";
    private string COL_USERCOUNT = "UserCount";
    private string PRE_DEFINED = "IsPreDefined";
    private string CREATED_DATE = "CreatedDate";

    #endregion Constants

    #region Fields

    private string _UserID = string.Empty;

    #endregion Fields

    #region Enums

    enum DataBindAction
    {
        BindRoleListGrid
    }

    enum PostBackAction
    {
        RebindUserRoleClick
        , DeleteMSRole
    }

    #endregion Enums

    #region Properties

    protected bool HasMSUserManagement
    {
        get
        {
            return GeneralFuncsLib.HasMSUserManagementFeature;
        }
    }

    private bool DisableCreateMSRole
    {
        get
        {
            var disableCreateMSRole =
                GeneralFuncsLib.GetClientExtendedSetting("DisableCreateMSRole");
            return disableCreateMSRole.Data != null && disableCreateMSRole.Data.ToLower().Equals("true");
        } 
    }

    private bool HasDeleteMSRoleAbility
    {
        get
        {
            return (HasMSUserManagement && IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_DELETE_MS_ROLE));
        }
    }

    #endregion Properties

    #region Methods

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindRoleListGrid:
                {
                    uxReportGrid.Columns.FindByUniqueName(COL_DELETE).Visible
                        = uxReportGrid.Columns.FindByUniqueName(COL_USERCOUNT).Visible
                        = HasDeleteMSRoleAbility;

                    uxReportGrid.Columns.FindByUniqueName(USER_ROLE_TYPE).Visible = HasMSUserManagement;
                    uxReportGrid.DataSource = WebServices.SecurityServices.GetHierarchyTreeForUser(
                        SessionManager.CurrentUser.ASClient,
                        SessionManager.CurrentUser.UserID,
                        HasMSUserManagement ? 0 : WebSiteConstants.AS_SYSTEM_CS).ToDataTable();

                }
                break;
        }
    }

    protected override void OnPostBackActions(Enum type, object param)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.RebindUserRoleClick:
                {
                    uxReportGrid.Rebind();

                }
                break;
            case PostBackAction.DeleteMSRole:
                {
                    int hierarchyID = Int32.Parse(param.ToString());
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parameters.Add(new FilterParameter("@HierarchyID", hierarchyID, DbType.Int32));
                    parameters.Add(new FilterParameter("@ReturnValue", 1, DbType.Int32, true));
                    WebServices.SecurityServices.GetReports("spa_SEC_DeleteMSRole", parameters);
                    uxReportGrid.Rebind();
                }
                break;
        }
    }

    protected void uxReportGrid_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (IsIntruderDetected) return;

        if (e.CommandName == "DeleteMSRole")
        {
            GridDataItem item = e.Item as GridDataItem;
            OnPostBackActions(PostBackAction.DeleteMSRole, item.GetDataKeyValue("HierarchyID"));
        }
    }
    #region Override

    #endregion

    protected override void PageInitialize()
    {
        this.ExporterIDs.Add("uxExporter_Btm");
        base.PageInitialize();
        this.IsBindDataOnLoad = true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected)
            return;
        uxCreateMSRolePlaceHolder.Visible = HasMSUserManagement && !DisableCreateMSRole;
    }

    protected override void DoGridNeedDataSource(ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        if (sender == uxReportGrid && sender.Visible)
        {
            OnDataBindControls(DataBindAction.BindRoleListGrid);

        }
    }

    protected override void DoNeedExportConfig(UxExport sender, ExportConfig exportConfig)
    {
        base.DoNeedExportConfig(sender, exportConfig);
        uxReportGrid.Columns.FindByUniqueName(USER_ROLE_TYPE).Visible = HasMSUserManagement;
        uxReportGrid.Columns.FindByUniqueName(COL_USERCOUNT).Visible  = HasDeleteMSRoleAbility;
        exportConfig.FileName = GeneralFuncsLib.FormatFileName(GetLocalResourceObject("ManageRoles_aspx_UserRoleFilename").ToString());
    }

    protected override void DoItemDataBound(ASGrid sender, GridItemEventArgs e)
    {
        if (sender == uxReportGrid && sender.Visible)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = e.Item as GridDataItem;
                DataRowView rowView = ((System.Data.DataRowView)(e.Item.DataItem));
                int? systemId = GeneralFuncsLib.GetIntegerValue(rowView[SYSTEM_ID]);
                bool preDefineRole = false;
                bool.TryParse(rowView[PRE_DEFINED].ToString(), out preDefineRole);
                if (preDefineRole)
                {
                    dataItem[HIERARCHY_NAME].Text = string.Format(
                        "<a href='#' onclick='return ShowPopupModal(\"ViewRoleInfo.aspx?{0}\",\"auto\");'>{1}</a>", 
                        BuildSecureQueryString(string.Format("id={0}&sysId={1}", rowView[HIERARCHY_ID].ToString(), systemId)),
                        VeraCodeSolution.ValidateResponseData(rowView[HIERARCHY_NAME].ToString()));
                }
                else
                {
                    // Set hyperlink for User Role Name
                    dataItem[HIERARCHY_NAME].Text = string.Format(
                        "<a href='#' onclick='return ShowPopupModal(\"{0}\",\"auto\")'>{1}</a>",
                        BuildEditURL(rowView[HIERARCHY_ID].ToString(), systemId, rowView[CREATED_DATE].ToString()),
                        VeraCodeSolution.ValidateResponseData(rowView[HIERARCHY_NAME].ToString()));
                }

                if (HasDeleteMSRoleAbility)
                {
                    int hierarchyID = rowView["HierarchyID"].ToString().ToInt();
                    if (systemId == WebSiteConstants.AS_SYSTEM_MS)
                    {
                        if (string.Compare(rowView[COL_USERCOUNT].ToString(), "0") != 0)
                        {
                            string queryString = string.Format("<a href='#' onclick='return ShowPopupModal(\"MSRole_UserList_Modal.aspx?{0}\",\"auto\")'>"
                                , BuildSecureQueryString("HierarchyID=" + hierarchyID.ToSafeString())

                                ) + "{0}</a>";

                            dataItem[COL_USERCOUNT].Text = VeraCodeSolution.GetOutputHtmlString(string.Format(queryString, rowView[COL_USERCOUNT]));
                            dataItem[COL_DELETE].Text = VeraCodeSolution.GetOutputHtmlString(string.Format(queryString, GetLocalResourceObject("uxDeleteResource1.Text").ToString()));                      
                        }
                    }
                }
                else
                {
                    dataItem[COL_USERCOUNT].Text = string.Empty;
                }
                if (preDefineRole || !HasDeleteMSRoleAbility || systemId == WebSiteConstants.AS_SYSTEM_CS)
                    dataItem[COL_DELETE].Text = string.Empty;
            }
        }
    }

    protected void uxRebinData_Click(object poster, EventArgs e)
    {
        if (IsIntruderDetected)
            return;
        OnPostBackActions(PostBackAction.RebindUserRoleClick);

    }

    protected string ValidateResponseData(object a)
    {
        return VeraCodeSolution.ValidateResponseData(((string)a));
    }

    protected string BuildEditURL(string hierarchyID, int? systemId, string createdDate)
    {
        string editPage =
            systemId.HasValue && systemId.Value == WebSiteConstants.AS_SYSTEM_MS
            ? "EditMSRole_Modal.aspx?" : "EditRole_Modal.aspx?";
        return editPage + BuildSecureQueryString(string.Format("id={0}&createdDate={1}", hierarchyID, createdDate));
    }

    #endregion Methods
}
