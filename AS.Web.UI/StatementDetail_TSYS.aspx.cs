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
using iTextSharp.text;
using iTextSharp.text.pdf;
using WebSupergoo.ABCpdf9;

using AS.Common.Utilities;
using System.Configuration;
using iTextSharp.text.html.simpleparser;
using System.Web.UI.HtmlControls;

[PagePermission("StatementRpt,MSStatementRpt")]
public partial class StatementDetail_TSYS : ReportPage
{
    #region Enum

    enum DataBindAction
    {
        BindPlanSummary,
        BindDeposits,
        BindChargebacks,
        BindFees,
        Adjustments,
    }

    enum ReportType
    {
        Deposits,
        Chargebacks,
        PlanSummary,
        Fees,
        Adjustments,
    }

    enum PostBackAction
    {
        ExportPDF
    }

    #endregion Enum

    #region Constants
    private const string FILE_NAME_EXPORT = "StatementDetails";
    private const string SPA_STATEMENT_DETAIL = "spa_stmnt_GetStatementDetails_TSYS";

    private const string DASH = "-";

    #endregion Constants

    #region Properties

    protected string MerchantNumber = "";
    protected DateTime ReportDate = DateTime.Now;
    protected double TotalDiscountDue = 0;
    protected double DiscountPaid = 0;
    protected double NetDiscountDue = 0;
    protected double TotalFeesDue = 0;
    protected double FeesDue = 0;
    protected double FeesPaid = 0;
    protected double NetFeesDue = 0;
    protected double AmountDeducted = 0;

    protected ClientInfo ContactInformation = null;

    #endregion Properties

    #region Methods

