using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Web.Business;
using System;
using System.Data;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using AS.Utilities;

[PagePermission("RskPort,MSRskPort")]
public partial class rm_MCF_PortfolioCredits : ReportPage
{
    #region Enums

    enum DataBindAction
    {
        BindPortfolioGrid,
        InitFilteringOption
    }

    enum PostBackAction
    {
        ChangeMerchantWorked,
        SearchPortfolio,
        SelectedAccountNumber
    }
    #endregion

    #region Const

    const string MERCHANT_NUMBER = "MerchantNumber";
    const string MERCHANT_NAME = "MerchantName";
    const string CARD_NUMBER = "PartialAccountNumber";
    const string BATCH_NUMBER = "BatchNumber";
    const string IMAGE = "<a class=\"atab\"href=\"#\" style=\"cursor:pointer\" onclick=\" return ShowPopupModal('{0}','auto');\"><img src='../res/images/information.gif' border=\"0\"/></a>&nbsp; &nbsp;";
    private const string MATCHED_CODE = "MatchCode";
    private const string MATCHED_NAME = "MatchName";
    #endregion Const

    #region Fields

    private bool _isExporting = false;
    private string _cardSearchIntruderQuery = string.Empty;
    private string _merchantIntruderQuery = string.Empty;
    private string _batchDetailIntruderQuery = string.Empty;

    #endregion Fields

    #region Properties

    private WebSiteEnums.MerchantWorkedType MerchantWorkType
    {
        get
        {
            WebSiteEnums.MerchantWorkedType merchantWorkType = WebSiteEnums.MerchantWorkedType.All;
            if (this.optAllMerchant.Checked)
                merchantWorkType = WebSiteEnums.MerchantWorkedType.All;
            else if (this.optNotWorkedMerchant.Checked)
                merchantWorkType = WebSiteEnums.MerchantWorkedType.NotWorked;
            else if (this.optWorkedMerchant.Checked)
                merchantWorkType = WebSiteEnums.MerchantWorkedType.Worked;
            return merchantWorkType;
        }
        set
        {
            optAllMerchant.Checked = optNotWorkedMerchant.Checked = optWorkedMerchant.Checked = false;
            if (value == WebSiteEnums.MerchantWorkedType.All)
                this.optAllMerchant.Checked = true;
            else if (value == WebSiteEnums.MerchantWorkedType.NotWorked)
                this.optNotWorkedMerchant.Checked = true;
            else if (value == WebSiteEnums.MerchantWorkedType.Worked)
                this.optWorkedMerchant.Checked = true;
        }
    }

    private string MerchantIntruderQuery
    {
        get
        {
            if (_merchantIntruderQuery.IsNullOrEmpty())
            {
                _merchantIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(
                    uxPortfolioGrid.ID, new string[] { MERCHANT_NUMBER });
            }
            return _merchantIntruderQuery;
        }
    }

    private string BatchDetailIntruderQuery
    {
        get
        {
            if (_batchDetailIntruderQuery.IsNullOrEmpty())
            {
                _batchDetailIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(
                    uxPortfolioGrid.ID, new string[] { BATCH_NUMBER });
            }
            return _batchDetailIntruderQuery;
        }
    }

    private string CardSearchIntruderQuery
    {
        get
        {
            if (_cardSearchIntruderQuery.IsNullOrEmpty())
            {
                GeneralFuncsLib.BuildIntruderInfoForQueryStr(uxPortfolioGrid.ID, new string[] { "AccountNumber" });
            }
            return _cardSearchIntruderQuery;
        }
    }

    #endregion Properties

    #region Methods

