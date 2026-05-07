using AS.Common;
using AS.Common.DBManager;
using AS.Common.WebUI;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Web.Business;
using AS.Web.UI.AppCode.General;
using System;
using System.Data;
using Telerik.Web.UI;
using GeneralFuntionBusiness = AS.Web.Business.General.GeneralFuncsLib;

[PagePermission("RetChbRpt,MSRetChbRpt")]
public partial class gen_Retrievals_Chargebacks : ReportPage
{
    #region Enums
    enum DataBindAction
    {
        BindRetrievalGrid,
        BindChargebackGrid,
        BindDrillDownGrid
    }
    #endregion
    #region constant
    string REPORT_HEADER_HIERACHY = string.Empty;
    string REPORT_HEADER_RETRIEVAL = string.Empty;
    string REPORT_HEADER_CHARGEBACK = string.Empty;

    #endregion

    #region Properties
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
    private const string IMAGE = "<a class=\"image-link\" href=\"#\" style=\"cursor:pointer\" onclick=\" return ShowPopupModal('{0}','auto');\"><img src='res/img/information.png' border=\"0\"/></a>&nbsp; &nbsp;";

    string _RetrievalIntruderQuery = string.Empty;
    string _ChargebackIntruderQuery = string.Empty;
    string _CBSequenceIntruderQuery = string.Empty;
    private string RetrievalIntruderQuery
    {
        get
        {
            if (_RetrievalIntruderQuery == string.Empty) _RetrievalIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(uxReportGrid.ID, new string[] { "ReferenceNumber" });
            return _RetrievalIntruderQuery;
        }
    }

    private string ChargebackIntruderQuery
    {
        get
        {
            if (_ChargebackIntruderQuery == string.Empty) _ChargebackIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(uxChargebackReportGrid.ID, new string[] { "ReferenceNumber" });
            return _ChargebackIntruderQuery;
        }
    }
    private string CBSequenceIntruderQuery
    {
        get
        {
            if (_CBSequenceIntruderQuery == string.Empty) _CBSequenceIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(uxChargebackReportGrid.ID, new string[] { "CBSeqNo" });
            return _CBSequenceIntruderQuery;
        }
    }
    string _CardSearchIntruderQuery_Retrieval = string.Empty;
    private string CardSearchIntruderQuery_Retrieval
    {
        get
        {

            if (_CardSearchIntruderQuery_Retrieval == string.Empty)
            {
                _CardSearchIntruderQuery_Retrieval = GeneralFuncsLib.BuildIntruderInfoForQueryStr(uxReportGrid.ID, new string[] { "CardNumber" });
            }
            return _CardSearchIntruderQuery_Retrieval;
        }
    }

