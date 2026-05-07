using AS.Common;
using AS.Common.DBManager;
using AS.Common.WebUI;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Controls.UserControls;
using AS.Web.Business;
using System;
using System.Data;
using System.Drawing;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using AS.Utilities;

[PagePermission("ReturnRpt,MSReturnRpt")]
public partial class gen_RiskReport : ReportPage
{
    #region Enums
    enum DataBindAction
    {
        BindDrilldownGrid,
        BindReportGrid,
        BindMSReportGrid,
        BindVerifyGrid,
        BindNonVerifyGrid,
    }

    enum PostBackAction
    {
        Verify
    }

    enum Verified
    {
        NonVerified = 0,
        Verified = 1,
        All = 2
    }

    #endregion

    #region Const
    private const string ReturnCount = "ReturnCount";
    private const string FullReturn = "FullReturn";
    private const string PartialReturn = "PartialReturn";
    private const string NoMatch = "NoMatch";
    private string _CurrentSortExpr = string.Empty;
    private string _CurrentSortOrder = string.Empty;
    const string IMAGE = "<a class=\"image-link\" href=\"#\" onclick=\" return ShowPopupModal('{0}','auto');\"><img src='res/img/information.png' border=\"0\"/></a>&nbsp; &nbsp;";
    private const string MATCHED_CODE = "MatchCode";
    private const string MATCHED_NAME = "MatchName";

    #endregion

    private string _Target = "_parent";

    private string _GridTitle
    {
        get
        {
            return GeneralFuncsLib.GetFullGridTitleName(ReportFilter) + " " + GeneralFuncsLib.GetDateFilterText(ReportFilter);
        }
    }

    private string ReportTitle
    {
        get
        {
            return GeneralFuncsLib.ReturnReportTitle();
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
        this.GridIDs.Add("uxNonVerifiedGrid");
        this.GridIDs.Add("uxVerifiedGrid");

        this.ExporterIDs.Add("uxExporterReport");
        this.ExporterIDs.Add("uxExporterNonVerifiedTop");
        this.ExporterIDs.Add("uxExporterVerifiedTop");

        ReportChart = (IChart)uxChart;
        base.PageInitialize();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (aperia.controls.KendoChart.IsDataRequest) return;
        if (SessionManager.ClientFrameInfo != string.Empty) _Target = SessionManager.ClientFrameInfo;
        IsBindDataOnLoad = true;
        //46652 - AW Multi-currency Transaction Display
        GeneralFuncsLib.ShowHideAWTransactionDetail(uxReportGrid);
        if (IsIntruderDetected) return;
    }

    protected override void OnPreRender(EventArgs e)
    {
        uxDrilldownGrid.Columns.FindByUniqueName("EntityName").Visible = GeneralFuncsLib.SHOW_PROCESSINGDATA_ENTITYNAME();
        base.OnPreRender(e);
    }

    bool _isExporting = false;
    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, AS.Controls.Exporter.ExportConfig exportConfig)
    {
        //46652 - AW Multi-currency Transaction Display
        GeneralFuncsLib.ShowHideAWTransactionDetail(uxReportGrid);
        _isExporting = true;
        uxReportGrid.Columns.FindByUniqueName("AccountNumber").Visible = false;
        uxReportGrid.Columns.FindByUniqueName("PartialCardNumber").Visible = true;
        uxNonVerifiedGrid.Columns.FindByUniqueName("VerifyReturns").Visible = false;
        if(IsShowRoutingAccount)
        {
            uxReportGrid.Columns.FindByUniqueName("RoutingAccountNumber").Visible = false;
            uxReportGrid.Columns.FindByUniqueName("PartialRoutingACC").Visible = true;

            uxVerifiedGrid.Columns.FindByUniqueName("PartialRoutingACC").Visible = true;
            uxNonVerifiedGrid.Columns.FindByUniqueName("PartialRoutingACC").Visible = true;
        }

        base.DoNeedExportConfig(sender, exportConfig);
        string fileName = ((UxExport)sender).GridTitle.ToString() + " - " + _GridTitle;

        if (uxDrilldownGrid.Visible && sender == uxExporter)
        {
                uxDrilldownGrid.DisplayEntityNameWhenExport();
                if (uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderText == GetLocalResourceObject("ReturnReport_aspx_cs_Text_MerchantNumber").ToString()
                || uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderText == GetLocalResourceObject("ReturnReport_aspx_cs_Text_Merchant").ToString()
                || GeneralFuncsLib.IsMerchantMode(ReportFilter.CurrentValue.HierarchyMode))
                {
                    uxDrilldownGrid.Columns.FindByUniqueName("EntityName").HeaderText = GetLocalResourceObject("ReturnReport_aspx_cs_Text_MerchantName").ToString();
                }
        }

        exportConfig.FileName = GeneralFuncsLib.FormatFileName(fileName);
        exportConfig.ReportHeader = fileName;
    }

