using System;
using System.Data;
using System.Collections.Generic;
using Telerik.Web.UI;
using AS.Controls.Grid;
using AS.Common.DBManager;
using AS.Common;
using AS.Controls.UserControls;
using AS.Controls.Exporter;
using System.Drawing;
using AS.Web.Business;
using AS.Utilities;
using AS.Controls.Pages;
using System.Text.RegularExpressions;

namespace As.VisionWeb.Web
{
    [PagePermission("RskAdhoc,MSRskAdhoc")]
    public partial class RiskManagementAdhocList : ReportPage
    {
        #region Enums       
        enum PostBackAction
        {
            CreateAdhoc,
            ViewAdhoc,
            ViewResult,
            RebindAdhocList,
            ChangeMerchantWorked,
            ViewMerchantInfo
        }
        #endregion
        #region properties
        private const string SESSION_FILTERING_OPTIONS = "BarometerFilteringOptions";

        private const string MERCHANT_NAME = "MerchantName";
        private const string MERCHANT_NUMBER = "MerchantNumber";
        private const string FEATURE_MODE = "FeatureMode";
        private const string MODE = "Mode";
        private const string REVIEW_MODE = "ReviewMode";
        private const string ASSIGNMENT_ID = "AssignmentID";
        private int _AssignmentID = 0;
        private DateTime _ReportDate = DateTime.Now;
        private string _AssignmentName = string.Empty;
        private Dictionary<Color, Color> _foreColorDict;
        private DataTable _AssignmentsTable = null;
        protected DataTable AssignmentsTable
        {
            get
            {
                if (_AssignmentsTable == null) LoadAssignmentsDataSource();
                return _AssignmentsTable;
            }
        }

        string _AssignmentIntruderQuery = string.Empty;
        private string AssignmentIntruderQuery
        {
            get
            {
                if (_AssignmentIntruderQuery.Length == 0)
                    _AssignmentIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(this.ID, new string[] { ASSIGNMENT_ID });
                return _AssignmentIntruderQuery;
            }
        }
        #endregion

        #region override method

        protected override void PageInitialize()
        {
            this.GridIDs.Add("uxAssignmentGrid");
            this.GridIDs.Add("uxGrid");
            this.ExporterIDs.Add("uxExporter");
            IsBindDataOnLoad = true;
            base.PageInitialize();
        }

        protected override void DoGridNeedDataSource(ASGrid sender, GridNeedDataSourceEventArgs e)
        {
            if (IsIntruderDetected) return;
            if (sender == uxAssignmentGrid)
            {
                uxAssignmentGrid.DataSource = AssignmentsTable;
                if (AssignmentsTable.Rows.Count == 0)
                {
                    uxAssignmentGrid.AllowSorting = false;
                }
                else
                    uxAssignmentGrid.AllowSorting = true;
            }
            else if (sender == uxGrid && pnlReportGrid.Visible)
            {
                if (String.IsNullOrEmpty(uxGrid.AS_SortExpression)) uxGrid.AS_SortExpression = MERCHANT_NAME;
                RiskQueue riskQueue = RiskSessionManager.RiskQueue;
                Session[SESSION_FILTERING_OPTIONS] = string.Format("{0};{1}", uxGrid.MasterTableView.CurrentPageIndex, uxGrid.AS_SortExpression);
                _AssignmentID = riskQueue.AssignmentID;
                _ReportDate = riskQueue.ReportDate;
                string spName = "spa_RM_MCF_GetRainbowReportForAdhoc";
                FilterParameterCollection parameterList = new FilterParameterCollection();
                parameterList.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                parameterList.Add(new FilterParameter("@ReportDate", _ReportDate, DbType.Date));
                parameterList.Add(new FilterParameter("@AssignmentID", _AssignmentID, DbType.Int32));
                parameterList.Add(new FilterParameter("@LanguageID", SessionManager.CurrentLanguage, DbType.Int32));
                uxGrid.DataSourceInvoker = new ASFuncInvoker(WebServices.RiskServices, "GetReports", new object[] { spName, ReportServices.ConvertToFilterParamWSArray(parameterList) });
                uxExporter.GridSubTitle = VeraCodeSolution.ValidateResponseData(riskQueue.AdhocName);
                uxExporter.GridHeader = VeraCodeSolution.ValidateResponseData(riskQueue.AdhocName);

            }
        }

