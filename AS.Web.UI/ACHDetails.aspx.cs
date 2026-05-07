using AS.Common;
using AS.Common.DBManager;
using AS.Common.Formater;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Controls.UserControls;
using AS.Web.Business;
using System;
using System.Data;
using Telerik.Web.UI;

[PagePermission("ACHRpt,MSDepRpt")]
public partial class ACHDetails : ReportPage
{
    enum DataBindAction
    {
        BindReportGrid,
        BindDrilldownGrid,
    }

    private string _GridTitle
    {
        get
        {
            return GeneralFuncsLib.GetFullGridTitleName(ReportFilter) + " " + GeneralFuncsLib.GetDateFilterText(ReportFilter);
        }
    }
    private string _GridDrillDownHeader
    {
        get
        {
            return GeneralFuncsLib.GetGridDrilldownColumnName(ReportFilter.CurrentValue.HierarchyMode, ReportFilter.CurrentValue.Value);
        }
    }
    DateTime _BeginDate
    {
        set { ViewState["_BeginDate"] = value; }
        get
        {
            if (ViewState["_BeginDate"] == null)
                return DateTime.Today;
            else return DateTime.Parse(ViewState["_BeginDate"].ToString());
        }
    }
    DateTime _EndDate
    {
        set { ViewState["_EndDate"] = value; }
        get
        {
            if (ViewState["_EndDate"] == null)
                return DateTime.Today;
            else return DateTime.Parse(ViewState["_EndDate"].ToString());
        }
    }
    protected override void PageInitialize()
    {
        if (IsIntruderDetected) return;

        this.ExporterIDs.Add("uxExporterDrilldownTop");
        this.ExporterIDs.Add("uxExporterReportTop");
        base.PageInitialize();
    }

    private string GetActivity(bool isMerchant)
    {
        string value = GeneralFuncsLib.GetValueForActivityFunc(isMerchant, ReportFilter);
        string dateText = GeneralFuncsLib.GetDateTextForActivityFunc(isMerchant, ReportFilter);
        return string.Format("ACH Details: {0} {1}", value, dateText);
    }

