using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Web.Services;
using Telerik.Web.UI;
using AS.Controls.Exporter;
using AS.Controls.Pages;
using AS.Common;
using AS.Common.DBManager;
using Resources;
using System.Web;

[PagePermission("RskReso,MSRskReso")]
public partial class rm_MCF_ResolutionMaintenance : ReportPage
{
    #region Enums

    enum DataBindAction
    {
        BindResolutionGrid,
    }

    enum PostBackAction
    {
        ChangeActivate,
        ChangeStatus,
        CreateNewClick,
        AddResolution,
        CancelAddResolution,
        UpdateResolution,
    }

    #endregion

    #region Constants

    private const string SESSION_FILTERING_OPTIONS = "EscalationResolutionFilteringOptions";

    #endregion Constants

    #region Properties

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

    #endregion Properties

    #region Methods

    #region Public Methods
    
    [WebMethod(EnableSession = true)]
    public static string[] GetEscalationCountByResolution(int resolutionID, bool isActive)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@ResolutionID", resolutionID, DbType.Int32));
        parameters.Add(new FilterParameter("@Closed", 0, DbType.Int32));
        DataTable tb = WebServices.RiskServices.GetReports("spa_RM_MCF_GetEscalationCountByResolution", parameters);
        int result = Convert.ToInt32(tb.Rows[0][0].ToString());
        if (result == 0)
        {
            return new string[] { result.ToString(), resolutionID.ToString(), isActive.ToString().ToLower() };
        }
        else
        {
            ReportPage rp = new ReportPage();
            string queryString = rp.BuildSecureQueryString(string.Format("ResolutionID={0}", resolutionID));
            return new string[] { result.ToString(), queryString };
        }
    }

    #endregion Public Methods

    #region Protected Methods

    protected override void PageInitialize()
    {
        this.GridIDs.Add("uxGrid");
        this.ExporterIDs.Add("uxExportTop");
        this.ExporterIDs.Add("uxExportBottom");
        base.PageInitialize();
        this.IsSecureCSRF = true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected)
        {
            return;
        }
        IsBindDataOnLoad = true;
        if (!IsPostBack)
        {
            SetFilterDefault();
        }
        uxAddResolutionTextLabel.CssClass = "control-label";
        uxAddResolutionTextErrMsg.ShowOnLoad = false;
    }

    protected void uxChangeFilterStatus_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.ChangeStatus);        
    }

    protected void uxGrid_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (IsIntruderDetected) return;

        if (e.CommandName == RadGrid.EditCommandName)
        {
            //pnlAddResolution.Visible = false;
            AjaxAddResponseScript("HideCreatePanel('" + pnlAddResolution.ClientID + "');");
        }
        else if (e.CommandName == RadGrid.UpdateCommandName)
        {
            OnPostBackActions(PostBackAction.UpdateResolution, e);
        }
    }

    protected void uxGrid_PagerEventClick(object source, AS.Controls.Grid.ASPagerEventArgs e)
    {
        uxGrid.MasterTableView.ClearEditItems();
    }

    protected void uxCreateNew_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.CreateNewClick);
    }

    protected void uxAddResolution_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.AddResolution);
    }

    protected void uxAddResolutionCancel_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.CancelAddResolution);
        uxGrid.MasterTableView.Rebind();
    }

    protected void uxActivateDeactivate_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.ChangeActivate);
    }    

    protected override void DoGridNeedDataSource(AS.Controls.Grid.ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.BindResolutionGrid, sender);
    }

    protected override void DoItemDataBound(AS.Controls.Grid.ASGrid sender, GridItemEventArgs e)
    {
        if (IsIntruderDetected)
            return;

        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            var activateDeactivateItem = dataItem["ActivateDeactivate"];
            if (!e.Item.IsInEditMode)
            {
                var rowData = e.Item.DataItem as DataRowView;
                var resolutionItem = (Literal)dataItem["Resolution"].FindControl("LiteralResolution");                
                bool isActive = (bool)rowData["IsActive"];

                // Resolution column
                if (!resolutionItem.Text.Equals(RiskGeneral.NBSP))
                {
                    resolutionItem.Text = VeraCodeSolution.ValidateResponseData(rowData["Resolution"].ToString());
                }

                // Active/Deactive column
                activateDeactivateItem.Text = VeraCodeSolution.DoVeraCode(RiskGeneral.BuildActiveDeactiveLink(rowData["ResolutionID"], isActive));

                if (!isActive)
                {
                    resolutionItem.Text = VeraCodeSolution.DoVeraCode(string.Format(RiskGeneral.MUTED_TEXT_FORMAT, resolutionItem.Text));
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

    protected void cancel_click(object sender, EventArgs e)
    {
        uxGrid.MasterTableView.ClearEditItems();
        uxGrid.MasterTableView.Rebind();
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

        uxGrid.Columns.FindByUniqueName("ActivateDeactivate").Visible = false;
        uxGrid.Columns.FindByUniqueName("Edit").Visible = false;
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindResolutionGrid:
                {
                    if (IsIntruderDetected)
                    {
                        return;
                    }

                    if (string.IsNullOrEmpty(uxGrid.AS_SortExpression))
                    {
                        uxGrid.AS_SortExpression = "Resolution ASC";
                    }

                    DataTable tbl = WebServices.RiskServices.GetResolution(GetLoggedInUserParams().AddLanguageID(), FilterStatus, uxGrid.AS_SortExpression, true);

                    if (tbl.Rows.Count > 0)
                    {
                        uxGrid.MasterTableView.AllowPaging = true;
                        uxGrid.MasterTableView.PagerStyle.AlwaysVisible = true;
                    }
                    else
                    {
                        uxGrid.MasterTableView.AllowPaging = false;
                        uxGrid.MasterTableView.PagerStyle.AlwaysVisible = false;
                    }

                    uxGrid.DataSource = tbl;
                    if (tbl.Rows.Count == 0)
                    {
                        uxGrid.AllowSorting = false;
                    }
                    else
                    {
                        uxGrid.AllowSorting = true;
                    }
                } 
                break;
        }
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.ChangeActivate:
                {
                    string[] parts = uxActivateDeactivateData.Value.Split(';');
                    int resolutionID = Int32.Parse(parts[0]);
                    int isActive = (string.Compare(parts[1], "true") == 0 ? 0 : 1);

                    int result = WebServices.RiskServices.ActiveDeactiveResolution(GetLoggedInUserParams(), resolutionID, isActive, true);
                    if (result == RiskErrorCode.ERROR_PROCESSING_FAILED)
                    {
                        RiskGeneral.ShowMessage((ReportPage)this.Page, 
                            RiskErrorCode.GetErrorMessage(result, RiskErrorCode.ObjectType.Others));
                    }

                    uxGrid.MasterTableView.ClearEditItems();
                    uxGrid.MasterTableView.Rebind();
                }
                break;
            case PostBackAction.ChangeStatus:
                {
                    Session[SESSION_FILTERING_OPTIONS] = this.FilterStatus.ToString();
                    uxGrid.Rebind();
                }
                break;
            case PostBackAction.CreateNewClick:
                {
                    uxAddResolutionTextLabel.CssClass = string.Empty;
                    uxAddResolutionTextErrMsg.Message = string.Empty;
                    pnlAddResolution.Visible = true;
                    uxAddResolutionText.Text = string.Empty;
                    uxAddResolutionText.Focus();
                    uxGrid.MasterTableView.ClearEditItems();
                    uxGrid.MasterTableView.Rebind();
                }
                break;
            case PostBackAction.AddResolution:
                {
                    if (IsIntruderDetected) return;

                    string resolution = uxAddResolutionText.Text.Trim();
                    int result = WebServices.RiskServices.AddResolution(GetLoggedInUserParams(), resolution, true);

                    if (RiskErrorCode.HasError(result))
                    {
                        ShowErrorMessageWhenAdding(result);
                        return;
                    }

                    pnlAddResolution.Visible = false;
                    uxGrid.Rebind();
                }
                break;
            case PostBackAction.CancelAddResolution:
                {
                    pnlAddResolution.Visible = false;
                }
                break;
            case PostBackAction.UpdateResolution:
                {
                    pnlAddResolution.Visible = false;
                    Telerik.Web.UI.GridCommandEventArgs e = sender as Telerik.Web.UI.GridCommandEventArgs;

                    int resolutionID = int.Parse(((HiddenField)e.Item.FindControl("txtResolutionID")).Value.Trim());
                    string description = ((TextBox)e.Item.FindControl("txtEditResolution")).Text;
                    int result = WebServices.RiskServices.UpdateResolution(GetLoggedInUserParams(), resolutionID, description);

                    if (RiskErrorCode.HasError(result))
                    {
                        RiskGeneral.ShowMessageAjax((ReportPage)this.Page,
                            RiskErrorCode.GetErrorMessage(result, RiskErrorCode.ObjectType.Resolution));
                        return;
                    }

                    uxGrid.MasterTableView.ClearEditItems();
                    uxGrid.MasterTableView.Rebind();
                }
                break;
        }
    }

    #endregion Protected Methods
    
    #region Private Methods
    
    private void SetFilterDefault()
    {
        if (Session[SESSION_FILTERING_OPTIONS] != null)
        {
            string status = GeneralFuncsLib.NvlString(Session[SESSION_FILTERING_OPTIONS]);
            if (status.Length > 0)
            {
                int statusValue = 0;

                if (Int32.TryParse(status, out statusValue))
                {
                    this.FilterStatus = statusValue;
                }
            }
        }
    }

    private void ShowErrorMessageWhenAdding(int errorCode)
    {
        uxAddResolution.Focus();
        uxAddResolutionTextLabel.CssClass = RiskGeneral.CSS_ERROR_LABEL;
        uxAddResolutionTextErrMsg.ShowOnLoad = true;
        uxAddResolutionTextErrMsg.Message = RiskErrorCode.GetErrorMessage(errorCode, RiskErrorCode.ObjectType.Others);
        uxGrid.Rebind();
    }

    #endregion Private Methods

    #endregion Methods
}
