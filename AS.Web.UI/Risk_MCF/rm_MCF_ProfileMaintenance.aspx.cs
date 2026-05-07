using System;
using System.Data;
using Telerik.Web.UI;
using AS.Common;
using System.Drawing;
using AS.Common.DBManager;
using Resources;
using AS.Controls.Exporter;
using AS.Controls.UserControls;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web;
using AS.Controls.Pages;

[PagePermission("RskProfile,MSRskProfile")]
public partial class rm_MCF_ProfileMaintenance : ReportPage
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

    #region Properties
    string groupEditedText = "";
    string descriptionEditedText = "";
    protected bool IsShowIEReport
    {
        get
        {
            return GeneralFuncsLib.GetDataOfExtendedSetting("SHOW_SEND_IE_REPORT").Equals("true") ? true : false;
        }
    }
    #endregion


    #region Overrides

    protected override void PageInitialize()
    {
        this.GridIDs.Add("uxGroupList");
        this.ExporterIDs.Add("uxExportTop");
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
                int groupID = int.Parse(rowData["RecordId"].ToString());
                var groupNameItem = dataItem["ProfileName"];
                var descriptionItem = dataItem["Description"];
                bool isActive = (string.Compare(rowData["IsActive"].ToString(), "0") != 0);

                activateDeactivateItem.Text = VeraCodeSolution.DoVeraCode(string.Format("<a href=\"#\" onclick=\"ActivateDeactivate({0}, {1}); return false;\">{2}</a>",
                    rowData["RecordId"], isActive.ToString().ToLower(), (isActive ? GetLocalResourceObject("rm_ProfileMaintenance_aspx_cs_Deactivate").ToString() : GetLocalResourceObject("rm_ProfileMaintenance_aspx_cs_Active").ToString())));
                if (!isActive)
                {                   
                    groupNameItem.ForeColor = Color.Gray;
                    groupNameItem.Font.Italic = true;
                    descriptionItem.ForeColor = Color.Gray;
                    descriptionItem.Font.Italic = true;
                }
            }
            else
            {
                activateDeactivateItem.Text = string.Empty;
            }
        }
        if (e.Item is GridEditableItem && e.Item.IsInEditMode)
        {
            e.Item.CssClass = RiskGeneral.GetCssGridEditableItem(e.Item.RowIndex);
        }
    }

    protected override void DoSwitchView()
    {
        uxGroupList.Columns.FindByUniqueName("IEReport").Visible = IsShowIEReport;
        if (GeneralFuncsLib.GetDataOfExtendedSetting("DISABLE_MIF_PROFILE_EDIT_MAINTENANCE") == "true")
        {
            uxGroupList.Columns.FindByUniqueName("Edit").Visible = false;
            uxGroupList.Columns.FindByUniqueName("ActivateDeactivateHidden").Visible = false;
            uxGroupList.Columns.FindByUniqueName("ActivateDeactivate").Visible = false;
            uxCreateMode.Visible = false;
        }
        else
        {
            uxGroupList.Columns.FindByUniqueName("Edit").Visible = true;
            uxGroupList.Columns.FindByUniqueName("ActivateDeactivateHidden").Visible = true;
            uxGroupList.Columns.FindByUniqueName("ActivateDeactivate").Visible = true;
            uxCreateMode.Visible = true;
        }
    }

    protected override void DoNeedExportConfig(UxExport sender, ExportConfig exportConfig)
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
        uxGroupList.Columns.FindByUniqueName("IEReport").Visible = false;
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        if (IsIntruderDetected) return;

        switch ((DataBindAction)type)
        {
            case DataBindAction.BindDataGroups:
                {
                    if (string.IsNullOrEmpty(uxGroupList.AS_SortExpression))
                        uxGroupList.AS_SortExpression = "ProfileName ASC";

                    FilterParameterCollection parameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parameters.AddLanguageID();
                    parameters.Add(new FilterParameter("@IsActive", this.FilterStatus, DbType.Int32));
                    parameters.Add(new FilterParameter("@SortOrder", uxGroupList.AS_SortExpression, DbType.String));
                    DataTable table = WebServices.RiskServices.GetReports("spa_RM_MCF_GetAllProfiles", parameters);
                    uxGroupList.DataSource = table;
                    if (table.Rows.Count == 0)
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
                    pnlAddGroup.Visible = true;
                    uxAddGroupTextErrMsg.Message = string.Empty;
                    uxAddGroupTextLabel.CssClass = string.Empty;
                    uxAddGroupText.Text = "";
                    uxAddDescription.Text = "";
                    uxAddSendToIEReport.Checked = false;
                    uxAddGroupText.Focus();
                    uxGroupList.MasterTableView.ClearEditItems();
                    uxGroupList.Rebind();
                }
                break;
            case PostBackAction.CreateAction:
                {
                    string group = (uxAddGroupText.Text.Trim());
                    string description = uxAddDescription.Text.Trim();
                    bool isSendToIEReport = uxAddSendToIEReport.Checked;
                    //Add new Group        
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parameters.Add(new FilterParameter("@ProfileName", group, DbType.String));
                    parameters.Add(new FilterParameter("@Description", description, DbType.String));
                    parameters.Add(new FilterParameter("@IsSendToIEReport", isSendToIEReport, DbType.Boolean));
                    parameters.Add(new FilterParameter("@ReturnValue", 1, DbType.Int32, true));

                    FilterParameterCollection parameterOut = new FilterParameterCollection();
                    WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_AddProfile", parameters, out parameterOut);
                    int result = Convert.ToInt32(parameterOut[0].ParameterValue);
                    uxGroupList.Rebind();
                    if (RiskErrorCode.HasError(result))
                    {
                        uxAddGroupText.Focus();
                        uxAddGroupTextErrMsg.Message = RiskErrorCode.GetErrorMessage(result, RiskErrorCode.ObjectType.Others);
                        uxAddGroupTextErrMsg.ShowOnLoad = true;
                        uxAddGroupTextLabel.CssClass = RiskGeneral.CSS_ERROR_LABEL;
                        return;
                    }

                    pnlAddGroup.Visible = IsIntruderDetected = false;
                }
                break;
            case PostBackAction.OpenFormEdit:
                {
                    pnlAddGroup.Visible = false;
                }
                break;
            case PostBackAction.UpdateAction:
                {
                    Telerik.Web.UI.GridCommandEventArgs e = sender as Telerik.Web.UI.GridCommandEventArgs;

                    GridEditableItem editedItem = e.Item as GridEditableItem;
                    int recordID = Int32.Parse(((HiddenField)e.Item.FindControl("txtRecordID")).Value.Trim());

                    string reason = ((editedItem.FindControl("txtEditProfileName") as TextBox).Text.Trim());
                    string description = ((editedItem.FindControl("txtEditProfileDescription") as TextBox).Text.Trim());
                    bool isSendToIEReport = (editedItem.FindControl("uxSendToIEReport") as CheckBox).Checked;
                    
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parameters.Add(new FilterParameter("@RecordID", recordID, DbType.Int32));
                    parameters.Add(new FilterParameter("@ProfileName", reason, DbType.String));
                    parameters.Add(new FilterParameter("@Description", description, DbType.String));
                    parameters.Add(new FilterParameter("@IsSendToIEReport", isSendToIEReport, DbType.Boolean));
                    parameters.Add(new FilterParameter("@ReturnValue", 1, DbType.Int32, true));

                    FilterParameterCollection parameterOut = new FilterParameterCollection();
                    WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_UpdateProfile", parameters, out parameterOut);

                    int result = Convert.ToInt32(parameterOut[0].ParameterValue);
                    if (RiskErrorCode.HasError(result))
                    {
                        RiskGeneral.ShowMessageAjax((ReportPage)this.Page,
                            VeraCodeExtensions.DoVeraCode(
                                RiskErrorCode.GetErrorMessage(result, RiskErrorCode.ObjectType.Others)));
                        return;
                    }
                    else
                    {
                        uxGroupList.MasterTableView.ClearEditItems();
                        uxGroupList.MasterTableView.Rebind();
                    }       
                }
                break;
            case PostBackAction.ActivateDeactivate:
                {
                    string[] parts = uxActivateDeactivateData.Value.Split(';');
                    int recordID = Int32.Parse(parts[0]);
                    int isActive = (string.Compare(parts[1], "true") == 0 ? 0 : 1);

                    FilterParameterCollection parameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parameters.Add(new FilterParameter("@RecordID", recordID, DbType.Int32));
                    parameters.Add(new FilterParameter("@IsActive", isActive, DbType.Int32));
                    parameters.Add(new FilterParameter("@ReturnValue", 1, DbType.Int32, true));
                    FilterParameterCollection parameterOut = new FilterParameterCollection();
                    WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_UpdateProfileStaus", parameters, out parameterOut);

                    int result = Convert.ToInt32(parameterOut[0].ParameterValue);

                    if (result == 0)
                    {
                        //AjaxAddResponseScript("setTimeout('OpenMessageWindow(\"CaseModal1.aspx?" + queryStr + "\")',500);");

                        AjaxAddResponseScript(string.Format("setTimeout('ShowPopupModal(\"rm_MCF_MerchantsInProfile.aspx?{0}\",\"auto\")', 500);", BuildSecureQueryString("RecordID=" + recordID.ToString())));
                    }
                    uxGroupList.MasterTableView.ClearEditItems();
                    uxGroupList.Rebind();
                }
                break;
            case PostBackAction.ChangeFilter:
                {
                    uxGroupList.Rebind();
                }
                break;
        }
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
        if (!IsPostBack)
        {
            uxPlAddShowIEReport.Visible = IsShowIEReport;
        }
    }

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
            OnPostBackActions(PostBackAction.OpenFormEdit, e);       
        else if (e.CommandName == RadGrid.UpdateCommandName)
        {
            OnPostBackActions(PostBackAction.UpdateAction, e);
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
    protected void cancel_click(object sender, EventArgs e)
    {
        uxGroupList.MasterTableView.ClearEditItems();
        uxGroupList.MasterTableView.Rebind();
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

    protected void uxActivateDeactivate_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.ActivateDeactivate);
    }

}
