using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Telerik.Web.UI;
using AS.Controls.Exporter;
using AS.Utilities;

public partial class UserControls_RiskAssignmentListSimple : GlobalUserControl
{
    public event GridNeedDataSourceEventHandler NeedDataSource;
    public event GridItemEventHandler ItemDataBound;

    private const string ALERT = "AlertMerchantCount";
    private const string TOTALMERCHANT = "TotalMerchantCount";
    private const string WORKED_COUNT = "WorkedMerchantCount";
    private const string WORKED_VOLUME = "WorkedMerchantAmount";

    protected void uxExport_NeedExportConfig(object sender, ExportConfig exportConfig)
    {
        uxReportGrid.Columns.FindByUniqueName("TotalMerchantCount").HeaderText = GetLocalResourceObject("RiskAssignmentListSimple_ascx_cs_MerchantCountTotalMerch").ToString();
        uxReportGrid.Columns.FindByUniqueName("AlertMerchantCount").HeaderText = GetLocalResourceObject("RiskAssignmentListSimple_ascx_cs_MerchantCountAlert").ToString();

        uxReportGrid.Columns.FindByUniqueName("WorkedMerchantCount").HeaderText = GetLocalResourceObject("RiskAssignmentListSimple_ascx_cs_WorkedNumber").ToString();
        uxReportGrid.Columns.FindByUniqueName("WorkedMerchantAmount").HeaderText = GetLocalResourceObject("RiskAssignmentListSimple_ascx_cs_WorkedVolume").ToString();
        uxReportGrid.Columns.FindByUniqueName("CompletedPercent").Visible = true;
        uxReportGrid.Columns.FindByUniqueName("CompleteTemplate").Visible = false;

        System.Collections.Generic.Dictionary<string, int> alignmenter = new System.Collections.Generic.Dictionary<string, int>();
        alignmenter.Add("CompletedPercent", 1);
        uxExporter.Alignmenter = uxExporterBottom.Alignmenter = alignmenter;

        exportConfig.FileName = GeneralFuncsLib.GetLegalFileName(GetLocalResourceObject("RiskAssignmentListSimple_ascx_cs_ActiveAssignments").ToString());
        exportConfig.ReportHeader = GetLocalResourceObject("RiskAssignmentListSimple_ascx_cs_ActiveAssgnmentsHeader").ToString();
    }

    protected void uxGrid_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        NeedDataSource.Fire(d => (d as GridNeedDataSourceEventHandler)(this, e));

    }

    protected void uxGrid_ItemDataBound(object source, GridItemEventArgs e)
    {
        ItemDataBound.Fire(d => (d as GridItemEventHandler)(this, e));
    }


    public object DataSource
    {
        get { return uxReportGrid.DataSource; }
        set { uxReportGrid.DataSource = value; }
    }
}
