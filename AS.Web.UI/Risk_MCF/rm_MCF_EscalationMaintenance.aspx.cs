using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Web.Services;
using Telerik.Web.UI;
using Resources;
using AS.Controls.Exporter;
using AS.Controls.Pages;
using AS.Common;
using AS.Common.DBManager;
using System.Web;

[PagePermission("RskEsca,MSRskEsca")]
public partial class rm_MCF_EscalationMaintenance : ReportPage
{
    #region Constants

    private const string SESSION_FILTERING_OPTIONS = "EscalationStatusFilteringOptions";

    #endregion Constants

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
        ChangeFilter,
        Sorting,
    }

    #endregion

    #region Propeties

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

    #endregion Propeties

    #region Methods

    #region Protected Methods
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected)
        {
            return;
        }
        if (!Page.IsPostBack)
        {
            SetFilterDefault();
        }

        uxAddEscalationLabel.CssClass = "control-label";
        uxAddEscalationTextMsg.ShowOnLoad = false;
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
        if (IsIntruderDetected || sender != uxGrid)
        {
            return;
        }

        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            var activateDeactivateItem = dataItem["ActivateDeactivate"];
            if (!e.Item.IsInEditMode)
            {
                var rowData = e.Item.DataItem as DataRowView;
                var statusItem = (Literal)dataItem["Status"].FindControl("LiteralStatus");
                bool isActive = (bool)rowData["IsActive"];

                if (!statusItem.Text.Equals(RiskGeneral.NBSP))
                {
                    statusItem.Text = VeraCodeExtensions.ValidateResponseData(rowData["Status"].ToString());
                }

                activateDeactivateItem.Text = VeraCodeSolution.DoVeraCode(RiskGeneral.BuildActiveDeactiveLink(rowData["EscalationStatusID"], isActive));

                if (!isActive)
                {
                    statusItem.Text = VeraCodeSolution.DoVeraCode(string.Format(RiskGeneral.MUTED_TEXT_FORMAT, statusItem.Text));
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
        if (IsIntruderDetected)
        {
            return;
        }
        base.DoNeedExportConfig(sender, exportConfig);
        exportConfig.ReportHeader = uxExportTop.GridHeader;
        exportConfig.FileName = HttpUtility.UrlEncode( GeneralFuncsLib.FormatFileName(uxExportTop.GridHeader));

        if (Request.Browser.Browser.ToUpper() == WebSiteConstants.BROWSER_INTERNETEXPLORER)
        {
            exportConfig.FileName = Server.UrlPathEncode(GeneralFuncsLib.FormatFileName(uxExportTop.GridHeader));
        }
        else
        {
            exportConfig.FileName = GeneralFuncsLib.FormatFileName(uxExportTop.GridHeader);
        }
        uxGrid.Columns.FindByUniqueName("ActivateDeactivate").Visible = false;
        uxGrid.Columns.FindByUniqueName("Edit").Visible = false;
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        if (IsIntruderDetected)
        {
            return;
        }
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindDataGrid:
                if (uxGrid.AS_SortExpression.IsNullOrEmpty())
                {
                    uxGrid.AS_SortExpression = "Status ASC";
                }
                DataTable tbl = WebServices.RiskServices.GetEscalationStatus(
                    GetLoggedInUserParams().AddLanguageID(), FilterStatus, uxGrid.AS_SortExpression, true);
                uxGrid.DataSource = tbl;
                uxGrid.AllowSorting = tbl.Rows.Count == 0;
                break;
        }
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        if (IsIntruderDetected)
        {
            return;
        }
        switch ((PostBackAction)type)
        {
            case PostBackAction.OpenFormCreate:
                {
                    uxAddEscalationLabel.CssClass = string.Empty;
                    uxAddEscalationTextMsg.Message = string.Empty;
                    pnlAddEscalation.Visible = true;
                    uxAddEscalationText.Text = string.Empty;
                    uxAddEscalationText.Focus();
                    uxGrid.MasterTableView.ClearEditItems();
                    uxGrid.MasterTableView.Rebind();
                }
                break;
            case PostBackAction.OpenFormEdit:
                {
                    AjaxAddResponseScript("HideCreatePanel('" + pnlAddEscalation.ClientID + "');");
                }
                break;
            case PostBackAction.CreateAction:
                {
                    string statusName = uxAddEscalationText.Text.Trim();
                    int result = WebServices.RiskServices.AddEscalationStatus(GetLoggedInUserParams(), statusName, true);

                    if (RiskErrorCode.HasError(result))
                    {
                        ShowErrorMessageWhenAdding(result);
                        return;
                    }

                    pnlAddEscalation.Visible = false;
                    uxGrid.Rebind();
                }
                break;
            case PostBackAction.UpdateAction:
                {
                    Telerik.Web.UI.GridCommandEventArgs e = sender as Telerik.Web.UI.GridCommandEventArgs;

                    int escalationStatusID = Int32.Parse(((HiddenField)e.Item.FindControl("txtEscalationStatusID")).Value.Trim());
                    GridEditableItem editedItem = e.Item as GridEditableItem;
                    string statusName = ((editedItem.FindControl("txtEditStatusName") as TextBox).Text.Trim());

                    if (statusName.IsNullOrEmpty())
                    {
                        RiskGeneral.ShowMessageAjax((ReportPage)this.Page,
                            VeraCodeExtensions.DoVeraCode(GetLocalResourceObject("rm_EscalationMaintenance_aspx_cs_Text1").ToString() + " " + Resources.ValMsg.Required));
                        return;
                    }

                    int result = WebServices.RiskServices.UpdateEscalationStatus(GetLoggedInUserParams(), escalationStatusID, statusName, -1, true);
                    if (RiskErrorCode.HasError(result))
                    {
                        RiskGeneral.ShowMessageAjax((ReportPage)this.Page,
                            VeraCodeExtensions.DoVeraCode(
                                RiskErrorCode.GetErrorMessage(result, RiskErrorCode.ObjectType.Escalation)));
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
                    int result = WebServices.RiskServices.ActiveDeactiveEscalationStatus(GetLoggedInUserParams(), statusID, isActive, true);
                    if (result == RiskErrorCode.ERROR_PROCESSING_FAILED)
                    {
                        RiskGeneral.ShowMessage((ReportPage)this.Page,
                            VeraCodeExtensions.DoVeraCode(RiskErrorCode.GetErrorMessage(result, RiskErrorCode.ObjectType.Others)));
                        return;
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
            case PostBackAction.Sorting:
                {
                    uxGrid.MasterTableView.ClearEditItems();
                }
                break;
        }
    }

    #endregion Overrides

    protected void uxChangeFilterStatus_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.ChangeFilter);
    }

    protected void uxGrid_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName == RadGrid.EditCommandName)
        {
            OnPostBackActions(PostBackAction.OpenFormEdit);
        }
        else if (e.CommandName == RadGrid.UpdateCommandName)
        {
            OnPostBackActions(PostBackAction.UpdateAction, e);
        }
        else if (e.CommandName == RadGrid.SortCommandName)
        {
            OnPostBackActions(PostBackAction.Sorting, e);
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

    #endregion Protected Methods

    #region Private Methods

    private void ShowErrorMessageWhenAdding(int errorCode)
    {
        uxAddEscalation.Focus();
        uxAddEscalationLabel.CssClass = RiskGeneral.CSS_ERROR_LABEL;
        uxAddEscalationTextMsg.ShowOnLoad = true;
        uxAddEscalationTextMsg.Message = RiskErrorCode.GetErrorMessage(errorCode, RiskErrorCode.ObjectType.Others);
        uxGrid.Rebind();
    }

    #endregion Private Methods

    [WebMethod(EnableSession = true)]
    public static string[] GetEscalationCountByEscalationStatus(int statusID, bool isActive)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@StatusID", statusID, DbType.Int32));
        parameters.Add(new FilterParameter("@Closed", 0, DbType.Int32));
        DataTable tb = WebServices.RiskServices.GetReports("spa_RM_MCF_GetEscalationCountByEscalationStatus", parameters);
        int result = Convert.ToInt32(tb.Rows[0][0].ToString());
        if (result == 0)
        {
            return new string[] { result.ToString(), statusID.ToString(), isActive.ToString().ToLower() };
        }
        else
        {
            ReportPage rp = new ReportPage();
            string queryString = rp.BuildSecureQueryString(string.Format("StatusID={0}", statusID));
            return new string[] { result.ToString(), queryString };
        }
    }

    #endregion Methods
}
