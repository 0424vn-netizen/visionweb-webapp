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
using AS.Common.Formater;

public partial class UserControls_SecurityReportControl : GlobalUserControl
{
    #region Enums
    enum DataBindAction
    {
        BindStatisticsReport,
        BindBarometer,
        BindTransaction,
        BindChargeBack
    }
    enum PostBackAction
    {
        SelectedAccountNumber
    }
    const string IMAGE = "<a class=\"atab\"href=\"#\" style=\"cursor:pointer\" onclick=\" return ShowPopupModalChild({0},'{1}','auto');\"><img src='../res/images/information.gif' border=\"0\"/></a>&nbsp; &nbsp;";
    #endregion

    #region Properties
    private int index = 0;
    private int _TransCountKeyed = 0;
    private decimal _TransAmount = 0;

    private int _ChargebackCountKeyed = 0;
    private decimal _ChargebackAmount = 0;

    private int _DailyFcCountKeyed = 0;
    private decimal _DailyFcAmount = 0;

    public DateTime ReportDate
    {
        get
        {
            if (ViewState["ReportDate"] == null)
                return DateTime.Now;
            else
                return (DateTime)ViewState["ReportDate"];
        }
        set
        {
            ViewState["ReportDate"] = value;
        }
    }

    public string MerchantNumber
    {
        get
        {
            if (ViewState["MerchantNumber"] == null)
                return string.Empty;
            else
                return ViewState["MerchantNumber"].ToASString();
        }
        set
        {
            ViewState["MerchantNumber"] = value;
        }
    }

    #endregion

