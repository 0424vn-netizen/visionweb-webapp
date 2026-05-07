using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Pages;
using System;
using System.Data;
using System.Web.UI;

[PagePermission("CCView,MSCCView")]
public partial class FullCC : NonReportPage
{
    int _index = 0;
    private int Index
    {
        get
        {
            if (SecureQueryString["idx"] != null)
                int.TryParse(SecureQueryString["idx"].ToString(), out _index);
            return _index;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;
        if (!IsPostBack)
        {
            string fullCC = String.Empty;
            int recordID = -1;
            string issueBank = String.Empty;
            string country = String.Empty;
            string reportType = String.Empty;
            string reportDate = string.Empty;

            if (IsSecureQueryString)
            {
                int.TryParse(SecureQueryString["cn"].ToString(), out recordID);
                reportType = SecureQueryString["rt"].ToString();
                reportDate = SecureQueryString["reportdate"].ToString();

                FilterParameterCollection parameters_FullCard = new FilterParameterCollection();
                parameters_FullCard.Add(new FilterParameter("@UserID", SessionManager.CurrentUser.UserID, DbType.AnsiString));
                parameters_FullCard.Add(new FilterParameter("@ASClient", SessionManager.CurrentUser.ASClient, DbType.Int32));
                parameters_FullCard.Add(new FilterParameter("@RecordID", recordID, System.Data.DbType.Int32));
                parameters_FullCard.Add(new FilterParameter("@ReportType", reportType, System.Data.DbType.AnsiString));
                parameters_FullCard.Add(new FilterParameter("@ReportDate", DateTime.Parse(reportDate), System.Data.DbType.DateTime));
                parameters_FullCard.AddDecryptDataParams("AccountNumber");

                DataTable dt_FullCard = WebServices.CsReportServices.GetReports("spa_cs_GetFullCardNumber", parameters_FullCard);
                if (dt_FullCard != null && dt_FullCard.Rows.Count > 0)
                {
                    fullCC = dt_FullCard.Rows[0][0].ToString();
                }
                
                FilterParameterCollection parameters = new FilterParameterCollection();
                parameters.Add(new FilterParameter("@UserID", SessionManager.CurrentUser.UserID, DbType.AnsiString));
                parameters.Add(new FilterParameter("@ASClient", SessionManager.CurrentUser.ASClient, DbType.Int32));

                var accountNumberParam = fullCC != null && fullCC.Length >= 12 ? fullCC.Substring(0, 12) : string.Empty;
                parameters.Add(new FilterParameter("@AccountNumber", accountNumberParam, System.Data.DbType.String));

                parameters.AddLanguageID();
                DataTable dt = WebServices.CsReportServices.GetReports("spa_cs_GetIssuingBank", parameters);
                if (dt.Rows.Count > 0)
                {
                    issueBank = dt.Rows[0]["IssuingBank"].ToString();
                    country = dt.Rows[0]["CountryName"].ToString();
                }
            }
            ltrFullCC.Text = VeraCodeSolution.DoVeraCode(fullCC);
            ltrIssueBank.Text = VeraCodeSolution.DoVeraCode(issueBank);
            ltrCountry.Text = VeraCodeSolution.DoVeraCode(country);
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }

    protected void uxClose_Click(object sender, EventArgs e)
    {
        if (Index == 0)
        {
            this.AjaxAddResponseScript("parent.HidePopupModal();");
        }
        else
        {
            this.AjaxAddResponseScript("parent.HidePopupModalChild(" + Index + ");");
        }
    }
}
