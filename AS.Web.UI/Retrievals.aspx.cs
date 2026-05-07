using System.Linq;
using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Controls.UserControls;
using AS.Web.Business;
using System;
using System.Data;
using Telerik.Web.UI;

[PagePermission("RetRpt,MSRetRpt")]

public partial class gen_Retrievals : ReportPage
{
    enum DataBindAction
    {
        BindRetrievalGrid,
        BindDrillDownGrid
    }
    private const string TODAY = "Today";
    private const string MTD = "MTD";
    private const string YTD = "YTD";
    string REPORT_HEADER = string.Empty;
    string REPORT_HEADER_DETAIL = string.Empty;
    private string _Target = "_parent";
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
    string _RetrievalIntruderQuery = string.Empty;
    private string RetrievalIntruderQuery
    {
        get
        {
            if (_RetrievalIntruderQuery == string.Empty) _RetrievalIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(uxReportGrid.ID, new string[] { "ReferenceNumber" });
            return _RetrievalIntruderQuery;
        }
    }

    string _CardSearchIntruderQuery = string.Empty;
    private string CardSearchIntruderQuery
    {
        get
        {

            if (_CardSearchIntruderQuery == string.Empty)
            {
                _CardSearchIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(uxReportGrid.ID, new string[] { "CardNumber" });
            }
            return _CardSearchIntruderQuery;
        }
    }

    private const string IMAGE = "<a class=\"image-link\" href=\"#\" onclick=\" return ShowPopupModal('{0}','auto');\"><img src='res/img/information.png' border=\"0\"/></a>&nbsp; &nbsp;";

    private bool IsShowRoutingAccount
    {
        get
        {
            if (ViewState["ShowRoutingAccount"] != null)
                return (bool)(ViewState["ShowRoutingAccount"]);
            else
            {
                ViewState["ShowRoutingAccount"] = GeneralFuncsLib.Show_RoutingAccountNumber;
                return (bool)ViewState["ShowRoutingAccount"];
            }
        }
        set
        {
            ViewState["ShowRoutingAccount"] = value;
        }
    }

