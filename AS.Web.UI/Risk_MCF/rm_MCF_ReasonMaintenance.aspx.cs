using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Exporter;
using AS.Controls.Pages;
using AS.Controls.Validators;
using Resources;
using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

[PagePermission("RskReason,MSRskReason")]
public partial class rm_MCF_ReasonMaintenance : ReportPage
{
    #region Enum

    enum DataBindAction
    {
        BindDataGrid
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

    #region Propertise
    private const string SESSION_FILTERING_OPTIONS = "EscalationReasonFilteringOptions";
    string statusEditedText = "";
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
        if (!Page.IsPostBack)
        {
            SetFilterDefault();
        }
    }

    #region Overrides
    protected override void PageInitialize()
    {
        this.GridIDs.Add("uxGrid");
        this.ExporterIDs.Add("uxExportTop");
        base.PageInitialize();
        IsBindDataOnLoad = true;
        this.IsSecureCSRF = true;
    }

    protected override void DoGridNeedDataSource(AS.Controls.Grid.ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        if (sender == uxGrid)
        {
            OnDataBindControls(DataBindAction.BindDataGrid);
        }
    }

    protected override void DoItemDataBound(AS.Controls.Grid.ASGrid sender, GridItemEventArgs e)
    {
        if (IsIntruderDetected) return;
        if (sender != uxGrid) return;
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            var activateDeactivateItem = dataItem["ActivateDeactivate"];
            if (!e.Item.IsInEditMode)
            {
                var rowData = e.Item.DataItem as DataRowView;
                var statusItem = dataItem["Reason"];

                bool isActive = (bool)rowData["IsActive"];


                if (!statusItem.Text.Equals("&nbsp;"))
                    statusItem.Text = VeraCodeSolution.ValidateResponseData(rowData["Reason"].ToString());

                activateDeactivateItem.Text = VeraCodeSolution.DoVeraCode(string.Format("<a href=\"#\" onclick=\"ActivateDeactivate({0}, {1}); return false;\">{2}</a>",
                    rowData["EscalationReasonID"], isActive.ToString().ToLower(), (isActive ? GetLocalResourceObject("rm_Reasonmaintenance_aspx_cs_DeActive").ToString() : GetLocalResourceObject("rm_Reasonmaintenance_aspx_cs_Active").ToString())));

                if (!isActive)
                {
                    statusItem.ForeColor = Color.Gray;
                    statusItem.Font.Italic = true;
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

    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, ExportConfig exportConfig)
    {
        if (IsIntruderDetected) return;
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
        uxGrid.Columns.FindByUniqueName("Edit").Visible = false;
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        if (IsIntruderDetected) return;
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindDataGrid:
                if (string.IsNullOrEmpty(uxGrid.AS_SortExpression))
                    uxGrid.AS_SortExpression = "Reason ASC";

                FilterParameterCollection parameters = new FilterParameterCollection();
                parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                parameters.AddLanguageID();
                parameters.Add(new FilterParameter("@IsActive", FilterStatus, DbType.Int32));
                parameters.Add(new FilterParameter("@SortOrder", uxGrid.AS_SortExpression, DbType.String));
                DataTable tbl = WebServices.RiskServices.GetReports("spa_RM_MCF_GetEscalationReason", parameters);
                uxGrid.DataSource = tbl;
                if (tbl.Rows.Count == 0)
                    uxGrid.AllowSorting = false;
                else
                    uxGrid.AllowSorting = true;
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
                uxAddEscalationTextErrMsg.Message = string.Empty;
                uxAddEscalationTextLabel.CssClass = string.Empty;
                pnlAddEscalation.Visible = true;
                uxAddEscalationText.Text = string.Empty;
                uxAddEscalationText.Focus();
                uxGrid.MasterTableView.ClearEditItems();
                uxGrid.MasterTableView.Rebind();
            }
            break;
            case PostBackAction.OpenFormEdit:
            {
                //AjaxAddResponseScript("HideCreatePanel('" + pnlAddEscalation.ClientID + "');");
                pnlAddEscalation.Visible = false;
            }
            break;
            case PostBackAction.CreateAction:
            {
                string status = uxAddEscalationText.Text.Trim();

                FilterParameterCollection parameters = new FilterParameterCollection();
                parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                parameters.Add(new FilterParameter("@Description", status, DbType.String));
                parameters.Add(new FilterParameter("@ReturnValue", 1, DbType.Int32, true));
                FilterParameterCollection parameterOut = new FilterParameterCollection();
                WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_AddEscalationReason", parameters, out parameterOut);
                int result = Convert.ToInt32(parameterOut[0].ParameterValue);
                uxGrid.Rebind();
                if (RiskErrorCode.HasError(result))
                {
                    uxAddEscalation.Focus();
                    uxAddEscalationTextErrMsg.Message = RiskErrorCode.GetErrorMessage(result, RiskErrorCode.ObjectType.Others);
                    uxAddEscalationTextErrMsg.ShowOnLoad = true;
                    uxAddEscalationTextLabel.CssClass = RiskGeneral.CSS_ERROR_LABEL;
                    return;
                }
                pnlAddEscalation.Visible = false;                
            }
            break;
            case PostBackAction.UpdateAction:
            {
                Telerik.Web.UI.GridCommandEventArgs e = sender as Telerik.Web.UI.GridCommandEventArgs;

                int reasonID = Int32.Parse(((HiddenField)e.Item.FindControl("txtEscalationReasonID")).Value.Trim());
                GridEditableItem editedItem = e.Item as GridEditableItem;
                string reason = ((editedItem.FindControl("txtEditReasonName") as TextBox).Text.Trim());
                               
                FilterParameterCollection parameters = new FilterParameterCollection();
                parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                parameters.Add(new FilterParameter("@ReasonID", reasonID, DbType.Int32));
                parameters.Add(new FilterParameter("@Description", reason, DbType.String));
                parameters.Add(new FilterParameter("@IsActive", -1, DbType.Int32));
                parameters.Add(new FilterParameter("@ReturnValue", 1, DbType.Int32, true));

                FilterParameterCollection parameterOut = new FilterParameterCollection();
                WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_UpdateEscalationReason", parameters, out parameterOut);
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
                    uxGrid.MasterTableView.ClearEditItems();
                    uxGrid.MasterTableView.Rebind();
                }                
            }
            break;
            case PostBackAction.CancelAction:
            {
                //view event uxAddEscalation_Click
            }
            break;
            case PostBackAction.ActivateDeactivate:
            {
                string[] parts = uxActivateDeactivateData.Value.Split(';');
                int statusID = Int32.Parse(parts[0]);
                int isActive = (string.Compare(parts[1], "true") == 0 ? 0 : 1);

                FilterParameterCollection parameters = new FilterParameterCollection();
                parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                parameters.Add(new FilterParameter("@ReasonID", statusID, DbType.Int32));
                parameters.Add(new FilterParameter("@IsActive", isActive, DbType.Int32));
                parameters.Add(new FilterParameter("@ReturnValue", 1, DbType.Int32, true));
                FilterParameterCollection parameterOut = new FilterParameterCollection();
                WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_DeleteEscalationReason", parameters, out parameterOut);
                int result = Convert.ToInt32(parameterOut[0].ParameterValue);

                if (result == 0)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "showMsg", string.Format("alert('{0}')", MessageManager.Generic_ProcessingFailed), true);
                }

