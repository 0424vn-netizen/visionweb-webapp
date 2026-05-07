using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using AS.Common.DBManager;
using AS.Common.Formater;
using AS.Controls.Grid;
using Telerik.Web.UI;
using System.IO;
using System.Text;
using AS.Common;
using AS.Controls.Pages;
using AS.Web.Business;

[PagePermission("StatementRpt,MSStatementRpt")]
public partial class StatementDetail_MPS: ReportPage
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
    #endregion

    #region Properties
    string MerchantNumber = "";
    protected DateTime ReportDate = DateTime.Now;
    protected string HeaderString = "";
    protected string MinBillAdjustment = "0.0";
    protected string total_debit = "0.0";

    protected bool isCustomStatementTheme = false;
    protected ClientInfo ContactInformation = null;

    #endregion

    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);

        if (IsSecureQueryString)
        {
            this.MerchantNumber = SecureQueryString["MerchantNumber"];
        }

        if (!MerchantNumber.IsNullOrEmpty())
        {
            DataTable clientInfo = GeneralFuncsLib.GetStatementCustomTheme(MerchantNumber);

            if (clientInfo != null && clientInfo.Rows.Count > 0)
            {
                ContactInformation = new ClientInfo();
                ContactInformation.ClientName = clientInfo.Rows[0]["EntityName"].ToString();
                ContactInformation.Address1 = clientInfo.Rows[0]["Address1"].ToString();
                ContactInformation.Address2 = clientInfo.Rows[0]["Address2"].ToString();
                ContactInformation.Zip = clientInfo.Rows[0]["Zip"].ToString();
                ContactInformation.Phone = clientInfo.Rows[0]["Phone"].ToString();
                ContactInformation.Fax = clientInfo.Rows[0]["Fax"].ToString();
                ContactInformation.ContactEmail = clientInfo.Rows[0]["ContactEmail"].ToString();


                this.Theme = clientInfo.Rows[0]["ThemeName"].ToString();
                isCustomStatementTheme = true;
            }
            else ContactInformation = SessionManager.ClientInfo;
        }

    }

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
    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;
        IsBindDataOnLoad = true;
        string reportTitle = "<img src='App_Themes/{0}/img/{1}' alt='logo' />";

        if (isCustomStatementTheme)
        {
            uxReportTitle.ReportTitle = string.Format(reportTitle, Theme, "logo.png");
        }
        else
        {
            if (!GeneralFuncsLib.GetDataOfExtendedSetting("ThemeForStatement").IsNullOrEmpty())
            {
                uxReportTitle.ReportTitle = string.Format(reportTitle, GeneralFuncsLib.GetDataOfExtendedSetting("ThemeForStatement"), "logo.png");
            }
            else
            {
                if (!GeneralFuncsLib.GetDataOfExtendedSetting("LOGO_STMT").IsNullOrEmpty())
                {
                    uxReportTitle.ReportTitle = string.Format(reportTitle, SessionManager.CurrentUserTheme.ThemeName, GeneralFuncsLib.GetDataOfExtendedSetting("LOGO_STMT"));
                }
                else
                {
                    uxReportTitle.ReportTitle = string.Format(reportTitle, SessionManager.CurrentUserTheme.ThemeName, "logo.png");
                }
            }
        }

        if (IsSecureQueryString && !IsPostBack)
        {
            this.MerchantNumber = SecureQueryString["MerchantNumber"];
            string _reportdate = SecureQueryString["ReportDate"];
            this.ReportDate = new DateTime(long.Parse(_reportdate));
            BindData();
            DoDataBindControls(uxSettlementDiscount);
        }
        foreach (GridBoundColumn col in uxDeposits.Columns)
        {
            col.AllowSorting = false;
        }
        foreach (GridBoundColumn col in uxCardSumary.Columns)
        {
            col.AllowSorting = false;
        }
        //foreach (GridBoundColumn col in uxSettlementDiscount.Columns)
        //{
        //    col.AllowSorting = false;
        //}
        foreach (GridBoundColumn col in uxSurcharge.Columns)
        {
            col.AllowSorting = false;
        }
        foreach (GridBoundColumn col in uxOtherFee.Columns)
        {
            col.AllowSorting = false;
        }
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }
    protected void BindData()
    {
        uxRepeaterDepositSumary1.DataSource = GetStatementDetail();
        uxRepeaterDepositSumary1.DataBind();
        DataTable tbl = new DataTable();
        tbl = GetMerchantInfomation();
        uxMerchantInfo.DataSource = tbl;
        uxMerchantInfo.DataBind();

        //Add message statement for total
        if (GeneralFuncsLib.HasStatementMessageSection())
        {
            tbl = GetStatementMessage();
            if (tbl.Rows.Count > 0)
            {
                pnlMessage.Visible = true;
                uxMessage.Text = VeraCodeSolution.DoVeraCode(tbl.Rows[0]["Message"].ToString());
            }
        }

        DataTable tblMinBilAdj = new DataTable();
        tblMinBilAdj = GetMinBillAdj();
        if (tblMinBilAdj.Rows.Count > 0 && tblMinBilAdj.Rows.Count > 0)
        {
            if (Double.Parse(tblMinBilAdj.Rows[0]["MinBillAdjustment"].ToString()) == 0.0)
            {
                uxMinBillAdj.Visible = false;
            }
            else
            {
                uxMinBillAdj.Visible = true;
                MinBillAdjustment = tblMinBilAdj.Rows[0]["MinBillAdjustment"].ToString();
            }
        }
        else
        {
            uxMinBillAdj.Visible = false;
        }

        DataTable tblTotalDebit = new DataTable();
        tblTotalDebit = GetTotalDebit();
        if (tblTotalDebit.Rows.Count > 0 && tblMinBilAdj.Rows.Count > 0)
        {
            total_debit = AS.Common.Formater.FormatData.FormatCurrency((Double.Parse(tblTotalDebit.Rows[0]["total_debit"].ToString()) + Double.Parse(MinBillAdjustment)), SessionManager.CurrencyFortmat);
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
    protected string SumDepositSummaryNumber(object a, object b)
    {
        return (int.Parse(a.ToString()) + int.Parse(b.ToString())).ToString();
    }
    protected double SumDepositSummaryAmount(object a, object b)
    {
        return (double.Parse(a.ToString()) + double.Parse(b.ToString()));

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
    protected void uxSettlementDiscount_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Footer)
        {
            DataTable data = GetTotalData();
            ((Literal)e.Item.FindControl("uxLtrFeeAmount")).Text = FormatData.FormatCurrency(data.Rows[0]["FeeAmount"], SessionManager.CurrencyFortmat);
        }
    }

    protected void uxDeposits_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView dataRow = e.Item.DataItem as DataRowView;

            // Format Day
            dataItem["Day"].Text = VeraCodeSolution.DoVeraCode(GeneralFuncsLib.FormatMonthDay(dataRow["Day"]));
        }
    }

    protected DataTable GetTotalData()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();
        parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
        parameters.Add("@ReportDate", ReportDate, DbType.DateTime);
        string spaName = "spa_stmnt_GetSettlementDiscount_Total";
        return WebServices.CsReportServices.GetReports(spaName, parameters);
    }
    protected void DoDataBindControls(object sender)
    {
        Repeater grid = (Repeater)sender;
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();
        parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
        parameters.Add("@ReportDate", ReportDate, DbType.DateTime);
        string spaName = string.Empty;
        spaName = "spa_stmnt_GetSettlementDiscount";
        DataTable data = WebServices.CsReportServices.GetReports(spaName, parameters);
        grid.DataSource = data;
        if (data != null && data.Rows.Count > 0)
            grid.DataBind();
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
    private DataTable GetMinBillAdj()
    {
        FilterParameterCollection _Parameters = new FilterParameterCollection();
        _Parameters.AddLoggedInUserReportingParams();
        _Parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
        _Parameters.Add("@ReportDate", ReportDate, DbType.DateTime);
        return WebServices.CsReportServices.GetReports("spa_stmnt_min_billing_adjustment", _Parameters);
    }
    private DataTable GetTotalDebit()
    {
        FilterParameterCollection _Parameters = new FilterParameterCollection();
        _Parameters.AddLoggedInUserReportingParams();
        _Parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
        _Parameters.Add("@ReportDate", ReportDate, DbType.DateTime);
        return WebServices.CsReportServices.GetReports("spa_stmnt_GetTotalDebit", _Parameters);
    }
    private DataTable GetStatementMessage()
    {
        FilterParameterCollection _Parameters = new FilterParameterCollection();
        _Parameters.AddLoggedInUserReportingParams();
        _Parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
        _Parameters.Add("@ReportDate", ReportDate, DbType.DateTime);
        return WebServices.CsReportServices.GetReports("spa_stmnt_GetMessage", _Parameters);
    }
    #endregion
}
