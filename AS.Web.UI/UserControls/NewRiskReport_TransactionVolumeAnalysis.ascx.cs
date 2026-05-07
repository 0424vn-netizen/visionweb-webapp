using AS.Common;
using AS.Common.DBManager;
using AS.Common.Formater;
using AS.Controls.Grid;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using AS.Web.UI;
using Telerik.Web.UI;
using System.Linq;
using System.Text;
using AS.Controls;

public partial class UserControls_NewRiskReportRiskManagement : GlobalUserControl
{
    private const string SPA_GET_TRANSACTION_VOLUME_ANALYSIS = "spa_rm_cs_RiskReport_GetTransactionVolumeAnalysis";
    private const string UNCHANGE_COLUMN = "Time";
    private List<string> CONTRACTED_EXPECTED = new List<string> { "contracted expected", "contract expected" };
    private const string TOOLTIP_ROW_FORMAT = "<tr><td>{0}&nbsp;&nbsp;</td><td>{1}</td></tr>";
    private const string TOOLTIP_ROW_FORMAT_ALICE = "<tr class='text-italic text-default-gray'><td colspan='2' >{0}</td></tr>";
    private const string ZERO_VALUE = "0.00";
    private const string VOLUME = "Volume";
    private const string VOLUME2 = "Volume2";
    private const string AVG_TICKET = "AVGTicket";
    private const string KEYED_PERCENT = "KeyedPercent";
    private const string LARGE_TRANS = "LargeTrans";
    private const string TOOLTIP_GRID_TEXT1_FORMAT = "<span class='tooltip-text1'>{0}</span>";
    private const string TOOLTIP_GRID_TEXT2_FORMAT = "<span class='tooltip-text2'>{0}</span>";
    private const string TOOLTIP_GRID_TEXT3_FORMAT = "<span id='txtTotal' title='{0}'>{1}</span>";
    private const string EM_DASH = "—";
    private const string ANNUAL_VOLUME = "AnnualVolume";

    #region properties
    private string _MerchantNumber = string.Empty;

