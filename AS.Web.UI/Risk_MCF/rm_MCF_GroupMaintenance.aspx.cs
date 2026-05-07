using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using Telerik.Web.UI;
using AS.Common;
using AS.Controls.Exporter;
using AS.Controls.Pages;
using AS.Common.DBManager;
using Resources;
using System.Text.RegularExpressions;
using System.Web;

[PagePermission("RskGroup,MSRskGroup")]
public partial class rm_MCF_GroupMaintenance : ReportPage
{
    #region Enum

    enum DataBindAction
    {
        BindDataGroups
    }

    enum PostBackAction
    {
        OpenFormCreate,
        OpenFormEdit,
        CreateAction,
        UpdateAction,
        CancelAction,
        ActivateDeactivate,
        ChangeFilter
    }

    #endregion

    #region Constants

    private const string SESSION_FILTERING_OPTIONS = "GroupMaintenanceFilteringOptions";

    #endregion Constants

    #region Properties

    DataTable tbl = new DataTable();
    private bool _isExported = false;

    #endregion Properties

    #region Methods

    #region Protected Methods

    protected void Page_Load(object sender, EventArgs e)
    {
        //uxGroupList.IsIntruder = true;
        //uxGroupList.IntruderSourceName = GeneralFuncsLib.GetRequestFileName() + uxGroupList.ID;

        if (!IsPostBack)
        {
            SetFilterDefault();
        }

        uxGroupLabel.CssClass = "control-label";
        uxAddGroupErrMsg.ShowOnLoad = false;
    }

    #region Overrides

    protected override void PageInitialize()
    {
        this.GridIDs.Add("uxGroupList");
        this.ExporterIDs.Add("uxExportTop");
        this.ExporterIDs.Add("uxExportBottom");
        base.PageInitialize();
        IsBindDataOnLoad = true;
        this.IsSecureCSRF = true;
    }

    protected override void DoGridNeedDataSource(AS.Controls.Grid.ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        if (sender == uxGroupList)
        {
            OnDataBindControls(DataBindAction.BindDataGroups, sender);
        }
    }

