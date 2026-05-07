using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Controls.UserControls;
using AS.Web.Business;
using System;
using System.Data;
using System.Linq;
using Telerik.Web.UI;
using GeneralFuntionBusiness = AS.Web.Business.General.GeneralFuncsLib;

[PagePermission("ChbRpt,MSChbRpt")]
public partial class gen_Chargebacks : ReportPage
{
    #region Enums
    enum DataBindAction
    {
        BindDrilldownGrid
        , BindReportGrid
    }
    #endregion
    private const string TODAY = "Today";
    private const string MTD = "MTD";
    private const string YTD = "YTD";

    string _ChargebackIntruderQuery = string.Empty;
    private string _Target = "_parent";
    private const string IMAGE = "<a class=\"image-link\" href=\"#\" onclick=\" return ShowPopupModal('{0}','auto');\"><img src='res/img/information.png' border=\"0\"/></a>&nbsp; &nbsp;";
    private string ChargebackIntruderQuery
    {
        get
        {
            if (_ChargebackIntruderQuery == string.Empty) _ChargebackIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(uxReportGrid.ID, new string[] { "ReferenceNumber" });
            return _ChargebackIntruderQuery;
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
    string _CBSequenceIntruderQuery = string.Empty;
    private string CBSequenceIntruderQuery
    {
        get
        {
            if (_CBSequenceIntruderQuery == string.Empty) _CBSequenceIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(uxReportGrid.ID, new string[] { "CBSeqNo" });
            return _CBSequenceIntruderQuery;
        }
    }
    string REPORT_HEADER_DETAIL = string.Empty;
    string REPORT_HEADER = string.Empty;

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
                uxReportGrid.Columns.FindByUniqueName("CBType").Visible = false;
                uxReportGrid.Columns.FindByUniqueName("CBTypeDesc").Visible = false;
                uxReportGrid.Columns.FindByUniqueName("Disposition").Visible = false;
                uxReportGrid.Columns.FindByUniqueName("CBSeqNo").Visible = false;
                uxReportGrid.Columns.FindByUniqueName("RepresentedCBAmount").Visible = false;
                //uxReportGrid.XOverFlowable = false;
            }
            else
            {
                uxReportGrid.Columns.FindByUniqueName("ExpirationDate").Visible = true;
                uxReportGrid.Columns.FindByUniqueName("CBType").Visible = true;
                uxReportGrid.Columns.FindByUniqueName("CBTypeDesc").Visible = true;
                uxReportGrid.Columns.FindByUniqueName("Disposition").Visible = true;
                uxReportGrid.Columns.FindByUniqueName("CBSeqNo").Visible = true;
                uxReportGrid.Columns.FindByUniqueName("RepresentedCBAmount").Visible = true;
            }
        }
        else
        {
            uxPanelFaxNumber.Visible = false;
            uxReportGrid.Visible = false;
            uxDrilldownGrid.Visible = true;
        }

