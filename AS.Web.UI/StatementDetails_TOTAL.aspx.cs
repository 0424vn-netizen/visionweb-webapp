using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AS.Common.DBManager;
using AS.Common.Formater;
using AS.Controls.Grid;
using Telerik.Web.UI;
using System.IO;
using System.Text;
using AS.Common;
using AS.Controls.Pages;
using AS.Web.Business;
using System.Data;
using AS.Common.Utilities;
using System.Configuration;
using WebSupergoo.ABCpdf9;
using System.Resources;

[PagePermission("StatementRpt,MSStatementRpt")]
public partial class StatementDetails_TOTAL : ReportPage
{
    #region Enum

    enum ReportType
    {
        Deposit,
        CardSummary,
        SettleDisCount,
        SurCharge,
        OtherFee,
        ProcessingRate,
        TransactionFee,
        Event = 1,
        MonthlyPromotion = 2
    }

    #endregion

    protected bool isCustomStatementTheme = false;
    string MerchantNumber = string.Empty;
    protected DateTime ReportDate = DateTime.Now;
    protected ClientInfo _contactInformation = null;
    protected ClientInfo ContactInformation
    {
        get
        {
            if (_contactInformation != null)
            {
                return _contactInformation;
            }
            else
            {
                FilterParameterCollection parameters = new FilterParameterCollection();
                parameters.Add(new FilterParameter("@ASClient", SessionManager.CurrentClient, System.Data.DbType.Int32));
                parameters.Add(new FilterParameter("@ContactName", "CupcakeStatementClientInformation", System.Data.DbType.AnsiString));
                DataTable clientInfo = WebServices.MsReportServices.GetReports("spa_GetContactUsInfo", parameters);

                if (clientInfo != null && clientInfo.Rows.Count > 0)
                {
                    _contactInformation = new ClientInfo();
                    _contactInformation.ClientName = clientInfo.Rows[0]["RefTblCol1"].ToString();
                    _contactInformation.Address1 = clientInfo.Rows[0]["RefTblCol2"].ToString();
                    _contactInformation.Address2 = clientInfo.Rows[0]["RefTblCol3"].ToString();
                    _contactInformation.Phone = clientInfo.Rows[0]["RefTblCol4"].ToString();
                    _contactInformation.ContactEmail = clientInfo.Rows[0]["RefTblCol5"].ToString();
                    return _contactInformation;
                }
                else
                {
                    return SessionManager.ClientInfo;
                }
            }
        }
    }
    protected double _totalFeesBillToYourAccount = 0;
    private double _totalProcessingFees = 0;

