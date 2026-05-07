using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Web.Business;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using Telerik.Web.UI;
using System.Linq;

public partial class CardHistoryModal : ReportPage
{
    enum DataBindAction
    {
        BindReportGrid,
        BindAuthGrid,
        BindChargebackGrid,
        BindRetrievalGrid
    }
    string _BatchNumber = string.Empty;
    string _TerminalNumber = string.Empty;
    DateTime _ReportDate = DateTime.Today;
    string _SourceName = string.Empty;
    string _KeyName = string.Empty;
    string _AccountNumber = string.Empty;
    string _PartialCardNumber = string.Empty;
    string _CardInfo = string.Empty;
    string _authIntruderQueryCardGrid = string.Empty;
    string _authIntruderQueryAuthGrid = string.Empty;

    private string AuthIntruderQueryCardGrid
    {
        get
        {

            if (_authIntruderQueryCardGrid.IsNullOrEmpty())
            {
                _authIntruderQueryCardGrid = GeneralFuncsLib.BuildIntruderInfoForQueryStr(
                    uxReportGrid.ID, new string[] { "AuthorizationNumber" });
            }
            return _authIntruderQueryCardGrid;
        }
    }

    private string AuthIntruderQueryAuthGrid
    {
        get
        {

            if (_authIntruderQueryAuthGrid.IsNullOrEmpty())
            {
                _authIntruderQueryAuthGrid = GeneralFuncsLib.BuildIntruderInfoForQueryStr(
                    uxAuthorizationGrid.ID, new string[] { "AuthorizationNumber" });
            }
            return _authIntruderQueryAuthGrid;
        }
    }

    private bool FromG2
    {
        get
        {
            var from = SecureQueryString["from"];
            return !string.IsNullOrEmpty(from) && from.Equals("G2");
        }
    }

    private bool AlwaysPassFullCard
    {
        get
        {
            return GeneralFuncsLib.GetDataOfExtendedSetting("CARD_HISTORY_ALWAYS_PASS_FULL_CARD").ToLower().Equals("true");
        }
    }

    private bool IsNotInMif
    {
        get
        {
            if (SecureQueryString["isNotInMif"].IsNotNullData() && SecureQueryString["isNotInMif"].ToLower().Equals("true"))
                return true;
            return false;
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        //((MasterPageNormal)this.Master).ShowLeftNaviControl = true;
    }

    protected override void PageInitialize()
    {
        this.GridIDs.Add("uxAuthorizationGrid");
        this.GridIDs.Add("uxChargebackReportGrid");
        this.GridIDs.Add("uxRetrievalsReportGrid");
        this.ExporterIDs.Add("uxExporterTop");
        this.ExporterIDs.Add("uxExporterAuthTop");
        this.ExporterIDs.Add("uxExportChargebackTop");
        this.ExporterIDs.Add("uxExporterRetrievalTop");
        this.IsBindDataOnLoad = true;
        base.PageInitialize();
    }
    protected override void DoSwitchView()
    {
        uxReportGrid.Columns.FindByUniqueName("PartialCardNumber").Visible = false;
        uxRetrievalsReportGrid.Columns.FindByUniqueName("PartialCardNumber").Visible = false;
        uxChargebackReportGrid.Columns.FindByUniqueName("PartialCardNumber").Visible = false;
        uxAuthorizationGrid.Columns.FindByUniqueName("PartialCardNumber").Visible = false;

        if (GeneralFuncsLib.GetDataOfExtendedSetting("REMOVE_COLUMN_RTCB") == "true")
        {
            uxChargebackReportGrid.Columns.FindByUniqueName("RepresentedCBAmount").Visible = false;
        }
        else
        {
            uxChargebackReportGrid.Columns.FindByUniqueName("RepresentedCBAmount").Visible = true;
        }
    }
    private void ProcessQueryString()
    {
        _SourceName = SecureQueryString[WebSiteConstants.INTRUDER_SOURCE_PARAM_NAME];
        _KeyName = SecureQueryString[WebSiteConstants.INTRUDER_KEY_PARAM_NAME];
        _AccountNumber = SecureQueryString["cnf"];
        _PartialCardNumber = SecureQueryString["cn"];
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        ProcessQueryString();
        if (IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_CC) || GeneralFuncsLib.CheckCSViewFullCard(this))
        {
            _AccountNumber = SecureQueryString["cnf"];
        }
        else
        {
            _AccountNumber = SecureQueryString["cn"];
        }
        _CardInfo = _AccountNumber;
        if (FromG2)
        {
            _CardInfo = string.Format("<text title='{0}'>{1}</text>", _CardInfo, _PartialCardNumber);
        }
        uxReportTitle.HasShowHierarchy = false;
        uxReportTitle.ReportTitle = VeraCodeSolution.DoVeraCode(GetLocalResourceObject("CardHistoryModal_asp_cs_CardNumber").ToString() + ": " + _CardInfo);
        ((MasterPageNormal)Page.Master).HideHeaderMenu = true;

        //46652 - AW Multi-currency Transaction Display
        GeneralFuncsLib.ShowHideAWTransactionDetail(uxReportGrid);
        GeneralFuncsLib.ShowHideAWTransactionDetail(uxAuthorizationGrid);
        GeneralFuncsLib.ShowHideAWTransactionDetail(uxChargebackReportGrid);

        if (IsNotInMif)
        {
            HideColumnGeographicData(uxReportGrid);
            HideColumnGeographicData(uxAuthorizationGrid);
            HideColumnGeographicData(uxChargebackReportGrid);
            HideColumnGeographicData(uxRetrievalsReportGrid);
            uxReportGrid.Columns.FindByDataField("MerchantName").Visible = false;
        }
        else
        {
            //Sprint 8 - 46716 -CAYAN - Add Geographic Data to CC Transactions
            GeneralFuncsLib.ShowHideColumnGeographicData(uxReportGrid);
            uxReportGrid.Columns.FindByDataField("MerchantName").Visible = GeneralFuncsLib.IsAddGeographicData();
            GeneralFuncsLib.ShowHideColumnGeographicData(uxAuthorizationGrid);
            GeneralFuncsLib.ShowHideColumnGeographicData(uxChargebackReportGrid);
            GeneralFuncsLib.ShowHideColumnGeographicData(uxRetrievalsReportGrid);
        }
    }

