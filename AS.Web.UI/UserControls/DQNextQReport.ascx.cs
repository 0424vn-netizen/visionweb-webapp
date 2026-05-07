using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Drawing;
using AS.Common;
using AS.Controls.Pages;
using AS.Controls.Grid;
using System.IO;
using AS.Common.DBManager;
using AS.Utilities;
using AS.Controls.Global;
using System.Collections.Generic;
using System.Web.Services;
using System.Text;
using System.Text.RegularExpressions;
using AS.Common.Formater;
using AS.VW.Common;

public partial class UserControls_DQNextQReport : GlobalUserControl
{
    #region Enums
    enum DataBindAction
    {
        BindMerchantInfo,
        BindBarometer,
        BindTransaction,
        BindChargeBack,
        BindDailyFC
    }

    enum PostBackAction
    {
        SelectedAccountNumber
    }
    #endregion

    #region Properties
    const string IMAGE = "<a class=\"atab\"href=\"#\" style=\"cursor:pointer\" onclick=\" return ShowPopupModalChild({0},'{1}','auto');\"><img src='../res/images/information.gif' border=\"0\"/></a>&nbsp; &nbsp;";
    int index = 0;
    private int _totalRows = 0;
    private int _TransCountKeyed = 0;
    private decimal _TransAmount = 0;

    private int _ChargebackCountKeyed = 0;
    private decimal _ChargebackAmount = 0;

    private int _DailyFcCountKeyed = 0;
    private decimal _DailyFcAmount = 0;


    private DateTime _ReportDate = DateTime.Now;
    public DateTime ReportDate
    {
        get { return _ReportDate; }
        set { _ReportDate = value; }
    }

    private const int ScrollRowCount = 30;
    private const int ScrollRowCountForTrans = 10;

    protected string urlImage
    {
        get
        {
            string assignmentTypeQueryString =
           RiskSessionManager.DetectionQueue.AssignmentType != WebSiteEnums.AssignmentType.DetectionQueue
           ? "&AssignmentType=" + (int)RiskSessionManager.DetectionQueue.AssignmentType : "";

            string queryStringImage = this.Page.BuildSecureQueryString("MerchantNumber=" + RiskSessionManager.currentMerchantNumber + "&ReportDate=" + RiskSessionManager.DetectionQueue.ReportDate + "&AssignmentID=" + RiskSessionManager.DetectionQueue.AssignmentID);
            return "rm_DQReasonModal.aspx?" + queryStringImage;
        }
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsIntruderDetected) return;
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        if (Page.IsIntruderDetected) return;
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindMerchantInfo:
                {
                    uxMerchantInfo.DataSource = RiskSessionManager.CurrentRiskNextQueue.MerchantInfo;
                    uxMerchantInfo.DataBind();
                }
                break;
            case DataBindAction.BindBarometer:
                {
                    uxBarometerGrid.DataSource = RiskSessionManager.CurrentRiskNextQueue.Barometer;
                    uxBarometerGrid.DataBind();
                    uxBarometerGrid.Visible = true;
                }
                break;
            case DataBindAction.BindTransaction:
                {
                    DataTable transactionList = RiskSessionManager.CurrentRiskNextQueue.Transaction;
                    uxReportGridTransactions.Visible = true;
                    BindTransaction(transactionList, 1);
                }
                break;
            case DataBindAction.BindChargeBack:
                {
                    DataTable chargeback = RiskSessionManager.CurrentRiskNextQueue.ChargeBack;
                    CalculateChargebackTotal(chargeback);
                    //if (IsExport && chargeback != null && chargeback.Rows.Count > 0)
                    //{
                    //    DataRow dr = chargeback.NewRow();
                    //    dr["PartialAccountNumber"] = "Total: " + chargeback.Rows.Count;
                    //    dr["Keyed"] = _ChargebackCountKeyed;
                    //    dr["TransactionAmount"] = _ChargebackAmount;
                    //    chargeback.Rows.Add(dr);
                    //}

                    uxChargeback.DataSource = chargeback;
                    uxChargeback.DataBind();
                    uxChargeback.Visible = true;
                }
                break;

