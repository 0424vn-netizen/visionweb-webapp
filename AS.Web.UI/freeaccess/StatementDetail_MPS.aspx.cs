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
using Dundas.Charting.WebControl;
using WebSupergoo.ABCpdf9;

public partial class StatementDetail_MPS : ReportPage
{

    #region Properties
    string MerchantNumber = "";
    protected DateTime ReportDate = DateTime.Now;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsStaticQueryString && !IsPostBack)
        {
            //this.MerchantNumber = SecureQueryString["MerchantNumber"];
            //string _reportdate = SecureQueryString["ReportDate"];
            DateTime tempDate = new DateTime(2012, 10, 31);
            this.MerchantNumber = "216";
            string _reportdate = tempDate.Ticks.ToString();
            this.ReportDate = new DateTime(long.Parse(_reportdate));
            BindData();
            BindMerchantAccountInformationSummary();
            BindVolumeCardType();
            BindYTD();
        }

    }
    protected void BindData()
    {
        DataTable tbl = new DataTable();
        tbl = GetMerchantInfomation();
        uxMerchantInfo.DataSource = tbl;
        uxMerchantInfo.DataBind();

        uxDeposits.DataSource = GetDepositDetail();
        uxDeposits.DataBind();

        uxDeposits_BatchTotal.DataSource = GetDepositDetail_BatchTotal();
        uxDeposits_BatchTotal.DataBind();

        uxDeposits_Adjustment.DataSource = GetDepositDetail_Adjustment();
        uxDeposits_Adjustment.DataBind();

        uxAdjustment.DataSource = new DataTable();
        uxAdjustment.DataBind();

        uxSettelement.DataSource = GetSettlementDiscount();
        uxSettelement.DataBind();


        uxSettelement_Total.DataSource = GetSettlementDiscount_Total();
        uxSettelement_Total.DataBind();

        uxProducts.DataSource = GetProductsServices();
        uxProducts.DataBind();

        uxProducts_Total.DataSource = GetProductsServices_Total();
        uxProducts_Total.DataBind();

        uxDue.DataSource = GetDuesAssessments();
        uxDue.DataBind();

        uxDue_Total.DataSource = GetDuesAssessments_Total();
        uxDue_Total.DataBind();

        uxInterchange.DataSource = GetInterchange();
        uxInterchange.DataBind();

        uxInterchange_Total.DataSource = GetInterchange_Total();
        uxInterchange_Total.DataBind();

    }


    #region get SPAs
    private DataTable GetMerchantInfomation()
    {
        FilterParameterCollection _Parameters = AddLoggedInUserParams();
        _Parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
        _Parameters.Add("@ReportDate", ReportDate, DbType.DateTime);
        return WebServices.CsReportServices_MPS.GetReports("spa_stmnt_GetMerchantHeaderInformation", _Parameters);
    }
    private DataTable GetSettlementDiscount()
    {
        FilterParameterCollection _Parameters = AddLoggedInUserParams();
        _Parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
        _Parameters.Add("@ReportDate", ReportDate, DbType.DateTime);
        return WebServices.CsReportServices_MPS.GetReports("spa_stmnt_GetMerchantSettleDiscount", _Parameters);
    }
    private DataTable GetSettlementDiscount_Total()
    {
        FilterParameterCollection _Parameters = AddLoggedInUserParams();
        _Parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
        _Parameters.Add("@ReportDate", ReportDate, DbType.DateTime);
        return WebServices.CsReportServices_MPS.GetReports("spa_stmnt_GetMerchantSettleDiscount_Total", _Parameters);
    }

    private DataTable GetProductsServices()
    {
        FilterParameterCollection _Parameters = AddLoggedInUserParams();
        _Parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
        _Parameters.Add("@ReportDate", ReportDate, DbType.DateTime);
        return WebServices.CsReportServices_MPS.GetReports("spa_stmnt_GetMerchantProductService", _Parameters);
    }
    private DataTable GetProductsServices_Total()
    {
        FilterParameterCollection _Parameters = AddLoggedInUserParams();
        _Parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
        _Parameters.Add("@ReportDate", ReportDate, DbType.DateTime);
        return WebServices.CsReportServices_MPS.GetReports("spa_stmnt_GetMerchantProductService_Total", _Parameters);
    }
    private DataTable GetDuesAssessments()
    {
        FilterParameterCollection _Parameters = AddLoggedInUserParams();
        _Parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
        _Parameters.Add("@ReportDate", ReportDate, DbType.DateTime);
        return WebServices.CsReportServices_MPS.GetReports("spa_stmnt_GetMerchantDuesAssessments", _Parameters);
    }
    private DataTable GetDuesAssessments_Total()
    {
        FilterParameterCollection _Parameters = AddLoggedInUserParams();
        _Parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
        _Parameters.Add("@ReportDate", ReportDate, DbType.DateTime);
        return WebServices.CsReportServices_MPS.GetReports("spa_stmnt_GetMerchantDuesAssessments_Total", _Parameters);
    }
    private DataTable GetInterchange()
    {
        FilterParameterCollection _Parameters = AddLoggedInUserParams();
        _Parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
        _Parameters.Add("@ReportDate", ReportDate, DbType.DateTime);
        return WebServices.CsReportServices_MPS.GetReports("spa_stmnt_GetMerchantInterchange", _Parameters);
    }
    private DataTable GetInterchange_Total()
    {
        FilterParameterCollection _Parameters = AddLoggedInUserParams();
        _Parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
        _Parameters.Add("@ReportDate", ReportDate, DbType.DateTime);
        return WebServices.CsReportServices_MPS.GetReports("spa_stmnt_GetMerchantInterchange_Total", _Parameters);
    }
    private DataTable GetDepositDetail()
    {
        FilterParameterCollection _Parameters = AddLoggedInUserParams();
        _Parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
        _Parameters.Add("@ReportDate", ReportDate, DbType.DateTime);
        return WebServices.CsReportServices_MPS.GetReports("spa_stmnt_GetMerchantDepositDetail", _Parameters);
    }
    private DataTable GetDepositDetail_BatchTotal()
    {
        FilterParameterCollection _Parameters = AddLoggedInUserParams();
        _Parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
        _Parameters.Add("@ReportDate", ReportDate, DbType.DateTime);
        return WebServices.CsReportServices_MPS.GetReports("spa_stmnt_GetMerchantDepositDetail_Total", _Parameters);
    }
    private DataTable GetDepositDetail_Adjustment()
    {
        FilterParameterCollection _Parameters = AddLoggedInUserParams();
        _Parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
        _Parameters.Add("@ReportDate", ReportDate, DbType.DateTime);
        return WebServices.CsReportServices_MPS.GetReports("spa_stmnt_GetMerchantAdjustmentCredits", _Parameters);
    }
    #endregion

    #region BindChart
    private void BindVolumeCardType()
    {
        FilterParameterCollection _Parameters = AddLoggedInUserParams();
        _Parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
        _Parameters.Add("@ReportDate", ReportDate, DbType.DateTime);

        DataTable dtChart = WebServices.CsReportServices_MPS.GetReports("spa_stmnt_GetMerchantAccountVolumeChart", _Parameters);

        if (dtChart == null || dtChart.Rows.Count <= 0)
        {
            uxVolumeCardTypeChart.Series["Default"].Points.Clear();
            return;
        }

        for (int i = 0; i < dtChart.Rows.Count; i++)
        {
            DataRow dr = dtChart.Rows[i];
            DataPoint point = new DataPoint();
            point.LegendText = dtChart.Rows[i]["CardType"].ToString();
            if (Convert.ToDouble(dr["PercentVolume"]) == 0) point.Empty = true;
            point.YValues[0] = Convert.ToDouble(dr["PercentVolume"]);

            uxVolumeCardTypeChart.Series["Default"].Points.Add(point);
        }


    }

    private void BindYTD()
    {
        FilterParameterCollection _Parameters = AddLoggedInUserParams();
        _Parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
        _Parameters.Add("@ReportDate", ReportDate, DbType.DateTime);

        DataTable _YTDData = WebServices.CsReportServices_MPS.GetReports("spa_stmnt_GetMerchantAccount12MonthVolumeChart", _Parameters);
        int MonthsNumber = 12;
        //_YTDData.Rows.RemoveAt(0);
        //_YTDData.Rows.RemoveAt(0);

        // Fill 12 months        
        //if (_YTDData == null) _YTDData = NewYTDDataDataTable();
        //DataTable FullMonthsYTD = NewYTDDataDataTable();
        //string MonthIndicator = string.Empty;
        decimal _YTDTotalVolume = 0;
        for (int i = 0; i < MonthsNumber; i++)
        {
            _YTDTotalVolume += (decimal)_YTDData.Rows[i]["Volume"];

        }
        //DataTable _YTDFullMonths = FullMonthsYTD;

        //uxVolumeYTDChart.Series["Default"].Points.Clear();

        //int MonthsNumber = DateTime.Today.Month;

        object[] xValues = new object[MonthsNumber];
        object[] yVolumes = new object[MonthsNumber];
        for (int i = 0; i < MonthsNumber; i++)
        {
            xValues[i] = _YTDData.Rows[i]["Month"];
        }

        for (int i = 0; i < MonthsNumber; i++)
        {
            yVolumes[i] = _YTDData.Rows[i]["Volume"];
        }

        uxVolumeYTDChart.Series["Default"].Points.Clear();
        uxVolumeYTDChart.Series["Default"].Points.DataBindXY(xValues, yVolumes);

        if (_YTDTotalVolume == 0)
        {
            uxVolumeYTDChart.ChartAreas["Area1"].AxisY.Enabled = AxisEnabled.True;
            uxVolumeYTDChart.ChartAreas["Area1"].AxisY.StartFromZero = true;
            uxVolumeYTDChart.ChartAreas["Area1"].AxisY.Minimum = 0;
            uxVolumeYTDChart.ChartAreas["Area1"].AxisY.Maximum = 4000;
            uxVolumeYTDChart.ChartAreas["Area1"].AxisY.Interval = 2000;
            uxVolumeYTDChart.ChartAreas["Area1"].AxisX.LabelsAutoFit = true;
            uxVolumeYTDChart.ChartAreas["Area1"].AxisX.LabelsAutoFitStyle = LabelsAutoFitStyle.OffsetLabels;
        }

    }


    private void BindMerchantAccountInformationSummary()
    {
        FilterParameterCollection _Parameters = AddLoggedInUserParams();
        _Parameters.Add("@MerchantNumber", MerchantNumber, DbType.String);
        _Parameters.Add("@ReportDate", ReportDate, DbType.DateTime);

        DataTable merchantAccountInformationSummary = WebServices.CsReportServices_MPS.GetReports("spa_stmnt_GetMerchantAccountInformationSummary", _Parameters);

        uxMerchantAccountInformationSummary.DataSource = merchantAccountInformationSummary;
        uxMerchantAccountInformationSummary.DataBind();

    }
    private DataTable GetDataSource(string spaName)
    {
        ReportServices service = WebServices.MsReportServices; //
        FilterParameterCollection _Parameters = new FilterParameterCollection();
        _Parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        return service.GetReports(spaName, _Parameters);
    }
    private DataTable NewYTDDataDataTable()
    {
        DataTable tbl = new DataTable();
        tbl.Columns.Add("Months", System.Type.GetType("System.String"));
        tbl.Columns.Add("Volume", System.Type.GetType("System.Decimal"));
        tbl.Columns.Add("TransactionCount", System.Type.GetType("System.Int32"));
        return tbl;
    }

    #endregion

    public FilterParameterCollection AddLoggedInUserParams()
    {
        FilterParameterCollection source = new FilterParameterCollection();
        string userMode = "";
        //switch (SessionManager.CurrentUserType)
        //{
        //    case WebSiteEnums.UserHierarchyMode.CS:
        //    case WebSiteEnums.UserHierarchyMode.AS:
        //        userMode = "CSUSER";
        //        break;
        //    case WebSiteEnums.UserHierarchyMode.Site:
        //        userMode = "CLIENT";
        //        break;
        //    case WebSiteEnums.UserHierarchyMode.Hierarchy:
        //    case WebSiteEnums.UserHierarchyMode.Merchant:
        //    case WebSiteEnums.UserHierarchyMode.Headquarter:
        //        userMode = GeneralFuncsLib.GetHierarchyInfo(SessionManager.CurrentUser.EntityType).UserMode;
        //        break;
        //}
        userMode = "CSUSER";
        source.Add(new AS.Common.DBManager.FilterParameter("@UserMode", userMode, DbType.AnsiString));
        source.Add(new AS.Common.DBManager.FilterParameter("@UserID", "asadmin", DbType.AnsiString));
        source.Add(new AS.Common.DBManager.FilterParameter("@ASClient", 23, DbType.Int32));

        return source;
    }
    public FilterParameterCollection AddLoggedInUserParams(int siteID)
    {
        FilterParameterCollection source = AddLoggedInUserParams();
        if (siteID >= 0)
            source.Add(new AS.Common.DBManager.FilterParameter("@SiteID", siteID, DbType.Int32));

        return source;
    }

}
