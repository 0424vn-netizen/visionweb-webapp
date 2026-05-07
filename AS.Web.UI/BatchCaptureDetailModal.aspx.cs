using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AS.Common;
using Telerik.Web.UI;
using System.Data;
using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Web.Business;

public partial class BatchCaptureDetailModal : ReportPage
{
    #region Enums
    enum DataBindAction
    {
        BindReportGrid,
    }
    #endregion

    string _MerchantNumber = string.Empty;
    string _MerchantName = string.Empty;
    string _BatchNumber = string.Empty;
    DateTime _ReportDate = DateTime.Now;
    protected override void PageInitialize()
    {
        this.ExporterIDs.Add("uxExporterBottom");
        base.PageInitialize();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
        PageType = SecurePageType.Modal;
        IsBindDataOnLoad = true;
        if (SecureQueryString != null)
        {
            ProcessQueryString();
        }
        _MerchantName = GeneralFuncsLib.GetMerchantName(_MerchantNumber);
        if (!IsPostBack)
        {
            uxMerchantInfo.Text = VeraCodeSolution.DoVeraCode(_MerchantNumber + (!_MerchantName.IsNullOrEmpty() ? " - " + _MerchantName : string.Empty));

        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }

    private string GetGridHeader()
    {
        return string.Format(GetLocalResourceObject("BatchCaptureDetailModal_aspx_cs_BRD").ToString(), _BatchNumber, _ReportDate.ToShortDateString());
    }

    bool _isExporting = false;
    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, AS.Controls.Exporter.ExportConfig exportConfig)
    {
        _isExporting = true;
        uxReportGrid.Columns.FindByUniqueName("AccountNumber").Visible = false;
        uxReportGrid.Columns.FindByUniqueName("PartialCardNumber").Visible = true;
        base.DoNeedExportConfig(sender, exportConfig);
        exportConfig.FileName = GeneralFuncsLib.FormatFileName(uxExporter.GridHeader);
        if (uxExporter.ExportButtonType == AS.Controls.UserControls.UxExport.ExportType.Excel)
        {
            exportConfig.ReportHeader = GetLocalResourceObject("BatchCaptureDetailModal_aspx_BCD").ToString() + "<br />" + GetLocalResourceObject("BatchCaptureDetailModal_aspx_cs_Merchant").ToString() + ": " + uxMerchantInfo.Text + "<br />" + GetLocalResourceObject("BatchCaptureDetailModal_aspx_cs_BatchNumber").ToString() + ": " + _BatchNumber + " " + GetLocalResourceObject("BatchCaptureDetailModal_aspx_cs_ReportDate").ToString() + ": " + _ReportDate.ToShortDateString();
        }
        else
        {
            exportConfig.ReportHeader = GetLocalResourceObject("BatchCaptureDetailModal_aspx_BCD").ToString() + Environment.NewLine + GetLocalResourceObject("BatchCaptureDetailModal_aspx_cs_Merchant").ToString() + ": " + uxMerchantInfo.Text + Environment.NewLine + GetLocalResourceObject("BatchCaptureDetailModal_aspx_cs_BatchNumber").ToString() + ": " + _BatchNumber + " " + GetLocalResourceObject("BatchCaptureDetailModal_aspx_cs_ReportDate").ToString() + ": " + _ReportDate.ToShortDateString();
        }
    }

    protected override void DoGridNeedDataSource(ASGrid sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.BindReportGrid, sender);
    }

    protected override void DoItemDataBound(ASGrid sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView dataRow = e.Item.DataItem as DataRowView;

            dataItem["CardType"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["CardDescription"].ToString());
            //dataItem["TransType"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["TransactionDescription"].ToString());
            dataItem["MktCode"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["MarketDataDescription"].ToString().Trim());
            dataItem["MOTO"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["MOTODescription"].ToString().Trim());
            dataItem["ExpDate"].ToolTip = "MM/YY";
        }
    }

    protected override void DoSwitchView()
    {
        if (IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_CC))
        {
            uxReportGrid.Columns.FindByUniqueName("AccountNumber").Visible = true;
            uxReportGrid.Columns.FindByUniqueName("PartialCardNumber").Visible = false;
        }
        else
        {
            uxReportGrid.Columns.FindByUniqueName("AccountNumber").Visible = false;
            uxReportGrid.Columns.FindByUniqueName("PartialCardNumber").Visible = true;
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindReportGrid:
                {
                    string merchantName = GeneralFuncsLib.GetMerchantName(_MerchantNumber);
                    uxExporter.GridHeader = VeraCodeSolution.ValidateResponseData(GetGridHeader());
                    FilterParameterCollection _params = new FilterParameterCollection();
                    _params.AddLoggedInUserParams(0);
                    _params.Add(new FilterParameter("@BeginDate", _ReportDate, DbType.Date));
                    _params.Add(new FilterParameter("@EndDate", _ReportDate, DbType.Date));
                    _params.Add(new FilterParameter("@MerchantNumber", _MerchantNumber, DbType.AnsiString));
                    _params.Add(new FilterParameter("@BatchNumber", _BatchNumber, DbType.AnsiString));
                    if (IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_CC))
                    {
                        _params.AddDecryptDataParams("AccountNumber", _isExporting);
                    }
                    _params.AddLanguageID();
                    string spaName = "spa_cs_GetBatchSearchDetail";
                    ((ASGrid)sender).DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(_params) });

                }
                break;
        }
    }

    private void ProcessQueryString()
    {
        _ReportDate = DateTime.Parse(SecureQueryString["ReportDate"]);
        _MerchantNumber = SecureQueryString["MerchantNumber"];
        _BatchNumber = SecureQueryString["BatchNumber"];
    }
}
