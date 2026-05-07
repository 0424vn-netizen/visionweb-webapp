using System.Linq;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using AS.Common;
using AS.Common.DBManager;
using AS.Common.Formater;
using AS.Common.Logger;
using AS.Common.WebUI;
using AS.Controls.Pages;
using AS.Core.WCF;
using AS.Notification.ServiceContract.Interfaces;
using AS.Notification.ServiceContract.Models;
using AS.Threading;
using AS.Web.Business;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading;
using WebSupergoo.ABCpdf9;

[PagePermission("MSDashboard,Dashboard")]
public partial class Dashboard : ReportPage
{
    #region CONST STRING
    private const string TODAY = "Today";
    private const string MTD = "MTD";
    private const string YTD = "YTD";
    private const string CARD_TYPE = "CardType";

    private const string ALL = "ALL";
    private const string MONTHS = "Months";
    private const string VOLUME = "Volume";
    private const string SALE_AMOUNT = "SaleAmount";
    private const string SALE_COUNT = "SaleCount";
    private const string TRANSACTION_COUNT = "TransactionCount";
    private const string RETURN_AMOUNT = "ReturnAmount";
    private const string RETURN_COUNT = "ReturnCount";
    private const string RETURN_COUNT_LABEL = "Returns Count";
    private const string RETURN_PERCENT = "ReturnPercent";
    private const string KEYED_AMOUNT = "KeyedAmount";
    private const string KEYED_AMOUNT_LABEL = "Keyed $";
    private const string KEYED_COUNT = "KeyedCount";
    private const string NET_AMOUNT = "NetAmount";
    private const string KEYED_PERCENT = "KeyedPercent";
    private const string RETRIEVAL_AMOUNT = "RetrievalAmount";
    private const string RETRIEVAL_COUNT = "RetrievalCount";
    private const string RETRIEVAL_PERCENT = "RetrievalPercent";

    private const string CHARGEBACK_AMOUNT = "ChargeBackAmount";
    private const string CHARGEBACK_COUNT = "ChargeBackCount";
    private const string CHARGEBACK_PERCENT = "ChargeBackPercent";

    private const string LAST12MONTHS_SALESVOLUME = "Last12MonthsSalesVolume";
    private const string LAST12MONTHS_TRANSACTION = "Last12MonthsSalesTransaction";
    private const string MTD_SALESVOLUME = "MTDSalesVolume";
    private const string MTD_TRANSACTION = "MTDSalesTransaction";
    private const string YTD_SALESVOLUME = "YTDSalesVolume";
    private const string YTD_TRANSACTION = "YTDSalesTransaction";
    private const string MERCHANT_STATUS = "MerchantStatus";
    private const string CARD_URL = "<a href=\"#\" onclick=\"return ShowPopupModal('CardTypeModal.aspx?{0}','auto');\">{1}</a>";
    #endregion

    #region Retrieval
    protected string _RetrievalVolumeToday;
    protected string _RetrievalVolumeMTD;
    protected string _RetrievalVolumeYTD;
    protected string _RetrievalTransToday;
    protected string _RetrievalTransMTD;
    protected string _RetrievalTransYTD;
    protected string _ChargebackVolumeToday;
    protected string _ChargebackVolumeMTD;
    protected string _ChargebackVolumeYTD;
    protected string _ChargebackTransToday;
    protected string _ChargebackTransMTD;
    protected string _ChargebackTransYTD;
    #endregion

    #region ENUMS
    enum DataBindAction
    {
        BindCardVolumes,
        BindSaleData,
        BindYTDData,
        BindMerchantApproval
    }
    #endregion

    #region FIELDS
    protected DataTable _CardVolumes = null;
    protected DataTable _SalesData = null;
    protected DataTable _YTDData = null;
    protected DataTable _MerchantApprovalData = null;

    protected string _TotalOpenMerchants = string.Empty;
    protected string _MerchantApprovals = string.Empty;
    protected string _TotalClosedMerchants = string.Empty;
    protected int _NotifyMessage = 0;
    FilterParameterCollection _Parameters;
    private int _ThreadCompleteCount = 0;
    private bool _HasMerchantAppRptPermission;
    private int _clientId;
    private int _languageId;
    private string _currency;

    protected bool IsFultonClient = false;
    protected bool Has12MonthsChart = false;
    public DataTable YTDData
    {
        get
        {
            if (_YTDData == null)
                return null;
            DataTable data = _YTDData.Copy();
            foreach (DataRow row in data.Rows)
            {
                row[MONTHS] = row[MONTHS].DateFormatMMMYY();
            }
            return data;
        }
    }
    public static string Dashboard_URL = System.Configuration.ConfigurationManager.AppSettings["Dashboard_URL"];
    #endregion

    #region EVENTS
    protected override void PageInitialize()
    {
        //ReportChart = (IChart)uxDashBoardChart;
        base.PageInitialize();
    }

    string MrchStatusOpen = string.Empty;
    string MrchStatusClose = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        // Fix Bug Language ID = 0
        _languageId = SessionManager.CurrentLanguage;
        // Fix issue: can not read session in thread
        _currency = SessionManager.CurrencyFortmat;

        MrchStatusOpen = GetLocalResourceObject("Dashboard_aspx_cs_Opened").ToString();
        MrchStatusClose = GetLocalResourceObject("Dashboard_aspx_cs_Closed").ToString();


        if (aperia.controls.KendoChart.IsDataRequest) return;
        uxPageTitle.HasPageTitle = SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.Hierarchy;
        if (SessionManager.CurrentUser.ASClient == 91)
        {
            FT_KeyedAmount.Visible = true;
            FT_ReturnCount.Visible = true;
            IsFultonClient = true;
        }

        Has12MonthsChart = GeneralFuncsLib.Has12MonthsChart();
        uxTabView.FindTab(t => t.Value == "by12months").Visible = Has12MonthsChart;

        if (!IsPostBack)
        {
            GetParameters();
            _HasMerchantAppRptPermission = true;
            if (SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.Merchant)
            {
                _HasMerchantAppRptPermission = false;
            }

            LoadDashboardData();
            while (_ThreadCompleteCount < 4)//4: count of function need to pararel
            {
                Thread.Sleep(100);
            }
            BindData();
            uxYTDChart.BindChart();
            uxMTDChart.BindChart();

            if (Has12MonthsChart)
            {
                uxLast12MonthsChart.BindChart();
            }
        }
        else if (_SalesData == null && SessionManager.SingleSignOnMenuMod)
        {
            GetParameters();
            GetSaleData();
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        if (this.IsIntruderDetected) return;

        switch ((DataBindAction)type)
        {
            case DataBindAction.BindCardVolumes:
                {
                    GetCardVolumesData(_languageId);
                }
                break;
            case DataBindAction.BindSaleData:
                {
                    BindSaleData();
                }
                break;
            case DataBindAction.BindYTDData:
                {
                    GetYTDData();
                }
                break;
            case DataBindAction.BindMerchantApproval:
                {
                    BindMerchantApproval();
                }
                break;
        }
    }

    protected void UxExportMonthlyCardVolume_ExportCSV(object sender, string title, string subtitle)
    {
        GetParameters();
        GetYTDData();

        StringBuilder strCSV = new StringBuilder();
        strCSV.Append(string.Format(", {0}, {1}", GetLocalResourceObject("Literal1Resource1.Text").ToString(), GetLocalResourceObject("Literal2Resource1.Text").ToString()) + System.Environment.NewLine);
        foreach (DataRow row in YTDData.Rows)
        {
            strCSV.Append("\"" + "'" + row[MONTHS].ToString() + "\"," + "\"" + FormatCurrencyForCSV(row[VOLUME]) + "\"," + "\"" + FormatInteger(row[TRANSACTION_COUNT]) + "\"" + System.Environment.NewLine);
        }

        string csvFileName = GeneralFuncsLib.GetFileName(title + subtitle);
        File.WriteAllText(Server.MapPath("~/App_Data/" + csvFileName + ".csv"), strCSV.ToString(), Encoding.UTF8);
        DownloadFile(csvFileName, Server.MapPath("~/App_Data/" + csvFileName + ".csv"), "csv");
    }

