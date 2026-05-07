using AS.Common;
using AS.Common.DBManager;
using AS.Web.UI.Controls;
using System;
using System.Data;
using System.Web.UI;
using System.Text;
using AS.Common.Formater;

public partial class VoucharModal : ReportPage
{
    string _SourceName = string.Empty;
    string _KeyName = string.Empty;
    string _TransactionID = string.Empty;
    bool _IsParentInRisk = false;
    int index = 0;
    private int Index
    {
        get
        {
            if (SecureQueryString["idx"] != null)
            {
                int.TryParse(SecureQueryString["idx"].ToString(), out index);
            }
            return index;
        }
    }

    private bool IsNotInMif
    {
        get
        {
            if (SecureQueryString["isNotInMif"].IsNotNullData() && SecureQueryString["isNotInMif"].ToLower().Equals("true"))
                return true;
            return false;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
        PageType = SecurePageType.Modal;
        BindVoucher();
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }
    private void BindVoucher()
    {
        string transactionID = this.SecureQueryString["TransactionID"];
        string merchantNumber = SecureQueryString["merch"];
        string reportDate = string.Empty;
        if (SecureQueryString["IsParentInRisk"] != null)
        {
            _IsParentInRisk = bool.Parse(SecureQueryString["IsParentInRisk"]);          
        }
        if (IsSecureQueryString && !string.IsNullOrEmpty(SecureQueryString["ReportDate"]))
        {
            reportDate = SecureQueryString["ReportDate"];
        }
        if (string.IsNullOrEmpty(merchantNumber))
        {
            merchantNumber = this.SavedReportFilterValue.Value;
        }
        FilterParameterCollection parameters = new FilterParameterCollection();

        parameters.Add("@HierarchyFilterMode", "MERCHANTNUMBER", System.Data.DbType.String);
        parameters.Add("@HierarchyFilterValue", merchantNumber, System.Data.DbType.String);
        parameters.Add("@IsNotInMif", IsNotInMif, System.Data.DbType.Boolean);

        if (IsSecureQueryString && !string.IsNullOrEmpty(SecureQueryString["ReportDate"]))
        {
            parameters.Add("@DateFilterMode", (int)DateOptionMode.Daily, System.Data.DbType.Int32);
            parameters.Add("@BeginDate", reportDate, System.Data.DbType.DateTime);
            parameters.Add("@EndDate", reportDate, System.Data.DbType.DateTime);
        }
        else
        {
            parameters.Add("@DateFilterMode", this.SavedReportFilterValue.DateOption, System.Data.DbType.Int32);
            parameters.Add("@BeginDate", this.SavedReportFilterValue.DateOptionValue.From, System.Data.DbType.DateTime);
            parameters.Add("@EndDate", this.SavedReportFilterValue.DateOptionValue.To, System.Data.DbType.DateTime);
        }
       
        parameters.Add("@TransactionID", transactionID, System.Data.DbType.String);
        DataTable dt = null;

        if (IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_CC) && !GeneralFuncsLib.HasIPForFullCard(this))
        {
            parameters.AddLoggedInUserReportingParams();
            parameters.AddLoggedInUserPrimaryUserID();
            dt = WebServices.CsReportServices.GetReports("spa_cs_GetVoucher", parameters);
        }
        else
        {
            parameters.AddLoggedInUserReportingParams();
            dt = WebServices.CsReportServices.GetReports("spa_ms_GetVoucher", parameters);

        }
        StringBuilder InfoBuilder = new StringBuilder();

        if (dt != null && dt.Rows.Count > 0)
        { 
            lblMerchantName.Text = VeraCodeSolution.DoVeraCode("<b>"+dt.Rows[0]["MerchantName"].ToString()+"</b>");
            lblAddress.Text = VeraCodeSolution.DoVeraCode(dt.Rows[0]["Address1"].ToString() + "<br/>" + dt.Rows[0]["Address2"].ToString());
            InfoBuilder.Append("<table style='width:300px'>");
            InfoBuilder.Append("<tr><td>" + GetLocalResourceObject("VoucherModal_aspx_cs_MerchatnNumber").ToString() + "</td><td class=\"value\">" + merchantNumber + "</td></tr>");
            InfoBuilder.Append("<tr><td>" + GetLocalResourceObject("VoucherModal_aspx_cs_TransDate").ToString() + "</td><td class=\"value\">" + Convert.ToDateTime(dt.Rows[0]["TransactionDate"].ToString()).ToShortDateString() + "</td></tr>");
            InfoBuilder.Append("<tr><td>" + GetLocalResourceObject("VoucherModal_aspx_cs_CardNum").ToString() + "</td><td class=\"value\">" + dt.Rows[0]["PartialCardNumber"] + "</td></tr>");

            if (GeneralFuncsLib.Show_RoutingAccountNumber)
                InfoBuilder.Append("<tr><td>" + GetLocalResourceObject("VoucherModal_aspx_cs_Routing").ToString() + "</td><td class=\"value\">" + dt.Rows[0]["PartialRoutingACC"] + "</td></tr>");

            InfoBuilder.Append("<tr><td>" + GetLocalResourceObject("VoucherModal_aspx_cs_CardType").ToString() + "</td><td class=\"value\">" + dt.Rows[0]["CardType"] + "</td></tr>");
            InfoBuilder.Append("<tr><td>" + GetLocalResourceObject("VoucherModal_aspx_cs_BatchNum").ToString() + "</td><td class=\"value\">" + dt.Rows[0]["BatchNumber"] + "</td></tr>");
            InfoBuilder.Append("<tr><td>" + GetLocalResourceObject("VoucherModal_aspx_cs_AuthNum").ToString() + "</td><td class=\"value\">" + dt.Rows[0]["AuthorizationNumber"] + "</td></tr>");
            
            decimal transamount = 0;
            try
            {
                transamount = System.Convert.ToDecimal(dt.Rows[0]["TransactionAmount"]);
            }
            catch { }
            if (transamount < 0)
                InfoBuilder.Append("<tr><td>" + GetLocalResourceObject("VoucherModal_aspx_cs_TransAmount").ToString() + "</td><td class=\"value\"><font color='red'>" + FormatData.FormatCurrency(transamount, SessionManager.CurrencyFortmat) + "</font></td></tr>");
            else
                InfoBuilder.Append("<tr><td>" + GetLocalResourceObject("VoucherModal_aspx_cs_TransAmount").ToString() + "</td><td class=\"value\">" + FormatData.FormatCurrency(transamount, SessionManager.CurrencyFortmat) + "</td></tr>");
            InfoBuilder.Append("</table>");
            lblInfo.Text = VeraCodeSolution.DoVeraCode(InfoBuilder.ToString());
        }
    }
}
