using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AS.Controls.Pages;
using AS.Web.UI.Controls;
using AS.Common.DBManager;
using System.Data;
using AS.Controls.Grid;
using AS.Web.Business;
using Telerik.Web.UI;
using AS.Common;
using AS.Security.WS.Entities;

[PagePermission("UserAuditReport,MSUserAuditReport")]
public partial class UserAuditReport : ReportPage
{
    #region Enum
    enum DataBindAction
    {
        BindMessageGrid,
    }
    enum PostBackAction
    {
        DoSearching,
    }
    #endregion
    private HierarchyFilterValue _reportValue = null;
    private bool IsSearch = false;

    //44648 - VW - Implement Change Log to review permission changes made to user roles
    public int HierarchyId
    {
        get
        {
            int hierarchyId = 0;
            if (SecureQueryString.IsNotNullData() && !SecureQueryString["hierarchyId"].IsNullOrEmpty())
            {
                int.TryParse(SecureQueryString["hierarchyId"].ToString(), out hierarchyId);
            }
            return hierarchyId;
        }
    }
    //End

    protected override void PageInitialize()
    {
        this.ExporterIDs.Add("uxExporterBottom");
        base.PageInitialize();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;

        IsBindDataOnLoad = true;
        if (!IsPostBack)
        {
            GetDateFilter();
            //44648 - VW - Implement Change Log to review permission changes made to user roles
            //Set filter item when navigate form Changelog modal
            if (HierarchyId != 0)
            {
                Hierarchy hierarchyObj = WebServices.SecurityServices.GetHierarchyById(HierarchyId);
                uxRange.Checked = true;
                uxFromDate.SelectedDate = hierarchyObj.CreatedDate;
                uxEndDate.SelectedDate = DateTime.Now;
                uxChangedEntity.Text = hierarchyObj.HierarchyName;
                uxRdEqual.Checked = true;
                uxRdContain.Checked = false;
            }

            //End
            SetDateFilter();
        }
    }


