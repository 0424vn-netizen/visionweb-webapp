using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Web.Business;
using AS.Web.UI.Controls;
using System;
using System.Data;
using Telerik.Web.UI;

[PagePermission("ASServiceTest")]
public partial class ServiceTest : ReportPage
{
    #region Enum

    enum DataBindAction
    {
        BindReportGrid,
        BindHeaderText,
    }

    #endregion

    #region Const

    string REPORT_HEADER = string.Empty;
    string DEFAULT_EMPTY = "_";
    #endregion

    #region Properties

    private string _Target = "_parent";

    private string _GridTitle
    {
        get
        {
            var gridTitleName = !string.IsNullOrEmpty(SessionManager.CurrentReportFilter.Value) ? SessionManager.CurrentReportFilter.Value + ": " + GeneralFuncsLib.GetMerchantName(SessionManager.CurrentReportFilter.Value) : string.Empty;
            return gridTitleName + " " + this.GetDateFilterText();
        }
    }

    private string GetDateFilterText()
    {
        string dateText = string.Empty;
        string beginDateText = string.Empty;
        string endDateText = string.Empty;
        switch (SessionManager.CurrentReportFilter.DateOption)
        {
            case AS.Web.UI.Controls.DateOptionMode.Daily:
                {
                    beginDateText = SessionManager.CurrentReportFilter.DateOptionValue.From.ToGenericDateString();
                    dateText = string.Format("{0}", beginDateText);
                }
                break;
            case AS.Web.UI.Controls.DateOptionMode.Monthly:
                {
                    beginDateText = SessionManager.CurrentReportFilter.DateOptionValue.From.GetFirstDayOfMonth().ToGenericDateString();
                    if (SessionManager.CurrentReportFilter.DateOptionValue.From.Year == DateTime.Today.Year && SessionManager.CurrentReportFilter.DateOptionValue.From.Month == DateTime.Today.Month)
                        endDateText = DateTime.Today.ToGenericDateString();
                    else
                        endDateText = SessionManager.CurrentReportFilter.DateOptionValue.From.GetLastDayOfMonth().ToGenericDateString();
                }
                dateText = string.Format("{0} - {1}", beginDateText, endDateText);
                break;
            case AS.Web.UI.Controls.DateOptionMode.DateRange:
                {
                    beginDateText = SessionManager.CurrentReportFilter.DateOptionValue.From.ToGenericDateString();
                    endDateText = SessionManager.CurrentReportFilter.DateOptionValue.To.ToGenericDateString();
                }
                dateText = string.Format("{0} - {1}", beginDateText, endDateText);
                break;
        }

        return string.Format("({0})", dateText);
    }

    #endregion

    protected override void PageInitialize()
    {
        if (IsIntruderDetected) return;
        base.PageInitialize();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        REPORT_HEADER = GetLocalResourceObject("ServiceTest_aspx_cs_Text_ServiceTest").ToString() + " ";
        IsBindDataOnLoad = true;
        if (SessionManager.ClientFrameInfo != string.Empty) _Target = SessionManager.ClientFrameInfo;
        if(SessionManager.CurrentReportFilter != null && !GeneralFuncsLib.IsMerchantMode(SessionManager.CurrentReportFilter.HierarchyMode)){
            SessionManager.CurrentReportFilter.Value = string.Empty;
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        OnDataBindControls(DataBindAction.BindHeaderText);
    }

    bool _isExporting = false;
    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, AS.Controls.Exporter.ExportConfig exportConfig)
    {
        base.DoNeedExportConfig(sender, exportConfig);
        _isExporting = true;
        string fileName = REPORT_HEADER + _GridTitle;
        exportConfig.FileName = GeneralFuncsLib.FormatFileName(fileName);
        exportConfig.ReportHeader = fileName;
    }

    protected override void DoGridNeedDataSource(ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.BindReportGrid, sender);
    }

    protected override void DoItemDataBound(ASGrid sender, GridItemEventArgs e)
    {
        if (sender == uxReportGrid && sender.Visible)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = e.Item as GridDataItem;
                DataRowView dataRow = e.Item.DataItem as DataRowView;
                if (sender == uxReportGrid)
                {
                    dataItem["CardType"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["CardDescription"].ToString());
                    dataItem["AVS"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["AVSDescription"].ToString());
                    dataItem["CVV"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["CVVDescription"].ToString());
                    dataItem["AuthSource"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["AuthorizationSourceDescription"].ToString());
                    dataItem["CustID"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["CustIDDescription"].ToString());
                    dataItem["MOTO"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["MOTODescription"].ToString());
                    dataItem["TransCode"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["TransactionCode"].ToString());
                    dataItem["Approved"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["ApprovedDescription"].ToString());
                    dataItem["ResponseCode"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["ResponseCodeDescription"].ToString());
                    dataItem["KeyedEntry"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["EntryModeDescription"].ToString());
                    dataItem["ExpirationDate"].ToolTip = "MM/YY";

                    if (dataRow["AccountNumber"].ToString().IsNullOrEmpty())
                        dataItem["PartialCardNumber"].Text = DEFAULT_EMPTY;
                    
                    if (dataRow["Approved"].ToString() == "D")
                    {
                        dataItem["Approved"].Text = string.Format(WebSiteConstants.DECLINED_TEXT, dataItem["Approved"].Text);                        
                    }
                }
            }
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindReportGrid:
                {
                    string spaName = "spa_GetAuthorizationDetail";
                    FilterParameterCollection parames = new FilterParameterCollection();
                    parames.AddLoggedInUserReportingParams();
                    parames.AddLoggedInUserPrimaryUserID();
                    parames.AddLanguageID();

                    HierarchyFilterValue reportFilter = SessionManager.CurrentReportFilter != null ? SessionManager.CurrentReportFilter : new HierarchyFilterValue();
                    parames.Add(new FilterParameter("@DateFilterMode", (int)reportFilter.DateOption, DbType.Int32));
                    parames.Add(new FilterParameter("@BeginDate", reportFilter.DateOptionValue.From, DbType.Date));
                    DateTime endDate = reportFilter.DateOption == DateOptionMode.DateRange ? reportFilter.DateOptionValue.To : reportFilter.DateOptionValue.From;
                    parames.Add(new FilterParameter("@EndDate", endDate, DbType.Date));
                    parames.Add(new FilterParameter("@HierarchyFilterMode", SessionManager.CurrentReportFilter.HierarchyMode, DbType.AnsiString));
                    parames.Add(new FilterParameter("@HierarchyFilterValue", reportFilter.Value, DbType.AnsiString));
                    parames.AddDecryptDataParams("AccountNumber", _isExporting);

                    ((ASGrid)sender).DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(parames) });
                }
                break;

            case DataBindAction.BindHeaderText:
                {
                    uxExporter.GridTitle = VeraCodeSolution.ValidateResponseData(REPORT_HEADER);
                    uxExporter.GridSubTitle = VeraCodeSolution.ValidateResponseData(_GridTitle);
                }
                break;
        }
    }

    protected void uxReportFilter_Search(object sender)
    {
        uxReportGrid.Rebind();
    }
}
