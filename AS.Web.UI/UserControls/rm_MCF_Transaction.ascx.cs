using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Global;
using AS.Controls.Pages;
using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AS.Utilities;
using AS.Controls.Grid;
using System.IO;
using System.Web.Services;
using System.Text;

// System.Web.UI.UserControl: Using call Ajax
public partial class UserControls_rm_MCF_Transaction : System.Web.UI.UserControl
{
    private int _totalRows = 0;
    private int _TransCountKeyed = 0;
    private decimal _TransAmount = 0;
    int index = 0;
    private const string MATCHED_CODE = "MatchCode";
    private const string MATCHED_NAME = "MatchName";

    enum DataBindAction
    {
        BindTransaction,
    }

    public string MerchantList { get; set; }
    public int FilterWorkingStatus { get; set; }
    public DateTime ReportDate { get; set; }
    public bool IsCSViewFullCard { get; set; }
    public int PageIndex { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        tltDupeCount.Text = string.Format("<span class='tooltip-text1'>{0}</span>", GetLocalResourceObject("DupeCount.HeaderDescription").ToString());
        BindTransaction(PageIndex);
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

        string acctNumberText = string.Format("{0};{1};{2};{3};{4};{5}", e.Item.ItemIndex.ToString(), rowView["ReportType"].ToString(), rowView["RecordID"].ToString(), 
            rowView["PartialAccountNumber"].ToString(), rowView["ReportDate"].ToString(), rowView["MerchantNumber"].ToString());
        string urlCard = "<a class=\"link\" href=\"javascript:void(0)\" onclick=\"ShowPopupAcctNumber('" + acctNumberText + "');\">";
        TableColumnContent partialAccountNumber = dataItem.FindControl("uxPartialAccountNumber") as TableColumnContent;
        if (!IsCSViewFullCard)
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
        
        if (!String.IsNullOrEmpty(rowView["AuthorizationNumber"].ToString()))
        {
            string authText = string.Format("{0};{1};{2}", rowView["AuthorizationNumber"], rowView["MerchantNumber"].ToString()
                , rowView["TransactionDate"]);
            string urlAuth = "<a class=\"link\" href=\"javascript:void(0)\" onclick=\"ShowPopupAuthNumber('" + authText + "');\">";
            authorizationNumber.Text = VeraCodeSolution.DoVeraCode(urlAuth + rowView["AuthorizationNumber"].ToString() + "</a>");
        }

        TableColumnContent transactionAmount = dataItem.FindControl("uxTransactionAmount") as TableColumnContent;
        if (rowView["TransactionAmount"].IsNullOrEmpty())
        {
            transactionAmount.Text = VeraCodeSolution.DoVeraCode(AS.Common.Formater.FormatData.FormatCurrency(Convert.ToDecimal(0), SessionManager.CurrencyFortmat));
        }
        else
        {
            transactionAmount.Text = VeraCodeSolution.DoVeraCode(AS.Common.Formater.FormatData.FormatCurrency(Convert.ToDecimal(rowView["TransactionAmount"].ToString()), SessionManager.CurrencyFortmat));
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

    private void BindTransaction(int pageIndex)
    {
        DataTable data = GetTransactionList(pageIndex, ReportDate, MerchantList);
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

    private DataTable GetTransactionList(int pageIndex, DateTime reportDate, string merchantNumber)
    {
        FilterParameterCollection parameterList = new FilterParameterCollection();
        parameterList.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameterList.AddLanguageID();
        parameterList.Add(new FilterParameter("@ReportDate", reportDate, DbType.DateTime));
        parameterList.Add(new FilterParameter("@MerchantList", merchantNumber, DbType.String));
        SecurePage securePage = HttpContext.Current.Handler as SecurePage;
        if (GeneralFuncsLib.CheckCSViewFullCard(securePage))
        {
            parameterList.AddDecryptDataParams("AccountNumber", false);
        }
        parameterList.Add(new FilterParameter("@PageNo", pageIndex, DbType.Int32));
        parameterList.Add(new FilterParameter("@PageSize", WebSiteSettings.DefaultRiskPageSize, DbType.Int32));
        parameterList.Add(new FilterParameter("@IsPaging", true, DbType.Boolean));

        return WebServices.RiskServices.GetReports("spa_RM_MCF_GetFlatReportTransactionDetail_MultiMerchant", parameterList);
    }
}