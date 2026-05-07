using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Global;
using AS.Controls.Pages;
using System;
using System.Data;
using System.Web;
using Telerik.Web.UI;

[PagePermission("RskManAssAlert,MSRskManAssAlert")]
public partial class rm_MCF_MgmtReport_AssignmentAlert : NonReportPage
{
    #region Enums
    enum DataBindAction
    {
        BindReportGrid,
        BindDetailGrid,
    }
    enum PostBackAction
    {
        ExportExcel,
        ExportCSV,
        DoFilterAction,
    }
    enum ViewMode
    {
        Date = 0,
        Assignment = 1,
        Parameter = 2
    }
    #endregion
    #region Properties

    private FilterParameterCollection _parameters = null;

    #endregion
    private void BuildBaseParameters()
    {
        _parameters = new FilterParameterCollection();
        _parameters.AddMgmtLoggedInUser();
        _parameters.AddLanguageID();

        _parameters.Add(new FilterParameter("@DateFilterMode", (int)RiskSessionManager.RiskMgmtReportFilter.DateType, DbType.Int32));
        DateTime beginDate = RiskSessionManager.RiskMgmtReportFilter.FromDate;
        DateTime endDate = RiskSessionManager.RiskMgmtReportFilter.ToDate;
        GeneralFuncsLib.GetRealDateRange(RiskSessionManager.RiskMgmtReportFilter.DateType, ref beginDate, ref endDate);

        _parameters.Add(new FilterParameter("@BeginDate", beginDate, DbType.DateTime));
        _parameters.Add(new FilterParameter("@EndDate", endDate, DbType.DateTime));
    }