                uxGrid.MasterTableView.ClearEditItems();
                uxGrid.MasterTableView.Rebind();
            }
            break;
            case PostBackAction.ChangeFilter:
            {
                Session[SESSION_FILTERING_OPTIONS] = this.FilterStatus.ToString();
                uxGrid.Rebind();
            }
            break;
        }
    }
    #endregion

    protected void uxChangeFilterStatus_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.ChangeFilter);
    }

    protected void uxGrid_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName == RadGrid.EditCommandName)
            OnPostBackActions(PostBackAction.OpenFormEdit);
        else if (e.CommandName == RadGrid.UpdateCommandName)
        {
            OnPostBackActions(PostBackAction.UpdateAction, e);
        }        
    }

    protected void uxGrid_PagerEventClick(object source, AS.Controls.Grid.ASPagerEventArgs e)
    {
        uxGrid.MasterTableView.ClearEditItems();
    }

    protected void uxAddEscalationCancel_Click(object sender, EventArgs e)
    {
        pnlAddEscalation.Visible = false;
        uxGrid.MasterTableView.Rebind();
    }

    protected void uxCreateNew_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.OpenFormCreate);
    }

    protected void uxAddEscalation_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.CreateAction);
    }

    protected void uxActivateDeactivate_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.ActivateDeactivate);
    }

    private void SetFilterDefault()
    {
        if (Session[SESSION_FILTERING_OPTIONS] != null)
        {
            string status = GeneralFuncsLib.NvlString(Session[SESSION_FILTERING_OPTIONS]);
            if (status.Length > 0)
            {
                int statusValue = 0;

                if (Int32.TryParse(status, out statusValue))
                    this.FilterStatus = statusValue;
            }
        }
    }

    protected void cancel_click(object sender, EventArgs e)
    {
        uxGrid.MasterTableView.ClearEditItems();
        uxGrid.MasterTableView.Rebind();
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

    

}