    protected override void DoInitializeExport(AS.Controls.UserControls.UxExport sender, AS.Controls.UserControls.ExportEventArgs e)
    {
        if (sender.ExportButtonType == AS.Controls.UserControls.UxExport.ExportType.Excel)
        {
            DataTable dt = e.DataSource;

            foreach (DataRow row in dt.Rows)
            {
                row["OldValue"] = row["OldValue"].ToString().Replace(" ", "&nbsp;");
                row["NewValue"] = row["NewValue"].ToString().Replace(" ", "&nbsp;");
                row["ChangedFirstName"] = row["ChangedFirstName"].ToString().Replace(" ", "&nbsp;");
                row["ChangedLastName"] = row["ChangedLastName"].ToString().Replace(" ", "&nbsp;");
            }
        }


        base.DoInitializeExport(sender, e);
    }
    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, AS.Controls.Exporter.ExportConfig exportConfig)
    {
        base.DoNeedExportConfig(sender, exportConfig);
        string headerName = GetLocalResourceObject("UserAuditReport_aspx_cs_AdministrativeReport").ToString();
        if (sender.ExportButtonType == AS.Controls.UserControls.UxExport.ExportType.Excel)
        {
            exportConfig.AllowHtmlEncoded = false;
            headerName = HttpUtility.HtmlEncode(GetLocalResourceObject("UserAuditReport_aspx_cs_AdministrativeReport").ToString());
        }

        exportConfig.ReportHeader = headerName;
        exportConfig.FileName = HttpUtility.UrlEncode(GeneralFuncsLib.FormatFileName(GetLocalResourceObject("UserAuditReport_aspx_cs_AdministrativeReport").ToString()));


    }
    protected void uxGridUserAuditReport_OnItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView rowView = e.Item.DataItem as DataRowView;
            Literal oldValueLabel = dataItem.FindControl("OldValue") as Literal;
            Literal newValueLabel = dataItem.FindControl("NewValue") as Literal;
            string newValue = rowView["NewValue"].ToString();
            string oldValue = rowView["OldValue"].ToString();
            oldValueLabel.Text = VeraCodeSolution.GetOutputHtmlString(oldValue);
            newValueLabel.Text = VeraCodeSolution.GetOutputHtmlString(newValue);
        }
    }
    protected override void DoGridNeedDataSource(ASGrid sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.BindMessageGrid, sender);
    }

    protected override void DoItemDataBound(AS.Controls.Grid.ASGrid sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (sender == uxReportGrid)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = e.Item as GridDataItem;
                DataRowView dataRow = e.Item.DataItem as DataRowView;

                //41876 : Bug #35956: [User Audit Report] The value is not wrapping in 
                //the Old Value and New Value fields

                //dataItem["OldValue"].Style.Add("white-space", "pre");
                //dataItem["NewValue"].Style.Add("white-space", "pre");
                //dataItem["ChangedFirstName"].Style.Add("white-space", "pre");
                //dataItem["ChangedLastName"].Style.Add("white-space", "pre");
            }
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindMessageGrid:
                {
                    SetDateFilter();
                    FilterParameterCollection _parames = new FilterParameterCollection();
                    _parames.AddLoggedInUserReportingParams();
                    _parames.Add(new FilterParameter("@DateFilterMode", (int)this._reportValue.DateOption, DbType.Int32));
                    _parames.Add(new FilterParameter("@BeginDate", this._reportValue.DateOptionValue.From, DbType.Date));
                    DateTime endDate = this._reportValue.DateOption == DateOptionMode.DateRange ? this._reportValue.DateOptionValue.To : this._reportValue.DateOptionValue.From;
                    _parames.Add(new FilterParameter("@EndDate", endDate, DbType.Date));
                    _parames.Add(new FilterParameter("@ChangedEntity", EscapeSpecialCharacter(uxChangedEntity.Text), DbType.AnsiString));
                    //44649 - VW - User Audit Report - New filter options for Changed Entity
                    //1.Contain,2.Equal
                    _parames.Add(new FilterParameter("@ChangedEntityFilterType", uxRdEqual.Checked ? 2 : 1, DbType.AnsiString));
                    if (ViewState["Administator"] != null)
                    {
                        _parames.Add(new FilterParameter("@AdministratorUserID", ViewState["Administator"].ToString(), DbType.String));
                    }
                    _parames.AddLanguageID();
                    if (IsSearch)
                    {
                        uxReportGrid.MasterTableView.CurrentPageIndex = 0;
                    }
                    ((ASGrid)sender).DataSourceInvoker = new ASFuncInvoker(WebServices.RiskServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { "spa_GetAuditUserChanges", ReportServices.ConvertToFilterParamWSArray(_parames) });
                }
                break;
        }
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.DoSearching:
                {
                    uxReportGrid.Rebind();
                }
                break;
        }
    }

    protected void uxSearch_Click(object sender, EventArgs e)
    {
        IsSearch = true;
        SetDateFilter();
        if (uxAdministratorName.SelectedValue != string.Empty)
        {
            ViewState["Administator"] = uxAdministratorName.SelectedValue;
        }
        else
        {
            ViewState["Administator"] = null;
            uxAdministratorName.Text = string.Empty;
        }
        BindChangedBy(uxAdministratorName.Text);
        OnPostBackActions(PostBackAction.DoSearching);
    }

    // Get Date from Session if not null and assign to control
    private void GetDateFilter()
    {
        this._reportValue = SavedReportFilterValue;
        if (this._reportValue == null)
        {
            this._reportValue = new HierarchyFilterValue();
            this._reportValue.DateOption = DateOptionMode.DateRange;
            this._reportValue.DateOptionValue.To = DateTime.Now;
            this._reportValue.DateOptionValue.From = DateTime.Now.GetFirstDayOfMonth();
        }
        switch (_reportValue.DateOption)
        {
            case DateOptionMode.Daily:
                this.uxDaily.Checked = true;
                this.uxDate.SelectedDate = this._reportValue.DateOptionValue.From;
                break;
            case DateOptionMode.Monthly:
                this.uxMonthly.Checked = true;
                this.uxDate.SelectedDate = this._reportValue.DateOptionValue.From;
                break;
            case DateOptionMode.DateRange:
                this.uxRange.Checked = true;
                this.uxFromDate.SelectedDate = this._reportValue.DateOptionValue.From;
                this.uxEndDate.SelectedDate = this._reportValue.DateOptionValue.To;
                break;
        }
    }

    // Set Date to Session
    private void SetDateFilter()
    {
        //Get report filter form session
        if (SavedReportFilterValue != null)
            this._reportValue = SavedReportFilterValue;
        else
            this._reportValue = new HierarchyFilterValue();
        // Set Date Option         
        if (uxDaily.Checked)
        {
            this._reportValue.DateOption = DateOptionMode.Daily;
            this._reportValue.DateOptionValue.From = this._reportValue.DateOptionValue.To = uxDate.SelectedDate.Value;
        }
        else if (uxMonthly.Checked)
        {
            this._reportValue.DateOption = DateOptionMode.Monthly;
            this._reportValue.DateOptionValue.From = uxDate.SelectedDate.Value;
        }
        else
        {
            this._reportValue.DateOption = DateOptionMode.DateRange;
            this._reportValue.DateOptionValue.From = uxFromDate.SelectedDate.Value;
            this._reportValue.DateOptionValue.To = uxEndDate.SelectedDate.Value;
        }

        // Set report filter
        if (SavedReportFilterValue != null)
            SavedReportFilterValue = this._reportValue;

    }

    //43659 - VW - User Audit Report - Changed By logic update
    protected void uxAdministratorName_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        BindChangedBy(e.Text);
    }

    private void BindChangedBy(string searchKey)
    {
        uxAdministratorName.Items.Clear();
        if (searchKey.Length > 2)
        {
            string changeBy = EscapeSpecialCharacter(searchKey);
            FilterParameterCollection param = new FilterParameterCollection();
            param.Add(new FilterParameter("@UserHierarchyID", SessionManager.CurrentHierarchyId, DbType.Int32));
            param.Add(new FilterParameter("@ChangedByFilter", changeBy, DbType.AnsiString));
            param.AddLoggedInUserReportingParams();
            uxAdministratorName.DataTextField = "UserNameFull";
            uxAdministratorName.DataValueField = "UserID";
            uxAdministratorName.DataSource = WebServices.SecurityServices.GetReports("spa_SEC_GetAdministratorList", param);
            uxAdministratorName.DataBind();
        }
        uxAdministratorName.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(string.Empty, string.Empty));
    }

    private string EscapeSpecialCharacter(string text)
    {
        return text.Replace("[", "[[]").Replace("%", "[%]").Replace(",", "[,]").Replace("_", "[_]");
    }
}
