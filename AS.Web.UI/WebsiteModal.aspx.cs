using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Web.Business;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using Telerik.Web.UI;
using System.Linq;
using System.Web.UI.WebControls;
using AS.Controls.UserControls;
using AS.Controls.Exporter;
using System.Collections.Generic;

public partial class WebsiteModal : ReportPage
{
    enum DataBindAction
    {
        BindReportGrid
    }
    DateTime _ReportDate = DateTime.Today;
    //private DataTable dtReport = null;


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
    }

    protected override void PageInitialize()
    {
        this.ExporterIDs.Add("uxExporterTop");
        this.GridIDs.Add("uxReportGrid");
        this.IsBindDataOnLoad = true;
        base.PageInitialize();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }

    protected override void DoGridNeedDataSource(AS.Controls.Grid.ASGrid sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if (uxReportGrid.Visible && sender == uxReportGrid)
        {
            OnDataBindControls(DataBindAction.BindReportGrid, sender);
        }

    }
    bool _isExporting = false;
    protected override void OnDataBindControls(Enum type, object sender)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();
        string spaName = string.Empty;
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindReportGrid:
                spaName = "spa_cs_AW_GetAllBusinessWebsites";
                parameters.Add(new FilterParameter("@MerchantNumber", SessionManager.CurrentMerchantNumber, DbType.AnsiString));
                (sender as ASGrid).DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(parameters) });
                break;
        }

    }
    protected override void DoItemDataBound(ASGrid sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView dataRow = e.Item.DataItem as DataRowView;
            dataItem["WebsiteURL"].ToolTip = VeraCodeSolution.ValidateResponseData(dataRow["WebsiteURL"].ToString());

        }
    }

    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, AS.Controls.Exporter.ExportConfig exportConfig)
    {
        uxExporterTop.Formatter = new Dictionary<string, Func<object, string>>();
        foreach (GridColumn col in uxReportGrid.MasterTableView.Columns)
        {
            if (col.UniqueName.Equals("PhoneNumber"))
            {
                uxExporterTop.Formatter.Add(col.UniqueName, new Func<object, string>(MyConvertEm_DashForPhoneNumber));
            }
            else if (col.UniqueName.Equals("ActivationDate"))
            {
                uxExporterTop.Formatter.Add(col.UniqueName, new Func<object, string>(MyConvertEm_DashForDateTime));
            }
            else
            {
                uxExporterTop.Formatter.Add(col.UniqueName, new Func<object, string>(MyConvertEm_Dash));
            }
        }
        base.DoNeedExportConfig(sender, exportConfig);
        string title = Page.Title.Trim();
        exportConfig.ReportHeader = title;
        exportConfig.FileName =
           string.Format("AW_{0}_{1}_{2}", SessionManager.CurrentMerchantNumber, title.Replace(" ", ""), _ReportDate.ToString("MM/dd/yyyy").Replace('/', '_'));
    }

    private string MyConvertEm_Dash(object obj)
    {

        if (obj != null && !string.IsNullOrEmpty(obj.ToString()))

            return obj.ToString();

        else

            return WebSiteConstants.HTML_EM_DASH_ENCODE;

    }

    private string MyConvertEm_DashForPhoneNumber(object obj)
    {

        if (obj != null && !string.IsNullOrEmpty(obj.ToString()))

            return obj.FormatPhone();

        else

            return WebSiteConstants.HTML_EM_DASH_ENCODE;

    }

    private string MyConvertEm_DashForDateTime(object obj)
    {

        if (obj != null && !string.IsNullOrEmpty(obj.ToString()))

            return Convert.ToDateTime(obj).ToString("MM/dd/yyyy");

        else

            return WebSiteConstants.HTML_EM_DASH_ENCODE;

    }

}
