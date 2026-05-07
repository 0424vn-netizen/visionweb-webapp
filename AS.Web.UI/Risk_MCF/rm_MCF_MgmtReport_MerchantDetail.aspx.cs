using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Exporter;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Controls.UserControls;
using AS.Web.Business;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

[PagePermission("RskManMerch,MSRskManMerch")]
public partial class rm_MCF_MgmtReport_MerchantDetail : ReportPage
{
    #region Enums

    enum DataBindAction
    {
        BindReportGrid,
    }

    enum PostBackAction
    {
        MerchantNumberClick,
        MerchantNameClick,
        DoFilterAction,
    }

    #endregion

    #region Fields

    private string _MerchantNumber;
    private const string SESSION_GRID = "Session_MgmtReport_MerchantDetail_uxReportGrid";
    private string _merchantProfileIntruderQuery = string.Empty;

    #endregion Fields

    #region Properties

    private string MerchantProfileIntruderQuery
    {
        get
        {
            if (_merchantProfileIntruderQuery.Length == 0)
            {
                _merchantProfileIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(
                    this.ID, new string[] { "MerchantNumber" });
            }
            return _merchantProfileIntruderQuery;
        }
    }

    #endregion Properties

    #region Methods

    protected override void PageInitialize()
    {
        base.PageInitialize();
        IsBindDataOnLoad = true;
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session[SESSION_GRID] != null)
            {
                string[] parts = GeneralFuncsLib.NvlString(Session[SESSION_GRID]).Split(';');
                int tempInt = 0;
                Int32.TryParse(parts[0], out tempInt);
                uxReportGrid.MasterTableView.CurrentPageIndex = tempInt;
                uxReportGrid.AS_SortExpression = parts[1];
            }
        }

        //46652 - AW - Multi-Currency Transaction Display
        uxReportGrid.Columns.FindByUniqueName("ParameterIndicator").HeaderText =
            uxReportGrid.Columns.FindByUniqueName("ParameterIndicator").HeaderText.ToCurrencySymbol();
        uxReportGrid.Columns.FindByUniqueName("ParameterIndicator").HeaderTooltip =
            uxReportGrid.Columns.FindByUniqueName("ParameterIndicator").HeaderTooltip.ToCurrencySymbol();
        uxReportGrid.Columns.FindByUniqueName("ParameterIndicatorText").HeaderText =
            uxReportGrid.Columns.FindByUniqueName("ParameterIndicatorText").HeaderText.ToCurrencySymbol();
        uxReportGrid.Columns.FindByUniqueName("ParameterIndicatorText").HeaderTooltip =
            uxReportGrid.Columns.FindByUniqueName("ParameterIndicatorTextCSV").HeaderTooltip.ToCurrencySymbol();
        uxReportGrid.Columns.FindByUniqueName("ParameterIndicatorTextCSV").HeaderText =
            uxReportGrid.Columns.FindByUniqueName("ParameterIndicatorTextCSV").HeaderText.ToCurrencySymbol();
        uxReportGrid.Columns.FindByUniqueName("ParameterIndicatorTextCSV").HeaderTooltip =
            uxReportGrid.Columns.FindByUniqueName("ParameterIndicatorTextCSV").HeaderTooltip.ToCurrencySymbol();
    }

    protected override void OnPreRender(EventArgs e)
    {
        RiskSessionManager.RiskMgmtReportFilter.KeepSession = false;
        base.OnPreRender(e);
    }

    protected override void DoGridNeedDataSource(ASGrid sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if (sender == uxReportGrid && sender.Visible)
        {
            Session[SESSION_GRID] = string.Format("{0};{1}", sender.MasterTableView.CurrentPageIndex.ToString(), sender.AS_SortExpression);
            OnDataBindControls(DataBindAction.BindReportGrid, sender);
        }
    }

    protected override void DoItemDataBound(ASGrid sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (IsIntruderDetected)
            return;
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = (GridDataItem)e.Item;
            DataRowView dataRow = e.Item.DataItem as DataRowView;
            int parameterPrecision = int.Parse(GeneralFuncsLib.NvlString(dataRow["ParameterPrecision"]));
            string indicatorFormat = "#,##0" + (parameterPrecision > 0 ? ".".PadRight(parameterPrecision + 1, '0') : string.Empty);

            string parameterIndicator = GeneralFuncsLib.NvlString(dataRow["ParameterIndicator"]);
            string actualValue = GeneralFuncsLib.NvlString(dataRow["ParameterIndicator_ActualValue"]);
            string dataType = GeneralFuncsLib.NvlString(dataRow["ParameterDataType"]).ToLower();

            string parameterThreshold = GeneralFuncsLib.NvlString(dataRow["ParameterThreshold"]);
            string actualThreshold = GeneralFuncsLib.NvlString(dataRow["ParameterThreshold_ActualValue"]);
            string thresholdType = GeneralFuncsLib.NvlString(dataRow["ThresholdType"]).ToLower();

            string parameterThresholdHigh = GeneralFuncsLib.NvlString(dataRow["ParameterThresholdHigh"]);
            string parameterThresholdType = GeneralFuncsLib.NvlString(dataRow["ParameterThresholdType"]);


            if (!parameterIndicator.Equals("0") && !parameterIndicator.IsNullOrEmpty())
            {
                parameterIndicator = decimal.Parse(parameterIndicator).ToString(indicatorFormat);
            }
            if (!actualValue.Equals("0") && !actualValue.IsNullOrEmpty())
            {
                actualValue = decimal.Parse(actualValue).ToString(indicatorFormat);
            }

            if (!parameterThreshold.Equals("0") && !parameterThreshold.IsNullOrEmpty())
            {
                parameterThreshold = decimal.Parse(parameterThreshold).ToString(indicatorFormat);
            }
            if (!actualThreshold.Equals("0") && !String.IsNullOrEmpty(actualThreshold))
            {
                actualThreshold = decimal.Parse(actualThreshold).ToString(indicatorFormat);
            }
            if (!parameterThresholdHigh.Equals("0") && !parameterThresholdHigh.IsNullOrEmpty())
            {
                parameterThresholdHigh = decimal.Parse(parameterThresholdHigh).ToString(indicatorFormat);
            }

            //[44432]: Bug #37842
            //process indicator
            dataItem["ParameterIndicator"].Text = VeraCodeSolution.ValidateResponseData(
                GeneralFuncsLib.FormatParameterDataType(dataType, parameterIndicator, parameterPrecision));
            dataItem["ActualValue"].Text = VeraCodeSolution.ValidateResponseData(
                             GeneralFuncsLib.FormatParameterDataType(dataType, actualValue, parameterPrecision));


            if (!parameterIndicator.IsNullOrEmpty())
            {
                if (decimal.Parse(parameterIndicator) < 0)
                {
                    dataItem["ParameterIndicator"].Style.Add("color", "Red");
                }
            }
            if (!actualValue.IsNullOrEmpty())
            {
                if (decimal.Parse(actualValue) < 0)
                {
                    dataItem["ActualValue"].Style.Add("color", "Red");
                }
            }

            switch (parameterThresholdType)
            {
                case "LowHigh":
                    dataItem["ParameterThreshold"].Text = VeraCodeSolution.ValidateResponseData(
                        GeneralFuncsLib.FormatLowHighThreshold(parameterThreshold, parameterThresholdHigh, thresholdType));
                    break;
                default:
                    dataItem["ParameterThreshold"].Text = VeraCodeSolution.ValidateResponseData(
                        GeneralFuncsLib.FormatParameterDataType(thresholdType, parameterThreshold));
                    break;
            }
            dataItem["ActualThreshold"].Text = VeraCodeSolution.ValidateResponseData(
                             GeneralFuncsLib.FormatParameterDataType(thresholdType, actualThreshold));

            if (!parameterThreshold.IsNullOrEmpty())
            {
                if (decimal.Parse(parameterThreshold) < 0)
                {
                    dataItem["ParameterThreshold"].Style.Add("color", "Red");
                }
            }
            if (!actualThreshold.IsNullOrEmpty())
            {
                if (decimal.Parse(actualThreshold) < 0)
                {
                    dataItem["ActualThreshold"].Style.Add("color", "Red");
                }
            }

            // 10/10/2018 |41044 PIVOT New Parameter
            string topCard = getTopCardText(dataRow["ParameterThreshold_ActualValue1"].ToSafeString());
            if (!topCard.IsNullOrEmpty())
            {
                dataItem["ActualThreshold"].Text += "<br/>" + topCard;
            }
        }
    }

    protected void uxMerchantName_Command(object sender, CommandEventArgs e)
    {
        RiskSessionManager.RiskMgmtReportFilter.KeepSession = true;
        _MerchantNumber = e.CommandArgument != null ? e.CommandArgument.ToString() : string.Empty;
        if (e.CommandName == "MerchantNameClick")
        {
            OnPostBackActions(PostBackAction.MerchantNameClick);
        }
        else if (e.CommandName == "MerchantNumberClick")
        {
            OnPostBackActions(PostBackAction.MerchantNumberClick);
        }
    }

    protected DataTable DoInitializeExport(DataTable dt)
    {
        dt.Columns.Add("ParameterIndicatorText");
        dt.Columns.Add("ActualValueText");
        dt.Columns.Add("ParameterThresholdText");
        dt.Columns.Add("ActualThresholdText");
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            int parameterPrecision = int.Parse(GeneralFuncsLib.NvlString(dt.Rows[i]["ParameterPrecision"]));
            string indicatorFormat = "#,##0" + (parameterPrecision > 0 ? ".".PadRight(parameterPrecision + 1, '0') : string.Empty);

            string parameterIndicator = GeneralFuncsLib.NvlString(dt.Rows[i]["ParameterIndicator"]);
            string actualValue = GeneralFuncsLib.NvlString(dt.Rows[i]["ParameterIndicator_ActualValue"]);
            string dataType = GeneralFuncsLib.NvlString(dt.Rows[i]["ParameterDataType"]).ToLower();

            string parameterThreshold = GeneralFuncsLib.NvlString(dt.Rows[i]["ParameterThreshold"]);
            string actualThreshold = GeneralFuncsLib.NvlString(dt.Rows[i]["ParameterThreshold_ActualValue"]);
            string thresholdType = GeneralFuncsLib.NvlString(dt.Rows[i]["ThresholdType"]).ToLower();

            string parameterThresholdHigh = GeneralFuncsLib.NvlString(dt.Rows[i]["ParameterThresholdHigh"]);
            string parameterThresholdType = GeneralFuncsLib.NvlString(dt.Rows[i]["ParameterThresholdType"]);


            if (!parameterIndicator.Equals("0") && !parameterIndicator.IsNullOrEmpty())
            {
                parameterIndicator = decimal.Parse(parameterIndicator).ToString(indicatorFormat);
            }
            if (!actualValue.Equals("0") && !actualValue.IsNullOrEmpty())
            {
                actualValue = decimal.Parse(actualValue).ToString(indicatorFormat);
            }

            if (!parameterThreshold.Equals("0") && !parameterThreshold.IsNullOrEmpty())
            {
                parameterThreshold = decimal.Parse(parameterThreshold).ToString(indicatorFormat);
            }
            if (!actualThreshold.Equals("0") && !String.IsNullOrEmpty(actualThreshold))
            {
                actualThreshold = decimal.Parse(actualThreshold).ToString(indicatorFormat);
            }
            if (!parameterThresholdHigh.Equals("0") && !parameterThresholdHigh.IsNullOrEmpty())
            {
                parameterThresholdHigh = decimal.Parse(parameterThresholdHigh).ToString(indicatorFormat);
            }

            //[44432]: Bug #37842
            //process indicator
            dt.Rows[i]["ParameterIndicatorText"] = VeraCodeSolution.ValidateResponseData(
                GeneralFuncsLib.FormatParameterExport(dataType, parameterIndicator, parameterPrecision));
            dt.Rows[i]["ActualValueText"] = VeraCodeSolution.ValidateResponseData(
                             GeneralFuncsLib.FormatParameterExport(dataType, actualValue, parameterPrecision));
            dt.Rows[i]["ParameterThresholdText"] = VeraCodeSolution.ValidateResponseData(
                             GeneralFuncsLib.FormatParameterExport(dataType, parameterThreshold, 0));

            switch (parameterThresholdType)
            {
                case "LowHigh":
                    dt.Rows[i]["ParameterThresholdText"] = VeraCodeSolution.ValidateResponseData(
                        GeneralFuncsLib.FormatLowHighThreshold(parameterThreshold, parameterThresholdHigh, thresholdType));
                    break;
                default:
                    dt.Rows[i]["ParameterThresholdText"] = VeraCodeSolution.ValidateResponseData(
                        GeneralFuncsLib.FormatParameterExport(thresholdType, parameterThreshold, 0));
                    break;
            }
            dt.Rows[i]["ActualThresholdText"] = VeraCodeSolution.ValidateResponseData(
                             GeneralFuncsLib.FormatParameterExport(thresholdType, actualThreshold, 0));

            // 10/10/2018 |41044 PIVOT New Parameter
            string topCard = getTopCardText(GeneralFuncsLib.NvlString(dt.Rows[i]["ParameterThreshold_ActualValue1"]));
            if (!topCard.IsNullOrEmpty())
            {
                dt.Rows[i]["ActualThresholdText"] += "\r\n" + topCard;
            }
        }

        return dt;
    }

    private bool _isExporting { get; set; }
    protected override void DoNeedExportConfig(UxExport sender, ExportConfig exportConfig)
    {
        if (IsIntruderDetected)
            return;
        _isExporting = true;
        UxExport uxExport = sender as UxExport;
        ASGrid grid = uxExport.Grid as ASGrid;

        if (grid.Columns.FindByUniqueNameSafe("ParameterIndicatorText") != null)
        {
            grid.Columns.FindByUniqueName("ParameterIndicatorText").Visible = true;
            grid.Columns.FindByUniqueName("ParameterIndicator").Visible = false;
        }
        if (grid.Columns.FindByUniqueNameSafe("ActualValueText") != null)
        { 
            grid.Columns.FindByUniqueName("ActualValueText").Visible = true;
            grid.Columns.FindByUniqueName("ActualValue").Visible = false;
        }
        if (grid.Columns.FindByUniqueNameSafe("ParameterThresholdText") != null)
        {
            grid.Columns.FindByUniqueName("ParameterThresholdText").Visible = true;
            grid.Columns.FindByUniqueName("ParameterThreshold").Visible = false;
        }
        if (grid.Columns.FindByUniqueNameSafe("ActualThresholdText") != null)
        {
            grid.Columns.FindByUniqueName("ActualThresholdText").Visible = true;
            grid.Columns.FindByUniqueName("ActualThreshold").Visible = false;
        }


        grid.Columns.FindByUniqueName("MerchantNameEx").Visible = true;
        grid.Columns.FindByUniqueName("MerchantName").Visible = false;
        grid.Columns.FindByUniqueName("MerchantNumberEx").Visible = true;
        grid.Columns.FindByUniqueName("MerchantNumber").Visible = false;
        base.DoNeedExportConfig(sender, exportConfig);

        exportConfig.FileName = GeneralFuncsLib.FormatFileName(Server.HtmlDecode(uxExporter.GridHeader));
        exportConfig.ReportHeader = Server.HtmlDecode(uxExporter.GridHeader);
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.MerchantNameClick:
                string url = "rm_MCF_RiskReport.aspx?" + this.BuildSecureQueryString(
                    string.Format("merchantnumber={0}&IsPopup={1}{2}",
                                  _MerchantNumber,
                                  true,
                                  MerchantProfileIntruderQuery));
                AjaxAddResponseScript("openPopupWindow('" + url + "','RiskReport');");
                break;
            case PostBackAction.MerchantNumberClick:
                RiskSessionManager.RiskReportReferrer = "MerchantDetail";
                RiskSessionManager.RiskReportReferrerInfo = new ReferrerInfo(_MerchantNumber, ResolveUrl("~/risk_MCF/") + "rm_MCF_MgmtReport_MerchantDetail.aspx", "Merchant Detail");
                url = this.BuildSecureQueryString(string.Format("merchantnumber={0}", _MerchantNumber));
                url = ResolveUrl("~/") + "MerchantProfile.aspx?" + url;
                Response.Redirect(url, true);
                break;
            case PostBackAction.DoFilterAction:
                uxReportGrid.CurrentPageIndex = 0;
                uxReportGrid.Rebind();
                break;
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindReportGrid:
                {
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    parameters.AddMgmtLoggedInUser();

                    parameters.Add(new FilterParameter("@DateFilterMode", (int)RiskSessionManager.RiskMgmtReportFilter.DateType, DbType.Int32));
                    DateTime beginDate = RiskSessionManager.RiskMgmtReportFilter.FromDate;
                    DateTime endDate = RiskSessionManager.RiskMgmtReportFilter.ToDate;
                    GeneralFuncsLib.GetRealDateRange(RiskSessionManager.RiskMgmtReportFilter.DateType, ref beginDate, ref endDate);

                    parameters.Add(new FilterParameter("@BeginDate", beginDate, DbType.DateTime));
                    parameters.Add(new FilterParameter("@EndDate", endDate, DbType.DateTime));
                    parameters.Add(new FilterParameter("@AssignmentList", uxReportFilter.AssignmentFilterValue, DbType.AnsiString));
                    parameters.Add(new FilterParameter("@ParameterList", uxReportFilter.ParameterFilterValue, DbType.AnsiString));
                    parameters.Add(new FilterParameter("@MerchantList", uxReportFilter.MerchantNumber, DbType.AnsiString));
                    parameters.Add(new FilterParameter("@MerchantName", uxReportFilter.MerchantName, DbType.AnsiString));
                    parameters.AddLanguageID();
                    //44617: Export real excel
                    DataTable data = WebServices.SecurityServices.GetReports("spa_RM_MCF_Mgmt_GetMerchantDetail", parameters);
                    ((ASGrid)sender).DataSource = _isExporting ? DoInitializeExport(data) : data;

                    uxExporter.GridTitle = GetLocalResourceObject("rm_MerchantDetail_aspx_cs_GridTitle").ToString();
                    uxExporter.GridSubTitle = VeraCodeSolution.ValidateResponseData(uxReportFilter.HeaderText);
                    uxExporter.GridHeader = uxExporter.GridTitle + " - " + uxExporter.GridSubTitle;
                }
                break;
            default:
                break;
        }
    }

    protected void OnSearchEvent(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.DoFilterAction);
    }

    protected void uxSearch_Click(object sender, EventArgs e)
    {
        uxReportGrid.CurrentPageIndex = 0;
        uxReportGrid.Rebind();
    }

    #endregion Methods

    // 10/10/2018 |41044 PIVOT New Parameter
    private string getTopCardText(string number)
    {
        int topCardNumber = 0;
        int.TryParse(number.Split('.')[0], out topCardNumber);
        string topCardText = null;
        if (topCardNumber > 0)
        {
            topCardText = GetLocalResourceObject("rm_MgmtReport_MerchantDetail_aspx_cs_TopCard").ToString();
            if (topCardNumber > 1)
            {
                topCardText = string.Format(GetLocalResourceObject("rm_MgmtReport_MerchantDetail_aspx_cs_TopCards").ToString(), topCardNumber);
            }
        }

        return topCardText;
    }
}