    private void InitReportFilter()
    {
        if (SavedReportFilterValue == null)
        {
            SavedReportFilterValue = new AS.Web.UI.Controls.HierarchyFilterValue();
            SavedReportFilterValue.Value = string.Empty;
            SavedReportFilterValue.ID = GeneralFuncsLib.GetMerchantHierarchyInfo().HierarchyID;
            SavedReportFilterValue.HierarchyMode = GeneralFuncsLib.GetMerchantHierarchyInfo().HierarchyMode;
            SavedReportFilterValue.DateOption = AS.Web.UI.Controls.DateOptionMode.DateRange;
            SavedReportFilterValue.DateOptionValue.From = uxBeginDate.SelectedDate.Value;
            SavedReportFilterValue.DateOptionValue.To = uxEndDate.SelectedDate.Value;
        }
        else
        {
            SavedReportFilterValue.Value = string.Empty;
            SavedReportFilterValue.ID = GeneralFuncsLib.GetMerchantHierarchyInfo().HierarchyID;
            SavedReportFilterValue.HierarchyMode = GeneralFuncsLib.GetMerchantHierarchyInfo().HierarchyMode;
            SavedReportFilterValue.DateOption = AS.Web.UI.Controls.DateOptionMode.DateRange;
            SavedReportFilterValue.DateOptionValue.From = uxBeginDate.SelectedDate.Value;
            SavedReportFilterValue.DateOptionValue.To = uxEndDate.SelectedDate.Value;
        }
    }

    protected override void PageInitialize()
    {
        if (IsIntruderDetected)
            return;

        this.ExporterIDs.Add("uxExporterTop");
        this.GridIDs.Add("uxPortfolioGrid");
        base.PageInitialize();
    }

    protected void SetCheckMatch(int isCheck)
    {
        switch (isCheck)
        {
            case -1:
                uxOption.SelectedIndex = 0;
                break;
            case 0:
                uxOption.SelectedIndex = 3;
                break;
            case 1:
                uxOption.SelectedIndex = 1;
                break;
            case 2:
                uxOption.SelectedIndex = 2;
                break;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        IsBindDataOnLoad = true;
        if (!IsPostBack)
        {
            Int64 amount = 50;

            if (RiskSessionManager.PortfolioReferer != string.Empty)
            {
                string[] portfolioReferer = RiskSessionManager.PortfolioReferer.Split(',');
                DateTime from = new DateTime(
                    Int32.Parse(portfolioReferer[1].Split('/')[2]),
                    Int32.Parse(portfolioReferer[1].Split('/')[0]),
                    Int32.Parse(portfolioReferer[1].Split('/')[1]));
                DateTime to = new DateTime(
                    Int32.Parse(portfolioReferer[2].Split('/')[2]),
                    Int32.Parse(portfolioReferer[2].Split('/')[0]),
                    Int32.Parse(portfolioReferer[2].Split('/')[1]));
                ViewState["amount"] = portfolioReferer[0];
                ViewState["from"] = from;
                ViewState["to"] = to;
                ViewState["checkmatch"] = Int32.Parse(portfolioReferer[3]);
                ViewState["isworked"] = portfolioReferer[4];
                uxTransactionAmount.Text = VeraCodeSolution.DoVeraCode(portfolioReferer[0]);
                uxBeginDate.SelectedDate = from;
                uxEndDate.SelectedDate = to;
                SetCheckMatch(Int32.Parse(portfolioReferer[3]));
                MerchantWorkType = (WebSiteEnums.MerchantWorkedType)Enum.Parse(
                    typeof(WebSiteEnums.MerchantReportType),
                    ViewState["isworked"].ToString());
            }
            else
            {
                DateTime dateValue = DateTime.Now.Date;
                uxTransactionAmount.Text = VeraCodeSolution.DoVeraCode(
                    FormatTransactionAmount(amount));
                uxBeginDate.SelectedDate = dateValue;
                uxEndDate.SelectedDate = dateValue;
                SetCheckMatch(-1);
                ViewState["amount"] = uxTransactionAmount.Text;
                ViewState["from"] = uxBeginDate.SelectedDate.Value;
                ViewState["to"] = uxEndDate.SelectedDate.Value;
                ViewState["checkmatch"] = CheckMatch();
                ViewState["isworked"] = MerchantWorkType;
            }
            uxBeginDate.MaxDate = uxEndDate.MaxDate = DateTime.Now;
        }
        //46652 - AW Multi-currency Transaction Display
        GeneralFuncsLib.ShowHideAWTransactionDetail(uxPortfolioGrid);
    }

    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender,
        AS.Controls.Exporter.ExportConfig exportConfig)
    {
        //46652 - AW Multi-currency Transaction Display
        GeneralFuncsLib.ShowHideAWTransactionDetail(uxPortfolioGrid);

        _isExporting = true;
        uxPortfolioGrid.Columns.FindByUniqueName("PartialAccountNumber").Visible = true;
        uxPortfolioGrid.Columns.FindByUniqueName("AccountNumber").Visible = false;
        uxPortfolioGrid.Columns.FindByUniqueName("CheckBoxColumn").Visible = false;
        uxPortfolioGrid.Columns.FindByUniqueName("Worked").Visible = false;

        base.DoNeedExportConfig(sender, exportConfig);
        exportConfig.ReportHeader = uxExporterTop.GridHeader;
        exportConfig.FileName = uxPortfolioGrid.GridName;
    }