    #region Protected Methods

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
                uxContentPdf.Visible = false;
                BindData(false);
            }
        }
        //46652 - AW - Multi-Currency Transaction Display
        Literal11.Text = Literal11.Text.ToCurrencySymbol();
        Literal13.Text = Literal13.Text.ToCurrencySymbol();
        Literal26.Text = Literal26.Text.ToCurrencySymbol();
        Literal27.Text = Literal27.Text.ToCurrencySymbol();
        Literal37.Text = Literal37.Text.ToCurrencySymbol();
        Literal38.Text = Literal38.Text.ToCurrencySymbol();
        Literal39.Text = Literal39.Text.ToCurrencySymbol();
        Literal48.Text = Literal48.Text.ToCurrencySymbol();
        Literal49.Text = Literal49.Text.ToCurrencySymbol();
        Literal75.Text = Literal75.Text.ToCurrencySymbol();
        Literal77.Text = Literal77.Text.ToCurrencySymbol();
        Literal89.Text = Literal89.Text.ToCurrencySymbol();
        Literal90.Text = Literal90.Text.ToCurrencySymbol();
        Literal100.Text = Literal100.Text.ToCurrencySymbol();
        Literal101.Text = Literal101.Text.ToCurrencySymbol();
        Literal111.Text = Literal111.Text.ToCurrencySymbol();
        Literal112.Text = Literal112.Text.ToCurrencySymbol();

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }

    protected void DisplayLogo()
    {
        string themeName = string.Empty;

        if (SessionManager.CurrentUserType != WebSiteEnums.UserHierarchyMode.Merchant)
        {
            ASThemeCollection themes = WebServices.SecurityServices.GetASThemsByUser(
                SessionManager.CurrentUser.ASClient, MerchantNumber, 0);
            if (themes.Count > 0)
            {
                themeName = themes[0].ThemeName;
            }
        }
        else
        {
            themeName = SessionManager.CurrentUserTheme.ThemeName;
        }

        string logoUrl = string.Format("<img src='App_Themes/{0}/img/logo.png' alt='logo' />", themeName);
        uxReportTitle.ReportTitle = string.Format("<img src='App_Themes/{0}/img/logo.png' alt='logo' />", themeName);
    }

    protected void BindMerchantData(bool isExportPdf)
    {
        // Merchant Information
        DataTable tbl = GetMerchantInfomation();
        string amountDeducted = string.Empty;
        string clientInfo = string.Empty;
        if (tbl != null && tbl.Rows.Count > 0)
        {
            // Bind AmountDeducted to the "AMOUNT DEDUCTED FROM ACCOUNT"
            amountDeducted = VeraCodeSolution.DoVeraCode(
                FormatCurrency(tbl.Rows[0]["AmountDeducted"]));

            //Client Information
            clientInfo = string.Format("{0}<br />{1}",
                tbl.Rows[0]["ClientName"].ToString(),
                FormatAddress(tbl.Rows[0]["ClientAddress1"],
                              tbl.Rows[0]["ClientAddress2"],
                              tbl.Rows[0]["ClientAddress3"]));
        }

        if (isExportPdf)
        {
            uxMerchantInfoPdf.DataSource = tbl;
            uxMerchantInfoPdf.DataBind();
            if (tbl != null && tbl.Rows.Count > 0)
            {
                uxAmountDeductedPdf.Text = amountDeducted;
                uxClientInfoPdf.Text = VeraCodeSolution.GetOutputHtmlString(clientInfo);
            }
        }
        else
        {
            uxMerchantInfo.DataSource = tbl;
            uxMerchantInfo.DataBind();
            if (tbl != null && tbl.Rows.Count > 0)
            {
                uxAmountDeducted.Text = amountDeducted;
                uxClientInfo.Text = VeraCodeSolution.GetOutputHtmlString(clientInfo);
            }
        }
    }

    protected override void DoGridNeedDataSource(AS.Controls.Grid.ASGrid sender,
        Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        switch (sender.ID)
        {
            case "uxPlanSummary":
                OnDataBindControls(DataBindAction.BindPlanSummary, sender);
                break;
            case "uxDeposits":
                OnDataBindControls(DataBindAction.BindDeposits, sender);
                break;
            case "uxChargebacks":
                OnDataBindControls(DataBindAction.BindChargebacks, sender);
                break;
            case "uxFees":
                OnDataBindControls(DataBindAction.BindFees, sender);
                break;
            case "uxAdjustments":
                OnDataBindControls(DataBindAction.Adjustments, sender);
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
            case DataBindAction.BindPlanSummary:
                {
                    parameters.Add("@ReportType", 3, DbType.Int32);
                    grid.DataSourceInvoker = new ASFuncInvoker(
                        WebServices.CsReportServices,
                        WebSiteConstants.GET_REPORT_METHOD_NAME,
                        new object[] {
                            SPA_STATEMENT_DETAIL,
                            ReportServices.ConvertToFilterParamWSArray(parameters) });
                }
                break;
            case DataBindAction.BindDeposits:
                {
                    parameters.Add("@ReportType", 5, DbType.Int32);
                    grid.DataSourceInvoker = new ASFuncInvoker(
                        WebServices.CsReportServices,
                        WebSiteConstants.GET_REPORT_METHOD_NAME,
                        new object[] {
                            SPA_STATEMENT_DETAIL,
                            ReportServices.ConvertToFilterParamWSArray(parameters) });
                }
                break;
            case DataBindAction.BindChargebacks:
                {
                    parameters.Add("@ReportType", 6, DbType.Int32);
                    grid.DataSourceInvoker = new ASFuncInvoker(
                        WebServices.CsReportServices,
                        WebSiteConstants.GET_REPORT_METHOD_NAME,
                        new object[] {
                            SPA_STATEMENT_DETAIL,
                            ReportServices.ConvertToFilterParamWSArray(parameters) });
                }
                break;
            case DataBindAction.BindFees:
                {
                    parameters.Add("@ReportType", 7, DbType.Int32);
                    grid.DataSourceInvoker = new ASFuncInvoker(
                        WebServices.CsReportServices,
                        WebSiteConstants.GET_REPORT_METHOD_NAME,
                        new object[] {
                            SPA_STATEMENT_DETAIL,
                            ReportServices.ConvertToFilterParamWSArray(parameters) });
                }
                break;
            case DataBindAction.Adjustments:
                {
                    parameters.Add("@ReportType", 9, DbType.Int32);
                    grid.DataSourceInvoker = new ASFuncInvoker(
                        WebServices.CsReportServices,
                        WebSiteConstants.GET_REPORT_METHOD_NAME,
                        new object[] {
                            SPA_STATEMENT_DETAIL,
                            ReportServices.ConvertToFilterParamWSArray(parameters) });
                }
                break;
        }
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.ExportPDF:
                {
                    string storedFile = System.Guid.NewGuid().ToString();
                    ExportPDF(Server.MapPath("~/App_Data/ExportedFiles/" + storedFile + ".pdf"));
                    break;
                }
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
        {
            uxClose.OnClientClick = "return parent.HidePopupModal();";
        }
    }

    protected void ButtonExcel_Click(object sender, EventArgs e)
    {
        this.IsNoCache = false;
        uxPlaceHolderStyle.Visible = true;
        pnlBreakLineExportAdustments.Visible = true;
        pnlBreakLineExportChargeBack.Visible = true;
        pnlBreakLineExportDeposit.Visible = true;
        pnlBreakLineExportFees.Visible = true;
        pnlBreakLineExportPlanSummary.Visible = true;
        ProcessQueryString();
        FilterParameterCollection parameters = new FilterParameterCollection();

        //DEPOSITS
        parameters.AddLoggedInUserReportingParams();
        parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
        parameters.Add("@ASClient", MerchantNumber, DbType.String);
        parameters.Add("@ReportDate", ReportDate, DbType.DateTime);
        parameters.Add("@ReportType", 5, DbType.Int32);
        uxDeposit.DataSource = WebServices.CsReportServices.GetReports(
            SPA_STATEMENT_DETAIL, parameters);
        parameters.Clear();

        //ADJUSTMENTS
        parameters.AddLoggedInUserReportingParams();
        parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
        parameters.Add("@ReportDate", ReportDate, DbType.DateTime);
        parameters.Add("@ReportType", 9, DbType.Int32);
        uxAdjustments.DataSource = WebServices.CsReportServices.GetReports(
            SPA_STATEMENT_DETAIL, parameters);
        parameters.Clear();

        //CHARGEBACKS
        parameters.AddLoggedInUserReportingParams();
        parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
        parameters.Add("@ReportDate", ReportDate, DbType.DateTime);
        parameters.Add("@ReportType", 6, DbType.Int32);
        uxChargebacks.DataSource = WebServices.CsReportServices.GetReports(
            SPA_STATEMENT_DETAIL, parameters);
        parameters.Clear();

        //PLAN_SUMMARY
        parameters.AddLoggedInUserReportingParams();
        parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
        parameters.Add("@ReportDate", ReportDate, DbType.DateTime);
        parameters.Add("@ReportType", 3, DbType.Int32);
        uxPlanSummary.DataSource = WebServices.CsReportServices.GetReports(
            SPA_STATEMENT_DETAIL, parameters);
        parameters.Clear();

        //FEES
        parameters.AddLoggedInUserReportingParams();
        parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
        parameters.Add("@ReportDate", ReportDate, DbType.DateTime);
        parameters.Add("@ReportType", 7, DbType.Int32);
        uxFees.DataSource = WebServices.CsReportServices.GetReports(
            SPA_STATEMENT_DETAIL, parameters);
        parameters.Clear();

        Response.Clear(); //this clears the Response of any headers or previous output

        Response.Buffer = true; //make sure that the entire output is rendered simultaneously
        Response.ContentType = "application/vnd.ms-excel";

        HttpContext.Current.Response.AppendHeader("content-disposition", string.Format("attachment; filename={0}.xls", FILE_NAME_EXPORT));


        StringWriter sw = new StringWriter();
        HtmlTextWriter textWriter = new HtmlTextWriter(sw);
        File.WriteAllText(Server.MapPath("~/App_Data/" + FILE_NAME_EXPORT + ".xls"), textWriter.ToString(), Encoding.UTF8);

        //uxPlaceHolderStyle.RenderControl(textWriter);
        uxExporterPlh.RenderControl(textWriter);
        Response.Write(sw.ToString());
        sw.Close();
        textWriter.Close();
        Response.Flush();
        Response.End();
    }

    protected void ButtonPDF_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.ExportPDF);
    }

    protected string FormatCurrency(object data)
    {
        return FormatData.FormatCurrency(data, SessionManager.CurrencyFortmat);
    }

    protected string FormatCurrencyHasZeroValue(object data)
    {
        string value = FormatInteger(data);
        if (value.IsNullOrEmpty() || value.Equals("0"))
        {
            return DASH;
        }
        return FormatData.FormatCurrency(data, SessionManager.CurrencyFortmat);
    }

    protected string FormatInteger(object data)
    {
        return FormatData.FormatInteger(data);
    }

    protected string FormatInteger2Digits(object data)
    {
        string number = FormatInteger(data);
        if (number.IsNullOrEmpty() || number.Trim().Equals("0"))
        {
            number = DASH;
        }
        else if (number.Length < 2)
        {
            number = number.PadLeft(2, '0');
        }
        return number;
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

    protected string FormatAddress(object addr1, object addr2, object addr3)
    {
        StringBuilder result = new StringBuilder();
        if (addr1 != null && !addr1.ToString().IsNullOrEmpty())
        {
            result.AppendFormat("{0}<br />", addr1.ToString());
        }
        if (addr2 != null && !addr2.ToString().IsNullOrEmpty())
        {
            result.AppendFormat("{0}<br />", addr2.ToString());
        }
        if (addr3 != null && !addr3.ToString().IsNullOrEmpty())
        {
            result.AppendFormat("{0}<br />", addr3.ToString());
        }
        return result.ToString();
    }

    protected string FormatAddress(object addr1, object addr2, object addr3, int index)
    {
        string address = string.Empty;
        object[] listAddress = new object[] { addr1, addr2, addr3 };
        int countAddressIsNotNull = 0;
        for (int i = 0; i < listAddress.Length; i++)
        {
            if (listAddress[i] != null && !listAddress[i].ToString().IsNullOrEmpty())
            {
                countAddressIsNotNull++;
                if (countAddressIsNotNull == index)
                {
                    address = listAddress[i].ToString();
                    break;
                }
            }
        }
        return address;
    }

    protected void DoDataBindControls(Enum type, object sender)
    {
        Repeater grid = (Repeater)sender;
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();
        parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
        parameters.Add("@ReportDate", ReportDate, DbType.DateTime);
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindDeposits:
                parameters.Add("@ReportType", 5, DbType.Int32);
                break;
            case DataBindAction.BindChargebacks:
                parameters.Add("@ReportType", 6, DbType.Int32);
                break;
            case DataBindAction.BindPlanSummary:
                parameters.Add("@ReportType", 3, DbType.Int32);
                break;
            case DataBindAction.BindFees:
                parameters.Add("@ReportType", 7, DbType.Int32);
                break;
            case DataBindAction.Adjustments:
                parameters.Add("@ReportType", 9, DbType.Int32);
                break;
        }
        DataTable data = WebServices.CsReportServices.GetReports(SPA_STATEMENT_DETAIL, parameters);
        grid.DataSource = data;
        if (data != null && data.Rows.Count > 0)
        {
            grid.DataBind();
        }
    }

    protected DataTable GetTotalData(Enum type)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();
        parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
        parameters.Add("@ReportDate", ReportDate, DbType.DateTime);
        //DataTable data = null;
        //Get Total
        switch ((ReportType)type)
        {
            case ReportType.Deposits:
                parameters.Add("@ReportType", 12, DbType.Int32);
                break;
            case ReportType.Adjustments:
                parameters.Add("@ReportType", 10, DbType.Int32);
                break;
            case ReportType.Chargebacks:
                parameters.Add("@ReportType", 13, DbType.Int32);
                break;
            case ReportType.PlanSummary:
                parameters.Add("@ReportType", 4, DbType.Int32);
                break;
            case ReportType.Fees:
                parameters.Add("@ReportType", 8, DbType.Int32);
                break;
        }
        return WebServices.CsReportServices.GetReports(SPA_STATEMENT_DETAIL, parameters);
    }

    protected void uxDeposits_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Footer)
        {
            DataTable data = GetTotalData(ReportType.Deposits);
            ((Literal)e.Item.FindControl("uxLtrDeposit_SaleCount")).Text =
                VeraCodeSolution.ValidateResponseData(
                    FormatData.FormatInteger(data.Rows[0]["SaleCount"]));
            ((Literal)e.Item.FindControl("uxLtrDeposit_SaleAmount")).Text =
                VeraCodeSolution.ValidateResponseData(
                    FormatData.FormatCurrency(data.Rows[0]["SaleAmount"], SessionManager.CurrencyFortmat));
            ((Literal)e.Item.FindControl("uxLtrDeposit_CreditAmount")).Text =
                VeraCodeSolution.ValidateResponseData(
                    FormatData.FormatCurrency(data.Rows[0]["CreditAmount"], SessionManager.CurrencyFortmat));
            ((Literal)e.Item.FindControl("uxLtrDeposit_DiscountPD")).Text =
                VeraCodeSolution.ValidateResponseData(
                    FormatData.FormatCurrency(data.Rows[0]["DiscountPD"], SessionManager.CurrencyFortmat));
            ((Literal)e.Item.FindControl("uxLtrDeposit_NetDeposit")).Text =
                VeraCodeSolution.DoVeraCode(
                    FormatData.FormatCurrency(data.Rows[0]["NetDeposit"], SessionManager.CurrencyFortmat));

            DiscountPaid = double.Parse(data.Rows[0]["DiscountPD"].ToString());
        }
    }

    protected void uxDepositsPdf_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Footer)
        {
            DataTable data = GetTotalData(ReportType.Deposits);
            ((Literal)e.Item.FindControl("uxLtrDeposit_SaleCount_Pdf")).Text =
                VeraCodeSolution.ValidateResponseData(
                    FormatData.FormatInteger(data.Rows[0]["SaleCount"]));
            ((Literal)e.Item.FindControl("uxLtrDeposit_SaleAmount_Pdf")).Text =
                VeraCodeSolution.ValidateResponseData(
                    FormatData.FormatCurrency(data.Rows[0]["SaleAmount"], SessionManager.CurrencyFortmat));
            ((Literal)e.Item.FindControl("uxLtrDeposit_CreditAmount_Pdf")).Text =
                VeraCodeSolution.ValidateResponseData(
                    FormatData.FormatCurrency(data.Rows[0]["CreditAmount"], SessionManager.CurrencyFortmat));
            ((Literal)e.Item.FindControl("uxLtrDeposit_DiscountPD_Pdf")).Text =
                VeraCodeSolution.ValidateResponseData(
                    FormatData.FormatCurrency(data.Rows[0]["DiscountPD"], SessionManager.CurrencyFortmat));
            ((Literal)e.Item.FindControl("uxLtrDeposit_NetDeposit_Pdf")).Text =
                VeraCodeSolution.DoVeraCode(
                    FormatData.FormatCurrency(data.Rows[0]["NetDeposit"], SessionManager.CurrencyFortmat));

            DiscountPaid = double.Parse(data.Rows[0]["DiscountPD"].ToString());
        }
    }

    protected void uxAdjustments_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Footer)
        {
            DataTable data = GetTotalData(ReportType.Adjustments);
            ((Literal)e.Item.FindControl("uxLtrAdjustment_SaleCount")).Text = VeraCodeSolution.ValidateResponseData(FormatData.FormatInteger(data.Rows[0]["SaleCount"]));

            //((Literal)e.Item.FindControl("uxLtrAdjustment_SaleAmount")).Text = VeraCodeSolution.ValidateResponseData(FormatData.FormatCurrency(data.Rows[0]["SaleAmount"]));
            ((Literal)e.Item.FindControl("uxLtrAdjustment_SaleAmount")).Text = FormatData.FormatCurrency(data.Rows[0]["SaleAmount"], SessionManager.CurrencyFortmat);

            //((Literal)e.Item.FindControl("uxLtrAdjustment_CreditAmount")).Text = VeraCodeSolution.ValidateResponseData(FormatData.FormatCurrency(data.Rows[0]["CreditAmount"]));
            ((Literal)e.Item.FindControl("uxLtrAdjustment_CreditAmount")).Text = FormatData.FormatCurrency(data.Rows[0]["CreditAmount"], SessionManager.CurrencyFortmat);

            //((Literal)e.Item.FindControl("uxLtrAdjustment_DiscountPD")).Text = VeraCodeSolution.ValidateResponseData(FormatData.FormatCurrency(data.Rows[0]["DiscountPD"]));
            ((Literal)e.Item.FindControl("uxLtrAdjustment_DiscountPD")).Text = FormatData.FormatCurrency(data.Rows[0]["DiscountPD"], SessionManager.CurrencyFortmat);

            //((Literal)e.Item.FindControl("uxLtrAdjustment_NetDeposit")).Text = VeraCodeSolution.ValidateResponseData(FormatData.FormatCurrency(data.Rows[0]["NetDeposit"]));
            ((Literal)e.Item.FindControl("uxLtrAdjustment_NetDeposit")).Text = FormatData.FormatCurrency(data.Rows[0]["NetDeposit"], SessionManager.CurrencyFortmat);
        }
    }

    protected void uxAdjustmentsPdf_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Footer)
        {
            DataTable data = GetTotalData(ReportType.Adjustments);
            ((Literal)e.Item.FindControl("uxLtrAdjustment_SaleCount_Pdf")).Text =
                VeraCodeSolution.ValidateResponseData(
                    FormatData.FormatInteger(data.Rows[0]["SaleCount"]));
            ((Literal)e.Item.FindControl("uxLtrAdjustment_SaleAmount_Pdf")).Text =
                VeraCodeSolution.ValidateResponseData(
                    FormatData.FormatCurrency(data.Rows[0]["SaleAmount"], SessionManager.CurrencyFortmat));
            ((Literal)e.Item.FindControl("uxLtrAdjustment_CreditAmount_Pdf")).Text =
                VeraCodeSolution.ValidateResponseData(
                    FormatData.FormatCurrency(data.Rows[0]["CreditAmount"], SessionManager.CurrencyFortmat));
            ((Literal)e.Item.FindControl("uxLtrAdjustment_DiscountPD_Pdf")).Text =
                VeraCodeSolution.ValidateResponseData(
                    FormatData.FormatCurrency(data.Rows[0]["DiscountPD"], SessionManager.CurrencyFortmat));
            ((Literal)e.Item.FindControl("uxLtrAdjustment_NetDeposit_Pdf")).Text =
                VeraCodeSolution.ValidateResponseData(
                    FormatData.FormatCurrency(data.Rows[0]["NetDeposit"], SessionManager.CurrencyFortmat));
        }
    }


    protected void uxChargebacks_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Footer)
        {
            DataTable data = GetTotalData(ReportType.Chargebacks);
            ((Literal)e.Item.FindControl("uxLtrChargeback_SaleCount")).Text = FormatData.FormatInteger(data.Rows[0]["SaleCount"]);

            //((Literal)e.Item.FindControl("uxLtrChargeback_SaleAmount")).Text = VeraCodeSolution.ValidateResponseData(FormatData.FormatCurrency(data.Rows[0]["SaleAmount"]));
            ((Literal)e.Item.FindControl("uxLtrChargeback_SaleAmount")).Text = FormatData.FormatCurrency(data.Rows[0]["SaleAmount"], SessionManager.CurrencyFortmat);

            //((Literal)e.Item.FindControl("uxLtrChargeback_CreditAmount")).Text = VeraCodeSolution.ValidateResponseData(FormatData.FormatCurrency(data.Rows[0]["CreditAmount"]));
            ((Literal)e.Item.FindControl("uxLtrChargeback_CreditAmount")).Text = FormatData.FormatCurrency(data.Rows[0]["CreditAmount"], SessionManager.CurrencyFortmat);

            //((Literal)e.Item.FindControl("uxLtrChargeback_DiscountPD")).Text = VeraCodeSolution.ValidateResponseData(FormatData.FormatCurrency(data.Rows[0]["DiscountPD"]));
            ((Literal)e.Item.FindControl("uxLtrChargeback_DiscountPD")).Text = FormatData.FormatCurrency(data.Rows[0]["DiscountPD"], SessionManager.CurrencyFortmat);

            //((Literal)e.Item.FindControl("uxLtrChargeback_NetDeposit")).Text = VeraCodeSolution.ValidateResponseData(FormatData.FormatCurrency(data.Rows[0]["NetDeposit"]));
            ((Literal)e.Item.FindControl("uxLtrChargeback_NetDeposit")).Text = FormatData.FormatCurrency(data.Rows[0]["NetDeposit"], SessionManager.CurrencyFortmat);
        }
    }

    protected void uxChargebacksPdf_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Footer)
        {
            DataTable data = GetTotalData(ReportType.Chargebacks);
            ((Literal)e.Item.FindControl("uxLtrChargeback_SaleCount_Pdf")).Text = FormatData.FormatInteger(data.Rows[0]["SaleCount"]);
            ((Literal)e.Item.FindControl("uxLtrChargeback_SaleAmount_Pdf")).Text = FormatData.FormatCurrency(data.Rows[0]["SaleAmount"], SessionManager.CurrencyFortmat);
            ((Literal)e.Item.FindControl("uxLtrChargeback_CreditAmount_Pdf")).Text = FormatData.FormatCurrency(data.Rows[0]["CreditAmount"], SessionManager.CurrencyFortmat);
            ((Literal)e.Item.FindControl("uxLtrChargeback_DiscountPD_Pdf")).Text = FormatData.FormatCurrency(data.Rows[0]["DiscountPD"], SessionManager.CurrencyFortmat);
            ((Literal)e.Item.FindControl("uxLtrChargeback_NetDeposit_Pdf")).Text = FormatData.FormatCurrency(data.Rows[0]["NetDeposit"], SessionManager.CurrencyFortmat);
        }
    }

    protected void uxPlanSummary_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Footer)
        {
            DataTable data = GetTotalData(ReportType.PlanSummary);
            ((Literal)e.Item.FindControl("uxLtrPlan_SaleCount")).Text =
                VeraCodeSolution.DoVeraCode(
                    FormatData.FormatInteger(data.Rows[0]["SaleCount"]));
            ((Literal)e.Item.FindControl("uxLtrPlan_SaleAmount")).Text =
                VeraCodeSolution.DoVeraCode(
                    FormatData.FormatCurrency(data.Rows[0]["SaleAmount"], SessionManager.CurrencyFortmat));
            ((Literal)e.Item.FindControl("uxLtrPlan_CreditCount")).Text =
                VeraCodeSolution.DoVeraCode(
                    FormatData.FormatInteger(data.Rows[0]["CreditCount"]));
            ((Literal)e.Item.FindControl("uxLtrPlan_CreditAmount")).Text =
                VeraCodeSolution.DoVeraCode(
                    FormatData.FormatCurrency(data.Rows[0]["CreditAmount"], SessionManager.CurrencyFortmat));
            ((Literal)e.Item.FindControl("uxLtrPlan_NetAmount")).Text =
                VeraCodeSolution.DoVeraCode(
                    FormatData.FormatCurrency(data.Rows[0]["NetAmount"], SessionManager.CurrencyFortmat));
            ((Literal)e.Item.FindControl("uxLtrPlan_AvgTkt")).Text =
                VeraCodeSolution.DoVeraCode(
                    FormatData.FormatCurrency(data.Rows[0]["AverageTicket"], SessionManager.CurrencyFortmat));
            ((Literal)e.Item.FindControl("uxLtrPlan_DiscountDue")).Text =
                VeraCodeSolution.DoVeraCode(
                    FormatData.FormatCurrency(data.Rows[0]["DiscountDue"], SessionManager.CurrencyFortmat));

            TotalDiscountDue = double.Parse(data.Rows[0]["DiscountDue"].ToString());
        }
    }

    protected void uxPlanSummaryPdf_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Footer)
        {
            DataTable data = GetTotalData(ReportType.PlanSummary);
            ((Literal)e.Item.FindControl("uxLtrPlan_SaleCount_Pdf")).Text =
                VeraCodeSolution.DoVeraCode(
                    FormatData.FormatInteger(data.Rows[0]["SaleCount"]));
            ((Literal)e.Item.FindControl("uxLtrPlan_SaleAmount_Pdf")).Text =
                VeraCodeSolution.DoVeraCode(
                    FormatData.FormatCurrency(data.Rows[0]["SaleAmount"], SessionManager.CurrencyFortmat));
            ((Literal)e.Item.FindControl("uxLtrPlan_CreditCount_Pdf")).Text =
                VeraCodeSolution.DoVeraCode(
                    FormatData.FormatInteger(data.Rows[0]["CreditCount"]));
            ((Literal)e.Item.FindControl("uxLtrPlan_CreditAmount_Pdf")).Text =
                VeraCodeSolution.DoVeraCode(
                    FormatData.FormatCurrency(data.Rows[0]["CreditAmount"], SessionManager.CurrencyFortmat));
            ((Literal)e.Item.FindControl("uxLtrPlan_NetAmount_Pdf")).Text =
                VeraCodeSolution.DoVeraCode(
                    FormatData.FormatCurrency(data.Rows[0]["NetAmount"], SessionManager.CurrencyFortmat));
            ((Literal)e.Item.FindControl("uxLtrPlan_AvgTkt_Pdf")).Text =
                VeraCodeSolution.DoVeraCode(
                    FormatData.FormatCurrency(data.Rows[0]["AverageTicket"], SessionManager.CurrencyFortmat));
            ((Literal)e.Item.FindControl("uxLtrPlan_DiscountDue_Pdf")).Text =
                VeraCodeSolution.DoVeraCode(
                    FormatData.FormatCurrency(data.Rows[0]["DiscountDue"], SessionManager.CurrencyFortmat));

            TotalDiscountDue = double.Parse(data.Rows[0]["DiscountDue"].ToString());
        }
    }

    private double totalFees = 0.00;
    protected void uxFees_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            //do calculate other fees
            //there is no data for the total fees. So, do the calculation on the fees detail to make sure it does have the total fees even though it is 0 or non-0;
            if (((DataRowView)e.Item.DataItem).Row["Total"] != null) totalFees += double.Parse(((DataRowView)e.Item.DataItem).Row["Total"].ToString());
        }
        else if (e.Item.ItemType == ListItemType.Footer)
        {
            DataTable data = GetTotalData(ReportType.Fees);
            //TK32285 fixed on the 0 total fee --> shows blank or 0 instead.
            //Need to apply the fix into the uxFeesPdf_ItemDataBound method also
            if (data.Rows.Count == 0)
            {
                //show the amount as expected, then push into this command code
                if (this.uxFees.Items.Count > 0) //there is detail fees
                {
                    ((Literal)e.Item.FindControl("uxLtrFees_Total")).Text = FormatData.FormatCurrency(totalFees, SessionManager.CurrencyFortmat);
                    TotalFeesDue = totalFees;
                    this.pnlFees.FindControl("uxFees").Visible = true;
                    this.pnlFees.FindControl("divNullData").Visible = false;
                }
                else
                { //no detail fees at all, then show "No data found."
                    this.pnlFees.FindControl("uxFees").Visible = false;
                    this.pnlFees.FindControl("divNullData").Visible = true;
                }
            }
            else
            {
                ((Literal)e.Item.FindControl("uxLtrFees_Total")).Text = FormatData.FormatCurrency(data.Rows[0]["Amount"], SessionManager.CurrencyFortmat);
                TotalFeesDue = double.Parse(data.Rows[0]["Amount"].ToString());
            }
        }
    }

    private double totalFeesForPDF = 0.00;
    protected void uxFeesPdf_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            //do calculate other fees
            //there is no data for the total fees. So, do the calculation on the fees detail to make sure it does have the total fees even though it is 0 or non-0;
            if (((DataRowView)e.Item.DataItem).Row["Total"] != null) totalFeesForPDF += double.Parse(((DataRowView)e.Item.DataItem).Row["Total"].ToString());
        }
        else if (e.Item.ItemType == ListItemType.Footer)
        {
            DataTable data = GetTotalData(ReportType.Fees);
            //TK32285 fixed on the 0 total fee --> shows blank or 0 instead.
            //Need to apply the fix into the uxFees_ItemDataBound method also
            if (data.Rows.Count == 0)
            {
                //show the amount as expected, then push into this command code
                if (this.uxFeesPdf.Items.Count > 0) //there is detail fees
                {
                    ((Literal)e.Item.FindControl("uxLtrFees_Total_Pdf")).Text = FormatData.FormatCurrency(totalFeesForPDF, SessionManager.CurrencyFortmat);
                    TotalFeesDue = totalFeesForPDF;
                    this.pnlFees.FindControl("uxFeesPdf").Visible = true;
                    this.pnlFees.FindControl("divNullData_pdf").Visible = false;
                }
                else
                { //no detail fees at all, then show "No data found."
                    this.pnlFees.FindControl("uxFees").Visible = false;
                    this.pnlFees.FindControl("divNullData_pdf").Visible = true;
                }
            }
            else
            {
                ((Literal)e.Item.FindControl("uxLtrFees_Total_Pdf")).Text = FormatData.FormatCurrency(data.Rows[0]["Amount"], SessionManager.CurrencyFortmat);
                TotalFeesDue = double.Parse(data.Rows[0]["Amount"].ToString());
            }
        }
    }


    #endregion Protected Methods

    #region get SPAs

    private DataTable GetMerchantInfomation()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();
        parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
        parameters.Add("@ReportDate", ReportDate, DbType.DateTime);
        parameters.Add("@ReportType", 1, DbType.Int32);
        if (IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_DDA)
            || IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_DDA_MS))
        {
            parameters.Add(new FilterParameter("@IsViewFull", true, DbType.Boolean));
            parameters.AddDecryptDataParams("DDANumber");
        }
        return WebServices.CsReportServices.GetReports(SPA_STATEMENT_DETAIL, parameters);
    }

    private DataTable GetStatementTotal()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();
        parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
        parameters.Add("@ReportDate", ReportDate, DbType.DateTime);
        parameters.Add("@ReportType", 11, DbType.Int32);
        return WebServices.CsReportServices.GetReports(SPA_STATEMENT_DETAIL, parameters);
    }

    private string GetMessage()
    {
        string message = string.Empty;

        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();
        parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
        parameters.Add("@ReportDate", ReportDate, DbType.DateTime);
        parameters.Add("@ReportType", 2, DbType.Int32);
        DataTable dtMessage = WebServices.CsReportServices.GetReports(SPA_STATEMENT_DETAIL, parameters);
        if (dtMessage != null && dtMessage.Rows.Count > 0)
        {
            message = dtMessage.Rows[0]["Message"].ToString();
        }
        return message;
    }


    private void BindStatementTotal(bool isExportPdf)
    {
        // Statement Total
        DataTable tblStmTotal = GetStatementTotal();
        if (tblStmTotal != null && tblStmTotal.Rows.Count > 0)
        {
            FeesDue = double.Parse(tblStmTotal.Rows[0]["FeesDue"].ToString());
            AmountDeducted = double.Parse(tblStmTotal.Rows[0]["AmountDeducted"].ToString());
        }
        FeesPaid = 0;
        NetDiscountDue = TotalDiscountDue - DiscountPaid;
        NetFeesDue = TotalFeesDue - FeesPaid;

        if (isExportPdf)
        {
            uxLtrDiscountPaidPdf.Text = VeraCodeSolution.DoVeraCode(FormatCurrency(DiscountPaid));
            uxLtrNetDiscountDuePdf.Text = VeraCodeSolution.DoVeraCode(FormatCurrency(NetDiscountDue));
            uxLtrFeesDuePdf.Text = VeraCodeSolution.DoVeraCode(FormatCurrency(FeesDue));
            uxLtrFeesPaidPdf.Text = VeraCodeSolution.DoVeraCode(FormatCurrency(FeesPaid));
            uxLtrNetFeesDuePdf.Text = VeraCodeSolution.DoVeraCode(FormatCurrency(NetFeesDue));
            uxLtrAmountDeductedPdf.Text = VeraCodeSolution.DoVeraCode(FormatCurrency(AmountDeducted));
        }
        else
        {
            uxLtrDiscountPaid.Text = VeraCodeSolution.DoVeraCode(FormatCurrency(DiscountPaid));
            uxLtrNetDiscountDue.Text = VeraCodeSolution.DoVeraCode(FormatCurrency(NetDiscountDue));
            uxLtrFeesDue.Text = VeraCodeSolution.DoVeraCode(FormatCurrency(FeesDue));
            uxLtrFeesPaid.Text = VeraCodeSolution.DoVeraCode(FormatCurrency(FeesPaid));
            uxLtrNetFeesDue.Text = VeraCodeSolution.DoVeraCode(FormatCurrency(NetFeesDue));
            uxLtrAmountDeducted.Text = VeraCodeSolution.DoVeraCode(FormatCurrency(AmountDeducted));
        }
    }

    private void BindMessage(bool isExportPdf)
    {
        // Statement Message
        if (isExportPdf)
        {
            uxMessagePdf.Text = VeraCodeSolution.GetOutputHtmlString(GetMessage());
        }
        else
        {
            uxMessage.Text = VeraCodeSolution.GetOutputHtmlString(GetMessage());
        }
    }

    #endregion get SPAs

    private string BuildContentForPDFExport()
    {
        string result = string.Empty;
        this.IsNoCache = false;
        ProcessQueryString();
        BindData(true);

        StringWriter sw = new StringWriter();
        HtmlTextWriter textWriter = new HtmlTextWriter(sw);

        string themeName = string.Empty;

        if (SessionManager.CurrentUserType != WebSiteEnums.UserHierarchyMode.Merchant)
        {
            ASThemeCollection themes = WebServices.SecurityServices.GetASThemsByUser(
                SessionManager.CurrentUser.ASClient, MerchantNumber, 0);
            if (themes.Count > 0)
            {
                themeName = themes[0].ThemeName;
            }
        }
        else
        {
            themeName = SessionManager.CurrentUserTheme.ThemeName;
        }

        // Logo
        string domain = string.Format("file:///{0}", Server.MapPath("~/"));  // ConfigurationManager.AppSettings["VisionWeb_UrlByIP"];
        string logoURL = string.Format("/App_Themes/{0}/img/logo.png", themeName);
        uxImageLogo.Src = String.Format("{0}{1}", domain, logoURL);
        Logo.Visible = true;
        Logo.RenderControl(textWriter);
        Logo.Visible = false;

        // Content
        uxContentPdf.Visible = true;
        uxContentPdf.RenderControl(textWriter);
        uxContentPdf.Visible = false;

        result = string.Format("<!DOCTYPE html><html lang='en'><body>{0}</body></html>", sw.ToString());
        sw.Close();

        return result;
    }

    private void ExportPDF(string fileName)
    {
        PdfFactory pdfCreator = new PdfFactory();
        pdfCreator.CreateContent += new PdfFactory.PdfFactoryHanlder(pdfCreator_CreateContent);
        pdfCreator.Create(Response.OutputStream);

        Response.AppendHeader("content-disposition", "attachment; filename=" + FILE_NAME_EXPORT + ".pdf");
        Response.ContentType = "application/pdf";
        Response.Flush();
        Response.End();
    }

    private void pdfCreator_CreateContent(PdfFactory.PdfAgent agent, XRect rect)
    {
        agent.AddBrowserHtml(BuildContentForPDFExport());
    }

    private void BindData(bool isExportPdf)
    {
        BindMerchantData(isExportPdf);
        BindMessage(isExportPdf);
        DoDataBindControls(DataBindAction.BindPlanSummary, isExportPdf ? uxPlanSummaryPdf : uxPlanSummary);
        DoDataBindControls(DataBindAction.BindDeposits, isExportPdf ? uxDepositPdf : uxDeposit);
        DoDataBindControls(DataBindAction.BindChargebacks, isExportPdf ? uxChargebacksPdf : uxChargebacks);
        DoDataBindControls(DataBindAction.BindFees, isExportPdf ? uxFeesPdf : uxFees);
        DoDataBindControls(DataBindAction.Adjustments, isExportPdf ? uxAdjustmentsPdf : uxAdjustments);
        BindStatementTotal(isExportPdf);
    }

    #endregion Methods
}