        protected override void DoItemDataBound(ASGrid sender, GridItemEventArgs e)
        {
            if (IsIntruderDetected) return;
            if (sender == uxAssignmentGrid && uxAssignmentGrid.Visible)
            {
                if (e.Item is GridDataItem)
                {
                    var dataItem = e.Item as GridDataItem;
                    dataItem["ID"].Text = VeraCodeSolution.DoVeraCode((dataItem.ItemIndex + 1).ToString());
                    string assignmentId = dataItem["AssignmentID"].Text.Trim();
                    string ResultUrl = string.Format("<a class=\"link\" href=\"#\" style=\"cursor:pointer\" onclick=\"ViewRainbowReport('{0}'); return false;\">" + GetLocalResourceObject("rm_AdhocList_aspx_cs_ViewResult").ToString() + "</a>", assignmentId);
                    dataItem["Result"].Text = VeraCodeSolution.DoVeraCode(ResultUrl);
                    string assignmentName = dataItem["AssignmentName"].Text.Trim();
                    string ReviewUrl = string.Format("<a class=\"link\" href=\"#\" style=\"cursor:pointer\" onclick=\"ReviewAdhoc('{0}'); return false;\">" + VeraCodeSolution.ValidateResponseData(assignmentName) + "</a>", assignmentId);
                    dataItem["AssignmentName"].Text = VeraCodeSolution.DoVeraCode(ReviewUrl);
                }
            }
            else if (sender == uxGrid && uxGrid.Visible)
            {
                uxGrid_ItemDataBound(sender, e);
            }
        }

        protected override void OnPostBackActions(Enum type, object sender)
        {
            if (this.IsIntruderDetected) return;
            int assignmentID = 0;
            switch ((PostBackAction)type)
            {
                case PostBackAction.CreateAdhoc:
                    assignmentID = Save();
                    string queryString = this.BuildSecureQueryString(string.Format("{0}={1}{2}", ASSIGNMENT_ID, assignmentID, this.AssignmentIntruderQuery));
                    if (RiskSessionManager.IsUsingMarketData)
                        AjaxAddResponseScript("ShowPopupModal('rm_MCF_AdhocCreate_MarketData.aspx?" + queryString + "','auto');");
                    else
                        AjaxAddResponseScript("ShowPopupModal('rm_MCF_AdhocCreate.aspx?" + queryString + "','auto');");
                    break;
                case PostBackAction.ViewAdhoc:
                    _AssignmentID = int.Parse(VeraCodeSolution.DoVeraCode(hddAssignID.Value.ToString()));
                    string queryString1 = this.BuildSecureQueryString(string.Format("{0}={1}&{2}={3}&{4}={5}&{6}={7}{8}",
                        ASSIGNMENT_ID, _AssignmentID, FEATURE_MODE, 1, MODE, (int)WebSiteEnums.ParamFilterMode.Adhoc, REVIEW_MODE, 1, this.AssignmentIntruderQuery));
                    if (RiskSessionManager.IsUsingMarketData)
                        AjaxAddResponseScript("ShowPopupModal('rm_MCF_AdhocCreate_MarketData.aspx?" + queryString1 + "','auto');");
                    else
                        AjaxAddResponseScript("ShowPopupModal('rm_MCF_AdhocCreate.aspx?" + queryString1 + "','auto');");
                    break;
                case PostBackAction.ViewResult:
                    viewResultAction();
                    break;
                case PostBackAction.RebindAdhocList:
                    uxAssignmentGrid.Rebind();
                    break;
            }
        }

        protected override void DoNeedExportConfig(UxExport sender, ExportConfig exportConfig)
        {
            uxExporter.Formatter = new Dictionary<string, Func<object, string>>
            {
                { "VolumePercent", new Func<object, string>(MyConvert) },
                { "ContractualVolume", new Func<object, string>(MyConvertPercent) },
                { "AverageTicketPercent", new Func<object, string>(MyConvert) },
                { "AuthorizationPercent", new Func<object, string>(MyConvert) },
                { "DeclinedAuthorizationPercent", new Func<object, string>(MyConvert) },
                { "KeyPercent", new Func<object, string>(MyConvert) },
                { "EvenDollarTransactionPercent", new Func<object, string>(MyConvert) },
                { "DuplicateDollarTransactionPercent", new Func<object, string>(MyConvert) },
                { "ReturnPercent", new Func<object, string>(MyConvert) },
                { "TodayPrepaidCardSalesPercent", new Func<object, string>(ConvertPercentWithEmptyDash) }
            };

            exportConfig.AllowHtmlEncoded = true;
            exportConfig.PageDirection = PageDirection.Landscape;
            base.DoNeedExportConfig(sender, exportConfig);
            exportConfig.FileName = GeneralFuncsLib.GetFileName(uxExporter.GridHeader);

            foreach (GridColumn col in uxGrid.MasterTableView.Columns)
            {
                col.HeaderText = Regex.Replace(col.HeaderText, @"<[a-zA-Z\/].*?>", string.Empty);
            }
        }