    private bool CheckCSViewFullCard()
    {
        if ((SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.CS
            || SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.AS)
            && (IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_CC)))
        {
            return true;
        }
        return false;
    }

    protected override void DoSwitchView()
    {
        if (GeneralFuncsLib.IsMerchantMode(ReportFilter.CurrentValue.HierarchyMode)
            && !string.IsNullOrEmpty(ReportFilter.CurrentValue.Value))
        {
            if (CheckCSViewFullCard())
            {
                uxReportGrid.Visible = true;
                uxNonVerifiedGrid.Visible = false;
                uxVerifiedGrid.Visible = false;
                uxDrilldownGrid.Visible = false;

                uxReportGrid.Columns.FindByUniqueName("AccountNumber").Visible = true;
                uxReportGrid.Columns.FindByUniqueName("PartialCardNumber").Visible = false;
                if (IsShowRoutingAccount)
                {
                    uxReportGrid.Columns.FindByUniqueName("RoutingAccountNumber").Visible = true;
                    uxReportGrid.Columns.FindByUniqueName("PartialRoutingACC").Visible = false;
                }
            }
            else
            {
                if (SessionManager.CurrentUser.EntityID != ReportFilter.CurrentValue.Value)
                {
                    uxReportGrid.Visible = true;
                    uxVerifiedGrid.Visible = false;
                    uxNonVerifiedGrid.Visible = false;

                    uxReportGrid.Columns.FindByUniqueName("AccountNumber").Visible = false;
                    uxReportGrid.Columns.FindByUniqueName("PartialCardNumber").Visible = true;
                    if (IsShowRoutingAccount)
                    {
                        uxReportGrid.Columns.FindByUniqueName("RoutingAccountNumber").Visible = false;
                        uxReportGrid.Columns.FindByUniqueName("PartialRoutingACC").Visible = true;
                    }
                }
                else
                {
                    uxReportGrid.Visible = false;
                    uxVerifiedGrid.Visible = true;
                    uxNonVerifiedGrid.Visible = true;

                    if (IsShowRoutingAccount)
                    {
                        uxVerifiedGrid.Columns.FindByUniqueName("PartialRoutingACC").Visible = true;
                        uxNonVerifiedGrid.Columns.FindByUniqueName("PartialRoutingACC").Visible = true;
                    }
                }
                uxDrilldownGrid.Visible = false;
            }
        }
        else
        {
            uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderText = GeneralFuncsLib.GetGridDrilldownColumnName(ReportFilter.CurrentValue.HierarchyMode, ReportFilter.CurrentValue.Value);
            uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderTooltip = GeneralFuncsLib.GetGridDrilldownColumnName(ReportFilter.CurrentValue.HierarchyMode, ReportFilter.CurrentValue.Value);
            uxDrilldownGrid.Visible = true;
            uxReportGrid.Visible = false;
            uxNonVerifiedGrid.Visible = false;
            uxVerifiedGrid.Visible = false;
            GeneralFuncsLib.VisibleHierarchyNameByFilteringMode(ReportFilter.CurrentValue.HierarchyMode, ReportFilter.CurrentValue.Value, uxDrilldownGrid, uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderText);
        }
    }

    protected override void DoGridNeedDataSource(ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        if (aperia.controls.KendoChart.IsDataRequest) return;

        if (sender == uxDrilldownGrid && uxDrilldownGrid.Visible)
            OnDataBindControls(DataBindAction.BindDrilldownGrid, sender);
        if (sender == uxReportGrid && uxReportGrid.Visible)
            OnDataBindControls(DataBindAction.BindReportGrid, sender);
        if (sender == uxNonVerifiedGrid && sender.Visible)
            OnDataBindControls(DataBindAction.BindNonVerifyGrid, sender);
        if (sender == uxVerifiedGrid && sender.Visible)
            OnDataBindControls(DataBindAction.BindVerifyGrid, sender);
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddHierarchyFilterParams(this);
        parameters.AddLoggedInUserReportingParams();

        string spaName = string.Empty;
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindDrilldownGrid:
                {
                    parameters.Add(new FilterParameter("@ReportType", "Returns", DbType.AnsiString));
                    (sender as ASGrid).DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { WebSiteConstants.GET_REPORT_SPA_NAME, ReportServices.ConvertToFilterParamWSArray(parameters) });
                    uxExporter.GridTitle = VeraCodeSolution.ValidateResponseData(BuildGridHeader());
                    uxExporter.GridSubTitle = VeraCodeSolution.ValidateResponseData(_GridTitle);
                    GeneralFuncsLib.VisibleHierarchyNameByFilteringMode(ReportFilter.CurrentValue.HierarchyMode, ReportFilter.CurrentValue.Value, uxDrilldownGrid, uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderText);
                }
                break;
            case DataBindAction.BindReportGrid:
                {
                    if (CheckCSViewFullCard())
                    {
                        parameters.AddDecryptDataParams("AccountNumber", _isExporting);
                        if(IsShowRoutingAccount)
                            parameters.AddDecryptDataParams("RoutingAccountNumber", _isExporting);
                    }
                    parameters.AddLoggedInUserPrimaryUserID();
                    parameters.AddLanguageID();
                    spaName = "spa_cs_GetMerchantReturns";
                    (sender as ASGrid).DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(parameters) });
                    uxExporterReport.GridTitle = VeraCodeSolution.ValidateResponseData(BuildGridHeader());
                    uxExporterReport.GridSubTitle = VeraCodeSolution.ValidateResponseData(_GridTitle);
                }
                break;
            case DataBindAction.BindNonVerifyGrid:
                {
                    parameters.AddLoggedInUserPrimaryUserID();
                    parameters.Add(new FilterParameter("@IsVerified", Verified.NonVerified, DbType.Int32));
                    spaName = "spa_ms_GetMerchantReturns";

                    (sender as ASGrid).DataSourceInvoker = new ASFuncInvoker(WebServices.MsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(parameters) });
                    uxExporterNonVerifiedTop.GridTitle = VeraCodeSolution.ValidateResponseData(GetLocalResourceObject("ReturnReport_aspx_cs_NonVerifiedReturn").ToString());
                    uxExporterNonVerifiedTop.GridSubTitle = VeraCodeSolution.ValidateResponseData(_GridTitle);
                }
                break;

            case DataBindAction.BindVerifyGrid:
                {
                    parameters.AddLoggedInUserPrimaryUserID();
                    parameters.Add(new FilterParameter("@IsVerified", Verified.Verified, DbType.Int32));
                    spaName = "spa_ms_GetMerchantReturns";
                    (sender as ASGrid).DataSourceInvoker = new ASFuncInvoker(WebServices.MsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(parameters) });
                    uxExporterVerifiedTop.GridTitle = VeraCodeSolution.ValidateResponseData(GetLocalResourceObject("ReturnReport_aspx_cs_VerifiedReturn").ToString());
                    uxExporterVerifiedTop.GridSubTitle = VeraCodeSolution.ValidateResponseData(_GridTitle);
                }
                break;
        }
    }

    protected override void DoItemDataBound(ASGrid sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridHeaderItem && sender == uxNonVerifiedGrid)
        {
            GridHeaderItem headerItem = e.Item as GridHeaderItem;
            ((CheckBox)headerItem["VerifyReturns"].FindControl("chkAllVerify")).Enabled = uxNonVerifiedGrid.AS_DataSource.Rows.Count > 0;
        }
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView dataRow = e.Item.DataItem as DataRowView;

            if (sender == uxDrilldownGrid && sender.Visible)
            {
                if (!GeneralFuncsLib.SHOW_PROCESSINGDATA_ENTITYNAME())
                {
                    if (GeneralFuncsLib.IsMerchantModeToSetTooltip(ReportFilter) && dataRow.DataView.Table.Columns["EntityName"] != null)
                    {
                        dataItem["DrilldownColumn"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["EntityName"].ToString());
                    }
                }
                dataItem["EntityName"].Text = VeraCodeSolution.DoVeraCode(dataRow["EntityName"].ToString());
            }
            else
            {
                string queryString = BuildSecureQueryString("BatchNumber=" + dataRow["BatchNumber"].ToString() + "&TerminalNumber=" + dataRow["TerminalNumber"].ToString() + "&ReportDate=" + ((DateTime)dataRow["ReportDate"]).ToString("MM/dd/yyyy") + BatchDetailIntruderQuery(((ASGrid)sender)));
                string urlBatch = "BatchDetailModal.aspx?" + queryString;
                string url = string.Format("<a href=\"#\" onclick=\" return ShowPopupModal('{0}','auto');\">{1}</a>", urlBatch, dataRow["BatchNumber"].ToString());
                dataItem["BatchNumber"].Text = VeraCodeSolution.DoVeraCode(url);

                dataItem["CardType"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["CardDescription"].ToString());

                dataItem["ExpirationDate"].ToolTip = "MM/YY";

                string urlCard = GeneralFuncsLib.BuildCardUrl((SecurePage)this.Page, dataRow["PartialCardNumber"].ToString(), dataRow["AccountNumber"].ToString(), ReportFilter.CurrentValue.Value);

                string urlRouting = string.Empty;
                if (IsShowRoutingAccount)
                {
                    urlRouting = GeneralFuncsLib.BuildCardUrl((SecurePage)this.Page, dataRow["PartialRoutingACC"].ToString(), dataRow["RoutingAccountNumber"].ToString(), ReportFilter.CurrentValue.Value);
                }

                if (CheckCSViewFullCard())
                {
                    dataItem["AccountNumber"].Text = VeraCodeSolution.DoVeraCode(urlCard + dataRow["AccountNumber"].ToString() + "</a>");

                    if (IsShowRoutingAccount)
                        dataItem["RoutingAccountNumber"].Text = VeraCodeSolution.DoVeraCode(urlRouting + dataRow["RoutingAccountNumber"].ToString() + "</a>");
                }
                else
                {
                    if (SessionManager.CurrentUser.EntityID != ReportFilter.CurrentValue.Value)
                    {
                        if (GeneralFuncsLib.HasIPForFullCard((SecurePage)this))
                        {
                            dataItem["PartialCardNumber"].Text = VeraCodeSolution.DoVeraCode(
                                String.Format(IMAGE, BuildUrlForFullCard(dataRow["RecordId"].ToString(), dataRow["IssueBank"].ToString(), dataRow["ReportDate"].ToString()))
                                + urlCard + dataRow["PartialCardNumber"].ToString() + "</a>");

                            if (IsShowRoutingAccount)
                                dataItem["PartialRoutingACC"].Text = VeraCodeSolution.DoVeraCode(
                                String.Format(IMAGE, BuildUrlForFullCard(dataRow["RecordId"].ToString(), dataRow["IssueBank"].ToString(), dataRow["ReportDate"].ToString()))
                                + urlRouting + dataRow["PartialRoutingACC"].ToString() + "</a>"); ;
                        }
                        else
                        {
                            dataItem["PartialCardNumber"].Text = VeraCodeSolution.DoVeraCode(urlCard + dataRow["PartialCardNumber"].ToString() + "</a>");

                            if (IsShowRoutingAccount)
                                dataItem["PartialRoutingACC"].Text = VeraCodeSolution.DoVeraCode(urlRouting + dataRow["PartialRoutingACC"].ToString() + "</a>");
                        }
                    }
                    else // Verify Available
                    {
                        if (GeneralFuncsLib.HasIPForFullCard((SecurePage)this.Page))
                        {
                            dataItem["PartialCardNumber"].Text = VeraCodeSolution.DoVeraCode(
                                String.Format(IMAGE, BuildUrlForFullCard(dataRow["RecordId"].ToString(), dataRow["IssueBank"].ToString(), dataRow["ReportDate"].ToString())) + VeraCodeSolution.DoVeraCode(string.Format("<a href='{0}' target='{2}'>{1}</a>"
                            , urlCard + dataRow["PartialCardNumber"].ToString() + "</a>"))
                            );

                            if (IsShowRoutingAccount)
                                dataItem["PartialRoutingACC"].Text = VeraCodeSolution.DoVeraCode(
                                    String.Format(IMAGE, BuildUrlForFullCard(dataRow["RecordId"].ToString(), dataRow["IssueBank"].ToString(), dataRow["ReportDate"].ToString())) + VeraCodeSolution.DoVeraCode(string.Format("<a href='{0}' target='{2}'>{1}</a>"
                                , urlRouting + dataRow["PartialRoutingACC"].ToString() + "</a>"))
                                );
                        }
                        else
                        {
                            dataItem["PartialCardNumber"].Text = VeraCodeSolution.DoVeraCode(urlCard + dataRow["PartialCardNumber"].ToString() + "</a>");

                            if (IsShowRoutingAccount)
                                dataItem["PartialRoutingACC"].Text = VeraCodeSolution.DoVeraCode(urlRouting + dataRow["PartialRoutingACC"].ToString() + "</a>");
                        }
                    }
                }
                if ((sender == uxVerifiedGrid || sender == uxReportGrid) && sender.Visible)
                {
                    // 46807
                    dataItem[MATCHED_CODE].ToolTip = VeraCodeSolution.DoVeraCode(dataRow[MATCHED_NAME].ToString());

                    string matchCode = dataRow[MATCHED_CODE].ToString();

                    if (matchCode.Equals("P", StringComparison.OrdinalIgnoreCase) || matchCode.Equals("CP", StringComparison.OrdinalIgnoreCase))
                    {
                        dataItem[MATCHED_CODE].Text = GeneralFuncsLib.FormatBorderText(matchCode, Color.Blue);
                    }
                    else if (matchCode.Equals("U", StringComparison.OrdinalIgnoreCase) || matchCode.Equals("SC", StringComparison.OrdinalIgnoreCase))
                    {
                        dataItem[MATCHED_CODE].Text = GeneralFuncsLib.FormatBorderText(matchCode, Color.Red);
                    }
                    else if (matchCode.Equals("M", StringComparison.OrdinalIgnoreCase) || matchCode.Equals("C", StringComparison.OrdinalIgnoreCase))
                    {
                        dataItem[MATCHED_CODE].Text = GeneralFuncsLib.FormatBorderText(matchCode, "#3fbf00".ToColor());
                    }
                }


                if (sender == uxNonVerifiedGrid && sender.Visible)  // for Merchant View
                {
                    ((CheckBox)dataItem["VerifyReturns"].FindControl("chkVerify")).Attributes.Add("onclick", "javascript:return doVerifyReturns('" + dataRow["RecordId"].ToString() + "');");
                }
            }
        }
    }

    private string BuildGridHeader()
    {
        string prefixTitle = ReportTitle.IsNullOrEmpty() ? GetLocalResourceObject("ReturnReport_aspx_cs_Return").ToString() : ReportTitle;
        return VeraCodeSolution.ValidateResponseData(prefixTitle);
    }



    protected void btnVerify_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.Verify);
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.Verify:
                if (uxHidClick.Value == "ClickedVerify")
                {
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    FilterParameterCollection outParameters = new FilterParameterCollection();

                    int recordid = Convert.ToInt32(uxHidArgs.Value);
                    parameters.Add(new FilterParameter("@RecordId", recordid, DbType.Int32));
                    parameters.Add(new FilterParameter("@ASClient", SessionManager.CurrentUser.ASClient, DbType.Int32));
                    parameters.Add(new FilterParameter("@DateFilterMode", (int)ReportFilter.CurrentValue.DateOption, DbType.Int32));
                    parameters.Add(new FilterParameter("@BeginDate", ReportFilter.CurrentValue.DateOptionValue.From, DbType.DateTime));
                    parameters.Add(new FilterParameter("@EndDate", ReportFilter.CurrentValue.DateOptionValue.To, DbType.DateTime));
                    parameters.Add(new FilterParameter("@MerchantNumber", ReportFilter.CurrentValue.Value, DbType.AnsiString));

                    WebServices.MsReportServices.ExecuteNonQueryCommand("spa_ms_UpdateNonVerifiedReturnsByRecordId", parameters, out outParameters);

                    uxHidArgs.Value = "";
                    uxHidClick.Value = "";

                    uxChart.BindChart();
                    uxNonVerifiedGrid.Rebind();
                    uxVerifiedGrid.Rebind();
                }
                break;
        }
    }

    private string BuildUrlForFullCard(string encryptedCC, string issueBank, string ReportDate)
    {
        string queryString = BuildSecureQueryString("rt=Returns" + "&cn=" + Server.UrlEncode(encryptedCC) + "&issue=" + issueBank + "&reportdate=" + ReportDate);
        queryString = "FullCC.aspx?" + queryString;
        return queryString;
    }

    string _BatchDetailIntruderQuery = string.Empty;
    private string BatchDetailIntruderQuery(ASGrid grid)
    {
        if (_BatchDetailIntruderQuery == string.Empty)
        {
            _BatchDetailIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(grid.ID, new string[] { "BatchNumber", "TerminalNumber" });
        }
        return _BatchDetailIntruderQuery;
    }
    string _CardSearchIntruderQuery = string.Empty;
    private string CardSearchIntruderQuery(ASGrid grid)
    {
        if (_CardSearchIntruderQuery == string.Empty)
        {
            _CardSearchIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(grid.ID, new string[] { "AccountNumber" });
        }
        return _CardSearchIntruderQuery;
    }
}