    protected override void DoReportFilterAction(AS.Web.UI.Controls.ReportFilterEventArgs e)
    {
        if (e.ActionType == AS.Web.UI.Controls.ReportFilterEventType.Submit || e.ActionType == AS.Web.UI.Controls.ReportFilterEventType.DrillDown)
        {
            if (GeneralFuncsLib.IsMerchantMode(e.HierachyValue.HierarchyMode))
            {
                string merchantName = GeneralFuncsLib.GetMerchantName(e.HierachyValue.Value);
                string activity = GetActivity(!string.IsNullOrEmpty(merchantName));
                string merchantNumber = string.IsNullOrEmpty(merchantName) ? "" : e.HierachyValue.Value;
                GeneralFuncsLib.SaveUserActivity(merchantNumber, activity);
            }
            BuildDateValue();
        }
        base.DoReportFilterAction(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        IsBindDataOnLoad = true;
        if (SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.CS || SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.AS)
        {
            uxPageTitle.ReportTitle = VeraCodeSolution.DoVeraCode(GetLocalResourceObject("Text_Title_ACHDetail").ToString());
            this.Title = VeraCodeSolution.DoVeraCode(GetLocalResourceObject("Text_ACHDetail").ToString());
        }
        else
        {
            uxPageTitle.ReportTitle = VeraCodeSolution.DoVeraCode(GetLocalResourceObject("Text_Title_DepositDetail").ToString());
            this.Title = VeraCodeSolution.DoVeraCode(GetLocalResourceObject("Text_DepositDetail").ToString());
        }
        if (!IsPostBack)
            BuildDateValue();
    }

    protected void uxReportGrid_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e is GridSortCommandEventArgs)
        {
            BuildDateValue();
        }
    }

    protected override void DoSwitchView()
    {
        if (GeneralFuncsLib.IsMerchantMode(ReportFilter.CurrentValue.HierarchyMode)
           && !string.IsNullOrEmpty(ReportFilter.CurrentValue.Value))
        {
            uxDrilldownGrid.Visible = false;
            uxReportGrid.Visible = true;
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
        else
        {
            uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderText = VeraCodeSolution.DoVeraCode(_GridDrillDownHeader);
            uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderTooltip = VeraCodeSolution.DoVeraCode(_GridDrillDownHeader);
            uxDrilldownGrid.Visible = true;
            uxReportGrid.Visible = false;
            GeneralFuncsLib.VisibleHierarchyNameByFilteringMode(ReportFilter.CurrentValue.HierarchyMode, ReportFilter.CurrentValue.Value, uxDrilldownGrid, uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderText);                    
        }
    }
    protected override void DoGridNeedDataSource(ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        if (sender == uxReportGrid && uxReportGrid.Visible)
        {
            OnDataBindControls(DataBindAction.BindReportGrid, sender);
        }
        if (sender == uxDrilldownGrid && uxDrilldownGrid.Visible)
        {
            OnDataBindControls(DataBindAction.BindDrilldownGrid, sender);
        }
    }

    bool _isExporting = false;
    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, AS.Controls.Exporter.ExportConfig exportConfig)
    {
        _isExporting = true;
        base.DoNeedExportConfig(sender, exportConfig);
        string fileName = ((UxExport)sender).GridTitle.ToString() + _GridTitle;
        if (sender == this.uxExporterDrilldownTop)
        {
            if (sender.ExportButtonType == AS.Controls.UserControls.UxExport.ExportType.Excel)
            {
                uxDrilldownGrid.Columns.FindByUniqueName("EntityName").Visible = true;
                if (uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderText == GetLocalResourceObject("Text_MerchantNumber").ToString()
                || uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderText == GetLocalResourceObject("Text_Merchant").ToString()
                || GeneralFuncsLib.IsMerchantMode(ReportFilter.CurrentValue.HierarchyMode))
                {
                    uxDrilldownGrid.Columns.FindByUniqueName("EntityName").HeaderText = GetLocalResourceObject("Text_MerchantName").ToString();
                }
            }
        }
        exportConfig.FileName = fileName.Replace(" ", "");
        exportConfig.ReportHeader = fileName;
        if (uxReportGrid.Visible)
        {
            uxReportGrid.Columns.FindByUniqueName("PartialDDANumber").Visible = true;
            uxReportGrid.Columns.FindByUniqueName("DDANumber").Visible = false;
            uxReportGrid.Columns.FindByUniqueName("PartialRoutingNumber").Visible = true;
            uxReportGrid.Columns.FindByUniqueName("RoutingNumber").Visible = false;
        }
    }

    private void BuildDateValue()
    {
        _BeginDate = ReportFilter.CurrentValue.DateOptionValue.From;
        _EndDate = ReportFilter.CurrentValue.DateOptionValue.To;
        switch (ReportFilter.CurrentValue.DateOption)
        {
            case AS.Web.UI.Controls.DateOptionMode.Daily:
                {
                    _EndDate = _BeginDate;
                }
                break;
            case AS.Web.UI.Controls.DateOptionMode.Monthly:
                {
                    _BeginDate = _BeginDate.GetFirstDayOfMonth();
                    if (_BeginDate.Year == DateTime.Today.Year && _BeginDate.Month == DateTime.Today.Month)
                        _EndDate = DateTime.Today;
                    else
                        _EndDate = _BeginDate.GetLastDayOfMonth();
                }
                break;
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        ASGrid grid = (ASGrid)sender;
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();
        parameters.AddHierarchyFilterParams((ReportPage)this.Page);

        switch ((DataBindAction)type)
        {
            case DataBindAction.BindReportGrid:
                {
                    //parameters.AddDecryptDataParams("RoutingNumber", _isExporting);
                    if (this.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_DDA) || this.IsUserWithPermission("MS" + WebSiteConstants.SEC_PERMISSION_DDA))
                    {
                        if (SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.CS || SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.AS)
                            parameters.AddDecryptDataParams("DDANumber,RoutingNumber", _isExporting);
                        else
                            parameters.AddDecryptDataParams("DDANumber", _isExporting);
                    }
                    else
                    {
                        if (SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.CS || SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.AS)
                            parameters.AddDecryptDataParams("RoutingNumber", _isExporting);
                    }
                    parameters.AddLoggedInUserPrimaryUserID();
                    string spaName = "spa_GetMerchantDeposit";
                    grid.DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(parameters) });

                    if (SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.CS || SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.AS)
                    {
                        uxExporterReportTop.GridTitle = VeraCodeSolution.ValidateResponseData(GetLocalResourceObject("Text_ACHReport").ToString()) + " ";
                    }
                    else
                    {
                        uxExporterReportTop.GridTitle = VeraCodeSolution.ValidateResponseData(GetLocalResourceObject("Text_DepositReport").ToString()) + " ";
                    }
                    uxExporterReportTop.GridSubTitle = VeraCodeSolution.ValidateResponseData(_GridTitle);
                }
                break;
            case DataBindAction.BindDrilldownGrid:
                {
                    //if (this.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_DDA) || this.IsUserWithPermission("MS" + WebSiteConstants.SEC_PERMISSION_DDA))
                    //    parameters.AddDecryptDataParams("AccountNumber", _isExporting);
                    parameters.Add("@ReportType", "ACH", DbType.String);
                    string spaName = WebSiteConstants.GET_REPORT_SPA_NAME;
                    grid.DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(parameters) });
                    if (SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.CS || SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.AS)
                    {
                        uxExporterDrilldownTop.GridTitle = VeraCodeSolution.ValidateResponseData(GetLocalResourceObject("Text_ACHReport").ToString()) + " ";
                    }
                    else
                    {
                        uxExporterDrilldownTop.GridTitle = VeraCodeSolution.ValidateResponseData(GetLocalResourceObject("Text_DepositReport").ToString()) + " ";
                    }
                    uxExporterDrilldownTop.GridSubTitle = VeraCodeSolution.ValidateResponseData(_GridTitle);
                    GeneralFuncsLib.VisibleHierarchyNameByFilteringMode(ReportFilter.CurrentValue.HierarchyMode, ReportFilter.CurrentValue.Value, uxDrilldownGrid, uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderText);                  
                }
                break;
        }
    }
    protected override void DoItemDataBound(ASGrid sender, GridItemEventArgs e)
    {
        if (sender == uxDrilldownGrid && uxDrilldownGrid.Visible)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = e.Item as GridDataItem;
                DataRowView dataRow = e.Item.DataItem as DataRowView;
                if (GeneralFuncsLib.IsMerchantModeToSetTooltip(ReportFilter) && dataRow.DataView.Table.Columns["Entity"] != null)
                {
                    dataItem["DrilldownColumn"].ToolTip = VeraCodeSolution.ValidateResponseData(GeneralFuncsLib.GetMerchantName(dataRow["Entity"].ToString()));
                }
            }
        }
        if (sender == uxReportGrid && uxReportGrid.Visible)
        {
            DataRowView dataRow = e.Item.DataItem as DataRowView;
            string queryString = string.Empty, urlDeposite = string.Empty, url = string.Empty;
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = e.Item as GridDataItem;

                queryString = BuildSecureQueryString("MerchantNumber=" + ReportFilter.CurrentValue.Value + "&BeginDate=" + ((DateTime)dataRow["ReportDate"])
                                                + "&EndDate=" + ((DateTime)dataRow["ReportDate"])
                                                + "&DateRange=1");
                urlDeposite = "ACHDetailsModal.aspx?" + queryString;
                url = string.Format("<a href=\"#\" onclick=\" return ShowPopupModal('{0}','auto');\">{1}</a>", urlDeposite, ((DateTime)dataRow["ReportDate"]).ToString("MM/dd/yyyy"));

                dataItem["ReportDate"].Text = VeraCodeSolution.DoVeraCode(url);
                if (dataRow["RoutingNumber"].ToString().IsNullOrEmpty())
                {
                    dataItem["RoutingNumber"].Text = VeraCodeSolution.DoVeraCode(dataRow["PartialRoutingNumber"].ToString());
                }
            }
            if (e.Item is GridFooterItem)
            {
                GridFooterItem footerItem = e.Item as GridFooterItem;


                queryString = BuildSecureQueryString("MerchantNumber=" + ReportFilter.CurrentValue.Value + "&BeginDate=" + _BeginDate
                                                + "&EndDate=" + _EndDate
                                                + "&DateRange=0");
                urlDeposite = "ACHDetailsModal.aspx?" + queryString;

                string css;
                string _tmp = FormatData.FormatCurrency((decimal)uxReportGrid.AS_Total["NetDepositAmount"], SessionManager.CurrencyFortmat);

                if ((decimal)uxReportGrid.AS_Total["NetDepositAmount"] < 0)
                {
                    css = "class=\"negative\"";
                    _tmp = "(" + _tmp.Replace("-", "") + ")";
                }
                else
                {
                    css = String.Empty;
                }

                url = string.Format("<a {0} href=\"#\" onclick=\" return ShowPopupModal('{1}','auto');\">{2}</a>", css, urlDeposite, _tmp);
                footerItem["NetDepositAmount"].Text = VeraCodeSolution.DoVeraCode(url);
            }
        }
    }
}
