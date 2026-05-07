using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Telerik.Web.UI;
using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Pages;
using AS.Controls.Grid;
using AS.Web.Business;

[PagePermission("ClosedMerchRpt")]
public partial class ClosedMerchantReport : ReportPage
{
    #region Enum

    enum DataBindAction
    {
        BindGrid,
        InitFiltering
    }

    enum PostBackAction
    {
        GoMerchantProfile,
    }

    #endregion

    #region Const

    string REPORT_HEADER = string.Empty;
    const string MERCHANT_NUMBER = "MerchantNumber";
    const string MERCHANT_NAME = "MerchantName";

    #endregion

    #region Properties

    private string _GridHeader
    {
        get
        {
            return REPORT_HEADER + GeneralFuncsLib.GetFullGridTitleName(ReportFilter) + " " + GeneralFuncsLib.GetDateFilterText(ReportFilter);
        }
    }

    private string _DateFilterText
    {
        get
        {
            string dateText = string.Empty;
            string beginDateText = string.Empty;
            string endDateText = string.Empty;
            switch (ReportFilter.CurrentValue.DateOption)
            {
                case AS.Web.UI.Controls.DateOptionMode.Daily:
                    {
                        beginDateText = ReportFilter.CurrentValue.DateOptionValue.From.ToGenericDateString();
                        dateText = string.Format("{0}", beginDateText);
                    }
                    break;
                case AS.Web.UI.Controls.DateOptionMode.Monthly:
                    {
                        beginDateText = ReportFilter.CurrentValue.DateOptionValue.From.GetFirstDayOfMonth().ToGenericDateString();
                        if (ReportFilter.CurrentValue.DateOptionValue.From.Year == DateTime.Today.Year && ReportFilter.CurrentValue.DateOptionValue.From.Month == DateTime.Today.Month)
                            endDateText = DateTime.Today.ToGenericDateString();
                        else
                            endDateText = ReportFilter.CurrentValue.DateOptionValue.From.GetLastDayOfMonth().ToGenericDateString();
                    }
                    dateText = string.Format("{0} - {1}", beginDateText, endDateText);
                    break;
                case AS.Web.UI.Controls.DateOptionMode.DateRange:
                    {
                        beginDateText = ReportFilter.CurrentValue.DateOptionValue.From.ToGenericDateString();
                        endDateText = ReportFilter.CurrentValue.DateOptionValue.To.ToGenericDateString();
                    }
                    dateText = string.Format("{0} - {1}", beginDateText, endDateText);
                    break;
            }

            return dateText;
        }
    }

