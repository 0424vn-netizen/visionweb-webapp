using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AS.Common.DBManager;
using System.Data;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Web.Business;
using System.IO;
using System.Text;
using AS.Common.Formater;
using AS.Common;
using AS.Security.WS.Entities;



[PagePermission("StatementRpt,MSStatementRpt")]
public partial class StatementDetail_ORION : ReportPage
{

    #region Enum
    enum DataBindAction
    {
        BindDeposit,
        BindCardSumary,
        BindSettleDisCount,
        BindSurCharge,
        BindOtherFee
    }

    enum ReportType
    {
        Deposit,
        CardSummary,
        SettleDisCount,
        SurCharge,
        OtherFee,
    }
    #endregion

    #region Properties
    string MerchantNumber = "";
    protected DateTime ReportDate = DateTime.Now;
    protected string HeaderString = "";
    protected string MinBillAdjustment = "0.0";
    protected string TotalAmount = "0.0";
    protected ClientInfo ContactInformation = null;
    #endregion

    protected override void PageInitialize()
    {
        if (IsIntruderDetected) return;

        this.GridIDs.Add("uxDeposits");
        this.GridIDs.Add("uxCardSumary");
        this.GridIDs.Add("uxSettlementDiscount");
        this.GridIDs.Add("uxSurcharge");
        this.GridIDs.Add("uxOtherFee");
        base.PageInitialize();

    }
    private void ProcessQueryString()
    {
        this.MerchantNumber = SecureQueryString["MerchantNumber"];
        string _reportdate = SecureQueryString["ReportDate"];
        this.ReportDate = new DateTime(long.Parse(_reportdate));
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;
        IsBindDataOnLoad = true;
        if (IsSecureQueryString)
        {
            ProcessQueryString();
            DisplayLogo();
            if (!IsPostBack)
            {                
                BindData();
                DoDataBindControls(DataBindAction.BindDeposit, uxDeposits);
                DoDataBindControls(DataBindAction.BindCardSumary, uxCardSumary);
                DoDataBindControls(DataBindAction.BindSettleDisCount, uxSettlementDiscount);
                DoDataBindControls(DataBindAction.BindSurCharge, uxSurcharge);
                DoDataBindControls(DataBindAction.BindOtherFee, uxOtherFee);
            }            
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }

    protected void DisplayLogo()
    {
        string themeName = "Orion";

        if (SessionManager.CurrentUserType != WebSiteEnums.UserHierarchyMode.Merchant)
        {
            ASThemeCollection themes = WebServices.SecurityServices.GetASThemsByUser(SessionManager.CurrentUser.ASClient, MerchantNumber, 0);
            if (themes.Count > 0)
            {
                themeName = themes[0].ThemeName;
            }
        }
        else themeName = SessionManager.CurrentUserTheme.ThemeName;

        string logoUrl = string.Format("<img src='App_Themes/{0}/img/logo.png' alt='logo' />", themeName);
        uxReportTitle.ReportTitle = logoUrl;

        // Bind contact information
        string _ClientContactInfo = "ClientInformation_" + themeName;
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.Add(new FilterParameter("@ASClient", SessionManager.CurrentClient, System.Data.DbType.Int32));
        parameters.Add(new FilterParameter("@ContactName", _ClientContactInfo, System.Data.DbType.AnsiString));
        DataTable clientInfo = WebServices.SecurityServices.GetReports("spa_GetContactUsInfo", parameters);

        if (clientInfo != null && clientInfo.Rows.Count > 0)
        {
            ContactInformation = new ClientInfo();
            ContactInformation.ClientName = clientInfo.Rows[0]["RefTblCol1"].ToString();
            ContactInformation.Address1 = clientInfo.Rows[0]["RefTblCol2"].ToString();
            ContactInformation.Address2 = clientInfo.Rows[0]["RefTblCol3"].ToString();
            ContactInformation.Zip = clientInfo.Rows[0]["RefTblCol4"].ToString();
            ContactInformation.Phone = clientInfo.Rows[0]["RefTblCol5"].ToString();
            ContactInformation.Fax = clientInfo.Rows[0]["RefTblCol6"].ToString();
            ContactInformation.ClientEmail = clientInfo.Rows[0]["RefTblCol7"].ToString();
            ContactInformation.ContactEmail = clientInfo.Rows[0]["RefTblCol8"].ToString();
            ContactInformation.NoReplyEmail = clientInfo.Rows[0]["RefTblCol9"].ToString();
        }
        else ContactInformation = SessionManager.ClientInfo;
    }

    protected void BindData()
    {
        uxRepeaterDepositSumary1.DataSource = GetStatementDetail();
        uxRepeaterDepositSumary1.DataBind();
        DataTable tbl = new DataTable();
        tbl = GetMerchantInfomation();
        uxMerchantInfo.DataSource = tbl;
        uxMerchantInfo.DataBind();

        DataTable tblStmTotal = GetStatementTotal();
        if (tblStmTotal != null && tblStmTotal.Rows.Count > 0)
        {
            MinBillAdjustment = double.Parse(tblStmTotal.Rows[0]["MinBillAdjustment"].ToString()).ToString("#0.00");
            TotalAmount = double.Parse(tblStmTotal.Rows[0]["TotalAmount"].ToString()).ToString("#0.00");
            uxMinBillAdjustment.Text = VeraCodeSolution.DoVeraCode(MinBillAdjustment);
            uxTotalAmount.Text = VeraCodeSolution.DoVeraCode(TotalAmount);
        }
    }

    protected override void DoGridNeedDataSource(AS.Controls.Grid.ASGrid sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        switch (sender.ID)
        {
            case "uxDeposits":
                OnDataBindControls(DataBindAction.BindDeposit, sender);
                break;
            case "uxCardSumary":
                OnDataBindControls(DataBindAction.BindCardSumary, sender);
                break;
            case "uxSettlementDiscount":
                OnDataBindControls(DataBindAction.BindSettleDisCount, sender);
                break;
            case "uxSurcharge":
                OnDataBindControls(DataBindAction.BindSurCharge, sender);
                break;
            case "uxOtherFee":
                OnDataBindControls(DataBindAction.BindOtherFee, sender);
                break;
        }
    }
    protected override void OnDataBindControls(Enum type, object sender)
    {
        ASGrid grid = (ASGrid)sender;
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();
        parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
        parameters.Add("@ReportDate", ReportDate, DbType.DateTime);
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindDeposit:
                {
                    string spaName = "spa_stmnt_deposits";
                    grid.DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(parameters) });
                }
                break;

