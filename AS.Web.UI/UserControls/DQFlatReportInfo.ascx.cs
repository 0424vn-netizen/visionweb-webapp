using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Drawing;
using AS.Common;
using AS.Controls.Pages;
using AS.Controls.Grid;
using AS.Utilities;
using AS.Common.Formater;
using AS.Utilities;

public partial class UserControls_DQFlatReportInfo : GlobalUserControl
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

    #region Const
    private const string MATCHED_CODE = "MatchCode";
    private const string MATCHED_NAME = "MatchName";
    #endregion

    #region Properties

    int index = 0;
    private int _CountKeyed = 0;
    private decimal _TransAmount = 0;

    private string IMAGE
    {
        get
        {
            return "<a class=\"image-link\" href=\"#\" style=\"cursor:pointer\" onclick=\" return ShowPopupModalChild({0},'{1}','auto');\"><img src='" + ResolveUrl("~/") + "res/img/information.png' border='0' /></a>";
        }
    }
    private string CardSearchIntruderQuery(ASGrid grid)
    {
        string _CardSearchIntruderQuery = string.Empty;
        if (_CardSearchIntruderQuery == string.Empty)
        {
            _CardSearchIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(grid.ID, new string[] { "AccountNumber" });
        }
        return _CardSearchIntruderQuery;
    }

    private string CardSearchIntruderQuery(ASGrid grid, string columnName)
    {
        string _CardSearchIntruderQuery = string.Empty;
        if (_CardSearchIntruderQuery == string.Empty)
        {
            _CardSearchIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(grid.ID, new string[] { columnName });
        }
        return _CardSearchIntruderQuery;
    }

    private string AuthIntruderQuery(ASGrid grid)
    {
        string _AuthIntruderQuery = string.Empty;
        if (_AuthIntruderQuery == string.Empty)
        {
            _AuthIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(grid.ID, new string[] { "AuthorizationNumber" });
        }
        return _AuthIntruderQuery;
    }

    private DateTime _ReportDate = DateTime.Now;
    public DateTime ReportDate
    {
        get { return _ReportDate; }
        set { _ReportDate = value; }
    }

    private string _MerchantNumber = string.Empty;
    public string MerchantNumber
    {
        get { return _MerchantNumber; }
        set { _MerchantNumber = value; }
    }

    private string _MerchantName = string.Empty;
    public string MerchantName
    {
        get { return _MerchantName; }
        set { _MerchantName = value; }
    }

    private DataSet _DQFlatReportInfo = null;
    public DataSet DQFlatReportInfo
    {
        get
        {
            return _DQFlatReportInfo;
        }
        set
        {
            _DQFlatReportInfo = value;
        }
    }

    private DataTable _MerchantInfo = null;
    public DataTable MerchantInfo
    {
        get
        {
            return _MerchantInfo;
        }
        set
        {
            _MerchantInfo = ToDataTable(value);
        }
    }


    private DataTable _BarometerInfo = null;
    public DataTable BarometerInfo
    {
        get
        {
            return _BarometerInfo;
        }
        set
        {
            _BarometerInfo = ToDataTable(value);
        }
    }

    private DataTable _TransactionList
    {
        get
        {
            if (Session[this.ClientID] != null)
                return (DataTable)Session[this.ClientID];
            return null;
        }
        set
        {
            Session[this.ClientID] = value;
        }
    }
    public DataTable TransactionList
    {
        get
        {
            return _TransactionList;
        }
        set
        {
            _TransactionList = ToDataTable(value);
        }
    }

    private DataTable _ChargebackList = null;
    public DataTable ChargebackList
    {
        get
        {
            return _ChargebackList;
        }
        set
        {
            _ChargebackList = ToDataTable(value);
        }
    }

    private DataTable _DailyFCList = null;
    public DataTable DailyFCList
    {
        get
        {
            return _DailyFCList;
        }
        set
        {
            _DailyFCList = ToDataTable(value);
        }
    }

    private const int ScrollRowCount = 30;

    private bool HasRQColumn
    {
        get
        {
            return GeneralFuncsLib.HasQueuingMechanismFeature
                                && RiskSessionManager.DetectionQueue != null
                                && RiskSessionManager.DetectionQueue.ReportDate == DateTime.Today;
        }
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsIntruderDetected) return;
        //46652 - AW - Multi-Currency Transaction Display
        uxBarometerGrid.Columns.FindByUniqueName("TodayFirstTimeRetrievalVolume").HeaderText =
            uxBarometerGrid.Columns.FindByUniqueName("TodayFirstTimeRetrievalVolume").HeaderText.ToCurrencySymbol();
        uxBarometerGrid.Columns.FindByUniqueName("TodayFirstTimeRetrievalVolume").HeaderTooltip =
            uxBarometerGrid.Columns.FindByUniqueName("TodayFirstTimeRetrievalVolume").HeaderTooltip.ToCurrencySymbol();
        uxBarometerGrid.Columns.FindByUniqueName("TodayHighestTransactionAmount").HeaderText =
            uxBarometerGrid.Columns.FindByUniqueName("TodayHighestTransactionAmount").HeaderText.ToCurrencySymbol();
         uxBarometerGrid.Columns.FindByUniqueName("TodayHighestTransactionAmount").HeaderTooltip =
            uxBarometerGrid.Columns.FindByUniqueName("TodayHighestTransactionAmount").HeaderTooltip.ToCurrencySymbol();
         uxBarometerGrid.Columns.FindByUniqueName("TodayHighestTransactionAmount").HeaderText =
             uxBarometerGrid.Columns.FindByUniqueName("TodayHighestTransactionAmount").HeaderText.ToCurrencySymbol();
         uxBarometerGrid.Columns.FindByUniqueName("TodayHighestTransactionAmount").HeaderTooltip =
            uxBarometerGrid.Columns.FindByUniqueName("TodayHighestTransactionAmount").HeaderTooltip.ToCurrencySymbol();
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        if (Page.IsIntruderDetected) return;
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindMerchantInfo:
                {
                    uxMerchantInfo.DataSource = _MerchantInfo;
                    uxMerchantInfo.DataBind();
                }
                break;
            case DataBindAction.BindBarometer:
                {
                    uxBarometerGrid.DataSource = _BarometerInfo;
                    uxBarometerGrid.DataBind();
                }
                break;
            case DataBindAction.BindTransaction:
                {
                    HandleTable(_TransactionList);
                    uxReportGridTransactions.DataSource = _TransactionList;
                    uxReportGridTransactions.DataBind();
                }
                break;
            case DataBindAction.BindChargeBack:
                {
                    uxChargeback.DataSource = _ChargebackList;
                    uxChargeback.DataBind();
                }
                break;

            default: break;
        }
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

    private DataTable ToDataTable(DataTable dataList)
    {
        var list = from rows in dataList.AsEnumerable()
                   where rows.Field<string>("MerchantNumber").Trim() == this.MerchantNumber
                   select rows;
        if (list.Count<DataRow>() == 0)
            return new DataTable();
        return list.CopyToDataTable<DataRow>();
    }

    protected string buildUrlImage(string currentMerchantNumber)
    {

        string assignmentTypeQueryString =
        RiskSessionManager.DetectionQueue.AssignmentType != WebSiteEnums.AssignmentType.DetectionQueue
        ? "&AssignmentType=" + (int)RiskSessionManager.DetectionQueue.AssignmentType : "";

        string queryStringImage = this.Page.BuildSecureQueryString("MerchantNumber=" + currentMerchantNumber + "&ReportDate=" + RiskSessionManager.DetectionQueue.ReportDate + "&AssignmentID=" + RiskSessionManager.DetectionQueue.AssignmentID);
        return "rm_DQReasonModal.aspx?" + queryStringImage;

    }

    public void GetFlatReportInfo()
    {
        //_MerchantInfo = ToDataTable(_DQFlatReportInfo.Tables[0]);
        //_BarometerInfo = ToDataTable(_DQFlatReportInfo.Tables[1]);
        //_TransactionList = ToDataTable(_DQFlatReportInfo.Tables[2]);
        //_ChargebackList = ToDataTable(_DQFlatReportInfo.Tables[3]);
        //_DailyFCList = ToDataTable(_DQFlatReportInfo.Tables[4]);

        //Bind Merchant Information
        OnDataBindControls(DataBindAction.BindMerchantInfo);

        //Bind Barometer
        OnDataBindControls(DataBindAction.BindBarometer);

        //Bind Transactions
        OnDataBindControls(DataBindAction.BindTransaction);

        //Bind Chargeback
        OnDataBindControls(DataBindAction.BindChargeBack);

        //Bind Daily FC
        OnDataBindControls(DataBindAction.BindDailyFC);
    }

    private void HandleTable(DataTable dt)
    {
        if (dt == null) return;
        _CountKeyed = 0;
        _TransAmount = 0;
        foreach (DataRow dr in dt.Rows)
        {
            if (dr["Keyed"].ToString() == "Y") _CountKeyed++;
            _TransAmount += Convert.ToDecimal(dr["TransactionAmount"].ToString());
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

    #region Barometer
    protected void uxBarometerGrid_ItemDataBound(object sender, GridItemEventArgs e)
    {
        switch (e.Item.ItemType)
        {
            case GridItemType.AlternatingItem:
            case GridItemType.Item:
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    var rowItem = (e.Item.DataItem as DataRowView).Row;
                    string url = string.Empty;

                    //profile
                    var profile = dataItem["ProfileDescription"];
                    profile.ToolTip = VeraCodeSolution.DoVeraCode(rowItem["ProfileDescription"].ToString());
                    profile.Text = VeraCodeSolution.DoVeraCode(rowItem["ProfileDescription"].ToString());

                    //riskscore
                    var riskScore = dataItem["RiskScore"];
                    Color riskScoreColor = rowItem["RiskScoreColor"].ToString().ToColor();
                    if (!GeneralFuncsLib.NvlString(rowItem["RiskScore"]).Equals("0"))
                    {
                        if (!String.IsNullOrEmpty(GeneralFuncsLib.NvlString(rowItem["RiskScore"])))
                        {
                            string queryString = Page.BuildSecureQueryString("MerchantNumber=" + rowItem["MerchantNumber"] + "&ReportDate=" + ReportDate);
                            string urlRiskScoreDetail = "rm_RiskScoreDetailModal.aspx?" + queryString;
                            url = "<a class=\"link\" href='#' style=\"cursor:pointer\" onclick=\" return ShowPopupModal('" + urlRiskScoreDetail + "','auto'); return false;\">";

                            riskScore.Text = VeraCodeSolution.GetOutputHtmlString(url + dataItem["RiskScore"].Text + "</a>");
                        }
                        else
                        {
                            riskScore.Text = String.Empty;
                        }
                    }
                    else
                    {
                        riskScore.Text = VeraCodeSolution.DoVeraCode(
                            rowItem["RiskScore"].ToString().Trim() != string.Empty ?
                            dataItem["RiskScore"].Text : string.Empty
                            );
                    }
                    riskScore.Text = GeneralFuncsLib.FormatBorderText(riskScore.Text, riskScoreColor);

                    //volume percent
                    var volumePercent = dataItem["VolumePercent"];
                    volumePercent.ToolTip = string.Format("{0:C}", rowItem["TodayVolume"]).ToCurrencySymbol();
                    Color volPercentColor = rowItem["VolumeColor"].ToString().ToColor();
                    string rptDate = ReportDate.ToShortDateString();
                    rptDate = Convert.ToDateTime(rptDate).ToShortDateString();
                    if (!GeneralFuncsLib.NvlString(rowItem["VolumePercent"]).Equals("0") && GeneralFuncsLib.NvlString(rowItem["VolumePercent"]).Length > 0)
                    {
                        string queryString1 = Page.BuildSecureQueryString(string.Format("MerchantNumber={0}&ReportDate={1}",
                        rowItem["MerchantNumber"], rowItem["ReportDate"]));
                        string url1 = string.Format("<a class=\"link\" href=\"#\" style=\"cursor:pointer\" onclick=\"OpenInstanceWindow('rm_TransactionDetailsModal.aspx?{0}','DQWindow'); return false;\">", queryString1);

                        decimal VolumePercent = 0;
                        decimal.TryParse(GeneralFuncsLib.NvlString(rowItem["VolumePercent"]), out VolumePercent);

                        dataItem["VolumePercent"].Text = VeraCodeSolution.DoVeraCode(url1 + VolumePercent.ToString("#,#0") + "%" + "</a>");
                    }
                    else
                    {
                        volumePercent.Text = VeraCodeSolution.DoVeraCode(
                            rowItem["VolumePercent"].ToString().Trim() != string.Empty ?
                             (decimal.Parse(rowItem["VolumePercent"].ToString())).ToString("#,#0") + "%" : string.Empty
                             );
                    }
                    volumePercent.Text = GeneralFuncsLib.FormatBorderText(volumePercent.Text, volPercentColor);

                    // 40965
                    var contractualVolume = dataItem["ContractualVolume"];
                    if (rowItem["ContractualVolume"] != DBNull.Value)
                    {
                        contractualVolume.ToolTip = string.Format("{0:C}", rowItem["ContractualDailyVolume"]).ToCurrencySymbol(); 
                    }
                    else
                    {
                        contractualVolume.ToolTip = GetLocalResourceObject("ContractualValuenotBeenProvided_Resource").ToString();
                    }
                    Color contractualVolumeColor = rowItem["CVColor"].ToString().ToColor();
                    contractualVolume.Text = GeneralFuncsLib.FormatBorderText((rowItem["ContractualVolume"] != DBNull.Value ? string.Format("{0:N}%", decimal.Parse(rowItem["ContractualVolume"].ToString())) : WebSiteConstants.HTML_EM_DASH_ENCODE), contractualVolumeColor);


                    //average ticket
                    var avgTicket = dataItem["AverageTicketPercent"];
                    Color avgTicketColor = rowItem["AvgTktColor"].ToString().ToColor();
                    avgTicket.ToolTip = VeraCodeSolution.DoVeraCode(string.Format("{0:C}", rowItem["AverageTicketVolume"]).ToCurrencySymbol());
                    avgTicket.Text = VeraCodeSolution.DoVeraCode(
                        rowItem["AverageTicketPercent"].ToString() != string.Empty ?
                        (decimal.Parse(rowItem["AverageTicketPercent"].ToString())).ToString("#,#0") + "%" : string.Empty
                        );
                    avgTicket.Text = GeneralFuncsLib.FormatBorderText(avgTicket.Text, avgTicketColor);

                    //auth percent
                    var authPercent = dataItem["AuthorizationPercent"];
                    Color authPercentColor = rowItem["AuthColor"].ToString().ToColor();
                    authPercent.ToolTip = string.Format("{0:C} ({1})", rowItem["TodayAuthorizationVolume"], rowItem["TodayAuthorizationCount"]).ToCurrencySymbol();
                    if (!GeneralFuncsLib.NvlString(rowItem["AuthorizationPercent"]).Equals("0") && GeneralFuncsLib.NvlString(rowItem["AuthorizationPercent"]).Length > 0)
                    {
                        string queryString1 = this.Page.BuildSecureQueryString(string.Format("MerchantNumber={0}&ReportDate={1}",
                        rowItem["MerchantNumber"], rowItem["ReportDate"]));
                        string url1 = string.Format("<a class=\"link\" href=\"#\" style=\"cursor:pointer\" onclick=\"OpenInstanceWindow('rm_TransactionHistoryModal.aspx?{0}','DQWindow'); return false;\">", queryString1);
                        decimal authPerc = 0;
                        decimal.TryParse(GeneralFuncsLib.NvlString(rowItem["AuthorizationPercent"]), out authPerc);

                        dataItem["AuthorizationPercent"].Text = VeraCodeSolution.DoVeraCode(url1 + authPerc.ToString("#,#0") + "%" + "</a>");
                    }
                    else
                    {
                        authPercent.Text = VeraCodeSolution.DoVeraCode(rowItem["AuthorizationPercent"].ToString().Trim() != string.Empty ?
                       (decimal.Parse(rowItem["AuthorizationPercent"].ToString()).ToString("#,#0") + "%") : string.Empty);
                    }
                    authPercent.Text = GeneralFuncsLib.FormatBorderText(authPercent.Text, authPercentColor);

                    //decline percent
                    var decPercent = dataItem["DeclinedAuthorizationPercent"];
                    Color decPercentColor = rowItem["DeclAuthPctColor"].ToString().ToColor();
                    decPercent.ToolTip = VeraCodeSolution.DoVeraCode(string.Format("{0:C}", rowItem["TodayDeclinedAuthorizationVolume"]).ToCurrencySymbol());
                    decPercent.Text = VeraCodeSolution.DoVeraCode(
                        rowItem["DeclinedAuthorizationPercent"].ToString().Trim() != string.Empty ?
                        (decimal.Parse(rowItem["DeclinedAuthorizationPercent"].ToString())).ToString("#,#0.00") + "%" : string.Empty
                        );
                    decPercent.Text = GeneralFuncsLib.FormatBorderText(decPercent.Text, decPercentColor);

                    //# repeat auth
                    var rptAuth = dataItem["RepeatAuthorizationCount"];
                    Color rptAuthColor = rowItem["RptAuthColor"].ToString().ToColor();
                    rptAuth.Text = GeneralFuncsLib.FormatBorderText(rptAuth.Text, rptAuthColor);

                    //FC (foreign card)
                    var fc = dataItem["TodayForeignCardCount"];
                    Color fcColor = rowItem["FCColor"].ToString().ToColor();
                    fc.ToolTip = string.Format("{0:C}", rowItem["TodayForeignCardVolume"]).ToCurrencySymbol();
                    fc.Text = GeneralFuncsLib.FormatBorderText(fc.Text, fcColor);

                    //key percent
                    var keyPercent = dataItem["KeyPercent"];
                    Color keyPercentColor = rowItem["KeyColor"].ToString().ToColor();
                    keyPercent.ToolTip = VeraCodeSolution.DoVeraCode(string.Format("{0} ({1})", GeneralFuncsLib.FormatCurrencyTooltip(rowItem["TodayKeyVolume"], SessionManager.CurrencyFortmat), rowItem["TodayKeyCount"])).ToCurrencySymbol();
                    keyPercent.Text = VeraCodeSolution.DoVeraCode(
                        rowItem["KeyPercent"].ToString().Trim() != string.Empty ?
                        (decimal.Parse(rowItem["KeyPercent"].ToString())).ToString("#,#0") + "%" : string.Empty
                        );
                    keyPercent.Text = GeneralFuncsLib.FormatBorderText(keyPercent.Text, keyPercentColor);

                    //even percent
                    var evenPercent = dataItem["EvenDollarTransactionPercent"];
                    Color evenPercentColor = rowItem["EvenColor"].ToString().ToColor();
                    evenPercent.Text = VeraCodeSolution.DoVeraCode(
                        rowItem["EvenDollarTransactionPercent"].ToString() != string.Empty ?
                        (decimal.Parse(rowItem["EvenDollarTransactionPercent"].ToString())).ToString("#,#0") + "%" : string.Empty
                        );
                    evenPercent.Text = GeneralFuncsLib.FormatBorderText(evenPercent.Text, evenPercentColor, true);

                    //duplicate percent
                    var dupPercent = dataItem["DuplicateDollarTransactionPercent"];
                    Color dupPercentColor = rowItem["DupColor"].ToString().ToColor();
                    dupPercent.ToolTip = VeraCodeSolution.DoVeraCode(rowItem["DuplicateDollarTransactionCount"].ToString()).ToCurrencySymbol();
                    dupPercent.Text = VeraCodeSolution.DoVeraCode(
                        rowItem["DuplicateDollarTransactionPercent"].ToString() != string.Empty ?
                        (decimal.Parse(rowItem["DuplicateDollarTransactionPercent"].ToString())).ToString("#,#0") + "%" : string.Empty
                        );
                    dupPercent.Text = GeneralFuncsLib.FormatBorderText(dupPercent.Text, dupPercentColor);

                    //duplicate bin
                    var dupBin = dataItem["DuplicateBin"];
                    Color dupBinColor = rowItem["SixDupBinColor"].ToString().ToColor();
                    dupBin.ToolTip = VeraCodeSolution.DoVeraCode(string.Format("{0:C}", rowItem["DuplicateBin6TransactionAmount"]).ToCurrencySymbol());
                    dupBin.Text = GeneralFuncsLib.FormatBorderText(dupBin.Text, dupBinColor);

                    //SC
                    var sc = dataItem["SingleCardTransToday"];
                    Color scColor = rowItem["DupBinColor"].ToString().ToColor();
                    sc.ToolTip = VeraCodeSolution.DoVeraCode(string.Format("{0:C}", rowItem["VolumeOfSimilarCardTransaction"]).ToCurrencySymbol());
                    sc.Text = GeneralFuncsLib.FormatBorderText(sc.Text, scColor);

                    //# negative
                    var negative = dataItem["NegativeBatchCount"];
                    Color negColor = rowItem["NegColor"].ToString().ToColor();
                    negative.Text = GeneralFuncsLib.FormatBorderText(negative.Text, negColor);

                    //# zero
                    var zero = dataItem["ZeroBatchCount"];
                    Color zeroColor = rowItem["ZeroColor"].ToString().ToColor();
                    zero.Text = GeneralFuncsLib.FormatBorderText(zero.Text, zeroColor);

                    // TK25211 - VWEB - Add Transaction Volume as a column in Barometer Report
                    // TV
                    var todayVol = dataItem["TodayVolume"];
                    var todayVolColor = rowItem["VolumeColor"].ToString().ToColor();
                    todayVol.Text = GeneralFuncsLib.FormatBorderText(todayVol.Text, todayVolColor);

                    //RTVL
                    var rtvl = dataItem["TodayFirstTimeRetrievalVolume"];
                    Color rtvlColor = rowItem["RTVLColor"].ToString().ToColor();
                    rtvl.ToolTip = VeraCodeSolution.DoVeraCode(rowItem["TodayFirstTimeRetrievalCount"].ToString().ToCurrencySymbol());
                    rtvl.Text = VeraCodeSolution.DoVeraCode(rowItem["TodayFirstTimeRetrievalVolume"].ToString().Trim() != string.Empty ?
                        GeneralFuncsLib.FormatCurrency(rowItem["TodayFirstTimeRetrievalVolume"].ToString()) : string.Empty
                        );
                    rtvl.Text = GeneralFuncsLib.FormatBorderText(rtvl.Text, rtvlColor);

                    //CB
                    var cb = dataItem["TodayChargebackVolume"];
                    Color cbColor = rowItem["CBColor"].ToString().ToColor();
                    cb.ToolTip = VeraCodeSolution.DoVeraCode(rowItem["TodayChargebackCount"].ToString().ToCurrencySymbol());
                    cb.Text = VeraCodeSolution.DoVeraCode(rowItem["TodayChargebackVolume"].ToString().Trim() != string.Empty ?
                        GeneralFuncsLib.FormatCurrency(rowItem["TodayChargebackVolume"].ToString()) : string.Empty
                        );
                    cb.Text = GeneralFuncsLib.FormatBorderText(cb.Text, cbColor);

                    //Rtn Percent
                    var rtnPercent = dataItem["ReturnPercent"];
                    Color rtnPercentColor = rowItem["RtnColor"].ToString().ToColor();
                    rtnPercent.ToolTip = VeraCodeSolution.DoVeraCode(GeneralFuncsLib.FormatCurrencyTooltip(rowItem["TodayReturnAmount"], SessionManager.CurrencyFortmat)).ToCurrencySymbol();
                    rtnPercent.Text = VeraCodeSolution.DoVeraCode(rowItem["ReturnPercent"].ToString().Trim() != string.Empty ?
                        (decimal.Parse(rowItem["ReturnPercent"].ToString())).ToString("#,#0") + "%" : string.Empty);
                    rtnPercent.Text = GeneralFuncsLib.FormatBorderText(rtnPercent.Text, rtnPercentColor);

                    //Max ticket $
                    var maxTkt = dataItem["TodayHighestTransactionAmount"];
                    Color maxTktColor = rowItem["MaxTktColor"].ToString().ToColor();
                    maxTkt.Text = VeraCodeSolution.DoVeraCode(
                        rowItem["TodayHighestTransactionAmount"].ToString().Trim() != string.Empty ?
                        GeneralFuncsLib.FormatCurrency(rowItem["TodayHighestTransactionAmount"].ToString()) : string.Empty
                        );
                    maxTkt.Text = GeneralFuncsLib.FormatBorderText(maxTkt.Text, maxTktColor);

                    //# Tkts
                    var tkt = dataItem["TodayTransactionCount"];
                    Color tktColor = rowItem["TktsColor"].ToString().ToColor();
                    tkt.Text = GeneralFuncsLib.FormatBorderText(tkt.Text, tktColor, true);

                    //# batch
                    var batch = dataItem["TodayBatchCount"];
                    Color batchColor = rowItem["BatchColor"].ToString().ToColor();
                    batch.Text = GeneralFuncsLib.FormatBorderText(batch.Text, batchColor, true);

                    //SIC
                    var sic = dataItem["SIC"];
                    sic.ToolTip = rowItem["SICDescription"].ToString().Replace("&nbsp;", string.Empty);
                }
                break;
        }
    }
    #endregion

    #region Transactions
    protected void uxReportGridTransactions_ItemDataBound(object sender, GridItemEventArgs e)
    {
        switch (e.Item.ItemType)
        {
            case GridItemType.AlternatingItem:
            case GridItemType.Item:
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    DataRowView rowView = e.Item.DataItem as DataRowView;
                    var rowItem = (e.Item.DataItem as DataRowView).Row;
                    dataItem["CardType"].ToolTip = VeraCodeSolution.DoVeraCode(rowView["CardDescription"].ToString());
                    dataItem["Keyed"].ToolTip = VeraCodeSolution.DoVeraCode(rowView["EntryModeDescription"].ToString());
                    dataItem["TransactionType"].ToolTip = VeraCodeSolution.DoVeraCode(rowView["TransactionType"].ToString());

                    var dupe = dataItem["DupeCount"];
                    var dupeToolTip = VeraCodeSolution.DoVeraCode(string.Format("{0:C}", rowItem["DupeAmount"]));
                    dupe.ToolTip = dupeToolTip;

                    if (!String.IsNullOrEmpty(rowView["AccountNumber"].ToString()))
                    {
                        string fullcard = rowView["AccountNumber"].ToString();

                        string queryString = Page.BuildSecureQueryString("cn=" + rowView["PartialAccountNumber"] + "&cnf=" + fullcard + "&merch=" + rowView["MerchantNumber"].ToString() + "&isRisk=1");
                        string urlCardDetail = ResolveUrl("~/") + "CardHistoryModal.aspx?" + queryString;
                        string urlCard = "<a class=\"link\" href=\"javascript:void(0)\" onclick=\"openPopupWindow('" + urlCardDetail + "','CardHistoryWindow'); return false;\">";

                        if (CheckCSViewFullCard())
                        {
                            uxReportGridTransactions.Columns.FindByUniqueName("AccountNumber").Visible = true;
                            uxReportGridTransactions.Columns.FindByUniqueName("PartialAccountNumber").Visible = false;
                            dataItem["AccountNumber"].Text = VeraCodeSolution.DoVeraCode(urlCard + fullcard + "</a>");
                        }
                        else
                        {
                            uxReportGridTransactions.Columns.FindByUniqueName("AccountNumber").Visible = false;
                            uxReportGridTransactions.Columns.FindByUniqueName("PartialAccountNumber").Visible = true;
                            if (GeneralFuncsLib.HasIPForFullCard((SecurePage)this.Page))
                            {
                                dataItem["PartialAccountNumber"].Text = VeraCodeSolution.DoVeraCode(
                                    String.Format(IMAGE, (index + 1), BuildUrlForFullCard("Chargebacks", rowView["RecordId"].ToString(), rowView["IssuingBank"].ToString(), rowView["ReportDate"].ToString())) + VeraCodeSolution.DoVeraCode(urlCard + rowView["PartialAccountNumber"].ToString() + "</a>")
                                    );
                            }
                            else
                            {
                                dataItem["PartialAccountNumber"].Text = VeraCodeSolution.DoVeraCode(urlCard + rowView["PartialAccountNumber"].ToString() + "</a>");
                            }
                        }
                    }

                    //handle for Auth
                    if (!String.IsNullOrEmpty(rowView["AuthorizationNumber"].ToString()))
                    {
                        dataItem["AuthorizationNumber"].Text = VeraCodeSolution.DoVeraCode(
                            GeneralFuncsLib.BuildRskAuthUrlPopupModalChild(
                            (SecurePage)Page, rowView["AuthorizationNumber"],
                            rowView["MerchantNumber"].ToString(), string.Empty,
                            rowView["TransactionDate"], index + 1,
                            rowView["AuthorizationNumber"].ToString(), false)
                            );
                    }

                    var transactionTime = dataItem["TransactionTime"];
                    transactionTime.Text = VeraCodeSolution.DoVeraCode(string.IsNullOrEmpty(rowView["TransactionTime"].ToString()) ? string.Empty : Convert.ToDateTime(rowView["TransactionTime"].ToString()).ToLongTimeString());

                    var transactionAmount = dataItem["TransactionAmount"];
                    transactionAmount.Text = VeraCodeSolution.DoVeraCode(AS.Common.Formater.FormatData.FormatCurrency(Convert.ToDecimal(rowView["TransactionAmount"].ToString()), SessionManager.CurrencyFortmat));

                    if (GeneralFuncsLib.NvlString(rowView["HighestTransactionAmountFlag"].ToString().ToLower()).Equals("true"))
                    {
                        transactionAmount.Text = GeneralFuncsLib.FormatBorderText(transactionAmount.Text, Color.Green);
                        transactionAmount.ToolTip = VeraCodeSolution.DoVeraCode(this.GetLocalResourceObject("DQFlatReportInfoCS_Text_HighestTransactionAmount").ToString());
                    }

                    if (GeneralFuncsLib.NvlString(rowView["DuplicateFlag"].ToString().ToLower()).Equals("true"))
                    {
                        foreach (ASGridBoundColumn col in uxReportGridTransactions.Columns)
                        {
                            dupe.ToolTip = dupeToolTip;
                            var ctr = dataItem[col.UniqueName];
                            ctr.BackColor = Color.Yellow;
                            dataItem[col.UniqueName].ToolTip = VeraCodeSolution.DoVeraCode(this.GetLocalResourceObject("DQFlatReportInfoCS_Text_DuplicateAccountNumber").ToString());
                            if (GeneralFuncsLib.NvlString(rowView["HighestTransactionAmountFlag"].ToString().ToLower()).Equals("true"))
                            {
                                transactionAmount.ToolTip = VeraCodeSolution.DoVeraCode(
                                    this.GetLocalResourceObject("DQFlatReportInfoCS_Text_DuplicateAccountNumber").ToString() + ";" +
                                    this.GetLocalResourceObject("DQFlatReportInfoCS_Text_HighestTransactionAmount"));
                            }
                        }
                    }

                    //42895 – VW – IPMT - Add hover over for Ctry column in Next Queue and Security Report
                    var countryCode = dataItem["CountryCode"];
                    countryCode.ToolTip = VeraCodeSolution.DoVeraCode(rowView["CountryName"].ToSafeString());

                    // 46807
                    dataItem[MATCHED_CODE].ToolTip = VeraCodeSolution.DoVeraCode(rowView[MATCHED_NAME].ToString());

                    string matchedCode = rowView[MATCHED_CODE].ToString();

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
                break;
            case GridItemType.Footer:
                {
                    GridFooterItem footerItem = e.Item as GridFooterItem;
                    var isShowOnAccountNumber = uxReportGridTransactions.Columns.FindByUniqueName("AccountNumber").Visible;
                    if (CheckCSViewFullCard() && isShowOnAccountNumber)
                    {
                        footerItem["AccountNumber"].Text = "&nbsp;" + this.GetLocalResourceObject("DQFlatReportInfoCS_Text_Total").ToString() + ": " + _TransactionList.Rows.Count;
                        footerItem["AccountNumber"].Font.Bold = true;
                    }
                    else
                    {
                        footerItem["PartialAccountNumber"].Text = "&nbsp;" + this.GetLocalResourceObject("DQFlatReportInfoCS_Text_Total").ToString() + ": " + _TransactionList.Rows.Count;
                        footerItem["PartialAccountNumber"].Font.Bold = true;
                    }
                    footerItem["Keyed"].Text = _CountKeyed.ToString();
                    footerItem["Keyed"].Font.Bold = true;
                    footerItem["TransactionAmount"].Text = AS.Common.Formater.FormatData.FormatCurrency(_TransAmount, SessionManager.CurrencyFortmat);// +"&nbsp;&nbsp;";
                    footerItem["TransactionAmount"].Font.Bold = true;
                }
                break;
        }
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
        foreach (GridDataItem item in uxReportGridTransactions.MasterTableView.Items)
        {
            //Format Dulicate 
            if (item["DuplicateFlag"].Text.Contains("True"))
            {
                uxReportGridTransactions.MasterTableView.Items[item.ItemIndex].Attributes.Add("style", "background-color:Yellow !important");
                uxReportGridTransactions.MasterTableView.Items[item.ItemIndex].ToolTip = this.GetLocalResourceObject("DQFlatReportInfoCS_Text_DuplicateAccountNumber").ToString();
            }
            
            //Format Trans Amt
            if (item["HighestTransactionAmountFlag"].Text.Contains("True"))
            {
                var transactionAmount = item["TransactionAmount"];
                transactionAmount.Text = GeneralFuncsLib.FormatBorderText(transactionAmount.Text, Color.Green);
                if (item["DuplicateFlag"].Text.Contains("True"))
                    item["TransactionAmount"].ToolTip = this.GetLocalResourceObject("DQFlatReportInfoCS_Text_DuplicateAccountNumber").ToString() + "; " +
                        this.GetLocalResourceObject("DQFlatReportInfoCS_Text_MaximumTicketAmount").ToString();
                else
                    item["TransactionAmount"].ToolTip = this.GetLocalResourceObject("DQFlatReportInfoCS_Text_MaximumTicketAmount").ToString();
            }

        }

        //uxReportGridTransactions.ClientSettings.Scrolling.AllowScroll = uxReportGridTransactions.Items.Count > 0;
        if (uxReportGridTransactions.Items.Count == 0)
            uxReportGridTransactions.ShowFooter = false;
        else
            uxReportGridTransactions.ShowFooter = true;
    }

    protected void uxReportGridTransactions_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        string whichGrid = ((ASGrid)sender).ClientID.Replace("_" + this.uxReportGridTransactions.ID, string.Empty);
        this.uxReportGridTransactions.DataSource = (DataTable)Session[whichGrid];
        HandleTable((DataTable)Session[whichGrid]);
    }

    #endregion

    private string BuildUrlForFullCard(string reportType, string encryptedCC, string issueBank, string ReportDate)
    {
        string queryString = Page.BuildSecureQueryString("rt=" + reportType + "&cn=" + Server.UrlEncode(encryptedCC) + "&issue=" + issueBank + "&reportdate=" + ReportDate + "&idx=" + (index + 1));
        queryString = ResolveUrl("~/") + "FullCC.aspx?" + queryString;
        return queryString;
    }

    #region ChargeBacks
    protected void uxChargeback_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridHeaderItem)
        {
            _CountKeyed = 0;
            _TransAmount = 0;
        }
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView rowView = e.Item.DataItem as DataRowView;
            string fullcard = rowView["AccountNumber"].ToString();
            string queryString = Page.BuildSecureQueryString("cn=" + rowView["PartialAccountNumber"] + "&cnf=" + fullcard + "&merch=" + rowView["MerchantNumber"].ToString() + "&isRisk=1");
            string urlCardDetail = ResolveUrl("~/") + "CardHistoryModal.aspx?" + queryString;
            string urlCard = "<a class=\"link\" href=\"javascript:void(0)\" onclick=\"openPopupWindow('" + urlCardDetail + "','CardHistoryWindow'); return false;\">";

            if (!String.IsNullOrEmpty(fullcard))
            {
                if (CheckCSViewFullCard())
                {
                    uxChargeback.Columns.FindByUniqueName("AccountNumber").Visible = true;
                    uxChargeback.Columns.FindByUniqueName("PartialAccountNumber").Visible = false;
                    dataItem["AccountNumber"].Text = VeraCodeSolution.DoVeraCode(urlCard + fullcard + "</a>");
                }
                else
                {
                    uxChargeback.Columns.FindByUniqueName("AccountNumber").Visible = false;
                    uxChargeback.Columns.FindByUniqueName("PartialAccountNumber").Visible = true;
                    if (GeneralFuncsLib.HasIPForFullCard((SecurePage)this.Page))
                    {
                        dataItem["PartialAccountNumber"].Text = VeraCodeSolution.DoVeraCode(
                            String.Format(IMAGE, (index + 1), BuildUrlForFullCard("Chargebacks", rowView["RecordId"].ToString(), rowView["IssuingBank"].ToString(), rowView["ReportDate"].ToString())) + VeraCodeSolution.DoVeraCode(urlCard + rowView["PartialAccountNumber"].ToString() + "</a>")
                            );
                    }
                    else
                    {
                        dataItem["PartialAccountNumber"].Text = VeraCodeSolution.DoVeraCode(urlCard + rowView["PartialAccountNumber"].ToString() + "</a>");
                    }
                }
            }

            //  42867 - Chargeback section of the Next Queue 'Key' always N

            if (rowView["Keyed"].IsNullOrEmpty())
            {
                dataItem["Keyed"].Text = WebSiteConstants.HTML_EM_DASH;
            }

            if (dataItem["Keyed"].Text == "Y") _CountKeyed++;
            _TransAmount += Convert.ToDecimal(rowView["TransactionAmount"].ToString());
            dataItem["TransactionAmount"].Text = VeraCodeSolution.DoVeraCode(
                AS.Common.Formater.FormatData.FormatCurrency(Convert.ToDecimal(rowView["TransactionAmount"].ToString()), SessionManager.CurrencyFortmat)
                );
        }
        if (e.Item is GridFooterItem)
        {
            //[42596] - Bug #35948: [STAG Environment]: Missing Total
            GridFooterItem footerItem = e.Item as GridFooterItem;
            var isShowOnAccountNumber = uxChargeback.Columns.FindByUniqueName("AccountNumber").Visible;
            string total = "&nbsp;" + this.GetLocalResourceObject("DQFlatReportInfoCS_Text_Total").ToString() + ": " + _ChargebackList.Rows.Count;
            if (CheckCSViewFullCard() && isShowOnAccountNumber)
            {
                footerItem["AccountNumber"].Text = total;
                footerItem["AccountNumber"].Font.Bold = true;
            }
            else
            {
                footerItem["PartialAccountNumber"].Text = total;
                footerItem["PartialAccountNumber"].Font.Bold = true;
            }

            footerItem["Keyed"].Text = _CountKeyed.ToString();
            footerItem["Keyed"].Font.Bold = true;
            footerItem["TransactionAmount"].Text = AS.Common.Formater.FormatData.FormatCurrency(_TransAmount, SessionManager.CurrencyFortmat);// +"&nbsp;&nbsp;";
            footerItem["TransactionAmount"].Font.Bold = true;
        }
    }

    protected void uxChargeback_PreRender(object sender, EventArgs e)
    {
        //uxChargeback.ClientSettings.Scrolling.AllowScroll = uxChargeback.Items.Count > ScrollRowCount;
        if (uxChargeback.Items.Count == 0)
        {
            uxChargeback.ShowFooter = false;
        }
        else uxChargeback.ShowFooter = true;
    }
    #endregion


    protected void uxMerchantInfo_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            //46652 - AW - Multi-Currency Transaction Display
            var control = e.Item.FindControl("lituxMerchantInfoHeaderAuth") as Literal;
            control.Text = control.Text.ToCurrencySymbol();

            DataRowView rowItem = e.Item.DataItem as DataRowView;
            //checkbox column
            Control ctrl = e.Item.FindControl("chkMerchantWorked");
            string checkboxId = string.Empty;
            if (ctrl != null)
            {
                HtmlInputCheckBox chkBox = ctrl as HtmlInputCheckBox;
                chkBox.Checked = rowItem["Worked"].Equals(true);
                chkBox.Value = rowItem["MerchantNumber"].ToString();
                chkBox.Attributes.Add("onclick", string.Format("ChangeMerchantWorked(this,{0})", rowItem["TodayVolume"]));
                checkboxId = chkBox.ClientID;
            }

            string url = String.Empty;
            if (SessionManager.CurrentUserPermissions.Contains(WebSiteConstants.VIEW_RISK_REPORT))
            {
                url = url + String.Format("<a href='#' onclick=\"MerchantNumberClick('{0}','{1}',this); $('#{2}').prop('checked',true); return false;\">",
                    rowItem["MerchantNumber"], rowItem["TodayVolume"], checkboxId) + rowItem["MerchantNumber"] + "</a>";
            }
            else
            {
                url = rowItem["MerchantNumber"].ToString();
            }

            ((Literal)e.Item.FindControl("txtMerchantNumber")).Text = VeraCodeSolution.DoVeraCode(url);
            Label lblProfile = e.Item.FindControl("lblProfile") as Label;
            lblProfile.Text = VeraCodeSolution.DoVeraCode(rowItem["ProfileDescription"].ToString());
            if (rowItem["Profile"].ToString() != "0")
                lblProfile.ToolTip = VeraCodeSolution.DoVeraCode(rowItem["Profile"] + " - " + rowItem["ProfileDescription"]);

            var colRQColumn = e.Item.FindControl("colRQColumn") as HtmlGenericControl;
            if (colRQColumn != null)
            {
                colRQColumn.Visible = HasRQColumn;
                if (HasRQColumn)
                {
                    HtmlInputCheckBox chkRequeueSingleMerchant = colRQColumn.FindControl("chkItemCV") as HtmlInputCheckBox;
                    if (chkRequeueSingleMerchant != null)
                    {
                        chkRequeueSingleMerchant.Value = rowItem["MerchantNumber"].ToString();
                        chkRequeueSingleMerchant.Attributes.Add("onclick", string.Format("RequeueSingleMerchant(this, true)"));
                        chkRequeueSingleMerchant.Disabled = !GeneralFuncsLib.ReadOnlyRiskManagementEnableStatus((SecurePage)Page);
                        chkRequeueSingleMerchant.Visible = true;

                        if (RiskSessionManager.DetectionQueueTemporaryRequeueInfo.RequeuedMerchantList != null)
                        {
                            chkRequeueSingleMerchant.Checked = RiskSessionManager.DetectionQueueTemporaryRequeueInfo.RequeuedMerchantList.Contains(rowItem["MerchantNumber"].ToString());
                        }

                        if (RiskSessionManager.DetectionQueueTemporaryRequeueInfo.IsRequeueAll)
                        {
                            chkRequeueSingleMerchant.Checked = true;
                            chkRequeueSingleMerchant.Disabled = true;
                        }
                    }
                }
            }

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
            sender.Text = "(" + AS.Common.Formater.FormatData.FormatCurrency(temp, SessionManager.CurrencyFortmat) + ")";
        }
        else
        {
            sender.Text = AS.Common.Formater.FormatData.FormatCurrency(temp, SessionManager.CurrencyFortmat);
        }
    }
}
