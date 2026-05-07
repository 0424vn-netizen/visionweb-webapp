using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Web.Business;
using AS.Web.UI.Controls;
using System;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

[PagePermission("ACHRpt,MSDepRpt")]
public partial class ACHDetailsModal : ReportPage
{
    enum DataBindAction
    {
        BindReportGrid,
    }
    string _MerchantNumber = string.Empty;
    int _DateRange = int.MinValue;
    DateTime _BeginDate = DateTime.Today;
    DateTime _EndDate = DateTime.Today;

    protected override void PageInitialize()
    {
        base.PageInitialize();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;
        if (IsIntruderDetected) return;
        IsBindDataOnLoad = true;
        ProcessQueryString();
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }
    void ProcessQueryString()
    {
        _MerchantNumber = SecureQueryString["MerchantNumber"];
        _BeginDate = DateTime.Parse(SecureQueryString["BeginDate"]);
        _EndDate = DateTime.Parse(SecureQueryString["EndDate"]);
        _DateRange = int.Parse(SecureQueryString["DateRange"]);
    }
    bool _isExporting = false;
    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, AS.Controls.Exporter.ExportConfig exportConfig)
    {
        _isExporting = true;
        uxReportGrid.Columns.FindByUniqueName("PartialDDANumber").Visible = true;
        uxReportGrid.Columns.FindByUniqueName("DDANumber").Visible = false;
        uxReportGrid.Columns.FindByUniqueName("PartialRoutingNumber").Visible = true;
        uxReportGrid.Columns.FindByUniqueName("RoutingNumber").Visible = false;
        base.DoNeedExportConfig(sender, exportConfig);
        exportConfig.FileName = GeneralFuncsLib.FormatFileName("ACHDetails_" + uxTitle.Text + "_" + uxExporter.GridSubTitle);
        if (sender.ExportButtonType != AS.Controls.UserControls.UxExport.ExportType.Excel)
        {
            exportConfig.ReportHeader = GetLocalResourceObject("Text_ACHDetails").ToString() + Environment.NewLine + uxTitle.Text + Environment.NewLine + uxExporter.GridSubTitle;
        }
        else
        {
            exportConfig.ReportHeader = GetLocalResourceObject("Text_ACHDetails").ToString() + "<br />" + uxTitle.Text + "<br />" + uxExporter.GridSubTitle;
        }
    }

    protected override void DoGridNeedDataSource(AS.Controls.Grid.ASGrid sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.BindReportGrid, sender);
    }

    protected override void DoItemDataBound(AS.Controls.Grid.ASGrid sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView dataRow = e.Item.DataItem as DataRowView;

            dataItem["TransactionCode"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["TransactionCode"] != null ? dataRow["TransactionCode"].ToString() : string.Empty);
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        FilterParameterCollection parames = new FilterParameterCollection();
        parames.AddLoggedInUserReportingParams();
        parames.AddLoggedInUserPrimaryUserID();
        parames.Add(new FilterParameter("@HierarchyFilterMode", "MERCHANTNUMBER", DbType.AnsiString));
        parames.Add(new FilterParameter("@HierarchyFilterValue", _MerchantNumber, DbType.AnsiString));

        if (this.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_DDA) || this.IsUserWithPermission("MS" + WebSiteConstants.SEC_PERMISSION_DDA))
        {
            if (SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.CS || SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.AS)
                parames.AddDecryptDataParams("DDANumber,RoutingNumber", _isExporting);
            else
                parames.AddDecryptDataParams("DDANumber", _isExporting);
        }
        else
        {
            if (SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.CS || SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.AS)
                parames.AddDecryptDataParams("RoutingNumber", _isExporting);
        }
        string spaName = "spa_GetMerchantDepositDetail";
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindReportGrid:
                {
                    if (_DateRange == 0)
                    {
                        parames.Add(new FilterParameter("@DateFilterMode", (int)DateOptionMode.DateRange, DbType.Int32));
                    }
                    else
                    {
                        parames.Add(new FilterParameter("@DateFilterMode", (int)DateOptionMode.Daily, DbType.Int32));
                    }
                    parames.Add(new FilterParameter("@BeginDate", _BeginDate, DbType.Date));
                    parames.Add(new FilterParameter("@EndDate", _EndDate, DbType.Date));
                    ((ASGrid)sender).DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(parames) });

                    string merchantName = GeneralFuncsLib.GetMerchantName(_MerchantNumber);
                    uxTitle.Text = VeraCodeSolution.DoVeraCode(GetLocalResourceObject("Text_Merchant").ToString() + ": " + _MerchantNumber);
                    if (!string.IsNullOrEmpty(merchantName))
                    {
                        uxTitle.Text += VeraCodeSolution.DoVeraCode(" - " + merchantName);
                    }


                    if (_DateRange != 0)
                    {
                        uxExporter.GridSubTitle = VeraCodeSolution.ValidateResponseData(GetLocalResourceObject("Text_ReportDate").ToString() + ": " + _BeginDate.ToString("MM/dd/yyyy"));
                    }
                    else
                    {
                        uxExporter.GridSubTitle = VeraCodeSolution.ValidateResponseData(GetLocalResourceObject("Text_ReportPeriod").ToString() + ": " + _BeginDate.ToString("MM/dd/yyyy") + " - " + _EndDate.ToString("MM/dd/yyyy"));
                    }
                }
                break;
        }

    }
    protected override void DoSwitchView()
    {
        if (this.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_DDA) || this.IsUserWithPermission("MS" + WebSiteConstants.SEC_PERMISSION_DDA))
        {
            uxReportGrid.Columns.FindByUniqueName("PartialDDANumber").Visible = false;
            uxReportGrid.Columns.FindByUniqueName("DDANumber").Visible = true;
        }
        else
        {
            uxReportGrid.Columns.FindByUniqueName("PartialDDANumber").Visible = true;
            uxReportGrid.Columns.FindByUniqueName("DDANumber").Visible = false;
        }
        if (SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.CS || SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.AS)
        {
            uxReportGrid.Columns.FindByUniqueName("PartialRoutingNumber").Visible = false;
            uxReportGrid.Columns.FindByUniqueName("RoutingNumber").Visible = true;
        }
        else
        {
            uxReportGrid.Columns.FindByUniqueName("PartialRoutingNumber").Visible = true;
            uxReportGrid.Columns.FindByUniqueName("RoutingNumber").Visible = false;
        }
    }
}