    protected void UxExportMonthlyCardVolume_ExportExcel(object sender, string title, string subtitle)
    {
        ExportExcel(UxExportMonthlyCardVolumePre(), title, subtitle);
    }

    protected void UxExportMonthlyCardVolume_ExportPdf(object sender, string title, string subtitle)
    {
        ExportPdf(UxExportMonthlyCardVolumePre(), title, subtitle);
    }

    protected void uxExportCardVolume_ExportCSV(object sender, string title, string subtitle)
    {
        GetParameters();
        GetCardVolumesData(_languageId);
        var strCSV = new StringBuilder();
        strCSV.Append(GetLocalResourceObject("Dashboard_aspx_cs_CardVolumeHeaderCSV").ToString() + System.Environment.NewLine);
        foreach (DataRow row in _CardVolumes.Rows)
        {
            if (row[CARD_TYPE].ToString().Trim().CompareTo(ALL) == 0)
            {
                strCSV.Append("\"" + GetLocalResourceObject("Dashboard_aspx_cs_strCSVTotal").ToString() + "\",");
                strCSV.Append("\"" + FormatCurrencyForCSV(row[MTD_SALESVOLUME]) + "\",");
                strCSV.Append("\"" + FormatInteger(row[MTD_TRANSACTION]) + "\",");
                strCSV.Append("\"" + FormatCurrencyForCSV(row[YTD_SALESVOLUME]) + "\",");
                strCSV.Append("\"" + FormatInteger(row[YTD_TRANSACTION]) + "\","); //#31975: Dashboard-CardVolume export CVS missing ,
                strCSV.Append("\"" + FormatCurrencyForCSV(row[LAST12MONTHS_SALESVOLUME]) + "\",");
                strCSV.Append("\"" + FormatInteger(row[LAST12MONTHS_TRANSACTION]) + "\",");
            }
            else
            {
                strCSV.Append("\"" + FormatString(row[CARD_TYPE]) + "\",");
                strCSV.Append("\"" + FormatCurrencyForCSV(row[MTD_SALESVOLUME]) + "\",");
                strCSV.Append("\"" + FormatInteger(row[MTD_TRANSACTION]) + "\",");
                strCSV.Append("\"" + FormatCurrencyForCSV(row[YTD_SALESVOLUME]) + "\",");
                strCSV.Append("\"" + FormatInteger(row[YTD_TRANSACTION]) + "\",");
                strCSV.Append("\"" + FormatCurrencyForCSV(row[LAST12MONTHS_SALESVOLUME]) + "\",");
                strCSV.Append("\"" + FormatInteger(row[LAST12MONTHS_TRANSACTION]) + "\"" + System.Environment.NewLine);
            }
        }
        string csvFileName = GeneralFuncsLib.GetFileName(title + subtitle);
        File.WriteAllText(Server.MapPath("~/App_Data/" + csvFileName + ".csv"), strCSV.ToString(), Encoding.UTF8);
        DownloadFile(csvFileName, Server.MapPath("~/App_Data/" + csvFileName + ".csv"), "csv");
    }

    protected void uxExportCardVolume_ExportExcel(object sender, string title, string subtitle)
    {
        ExportExcel(uxExportCardVolumePre(), title, subtitle);
    }

    protected void uxExportCardVolume_ExportPdf(object sender, string title, string subtitle)
    {
        ExportPdf(uxExportCardVolumePre(), title, subtitle);
    }

    protected void UxExportVolumeAnalysis_ExportCSV(object sender, string title, string subtitle)
    {
        GetParameters();
        GetSaleData();

        var strCSV = new StringBuilder();
        strCSV.Append(string.Format("{0}, {1}, {2}, {3}", GetLocalResourceObject("Dashboard_aspx_cs_Type").ToString(),
            GetLocalResourceObject("Literal6Resource1.Text").ToString(),
            GetLocalResourceObject("Literal7Resource1.Text").ToString(),
            GetLocalResourceObject("Literal8Resource1.Text").ToString()) + System.Environment.NewLine);
        strCSV.Append("\"" + GetLocalResourceObject("Literal9Resource1.Text").ToString() + "\"," + "\"" + FormatCurrencyForCSV(_SalesData.Rows[0][SALE_AMOUNT]) + "\"," + "\"" + FormatCurrencyForCSV(_SalesData.Rows[1][SALE_AMOUNT]) + "\"," + "\"" + FormatCurrencyForCSV(_SalesData.Rows[2][SALE_AMOUNT]) + "\"" + System.Environment.NewLine);
        strCSV.Append("\"" + GetLocalResourceObject("Literal10Resource1.Text").ToString() + "\"," + "\"" + FormatInteger(_SalesData.Rows[0][SALE_COUNT]) + "\"," + "\"" + FormatInteger(_SalesData.Rows[1][SALE_COUNT]) + "\"," + "\"" + FormatInteger(_SalesData.Rows[2][SALE_COUNT]) + "\"" + System.Environment.NewLine);

        strCSV.Append("\"" + GetLocalResourceObject("Literal11Resource1.Text").ToString() + " \"," + "\"" + FormatCurrencyForCSV(_SalesData.Rows[0][RETURN_AMOUNT]) + "\"," + "\"" + FormatCurrencyForCSV(_SalesData.Rows[1][RETURN_AMOUNT]) + "\"," + "\"" + FormatCurrencyForCSV(_SalesData.Rows[2][RETURN_AMOUNT]) + "\"" + System.Environment.NewLine);
        if (IsFultonClient)
            strCSV.Append("\"" + GetLocalResourceObject("Literal38Resource1.Text").ToString() + "\"," + "\"" + FormatInteger(_SalesData.Rows[0][RETURN_COUNT]) + "\"," + "\"" + FormatInteger(_SalesData.Rows[1][RETURN_COUNT]) + "\"," + "\"" + FormatInteger(_SalesData.Rows[2][RETURN_COUNT]) + "\"" + System.Environment.NewLine);
        strCSV.Append("\"" + GetLocalResourceObject("Literal12Resource1.Text").ToString() + "\"," + "\"" + FormatPercent(_SalesData.Rows[0][RETURN_PERCENT]) + "\"," + "\"" + FormatPercent(_SalesData.Rows[1][RETURN_PERCENT]) + "\"," + "\"" + FormatPercent(_SalesData.Rows[2][RETURN_PERCENT]) + "\"" + System.Environment.NewLine);

        strCSV.Append("\"" + GetLocalResourceObject("Literal21Resource1.Text").ToString() + "\"," + "\"" + FormatCurrencyForCSV(_SalesData.Rows[0][NET_AMOUNT]) + "\"," + "\"" + FormatCurrencyForCSV(_SalesData.Rows[1][NET_AMOUNT]) + "\"," + "\"" + FormatCurrencyForCSV(_SalesData.Rows[2][NET_AMOUNT]) + "\"" + System.Environment.NewLine);

        strCSV.Append("\"" + GetLocalResourceObject("Literal13Resource1.Text").ToString() + "\"," + "\"" + FormatCurrencyForCSV(_SalesData.Rows[0][CHARGEBACK_AMOUNT]) + "\"," + "\"" + FormatCurrencyForCSV(_SalesData.Rows[1][CHARGEBACK_AMOUNT]) + "\"," + "\"" + FormatCurrencyForCSV(_SalesData.Rows[2][CHARGEBACK_AMOUNT]) + "\"" + System.Environment.NewLine);
        strCSV.Append("\"" + GetLocalResourceObject("Literal14Resource1.Text").ToString() + "\"," + "\"" + FormatInteger(_SalesData.Rows[0][CHARGEBACK_COUNT]) + "\"," + "\"" + FormatInteger(_SalesData.Rows[1][CHARGEBACK_COUNT]) + "\"," + "\"" + FormatInteger(_SalesData.Rows[2][CHARGEBACK_COUNT]) + "\"" + System.Environment.NewLine);
        strCSV.Append("\"" + GetLocalResourceObject("Literal15Resource1.Text").ToString() + "\"," + "\"" + FormatPercent(_SalesData.Rows[0][CHARGEBACK_PERCENT]) + "\"," + "\"" + FormatPercent(_SalesData.Rows[1][CHARGEBACK_PERCENT]) + "\"," + "\"" + FormatPercent(_SalesData.Rows[2][CHARGEBACK_PERCENT]) + "\"" + System.Environment.NewLine);

        strCSV.Append("\"" + GetLocalResourceObject("Literal16Resource1.Text").ToString() + "\"," + "\"" + FormatCurrencyForCSV(_SalesData.Rows[0][RETRIEVAL_AMOUNT]) + "\"," + "\"" + FormatCurrencyForCSV(_SalesData.Rows[1][RETRIEVAL_AMOUNT]) + "\"," + "\"" + FormatCurrencyForCSV(_SalesData.Rows[2][RETRIEVAL_AMOUNT]) + "\"" + System.Environment.NewLine);
        strCSV.Append("\"" + GetLocalResourceObject("Literal17Resource1.Text").ToString() + "\"," + "\"" + FormatInteger(_SalesData.Rows[0][RETRIEVAL_COUNT]) + "\"," + "\"" + FormatInteger(_SalesData.Rows[1][RETRIEVAL_COUNT], _SalesData.Rows[1][CHARGEBACK_COUNT]) + "\"," + "\"" + FormatInteger(_SalesData.Rows[2][RETRIEVAL_COUNT]) + "\"" + System.Environment.NewLine);
        strCSV.Append("\"" + GetLocalResourceObject("Literal18Resource1.Text").ToString() + "\"," + "\"" + FormatPercent(_SalesData.Rows[0][RETRIEVAL_PERCENT]) + "\"," + "\"" + FormatPercent(_SalesData.Rows[1][RETRIEVAL_PERCENT]) + "\"," + "\"" + FormatPercent(_SalesData.Rows[2][RETRIEVAL_PERCENT]) + "\"" + System.Environment.NewLine);

        if (IsFultonClient)
            strCSV.Append("\"" + GetLocalResourceObject("Literal39Resource1.Text").ToString() + "\"," + "\"" + FormatCurrencyForCSV(_SalesData.Rows[0][KEYED_AMOUNT]) + "\"," + "\"" + FormatCurrencyForCSV(_SalesData.Rows[1][KEYED_AMOUNT]) + "\"," + "\"" + FormatCurrencyForCSV(_SalesData.Rows[2][KEYED_AMOUNT]) + "\"" + System.Environment.NewLine);
        strCSV.Append("\"" + GetLocalResourceObject("Literal19Resource1.Text").ToString() + "\"," + "\"" + FormatInteger(_SalesData.Rows[0][KEYED_COUNT]) + "\"," + "\"" + FormatInteger(_SalesData.Rows[1][KEYED_COUNT]) + "\"," + "\"" + FormatInteger(_SalesData.Rows[2][KEYED_COUNT]) + "\"" + System.Environment.NewLine);
        strCSV.Append("\"" + GetLocalResourceObject("Literal20Resource1.Text").ToString() + "\"," + "\"" + FormatPercent(_SalesData.Rows[0][KEYED_PERCENT]) + "\"," + "\"" + FormatPercent(_SalesData.Rows[1][KEYED_PERCENT]) + "\"," + "\"" + FormatPercent(_SalesData.Rows[2][KEYED_PERCENT]) + "\"" + System.Environment.NewLine);


        string csvFileName = GeneralFuncsLib.GetFileName(title + subtitle);
        File.WriteAllText(Server.MapPath("~/App_Data/" + csvFileName + ".csv"), strCSV.ToString(), Encoding.UTF8);
        DownloadFile(csvFileName, Server.MapPath("~/App_Data/" + csvFileName + ".csv"), "csv");
    }

