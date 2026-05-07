using AS.Common.DBManager;
using AS.Controls.Exporter;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Controls.UserControls;
using AS.Core.Common.VeraCode;
using AS.Web.Business;
using AS.Web.UI.Controls;
using System;
using System.Data;
using Telerik.Web.UI;

[PagePermission("RiskWorkLog,MSRiskWorkLog")]
public partial class rm_MCF_RiskWorkLog : ReportPage
{
    enum DataBindAction
    {
        BindReportGrid,
    }

    enum PostBackAction
    {
        DoFilterAction
    }

    public string HeaderText
    {
        get
        {
            string result = GetDateFilterText();
            result = string.IsNullOrEmpty(result) ? string.Empty : string.Format("<span class='risk-work-log-gridSubTitle'>{0} {1}</span>", GetLocalResourceObject("ReportDateResource").ToString(), GetDateFilterText());
            return result;
        }
    }
    public string GetDateFilterExportText
    {
        get
        {
            return GetDateFilterText().Replace("(", "").Replace(")", "");
        }
    }

    private const string FILE_NAME = "RiskWorkLog_{0}";

    protected override void PageInitialize()
    {
        base.PageInitialize();
        IsBindDataOnLoad = true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ux_RiskWorkLogTitle.Visible = true;
            uxRiskWorkLog.Visible = false;
        }
    }

    #region --- Event Handles ----

    protected override void OnPostBackActions(Enum type, object sender)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.DoFilterAction:
                ux_RiskWorkLogTitle.Visible = false;
                uxRiskWorkLog.Visible = true;
                uxRiskWorkLog.CurrentPageIndex = 0;
                GetDataSourceGridWorkLog();
                uxExporter.GridTitle = GetLocalResourceObject("uxPageTitleResource.Title").ToString();
                uxExporter.GridSubTitle = VeraCodeSolution.DoVeraCode(HeaderText);
                uxRiskWorkLog.Rebind();
                break;
        }
    }

    #endregion --- Event Handles ----

    protected void uxReportFilter_SubmitFiltering(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.DoFilterAction, sender);
    }

    protected void GetDataSourceGridWorkLog()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters = BuilParameterForReport();

        uxRiskWorkLog.DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices,
     WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { "spa_RM_MCF_GetRiskWorkLog", ReportServices.ConvertToFilterParamWSArray(parameters) });
    }

    private FilterParameterCollection BuilParameterForReport()
    {

        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLanguageID();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        HierarchyFilterValue reportFilter = uxReportFilter.ReportFilterValue ?? new HierarchyFilterValue();
        DateTime beginDate = reportFilter.DateOptionValue.From;
        DateTime endDate = reportFilter.DateOptionValue.To;
        GeneralFuncsLib.GetRealDateRange(reportFilter.DateOption, ref beginDate, ref endDate);
        parameters.Add(new FilterParameter("@BeginDate", beginDate, DbType.DateTime));
        parameters.Add(new FilterParameter("@EndDate", endDate, DbType.DateTime));
        parameters.Add(new FilterParameter("@MerchantNumberList", uxReportFilter.MerchantIDFilterValue, DbType.String));
        parameters.Add(new FilterParameter("@UserList", uxReportFilter.UserFilterValue, DbType.String));
        parameters.Add(new FilterParameter("@AssignmentList", uxReportFilter.AssignmentListFilterValue, DbType.String));
        parameters.Add(new FilterParameter("@ViewList", uxReportFilter.DQViewFilterValue, DbType.String));
        if (uxRiskWorkLog.MasterTableView.SortExpressions.Count == 0)
        {
            parameters.Add(new FilterParameter("@stOrder", "CreatedDTS ASC", DbType.String));
        }

        return parameters;
    }

    protected override void DoNeedExportConfig(UxExport sender, ExportConfig exportConfig)
    {
        if (IsIntruderDetected)
            return;
        UxExport uxExport = sender as UxExport;
        AS.Controls.Grid.ASGrid grid = uxExport.Grid as AS.Controls.Grid.ASGrid;

        base.DoNeedExportConfig(sender, exportConfig);
        string header = System.Environment.NewLine;
        header = ((UxExport)sender).GridTitle.ToString() + System.Environment.NewLine + string.Format("{0} {1}", GetLocalResourceObject("ReportDateResource").ToString(), GetDateFilterText());
        exportConfig.FileName = GeneralFuncsLib.FormatFileName(Server.HtmlDecode(string.Format(FILE_NAME, GetDateFilterExportText)));
        exportConfig.ReportHeader = Server.HtmlDecode(header);
    }

    protected void uxRiskWorkLog_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        GetDataSourceGridWorkLog();
    }

    private string GetDateFilterText()
    {
        HierarchyFilterValue reportFilter = uxReportFilter.ReportFilterValue;
        if (reportFilter != null)
        {
            DateTime beginDate = reportFilter.DateOptionValue.From;
            DateTime endDate = reportFilter.DateOptionValue.To;
            GeneralFuncsLib.GetRealDateRange(reportFilter.DateOption, ref beginDate, ref endDate);
            string dateText = (reportFilter.DateOption == DateOptionMode.Daily ? beginDate.ToGenericDateString() : string.Format("{0} - {1}", beginDate.ToGenericDateString(), endDate.ToGenericDateString()));

            return string.Format("({0})", dateText);
        }

        return string.Empty;
    }
    protected void uxRiskWorkLog_Init(object sender, EventArgs e)
    {
        this.uxRiskWorkLog.PageSizeList = new int[] { 20, 40, 50, 100 };
        this.uxRiskWorkLog.PageSize = 20;
    }
}