    protected override void DoGridNeedDataSource(ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        if (sender == uxPortfolioGrid && uxPortfolioGrid.Visible)
        {
            OnDataBindControls(DataBindAction.BindPortfolioGrid, sender);
        }
    }

    protected override void DoSwitchView()
    {
        if (GeneralFuncsLib.CheckCSViewFullCard((SecurePage)Page))
        {
            uxPortfolioGrid.Columns.FindByUniqueName("AccountNumber").Visible = true;
            uxPortfolioGrid.Columns.FindByUniqueName("PartialAccountNumber").Visible = false;
        }
        else
        {
            uxPortfolioGrid.Columns.FindByUniqueName("AccountNumber").Visible = false;
            uxPortfolioGrid.Columns.FindByUniqueName("PartialAccountNumber").Visible = true;
        }
    }

    protected override void DoItemDataBound(ASGrid sender, GridItemEventArgs e)
    {
        if (IsIntruderDetected)
            return;

        if (e.Item is GridDataItem)
        {
            DataRowView dataRow = e.Item.DataItem as DataRowView;
            string cardNumber = dataRow[CARD_NUMBER].ToString();
            string batchNumber = dataRow[BATCH_NUMBER].ToString();
            string recordID = dataRow["RecordID"].ToString();
            string reportDate = dataRow["ReportDate"].ToString();

            GridDataItem dataItem = e.Item as GridDataItem;

            string accountQuery = string.Empty;

            string queryString = BuildSecureQueryString("cn=" + dataRow["PartialAccountNumber"] + "&cnf=" + dataRow["AccountNumber"] + "&merch=" + dataRow[MERCHANT_NUMBER].ToString() + "&isRisk=1");
            string urlCardDetail = ResolveUrl("~/") + "CardHistoryModal.aspx?" + queryString;
            string urlCard = "<a class=\"link\" href=\"javascript:void(0)\" onclick=\"openPopupWindow('" + urlCardDetail + "','CardHistoryWindow'); return false;\">";

            if (GeneralFuncsLib.CheckCSViewFullCard((SecurePage)Page))
            {
                dataItem["AccountNumber"].Text = VeraCodeSolution.DoVeraCode(urlCard + dataRow["AccountNumber"].ToString() + "</a>");
            }
            else
            {
                accountQuery = this.BuildSecureQueryString(string.Format("cn={0}&returl={1}&retlabel={2}{3}", dataRow["PartialAccountNumber"].ToString(), ResolveUrl("~/risk_MCF/") + "rm_MCF_PortfolioCredits.aspx", GetLocalResourceObject("rm_PortfolioCredits_aspx_cs_String1").ToString(), CardSearchIntruderQuery));
                if (GeneralFuncsLib.HasIPForFullCard((SecurePage)this))
                {
                    dataItem["PartialAccountNumber"].Text = VeraCodeSolution.DoVeraCode(string.Format(IMAGE, BuildUrlForFullCard(dataRow["RecordId"].ToString(), dataRow["IssuingBank"].ToString(), dataRow["ReportDate"].ToString())) + VeraCodeSolution.DoVeraCode(urlCard + dataRow["PartialAccountNumber"].ToString() + "</a>"));
                }
                else
                {
                    dataItem["PartialAccountNumber"].Text = VeraCodeSolution.DoVeraCode(urlCard + dataRow["PartialAccountNumber"].ToString() + "</a>");
                }
            }

            foreach (GridColumn col in uxPortfolioGrid.MasterTableView.RenderColumns)
            {
                // Merchant Name Column
                string url = string.Format("<a class=\"link\" href='#' style=\"cursor:pointer\" onclick=\"MerchantNumber_hyperLink_Click('{0}',{1});return false;\">{2}</a>", dataRow[MERCHANT_NUMBER], recordID, dataRow[MERCHANT_NAME].ToString());
                dataItem[MERCHANT_NAME].Text = VeraCodeSolution.GetOutputHtmlString(url);

                dataItem[MERCHANT_NAME].ToolTip = VeraCodeSolution.DoVeraCode(dataRow[MERCHANT_NUMBER].ToString());

                dataItem["CardTypeCode"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["LongDescription"].ToString());

                // Batch Number column
                url = string.Format(
                    "<a class=\"link\" href='#' style=\"cursor:pointer\" onclick=\"BatchNumber_hyperLink_Click('{0}','{1}', '{2}', '{3}', '{4}');return false;\">{5}</a>",
                    dataRow[MERCHANT_NUMBER], recordID, batchNumber,
                    dataRow["TerminalNumber"], reportDate, dataRow[BATCH_NUMBER].ToString());
                dataItem[BATCH_NUMBER].Text = dataRow[BATCH_NUMBER] != null && !string.IsNullOrEmpty(dataRow[BATCH_NUMBER].ToString()) ? VeraCodeSolution.GetOutputHtmlString(url) : string.Empty;



               // dataItem["MatchToSales"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["MatchToSalesDescription"].ToString());

                // 46807
                dataItem[MATCHED_CODE].ToolTip = VeraCodeSolution.DoVeraCode(dataRow[MATCHED_NAME].ToString());

                string matchedCode = dataRow[MATCHED_CODE].ToString();

                if (matchedCode.Equals("P", StringComparison.OrdinalIgnoreCase) || matchedCode.Equals("CP", StringComparison.OrdinalIgnoreCase))
                {
                    dataItem[MATCHED_CODE].Text = GeneralFuncsLib.FormatBorderText(matchedCode, Color.Blue);
                }
                else if (matchedCode.Equals("U", StringComparison.OrdinalIgnoreCase) || matchedCode.Equals("SC", StringComparison.OrdinalIgnoreCase))
                {
                    dataItem[MATCHED_CODE].Text = GeneralFuncsLib.FormatBorderText(matchedCode, Color.Red);
                }
                else if (matchedCode.Equals("M", StringComparison.OrdinalIgnoreCase) || matchedCode.Equals("C", StringComparison.OrdinalIgnoreCase))
                {
                    dataItem[MATCHED_CODE].Text = GeneralFuncsLib.FormatBorderText(matchedCode, "#3fbf00".ToColor());
                }


            }

            GridDataItem item = (GridDataItem)e.Item;

            Control ctrl = item["CheckBoxColumn"].FindControl("cidMerchantWorked");
            if (ctrl != null)
            {
                HtmlInputCheckBox chkBox = ctrl as HtmlInputCheckBox;
                chkBox.Checked = item["Worked"].Text.Trim() == "1";
                var color = System.Drawing.Color.Transparent;
                var accountNumber = item["AccountNumber"];
                var partialAccountNumber = item["PartialAccountNumber"];
                if (chkBox.Checked)
                {
                    color = System.Drawing.Color.LightPink;
                }
                accountNumber.Text = GeneralFuncsLib.FormatBorderText(accountNumber.Text, color);
                partialAccountNumber.Text = GeneralFuncsLib.FormatBorderText(partialAccountNumber.Text, color);
                chkBox.Disabled = !GeneralFuncsLib.ReadOnlyRiskManagementEnableStatus((SecurePage)Page);
            }
        }
    }

