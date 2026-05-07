using System;
using Telerik.Web.UI;
using AS.Controls.Exporter;
using AS.Controls.UserControls;
using AS.Controls.Grid;
using AS.Common.DBManager;
using System.Data;
using System.Web.UI;
using AS.Controls.Pages;
using AS.Common;
using AS.Common.Formater;

[PagePermission("StatementRpt,MSStatementRpt")]
public partial class StatementDetails_Chain : ReportPage
{
    enum BindDataAction
    {
        BindReportDate,
        BindGrid
    }
    enum PostbackAction
    {
        ShowStatement
    }

    protected override void PageInitialize()
    {
        if (IsIntruderDetected) return;

        this.ExporterIDs.Add("uxExportTop");
        this.ExporterIDs.Add("uxExportBot");
        base.PageInitialize();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;
        if (!IsSecureQueryString)
        {
            IsIntruderDetected = true;
            return;
        }
        if (!IsPostBack)
        {
            OnDataBindControls(BindDataAction.BindReportDate, sender);
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }

    protected void uxShowStatement_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostbackAction.ShowStatement, sender);
    }
    protected override void OnDataBindControls(Enum type, object sender)
    {
        base.OnDataBindControls(type, sender);
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();
        parameters.Add(new FilterParameter("@ChainNumber", this.SecureQueryString["Chain"], DbType.AnsiString));
        switch ((BindDataAction)type)
        {
            case BindDataAction.BindGrid:
                parameters.Add(new FilterParameter("@ReportDate", this.uxReportDate.SelectedValue, DbType.Date));
                DataTable dt = WebServices.CsReportServices.GetReports("spa_MonthEndChainStatementSummary", parameters);
                this.uxReportGrid.DataSource = dt;
                int activeMerchantCount = (dt.Rows.Count > 0) ? int.Parse(dt.Rows[0]["ActiveMerchantCount"].ToString()) : 0;

                string gridHeader = string.Format(GetLocalResourceObject("StatementDetails_Chain_aspx_cs_StatementSummaryActiveMetchant").ToString(), this.SecureQueryString["Chain"], activeMerchantCount);
                this.uxExportTop.GridHeader = this.uxExportBot.GridHeader = VeraCodeSolution.DoVeraCode(gridHeader);
                break;
            case BindDataAction.BindReportDate:
                this.uxContainerDates.HeaderText = string.Format(this.uxContainerDates.HeaderText, this.SecureQueryString["Chain"]);
                this.uxReportDate.DataTextField = "ReportDate";
                this.uxReportDate.DataValueField = "ReportDate";
                this.uxReportDate.DataTextFormatString = GetLocalResourceObject("StatementDetails_Chain_aspx_cs_STMT").ToString() + " - {0: MM/dd/yyyy}";
                this.uxReportDate.DataSource = WebServices.CsReportServices.GetReports("spa_MonthEndChainStatementSummary", parameters);
                this.uxReportDate.DataBind();
                break;
        }
    }
    protected override void OnPostBackActions(Enum type, object sender)
    {
        base.OnPostBackActions(type, sender);
        switch ((PostbackAction)type)
        {
            case PostbackAction.ShowStatement:
                this.uxReportGrid.Visible = true;
                this.uxReportGrid.Rebind();
                break;
        }
    }
    protected override void DoGridNeedDataSource(ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        if (IsIntruderDetected) return;
        base.DoGridNeedDataSource(sender, e);
        OnDataBindControls(BindDataAction.BindGrid, sender);
    }
    protected override void DoItemDataBound(ASGrid sender, GridItemEventArgs e)
    {
        base.DoItemDataBound(sender, e);
        if (e.Item is GridDataItem)
        {
            GridDataItem item = e.Item as GridDataItem;
            DataRowView dv = e.Item.DataItem as DataRowView;
            DateTime selectedDate = Convert.ToDateTime(this.uxReportDate.SelectedValue);
            string qString = BuildSecureQueryString(string.Format("MerchantNumber={0}&ReportDate={1}&index=1", dv["MerchantNumber"], selectedDate.Date.Ticks));
            item["MerchantNumber"].Text = VeraCodeSolution.DoVeraCode(string.Format("<a href='#' onclick=\"return parent.ShowPopupModalChild(1, 'StatementDetail_ORION.aspx?{0}','auto')\">{1}</a>", qString, dv["MerchantNumber"]));
        }
    }
    protected override void DoNeedExportConfig(UxExport sender, ExportConfig exportConfig)
    {
        base.DoNeedExportConfig(sender, exportConfig);
        string gridHeader = string.Format(GetLocalResourceObject("StatementDetails_Chain_aspx_cs_StatementSummary").ToString(), this.SecureQueryString["Chain"], Convert.ToDateTime(this.uxReportDate.SelectedValue).ToGenericDateString());
        exportConfig.ReportHeader = VeraCodeSolution.DoVeraCode(gridHeader);
        exportConfig.FileName = VeraCodeSolution.DoVeraCode(gridHeader);
    }
}