    #region Const
    private const string MATCHED_CODE = "MatchCode";
    private const string MATCHED_NAME = "MatchName";
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsIntruderDetected) return;
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        if (Page.IsIntruderDetected) return;

        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.AddLanguageID();
        parameters.Add(new FilterParameter("@ReportDate", this.ReportDate, DbType.DateTime));

        switch ((DataBindAction)type)
        {
            case DataBindAction.BindStatisticsReport:
                {
                    parameters.Add(new FilterParameter("@MerchantNumber", this.MerchantNumber, DbType.String));

                    uxStatisticsReport.DataSource = WebServices.RiskServices.GetReports("spa_rm_cs_SecurityReport_GetStatisticsReport", parameters);
                    uxStatisticsReport.DataBind();
                }
                break;
            case DataBindAction.BindBarometer:
                {
                    parameters.Add(new FilterParameter("@MerchantNumber", this.MerchantNumber, DbType.String));
                    uxBarometerGrid.DataSource = WebServices.RiskServices.GetReports("spa_rm_SecurityReport_GetBarometerReport", parameters);
                    uxBarometerGrid.DataBind();
                }
                break;
            case DataBindAction.BindTransaction:
                {
                    parameters.Add(new FilterParameter("@MerchantList", this.MerchantNumber, DbType.String));

                    DataTable transactionList = WebServices.RiskServices.GetReports("spa_rm_GetFlatReportTransactionDetail_MultiMerchant", parameters);
                    CalculateTransactionTotal(transactionList);
                    if (IsExport && transactionList != null && transactionList.Rows.Count > 0)
                    {
                        DataRow dr = transactionList.NewRow();
                        dr["PartialAccountNumber"] = "Total: " + transactionList.Rows.Count;
                        dr["Keyed"] = _TransCountKeyed;
                        dr["TransactionAmount"] = _TransAmount;
                        transactionList.Rows.Add(dr);
                    }
                    uxReportGridTransactions.DataSource = transactionList;
                    uxReportGridTransactions.DataBind();
                }
                break;
            case DataBindAction.BindChargeBack:
                {
                    parameters.Add(new FilterParameter("@MerchantList", this.MerchantNumber, DbType.String));
                    DataTable chargeback = WebServices.RiskServices.GetReports("spa_rm_GetFlatReportChargebackDetail_MultiMerchant", parameters);
                    CalculateChargebackTotal(chargeback);
                    if (IsExport && chargeback != null && chargeback.Rows.Count > 0)
                    {
                        DataRow dr = chargeback.NewRow();
                        dr["PartialAccountNumber"] = "Total: " + chargeback.Rows.Count;
                        dr["Keyed"] = _ChargebackCountKeyed;
                        dr["TransactionAmount"] = _ChargebackAmount;
                        chargeback.Rows.Add(dr);
                    }
                    uxChargeback.DataSource = chargeback;
                    uxChargeback.DataBind();
                }
                break;
            default: break;
        }
    }
    protected void uxReportGridTransactions_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.BindTransaction);
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
        OnDataBindControls(DataBindAction.BindStatisticsReport);
        OnDataBindControls(DataBindAction.BindBarometer);
        OnDataBindControls(DataBindAction.BindTransaction);
        OnDataBindControls(DataBindAction.BindChargeBack);
        phlStatisticsReport.Visible = uxPlcBarometer.Visible = uxPanelChargeback.Visible = uxPanelTransaction.Visible = true;
    }

    private void CalculateTransactionTotal(DataTable dt)
    {
        _TransCountKeyed = 0;
        _TransAmount = 0;
        if (dt == null) return;
        foreach (DataRow dr in dt.Rows)
        {
            if (dr["Keyed"].ToString() == "Y") _TransCountKeyed++;
            _TransAmount += Convert.ToDecimal(dr["TransactionAmount"].ToString());
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
                    profile.Text = VeraCodeSolution.ValidateResponseData(rowItem["ProfileDescription"].ToString());

                    //riskscore
                    var riskScore = dataItem["RiskScore"];
                    if (!GeneralFuncsLib.NvlString(rowItem["RiskScore"]).Equals("0"))
                    {
                        string queryString = Page.BuildSecureQueryString("MerchantNumber=" + rowItem["MerchantNumber"] + "&ReportDate=" + ReportDate);
                        string urlRiskScoreDetail = "rm_RiskScoreDetailModal.aspx?" + queryString;
                        url = "<a class=\"link\" href='#' style=\"cursor:pointer\" onclick=\" return ShowPopupModal('" + urlRiskScoreDetail + "'); return false;\">";

                        riskScore.Text = VeraCodeSolution.GetOutputHtmlString(url + dataItem["RiskScore"].Text + "</a>");
                    }
                    else
                    {
                        riskScore.Text = VeraCodeSolution.DoVeraCode(rowItem["RiskScore"].ToString().Trim() != string.Empty ?
                            dataItem["RiskScore"].Text : string.Empty);
                    }

                    //volume percent
                    var volumePercent = dataItem["VolumePercent"];
                    volumePercent.ToolTip = string.Format("{0:C}", rowItem["TodayVolume"]);

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
                        volumePercent.Text = VeraCodeSolution.DoVeraCode(rowItem["VolumePercent"].ToString().Trim() != string.Empty ?
                             (decimal.Parse(rowItem["VolumePercent"].ToString())).ToString("#,#0") + "%" : string.Empty);
                    }

                    //average ticket
                    var avgTicket = dataItem["AverageTicketPercent"];
                    avgTicket.ToolTip = string.Format("{0:C}", rowItem["AverageTicketVolume"]);
                    avgTicket.Text = VeraCodeSolution.DoVeraCode(rowItem["AverageTicketPercent"].ToString() != string.Empty ?
                        (decimal.Parse(rowItem["AverageTicketPercent"].ToString())).ToString("#,#0") + "%" : string.Empty);

                    //auth percent
                    var authPercent = dataItem["AuthorizationPercent"];
                    authPercent.ToolTip = string.Format("{0:C} ({1})", rowItem["TodayAuthorizationVolume"], rowItem["TodayAuthorizationCount"]);
                    if (!GeneralFuncsLib.NvlString(rowItem["AuthorizationPercent"]).Equals("0") && GeneralFuncsLib.NvlString(rowItem["AuthorizationPercent"]).Length > 0)
                    {
                        string queryString1 = Page.BuildSecureQueryString(string.Format("MerchantNumber={0}&ReportDate={1}",
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
                    //decline percent
                    var decPercent = dataItem["DeclinedAuthorizationPercent"];
                    decPercent.ToolTip = string.Format("{0:C}", rowItem["TodayDeclinedAuthorizationVolume"]);
                    decPercent.Text = VeraCodeSolution.DoVeraCode(rowItem["DeclinedAuthorizationPercent"].ToString().Trim() != string.Empty ?
                        (decimal.Parse(rowItem["DeclinedAuthorizationPercent"].ToString())).ToString("#,#0") + "%" : string.Empty);

                    //FC (foreign card)
                    var fc = dataItem["TodayForeignCardCount"];
                    fc.ToolTip = string.Format("{0:C}", rowItem["TodayForeignCardVolume"]);

                    //key percent
                    var keyPercent = dataItem["KeyPercent"];
                    keyPercent.ToolTip = string.Format("{0} ({1})", FormatData.FormatCurrency(rowItem["TodayKeyVolume"], SessionManager.CurrencyFortmat), rowItem["TodayKeyCount"]);
                    keyPercent.Text = VeraCodeSolution.DoVeraCode(rowItem["KeyPercent"].ToString().Trim() != string.Empty ?
                        (decimal.Parse(rowItem["KeyPercent"].ToString())).ToString("#,#0") + "%" : string.Empty);

                    //even percent
                    var evenPercent = dataItem["EvenDollarTransactionPercent"];
                    evenPercent.Text = VeraCodeSolution.DoVeraCode(rowItem["EvenDollarTransactionPercent"].ToString() != string.Empty ?
                        (decimal.Parse(rowItem["EvenDollarTransactionPercent"].ToString())).ToString("#,#0") + "%" : string.Empty);

                    //duplicate percent
                    var dupPercent = dataItem["DuplicateDollarTransactionPercent"];
                    dupPercent.ToolTip = rowItem["DuplicateDollarTransactionCount"].ToString();
                    dupPercent.Text = VeraCodeSolution.DoVeraCode(rowItem["DuplicateDollarTransactionPercent"].ToString() != string.Empty ?
                        (decimal.Parse(rowItem["DuplicateDollarTransactionPercent"].ToString())).ToString("#,#0") + "%" : string.Empty);

                    //duplicate bin
                    var dupBin = dataItem["DuplicateBin"];
                    dupBin.ToolTip = VeraCodeSolution.DoVeraCode(string.Format("{0:C}", rowItem["DuplicateBin6TransactionAmount"]));

                    //SC
                    var sc = dataItem["SingleCardTransToday"];
                    sc.ToolTip = VeraCodeSolution.DoVeraCode(string.Format("{0:C}", rowItem["VolumeOfSimilarCardTransaction"]));
                    //RTVL
                    var rtvl = dataItem["TodayFirstTimeRetrievalVolume"];
                    rtvl.ToolTip = VeraCodeSolution.DoVeraCode(rowItem["TodayFirstTimeRetrievalCount"].ToString());
                    rtvl.Text = VeraCodeSolution.DoVeraCode(
                        rowItem["TodayFirstTimeRetrievalVolume"].ToString().Trim() != string.Empty ?
                        GeneralFuncsLib.FormatCurrency(rowItem["TodayFirstTimeRetrievalVolume"].ToString()) : string.Empty
                        );

                    //CB
                    var cb = dataItem["TodayChargebackVolume"];
                    cb.ToolTip = rowItem["TodayChargebackCount"].ToString();
                    cb.Text = VeraCodeSolution.DoVeraCode(rowItem["TodayChargebackVolume"].ToString().Trim() != string.Empty ?
                        GeneralFuncsLib.FormatCurrency(rowItem["TodayChargebackVolume"].ToString()) : string.Empty);

                    //Rtn Percent
                    var rtnPercent = dataItem["ReturnPercent"];
                    rtnPercent.ToolTip = VeraCodeSolution.DoVeraCode(FormatData.FormatCurrency(rowItem["TodayReturnAmount"], SessionManager.CurrencyFortmat));
                    rtnPercent.Text = VeraCodeSolution.DoVeraCode(rowItem["ReturnPercent"].ToString().Trim() != string.Empty ?
                        (decimal.Parse(rowItem["ReturnPercent"].ToString())).ToString("#,#0") + "%" : string.Empty
                        );

                    //Max ticket $
                    var maxTkt = dataItem["TodayHighestTransactionAmount"];
                    maxTkt.Text = VeraCodeSolution.DoVeraCode(rowItem["TodayHighestTransactionAmount"].ToString().Trim() != string.Empty ?
                        GeneralFuncsLib.FormatCurrency(rowItem["TodayHighestTransactionAmount"].ToString()) : string.Empty);
                    //SIC
                    var sic = dataItem["SIC"];
                    sic.ToolTip = rowItem["SICDescription"].ToString().Replace("&nbsp;", string.Empty);
                    //*/
                }
                break;
        }
    }
    #endregion



    #region Transactions


    public string BuildBlueDot(string recordId, string reportType, string reportDate, int index)
    {
        string queryString = Page.BuildSecureQueryString(
            string.Format("rt={0}&cn={1}&reportdate={2}&idx={3}",
                          reportType,
                          Page.Server.UrlEncode(recordId),
                          reportDate,
                          index));

        string urlFullCardDetail = string.Format(Page.ResolveUrl("~") + "FullCC.aspx?{0}", queryString);
        string img = string.Format("<img src='{0}res/img/information.png' border=\"0\"/>", Page.ResolveUrl("~"));
        return string.Format(
                "<a href=\"#\" style=\"cursor:pointer\" onclick=\" return parent.ShowPopupModalChild({0}, '{1}','auto');\">{2}</a>&nbsp; &nbsp;",
                index, urlFullCardDetail, img);
    }

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

                    string acctNumberText = e.Item.ItemIndex.ToString() + ";NQTransactionDetail";
                    string urlCard = "<a class=\"link\" href=\"javascript:void(0)\" onclick=\"ShowPopupAcctNumber('" + acctNumberText + "');\">";

                    dataItem["PartialAccountNumber"].Text = (GeneralFuncsLib.CheckCSViewFullCard((SecurePage)this.Page))
                   ? VeraCodeSolution.DoVeraCode(urlCard + rowView["PartialAccountNumber"].ToString() + "</a>&nbsp; &nbsp;")
                   : VeraCodeSolution.DoVeraCode(urlCard + rowView["PartialAccountNumber"].ToString() + "</a>");

                    //dupe column 
                    var dupe = dataItem["DupeCount"];
                    var dupeToolTip = VeraCodeSolution.DoVeraCode(string.Format("{0:C}", rowItem["DupeAmount"]));
                    dupe.ToolTip = dupeToolTip;

                    //handle for Auth
                    if (IsExport)
                    {
                        dataItem["AuthorizationNumber"].Text = VeraCodeSolution.DoVeraCode(rowView["AuthorizationNumber"].ToSafeString());
                    }
                    else if (!String.IsNullOrEmpty(rowView["AuthorizationNumber"].ToString()))
                    {
                        dataItem["AuthorizationNumber"].Text = VeraCodeSolution.DoVeraCode(
                            GeneralFuncsLib.BuildRskAuthUrlPopupModalChild(
                            (SecurePage)Page, rowView["AuthorizationNumber"],
                            rowView["MerchantNumber"].ToString(), string.Empty,
                            rowView["TransactionDate"], index + 1,
                            rowView["AuthorizationNumber"].ToString(), false
                            ));
                    }
                    var transactionAmount = dataItem["TransactionAmount"];
                    transactionAmount.Text = VeraCodeSolution.DoVeraCode(AS.Common.Formater.FormatData.FormatCurrency(Convert.ToDecimal(rowView["TransactionAmount"].ToString()), SessionManager.CurrencyFortmat));
                    dataItem["TransactionTime"].Text = VeraCodeSolution.DoVeraCode(
                        string.IsNullOrEmpty(rowView["TransactionTime"].ToString()) ? string.Empty : Convert.ToDateTime(rowView["TransactionTime"].ToString()).ToLongTimeString()
                        );

                    if (GeneralFuncsLib.NvlString(rowView["HighestTransactionAmountFlag"].ToString().ToLower()).Equals("true"))
                    {                       
                        transactionAmount.Text = GeneralFuncsLib.FormatBorderText(transactionAmount.Text, Color.Green);
                        dataItem["TransactionAmount"].ToolTip = VeraCodeSolution.DoVeraCode(this.GetLocalResourceObject("DQNextQReportCS_Text_HighestTransactionAmount").ToString());
                    }
                    if (GeneralFuncsLib.NvlString(rowView["DuplicateFlag"].ToString().ToLower()).Equals("true"))
                    {
                        foreach (ASGridBoundColumn col in uxReportGridTransactions.Columns)
                        {                           
                            dupe.ToolTip = dupeToolTip;
                            var uniqueName = dataItem[col.UniqueName];
                            uniqueName.BackColor = Color.Yellow;
                            uniqueName.ToolTip = VeraCodeSolution.DoVeraCode(GetLocalResourceObject("DQNextQReportCS_Text_DuplicateAccountNumber").ToString());
                            if (GeneralFuncsLib.NvlString(rowView["HighestTransactionAmountFlag"].ToString().ToLower()).Equals("true"))
                            {
                                transactionAmount.Text = GeneralFuncsLib.FormatBorderText(transactionAmount.Text, Color.Green);
                                dataItem["TransactionAmount"].ToolTip = VeraCodeSolution.DoVeraCode(GetLocalResourceObject("DQNextQReportCS_Text_DuplicateAccountNumber").ToString() + ";" +
                                   GetLocalResourceObject("DQNextQReportCS_Text_HighestTransactionAmount").ToString());
                            }
                        }
                    }

                    if (IsExport && dataItem.ItemIndex == ((DataTable)uxReportGridTransactions.DataSource).Rows.Count - 1)
                    {
                        dataItem["PartialAccountNumber"].Font.Bold = true;
                        dataItem["Keyed"].Font.Bold = true;
                        dataItem["TransactionAmount"].Font.Bold = true;
                        dataItem.Attributes.Add("class", "rgFooter");
                    }

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

                    footerItem["PartialAccountNumber"].Text = "&nbsp;" + GetLocalResourceObject("DQNextQReportCS_Text_Total").ToString() + ": " + ((DataTable)uxReportGridTransactions.DataSource).Rows.Count;
                    footerItem["PartialAccountNumber"].Font.Bold = true;
                    footerItem["Keyed"].Text = _TransCountKeyed.ToString();
                    footerItem["Keyed"].Font.Bold = true;

                    footerItem["TransactionAmount"].Text = AS.Common.Formater.FormatData.FormatCurrency(_TransAmount, SessionManager.CurrencyFortmat);// +"&nbsp;&nbsp;";
                    footerItem["TransactionAmount"].Font.Bold = true;
                }
                break;
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
            recordID = uxChargeback.Items[itemIndex].GetDataKeyValue("RecordID").ToString();
            partialCardNum = uxChargeback.Items[itemIndex].GetDataKeyValue("PartialAccountNumber").ToString();
            rpDate = DateTime.Parse(uxChargeback.Items[itemIndex].GetDataKeyValue("ReportDate").ToString());
        }
        else
        {
            recordID = uxReportGridTransactions.Items[itemIndex].GetDataKeyValue("RecordID").ToString();
            partialCardNum = uxReportGridTransactions.Items[itemIndex].GetDataKeyValue("PartialAccountNumber").ToString();
            rpDate = DateTime.Parse(uxReportGridTransactions.Items[itemIndex].GetDataKeyValue("ReportDate").ToString());
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
        //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "openPopupCardWindow('" + urlCardDetail + "');", true);
        RadAjaxManager ajax = RadAjaxManager.GetCurrent(this.Page);
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
        foreach (GridDataItem item in uxReportGridTransactions.MasterTableView.Items)
        {
            //Format Dulicate 
            if (item["DuplicateFlag"].Text.Contains("True"))
            {
                uxReportGridTransactions.MasterTableView.Items[item.ItemIndex].Attributes.Add("style", "background-color:Yellow !important");
                uxReportGridTransactions.MasterTableView.Items[item.ItemIndex].ToolTip = GetLocalResourceObject("DQNextQReportCS_Text_DuplicateAccountNumber").ToString();
            }
            //Format Trans Amt
            if (item["HighestTransactionAmountFlag"].Text.Contains("True"))
            {
                var transactionAmount = item["TransactionAmount"];
                transactionAmount.Text = GeneralFuncsLib.FormatBorderText(transactionAmount.Text, Color.Green);
                if (item["DuplicateFlag"].Text.Contains("True"))
                    transactionAmount.ToolTip = GetLocalResourceObject("DQNextQReportCS_Text_DuplicateAccountNumber").ToString() + "; " +
                        GetLocalResourceObject("DQNextQReportCS_Text_MaximumTicketAmount").ToString();
                else
                    transactionAmount.ToolTip = GetLocalResourceObject("DQNextQReportCS_Text_MaximumTicketAmount").ToString();

            }

        }
        if (uxReportGridTransactions.Items.Count == 0)
            uxReportGridTransactions.ShowFooter = false;
        else
            uxReportGridTransactions.ShowFooter = true;
    }

    #endregion

    private string BuildUrlForFullCard(string reportType, string encryptedCC, string issueBank, string ReportDate)
    {
        string queryString = Page.BuildSecureQueryString("rt=" + reportType + "&cn=" + Server.UrlEncode(encryptedCC) + "&issue=" + issueBank + "&reportdate=" + ReportDate + "&idx=" + (index + 1));
        queryString = ResolveUrl("~/") + "FullCC.aspx?" + queryString;
        return queryString;
    }

    #region ChargeBacks

    private string BuildUrlForFullCard(string recordId, string reportType, string ReportDate)
    {
        string queryString = Page.BuildSecureQueryString("rt=" + reportType + "&cn=" + recordId + "&reportdate=" + ReportDate + "&idx=" + (index + 1));
        queryString = ResolveUrl("~/") + "FullCC.aspx?" + queryString;
        return queryString;
    }


    protected void uxChargeback_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView rowView = e.Item.DataItem as DataRowView;
            if (rowView["TransactionAmount"].IsNullOrEmpty())
            {
                dataItem["TransactionAmount"].Text = "0";
            }
            else
            {
                dataItem["TransactionAmount"].Text = AS.Common.Formater.FormatData.FormatCurrency(Convert.ToDecimal(rowView["TransactionAmount"].ToString()), SessionManager.CurrencyFortmat);
            }

            string acctNumberText = e.Item.ItemIndex.ToString() + ";Chargebacks";
            string urlCard = "<a class=\"link\" href=\"javascript:void(0)\" onclick=\"ShowPopupAcctNumber('" + acctNumberText + "');\">";
            dataItem["PartialAccountNumber"].Text =
                GeneralFuncsLib.CheckCSViewFullCard((SecurePage)this.Page) ? VeraCodeSolution.DoVeraCode(urlCard + rowView["PartialAccountNumber"].ToString() + "</a>&nbsp; &nbsp;")
           : VeraCodeSolution.DoVeraCode(urlCard + rowView["PartialAccountNumber"].ToString() + "</a>");

            if (IsExport)
                dataItem["PartialAccountNumber"].Text = VeraCodeSolution.DoVeraCode(urlCard + rowView["PartialAccountNumber"].ToString() + "</a>");


        }
        if (e.Item is GridFooterItem)
        {
            GridFooterItem footerItem = e.Item as GridFooterItem;
            footerItem["Keyed"].Text = _ChargebackCountKeyed.ToString();
            footerItem["Keyed"].Font.Bold = true;
            footerItem["TransactionAmount"].Text = AS.Common.Formater.FormatData.FormatCurrency(_ChargebackAmount, SessionManager.CurrencyFortmat);// +"&nbsp;&nbsp;";
            footerItem["TransactionAmount"].Font.Bold = true;
        }
    }

    protected void uxChargeback_PreRender(object sender, EventArgs e)
    {
        if (uxChargeback.Items.Count == 0)
            uxChargeback.ShowFooter = false;
        else uxChargeback.ShowFooter = true;
    }
    #endregion



    protected void uxMerchantInfo_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
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
        // [IPMT_Aperia] - 39469 - IPMT - Issue in Next Queue Report- PROD Issue-*Need remediation
        // Force disable sorting function.
        uxBarometerGrid.AllowSorting = uxChargeback.AllowSorting = uxReportGridTransactions.AllowSorting = false;
        base.OnPreRender(e);
        uxAccount.Visible = !IsExport;

        uxAccountNumberClick.Visible = !IsExport;
        uxAccReload.Visible = !IsExport;
        if (uxStatisticsReport.Items.Count > 0)
        {
            ((ImageButton)uxStatisticsReport.Items[0].FindControl("uxCwinButton")).Visible = !IsExport;

            ((Literal)uxStatisticsReport.Items[0].FindControl("txtMerchantNumber")).Text = "<font color=\"Green\">&nbsp;" + this.MerchantNumber + "&nbsp;</font>";

        }

    }
    public void PrepareForExport()
    {
        uxReportGridTransactions.Visible = true;
        uxReportGridTransactions.AllowSorting = false;
        uxReportGridTransactions.ShowFooter = false;
        OnDataBindControls(DataBindAction.BindTransaction);
        uxReportGridTransactions.DataBind();

        uxChargeback.Visible = true;
        uxChargeback.AllowSorting = false;
        uxChargeback.ShowFooter = false;
        OnDataBindControls(DataBindAction.BindChargeBack);
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
            ExportToExcel(HtmlExport);
        }
        else
        {
            base.Render(writer);
        }
    }
    private void ExportToExcel(string htmlString)
    {
        htmlString = "<html xmlns=\"http://www.w3.org/1999/xhtml\">  "
                + @"
                     <head>
                        <style type='text/css'>
                            td
                            {
                                border-style: solid;
                                border-width: thin;
                            }
                            tr.borderBottom td
                            {
                                border-bottom-style: solid;
                                border-bottom-width: thin;
                            }
                            tr.borderTop td
                            {
                                border-top-style: solid;
                                border-top-width: thin;
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
    }

    public void VisibleGrid()
    {
        uxBarometerGrid.Visible = false;
        uxReportGridTransactions.Visible = false;
        uxChargeback.Visible = false;

    }


    protected void uxCwinButton_Click(object sender, ImageClickEventArgs e)
    {
        string riskReportIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(this.ID, new string[] { "MerchantNumber" });
        string queryString = Page.BuildSecureQueryString(string.Format("{0}={1}{2}",
            "MerchantNumber", RiskSessionManager.currentMerchantNumber, riskReportIntruderQuery));
        string url = "rm_RiskReport.aspx?" + queryString;
        Page.Response.Redirect(url);
    }
}