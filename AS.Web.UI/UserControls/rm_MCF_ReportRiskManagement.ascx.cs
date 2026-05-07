using AS.Common;
using AS.Common.DBManager;
using AS.Common.Formater;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class UserControls_rm_MCF_ReportRiskManagement : GlobalUserControl
{
    #region Fields

    protected const int PERCENT_2_DECIMAL_DIGITS = 2;

    // Contractual column
    private const string ANNUAL_VOL_COLUMN = "AnnualVolume";
    private const string AVERAGE_TICKET_COLUMN = "AverageTicket";
    private const string AVERAGE_COLUMN = "NumberOfAverage";
    private const string PERCENT_KEYED_COLUMN = "PercentKeyed";
    private const string MAX_AUTH_COLUMN = "MaxAuthorizationAmount";
    private const string MAX_SALE_COLUMN = "MaxSaleAmount";

    // Actual column
    private const string ACTUAL_ANNUAL_VOL_COLUMN = "ActualAnnualVolume";
    private const string ACTUAL_AVERAGE_TICKET_COLUMN = "ActualAverageTicket";
    private const string ACTUAL_AVERAGE_COLUMN = "ActualNumberOfAverage";
    private const string ACTUAL_MAX_SALE_COLUMN = "ActualMaxSaleAmount";

    // Monthly column
    private const string MONTHLY_ANNUAL_VOL_COLUMN = "MonthlyAnnualVolume";
    private const string MONTHLY_AVERAGE_TICKET_COLUMN = "MonthlyAverageTicket";
    private const string MONTHLY_AVERAGE_COLUMN = "MonthlyNumberOfAverage";
    private const string MONTHLY_MAX_SALE_COLUMN = "MonthlyMaxSaleAmount";

    private string NA_VALUE
    {
        get
        {
            return GetLocalResourceObject("RiskReportRiskManagement_ascx_cs_NAValue").ToString();
        }
    }


    enum DataBindAction
    {
        BindVolumeTicket,
        BindContractual,
        BindChargebackRetrievalAnalysis,
        BindKeyedAnalysis,
        BindAuthorization,
        BindDailyForeignCard,
        BindACHAnalysis
    }

    #endregion Fields


    #region properties
    private string _MerchantNumber = string.Empty;
    public string MerchantNumber
    {
        get { return _MerchantNumber; }
        set { _MerchantNumber = value; }
    }


    private bool DisplayNewContractual
    {
        get
        {
            var setting = GeneralFuncsLib.GetClientExtendedSetting("DisplayNewContractualRskMgmtGui");
            return setting.Data != null && setting.Data.ToLower().Equals("true");
        }
    }

    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        if (Page.IsIntruderDetected) return;

        FilterParameterCollection parameters = new FilterParameterCollection();

        switch ((DataBindAction)type)
        {
            case DataBindAction.BindVolumeTicket:
                {
                    DataTable info = new DataTable();

                    //Call EscalationStatus
                    parameters.AddLoggedInUserReportingParams(false);
                    parameters.Add(new FilterParameter("@ReportDate", DateTime.Today, DbType.Date));
                    parameters.Add(new FilterParameter("@MerchantNumber", this.MerchantNumber, DbType.String));

                    info = WebServices.RiskServices.GetReports("spa_RM_MCF_GetVolumeTicketAnalysis", parameters);
                    if (info.Rows.Count == 0)
                    {
                        info.Rows.Add(info.NewRow());
                    }

                    DataRow row = info.Rows[0];

                    uxDailyCurrentVolume.Text = GeneralFuncsLib.FormatCurrency(row["DailyCurrentVolume"]);
                    SetColorForControl(uxDailyCurrentVolume, row["DailyCurrentVolume"].ToString());
                    uxDailyAverageVolume.Text = GeneralFuncsLib.FormatCurrency(row["DailyAverageVolume"]);
                    SetColorForControl(uxDailyAverageVolume, row["DailyAverageVolume"].ToString());
                    uxDailyChangeVolume.Text = FormatData.FormatNumber(row["DailyChangeVolume"], PERCENT_2_DECIMAL_DIGITS);
                    uxDailyCurrentTicket.Text = GeneralFuncsLib.FormatCurrency(row["DailyCurrentTicket"]);
                    SetColorForControl(uxDailyCurrentTicket, row["DailyCurrentTicket"].ToString());
                    uxDailyAverageTicket.Text = GeneralFuncsLib.FormatCurrency(row["DailyAverageTicket"]);
                    SetColorForControl(uxDailyAverageTicket, row["DailyAverageTicket"].ToString());
                    uxDailyChangeTicket.Text = FormatData.FormatNumber(row["DailyChangeTicket"], PERCENT_2_DECIMAL_DIGITS);

                    ux7DaysCurrentVolume.Text = GeneralFuncsLib.FormatCurrency(row["7DaysCurrentVolume"]);
                    SetColorForControl(ux7DaysCurrentVolume, row["7DaysCurrentVolume"].ToString());
                    ux7DaysAverageVolume.Text = GeneralFuncsLib.FormatCurrency(row["7DaysAverageVolume"]);
                    SetColorForControl(ux7DaysAverageVolume, row["7DaysAverageVolume"].ToString());
                    ux7DaysChangeVolume.Text = FormatData.FormatNumber(row["7DaysChangeVolume"], PERCENT_2_DECIMAL_DIGITS);
                    ux7DaysCurrentTicket.Text = GeneralFuncsLib.FormatCurrency(row["7DaysCurrentTicket"]);
                    SetColorForControl(ux7DaysCurrentTicket, row["7DaysCurrentTicket"].ToString());
                    ux7DaysAverageTicket.Text = GeneralFuncsLib.FormatCurrency(row["7DaysAverageTicket"]);
                    SetColorForControl(ux7DaysAverageTicket, row["7DaysAverageTicket"].ToString());
                    ux7DaysChangeTicket.Text = FormatData.FormatNumber(row["7DaysChangeTicket"], PERCENT_2_DECIMAL_DIGITS);

                    uxMTDCurrentVolume.Text = GeneralFuncsLib.FormatCurrency(row["MTDCurrentVolume"]);
                    SetColorForControl(uxMTDCurrentVolume, row["MTDCurrentVolume"].ToString());
                    uxMTDCurrentTicket.Text = GeneralFuncsLib.FormatCurrency(row["MTDCurrentTicket"]);
                    SetColorForControl(uxMTDCurrentTicket, row["MTDCurrentTicket"].ToString());
                    uxYTDCurrentVolume.Text = GeneralFuncsLib.FormatCurrency(row["YTDCurrentVolume"]);
                    SetColorForControl(uxYTDCurrentVolume, row["YTDCurrentVolume"].ToString());
                    uxYTDCurrentTicket.Text = GeneralFuncsLib.FormatCurrency(row["YTDCurrentTicket"]);
                    SetColorForControl(uxYTDCurrentTicket, row["YTDCurrentTicket"].ToString());
                    uxActiveDays.Text = row["ActiveDay"] != DBNull.Value ? VeraCodeSolution.DoVeraCode(FormatData.FormatInteger(row["ActiveDay"]).ToString()) : "0";
                }
                break;
            case DataBindAction.BindContractual:
                {
                    uxContainerContractual.Visible = !DisplayNewContractual;
                    uxContainerNewContractual.Visible = DisplayNewContractual;
                    if (DisplayNewContractual)
                    {
                        BindNewContractual(parameters);
                    }
                    else
                    {
                        BindContractual(parameters);
                    }
                }
                break;
            case DataBindAction.BindChargebackRetrievalAnalysis:
                {
                    DataTable info = new DataTable();
                    parameters.AddLoggedInUserReportingParams(false);
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
                    SetColorForControl(uxDailyAmountChargebacks, row["DailyAmountChargebacks"].ToString());
                    uxDailyRatioSalesChargebacks.Text = GeneralFuncsLib.FormatNumber(row["DailyRatioSalesChargebacks"], FOUR_DECI_FORMAT);
                    uxDailyQuantityRetrievals.Text = GeneralFuncsLib.FormatWholeNumber(row["DailyQuantityRetrievals"]);
                    uxDailyAmountRetrievals.Text = GeneralFuncsLib.FormatCurrency(row["DailyAmountRetrievals"]);
                    SetColorForControl(uxDailyAmountRetrievals, row["DailyAmountRetrievals"].ToString());
                    uxDailyRatioSalesRetrievals.Text = GeneralFuncsLib.FormatNumber(row["DailyRatioSalesRetrievals"], FOUR_DECI_FORMAT);

                    uxMTDQuantityChargebacks.Text = GeneralFuncsLib.FormatWholeNumber(row["MTDQuantityChargebacks"]);
                    uxMTDAmountChargebacks.Text = GeneralFuncsLib.FormatCurrency(row["MTDAmountChargebacks"]);
                    SetColorForControl(uxMTDAmountChargebacks, row["MTDAmountChargebacks"].ToString());
                    uxMTDRatioSalesChargebacks.Text = GeneralFuncsLib.FormatNumber(row["MTDRatioSalesChargebacks"], FOUR_DECI_FORMAT);
                    uxMTDQuantityRetrievals.Text = GeneralFuncsLib.FormatWholeNumber(row["MTDQuantityRetrievals"]);
                    uxMTDAmountRetrievals.Text = GeneralFuncsLib.FormatCurrency(row["MTDAmountRetrievals"]);
                    SetColorForControl(uxMTDAmountRetrievals, row["MTDAmountRetrievals"].ToString());
                    uxMTDRatioSalesRetrievals.Text = GeneralFuncsLib.FormatNumber(row["MTDRatioSalesRetrievals"], FOUR_DECI_FORMAT);

                    ux3MonthsQuantityChargebacks.Text = GeneralFuncsLib.FormatWholeNumber(row["3MonthsQuantityChargebacks"]);
                    ux3MonthsAmountChargebacks.Text = GeneralFuncsLib.FormatCurrency(row["3MonthsAmountChargebacks"]);
                    SetColorForControl(ux3MonthsAmountChargebacks, row["3MonthsAmountChargebacks"].ToString());
                    ux3MonthsRatioSalesChargebacks.Text = GeneralFuncsLib.FormatNumber(row["3MonthsRatioSalesChargebacks"], FOUR_DECI_FORMAT);
                    ux3MonthsQuantityRetrievals.Text = GeneralFuncsLib.FormatWholeNumber(row["3MonthsQuantityRetrievals"]);
                    ux3MonthsAmountRetrievals.Text = GeneralFuncsLib.FormatCurrency(row["3MonthsAmountRetrievals"]);
                    SetColorForControl(ux3MonthsAmountRetrievals, row["3MonthsAmountRetrievals"].ToString());
                    ux3MonthsRatioSalesRetrievals.Text = GeneralFuncsLib.FormatNumber(row["3MonthsRatioSalesRetrievals"], FOUR_DECI_FORMAT);

                    uxYTDQuantityChargebacks.Text = GeneralFuncsLib.FormatWholeNumber(row["YTDQuantityChargebacks"]);
                    uxYTDAmountChargebacks.Text = GeneralFuncsLib.FormatCurrency(row["YTDAmountChargebacks"]);
                    SetColorForControl(uxYTDAmountChargebacks, row["YTDAmountChargebacks"].ToString());
                    uxYTDRatioSalesChargebacks.Text = GeneralFuncsLib.FormatNumber(row["YTDRatioSalesChargebacks"], FOUR_DECI_FORMAT);
                    uxYTDQuantityRetrievals.Text = GeneralFuncsLib.FormatWholeNumber(row["YTDQuantityRetrievals"]);
                    uxYTDAmountRetrievals.Text = GeneralFuncsLib.FormatCurrency(row["YTDAmountRetrievals"]);
                    SetColorForControl(uxYTDAmountRetrievals, row["YTDAmountRetrievals"].ToString());
                    uxYTDRatioSalesRetrievals.Text = GeneralFuncsLib.FormatNumber(row["YTDRatioSalesRetrievals"], FOUR_DECI_FORMAT);
                }
                break;
            case DataBindAction.BindAuthorization:
                {
                    DataTable info = new DataTable();
                    parameters.AddLoggedInUserReportingParams(false);
                    parameters.Add(new FilterParameter("@ReportDate", DateTime.Today, DbType.Date));
                    parameters.Add(new FilterParameter("@MerchantNumber", this.MerchantNumber, DbType.String));
                    info = WebServices.RiskServices.GetReports("spa_RM_MCF_GetPercentAuthOfSalesAnalysis", parameters);

                    if (info.Rows.Count == 0)
                    {
                        info.Rows.Add(info.NewRow());
                    }

                    DataRow row = info.Rows[0];

                    uxDailyPercentAuthCount.Text = FormatData.FormatNumber(row["DailyPercentAuthCount"], PERCENT_2_DECIMAL_DIGITS);
                    ux7DaysPercentAuthCount.Text = FormatData.FormatNumber(row["7DaysPercentAuthCount"], PERCENT_2_DECIMAL_DIGITS);
                    uxDailyDaysPercentAuthAmount.Text = FormatData.FormatNumber(row["DailyDaysPercentAuthAmount"], PERCENT_2_DECIMAL_DIGITS);
                    ux7DaysPercentAuthAmount.Text = FormatData.FormatNumber(row["7DaysPercentAuthAmount"], PERCENT_2_DECIMAL_DIGITS);
                }
                break;
            case DataBindAction.BindKeyedAnalysis:
                {
                    DataTable info = new DataTable();
                    parameters.AddLoggedInUserReportingParams(false);
                    parameters.Add(new FilterParameter("@ReportDate", DateTime.Today, DbType.Date));
                    parameters.Add(new FilterParameter("@MerchantNumber", this.MerchantNumber, DbType.String));
                    info = WebServices.RiskServices.GetReports("spa_RM_MCF_GetKeyedAnalysis", parameters);

                    if (info.Rows.Count == 0)
                    {
                        info.Rows.Add(info.NewRow());
                    }

                    DataRow row = info.Rows[0];

                    uxDailyPercentKeyedCount.Text = FormatData.FormatNumber(row["DailyPercentKeyedCount"], PERCENT_2_DECIMAL_DIGITS);
                    ux7DaysPercentKeyedCount.Text = FormatData.FormatNumber(row["7DaysPercentKeyedCount"], PERCENT_2_DECIMAL_DIGITS);
                    uxMTDPercentKeyedCount.Text = FormatData.FormatNumber(row["MTDPercentKeyedCount"], PERCENT_2_DECIMAL_DIGITS);
                    uxYTDPercentKeyedCount.Text = FormatData.FormatNumber(row["YTDPercentKeyedCount"], PERCENT_2_DECIMAL_DIGITS);

                    uxDailyKeyedVolume.Text = GeneralFuncsLib.FormatCurrency(row["DailyKeyedVolume"]);
                    SetColorForControl(uxDailyKeyedVolume, row["DailyKeyedVolume"].ToString());
                    ux7DaysKeyedVolume.Text = GeneralFuncsLib.FormatCurrency(row["7DaysKeyedVolume"]);
                    SetColorForControl(ux7DaysKeyedVolume, row["7DaysKeyedVolume"].ToString());
                    uxMTDKeyedVolume.Text = GeneralFuncsLib.FormatCurrency(row["MTDKeyedVolume"]);
                    SetColorForControl(uxMTDKeyedVolume, row["MTDKeyedVolume"].ToString());
                    uxYTDKeyedVolume.Text = GeneralFuncsLib.FormatCurrency(row["YTDKeyedVolume"]);
                    SetColorForControl(uxYTDKeyedVolume, row["YTDKeyedVolume"].ToString());

                    uxDailyKeyedCount.Text = GeneralFuncsLib.FormatWholeNumber(row["DailyKeyedCount"]);
                    ux7DaysKeyedCount.Text = GeneralFuncsLib.FormatWholeNumber(row["7DaysKeyedCount"]);
                    uxMTDKeyedCount.Text = GeneralFuncsLib.FormatWholeNumber(row["MTDKeyedCount"]);
                    uxYTDKeyedCount.Text = GeneralFuncsLib.FormatWholeNumber(row["YTDKeyedCount"]);
                }
                break;
            case DataBindAction.BindDailyForeignCard:
                {
                    DataTable info = new DataTable();
                    parameters.AddLoggedInUserReportingParams(false);
                    parameters.Add(new FilterParameter("@ReportDate", DateTime.Today, DbType.Date));
                    parameters.Add(new FilterParameter("@MerchantNumber", this.MerchantNumber, DbType.String));
                    info = WebServices.RiskServices.GetReports("spa_RM_MCF_GetDailyForeignCardAnalysis", parameters);

                    if (info.Rows.Count == 0)
                    {
                        info.Rows.Add(info.NewRow());
                    }

                    DataRow row = info.Rows[0];

                    uxDailyForeignCardVolume.Text = GeneralFuncsLib.FormatCurrency(row["DailyForeignCardVolume"]);
                    uxDailyForeignCardCount.Text = GeneralFuncsLib.FormatWholeNumber(row["DailyForeignCardCount"]);
                    uxDailyPercentFCVolume.Text = FormatData.FormatNumber(row["DailyPercentFCVolume"], PERCENT_2_DECIMAL_DIGITS);
                    ux7DaysPercentFCVolume.Text = FormatData.FormatNumber(row["7DaysPercentFCVolume"], PERCENT_2_DECIMAL_DIGITS);
                }
                break;
            case DataBindAction.BindACHAnalysis:
                {
                    DataTable info = new DataTable();

                    //Call EscalationStatus
                    parameters.AddLoggedInUserReportingParams(false);
                    parameters.Add(new FilterParameter("@ReportDate", DateTime.Today, DbType.Date));
                    parameters.Add(new FilterParameter("@MerchantNumber", this.MerchantNumber, DbType.String));
                    info = WebServices.RiskServices.GetReports("spa_RM_MCF_GetACHRejectAnalysis", parameters);

                    if (info.Rows.Count == 0)
                    {
                        info.Rows.Add(info.NewRow());
                    }

                    DataRow row = info.Rows[0];

                    ux7DaysQuantityDebits.Text = GeneralFuncsLib.FormatWholeNumber(row["7DaysQuantityDebits"]);
                    ux7DaysAmountDebits.Text = GeneralFuncsLib.FormatCurrency(row["7DaysAmountDebits"]);
                    SetColorForControl(ux7DaysAmountDebits, row["7DaysAmountDebits"].ToString());
                    ux6MonthsQuantityDebits.Text = GeneralFuncsLib.FormatWholeNumber(row["6MonthsQuantityDebits"]);
                    ux6MonthsAmountDebits.Text = GeneralFuncsLib.FormatCurrency(row["6MonthsAmountDebits"]);
                    SetColorForControl(ux6MonthsAmountDebits, row["6MonthsAmountDebits"].ToString());

                    ux7DaysQuantityCredits.Text = GeneralFuncsLib.FormatWholeNumber(row["7DaysQuantityCredits"]);
                    ux7DaysAmountCredits.Text = GeneralFuncsLib.FormatCurrency(row["7DaysAmountCredits"]);
                    SetColorForControl(ux7DaysAmountCredits, row["7DaysAmountCredits"].ToString());
                    ux6MonthsQuantityCredits.Text = GeneralFuncsLib.FormatWholeNumber(row["6MonthsQuantityCredits"]);
                    ux6MonthsAmountCredits.Text = GeneralFuncsLib.FormatCurrency(row["6MonthsAmountCredits"]);
                    SetColorForControl(ux6MonthsAmountCredits, row["6MonthsAmountCredits"].ToString());
                }
                break;
        }
    }

    #region Volume/Ticket Analysis
    public void GetDataForVolumeTicket()
    {
        OnDataBindControls(DataBindAction.BindVolumeTicket);
    }


    #endregion

    public void GetDataForContractual()
    {
        OnDataBindControls(DataBindAction.BindContractual);
    }

    private void SetColorForControl(Label lb, string str)
    {
        if (string.IsNullOrEmpty(str))
            str = "0";
        else if (str.Equals(NA_VALUE))
            return;
        double i = double.Parse(str);
        if (i < 0)
            lb.ForeColor = System.Drawing.Color.Red;
    }

    public void GetDataForRetrievalChargebackAnalysis()
    {
        OnDataBindControls(DataBindAction.BindChargebackRetrievalAnalysis);
    }


    public void GetDataForKeyedAnalysis()
    {
        OnDataBindControls(DataBindAction.BindKeyedAnalysis);

    }


    public void GetDataForAuthorizations()
    {
        OnDataBindControls(DataBindAction.BindAuthorization);
    }



    public void GetDataForDailyForeignCard()
    {
        OnDataBindControls(DataBindAction.BindDailyForeignCard);
    }


    public void GetDataForACHAnalysis()
    {
        OnDataBindControls(DataBindAction.BindACHAnalysis);
    }



    #region Methods

    public void GetData()
    {
        if (Page.IsIntruderDetected) return;

        GetDataForACHAnalysis();
        GetDataForAuthorizations();
        GetDataForContractual();
        GetDataForDailyForeignCard();
        GetDataForKeyedAnalysis();
        GetDataForRetrievalChargebackAnalysis();
        GetDataForVolumeTicket();
    }

    #region Private Methods

    private void BindContractual(FilterParameterCollection parameters)
    {
        DataTable info = LoadContractualData(parameters);

        if (info.Rows.Count == 0)
        {
            info.Rows.Add(info.NewRow());
        }

        DataRow row = info.Rows[0];

        // Annual Volumn
        SetValueAndFormatCurrencyCellContractual(uxAnnualBankCardVolume, row[ANNUAL_VOL_COLUMN]);

        // Average Ticket
        SetValueAndFormatCurrencyCellContractual(uxAverageTicket, row[AVERAGE_TICKET_COLUMN]);

        // Percent Keyed
        SetValueAndFormatPercentCellContractual(uxPercentKeyed, row[PERCENT_KEYED_COLUMN]);

        // Max Auth
        SetValueAndFormatCurrencyCellContractual(uxMaxAuth, row[MAX_AUTH_COLUMN]);

        // Max Sale Amount
        SetValueAndFormatCurrencyCellContractual(uxMaxSaleAmount, row[MAX_SALE_COLUMN]);
    }

    private void BindNewContractual(FilterParameterCollection parameters)
    {
        DataTable info = LoadContractualData(parameters);

        // If there is no row in result, add 1 empty row
        if (info.Rows.Count == 0)
        {
            info.Rows.Add(info.NewRow());
        }
        DataRow row = info.Rows[0];

        // Annual Volumn
        SetValueAndFormatCurrencyCellContractual(uxAnnualVolContractual, row[ANNUAL_VOL_COLUMN]);
        SetValueAndFormatCurrencyCellContractual(uxAnnualVolActual, row[ACTUAL_ANNUAL_VOL_COLUMN]);
        SetValueAndFormatCurrencyCellContractual(uxAnnualVolMonthly, row[MONTHLY_ANNUAL_VOL_COLUMN]);

        // Average Ticket
        SetValueAndFormatCurrencyCellContractual(uxAverageTicketContractual, row[AVERAGE_TICKET_COLUMN]);
        SetValueAndFormatCurrencyCellContractual(uxAverageTicketActual, row[ACTUAL_AVERAGE_TICKET_COLUMN]);
        SetValueAndFormatCurrencyCellContractual(uxAverageTicketMonthly, row[MONTHLY_AVERAGE_TICKET_COLUMN]);

        // Number of Average
        SetValueAndFormatNumberCellContractual(uxAverageContractual, row[AVERAGE_COLUMN]);
        SetValueAndFormatNumberCellContractual(uxAverageActual, row[ACTUAL_AVERAGE_COLUMN]);
        SetValueAndFormatNumberCellContractual(uxAverageMonthly, row[MONTHLY_AVERAGE_COLUMN]);

        // Max Sale
        SetValueAndFormatCurrencyCellContractual(uxMaxSaleContractual, row[MAX_SALE_COLUMN]);
        SetValueAndFormatCurrencyCellContractual(uxMaxSaleActual, row[ACTUAL_MAX_SALE_COLUMN]);
        SetValueAndFormatCurrencyCellContractual(uxMaxSaleMonthly, row[MONTHLY_MAX_SALE_COLUMN]);
    }

    private DataTable LoadContractualData(FilterParameterCollection parameters)
    {
        DataTable info = new DataTable();
        parameters.AddLoggedInUserReportingParams(false);
        parameters.Add(new FilterParameter("@ReportDate", DateTime.Today, DbType.Date));
        parameters.Add(new FilterParameter("@MerchantNumber", this.MerchantNumber, DbType.String));
        info = WebServices.RiskServices.GetReports("spa_RM_MCF_GetContractualAnalysis", parameters);
        return info;
    }

    private void SetValueAndFormatCurrencyCellContractual(Label control, object cell)
    {
        control.Text = VeraCodeSolution.DoVeraCode(GeneralFuncsLib.FormatCurrency(cell));
        SetColorForControl(control, cell.ToString());
    }

    private void SetValueAndFormatNumberCellContractual(Label control, object cell)
    {
        string cellValue = FormatData.FormatInteger(cell);
        control.Text = VeraCodeSolution.DoVeraCode(
            string.IsNullOrEmpty(cellValue) ? NA_VALUE : cellValue);
    }

    private void SetValueAndFormatPercentCellContractual(AS.Controls.Global.Literal control, object cell)
    {
        if (cell.ToString().Equals(NA_VALUE))
        {
            control.Text = VeraCodeSolution.DoVeraCode(cell.ToString());
        }
        else
        {
            control.Text = VeraCodeSolution.DoVeraCode(
                FormatData.FormatNumber(cell, PERCENT_2_DECIMAL_DIGITS) + "%");
        }
    }

    #endregion Private Methods
    #endregion


}