    // header : clientinfo
    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);

        this.PageType = SecurePageType.Modal;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsSecureQueryString && !IsPostBack)
        {
            this.MerchantNumber = SecureQueryString["MerchantNumber"];
            ltMerchantID.Text = VeraCodeSolution.DoVeraCode(this.MerchantNumber);
            string _reportdate = SecureQueryString["ReportDate"];
            this.ReportDate = new DateTime(long.Parse(_reportdate));
            ltReportDate.Text = VeraCodeSolution.DoVeraCode(this.ReportDate.ToString("MMM dd, yyyy"));
            BindData();
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }

    protected void BindClientInfomation()
    {
        if (ContactInformation != null)
        {
            ltClientname.Text = ContactInformation.ClientName.Replace("&reg;", "<span class=\"reg\">&reg;</span>&nbsp;&nbsp;&nbsp;&nbsp;");
            if (!string.IsNullOrEmpty(ContactInformation.Address1))
            {
                plhdAdd1.Visible = true;
            }
            else
            {
                plhdAdd1.Visible = false;
            }
            ltClientAddress1.Text = VeraCodeSolution.DoVeraCode(ContactInformation.Address1);
            if (!string.IsNullOrEmpty(ContactInformation.Address2))
            {
                plhdAdd2.Visible = true;
            }
            else
            {
                plhdAdd2.Visible = false;
            }
            ltClientAddress2.Text = VeraCodeSolution.DoVeraCode(ContactInformation.Address2);
            ltClientContact.Text = VeraCodeSolution.DoVeraCode(ContactInformation.ContactEmail);
            ltQuestion.Text = VeraCodeSolution.DoVeraCode(ContactInformation.Phone);
        }
    }

    protected void BindData()
    {
        // Client info
        BindClientInfomation();

        // Business name
        BindBusinessName();

        // Monthly statement
        BindMonthlyStatement();

        // In the news
        BindMessages();

        // Detail of deposits by day
        BindDetailOfDepositsByDay();

        // Monthly Fees and Promotions
        BindMonthlyFeesAndPromotions();

        // Event-Driven Fees
        BindEventDrivenFees();

        // Processing Rate
        BindProcessingRate();

        // Transaction Fee
        BindTransactionFee();

        ltTotalFees.Text = FormatCurrency(_totalProcessingFees);
    }

    // Business name
    protected void BindBusinessName()
    {
        DataTable data = new DataTable();
        data = GetMerchantInfomation();
        if (data.Rows.Count > 0)
        {
            ltBusinessName.Text = data.Rows[0]["Name"].ToString();
            ltOwner.Text = data.Rows[0]["Attention"].ToString();
            ltAddress1.Text = data.Rows[0]["Address1"].ToString();
            if (!string.IsNullOrEmpty(data.Rows[0]["Address2"].ToString()))
            {
                ltAddress2.Text = data.Rows[0]["Address2"].ToString();
                pnMerchantAddress2.Visible = true;
            }
            else
            {
                pnMerchantAddress2.Visible = false;
            }

            ltZipCodeCity.Text = data.Rows[0]["City"].ToString() + ", " + data.Rows[0]["State"].ToString() + ", " + data.Rows[0]["Zip"].ToString();
        }
    }

    // In the news
    protected void BindMessages()
    {
        if (GeneralFuncsLib.HasStatementMessageSection())
        {
            DataTable data = GetStatementMessage();
            if (data.Rows.Count > 0)
            {
                ltMessages.Text = VeraCodeSolution.DoVeraCode(data.Rows[0]["Message"].ToString());
            }
            else
            {
                ltMessages.Text = VeraCodeSolution.DoVeraCode(GetLocalResourceObject("ltEDFNoDataResource1.Text").ToString());
            }
        }
    }

    // Detail of deposits by day
    protected void BindDetailOfDepositsByDay()
    {
        DataTable data = GetDetailOfDepositsByDay(false);
        if (data != null && data.Rows.Count > 0)
        {
            rptDetailOfDepositsByDay.DataSource = data;
            rptDetailOfDepositsByDay.DataBind();
            pnNoData_DetailDepositByDay.Visible = false;
        }
        else
        {
            pnNoData_DetailDepositByDay.Visible = true;
        }
    }
    protected void rptDetailOfDepositsByDay_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Footer)
        {
            DataTable data = DSDepositsByDatSummary;
            if (data != null && data.Rows.Count > 0)
            {
                ((Literal)e.Item.FindControl("ltTotalDODBD_Sales")).Text = FormatData.FormatCurrency(data.Rows[0]["Sales"], SessionManager.CurrencyFortmat);
                ((Literal)e.Item.FindControl("ltTotalDODBD_Refunds")).Text = FormatData.FormatCurrency(data.Rows[0]["Credits"], SessionManager.CurrencyFortmat);
                ((Literal)e.Item.FindControl("ltTotalDODBD_TotalDeposit")).Text = FormatData.FormatCurrency(data.Rows[0]["NetDeposit"], SessionManager.CurrencyFortmat);
                ((Literal)e.Item.FindControl("ltTotalDODBD_NumberOfTrans")).Text = VeraCodeSolution.DoVeraCode(data.Rows[0]["Items"].ToString());
            }
        }
    }
    private DataTable _depositsByDaySummary;
    private DataTable DSDepositsByDatSummary
    {
        get
        {
            if (_depositsByDaySummary != null && _depositsByDaySummary.Rows.Count > 0)
            {
                return _depositsByDaySummary;
            }
            else
            {
                _depositsByDaySummary = GetDetailOfDepositsByDay(true);
                return _depositsByDaySummary;
            }
        }
    }

    // Monthly Fees and Promotions
    private DataTable _monthlyFeesAndPromotions;
    private DataTable DSMonthlyFeesAndPromotions
    {
        get
        {
            if (_monthlyFeesAndPromotions != null && _monthlyFeesAndPromotions.Rows.Count > 0)
            {
                return _monthlyFeesAndPromotions;
            }
            else
            {
                _monthlyFeesAndPromotions = GetCupcakeFeesByReportType((int)ReportType.MonthlyPromotion);
                return _monthlyFeesAndPromotions;
            }
        }
    }
    protected void BindMonthlyFeesAndPromotions()
    {
        DataTable data = DSMonthlyFeesAndPromotions;
        if (data != null && data.Rows.Count > 0)
        {
            rptMonthlyFeesAndPromotion.DataSource = data;
            rptMonthlyFeesAndPromotion.DataBind();
            pnNoData_MonthlyFeesPromotions.Visible = false;
        }
        else
        {
            pnNoData_MonthlyFeesPromotions.Visible = true;
        }
    }

    protected void rptMonthlyFeesAndPromotion_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Footer)
        {
            DataTable data = DSMonthlyFeesAndPromotions;
            if (data != null && data.Rows.Count > 0)
            {
                ((Literal)e.Item.FindControl("ltMonthlyFeeAndPromotions_TT")).Text = FormatData.FormatCurrency(data.Rows[0]["SumOfTotal"], SessionManager.CurrencyFortmat);
            }
        }
    }


    // Event-Driven Fees
    protected void BindEventDrivenFees()
    {
        DataTable data = DSEventDrivenFee;
        if (data != null && data.Rows.Count > 0)
        {
            rptEventDrivenFees.DataSource = data;
            rptEventDrivenFees.DataBind();
            pnNoData_EventDrivenFees.Visible = false;
        }
        else
        {
            pnNoData_EventDrivenFees.Visible = true;
        }
    }
    protected void rptEventDrivenFees_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Footer)
        {
            DataTable data = DSEventDrivenFee;
            if (data != null && data.Rows.Count > 0)
            {
                ((Literal)e.Item.FindControl("ltEventDrivenFees_TTCharge")).Text = VeraCodeSolution.DoVeraCode(FormatData.FormatCurrency(data.Rows[0]["SumOfTotalCharge"]));
            }
        }
    }

    // Monthly statement
    protected void BindMonthlyStatement()
    {
        DataTable data = GetMonthlyStatement();
        if (data != null && data.Rows.Count > 0)
        {
            ltTotalDepositsToYourAccount.Text = VeraCodeSolution.DoVeraCode(FormatData.FormatCurrency(data.Rows[0]["NetDeposit"], SessionManager.CurrencyFortmat));
            ltMonthlyFeesAndPromotions.Text = VeraCodeSolution.DoVeraCode(FormatData.FormatCurrency(data.Rows[0]["Amount"], SessionManager.CurrencyFortmat));
            ltProcessingFees.Text = VeraCodeSolution.DoVeraCode(FormatData.FormatCurrency(data.Rows[0]["ProcessingFees"], SessionManager.CurrencyFortmat));
            ltEventFees.Text = VeraCodeSolution.DoVeraCode(FormatData.FormatCurrency(data.Rows[0]["EventFees"], SessionManager.CurrencyFortmat));
            ltTotalFeesBill.Text = VeraCodeSolution.DoVeraCode(FormatData.FormatCurrency(data.Rows[0]["TotalFeeMonthly"], SessionManager.CurrencyFortmat));
        }
        DataTable data1 = DSDepositsByDatSummary;
        if (data1 != null && data1.Rows.Count > 0)
        {
            ltDepositsSale.Text = VeraCodeSolution.DoVeraCode(FormatData.FormatCurrency(data1.Rows[0]["Sales"], SessionManager.CurrencyFortmat));
            ltDepositsRefunds.Text = VeraCodeSolution.DoVeraCode(FormatData.FormatCurrency(data1.Rows[0]["Credits"], SessionManager.CurrencyFortmat));
        }
    }

    // Processing Rate
    protected void BindProcessingRate()
    {
        DataTable data = DSProcessingRate;
        if (data != null && data.Rows.Count > 0)
        {
            rptProcessingRate.DataSource = data;
            rptProcessingRate.DataBind();
            pnNoData_ProcessingRate.Visible = false;
        }
        else
        {
            pnNoData_ProcessingRate.Visible = true;
        }
    }
    protected void rptProcessingRate_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Footer)
        {
            DataTable data = DSProcessingRate;
            if (data != null && data.Rows.Count > 0)
            {
                ((Literal)e.Item.FindControl("ltPRSumOfSale")).Text = FormatData.FormatCurrency(data.Rows[0]["SumOfSales"], SessionManager.CurrencyFortmat);
                ((Literal)e.Item.FindControl("ltPRSumOfRefund")).Text = FormatData.FormatCurrency(data.Rows[0]["SumOfRefund"], SessionManager.CurrencyFortmat);
                ((Literal)e.Item.FindControl("ltPRSumOfTotalFee")).Text = FormatData.FormatCurrency(data.Rows[0]["SumOfTotalFee"], SessionManager.CurrencyFortmat);
                _totalProcessingFees += double.Parse(data.Rows[0]["SumOfTotalFee"].ToString());
            }
        }
    }

    // Transaction Fee
    protected void BindTransactionFee()
    {
        DataTable data = DSTransactionFee;
        if (data != null && data.Rows.Count > 0)
        {
            rptTransactionFees.DataSource = data;
            rptTransactionFees.DataBind();
            pnNoData_TransactionFee.Visible = false;
        }
        else
        {
            pnNoData_TransactionFee.Visible = true;
        }
    }
    protected void rptTransactionFees_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Footer)
        {
            DataTable data = DSTransactionFee;
            if (data != null && data.Rows.Count > 0)
            {
                ((Literal)e.Item.FindControl("ltTFSumOfSale")).Text = FormatData.FormatInteger(data.Rows[0]["SumOfSales"]);
                ((Literal)e.Item.FindControl("ltTFSumOfRefund")).Text = FormatData.FormatInteger(data.Rows[0]["SumOfRefund"]);
                ((Literal)e.Item.FindControl("ltTFSumOfTotalFee")).Text = FormatData.FormatCurrency(data.Rows[0]["SumOfTotalFee"], SessionManager.CurrencyFortmat);
                _totalProcessingFees += double.Parse(data.Rows[0]["SumOfTotalFee"].ToString());
            }
        }
    }

    // Business name
    private DataTable GetMerchantInfomation()
    {
        FilterParameterCollection _Parameters = new FilterParameterCollection();
        _Parameters.AddLoggedInUserReportingParams();
        _Parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
        _Parameters.Add("@ReportDate", ReportDate, DbType.DateTime);
        if (IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_DDA) || IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_DDA_MS))
        {
            _Parameters.Add(new FilterParameter("@IsViewFull", true, DbType.Boolean));
            _Parameters.AddDecryptDataParams("DDANumber");
        }
        return WebServices.CsReportServices.GetReports("spa_stmnt_MerchantInfo", _Parameters);
    }

    // Monthly statement
    protected DataTable GetMonthlyStatement()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();
        parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
        parameters.Add("@ReportDate", ReportDate, DbType.DateTime);
        string spaName = "spa_stmnt_GetCupcakeMonthlyStatement_TOTAL";
        return WebServices.CsReportServices.GetReports(spaName, parameters);
    }

    // In the news
    private DataTable GetStatementMessage()
    {
        FilterParameterCollection _Parameters = new FilterParameterCollection();
        _Parameters.AddLoggedInUserReportingParams();
        _Parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
        _Parameters.Add("@ReportDate", ReportDate, DbType.DateTime);
        return WebServices.CsReportServices.GetReports("spa_stmnt_GetMessage", _Parameters);
    }

    // Detail of deposits by day
    private DataTable GetDetailOfDepositsByDay(bool isFooter)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();
        parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
        parameters.Add("@ReportDate", ReportDate, DbType.DateTime);
        string spaName = string.Empty;
        if (!isFooter)
        {
            spaName = "spa_stmnt_deposits";
        }
        else
        {
            spaName = "spa_stmnt_deposits_Total";
        }
        return WebServices.CsReportServices.GetReports(spaName, parameters);
    }

    // Monthly Fees and Promotions
    private DataTable GetCupcakeFeesByReportType(int reportType)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();
        parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
        parameters.Add("@ReportDate", ReportDate, DbType.DateTime);
        parameters.Add("@ReportType", reportType, DbType.Int16);
        string spaName = "spa_stmnt_GetCupcakeFeesByReportType_TOTAL";
        return WebServices.CsReportServices.GetReports(spaName, parameters);
    }

    // Event Driven Fee
    private DataTable _eventDrivenFee;
    private DataTable DSEventDrivenFee
    {
        get
        {
            if (_eventDrivenFee != null && _eventDrivenFee.Rows.Count > 0)
            {
                return _eventDrivenFee;
            }
            else
            {
                _eventDrivenFee = GetCupcakeFeesByReportType((int)ReportType.Event);
                return _eventDrivenFee;
            }
        }
    }

    // Processing Rate
    private DataTable _dsProcessingRate = null;
    private DataTable DSProcessingRate
    {
        get
        {
            if (_dsProcessingRate != null && _dsProcessingRate.Rows.Count > 0)
            {
                return _dsProcessingRate;
            }
            else
            {
                FilterParameterCollection parameters = new FilterParameterCollection();
                parameters.AddLoggedInUserReportingParams();
                parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
                parameters.Add("@ReportDate", ReportDate, DbType.DateTime);
                string spaName = "spa_stmnt_GetProcessingRate_TOTAL";
                DataTable data = WebServices.CsReportServices.GetReports(spaName, parameters);
                _dsProcessingRate = data;
                return _dsProcessingRate;
            }
        }
    }

    // Transaction Fee
    private DataTable _dsTransactionFee = null;
    private DataTable DSTransactionFee
    {
        get
        {
            if (_dsTransactionFee != null && _dsTransactionFee.Rows.Count > 0)
            {
                return _dsTransactionFee;
            }
            else
            {
                FilterParameterCollection parameters = new FilterParameterCollection();
                parameters.AddLoggedInUserReportingParams();
                parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
                parameters.Add("@ReportDate", ReportDate, DbType.DateTime);
                string spaName = "spa_stmnt_GetTransactionFee_TOTAL";
                DataTable data = WebServices.CsReportServices.GetReports(spaName, parameters);
                _dsTransactionFee = data;
                return _dsTransactionFee;
            }
        }
    }

    #region Format data
    protected string FormatCurrency(object data)
    {
        if (data != null && !string.IsNullOrEmpty(data.ToString()))
            return FormatData.FormatCurrency(data, SessionManager.CurrencyFortmat);
        return string.Empty;
    }
    protected string FormatInteger(object data)
    {
        if (data != null && !string.IsNullOrEmpty(data.ToString()))
            return FormatData.FormatInteger(data);
        return string.Empty;
    }
    protected string FormatDate(object data)
    {
        if (data is DateTime)
        {
            return (DateTime.Parse(data.ToString())).ToString("MM/dd");
        }
        return data.ToString();
    }
    protected string FormatPercent(object data)
    {
        if (data != null && !string.IsNullOrEmpty(data.ToString()))
            return GeneralFuncsLib.FormatPercent(data);
        return string.Empty;
    }
    #endregion

    #region PDF Export

    /// <summary>
    /// Fixed height: H of title + header + 1 record
    /// </summary>
    private int FixedGridHeight = 140;

    /// <summary>
    /// Grid view style header: center
    /// </summary>
    private string headerStyleCenter = "<p align=\"center\"><font color=\"white\" size=\"2\">{0}</font></p>";

    /// <summary>
    /// Grid view style header: center
    /// </summary>
    private string headerStyleRight = "<p align=\"right\"><font color=\"white\" size=\"2\">{0}</font></p>";

    /// <summary>
    /// Grid view style header: left
    /// </summary>
    private string headerStyleLeft = "<p align=\"left\"><font color=\"white\" size=\"2\">{0}</font></p>";

    /// <summary>
    /// Grid view total style header: center - color 0 0 0 
    /// </summary>
    private string headerTotalStyleCenter = "<p align=\"center\"><b><font size=\"2\">{0}</font></b></p>";

    /// <summary>
    /// Grid view total style header: right - color 0 0 0 
    /// </summary>
    private string headerTotalStyleRight1 = "<p align=\"right\"><b><font size=\"2\">{0}</font></b></p>";

    /// <summary>
    /// Grid view total style header: right - color 255 255 255
    /// </summary>
    private string headerTotalStyleRight2 = "<p align=\"right\"><font color=\"white\" size=\"2\">{0}</font></p>";

    /// <summary>
    /// Grid view style data: center
    /// </summary>
    private string dataStyleCenter = "<p align=\"center\"><font size=\"2\">{0}</font></p>";

    /// <summary>
    /// Grid view style data: left
    /// </summary>
    private string dataStyleLeft = "<p align=\"left\"><font size=\"2\">{0}</font></p>";

    /// <summary>
    /// Grid view style data: right
    /// </summary>
    private string dataStyleRight = "<p align=\"right\"><font size=\"2\">{0}</font></p>";

    /// <summary>
    /// Grid view style data: money $0:#,##0.00
    /// </summary>
    private string dataStyleCurrency1 = string.Format("<p align=\"right\"><font size=\"2\">{0}{0:#,##0.00}</font></p>", SessionManager.CurrencySymbol);

    /// <summary>
    /// Grid view style data: money -$0:#,00 <b></b>
    /// </summary>
    private string dataStyleNegativeCurrency1 = string.Format("<p align=\"right\"><font size=\"2\" color=\"#FF0000\">({0}{0:#,##0.00})</font></p>", SessionManager.CurrencySymbol);

    /// <summary>
    /// Grid view style data: money $0:#,##0.00
    /// </summary>
    private string dataStyleCurrency2 = string.Format("<p align=\"right\"><b><font size=\"2\">{0}{0:#,##0.00}</font></b></p>", SessionManager.CurrencySymbol);

    /// <summary>
    /// Grid view style data: money -$0:#,00 <b></b>
    /// </summary>
    private string dataStyleNegativeCurrency2 = string.Format("<p align=\"right\"><b><font size=\"2\" color=\"#FF0000\">({0}{0:#,##0.00})</font></b></p>", SessionManager.CurrencySymbol);

    /// <summary>
    /// Grid view style data: number
    /// </summary>
    private string dataStyleNumberRight = "<p align=\"right\"><font size=\"2\">{0:0}</font></p>";

    /// <summary>
    /// Grid view style data: number <b></b>
    /// </summary>
    private string dataStyleNumberRight1 = "<p align=\"right\"><b><font size=\"2\">{0:0}</font></b></p>";

    /// <summary>
    /// Grid view style data: datetime MM/dd
    /// </summary>
    private string dataStyleDateTime = "<p align=\"center\"><font size=\"2\">{0:MM/dd}</font></p>";

    /// <summary>
    /// Grid view style data: rate %
    /// </summary>
    private string dataStyleRate = "<p align=\"right\"><font size=\"2\">{0:0.00}%</font></p>";

    /// <summary>
    /// Grid view title style.
    /// </summary>
    private string titleStyle = "<p><font size='4' color='#FFFFFF'>&nbsp;&nbsp;<b>{0}</b></font></p>";

    /// <summary>
    /// Grid view title style.
    /// </summary>
    private string subTitleStyle = "<p><font color='#0067B6' size='4'>&nbsp;&nbsp;<b>{0}</b></font></p>";

    /// <summary>
    /// Display format currency negative.
    /// </summary>
    private string CurrencyNagativeStyle = string.Format("<span style='color:red;'>({0}{0:#,##0.00})</span>", SessionManager.CurrencySymbol);

    /// <summary>
    /// Display format currency.
    /// </summary>
    private string CurrencyStyle = string.Format("<span>{0}{0:#,##0.00}</span>", SessionManager.CurrencySymbol);

    /// <summary>
    /// Grid view title padding.
    /// </summary>
    private int titlePadding = 7;

    /// <summary>
    /// Grid view background color: dark gray - #818386
    /// </summary>
    /// 
    private string headerBgr = "129, 131, 134";

    /// <summary>
    /// Grid view row color: light gray - #E0E1E3
    /// </summary>
    private string rowBgrColor = "224, 225, 227";

    /// <summary>
    /// Grid view title grid background color: dark blue - #0060A9
    /// </summary>
    private string titleBgrColor = "0, 96, 169";

    /// <summary>
    /// Grid view sub title grid background color: #FFFFFF
    /// </summary>
    private string subTitleBgrColor = "255, 255, 255";

    /// <summary>
    /// Do export PDF
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">EventArgs</param>
    protected void uxExportPDF_Click(object sender, ImageClickEventArgs e)
    {
        MerchantNumber = SecureQueryString["MerchantNumber"];
        ReportDate = new DateTime(long.Parse(SecureQueryString["ReportDate"]));

        PdfFactory pdfCreator = new PdfFactory();
        pdfCreator.SetPageSize(612, 792);
        pdfCreator.SetHeaderSize(64);
        pdfCreator.SetPageMargin(36);
        pdfCreator.CreateHeader += new PdfFactory.PdfFactoryHanlder(pdfCreator_CreateHeader);
        pdfCreator.CreateContent += new PdfFactory.PdfFactoryHanlder(pdfCreator_CreateContent);
        pdfCreator.CreateFooter += new PdfFactory.PdfFactoryHanlder(pdfCreator_CreateFooter);
        pdfCreator.Create(Response.OutputStream);

        string trueFileName = "StatementDetails";
        Response.AppendHeader("content-disposition", "attachment; filename=" + trueFileName + ".pdf");
        Response.ContentType = "application/pdf";
        Response.Flush();
        Response.End();
    }

    /// <summary>
    /// Create PDF Header
    /// </summary>
    /// <param name="pdfAgent">PdfAgent</param>
    /// <param name="rect">XRect</param>
    private void pdfCreator_CreateHeader(PdfFactory.PdfAgent pdfAgent, XRect rect)
    {
        Doc pdfDoc = pdfAgent.PdfDoc;
        pdfDoc.Units = WebSupergoo.ABCpdf9.UnitType.Points;
        pdfDoc.HtmlOptions.Engine = EngineType.Gecko;
        pdfDoc.FontSize = 8;
        pdfDoc.Font = pdfDoc.AddFont("Arial");
        pdfDoc.HtmlOptions.UseNoCache = true;
        pdfDoc.HtmlOptions.PageCacheClear();
        pdfDoc.HtmlOptions.FontEmbed = true;
        pdfDoc.HtmlOptions.FontSubstitute = false;
        pdfDoc.HtmlOptions.FontProtection = false;
        pdfDoc.HtmlOptions.UseScript = true;
        pdfDoc.Rendering.DotsPerInch = 72;

        string templateFile = HttpContext.Current.Server.MapPath("~/App_Data/PDF_Header_TOTAL.htm");

        string domain = string.Format("file:///{0}", Server.MapPath("~/"));  // ConfigurationManager.AppSettings["VisionWeb_UrlByIP"];
        uxImageLogo.Src = String.Format("{0}{1}", domain, "/App_Themes/TOTAL/img/logo_Gbig.png");

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        StringWriter sw = new StringWriter(sb);
        HtmlTextWriter hWriter = new HtmlTextWriter(sw);
        Logo.Visible = true;
        Logo.RenderControl(hWriter);
        Logo.Visible = false;

        StringBuilder customContent = new StringBuilder(File.ReadAllText(templateFile, Encoding.GetEncoding(1252)));
        customContent.Replace("[Logo]", sb.ToSafeString());
        customContent.Replace("[_Page_]", GetLocalResourceObject("pdf_template_Page").ToString());
        customContent.Replace("[_of_]", GetLocalResourceObject("pdf_template_of").ToString());
        customContent.Replace("[_MerchantID_]", GetLocalResourceObject("pdf_template_MerchantID").ToString());
        customContent.Replace("[_StatementDate_]", GetLocalResourceObject("pdf_template_StatementDate").ToString());
        customContent.Replace("[_Questions_]", GetLocalResourceObject("pdf_template_Question").ToString());
        ReplaceDataHeader(customContent, pdfDoc);
        pdfDoc.AddImageHtml(customContent.ToSafeString());
    }

    /// <summary>
    /// Create PDF main content
    /// </summary>
    /// <param name="pdfAgent">PdfAgent</param>
    /// <param name="rect">XRect</param>
    private void pdfCreator_CreateContent(PdfFactory.PdfAgent pdfAgent, XRect rect)
    {
        //Init document.
        Doc pdfDoc = pdfAgent.PdfDoc;
        pdfDoc.FontSize = 8;
        pdfDoc.Font = pdfDoc.AddFont("Arial");
        pdfDoc.Units = WebSupergoo.ABCpdf9.UnitType.Points;
        pdfDoc.HtmlOptions.Engine = EngineType.Gecko;
        pdfDoc.HtmlOptions.UseNoCache = true;
        pdfDoc.HtmlOptions.PageCacheClear();
        pdfDoc.HtmlOptions.FontEmbed = true;
        pdfDoc.HtmlOptions.FontProtection = false;
        pdfDoc.HtmlOptions.FontSubstitute = true;
        pdfDoc.HtmlOptions.UseScript = true;
        pdfDoc.Rendering.DotsPerInch = 72;

        // The main page.
        string mainPageTemplate = HttpContext.Current.Server.MapPath("~/App_Data/PDF_MainPage_TOTAL.htm");
        StringBuilder mainPageContent = new StringBuilder(File.ReadAllText(mainPageTemplate, Encoding.GetEncoding(1252)));
        mainPageContent.Replace("[_MonthlyStatement_]", GetLocalResourceObject("Literal3Resource1.Text").ToString());
        mainPageContent.Replace("[_STATEMENTATAGLANCE_]", GetLocalResourceObject("Literal4Resource1.Text").ToString());
        mainPageContent.Replace("[_DEPOSITS_]", GetLocalResourceObject("Literal5Resource1.Text").ToString());
        mainPageContent.Replace("[_Sales_]", GetLocalResourceObject("Literal6Resource1.Text").ToString());
        mainPageContent.Replace("[_Refunds_]", GetLocalResourceObject("Literal7Resource1.Text").ToString());
        mainPageContent.Replace("[_TotalDepositsToYourAccount_]", GetLocalResourceObject("Literal8Resource1.Text").ToString());
        mainPageContent.Replace("[_BILLEDTOYOURACCOUNT_]", GetLocalResourceObject("Literal9Resource1.Text").ToString());
        mainPageContent.Replace("[_MonthlyFeesandPromotions_]", GetLocalResourceObject("Literal10Resource1.Text").ToString());
        mainPageContent.Replace("[_ProcessingFees_]", GetLocalResourceObject("Literal11Resource1.Text").ToString());
        mainPageContent.Replace("[_EventDrivenFees_]", GetLocalResourceObject("Literal12Resource1.Text").ToString());
        mainPageContent.Replace("[_TotalProcessingFeeBilledToYourAccount_]", GetLocalResourceObject("Literal13Resource1.Text").ToString());
        mainPageContent.Replace("[_INTHENEWS_]", GetLocalResourceObject("Literal14Resource1.Text").ToString());

        // Replace data for main page content.
        var merchantInfo = GetMerchantInfomation();
        var monthlyStatement = GetMonthlyStatement();
        var statementMessage = GetStatementMessage();
        var depositsTotal = DSDepositsByDatSummary;


        ReplaceDataMainPage(mainPageContent, merchantInfo, monthlyStatement, statementMessage, depositsTotal);
        pdfAgent.PdfDoc.Pos.Y += 1;
        pdfAgent.AddBoxHtml("", titleBgrColor, 1, pdfDoc.Rect.Width, 0, 0);
        pdfAgent.NewLine();
        pdfDoc.AddImageHtml(mainPageContent.ToSafeString());

        // The detail 1 page.
        pdfAgent.NewPage();
        pdfAgent.NewLine();
        string pdf = string.Empty;

        // DETAIL OF DEPOSITS BY DAY
        var detailDeposit = GetDetailOfDepositsByDay(false);
        if (detailDeposit != null)
        {
            // Add title
            pdf = string.Format(titleStyle, GetLocalResourceObject("Literal15Resource1.Text").ToString());
            pdfAgent.AddTextTitle(pdf, titleBgrColor, titlePadding);

            pdfAgent.AddTable(new PdfFactory.TableSettings() { Name = "DetailDeposit", Border = PdfFactory.TableSettings.BorderType.Empty, RowColor = rowBgrColor },
                    new PdfFactory.ColumnSettings[]{
                new PdfFactory.ColumnSettings(){ HeaderText = string.Format(headerStyleCenter, GetLocalResourceObject("StatementDetails_TOTAL_aspx_cs_Date").ToString()), DataField = "Day", Width = 1, FormatString = dataStyleDateTime, HeaderBgColor = headerBgr },
                new PdfFactory.ColumnSettings(){ HeaderText = string.Format(headerStyleCenter, GetLocalResourceObject("Literal16Resource1.Text").ToString()), Width = 1, DataField = "ReferenceNumber", FormatString = dataStyleCenter, HeaderBgColor = headerBgr },
                new PdfFactory.ColumnSettings(){ HeaderText = string.Format(headerStyleCenter, GetLocalResourceObject("Literal17Resource1.Text").ToString()), DataField = "Items", Width = 1, FormatString = dataStyleCenter, HeaderBgColor = headerBgr},
                new PdfFactory.ColumnSettings(){ HeaderText = string.Format(headerStyleRight, GetLocalResourceObject("Literal18Resource1.Text").ToString()), DataField = "Sales", Width = 1, FormatString = dataStyleCurrency1, HeaderBgColor = headerBgr, NegativeNumberFormatString = dataStyleNegativeCurrency1 },
                new PdfFactory.ColumnSettings(){ HeaderText = string.Format(headerStyleRight, GetLocalResourceObject("Literal19Resource1.Text").ToString()), DataField = "Credits", Width = 1, FormatString = dataStyleCurrency1, HeaderBgColor = headerBgr, NegativeNumberFormatString = dataStyleNegativeCurrency1 },
                new PdfFactory.ColumnSettings(){ HeaderText = string.Format(headerStyleRight, GetLocalResourceObject("Literal20Resource1.Text").ToString()), DataField = "NetDeposit", Width = 1, FormatString = dataStyleCurrency1, HeaderBgColor = headerBgr, NegativeNumberFormatString = dataStyleNegativeCurrency1 },

            }, detailDeposit, FormatCell, 5, false);
        }

        DataTable tempData = null;

        if (detailDeposit != null && depositsTotal != null && depositsTotal.Rows.Count > 0)
        {
            tempData = detailDeposit.Clone();
            pdfAgent.PdfDoc.Pos.Y += 5;
            pdfAgent.AddBoxHtml("", titleBgrColor, 1, pdfDoc.Rect.Width, 0, 0);
            pdfAgent.AddTable(new PdfFactory.TableSettings() { Name = "TotalDeposit", Border = PdfFactory.TableSettings.BorderType.Empty, RowColor = rowBgrColor },
                    new PdfFactory.ColumnSettings[]{
                new PdfFactory.ColumnSettings(){ HeaderText = string.Format(headerTotalStyleRight1,GetLocalResourceObject("text_TotalSales_Refunds_capitalize").ToString()), Width = 2 },
                new PdfFactory.ColumnSettings(){ HeaderText = string.Empty, Width=0 },
                new PdfFactory.ColumnSettings(){ HeaderText = string.Format(headerTotalStyleCenter, depositsTotal.Rows[0]["Items"].ToSafeString()), Width=1 },
                new PdfFactory.ColumnSettings(){ HeaderText = FormatDataGridCurrency2(depositsTotal.Rows[0]["Sales"].ToSafeString()), Width = 1 },
                new PdfFactory.ColumnSettings(){ HeaderText = FormatDataGridCurrency2(depositsTotal.Rows[0]["Credits"].ToSafeString()), Width = 1 },
                new PdfFactory.ColumnSettings(){ HeaderText = FormatDataGridCurrency2(depositsTotal.Rows[0]["NetDeposit"].ToSafeString()), Width = 1 },

            }, tempData, FormatCell, 5, true);
        }

        pdfAgent.NewLine(4);

        if (pdfAgent.PdfDoc.Pos.Y <= FixedGridHeight)
        {
            pdfAgent.NewPage();
            pdfAgent.NewLine();
        }

        // MONTHLY FEES AND PROMOTIONS
        var monthlyFeePro = DSMonthlyFeesAndPromotions;

        if (monthlyFeePro != null)
        {
            // Add title
            pdf = string.Format(titleStyle, GetLocalResourceObject("Literal22Resource1.Text").ToString());
            pdfAgent.AddTextTitle(pdf, titleBgrColor, titlePadding);

            pdfAgent.AddTable(new PdfFactory.TableSettings() { Name = "MonthlyFPDetail", Border = PdfFactory.TableSettings.BorderType.Empty, RowColor = rowBgrColor },
                    new PdfFactory.ColumnSettings[]{
                new PdfFactory.ColumnSettings(){ HeaderText = string.Format(headerStyleLeft, GetLocalResourceObject("text_fee_description_capitallize").ToString()), DataField = "FeeDescription", Width = 61, FormatString = dataStyleLeft, HeaderBgColor = headerBgr},
                new PdfFactory.ColumnSettings(){ HeaderText = string.Format(headerStyleCenter, GetLocalResourceObject("text_qty_capitallize").ToString()), DataField = "Count", Width = 13, FormatString = dataStyleCenter, HeaderBgColor = headerBgr},
                new PdfFactory.ColumnSettings(){ HeaderText = string.Format(headerStyleRight, GetLocalResourceObject("text_itemfee_capitallize").ToString()), DataField = "ItemFee", Width = 13, FormatString = dataStyleCurrency1, HeaderBgColor = headerBgr, NegativeNumberFormatString = dataStyleNegativeCurrency1},
                new PdfFactory.ColumnSettings(){ HeaderText = string.Format(headerStyleRight, GetLocalResourceObject("text_totalfee_capitallize").ToString()), DataField = "Total", Width = 13, FormatString = dataStyleCurrency1, HeaderBgColor = headerBgr, NegativeNumberFormatString = dataStyleNegativeCurrency1},

            }, monthlyFeePro, FormatCell, 8, false);
        }

        if (monthlyFeePro != null && monthlyFeePro.Rows.Count > 0)
        {
            tempData = monthlyFeePro.Clone();
            pdfAgent.PdfDoc.Pos.Y += 5;
            pdfAgent.AddBoxHtml("", titleBgrColor, 1, pdfDoc.Rect.Width, 0, 0);
            pdfAgent.AddTable(new PdfFactory.TableSettings() { Name = "MonthlyFPTotal", Border = PdfFactory.TableSettings.BorderType.Empty, RowColor = rowBgrColor },
                    new PdfFactory.ColumnSettings[]{
                new PdfFactory.ColumnSettings(){ HeaderText = string.Format(headerTotalStyleRight1,GetLocalResourceObject("text_total_capitallize").ToString()), Width = 87 },
                new PdfFactory.ColumnSettings(){ HeaderText = string.Empty, Width = 0 },
                new PdfFactory.ColumnSettings(){ HeaderText = string.Empty, Width = 0 },
                new PdfFactory.ColumnSettings(){ HeaderText = FormatDataGridCurrency2(monthlyFeePro.Rows[0]["SumOfTotal"].ToSafeString()), Width = 13 },

            }, tempData, FormatCell, 8, true);
        }

        // The Detail 2 page.
        pdfAgent.NewPage();
        pdfAgent.NewLine();
        pdf = string.Empty;

        // Section PPROCESSING FEES
        // PROCESSING RATE
        var processingRate = DSProcessingRate;

        if (processingRate != null)
        {
            // Add title
            pdf = string.Format(titleStyle, GetLocalResourceObject("Literal28Resource1.Text").ToString());
            pdfAgent.AddTextTitle(pdf, titleBgrColor, titlePadding);
            pdfAgent.NewLine(1);

            pdf = string.Format(subTitleStyle, GetLocalResourceObject("Literal34Resource1.Text").ToString());
            pdfAgent.AddTextTitle(pdf, subTitleBgrColor, titlePadding);

            pdfAgent.AddTable(new PdfFactory.TableSettings() { Name = "ProcessingRateDetail", Border = PdfFactory.TableSettings.BorderType.Empty, RowColor = rowBgrColor },
                    new PdfFactory.ColumnSettings[]{
                new PdfFactory.ColumnSettings(){ HeaderText = string.Format(headerStyleLeft, GetLocalResourceObject("text_cardtype_capitallize").ToString()), DataField = "CardType", Width = 4, FormatString = dataStyleLeft, HeaderBgColor=headerBgr},
                new PdfFactory.ColumnSettings(){ HeaderText = string.Format(headerStyleRight, GetLocalResourceObject("text_rate_capitallize").ToString()), DataField = "Rate", Width = 1.5, FormatString = dataStyleRate, HeaderBgColor=headerBgr},
                new PdfFactory.ColumnSettings(){ HeaderText = string.Format(headerStyleRight, GetLocalResourceObject("text_sale_capitallize").ToString()), DataField = "Sales", Width = 1.5, FormatString = dataStyleCurrency1, HeaderBgColor = headerBgr, NegativeNumberFormatString = dataStyleNegativeCurrency1},
                new PdfFactory.ColumnSettings(){ HeaderText = string.Format(headerStyleRight, GetLocalResourceObject("text_refund_capitallize").ToString()), DataField = "Refund", Width = 1.5, FormatString = dataStyleCurrency1, HeaderBgColor = headerBgr, NegativeNumberFormatString = dataStyleNegativeCurrency1},
                new PdfFactory.ColumnSettings(){ HeaderText = string.Format(headerStyleRight, GetLocalResourceObject("text_totalfee_capitallize").ToString()), DataField = "TotalFee", Width = 1.5, FormatString = dataStyleCurrency1, HeaderBgColor = headerBgr, NegativeNumberFormatString = dataStyleNegativeCurrency1},

            }, processingRate, FormatCell, 8, false);
        }

        if (processingRate != null && processingRate.Rows.Count > 0)
        {
            tempData = processingRate.Clone();
            pdfAgent.PdfDoc.Pos.Y += 5;
            pdfAgent.AddBoxHtml("", titleBgrColor, 1, pdfDoc.Rect.Width, 0, 0);
            pdfAgent.AddTable(new PdfFactory.TableSettings() { Name = "ProcessingRateTotal", Border = PdfFactory.TableSettings.BorderType.Empty, RowColor = rowBgrColor },
                    new PdfFactory.ColumnSettings[]{
                new PdfFactory.ColumnSettings(){ HeaderText = string.Format(headerTotalStyleRight1,GetLocalResourceObject("text_total_capitallize").ToString()), Width = 5.5 },
                new PdfFactory.ColumnSettings(){ HeaderText = string.Empty, Width=0 },
                new PdfFactory.ColumnSettings(){ HeaderText = FormatDataGridCurrency2(processingRate.Rows[0]["SumOfSales"].ToSafeString()), Width = 1.5 },
                new PdfFactory.ColumnSettings(){ HeaderText = FormatDataGridCurrency2(processingRate.Rows[0]["SumOfRefund"].ToSafeString()), Width = 1.5 },
                new PdfFactory.ColumnSettings(){ HeaderText = FormatDataGridCurrency2(processingRate.Rows[0]["SumOfTotalFee"].ToSafeString()), Width = 1.5 },

            }, tempData, FormatCell, 8, true);
        }

        if (pdfAgent.PdfDoc.Pos.Y <= FixedGridHeight)
        {
            pdfAgent.NewPage();
            pdfAgent.NewLine();
        }

        // TRANSACTION FEE
        var transactionFee = DSTransactionFee;

        if (transactionFee != null)
        {
            // Add title
            pdfAgent.NewLine(2);
            pdf = string.Format(subTitleStyle, GetLocalResourceObject("Literal36Resource1.Text").ToString());
            pdfAgent.AddTextTitle(pdf, subTitleBgrColor, titlePadding);
            pdfAgent.PdfDoc.Pos.Y += 2;
            pdfAgent.AddBoxHtml("", titleBgrColor, 1, pdfDoc.Rect.Width, 0, -26);
            pdfAgent.AddTable(new PdfFactory.TableSettings() { Name = "TransactionRateDetail", Border = PdfFactory.TableSettings.BorderType.Empty, RowColor = rowBgrColor },
                    new PdfFactory.ColumnSettings[]{
                new PdfFactory.ColumnSettings(){ HeaderText = string.Format(headerStyleLeft, GetLocalResourceObject("text_cardtype_capitallize").ToString()), DataField = "CardType", Width = 4, FormatString = dataStyleLeft, HeaderBgColor = headerBgr},
                new PdfFactory.ColumnSettings(){ HeaderText = string.Format(headerStyleRight, GetLocalResourceObject("text_rate_capitallize").ToString()), DataField = "Rate", Width = 1.5, FormatString = dataStyleCurrency1, HeaderBgColor = headerBgr, NegativeNumberFormatString = dataStyleNegativeCurrency1},
                new PdfFactory.ColumnSettings(){ HeaderText = string.Format(headerStyleRight, GetLocalResourceObject("text_sale_capitallize").ToString()), DataField = "Sales", Width = 1.5, FormatString = dataStyleNumberRight, HeaderBgColor = headerBgr},
                new PdfFactory.ColumnSettings(){ HeaderText = string.Format(headerStyleRight, GetLocalResourceObject("text_refund_capitallize").ToString()), DataField = "Refund", Width = 1.5, FormatString = dataStyleNumberRight, HeaderBgColor = headerBgr},
                new PdfFactory.ColumnSettings(){ HeaderText = string.Format(headerStyleRight, GetLocalResourceObject("text_totalfee_capitallize").ToString()), DataField = "TotalFee", Width = 1.5, FormatString = dataStyleCurrency1, HeaderBgColor = headerBgr, NegativeNumberFormatString = dataStyleNegativeCurrency1},

            }, transactionFee, FormatCell, 8, false);
        }

        if (transactionFee != null && transactionFee.Rows.Count > 0)
        {
            tempData = transactionFee.Clone();
            pdfAgent.PdfDoc.Pos.Y += 5;
            pdfAgent.AddBoxHtml("", titleBgrColor, 1, pdfDoc.Rect.Width, 0, 0);
            pdfAgent.AddTable(new PdfFactory.TableSettings() { Name = "ProcessingRateTotal", Border = PdfFactory.TableSettings.BorderType.Empty, RowColor = rowBgrColor },
                    new PdfFactory.ColumnSettings[]{
                new PdfFactory.ColumnSettings(){ HeaderText = string.Format(headerTotalStyleRight1,GetLocalResourceObject("text_total_capitallize").ToString()), Width = 5.5 },
                new PdfFactory.ColumnSettings(){ HeaderText = string.Empty, Width = 0 },
                new PdfFactory.ColumnSettings(){ HeaderText = string.Format(dataStyleNumberRight1, transactionFee.Rows[0]["SumOfSales"].ToSafeDecimal()), Width = 1.5 },
                new PdfFactory.ColumnSettings(){ HeaderText = string.Format(dataStyleNumberRight1, transactionFee.Rows[0]["SumOfRefund"].ToSafeDecimal()), Width = 1.5 },
                new PdfFactory.ColumnSettings(){ HeaderText = FormatDataGridCurrency2(transactionFee.Rows[0]["SumOfTotalFee"].ToSafeString()), Width = 1.5 },

            }, tempData, FormatCell, 8, true);
        }

        //Processing Fees
        pdfAgent.PdfDoc.Pos.Y += 1;
        pdfAgent.AddBoxHtml("", titleBgrColor, 1, pdfDoc.Rect.Width, 0, 0);
        pdfAgent.AddTable(new PdfFactory.TableSettings() { Name = "TotalFees", Border = PdfFactory.TableSettings.BorderType.Empty, RowColor = rowBgrColor },
                new PdfFactory.ColumnSettings[]{
            new PdfFactory.ColumnSettings(){ HeaderText = string.Empty, Width=4 },
            new PdfFactory.ColumnSettings(){ HeaderText = string.Empty, Width=1.5 },
            new PdfFactory.ColumnSettings(){ HeaderText = string.Empty, Width=1.5 },
            new PdfFactory.ColumnSettings(){ HeaderText = string.Empty, Width=1.5 },
            new PdfFactory.ColumnSettings(){ HeaderText = string.Format(headerStyleCenter,GetLocalResourceObject("ltTotalProcFeesResource1.Text").ToString()), Width = 1.5, HeaderBgColor = headerBgr }

        }, tempData, FormatCell, 4, true);
        pdfAgent.PdfDoc.Pos.Y += 3;
        pdfAgent.AddBoxHtml("", titleBgrColor, 1, 81, 459, -2);

        if (processingRate != null && processingRate.Rows.Count > 0)
        {
            _totalProcessingFees += Convert.ToDouble(processingRate.Rows[0]["SumOfTotalFee"].ToSafeString());
        }

        if (transactionFee != null && transactionFee.Rows.Count > 0)
        {
            _totalProcessingFees += Convert.ToDouble(transactionFee.Rows[0]["SumOfTotalFee"].ToSafeString());
        }

        pdfAgent.AddTable(new PdfFactory.TableSettings() { Name = "TotalFeesValue", Border = PdfFactory.TableSettings.BorderType.Empty, RowColor = rowBgrColor },
                new PdfFactory.ColumnSettings[]{
            new PdfFactory.ColumnSettings(){ HeaderText = string.Empty, Width = 4.5 },
            new PdfFactory.ColumnSettings(){ HeaderText = string.Empty, Width = 1.5 },
            new PdfFactory.ColumnSettings(){ HeaderText = string.Empty, Width = 1.5 },
            new PdfFactory.ColumnSettings(){ HeaderText = string.Empty, Width = 1.5 },
            new PdfFactory.ColumnSettings(){ HeaderText = FormatDataGridCurrency2(_totalProcessingFees.ToSafeString()), Width = 1.5 }

        }, tempData, FormatCell, 8, true);

        pdfAgent.NewLine(8);

        if (pdfAgent.PdfDoc.Pos.Y <= FixedGridHeight)
        {
            pdfAgent.NewPage();
            pdfAgent.NewLine();
        }

        // EVENT-DRIVEN FEES

        var eventDrivenFee = DSEventDrivenFee;
        if (eventDrivenFee != null)
        {
            // Add title
            pdf = string.Format(titleStyle, GetLocalResourceObject("ltEventDrivenFeesResource1.Text").ToString());
            pdfAgent.AddTextTitle(pdf, titleBgrColor, titlePadding);

            pdfAgent.AddTable(new PdfFactory.TableSettings() { Name = "EventDrivenFeeDetail", Border = PdfFactory.TableSettings.BorderType.Empty, RowColor = rowBgrColor },
                    new PdfFactory.ColumnSettings[]{
                new PdfFactory.ColumnSettings(){ HeaderText = string.Format(headerStyleLeft,GetLocalResourceObject("text_Type_capitallize").ToString()), DataField = "Type", Width = 44, FormatString = dataStyleLeft, HeaderBgColor = headerBgr},
                new PdfFactory.ColumnSettings(){ HeaderText = string.Format(headerStyleRight,GetLocalResourceObject("text_rate_capitallize").ToString()), DataField = "Rate", Width = 12, FormatString = dataStyleCurrency1, HeaderBgColor = headerBgr, NegativeNumberFormatString = dataStyleNegativeCurrency1},
                new PdfFactory.ColumnSettings(){ HeaderText = string.Format(headerStyleCenter,GetLocalResourceObject("text_qty_capitallize").ToString()), DataField = "Qty", Width = 14, FormatString = dataStyleCenter, HeaderBgColor = headerBgr},
                new PdfFactory.ColumnSettings(){ HeaderText = string.Format(headerStyleRight,GetLocalResourceObject("text_totalcharge_capitallize").ToString()), DataField = "TotalCharge", Width = 18, FormatString = dataStyleCurrency1, HeaderBgColor = headerBgr, NegativeNumberFormatString = dataStyleNegativeCurrency1},

            }, eventDrivenFee, FormatCell, 8, false);
        }

        if (eventDrivenFee != null && eventDrivenFee.Rows.Count > 0)
        {
            tempData = eventDrivenFee.Clone();
            pdfAgent.PdfDoc.Pos.Y += 2;
            pdfAgent.AddBoxHtml("", titleBgrColor, 1, pdfDoc.Rect.Width, 0, -5);
            pdfAgent.AddTable(new PdfFactory.TableSettings() { Name = "EventDrivenFeeTotal", Border = PdfFactory.TableSettings.BorderType.Empty, RowColor = rowBgrColor },
                    new PdfFactory.ColumnSettings[]{
                new PdfFactory.ColumnSettings(){ HeaderText = string.Format(headerTotalStyleRight1, GetLocalResourceObject("text_total_capitallize").ToString()), Width = 82 },
                new PdfFactory.ColumnSettings(){ HeaderText = string.Empty, Width = 0 },
                new PdfFactory.ColumnSettings(){ HeaderText = string.Empty, Width = 0 },
                new PdfFactory.ColumnSettings(){ HeaderText = FormatDataGridCurrency2(eventDrivenFee.Rows[0]["SumOfTotalCharge"].ToSafeString()), Width = 18 },

            }, tempData, FormatCell, 8, true);
        }
    }

    /// <summary>
    /// Create PDF footer
    /// </summary>
    /// <param name="pdfAgent">PdfAgent</param>
    /// <param name="rect">XRect</param>
    private void pdfCreator_CreateFooter(PdfFactory.PdfAgent pdfAgent, XRect rect)
    {
        //Do something
    }

    /// <summary>
    /// Table format cell define
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="rowIndex"></param>
    /// <param name="colIndex"></param>
    /// <param name="colName"></param>
    /// <param name="text"></param>
    /// <param name="dataCell"></param>
    /// <param name="dataIndex"></param>
    /// <returns></returns>
    private string FormatCell(string tableName, int rowIndex, int colIndex, string colName, string text, object dataCell, int dataIndex)
    {
        string returnText = text;
        if (dataCell == DBNull.Value)
        {
            return string.Empty;
        }

        return returnText;
    }

    /// <summary>
    /// Replace all data into header template
    /// </summary>
    /// <param name="headerTemplate">Input template</param>
    private void ReplaceDataHeader(StringBuilder headerTemplate, Doc doc)
    {
        // Client infomation
        if (ContactInformation != null)
        {
            string question = string.Empty;
            if (ContactInformation.Phone.IndexOf("/") > -1)
            {
                question = ContactInformation.Phone.Split('/')[0];
            }
            else
            {
                question = ContactInformation.Phone;
            }
            headerTemplate.Replace("[ClientName]", ContactInformation.ClientName);
            headerTemplate.Replace("[Address1]", ContactInformation.Address1);
            headerTemplate.Replace("[Address2]", ContactInformation.Address2);
            headerTemplate.Replace("[Contact]", ContactInformation.ContactEmail);
            headerTemplate.Replace("[Question]", question);
        }
        else
        {
            headerTemplate.Replace("[ClientName]", string.Empty);
            headerTemplate.Replace("[Address1]", string.Empty);
            headerTemplate.Replace("[Address2]", string.Empty);
            headerTemplate.Replace("[Contact]", string.Empty);
            headerTemplate.Replace("[Question]", string.Empty);
        }

        // Other
        headerTemplate.Replace("[PageNumber]", doc.PageNumber.ToSafeString());
        headerTemplate.Replace("[PageTotal]", doc.PageCount.ToSafeString());
        headerTemplate.Replace("[MerchantNumber]", MerchantNumber);
        headerTemplate.Replace("[StatementPeriod]", new DateTime(long.Parse(SecureQueryString["ReportDate"])).ToString("MMM d, yyyy"));

    }

    /// <summary>
    /// Replace all data for main page
    /// </summary>
    /// <param name="template">Input template</param>
    private void ReplaceDataMainPage(StringBuilder template, DataTable mif, DataTable monthlyStatement, DataTable statementMessage, DataTable depositsTotal)
    {
        // business information
        if (mif != null && mif.Rows.Count > 0)
        {
            // business name
            template.Replace("[BusinessName]", mif.Rows[0]["Name"].ToSafeString());

            // Owner
            var owner = mif.Rows[0]["Attention"].ToSafeString();
            if (!string.IsNullOrEmpty(owner))
            {
                owner = "</br></br>" + owner;
            }
            template.Replace("[Owner]", owner);

            // address 1
            var address1 = mif.Rows[0]["Address1"].ToSafeString();
            if (!string.IsNullOrEmpty(address1))
            {
                address1 = "</br></br>" + address1;
            }
            template.Replace("[Address1]", address1);

            //address 2
            var address2 = mif.Rows[0]["Address2"].ToSafeString();
            if (!string.IsNullOrEmpty(address2))
            {
                address2 = "</br></br>" + address2;
            }
            template.Replace("[Address2]", address2);


            // city + state + zip
            var areaInfo = string.Empty;
            areaInfo += mif.Rows[0]["City"].ToSafeString();

            var state = mif.Rows[0]["State"].ToSafeString();
            if (!string.IsNullOrEmpty(state))
            {
                areaInfo += ", " + state;
            }

            var zip = mif.Rows[0]["Zip"].ToSafeString();
            if (!string.IsNullOrEmpty(zip))
            {
                areaInfo += ", " + zip;
            }
            if (!string.IsNullOrEmpty(areaInfo))
            {
                if (areaInfo.IndexOf(",") == 0)
                {
                    areaInfo = areaInfo.Remove(0, 2);
                }
                areaInfo = "</br></br>" + areaInfo;
            }
            template.Replace("[Area]", areaInfo);

        }
        else
        {
            template.Replace("[BusinessName]", string.Empty);
            template.Replace("[Address1]", string.Empty);
            template.Replace("[Address2]", string.Empty);
            template.Replace("[City]", string.Empty);
            template.Replace("[State]", string.Empty);
            template.Replace("[Zip]", string.Empty);
        }

        // Monthly statement

        if (depositsTotal != null && depositsTotal.Rows.Count > 0)
        {
            template.Replace("[Sales]", FormatDataCurrency(depositsTotal.Rows[0]["Sales"].ToSafeString()));
            template.Replace("[Refunds]", FormatDataCurrency(depositsTotal.Rows[0]["Credits"].ToSafeString()));
        }
        else
        {
            template.Replace("[Sales]", string.Format(CurrencyStyle, 0));
            template.Replace("[Refunds]", string.Format(CurrencyStyle, 0));
        }

        if (monthlyStatement != null && monthlyStatement.Rows.Count > 0)
        {
            template.Replace("[DepositTotal]", FormatDataCurrency(monthlyStatement.Rows[0]["NetDeposit"].ToSafeString()));
            template.Replace("[MonthyFP]", FormatDataCurrency(monthlyStatement.Rows[0]["Amount"].ToSafeString()));
            template.Replace("[ProcessingFees]", FormatDataCurrency(monthlyStatement.Rows[0]["ProcessingFees"].ToSafeString()));
            template.Replace("[EventFees]", FormatDataCurrency(monthlyStatement.Rows[0]["EventFees"].ToSafeString()));
            template.Replace("[TotalFees]", FormatDataCurrency(monthlyStatement.Rows[0]["TotalFeeMonthly"].ToSafeString()));
        }
        else
        {
            template.Replace("[DepositTotal]", string.Format(CurrencyStyle, 0));
            template.Replace("[MonthyFP]", string.Format(CurrencyStyle, 0));
            template.Replace("[ProcessingFees]", string.Format(CurrencyStyle, 0));
            template.Replace("[EventFees]", string.Format(CurrencyStyle, 0));
            template.Replace("[TotalFees]", string.Format(CurrencyStyle, 0));
        }

        // In the news
        if (statementMessage != null && statementMessage.Rows.Count > 0)
        {
            template.Replace("[Message]", statementMessage.Rows[0]["Message"].ToSafeString());
        }
        else
        {
            template.Replace("[Message]", GetLocalResourceObject("Literal37Resource1.Text").ToString());
        }
    }

    /// <summary>
    /// Format display currency on grid view: negative and positive
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private string FormatDataGridCurrency1(string data)
    {
        try
        {
            double value = Convert.ToDouble(data);
            if (value < 0)
            {
                return string.Format(dataStyleNegativeCurrency1, 0 - value);
            }
            else
            {
                return string.Format(dataStyleCurrency1, value);
            }
        }
        catch (Exception e)
        {
            return data;
        }

    }

    /// <summary>
    /// Format display currency on grid view: negative and positive <b></b>
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private string FormatDataGridCurrency2(string data)
    {
        try
        {
            double value = Convert.ToDouble(data);
            if (value < 0)
            {
                return string.Format(dataStyleNegativeCurrency2, 0 - value);
            }
            else
            {
                return string.Format(dataStyleCurrency2, value);
            }
        }
        catch (Exception e)
        {
            return data;
        }

    }

    /// <summary>
    /// Format currency: negative and positive
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private string FormatDataCurrency(string data)
    {
        try
        {
            double value = Convert.ToDouble(data);
            if (value < 0)
            {
                return string.Format(CurrencyNagativeStyle, 0 - value);
            }
            else
            {
                return string.Format(CurrencyStyle, value);
            }
        }
        catch (Exception e)
        {
            return data;
        }

    }

    #endregion
}