            default: break;
        }
    }
    protected void uxReportGridTransactions_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        //OnDataBindControls(DataBindAction.BindTransaction);
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        if (Page.IsIntruderDetected) return;
        switch ((PostBackAction)type)
        {
            case PostBackAction.SelectedAccountNumber:
                string queryStr = uxAccountValue.Value;
                Response.Redirect(queryStr, true);
                break;
            default: break;
        }
    }


    public void GetFlatReportInfo()
    {
        //Bind Merchant Information
        OnDataBindControls(DataBindAction.BindMerchantInfo);
        OnDataBindControls(DataBindAction.BindBarometer);
        OnDataBindControls(DataBindAction.BindTransaction);
        OnDataBindControls(DataBindAction.BindChargeBack);
    }

    private void CalculateTransactionTotal(DataTable dt)
    {
        if (dt.IsNotNullData() && dt.Rows.Count > 0)
        {
            DataRow row = dt.Rows[0];
            _totalRows = Convert.ToInt32(row["TotalRows"].ToString());
            _TransCountKeyed = Convert.ToInt32(row["Count_Key"].ToString());
            _TransAmount = Convert.ToDecimal(row["SUM_Amount"].ToString());
        }
    }

    private void CalculateChargebackTotal(DataTable dt)
    {
        _ChargebackAmount = 0;
        _ChargebackCountKeyed = 0;
        if (dt == null) return;
        foreach (DataRow dr in dt.Rows)
        {
            if (dr["Keyed"].ToString() == "Y") _ChargebackCountKeyed++;
            _ChargebackAmount += Convert.ToDecimal(dr["TransactionAmount"].ToString());
        }
    }

    private void CalculateDailyFCTotal(DataTable dt)
    {
        _ChargebackAmount = 0;
        _ChargebackCountKeyed = 0;
        if (dt == null) return;
        foreach (DataRow dr in dt.Rows)
        {
            _DailyFcCountKeyed += Convert.ToInt32(dr["TransactionCount"]);
            _DailyFcAmount += Convert.ToDecimal(dr["Volume"]);
        }
    }

    protected string MerchantNameColor(object activeDayFlag)
    {
        switch (activeDayFlag.ToString())
        {
            case "1":
                return "Red";
            case "2":
                return "Brown";
            case "3":
                return "Blue";
            case "4":
                return "Green";
            case "5":
                return "Black";
        }
        return "Black";
    }

    bool CheckCSViewFullCard()
    {
        if ((SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.AS
            || SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.CS)
            && Page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_CC))
            return true;
        return false;
    }

    bool CheckCSViewFullCard(SecurePage Page)
    {
        if ((SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.AS
            || SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.CS)
            && Page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_CC))
            return true;
        return false;
    }

    #region Barometer
    protected void uxBarometerGrid_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        switch (e.Item.ItemType)
        {
            case ListItemType.AlternatingItem:
            case ListItemType.Item:
                {
                    RepeaterItem dataItem = e.Item as RepeaterItem;
                    var rowItem = (e.Item.DataItem as DataRowView).Row;
                    string url = string.Empty;

                    //profile
                    TableColumnContent profile = dataItem.FindControl("uxProfileDescription") as TableColumnContent;
                    profile.ToolTip = VeraCodeSolution.DoVeraCode(rowItem["ProfileDescription"].ToString());
                    profile.Text = VeraCodeSolution.ValidateResponseData(rowItem["ProfileDescription"].ToASString());

                    //riskscore
                    TableColumnContent bA = dataItem.FindControl("uxBA") as TableColumnContent;
                    bA.Text = rowItem["BusinessAge"].ToString().Trim();

                    //riskscore
                    TableColumnContent riskScore = dataItem.FindControl("uxRiskScore") as TableColumnContent;
                    Color riskScoreBgColor = rowItem["RiskScoreColor"].ToString().ToColor();
                    riskScore.Text = VeraCodeSolution.ValidateResponseData(riskScore.CustomFormatDataValue(rowItem["RiskScore"].ToString(), riskScore.ASFormat));
                    if (!GeneralFuncsLib.NvlString(rowItem["RiskScore"]).Equals("0"))
                    {
                        string queryString = Page.BuildSecureQueryString("MerchantNumber=" + rowItem["MerchantNumber"] + "&ReportDate=" + ReportDate);
                        string urlRiskScoreDetail = "rm_RiskScoreDetailModal.aspx?" + queryString;
                        url = "<a class=\"link\" href='#' style=\"cursor:pointer\" onclick=\" return ShowPopupModal('" + urlRiskScoreDetail + "'); return false;\">";
                        riskScore.Text = VeraCodeSolution.GetOutputHtmlString(url + riskScore.Text + "</a>");
                    }
                    else
                    {
                        riskScore.Text = VeraCodeSolution.DoVeraCode(rowItem["RiskScore"].ToString().Trim() != string.Empty ? riskScore.Text : string.Empty);
                    }
                    riskScore.Text = GeneralFuncsLib.FormatBorderText(riskScore.Text, riskScoreBgColor);

                    //volume percent
                    TableColumnContent volumePercent = dataItem.FindControl("uxVolumePercent") as TableColumnContent;
                    volumePercent.ToolTip = string.Format("{0:C}", rowItem["TodayVolume"]).ToCurrencySymbol();
                    string rptDate = ReportDate.ToShortDateString();
                    rptDate = Convert.ToDateTime(rptDate).ToShortDateString();
                    if (!GeneralFuncsLib.NvlString(rowItem["VolumePercent"]).Equals("0") && GeneralFuncsLib.NvlString(rowItem["VolumePercent"]).Length > 0)
                    {
                        string queryString1 = Page.BuildSecureQueryString(string.Format("MerchantNumber={0}&ReportDate={1}",
                        rowItem["MerchantNumber"], rowItem["ReportDate"]));
                        string url1 = string.Format("<a class=\"link\" href=\"#\" style=\"cursor:pointer\" onclick=\"OpenInstanceWindow('rm_TransactionDetailsModal.aspx?{0}','DQWindow'); return false;\">", queryString1);
                        decimal VolumePercent = 0;
                        decimal.TryParse(GeneralFuncsLib.NvlString(rowItem["VolumePercent"]), out VolumePercent);
                        volumePercent.Text = VeraCodeSolution.DoVeraCode(url1 + VolumePercent.ToString("#,#0") + "%" + "</a>");
                    }
                    else
                    {
                        volumePercent.Text = VeraCodeSolution.DoVeraCode(rowItem["VolumePercent"].ToString().Trim() != string.Empty ?
                             (decimal.Parse(rowItem["VolumePercent"].ToString())).ToString("#,#0") + "%" : string.Empty);
                    }
                    var volumeColor = rowItem["VolumeColor"];
                    if (!volumeColor.IsNullOrEmpty())
                    {
                        volumePercent.Text = GeneralFuncsLib.FormatBorderText(volumePercent.Text, volumeColor.ToString().ToColor());
                    }

                    // 40965 Contractual Volume
                    TableColumnContent uxContractualVolume = dataItem.FindControl("uxContractualVolume") as TableColumnContent;
                    if (rowItem["ContractualVolume"] != DBNull.Value)
                    {
                        uxContractualVolume.ToolTip = string.Format("{0:C}", rowItem["ContractualDailyVolume"]).ToCurrencySymbol();
                    }
                    else
                    {
                        uxContractualVolume.ToolTip = GetLocalResourceObject("ContractualValuenotBeenProvided_Resource").ToString();
                    }
                    uxContractualVolume.Text = VeraCodeSolution.DoVeraCode(rowItem["ContractualVolume"] != DBNull.Value ?
                         string.Format("{0:N}%", (decimal.Parse(rowItem["ContractualVolume"].ToString()))) : WebSiteConstants.HTML_EM_DASH_ENCODE);
                    var uxContractualVolumeColor = rowItem["CVColor"].ToASString();
                    if (!uxContractualVolumeColor.IsNullOrEmpty())
                    {
                        uxContractualVolume.Text = GeneralFuncsLib.FormatBorderText(uxContractualVolume.Text, uxContractualVolumeColor.ToColor());
                    }

                    //average ticket
                    TableColumnContent avgTicket = dataItem.FindControl("uxAverageTicketPercent") as TableColumnContent;
                    avgTicket.ToolTip = string.Format("{0:C}", rowItem["AverageTicketVolume"]).ToCurrencySymbol();
                    avgTicket.Text = VeraCodeSolution.DoVeraCode(rowItem["AverageTicketPercent"].ToString() != string.Empty ?
                        (decimal.Parse(rowItem["AverageTicketPercent"].ToString())).ToString("#,#0") + "%" : string.Empty);
                    var avgTicketColor = rowItem["AvgTktColor"].ToASString();
                    if (!avgTicketColor.IsNullOrEmpty())
                    {
                        avgTicket.Text = GeneralFuncsLib.FormatBorderText(avgTicket.Text, avgTicketColor.ToColor());
                    }

                    //auth percent
                    var authPercent = dataItem.FindControl("uxAuthorizationPercent") as TableColumnContent;
                    authPercent.ToolTip = string.Format("{0:C} ({1})", rowItem["TodayAuthorizationVolume"], rowItem["TodayAuthorizationCount"]).ToCurrencySymbol();
                    if (!GeneralFuncsLib.NvlString(rowItem["AuthorizationPercent"]).Equals("0") && GeneralFuncsLib.NvlString(rowItem["AuthorizationPercent"]).Length > 0)
                    {
                        string queryString1 = this.Page.BuildSecureQueryString(string.Format("MerchantNumber={0}&ReportDate={1}",
                        rowItem["MerchantNumber"], rowItem["ReportDate"]));
                        string url1 = string.Format("<a class=\"link\" href=\"#\" style=\"cursor:pointer\" onclick=\"OpenInstanceWindow('rm_TransactionHistoryModal.aspx?{0}','DQWindow'); return false;\">", queryString1);
                        decimal authPerc = 0;
                        decimal.TryParse(GeneralFuncsLib.NvlString(rowItem["AuthorizationPercent"]), out authPerc);
                        authPercent.Text = VeraCodeSolution.DoVeraCode(url1 + authPerc.ToString("#,#0") + "%" + "</a>");
                    }
                    else
                    {
                        authPercent.Text = VeraCodeSolution.DoVeraCode(rowItem["AuthorizationPercent"].ToString().Trim() != string.Empty ?
                       (decimal.Parse(rowItem["AuthorizationPercent"].ToString()).ToString("#,#0") + "%") : string.Empty);
                    }
                    var authPercentColor = rowItem["AuthColor"];
                    if (!authPercentColor.IsNullOrEmpty())
                    {
                        authPercent.Text = GeneralFuncsLib.FormatBorderText(authPercent.Text, authPercentColor.ToString().ToColor());
                    }

                    //decline percent
                    TableColumnContent decPercent = dataItem.FindControl("uxDeclinedAuthorizationPercent") as TableColumnContent;
                    decPercent.ToolTip = string.Format("{0:C}", rowItem["TodayDeclinedAuthorizationVolume"]).ToCurrencySymbol();
                    decPercent.Text = VeraCodeSolution.DoVeraCode(rowItem["DeclinedAuthorizationPercent"].ToString().Trim() != string.Empty ?
                        (decimal.Parse(rowItem["DeclinedAuthorizationPercent"].ToString())).ToString("#,#0.00") + "%" : string.Empty);
                    var decPercentColor = rowItem["DeclAuthPctColor"];
                    if (!decPercentColor.IsNullOrEmpty())
                    {
                        decPercent.Text = GeneralFuncsLib.FormatBorderText(decPercent.Text, decPercentColor.ToString().ToColor());
                    }

                    //# repeat auth
                    TableColumnContent rptAuth = dataItem.FindControl("uxRepeatAuthorizationCount") as TableColumnContent;
                    rptAuth.Text = VeraCodeSolution.DoVeraCode(rowItem["RepeatAuthorizationCount"].ToString());
                    var rptAuthColor = rowItem["RptAuthColor"];
                    if (!rptAuthColor.IsNullOrEmpty())
                    {
                        rptAuth.Text = GeneralFuncsLib.FormatBorderText(rptAuth.Text, rptAuthColor.ToString().ToColor());
                    }


                    //FC (foreign card)
                    TableColumnContent fc = dataItem.FindControl("uxTodayForeignCardCount") as TableColumnContent;
                    fc.ToolTip = string.Format("{0:C}", rowItem["TodayForeignCardVolume"]).ToCurrencySymbol();
                    fc.Text = VeraCodeSolution.DoVeraCode(rowItem["TodayForeignCardCount"].ToASString());
                    var fcColor = rowItem["FCColor"];
                    if (!fcColor.IsNullOrEmpty())
                    {
                        fc.Text = GeneralFuncsLib.FormatBorderText(fc.Text, fcColor.ToString().ToColor());
                    }

                    //key percent
                    TableColumnContent keyPercent = dataItem.FindControl("uxKeyPercent") as TableColumnContent;
                    keyPercent.ToolTip = string.Format("{0} ({1})", GeneralFuncsLib.FormatCurrencyTooltip(rowItem["TodayKeyVolume"], SessionManager.CurrencyFortmat), rowItem["TodayKeyCount"]).ToCurrencySymbol();
                    keyPercent.Text = VeraCodeSolution.DoVeraCode(rowItem["KeyPercent"].ToString().Trim() != string.Empty ?
                        (decimal.Parse(rowItem["KeyPercent"].ToString())).ToString("#,#0") + "%" : string.Empty);
                    var keyPercentColor = rowItem["KeyColor"];
                    if (!keyPercentColor.IsNullOrEmpty())
                    {
                        keyPercent.Text = GeneralFuncsLib.FormatBorderText(keyPercent.Text, keyPercentColor.ToString().ToColor());
                    }

                    //even percent
                    TableColumnContent evenPercent = dataItem.FindControl("uxEvenDollarTransactionPercent") as TableColumnContent;
                    evenPercent.Text = VeraCodeSolution.DoVeraCode(rowItem["EvenDollarTransactionPercent"].ToString() != string.Empty ?
                        (decimal.Parse(rowItem["EvenDollarTransactionPercent"].ToString())).ToString("#,#0") + "%" : string.Empty);
                    var evenPercentColor = rowItem["EvenColor"];
                    if (!evenPercentColor.IsNullOrEmpty())
                    {
                        evenPercent.Text = GeneralFuncsLib.FormatBorderText(evenPercent.Text, evenPercentColor.ToString().ToColor());
                    }

                    //duplicate percent
                    TableColumnContent dupPercent = dataItem.FindControl("uxDuplicateDollarTransactionPercent") as TableColumnContent;
                    dupPercent.ToolTip = rowItem["DuplicateDollarTransactionCount"].ToString().ToCurrencySymbol();
                    dupPercent.Text = VeraCodeSolution.DoVeraCode(rowItem["DuplicateDollarTransactionPercent"].ToString() != string.Empty ?
                        (decimal.Parse(rowItem["DuplicateDollarTransactionPercent"].ToString())).ToString("#,#0") + "%" : string.Empty);
                    var dupPercentColor = rowItem["DupColor"];
                    if (!dupPercentColor.IsNullOrEmpty())
                    {
                        dupPercent.Text = GeneralFuncsLib.FormatBorderText(dupPercent.Text, dupPercentColor.ToString().ToColor());
                    }

                    //duplicate bin
                    TableColumnContent dupBin = dataItem.FindControl("uxDuplicateBin") as TableColumnContent;
                    dupBin.ToolTip = VeraCodeSolution.DoVeraCode(string.Format("{0:C}", rowItem["DuplicateBin6TransactionAmount"]).ToCurrencySymbol());
                    dupBin.Text = VeraCodeSolution.DoVeraCode(rowItem["DuplicateBin"].ToString());
                    var dupBinColor = rowItem["SixDupBinColor"];
                    if (!dupBinColor.IsNullOrEmpty())
                    {
                        dupBin.Text = GeneralFuncsLib.FormatBorderText(dupBin.Text, dupBinColor.ToString().ToColor());
                    }

                    //SC
                    TableColumnContent sc = dataItem.FindControl("uxSingleCardTransToday") as TableColumnContent;
                    sc.ToolTip = VeraCodeSolution.DoVeraCode(string.Format("{0:C}", rowItem["VolumeOfSimilarCardTransaction"]).ToCurrencySymbol());
                    sc.Text = VeraCodeSolution.DoVeraCode(rowItem["SingleCardTransToday"].ToString());
                    var scColor = rowItem["DupBinColor"];
                    if (!scColor.IsNullOrEmpty())
                    {
                        sc.Text = GeneralFuncsLib.FormatBorderText(sc.Text, scColor.ToString().ToColor());
                    }

                    //# negative
                    TableColumnContent negative = dataItem.FindControl("uxNegativeBatchCount") as TableColumnContent;
                    negative.Text = VeraCodeSolution.DoVeraCode(rowItem["NegativeBatchCount"].ToString());
                    var negativeColor = rowItem["NegColor"];
                    if (!negativeColor.IsNullOrEmpty())
                    {
                        negative.Text = GeneralFuncsLib.FormatBorderText(negative.Text, negativeColor.ToString().ToColor());
                    }

                    //# zero
                    TableColumnContent zero = dataItem.FindControl("uxZeroBatchCount") as TableColumnContent;
                    zero.Text = VeraCodeSolution.DoVeraCode(rowItem["ZeroBatchCount"].ToString());
                    var zeroColor = rowItem["ZeroColor"];
                    if (!zeroColor.IsNullOrEmpty())
                    {
                        zero.Text = GeneralFuncsLib.FormatBorderText(zero.Text, zeroColor.ToString().ToColor());
                    }

                    // TK25211 - VWEB - Add Transaction Volume as a column in Barometer Report
                    // TV
                    TableColumnContent todayVol = dataItem.FindControl("uxTodayVolume") as TableColumnContent;
                    todayVol.Text = VeraCodeSolution.DoVeraCode(todayVol.CustomFormatDataValue(rowItem["TodayVolume"], FormatType.Currency));
                    var todayVolColor = rowItem["VolumeColor"];
                    if (!todayVolColor.IsNullOrEmpty())
                    {
                        todayVol.Text = GeneralFuncsLib.FormatBorderText(todayVol.Text, todayVolColor.ToString().ToColor());
                    }

                    //RTVL
                    TableColumnContent rtvl = dataItem.FindControl("uxTodayFirstTimeRetrievalVolume") as TableColumnContent;
                    rtvl.ToolTip = VeraCodeSolution.DoVeraCode(rowItem["TodayFirstTimeRetrievalCount"].ToString().ToCurrencySymbol());
                    rtvl.Text = VeraCodeSolution.DoVeraCode(
                        rowItem["TodayFirstTimeRetrievalVolume"].IsNullOrEmpty() ? string.Empty :
                        FormatData.FormatCurrency(rowItem["TodayFirstTimeRetrievalVolume"].ToDecimalAmount(), SessionManager.CurrencyFortmat)
                        );
                    var rtvlVolColor = rowItem["RTVLColor"];
                    if (!rtvlVolColor.IsNullOrEmpty())
                    {
                        rtvl.Text = GeneralFuncsLib.FormatBorderText(rtvl.Text, rtvlVolColor.ToString().ToColor());
                    }

                    //CB
                    TableColumnContent cb = dataItem.FindControl("uxTodayChargebackVolume") as TableColumnContent;
                    cb.ToolTip = rowItem["TodayChargebackCount"].ToString().ToCurrencySymbol();
                    cb.Text = VeraCodeSolution.DoVeraCode(
                        rowItem["TodayChargebackVolume"].IsNullOrEmpty() ? string.Empty :
                        FormatData.FormatCurrency(rowItem["TodayChargebackVolume"].ToDecimalAmount(), SessionManager.CurrencyFortmat) );
                    var cbColor = rowItem["CBColor"];
                    if (!cbColor.IsNullOrEmpty())
                    {
                        cb.Text = GeneralFuncsLib.FormatBorderText(cb.Text, cbColor.ToString().ToColor());
                    }

                    //Rtn Percent
                    TableColumnContent rtnPercent = dataItem.FindControl("uxReturnPercent") as TableColumnContent;
                    rtnPercent.ToolTip = VeraCodeSolution.DoVeraCode(GeneralFuncsLib.FormatCurrencyTooltip(rowItem["TodayReturnAmount"], SessionManager.CurrencyFortmat).ToCurrencySymbol());
                    rtnPercent.Text = VeraCodeSolution.DoVeraCode(
                        rowItem["ReturnPercent"].IsNullOrEmpty() ? string.Empty :
                        (rowItem["ReturnPercent"].ToDecimalAmount()).ToString("#,#0") + "%" 
                        );
                    var rtnPercentColor = rowItem["RtnColor"];
                    if (!rtnPercentColor.IsNullOrEmpty())
                    {
                        rtnPercent.Text = GeneralFuncsLib.FormatBorderText(rtnPercent.Text, rtnPercentColor.ToString().ToColor());
                    }

                    //Max ticket $
                    TableColumnContent maxTkt = dataItem.FindControl("uxTodayHighestTransactionAmount") as TableColumnContent;
                    maxTkt.Text = VeraCodeSolution.DoVeraCode(
                        rowItem["TodayHighestTransactionAmount"].IsNullOrEmpty() ? string.Empty :
                        FormatData.FormatCurrency(rowItem["TodayHighestTransactionAmount"].ToDecimalAmount(), SessionManager.CurrencyFortmat));
                    var maxTktColor = rowItem["MaxTktColor"];
                    if (!maxTktColor.IsNullOrEmpty())
                    {
                        maxTkt.Text = GeneralFuncsLib.FormatBorderText(maxTkt.Text, maxTktColor.ToString().ToColor());
                    }

                    //# Tkts
                    TableColumnContent tkt = dataItem.FindControl("uxTodayTransactionCount") as TableColumnContent;
                    tkt.Text = VeraCodeSolution.DoVeraCode(rowItem["TodayTransactionCount"].ToString());
                    var tktColor = rowItem["TktsColor"];
                    if (!tktColor.IsNullOrEmpty())
                    {
                        tkt.Text = GeneralFuncsLib.FormatBorderText(tkt.Text, tktColor.ToString().ToColor());
                    }

                    //# batch
                    TableColumnContent batch = dataItem.FindControl("uxTodayBatchCount") as TableColumnContent;
                    batch.Text = VeraCodeSolution.DoVeraCode(rowItem["TodayBatchCount"].ToString());
                    var batchColor = rowItem["BatchColor"];
                    if (!batchColor.IsNullOrEmpty())
                    {
                        batch.Text = GeneralFuncsLib.FormatBorderText(batch.Text, batchColor.ToString().ToColor());
                    }

                    //SIC
                    TableColumnContent sic = dataItem.FindControl("uxSIC") as TableColumnContent;
                    sic.ToolTip = rowItem["SICDescription"].ToString().Replace("&nbsp;", string.Empty);
                    sic.Text = VeraCodeSolution.DoVeraCode(rowItem["SICCode"].ToString());
                    //*/
                }
                break;
        }
    }
    #endregion

    #region Transactions
    [WebMethod(EnableSession = true)]
    public string[] GetNQTransactions(int pageIndex, int pageSize, bool havePager = true)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@ReportDate", RiskSessionManager.DetectionQueue.ReportDate, DbType.DateTime));
        parameters.Add(new FilterParameter("@MerchantNumber", RiskSessionManager.currentMerchantNumber, DbType.String));
        parameters.Add(new FilterParameter("@Mode", 0, DbType.Int32));

        SecurePage securePage = HttpContext.Current.Handler as SecurePage;
        if (GeneralFuncsLib.CheckCSViewFullCard(securePage))
        {
            parameters.AddDecryptDataParams("AccountNumber", false);
        }
        parameters.Add(new FilterParameter("@PageNo", pageIndex, DbType.Int32));
        parameters.Add(new FilterParameter("@PageSize", pageSize, DbType.Int32));
        parameters.Add(new FilterParameter("@IsPaging", true, DbType.Boolean));

        DataTable transactionList = WebServices.RiskServices.GetReports("spa_rm_GetNextQueueReportTransactionDetail", parameters);

        string data = BindTransactionForWS(transactionList);
        string pager = string.Empty;
        if (havePager)
            pager = GetPager(transactionList, pageSize, pageIndex);
        return new string[] { data, pager };
    }

    private string GetPager(DataTable data, int pageSize, int pageIndex)
    {
        string temp = string.Empty;
        if (data.IsNotNullData() && data.Rows.Count > 0)
        {
            int totalRows = Convert.ToInt32(data.Rows[0]["TotalRows"].ToString());
            if (totalRows > pageSize)
            {
                using (var sw = new StringWriter())
                {
                    var page = new Page();

                    Control control = page.LoadControl("~/UserControls/ASPager.ascx");
                    ((UserControls_ASPager)control).InitPager(totalRows, pageSize, pageIndex - 1);

                    page.Controls.Add(control);
                    HttpContext.Current.Server.Execute(page, sw, false);
                    temp = sw.ToString();
                }
            }
        }
        return temp;
    }

    private string BindTransactionForWS(DataTable data)
    {
        CalculateTransactionTotal(data);
        // Format data
        return FormatTransaction(data);

    }

    private void FormatTooltip(TableColumnContent content, string tooltipDup, bool isDup)
    {
        if (isDup)
        {
            content.BackgroundColor = "yellow !important";
            content.ToolTip = tooltipDup.ToCurrencySymbol();
        }

    }

    private string FormatTransaction(DataTable data)
    {
        StringBuilder list = new StringBuilder();
        SecurePage securePage = HttpContext.Current.Handler as SecurePage;

        for (int index = 0; index < data.Rows.Count; index++)
        {
            DataRow row = data.Rows[index];

            string txtDupAccountNum = string.Empty;
            bool isDup = false;
            if (GeneralFuncsLib.NvlString(row["DuplicateFlag"].ToString().ToLower()).Equals("true"))
            {
                isDup = true;
                txtDupAccountNum = VeraCodeSolution.DoVeraCode(RiskSessionManager.DQNextQReportCS_Text_DuplicateAccountNumber_Tooltip);
            }

            string acctNumberText = index.ToString() + ";NextQueue";
            string urlCard = "<a class=\"link\" href=\"javascript:void(0)\" onclick=\"ShowPopupAcctNumber('" + acctNumberText + "');\">";

            TableColumnContent uxPartialAccountNumber = new TableColumnContent(Alignment.Center);
            if (IsExport || !CheckCSViewFullCard(securePage))
                uxPartialAccountNumber.Text = VeraCodeSolution.DoVeraCode(urlCard + row["PartialAccountNumber"].ToString() + "</a>");
            else
                uxPartialAccountNumber.Text = VeraCodeSolution.DoVeraCode(urlCard + row["AccountNumber"].ToString() + "</a>");

            FormatTooltip(uxPartialAccountNumber, txtDupAccountNum, isDup);

            TableColumnContent cardType = new TableColumnContent(Alignment.Center);
            cardType.Text = VeraCodeSolution.DoVeraCode(row["CardType"].ToASString());
            cardType.ToolTip = VeraCodeSolution.DoVeraCode(row["CardDescription"].ToASString());
            // Format 1 line in one time
            FormatTooltip(cardType, txtDupAccountNum, isDup);

            TableColumnContent uxCountryCode = new TableColumnContent(Alignment.Center);
            uxCountryCode.Text = VeraCodeSolution.DoVeraCode(row["CountryCode"].ToASString());
            FormatTooltip(uxCountryCode, txtDupAccountNum, isDup);

            TableColumnContent uxFileSource = new TableColumnContent(Alignment.Center);
            uxFileSource.Text = VeraCodeSolution.DoVeraCode(row["FileSource"].ToASString());
            FormatTooltip(uxFileSource, txtDupAccountNum, isDup);

            TableColumnContent uxTransactionDate = new TableColumnContent(Alignment.Center);
            uxTransactionDate.Text = VeraCodeSolution.DoVeraCode(uxTransactionDate.CustomFormatDataValue(row["TransactionDate"], FormatType.Date));
            FormatTooltip(uxTransactionDate, txtDupAccountNum, isDup);

            TableColumnContent uxTransactionTime = new TableColumnContent(Alignment.Center);
            uxTransactionTime.Text = VeraCodeSolution.DoVeraCode(
                string.IsNullOrEmpty(row["TransactionTime"].ToString()) ? string.Empty : Convert.ToDateTime(row["TransactionTime"].ToString()).ToLongTimeString()
                );

            FormatTooltip(uxTransactionTime, txtDupAccountNum, isDup);

            TableColumnContent uxDupeCount = new TableColumnContent(Alignment.Center);
            uxDupeCount.Text = VeraCodeSolution.DoVeraCode(row["DupeCount"].ToASString());

            //dupe column 
            var dupeToolTip = VeraCodeSolution.DoVeraCode(string.Format("{0:C}", row["DupeAmount"]).ToCurrencySymbol());
            uxDupeCount.ToolTip = dupeToolTip;
            FormatTooltip(uxDupeCount, txtDupAccountNum, isDup);

            TableColumnContent uxAuthorizationNumber = new TableColumnContent(Alignment.Center);
            //handle for Auth
            if (IsExport)
            {
                uxAuthorizationNumber.Text = VeraCodeSolution.DoVeraCode(row["AuthorizationNumber"].ToSafeString());
            }
            else if (!String.IsNullOrEmpty(row["AuthorizationNumber"].ToString()))
            {

                uxAuthorizationNumber.Text = VeraCodeSolution.DoVeraCode(
                    GeneralFuncsLib.BuildRskAuthUrlPopupModalChild(
                    securePage, row["AuthorizationNumber"],
                    row["MerchantNumber"].ToString(), string.Empty,
                    row["TransactionDate"], 1,
                    row["AuthorizationNumber"].ToString(), false
                    ));
            }
            FormatTooltip(uxAuthorizationNumber, txtDupAccountNum, isDup);

            TableColumnContent uxKeyed = new TableColumnContent(Alignment.Center);
            uxKeyed.Text = VeraCodeSolution.DoVeraCode(row["Keyed"].ToASString());
            uxKeyed.ToolTip = VeraCodeSolution.DoVeraCode(row["EntryModeDescription"].ToASString());
            FormatTooltip(uxKeyed, txtDupAccountNum, isDup);

            TableColumnContent uxTransactionType = new TableColumnContent(Alignment.Center);
            uxTransactionType.Text = VeraCodeSolution.DoVeraCode(row["TransactionDescription"].ToASString());
            uxTransactionType.ToolTip = VeraCodeSolution.DoVeraCode(row["TransactionType"].ToASString());
            FormatTooltip(uxTransactionType, txtDupAccountNum, isDup);

            //Format Match
            TableColumnContent uxMatch = new TableColumnContent(Alignment.Center);
            uxMatch.Text = VeraCodeSolution.DoVeraCode(row["MatchCode"].ToASString());
            uxMatch.ToolTip = VeraCodeSolution.DoVeraCode(row["MatchName"].ToASString());
            if (row["MatchCode"].ToASString() == "P")
            {
                uxMatch.Text = GeneralFuncsLib.FormatBorderText(uxMatch.Text, Color.Blue);
            }
            else if (row["MatchCode"].ToASString() == "U")
            {
                uxMatch.Text = GeneralFuncsLib.FormatBorderText(uxMatch.Text, Color.Red);
            }
            else if (row["MatchCode"].ToASString() == "M")
            {
                uxMatch.Text = GeneralFuncsLib.FormatBorderText(uxMatch.Text, "#3fbf00".ToColor());
            }
            FormatTooltip(uxMatch, txtDupAccountNum, isDup);

            TableColumnContent uxTransactionAmount = new TableColumnContent(Alignment.Right);

            uxTransactionAmount.Text = VeraCodeSolution.DoVeraCode(FormatData.FormatCurrency(row["TransactionAmount"].ToDecimalAmount(), SessionManager.CurrencyFortmat));

            if (GeneralFuncsLib.NvlString(row["HighestTransactionAmountFlag"].ToString().ToLower()).Equals("true"))
            {
                uxTransactionAmount.Text = GeneralFuncsLib.FormatBorderText(uxTransactionAmount.Text, Color.Green);
                uxTransactionAmount.ToolTip = VeraCodeSolution.DoVeraCode(RiskSessionManager.DQNextQReportCS_Text_HighestTransactionAmount_Tooltip);
            }

            if (GeneralFuncsLib.NvlString(row["DuplicateFlag"].ToString().ToLower()).Equals("true"))
            {

                if (GeneralFuncsLib.NvlString(row["HighestTransactionAmountFlag"].ToString().ToLower()).Equals("true"))
                {
                    uxTransactionAmount.Text = GeneralFuncsLib.FormatBorderText(uxTransactionAmount.Text, Color.Green);
                    uxTransactionAmount.ToolTip = VeraCodeSolution.DoVeraCode(RiskSessionManager.DQNextQReportCS_Text_DuplicateAccountNumber_Tooltip + ";" +
                       RiskSessionManager.DQNextQReportCS_Text_HighestTransactionAmount_Tooltip);
                }
            }
            FormatTooltip(uxTransactionAmount, txtDupAccountNum, isDup);

            list.Append(string.Format("<tr class='{0}'>", index % 2 == 0 ? "Row" : "AltRow"));
            // Acct #
            list.Append(uxPartialAccountNumber.RenderHtml());
            // CT
            list.Append(cardType.RenderHtml());
            //Ctry
            list.Append(uxCountryCode.RenderHtml());
            //Source
            list.Append(uxFileSource.RenderHtml());
            //Trans Dt
            list.Append(uxTransactionDate.RenderHtml());
            //Trans time
            list.Append(uxTransactionTime.RenderHtml());
            // Dupe
            list.Append(uxDupeCount.RenderHtml());
            //AuthorizationNumber
            list.Append(uxAuthorizationNumber.RenderHtml());
            // Keyed
            list.Append(uxKeyed.RenderHtml());
            // TransactionType
            list.Append(uxTransactionType.RenderHtml());
            // Match
            list.Append(uxMatch.RenderHtml());
            // TransactionAmount
            list.Append(uxTransactionAmount.RenderHtml());

            list.Append("</tr>");
        }
        // Append total row
        if (data.Rows.Count > 0)
            list.Append(TransactionTotal());

        return list.ToString();
    }

    private string TransactionTotal()
    {
        StringBuilder line = new StringBuilder();

        line.Append("<tr class='Footer'>");
        line.Append(string.Format("<td align='left' class='heading'>{0}</td>", "&nbsp;" + RiskSessionManager.DQNextQReportCS_Text_Total + ": " + _totalRows));
        line.Append("<td></td><td></td><td></td><td></td><td></td><td></td><td></td>");
        line.Append(string.Format("<td align='center'>{0}</td>", VeraCodeSolution.DoVeraCode(_TransCountKeyed.ToString())));
        line.Append("<td></td><td></td>");
        line.Append(string.Format("<td align='right'>{0}</td>", VeraCodeSolution.DoVeraCode(FormatData.FormatCurrency(_TransAmount, SessionManager.CurrencyFortmat))));
        line.Append("</tr>");
        return line.ToString();
    }

    protected void uxReportGridTransactions_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        RepeaterItem dataItem = e.Item as RepeaterItem;
        DataRowView rowView = e.Item.DataItem as DataRowView;
        var rowItem = (e.Item.DataItem as DataRowView).Row;

        TableColumnContent cardType = dataItem.FindControl("uxCardType") as TableColumnContent;
        cardType.Text = VeraCodeSolution.DoVeraCode(rowView["CardType"].ToASString());
        cardType.ToolTip = VeraCodeSolution.DoVeraCode(rowView["CardDescription"].ToASString());

        TableColumnContent uxCountryCode = dataItem.FindControl("uxCountryCode") as TableColumnContent;
        uxCountryCode.Text = VeraCodeSolution.DoVeraCode(rowView["CountryCode"].ToASString());
        //42895 – VW – IPMT - Add hover over for Ctry column in Next Queue and Security Report
        uxCountryCode.ToolTip = VeraCodeSolution.DoVeraCode(rowView["CountryName"].ToASString());

        TableColumnContent uxFileSource = dataItem.FindControl("uxFileSource") as TableColumnContent;
        uxFileSource.Text = VeraCodeSolution.DoVeraCode(rowView["FileSource"].ToASString());

        TableColumnContent uxTransactionDate = dataItem.FindControl("uxTransactionDate") as TableColumnContent;
        uxTransactionDate.Text = VeraCodeSolution.DoVeraCode(uxTransactionDate.CustomFormatDataValue(rowView["TransactionDate"], FormatType.Date));

        TableColumnContent uxDupeCount = dataItem.FindControl("uxDupeCount") as TableColumnContent;
        uxDupeCount.Text = VeraCodeSolution.DoVeraCode(rowView["DupeCount"].ToASString());

        TableColumnContent keyed = dataItem.FindControl("uxKeyed") as TableColumnContent;
        keyed.Text = VeraCodeSolution.DoVeraCode(rowView["Keyed"].ToASString());
        keyed.ToolTip = VeraCodeSolution.DoVeraCode(rowView["EntryModeDescription"].ToASString());

        TableColumnContent transactionType = dataItem.FindControl("uxTransactionType") as TableColumnContent;
        transactionType.Text = VeraCodeSolution.DoVeraCode(rowView["TransactionDescription"].ToASString());
        transactionType.ToolTip = VeraCodeSolution.DoVeraCode(rowView["TransactionType"].ToASString());

        string acctNumberText = e.Item.ItemIndex.ToString() + ";NextQueue";
        string urlCard = "<a class=\"link\" href=\"javascript:void(0)\" onclick=\"ShowPopupAcctNumber('" + acctNumberText + "');\">";
        TableColumnContent partialAccountNumber = dataItem.FindControl("uxPartialAccountNumber") as TableColumnContent;
        if (IsExport || !CheckCSViewFullCard())
            partialAccountNumber.Text = VeraCodeSolution.DoVeraCode(urlCard + rowView["PartialAccountNumber"].ToString() + "</a>");
        else
            partialAccountNumber.Text = VeraCodeSolution.DoVeraCode(urlCard + rowView["AccountNumber"].ToString() + "</a>");
        //dupe column 
        var dupeToolTip = VeraCodeSolution.DoVeraCode(string.Format("{0:C}", rowItem["DupeAmount"]).ToCurrencySymbol());
        TableColumnContent dupe = dataItem.FindControl("uxDupeCount") as TableColumnContent;
        dupe.Text = VeraCodeSolution.DoVeraCode(rowView["DupeCount"].ToASString());
        dupe.ToolTip = dupeToolTip;

        //handle for Auth
        TableColumnContent authorizationNumber = dataItem.FindControl("uxAuthorizationNumber") as TableColumnContent;
        if (IsExport)
        {
            authorizationNumber.Text = VeraCodeSolution.DoVeraCode(rowView["AuthorizationNumber"].ToSafeString());
        }
        else if (!String.IsNullOrEmpty(rowView["AuthorizationNumber"].ToString()))
        {
            authorizationNumber.Text = VeraCodeSolution.DoVeraCode(
                GeneralFuncsLib.BuildRskAuthUrlPopupModalChild(
                (SecurePage)Page, rowView["AuthorizationNumber"],
                rowView["MerchantNumber"].ToString(), string.Empty,
                rowView["TransactionDate"], index + 1,
                rowView["AuthorizationNumber"].ToString(), false
                ));
        }

        TableColumnContent transactionAmount = dataItem.FindControl("uxTransactionAmount") as TableColumnContent;
        transactionAmount.Text = VeraCodeSolution.DoVeraCode(FormatData.FormatCurrency(rowView["TransactionAmount"].ToDecimalAmount(), SessionManager.CurrencyFortmat));

        TableColumnContent transactionTime = dataItem.FindControl("uxTransactionTime") as TableColumnContent;
        transactionTime.Text = VeraCodeSolution.DoVeraCode(
            string.IsNullOrEmpty(rowView["TransactionTime"].ToString()) ? string.Empty : Convert.ToDateTime(rowView["TransactionTime"].ToString()).ToLongTimeString()
            );

        if (GeneralFuncsLib.NvlString(rowView["HighestTransactionAmountFlag"].ToString().ToLower()).Equals("true"))
        {
            transactionAmount.Text = GeneralFuncsLib.FormatBorderText(transactionAmount.Text, Color.Green);
            transactionAmount.ToolTip = VeraCodeSolution.DoVeraCode(this.GetLocalResourceObject("DQNextQReportCS_Text_HighestTransactionAmount").ToString());
        }
        if (GeneralFuncsLib.NvlString(rowView["DuplicateFlag"].ToString().ToLower()).Equals("true"))
        {
            foreach (Control ctrl in dataItem.Controls)
            {
                dupe.ToolTip = dupeToolTip;
                if (ctrl is TableColumnContent)
                {
                    ((TableColumnContent)ctrl).BackgroundColor = "yellow !important";
                    ((TableColumnContent)ctrl).ToolTip = VeraCodeSolution.DoVeraCode(GetLocalResourceObject("DQNextQReportCS_Text_DuplicateAccountNumber").ToString());
                }
            }

            if (GeneralFuncsLib.NvlString(rowView["HighestTransactionAmountFlag"].ToString().ToLower()).Equals("true"))
            {
                transactionAmount.Text = GeneralFuncsLib.FormatBorderText(transactionAmount.Text, Color.Green);
                transactionAmount.ToolTip = VeraCodeSolution.DoVeraCode(GetLocalResourceObject("DQNextQReportCS_Text_DuplicateAccountNumber").ToString() + ";" +
                   GetLocalResourceObject("DQNextQReportCS_Text_HighestTransactionAmount").ToString());
            }
        }

        if (IsExport && dataItem.ItemIndex == ((DataTable)uxReportGridTransactions.DataSource).Rows.Count - 1)
        {
            partialAccountNumber.CssClass = keyed.CssClass = transactionAmount.CssClass = "heading";
            //dataItem.Attributes.Add("class", "rgFooter"); 
        }

        //Format Match
        //46807 - [VWEB] - Transaction Type Fix and Match Column Addition to Risk Report
        TableColumnContent uxMatch = dataItem.FindControl("uxMatch") as TableColumnContent;
        uxMatch.Text = VeraCodeSolution.DoVeraCode(rowView["MatchCode"].ToASString());
        uxMatch.ToolTip = VeraCodeSolution.DoVeraCode(rowView["MatchName"].ToASString());
        if (rowView["MatchCode"].ToASString() == "P")
        {
            uxMatch.Text = GeneralFuncsLib.FormatBorderText(uxMatch.Text, Color.Blue);
        }
        else if (rowView["MatchCode"].ToASString() == "U")
        {
            uxMatch.Text = GeneralFuncsLib.FormatBorderText(uxMatch.Text, Color.Red);
        }
        else if (rowView["MatchCode"].ToASString() == "M")
        {
            uxMatch.Text = GeneralFuncsLib.FormatBorderText(uxMatch.Text, "#3fbf00".ToColor());
        }
    }

    protected void uxAccountNumberClick_Click(object sender, EventArgs e)
    {

        string[] arg = uxHiddenAccountNumberClick.Value.Split(';');
        int itemIndex = Convert.ToInt32(arg[0]);
        string recordID = string.Empty;
        string partialCardNum = string.Empty;
        DateTime rpDate = DateTime.Now;
        if (arg[1] == "Chargebacks")
        {
            DataTable chargeback = RiskSessionManager.CurrentRiskNextQueue.ChargeBack;
            recordID = chargeback.Rows[itemIndex]["RecordID"].ToString();
            partialCardNum = chargeback.Rows[itemIndex]["PartialAccountNumber"].ToString();
            rpDate = DateTime.Parse(chargeback.Rows[itemIndex]["ReportDate"].ToString());
        }
        else
        {
            DataTable transactions = RiskSessionManager.CurrentRiskNextQueue.Transaction;
            recordID = transactions.Rows[itemIndex]["RecordID"].ToString();
            partialCardNum = transactions.Rows[itemIndex]["PartialAccountNumber"].ToString();
            rpDate = DateTime.Parse(transactions.Rows[itemIndex]["ReportDate"].ToString());
        }
        string reportType = arg[1];
        string fullCC = String.Empty;
        FilterParameterCollection parameters_FullCard = new FilterParameterCollection();

        parameters_FullCard.Add(new FilterParameter("@UserID", SessionManager.CurrentUser.UserID, DbType.AnsiString));
        parameters_FullCard.Add(new FilterParameter("@ASClient", SessionManager.CurrentUser.ASClient, DbType.Int32));
        parameters_FullCard.Add(new FilterParameter("@RecordID", Convert.ToInt32(recordID), System.Data.DbType.Int32));
        parameters_FullCard.Add(new FilterParameter("@ReportType", reportType, System.Data.DbType.AnsiString));
        parameters_FullCard.Add(new FilterParameter("@ReportDate", rpDate, System.Data.DbType.DateTime));

        SecurePage securePage = HttpContext.Current.Handler as SecurePage;
        if (GeneralFuncsLib.CheckCSViewFullCard((SecurePage)this.Page))
        {
            parameters_FullCard.AddDecryptDataParams("AccountNumber");
        }

        DataTable dt_FullCard = WebServices.CsReportServices.GetReports("spa_cs_GetFullCardNumber", parameters_FullCard);
        if (dt_FullCard != null && dt_FullCard.Rows.Count > 0)
        {
            fullCC = dt_FullCard.Rows[0][0].ToString();
        }

        string queryString = Page.BuildSecureQueryString("cn=" + partialCardNum + "&cnf=" + fullCC + "&merch=" + RiskSessionManager.currentMerchantNumber + "&isRisk=1");
        string urlCardDetail = ResolveUrl("~/") + "CardHistoryModal.aspx?" + queryString;
        Telerik.Web.UI.RadAjaxManager ajax = Telerik.Web.UI.RadAjaxManager.GetCurrent(this.Page);
        ajax.ResponseScripts.Add("openPopupCardWindow('" + urlCardDetail + "');");
    }

    protected string ConvertDate(object date)
    {
        if (date == DBNull.Value)
            return string.Empty;
        return Convert.ToDateTime(date).ToString("MM/dd/yyyy");
    }

    protected void uxAccount_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.SelectedAccountNumber);
    }

    protected void uxReportGridTransactions_PreRender(object sender, EventArgs e)
    {
        uxPartialAccountNumberTotal.Text = "&nbsp;" + GetLocalResourceObject("DQNextQReportCS_Text_Total").ToString() + ": " + _totalRows;
        uxTransKeyedTotal.Text = VeraCodeSolution.DoVeraCode(_TransCountKeyed.ToString());
        uxTransactionAmountFooterTotal.Text = VeraCodeSolution.DoVeraCode(FormatData.FormatCurrency(_TransAmount, SessionManager.CurrencyFortmat));

        if (uxReportGridTransactions.Items.Count == 0)
            uxReportGridTransactionsFooter.Visible = false;
        else
        {
            uxReportGridTransactionsFooter.Visible = true;
        }
    }

    #endregion

    private string BuildUrlForFullCard(string recordId, string reportType, string ReportDate)
    {
        string queryString = Page.BuildSecureQueryString("rt=" + reportType + "&cn=" + recordId + "&reportdate=" + ReportDate + "&idx=" + (index + 1));
        queryString = ResolveUrl("~/") + "FullCC.aspx?" + queryString;
        return queryString;
    }

    #region ChargeBacks
    protected void uxChargeback_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        RepeaterItem dataItem = e.Item as RepeaterItem;
        DataRowView rowView = e.Item.DataItem as DataRowView;

        string blueDot = string.Format(IMAGE, BuildUrlForFullCard(rowView["RecordID"].ToString(), ReportType.CHARGEBACKS, rowView["ReportDate"].ToString()), 1);
        string acctNumberText = e.Item.ItemIndex.ToString() + ";Chargebacks";
        string urlCard = "<a class=\"link\" href=\"javascript:void(0)\" onclick=\"ShowPopupAcctNumber('" + acctNumberText + "');\">";

        TableColumnContent partialAccountNumber = dataItem.FindControl("uxPartialAccountNumber") as TableColumnContent;
        if (IsExport || !CheckCSViewFullCard())
            partialAccountNumber.Text = VeraCodeSolution.DoVeraCode(urlCard + rowView["PartialAccountNumber"].ToString() + "</a>");
        else
            partialAccountNumber.Text = VeraCodeSolution.DoVeraCode(urlCard + rowView["AccountNumber"].ToString() + "</a>");


        TableColumnContent transactionDate = dataItem.FindControl("uxTransactionDate") as TableColumnContent;
        transactionDate.Text = VeraCodeSolution.DoVeraCode(transactionDate.CustomFormatDataValue(rowView["TransactionDate"], FormatType.Date));
        TableColumnContent reasonCode = dataItem.FindControl("uxReasonCode") as TableColumnContent;
        reasonCode.Text = VeraCodeSolution.DoVeraCode(rowView["ReasonCode"].ToASString() + "&nbsp;");
        TableColumnContent keyed = dataItem.FindControl("uxKeyed") as TableColumnContent;
        //  42867 - Chargeback section of the Next Queue 'Key' always N
        if (rowView["Keyed"].IsNullOrEmpty())
        {
            keyed.Text = WebSiteConstants.HTML_EM_DASH;
        }
        else
        {
            keyed.Text = VeraCodeSolution.DoVeraCode(rowView["Keyed"].ToASString());
        }

        TableColumnContent transactionAmount = dataItem.FindControl("uxTransactionAmount") as TableColumnContent;
        if (rowView["TransactionAmount"].IsNullOrEmpty())
        {
            transactionAmount.Text = WebSiteConstants.HTML_EM_DASH;
        }
        else
        {
            transactionAmount.Text = FormatData.FormatCurrency(rowView["TransactionAmount"].ToDecimalAmount(), SessionManager.CurrencyFortmat);
        }

        // Card type
        TableColumnContent cardType = dataItem.FindControl("uxCardType") as TableColumnContent;
        cardType.Text = VeraCodeSolution.DoVeraCode(rowView["CardType"].ToASString());

        // Report date
        TableColumnContent reportDate = dataItem.FindControl("uxReportDate") as TableColumnContent;
        reportDate.Text = VeraCodeSolution.DoVeraCode(reportDate.CustomFormatDataValue(rowView["ReportDate"], FormatType.Date));
    }

    protected void uxChargeback_PreRender(object sender, EventArgs e)
    {
        if (uxChargeback.Items.Count == 0)
            uxChargebackFooter.Visible = false;
        else uxChargebackFooter.Visible = true;

        uxKeyedTotal.Text = _ChargebackCountKeyed.ToString();
        uxTransactionAmountTotal.Text = FormatData.FormatCurrency(_ChargebackAmount, SessionManager.CurrencyFortmat);
    }
    #endregion

    protected void uxMerchantInfo_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            //46652 - AW - Multi-Currency Transaction Display
            var control = e.Item.FindControl("lituxMerchantInfoHeaderAuth") as System.Web.UI.WebControls.Literal;
            control.Text = control.Text.ToCurrencySymbol();

            DataRowView rowItem = e.Item.DataItem as DataRowView;
            //checkbox column
            Control ctrl = e.Item.FindControl("chkMerchantWorked");
            if (ctrl != null)
            {
                HtmlInputCheckBox chkBox = ctrl as HtmlInputCheckBox;
                chkBox.Checked = rowItem["Worked"].Equals(true);
                chkBox.Value = rowItem["MerchantNumber"].ToString();
                chkBox.Attributes.Add("onclick", string.Format("ChangeMerchantWorked(this,{0})", rowItem["TodayVolume"]));
            }

            Label lblProfile = e.Item.FindControl("lblProfile") as Label;
            lblProfile.Text = VeraCodeSolution.DoVeraCode(rowItem["ProfileDescription"].ToString());
            if (rowItem["Profile"].ToString() != "0")
                lblProfile.ToolTip = VeraCodeSolution.DoVeraCode(rowItem["Profile"] + " - " + rowItem["ProfileDescription"]);

            //Format data
            Label uxTodayAuthorizationVolume = e.Item.FindControl("uxTodayAuthorizationVolume") as Label;
            FormatCurrency(uxTodayAuthorizationVolume, VeraCodeSolution.DoVeraCode(rowItem["TodayAuthorizationVolume"].ToString()));
            Label uxTodayVolume = e.Item.FindControl("uxTodayVolume") as Label;
            FormatCurrency(uxTodayVolume, VeraCodeSolution.DoVeraCode(rowItem["TodayVolume"].ToString()));
            Label uxAVGDailyVolume = e.Item.FindControl("uxAVGDailyVolume") as Label;
            FormatCurrency(uxAVGDailyVolume, VeraCodeSolution.DoVeraCode(rowItem["AVGDailyVolume"].ToString()));
            Label uxTodayAverageTicket = e.Item.FindControl("uxTodayAverageTicket") as Label;
            FormatCurrency(uxTodayAverageTicket, VeraCodeSolution.DoVeraCode(rowItem["TodayAverageTicket"].ToString()));
            Label uxExpectedAverageTicket = e.Item.FindControl("uxExpectedAverageTicket") as Label;
            FormatCurrency(uxExpectedAverageTicket, VeraCodeSolution.DoVeraCode(rowItem["ExpectedAverageTicket"].ToString()));
            Label uxMTDVolume = e.Item.FindControl("uxMTDVolume") as Label;
            FormatCurrency(uxMTDVolume, VeraCodeSolution.DoVeraCode(rowItem["MTDVolume"].ToString()));
            Label uxExpectedMonthlyVolume = e.Item.FindControl("uxExpectedMonthlyVolume") as Label;
            FormatCurrency(uxExpectedMonthlyVolume, VeraCodeSolution.DoVeraCode(rowItem["ExpectedMonthlyVolume"].ToString()));
            Label uxMV1 = e.Item.FindControl("uxMV1") as Label;
            FormatCurrency(uxMV1, VeraCodeSolution.DoVeraCode(rowItem["MV1"].ToString()));
            Label uxTodaySaleAmount = e.Item.FindControl("uxTodaySaleAmount") as Label;
            FormatCurrency(uxTodaySaleAmount, VeraCodeSolution.DoVeraCode(rowItem["TodaySaleAmount"].ToString()));
            Label uxTodayReturnAmount = e.Item.FindControl("uxTodayReturnAmount") as Label;
            FormatCurrency(uxTodayReturnAmount, VeraCodeSolution.DoVeraCode(rowItem["TodayReturnAmount"].ToString()));
            Label uxYTDVolume = e.Item.FindControl("uxYTDVolume") as Label;
            FormatCurrency(uxYTDVolume, VeraCodeSolution.DoVeraCode(rowItem["YTDVolume"].ToString()));
            Label uxExpectedYearlyVolume = e.Item.FindControl("uxExpectedYearlyVolume") as Label;
            FormatCurrency(uxExpectedYearlyVolume, VeraCodeSolution.DoVeraCode(rowItem["ExpectedYearlyVolume"].ToString()));
            Label uxMV2 = e.Item.FindControl("uxMV2") as Label;
            FormatCurrency(uxMV2, VeraCodeSolution.DoVeraCode(rowItem["MV2"].ToString()));
            Label uxMV3 = e.Item.FindControl("uxMV3") as Label;
            FormatCurrency(uxMV3, VeraCodeSolution.DoVeraCode(rowItem["MV3"].ToString()));
            System.Web.UI.WebControls.LinkButton uxMerchantNumLink = e.Item.FindControl("uxMerchantNumLink") as System.Web.UI.WebControls.LinkButton;
            uxMerchantNumLink.OnClientClick = "openPopupWindow('rm_RiskReport.aspx?" + Page.BuildSecureQueryString(string.Format("merchantNumber={0}&IsPopup=true", RiskSessionManager.currentMerchantNumber)) + "','RiskReport'); return false;";
            uxMerchantNumLink.Text = rowItem["MerchantNumber"].ToString();
        }
    }

    protected void FormatCurrency(Label sender, string value)
    {
        if (value.IsNullOrEmpty()) return;
        Double temp = Double.Parse(value);
        if (temp < 0)
        {
            sender.ForeColor = Color.Red;
            temp = temp * -1;
            sender.Text = "(" + FormatData.FormatCurrency(temp, SessionManager.CurrencyFortmat) + ")";
        }
        else
        {
            sender.Text = FormatData.FormatCurrency(temp, SessionManager.CurrencyFortmat);
        }
    }

    public bool IsExport
    {
        get;
        set;
    }

    public string HtmlExport
    {
        get;
        set;
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        RiskSessionManager.DQNextQReportCS_Text_DuplicateAccountNumber_Tooltip = GetLocalResourceObject("DQNextQReportCS_Text_DuplicateAccountNumber").ToString();
        RiskSessionManager.DQNextQReportCS_Text_HighestTransactionAmount_Tooltip = GetLocalResourceObject("DQNextQReportCS_Text_HighestTransactionAmount").ToString();
        RiskSessionManager.DQNextQReportCS_Text_Total = GetLocalResourceObject("DQNextQReportCS_Text_Total").ToString();

        uxAccount.Visible = !IsExport;
        uxAccountNumberClick.Visible = !IsExport;
        uxAccReload.Visible = !IsExport;
        if (uxMerchantInfo.Items.Count > 0)
        {
            ((ImageButton)uxMerchantInfo.Items[0].FindControl("uxCwinButton")).Visible = !IsExport;
            if (IsExport)
            {
                ((System.Web.UI.WebControls.Literal)uxMerchantInfo.Items[0].FindControl("txtMerchantNumber")).Text = "<font color=\"Green\">&nbsp;" + RiskSessionManager.currentMerchantNumber + "&nbsp;</font>";
                ((System.Web.UI.WebControls.LinkButton)uxMerchantInfo.Items[0].FindControl("uxMerchantNumLink")).Visible = false;
                uxMerchantInfoTitle.Visible = true;
            }
        }
    }

    public void PrepareForExport()
    {
        IsExport = true;
        OnDataBindControls(DataBindAction.BindBarometer);
        OnDataBindControls(DataBindAction.BindChargeBack);
        OnDataBindControls(DataBindAction.BindTransaction);
        uxPnlPager.Visible = false;
    }

    private string TransactionHtml()
    {
        string[] datas = this.GetNQTransactions(1, int.MaxValue, false);
        if (string.IsNullOrEmpty(datas[0]))
            return string.Empty;
        StringWriter tw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(tw);
        StringBuilder strs = new StringBuilder();
        //Header
        strs.Append("<table> <tr>");
        TableColumnHeader25.RenderControl(hw);
        TableColumnHeader30.RenderControl(hw);
        TableColumnHeader31.RenderControl(hw);
        TableColumnHeader32.RenderControl(hw);
        TableColumnHeader33.RenderControl(hw);
        TableColumnHeader34.RenderControl(hw);
        TableColumnHeader35.RenderControl(hw);
        TableColumnHeader36.RenderControl(hw);
        TableColumnHeader37.RenderControl(hw);
        TableColumnHeader38.RenderControl(hw);
        TableColumnHeader39.RenderControl(hw);
        TableColumnHeader40.RenderControl(hw);
        strs.Append(tw.ToString());
        strs.Append("</tr>");

        //Data
        strs.Append(datas[0]);
        strs.Append("</table>");

        return strs.ToString();
    }

    protected override void Render(HtmlTextWriter writer)
    {
        if (IsExport)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter hWriter = new HtmlTextWriter(sw);
            base.Render(hWriter);
            HtmlExport = sb.ToString();
            //Replace full transaction
            string reg = "(<!--{0}-->((.|\n)*)<!--/{0}-->)";
            string regHeader = string.Format(reg, "Transaction");
            Match headerMatch = Regex.Match(HtmlExport, regHeader);
            string data = TransactionHtml();
            if (headerMatch.Success && !string.IsNullOrEmpty(data))
                HtmlExport = HtmlExport.Replace(headerMatch.Value, data);

            ExportToExcel(HtmlExport);
        }
        else
        {
            base.Render(writer);
        }
    }

    private void ExportToExcel(string htmlString)
    {
        htmlString = "<html xmlns=\"http://www.w3.org/1999/xhtml\"><meta http-equiv=\"content-type\" content=\"application/xhtml+xml; charset=UTF-8\" />  "
                + @"
                     <head>
                        <style type='text/css'>
                            table tr{
                                border: thin solid #333;
                            }  
                            tr.Footer td
                            {
                                font-weight:bold;
                                font-style: italic;    
                            }
                            .rgMasterTable, .MPSBorder{ border:thin solid #000;} 
                            .sub-table{ border:0px solid #000;}
                            .rgHeader{ background-color: #ddd; font-weight:bold; } 
                            table.MPSBorder td.Caption{ background-color: #ddd;}   
                        </style>
                    </head>"
            + htmlString + "</html>";
        htmlString = htmlString.Replace("!important", string.Empty);
        htmlString = htmlString.Replace("class=\"Caption AltRow\"", "class=\"Caption\"");
        string fileName = GetLocalResourceObject("DQNextReportCS_Test_NestQReport").ToString() + "_" + RiskSessionManager.currentMerchantNumber;
        Response.Clear(); //this clears the Response of any headers or previous output
        Response.Buffer = true; //make sure that the entire output is rendered simultaneously
        Response.ContentType = "application/vnd.ms-excel";
        HttpContext.Current.Response.AppendHeader("content-disposition", AS.Common.VeraCodeSolution.RemoveCRLF(string.Format("attachment; filename={0}.xls", fileName)));
        Response.Write(htmlString);
        Response.End();
        IsExport = false;
    }

    public void VisibleGrid()
    {
        uxBarometerGrid.Visible = false;
        uxReportGridTransactions.Visible = false;
        uxChargeback.Visible = false;
    }

    public string BuildBlueDot(string recordId, string reportType, string reportDate, int index)
    {
        string queryString = Page.BuildSecureQueryString(
            string.Format("rt={0}&cn={1}&reportdate={2}&idx={3}",
                          reportType,
                          Page.Server.UrlEncode(recordId),
                          reportDate,
                          index));

        string urlFullCardDetail = string.Format(Page.ResolveUrl("~") + "FullCC.aspx?{0}", queryString);
        //string img = string.Format("<img src='{0}res/img/information.png' border=\"0\"/>", Page.ResolveUrl("~"));
        return string.Format(
                "<span class='info-link' onclick=\" return parent.ShowPopupModalChild({0}, '{1}','auto');\"></span>",
                index, urlFullCardDetail);
    }

    private void BindTransaction(DataTable data, int pageIndex)
    {
        CalculateTransactionTotal(data);
        uxReportGridTransactions.DataSource = data;
        uxReportGridTransactions.DataBind();

        if (data.IsNotNullData() && data.Rows.Count > 0)
        {
            int totalRows = Convert.ToInt32(data.Rows[0]["TotalRows"].ToString());
            int pageSize = WebSiteSettings.DefaultRiskPageSize;
            if (totalRows > pageSize)
            {
                uxPnlPager.Visible = true;
                uxPnlPager.Controls.Clear();
                Control control = this.Page.LoadControl("~/UserControls/ASPager.ascx");
                ((UserControls_ASPager)control).InitPager(totalRows, pageSize, pageIndex - 1);
                uxPnlPager.Controls.Add(control);
            }
            else
            {
                uxPnlPager.Visible = false;
            }
        }
    }
}