    protected void UxExportVolumeAnalysis_ExportExcel(object sender, string title, string subtitle)
    {
        ExportExcel(UxExportVolumeAnalysisPre(), title, subtitle);
    }

    protected void UxExportVolumeAnalysis_ExportPdf(object sender, string title, string subtitle)
    {
        ExportPdf(UxExportVolumeAnalysisPre(), title, subtitle);
    }

    protected void UxExportTotalMerchant_ExportCSV(object sender, string title, string subtitle)
    {
        GetParameters();
        if (SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.Merchant)
            _HasMerchantAppRptPermission = false;
        else
            _HasMerchantAppRptPermission = true;
        BindMerchantApproval();

        var strCSV = new StringBuilder();
        strCSV.Append(", " + GetLocalResourceObject("Dashboard_aspx_cs_SharpMerchants").ToString() + System.Environment.NewLine);
        strCSV.Append("\"" + GetLocalResourceObject("Dashboard_aspx_cs_TotalOpenMerchants").ToString() + "\"," + "\"" + _TotalOpenMerchants + "\"" + System.Environment.NewLine);
        strCSV.Append("\"" + GetLocalResourceObject("Dashboard_aspx_cs_MerchantApprovals").ToString() + "\"," + "\"" + _MerchantApprovals + "\"" + System.Environment.NewLine);

        string csvFileName = GeneralFuncsLib.GetFileName(title + subtitle);
        File.WriteAllText(Server.MapPath("~/App_Data/" + csvFileName + ".csv"), strCSV.ToString(), Encoding.UTF8);
        DownloadFile(csvFileName, Server.MapPath("~/App_Data/" + csvFileName + ".csv"), "csv");
    }

    protected void UxExportTotalMerchant_ExportExcel(object sender, string title, string subtitle)
    {
        ExportExcel(UxExportTotalMerchantPre(), title, subtitle);
    }

    protected void UxExportTotalMerchant_ExportPdf(object sender, string title, string subtitle)
    {
        ExportPdf(UxExportTotalMerchantPre(), title, subtitle);
    }
    #endregion

    #region METHODS

    #region LOADDATA METHODS
    private void DoCallback(CallbackArgs args)
    {
        _ThreadCompleteCount++;
        if (args.Status == MethodCallStatus.Exception)
        {
            LoggerManager.Error(args.Exception.ToString());
        }
    }

    private void LoadDashboardData()
    {
        AsyncCaller Caller = new AsyncCaller();
        _clientId = SessionManager.CurrentClient;

        MethodCaller BindCardVolumesMethod = new MethodCaller(() => { OnDataBindControls(DataBindAction.BindCardVolumes); return null; });
        MethodCaller BindSaleDataMethod = new MethodCaller(() => { OnDataBindControls(DataBindAction.BindSaleData); return null; });
        MethodCaller BindYTDDataMethod = new MethodCaller(() => { OnDataBindControls(DataBindAction.BindYTDData); return null; });
        MethodCaller BindMerchantApprovalMethod = new MethodCaller(() => { OnDataBindControls(DataBindAction.BindMerchantApproval); return null; });

        CallbackMethod DoNothingCallBack = new CallbackMethod(this.DoCallback);

        List<MethodCaller> callerList = new List<MethodCaller>(); List<CallbackMethod> callbackList = new List<CallbackMethod>();
        callerList.Add(BindCardVolumesMethod); callbackList.Add(DoNothingCallBack);
        callerList.Add(BindSaleDataMethod); callbackList.Add(DoNothingCallBack);
        callerList.Add(BindYTDDataMethod); callbackList.Add(DoNothingCallBack);
        callerList.Add(BindMerchantApprovalMethod); callbackList.Add(DoNothingCallBack);

        Caller.Start(callerList, callbackList);
    }
    #endregion

    #region BINDDATA METHODS
    private void BindData()
    {
        uxMonthlyCardVolume.DataSource = YTDData;
        uxMonthlyCardVolume.DataBind();

        rptCardVolume.DataSource = _CardVolumes;
        rptCardVolume.DataBind();

        rptMerchantApproval.DataSource = _MerchantApprovalData;
        rptMerchantApproval.DataBind();
    }

    //private void BindYTDData()
    //{
    //    GetYTDData();
    //    uxMonthlyCardVolume.DataSource = _YTDData;
    //    uxMonthlyCardVolume.DataBind();
    //}

    private void GetYTDData()
    {
        _YTDData = GetDataSource("spa_ms_Reskin_GetDashboardYTDVolumeTransChart");
    }