        if (GeneralFuncsLib.GetDataOfExtendedSetting("SHOW_RDR").ToLower().Equals("true"))
        {
            SetGridRDRColumnsVisible(uxDrilldownGrid, "Chargebacks");
            SetGridRDRColumnsVisible(uxReportGrid, "ChargebacksDetail");
        }
    }
    bool _isExporting = false;
    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, AS.Controls.Exporter.ExportConfig exportConfig)
    {
        //46652 - AW Multi-currency Transaction Display
        GeneralFuncsLib.ShowHideAWTransactionDetail(uxReportGrid);

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
            uxDrilldownGrid.DisplayEntityNameWhenExport();
            if (uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderText == "Merchant ID"
            || uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderText == "Merchant"
            || GeneralFuncsLib.IsMerchantMode(ReportFilter.CurrentValue.HierarchyMode))
            {
                uxDrilldownGrid.Columns.FindByUniqueName("EntityName").HeaderText = "Merchant Name";
            }
        }
        base.DoNeedExportConfig(sender, exportConfig);

        string fileName = ((UxExport)sender).GridTitle.ToString() + _GridTitle;

        exportConfig.FileName = GeneralFuncsLib.FormatFileName(fileName);
        exportConfig.ReportHeader = fileName;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        REPORT_HEADER_DETAIL = GetLocalResourceObject("Chargebacks_aspx_cs_ChargebackDetail").ToString() + " ";
        REPORT_HEADER = GetLocalResourceObject("Chargebacks_aspx_cs_Chargeback").ToString() + " ";
        IsBindDataOnLoad = true;
        if (SessionManager.ClientFrameInfo != string.Empty) _Target = SessionManager.ClientFrameInfo;
        if (IsSecureQueryString && !IsPostBack)
        {
            BindReportFilter();
        }
        //46652 - AW Multi-currency Transaction Display
        GeneralFuncsLib.ShowHideAWTransactionDetail(uxReportGrid);
    }

    protected override void OnPreRender(EventArgs e)
    {
        uxDrilldownGrid.Columns.FindByUniqueName("EntityName").Visible = GeneralFuncsLib.SHOW_PROCESSINGDATA_ENTITYNAME();
        base.OnPreRender(e);
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        ASGrid grid = (ASGrid)sender;
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();
        parameters.AddHierarchyFilterParams((ReportPage)this.Page);
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindDrilldownGrid:
                {
                    parameters.Add("@ReportType", "Chargebacks", DbType.String);
                    uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderText = _GridDrillDownHeader;
                    uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderTooltip = _GridDrillDownHeader;

                    string spaName = WebSiteConstants.GET_REPORT_SPA_NAME;
                    grid.DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(parameters) });
                    uxExportDrilldownTop.GridTitle = VeraCodeSolution.ValidateResponseData(REPORT_HEADER);
                    uxExportDrilldownTop.GridSubTitle = VeraCodeSolution.ValidateResponseData(_GridTitle);
                    GeneralFuncsLib.VisibleHierarchyNameByFilteringMode(ReportFilter.CurrentValue.HierarchyMode, ReportFilter.CurrentValue.Value, uxDrilldownGrid, uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderText);
                }
                break;
            case DataBindAction.BindReportGrid:
                {
                    parameters.AddLoggedInUserPrimaryUserID();
                    parameters.Add("@Format", "Chargeback", DbType.String);
                    if (CheckCSViewFullCard() || GeneralFuncsLib.HasIPForFullCard((SecurePage)this))
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
                    uxExportReportGrid.GridTitle = VeraCodeSolution.ValidateResponseData(REPORT_HEADER_DETAIL);
                    uxExportReportGrid.GridSubTitle = VeraCodeSolution.ValidateResponseData(_GridTitle);
                }
                break;
        }
    }
    protected override void DoGridNeedDataSource(ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        if (sender == uxReportGrid)
        {
            OnDataBindControls(DataBindAction.BindReportGrid, uxReportGrid);
        }
        if (sender == uxDrilldownGrid)
        {
            OnDataBindControls(DataBindAction.BindDrilldownGrid, uxDrilldownGrid);
        }

    }
    protected override void DoItemDataBound(ASGrid sender, GridItemEventArgs e)
    {
        if (sender == uxReportGrid)
        {
            if (uxReportGrid.Visible && GeneralFuncsLib.GetDataOfExtendedSetting("SHOW_RDR").ToLower().Equals("true"))
            {
                SetTooltipForReportGrid();
            }

            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = e.Item as GridDataItem;
                DataRowView dataRow = e.Item.DataItem as DataRowView;
                dataItem["ReasonCode"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["ReasonCodeDesc"].ToString());
                dataItem["CardType"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["CardTypeDesc"].ToString());
                dataItem["ExpirationDate"].ToolTip = "MM/YY";
                if (dataRow["ReferenceNumber"].ToString().Trim() != String.Empty)
                {
                    dataItem["ReferenceNumber"].Text = VeraCodeSolution.DoVeraCode(string.Format("<a href=\"#\" onclick=\"ShowPopupModal('{0}','auto');return false;\" >{1}</a>", "RetrievalsChargebacksDetail.aspx?" 
                        + this.BuildSecureQueryString("referenceNumber=" + dataRow["ReferenceNumber"].ToString() 
                        + "&rcType=c" + "&sourceMerchant=" + ReportFilter.CurrentValue.Value
                        + "&sourceMerchantMode=" + ReportFilter.CurrentValue.HierarchyMode + ChargebackIntruderQuery), dataRow["ReferenceNumber"].ToString()));
                }

                if (SessionManager.CurrentUser.ASClient != 23)
                {
                    if (dataRow["CBSeqNo"].ToString().Trim() != String.Empty)
                        dataItem["CBSeqNo"].Text = VeraCodeSolution.DoVeraCode(string.Format("<a href=\"#\" onclick=\"ShowPopupModal('{0}','auto');return false;\" >{1}</a>", "CBSequenceDetail.aspx?" + this.BuildSecureQueryString("CBSequenceNumber=" + dataRow["CBSeqNo"].ToString() + "&rptDate=" + dataRow["ReportDate"] + "&recid=" + dataRow["RecordID"] + CBSequenceIntruderQuery), dataRow["CBSeqNo"].ToString()));
                }

                string urlCard = GeneralFuncsLib.BuildCardUrl((SecurePage)this.Page, dataRow["PartialCardNumber"].ToString(), dataRow["CardNumber"].ToString(), ReportFilter.CurrentValue.Value);

                string urlRouting = string.Empty;
                if (IsShowRoutingAccount)
                {
                    urlRouting = GeneralFuncsLib.BuildCardUrl((SecurePage)this.Page, dataRow["PartialRoutingACC"].ToString(), dataRow["RoutingAccountNumber"].ToString(), ReportFilter.CurrentValue.Value);
                }

                if (GeneralFuncsLib.HasIPForFullCard(this))
                {
                    dataItem["PartialCardNumber"].Text = VeraCodeSolution.DoVeraCode(String.Format(IMAGE, BuildUrlForFullCard(dataRow["RecordId"].ToString(), dataRow["IssueBank"].ToString(), dataRow["ReportDate"].ToString())) + VeraCodeSolution.DoVeraCode(urlCard + dataRow["PartialCardNumber"].ToString() + "</a>"));

                    if (IsShowRoutingAccount)
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
            SetTooltipForGrid();
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

    private void SetTooltipForGrid()
    {
        SetTooltipForColumn(uxDrilldownGrid, "RepresentedCBCount", "ASGridBoundColumnResource7", "OtherChargebacksCount");
        SetTooltipForColumn(uxDrilldownGrid, "ChargebacksCount", "ASGridBoundColumnResource8", "FirstTimeCBCount");
        SetTooltipForColumn(uxDrilldownGrid, "ChargebacksAmount", "ASGridBoundColumnResource9", "FirstTimeCBAmount");
        if (GeneralFuncsLib.GetDataOfExtendedSetting("SHOW_RDR").ToLower().Equals("true"))
        {
            SetTooltipForColumn(uxDrilldownGrid, "FirstChargebackRDRCount", "ASGridBoundColumnResource39", "FirstChargebackRDRCount");
            SetTooltipForColumn(uxDrilldownGrid, "FirstChargebackRDRAmount", "ASGridBoundColumnResource40", "FirstChargebackRDRAmount");
            SetTooltipForColumn(uxDrilldownGrid, "PostChargebackRDRCount", "ASGridBoundColumnResource41", "PostChargebackRDRCount");
            SetTooltipForColumn(uxDrilldownGrid, "PostChargebackRDRAmount", "ASGridBoundColumnResource42", "PostChargebackRDRAmount");
        }
    }

    private void SetTooltipForColumn(ASGrid grid, string colName, string resourceKey, string resourceKeyDescription)
    {
        var columnTarget = grid.MasterTableView.Columns.FindByUniqueName(colName);
        string columnIdCBCount = GetLocalResourceObject(resourceKey + ".HeaderTooltip").ToString();
        var toolTipPosition = ToolTipPosition.TopRight;
        if (colName == "ChargebacksAmount")
            toolTipPosition = ToolTipPosition.TopLeft;

        if (!columnTarget.HeaderText.Contains(string.Format("id='{0}'", columnIdCBCount)))
        {
            string headerTextColumn = GetLocalResourceObject(resourceKey + ".HeaderText").ToString();
            string columnToolTip = GetLocalResourceObject(resourceKeyDescription + ".HeaderDescription").ToString();
            RM_MCF_GeneralFuncsLib.GenerateTooltipHeaderDefaulColumnID(columnTarget, headerTextColumn, grid, columnIdCBCount, columnToolTip, toolTipPosition);
            columnTarget.HeaderTooltip = " ";
        }
    }

    private string BuildUrlForFullCard(string encryptedCC, string issueBank, string ReportDate)
    {
        string queryString = BuildSecureQueryString("rt=Chargebacks" + "&cn=" + Server.UrlEncode(encryptedCC) + "&issue=" + issueBank + "&reportdate=" + ReportDate);
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

    private void SetGridRDRColumnsVisible(ASGrid grid, string chargebackType)
    {
        if (grid.Visible)
        {
            var rdrColumnNames = GeneralFuntionBusiness.GetRDRColumnNames(chargebackType);
            foreach (var columnName in rdrColumnNames)
            {
                var column = grid.Columns.FindByUniqueName(columnName);
                if (column != null)
                {
                    column.Visible = true;
                }
            }
        }
    }

    private void SetTooltipForReportGrid()
    {
        SetTooltipForColumn(uxReportGrid, "FirstChargebackRDRAmount", "ASGridBoundColumnResource40", "FirstChargebackRDRAmount");
        SetTooltipForColumn(uxReportGrid, "PostChargebackRDRAmount", "ASGridBoundColumnResource42", "PostChargebackRDRAmount");
    }
}
