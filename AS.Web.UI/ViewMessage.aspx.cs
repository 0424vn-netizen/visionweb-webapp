using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AS.Controls.Pages;
using AS.Web.UI.Controls;
using AS.Common.DBManager;
using AS.Controls.Grid;
using System.Data;
using AS.Web.Business;

[PagePermission("MSViewMsg")]
public partial class ViewMessage : ReportPage
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

    protected override void PageInitialize()
    {
        this.GridIDs.Add("uxReportGrid");
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
            SetDateFilter();
        }
    }

    

    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, AS.Controls.Exporter.ExportConfig exportConfig)
    {
        base.DoNeedExportConfig(sender, exportConfig);
        exportConfig.ReportHeader = uxExporter.GridHeader;
        exportConfig.FileName = GeneralFuncsLib.FormatFileName(uxExporter.GridHeader);
    }

    protected override void DoGridNeedDataSource(ASGrid sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.BindMessageGrid, sender);
    }

    protected override void DoItemDataBound(AS.Controls.Grid.ASGrid sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        
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
                    _parames.Add(new FilterParameter("@DateFilterMode", (int)_reportValue.DateOption, DbType.Int32));
                    _parames.Add(new FilterParameter("@BeginDate", _reportValue.DateOptionValue.From, DbType.Date));
                    DateTime endDate = _reportValue.DateOption == DateOptionMode.DateRange ? _reportValue.DateOptionValue.To : _reportValue.DateOptionValue.From;
                    _parames.Add(new FilterParameter("@EndDate", endDate, DbType.Date));
                    ((ASGrid)sender).DataSourceInvoker = new ASFuncInvoker(WebServices.RiskServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { "spa_GetMyMessages", ReportServices.ConvertToFilterParamWSArray(_parames) });
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
        SetDateFilter();
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
            if (GeneralFuncsLib.HasExtendedSetting("FILTERING_OPTIONS_DATERANGE"))
            {
                this._reportValue.DateOptionValue.From = DateTime.Now.AddDays(-90);
            }
            else
            {
                this._reportValue.DateOptionValue.From = DateTime.Now.GetFirstDayOfMonth();
            }
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
        if(SavedReportFilterValue != null)
            SavedReportFilterValue = this._reportValue;
    }
}