        private void InitDicColor()
        {
            _foreColorDict = new Dictionary<Color, Color>();

            _foreColorDict.Add(Color.SkyBlue, Color.White);
            _foreColorDict.Add(Color.Fuchsia, Color.White);
            _foreColorDict.Add(Color.Red, Color.White);
            _foreColorDict.Add(Color.Goldenrod, Color.Black);
            _foreColorDict.Add(Color.Yellow, Color.Black);
            _foreColorDict.Add(Color.Purple, Color.White);
            _foreColorDict.Add(ColorTranslator.FromHtml("#FF6600"), Color.White);
            _foreColorDict.Add(Color.Orange, Color.White);
            _foreColorDict.Add(Color.White, Color.Black);
            _foreColorDict.Add(Color.FromName("&nbsp;"), Color.FromName(""));
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsIntruderDetected) return;
            if (!IsPostBack)
            {
                if (RiskSessionManager.RiskQueue == null)
                {
                    RiskSessionManager.RiskQueue = new RiskQueue
                    {
                        AssignmentID = 0,
                        ReportDate = DateTime.Now
                    };
                }
                pnlReportGrid.Visible = false;
            }
            else
            {
                if (RiskSessionManager.RiskQueue == null)
                {
                    RiskSessionManager.RiskQueue = new RiskQueue
                    {
                        AssignmentID = 0,
                        ReportDate = DateTime.Now
                    };
                    pnlReportGrid.Visible = false;
                }
                uxGrid.Columns.FindByDataField("TodayHighestTransactionAmount").HeaderText =
                        uxGrid.Columns.FindByDataField("TodayHighestTransactionAmount").HeaderText.ToCurrencySymbol();

            }
            InitDicColor();
        }

        #region control event

        protected void uxLnkCreateAssignemnt_Click(object sender, EventArgs e)
        {
            OnPostBackActions(PostBackAction.CreateAdhoc, sender);
        }

        protected void btnViewResult_Click(object sender, EventArgs e)
        {
            OnPostBackActions(PostBackAction.ViewResult, sender);
        }

        protected void btnReview_Click(object sender, EventArgs e)
        {
            OnPostBackActions(PostBackAction.ViewAdhoc, sender);
        }

        protected void btnRebind_Click(object sender, EventArgs e)
        {
            OnPostBackActions(PostBackAction.RebindAdhocList, sender);
        }

        protected void uxGrid_DoReportHeader(object sender, LineArgs e)
        {
            e.ReportHeader = uxExporter.GridHeader;
        }


        #endregion