    private void GetCardVolumesData(int languageId)
    {
        var parameters = _Parameters.CloneCollection();
        parameters.Add(new FilterParameter("@LanguageID", languageId, DbType.Int32));
        DataTable dt = GetDataSource("spa_ms_Reskin_GetDashboardCard", parameters);

        _CardVolumes = NewCardVolumesDataTable();
        foreach (DataRow row in dt.Rows)
        {
            _CardVolumes.ImportRow(row);
        }
        //// Only ALL
        if (_CardVolumes.Rows.Count == 1 && _CardVolumes.Rows[0][CARD_TYPE] == ALL)
            _CardVolumes.Rows.RemoveAt(0);

        for (int i = 0; i < _CardVolumes.Rows.Count; i++)
        {
            if (_CardVolumes.Rows[i][CARD_TYPE].ToString().Trim().CompareTo(ALL) == 0)
            {
                DataRow totalRow = _CardVolumes.NewRow();
                for (int j = 0; j < _CardVolumes.Rows[i].ItemArray.Length; j++)
                {
                    totalRow[j] = _CardVolumes.Rows[i].ItemArray[j];
                }
                _CardVolumes.Rows.RemoveAt(i);
                _CardVolumes.Rows.Add(totalRow);
                break;
            }
        }
    }

    private void BindSaleData()
    {
        GetSaleData();
        GetRetrievalChargeback();
    }

    private void GetSaleData()
    {
        _SalesData = GetDataSourceByDataSet("spa_ms_Reskin_GetDashboardSale");
        Session.Add("_SalesData", _SalesData);
        int MetricsDimension = 3;
        int RemainiingDimension;
        if (_SalesData == null) _SalesData = NewSalesDataDataTable();
        RemainiingDimension = MetricsDimension - _SalesData.Rows.Count;
        if (RemainiingDimension > 0)
        {
            // Fill remain dimension (Today, MTD, YTD)
            for (int i = 0; i < RemainiingDimension; i++)
            {
                DataRow EmptyDashboardSales = _SalesData.NewRow();
                _SalesData.Rows.Add(EmptyDashboardSales);
            }
        }
    }

    private void GetRetrievalChargeback()
    {
        _RetrievalVolumeToday = !_SalesData.Rows[0].IsNull(RETRIEVAL_AMOUNT) && decimal.Parse(_SalesData.Rows[0][RETRIEVAL_AMOUNT].ToString()) == decimal.Zero ? FormatCurrency(_SalesData.Rows[0][RETRIEVAL_AMOUNT]) : BuildRetrieval(TODAY, FormatCurrency(_SalesData.Rows[0][RETRIEVAL_AMOUNT]));
        _RetrievalVolumeMTD = !_SalesData.Rows[1].IsNull(RETRIEVAL_AMOUNT) && decimal.Parse(_SalesData.Rows[1][RETRIEVAL_AMOUNT].ToString()) == decimal.Zero ? FormatCurrency(_SalesData.Rows[1][RETRIEVAL_AMOUNT]) : BuildRetrieval(MTD, FormatCurrency(_SalesData.Rows[1][RETRIEVAL_AMOUNT]));
        _RetrievalVolumeYTD = !_SalesData.Rows[2].IsNull(RETRIEVAL_AMOUNT) && decimal.Parse(_SalesData.Rows[2][RETRIEVAL_AMOUNT].ToString()) == decimal.Zero ? FormatCurrency(_SalesData.Rows[2][RETRIEVAL_AMOUNT]) : BuildRetrieval(YTD, FormatCurrency(_SalesData.Rows[2][RETRIEVAL_AMOUNT]));

        _RetrievalTransToday = !_SalesData.Rows[0].IsNull(RETRIEVAL_COUNT) && int.Parse(_SalesData.Rows[0][RETRIEVAL_COUNT].ToString()) == 0 ? FormatInteger(_SalesData.Rows[0][RETRIEVAL_COUNT]) : BuildRetrieval(TODAY, FormatInteger(_SalesData.Rows[0][RETRIEVAL_COUNT]));
        _RetrievalTransMTD = !_SalesData.Rows[1].IsNull(RETRIEVAL_COUNT) && int.Parse(_SalesData.Rows[1][RETRIEVAL_COUNT].ToString()) == 0 ? FormatInteger(_SalesData.Rows[1][RETRIEVAL_COUNT]) : BuildRetrieval(MTD, FormatInteger(_SalesData.Rows[1][RETRIEVAL_COUNT]));
        _RetrievalTransYTD = !_SalesData.Rows[2].IsNull(RETRIEVAL_COUNT) && int.Parse(_SalesData.Rows[2][RETRIEVAL_COUNT].ToString()) == 0 ? FormatInteger(_SalesData.Rows[2][RETRIEVAL_COUNT]) : BuildRetrieval(YTD, FormatInteger(_SalesData.Rows[2][RETRIEVAL_COUNT]));

        _ChargebackVolumeToday = !_SalesData.Rows[0].IsNull(CHARGEBACK_AMOUNT) && decimal.Parse(_SalesData.Rows[0][CHARGEBACK_AMOUNT].ToString()) == decimal.Zero ? FormatCurrency(_SalesData.Rows[0][CHARGEBACK_AMOUNT]) : BuildChargeback(TODAY, FormatCurrency(_SalesData.Rows[0][CHARGEBACK_AMOUNT]));
        _ChargebackVolumeMTD = !_SalesData.Rows[1].IsNull(CHARGEBACK_AMOUNT) && decimal.Parse(_SalesData.Rows[1][CHARGEBACK_AMOUNT].ToString()) == decimal.Zero ? FormatCurrency(_SalesData.Rows[1][CHARGEBACK_AMOUNT]) : BuildChargeback(MTD, FormatCurrency(_SalesData.Rows[1][CHARGEBACK_AMOUNT]));
        _ChargebackVolumeYTD = !_SalesData.Rows[2].IsNull(CHARGEBACK_AMOUNT) && decimal.Parse(_SalesData.Rows[2][CHARGEBACK_AMOUNT].ToString()) == decimal.Zero ? FormatCurrency(_SalesData.Rows[2][CHARGEBACK_AMOUNT]) : BuildChargeback(YTD, FormatCurrency(_SalesData.Rows[2][CHARGEBACK_AMOUNT]));

        _ChargebackTransToday = !_SalesData.Rows[0].IsNull(CHARGEBACK_COUNT) && int.Parse(_SalesData.Rows[0][CHARGEBACK_COUNT].ToString()) == 0 ? FormatInteger(_SalesData.Rows[0][CHARGEBACK_COUNT]) : BuildChargeback(TODAY, FormatInteger(_SalesData.Rows[0][CHARGEBACK_COUNT]));
        _ChargebackTransMTD = !_SalesData.Rows[1].IsNull(CHARGEBACK_COUNT) && int.Parse(_SalesData.Rows[1][CHARGEBACK_COUNT].ToString()) == 0 ? FormatInteger(_SalesData.Rows[1][CHARGEBACK_COUNT]) : BuildChargeback(MTD, FormatInteger(_SalesData.Rows[1][CHARGEBACK_COUNT]));
        _ChargebackTransYTD = !_SalesData.Rows[2].IsNull(CHARGEBACK_COUNT) && int.Parse(_SalesData.Rows[2][CHARGEBACK_COUNT].ToString()) == 0 ? FormatInteger(_SalesData.Rows[2][CHARGEBACK_COUNT]) : BuildChargeback(YTD, FormatInteger(_SalesData.Rows[2][CHARGEBACK_COUNT]));
    }