    protected override void DoItemDataBound(AS.Controls.Grid.ASGrid sender, GridItemEventArgs e)
    {
        if (IsIntruderDetected) return;
        if (sender != uxGroupList) return;
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            var activateDeactivateItem = dataItem["ActivateDeactivate"];
            if (!e.Item.IsInEditMode)
            {
                var rowData = e.Item.DataItem as DataRowView;
                int groupID = int.Parse(rowData["GroupID"].ToString());
                var groupNameItem = (Literal)dataItem["GroupName"].FindControl("LiteralGroupName");
                var descriptionItem = (Literal)dataItem["Description"].FindControl("LiteralDescription");
                var memberCountItem = dataItem["MemberCount"];
                bool isActive = (string.Compare(rowData["IsActive"].ToString(), "0") != 0);
                bool deleteAllowed = (string.Compare(rowData["DeleteAllowed"].ToString(), "0") != 0);
                //TEST:bool deleteAllowed = false;
                //if (!groupNameItem.Text.Equals("&nbsp;"))
                //    groupNameItem.Text = VeraCodeSolution.ValidateResponseData(rowData["GroupName"].ToString());
                //if (!descriptionItem.Text.Equals("&nbsp;"))
                //    descriptionItem.Text = VeraCodeSolution.DoVeraCode(rowData["Description"].ToString());

                string queryString = BuildSecureQueryString(string.Format("GroupID={0}", groupID));
                if (string.Compare(rowData["MemberCount"].ToString(), "0") != 0)
                {
                    memberCountItem.Text = VeraCodeSolution.DoVeraCode(string.Format(
                        "<a href=\"#\" onclick=\"ShowMemberList('{0}'); return false;\">{1}</a>",
                        queryString,
                        rowData["MemberCount"]));
                }

                activateDeactivateItem.Text = VeraCodeSolution.DoVeraCode(
                    RiskGeneral.BuildActiveDeactiveLink(
                    rowData["GroupID"], isActive, deleteAllowed, queryString)
                    );

                //if (_IsAllow)
                //    activateDeactivateItem.Text = VeraCodeSolution.DoVeraCode(string.Format("<a href=\"javascript:ActivateDeactivate({0}, {1}, {2}, '{3}');\">{4}</a>",
                //        rowData["GroupID"], isActive.ToString().ToLower(), deleteAllowed.ToString().ToLower(), queryString, (isActive ? "Deactivate" : "Activate")));
                //else
                //{
                //    activateDeactivateItem.Text = VeraCodeSolution.DoVeraCode(string.Format("{0}", (isActive ? "Deactivate" : "Activate")));
                //    dataItem["Edit"].Text = "Edit";
                //}

                if (!isActive)
                {
                    groupNameItem.Text = string.Format(RiskGeneral.MUTED_TEXT_FORMAT, groupNameItem.Text);
                    descriptionItem.Text = string.Format(RiskGeneral.MUTED_TEXT_FORMAT, descriptionItem.Text);
                    memberCountItem.ForeColor = Color.Gray;
                    memberCountItem.Font.Italic = true;
                }
            }
            else
            {
                activateDeactivateItem.Text = string.Empty;
            }
        }
        if (e.Item.IsInEditMode && e.Item is GridEditableItem)
        {
            e.Item.CssClass = RiskGeneral.GetCssGridEditableItem(e.Item.RowIndex);
        }
    }

    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, ExportConfig exportConfig)
    {
        base.DoNeedExportConfig(sender, exportConfig);
        exportConfig.ReportHeader = uxExportTop.GridHeader;
        if (Request.Browser.Browser.ToUpper() == WebSiteConstants.BROWSER_INTERNETEXPLORER)
        {
            exportConfig.FileName = Server.UrlPathEncode(GeneralFuncsLib.FormatFileName(uxExportTop.GridHeader));
        }
        else
        {
            exportConfig.FileName = GeneralFuncsLib.FormatFileName(uxExportTop.GridHeader);
        }
        uxGroupList.Columns.FindByUniqueName("Edit").Visible = false;
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        if (IsIntruderDetected) return;
        
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindDataGroups:
            {
                if (string.IsNullOrEmpty(uxGroupList.AS_SortExpression))
                    uxGroupList.AS_SortExpression = "GroupName ASC";

                    FilterParameterCollection parameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parameters.AddLanguageID();
                    parameters.Add(new FilterParameter("@IsActive", FilterStatus, DbType.Int32));
                    parameters.Add(new FilterParameter("@SortOrder", uxGroupList.AS_SortExpression, DbType.String));
                    DataTable table = WebServices.RiskServices.GetReports("spa_RM_MCF_GetAllGroups", parameters);
                    uxGroupList.DataSource = table;
                    if (tbl.Rows.Count == 0)
                        uxGroupList.AllowSorting = false;
                    else
                        uxGroupList.AllowSorting = true;
                }
                break;
        }
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        if (IsIntruderDetected) return;
        switch ((PostBackAction)type)
        {
            case PostBackAction.OpenFormCreate:
            {
                uxGroupLabel.CssClass = string.Empty;
                uxAddGroupErrMsg.Message = string.Empty;
                pnlAddGroup.Visible = true;
                uxAddGroupText.Text = "";
                uxAddDescription.Text = "";
                uxAddGroupText.Focus();
                uxGroupList.MasterTableView.ClearEditItems();
                uxGroupList.Rebind();
            }
            break;
            case PostBackAction.CreateAction:
            {
                string group = uxAddGroupText.Text.Trim();
                string description = uxAddDescription.Text.Trim();
                int result = WebServices.RiskServices.AddGroup(GetLoggedInUserParams(), group, description, true);
                if (RiskErrorCode.HasError(result))
                {
                    ShowErrorMessageWhenAdding(result);
                    return;
                }
                pnlAddGroup.Visible = IsIntruderDetected = false;
                uxGroupList.Rebind();
            }
            break;
            case PostBackAction.OpenFormEdit:
            {
                Telerik.Web.UI.GridCommandEventArgs e = sender as Telerik.Web.UI.GridCommandEventArgs;
                AjaxAddResponseScript("HideCreatePanel('" + pnlAddGroup.ClientID + "');");
            }
            break;
            case PostBackAction.UpdateAction:
            {
                pnlAddGroup.Visible = false;

                Telerik.Web.UI.GridCommandEventArgs e = sender as Telerik.Web.UI.GridCommandEventArgs;

                GridEditableItem editedItem = e.Item as GridEditableItem;
                int groupId = Int32.Parse(((HiddenField) e.Item.FindControl("txtGroupID")).Value.Trim());
                string groupName = ((TextBox)e.Item.FindControl("txtEditGroupName")).Text;
                string description = ((TextBox)e.Item.FindControl("txtEditGroupDescription")).Text;
                bool isActive = (string.Compare(((HiddenField)e.Item.FindControl("txtActiveValue")).Value.Trim(), "0") != 0);

                // Validate input
                if (!ValidateGroupName(groupName))
                {
                    return;
                }

                // Update data
                int result = WebServices.RiskServices.UpdateGroup(GetLoggedInUserParams(), groupId, groupName, description, true);
                if (RiskErrorCode.HasError(result))
                {
                    RiskGeneral.ShowMessageAjax((ReportPage) this.Page,
                        RiskErrorCode.GetErrorMessage(result, RiskErrorCode.ObjectType.Group));
                    return;
                }
                else
                {
                    uxGroupList.MasterTableView.ClearEditItems();
                    uxGroupList.MasterTableView.Rebind();
                }
            }
            break;
            case PostBackAction.CancelAction:
            {
                // view event
            }
            break;
            case PostBackAction.ActivateDeactivate:
            {
                string[] parts = uxActivateDeactivateData.Value.Split(';');
                int groupId = Int32.Parse(parts[0]);
                int isActive = (string.Compare(parts[1], "true") == 0 ? 0 : 1);
                int result = WebServices.RiskServices.ActiveDeactiveGroup(GetLoggedInUserParams(), groupId, isActive, true);
                if (result == RiskErrorCode.ERROR_PROCESSING_FAILED)
                {
                    RiskGeneral.ShowMessage((ReportPage)this.Page,
                        RiskErrorCode.GetErrorMessage(result, RiskErrorCode.ObjectType.Others));
                }

                uxGroupList.MasterTableView.ClearEditItems();
                uxGroupList.Rebind();
            }
            break;
            case PostBackAction.ChangeFilter:
            {
                Session[SESSION_FILTERING_OPTIONS] = this.FilterStatus.ToString();
                uxGroupList.Rebind();
            }
            break;
        }
    }

    #endregion

    protected void uxAddGroup_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.CreateAction);
    }

    protected void uxCreateMode_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.OpenFormCreate);
    }

    protected void uxAddGroupCancel_Click(object sender, EventArgs e)
    {
        pnlAddGroup.Visible = false;
        uxGroupList.MasterTableView.Rebind();
    }

    protected void uxGroupList_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (IsIntruderDetected) return;

        if (e.CommandName == RadGrid.EditCommandName)
        {
            OnPostBackActions(PostBackAction.OpenFormEdit, e);
        }
        else if (e.CommandName == RadGrid.UpdateCommandName)
        {
            //"update" button clicked
            OnPostBackActions(PostBackAction.UpdateAction, e);
        }
        else if (e.CommandName == RadGrid.SortCommandName)
        {
            uxGroupList.MasterTableView.ClearEditItems();
        }
    }

    protected void uxGroupList_PagerEventClick(object source, AS.Controls.Grid.ASPagerEventArgs e)
    {
        uxGroupList.MasterTableView.ClearEditItems();
    }

    protected void uxChangeFilterStatus_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.ChangeFilter);
    }

    protected void uxActivateDeactivate_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.ActivateDeactivate);
    }

    #endregion Protected Methods

    #region Private Methods

    private void SetFilterDefault()
    {
        if (Session[SESSION_FILTERING_OPTIONS] != null)
        {
            string status = "Inactive";
            if (status.Length > 0)
            {
                int statusValue = 0;

                if (Int32.TryParse(status, out statusValue))
                    this.FilterStatus = statusValue;
            }
        }
    }

    private int FilterStatus
    {
        get
        {
            int filterStatusValue = -1;

            foreach (Control ctl in uxFilterStatusContainer.Controls)
            {
                if (ctl is RadioButton)
                {
                    RadioButton radioButton = ctl as RadioButton;

                    if (radioButton.Checked)
                    {
                        if (Int32.TryParse(radioButton.Attributes["xValue"], out filterStatusValue))
                        {
                            return filterStatusValue;
                        }
                    }
                }
            }

            return filterStatusValue;
        }

        set
        {
            int filterStatusValue = -1;

            foreach (Control ctl in uxFilterStatusContainer.Controls)
            {
                if (ctl is RadioButton)
                {
                    RadioButton radioButton = ctl as RadioButton;

                    if (Int32.TryParse(radioButton.Attributes["xValue"], out filterStatusValue))
                    {
                        if (filterStatusValue == value)
                            radioButton.Checked = true;
                        else
                            radioButton.Checked = false;
                    }
                }
            }

            if (this.FilterStatus < 0)
                uxFilterStatusAll.Checked = true;
        }
    }

    private bool ValidateGroupName(string groupName)
    {
        bool result = true;
        if (string.IsNullOrEmpty(groupName))
        {
            RiskGeneral.ShowMessage((ReportPage)this.Page,
                VeraCodeExtensions.DoVeraCode(GetLocalResourceObject("rm_GroupMaintenance_aspx_cs_GroupName").ToString() + " " + Resources.ValMsg.Required));
            result = false;
        }
        else if (RiskGeneral.CheckValidString(groupName)
            && (groupName.IndexOf("<") != -1 || groupName.IndexOf(">") != -1))
        {
            RiskGeneral.ShowMessage((ReportPage)this.Page,
                VeraCodeExtensions.DoVeraCode(GetLocalResourceObject("rm_GroupMaintenance_aspx_cs_GroupName").ToString() + " " + Resources.ValMsg.InvalidCharacter));
            result = false;
        }
        return result;
    }

    private void ShowErrorMessageWhenAdding(int errorCode)
    {
        uxAddGroupText.Focus();
        uxGroupLabel.CssClass = RiskGeneral.CSS_ERROR_LABEL;
        uxAddGroupErrMsg.ShowOnLoad = true;
        uxAddGroupErrMsg.Message = RiskErrorCode.GetErrorMessage(errorCode, RiskErrorCode.ObjectType.Others);
        uxGroupList.Rebind();
    }

    #endregion Private Methods

    #endregion Methods
}