        #region util
        private void uxGrid_ItemDataBound(ASGrid sender, GridItemEventArgs e)
        {
            switch (e.Item.ItemType)
            {
                case GridItemType.AlternatingItem:
                case GridItemType.Item:
                    GridDataItem dataItem = e.Item as GridDataItem;
                    var rowItem = (e.Item.DataItem as DataRowView).Row;
                    var merchantName = dataItem["MerchantName"];
                    string url = string.Empty;
                    var merchantNumber = rowItem["MerchantNumber"].ToString();

                    if (this.IsUserWithPermission(WebSiteConstants.VIEW_RISK_REPORT) || IsUserWithPermission("RskRP") || IsUserWithPermission("MSRskRP"))
                    {
                        url = string.Format("<a class=\"link\" href=\"#\" style=\"cursor:pointer\" onclick=\"hyperLink_Click('{0}'); return false;\">", merchantNumber) + rowItem["MerchantName"].ToString() + "</a>";
                    }
                    merchantName.Text = VeraCodeSolution.GetOutputHtmlString(url);
                    var merchantNameColor = Color.Transparent;
                    if (rowItem["Worked"].ToString().Equals("1"))
                    {
                        merchantNameColor = Color.LightPink;
                    }
                    merchantName.ToolTip = VeraCodeSolution.DoVeraCode(string.Format("{0} {1} - {2:MM/dd/yyyy}", rowItem["Watch"].ToString(), merchantNumber, rowItem["ApprovalDate"]));
                    merchantName.Text = GeneralFuncsLib.FormatBorderText(merchantName.Text, merchantNameColor);

                    //profile
                    var profile = dataItem["ProfileDescription"];
                    string profiletext = rowItem["ProfileDescription"].ToString();
                    profile.ToolTip = VeraCodeSolution.DoVeraCode(profiletext);

                    var riskScore = dataItem["RiskScore"];
                    Color riskColor = rowItem["RiskScoreColor"].ToString().ToColor();

                    if (!GeneralFuncsLib.NvlString(rowItem["RiskScore"]).Equals("0") && rowItem["RiskScore"] != DBNull.Value
                        && !rowItem["RiskScore"].ToString().IsNullOrEmpty())
                    {
                        string queryString = BuildSecureQueryString("MerchantNumber=" + merchantNumber + "&ReportDate=" + _ReportDate);
                        string urlRiskScoreDetail = "rm_MCF_RiskScoreDetailModal.aspx?" + queryString;
                        string urlR = "<a class=\"link\" href=\"#\" style=\"cursor:pointer\" onclick=\"ShowPopupModal('" + urlRiskScoreDetail + "','auto'); return false;\">";
                        riskScore.Text = VeraCodeSolution.GetOutputHtmlString(urlR + dataItem["RiskScore"].Text + "</a>");
                    }
                    else
                    {
                        riskScore.Text = VeraCodeSolution.DoVeraCode(rowItem["RiskScore"].ToString().Trim() != string.Empty ?
                            (int.Parse(rowItem["RiskScore"].ToString())).ToString("#,#0") : "0");
                    }
                    riskScore.Text = GeneralFuncsLib.FormatBorderText(riskScore.Text, riskColor);

                    //volume percent
                    var volumePercent = dataItem["VolumePercent"];
                    volumePercent.ToolTip = VeraCodeSolution.DoVeraCode(string.Format("{0:C}", rowItem["TodayVolume"]));
                    var volumePercentColor = rowItem["VolumeColor"].ToString().ToColor();

                    if (!GeneralFuncsLib.NvlString(rowItem["VolumePercent"]).Equals("0") && GeneralFuncsLib.NvlString(rowItem["VolumePercent"]).Length > 0)
                    {
                        string queryString1 = BuildSecureQueryString(string.Format("MerchantNumber={0}&ReportDate={1}",
                        merchantNumber, rowItem["ReportDate"]));
                        string url1 = string.Format("<a class=\"link\" href=\"#\" style=\"cursor:pointer\" onclick=\"OpenInstanceWindow('rm_MCF_TransactionDetailsModal.aspx?{0}','DQMCFWindow'); return false;\">", queryString1);
                        decimal VolumePercent = 0;
                        decimal.TryParse(GeneralFuncsLib.NvlString(rowItem["VolumePercent"]), out VolumePercent);
                        dataItem["VolumePercent"].Text = VeraCodeSolution.DoVeraCode(url1 + VolumePercent.ToString("#,#0") + "%" + "</a>");
                    }
                    else
                    {
                        if (rowItem["VolumePercent"].ToString().Trim() == string.Empty)
                        {
                            volumePercent.Text = VeraCodeSolution.DoVeraCode(string.Empty);
                        }
                        else
                        {
                            decimal VolumePercent = 0;
                            decimal.TryParse(GeneralFuncsLib.NvlString(rowItem["VolumePercent"]), out VolumePercent);
                            volumePercent.Text = VeraCodeSolution.DoVeraCode(VolumePercent.ToString("#,#0") + "%");
                        }
                    }
                    volumePercent.Text = GeneralFuncsLib.FormatBorderText(volumePercent.Text, volumePercentColor);

                    // 40965
                    var contractualVolume = dataItem["ContractualVolume"];
                    var contractualVolumeColor = rowItem["CVColor"].ToString().ToColor();


                    contractualVolume.Text = VeraCodeSolution.DoVeraCode(rowItem["ContractualVolume"] != DBNull.Value ?
                           string.Format("{0:N2}%", (decimal.Parse(rowItem["ContractualVolume"].ToString()))) : WebSiteConstants.HTML_EM_DASH_ENCODE);
                    contractualVolume.Text = GeneralFuncsLib.FormatBorderText(contractualVolume.Text, contractualVolumeColor);
                    if (rowItem["ContractualVolume"] != DBNull.Value)
                    {
                        contractualVolume.ToolTip = VeraCodeSolution.DoVeraCode(string.Format("{0:C}", rowItem["ContractualDailyVolume"]).ToCurrencySymbol());
                    }
                    else
                    {
                        contractualVolume.ToolTip = GetLocalResourceObject("ContractualValuenotBeenProvided_Resource").ToString();
                    }
                    //average ticket
                    var avgTicket = dataItem["AverageTicketPercent"];
                    Color avgTicketColor = rowItem["AvgTktColor"].ToString().ToColor();
                    avgTicket.ToolTip = VeraCodeSolution.DoVeraCode(GeneralFuncsLib.FormatCurrencyTooltip(rowItem["AverageTicketVolume"], SessionManager.CurrencyFortmat).ToCurrencySymbol());
                    avgTicket.Text = VeraCodeSolution.DoVeraCode(rowItem["AverageTicketPercent"].ToString() != string.Empty ?
                        (decimal.Parse(rowItem["AverageTicketPercent"].ToString())).ToString("#,#0") + "%" : string.Empty);
                    avgTicket.Text = GeneralFuncsLib.FormatBorderText(avgTicket.Text, avgTicketColor);

                    //auth percent
                    var authPercent = dataItem["AuthorizationPercent"];
                    Color authPercentColor = rowItem["AuthColor"].ToString().ToColor();
                    authPercent.ToolTip = VeraCodeSolution.DoVeraCode(string.Format("{2}{0} ({1})", rowItem["TodayAuthorizationVolume"].ToString(), rowItem["TodayAuthorizationCount"].ToString(), SessionManager.CurrencySymbol).ToCurrencySymbol());
                    authPercent.Text = VeraCodeSolution.DoVeraCode(rowItem["AuthorizationPercent"].ToString().Trim() != string.Empty ?
                        (decimal.Parse(rowItem["AuthorizationPercent"].ToString())).ToString("#,#0") + "%" : string.Empty);
                    authPercent.Text = GeneralFuncsLib.FormatBorderText(authPercent.Text, authPercentColor);

                    //ARS
                    var attrScore = dataItem["AttritionScore"];
                    Color attrScoreColor = rowItem["ARSColor"].ToString().ToColor();
                    attrScore.ToolTip = VeraCodeSolution.DoVeraCode(rowItem["AttritionReasonCodes"].ToString());
                    attrScore.Text = string.IsNullOrEmpty(rowItem["AttritionScore"].ToString()) ? string.Empty :
                        VeraCodeSolution.DoVeraCode(decimal.Parse(rowItem["AttritionScore"].ToString().Trim()).ToString("#,#0"));
                    attrScore.Text = GeneralFuncsLib.FormatBorderText(attrScore.Text, attrScoreColor);

                    //RRS
                    var rrsScore = dataItem["ReserveScore"];
                    Color RrsScoreColor = rowItem["RRSColor"].ToString().ToColor();
                    rrsScore.ToolTip = VeraCodeSolution.DoVeraCode(rowItem["ReserveReasonCodes"].ToString());
                    rrsScore.Text = string.IsNullOrEmpty(rowItem["ReserveScore"].ToString()) ? string.Empty :
                        VeraCodeSolution.DoVeraCode(decimal.Parse(rowItem["ReserveScore"].ToString().Trim()).ToString("#,#0"));
                    rrsScore.Text = GeneralFuncsLib.FormatBorderText(rrsScore.Text, RrsScoreColor);

                    //decline percent
                    var decPercent = dataItem["DeclinedAuthorizationPercent"];
                    Color decPercentColor = rowItem["DeclAuthPctColor"].ToString().ToColor();
                    decPercent.ToolTip = VeraCodeSolution.DoVeraCode(GeneralFuncsLib.FormatCurrencyTooltip(rowItem["TodayDeclinedAuthorizationVolume"], SessionManager.CurrencyFortmat).ToCurrencySymbol());
                    decPercent.Text = VeraCodeSolution.DoVeraCode(rowItem["DeclinedAuthorizationPercent"].ToString().Trim() != string.Empty ?
                        (decimal.Parse(rowItem["DeclinedAuthorizationPercent"].ToString())).ToString("#,#0") + "%" : string.Empty);
                    decPercent.Text = GeneralFuncsLib.FormatBorderText(decPercent.Text, decPercentColor);

                    //# repeat auth
                    var rptAuth = dataItem["RepeatAuthorizationCount"];
                    Color rptAuthColor = rowItem["RptAuthColor"].ToString().ToColor();
                    rptAuth.Text = GeneralFuncsLib.FormatBorderText(rptAuth.Text, rptAuthColor, true);

                    //FC (foreign card)
                    var fc = dataItem["TodayForeignCardCount"];
                    Color fcColor = rowItem["FCColor"].ToString().ToColor();
                    fc.ToolTip = VeraCodeSolution.DoVeraCode(GeneralFuncsLib.FormatCurrencyTooltip(rowItem["TodayForeignCardVolume"], SessionManager.CurrencyFortmat).ToCurrencySymbol());
                    fc.Text = GeneralFuncsLib.FormatBorderText(fc.Text, fcColor, true);

                    //key percent
                    var keyPercent = dataItem["KeyPercent"];
                    Color keyPercentColor = rowItem["KeyColor"].ToString().ToColor();
                    keyPercent.ToolTip = VeraCodeSolution.DoVeraCode(string.Format("{0} ({1})", GeneralFuncsLib.FormatCurrencyTooltip(rowItem["TodayKeyVolume"], SessionManager.CurrencyFortmat), rowItem["TodayKeyCount"].ToString()).ToCurrencySymbol());
                    keyPercent.Text = VeraCodeSolution.DoVeraCode(rowItem["KeyPercent"].ToString().Trim() != string.Empty ?
                        (decimal.Parse(rowItem["KeyPercent"].ToString())).ToString("#,#0") + "%" : string.Empty);
                    keyPercent.Text = GeneralFuncsLib.FormatBorderText(keyPercent.Text, keyPercentColor, true);

                    //even percent
                    var evenPercent = dataItem["EvenDollarTransactionPercent"];
                    Color evenPercentColor = rowItem["EvenColor"].ToString().ToColor();
                    evenPercent.Text = VeraCodeSolution.DoVeraCode(rowItem["EvenDollarTransactionPercent"].ToString() != string.Empty ?
                        (decimal.Parse(rowItem["EvenDollarTransactionPercent"].ToString())).ToString("#,#0") + "%" : string.Empty);
                    evenPercent.Text = GeneralFuncsLib.FormatBorderText(evenPercent.Text, evenPercentColor, true);

                    //duplicate percent
                    var dupPercent = dataItem["DuplicateDollarTransactionPercent"];
                    Color dupPercentColor = rowItem["DupColor"].ToString().ToColor();
                    dupPercent.ToolTip = VeraCodeSolution.DoVeraCode(rowItem["DuplicateDollarTransactionCount"].ToString().ToCurrencySymbol());
                    dupPercent.Text = VeraCodeSolution.DoVeraCode(rowItem["DuplicateDollarTransactionPercent"].ToString() != string.Empty ?
                        (decimal.Parse(rowItem["DuplicateDollarTransactionPercent"].ToString())).ToString("#,#0") + "%" : string.Empty);
                    dupPercent.Text = GeneralFuncsLib.FormatBorderText(dupPercent.Text, dupPercentColor);

                    //duplicate bin
                    var dupBin = dataItem["DuplicateBin"];
                    Color dupBinColor = rowItem["SixDupBinColor"].ToString().ToColor();
                    dupBin.Text = GeneralFuncsLib.FormatBorderText(dupBin.Text, dupBinColor);

                    //# negative
                    var negative = dataItem["NegativeBatchCount"];
                    Color negColor = rowItem["NegColor"].ToString().ToColor();
                    negative.Text = GeneralFuncsLib.FormatBorderText(negative.Text, negColor);

                    //# zero
                    var zero = dataItem["ZeroBatchCount"];
                    Color zeroColor = rowItem["ZeroColor"].ToString().ToColor();
                    zero.Text = GeneralFuncsLib.FormatBorderText(zero.Text, zeroColor);

                    //TV
                    var todayVol = dataItem["TodayVolume"];
                    var todayVolColor = rowItem["VolumeColor"].ToString().ToColor();
                    todayVol.Text = GeneralFuncsLib.FormatBorderText(todayVol.Text, todayVolColor);

                    //RTVL
                    var rtvl = dataItem["TodayFirstTimeRetrievalVolume"];
                    Color rtvlColor = rowItem["RTVLColor"].ToString().ToColor();
                    rtvl.ToolTip = VeraCodeSolution.DoVeraCode(rowItem["TodayFirstTimeRetrievalCount"].ToString());
                    rtvl.Text = GeneralFuncsLib.FormatBorderText(rtvl.Text, rtvlColor);

                    //CB
                    var cb = dataItem["TodayChargebackVolume"];
                    Color cbColor = rowItem["CBColor"].ToString().ToColor();
                    cb.ToolTip = VeraCodeSolution.DoVeraCode(rowItem["TodayChargebackCount"].ToString());
                    cb.Text = GeneralFuncsLib.FormatBorderText(cb.Text, cbColor);

                    //Rtn Percent
                    var rtnPercent = dataItem["ReturnPercent"];
                    Color rtnPercentColor = rowItem["RtnColor"].ToString().ToColor();
                    rtnPercent.ToolTip = VeraCodeSolution.DoVeraCode(GeneralFuncsLib.FormatCurrencyTooltip(rowItem["TodayReturnAmount"], SessionManager.CurrencyFortmat).ToCurrencySymbol());
                    rtnPercent.Text = VeraCodeSolution.DoVeraCode(rowItem["ReturnPercent"].ToString().Trim() != string.Empty ?
                        (decimal.Parse(rowItem["ReturnPercent"].ToString())).ToString("#,#0") + "%" : string.Empty);
                    rtnPercent.Text = GeneralFuncsLib.FormatBorderText(rtnPercent.Text, rtnPercentColor);

                    //ACH Return Amount
                    var achReturnAmount = dataItem["ACHReturnAmount"];
                    Color achReturnAmountColor = rowItem["ACHReturnAmountColor"].ToString().ToColor();
                    achReturnAmount.Text = GeneralFuncsLib.FormatBorderText(achReturnAmount.Text, achReturnAmountColor, true);

                    //Max ticket $
                    var maxTkt = dataItem["TodayHighestTransactionAmount"];
                    Color maxTktColor = rowItem["MaxTktColor"].ToString().ToColor();
                    maxTkt.Text = GeneralFuncsLib.FormatBorderText(maxTkt.Text, maxTktColor, true);

                    //# Tkts
                    var tkt = dataItem["TodayTransactionCount"];
                    Color tktColor = rowItem["TktsColor"].ToString().ToColor();
                    tkt.Text = GeneralFuncsLib.FormatBorderText(tkt.Text, tktColor, true);

                    //# batch
                    var batch = dataItem["TodayBatchCount"];
                    Color batchColor = rowItem["BatchColor"].ToString().ToColor();
                    batch.Text = GeneralFuncsLib.FormatBorderText(batch.Text, batchColor, true);

                    //SIC
                    var sic = dataItem["SICCode"];
                    sic.ToolTip = VeraCodeSolution.DoVeraCode(rowItem["SICDescription"].ToString().Replace("&nbsp;", ""));
                    //*/
                    //duplicate bin
                    var sc = dataItem["SingleCardTransToday"];
                    Color scColor = rowItem["DupBinColor"].ToString().ToColor();
                    sc.ToolTip = VeraCodeSolution.DoVeraCode(GeneralFuncsLib.FormatCurrencyTooltip(rowItem["VolumeOfSimilarCardTransaction"], SessionManager.CurrencyFortmat));
                    sc.Text = GeneralFuncsLib.FormatBorderText(sc.Text, scColor);

                    //Today Prepaid Card Sales Percent
                    var prepaidCardSalesPercentCell = dataItem["TodayPrepaidCardSalesPercent"];
                    var prepaidCardSalesPercent = GeneralFuncsLib.NvlString(rowItem["TodayPrepaidCardSalesPercent"]) != string.Empty ? decimal.Parse(rowItem["TodayPrepaidCardSalesPercent"].ToString()) : 0;

                    if (prepaidCardSalesPercent > 0)
                    {
                        Color prepaidCardSalesPercentColor = rowItem["TodayPrepaidCardSalesPercentColor"].ToString().ToColor();
                        prepaidCardSalesPercentCell.ToolTip = VeraCodeSolution.DoVeraCode(GeneralFuncsLib.FormatCurrencyTooltip(rowItem["TodayPrepaidSalesAmount"], SessionManager.CurrencyFortmat).ToCurrencySymbol());
                        prepaidCardSalesPercentCell.Text = VeraCodeSolution.DoVeraCode(prepaidCardSalesPercent.ToString("#,#0") + "%");
                        prepaidCardSalesPercentCell.Text = GeneralFuncsLib.FormatBorderText(prepaidCardSalesPercentCell.Text, prepaidCardSalesPercentColor);
                    }

                    break;
            }
        }
        private string MyConvertPercent(object obj)
        {
            if (!obj.IsNullOrEmpty())
                return string.Format("{0:N}%", (decimal.Parse(obj.ToString())));
            else
                return WebSiteConstants.HTML_EM_DASH_ENCODE;
        }