    #endregion

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        this.uxExporterTop.GridHeader = VeraCodeSolution.ValidateResponseData(_GridHeader);
    }

    protected override void PageInitialize()
    {
        if (IsIntruderDetected) return;
        this.GridIDs.Add("uxMerchantGrid");
        this.ExporterIDs.Add("uxExporterTop");
        this.ExporterIDs.Add("uxExporterBottom");
        base.PageInitialize();
    }

    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, AS.Controls.Exporter.ExportConfig exportConfig)
    {
        base.DoNeedExportConfig(sender, exportConfig);
        exportConfig.FileName = GeneralFuncsLib.FormatFileName(uxExporterTop.GridHeader);
        exportConfig.ReportHeader = uxExporterTop.GridHeader;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        REPORT_HEADER = GetLocalResourceObject("ClosedMerchantReport_aspx_cs_ClosedMerchantList").ToString() + " ";
        IsBindDataOnLoad = true;
    }

    protected void uxProcess_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.GoMerchantProfile);
    }

    protected override void DoGridNeedDataSource(ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        if (sender == uxMerchantGrid)
        {
            OnDataBindControls(DataBindAction.BindGrid, sender);
        }
    }

    protected override void DoItemDataBound(ASGrid sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView dataRow = e.Item.DataItem as DataRowView;

            string url = string.Format("<a class=\"link\" href=\"#\" style=\"cursor:pointer\" onclick=\"return Merchant_Click('{0}');\">{0}</a>", dataRow[MERCHANT_NUMBER]);
            dataItem[MERCHANT_NUMBER].Text = VeraCodeSolution.DoVeraCode(url);
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindGrid:
                {
                    string spName = "spa_cs_GetClosedMerchantReport";
                    FilterParameterCollection parames = new FilterParameterCollection();
                    parames.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parames.AddHierarchyFilterParams((ReportPage)this.Page);
                    ASGrid grid = (ASGrid)sender;
                    grid.DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spName, ReportServices.ConvertToFilterParamWSArray(parames) });
                }
                break;
            case DataBindAction.InitFiltering:
                {
                    ReportFilter.CurrentValue.DateOption = AS.Web.UI.Controls.DateOptionMode.DateRange;
                    ReportFilter.DateOptionMonthlyVisible = false;
                    ReportFilter.DateOptionDailyVisible = false;
                }

                break;
        }
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.GoMerchantProfile:
                string queryString = this.BuildSecureQueryString(string.Format("{0}={1}", MERCHANT_NUMBER, uxProcessData.Value));
                string url = string.Format("MerchantProfile.aspx?{0}", queryString);
                Response.Redirect(url);
                break;
        }
    }

    private string GetActivity(bool isMerchant)
    {
        string hierachyMode = ReportFilter.CurrentValue.HierarchyMode;
        string hierachyValue = ReportFilter.CurrentValue.Value;
        string value = string.Empty;

        string hierarchyModeValue = string.Empty;
        foreach (DataRow row in SessionManager.HierarchyFilterDrillDown.Rows)
        {
            if (row["CurrentHierarchyMode"].ToString() == hierachyMode)
            {

                hierarchyModeValue = row["CurrentHierarchyGridName"].ToString();
            }
        }
        if (isMerchant)
        {
            value = "View " + hierachyValue;
        }
        else
        {
            if (string.IsNullOrEmpty(hierachyValue))
            {
                if (GeneralFuncsLib.IsMerchantMode(hierachyMode))
                    value = GetLocalResourceObject("ClosedMerchantReport_aspx_cs_SearchAllMerchants").ToString();
                else
                    value = "Search all " + hierarchyModeValue + "s";//_how?
            }
            else
            {
                value = string.Format(GetLocalResourceObject("ClosedMerchantReport_aspx_cs_SearchBy").ToString(), hierarchyModeValue, hierachyValue);
            }
        }

        string dateText = string.Empty;
        string beginDateText = string.Empty;
        string endDateText = string.Empty;

        switch (ReportFilter.CurrentValue.DateOption)
        {
            case AS.Web.UI.Controls.DateOptionMode.Daily:
                {
                    beginDateText = ReportFilter.CurrentValue.DateOptionValue.From.ToGenericDateString();
                    dateText = string.Format(GetLocalResourceObject("ClosedMerchantReport_aspx_cs_On").ToString(), beginDateText);
                }
                break;
            case AS.Web.UI.Controls.DateOptionMode.Monthly:
                {
                    beginDateText = ReportFilter.CurrentValue.DateOptionValue.From.GetFirstDayOfMonth().ToGenericDateString();
                    if (ReportFilter.CurrentValue.DateOptionValue.From.Year == DateTime.Today.Year && ReportFilter.CurrentValue.DateOptionValue.From.Month == DateTime.Today.Month)
                        endDateText = DateTime.Today.ToGenericDateString();
                    else
                        endDateText = ReportFilter.CurrentValue.DateOptionValue.From.GetLastDayOfMonth().ToGenericDateString();
                }
                dateText = string.Format(GetLocalResourceObject("ClosedMerchantReport_aspx_cs_FromTo").ToString(), beginDateText, endDateText);
                break;
            case AS.Web.UI.Controls.DateOptionMode.DateRange:
                {
                    beginDateText = ReportFilter.CurrentValue.DateOptionValue.From.ToGenericDateString();
                    endDateText = ReportFilter.CurrentValue.DateOptionValue.To.ToGenericDateString();
                }
                dateText = string.Format(GetLocalResourceObject("ClosedMerchantReport_aspx_cs_FromTo").ToString(), beginDateText, endDateText);
                break;
        }

        return string.Format(GetLocalResourceObject("ClosedMerchantReport_aspx_cs_ClosedMerchantReport").ToString(), value, dateText);

    }
    protected override void DoReportFilterAction(AS.Web.UI.Controls.ReportFilterEventArgs e)
    {
        if (e.ActionType == AS.Web.UI.Controls.ReportFilterEventType.Submit)
        {
            string merchantName = GeneralFuncsLib.GetMerchantName(e.HierachyValue.Value);
            string activity = GetActivity(!string.IsNullOrEmpty(merchantName));
            string merchantNumber = string.IsNullOrEmpty(merchantName) ? "" : e.HierachyValue.Value;
            GeneralFuncsLib.SaveUserActivity(merchantNumber, activity);
        }
        base.DoReportFilterAction(e);
    }
}
