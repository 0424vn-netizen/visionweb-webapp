using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AS.Controls.Grid;
using AS.Common.DBManager;
using AS.Common;
using AS.Controls.UserControls;
using Telerik.Web.UI;
using System.Data;
using AS.Web.Business;
using AS.Controls.Pages;

[PagePermission("AuthLogRpt,MSAuthLogRpt")]
public partial class AuthorizationLog : ReportPage
{
    #region Enum

    enum DataBindAction
    {
        BindDrilldownGrid,
        BindReportGrid,
        BindHeaderText,
    }

    #endregion
   
    #region Const

    string REPORT_HEADER = string.Empty;
    const string MERCHANT_NUMBER = "MerchantNumber";
    const string MERCHANT_NAME = "MerchantName";
    const string DRILLDOWN = "DrilldownColumn";
    const string IMAGE = "<a class=\"image-link\" href=\"#\" onclick=\" return ShowPopupModal('{0}','auto');\"><img src='res/img/information.png' border=\"0\"/></a>&nbsp; &nbsp;";
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

        this.ExporterIDs.Add("uxExporterDrilldown");
        //this.ExporterIDs.Add("uxExporterDrilldownBottom");
        //this.ExporterIDs.Add("uxExporterBottom");
        base.PageInitialize();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        REPORT_HEADER = GetLocalResourceObject("AuthorizationLog_aspx_cs_Text_AuthLog").ToString() + " ";
        IsBindDataOnLoad = true;
        if (SessionManager.ClientFrameInfo != string.Empty) _Target = SessionManager.ClientFrameInfo;
        //46652 - AW Multi-currency Transaction Display
        GeneralFuncsLib.ShowHideAWTransactionDetail(uxReportGrid);
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        OnDataBindControls(DataBindAction.BindHeaderText);
        uxDrilldownGrid.Columns.FindByUniqueName("EntityName").Visible = GeneralFuncsLib.SHOW_PROCESSINGDATA_ENTITYNAME();

    }

    bool _isExporting = false;
    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, AS.Controls.Exporter.ExportConfig exportConfig)
    {
        //46652 - AW Multi-currency Transaction Display
        GeneralFuncsLib.ShowHideAWTransactionDetail(uxReportGrid);
        base.DoNeedExportConfig(sender, exportConfig);
        _isExporting = true;
        uxReportGrid.Columns.FindByUniqueName("AccountNumber").Visible = false;
        uxReportGrid.Columns.FindByUniqueName("PartialCardNumber").Visible = true;
        if (IsShowRoutingAccount)
        {
            uxReportGrid.Columns.FindByUniqueName("RoutingAccountNumber").Visible = false;
            uxReportGrid.Columns.FindByUniqueName("PartialRoutingACC").Visible = true;
        }
        string fileName = REPORT_HEADER + _GridTitle;
        if (!uxReportGrid.Visible)
        {
            uxDrilldownGrid.DisplayEntityNameWhenExport();
            if (uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderText == GetLocalResourceObject("AuthorizationLog_aspx_cs_Text_MerchantNumber").ToString()
            || uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderText == GetLocalResourceObject("AuthorizationLog_aspx_cs_Text_Merchant").ToString()
            || GeneralFuncsLib.IsMerchantMode(ReportFilter.CurrentValue.HierarchyMode))
            {
                uxDrilldownGrid.Columns.FindByUniqueName("EntityName").HeaderText = GetLocalResourceObject("AuthorizationLog_aspx_cs_Text_MerchantName").ToString();
            }
        }
        //base.DoNeedExportConfig(sender, exportConfig);        
        exportConfig.FileName = GeneralFuncsLib.FormatFileName(fileName);
        exportConfig.ReportHeader = fileName;
    }

    protected override void DoGridNeedDataSource(ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        if (sender == uxDrilldownGrid && uxDrilldownGrid.Visible)
        {
            OnDataBindControls(DataBindAction.BindDrilldownGrid, sender);
        }
        if (sender == uxReportGrid && uxReportGrid.Visible)
        {
            OnDataBindControls(DataBindAction.BindReportGrid, sender);
        }
    }

    protected override void DoItemDataBound(ASGrid sender, GridItemEventArgs e)
    {
        if ((sender == uxDrilldownGrid || sender == uxReportGrid) && sender.Visible)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = e.Item as GridDataItem;
                DataRowView dataRow = e.Item.DataItem as DataRowView;
                if (sender == uxDrilldownGrid)
                {
                    if (!GeneralFuncsLib.SHOW_PROCESSINGDATA_ENTITYNAME())
                    {
                        if (GeneralFuncsLib.IsMerchantModeToSetTooltip(ReportFilter) && dataRow.DataView.Table.Columns["EntityName"] != null)
                        {
                            dataItem[DRILLDOWN].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["EntityName"].ToString());
                        }
                    }
                    dataItem["EntityName"].Text = VeraCodeSolution.DoVeraCode(dataRow["EntityName"].ToString());
                }

                if (sender == uxReportGrid)
                {
                    dataItem["CardType"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["CardDescription"].ToString());
                    dataItem["AVS"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["AVSDescription"].ToString());
                    dataItem["CVV"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["CVVDescription"].ToString());
                    dataItem["AuthSource"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["AuthorizationSourceDescription"].ToString());
                    dataItem["CustID"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["CustIDDescription"].ToString());
                    dataItem["MOTO"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["MOTODescription"].ToString());
                    dataItem["TransCode"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["TransactionCode"].ToString());
                    dataItem["Approved"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["ApprovedDescription"].ToString());
                    dataItem["ResponseCode"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["ResponseCodeDescription"].ToString());
                    dataItem["KeyedEntry"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["EntryModeDescription"].ToString());
                    dataItem["ExpirationDate"].ToolTip = "MM/YY";

                    string urlCard = GeneralFuncsLib.BuildCardUrl((SecurePage)this.Page, dataRow["PartialCardNumber"].ToString(), dataRow["AccountNumber"].ToString(), ReportFilter.CurrentValue.Value);

                    string urlRouting = string.Empty;
                    if (IsShowRoutingAccount)
                    {
                        urlRouting = GeneralFuncsLib.BuildCardUrl((SecurePage)this.Page, dataRow["PartialRoutingACC"].ToString(), dataRow["RoutingAccountNumber"].ToString(), ReportFilter.CurrentValue.Value);
                    }

                    if (CheckCSViewFullCard())
                    {
                        if (!dataRow["AccountNumber"].ToString().IsNullOrEmpty() || !dataRow["RoutingAccountNumber"].ToString().IsNullOrEmpty())
                        {
                            dataItem["AccountNumber"].Text = VeraCodeSolution.DoVeraCode(urlCard + dataRow["AccountNumber"].ToString() + "</a>");
                            if (IsShowRoutingAccount)
                                dataItem["RoutingAccountNumber"].Text = VeraCodeSolution.DoVeraCode(urlRouting + dataRow["RoutingAccountNumber"].ToString() + "</a>");
                        }
                    }
                    else
                    {
                        if (GeneralFuncsLib.HasIPForFullCard((SecurePage)this))
                        {
                            dataItem["PartialCardNumber"].Text = VeraCodeSolution.DoVeraCode(String.Format(
                                IMAGE,
                                BuildUrlForFullCard(dataRow["RecordId"].ToString(),
                                dataRow["IssueBank"].ToString(),
                                dataRow["ReportDate"].ToString()))
                                + VeraCodeSolution.DoVeraCode(urlCard + dataRow["PartialCardNumber"].ToString() + "</a>")
                                );

                            if (IsShowRoutingAccount)
                                dataItem["PartialRoutingACC"].Text = VeraCodeSolution.DoVeraCode(String.Format(
                                IMAGE,
                                BuildUrlForFullCard(dataRow["RecordId"].ToString(),
                                dataRow["IssueBank"].ToString(),
                                dataRow["ReportDate"].ToString()))
                                + VeraCodeSolution.DoVeraCode(urlRouting + dataRow["PartialRoutingACC"].ToString() + "</a>")
                                );
                        }
                        else
                        {
                            dataItem["PartialCardNumber"].Text = VeraCodeSolution.DoVeraCode(urlCard + dataRow["PartialCardNumber"].ToString() + "</a>");

                            if (IsShowRoutingAccount)
                                dataItem["PartialRoutingACC"].Text = VeraCodeSolution.DoVeraCode(urlRouting + dataRow["PartialRoutingACC"].ToString() + "</a>");
                        }
                    }

                    if (dataRow["Approved"].ToString() == "D")
                    {
                        dataItem["Approved"].Text = string.Format(WebSiteConstants.DECLINED_TEXT, dataItem["Approved"].Text);
                    }
                }
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

    protected override void DoSwitchView()
    {
        if (GeneralFuncsLib.IsMerchantMode(ReportFilter.CurrentValue.HierarchyMode)
            && !string.IsNullOrEmpty(ReportFilter.CurrentValue.Value))
        {
            uxReportGrid.Visible = true;
            uxDrilldownGrid.Visible = false;

            if (CheckCSViewFullCard())
            {
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
                uxReportGrid.Columns.FindByUniqueName("AccountNumber").Visible = false;
                uxReportGrid.Columns.FindByUniqueName("PartialCardNumber").Visible = true;
                if (IsShowRoutingAccount)
                {
                    uxReportGrid.Columns.FindByUniqueName("RoutingAccountNumber").Visible = false;
                    uxReportGrid.Columns.FindByUniqueName("PartialRoutingACC").Visible = true;
                }
            }
        }
        else
        {

            uxDrilldownGrid.Columns.FindByUniqueName(DRILLDOWN).HeaderText = GeneralFuncsLib.GetGridDrilldownColumnName(ReportFilter.CurrentValue.HierarchyMode, ReportFilter.CurrentValue.Value);
            uxDrilldownGrid.Columns.FindByUniqueName(DRILLDOWN).HeaderTooltip = GeneralFuncsLib.GetGridDrilldownColumnName(ReportFilter.CurrentValue.HierarchyMode, ReportFilter.CurrentValue.Value);
            uxReportGrid.Visible = false;
            uxDrilldownGrid.Visible = true;
            GeneralFuncsLib.VisibleHierarchyNameByFilteringMode(ReportFilter.CurrentValue.HierarchyMode, ReportFilter.CurrentValue.Value, uxDrilldownGrid, uxDrilldownGrid.Columns.FindByUniqueName(DRILLDOWN).HeaderText);
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindDrilldownGrid:
                {
                    FilterParameterCollection parames = new FilterParameterCollection();
                    parames.AddLoggedInUserReportingParams();
                    parames.AddHierarchyFilterParams((ReportPage)this);
                    parames.Add(new FilterParameter("@ReportType", "Authorization", DbType.AnsiString));
                    GeneralFuncsLib.VisibleHierarchyNameByFilteringMode(ReportFilter.CurrentValue.HierarchyMode, ReportFilter.CurrentValue.Value, uxDrilldownGrid, uxDrilldownGrid.Columns.FindByUniqueName(DRILLDOWN).HeaderText);
                    ((ASGrid)sender).DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { WebSiteConstants.GET_REPORT_SPA_NAME, ReportServices.ConvertToFilterParamWSArray(parames) });
                }
                break;
            case DataBindAction.BindReportGrid:
                {
                    string spaName = "spa_GetAuthorizationDetail";
                    FilterParameterCollection parames = new FilterParameterCollection();
                    parames.AddHierarchyFilterParams((ReportPage)this);
                    parames.AddLoggedInUserReportingParams();
                    parames.AddLoggedInUserPrimaryUserID();

                    if (CheckCSViewFullCard())
                    {
                        parames.AddDecryptDataParams("AccountNumber", _isExporting);
                        if(IsShowRoutingAccount)
                            parames.AddDecryptDataParams("RoutingAccountNumber", _isExporting);
                    }
                    parames.AddLanguageID();
                    ((ASGrid)sender).DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(parames) });
                }
                break;

            case DataBindAction.BindHeaderText:
                {
                    if (uxDrilldownGrid.Visible)
                    {
                        uxExporterDrilldown.GridTitle = VeraCodeSolution.ValidateResponseData(REPORT_HEADER);
                        uxExporterDrilldown.GridSubTitle = VeraCodeSolution.ValidateResponseData(_GridTitle);
                    }
                    else if (uxReportGrid.Visible)
                    {
                        uxExporter.GridTitle = VeraCodeSolution.ValidateResponseData(REPORT_HEADER);
                        uxExporter.GridSubTitle = VeraCodeSolution.ValidateResponseData(_GridTitle);
                    }
                }
                break;
        }
    }

    string _CardSearchIntruderQuery = string.Empty;
    private string CardSearchIntruderQuery
    {
        get
        {
            if (_CardSearchIntruderQuery == string.Empty)
            {
                GeneralFuncsLib.BuildIntruderInfoForQueryStr(uxReportGrid.ID, new string[] { "AccountNumber" });
            }
            return _CardSearchIntruderQuery;
        }
    }

    private string BuildUrlForFullCard(string encryptedCC, string issueBank, string ReportDate)
    {
        string queryString = BuildSecureQueryString("rt=AuthDetail" + "&cn=" + Server.UrlEncode(encryptedCC) + "&issue=" + issueBank + "&reportdate=" + ReportDate);
        queryString = "FullCC.aspx?" + queryString;
        return queryString;
    }

    private string GetActivity(bool isMerchant)
    {
        string value = GeneralFuncsLib.GetValueForActivityFunc(isMerchant, ReportFilter);
        string dateText = GeneralFuncsLib.GetDateTextForActivityFunc(isMerchant, ReportFilter);
        return string.Format(GetLocalResourceObject("AuthorizationLog_aspx_cs_Text_AuthLog1").ToString() + " {0} {1}", value, dateText);

    }

    protected override void DoReportFilterAction(AS.Web.UI.Controls.ReportFilterEventArgs e)
    {
        if (e.ActionType == AS.Web.UI.Controls.ReportFilterEventType.Submit)
        {
            string merchantName = GeneralFuncsLib.GetMerchantName(e.HierachyValue.Value);
            string activity = GetActivity(!string.IsNullOrEmpty(merchantName));
            string merchantNumber = string.IsNullOrEmpty(merchantName) ? "" : e.HierachyValue.Value;
            GeneralFuncsLib.SaveUserActivity(merchantNumber, activity);
        }
        base.DoReportFilterAction(e);
    }
}