    protected override void PageInitialize()
    {
        if (IsIntruderDetected) return;

        this.ExporterIDs.Add("uxExportDrilldownTop");
        this.ExporterIDs.Add("uxExportDrilldownBottom");
        this.ExporterIDs.Add("uxExportReportGrid");
        this.ExporterIDs.Add("uxExportReportGridBottom");
        base.PageInitialize();
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
    protected override void DoSwitchView()
    {
        uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderText = VeraCodeSolution.DoVeraCode(_GridDrillDownHeader);
        uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderTooltip = VeraCodeSolution.DoVeraCode(_GridDrillDownHeader);
        GeneralFuncsLib.VisibleHierarchyNameByFilteringMode(ReportFilter.CurrentValue.HierarchyMode, ReportFilter.CurrentValue.Value, uxDrilldownGrid, uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderText);
        if (GeneralFuncsLib.IsMerchantMode(ReportFilter.CurrentValue.HierarchyMode)
            && !string.IsNullOrEmpty(ReportFilter.CurrentValue.Value))
        {
            uxPanelFaxNumber.Visible = true;
            uxReportGrid.Visible = true;
            uxDrilldownGrid.Visible = false;
            if (CheckCSViewFullCard())
            {
                uxReportGrid.Columns.FindByUniqueName("CardNumber").Visible = true;
                uxReportGrid.Columns.FindByUniqueName("PartialCardNumber").Visible = false;

                if (IsShowRoutingAccount)
                {
                    uxReportGrid.Columns.FindByUniqueName("RoutingAccountNumber").Visible = true;
                    uxReportGrid.Columns.FindByUniqueName("PartialRoutingACC").Visible = false;
                }
            }
            else
            {
                uxReportGrid.Columns.FindByUniqueName("CardNumber").Visible = false;
                uxReportGrid.Columns.FindByUniqueName("PartialCardNumber").Visible = true;

                if (IsShowRoutingAccount)
                {
                    uxReportGrid.Columns.FindByUniqueName("RoutingAccountNumber").Visible = false;
                    uxReportGrid.Columns.FindByUniqueName("PartialRoutingACC").Visible = true;
                }
            }

            if (GeneralFuncsLib.GetDataOfExtendedSetting("REMOVE_COLUMN_RTCB") == "true")
            {
                uxReportGrid.Columns.FindByUniqueName("ExpirationDate").Visible = false;
                uxReportGrid.Columns.FindByUniqueName("RequestType").Visible = false;
            }
            else
            {
                uxReportGrid.Columns.FindByUniqueName("ExpirationDate").Visible = true;
                uxReportGrid.Columns.FindByUniqueName("RequestType").Visible = true;
            }
        }
        else
        {
            uxPanelFaxNumber.Visible = false;
            uxReportGrid.Visible = false;
            uxDrilldownGrid.Visible = true;
        }
    }
    bool _isExporting = false;
    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, AS.Controls.Exporter.ExportConfig exportConfig)
    {
        _isExporting = true;
        uxReportGrid.Columns.FindByUniqueName("CardNumber").Visible = false;
        uxReportGrid.Columns.FindByUniqueName("PartialCardNumber").Visible = true;
        if (IsShowRoutingAccount)
        {
            uxReportGrid.Columns.FindByUniqueName("RoutingAccountNumber").Visible = false;
            uxReportGrid.Columns.FindByUniqueName("PartialRoutingACC").Visible = true;
        }
        if (!uxReportGrid.Visible)
        {
            //if (sender.ExportButtonType == AS.Controls.UserControls.UxExport.ExportType.Excel)
            //{
            uxDrilldownGrid.DisplayEntityNameWhenExport();
            if (uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderText == GetLocalResourceObject("Retrievals_aspx_cs_MerchantNumber").ToString()
            || uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderText == GetLocalResourceObject("Retrievals_aspx_cs_Merchant").ToString()
            || GeneralFuncsLib.IsMerchantMode(ReportFilter.CurrentValue.HierarchyMode))
            {
                uxDrilldownGrid.Columns.FindByUniqueName("EntityName").HeaderText = GetLocalResourceObject("Retrievals_aspx_cs_MerchantName").ToString();
                //}
            }
        }
        base.DoNeedExportConfig(sender, exportConfig);
        string fileName = ((UxExport)sender).GridTitle.ToString() + _GridTitle;

        exportConfig.FileName = GeneralFuncsLib.FormatFileName(fileName);
        exportConfig.ReportHeader = fileName;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        REPORT_HEADER = GetLocalResourceObject("Retrievals_aspx_cs_Retrievals").ToString() + " ";
        REPORT_HEADER_DETAIL = GetLocalResourceObject("Retrievals_aspx_cs_RetrievalDetail").ToString() + " ";
        IsBindDataOnLoad = true;
        if (SessionManager.ClientFrameInfo != string.Empty) _Target = SessionManager.ClientFrameInfo;
        if (IsSecureQueryString && !IsPostBack)
        {
            BindReportFilter();
        }
    }
    protected override void DoGridNeedDataSource(ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        if (sender == uxReportGrid && uxReportGrid.Visible)
        {
            OnDataBindControls(DataBindAction.BindRetrievalGrid, sender);
        }
        if (sender == uxDrilldownGrid && uxDrilldownGrid.Visible)
        {
            OnDataBindControls(DataBindAction.BindDrillDownGrid, sender);
        }
    }
    protected override void OnDataBindControls(Enum type, object sender)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();
        parameters.AddHierarchyFilterParams((ReportPage)this.Page);
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindRetrievalGrid:
                {
                    parameters.AddLoggedInUserPrimaryUserID();
                    parameters.Add("@Format", "Retrieval", DbType.String);
                    uxExportReportGrid.GridTitle = VeraCodeSolution.ValidateResponseData(REPORT_HEADER_DETAIL);
                    uxExportReportGrid.GridSubTitle = VeraCodeSolution.ValidateResponseData(_GridTitle);
                    ASGrid grid = (ASGrid)sender;
                    if (CheckCSViewFullCard())
                    {
                        parameters.AddDecryptDataParams("CardNumber", _isExporting);
                        if (IsShowRoutingAccount)
                        {
                            parameters.AddDecryptDataParams("RoutingAccountNumber", _isExporting);
                        }
                    }
                    parameters.AddLanguageID();
                    string spaName = "spa_GetMerchantRetrievalsChargebacks";
                    grid.DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(parameters) });
                }
                break;
            case DataBindAction.BindDrillDownGrid:
                {
                    parameters.Add("@ReportType", "Retrievals", DbType.String);
                    uxExportDrilldownTop.GridTitle = VeraCodeSolution.ValidateResponseData(REPORT_HEADER);
                    uxExportDrilldownTop.GridSubTitle = VeraCodeSolution.ValidateResponseData(_GridTitle);
                    ASGrid grid = (ASGrid)sender;
                    string spaName = WebSiteConstants.GET_REPORT_SPA_NAME;
                    grid.DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(parameters) }); uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderTooltip = GeneralFuncsLib.GetGridDrilldownColumnName(ReportFilter.CurrentValue.HierarchyMode, ReportFilter.CurrentValue.Value);
                    GeneralFuncsLib.VisibleHierarchyNameByFilteringMode(ReportFilter.CurrentValue.HierarchyMode, ReportFilter.CurrentValue.Value, uxDrilldownGrid, uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderText);
                }
                break;
        }
    }
    protected override void DoItemDataBound(ASGrid sender, GridItemEventArgs e)
    {
        if (sender == uxReportGrid)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = e.Item as GridDataItem;
                DataRowView dataRow = e.Item.DataItem as DataRowView;
                dataItem["ReasonCode"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["ReasonCodeDesc"].ToString());
                dataItem["Type"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["CardTypeDesc"].ToString());
                dataItem["ExpirationDate"].ToolTip = "MM/YY";
                dataItem["ReferenceNumber"].Text = VeraCodeSolution.DoVeraCode(string.Format("<a href='#' onclick=\"ShowPopupModal('{0}','auto');return false;\" >{1}</a>", "RetrievalsChargebacksDetail.aspx?" + this.BuildSecureQueryString("referenceNumber=" + dataRow["ReferenceNumber"].ToString() + "&rcType=r"), dataRow["ReferenceNumber"].ToString()));

                string urlCard = GeneralFuncsLib.BuildCardUrl((SecurePage)this.Page, dataRow["PartialCardNumber"].ToString(), dataRow["CardNumber"].ToString(), ReportFilter.CurrentValue.Value);

                string urlRouting = string.Empty;
                if (IsShowRoutingAccount)
                {
                    urlRouting = GeneralFuncsLib.BuildCardUrl((SecurePage)this.Page, dataRow["PartialRoutingACC"].ToString(), dataRow["RoutingAccountNumber"].ToString(), ReportFilter.CurrentValue.Value);
                }

                if (GeneralFuncsLib.HasIPForFullCard(this))
                {
                    dataItem["PartialCardNumber"].Text = VeraCodeSolution.DoVeraCode(String.Format(IMAGE, BuildUrlForFullCard(dataRow["RecordId"].ToString(), dataRow["IssueBank"].ToString(), dataRow["ReportDate"].ToString())) + VeraCodeSolution.DoVeraCode(urlCard + dataRow["PartialCardNumber"].ToString() + "</a>"));
                    if(IsShowRoutingAccount)
                        dataItem["PartialRoutingACC"].Text = VeraCodeSolution.DoVeraCode(String.Format(IMAGE, BuildUrlForFullCard(dataRow["RecordId"].ToString(), dataRow["IssueBank"].ToString(), dataRow["ReportDate"].ToString())) + VeraCodeSolution.DoVeraCode(urlRouting + dataRow["PartialRoutingACC"].ToString() + "</a>"));
                }
                else
                {
                    dataItem["CardNumber"].Text = VeraCodeSolution.DoVeraCode(urlCard + dataRow["CardNumber"].ToString() + "</a>");
                    dataItem["PartialCardNumber"].Text = VeraCodeSolution.DoVeraCode(urlCard + dataRow["PartialCardNumber"].ToString() + "</a>");
                    if (IsShowRoutingAccount)
                    {
                        dataItem["RoutingAccountNumber"].Text = VeraCodeSolution.DoVeraCode(urlRouting + dataRow["RoutingAccountNumber"].ToString() + "</a>");
                        dataItem["PartialRoutingACC"].Text = VeraCodeSolution.DoVeraCode(urlRouting + dataRow["PartialRoutingACC"].ToString() + "</a>");
                    }
                }
            }
        }
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
    }
    private string BuildUrlForFullCard(string encryptedCC, string issueBank, string ReportDate)
    {
        string queryString = BuildSecureQueryString("rt=Retrievals" + "&cn=" + Server.UrlEncode(encryptedCC) + "&issue=" + issueBank + "&reportdate=" + ReportDate);
        queryString = "FullCC.aspx?" + queryString;
        return queryString;
    }

    private void BindReportFilter()
    {
        string type = String.Empty;
        string isMobileRedirect = String.Empty;
        if (IsSecureQueryString)
        {
            type = SecureQueryString["type"];
            isMobileRedirect = SecureQueryString["mb"];
        }

        if (!String.IsNullOrEmpty(type))
        {
            int DateOptionMode;
            DateTime beginDate;
            DateTime endDate;
            DateTime now = DateTime.Now.Date;
            switch (type)
            {
                case TODAY:
                    {
                        DateOptionMode = 1;
                        beginDate = now;
                        endDate = now;
                        break;
                    }
                case MTD:
                    {
                        DateOptionMode = 2;
                        beginDate = new DateTime(now.Year, now.Month, 1);
                        endDate = now;
                        break;
                    }
                case YTD:
                    {
                        DateOptionMode = 3;
                        beginDate = new DateTime(now.Year, 1, 1);
                        endDate = now;
                        break;
                    }
                default: // rolling
                    {
                        DateOptionMode = 3;
                        beginDate = GetRolling12Months(DateTime.Today, true);
                        endDate = GetRolling12Months(DateTime.Today, false);
                        break;
                    }
            }

            this.ReportFilter.CurrentValue.DateOption = (AS.Web.UI.Controls.DateOptionMode)DateOptionMode;
            if (DateOptionMode == 2)
                this.ReportFilter.CurrentValue.DateOptionValue.From = endDate;
            else
                this.ReportFilter.CurrentValue.DateOptionValue.From = beginDate;
            this.ReportFilter.CurrentValue.DateOptionValue.To = endDate;
        }

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

    private DateTime GetRolling12Months(DateTime dtDate, bool isBegin)
    {
        DateTime lstDayOfMonth = new DateTime(dtDate.Year, dtDate.Month, DateTime.DaysInMonth(dtDate.Year, dtDate.Month));
        if (isBegin)
        {
            if (dtDate == lstDayOfMonth)
            {
                DateTime bDate = dtDate.AddYears(-1).AddMonths(1);
                bDate = new DateTime(bDate.Year, bDate.Month, 1);
                return bDate;
            }
            else
            {
                DateTime bDate = dtDate.AddYears(-1);
                bDate = new DateTime(bDate.Year, bDate.Month, 1);
                return bDate;
            }
        }
        else
        {
            if (dtDate == lstDayOfMonth)
            {
                return dtDate;
            }
            else
            {
                DateTime eDate = dtDate.AddMonths(-1);
                eDate = new DateTime(eDate.Year, eDate.Month, DateTime.DaysInMonth(eDate.Year, eDate.Month));
                return eDate;
            }
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        uxDrilldownGrid.Columns.FindByUniqueName("EntityName").Visible = GeneralFuncsLib.SHOW_PROCESSINGDATA_ENTITYNAME();
        base.OnPreRender(e);
    }

}
