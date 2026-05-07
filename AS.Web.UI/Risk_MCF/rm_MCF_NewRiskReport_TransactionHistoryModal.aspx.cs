using System;
using System.Web.UI;

public partial class rm_MCF_NewRiskReport_TransactionHistoryModal : ReportPage
{
    private const string MERCHANT_NUMBER = "MerchantNumber";
    private const string REPORT_DATE = "ReportDate";
    string MerchantNumber = string.Empty;
    DateTime ReportDate;
    protected void Page_Load(object sender, EventArgs e)
    {
        ForcePostbackValidation = false;
        MerchantNumber = SecureQueryString[MERCHANT_NUMBER];
        ReportDate = DateTime.Parse(SecureQueryString[REPORT_DATE]);
        uxTransactionHistory.MerchantNumber = MerchantNumber;
        uxTransactionHistory.ReportDate = ReportDate;
        uxTransactionHistory.FlagLink = 0;
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }
    protected static string IsIEBrowser
    {
        get
        {
            return GeneralFuncsLib.GetIEBrowserMode();
        }
    }
}
