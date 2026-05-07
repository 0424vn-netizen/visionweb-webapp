using System.Linq;
using AS.Common;
using AS.Common.DBManager;
using AS.Common.WebUI;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Controls.UserControls;
using AS.Web.Business;
using System;
using System.Data;
using Telerik.Web.UI;
using AS.Common.Formater;

[PagePermission("PaymentRpt,MSPaymentRpt")]
public partial class PaymentHistory : ReportPage
{
    #region Enums

    enum DataBindAction
    {
        BindDrilldownGrid,
        BindReportGrid,
    }


    #endregion

    string REPORT_HEADER = string.Empty;
    string REPORT_HEADER_DEPOSIT = string.Empty;

    private string _GridTitle
    {
        get
        {
            return GeneralFuncsLib.GetFullGridTitleName(ReportFilter) + " " + GeneralFuncsLib.GetDateFilterText(ReportFilter);
        }
    }

    protected override void PageInitialize()
    {
        this.ExporterIDs.Add("uxExporterDrilldownTop");
        this.ExporterIDs.Add("uxExporterTop");
        this.ReportChart = (IChart)uxPaymentChart;
        base.PageInitialize();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (aperia.controls.KendoChart.IsDataRequest) return;
        REPORT_HEADER = GetLocalResourceObject("PaymentHistory_aspx_cs_PaymentSummary").ToString() + " ";
        REPORT_HEADER_DEPOSIT = GetLocalResourceObject("PaymentHistory_aspx_cs_DepositSummary").ToString() + " ";
        if (GeneralFuncsLib.GetDataOfExtendedSetting("IS_DEPOSIT_HISTORY") == "true")
        {
            //uxPageTitle.ReportTitle = "Trans History - Deposit History";
            this.Title = VeraCodeSolution.DoVeraCode(GetLocalResourceObject("DepositHistory.Title").ToString());
        }
        
        if (IsIntruderDetected) return;
        IsBindDataOnLoad = true;

        if (!IsPostBack)
        {
            if (IsSecureQueryString)
            {
                string isMobileRedirect = SecureQueryString["mb"];
                if (!String.IsNullOrEmpty(isMobileRedirect))
                {
                    string fromDate = SecureQueryString["BeginDate"];
                    string toDate = SecureQueryString["EndDate"];
                    string mid = SecureQueryString["MerchantNumber"];
                    if (!String.IsNullOrEmpty(fromDate))
                    {
                        this.ReportFilter.CurrentValue.DateOptionValue.From = DateTime.Parse(fromDate);
                    }
                    if (!String.IsNullOrEmpty(toDate))
                    {
                        this.ReportFilter.CurrentValue.DateOptionValue.To = DateTime.Parse(toDate);
                    }
                    if (!String.IsNullOrEmpty(mid))
                    {
                        this.ReportFilter.CurrentValue.Value = mid;
                        this.ReportFilter.CurrentValue.HierarchyMode = HierarchyMode.MERCHANT_NR;
                        string hid = string.Empty;
                        DataRow dataRow = SessionManager.HierarchyFilter.Select("HierarchyMode = '" + HierarchyMode.MERCHANT_NR + "'").FirstOrDefault();
                        if (dataRow != null)
                        {
                            hid = dataRow[0].ToString();
                        }
                        this.ReportFilter.CurrentValue.ID = hid;
                    }
                }
            }
            BuildDateValue();
        }
            
    }

    protected override void DoReportFilterAction(AS.Web.UI.Controls.ReportFilterEventArgs e)
    {
        base.DoReportFilterAction(e);
        if (e.ActionType == AS.Web.UI.Controls.ReportFilterEventType.Submit || e.ActionType == AS.Web.UI.Controls.ReportFilterEventType.DrillDown)
            BuildDateValue();
    }