    private void BuildExportParameters()
    {
        BuildBaseParameters();
        _parameters.Add(new FilterParameter("@ViewMode", (int)ViewMode.Date, DbType.Int32));
        _parameters.Add(new FilterParameter("@AssignmentList", uxReportFilter.AssignmentFilterValue, DbType.AnsiString));
        _parameters.Add(new FilterParameter("@ParameterList", uxReportFilter.ParameterFilterValue, DbType.AnsiString));
    }
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

    }
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            OnPostBackActions(PostBackAction.DoFilterAction);
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        string spName = "spa_RM_MCF_Mgmt_GetAssignmentAlertSummary";
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindReportGrid:
                {
                    HierarchyGrid grid = (HierarchyGrid)sender;
                    BuildBaseParameters();
                    _parameters.Add(new FilterParameter("@ViewMode", (int)ViewMode.Date, DbType.Int32));
                    _parameters.Add(new FilterParameter("@AssignmentList", uxReportFilter.AssignmentFilterValue, DbType.AnsiString));
                    _parameters.Add(new FilterParameter("@ParameterList", uxReportFilter.ParameterFilterValue, DbType.AnsiString));

                    _parameters.AddPagingParameters(true, false, grid.MasterTableView.CurrentPageIndex + 1, grid.MasterTableView.PageSize, string.Empty, string.Empty);
                    grid.DataSource = WebServices.RiskServices.GetReports(spName, _parameters);

                    _parameters.ClearPagingParameters();
                    _parameters.AddPagingParameters(false, true, grid.MasterTableView.CurrentPageIndex + 1, grid.MasterTableView.PageSize, string.Empty, string.Empty);
                    DataTable totalRows = WebServices.RiskServices.GetReports(spName, _parameters);
                    grid.VirtualItemCount = totalRows == null || totalRows.Rows.Count == 0 ? 0 : int.Parse(totalRows.Rows[0][0].ToString());

                    uxExporter.Visible = grid.VirtualItemCount > 0;
                    litGridHeader.Text = GetLocalResourceObject("PageResource1.Title").ToString();
                    litGridSubTitle.Text = VeraCodeSolution.ValidateResponseData(uxReportFilter.HeaderText);
                }
                break;
            case DataBindAction.BindDetailGrid:
                {
                    GridTableView detailView = sender as GridTableView;
                    if (detailView.Name == "Parameters")
                    {
                        detailView.Columns.FindByDataField("ParameterIndicator").HeaderText = detailView.Columns.FindByDataField("ParameterIndicator").HeaderText.ToCurrencySymbol();
                    }

                    _parameters.AddPagingParameters(true, false, detailView.CurrentPageIndex + 1, detailView.PageSize, string.Empty, string.Empty);
                    detailView.DataSource = WebServices.RiskServices.GetReports(spName, _parameters);

                    _parameters.ClearPagingParameters();
                    _parameters.AddPagingParameters(false, true, detailView.CurrentPageIndex + 1, detailView.PageSize, string.Empty, string.Empty);
                    DataTable totalRows = WebServices.RiskServices.GetReports(spName, _parameters);
                    detailView.VirtualItemCount = totalRows == null || totalRows.Rows.Count == 0 ? 0 : int.Parse(totalRows.Rows[0][0].ToString());
                }
                break;
            default:
                break;
        }
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        var fileName = GeneralFuncsLib.FormatFileName(litGridHeader.Text + " - " + litGridSubTitle.Text);
        switch ((PostBackAction)type)
        {
            case PostBackAction.ExportExcel:
                this.IsNoCache = true;
                BuildExportParameters();
                RiskExportAs.DownloadExportedFile(litGridHeader.Text + " - " + litGridSubTitle.Text, fileName, WebSiteEnums.MgmtReportType.AssignmentAlertSummary.ToString(),
                    AS.Controls.UserControls.UxExport.ExportType.Excel.ToString().ToLower(), _parameters, true);
                break;
            case PostBackAction.ExportCSV:
                this.IsNoCache = true;
                BuildExportParameters();
                RiskExportAs.DownloadExportedFile(litGridHeader.Text + " - " + litGridSubTitle.Text, fileName, WebSiteEnums.MgmtReportType.AssignmentAlertSummary.ToString(),
                    AS.Controls.UserControls.UxExport.ExportType.CSV.ToString().ToLower(), _parameters, true);
                break;
            case PostBackAction.DoFilterAction:
                uxReportGrid.CurrentPageIndex = 0;
                uxReportGrid.Rebind();
                break;
        }
    }

    #region Export
    protected void ButtonExcel_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.ExportExcel);
    }
    protected void ButtonCSV_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.ExportCSV);
    }
    #endregion

    protected void DoGridNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        if (!e.IsFromDetailTable)
        {
            if (sender == uxReportGrid)
            {
                OnDataBindControls(DataBindAction.BindReportGrid, sender);
            }
        }
    }

    protected void DoDetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
    {
        GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;
        BuildBaseParameters();
        _parameters.Add(new FilterParameter("@AssignmentList", uxReportFilter.AssignmentFilterValue, DbType.AnsiString));
        _parameters.Add(new FilterParameter("@ParameterList", uxReportFilter.ParameterFilterValue, DbType.AnsiString));
        switch (e.DetailTableView.Name)
        {
            case "Assignments":
                {
                    DateTime reportDate = (DateTime)dataItem.GetDataKeyValue("ReportDate");
                    _parameters.Add(new FilterParameter("@ViewMode", (int)ViewMode.Assignment, DbType.Int32));
                    _parameters.Add(new FilterParameter("@ReportDate", reportDate, DbType.AnsiString));
                    OnDataBindControls(DataBindAction.BindDetailGrid, e.DetailTableView);
                    break;
                }
            case "Parameters":
                {
                    _parameters.Add(new FilterParameter("@ViewMode", (int)ViewMode.Parameter, DbType.Int32));
                    DateTime reportDate = (DateTime)dataItem.GetDataKeyValue("ReportDate");
                    int assignmentID = int.Parse(dataItem.GetDataKeyValue("AssignmentID").ToString());
                    _parameters.Add(new FilterParameter("@ReportDate", reportDate, DbType.DateTime));
                    _parameters.Add(new FilterParameter("@AssignmentID", assignmentID, DbType.Int32));
                    OnDataBindControls(DataBindAction.BindDetailGrid, e.DetailTableView);
                }
                break;
        }
    }

    protected void DoItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView dataRow = e.Item.DataItem as DataRowView;
            switch (e.Item.OwnerTableView.Name)
            {
                case "DateView":
                    dataItem["ReportDate"].Text = VeraCodeSolution.DoVeraCode(GetLocalResourceObject("rm_MgmtReport_AssignmentAlert_aspx_cs_Date").ToString() + " <b>" + GeneralFuncsLib.FormatDate(dataRow["ReportDate"]) + "</b>");
                    break;
                case "Parameters":
                    {
                        int parameterPrecision = int.Parse(GeneralFuncsLib.NvlString(dataRow["ParameterPrecision"]));
                        string indicatorFormat = "#,##0" + (parameterPrecision > 0 ? ".".PadRight(parameterPrecision + 1, '0') : string.Empty);

                        string parameterIndicator = GeneralFuncsLib.NvlString(dataRow["ParameterIndicator"]);
                        string dataType = GeneralFuncsLib.NvlString(dataRow["ParameterDataType"]).ToLower();

                        string parameterThreshold = GeneralFuncsLib.NvlString(dataRow["ParameterThreshold"]);
                        string thresholdType = GeneralFuncsLib.NvlString(dataRow["ThresholdType"]).ToLower();

                        string parameterThresholdHigh = GeneralFuncsLib.NvlString(dataRow["ParameterThresholdHigh"]);
                        string parameterThresholdType = GeneralFuncsLib.NvlString(dataRow["ParameterThresholdType"]);


                        if (!parameterIndicator.Equals("0") && !parameterIndicator.IsNullOrEmpty())
                        {
                            parameterIndicator = decimal.Parse(parameterIndicator).ToString(indicatorFormat);
                        }


                        if (!parameterThreshold.Equals("0") && !parameterThreshold.IsNullOrEmpty())
                        {
                            parameterThreshold = decimal.Parse(parameterThreshold).ToString(indicatorFormat);
                        }

                        if (!parameterThresholdHigh.Equals("0") && !parameterThresholdHigh.IsNullOrEmpty())
                        {
                            parameterThresholdHigh = decimal.Parse(parameterThresholdHigh).ToString(indicatorFormat);
                        }

                        //process indicator
                        dataItem["ParameterIndicator"].Text = VeraCodeSolution.ValidateResponseData(
                            GeneralFuncsLib.FormatParameterDataType(dataType, parameterIndicator, parameterPrecision));


                        if (!parameterIndicator.IsNullOrEmpty())
                        {
                            if (decimal.Parse(parameterIndicator) < 0)
                            {
                                dataItem["ParameterIndicator"].Style.Add("color", "Red");
                            }
                        }

                        switch (parameterThresholdType)
                        {
                            case "LowHigh":
                                dataItem["ParameterThreshold"].Text = VeraCodeSolution.ValidateResponseData(
                                    GeneralFuncsLib.FormatLowHighThreshold(parameterThreshold, parameterThresholdHigh, thresholdType));
                                break;
                            default:
                                dataItem["ParameterThreshold"].Text = VeraCodeSolution.ValidateResponseData(
                                    GeneralFuncsLib.FormatParameterDataType(thresholdType, parameterThreshold));
                                break;
                        }

                        if (!parameterThreshold.IsNullOrEmpty())
                        {
                            if (decimal.Parse(parameterThreshold) < 0)
                            {
                                dataItem["ParameterThreshold"].Style.Add("color", "Red");
                            }
                        }
                    }
                    break;
            }
        }
    }

    protected void OnSearchEvent(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.DoFilterAction);
    }

    protected void Assignments_PreRender(object sender, EventArgs e)
    {
        GridTableView detailView = sender as GridTableView;
        detailView.Columns.FindByDataField("ParameterIndicator").HeaderText = detailView.Columns.FindByDataField("ParameterIndicator").HeaderText.ToCurrencySymbol();
        detailView.Columns.FindByDataField("ParameterIndicator").HeaderTooltip = detailView.Columns.FindByDataField("ParameterIndicator").HeaderTooltip.ToCurrencySymbol();

    }

    protected void uxReportGrid_PreRender(object sender, EventArgs e)
    {
        foreach (GridTableView v in uxReportGrid.MasterTableView.DetailTables)
        {
            foreach (var item in v.DetailTables)
            {
                if (item.Columns.FindByDataFieldSafe("ParameterIndicator") != null)
                {
                    item.Columns.FindByDataField("ParameterIndicator").HeaderText = item.Columns.FindByDataField("ParameterIndicator").HeaderText.ToCurrencySymbol();
                    item.Columns.FindByDataField("ParameterIndicator").HeaderTooltip = item.Columns.FindByDataField("ParameterIndicator").HeaderTooltip.ToCurrencySymbol();
                }
            }

        }
    }
}
