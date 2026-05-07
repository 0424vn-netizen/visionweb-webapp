using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AS.Controls.Pages;
using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Web.Business;
using AS.Web.UI.Controls;
using System.Data;
using AS.Common;


[PagePermission("ManCusChain")]
public partial class TransactionReport : ReportPage
{
    enum DataBindAction
    {
        BindReportGrid,
    }

    enum PostBackAction
    {
        SearchMessage,
    }

    private string MerchantName
    {
        get
        {
            if (ViewState["MerchantName"] != null)
                return ViewState["MerchantName"].ToString();
            else
                return string.Empty;
        }
        set
        {
            ViewState["MerchantName"] = value;
        }
    }
    
    protected override void PageInitialize()
    {
        this.ExporterIDs.Add("uxExportBottom");
        base.PageInitialize();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
        
        IsBindDataOnLoad = true;

        if (!IsPostBack)
        {
            this.uxDate.SelectedDate = DateTime.Now;
            MerchantName = GeneralFuncsLib.GetMerchantName(SessionManager.CurrentUser.UserID);
        }

    }

    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, AS.Controls.Exporter.ExportConfig exportConfig)
    {
        base.DoNeedExportConfig(sender, exportConfig);
        exportConfig.FileName = GetLocalResourceObject("TransactionReport_aspx_cs_ExportFilename").ToString();
        exportConfig.ReportHeader = GetLocalResourceObject("TransactionReport_aspx_cs_GridHeaderExport").ToString();
    }

    protected override void DoGridNeedDataSource(AS.Controls.Grid.ASGrid sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.BindReportGrid, sender);
    }

    protected override void DoItemDataBound(AS.Controls.Grid.ASGrid sender, Telerik.Web.UI.GridItemEventArgs e)
    {

    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindReportGrid:
                {
                    FilterParameterCollection parames = new FilterParameterCollection();
                    parames.AddLoggedInUserReportingParams();
                    parames.AddLoggedInUserPrimaryUserID();

                    DateOptionMode dateOption = DateOptionMode.None;
                    if (this.uxDaily.Checked)
                        dateOption = DateOptionMode.Daily;
                    else if (this.uxMonthly.Checked)
                        dateOption = DateOptionMode.Monthly;

                    parames.Add(new FilterParameter("@DateFilterMode", (int)dateOption, DbType.Int32));
                    parames.Add(new FilterParameter("@BeginDate", uxDate.SelectedDate, DbType.DateTime));
                    parames.Add(new FilterParameter("@EndDate", uxDate.SelectedDate, DbType.DateTime));
                    ((ASGrid)sender).DataSourceInvoker = new ASFuncInvoker(WebServices.MsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { "spa_ms_GetCustomChainReport", ReportServices.ConvertToFilterParamWSArray(parames) });
                    DateTime dtSelected = (DateTime)(uxDate.SelectedDate);
                    string title = GetLocalResourceObject("TransactionReport_aspx_cs_TransactionReport").ToString() + " - " + SessionManager.CurrentUser.UserID.ToString() + " : " + MerchantName + " (" + String.Format("{0:MM/dd/yyyy}", dtSelected) + ")";
                    uxExporter.GridHeader = VeraCodeSolution.ValidateResponseData(title);
                }
                break;
        }
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        if (this.IsIntruderDetected) return;
        switch ((PostBackAction)type)
        {
            case PostBackAction.SearchMessage:
                uxReportGrid.Rebind();
                break;
        }
    }

    protected void uxSearch_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.SearchMessage);
    }
}