    public void HideColumnGeographicData(ASGrid grid)
    {
        grid.Columns.FindByUniqueName("Owner").Visible = false;
        grid.Columns.FindByUniqueName("Contact").Visible = false;
        grid.Columns.FindByUniqueName("City").Visible = false;
        grid.Columns.FindByUniqueName("State").Visible = false;
        grid.Columns.FindByUniqueName("Zip").Visible = false;
        grid.Columns.FindByUniqueName("Phone").Visible = false;
        grid.Columns.FindByUniqueName("MerchantStatus").Visible = false;
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }

    protected override void DoGridNeedDataSource(AS.Controls.Grid.ASGrid sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if (uxReportGrid.Visible && sender == uxReportGrid)
        {
            OnDataBindControls(DataBindAction.BindReportGrid, sender);
        }
        if (uxRetrievalsReportGrid.Visible && sender == uxRetrievalsReportGrid)
        {
            OnDataBindControls(DataBindAction.BindRetrievalGrid, sender);
        }
        if (uxChargebackReportGrid.Visible && sender == uxChargebackReportGrid)
        {
            OnDataBindControls(DataBindAction.BindChargebackGrid, sender);
        }
        if (uxAuthorizationGrid.Visible && sender == uxAuthorizationGrid)
        {
            OnDataBindControls(DataBindAction.BindAuthGrid, sender);
        }
    }
    bool _isExporting = false;
    protected override void OnDataBindControls(Enum type, object sender)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();
        parameters.Add("@BeginDate", DateTime.Now.AddMonths(-18), DbType.DateTime);
        parameters.Add("@EndDate", DateTime.Now, DbType.DateTime);
        parameters.Add("@DateFilterMode", 1, DbType.Int32);
        parameters.Add("@IsNotInMif", IsNotInMif, DbType.Boolean);
        //if (this.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_CC) || GeneralFuncsLib.CheckCSViewFullCard(this))
        //{
        //    parameters.AddEncryptedInputParams("FullCardNumber");
        //    parameters.Add(new FilterParameter("@FullCardNumber", _AccountNumber, DbType.AnsiString));
        //}
        //else if (AlwaysPassFullCard)
        //{
        var fullCardNumber = SecureQueryString["cnf"];
        int lengthCardNumber = 20;
        if (!GeneralFuncsLib.GetDataOfExtendedSetting("CARD_NUMBER_LENGTH").IsNullOrEmpty())
            lengthCardNumber = Convert.ToInt32(GeneralFuncsLib.GetDataOfExtendedSetting("CARD_NUMBER_LENGTH"));
        //If is a real card number: is numeric and < length of card number
        if (fullCardNumber.All(c => char.IsDigit(c)) && fullCardNumber.Length < lengthCardNumber)
        {
            parameters.AddEncryptedInputParams("FullCardNumber");
        }
        parameters.Add(new FilterParameter("@FullCardNumber", fullCardNumber, DbType.AnsiString));
        //}
        //else
        //{
        //    parameters.Add(new FilterParameter("@First6CardNumber", _PartialCardNumber.Substring(0, 6), DbType.AnsiString));
        //    parameters.Add(new FilterParameter("@Last4CardNumber", _PartialCardNumber.Substring(12, 4), DbType.AnsiString));
        //}
        string spaName = string.Empty;
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindReportGrid:
                spaName = "spa_GetCardHistory";
                parameters.AddLoggedInUserPrimaryUserID();
                parameters.AddLanguageID();
                (sender as ASGrid).DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(parameters) });
                break;
            case DataBindAction.BindChargebackGrid:
                if (IsPostBack)
                {
                    parameters.AddLoggedInUserPrimaryUserID();
                    parameters.Add("@Format", "Chargeback", DbType.String);
                    parameters.AddLanguageID();
                    spaName = "spa_GetMerchantRetrievalsChargebacksHistory";
                    (sender as ASGrid).DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(parameters) });
                }
                else
                {
                    (sender as ASGrid).DataSource = new DataTable();
                }
                break;
            case DataBindAction.BindRetrievalGrid:
                if (IsPostBack)
                {
                    parameters.AddLoggedInUserPrimaryUserID();
                    parameters.Add("@Format", "Retrieval", DbType.String);
                    parameters.AddLanguageID();
                    spaName = "spa_GetMerchantRetrievalsChargebacksHistory";
                    (sender as ASGrid).DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(parameters) });
                }
                else
                {
                    (sender as ASGrid).DataSource = new DataTable();
                }
                break;
            case DataBindAction.BindAuthGrid:
                if (IsPostBack)
                {
                    parameters.AddLoggedInUserPrimaryUserID();
                    parameters.AddLanguageID();
                    spaName = "spa_GetAuthorizationDetailHistory";
                    (sender as ASGrid).DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(parameters) });
                }
                else
                {
                    (sender as ASGrid).DataSource = new DataTable();
                }
                break;
        }

    }

    protected void BuildMerchantNumberLink(DataRowView dataRow, GridDataItem dataItem)
    {
        var pramOpenMerchantNumber = string.Format("{0};{1}", "MerchantNumber", dataRow["MerchantNumber"]);
        string urlMerchantNumberLink = string.Format("<a class=\"link\" modalparam=\"{0}\" href=\"#\" onclick=\"OpenDetailModal(this);\">{1}</a>", pramOpenMerchantNumber, dataRow["MerchantNumber"]);
        dataItem["MerchantNumber"].Text = VeraCodeSolution.DoVeraCode(urlMerchantNumberLink);
    }
    protected override void DoItemDataBound(ASGrid sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView dataRow = e.Item.DataItem as DataRowView;
            if (sender == uxReportGrid && sender.Visible)
            {
                // 46716 -CAYAN - Add Geographic Data to CC Transactions
                if (GeneralFuncsLib.IsEnableMerchantLink((SecurePage)Page))
                {
                    BuildMerchantNumberLink(dataRow, dataItem);
                    //string urlMerchantNumberLink = "<a class=\"link\" href=\"#\" onclick=\"OpenMerchantNumber('" + dataRow["MerchantNumber"] + "'); return false;\">" + dataRow["MerchantNumber"] + "</a>";
                    //dataItem["MerchantNumber"].Text = VeraCodeSolution.DoVeraCode(urlMerchantNumberLink);
                }

                if (!SessionManager.CurrentUser.ASClient.Equals(WebSiteConstants.IPMT_CLIENT))
                {
                    var pramOpenBatchDetail = string.Format("{0};{1};{2};{3};{4}", "BatchDetail", dataRow["MerchantNumber"], dataRow["ReportDate"], dataRow["BatchNumber"], dataRow["TerminalNumber"]);
                    string url = string.Format("<a class=\"link\" modalparam=\"{0}\" href=\"#\" onclick=\"OpenDetailModal(this);\">{1}</a>", pramOpenBatchDetail, dataRow["BatchNumber"]);
                    dataItem["BatchNumber"].Text = VeraCodeSolution.DoVeraCode(url);
                }
                if (!String.IsNullOrEmpty(dataRow["AuthorizationNumber"].ToString().Trim()))
                {
                    if (SecureQueryString["isRisk"] != null && SecureQueryString["isRisk"] == "1")
                    {
                        if (!SessionManager.CurrentUser.ASClient.Equals(WebSiteConstants.IPMT_CLIENT))
                        {
                            var pramOpenCardHistory = string.Format("{0};{1};{2};{3}", "CardHistory", dataRow["AuthorizationNumber"].ToString(), dataRow["MerchantNumber"].ToString(), dataRow["TransactionDate"].ToString());
                            dataItem["AuthorizationNumber"].Text = VeraCodeSolution.DoVeraCode(string.Format("<a href=\"#\" modalparam=\"{0}\" style=\"cursor:pointer\" onclick=\"OpenDetailModal(this);\">{1}</a>", pramOpenCardHistory, dataItem["AuthorizationNumber"].Text));
                        }

                    }
                    else
                    {
                        if (!SessionManager.CurrentUser.ASClient.Equals(WebSiteConstants.IPMT_CLIENT))
                            dataItem["AuthorizationNumber"].Text = VeraCodeSolution.DoVeraCode(
                                GeneralFuncsLib.BuildAuthUrlPopupModal(
                                 (SecurePage)Page, dataItem["AuthorizationNumber"].Text,
                                 dataRow["AuthorizationNumber"], AuthIntruderQueryCardGrid,
                                 dataRow["MerchantNumber"].ToString(), 1, IsNotInMif)
                                 );
                    }
                }

                string cardDesc = VeraCodeSolution.DoVeraCode(dataRow["CardDescription"].ToString());
                if (!String.IsNullOrEmpty(cardDesc))
                {
                    dataItem["CardType"].ToolTip = cardDesc;
                }
                dataItem["TransCode"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["TransactionCode"].ToString());
                dataItem["ForcedTrans"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["ForcedTransDescription"].ToString());
                dataItem["ForeignCard"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["CountryName"].ToString());
                dataItem["KeyedEntry"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["EntryModeDescription"].ToString());
            }
            if (sender == uxAuthorizationGrid && sender.Visible)
            {
                if (!String.IsNullOrEmpty(dataRow["AuthorizationNumber"].ToString().Trim()))
                {
                    if (SecureQueryString["isRisk"] != null && SecureQueryString["isRisk"] == "1")
                    {
                        var isMCF = GeneralFuncsLib.IsMCFRisk();
                        if (!SessionManager.CurrentUser.ASClient.Equals(WebSiteConstants.IPMT_CLIENT))
                        {
                            var pramOpenCardHistory = string.Format("{0};{1};{2};{3}", "CardHistory", dataRow["AuthorizationNumber"].ToString(), dataRow["MerchantNumber"].ToString(), dataRow["TransactionDate"].ToString());

                            dataItem["AuthorizationNumber"].Text = VeraCodeSolution.DoVeraCode(string.Format("<a href=\"#\" modalparam=\"{0}\" style=\"cursor:pointer\" onclick=\"OpenDetailModal(this);\">{1}</a>",
                            pramOpenCardHistory, dataItem["AuthorizationNumber"].Text));
                        }

                    }
                    else
                    {
                        if (!SessionManager.CurrentUser.ASClient.Equals(WebSiteConstants.IPMT_CLIENT))
                            dataItem["AuthorizationNumber"].Text = VeraCodeSolution.DoVeraCode(GeneralFuncsLib.BuildAuthUrlPopupModalChild(
                                   (SecurePage)Page, dataRow["AuthorizationNumber"],
                                   dataRow["MerchantNumber"].ToString(), 1, AuthIntruderQueryAuthGrid,
                                   dataItem["AuthorizationNumber"].Text, true, IsNotInMif));
                    }
                }
                string cardDesc = VeraCodeSolution.DoVeraCode(dataRow["CardDescription"].ToString().Trim());
                if (!String.IsNullOrEmpty(cardDesc))
                {
                    dataItem["CardType"].ToolTip = cardDesc;
                }
                dataItem["TransCode"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["TransactionCode"].ToString());
                dataItem["Approved"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["ApprovedDescription"].ToString());
                dataItem["ResponseCode"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["ResponseCodeDescription"].ToString());
                dataItem["KeyedEntry"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["EntryModeDescription"].ToString());
                dataItem["AVS"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["AVSDescription"].ToString());
                if (dataRow["Approved"].ToString() == "D")
                {
                    dataItem["Approved"].Text = string.Format(WebSiteConstants.DECLINED_TEXT, dataItem["Approved"].Text);
                }

                // 46716 -CAYAN - Add Geographic Data to CC Transactions
                if (GeneralFuncsLib.IsEnableMerchantLink((SecurePage)Page))
                {
                    BuildMerchantNumberLink(dataRow, dataItem);
                    //string urlMerchantNumberLink = "<a class=\"link\" href=\"#\" onclick=\"OpenMerchantNumber('" + dataRow["MerchantNumber"] + "'); return false;\">" + dataRow["MerchantNumber"] + "</a>";
                    //dataItem["MerchantNumber"].Text = VeraCodeSolution.DoVeraCode(urlMerchantNumberLink);
                }

            }
            if (sender == uxChargebackReportGrid && sender.Visible)
            {
                string cardDesc = dataRow["CardTypeDesc"].ToString();
                if (!String.IsNullOrEmpty(cardDesc))
                    dataItem["CardType"].ToolTip = cardDesc;
                dataItem["ReasonCode"].ToolTip = dataRow["ReasonCodeDesc"].ToString();

                // 46716 -CAYAN - Add Geographic Data to CC Transactions
                if (GeneralFuncsLib.IsEnableMerchantLink((SecurePage)Page))
                {
                    BuildMerchantNumberLink(dataRow, dataItem);
                    //string urlMerchantNumberLink = "<a class=\"link\" href=\"#\" onclick=\"OpenMerchantNumber('" + dataRow["MerchantNumber"] + "'); return false;\">" + dataRow["MerchantNumber"] + "</a>";
                    //dataItem["MerchantNumber"].Text = VeraCodeSolution.DoVeraCode(urlMerchantNumberLink);
                }

            }
            if (sender == uxRetrievalsReportGrid && sender.Visible)
            {
                string cardDesc = dataRow["CardTypeDesc"].ToString();
                if (!String.IsNullOrEmpty(cardDesc))
                    dataItem["CardType"].ToolTip = cardDesc;
                dataItem["ReasonCode"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["ReasonCodeDesc"].ToString());
                //dataItem["ForcedTrans"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["ForcedTransDescription"].ToString());
                //dataItem["ForeignCard"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["CountryName"].ToString());

                // 46716 -CAYAN - Add Geographic Data to CC Transactions
                if (GeneralFuncsLib.IsEnableMerchantLink((SecurePage)Page))
                {
                    BuildMerchantNumberLink(dataRow, dataItem);
                    //string urlMerchantNumberLink = "<a class=\"link\" href=\"#\" onclick=\"OpenMerchantNumber('" + dataRow["MerchantNumber"] + "'); return false;\">" + dataRow["MerchantNumber"] + "</a>";
                    //dataItem["MerchantNumber"].Text = VeraCodeSolution.DoVeraCode(urlMerchantNumberLink);
                }
            }
        }
    }
    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, AS.Controls.Exporter.ExportConfig exportConfig)
    {
        //46652 - AW Multi-currency Transaction Display
        GeneralFuncsLib.ShowHideAWTransactionDetail(uxReportGrid);
        GeneralFuncsLib.ShowHideAWTransactionDetail(uxAuthorizationGrid);
        GeneralFuncsLib.ShowHideAWTransactionDetail(uxChargebackReportGrid);

        base.DoNeedExportConfig(sender, exportConfig);
        uxReportGrid.Columns.FindByUniqueName("PartialCardNumber").Visible = true;

        uxRetrievalsReportGrid.Columns.FindByUniqueName("PartialCardNumber").Visible = true;

        uxChargebackReportGrid.Columns.FindByUniqueName("PartialCardNumber").Visible = true;

        uxAuthorizationGrid.Columns.FindByUniqueName("PartialCardNumber").Visible = true;
        if (sender == uxExporterTop)
        {
            exportConfig.ReportHeader = uxExporterTop.GridTitle;
            exportConfig.FileName = GeneralFuncsLib.GetFileName(
                string.Format(GetLocalResourceObject("CardHistoryModal_asp_cs_CardFileName").ToString(), uxExporterTop.GridTitle, _PartialCardNumber));
        }
        if (sender == uxExporterAuthTop)
        {
            exportConfig.ReportHeader = uxExporterAuthTop.GridTitle;
            exportConfig.FileName = GeneralFuncsLib.GetFileName(
                string.Format(GetLocalResourceObject("CardHistoryModal_asp_cs_CardFileName").ToString(), uxExporterAuthTop.GridTitle, _PartialCardNumber));
        }
        if (sender == uxExportChargebackTop)
        {
            exportConfig.FileName = GeneralFuncsLib.GetFileName(
                string.Format(GetLocalResourceObject("CardHistoryModal_asp_cs_CardFileName").ToString(), uxExportChargebackTop.GridTitle, _PartialCardNumber));
            exportConfig.ReportHeader = uxExportChargebackTop.GridTitle;
        }
        if (sender == uxExporterRetrievalTop)
        {
            exportConfig.ReportHeader = uxExporterRetrievalTop.GridTitle;
            exportConfig.FileName = GeneralFuncsLib.GetFileName(
                string.Format(GetLocalResourceObject("CardHistoryModal_asp_cs_CardFileName").ToString(), uxExporterRetrievalTop.GridTitle, _PartialCardNumber));
        }
    }

    protected void uxShowAuth_Click(object sender, EventArgs e)
    {
        uxAuthorizationGrid.Rebind();
        this.AjaxAddResponseScript("LoadChargeback();");
    }

    protected void uxShowChargeback_Click(object sender, EventArgs e)
    {
        uxChargebackReportGrid.Rebind();
        if (SessionManager.CurrentClient != WebSiteConstants.ALLIEDWALLET_CLIENT)
        {
            this.AjaxAddResponseScript("LoadRetrieval();");
        }
    }

    protected void uxShowRetrieval_Click(object sender, EventArgs e)
    {
        uxRetrievalsReportGrid.Rebind();
        this.AjaxAddResponseScript(" $(\"#uxPanelRetrieval\").show();");
    }

    protected void uxShowModalDetail_Click(object sender, EventArgs e)
    {
        string paramValue = hdShowDetailModal.Value;
        string merchantNumberCM = string.Empty;
        string reportDateCM = string.Empty;
        string batchNumberCM = string.Empty;
        string terminalNumberCM = string.Empty;
        string authorizationNumberCM = string.Empty;
        string transactionDateCM = string.Empty;
        string func = string.Empty;
        if (!string.IsNullOrEmpty(paramValue))
        {
            var paramList = paramValue.ToString().Split(';');
            var mode = paramList.FirstOrDefault();
            switch (mode)
            {
                case "BatchDetail":
                    merchantNumberCM = paramList[1];
                    reportDateCM = paramList[2];
                    batchNumberCM = paramList[3];
                    terminalNumberCM = paramList[4];
                    string urlBatchDetail = "BatchDetailModal.aspx?" + this.BuildSecureQueryString("MerchantNumber=" + merchantNumberCM + "&ReportDate=" + reportDateCM +
                            "&BatchNumber=" + batchNumberCM + "&TerminalNumber=" + terminalNumberCM + "&isNotInMif=" + IsNotInMif);
                    if (SecureQueryString["isRisk"] != null && SecureQueryString["isRisk"] == "1")
                    {
                        string url = "Risk_MCF/rm_MCF_BatchDetailsModal.aspx?";
                        urlBatchDetail = url + this.BuildSecureQueryString("merch=" + merchantNumberCM + "&ReportDate=" + reportDateCM + "&BatchNumber=" + batchNumberCM + "&TerminalNumber=" + terminalNumberCM);
                    }

                    func = "ShowPopupModal('" + urlBatchDetail + "','auto'); return false;";

                    break;

                case "MerchantNumber":
                    merchantNumberCM = paramList[1];
                    string urlMerchantNumber = "MerchantProfile.aspx?" + this.BuildSecureQueryString(string.Format("MerchantNumber={0}&IsHideMenu=true", merchantNumberCM));
                    func = "openPopupWindow('" + urlMerchantNumber + "','auto'); return false;";

                    break;

                case "CardHistory":
                    if (!string.IsNullOrEmpty(paramValue))
                    {
                        authorizationNumberCM = paramList[1];
                        merchantNumberCM = paramList[2];
                        transactionDateCM = paramList[3];
                    }

                    string merchantName = GeneralFuncsLib.GetMerchantName(merchantNumberCM);
                    func = GeneralFuncsLib.BuildRskAuthFunc((SecurePage)Page, authorizationNumberCM, merchantNumberCM, merchantName,
                      transactionDateCM, 1, authorizationNumberCM, false, false, true) + " return false;";

                    break;
            }

            this.AjaxAddResponseScript(func);

        }
    }
}