    string _CardSearchIntruderQuery_Chargeback = string.Empty;
    private string CardSearchIntruderQuery_Chargeback
    {
        get
        {

            if (_CardSearchIntruderQuery_Chargeback == string.Empty)
            {
                _CardSearchIntruderQuery_Chargeback = GeneralFuncsLib.BuildIntruderInfoForQueryStr(uxChargebackReportGrid.ID, new string[] { "CardNumber" });
            }
            return _CardSearchIntruderQuery_Chargeback;
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

    #endregion

    protected override void PageInitialize()
    {
        if (IsIntruderDetected) return;

        this.ExporterIDs.Add("uxExporterDrilldownTop");
        this.ExporterIDs.Add("uxExportChargebackTop");
        this.ExporterIDs.Add("uxExporterRetrievalTop");
        this.GridIDs.Add("uxChargebackReportGrid");
        this.GridIDs.Add("uxReportGrid");
        this.ReportChart = (IChart)uxChart;
        base.PageInitialize();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        REPORT_HEADER_HIERACHY = GetLocalResourceObject("Retrievals_Chargebacks_aspx_cs_RetrievalChargeback").ToString() + " ";
        REPORT_HEADER_RETRIEVAL = GetLocalResourceObject("Retrievals_Chargebacks_aspx_cs_RetrievalDetail").ToString() + " ";
        REPORT_HEADER_CHARGEBACK = GetLocalResourceObject("Retrievals_Chargebacks_aspx_cs_ChargebackDetail").ToString() + " ";
        if (!IsPostBack)
            uxChart.BindChart();
        IsBindDataOnLoad = true;
        if (SessionManager.ClientFrameInfo != string.Empty) _Target = SessionManager.ClientFrameInfo;

        //46652 - AW Multi-currency Transaction Display
        GeneralFuncsLib.ShowHideAWTransactionDetail(uxChargebackReportGrid);
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
            uxChargebackReportGrid.Visible = true;
            uxDrilldownGrid.Visible = false;
            if (CheckCSViewFullCard())
            {
                uxReportGrid.Columns.FindByUniqueName("CardNumber").Visible = true;
                uxReportGrid.Columns.FindByUniqueName("PartialCardNumber").Visible = false;
                uxChargebackReportGrid.Columns.FindByUniqueName("CardNumber").Visible = true;
                uxChargebackReportGrid.Columns.FindByUniqueName("PartialCardNumber").Visible = false;

                if (IsShowRoutingAccount)
                {
                    uxReportGrid.Columns.FindByUniqueName("RoutingAccountNumber").Visible = true;
                    uxReportGrid.Columns.FindByUniqueName("PartialRoutingACC").Visible = false;
                    uxChargebackReportGrid.Columns.FindByUniqueName("RoutingAccountNumber").Visible = true;
                    uxChargebackReportGrid.Columns.FindByUniqueName("PartialRoutingACC").Visible = false;

                }
            }
            else
            {
                uxReportGrid.Columns.FindByUniqueName("CardNumber").Visible = false;
                uxReportGrid.Columns.FindByUniqueName("PartialCardNumber").Visible = true;
                uxChargebackReportGrid.Columns.FindByUniqueName("CardNumber").Visible = false;
                uxChargebackReportGrid.Columns.FindByUniqueName("PartialCardNumber").Visible = true;

                if (IsShowRoutingAccount)
                {
                    uxReportGrid.Columns.FindByUniqueName("RoutingAccountNumber").Visible = false;
                    uxReportGrid.Columns.FindByUniqueName("PartialRoutingACC").Visible = true;
                    uxChargebackReportGrid.Columns.FindByUniqueName("RoutingAccountNumber").Visible = false;
                    uxChargebackReportGrid.Columns.FindByUniqueName("PartialRoutingACC").Visible = true;
                }
            }

            if (GeneralFuncsLib.GetDataOfExtendedSetting("REMOVE_COLUMN_RTCB") == "true")
            {
                uxReportGrid.Columns.FindByUniqueName("ExpirationDate").Visible = false;
                uxReportGrid.Columns.FindByUniqueName("RequestType").Visible = false;
                uxChargebackReportGrid.Columns.FindByUniqueName("ExpirationDate").Visible = false;
                uxChargebackReportGrid.Columns.FindByUniqueName("CBType").Visible = false;
                uxChargebackReportGrid.Columns.FindByUniqueName("CBTypeDesc").Visible = false;
                uxChargebackReportGrid.Columns.FindByUniqueName("Disposition").Visible = false;
                uxChargebackReportGrid.Columns.FindByUniqueName("CBSeqNo").Visible = false;
                uxChargebackReportGrid.Columns.FindByUniqueName("RepresentedCBAmount").Visible = false;
                //uxChargebackReportGrid.XOverFlowable = false;
            }
            else
            {
                uxReportGrid.Columns.FindByUniqueName("ExpirationDate").Visible = true;
                uxReportGrid.Columns.FindByUniqueName("RequestType").Visible = true;
                uxChargebackReportGrid.Columns.FindByUniqueName("ExpirationDate").Visible = true;
                uxChargebackReportGrid.Columns.FindByUniqueName("CBType").Visible = true;
                uxChargebackReportGrid.Columns.FindByUniqueName("CBTypeDesc").Visible = true;
                uxChargebackReportGrid.Columns.FindByUniqueName("Disposition").Visible = true;
                uxChargebackReportGrid.Columns.FindByUniqueName("CBSeqNo").Visible = true;
                uxChargebackReportGrid.Columns.FindByUniqueName("RepresentedCBAmount").Visible = true;

            }
        }
        else
        {
            uxPanelFaxNumber.Visible = false;
            uxReportGrid.Visible = false;
            uxChargebackReportGrid.Visible = false;
            uxDrilldownGrid.Visible = true;
        }

        if (GeneralFuncsLib.GetDataOfExtendedSetting("SHOW_RDR").ToLower().Equals("true"))
        {
            SetGridRDRColumnsVisible(uxDrilldownGrid, "Chargebacks");
            SetGridRDRColumnsVisible(uxChargebackReportGrid, "ChargebacksDetail");
        }

    }
    bool _isExporting = false;
    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, AS.Controls.Exporter.ExportConfig exportConfig)
    {
        _isExporting = true;
        uxReportGrid.Columns.FindByUniqueName("CardNumber").Visible = false;
        uxReportGrid.Columns.FindByUniqueName("PartialCardNumber").Visible = true;
        uxChargebackReportGrid.Columns.FindByUniqueName("CardNumber").Visible = false;
        uxChargebackReportGrid.Columns.FindByUniqueName("PartialCardNumber").Visible = true;

        if (IsShowRoutingAccount)
        {
            uxReportGrid.Columns.FindByUniqueName("RoutingAccountNumber").Visible = false;
            uxReportGrid.Columns.FindByUniqueName("PartialRoutingACC").Visible = true;
            uxChargebackReportGrid.Columns.FindByUniqueName("RoutingAccountNumber").Visible = false;
            uxChargebackReportGrid.Columns.FindByUniqueName("PartialRoutingACC").Visible = true;
        }

        if (uxDrilldownGrid.Visible)
        {
            //if (sender.ExportButtonType == AS.Controls.UserControls.UxExport.ExportType.Excel)
            //{
            uxDrilldownGrid.DisplayEntityNameWhenExport();
            if (uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderText == GetLocalResourceObject("Retrievals_Chargebacks_aspx_cs_MerchantNumber").ToString()
            || uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderText == GetLocalResourceObject("Retrievals_Chargebacks_aspx_cs_Merchant").ToString()
            || GeneralFuncsLib.IsMerchantMode(ReportFilter.CurrentValue.HierarchyMode))
            {
                uxDrilldownGrid.Columns.FindByUniqueName("EntityName").HeaderText = GetLocalResourceObject("Retrievals_Chargebacks_aspx_cs_MerchantName").ToString();
            }
            //}
        }

        string gridTitle = ((AS.Controls.UserControls.UxExport)sender).GridTitle;
        ((AS.Controls.UserControls.UxExport)sender).GridTitle = ASRadControlHelper.HandleHtmlEncodeDecode(gridTitle, true);

        string gridHeader = ((AS.Controls.UserControls.UxExport)sender).GridHeader;
        ((AS.Controls.UserControls.UxExport)sender).GridHeader = ASRadControlHelper.HandleHtmlEncodeDecode(gridHeader, true);

        string gridSubTitle = ((AS.Controls.UserControls.UxExport)sender).GridSubTitle;
        ((AS.Controls.UserControls.UxExport)sender).GridSubTitle = ASRadControlHelper.HandleHtmlEncodeDecode(gridSubTitle, true);

        base.DoNeedExportConfig(sender, exportConfig);

        exportConfig.FileName = GeneralFuncsLib.FormatFileName(((AS.Controls.UserControls.UxExport)sender).GridHeader.ToString());
    }
    protected override void OnDataBindControls(Enum type, object sender)
    {
        ASGrid grid = (ASGrid)sender;
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();
        parameters.AddHierarchyFilterParams((ReportPage)this.Page);

        switch ((DataBindAction)type)
        {
            case DataBindAction.BindRetrievalGrid:
                {
                    parameters.AddLoggedInUserPrimaryUserID();
                    parameters.Add("@Format", "Retrieval", DbType.String);
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

                    //set grif title
                    uxExporterRetrievalTop.GridHeader = VeraCodeSolution.ValidateResponseData(REPORT_HEADER_RETRIEVAL + _GridTitle);
                    uxExporterRetrievalTop.GridTitle = VeraCodeSolution.ValidateResponseData(REPORT_HEADER_RETRIEVAL);
                    uxExporterRetrievalTop.GridSubTitle = VeraCodeSolution.ValidateResponseData(_GridTitle);
                }
                break;

            case DataBindAction.BindChargebackGrid:
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

                    //set grif title
                    uxExportChargebackTop.GridHeader = VeraCodeSolution.ValidateResponseData(REPORT_HEADER_CHARGEBACK + _GridTitle);
                    uxExportChargebackTop.GridTitle = VeraCodeSolution.ValidateResponseData(REPORT_HEADER_CHARGEBACK);
                    uxExportChargebackTop.GridSubTitle = VeraCodeSolution.ValidateResponseData(_GridTitle);
                }
                break;

            case DataBindAction.BindDrillDownGrid:
                {
                    parameters.Add("@ReportType", "RetrievalsChargebacks", DbType.String);
                    string spaName = WebSiteConstants.GET_REPORT_SPA_NAME;

                    uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderText = GeneralFuncsLib.GetGridDrilldownColumnName(ReportFilter.CurrentValue.HierarchyMode, ReportFilter.CurrentValue.Value);
                    uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderTooltip = GeneralFuncsLib.GetGridDrilldownColumnName(ReportFilter.CurrentValue.HierarchyMode, ReportFilter.CurrentValue.Value);
                    grid.DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(parameters) });