    private void BindMerchantApproval()
    {
        if (!_HasMerchantAppRptPermission)
        {
            pnlMerchantApproval.Visible = false;
            //_Align = "center";
        }
        else
        {
            pnlMerchantApproval.Visible = true;
            //_Align = "right";
            //_MerchantApprovalsList = GetDataSource("spa_ms_GetDashboardMerchantApproval");
            DataTable merchantApprovals = GetDataSource("spa_ms_GetDashboardMerchantApprovalList");
            if (merchantApprovals.Rows.Count > 0)
            {
                _TotalOpenMerchants = FormatInteger(merchantApprovals.Rows[0]["TotalOpenMerchants"]);
                _MerchantApprovals = FormatInteger(merchantApprovals.Rows[0]["ApprovalMerchants"]);
                _TotalClosedMerchants = FormatInteger(merchantApprovals.Rows[0]["TotalClosedMerchants"]);
            }

            if (IsFultonClient)
            {
                uxPnlApproval.Visible = false;
                uxPnlClosedMerchant.Visible = true;
                FT_MerchantApproval.Visible = true;
                _MerchantApprovalData = GetDataSource("spa_ms_Reskin_GetDashBoardMerchantAccount");
                if (_MerchantApprovalData.Rows.Count > 0)
                {
                    for (int i = 0; i < _MerchantApprovalData.Rows.Count; i++)
                    {
                        if (string.Compare(_MerchantApprovalData.Rows[i][MERCHANT_STATUS].ToString(), "OPEN", false) == 0)
                            _MerchantApprovalData.Rows[i][MERCHANT_STATUS] = MrchStatusOpen;
                        else if (string.Compare(_MerchantApprovalData.Rows[i][MERCHANT_STATUS].ToString(), "CLOSE", false) == 0)
                            _MerchantApprovalData.Rows[i][MERCHANT_STATUS] = MrchStatusClose;
                    }
                }
            }
            else
            {
                pnlMerchantApproval.Attributes["class"] += " col-xs-5";
                pnlMA.Attributes["class"] = "col-xs-12";
            }
        }
    }

    private void GetParameters()
    {
        _Parameters = new FilterParameterCollection();
        _Parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
    }

    private DataTable GetDataSource(string spaName, FilterParameterCollection parameters = null)
    {
        ReportServices service = WebServices.MsReportServices; //
        if (!IsPostBack)
            service.AddRequestHeader("ClientId", _clientId.ToString());
        return service.GetReports(spaName, parameters == null ? _Parameters : parameters);
    }

    private DataTable GetDataSourceByDataSet(string spaName)
    {
        DataTable result = new DataTable();
        ReportServices service = WebServices.MsReportServices; //
        if (!IsPostBack)
            service.AddRequestHeader("ClientId", _clientId.ToString());
        DataSet ds = service.GetReportsAsDataSet(spaName, _Parameters);
        if (ds.Tables.Count > 1)
        {
            result = ds.Tables[0];
            for (int i = 1; i < ds.Tables.Count; i++)
            {
                result.Merge(ds.Tables[i]);
            }
        }
        return result;
    }
    #endregion

    #region BUILD URL
    protected string BuildCardType(object cardType)
    {
        if (cardType.ToString().Trim().CompareTo(ALL) == 0)
            return string.Empty;
        return VeraCodeSolution.DoVeraCode(cardType.ToString());
        //return VeraCodeSolution.DoVeraCode(string.Format(CARD_URL, BuildSecureQueryString("cardtype=" + cardType.ToString()), cardType));
    }

    private string BuildRetrieval(string param1, string param2)
    {
        string queryString = ((ReportPage)this.Page).BuildSecureQueryString("type=" + param1);
        string urlBatchDetail = "Retrievals.aspx?" + queryString;
        return VeraCodeSolution.DoVeraCode(string.Format("<a class=\"link\" href=\"{0}\" style=\"cursor:pointer\" >{1}</a>", urlBatchDetail, param2));
    }

    private string BuildChargeback(string param1, string param2)
    {
        string queryString = ((ReportPage)this.Page).BuildSecureQueryString("type=" + param1);
        string urlBatchDetail = "Chargebacks.aspx?" + queryString;
        return VeraCodeSolution.DoVeraCode(string.Format("<a class=\"link\" href=\"{0}\" style=\"cursor:pointer\" >{1}</a>", urlBatchDetail, param2));
    }
    #endregion

    #region EXPORT METHODS
    private string UxExportMonthlyCardVolumePre()
    {
        StringBuilder strExcelTemplate = new StringBuilder(File.ReadAllText(Server.MapPath("~/App_Data/tpl_MonthlyCardVolume.htm")));
        strExcelTemplate.Replace("[MonthlyVolume]", GetLocalResourceObject("UxExportMonthlyCardVolumeResource1.Title").ToString());
        strExcelTemplate.Replace("[GrossSales]", GetLocalResourceObject("Literal1Resource1.Text").ToString());
        strExcelTemplate.Replace("[SharpTrans]", GetLocalResourceObject("Literal2Resource1.Text").ToString());
        GetParameters();
        GetYTDData();
        for (int i = YTDData.Rows.Count - 1; i >= 0; i--)
        {
            strExcelTemplate.Replace("CVR_MonthValue" + i.ToString(), YTDData.Rows[i][MONTHS].ToString() + "&nbsp;");
            strExcelTemplate.Replace("CVR_TransactionAmount_" + i.ToString(), FormatCurrency(YTDData.Rows[i][VOLUME]));
            strExcelTemplate.Replace("CVR_TransactionCount_" + i.ToString(), FormatInteger(YTDData.Rows[i][TRANSACTION_COUNT]));
        }

        return strExcelTemplate.ToString();
    }

    private string uxExportCardVolumePre()
    {
        var strExcelTemplate = new StringBuilder(File.ReadAllText(Server.MapPath("~/App_Data/tpl_CardVolume.htm")));
        strExcelTemplate.Replace("[CardVolume]", GetLocalResourceObject("uxExportCardVolumeResource1.Title").ToString());
        strExcelTemplate.Replace("[MTD]", GetLocalResourceObject("Literal25Resource1.Text").ToString());
        strExcelTemplate.Replace("[YTD]", GetLocalResourceObject("Literal26Resource1.Text").ToString());
        strExcelTemplate.Replace("[Last12Months]", GetLocalResourceObject("Literal27Resource1.Text").ToString());
        strExcelTemplate.Replace("[GrossSales1]", GetLocalResourceObject("Literal28Resource1.Text").ToString());
        strExcelTemplate.Replace("[#Trans1]", GetLocalResourceObject("Literal29Resource1.Text").ToString());
        strExcelTemplate.Replace("[GrossSales2]", GetLocalResourceObject("Literal30Resource1.Text").ToString());
        strExcelTemplate.Replace("[#Trans2]", GetLocalResourceObject("Literal31Resource1.Text").ToString());
        strExcelTemplate.Replace("[GrossSales3]", GetLocalResourceObject("Literal32Resource1.Text").ToString());
        strExcelTemplate.Replace("[#Trans3]", GetLocalResourceObject("Literal33Resource1.Text").ToString());

        var strPartOutput = new StringBuilder();
        StringBuilder strExcelPartTemplate;
        GetParameters();
        GetCardVolumesData(_languageId);

        for (int i = 0; i < _CardVolumes.Rows.Count; i++)
        {
            strExcelPartTemplate = new StringBuilder(File.ReadAllText(Server.MapPath("~/App_Data/tpl_CardVolumePart.htm")));

            if (_CardVolumes.Rows[i][CARD_TYPE].ToString().Trim().CompareTo(ALL) == 0)
            {
                strExcelPartTemplate.Replace("CV_CardType", GetLocalResourceObject("Dashboard_aspx_cs_strCSVTotal").ToString());
                strExcelPartTemplate.Replace("CV_Class", "class=\"borderBottom\"");
            }
            else
            {
                strExcelPartTemplate.Replace("CV_CardType", FormatString(_CardVolumes.Rows[i][CARD_TYPE]) + "&nbsp;");
                strExcelPartTemplate.Replace("CV_Class", string.Empty);
            }
            strExcelPartTemplate.Replace("CV_MTDSalesVolume", FormatCurrency(_CardVolumes.Rows[i][MTD_SALESVOLUME]));
            strExcelPartTemplate.Replace("CV_MTDSalesTransaction", FormatInteger(_CardVolumes.Rows[i][MTD_TRANSACTION]));
            strExcelPartTemplate.Replace("CV_YTDSalesVolume", FormatCurrency(_CardVolumes.Rows[i][YTD_SALESVOLUME]));
            strExcelPartTemplate.Replace("CV_YTDSalesTransaction", FormatInteger(_CardVolumes.Rows[i][YTD_TRANSACTION]));
            strExcelPartTemplate.Replace("CV_Last12MonthsSalesVolume", FormatCurrency(_CardVolumes.Rows[i][LAST12MONTHS_SALESVOLUME]));
            strExcelPartTemplate.Replace("CV_Last12MonthsSalesTransaction", FormatInteger(_CardVolumes.Rows[i][LAST12MONTHS_TRANSACTION]));
            strPartOutput.Append(strExcelPartTemplate.ToString());
        }
        strExcelTemplate.Replace("CV_DetailParts", strPartOutput.ToString());

        return strExcelTemplate.ToString();
    }

