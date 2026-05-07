using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using AS.Common.DBManager;
using AS.Common;

public partial class UserControls_rm_MCF_ACHRejectAnalysis : GlobalUserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    private string _MerchantNumber = string.Empty;
    public string MerchantNumber
    {
        get { return _MerchantNumber; }
        set { _MerchantNumber = value; }
    }

    public void GetData()
    {
        DataTable info = new DataTable();
        FilterParameterCollection parameters = new FilterParameterCollection();

        //Call EscalationStatus
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@ReportDate", DateTime.Today, DbType.Date));
        parameters.Add(new FilterParameter("@MerchantNumber", this.MerchantNumber, DbType.String));
        info = WebServices.RiskServices.GetReports("spa_RM_MCF_GetACHRejectAnalysis", parameters);

        if (info.Rows.Count == 0)
        {
            info.Rows.Add(info.NewRow());
        }

        DataRow row = info.Rows[0];

        ux7DaysQuantityDebits.Text = VeraCodeSolution.ValidateResponseData(GeneralFuncsLib.FormatWholeNumber(row["7DaysQuantityDebits"]));
        ux7DaysAmountDebits.Text = VeraCodeSolution.ValidateResponseData(GeneralFuncsLib.FormatCurrency(row["7DaysAmountDebits"]));
        ux6MonthsQuantityDebits.Text = VeraCodeSolution.ValidateResponseData(GeneralFuncsLib.FormatWholeNumber(row["6MonthsQuantityDebits"]));
        ux6MonthsAmountDebits.Text = VeraCodeSolution.ValidateResponseData(GeneralFuncsLib.FormatCurrency(row["6MonthsAmountDebits"]));

        ux7DaysQuantityCredits.Text = VeraCodeSolution.ValidateResponseData(GeneralFuncsLib.FormatWholeNumber(row["7DaysQuantityCredits"]));
        ux7DaysAmountCredits.Text = VeraCodeSolution.ValidateResponseData(GeneralFuncsLib.FormatCurrency(row["7DaysAmountCredits"]));
        ux6MonthsQuantityCredits.Text = VeraCodeSolution.ValidateResponseData(GeneralFuncsLib.FormatWholeNumber(row["6MonthsQuantityCredits"]));
        ux6MonthsAmountCredits.Text = VeraCodeSolution.ValidateResponseData(GeneralFuncsLib.FormatCurrency(row["6MonthsAmountCredits"]));
    }
}