                    //set grif title
                    uxExporterDrilldownTop.GridHeader = VeraCodeSolution.ValidateResponseData(REPORT_HEADER_HIERACHY + _GridTitle);
                    uxExporterDrilldownTop.GridTitle = VeraCodeSolution.ValidateResponseData(REPORT_HEADER_HIERACHY);
                    uxExporterDrilldownTop.GridSubTitle = VeraCodeSolution.ValidateResponseData(_GridTitle);
                    GeneralFuncsLib.VisibleHierarchyNameByFilteringMode(ReportFilter.CurrentValue.HierarchyMode, ReportFilter.CurrentValue.Value, uxDrilldownGrid, uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderText);
                }
                break;
        }
    }
    protected override void DoGridNeedDataSource(ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        if (sender == uxReportGrid)
        {
            OnDataBindControls(DataBindAction.BindRetrievalGrid, uxReportGrid);
        }
        if (sender == uxChargebackReportGrid)
        {
            OnDataBindControls(DataBindAction.BindChargebackGrid, uxChargebackReportGrid);
        }
        if (sender == uxDrilldownGrid)
        {
            OnDataBindControls(DataBindAction.BindDrillDownGrid, uxDrilldownGrid);
        }
    }
    protected override void DoItemDataBound(ASGrid sender, GridItemEventArgs e)
    {
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

        if (sender == uxChargebackReportGrid && uxChargebackReportGrid.Visible && GeneralFuncsLib.GetDataOfExtendedSetting("SHOW_RDR").ToLower().Equals("true"))
        {
            SetTooltipForChargebackReportGrid();
        }

        if (e.Item is GridDataItem)
        {
            if ((sender == uxChargebackReportGrid || sender == uxReportGrid) && sender.Visible) // for Merchant View in Retrievals and Chargebacks grid
            {
                GridDataItem dataItem = e.Item as GridDataItem;
                DataRowView rowView = e.Item.DataItem as DataRowView;
                dataItem["CardType"].ToolTip = VeraCodeSolution.DoVeraCode(rowView["CardTypeDesc"].ToString());
                dataItem["ReasonCode"].ToolTip = VeraCodeSolution.DoVeraCode(rowView["ReasonCodeDesc"].ToString());
                dataItem["ExpirationDate"].ToolTip = "MM/YY";
                if (sender == uxChargebackReportGrid)
                {
                    dataItem["ReferenceNumber"].Text = VeraCodeSolution.DoVeraCode(string.Format("<a href='#' onclick=\"ShowPopupModal('{0}','auto');return false;\" >{1}</a>", "RetrievalsChargebacksDetail.aspx?" + this.BuildSecureQueryString("referenceNumber=" + rowView["ReferenceNumber"].ToString() + "&rcType=c" + ChargebackIntruderQuery), rowView["ReferenceNumber"].ToString()));
                    if (SessionManager.CurrentUser.ASClient != 23)
                    {
                        if (!string.IsNullOrEmpty(rowView["CBSeqNo"].ToString()))
                            dataItem["CBSeqNo"].Text = VeraCodeSolution.DoVeraCode(string.Format("<a href='#' onclick=\"ShowPopupModal('{0}','auto');return false;\" >{1}</a>", "CBSequenceDetail.aspx?" + this.BuildSecureQueryString("CBSequenceNumber=" + rowView["CBSeqNo"].ToString() + "&rptDate=" + rowView["ReportDate"] + "&recid=" + rowView["RecordID"] + CBSequenceIntruderQuery), rowView["CBSeqNo"].ToString()));
                    }
                }
                else
                {
                    dataItem["ReferenceNumber"].Text = VeraCodeSolution.DoVeraCode(string.Format("<a href='#' onclick=\"ShowPopupModal('{0}','auto');return false;\" >{1}</a>", "RetrievalsChargebacksDetail.aspx?" + this.BuildSecureQueryString("referenceNumber=" + rowView["ReferenceNumber"].ToString() + "&rcType=r" + RetrievalIntruderQuery), rowView["ReferenceNumber"].ToString()));
                }

                string urlCard = GeneralFuncsLib.BuildCardUrl((SecurePage)this.Page, rowView["PartialCardNumber"].ToString(), rowView["CardNumber"].ToString(), ReportFilter.CurrentValue.Value);

                string urlRouting = string.Empty;
                if (IsShowRoutingAccount)
                {
                    urlRouting = GeneralFuncsLib.BuildCardUrl((SecurePage)this.Page, rowView["PartialRoutingACC"].ToString(), rowView["RoutingAccountNumber"].ToString(), ReportFilter.CurrentValue.Value);
                }

                if (sender == uxChargebackReportGrid)
                {
                    if (GeneralFuncsLib.HasIPForFullCard(this))
                    {
                        dataItem["PartialCardNumber"].Text = VeraCodeSolution.DoVeraCode(String.Format(IMAGE, BuildUrlForFullCard("Chargebacks", rowView["RecordId"].ToString(), rowView["IssueBank"].ToString(), rowView["ReportDate"].ToString())) + VeraCodeSolution.DoVeraCode(urlCard + rowView["PartialCardNumber"].ToString() + "</a>"));
                        if (IsShowRoutingAccount)
                            dataItem["PartialRoutingACC"].Text = VeraCodeSolution.DoVeraCode(String.Format(IMAGE, BuildUrlForFullCard("Chargebacks", rowView["RecordId"].ToString(), rowView["IssueBank"].ToString(), rowView["ReportDate"].ToString())) + VeraCodeSolution.DoVeraCode(urlRouting + rowView["PartialRoutingACC"].ToString() + "</a>"));
                    }
                    else
                    {
                        dataItem["CardNumber"].Text = VeraCodeSolution.DoVeraCode(urlCard + rowView["CardNumber"].ToString() + "</a>");
                        dataItem["PartialCardNumber"].Text = VeraCodeSolution.DoVeraCode(urlCard + rowView["PartialCardNumber"].ToString() + "</a>");
                        if (IsShowRoutingAccount)
                        {
                            dataItem["RoutingAccountNumber"].Text = VeraCodeSolution.DoVeraCode(urlRouting + rowView["RoutingAccountNumber"].ToString() + "</a>");
                            dataItem["PartialRoutingACC"].Text = VeraCodeSolution.DoVeraCode(urlRouting + rowView["PartialRoutingACC"].ToString() + "</a>");
                        }
                    }
                }
                else
                {
                    if (GeneralFuncsLib.HasIPForFullCard(this))
                    {
                        dataItem["PartialCardNumber"].Text = VeraCodeSolution.DoVeraCode(
                            String.Format(IMAGE, BuildUrlForFullCard("Retrievals", rowView["RecordId"].ToString(), rowView["IssueBank"].ToString(), rowView["ReportDate"].ToString())) + VeraCodeSolution.DoVeraCode(urlCard + rowView["PartialCardNumber"].ToString() + "</a>"));

                        if (IsShowRoutingAccount)
                        {
                            dataItem["PartialRoutingACC"].Text = VeraCodeSolution.DoVeraCode(
                            String.Format(IMAGE, BuildUrlForFullCard("Retrievals", rowView["RecordId"].ToString(), rowView["IssueBank"].ToString(), rowView["ReportDate"].ToString())) + VeraCodeSolution.DoVeraCode(urlRouting + rowView["PartialRoutingACC"].ToString() + "</a>"));
                        }
                    }
                    else
                    {
                        dataItem["CardNumber"].Text = VeraCodeSolution.DoVeraCode(urlCard + rowView["CardNumber"].ToString() + "</a>");
                        dataItem["PartialCardNumber"].Text = VeraCodeSolution.DoVeraCode(urlCard + rowView["PartialCardNumber"].ToString() + "</a>");
                        if (IsShowRoutingAccount)
                        {
                            dataItem["RoutingAccountNumber"].Text = VeraCodeSolution.DoVeraCode(urlRouting + rowView["RoutingAccountNumber"].ToString() + "</a>");
                            dataItem["PartialRoutingACC"].Text = VeraCodeSolution.DoVeraCode(urlRouting + rowView["PartialRoutingACC"].ToString() + "</a>");
                        }
                    }
                }

            }
        }
    }
    private void SetTooltipForGrid()
    {
        SetTooltipForColumn(uxDrilldownGrid, "RepresentedCBCount", "ASGridBoundColumnResource9", "OtherChargebacksCount");
        SetTooltipForColumn(uxDrilldownGrid, "ChargebacksCount", "ASGridBoundColumnResource10", "FirstTimeCBCount");
        SetTooltipForColumn(uxDrilldownGrid, "ChargebacksAmount", "ASGridBoundColumnResource11", "FirstTimeCBAmount");
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
    private string BuildUrlForFullCard(string reportType, string encryptedCC, string issueBank, string ReportDate)
    {
        string queryString = BuildSecureQueryString("rt=" + reportType + "&cn=" + Server.UrlEncode(encryptedCC) + "&issue=" + issueBank + "&reportdate=" + ReportDate);
        queryString = "FullCC.aspx?" + queryString;
        return queryString;
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        uxDrilldownGrid.Columns.FindByUniqueName("EntityName").Visible = GeneralFuncsLib.SHOW_PROCESSINGDATA_ENTITYNAME();
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

    private void SetTooltipForChargebackReportGrid()
    {
        SetTooltipForColumn(uxChargebackReportGrid, "FirstChargebackRDRAmount", "ASGridBoundColumnResource40", "FirstChargebackRDRAmount");
        SetTooltipForColumn(uxChargebackReportGrid, "PostChargebackRDRAmount", "ASGridBoundColumnResource42", "PostChargebackRDRAmount");
    }
}
