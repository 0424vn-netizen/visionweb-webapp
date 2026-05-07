using AS.Common.DBManager;
using AS.Controls.Exporter;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Controls.UserControls;
using AS.Web.Business;
using System;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

[PagePermission("RskRP,MSRskRP")]
public partial class rm_MCF_RiskNotesModal_MCPS : ReportPage
{

    enum DataBindAction
    {
        BindRiskNotes

    }

    private string MerchantNumber
    {
        get
        {
            return GeneralFuncsLib.NvlString(this.SecureQueryString["MerchantNumber"].ToString());
        }
    }

    #region Base overrides

    protected override void PageInitialize()
    {
        if (IsIntruderDetected) return;
        this.ExporterIDs.Add("uxExporterBottom");
        base.PageInitialize();
    }

    #endregion


    #region Grid events
 
    protected override void DoGridNeedDataSource(ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        if (sender == uxReportGrid)
        {
            OnDataBindControls(DataBindAction.BindRiskNotes, sender);
        }
    }

    #endregion


    protected override void DoNeedExportConfig(UxExport sender, ExportConfig exportConfig)
    {
        if (sender == uxExporter)
        {
            base.DoNeedExportConfig(sender, exportConfig);
            exportConfig.FileName = GeneralFuncsLib.GetLegalFileName(uxReportGrid.GridName);
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        if (this.IsIntruderDetected) return;
        FilterParameterCollection parameters = new FilterParameterCollection();

        switch ((DataBindAction)type)
        {
            case DataBindAction.BindRiskNotes:
                {
                    ASGrid grid = (ASGrid)sender;
                    //uxExporter.GridTitle = "MCPS Risk Notes - Merchant ID: ";
                    uxExporter.GridSubTitle = GetLocalResourceObject("rm_RiskNotesModal_MCPS_aspx_cs_String1").ToString() + " " + MerchantNumber;
                    grid.GridName = uxExporter.GridHeader = GetLocalResourceObject("rm_RiskNotesModal_MCPS_aspx_cs_String2").ToString() + " -" + uxExporter.GridSubTitle;

                    parameters.AddLoggedInUserReportingParams(false);
                    parameters.Add(new FilterParameter("@HierarchyFilterValue", this.MerchantNumber, DbType.String));

                    grid.DataSourceInvoker = new ASFuncInvoker(WebServices.RiskServices, "GetReports", new object[] { "spa_RM_MCF_GetRiskNotes", ReportServices.ConvertToFilterParamWSArray(parameters) });

                }
                break;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        IsBindDataOnLoad = true;
        if (!IsPostBack)
        {
            OnDataBindControls(DataBindAction.BindRiskNotes, uxReportGrid);
        }
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }
    public static string IsIEBrowser
    {
        get
        {
            return GeneralFuncsLib.GetIEBrowserMode();
        }
    }
}