        private string MyConvert(object obj)
        {
            if (obj.IsNullOrEmpty())
                return string.Empty;
            string str = string.Empty;
            try
            {
                str = decimal.Parse(obj.ToString()).ToString("#,#0") + "%";
            }
            catch (Exception)
            {
                str = string.Empty;
            }

            return str;
        }

        private string ConvertPercentWithEmptyDash(object obj)
        {
            if (obj.IsNullOrEmpty())
                return WebSiteConstants.HTML_EM_DASH_ENCODE;
            string str = string.Empty;
            try
            {
                var value = decimal.Parse(obj.ToString());
                str = value > 0 ? value.ToString("#,#0") + "%" : WebSiteConstants.HTML_EM_DASH_ENCODE;
            }
            catch (Exception)
            {
                str = string.Empty;
            }

            return str;
        }
        private void LoadAssignmentsDataSource()
        {
            FilterParameterCollection parameters = new FilterParameterCollection();
            parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
            parameters.AddLanguageID();
            _AssignmentsTable = WebServices.RiskServices.GetReports("spa_RM_MCF_GetAdhocListByUser", parameters);
        }
        private int Save()
        {
            int assignmentID = 0;
            string assignmentName = "Adhoc_" + DateTime.Now.ToShortDateString() + "_" + DateTime.Now.ToLongTimeString();
            FilterParameterCollection paramsIn = new FilterParameterCollection();
            paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
            paramsIn.Add(new FilterParameter("@AssignmentID", 0, DbType.Int32, true));
            paramsIn.Add(new FilterParameter("@AssignmentName", assignmentName, DbType.AnsiString));
            paramsIn.Add(new FilterParameter("@ExpirationDate", DateTime.Now.AddDays(7), DbType.DateTime));
            paramsIn.Add(new FilterParameter("@Owner", SessionManager.CurrentUser.UserID, DbType.AnsiString));
            paramsIn.Add(new FilterParameter("@FilterMode", (int)WebSiteEnums.ParamFilterMode.Adhoc, DbType.Int32));
            FilterParameterCollection paramsOut;

            WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_AddAssignment", paramsIn, out paramsOut);

            if (paramsOut != null && paramsOut.Count > 0)
                assignmentID = Int32.Parse(paramsOut[0].ParameterValue.ToString());

            return assignmentID;
        }
        private void viewResultAction()
        {
            _AssignmentID = int.Parse(VeraCodeSolution.DoVeraCode(hddAssignID.Value.ToString()));

            FilterParameterCollection paramsIn = new FilterParameterCollection();
            paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
            paramsIn.Add(new FilterParameter("@AssignmentID", _AssignmentID, DbType.Int32));

            DataTable tmp = WebServices.RiskServices.GetReports("spa_RM_MCF_GetAdhocInfo", paramsIn);
            if (tmp.Rows.Count > 0)
            {
                _AssignmentName = tmp.Rows[0]["AssignmentName"].ToString();
                _ReportDate = DateTime.Parse(tmp.Rows[0]["ReportDate"].ToString());

                RiskSessionManager.RiskQueue.AssignmentID = _AssignmentID;
                RiskSessionManager.RiskQueue.ReportDate = _ReportDate;
                RiskSessionManager.RiskQueue.AdhocName = "" + _AssignmentName;
                if (RiskSessionManager.RiskQueue.AssignmentID > 0)
                {
                    pnlReportGrid.Visible = true;
                }

                uxGrid.CurrentPageIndex = 0;
                uxGrid.Rebind();
            }
        }

