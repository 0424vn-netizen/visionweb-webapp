using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Exporter;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Controls.UserControls;
using AS.Web.Business;
using AS.Web.UI.AppCode.General;
using System;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

[PagePermission("RetChbRpt,MSRetChbRpt")]
public partial class RetrievalsChargebacksDetail : ReportPage
{
    #region Enum
    enum DataBindAction
    {
        BindReportGrid,
        BindChargebackGrid
    }
    #endregion
    #region Properties
    private string GET_REPORT_METHOD_NAME = "GetReports";
    private string GET_CSREPORT_METHOD_NAME = "GetCSReports";
    string _CardSearchIntruderQuery_Retrieval = string.Empty;
    string _CBSequenceIntruderQuery = string.Empty;
    private const string IMAGE = "<a class=\"image-link\" href=\"#\" style=\"cursor:pointer\" onclick=\" return parent.ShowPopupModalChild({0},'{1}','auto');\"><img src='res/img/information.png' border=\"0\"/></a>&nbsp; &nbsp;";
    private int curIdx = 0;
    private string _Target = "_parent";

    private string CBSequenceIntruderQuery
    {
        get
        {
            if (_CBSequenceIntruderQuery == string.Empty) _CBSequenceIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(uxReportChargebacks.ID, new string[] { "CBSeqNo" });
            return _CBSequenceIntruderQuery;
        }
    }
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
                _CardSearchIntruderQuery_Chargeback = GeneralFuncsLib.BuildIntruderInfoForQueryStr(uxReportChargebacks.ID, new string[] { "CardNumber" });
            }
            return _CardSearchIntruderQuery_Chargeback;
        }
    }

    public bool IsNotInMif
    {
        get
        {
            if (IsSecureQueryString)
            {
                if (SecureQueryString["isNotInMif"].IsNotNullData())
                {
                    return SecureQueryString["isNotInMif"].Equals("1");
                }
            }
            return false;
        }
    }

    public string HierarchValue
    {
        get
        {
            if (IsNotInMif)
            {
                if (SecureQueryString["merchantNumber"].IsNotNullData())
                {
                    return SecureQueryString["merchantNumber"].ToString();
                }
            }

            if (IsSecureQueryString && SecureQueryString["sourceMerchant"].IsNotNullData())
            {
                return SecureQueryString["sourceMerchant"].ToString();
            }    
                
            return this.SavedReportFilterValue.IsNotNullData() ? this.SavedReportFilterValue.Value : string.Empty;
        }
    }

    public string HierarchyMode
    {
        get
        {
            if (IsNotInMif)
            {
                return "MERCHANTNUMBER";
            }

            if (IsSecureQueryString && SecureQueryString["sourceMerchantMode"].IsNotNullData())
            {
                return SecureQueryString["sourceMerchantMode"].ToString();
            }    
                
            return this.SavedReportFilterValue.IsNotNullData() ? this.SavedReportFilterValue.HierarchyMode : string.Empty;
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
        this.GridIDs.Add("uxReportChargebacks"); ;
        this.ExporterIDs.Add("uxExportRetrievalsBottom");
        this.ExporterIDs.Add("uxExportChargebacksTop");
        this.ExporterIDs.Add("uxExportChargebacksBottom");
        this.IsBindDataOnLoad = true;
        base.PageInitialize();

    }
    private void ProcessQueryString()
    {
        string referenceNumber = SecureQueryString["referenceNumber"];
        string sourceName = SecureQueryString[WebSiteConstants.INTRUDER_SOURCE_PARAM_NAME];
        string keyName = SecureQueryString[WebSiteConstants.INTRUDER_KEY_PARAM_NAME];
        // continue to check data if intruder is not detected yet
        CheckDataIntruders(sourceName, keyName.Split(WebSiteConstants.INTRUDER_KEY_SEPERATOR.ToCharArray()), new object[] { referenceNumber });
    }
    protected override void DoSwitchView()
    {
        if (SecureQueryString["rcType"].ToString() == "r")
        {
            tblRetrieval.Visible = true;
            tblChargeback.Visible = false;
            this.Title = GetLocalResourceObject("RetrievalsChargebacksDetail_aspx_cs_RetrievalDetail").ToString();
        }
        else
        {
            tblRetrieval.Visible = false;
            tblChargeback.Visible = true;
            this.Title = GetLocalResourceObject("RetrievalsChargebacksDetail_aspx_cs_ChargeBackDetail").ToString();
        }
        if (GeneralFuncsLib.GetDataOfExtendedSetting("REMOVE_COLUMN_RTCB") == "true")
        {
            uxReportGrid.Columns.FindByUniqueName("RequestType").Visible = false;
            uxReportChargebacks.Columns.FindByUniqueName("CBType").Visible = false;
            uxReportChargebacks.Columns.FindByUniqueName("CBTypeDesc").Visible = false;
            uxReportChargebacks.Columns.FindByUniqueName("Disposition").Visible = false;
            uxReportChargebacks.Columns.FindByUniqueName("CBSeqNo").Visible = false;
            uxReportChargebacks.Columns.FindByUniqueName("RepresentedCBAmount").Visible = false;
            //uxReportChargebacks.XOverFlowable = false;
        }
        else
        {
            uxReportGrid.Columns.FindByUniqueName("RequestType").Visible = true;
            uxReportChargebacks.Columns.FindByUniqueName("CBType").Visible = true;
            uxReportChargebacks.Columns.FindByUniqueName("CBTypeDesc").Visible = true;
            uxReportChargebacks.Columns.FindByUniqueName("Disposition").Visible = true;
            uxReportChargebacks.Columns.FindByUniqueName("CBSeqNo").Visible = true;
            uxReportChargebacks.Columns.FindByUniqueName("RepresentedCBAmount").Visible = true;
        }

        if (IsShowRoutingAccount)
        {
            uxReportGrid.Columns.FindByUniqueName("RoutingAccountNumber").Visible = true;
            uxReportGrid.Columns.FindByUniqueName("PartialRoutingACC").Visible = false;
            uxReportChargebacks.Columns.FindByUniqueName("RoutingAccountNumber").Visible = true;
            uxReportChargebacks.Columns.FindByUniqueName("PartialRoutingACC").Visible = false;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected)
        {
            return;
        }
        if (SessionManager.ClientFrameInfo != string.Empty) _Target = SessionManager.ClientFrameInfo;
        PageType = SecurePageType.Modal;
        if (!IsPostBack)
        {
            GetGridTitle(false);
        }
    }

    private void GetGridTitle(bool isExporting)
    {
        string entityName = GeneralFuncsLib.GetMerchantName(HierarchValue, IsNotInMif);
        if (entityName.IsNullOrEmpty())
            uxMerchantInfo.Text = ASRadControlHelper.HandleHtmlEncodeDecode(HierarchValue, isExporting);
        else
            uxMerchantInfo.Text = ASRadControlHelper.HandleHtmlEncodeDecode(HierarchValue + " - " + entityName, isExporting);
    }


    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }
    protected override void DoItemDataBound(ASGrid sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView rowView = e.Item.DataItem as DataRowView;
            string cardType = rowView["CardType"].ToString();
            string reasonCode = rowView["ReasonCode"].ToString();
            string cardTypeDesc = rowView["CardTypeDesc"].ToString();
            string reasonCodeDesc = rowView["ReasonCodeDesc"].ToString();
            string cardNumber = rowView["CardNumber"].ToString();
            dataItem["CardType"].ToolTip = VeraCodeSolution.DoVeraCode(rowView["CardTypeDesc"].ToString());
            dataItem["ReasonCode"].ToolTip = VeraCodeSolution.DoVeraCode(rowView["ReasonCodeDesc"].ToString());
            if (sender == uxReportChargebacks && SessionManager.CurrentUser.ASClient != 23)
            {
                if (!string.IsNullOrEmpty(rowView["CBSeqNo"].ToString()))
                    dataItem["CBSeqNo"].Text = VeraCodeSolution.DoVeraCode(string.Format("<a href='#' onclick=\"parent.ShowPopupModalChild(1,'{0}',800,670);return false;\" >{1}</a>", "CBSequenceDetail.aspx?"
                        + this.BuildSecureQueryString("CBSequenceNumber=" + rowView["CBSeqNo"].ToString() + "&rptDate=" + rowView["ReportDate"] + "&recid=" + rowView["RecordID"] + "&idx=" + (curIdx + 1) + CBSequenceIntruderQuery + "&isNotInMif=" + IsNotInMif), rowView["CBSeqNo"].ToString()));
            }
            string queryString = string.Empty;

            queryString = BuildSecureQueryString("cn=" + rowView["PartialCardNumber"] + "&cnf=" + rowView["CardNumber"] + "&isNotInMif=" + IsNotInMif);
            string urlCardDetail = "CardHistoryModal.aspx?" + queryString;
            string urlCard = "<a class=\"link\" href=\"javascript:void(0)\" onclick=\"openPopupWindow('" + urlCardDetail + "','CardHistoryWindow'); return false;\">";

            string urlRouting = string.Empty;
            if (IsShowRoutingAccount)
            {
                string queryStringRouting = BuildSecureQueryString("cn=" + rowView["PartialRoutingACC"] + "&cnf=" + rowView["RoutingAccountNumber"] + "&isNotInMif=" + IsNotInMif);
                string urlRoutingDetail = "CardHistoryModal.aspx?" + queryString;
                urlRouting = "<a class=\"link\" href=\"javascript:void(0)\" onclick=\"openPopupWindow('" + urlCardDetail + "','CardHistoryWindow'); return false;\">";
            }

            var isCSViewFullCard = PermissionManager.CheckCSViewFullCard(((SecurePage)this.Page));

            if (sender == uxReportGrid)
            {
                if (isCSViewFullCard)
                {
                    dataItem["CardNumber"].Text = VeraCodeSolution.DoVeraCode(urlCard + rowView["CardNumber"].ToString() + "</a>");
                    if (IsShowRoutingAccount)
                        dataItem["RoutingAccountNumber"].Text = VeraCodeSolution.DoVeraCode(urlRouting + dataItem["RoutingAccountNumber"].Text + "</a>");
                }
                else
                {
                    if (GeneralFuncsLib.HasIPForFullCard(this))
                    {
                        dataItem["CardNumber"].Text = VeraCodeSolution.DoVeraCode(
                            String.Format(IMAGE, (curIdx + 1), BuildUrlForFullCard("Retrievals", rowView["RecordId"].ToString(),
                            rowView["IssueBank"].ToString(), rowView["ReportDate"].ToString())) + VeraCodeSolution.DoVeraCode(urlCard + rowView["PartialCardNumber"].ToString() + "</a>"));

                        if (IsShowRoutingAccount)
                            dataItem["RoutingAccountNumber"].Text = VeraCodeSolution.DoVeraCode(
                           String.Format(IMAGE, (curIdx + 1), BuildUrlForFullCard("Retrievals", rowView["RecordId"].ToString(),
                           rowView["IssueBank"].ToString(), rowView["ReportDate"].ToString())) + VeraCodeSolution.DoVeraCode(urlRouting + rowView["PartialRoutingACC"].ToString() + "</a>"));
                    }
                    else
                    {
                        dataItem["CardNumber"].Text = VeraCodeSolution.DoVeraCode(urlCard + rowView["PartialCardNumber"].ToString() + "</a>");
                        if (IsShowRoutingAccount)
                            dataItem["RoutingAccountNumber"].Text = VeraCodeSolution.DoVeraCode(urlRouting + rowView["PartialRoutingACC"].ToString() + "</a>");
                    }
                }
            }
            else
            {

                if (isCSViewFullCard)
                {
                    dataItem["CardNumber"].Text = VeraCodeSolution.DoVeraCode(urlCard + rowView["CardNumber"].ToString() + "</a>");
                    if (IsShowRoutingAccount)
                        dataItem["RoutingAccountNumber"].Text = VeraCodeSolution.DoVeraCode(urlRouting + dataItem["RoutingAccountNumber"].Text + "</a>");
                }
                else
                {
                    if (GeneralFuncsLib.HasIPForFullCard(this))
                    {
                        dataItem["CardNumber"].Text = VeraCodeSolution.DoVeraCode(
                            String.Format(IMAGE, (curIdx + 1), BuildUrlForFullCard("Chargebacks", rowView["RecordId"].ToString(), rowView["IssueBank"].ToString(),
                            rowView["ReportDate"].ToString())) + VeraCodeSolution.DoVeraCode(urlCard + rowView["PartialCardNumber"].ToString() + "</a>"));

                        if (IsShowRoutingAccount)
                            dataItem["RoutingAccountNumber"].Text = VeraCodeSolution.DoVeraCode(
                           String.Format(IMAGE, (curIdx + 1), BuildUrlForFullCard("Chargebacks", rowView["RecordId"].ToString(), rowView["IssueBank"].ToString(),
                           rowView["ReportDate"].ToString())) + VeraCodeSolution.DoVeraCode(urlRouting + rowView["PartialRoutingACC"].ToString() + "</a>"));
                    }
                    else
                    {
                        dataItem["CardNumber"].Text = VeraCodeSolution.DoVeraCode(urlCard + rowView["PartialCardNumber"].ToString() + "</a>");

                        if (IsShowRoutingAccount)
                            dataItem["RoutingAccountNumber"].Text = VeraCodeSolution.DoVeraCode(urlRouting + rowView["PartialRoutingACC"].ToString() + "</a>");
                    }
                }
            }
        }
    }
    private string BuildUrlForFullCard(string encryptedCC, string issueBank, string ReportDate)
    {
        string queryString = BuildSecureQueryString("rt=Chargebacks" + "&cn=" + Server.UrlEncode(encryptedCC) + "&issue=" + issueBank + "&reportdate=" + ReportDate + "&idx=" + (curIdx + 1) + "&isNotInMif=" + IsNotInMif);
        queryString = ResolveUrl("~/") + "FullCC.aspx?" + queryString;
        return queryString;
    }
    protected override void DoGridNeedDataSource(AS.Controls.Grid.ASGrid sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if (sender == uxReportGrid && uxReportGrid.Visible)
        {
            OnDataBindControls(DataBindAction.BindReportGrid, sender);
        }
        if (sender == uxReportChargebacks && uxReportChargebacks.Visible)
        {
            OnDataBindControls(DataBindAction.BindChargebackGrid, sender);
        }
    }
    protected bool AllowFullCC;
    protected override void OnDataBindControls(Enum type, object sender)
    {
        ASGrid grid = (ASGrid)sender;
        string referenceNumber = this.SecureQueryString["referenceNumber"].ToString();
        FilterParameterCollection parameters = new FilterParameterCollection();
        FilterParameter PermissionCodeParam = new FilterParameter(WebSiteConstants.SPA_PERMISSION_PARAM_NAME, WebSiteConstants.SEC_PERMISSION_CC, DbType.String);
        parameters.Add("@HierarchyFilterMode", HierarchyMode, DbType.String);
        parameters.Add("@HierarchyFilterValue", HierarchValue, DbType.String);
        parameters.Add("@DateFilterMode", this.SavedReportFilterValue.DateOption, DbType.Int32);
        parameters.Add("@BeginDate", this.SavedReportFilterValue.DateOptionValue.From, DbType.DateTime);
        parameters.Add("@EndDate", GeneralFuncsLib.GetEndDate(this.SavedReportFilterValue.DateOptionValue, this.SavedReportFilterValue.DateOption), DbType.DateTime);
        parameters.Add("@IsNotInMif", IsNotInMif, DbType.Boolean);
        AllowFullCC = IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_CC);
        string spa_name = string.Empty;
        string WebMethodName = string.Empty;
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindReportGrid:
                {
                    if (AllowFullCC && !GeneralFuncsLib.HasIPForFullCard(this))
                    {
                        parameters.AddLoggedInUserReportingParams();
                        parameters.AddLoggedInUserPrimaryUserID();
                        //parameters.Add(PermissionCodeParam);
                        parameters.Add(new FilterParameter("@RiskClient", false, DbType.Boolean));
                        parameters.AddDecryptDataParams("CardNumber");

                        if (IsShowRoutingAccount)
                            parameters.AddDecryptDataParams("RoutingAccountNumber");
                        spa_name = "spa_cs_GetMerchantRetrievalsChargebacksDetails";
                        WebMethodName = WebSiteConstants.GET_REPORT_FULL_VIEW_METHOD_NAME;
                    }
                    else
                    {
                        if (GeneralFuncsLib.HasIPForFullCard(this))
                        {
                            parameters.AddLoggedInUserReportingParams();
                            parameters.AddLoggedInUserPrimaryUserID();
                            parameters.Add(new FilterParameter("@RiskClient", true, DbType.Boolean));
                            parameters.AddDecryptDataParams("CardNumber");
                            if (IsShowRoutingAccount)
                                parameters.AddDecryptDataParams("RoutingAccountNumber");
                            spa_name = "spa_cs_GetMerchantRetrievalsChargebacksDetails";
                            WebMethodName = GET_CSREPORT_METHOD_NAME;
                        }
                        else
                        {
                            parameters.AddLoggedInUserReportingParams();
                            spa_name = "spa_ms_GetMerchantRetrievalsChargebacksDetails";
                            WebMethodName = GET_REPORT_METHOD_NAME;
                        }
                    }
                    parameters.Add("@Format", "Retrieval", System.Data.DbType.String);
                    parameters.Add("@ReferenceNumber", referenceNumber, System.Data.DbType.String);
                    parameters.AddLanguageID();
                    uxExporter.GridSubTitle = GetLocalResourceObject("RetrievalsChargebacksDetail_aspx_cs_Reference").ToString() + " " + VeraCodeSolution.DoVeraCode(this.SecureQueryString["referenceNumber"].ToString());

                }
                break;

            case DataBindAction.BindChargebackGrid:
                {
                    if (AllowFullCC && !GeneralFuncsLib.HasIPForFullCard(this))
                    {
                        parameters.AddLoggedInUserReportingParams();
                        parameters.AddLoggedInUserPrimaryUserID();
                        //parameters.Add(PermissionCodeParam);
                        parameters.Add(new FilterParameter("@RiskClient", false, DbType.Boolean));
                        parameters.AddDecryptDataParams("CardNumber");
                        if (IsShowRoutingAccount)
                            parameters.AddDecryptDataParams("RoutingAccountNumber");
                        spa_name = "spa_cs_GetMerchantRetrievalsChargebacksDetails";
                        WebMethodName = WebSiteConstants.GET_REPORT_FULL_VIEW_METHOD_NAME;
                    }
                    else
                    {
                        if (GeneralFuncsLib.HasIPForFullCard(this))
                        {
                            parameters.AddLoggedInUserReportingParams();
                            parameters.AddLoggedInUserPrimaryUserID();
                            parameters.Add(new FilterParameter("@RiskClient", true, DbType.Boolean));
                            parameters.AddDecryptDataParams("CardNumber");
                            if (IsShowRoutingAccount)
                                parameters.AddDecryptDataParams("RoutingAccountNumber");
                            spa_name = "spa_cs_GetMerchantRetrievalsChargebacksDetails";
                            WebMethodName = GET_CSREPORT_METHOD_NAME;
                        }
                        else
                        {
                            parameters.AddLoggedInUserReportingParams();
                            spa_name = "spa_ms_GetMerchantRetrievalsChargebacksDetails";
                            WebMethodName = GET_REPORT_METHOD_NAME;
                        }
                    }
                    parameters.Add("@Format", "Chargeback", System.Data.DbType.String);
                    parameters.Add("@ReferenceNumber", referenceNumber, System.Data.DbType.String);
                    parameters.AddLanguageID();
                    uxExportChargebacksTop.GridSubTitle = GetLocalResourceObject("RetrievalsChargebacksDetail_aspx_cs_Reference").ToString() + " " + VeraCodeSolution.DoVeraCode(this.SecureQueryString["referenceNumber"].ToString());
                }
                break;
        }
        grid.DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spa_name, ReportServices.ConvertToFilterParamWSArray(parameters) });

    }
    protected override void DoNeedExportConfig(UxExport sender, ExportConfig exportConfig)
    {
        GetGridTitle(true);
        base.DoNeedExportConfig(sender, exportConfig);
        ASGrid grid = sender.Grid as ASGrid;
        if (grid == uxReportGrid)  // for Retrievals
        {
            uxReportGrid.Columns.FindByUniqueName("CardNumber").Visible = false;
            uxReportGrid.Columns.FindByUniqueName("PartialCardNumber").Visible = true;

            if (IsShowRoutingAccount)
            {
                uxReportGrid.Columns.FindByUniqueName("RoutingAccountNumber").Visible = false;
                uxReportGrid.Columns.FindByUniqueName("PartialRoutingACC").Visible = true;
            }

            exportConfig.FileName = GeneralFuncsLib.GetFileName(string.Format(GetLocalResourceObject("RetrievalsChargebacksDetail_aspx_cs_ExportFileName").ToString(), uxMerchantInfo.Text) + SecureQueryString["referenceNumber"].ToString());
            if (sender.ExportButtonType == UxExport.ExportType.CSV)
            {
                exportConfig.ReportHeader = GetLocalResourceObject("RetrievalsChargebacksDetail_aspx_cs_RetrievalDetailHeaderReport").ToString() + " " + Environment.NewLine + GetLocalResourceObject("ltMerchantResource1.Text").ToString() + " " + uxMerchantInfo.Text + Environment.NewLine + GetLocalResourceObject("RetrievalsChargebacksDetail_aspx_cs_Reference").ToString() + SecureQueryString["referenceNumber"].ToString();
            }
            else if (sender.ExportButtonType == UxExport.ExportType.Excel)
            {
                exportConfig.ReportHeader = GetLocalResourceObject("RetrievalsChargebacksDetail_aspx_cs_RetrievalDetailHeaderReport").ToString() + " \r\n" + GetLocalResourceObject("ltMerchantResource1.Text").ToString() + " " + uxMerchantInfo.Text + "\r\n" + GetLocalResourceObject("RetrievalsChargebacksDetail_aspx_cs_Reference").ToString() + SecureQueryString["referenceNumber"].ToString();
            }
        }

        else if (grid == uxReportChargebacks)  // for Chargebacks
        {
            uxReportChargebacks.Columns.FindByUniqueName("CardNumber").Visible = false;
            uxReportChargebacks.Columns.FindByUniqueName("PartialCardNumber").Visible = true;

            if (IsShowRoutingAccount)
            {
                uxReportChargebacks.Columns.FindByUniqueName("RoutingAccountNumber").Visible = false;
                uxReportChargebacks.Columns.FindByUniqueName("PartialRoutingACC").Visible = true;
            }

            exportConfig.FileName = GeneralFuncsLib.GetFileName(string.Format(GetLocalResourceObject("RetrievalsChargebacksDetail_aspx_cs_ChargeBackFilename").ToString(), uxMerchantInfo.Text) + SecureQueryString["referenceNumber"].ToString());
            if (sender.ExportButtonType == UxExport.ExportType.CSV)
            {
                exportConfig.ReportHeader = GetLocalResourceObject("RetrievalsChargebacksDetail_aspx_cs_ChargeBackDetailHeaderReport").ToString() + " " + Environment.NewLine + GetLocalResourceObject("ltMerchantResource1.Text").ToString() + " " + uxMerchantInfo.Text + Environment.NewLine + GetLocalResourceObject("RetrievalsChargebacksDetail_aspx_cs_Reference").ToString() + SecureQueryString["referenceNumber"].ToString();
            }
            else if (sender.ExportButtonType == UxExport.ExportType.Excel)
            {
                exportConfig.ReportHeader = GetLocalResourceObject("RetrievalsChargebacksDetail_aspx_cs_ChargeBackDetailHeaderReport").ToString() + " \r\n" + GetLocalResourceObject("ltMerchantResource1.Text").ToString() + " " + uxMerchantInfo.Text + "\r\n" + GetLocalResourceObject("RetrievalsChargebacksDetail_aspx_cs_Reference").ToString() + SecureQueryString["referenceNumber"].ToString();
            }
        }
    }
    private string BuildUrlForFullCard(string reportType, string encryptedCC, string issueBank, string ReportDate)
    {
        string queryString = BuildSecureQueryString("rt=" + reportType + "&cn=" + Server.UrlEncode(encryptedCC) + "&issue=" + issueBank + "&reportdate=" + ReportDate + "&idx=" + (curIdx + 1) + "&isNotInMif=" + IsNotInMif);
        queryString = ResolveUrl("~/") + "FullCC.aspx?" + queryString;
        return queryString;
    }
}