    protected int CheckMatch()
    {
        int result = -1;
        switch (uxOption.SelectedIndex)
        {
            case 0:
                result = -1;
                break;
            case 1:
                result = 1;
                break;
            case 2:
                result = 2;
                break;
            case 3:
                result = 0;
                break;
        }
        return result;
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindPortfolioGrid:
                {
                    string spaName = "spa_RM_MCF_GetPortfolioCredits";
                    FilterParameterCollection parames = new FilterParameterCollection();
                    parames.AddLoggedInUserReportingParams(false);
                    parames.Add(new FilterParameter("@BeginDate", (DateTime)ViewState["from"], DbType.Date));
                    parames.Add(new FilterParameter("@EndDate", (DateTime)ViewState["to"], DbType.Date));
                    parames.Add(new FilterParameter("@TransAmount", (string)ViewState["amount"], DbType.AnsiString));
                    parames.Add(new FilterParameter("@CheckMatch", CheckMatch(), DbType.Int32));
                    parames.Add(new FilterParameter("@IsWorked", MerchantWorkType, DbType.Int32));
                    parames.AddLanguageID();

                    if (GeneralFuncsLib.CheckCSViewFullCard((SecurePage)Page))
                    {
                        parames.AddDecryptDataParams("AccountNumber", _isExporting);
                    }
                    ((ASGrid)sender).DataSourceInvoker = new ASFuncInvoker(
                        WebServices.RiskServices,
                        WebSiteConstants.GET_REPORT_METHOD_NAME,
                        new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(parames) });
                }
                break;
            case DataBindAction.InitFilteringOption:
                {
                    Int64 amount = 50;
                    Int64.TryParse(VeraCodeSolution.ValidateResponseData(uxTransactionAmount.Text.Trim()), out amount);
                    uxTransactionAmount.Text = VeraCodeSolution.DoVeraCode(FormatTransactionAmount(amount));

                    if (uxBeginDate.SelectedDate == null && uxBeginDate.InvalidTextBoxValue.Trim() == string.Empty)
                    {
                        uxBeginDate.SelectedDate = DateTime.Now.Date;
                    }
                    else if (uxBeginDate.SelectedDate == null && uxBeginDate.InvalidTextBoxValue.Trim() != string.Empty)
                    {
                        uxBeginDate.SelectedDate = Convert.ToDateTime(uxBeginDate.InvalidTextBoxValue.Trim());
                    }
                    if (uxEndDate.SelectedDate == null && uxEndDate.InvalidTextBoxValue.Trim() == string.Empty)
                    {
                        uxEndDate.SelectedDate = DateTime.Now.Date;
                    }
                    else if (uxEndDate.SelectedDate == null && uxEndDate.InvalidTextBoxValue.Trim() == string.Empty)
                    {
                        uxEndDate.SelectedDate = Convert.ToDateTime(uxEndDate.InvalidTextBoxValue.Trim());
                    }
                }
                break;
        }
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.SearchPortfolio:
                {
                    OnDataBindControls(DataBindAction.InitFilteringOption);

                    ViewState["amount"] = uxTransactionAmount.Text;
                    ViewState["from"] = uxBeginDate.SelectedDate.Value;
                    ViewState["to"] = uxEndDate.SelectedDate.Value;
                    ViewState["checkmatch"] = CheckMatch();
                    ViewState["isworked"] = (int)MerchantWorkType;
                    InitReportFilter();

                    RiskSessionManager.PortfolioReferer = string.Format("{0},{1},{2},{3},{4}",
                        ViewState["amount"].ToString(),
                        ((DateTime)ViewState["from"]).ToShortDateString(),
                        ((DateTime)ViewState["to"]).ToShortDateString(),
                        ViewState["checkmatch"].ToString(),
                        ViewState["isworked"].ToString());
                    uxPortfolioGrid.CurrentPageIndex = 0;
                    uxPortfolioGrid.Rebind();

                    string activity = string.Format(
                        GetLocalResourceObject("rm_PortfolioCredits_aspx_cs_String2").ToString(),
                        ViewState["amount"].ToString(),
                        ((DateTime)ViewState["from"]).ToShortDateString(),
                        ((DateTime)ViewState["to"]).ToShortDateString());
                    GeneralFuncsLib.SaveUserActivity("", activity, true);
                }
                break;
            case PostBackAction.ChangeMerchantWorked:
                {
                    string[] args = this.uxHiddenMerchantWorked.Value.Split(';');
                    bool status = Convert.ToBoolean(args[0]);
                    string merchantNumber = args[1];
                    int recordId = int.Parse(args[2]);

                    if (RiskSessionManager.IsAutoCheckWork)
                    {
                        UpdatePortfolioCredits(recordId, status);
                    }

                    string url = string.Empty;
                    if (args.Length == 3)
                    {
                        if (merchantNumber != "dummymerchantnumber")
                        {
                            RiskSessionManager.RiskReportReferrer = null;
                            string riskReportIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(this.ID, new string[] { "MerchantNumber" });
                            url = "rm_MCF_RiskReport.aspx?" + this.BuildSecureQueryString(string.Format("merchantnumber={0}&IsPopup={1}{2}", merchantNumber, true, riskReportIntruderQuery));
                            AjaxAddResponseScript("openPopupWindow('" + url + "','RiskReport');");
                        }
                        else
                        {
                            return;
                        }
                    }
                    else if (args.Length == 6)
                    {
                        string batchNumber = args[3];
                        string terminalNumber = args[4];
                        string reportDate = args[5];

                        InitReportFilter();
                        SavedReportFilterValue.Value = merchantNumber;

                        url = this.BuildSecureQueryString(string.Format(
                            "BatchNumber={0}&merch={1}&TerminalNumber={2}&ReportDate={3}{4}",
                            batchNumber, merchantNumber, terminalNumber, reportDate, this.BatchDetailIntruderQuery));
                        url = string.Format("rm_MCF_BatchDetailsModal.aspx?{0}", url);
                        this.AjaxAddResponseScript(string.Format("ShowPopupModal('{0}','auto');", url));
                    }
                    else
                    {
                        return;
                    }
                }
                break;
            case PostBackAction.SelectedAccountNumber:
                {
                    string queryStr = uxHdAccount.Value;
                    string merchantnumber = uxHiddenMerchantNumber.Value;
                    InitReportFilter();
                    SavedReportFilterValue.Value = merchantnumber;
                    Response.Redirect(queryStr, true);
                }
                break;
        }
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static void ChangeMerchantWorkedStatus(string merchantInfo)
    {
        string[] args = merchantInfo.Split(';');
        bool status = Convert.ToBoolean(args[0]);
        string merchantNumber = args[1];
        int recordId = int.Parse(args[2]);
        rm_MCF_PortfolioCredits p = new rm_MCF_PortfolioCredits();

        p.UpdatePortfolioCredits(recordId, status);
    }

    private void UpdatePortfolioCredits(int recordID, bool status)
    {
        if (GeneralFuncsLib.ReadOnlyRiskManagementEnableStatus((SecurePage)Page))
        {
            FilterParameterCollection paramesIn = new FilterParameterCollection();
            paramesIn.AddLoggedInUserReportingParams(false);
            WebServices.RiskServices.UpdatePortfolioCredits(paramesIn, recordID, status, true);
        }
    }

    protected void uxChangeMerchantWorked_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.ChangeMerchantWorked);
    }

    protected void uxAccount_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.SelectedAccountNumber);
    }

    protected void uxSearch_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.SearchPortfolio);
    }

    private string BuildUrlForFullCard(string encryptedCC, string issueBank, string ReportDate)
    {
        string queryString = BuildSecureQueryString("rt=Returns" + "&cn=" + Server.UrlEncode(encryptedCC) + "&issue=" + issueBank + "&reportdate=" + ReportDate);
        queryString = ResolveUrl("~/") + "FullCC.aspx?" + queryString;
        return queryString;
    }

    protected void btnChangeWorkedStatusOption_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.SearchPortfolio);
    }

    #region Private Methods

    private string FormatTransactionAmount(Int64 amount)
    {
        return amount < 1 ? "50" : amount.ToString();
    }

    #endregion Private Methods

    #endregion Methods
}