    protected DataTable DTTransVolumeAnalysis
    {
        get
        {
            return (DataTable)ViewState["TransVolumeAnalysis"];
        }
        set
        {
            ViewState["TransVolumeAnalysis"] = value;
        }
    }

    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        SetTooltipForGrid();
        if (!Page.IsPostBack)
        {
            //SetViewCombo();
            //GetData();
            //GetDisplayedColumnsByViewID();
        }
    }
    #region Methods


    public void SetViewCombo()
    {
        DataTable tb = GetViewColumn();
        uxViewColumns.DataSource = tb;
        uxViewColumns.DataBind();
        DataRow[] defaultRow = tb.Select("IsDefault = 1");
        if (!string.IsNullOrEmpty(RiskSessionManager.UserViewSelected) && tb.AsEnumerable().FirstOrDefault(r => r.Field<int>("CustomViewID") == int.Parse(RiskSessionManager.UserViewSelected)) != null)
        {
            uxViewColumns.SelectedValue = RiskSessionManager.UserViewSelected;

        }
        else if (defaultRow != null)
        {
            uxViewColumns.SelectedValue = defaultRow[0]["CustomViewID"].ToString();
        }
    }
    private void SetTooltipForGrid()
    {
        tltTime.Title = string.Format(TOOLTIP_GRID_TEXT1_FORMAT, GetLocalResourceObject("Time.HeaderTooltip").ToString()) + "&nbsp;&nbsp;";
        tltTime.Text = string.Format(TOOLTIP_GRID_TEXT2_FORMAT, GetLocalResourceObject("Time.HeaderDescription").ToString()) + "&nbsp;&nbsp;";
        tltSalesCount.Title = string.Format(TOOLTIP_GRID_TEXT1_FORMAT, GetLocalResourceObject("SalesCount.HeaderTooltip").ToString()) + "&nbsp;&nbsp;";
        tltSalesCount.Text = string.Format(TOOLTIP_GRID_TEXT2_FORMAT, GetLocalResourceObject("SalesCount.HeaderDescription").ToString()) + "&nbsp;&nbsp;";
        tltVolume.Title = string.Format(TOOLTIP_GRID_TEXT1_FORMAT, GetLocalResourceObject("Volume.HeaderTooltip").ToString()) + "&nbsp;&nbsp;";
        tltVolume.Text = string.Format(TOOLTIP_GRID_TEXT2_FORMAT, GetLocalResourceObject("Volume.HeaderDescription").ToString()) + "&nbsp;&nbsp;";
        tltAVGTicket.Title = string.Format(TOOLTIP_GRID_TEXT1_FORMAT, GetLocalResourceObject("AVGTicket.HeaderTooltip").ToString()) + "&nbsp;&nbsp;";
        tltAVGTicket.Text = string.Format(TOOLTIP_GRID_TEXT2_FORMAT, GetLocalResourceObject("AVGTicket.HeaderDescription").ToString()) + "&nbsp;&nbsp;";
        tltAuthCount.Title = string.Format(TOOLTIP_GRID_TEXT1_FORMAT, GetLocalResourceObject("AuthCount.HeaderTooltip").ToString()) + "&nbsp;&nbsp;";
        tltAuthCount.Text = string.Format(TOOLTIP_GRID_TEXT2_FORMAT, GetLocalResourceObject("AuthCount.HeaderDescription").ToString()) + "&nbsp;&nbsp;";
        tltAuthApprovalPercent.Title = string.Format(TOOLTIP_GRID_TEXT1_FORMAT, GetLocalResourceObject("AuthApprovalPercent.HeaderTooltip").ToString()) + "&nbsp;&nbsp;";
        tltAuthApprovalPercent.Text = string.Format(TOOLTIP_GRID_TEXT2_FORMAT, GetLocalResourceObject("AuthApprovalPercent.HeaderDescription").ToString()) + "&nbsp;&nbsp;";
        tltKeyedPercent.Title = string.Format(TOOLTIP_GRID_TEXT1_FORMAT, GetLocalResourceObject("KeyedPercent.HeaderTooltip").ToString()) + "&nbsp;&nbsp;";
        tltKeyedPercent.Text = string.Format(TOOLTIP_GRID_TEXT2_FORMAT, GetLocalResourceObject("KeyedPercent.HeaderDescription").ToString()) + "&nbsp;&nbsp;";
        tltReturns.Title = string.Format(TOOLTIP_GRID_TEXT1_FORMAT, GetLocalResourceObject("Returns.HeaderTooltip").ToString()) + "&nbsp;&nbsp;";
        tltReturns.Text = string.Format(TOOLTIP_GRID_TEXT2_FORMAT, GetLocalResourceObject("Returns.HeaderDescription").ToString()) + "&nbsp;&nbsp;";
        tltReturnPercent.Title = string.Format(TOOLTIP_GRID_TEXT1_FORMAT, GetLocalResourceObject("ReturnPercent.HeaderTooltip").ToString()) + "&nbsp;&nbsp;";
        tltReturnPercent.Text = string.Format(TOOLTIP_GRID_TEXT2_FORMAT, GetLocalResourceObject("ReturnPercent.HeaderDescription").ToString()) + "&nbsp;&nbsp;";
        tltChargeBackPercent.Title = string.Format(TOOLTIP_GRID_TEXT1_FORMAT, GetLocalResourceObject("ChargeBackPercent.HeaderTooltip").ToString()) + "&nbsp;&nbsp;";
        tltChargeBackPercent.Text = string.Format(TOOLTIP_GRID_TEXT2_FORMAT, GetLocalResourceObject("ChargeBackPercent.HeaderDescription").ToString()) + "&nbsp;&nbsp;";
        tltChargeBackVolume.Title = string.Format(TOOLTIP_GRID_TEXT1_FORMAT, GetLocalResourceObject("ChargeBackVolume.HeaderTooltip").ToString()) + "&nbsp;&nbsp;";
        tltChargeBackVolume.Text = string.Format(TOOLTIP_GRID_TEXT2_FORMAT, GetLocalResourceObject("ChargeBackVolume.HeaderDescription").ToString()) + "&nbsp;&nbsp;";
        tltRetrievals.Title = string.Format(TOOLTIP_GRID_TEXT1_FORMAT, GetLocalResourceObject("Retrievals.HeaderTooltip").ToString()) + "&nbsp;&nbsp;";
        tltRetrievals.Text = string.Format(TOOLTIP_GRID_TEXT2_FORMAT, GetLocalResourceObject("Retrievals.HeaderDescription").ToString()) + "&nbsp;&nbsp;";
        tltForeign.Title = string.Format(TOOLTIP_GRID_TEXT1_FORMAT, GetLocalResourceObject("Foreign.HeaderTooltip").ToString()) + "&nbsp;&nbsp;";
        tltForeign.Text = string.Format(TOOLTIP_GRID_TEXT2_FORMAT, GetLocalResourceObject("Foreign.HeaderDescription").ToString()) + "&nbsp;&nbsp;";
        tltACHRejects.Title = string.Format(TOOLTIP_GRID_TEXT1_FORMAT, GetLocalResourceObject("ACHRejects.HeaderTooltip").ToString()) + "&nbsp;&nbsp;";
        tltACHRejects.Text = string.Format(TOOLTIP_GRID_TEXT2_FORMAT, GetLocalResourceObject("ACHRejects.HeaderDescription").ToString()) + "&nbsp;&nbsp;";
        tltLargeTrans.Title = string.Format(TOOLTIP_GRID_TEXT1_FORMAT, GetLocalResourceObject("LargeTrans.HeaderTooltip").ToString()) + "&nbsp;&nbsp;";
        tltLargeTrans.Text = string.Format(TOOLTIP_GRID_TEXT2_FORMAT, GetLocalResourceObject("LargeTrans.HeaderDescription").ToString()) + "&nbsp;&nbsp;";
        tltReturnCount.Title = string.Format(TOOLTIP_GRID_TEXT1_FORMAT, GetLocalResourceObject("ReturnCount.HeaderTooltip").ToString()) + "&nbsp;&nbsp;";
        tltReturnCount.Text = string.Format(TOOLTIP_GRID_TEXT2_FORMAT, GetLocalResourceObject("ReturnCount.HeaderDescription").ToString()) + "&nbsp;&nbsp;";
        tltChargeBackCount.Title = string.Format(TOOLTIP_GRID_TEXT1_FORMAT, GetLocalResourceObject("ChargeBackCount.HeaderTooltip").ToString()) + "&nbsp;&nbsp;";
        tltChargeBackCount.Text = string.Format(TOOLTIP_GRID_TEXT2_FORMAT, GetLocalResourceObject("ChargeBackCount.HeaderDescription").ToString()) + "&nbsp;&nbsp;";
        tltSaleAmount.Title = string.Format(TOOLTIP_GRID_TEXT1_FORMAT, GetLocalResourceObject("SaleAmount.HeaderTooltip").ToString()) + "&nbsp;&nbsp;";
        tltSaleAmount.Text = string.Format(TOOLTIP_GRID_TEXT2_FORMAT, GetLocalResourceObject("SaleAmount.HeaderDescription").ToString()) + "&nbsp;&nbsp;";
        tltChargeBackCountPercent.Title = string.Format(TOOLTIP_GRID_TEXT1_FORMAT, GetLocalResourceObject("ChargeBackCountPercent.HeaderTooltip").ToString()) + "&nbsp;&nbsp;";
        tltChargeBackCountPercent.Text = string.Format(TOOLTIP_GRID_TEXT2_FORMAT, GetLocalResourceObject("ChargeBackCountPercent.HeaderDescription").ToString()) + "&nbsp;&nbsp;";
        tltReturnCountPercent.Title = string.Format(TOOLTIP_GRID_TEXT1_FORMAT, GetLocalResourceObject("ReturnCountPercent.HeaderTooltip").ToString()) + "&nbsp;&nbsp;";
        tltReturnCountPercent.Text = string.Format(TOOLTIP_GRID_TEXT2_FORMAT, GetLocalResourceObject("ReturnCountPercent.HeaderDescription").ToString()) + "&nbsp;&nbsp;";
        tltForeignCardCountPercent.Title = string.Format(TOOLTIP_GRID_TEXT1_FORMAT, GetLocalResourceObject("ForeignCardCountPercent.HeaderTooltip").ToString()) + "&nbsp;&nbsp;";
        tltForeignCardCountPercent.Text = string.Format(TOOLTIP_GRID_TEXT2_FORMAT, GetLocalResourceObject("ForeignCardCountPercent.HeaderDescription").ToString()) + "&nbsp;&nbsp;";
        tltDeclinedAuthorizationAmount.Title = string.Format(TOOLTIP_GRID_TEXT1_FORMAT, GetLocalResourceObject("DeclinedAuthorizationAmount.HeaderTooltip").ToString()) + "&nbsp;&nbsp;";
        tltDeclinedAuthorizationAmount.Text = string.Format(TOOLTIP_GRID_TEXT2_FORMAT, GetLocalResourceObject("DeclinedAuthorizationAmount.HeaderDescription").ToString()) + "&nbsp;&nbsp;";
        tltDeclinedAuthorizationCount.Title = string.Format(TOOLTIP_GRID_TEXT1_FORMAT, GetLocalResourceObject("DeclinedAuthorizationCount.HeaderTooltip").ToString()) + "&nbsp;&nbsp;";
        tltDeclinedAuthorizationCount.Text = string.Format(TOOLTIP_GRID_TEXT2_FORMAT, GetLocalResourceObject("DeclinedAuthorizationCount.HeaderDescription").ToString()) + "&nbsp;&nbsp;";
        tltDeclinedAuthorizationAmountPercent.Title = string.Format(TOOLTIP_GRID_TEXT1_FORMAT, GetLocalResourceObject("DeclinedAuthorizationAmountPercent.HeaderTooltip").ToString()) + "&nbsp;&nbsp;";
        tltDeclinedAuthorizationAmountPercent.Text = string.Format(TOOLTIP_GRID_TEXT2_FORMAT, GetLocalResourceObject("DeclinedAuthorizationAmountPercent.HeaderDescription").ToString()) + "&nbsp;&nbsp;";
        tltDeclinedAuthorizationCountPercent.Title = string.Format(TOOLTIP_GRID_TEXT1_FORMAT, GetLocalResourceObject("DeclinedAuthorizationCountPercent.HeaderTooltip").ToString()) + "&nbsp;&nbsp;";
        tltDeclinedAuthorizationCountPercent.Text = string.Format(TOOLTIP_GRID_TEXT2_FORMAT, GetLocalResourceObject("DeclinedAuthorizationCountPercent.HeaderDescription").ToString()) + "&nbsp;&nbsp;";
        tltRetrievalCountPercent.Title = string.Format(TOOLTIP_GRID_TEXT1_FORMAT, GetLocalResourceObject("RetrievalCountPercent.HeaderTooltip").ToString()) + "&nbsp;&nbsp;";
        tltRetrievalCountPercent.Text = string.Format(TOOLTIP_GRID_TEXT2_FORMAT, GetLocalResourceObject("RetrievalCountPercent.HeaderDescription").ToString()) + "&nbsp;&nbsp;";
        tltRetrievalAmountPercent.Title = string.Format(TOOLTIP_GRID_TEXT1_FORMAT, GetLocalResourceObject("RetrievalAmountPercent.HeaderTooltip").ToString()) + "&nbsp;&nbsp;";
        tltRetrievalAmountPercent.Text = string.Format(TOOLTIP_GRID_TEXT2_FORMAT, GetLocalResourceObject("RetrievalAmountPercent.HeaderDescription").ToString()) + "&nbsp;&nbsp;";
        tltFirstRetrievalCount.Title = string.Format(TOOLTIP_GRID_TEXT1_FORMAT, GetLocalResourceObject("FirstRetrievalCount.HeaderTooltip").ToString()) + "&nbsp;&nbsp;";
        tltFirstRetrievalCount.Text = string.Format(TOOLTIP_GRID_TEXT2_FORMAT, GetLocalResourceObject("FirstRetrievalCount.HeaderDescription").ToString()) + "&nbsp;&nbsp;";
    }
    public void GetData()
    {
        if (!string.IsNullOrEmpty(SessionManager.CurrentMerchantNumber))
        {
            if (Page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RSK_MNG_PRIVATE_VIEW) || Page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RSK_MNG_PRIVATE_VIEW_MS) ||
                Page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RSK_MNG_PUBLIC_VIEW_MS) || Page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RSK_MNG_PUBLIC_VIEW))
            {
                uxCustomizeColumnLink.Visible = true;
                string encodeURL = string.Format("rm_ManageCustomViewsModal.aspx?" + this.Page.BuildSecureQueryString("merchantNumber=" + SessionManager.CurrentMerchantNumber));
                uxCustomizeColumnLink.OnClientClick = "return doOpenNewPopup('" + encodeURL + "')";
            }
            else
            {
                uxCustomizeColumnLink.Visible = false;
            }
        }
        uxTransVolumeAnalysisDataGrid.Rebind();
    }
    #endregion

    /// <summary>
    /// Generate TransactionVolumeAnalysis Grid
    /// </summary>
    /// <param name="rebindReason">Flag to mark exporting</param>
    private void GenerateTransactionVolumeAnalysisGrid(GridRebindReason rebindReason)
    {
        var dataSource = GetTransactionVolumeAnalysisData();
        uxTransVolumeAnalysisDataGrid.DataSource = dataSource;
        this.DTTransVolumeAnalysis = dataSource;
        //if RiskReport_ConfigurationData is null then set default 14 comlumn display
    }
    private void ShowContractExpectedInfo()
    {
        var data = this.DTTransVolumeAnalysis;
        DataRow row = data.Select("DataType = 2").FirstOrDefault();
        bool flag = false;
        if (row != null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<table>");

            flag = true;
            string keyPercentToolTip = string.Empty;
            string keyPercent = string.Empty;
            if (!row[KEYED_PERCENT].IsNullOrEmpty())
            {
                keyPercent = !row[KEYED_PERCENT].ToString().Equals(ZERO_VALUE) ? MyPercentageConvert(row[KEYED_PERCENT]) : ZERO_VALUE + "%";
            }
            else
            {
                keyPercent = EM_DASH;
            }
            keyPercentToolTip = string.Format(TOOLTIP_ROW_FORMAT, GetLocalResourceObject(KEYED_PERCENT + ".HeaderText").ToString(), keyPercent);

            string annualToolTip = string.Empty;
            annualToolTip = FormatContractedTooltip(row[ANNUAL_VOLUME], ANNUAL_VOLUME);
            sb.Append(annualToolTip);

            string volumnToolTip = string.Empty;
            volumnToolTip = FormatContractedTooltip(row[VOLUME], VOLUME2);
            sb.Append(volumnToolTip);

            string avgTicketToolTip = string.Empty;
            avgTicketToolTip = FormatContractedTooltip(row[AVG_TICKET], AVG_TICKET);
            sb.Append(avgTicketToolTip);

            string largeTransTooltip = string.Empty;
            largeTransTooltip = FormatContractedTooltip(row[LARGE_TRANS], LARGE_TRANS);
            sb.Append(largeTransTooltip);

            sb.Append(keyPercentToolTip);

            if (SessionManager.CurrentSystem == WebSiteConstants.AS_SYSTEM_CS && GeneralFuncsLib.NvlString(row["IsDataFromAlice"]) == "True")
            {
                sb.Append(string.Format(TOOLTIP_ROW_FORMAT_ALICE, GetLocalResourceObject("Data_From_Alice")));
            }

            sb.Append("</table>");

            tltContractExpected.Text = VeraCodeSolution.DoVeraCode(sb.ToString());
        }

        lblContractExpected.Visible = iconContractExpectedInfo.Visible = flag;
    }
    private string FormatContractedTooltip(object value, string key)
    {
        string reformatValue = string.Empty;
        reformatValue = !value.IsNullOrEmpty() ? (!value.ToString().Equals(ZERO_VALUE) ? FormatCurrency(value) : EM_DASH) : EM_DASH;
        return string.Format(TOOLTIP_ROW_FORMAT, GetLocalResourceObject(key + ".HeaderText").ToString(), reformatValue);
    }
    private DataTable GetTransactionVolumeAnalysisData()
    {
        var parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(false);
        parameters.AddLanguageID();
        parameters.Add(new FilterParameter("@ReportDate", DateTime.Today, DbType.Date));
        parameters.Add(new FilterParameter("@MerchantNumber", SessionManager.CurrentMerchantNumber, DbType.String));
        return WebServices.RiskServices.GetReports(SPA_GET_TRANSACTION_VOLUME_ANALYSIS, parameters);
    }
    private DataTable GetCustomDisplayedColumns()
    {
        var parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(false);
        parameters.Add(new FilterParameter("@UserID", SessionManager.CurrentUser, DbType.String));
        parameters.Add(new FilterParameter("@CustomViewID", uxViewColumns.SelectedValue, DbType.Int32));
        parameters.Add(new FilterParameter("@UserRecID", SessionManager.CurrentUser.RecId, DbType.Guid));
        return WebServices.RiskServices.GetReports("spa_rm_Get_CustomView", parameters);
    }
    private DataTable GetViewColumn()
    {
        var parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(false);
        parameters.AddLanguageID();
        parameters.Add(new FilterParameter("@ViewMode", 0, System.Data.DbType.Int32));
        parameters.Add(new FilterParameter("@ASClient", SessionManager.CurrentClient, System.Data.DbType.Int32));
        parameters.Add(new FilterParameter("@UserRecID", SessionManager.CurrentUser.RecId, DbType.Guid));
        return WebServices.RiskServices.GetReports("spa_rm_GetCustomViewList", parameters);
    }
    protected void uxTransVolumeAnalysisDataGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        GenerateTransactionVolumeAnalysisGrid(e.RebindReason);
    }
    protected void uxExportTransVolumeAnalysis_NeedExportConfig(object sender, AS.Controls.Exporter.ExportConfig exportConfig)
    {
        var dataSource = GetTransactionVolumeAnalysisData();
        var defaultColumnData = GetDisplayedColumnsByViewID();
        var listDisplayedColumns = defaultColumnData.DisplayedColumns;
        var newColumnGridCollection = new GridColumn[listDisplayedColumns.Count + 1];
        foreach (GridColumn col in uxTransVolumeAnalysisDataGrid.MasterTableView.Columns)
        {
            if (col.UniqueName == UNCHANGE_COLUMN)
            {
                col.OrderIndex = 0;
                newColumnGridCollection[0] = col;
                continue;
            }
            var index = listDisplayedColumns.FindIndex(c => c.ToString() == col.UniqueName);
            if (index >= 0)
            {
                col.Visible = true;
                col.OrderIndex = (index + 1);
                newColumnGridCollection[index + 1] = col;
                //46652 - AW - Multi-Currency Transaction Display
                col.HeaderText = GetLocalResourceObject(col.UniqueName + ".HeaderText").ToString().Replace("$", SessionManager.CurrencySymbol);
            }
            else
            {
                col.Visible = false;
            }
        }
        uxTransVolumeAnalysisDataGrid.MasterTableView.Columns.Clear();
        foreach (GridColumn col in newColumnGridCollection)
        {
            if (col != null)
            {
                uxTransVolumeAnalysisDataGrid.MasterTableView.Columns.Add(col);
            }
        }
        uxExportTransVolumeAnalysis.Formatter = new Dictionary<string, Func<object, string>>();
        string exportType = ((AS.Controls.UserControls.UxExport)(sender)).ExportButtonType.ToString();
        if (exportType == "PDF")
        {
            if (listDisplayedColumns.Count > 10)
            {
                exportConfig.PageDirection = AS.Controls.Exporter.PageDirection.Landscape;
            }
        }
        foreach (var displayCol in listDisplayedColumns)
        {
            var col = uxTransVolumeAnalysisDataGrid.MasterTableView.Columns.Cast<GridColumn>().Where(w => w.UniqueName == displayCol).FirstOrDefault();
            if (col != null)
            {
                var format = ((AS.Controls.Grid.ASGridTemplateColumn)(col)).ASExportFormat;
                if (format == FormatType.Percentage)
                {
                    uxExportTransVolumeAnalysis.Formatter.Add(displayCol, new Func<object, string>(MyPercentageConvert));
                }
                else if (format == FormatType.Currency)
                {
                    if (exportType.ToLower() == "excel")
                    {
                        uxExportTransVolumeAnalysis.Formatter.Add(displayCol, new Func<object, string>(FormatContractExcel));
                    }
                    else
                    {
                        uxExportTransVolumeAnalysis.Formatter.Add(displayCol, new Func<object, string>(FormatContractNotExcel));
                    }
                }
                else
                {
                    uxExportTransVolumeAnalysis.Formatter.Add(displayCol, new Func<object, string>(FormatContractForCount));
                }
            }
        }
    }
    protected void uxTransVolumeAnalysisDataGrid_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            var item = (GridDataItem)e.Item;
            string timeValue = DataBinder.Eval(item.DataItem, "Time").ToString().Trim().ToLower();
            var contractValue = GetLocalResourceObject("txtContractExpected").ToString().ToLower();
            var sevenDayValue = GetLocalResourceObject("txt30day").ToString().ToLower();
            var total = GetLocalResourceObject("txtTotal").ToString().ToLower();
            string baseClass = item.ItemType == Telerik.Web.UI.GridItemType.Item ? "rgRow" : "rgAltRow";
            if (sevenDayValue.Contains(timeValue))
            {
                item.CssClass = baseClass + " section-separator";
            }
            if (total.Contains(timeValue))
            {
                item.CssClass = baseClass + " text-bold text-italic";
                Literal lt = e.Item.FindControl("litTime") as Literal;
                lt.Text = string.Format(TOOLTIP_GRID_TEXT3_FORMAT, GetLocalResourceObject("txtTotalToolTip").ToString(), lt.Text);
            }
            if (!contractValue.Contains(timeValue))
            {
                return;
            }
            item.CssClass = baseClass + " section-separator-contract-expected text-bold";
        }
    }
    protected string MyConvert(object obj)
    {
        if (obj == null || obj.ToString() == String.Empty)
            return String.Empty;
        var type = obj.GetType();
        if (type.Name == "Int32")
        {
            return (long.Parse(obj.ToString())).ToString("#,#0");
        }
        if (type.Name == "Decimal")
        {
            return FormatData.FormatCurrency(obj, SessionManager.CurrencyFortmat);
        }
        return obj.ToString();
    }
    protected string MyPercentageConvert(object obj)
    {
        if (obj == null || obj.ToString() == String.Empty)
            return GeneralFuncsLib.NA_VALUE;
        return decimal.Parse(obj.ToString()).ToString("#,#0.00") + "%";
    }
    #region Format data
    protected string FormatCurrency(object data)
    {
        if (data != null && !string.IsNullOrEmpty(data.ToString()))
            return FormatData.FormatCurrency(data, SessionManager.CurrencyFortmat);
        return string.Empty;
    }
    protected string FormatCurrencyForStatements(decimal currency)
    {
        return FormatData.FormatCurrency(currency, SessionManager.CurrencyFortmat);
    }
    protected string FormatContract(object data)
    {
        if (data != null && !string.IsNullOrEmpty(data.ToString()))
        {
            return FormatData.FormatCurrency(data, SessionManager.CurrencyFortmat);
        }
        return GeneralFuncsLib.NA_VALUE;
    }

    protected string FormatContractExcel(object data)
    {
        if (data != null && !string.IsNullOrEmpty(data.ToString()))
        {

            string strVal = FormatData.FormatCurrency(data, SessionManager.CurrencyFortmat);

            decimal temp = Convert.ToDecimal(data);
            if (temp < 0)
                strVal = string.Format("({0})", strVal.Replace("-", ""));
            return strVal;

        }
        return GeneralFuncsLib.NA_VALUE;
    }
    protected string FormatContractNotExcel(object data)
    {
        if (data != null && !string.IsNullOrEmpty(data.ToString()))
            return this.FormatCurrencyForStatements((decimal)data);
        return GeneralFuncsLib.NA_VALUE;
    }
    protected string FormatContractForCount(object data)
    {
        if (data != null && !string.IsNullOrEmpty(data.ToString()))
            return data.ToString();
        return GeneralFuncsLib.NA_VALUE;
    }
    #endregion
    protected void uxViewColumns_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        RiskSessionManager.UserViewSelected = uxViewColumns.SelectedValue;
        GetDisplayedColumnsByViewID();
        uxTransVolumeAnalysisDataGrid.Rebind();
    }
    protected TransVolumeUserDataItem GetDisplayedColumnsByViewID()
    {
        var dataSource = GetCustomDisplayedColumns();
        string listColumns = dataSource.Rows[0]["ViewData"].ToString();
        string[] columns = listColumns.Split(',');
        for (int i = 0; i < columns.Length; i++)
        {
            columns[i] = columns[i].Trim();
        }
        List<string> items = columns.ToList<string>();
        var columnData = new TransVolumeUserDataItem
        {
            DisplayedColumns = items
        };
        return columnData;
    }
    private void GenerateAllColumnsTransactionVolumeAnalysisGrid()
    {
        var dataSource = this.DTTransVolumeAnalysis;
        uxTransVolumeAnalysisDataGrid.DataSource = dataSource;
        var listColumns = new List<ColumnDisplayedConfigurationItem>();
        var colGrid = uxTransVolumeAnalysisDataGrid.MasterTableView.Columns;
        foreach (GridColumn col in colGrid)
        {
            if (col.UniqueName != UNCHANGE_COLUMN && col.UniqueName != WebSiteConstants.GRID_COLUMN_TEMP)
            {
                listColumns.Add(new ColumnDisplayedConfigurationItem
                {
                    ColumnName = col.UniqueName,
                    ColumnText = GetLocalResourceObject(col.UniqueName + ".HeaderText").ToString().ToCurrencySymbol(),
                    IsDisplayed = false,
                    OrderIndex = col.OrderIndex
                });
            }
        }
        RiskSessionManager.RiskReportTransactionVolumeColumn = listColumns;

    }
    protected void btnRebind_Click(object sender, EventArgs e)
    {
        RiskSessionManager.UserViewSelected = string.Empty;
        SetViewCombo();
        base.OnPreRender(e);
        uxTransVolumeAnalysisDataGrid.Rebind();
    }
    protected void uxTransVolumeAnalysisDataGrid_PreRender(object sender, EventArgs e)
    {
        var defaultColumnData = GetDisplayedColumnsByViewID();
        var listDisplayedColumns = defaultColumnData.DisplayedColumns;
        foreach (GridColumn col in uxTransVolumeAnalysisDataGrid.MasterTableView.Columns)
        {
            if (col.UniqueName == WebSiteConstants.GRID_COLUMN_TEMP)
            {
                continue;
            }
            if (col.UniqueName == UNCHANGE_COLUMN)
            {
                col.OrderIndex = 0;
                continue;
            }
            var index = listDisplayedColumns.FindIndex(c => c.ToString() == col.UniqueName);
            if (index >= 0)
            {
                col.Visible = true;
                col.OrderIndex = (index + 1);
            }
            else
            {
                col.Visible = false;
            }
        }


        ShowContractExpectedInfo();

        GenerateAllColumnsTransactionVolumeAnalysisGrid();


        // Sort column to export

        uxTransVolumeAnalysisDataGrid.MasterTableView.Rebind();

    }






}
