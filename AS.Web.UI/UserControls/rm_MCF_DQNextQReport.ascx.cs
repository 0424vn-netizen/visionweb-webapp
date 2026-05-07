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
using Ninject.Infrastructure;
using System.Globalization;
using AS.Common.Formater;

public partial class UserControls_rm_MCF_DQNextQReport : GlobalUserControl
{
    #region ---- Const ----
    private const string MATCHED_CODE = "MatchCode";
    private const string MATCHED_NAME = "MatchName";
    #endregion

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

    private int _TotalRowCB = 0;
    private int _ChargebackCountKeyed = 0;
    private decimal _ChargebackAmount = 0;

    private int _DailyFcCountKeyed = 0;
    private decimal _DailyFcAmount = 0;

    private DateTime _ReportDate = DateTime.Now.Date;
    public DateTime ReportDate
    {
        get { return _ReportDate; }
        set { _ReportDate = value; }
    }

    private string MerchantNumber { get; set; }

    private const int ScrollRowCount = 30;
    private const int ScrollRowCountForTrans = 10;

    private int _AssignmentID
    {
        get
        {

            if (!string.IsNullOrEmpty(Page.SecureQueryString["AssignmentID"]))
            {
                return Convert.ToInt32(Page.SecureQueryString["AssignmentID"]);
            }
            else
            {
                return -1;
            }
        }
    }
    private int CustomViewID
    {
        get
        {
            var session = RiskSessionManager.RiskMCFDQRainbowReport;
            if (session != null && session.ContainsKey(_AssignmentID))
            {
                return Convert.ToInt32(session[_AssignmentID]);
            }
            return -1;
        }
    }
    protected string urlImage
    {
        get
        {
            string queryStringImage = this.Page.BuildSecureQueryString("MerchantNumber=" + RiskSessionManager.MCF_CurrentNextQueue.MerchantNumber +
                "&ReportDate=" + _ReportDate + "&AssignmentID=" + _AssignmentID);
            return "rm_MCF_DQReasonModal.aspx?" + queryStringImage;
        }
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        tltDupeCount.Text = string.Format("<span class='tooltip-text1'>{0}</span>", GetLocalResourceObject("DupeCount.HeaderDescription").ToString());
        if (Page.IsIntruderDetected) return;
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        if (Page.IsIntruderDetected) return;
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindMerchantInfo:
                {
                    uxMerchantInfo.DataSource = RiskSessionManager.MCF_CurrentNextQueue.MerchantInfo;
                    uxMerchantInfo.DataBind();
                }
                break;
            case DataBindAction.BindBarometer:
                {
                    uxBarometerGrid.DataSource = RiskSessionManager.MCF_CurrentNextQueue.Barometer;
                    uxBarometerGrid.DataBind();
                    uxBarometerGrid.Visible = true;
                }
                break;
            case DataBindAction.BindTransaction:
                {
                    DataTable transactionList = RiskSessionManager.MCF_CurrentNextQueue.Transaction;
                    uxReportGridTransactions.Visible = true;
                    BindTransaction(transactionList, 1);
                }
                break;
            case DataBindAction.BindChargeBack:
                {
                    DataTable chargeback = RiskSessionManager.MCF_CurrentNextQueue.ChargeBack;
                    uxChargeback.Visible = true;
                    BindChargeback(chargeback, 1);
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
        if (dt.IsNotNullData() && dt.Rows.Count > 0)
        {
            DataRow row = dt.Rows[0];
            _TotalRowCB = Convert.ToInt32(row["TotalRows"].ToString());
            _ChargebackCountKeyed = Convert.ToInt32(row["Count_Key"].ToString());
            _ChargebackAmount = Convert.ToDecimal(row["SUM_Amount"].ToString());
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
                    HtmlGenericControl itemTemplate = new HtmlGenericControl();
                    itemTemplate.InnerHtml = RM_MCF_GeneralFuncsLib.GenGridBarometerNextQueue(uxBarometerGrid, CustomViewID, rowItem); ;
                    dataItem.Controls.Add(itemTemplate);
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
        parameters.Add(new FilterParameter("@ReportDate", ReportDate, DbType.DateTime));
        parameters.Add(new FilterParameter("@MerchantNumber", RiskSessionManager.MCF_CurrentNextQueue.MerchantNumber, DbType.String));
        parameters.Add(new FilterParameter("@Mode", 0, DbType.Int32));
        parameters.AddLanguageID();

        SecurePage securePage = HttpContext.Current.Handler as SecurePage;
        if (GeneralFuncsLib.CheckCSViewFullCard(securePage))
        {
            parameters.AddDecryptDataParams("AccountNumber", false);
        }
        parameters.Add(new FilterParameter("@PageNo", pageIndex, DbType.Int32));
        parameters.Add(new FilterParameter("@PageSize", pageSize, DbType.Int32));
        parameters.Add(new FilterParameter("@IsPaging", true, DbType.Boolean));

        DataTable transactionList = WebServices.RiskServices.GetReports("spa_RM_MCF_GetNextQueueReportTransactionDetail", parameters);

        string data = BindTransactionForWS(transactionList);
        string pager = string.Empty;
        if (havePager)
            pager = GetPager(transactionList, pageSize, pageIndex);
        return new string[] { data, pager };
    }

    [WebMethod(EnableSession = true)]
    public string[] GetNQChargebacks(int pageIndex, int pageSize, bool havePager = true)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@ReportDate", ReportDate, DbType.DateTime));
        parameters.Add(new FilterParameter("@MerchantNumber", RiskSessionManager.MCF_CurrentNextQueue.MerchantNumber, DbType.String));

        SecurePage securePage = HttpContext.Current.Handler as SecurePage;
        if (GeneralFuncsLib.CheckCSViewFullCard(securePage))
        {
            parameters.AddDecryptDataParams("AccountNumber", false);
        }
        parameters.Add(new FilterParameter("@PageNo", pageIndex, DbType.Int32));
        parameters.Add(new FilterParameter("@PageSize", pageSize, DbType.Int32));
        parameters.Add(new FilterParameter("@IsPaging", true, DbType.Boolean));
        parameters.AddLanguageID();

        DataTable chargebacks = WebServices.RiskServices.GetReports("spa_RM_MCF_GetNextQueueReportChargebackDetail", parameters);

        string data = BindChargebackForWS(chargebacks);
        string pager = string.Empty;
        if (havePager)
            pager = GetPager(chargebacks, pageSize, pageIndex, true);
        return new string[] { data, pager };
    }

    private string GetPager(DataTable data, int pageSize, int pageIndex, bool isChargeback = false)
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
                    ((UserControls_ASPager)control).FuncPageChange = "PageChanged";
                    if (isChargeback)
                        ((UserControls_ASPager)control).FuncPageChange = "PageChangedCB";
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
        //Set current current paged transactions list
        if (RiskSessionManager.MCF_CurrentNextQueue.Data != null && RiskSessionManager.MCF_CurrentNextQueue.Data.Length > 2)
            RiskSessionManager.MCF_CurrentNextQueue.Data[2] = data;

        CalculateTransactionTotal(data);
        // Format data
        return FormatTransaction(data);

    }