        #endregion

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string[] MerchantNumberClick(bool status, string merchantNumber)
        {
            ReportPage page = new ReportPage();
            string riskReportIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(page.ID, new string[] { "MerchantNumber" });
            string url = "rm_MCF_RiskReport.aspx?" + page.BuildSecureQueryString(string.Format("merchantnumber={0}&IsPopup={1}{2}", merchantNumber, true, riskReportIntruderQuery));
            return new string[] { url };
        }

        protected void uxGrid_Init(object sender, EventArgs e)
        {
            // Set Header for Grid
            foreach (GridColumn col in uxGrid.MasterTableView.Columns)
            {
                string headerText = RM_MCF_GeneralFuncsLib.GetResourceValue(string.Format("{0}_Text", col.UniqueName));
                col.HeaderText = string.Format("<span id='{0}'>{0}</span>", headerText);
                col.HeaderTooltip = string.Empty;
            }
        }

        protected void uxGrid_PreRender(object sender, EventArgs e)
        {
            // Set Tooltip for Grid
            foreach (GridColumn col in uxGrid.MasterTableView.Columns)
            {
                string headerText = RM_MCF_GeneralFuncsLib.GetResourceValue(string.Format("{0}_Text", col.UniqueName));
                string targetControlId = headerText;
                string toolTipContent = RM_MCF_GeneralFuncsLib.GetResourceValue(string.Format("{0}_Tooltip", col.UniqueName));
                RM_MCF_GeneralFuncsLib.GenerateTooltipHeaderDefaulColumn(col, headerText, uxGrid, targetControlId, toolTipContent);
            }
        }
    }
}