    private string UxExportVolumeAnalysisPre()
    {
        var strExcelTemplate = new StringBuilder(File.ReadAllText(Server.MapPath("~/App_Data/tpl_VolumeAnalysis.htm")));
        strExcelTemplate.Replace("[VolumeAnalysis]", GetLocalResourceObject("UxExportVolumeAnalysisResource1.Title").ToString());
        strExcelTemplate.Replace("[MTD]", GetLocalResourceObject("Literal6Resource1.Text").ToString());
        strExcelTemplate.Replace("[YTD]", GetLocalResourceObject("Literal7Resource1.Text").ToString());
        strExcelTemplate.Replace("[Last12Months]", GetLocalResourceObject("Literal8Resource1.Text").ToString());
        strExcelTemplate.Replace("[GrossSales]", GetLocalResourceObject("Literal9Resource1.Text").ToString());
        strExcelTemplate.Replace("[Transactions1]", GetLocalResourceObject("Literal10Resource1.Text").ToString());
        strExcelTemplate.Replace("[Returns]", GetLocalResourceObject("Literal11Resource1.Text").ToString());
        strExcelTemplate.Replace("[%Sales1]", GetLocalResourceObject("Literal12Resource1.Text").ToString());
        strExcelTemplate.Replace("[Chargebacks]", GetLocalResourceObject("Literal13Resource1.Text").ToString());
        strExcelTemplate.Replace("[Transactions2]", GetLocalResourceObject("Literal14Resource1.Text").ToString());
        strExcelTemplate.Replace("[%Sales2]", GetLocalResourceObject("Literal15Resource1.Text").ToString());
        strExcelTemplate.Replace("[Retrievals]", GetLocalResourceObject("Literal16Resource1.Text").ToString());
        strExcelTemplate.Replace("[Transactions3]", GetLocalResourceObject("Literal17Resource1.Text").ToString());
        strExcelTemplate.Replace("[%Sales3]", GetLocalResourceObject("Literal18Resource1.Text").ToString());
        strExcelTemplate.Replace("[Keyed]", GetLocalResourceObject("Literal19Resource1.Text").ToString());
        strExcelTemplate.Replace("[%Trans]", GetLocalResourceObject("Literal20Resource1.Text").ToString());
        strExcelTemplate.Replace("[NetVolume]", GetLocalResourceObject("Literal21Resource1.Text").ToString());

        GetParameters();
        GetSaleData();

        strExcelTemplate.Replace("VA_MTDSalesAmount", FormatCurrency(_SalesData.Rows[0][SALE_AMOUNT]));
        strExcelTemplate.Replace("VA_YTDSalesAmount", FormatCurrency(_SalesData.Rows[1][SALE_AMOUNT]));
        strExcelTemplate.Replace("VA_Last12MonthsSalesAmount", FormatCurrency(_SalesData.Rows[2][SALE_AMOUNT]));

        strExcelTemplate.Replace("VA_MTDSalesCount", FormatInteger(_SalesData.Rows[0][SALE_COUNT]));
        strExcelTemplate.Replace("VA_YTDSalesCount", FormatInteger(_SalesData.Rows[1][SALE_COUNT]));
        strExcelTemplate.Replace("VA_Last12MonthsSalesCount", FormatInteger(_SalesData.Rows[2][SALE_COUNT]));

        strExcelTemplate.Replace("VA_MTDReturnAmount", FormatCurrency(_SalesData.Rows[0][RETURN_AMOUNT]));
        strExcelTemplate.Replace("VA_YTDReturnAmount", FormatCurrency(_SalesData.Rows[1][RETURN_AMOUNT]));
        strExcelTemplate.Replace("VA_Last12MonthsReturnAmount", FormatCurrency(_SalesData.Rows[2][RETURN_AMOUNT]));

        strExcelTemplate.Replace("VA_MTDReturnPercent", FormatPercent(_SalesData.Rows[0][RETURN_PERCENT]));
        strExcelTemplate.Replace("VA_YTDReturnPercent", FormatPercent(_SalesData.Rows[1][RETURN_PERCENT]));
        strExcelTemplate.Replace("VA_Last12MonthsReturnPercent", FormatPercent(_SalesData.Rows[2][RETURN_PERCENT]));

        strExcelTemplate.Replace("VA_MTDChargebackAmount", FormatCurrency(_SalesData.Rows[0][CHARGEBACK_AMOUNT]));
        strExcelTemplate.Replace("VA_YTDChargebackAmount", FormatCurrency(_SalesData.Rows[1][CHARGEBACK_AMOUNT]));
        strExcelTemplate.Replace("VA_Last12MonthsChargebackAmount", FormatCurrency(_SalesData.Rows[2][CHARGEBACK_AMOUNT]));

        strExcelTemplate.Replace("VA_MTDChargebackCount", FormatInteger(_SalesData.Rows[0][CHARGEBACK_COUNT]));
        strExcelTemplate.Replace("VA_YTDChargebackCount", FormatInteger(_SalesData.Rows[1][CHARGEBACK_COUNT]));
        strExcelTemplate.Replace("VA_Last12MonthsChargebackCount", FormatInteger(_SalesData.Rows[2][CHARGEBACK_COUNT]));

        strExcelTemplate.Replace("VA_MTDChargeBackPercent", FormatPercent(_SalesData.Rows[0][CHARGEBACK_PERCENT]));
        strExcelTemplate.Replace("VA_YTDChargeBackPercent", FormatPercent(_SalesData.Rows[1][CHARGEBACK_PERCENT]));
        strExcelTemplate.Replace("VA_Last12MonthsChargebackPercent", FormatPercent(_SalesData.Rows[2][CHARGEBACK_PERCENT]));

        strExcelTemplate.Replace("VA_MTDRetrievalAmount", FormatCurrency(_SalesData.Rows[0][RETRIEVAL_AMOUNT]));
        strExcelTemplate.Replace("VA_YTDRetrievalAmount", FormatCurrency(_SalesData.Rows[1][RETRIEVAL_AMOUNT]));
        strExcelTemplate.Replace("VA_Last12MonthsRetrievalAmount", FormatCurrency(_SalesData.Rows[2][RETRIEVAL_AMOUNT]));

        strExcelTemplate.Replace("VA_MTDRetrievalCount", FormatInteger(_SalesData.Rows[0][RETRIEVAL_COUNT]));
        strExcelTemplate.Replace("VA_YTDRetrievalCount", FormatInteger(_SalesData.Rows[1][RETRIEVAL_COUNT]));
        strExcelTemplate.Replace("VA_Last12MonthsRetrievalCount", FormatInteger(_SalesData.Rows[2][RETRIEVAL_COUNT]));

        strExcelTemplate.Replace("VA_MTDRetrievalPercent", FormatPercent(_SalesData.Rows[0][RETRIEVAL_PERCENT]));
        strExcelTemplate.Replace("VA_YTDRetrievalPercent", FormatPercent(_SalesData.Rows[1][RETRIEVAL_PERCENT]));
        strExcelTemplate.Replace("VA_Last12MonthsRetrievalPercent", FormatPercent(_SalesData.Rows[2][RETRIEVAL_PERCENT]));

        strExcelTemplate.Replace("VA_MTDKeyedCount", FormatInteger(_SalesData.Rows[0][KEYED_COUNT]));
        strExcelTemplate.Replace("VA_YTDKeyedCount", FormatInteger(_SalesData.Rows[1][KEYED_COUNT]));
        strExcelTemplate.Replace("VA_Last12MonthsKeyedCount", FormatInteger(_SalesData.Rows[2][KEYED_COUNT]));

        strExcelTemplate.Replace("VA_MTDKeyedPercent", FormatPercent(_SalesData.Rows[0][KEYED_PERCENT]));
        strExcelTemplate.Replace("VA_YTDKeyedPercent", FormatPercent(_SalesData.Rows[1][KEYED_PERCENT]));
        strExcelTemplate.Replace("VA_Last12MonthsKeyedPercent", FormatPercent(_SalesData.Rows[2][KEYED_PERCENT]));

        strExcelTemplate.Replace("VA_MTDNetAmount", FormatCurrency(_SalesData.Rows[0][NET_AMOUNT]));
        strExcelTemplate.Replace("VA_YTDNetAmount", FormatCurrency(_SalesData.Rows[1][NET_AMOUNT]));
        strExcelTemplate.Replace("VA_Last12MonthsNetAmount", FormatCurrency(_SalesData.Rows[2][NET_AMOUNT]));

        if (IsFultonClient)
        {
            strExcelTemplate.Replace("VA_RETURNSCOUNT", string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>", GetLocalResourceObject("Literal38Resource1.Text").ToString(), FormatInteger(_SalesData.Rows[0][RETURN_COUNT]), FormatInteger(_SalesData.Rows[1][RETURN_COUNT]), FormatInteger(_SalesData.Rows[2][RETURN_COUNT])));
            strExcelTemplate.Replace("VA_KEYEDAMOUNT", string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>", GetLocalResourceObject("Literal39Resource1.Text").ToString(), FormatCurrency(_SalesData.Rows[0][KEYED_AMOUNT]), FormatCurrency(_SalesData.Rows[1][KEYED_AMOUNT]), FormatCurrency(_SalesData.Rows[2][KEYED_AMOUNT])));
        }
        else
        {
            strExcelTemplate.Replace("VA_RETURNSCOUNT", string.Empty);
            strExcelTemplate.Replace("VA_KEYEDAMOUNT", string.Empty);
        }

        return strExcelTemplate.ToString();
    }

    //remove
    private string UxExportTotalMerchantPre()
    {
        StringBuilder strExcelTemplate = new StringBuilder(File.ReadAllText(Server.MapPath("~/App_Data/tpl_TotalMerchant.htm")));
        GetParameters();
        if (SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.Merchant)
            _HasMerchantAppRptPermission = false;
        else
            _HasMerchantAppRptPermission = true;

        BindMerchantApproval();
        strExcelTemplate.Replace("TM_TotalOpenMerchants", _TotalOpenMerchants);
        strExcelTemplate.Replace("TM_MerchantApprovals", _MerchantApprovals);

        return strExcelTemplate.ToString();
    }

    private void DownloadFile(string fileName, string filePath, string fileType)
    {
        var encoding = new ASCIIEncoding();
        Response.ClearContent();
        try
        {
            Response.ClearHeaders();
        }
        catch (Exception ex)
        {
            AS.Common.Logger.LoggerManager.Error("ClearHeaders: Dashboard - DownloadFile:\n" + ex.ToString());
        }
        Response.BufferOutput = true;
        Response.Charset = "utf-8";
        Response.HeaderEncoding = UnicodeEncoding.UTF8;
        switch (fileType)
        {
            case "xls":
                Response.ContentType = "application/vnd.ms-excel";
                if (Request.Browser.Browser.ToUpper() == WebSiteConstants.BROWSER_INTERNETEXPLORER)
                {
                    string attachment = string.Format("attachment; filename={0}", Server.UrlPathEncode(fileName) + ".xls");
                    Response.AppendHeader("Content-Disposition", attachment);
                }
                else
                {
                    Response.AppendHeader("Content-Disposition", "attachment; filename=\"" + fileName + ".xls\"");
                }
                break;
            case "csv":
                Response.ContentType = "text/csv";
                if (Request.Browser.Browser.ToUpper() == WebSiteConstants.BROWSER_INTERNETEXPLORER)
                {
                    string attachment = string.Format("attachment; filename={0}", Server.UrlPathEncode(fileName) + ".csv");
                    Response.AppendHeader("Content-Disposition", attachment);
                }
                else
                    Response.AppendHeader("Content-Disposition", "attachment; filename=\"" + fileName + ".csv\"");
                break;
            case "pdf":
                Response.ContentType = "application/pdf";
                if (Request.Browser.Browser.ToUpper() == WebSiteConstants.BROWSER_INTERNETEXPLORER)
                {
                    string attachment = string.Format("attachment; filename={0}", Server.UrlPathEncode(fileName) + ".pdf");
                    Response.AppendHeader("Content-Disposition", attachment);
                }
                else
                    //TODO: remember to do veracode VeraCodeSolution.RemoveCRLF(header)
                    Response.AppendHeader("Content-Disposition", "attachment; filename=\"" + fileName + ".pdf\"");
                break;
        }

        var fileStream = new FileStream(filePath, FileMode.Open);
        int length = (int)fileStream.Length;
        var buffer = new byte[length];
        int bytesRead = fileStream.Read(buffer, 0, length);
        Response.BinaryWrite(buffer);
        Response.Flush();
        // Close response
        fileStream.Close();
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        Response.End();
    }

    private void ExportExcel(string exportedContent, string title, string subtitle)
    {
        string excelFileName = GeneralFuncsLib.GetFileName(title + subtitle);
        File.WriteAllText(Server.MapPath("~/App_Data/" + excelFileName + ".xls"), exportedContent, Encoding.UTF8);
        DownloadFile(excelFileName, Server.MapPath("~/App_Data/" + excelFileName + ".xls"), "xls");
    }

    private void ExportPdf(string exportedContent, string title, string subtitle)
    {
        string pdfFileName = GeneralFuncsLib.GetFileName(title + subtitle);
        Create_PDF_File(exportedContent, Server.MapPath("~/App_Data/" + pdfFileName + ".pdf"));
        DownloadFile(pdfFileName, Server.MapPath("~/App_Data/" + pdfFileName + ".pdf"), "pdf");
    }

    private void Create_PDF_File(string html, string fileName)
    {
        var theDoc = new Doc();
        var theID = 0;
        // Set up document
        html = html.Trim();
        theDoc.Rect.String = theDoc.MediaBox.String = "A4";
        theID = theDoc.AddImageHtml(html, true, 0, false);
        theDoc.Save(fileName);
        theDoc.Clear();
    }
    #endregion

    #region FORMAT METHODS
    protected string FormatCurrencyForCSV(object abc)
    {
        if (abc == DBNull.Value)
            abc = 0.00;
        if (decimal.Parse(abc.ToString()) < 0)
        {
            return "(" + AS.Common.Formater.FormatData.FormatCurrency(decimal.Parse(abc.ToString()) * -1, _currency) + ")";
        }
        return AS.Common.Formater.FormatData.FormatCurrency(abc, _currency);
    }

    protected string FormatString(object abc)
    {
        if (abc == DBNull.Value)
            abc = string.Empty;
        return abc.ToString();
    }

    protected string FormatCurrency(object abc)
    {
        if (abc == DBNull.Value)
            abc = 0.00;
        return VeraCodeSolution.DoVeraCode(FormatData.FormatCurrency(abc, _currency));
    }

    protected string FormatInteger(object abc)
    {
        if (abc == DBNull.Value)
            abc = 0;
        return VeraCodeSolution.DoVeraCode(FormatData.FormatInteger(abc));
    }

    protected string FormatInteger(object a, object b)
    {
        object result = 0;
        if (a == DBNull.Value && b == DBNull.Value)
            result = 0;
        else if (a == DBNull.Value)
            result = b;
        else if (b == DBNull.Value)
            result = a;
        else result = (int)a + (int)b;
        return VeraCodeSolution.DoVeraCode(FormatData.FormatInteger(result));
    }

    protected string FormatPercent(object abc, bool htmlFormat = false)
    {
        if (abc == DBNull.Value)
            abc = 0.00;

        string strPercent = htmlFormat ? "<sup>%</sup>" : "%";
        return VeraCodeSolution.DoVeraCode(FormatData.FormatNumber(abc, 2) + strPercent);
    }

    protected string ShowToDateVolume(DataRow row)
    {
        return Convert.ToInt32(row["Months"]) <= DateTime.Today.Month ? VeraCodeSolution.DoVeraCode(FormatCurrency(row["Volume"])) : string.Empty;
    }

    protected string ShowToDateTransaction(DataRow row)
    {
        return Convert.ToInt32(row["Months"]) <= DateTime.Today.Month ? VeraCodeSolution.DoVeraCode(FormatInteger(row["TransactionCount"])) : string.Empty;
    }
    #endregion


    protected bool ShowTabByPermission(string permissions, int matchItem = 1)
    {
        string[] listPermissions = permissions.Split(',');
        int count = 0;

        for (int i = 0; i < listPermissions.Count(); i++)
        {
            if (SessionManager.CurrentUserPermissions.Contains("," + listPermissions[i] + ","))
                count++;

        }
        if (count >= matchItem)
            return true;
        else
            return false;

    }
    #endregion

    #region NEW DATATABLE
    private DataTable NewYTDDataDataTable()
    {
        DataTable tbl = new DataTable();
        tbl.Columns.Add("Months", System.Type.GetType("System.String"));
        tbl.Columns.Add("Volume", System.Type.GetType("System.Decimal"));
        tbl.Columns.Add("TransactionCount", System.Type.GetType("System.Int64"));
        return tbl;
    }

    private DataTable NewSalesDataDataTable()
    {
        DataTable tbl = new DataTable();
        tbl.Columns.Add("SaleVolume", System.Type.GetType("System.Decimal"));
        tbl.Columns.Add("SaleTrans", System.Type.GetType("System.Int64"));
        tbl.Columns.Add("ReturnAmount", System.Type.GetType("System.Decimal"));
        tbl.Columns.Add("ReturnTrans", System.Type.GetType("System.Int64"));
        tbl.Columns.Add("ReturnSaleVolumePercent", System.Type.GetType("System.Decimal"));
        tbl.Columns.Add("KeyedVolumePercent", System.Type.GetType("System.Decimal"));
        tbl.Columns.Add("KeyedTransPercent", System.Type.GetType("System.Decimal"));
        tbl.Columns.Add("RetrievalVolume", System.Type.GetType("System.Decimal"));
        tbl.Columns.Add("RetrievalTrans", System.Type.GetType("System.Int64"));
        tbl.Columns.Add("RetrievalSaleVolumePercent", System.Type.GetType("System.Decimal"));
        tbl.Columns.Add("ChargeBackVolume", System.Type.GetType("System.Decimal"));
        tbl.Columns.Add("ChargeBackTrans", System.Type.GetType("System.Int64"));
        tbl.Columns.Add("ChargeBackSaleVolumePercent", System.Type.GetType("System.Decimal"));
        tbl.Columns.Add("NetVolume", System.Type.GetType("System.Decimal"));
        return tbl;
    }

    private DataTable NewCardVolumesDataTable()
    {
        var tbl = new DataTable();
        tbl.Columns.Add(CARD_TYPE, System.Type.GetType("System.String"));
        tbl.Columns.Add(MTD_SALESVOLUME, System.Type.GetType("System.Decimal"));
        tbl.Columns.Add(MTD_TRANSACTION, System.Type.GetType("System.Int64"));
        tbl.Columns.Add(YTD_SALESVOLUME, System.Type.GetType("System.Decimal"));
        tbl.Columns.Add(YTD_TRANSACTION, System.Type.GetType("System.Int64"));
        tbl.Columns.Add(LAST12MONTHS_SALESVOLUME, System.Type.GetType("System.Decimal"));
        tbl.Columns.Add(LAST12MONTHS_TRANSACTION, System.Type.GetType("System.Int64"));
        return tbl;
    }
    #endregion

    #region WEB METHOD
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static void UpdateClientFrameInfo(string clientFrameInfo)
    {
        if (GeneralFuncsLib.IsIFrameSupported())
        {
            SessionManager.ClientFrameInfo = clientFrameInfo;
        }
    }
    #endregion

    #region Notification

    [System.Web.Services.WebMethod(EnableSession = true)]
    //[ScriptMethod(UseHttpGet = true)]
    public static List<AlertItem> GetAlertFeed(int pageNo, int pageSize, bool isPaging, int maxIndex, string sourceIDs, string statusIDs, string sDescription)
    {
        try
        {
            NotificationAlertClient proxy = new NotificationAlertClient();
            string sortBy = string.Empty;
            sDescription = sDescription == "1" ? "DESC" : "ASC";
            if (statusIDs.Equals("-1")) statusIDs = "";
            if (sourceIDs.Equals("-1")) sourceIDs = "";
            List<AlertItem> alertlist = proxy.GetAlert(sourceIDs, SessionManager.CurrentClient, SessionManager.CurrentUser.RecId.ToString(), true,
                statusIDs, isPaging, pageNo, pageSize, maxIndex, sortBy, sDescription).AlertMessages;
            return alertlist;
        }
        catch (ThreadAbortException)
        {
            return new List<AlertItem>();
        }
        catch (Exception ex)
        {
            LoggerManager.Error("GetAlertFeed: " + ex);
            return new List<AlertItem>();
        }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true)]
    public static List<AlertSourceItem> GetAlertSources()
    {
        NotificationAlertClient proxy = new NotificationAlertClient();
        List<AlertSourceItem> alertlist = proxy.GetAlertSources(SessionManager.CurrentClient, SessionManager.CurrentUser.RecId.ToString()).SourceAppMessages;
        return alertlist;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true)]
    public static List<AlertItem> GetAlert()
    {
        NotificationAlertClient proxy = new NotificationAlertClient();
        string sourceIDs = string.Empty;
        string statusIDs = string.Empty;
        List<AlertItem> alertlist = proxy.GetAlert(sourceIDs, SessionManager.CurrentClient, SessionManager.CurrentUser.RecId.ToString(), false, statusIDs).AlertMessages;
        //return proxy.GetAlert();
        return alertlist.OrderBy(x => x.IsImportant).ToList().OrderBy(x => x.CreatedDTS).ToList();
    }

