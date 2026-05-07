
using System;
using System.Data;
using Telerik.Web.UI;
using AS.Controls.Pages;
using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Exporter;
using AS.Controls.Global;
using AS.Controls.Grid;

[PagePermission("ASLandingPage")]
public partial class ManageASUsers : ReportPage
{
    private const string FNAME_ATag = "<a href='{0}' style='cursor:pointer' >{1}</a>";
    private const string POPUP_ATAG = "<a href='javascript:void(0)' style='cursor:pointer' onclick=\"ShowPopupModal('{0}','auto');return false;\">{1}</a>";
    private bool _needReFilterData = false;

    string FilterStatus
    {
        get
        {
            if (chbxActive.Checked)
                return "1";
            else if (chbxInActive.Checked)
                return "0";
            else
                return string.Empty;

        }

    }

    protected override void PageInitialize()
    {
        base.PageInitialize();
        RegisterEvent();
        this.IsBindDataOnLoad = true;
        uxbtnCreateUser.Visible = IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_CREATEASUSERS);
        uxReportGrid.Columns.FindByUniqueName("EditAction").Visible = IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_EDITASUSERS) || IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RESETASUSERPWD) || IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_ACTIVATEDEACTIVATEASUSERS);

    }

    protected override void DoGridDataSourceReady(AS.Controls.Grid.ASGrid sender, EventArgs e)
    {
        base.DoGridDataSourceReady(sender, e);
        if (sender == uxReportGrid && sender.Visible)
        {
            // Re-filter data in case the Client doesn't have MS User Management
            if (_needReFilterData && sender.DataSource != null
                && ((DataTable)sender.DataSource).Rows.Count > 0)
            {
                sender.DataSource = ((DataTable)sender.DataSource);
            }
            // Reset to avoid calling FilterUserList in other cases
            _needReFilterData = false;

            if (uxReportGrid.AS_FilterExpression.IsNullOrEmpty()
                && (sender.DataSource == null || ((DataTable)sender.DataSource).Rows.Count == 0))
            {
                uxReportGrid.AllowFilteringByColumn = false;
            }
            else
            {
                uxReportGrid.AllowFilteringByColumn = true;
            }
        }
    }

    protected override void DoGridNeedDataSource(AS.Controls.Grid.ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        if (sender == uxReportGrid && sender.Visible)
        {
            FilterParameterCollection parameters = new FilterParameterCollection();
            parameters.AddLoggedInUserParamsWithRecId();
            parameters.Add(new FilterParameter("@HierarchyId", 0, DbType.Int32));
            parameters.Add(new FilterParameter("@FilterType",
                                          uxReportFiltering.ManageUserFilterOption.SearchType,
                                          DbType.String));
            parameters.Add(new FilterParameter("@FilterVal",
                                          uxReportFiltering.ManageUserFilterOption.SearchValue,
                                          DbType.String));
            parameters.Add(new FilterParameter("@ActvStat",
                                         FilterStatus,
                                         DbType.String));
            uxReportGrid.DataSourceInvoker =
           new ASFuncInvoker(
               WebServices.SecurityServices,
               WebSiteConstants.GET_REPORT_METHOD_NAME,
               new object[] {
                    "spa_SEC_GetASUserList",
                    AS.Security.Web.SecurityServices.SecurityService.ConvertToFilterParamWSArray(parameters)
                });


            _needReFilterData = true;
        }
    }
    protected override void DoItemDataBound(AS.Controls.Grid.ASGrid sender, GridItemEventArgs e)
    {
        base.DoItemDataBound(sender, e);
        if (e.Item is GridDataItem)
        {
            //Format DataItem
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView rowView = ((System.Data.DataRowView)(e.Item.DataItem));

            System.Web.UI.WebControls.PlaceHolder pnlEditLink = dataItem["EditAction"].FindControl("pnlEditLink") as System.Web.UI.WebControls.PlaceHolder;
            System.Web.UI.WebControls.PlaceHolder pnlResetPasswordLink = dataItem["EditAction"].FindControl("pnlResetPasswordLink") as System.Web.UI.WebControls.PlaceHolder;
            System.Web.UI.WebControls.PlaceHolder pnlActiveStatus = dataItem["EditAction"].FindControl("pnlActiveStatus") as System.Web.UI.WebControls.PlaceHolder;
            string userIdLink = "CreateNewASUser.aspx?" + BuildSecureQueryString("u=" + rowView["UserID"]);
            string viewDetailLink = "ViewASUserDetail.aspx?" + BuildSecureQueryString("u=" + rowView["UserID"]);

            userIdLink = string.Format(FNAME_ATag, userIdLink, rowView["UserID"]);

            viewDetailLink = string.Format(FNAME_ATag, viewDetailLink, rowView["UserID"]);
            if (pnlEditLink != null)
            {
                // Edit
                if (IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_EDITASUSERS))
                {
                    pnlEditLink.Visible = true;
                    System.Web.UI.WebControls.HyperLink linkEdit = dataItem["EditAction"].FindControl("uxEditLink") as System.Web.UI.WebControls.HyperLink;
                    if (linkEdit != null)
                    {
                        linkEdit.NavigateUrl = "CreateNewASUser.aspx?" + BuildSecureQueryString("u=" + rowView["UserID"]);
                    }
                }
                else
                {
                    pnlEditLink.Visible = false;
                }
            }
            // reset
            if (pnlResetPasswordLink != null)
            {
                if (IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RESETASUSERPWD))
                {
                    pnlResetPasswordLink.Visible = true;
                    //Reset Password Field  
                    string resetLink = "ManageUserResetPwd_Step1.aspx?" + BuildSecureQueryString("u=" + rowView["UserID"]);
                    resetLink = string.Format(POPUP_ATAG, resetLink, "Reset PWD");
                    // dataItem["ResetPassword"].Text = VeraCodeSolution.DoVeraCode(resetLink);


                    System.Web.UI.WebControls.Literal linkResetPassword = dataItem["EditAction"].FindControl("uxResetPasswordLink") as System.Web.UI.WebControls.Literal;
                    if (linkResetPassword != null)
                    {
                        linkResetPassword.Text = resetLink;
                    }
                }
                else
                {
                    pnlResetPasswordLink.Visible = false;
                }
            }

            // deactive
            if (pnlActiveStatus != null)
            {
                if (IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_ACTIVATEDEACTIVATEASUSERS))
                {
                    pnlActiveStatus.Visible = true;
                    System.Web.UI.WebControls.LinkButton linkuxActiveStatus = dataItem["EditAction"].FindControl("uxActiveStatus") as System.Web.UI.WebControls.LinkButton;
                    if (linkuxActiveStatus != null)
                    {
                        linkuxActiveStatus.Text = rowView["ActiveStatus"].ToString().Equals("Y") ? GetLocalResourceObject("lbDeactivateResource").ToString() : GetLocalResourceObject("lbActivateResource").ToString();
                        //linkuxActiveStatus.OnClientClick = string.Format("return validateActive('{0}', '{1}')", rowView["ActvStat"], rowView["UserID"].ToString());

                        if (rowView["ActiveStatus"].ToString().Equals("Y"))
                        {
                            linkuxActiveStatus.OnClientClick = "return ShowPopupModal('EditASUserConfirmModal.aspx?" + BuildSecureQueryString("u=" + rowView["UserID"].ToString() + "&action=" + WebSiteEnums.ASUserAction.Deactive + "&ControlID=" + linkuxActiveStatus.UniqueID) + "','auto');";
                        }
                        else
                        {
                            linkuxActiveStatus.OnClientClick = "return ShowPopupModal('EditASUserConfirmModal.aspx?" + BuildSecureQueryString("u=" + rowView["UserID"].ToString() + "&action=" + WebSiteEnums.ASUserAction.Active + "&ControlID=" + linkuxActiveStatus.UniqueID) + "','auto');";
                        }
                    }
                }
                else
                {
                    pnlActiveStatus.Visible = false;
                }
            }
            // UserID Field 
            dataItem["UserID"].Text = VeraCodeSolution.DoVeraCode(viewDetailLink);
            dataItem["ActiveStatus"].Text = rowView["ActiveStatus"].ToString().Equals("Y") ? GetLocalResourceObject("chbxActiveResource.Text").ToString() : GetLocalResourceObject("chbxInActiveResource.Text").ToString();

        }
    }
    protected void uxReportGrid_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Active")
        {
            string userid = ((GridDataItem)e.Item).GetDataKeyValue("UserID").ToString();
            AS.Security.WS.Entities.User user = WebServices.SecurityServices.GetUser(SessionManager.CurrentUser.ASClient, userid);
            if (user != null)
            {
                user.Status = user.Status == "1" ? "0" : "1";
            }
            else
            {
                return;
            }
            WebServices.SecurityServices.UpdateUser(user, 0, new string[] { }, string.Empty, string.Empty);
            uxReportGrid.Rebind();
        }
    }
    protected void RegisterEvent()
    {
        uxReportFiltering.Filtering += (s, e) =>
        {
            uxReportGrid.CurrentPageIndex = 0;
            uxReportGrid.Rebind();
        };
    }
    protected void chbxActive_CheckedChanged(object sender, EventArgs e)
    {
        uxReportGrid.CurrentPageIndex = 0;
        uxReportGrid.Rebind();
    }
    protected void uxReload_Click(object sender, EventArgs e)
    {
        this.uxReportGrid.Rebind();
    }
    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, ExportConfig exportConfig)
    {
        if (IsIntruderDetected) return;
        base.DoNeedExportConfig(sender, exportConfig);
        exportConfig.ExportMethod = ExportMethod.BY_FILESTREAM;
        exportConfig.FileName = GeneralFuncsLib.FormatFileName(GetLocalResourceObject("PageResource1.Title").ToString() + "-" + DateTime.Now.ToString("MMddyyyy"));

        exportConfig.ReportHeader = GetLocalResourceObject("PageResource1.Title").ToString() + "-" + DateTime.Now.ToString("MMddyyyy");

        this.uxExporter.Formatter = new System.Collections.Generic.Dictionary<string, Func<object, string>>();
        this.uxExporter.FormatterWS = new System.Collections.Generic.Dictionary<string, string>();

        uxReportGrid.Columns.FindByUniqueName("EditAction").Visible = false;
        uxExporter.Formatter.Add("ActiveStatus", new Func<object, string>(ConvertActiveText));

    }
    private string ConvertActiveText(object obj)
    {
        if (obj.IsNullOrEmpty())
            return string.Empty;
        if (obj.ToString().Equals("Y", StringComparison.OrdinalIgnoreCase))
            return GetLocalResourceObject("chbxActiveResource.Text").ToString();
        else
            return GetLocalResourceObject("chbxInActiveResource.Text").ToString();
    }


}
