using AS.Common;
using AS.Common.DBManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using AS.Web.UI.Controls;
using AS.Controls.UserControls;
using AS.Controls.Grid;
using AS.Web.Business;
using AS.Controls.Pages;
using AS.Common.DataProtection;

[PagePermission("RskG2MoneyLaundering,MsRskG2MoneyLaundering")]
public partial class rm_MCF_MgmtReport_G2MoneyLaundering : ReportPage
{
    #region Enum

    enum DataBindAction
    {
        BindingG2MoneyLaunderingGrid
    }

    #endregion

    #region Properties

    bool _isExporting = false;

    private string _GridSubTitle
    {
        get
        {
            var reporter = SessionManager.CurrentReportFilter;
            if (reporter != null)
                return string.Format(GetLocalResourceObject("GridSubtitle.Text").ToString(), reporter.DateOptionValue.From.ToShortDateString(),
                    reporter.DateOptionValue.To.ToShortDateString());
            return string.Empty;
        }
    }

    #endregion

    protected override void PageInitialize()
    {
        this.GridIDs.Add("uxGroupList");
        this.ExporterIDs.Add("uxExporter");
        base.PageInitialize();
        IsBindDataOnLoad = true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected override void DoGridNeedDataSource(AS.Controls.Grid.ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        if (sender == uxGroupList)
        {
            OnDataBindControls(DataBindAction.BindingG2MoneyLaunderingGrid, sender);
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindingG2MoneyLaunderingGrid:
                {
                    HierarchyFilterValue reportFilter = SessionManager.CurrentReportFilter ?? new HierarchyFilterValue();
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserReportingParams();       
                    parameters.Add(new FilterParameter("@DateFilterMode", 3, DbType.Int16));
                    parameters.Add(new FilterParameter("@BeginDate", reportFilter.DateOptionValue.From, DbType.DateTime));
                    parameters.Add(new FilterParameter("@EndDate", reportFilter.DateOptionValue.To, DbType.DateTime));
                    if (CheckCSViewFullCard() || GeneralFuncsLib.HasIPForFullCard((SecurePage)this))
                    {
                        parameters.AddDecryptDataParams("AccountNumber", _isExporting);
                    }
                    ((ASGrid)sender).DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { "spa_RM_MCF_GetMatchingReport", ReportServices.ConvertToFilterParamWSArray(parameters) });
                    //Set title for grid
                    uxExporter.GridTitle = VeraCodeSolution.ValidateResponseData(GetLocalResourceObject("uxReportTitleResource1.ReportTitle").ToString());
                    uxExporter.GridSubTitle = VeraCodeSolution.ValidateResponseData(_GridSubTitle);
                    break;
                }
        }
    }

    private bool CheckCSViewFullCard()
    {
        if ((SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.AS
            || SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.CS)
            && IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_CC))
        {
            return true;
        }
        return false;
    }

    protected void uxReportFilter_Search()
    {
        uxGroupList.MasterTableView.CurrentPageIndex = 0;
        uxGroupList.Rebind();
    }
    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, AS.Controls.Exporter.ExportConfig exportConfig)
    {
        _isExporting = true;
        base.DoNeedExportConfig(sender, exportConfig);
        var filename = string.Format(GetLocalResourceObject("ExportFileName").ToString(), "VisionWeb", SessionManager.ClientInfo.ClientName,
            DateTime.Now.ToString("MMddyyyyhhmmss"));
        exportConfig.FileName = GeneralFuncsLib.FormatFileName(filename);
        exportConfig.ReportHeader = string.Format("{0} - {1}", uxExporter.GridHeader, _GridSubTitle);
    }

    protected override void DoItemDataBound(ASGrid sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView rowView = e.Item.DataItem as DataRowView;
            if (sender == uxGroupList && sender.Visible)
            {
                string modalUrl = "rm_MCF_RiskReport.aspx?" + this.BuildSecureQueryString(string.Format("merchantnumber={0}&IsPopup=true", rowView["MerchantNumber"]));
                string urlMerch = "<a class=\"link\" href=\"javascript:void(0)\" onclick=\"openPopupWindow('" + modalUrl + "','RiskReport'); return false;\">";
                dataItem["MerchantNumber"].Text = VeraCodeSolution.DoVeraCode(urlMerch + rowView["MerchantNumber"] + "</a>");

                string queryString = BuildSecureQueryString("cn=" + rowView["PartialCardNumber"] + "&cnf=" + rowView["AccountNumber"] + "&merch=" + rowView["MerchantNumber"] + "&from=G2");
                string urlCardDetail = ResolveUrl("~/") + "CardHistoryModal.aspx?" + queryString;
                string urlCard = "<a class=\"link\" href=\"javascript:void(0)\" onclick=\"openPopupWindow('" + urlCardDetail + "','CardHistoryWindow'); return false;\">";
                dataItem["PartialCardNumber"].Text = VeraCodeSolution.DoVeraCode(urlCard + rowView["PartialCardNumber"] + "</a>");
                //Sprint 9 - 41559 - VW - G2 Transaction Laundering Report - small enhancements
                dataItem["MerchantName"].CssClass = "ellipsis";
                dataItem["MerchantName"].ToolTip = VeraCodeSolution.DoVeraCode(rowView["MerchantName"].ToSafeString());
            }
        }
    }
}