    [WebMethod(EnableSession = true)]
    public static int UpdateAlert(string alertId, int status, string sourceIDs)
    {
        if (sourceIDs.Equals("-1")) sourceIDs = "";
        NotificationAlertClient proxy = new NotificationAlertClient();
        return proxy.UpdateAlert(alertId, status, sourceIDs, SessionManager.CurrentUser.RecId.ToString(), SessionManager.CurrentClient);
    }

    [WebMethod(EnableSession = true)]
    public static int DeleteGroupAlert(string fDate, string tDate, string sourceIDs)
    {
        NotificationAlertClient proxy = new NotificationAlertClient();
        DateTime FDate = DateTime.Parse(fDate);
        DateTime TDate = DateTime.Parse(tDate);
        if (sourceIDs.Equals("-1")) sourceIDs = "";

        return proxy.DeleteAlertGroup(SessionManager.CurrentClient, SessionManager.CurrentUser.RecId.ToString(), FDate,
            TDate, sourceIDs);
    }

    #endregion
}

public class NotificationAlertClient : WcfClient<INotificationAlertServices>
{
    public ResponseAlertMessage GetAlert(string sourceIDs, int clientid, string recipientId, bool isFeed, string statusIDs, bool isPaging = false, int pageNo = 1, int pageSize = 10, int maxIndex = -1, string sortBy = "", string sortDes = "desc")
    {
        ResponseAlertMessage alerts = Proxy.GetAlertMessage(sourceIDs, clientid, recipientId, isFeed, statusIDs, -1, isPaging, pageNo, pageSize, maxIndex, sortBy, sortDes);
        return alerts;
    }

    public ResponseSourceAppMessage GetAlertSources(int clientid, string recipientId)
    {
        ResponseSourceAppMessage alerts = Proxy.GetSources(clientid, recipientId);
        return alerts;
    }

    public int UpdateAlert(string iDs, int status, string sourceIDs, string recipientId, int clientid)
    {
        Proxy.UpdateAlertStatus(status, iDs, sourceIDs, recipientId, clientid);
        return 1;
    }

    public int DeleteAlertGroup(int clientid, string recipientId, DateTime fDate, DateTime tDate, string sourceIDs = null)
    {
        Proxy.DeleteAlertGroup(clientid, recipientId, fDate, tDate, sourceIDs);
        return 1;
    }

}