    protected void uxReportGrid_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e is GridSortCommandEventArgs)
        {
            BuildDateValue();
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

    bool _isExporting = false;
    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, AS.Controls.Exporter.ExportConfig exportConfig)
    {
        _isExporting = true;
        if (uxReportGrid.Visible)
        {
            uxReportGrid.Columns.FindByUniqueName("PartialDDANumber").Visible = true;
            uxReportGrid.Columns.FindByUniqueName("DDANumber").Visible = false;
            uxReportGrid.Columns.FindByUniqueName("PartialRoutingNumber").Visible = true;
            uxReportGrid.Columns.FindByUniqueName("RoutingNumber").Visible = false;
        }
        //if (sender == uxExporterBottom)
        //    uxExporterBottom.GridHeader = uxExporterTop.GridHeader;
        //if (sender == uxExporterDrilldownBottom)
        //    uxExporterDrilldownBottom.GridHeader = uxExporterDrilldownTop.GridHeader;
        base.DoNeedExportConfig(sender, exportConfig);
        string fileName = string.Empty;
        if (GeneralFuncsLib.GetDataOfExtendedSetting("IS_DEPOSIT_HISTORY") == "true")        
            fileName = REPORT_HEADER_DEPOSIT;        
        else        
            fileName = REPORT_HEADER;        
        fileName = GeneralFuncsLib.FormatFileName(fileName + " - " + _GridTitle);        

        exportConfig.FileName = fileName.Replace(" ", "");
        exportConfig.ReportHeader = fileName;
        if (sender == uxExporterDrilldownTop)
        {
                uxDrilldownGrid.DisplayEntityNameWhenExport();
                if (uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderText == GetLocalResourceObject("PaymentHistory_aspx_cs_MerchantNumber").ToString()
                || uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderText == GetLocalResourceObject("PaymentHistory_aspx_cs_Merchant").ToString()
                || GeneralFuncsLib.IsMerchantMode(ReportFilter.CurrentValue.HierarchyMode))
                {
                    uxDrilldownGrid.Columns.FindByUniqueName("EntityName").HeaderText = GetLocalResourceObject("PaymentHistory_aspx_cs_MerchantName").ToString();
                }
        }
    }

    protected override void DoGridNeedDataSource(AS.Controls.Grid.ASGrid sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if (aperia.controls.KendoChart.IsDataRequest) return;

        if (sender == uxDrilldownGrid && uxDrilldownGrid.Visible)
            OnDataBindControls(DataBindAction.BindDrilldownGrid, sender);
        if (sender == uxReportGrid && uxReportGrid.Visible)
            OnDataBindControls(DataBindAction.BindReportGrid, sender);
    }

    protected override void DoItemDataBound(AS.Controls.Grid.ASGrid sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (IsIntruderDetected) return;
        if (sender == uxDrilldownGrid && uxDrilldownGrid.Visible)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = e.Item as GridDataItem;
                DataRowView dataRow = e.Item.DataItem as DataRowView; 
                if (!GeneralFuncsLib.SHOW_PROCESSINGDATA_ENTITYNAME())
                {
                    if (GeneralFuncsLib.IsMerchantModeToSetTooltip(ReportFilter) && dataRow.DataView.Table.Columns["EntityName"] != null)
                    {
                        dataItem["DrilldownColumn"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["EntityName"].ToString());
                    }
                }
                dataItem["EntityName"].Text = VeraCodeSolution.DoVeraCode(dataRow["EntityName"].ToString());

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
                urlDeposite = "PaymentHistoryDetails.aspx?" + queryString;
                url = string.Format("<a href=\"#\" onclick=\" return ShowPopupModal('{0}','auto');\">{1}</a>", urlDeposite, ((DateTime)dataRow["ReportDate"]).ToString("MM/dd/yyyy"));

                dataItem["ReportDate"].Text = VeraCodeSolution.DoVeraCode(url);
                if (uxReportGrid.Columns.FindByUniqueName("RoutingNumber").Visible && dataRow["RoutingNumber"].ToString().IsNullOrEmpty())
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
                urlDeposite = "PaymentHistoryDetails.aspx?" + queryString;


                decimal? _netDepositAmount = uxReportGrid.AS_Total["NetDepositAmount"].SafeGetDecimal();
                if (_netDepositAmount != null)
                {
                    string _tmp = FormatData.FormatCurrency(_netDepositAmount, SessionManager.CurrencyFortmat);
                    string css;
                    if (_netDepositAmount < 0)
                    {
                        _tmp = _tmp.Replace("-", "");
                        css = "class=\"negative\"";
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
            if (SessionManager.CurrentUser.ASClient == WebSiteConstants.ORION_CLIENT)
            {
                uxReportGrid.Columns.FindByUniqueName("TransType").Visible = true;
            }
            else
            {
                uxReportGrid.Columns.FindByUniqueName("TransType").Visible = false;
            }
        }
        else
        {
            uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderText = GeneralFuncsLib.GetGridDrilldownColumnName(ReportFilter.CurrentValue.HierarchyMode, ReportFilter.CurrentValue.Value);
            uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderTooltip = GeneralFuncsLib.GetGridDrilldownColumnName(ReportFilter.CurrentValue.HierarchyMode, ReportFilter.CurrentValue.Value);          
            uxDrilldownGrid.Visible = true;
            uxReportGrid.Visible = false;
            GeneralFuncsLib.VisibleHierarchyNameByFilteringMode(ReportFilter.CurrentValue.HierarchyMode, ReportFilter.CurrentValue.Value, uxDrilldownGrid, uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderText);
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        FilterParameterCollection parames = new FilterParameterCollection();
        parames.AddLoggedInUserReportingParams();
        parames.AddHierarchyFilterParams(this);

        string spaName = string.Empty;
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindDrilldownGrid:
                {
                    spaName = WebSiteConstants.GET_REPORT_SPA_NAME;
                    parames.Add(new FilterParameter("@ReportType", "Deposits", DbType.AnsiString));
                    if (GeneralFuncsLib.GetDataOfExtendedSetting("IS_DEPOSIT_HISTORY") == "true")
                    {
                        uxExporterDrilldownTop.GridTitle = VeraCodeSolution.ValidateResponseData(REPORT_HEADER_DEPOSIT);
                    }
                    else
                    {
                        uxExporterDrilldownTop.GridTitle = VeraCodeSolution.ValidateResponseData(REPORT_HEADER);
                    }
                    uxExporterDrilldownTop.GridSubTitle = VeraCodeSolution.ValidateResponseData(_GridTitle);
                    uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderTooltip = GeneralFuncsLib.GetGridDrilldownColumnName(ReportFilter.CurrentValue.HierarchyMode, ReportFilter.CurrentValue.Value);
                    GeneralFuncsLib.VisibleHierarchyNameByFilteringMode(ReportFilter.CurrentValue.HierarchyMode, ReportFilter.CurrentValue.Value, uxDrilldownGrid, uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderText);
                }
                break;
            case DataBindAction.BindReportGrid:
                {
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
                    parames.AddLoggedInUserPrimaryUserID();

                    spaName = "spa_GetMerchantDeposit";
                    if (GeneralFuncsLib.GetDataOfExtendedSetting("IS_DEPOSIT_HISTORY") == "true")
                    {
                        uxExporterTop.GridTitle = VeraCodeSolution.ValidateResponseData(REPORT_HEADER_DEPOSIT);
                    }
                    else
                    {
                        uxExporterTop.GridTitle = VeraCodeSolution.ValidateResponseData(REPORT_HEADER);
                    }
                    uxExporterTop.GridSubTitle = VeraCodeSolution.ValidateResponseData(_GridTitle);
                }
                break;
        }
        ((ASGrid)sender).DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(parames) });
    }


    protected override void OnPreRender(EventArgs e)
    { 
        base.OnPreRender(e);
        uxDrilldownGrid.Columns.FindByUniqueName("EntityName").Visible = GeneralFuncsLib.SHOW_PROCESSINGDATA_ENTITYNAME();
    }
}