    private string BindChargebackForWS(DataTable data)
    {
        CalculateChargebackTotal(data);
        // Format data
        return FormatChargeback(data);

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
        var totalData = data.Rows.Count;

        for (int index = 0; index < totalData; index++)
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
            uxTransactionTime.Text = VeraCodeSolution.DoVeraCode(row["TransactionTime"].ToString());

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
                    row["AuthorizationNumber"].ToString(), false, true
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
            uxMatch.Text = VeraCodeSolution.DoVeraCode(row[MATCHED_CODE].ToASString());
            uxMatch.ToolTip = VeraCodeSolution.DoVeraCode(row[MATCHED_NAME].ToASString());
            if (row[MATCHED_CODE].ToASString() == "P")
            {
                uxMatch.Text = GeneralFuncsLib.FormatBorderText(uxMatch.Text, Color.Blue);
            }
            else if (row[MATCHED_CODE].ToASString() == "U")
            {
                uxMatch.Text = GeneralFuncsLib.FormatBorderText(uxMatch.Text, Color.Red);
            }
            else if (row[MATCHED_CODE].ToASString() == "M")
            {
                uxMatch.Text = GeneralFuncsLib.FormatBorderText(uxMatch.Text, "#3fbf00".ToColor());
            }
            FormatTooltip(uxMatch, txtDupAccountNum, isDup);

            TableColumnContent uxTransactionAmount = new TableColumnContent(Alignment.Right);
            if (row["TransactionAmount"].IsNullOrEmpty())
            {
                uxTransactionAmount.Text = VeraCodeSolution.DoVeraCode(FormatData.FormatCurrency(Convert.ToDecimal(0), SessionManager.CurrencyFortmat));
            }
            else
            {
                uxTransactionAmount.Text = VeraCodeSolution.DoVeraCode(FormatData.FormatCurrency(Convert.ToDecimal(row["TransactionAmount"].ToString()), SessionManager.CurrencyFortmat));
            }

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

    private string FormatChargeback(DataTable data)
    {
        StringBuilder list = new StringBuilder();
        SecurePage securePage = HttpContext.Current.Handler as SecurePage;
        var totalData = data.Rows.Count;

        for (int index = 0; index < totalData; index++)
        {
            DataRow row = data.Rows[index];

            string acctNumberText = index.ToString() + ";Chargebacks";
            string urlCard = "<a class=\"link\" href=\"javascript:void(0)\" onclick=\"ShowPopupAcctNumber('" + acctNumberText + "');\">";

            TableColumnContent uxPartialAccountNumber = new TableColumnContent(Alignment.Center);
            if (IsExport || !CheckCSViewFullCard(securePage))
                uxPartialAccountNumber.Text = VeraCodeSolution.DoVeraCode(urlCard + row["PartialAccountNumber"].ToString() + "</a>");
            else
                uxPartialAccountNumber.Text = VeraCodeSolution.DoVeraCode(urlCard + row["AccountNumber"].ToString() + "</a>");

            TableColumnContent uxTransactionDate = new TableColumnContent(Alignment.Center);
            uxTransactionDate.Text = VeraCodeSolution.DoVeraCode(uxTransactionDate.CustomFormatDataValue(row["TransactionDate"], FormatType.Date));

            TableColumnContent reasonCode = new TableColumnContent(Alignment.Left);
            reasonCode.Text = VeraCodeSolution.DoVeraCode(row["ReasonCode"].ToASString() + "&nbsp;");

            TableColumnContent keyed = new TableColumnContent(Alignment.Center);
            if (row["Keyed"].IsNullOrEmpty())
            {
                keyed.Text = WebSiteConstants.HTML_EM_DASH;
            }
            else
            {
                keyed.Text = VeraCodeSolution.DoVeraCode(row["Keyed"].ToASString());
            }

            TableColumnContent uxTransactionAmount = new TableColumnContent(Alignment.Right);
            if (row["TransactionAmount"].IsNullOrEmpty())
            {
                uxTransactionAmount.Text = FormatData.FormatCurrency(Convert.ToDecimal(0), SessionManager.CurrencyFortmat);
            }
            else
            {
                uxTransactionAmount.Text = FormatData.FormatCurrency(Convert.ToDecimal(row["TransactionAmount"].ToString()), SessionManager.CurrencyFortmat);
            }

            // Card type
            TableColumnContent cardType = new TableColumnContent(Alignment.Center);
            cardType.Text = VeraCodeSolution.DoVeraCode(row["CardType"].ToASString());
            
            // Report date
            TableColumnContent reportDate = new TableColumnContent(Alignment.Center);
            reportDate.Text = VeraCodeSolution.DoVeraCode(reportDate.CustomFormatDataValue(row["ReportDate"], FormatType.Date));

            list.Append(string.Format("<tr class='{0}'>", index % 2 == 0 ? "Row" : "AltRow"));
            // Acct #
            list.Append(uxPartialAccountNumber.RenderHtml());
            // CT
            list.Append(cardType.RenderHtml());
            //Report Date
            list.Append(reportDate.RenderHtml());
            //Trans Dt
            list.Append(uxTransactionDate.RenderHtml());
            // Reason
            list.Append(reasonCode.RenderHtml());
            //Key
            list.Append(keyed.RenderHtml());
            // TransactionAmount
            list.Append(uxTransactionAmount.RenderHtml());

            list.Append("</tr>");
        }
        // Append total row
        if (data.Rows.Count > 0)
            list.Append(ChargebackTotal());

        return list.ToString();
    }

    private string ChargebackTotal()
    {
        StringBuilder line = new StringBuilder();

        line.Append("<tr class='Footer'>");
        line.Append(string.Format("<td align='left' class='heading'></td>"));
        line.Append("<td></td><td></td><td></td><td></td>");
        line.Append(string.Format("<td align='center'>{0}</td>", VeraCodeSolution.DoVeraCode(_ChargebackCountKeyed.ToString())));
        line.Append(string.Format("<td align='right'>{0}</td>", VeraCodeSolution.DoVeraCode(AS.Common.Formater.FormatData.FormatCurrency(_ChargebackAmount, SessionManager.CurrencyFortmat))));
        line.Append("</tr>");
        return line.ToString();
    }

    private string TransactionTotal()
    {
        StringBuilder line = new StringBuilder();

        line.Append("<tr class='Footer'>");
        line.Append(string.Format("<td align='left' class='heading'>{0}</td>", "&nbsp;" + RiskSessionManager.DQNextQReportCS_Text_Total + ": " + _totalRows));
        line.Append("<td></td><td></td><td></td><td></td><td></td><td></td><td></td>");
        line.Append(string.Format("<td align='center'>{0}</td>", VeraCodeSolution.DoVeraCode(_TransCountKeyed.ToString())));
        line.Append("<td></td><td></td>");
        line.Append(string.Format("<td align='right'>{0}</td>", VeraCodeSolution.DoVeraCode(AS.Common.Formater.FormatData.FormatCurrency(_TransAmount, SessionManager.CurrencyFortmat))));
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
                rowView["AuthorizationNumber"].ToString(), false, true
                ));
        }

        TableColumnContent transactionAmount = dataItem.FindControl("uxTransactionAmount") as TableColumnContent;
        if (rowView["TransactionAmount"].IsNullOrEmpty())
        {
            transactionAmount.Text = VeraCodeSolution.DoVeraCode(FormatData.FormatCurrency(Convert.ToDecimal(0), SessionManager.CurrencyFortmat));
        }
        else
        {
            transactionAmount.Text = VeraCodeSolution.DoVeraCode(FormatData.FormatCurrency(Convert.ToDecimal(rowView["TransactionAmount"].ToString()), SessionManager.CurrencyFortmat));
        }
        
        TableColumnContent transactionTime = dataItem.FindControl("uxTransactionTime") as TableColumnContent;
        transactionTime.Text = VeraCodeSolution.DoVeraCode(rowView["TransactionTime"].ToString());

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
        TableColumnContent uxMatch = dataItem.FindControl("uxMatch") as TableColumnContent;
        uxMatch.Text = VeraCodeSolution.DoVeraCode(rowView[MATCHED_CODE].ToASString());
        uxMatch.ToolTip = VeraCodeSolution.DoVeraCode(rowView[MATCHED_NAME].ToASString());
        if (rowView[MATCHED_CODE].ToASString() == "P")
        {
            uxMatch.Text = GeneralFuncsLib.FormatBorderText(uxMatch.Text, Color.Blue);
        }
        else if (rowView[MATCHED_CODE].ToASString() == "U")
        {
            uxMatch.Text = GeneralFuncsLib.FormatBorderText(uxMatch.Text, Color.Red);
        }
        else if (rowView[MATCHED_CODE].ToASString() == "M")
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
        DateTime rpDate;
        var culture = GeneralFuncsLib.GetCurrentCulture();

        if (arg[1] == "Chargebacks")
        {
            DataTable chargeback = RiskSessionManager.MCF_CurrentNextQueue.ChargeBack;
            recordID = chargeback.Rows[itemIndex]["RecordID"].ToString();
            partialCardNum = chargeback.Rows[itemIndex]["PartialAccountNumber"].ToString();
            rpDate = DateTime.Parse(chargeback.Rows[itemIndex]["ReportDate"].ToString(), CultureInfo.CreateSpecificCulture(culture));
        }
        else
        {
            DataTable transactions = RiskSessionManager.MCF_CurrentNextQueue.Transaction;
            recordID = transactions.Rows[itemIndex]["RecordID"].ToString();
            partialCardNum = transactions.Rows[itemIndex]["PartialAccountNumber"].ToString();
            rpDate = DateTime.Parse(transactions.Rows[itemIndex]["ReportDate"].ToString(), CultureInfo.CreateSpecificCulture(culture));
        }
        string reportType = arg[1];
        string fullCC = String.Empty;
        FilterParameterCollection parameters_FullCard = new FilterParameterCollection();

        parameters_FullCard.Add(new FilterParameter("@UserID", SessionManager.CurrentUser.UserID, DbType.AnsiString));
        parameters_FullCard.Add(new FilterParameter("@ASClient", SessionManager.CurrentUser.ASClient, DbType.Int32));
        parameters_FullCard.Add(new FilterParameter("@RecordID", Convert.ToInt32(recordID), DbType.Int32));
        parameters_FullCard.Add(new FilterParameter("@ReportType", reportType, DbType.AnsiString));
        parameters_FullCard.Add(new FilterParameter("@ReportDate", rpDate, DbType.DateTime));
        parameters_FullCard.Add(new FilterParameter("@IsMCF", true, DbType.Boolean)); // User for NextQueue

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

        string queryString = Page.BuildSecureQueryString("cn=" + partialCardNum + "&cnf=" + fullCC + "&merch=" + RiskSessionManager.MCF_CurrentNextQueue.MerchantNumber + "&isRisk=1");
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
        uxTransactionAmountFooterTotal.Text = VeraCodeSolution.DoVeraCode(AS.Common.Formater.FormatData.FormatCurrency(_TransAmount, SessionManager.CurrencyFortmat));
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
            transactionAmount.Text = FormatData.FormatCurrency(Convert.ToDecimal(0), SessionManager.CurrencyFortmat);
        }
        else
        {
            transactionAmount.Text = FormatData.FormatCurrency(Convert.ToDecimal(rowView["TransactionAmount"].ToString()), SessionManager.CurrencyFortmat);
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
        uxTransactionAmountTotal.Text = AS.Common.Formater.FormatData.FormatCurrency(_ChargebackAmount, SessionManager.CurrencyFortmat);
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
            string profileDescription = rowItem["ProfileDescription"].ToString();
            lblProfile.Text = VeraCodeSolution.DoVeraCode(!string.IsNullOrEmpty(profileDescription) ? profileDescription : WebSiteConstants.HTML_EM_DASH_ENCODE.ToString());
            if (rowItem["ProfileType"].ToString() != "0")
                lblProfile.ToolTip = VeraCodeSolution.DoVeraCode(rowItem["ProfileType"] + " - " + rowItem["ProfileDescription"]);

            Label lblSIC = e.Item.FindControl("lblSIC") as Label;
            if (!string.IsNullOrEmpty(rowItem["SIC"].ToString()))
            {
                lblSIC.Text = VeraCodeSolution.DoVeraCode(rowItem["SIC"].ToString());
                lblSIC.ToolTip = VeraCodeSolution.DoVeraCode(rowItem["SIC"].ToString());
            }
            else
            {
                lblSIC.Text = WebSiteConstants.HTML_EM_DASH_ENCODE;
            }

            Label lblHierarchyName = e.Item.FindControl("lblHierarchyName") as Label;
            lblHierarchyName.Text = VeraCodeSolution.DoVeraCode(rowItem["HierarchyName"].ToString());
            if (!string.IsNullOrEmpty(rowItem["HierarchyName"].ToString()))
                lblHierarchyName.ToolTip = VeraCodeSolution.DoVeraCode(rowItem["HierarchyName"].ToString());

            Label lblHierarchyValue = e.Item.FindControl("lblHierarchyValue") as Label;
            lblHierarchyValue.Text = VeraCodeSolution.DoVeraCode(rowItem["HierarchyValue"].ToString());
            if (!string.IsNullOrEmpty(rowItem["HierarchyValue"].ToString()))
                lblHierarchyValue.ToolTip = VeraCodeSolution.DoVeraCode(rowItem["HierarchyValue"].ToString());

            Label lblClassificationName = e.Item.FindControl("lblClassificationName") as Label;
            if (!string.IsNullOrEmpty(rowItem["ClassificationName"].ToString()))
            {
                lblClassificationName.Text = VeraCodeSolution.DoVeraCode(rowItem["ClassificationName"].ToString());
                lblClassificationName.ToolTip = VeraCodeSolution.DoVeraCode(rowItem["ClassificationName"].ToString());
            }
            else
            {
                lblClassificationName.Text = WebSiteConstants.HTML_EM_DASH_ENCODE;
            }

            //Format data
            Label uxTodayVolume = e.Item.FindControl("uxTodayVolume") as Label;
            FormatCurrency(uxTodayVolume, VeraCodeSolution.DoVeraCode(rowItem["TodayVolume"].ToString()));
            Label uxDailyVolume = e.Item.FindControl("uxDailyVolume") as Label;
            FormatCurrency(uxDailyVolume, VeraCodeSolution.DoVeraCode(rowItem["ExpectedDailyVolume"].ToString()));
            Label uxTodayAverageTicket = e.Item.FindControl("uxTodayAverageTicket") as Label;
            FormatCurrency(uxTodayAverageTicket, VeraCodeSolution.DoVeraCode(rowItem["TodayAverageTicket"].ToString()));
            Label uxTodayHighestTransactionAmount = e.Item.FindControl("uxTodayHighestTransactionAmount") as Label;
            FormatCurrency(uxTodayHighestTransactionAmount, VeraCodeSolution.DoVeraCode(rowItem["TodayHighestTransactionAmount"].ToString()));
            Label uxContractHighestTicket = e.Item.FindControl("uxContractHighestTicket") as Label;
            FormatCurrency(uxContractHighestTicket, VeraCodeSolution.DoVeraCode(rowItem["ContractHighestTicket"].ToString()));
            Label uxExpectedAverageTicket = e.Item.FindControl("uxExpectedAverageTicket") as Label;
            FormatCurrency(uxExpectedAverageTicket, VeraCodeSolution.DoVeraCode(rowItem["ExpectedAverageTicket"].ToString()));
            Label uxMTDVolume = e.Item.FindControl("uxMTDVolume") as Label;
            FormatCurrency(uxMTDVolume, VeraCodeSolution.DoVeraCode(rowItem["MTDVolume"].ToString()));
            Label uxMV1 = e.Item.FindControl("uxMV1") as Label;
            FormatCurrency(uxMV1, VeraCodeSolution.DoVeraCode(rowItem["MV1"].ToString()));
            Label uxYTDVolume = e.Item.FindControl("uxYTDVolume") as Label;
            FormatCurrency(uxYTDVolume, VeraCodeSolution.DoVeraCode(rowItem["YTDVolume"].ToString()));
            Label uxMV2 = e.Item.FindControl("uxMV2") as Label;
            FormatCurrency(uxMV2, VeraCodeSolution.DoVeraCode(rowItem["MV2"].ToString()));
            Label uxMV3 = e.Item.FindControl("uxMV3") as Label;
            FormatCurrency(uxMV3, VeraCodeSolution.DoVeraCode(rowItem["MV3"].ToString()));
            System.Web.UI.WebControls.LinkButton uxMerchantNumLink = e.Item.FindControl("uxMerchantNumLink") as System.Web.UI.WebControls.LinkButton;
            uxMerchantNumLink.OnClientClick = "openPopupWindow('rm_MCF_RiskReport.aspx?" + Page.BuildSecureQueryString(string.Format("merchantNumber={0}&IsPopup=true", RiskSessionManager.MCF_CurrentNextQueue.MerchantNumber)) + "','RiskReport'); return false;";
            uxMerchantNumLink.Text = rowItem["MerchantNumber"].ToString();
            Label uxMonthlyNetAmtContract = e.Item.FindControl("uxMonthlyNetAmtContract") as Label;
            FormatCurrency(uxMonthlyNetAmtContract, VeraCodeSolution.DoVeraCode(rowItem["ExpectedMonthlyVolume"].ToString()));

            Label uxKeyedTransPctContract = e.Item.FindControl("uxKeyedTransPctContract") as Label;
            var KeyedTransPctContract = rowItem["ExpectedPercentSWP"].ToString();
            KeyedTransPctContract = !string.IsNullOrEmpty(KeyedTransPctContract) ? AS.Common.Formater.FormatData.FormatPercent(KeyedTransPctContract) : WebSiteConstants.HTML_EM_DASH_ENCODE;
            uxKeyedTransPctContract.Text = KeyedTransPctContract;

            var KeyedTransPctToday = rowItem["KeyPercent"].ToString();
            KeyedTransPctToday = !string.IsNullOrEmpty(KeyedTransPctToday) ? AS.Common.Formater.FormatData.FormatPercent(KeyedTransPctToday) : WebSiteConstants.HTML_EM_DASH_ENCODE;
            Label uxKeyedTransPctToday = e.Item.FindControl("uxKeyedTransPctToday") as Label;
            uxKeyedTransPctToday.Text = KeyedTransPctToday;
        }
    }

    protected void FormatCurrency(Label sender, string value)
    {
        if (value.IsNullOrEmpty())
        {
            sender.Text = WebSiteConstants.HTML_EM_DASH_ENCODE;
            return;
        }
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
    protected string FormatCurrencyWithFormat(string value, string format)
    {
        if (value.IsNullOrEmpty() || (!value.IsNullOrEmpty() && value.Contains(WebSiteConstants.HTML_EM_DASH)))
        {
            return WebSiteConstants.HTML_EM_DASH;
        }
        Double temp = Double.Parse(value);
        return AS.Common.Formater.FormatData.FormatCurrency(temp);
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
            if (IsExport)
            {
                ((System.Web.UI.WebControls.Literal)uxMerchantInfo.Items[0].FindControl("txtMerchantNumber")).Text = "<font color=\"Green\">&nbsp;" + RiskSessionManager.MCF_CurrentNextQueue.MerchantNumber + "&nbsp;</font>";
                ((System.Web.UI.WebControls.LinkButton)uxMerchantInfo.Items[0].FindControl("uxMerchantNumLink")).Visible = false;
                uxMerchantInfoTitle.Visible = true;
            }
        }
    }

    public void PrepareForExport()
    {
        IsExport = true;
        OnDataBindControls(DataBindAction.BindBarometer);
        uxPnlPager.Visible = false;
        uxPnlPagerCB.Visible = false;
        uxPnlCustomview.Visible = false;
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

    private string ChargebackHtml()
    {
        string[] datas = this.GetNQChargebacks(1, int.MaxValue, false);
        if (string.IsNullOrEmpty(datas[0]))
            return string.Empty;
        StringWriter tw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(tw);
        StringBuilder strs = new StringBuilder();
        //Header
        strs.Append("<table> <tr>");
        uxPartialAccountNumberHeader.RenderControl(hw);
        TableColumnHeader44.RenderControl(hw);
        TableColumnHeader45.RenderControl(hw);
        TableColumnHeader26.RenderControl(hw);
        TableColumnHeader27.RenderControl(hw);
        TableColumnHeader28.RenderControl(hw);
        TableColumnHeader29.RenderControl(hw);
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
            StringBuilder sb = new StringBuilder();

            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter hWriter = new HtmlTextWriter(sw);
            base.Render(hWriter);
            HtmlExport = sb.ToString();

            //Replace full transaction and transaction
            string reg = "(<!--{0}-->((.|\n)*)<!--/{0}-->)";
            string regHeader = string.Format(reg, "Transaction");
            string regHeaderCB = string.Format(reg, "Chargeback");
            var barometer = string.Format(reg, "Barometer");
            Match headerMatch = Regex.Match(HtmlExport, regHeader);
            Match headerMatchCB = Regex.Match(HtmlExport, regHeaderCB);
            Match barometerRegex = Regex.Match(HtmlExport, barometer);

            var exportHtml = new StringBuilder();
            string dataTrans = TransactionHtml();
            string dataCB = ChargebackHtml();
            var statistic = GetStatisticInformation();
            var barometerHtml = string.Empty;

            if (headerMatchCB.Success && string.IsNullOrEmpty(dataCB))
                dataCB = headerMatchCB.Value;

            if (headerMatch.Success && string.IsNullOrEmpty(dataTrans))
                dataTrans = headerMatch.Value;

            if (barometerRegex.Success)
            {
                barometerHtml = barometerRegex.Value;
                barometerHtml = Regex.Replace(barometerHtml, "<input.*? />", string.Empty);
            }

            exportHtml.Append(statistic);
            exportHtml.Append(string.Format("<br/><div><strong>{0}</strong></div>", GetLocalResourceObject("BarometerReport.Text").ToString()));
            exportHtml.Append(barometerHtml);
            exportHtml.Append(string.Format("<br/><div><strong>{0}</strong></div>", GetLocalResourceObject("litHeaderChargeback90daysResource1.Text").ToString()));
            exportHtml.Append(dataCB);
            exportHtml.Append(string.Format("<br/><div><strong>{0}</strong></div>", GetLocalResourceObject("litHeaderTransactionTodayResource1.Text").ToString()));
            exportHtml.Append(dataTrans);

            var html = Regex.Replace(exportHtml.ToString(), "<input.*? />", string.Empty);
            ExportToExcel(html);
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
        htmlString = htmlString.Replace("td class=\"heading", "td style=\"font-weight:bold;\" class=\"");
        htmlString = htmlString.Replace("!important", string.Empty);
        htmlString = htmlString.Replace("class=\"Caption AltRow\"", "class=\"Caption\"");
        string fileName = GetLocalResourceObject("DQNextReportCS_Test_NestQReport").ToString() + "_" + RiskSessionManager.MCF_CurrentNextQueue.MerchantNumber;
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
                ((UserControls_ASPager)control).FuncPageChange = "PageChanged";
                ((UserControls_ASPager)control).InitPager(totalRows, pageSize, pageIndex - 1);
                uxPnlPager.Controls.Add(control);
            }
            else
            {
                uxPnlPager.Visible = false;
            }
        }
    }

    private void BindChargeback(DataTable data, int pageIndex)
    {
        CalculateChargebackTotal(data);
        uxChargeback.DataSource = data;
        uxChargeback.DataBind();

        if (data.IsNotNullData() && data.Rows.Count > 0)
        {
            int totalRows = Convert.ToInt32(data.Rows[0]["TotalRows"].ToString());
            int pageSize = WebSiteSettings.DefaultRiskPageSize;
            if (totalRows > pageSize)
            {
                uxPnlPagerCB.Visible = true;
                uxPnlPagerCB.Controls.Clear();
                Control control = this.Page.LoadControl("~/UserControls/ASPager.ascx");
                ((UserControls_ASPager)control).FuncPageChange = "PageChangedCB";
                ((UserControls_ASPager)control).InitPager(totalRows, pageSize, pageIndex - 1);
                uxPnlPagerCB.Controls.Add(control);
            }
            else
            {
                uxPnlPagerCB.Visible = false;
            }
        }
    }

    protected void uxRebindBarometer_Click(object sender, EventArgs e)
    {
        uxBarometerGrid.DataSource = RiskSessionManager.MCF_CurrentNextQueue.Barometer;
        uxBarometerGrid.DataBind();
        uxBarometerGrid.Visible = true;
    }
    private string GetStatisticInformation()
    {
        var currencyFormat = SessionManager.CurrencyFortmat;
        var data = RiskSessionManager.MCF_CurrentNextQueue.MerchantInfo;
        if (data == null || data.Rows.Count == 0)
            return string.Empty;

        var templateName = "~/App_Data/NextQueueStatisticExportTemplate.htm";
        var path = HttpContext.Current.Server.MapPath(templateName);
        var template = File.ReadAllText(path);

        //Merchant Info
        //bind resource text
        template = template.Replace("[MerchantInformation]", GetLocalResourceObject("litGridTitle3.Text").ToString().Trim())
            .Replace("[MerchantNumber_Text]", GetLocalResourceObject("MerchantNumber").ToString().Trim())
            .Replace("[City_State_Text]", GetLocalResourceObject("lituxMerchantInfoHeaderCityStateResource1.Text").ToString().Trim())
            .Replace("[SICCode_Text]", GetLocalResourceObject("lituxMerchantInfoHeaderSICCodeResource1.Text").ToString().Trim())
            .Replace("[BackendProcessor_Text]", GetLocalResourceObject("lituxMerchantInfoHeaderBEProcResource1.Text").ToString().Trim())
            .Replace("[Status_Text]", GetLocalResourceObject("lituxMerchantInfoHeaderStatusResource1.Text").ToString().Trim())
            .Replace("[ApprovalDate_Text]", GetLocalResourceObject("lituxMerchantInfoHeaderApprDateResource1.Text").ToString().Trim())
            .Replace("[ChainNumber_Text]", GetLocalResourceObject("lituxMerchantInfoHeaderChainNOResource1.Text").ToString().Trim());

        //bind data
        template = template.Replace("[MerchantNumber]", RiskSessionManager.MCF_CurrentNextQueue.MerchantNumber + "&nbsp;")
            .Replace("[City_State]", GetColumnValue("CityState", data).ToString())
            .Replace("[SICCode]", GetColumnValue("SIC", data).ToString())
            .Replace("[BackendProcessor]", GetColumnValue("BackEndProcessor", data).ToString())
            .Replace("[Status]", GetColumnValue("Status", data).ToString())
            .Replace("[ApprovalDate]", ConvertDate(GetColumnValue("ApprovalDate", data)) + "&nbsp;")
            .Replace("[ChainNumber]", GetColumnValue("ChainNumber", data).ToString() + "&nbsp;")
            .Replace(" [Bank_Association_Agent]", GetColumnValue("HierarchyValue", data).ToString())
            .Replace("[Bank_Association_Agent_Text]", GetColumnValue("HierarchyName", data).ToString());

        //Statistic info
        var KeyedTransPctContract = GetColumnValue("ExpectedPercentSWP", data).ToString();
        KeyedTransPctContract = KeyedTransPctContract != WebSiteConstants.HTML_EM_DASH ? AS.Common.Formater.FormatData.FormatPercent(KeyedTransPctContract) : WebSiteConstants.HTML_EM_DASH;

        var KeyedTransPctToday = GetColumnValue("KeyPercent", data).ToString();
        KeyedTransPctToday = KeyedTransPctToday != WebSiteConstants.HTML_EM_DASH ? AS.Common.Formater.FormatData.FormatPercent(KeyedTransPctToday) : WebSiteConstants.HTML_EM_DASH;

        template = template.Replace("[Risk_Text]", GetLocalResourceObject("lituxMerchantInfoHeaderRiskResource1.Text").ToString().Trim())
            .Replace("[HistoryPerformance_Text]", GetLocalResourceObject("lituxMerchantInfoHeaderHistoricPerformanceResource1.Text").ToString().Trim())
            .Replace("[Metrics_Text]", GetLocalResourceObject("lituxMerchantInfoHeaderMetricsResource1.Text").ToString().Trim())
            .Replace("[Contract_Text]", GetLocalResourceObject("lituxMerchantInfoHeaderContractResource1.Text").ToString().Trim())
            .Replace("[Today_Text]", GetLocalResourceObject("lituxMerchantInfoHeaderTodaysResource1.Text").ToString().Trim())
            .Replace("[RiskScore_Text]", GetLocalResourceObject("lituxMerchantInfoHeaderRiskScoreResource1.Text").ToString().Trim())
            .Replace("[RiskScore]", AS.Common.Formater.FormatData.FormatNumber(GetColumnValue("RiskScore", data), 0))
            .Replace("[KeyedTransPct_Text]", GetLocalResourceObject("lituxMerchantInfoHeaderKeyedTransPctResource1.Text").ToString().Trim())
            .Replace("[KeyedTransPct_C]", KeyedTransPctContract)
            .Replace("[KeyedTransPct_T]", KeyedTransPctToday)
            .Replace("[AvgTransAmt_Text]", GetLocalResourceObject("lituxMerchantInfoHeaderAvgTransAmtResource1.Text").ToString().Trim())
            .Replace("[AvgTransAmt_C]", FormatCurrencyWithFormat(GetColumnValue("ExpectedAverageTicket", data).ToString(), currencyFormat))
            .Replace("[AvgTransAmt_T]", FormatCurrencyWithFormat(GetColumnValue("TodayAverageTicket", data).ToString(), currencyFormat))
            .Replace("[LargestTransAmt_Text]", GetLocalResourceObject("lituxMerchantInfoHeaderLargestTransAmtResource1.Text").ToString().Trim())
            .Replace("[LargestTransAmt_C]", FormatCurrencyWithFormat(GetColumnValue("ContractHighestTicket", data).ToString(), currencyFormat))
            .Replace("[LargestTransAmt_T]", FormatCurrencyWithFormat(GetColumnValue("TodayHighestTransactionAmount", data).ToString(), currencyFormat))
            .Replace("[NetAmt_Text]", GetLocalResourceObject("lituxMerchantInfoHeaderTodayVolResource1.Text").ToString().Trim())
            .Replace("[NetAmt_C]", FormatCurrencyWithFormat(GetColumnValue("TodayVolume", data).ToString(), currencyFormat))
            .Replace("[NetAmt_T]", FormatCurrencyWithFormat(GetColumnValue("ExpectedDailyVolume", data).ToString(), currencyFormat))
            .Replace("[MerchantClassification_Text]", GetLocalResourceObject("lituxMerchantInfoHeaderMerchantClassificationResource1.Text").ToString().Trim())
            .Replace("[MerchantClassification]", GetColumnValue("ClassificationName", data).ToString())
            .Replace("[Profile_Text]", GetLocalResourceObject("lituxMerchantInfoHeaderProfileResource1.Text").ToString().Trim())
            .Replace("[Profile]", GetColumnValue("ProfileDescription", data).ToString())
            .Replace("[TimesWorked_Text]", GetLocalResourceObject("Worked").ToString())
            .Replace("[TimesWorked]", GetColumnValue("Worked", data).ToString())
            .Replace("[ParametersWorked_Text]", GetLocalResourceObject("ParametersOfWorked").ToString())
            .Replace("[ParametersWorked]", GetColumnValue("ParametersWorked", data).ToString())
            .Replace("[MTDNetAmt_Text]", GetLocalResourceObject("lituxMerchantInfoHeaderMTDNetAmtResource1.Text").ToString().Trim())
            .Replace("[MTDNetAmt]", FormatCurrencyWithFormat(GetColumnValue("MTDVolume", data).ToString(), currencyFormat))
            .Replace("[Mo1NetAmt_Text]", GetLocalResourceObject("lituxMerchantInfoHeaderMv1Resource1.Text").ToString().Trim())
            .Replace("[Mo1NetAmt]", FormatCurrencyWithFormat(GetColumnValue("MV1", data).ToString(), currencyFormat))
            .Replace("[Mo2NetAmt_Text]", GetLocalResourceObject("lituxMerchantInfoHeaderMV2Resource1.Text").ToString().Trim())
            .Replace("[Mo2NetAmt]", FormatCurrencyWithFormat(GetColumnValue("MV2", data).ToString(), currencyFormat))
            .Replace("[Mo3NetAmt_Text]", GetLocalResourceObject("lituxMerchantInfoHeaderMV3Resource1.Text").ToString().Trim())
            .Replace("[Mo3NetAmt]", FormatCurrencyWithFormat(GetColumnValue("MV3", data).ToString(), currencyFormat))
            .Replace("[YTDNetAmt_Text]", GetLocalResourceObject("lituxMerchantInfoHeaderYTDVolResource1.Text").ToString().Trim())
            .Replace("[YTDNetAmt]", FormatCurrencyWithFormat(GetColumnValue("YTDVolume", data).ToString(), currencyFormat))
            .Replace("[MonthlyNetAmt_Text]", GetLocalResourceObject("lituxMerchantInfoHeaderMonthlyNetAmtResource1.Text").ToString().Trim())
            .Replace("[MonthlyNetAmt_C]", FormatCurrencyWithFormat(GetColumnValue("ExpectedMonthlyVolume", data).ToString(), currencyFormat));
        return template;
    }

    private object GetColumnValue(string colName, DataTable source)
    {
        object columnValue = null;
        if (source == null || source.Rows.Count == 0)
            return WebSiteConstants.HTML_EM_DASH;

        if (source.Columns.Contains(colName))
        {
            columnValue = source.Rows[0][colName];
        }

        if (columnValue == null || columnValue == DBNull.Value || string.IsNullOrEmpty(columnValue.ToString()))
            return WebSiteConstants.HTML_EM_DASH;

        return columnValue;
    }
    private string GetProfileDescription(string type, string description)
    {
        var profile = description;
        if (type != "0")
            profile = type + " - " + description;

        return profile;
    }
}