            case DataBindAction.BindCardSumary:
                {
                    string spaName = "spa_stmnt_GetCardSummary";
                    grid.DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(parameters) });
                }
                break;
            case DataBindAction.BindSettleDisCount:
                {
                    string spaName = "spa_stmnt_GetSettlementDiscount";
                    grid.DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(parameters) });
                }
                break;
            case DataBindAction.BindSurCharge:
                {
                    string spaName = "spa_stmnt_surcharge_glo";
                    grid.DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(parameters) });
                }
                break;
            case DataBindAction.BindOtherFee:
                {
                    string spaName = "spa_stmnt_other_fees_summary";
                    grid.DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(parameters) });
                }
                break;
        }
    }
    
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        if (IsSecureQueryString && !string.IsNullOrEmpty(SecureQueryString["index"]))
        {
            uxClose.OnClientClick = "return parent.ClosePopupModal(" + SecureQueryString["index"] + ");";
        }
        else
            uxClose.OnClientClick = "return parent.HidePopupModal();";
    }
    protected string SumDepositSummaryNumber(object a, object b)
    {
        return (int.Parse(a.ToString()) + int.Parse(b.ToString())).ToString();
    }
    protected double SumDepositSummaryAmount(object a, object b)
    {
        return (double.Parse(a.ToString()) + double.Parse(b.ToString()));
    }

    #region get SPAs
    private DataTable GetStatementDetail()
    {
        FilterParameterCollection _Parameters = new FilterParameterCollection();
        _Parameters.AddLoggedInUserReportingParams();
        _Parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
        _Parameters.Add("@ReportDate", ReportDate, DbType.DateTime);
        return WebServices.CsReportServices.GetReports("spa_stmnt_GetDepositItemSummary", _Parameters);
    }
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
    
    private DataTable GetStatementTotal()
    {
        
        FilterParameterCollection _Parameters = new FilterParameterCollection();
        _Parameters.AddLoggedInUserReportingParams();
        _Parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
        _Parameters.Add("@ReportDate", ReportDate, DbType.DateTime);
        return WebServices.CsReportServices.GetReports("spa_stmnt_GetStatementTotal", _Parameters);
    }
    #endregion    

    protected void uxExportExcel_Click(object sender, EventArgs e)
    {        
        this.IsNoCache = false;
        uxPlaceHolderStyle.Visible = true;
        pnlDepositLineBreakExport.Visible = true;
        pnlCardSumaryLineBreakExport.Visible = true;
        pnlDepositItemLineBreakExport.Visible = true;
        pnlOtherFeeLineBreakExport.Visible = true;
        pnlSettlementDiscountLineBreakExport.Visible = true;
        pnlSurChargeLineBreakExport.Visible = true;
        ProcessQueryString();
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();
        parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
        parameters.Add("@ReportDate", ReportDate, DbType.DateTime);
        uxDeposits.DataSource = WebServices.CsReportServices.GetReports("spa_stmnt_deposits", parameters);
        uxCardSumary.DataSource = WebServices.CsReportServices.GetReports("spa_stmnt_GetCardSummary", parameters);
        uxSettlementDiscount.DataSource = WebServices.CsReportServices.GetReports("spa_stmnt_GetSettlementDiscount", parameters);
        uxSurcharge.DataSource = WebServices.CsReportServices.GetReports("spa_stmnt_surcharge_glo", parameters);
        uxOtherFee.DataSource = WebServices.CsReportServices.GetReports("spa_stmnt_other_fees_summary", parameters);

        Response.Clear(); //this clears the Response of any headers or previous output
        Response.Buffer = true; //make sure that the entire output is rendered simultaneously
        Response.ContentType = "application/vnd.ms-excel";

        string fileName = "StatementDetails";
        HttpContext.Current.Response.AppendHeader("content-disposition", string.Format("attachment; filename={0}.xls", fileName));
        StringWriter sw = new StringWriter();
        HtmlTextWriter textWriter = new HtmlTextWriter(sw);
        uxPlaceHolderStyle.RenderControl(textWriter);
        uxExporterPlh.RenderControl(textWriter);
        Response.Write(sw.ToString());
        sw.Close();
        textWriter.Close();
        Response.Flush();
        Response.End();
    }


    protected string FormatCurrency(object data)
    {
        return FormatData.FormatCurrency(data, SessionManager.CurrencyFortmat);
    }
    protected string FormatInteger(object data)
    {
        return FormatData.FormatInteger(data);
    }

    protected string FormatNumber4Digits(object data)
    {
        return FormatData.FormatNumber(data, 4);
    }

    protected string FormatDate(object data)
    {
        if (data is DateTime)
        {
            return (DateTime.Parse(data.ToString())).ToString(WebSiteConstants.DATE_FORMAT);
        }
        return data.ToString();
    }

    protected string FormatDate2(object data)
    {
        if (data is DateTime)
        {
            return (DateTime.Parse(data.ToString())).ToString("MM/dd");
        }
        return data.ToString();
    }

    protected void DoDataBindControls(Enum type, object sender)
    {
        Repeater grid = (Repeater)sender;
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();
        parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
        parameters.Add("@ReportDate", ReportDate, DbType.DateTime);
        string spaName = string.Empty;
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindDeposit:
                {
                    spaName = "spa_stmnt_deposits";
                }
                break;

            case DataBindAction.BindCardSumary:
                {
                    spaName = "spa_stmnt_GetCardSummary";
                }
                break;
            case DataBindAction.BindSettleDisCount:
                {
                    spaName = "spa_stmnt_GetSettlementDiscount";
                }
                break;
            case DataBindAction.BindSurCharge:
                {
                    spaName = "spa_stmnt_surcharge_glo";
                }
                break;
            case DataBindAction.BindOtherFee:
                {
                    spaName = "spa_stmnt_other_fees_summary";
                }
                break;
        }
        DataTable data = WebServices.CsReportServices.GetReports(spaName, parameters);
        grid.DataSource = data;
        if(data != null && data.Rows.Count > 0)
            grid.DataBind();
    }

    protected DataTable GetTotalData(Enum type)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();
        parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
        parameters.Add("@ReportDate", ReportDate, DbType.DateTime);
        string spaName = string.Empty;
        DataTable data = null;
        switch ((ReportType)type)
        {
            case ReportType.Deposit:
                {
                    spaName = "spa_stmnt_deposits_Total";
                    
                }
                break;

            case ReportType.CardSummary:
                {
                    spaName = "spa_stmnt_GetCardSummary_Total";
                }
                break;
            case ReportType.SettleDisCount:
                {
                    spaName = "spa_stmnt_GetSettlementDiscount_Total";
                }
                break;
            case ReportType.SurCharge:
                {
                    spaName = "spa_stmnt_surcharge_glo_Total";
                }
                break;
            case ReportType.OtherFee:
                {
                    spaName = "spa_stmnt_other_fees_summary_Total";
                }
                break;
        }
        return WebServices.CsReportServices.GetReports(spaName, parameters);
    }

    protected void uxDeposits_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Footer)
        {
            DataTable data = GetTotalData(ReportType.Deposit);
            ((Literal)e.Item.FindControl("uxLtrItems")).Text = FormatData.FormatInteger(data.Rows[0]["Items"]);
            ((Literal)e.Item.FindControl("uxLtrSales")).Text = FormatData.FormatCurrency(data.Rows[0]["Sales"], SessionManager.CurrencyFortmat);
            ((Literal)e.Item.FindControl("uxLtrCredits")).Text = FormatData.FormatCurrency(data.Rows[0]["Credits"], SessionManager.CurrencyFortmat);
            ((Literal)e.Item.FindControl("uxLtrDisc")).Text = FormatData.FormatCurrency(data.Rows[0]["Disc"], SessionManager.CurrencyFortmat);
            ((Literal)e.Item.FindControl("uxLtrNonBankMemo")).Text = FormatData.FormatCurrency(data.Rows[0]["NonBankMemo"], SessionManager.CurrencyFortmat);
            ((Literal)e.Item.FindControl("uxLtrNetDeposits")).Text = FormatData.FormatCurrency(data.Rows[0]["NetDeposit"], SessionManager.CurrencyFortmat);
        }
    }
    protected void uxCardSumary_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Footer)
        {
            DataTable data = GetTotalData(ReportType.CardSummary);
            ((Literal)e.Item.FindControl("uxLtrMasterCard")).Text = FormatData.FormatCurrency(data.Rows[0]["MasterCard"], SessionManager.CurrencyFortmat);
            ((Literal)e.Item.FindControl("uxLtrDiscover")).Text = FormatData.FormatCurrency(data.Rows[0]["Discover"], SessionManager.CurrencyFortmat);
            ((Literal)e.Item.FindControl("uxLtrVisa")).Text = FormatData.FormatCurrency(data.Rows[0]["Visa"], SessionManager.CurrencyFortmat);
            ((Literal)e.Item.FindControl("uxLtrAmex")).Text = FormatData.FormatCurrency(data.Rows[0]["Amex"], SessionManager.CurrencyFortmat);
            ((Literal)e.Item.FindControl("uxLtrDiners")).Text = FormatData.FormatCurrency(data.Rows[0]["Diners"], SessionManager.CurrencyFortmat);
            ((Literal)e.Item.FindControl("uxLtrOthers")).Text = FormatData.FormatCurrency(data.Rows[0]["Others"], SessionManager.CurrencyFortmat);
            ((Literal)e.Item.FindControl("uxLtrDebit")).Text = FormatData.FormatCurrency(data.Rows[0]["Debit"], SessionManager.CurrencyFortmat);
        }
    }
    protected void uxSettlementDiscount_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Footer)
        {
            DataTable data = GetTotalData(ReportType.SettleDisCount);
            ((Literal)e.Item.FindControl("uxLtrFeeAmount")).Text = FormatData.FormatCurrency(data.Rows[0]["FeeAmount"], SessionManager.CurrencyFortmat);
        }
    }
    protected void uxSurcharge_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Footer)
        {
            DataTable data = GetTotalData(ReportType.SurCharge);
            ((Literal)e.Item.FindControl("uxLtrAmount")).Text = FormatData.FormatCurrency(data.Rows[0]["SurchargeAmount"], SessionManager.CurrencyFortmat);
        }
    }
    protected void uxOtherFee_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Footer)
        {
            DataTable data = GetTotalData(ReportType.OtherFee);
            ((Literal)e.Item.FindControl("uxLtrAmount")).Text = FormatData.FormatCurrency(data.Rows[0]["Amount"], SessionManager.CurrencyFortmat);
        }
    }
    protected bool CheckInterChange(object InterChange)
    {
        if (InterChange != DBNull.Value && !InterChange.ToString().IsNullOrEmpty())
        {
            double itChg = Convert.ToDouble(InterChange);
            if (itChg > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
