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

public partial class UserControls_rm_MCF_RetrievalChargebackAnalysis : GlobalUserControl
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
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@ReportDate", DateTime.Today, DbType.Date));
        parameters.Add(new FilterParameter("@MerchantNumber", this.MerchantNumber, DbType.String));
        info = WebServices.RiskServices.GetReports("spa_RM_MCF_GetRetrievalChargebackAnalysis", parameters);

        if (info.Rows.Count == 0)
        {
            info.Rows.Add(info.NewRow());
        }

        const string FOUR_DECI_FORMAT = "{0:#,##0.0000}";
        DataRow row = info.Rows[0];

        uxDailyQuantityChargebacks.Text = GeneralFuncsLib.FormatWholeNumber(row["DailyQuantityChargebacks"]);
        uxDailyAmountChargebacks.Text = GeneralFuncsLib.FormatCurrency(row["DailyAmountChargebacks"]);
        uxDailyRatioSalesChargebacks.Text = GeneralFuncsLib.FormatNumber(row["DailyRatioSalesChargebacks"], FOUR_DECI_FORMAT);
        uxDailyQuantityRetrievals.Text = GeneralFuncsLib.FormatWholeNumber(row["DailyQuantityRetrievals"]);
        uxDailyAmountRetrievals.Text = GeneralFuncsLib.FormatCurrency(row["DailyAmountRetrievals"]);
        uxDailyRatioSalesRetrievals.Text = GeneralFuncsLib.FormatNumber(row["DailyRatioSalesRetrievals"], FOUR_DECI_FORMAT);

        uxMTDQuantityChargebacks.Text = GeneralFuncsLib.FormatWholeNumber(row["MTDQuantityChargebacks"]);
        uxMTDAmountChargebacks.Text = GeneralFuncsLib.FormatCurrency(row["MTDAmountChargebacks"]);
        uxMTDRatioSalesChargebacks.Text = GeneralFuncsLib.FormatNumber(row["MTDRatioSalesChargebacks"], FOUR_DECI_FORMAT);
        uxMTDQuantityRetrievals.Text = GeneralFuncsLib.FormatWholeNumber(row["MTDQuantityRetrievals"]);
        uxMTDAmountRetrievals.Text = GeneralFuncsLib.FormatCurrency(row["MTDAmountRetrievals"]);
        uxMTDRatioSalesRetrievals.Text = GeneralFuncsLib.FormatNumber(row["MTDRatioSalesRetrievals"], FOUR_DECI_FORMAT);

        ux3MonthsQuantityChargebacks.Text = GeneralFuncsLib.FormatWholeNumber(row["3MonthsQuantityChargebacks"]);
        ux3MonthsAmountChargebacks.Text = GeneralFuncsLib.FormatCurrency(row["3MonthsAmountChargebacks"]);
        ux3MonthsRatioSalesChargebacks.Text = GeneralFuncsLib.FormatNumber(row["3MonthsRatioSalesChargebacks"], FOUR_DECI_FORMAT);
        ux3MonthsQuantityRetrievals.Text = GeneralFuncsLib.FormatWholeNumber(row["3MonthsQuantityRetrievals"]);
        ux3MonthsAmountRetrievals.Text = GeneralFuncsLib.FormatCurrency(row["3MonthsAmountRetrievals"]);
        ux3MonthsRatioSalesRetrievals.Text = GeneralFuncsLib.FormatNumber(row["3MonthsRatioSalesRetrievals"], FOUR_DECI_FORMAT);

        uxYTDQuantityChargebacks.Text = GeneralFuncsLib.FormatWholeNumber(row["YTDQuantityChargebacks"]);
        uxYTDAmountChargebacks.Text = GeneralFuncsLib.FormatCurrency(row["YTDAmountChargebacks"]);
        uxYTDRatioSalesChargebacks.Text = GeneralFuncsLib.FormatNumber(row["YTDRatioSalesChargebacks"], FOUR_DECI_FORMAT);
        uxYTDQuantityRetrievals.Text = GeneralFuncsLib.FormatWholeNumber(row["YTDQuantityRetrievals"]);
        uxYTDAmountRetrievals.Text = GeneralFuncsLib.FormatCurrency(row["YTDAmountRetrievals"]);
        uxYTDRatioSalesRetrievals.Text = GeneralFuncsLib.FormatNumber(row["YTDRatioSalesRetrievals"], FOUR_DECI_FORMAT);

    }

}
