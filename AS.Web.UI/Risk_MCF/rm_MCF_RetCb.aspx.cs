using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Web.Business;
using AS.Web.UI.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using GeneralFuntionBusiness = AS.Web.Business.General.GeneralFuncsLib;

[PagePermission("RskRETCB,MSRskRETCB")]
public partial class rm_MCF_RetCb : ReportPage
{
    #region Enums

    enum DataBindAction
    {
        BindReportGrid,
        BuildFiltering
    }

    enum PostBackAction
    {
        ChangeMerchantWorked,
        ChangeWorkType
    }

    #endregion

    #region Const

    private const string MERCHANT_NUMBER = "MerchantNumber";
    private const string APROVAL_DATE = "ApprovalDate";
    private const string MERCHANT_NAME = "MerchantName";
    private const string BEGIN_DATE = "BeginDate";
    private const string END_DATE = "EndDate";
    private const string URL_TEMPLATE = "<a href=\"#\" onclick=\"hyperlink_Click(this,'{0}', '{1}');return false;\">{2}</a>";
    private const string IS_TURNED_BACK = "isTurnedBack";

    #endregion Const

    #region Fields

    private string _retrievalChargebackIntruderQuery = string.Empty;
    private string _riskRepportIntruderQuery = string.Empty;

    #endregion Fields

    #region Properties

    private WebSiteEnums.ReviewedTypes ReviewedType
    {
        get
        {
            WebSiteEnums.ReviewedTypes reviewType = WebSiteEnums.ReviewedTypes.All;

            RadioButton rad = new RadioButton();
            if (uxRadNotReviewed.Checked)
            {
                rad = uxRadNotReviewed;

            }
            else if (uxRadReviewed.Checked)
            {
                rad = uxRadReviewed;
            }
            else
            {
                rad = uxRadAll;
            }

            reviewType = (WebSiteEnums.ReviewedTypes)int.Parse(rad.Attributes["xValue"]);
            return reviewType;
        }
        set
        {
            uxRadAll.Checked = uxRadReviewed.Checked = uxRadReviewed.Checked = false;
            if (value == WebSiteEnums.ReviewedTypes.All)
                this.uxRadAll.Checked = true;
            else if (value == WebSiteEnums.ReviewedTypes.NotReviewed)
                this.uxRadNotReviewed.Checked = true;
            else if (value == WebSiteEnums.ReviewedTypes.Reviewed)
                this.uxRadReviewed.Checked = true;
        }
    }

    private string GridTitle
    {
        get
        {
            return GeneralFuncsLib.GetFullGridTitleName(ReportFilter) + " ";
        }
    }

    private string GridSubitle
    {
        get
        {
            return GeneralFuncsLib.GetDateFilterText(ReportFilter);
        }
    }

    private string GridDrillDownHeader
    {
        get
        {
            return GeneralFuncsLib.GetGridDrilldownColumnName(ReportFilter.CurrentValue.HierarchyMode, ReportFilter.CurrentValue.Value);
        }
    }

    public DateTime BeginDate
    {
        get
        {
            return ReportFilter.CurrentValue.DateOptionValue.From;
        }
    }

    public DateTime EndDate
    {
        get
        {
            return (ReportFilter.CurrentValue.DateOption == AS.Web.UI.Controls.DateOptionMode.Daily
                || ReportFilter.CurrentValue.DateOption == AS.Web.UI.Controls.DateOptionMode.Monthly)
                ? ReportFilter.CurrentValue.DateOptionValue.From : ReportFilter.CurrentValue.DateOptionValue.To;
        }
    }

    private string RetrievalChargebackIntruderQuery
    {
        get
        {
            if (_retrievalChargebackIntruderQuery.IsNullOrEmpty())
            {
                _retrievalChargebackIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(
                      this.ID, new string[] { MERCHANT_NUMBER, BEGIN_DATE, END_DATE });
            }
            return _retrievalChargebackIntruderQuery;
        }
    }

    private string RiskReportIntruderQuery
    {
        get
        {
            if (_riskRepportIntruderQuery.IsNullOrEmpty())
            {
                _riskRepportIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(
                    this.ID, new string[] { MERCHANT_NUMBER });
            }
            return _riskRepportIntruderQuery;
        }
    }

    #endregion Properties

    public const string NAValue = "N/A";
    private DataTable _extendedHierarchyFilter;
    public DataTable ExtendedHierarchyFilter
    {
        get
        {
            if (_extendedHierarchyFilter != null)
                return _extendedHierarchyFilter;
            DataTable info = SessionManager.HierarchyFilterExtend;
            if (info != null && info.Rows.Count > 0)
            {
                var _temp = info.Select("IsEnableInclusion = 1 and Url='" + this.Page.Request.AppRelativeCurrentExecutionFilePath + "'");
                if (_temp != null && _temp.Count() > 0)
                {
                    DataTable tbExtend = _temp.CopyToDataTable();
                    if (tbExtend != null)
                    {
                        _extendedHierarchyFilter = tbExtend;
                        return _extendedHierarchyFilter;
                    }
                }
            }
            return null;
        }
    }
    public string ViewMoreIncludeExcludeItems
    {
        get
        {
            string _queryString = BuildSecureQueryString(string.Format("typemodal={0}", "7"));
            string htmlCmd = "ShowPopupModal('rm_MCF_Filter_CommonViewMore_Modal.aspx?" + _queryString + "');";
            return htmlCmd;
        }
    }

    public string EditIncludeExcludeItems
    {
        get
        {
            string htmlCmd = string.Empty;
            if (ExtendedHierarchyFilter != null)
            {
                foreach (DataRow row in ExtendedHierarchyFilter.Rows)
                {
                    string _queryString = BuildSecureQueryString(string.Format("hierarchyMode={0}&isInExItem={1}", row["HierarchyMode"].ToString(), "1"));
                    htmlCmd += row["HierarchyName"].ToString() + "|s|" + _queryString + row["HierarchyName"].ToString() + "|e|";
                }
            }
            return htmlCmd;
        }
    }
    public string ExtendHierarchyFilterText
    {
        get
        {
            if (ExtendedHierarchyFilter != null)
            {
                string hierarchyFilterExtendtext = string.Empty;
                foreach (DataRow row in ExtendedHierarchyFilter.Rows)
                {
                    hierarchyFilterExtendtext += row["HierarchyName"].ToString() + ",";
                }
                return hierarchyFilterExtendtext;
            }
            return string.Empty;
        }
    }
    public string ExtendedHierarchyID
    {
        get
        {
            if (ExtendedHierarchyFilter != null)
            {
                string extendHierarchyID = string.Empty;
                foreach (DataRow row in ExtendedHierarchyFilter.Rows)
                {
                    extendHierarchyID += row["HierarchyID"].ToString() + ",";
                }
                return extendHierarchyID;
            }
            return string.Empty;
        }
    }

    #region Methods

    #region Protected Methods

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        //uxExporter.GridTitle = VeraCodeSolution.ValidateResponseData(_GridTitle);
        uxExporter.GridSubTitle = VeraCodeSolution.ValidateResponseData(GridTitle + GridSubitle);
        uxExporter.GridHeader = uxExporter.GridTitle + uxExporter.GridSubTitle;
        BindIncludeExcludeExtendFilter();
    }

    protected override void PageInitialize()
    {
        this.ExporterIDs.Add("uxExporter");
        base.PageInitialize();
        // Reset PageSize & PageIndex when clicking on Search button.
        uxReportFiltering.Filtering += (s, e) =>
        {
            SetRetrievalsChargebackReferer(true);
        };
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        IsBindDataOnLoad = true;
        if (!IsPostBack)
        {
            if (IsSecureQueryString && SecureQueryString[IS_TURNED_BACK].ToInt() == 1
                      && RiskSessionManager.MCF_RetrievalsChargebacksReferer != null)
            {
                UpdateReportSetting();
                uxHiddenItems.Value = SessionManager.IncludeExcludeItem;
            }
            else
            {
                SetRetrievalsChargebackReferer(true);
            }
        }
    }

    private string PercentageConvert(object obj)
    {
        if (obj == null)
            return string.Empty;
        return Decimal.Round(Decimal.Parse(obj.ToString()), 2).ToString() + "%";
    }

    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, AS.Controls.Exporter.ExportConfig exportConfig)
    {
        uxReportGrid.Columns.FindByUniqueName("IsReviewed").Visible = false;
        uxReportGrid.Columns.FindByUniqueName("ReviewedText").Visible = true;
        uxReportGrid.Columns.FindByUniqueName("Detail").Visible = false;
        uxReportGrid.Columns.FindByUniqueName(APROVAL_DATE).Visible = true;
        ShowRDRColumns();

        uxReportGrid.Columns.FindByUniqueName("NetAmount30Day").HeaderText = GetLocalResourceObject("rm_RetCb_aspx_cs_HeaderText1").ToString();// "30 Day Net";
        uxReportGrid.Columns.FindByUniqueName("CBAmount30Day").HeaderText = GetLocalResourceObject("rm_RetCb_aspx_cs_HeaderText2").ToString();// "30 Day CB";
        uxReportGrid.Columns.FindByUniqueName("ChargebackPercentOfSales30Day").HeaderText = GetLocalResourceObject("rm_RetCb_aspx_cs_HeaderText3").ToString();// "30 Day CB%";
        uxReportGrid.Columns.FindByUniqueName("ThirtyDaysFirstChargebackRDRCount").HeaderText = GetLocalResourceObject("rm_RetCb_aspx_cs_HeaderText9").ToString();
        uxReportGrid.Columns.FindByUniqueName("ThirtyDaysFirstChargebackRDRAmount").HeaderText = GetLocalResourceObject("rm_RetCb_aspx_cs_HeaderText10").ToString();
        uxReportGrid.Columns.FindByUniqueName("ThirtyDaysPostChargebackRDRCount").HeaderText = GetLocalResourceObject("rm_RetCb_aspx_cs_HeaderText11").ToString();
        uxReportGrid.Columns.FindByUniqueName("ThirtyDaysPostChargebackRDRAmount").HeaderText = GetLocalResourceObject("rm_RetCb_aspx_cs_HeaderText12").ToString();

        uxReportGrid.Columns.FindByUniqueName("NetAmount90Day").HeaderText = GetLocalResourceObject("rm_RetCb_aspx_cs_HeaderText4").ToString();//"90 Day Net";
        uxReportGrid.Columns.FindByUniqueName("CBAmount90Day").HeaderText = GetLocalResourceObject("rm_RetCb_aspx_cs_HeaderText5").ToString();// "90 Day CB";
        uxReportGrid.Columns.FindByUniqueName("ChargebackPercentOfSales90Day").HeaderText = GetLocalResourceObject("rm_RetCb_aspx_cs_HeaderText6").ToString();// "90 Day CB%";
        uxReportGrid.Columns.FindByUniqueName("NinetyDaysFirstChargebackRDRCount").HeaderText = GetLocalResourceObject("rm_RetCb_aspx_cs_HeaderText13").ToString();
        uxReportGrid.Columns.FindByUniqueName("NinetyDaysFirstChargebackRDRAmount").HeaderText = GetLocalResourceObject("rm_RetCb_aspx_cs_HeaderText14").ToString();
        uxReportGrid.Columns.FindByUniqueName("NinetyDaysPostChargebackRDRCount").HeaderText = GetLocalResourceObject("rm_RetCb_aspx_cs_HeaderText15").ToString();
        uxReportGrid.Columns.FindByUniqueName("NinetyDaysPostChargebackRDRAmount").HeaderText = GetLocalResourceObject("rm_RetCb_aspx_cs_HeaderText16").ToString();

        uxReportGrid.Columns.FindByUniqueName("Date").HeaderText = GetLocalResourceObject("rm_RetCb_aspx_cs_HeaderText7").ToString();//"Last Batch Date";
        uxReportGrid.Columns.FindByUniqueName("Amount").HeaderText = GetLocalResourceObject("rm_RetCb_aspx_cs_HeaderText8").ToString();//"Last Batch Amount";

        uxExporter.Formatter = new Dictionary<string, Func<object, string>>();
        uxExporter.Formatter.Add("ChargebackPercentOfSales30Day", new Func<object, string>(PercentageConvert));
        uxExporter.Formatter.Add("ChargebackPercentOfSales90Day", new Func<object, string>(PercentageConvert));

        base.DoNeedExportConfig(sender, exportConfig);
        exportConfig.ReportHeader = uxExporter.GridHeader;
        exportConfig.FileName = GeneralFuncsLib.FormatFileName(uxExporter.GridHeader);
    }

    protected override void DoGridNeedDataSource(AS.Controls.Grid.ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        if (sender == uxReportGrid && uxReportGrid.Visible)
        {
            OnDataBindControls(DataBindAction.BindReportGrid, sender);
        }
    }

    protected override void DoItemDataBound(AS.Controls.Grid.ASGrid sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView dataRow = e.Item.DataItem as DataRowView;
            //checkbox column
            Control ctrl = dataItem["IsReviewed"].FindControl("chkMerchantReviewed");
            if (ctrl != null)
            {
                HtmlInputCheckBox chkBox = ctrl as HtmlInputCheckBox;
                chkBox.Disabled = !GeneralFuncsLib.ReadOnlyRiskManagementEnableStatus((SecurePage)Page);
            }
            // Merchant Name column           
            string url = string.Format(URL_TEMPLATE, dataRow[MERCHANT_NUMBER], "RiskReport", dataRow[MERCHANT_NAME]);

            var merchantName = dataItem[MERCHANT_NAME];
            merchantName.Text = VeraCodeSolution.GetOutputHtmlString(url);
            var colorBorder = Color.Transparent;
            if (dataRow["IsReviewed"].ToBoolean())
            {
                colorBorder = Color.LightPink;
            }
            merchantName.Text = GeneralFuncsLib.FormatBorderText(merchantName.Text, colorBorder);

            merchantName.ToolTip = string.Format("{0}: {1}", GetLocalResourceObject("ASGridBoundColumnResource4.HeaderText").ToString(), GeneralFuncsLib.FormatDate(dataRow[APROVAL_DATE]));

            // Detail column
            url = string.Format(URL_TEMPLATE, dataRow[MERCHANT_NUMBER], "Detail", GetLocalResourceObject("detailText").ToString());
            dataItem["Detail"].Text = VeraCodeSolution.GetOutputHtmlString(url);
        }
        else if (e.Item is GridFooterItem)
        {
            GridFooterItem footerItem = e.Item as GridFooterItem;
        }
        SetTooltipForGrid();
    }

    private void SetTooltipForGrid()
    {
        SetTooltipForColumn(uxReportGrid, "RepresentedCBCount", "ASGridBoundColumnResource7", "OtherChargebacksCount");
        SetTooltipForColumn(uxReportGrid, "ChargebackCount", "ASGridBoundColumnResource8", "FirstTimeCBCount");
        SetTooltipForColumn(uxReportGrid, "ChargebackAmount", "ASGridBoundColumnResource9", "FirstTimeCBAmount");
        if (GeneralFuncsLib.GetDataOfExtendedSetting("SHOW_RDR").ToLower().Equals("true"))
        {
            SetTooltipForColumn(uxReportGrid, "FirstChargebackRDRCount", "ASGridBoundColumnResource19", "FirstChargebackRDRCount");
            SetTooltipForColumn(uxReportGrid, "FirstChargebackRDRAmount", "ASGridBoundColumnResource20", "FirstChargebackRDRAmount");
            SetTooltipForColumn(uxReportGrid, "PostChargebackRDRCount", "ASGridBoundColumnResource21", "PostChargebackRDRCount");
            SetTooltipForColumn(uxReportGrid, "PostChargebackRDRAmount", "ASGridBoundColumnResource22", "PostChargebackRDRAmount");

            SetTooltipForColumn(uxReportGrid, "ThirtyDaysFirstChargebackRDRCount", "ASGridBoundColumnResource23", "FirstChargebackRDRCount");
            SetTooltipForColumn(uxReportGrid, "ThirtyDaysFirstChargebackRDRAmount", "ASGridBoundColumnResource24", "FirstChargebackRDRAmount");
            SetTooltipForColumn(uxReportGrid, "ThirtyDaysPostChargebackRDRCount", "ASGridBoundColumnResource25", "PostChargebackRDRCount");
            SetTooltipForColumn(uxReportGrid, "ThirtyDaysPostChargebackRDRAmount", "ASGridBoundColumnResource26", "PostChargebackRDRAmount");

            SetTooltipForColumn(uxReportGrid, "NinetyDaysFirstChargebackRDRCount", "ASGridBoundColumnResource27", "FirstChargebackRDRCount");
            SetTooltipForColumn(uxReportGrid, "NinetyDaysFirstChargebackRDRAmount", "ASGridBoundColumnResource28", "FirstChargebackRDRAmount");
            SetTooltipForColumn(uxReportGrid, "NinetyDaysPostChargebackRDRCount", "ASGridBoundColumnResource29", "PostChargebackRDRCount");
            SetTooltipForColumn(uxReportGrid, "NinetyDaysPostChargebackRDRAmount", "ASGridBoundColumnResource30", "PostChargebackRDRAmount");
        }
    }

    private void SetTooltipForColumn(ASGrid grid, string colName, string resourceKey, string resourceKeyDescription)
    {
        var columnTarget = grid.MasterTableView.Columns.FindByUniqueName(colName);
        string columnIdCBCount = GetLocalResourceObject(resourceKey + ".HeaderTooltip").ToString();
        if (!columnTarget.HeaderText.Contains(string.Format("id='{0}'", columnIdCBCount)))
        {
            string headerTextColumn = GetLocalResourceObject(resourceKey + ".HeaderText").ToString();
            string columnToolTip = GetLocalResourceObject(resourceKeyDescription + ".HeaderDescription").ToString();
            RM_MCF_GeneralFuncsLib.GenerateTooltipHeaderDefaulColumnID(columnTarget, headerTextColumn, grid, columnIdCBCount, columnToolTip, ToolTipPosition.TopRight);
            columnTarget.HeaderTooltip = " ";
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindReportGrid:

                ShowRDRColumns();

                // Open risk report modal by click on Merchant Name hyperlink. At that page, hierarchy filter mode will be updated to Merchant
                // It causes the issue that get data incorrect.
                if (IsPostBack && RiskSessionManager.MCF_RetrievalsChargebacksReferer != null)
                {
                    UpdateHierarchyFiltering();
                }

                ASGrid grid = (ASGrid)sender;
                string spaName = "spa_RM_MCF_GetRetrievalsChargebacks";
                FilterParameterCollection parames = new FilterParameterCollection();
                parames.AddLoggedInUserReportingParams(false);
                parames.AddHierarchyFilterParams((ReportPage)this.Page);
                parames.Add(new FilterParameter("@ReviewType", Int32.Parse(uxHiddenWorkedType.Value), DbType.Int32));
                if (ExtendedHierarchyFilter != null && ExtendedHierarchyFilter.Select("HierarchyMode = '" + parames.FindFilterParameterByName("@HierarchyFilterMode", false).ParameterValue + "'").FirstOrDefault() != null)
                {

                    if (!string.IsNullOrEmpty(uxHiddenItems.Value))
                    {
                        parames.FindFilterParameterByName("@HierarchyFilterValue", false).ParameterValue = SessionManager.IncludeExcludeItem;
                        parames.Add(new FilterParameter("@IsIncluded", SessionManager.IsIncludeItem, DbType.Boolean));
                    }
                    else
                    {
                        SessionManager.IncludeExcludeItem = null;
                        SessionManager.IsIncludeItem = true;
                        parames.Add(new FilterParameter("@IsIncluded", SessionManager.IsIncludeItem, DbType.Boolean));
                    }
                }
                grid.DataSourceInvoker = new ASFuncInvoker(
                    WebServices.RiskServices,
                    WebSiteConstants.GET_REPORT_METHOD_NAME,
                    new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(parames) });
                break;
        }
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.ChangeWorkType:
                {
                    uxReportGrid.CurrentPageIndex = 0;
                    uxReportGrid.Rebind();
                }
                break;
        }
    }

    [WebMethod(EnableSession = true), ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string WebMethodUpdateMerchantWorkStatus(
        bool status,
        string merchant,
        string to,
        long begindate,
        long enddate,
        string HierarchyMode,
        string HierarchyValue,
        int PageSize,
        int PageIndex,
        string ReviewedType
        )
    {
        rm_MCF_RetCb page = new rm_MCF_RetCb();
         
        string jxResponseType = string.Empty;
        string jxResponseValue = string.Empty;
        DateTime bDate = new DateTime(begindate);
        DateTime eDate = new DateTime(enddate);
        
        if (!string.IsNullOrEmpty(to.Trim()))
        {
            if ("Detail".Equals(to.Trim()))
            {
                string isTurnedBackQueryStr = IS_TURNED_BACK + "=1";
                if (RiskSessionManager.MCF_RetrievalsChargebacksReferer == null)
                {
                    RiskSessionManager.MCF_RetrievalsChargebacksReferer = new MCF_RetrievalsChargebacks();
                }

                RiskSessionManager.MCF_RetrievalsChargebacksReferer.PageSize = PageSize;
                RiskSessionManager.MCF_RetrievalsChargebacksReferer.PageIndex = PageIndex;

                RiskSessionManager.RiskReportReferrer = "RetrievalChargeback";
                RiskSessionManager.RiskReportReferrerInfo =
                    new ReferrerInfo(
                        merchant,
                        "rm_MCF_RetCb.aspx?" + ((SecurePage)page).BuildSecureQueryString(isTurnedBackQueryStr),
                        "Retrievals/Chargebacks");
                string queryString = ((SecurePage)page).BuildSecureQueryString(
                     string.Format(
                         "{0}={1}&{2}={3:MM/dd/yyyy}&{4}={5:MM/dd/yyyy}{6}&{7}",
                         MERCHANT_NUMBER, merchant,
                         BEGIN_DATE, bDate,
                         END_DATE, eDate,
                         page.RetrievalChargebackIntruderQuery,
                         isTurnedBackQueryStr));
                jxResponseType = "Redirect";
                jxResponseValue = "rm_MCF_RetCbDetail.aspx?" + queryString;
            }
            else if ("RiskReport".Equals(to.Trim()))
            {

                string url = "rm_MCF_RiskReport.aspx?" + page.BuildSecureQueryString(string.Format("merchantnumber={0}&IsPopup={1}{2}", merchant, true, page.RiskReportIntruderQuery));
                jxResponseType = "OpenPopupWindow";
                jxResponseValue = url;
            }
        }
        else
        {
            DataTable result = page.UpdateMerchantReviewedStatus(bDate, merchant, status);
            if (status && result != null && result.Rows.Count > 0)
            {
                DataRow item = result.Rows[0];
                if (item["ReturnValue"].ToString() == "1")
                {
                    jxResponseType = "Msg";
                    jxResponseValue = item["WarningMessage"].ToString();
                }
            }
        }

        return JsonConvert.SerializeObject(new { ResponseType = jxResponseType, ResponseValue = jxResponseValue });

    }

    protected void uxChangeWorkType_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.ChangeWorkType);
    }

    protected void OnInitUxReportGrid(object sender, EventArgs e)
    {
        // Keep PageSize
        if (RiskSessionManager.MCF_RetrievalsChargebacksReferer != null)
        {
            uxReportGrid.PageSize = RiskSessionManager.MCF_RetrievalsChargebacksReferer.PageSize;
            uxReportGrid.MasterTableView.PageSize = RiskSessionManager.MCF_RetrievalsChargebacksReferer.PageSize;
        }
    }

    #endregion Protected Methods

    #region Private Methods
    private void BindIncludeExcludeExtendFilter()
    {
        // get iso number from db
        if (!string.IsNullOrEmpty(uxHiddenItems.Value))
        {
            ClientScript.RegisterStartupScript(GetType(), "startupscriptRefreshIncludeExcludeItems", "$(document).ready(function () {RefreshIncludeExcludeItems('" + SessionManager.IncludeExcludeItem + "','" + SessionManager.IsIncludeItem + "');});", true);
            var _temp = ExtendedHierarchyFilter.Select("HierarchyMode = '" + ReportFilter.CurrentValue.HierarchyMode + "'");
            if (_temp != null && _temp.Length > 0)
            {
                uxHiddenRefreshHF.Value = _temp[0]["Hierarchyname"].ToString();
            }
        }
        else
        {
            SessionManager.IsIncludeItem = true;
            ClientScript.RegisterStartupScript(GetType(), "startupscriptRefreshIncludeExcludeItems", "RefreshIncludeExcludeItems('','true');", true);
        }
    }

    private DataTable UpdateMerchantReviewedStatus(DateTime reportDate, string merchantNumber, bool status)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(false);
        parameters.AddLanguageID();
        parameters.Add(new FilterParameter("@ReportDate", reportDate, DbType.DateTime));
        parameters.Add(new FilterParameter("@MerchantNumber", merchantNumber, DbType.String));
        parameters.Add(new FilterParameter("@Status", status, DbType.Boolean));
        return WebServices.RiskServices.GetReports("spa_RM_MCF_UpdateMerchantReviewed ", parameters);
    }

    private void UpdateHierarchyFiltering()
    {
        // Set Filter
        ReportFilter.CurrentValue.HierarchyMode = RiskSessionManager.MCF_RetrievalsChargebacksReferer.HierarchyMode;
        ReportFilter.CurrentValue.Value = RiskSessionManager.MCF_RetrievalsChargebacksReferer.HierarchyValue;
        ReportFilter.CurrentValue.ID = GeneralFuncsLib.GetHierarchyInfo(
            RiskSessionManager.MCF_RetrievalsChargebacksReferer.HierarchyMode).HierarchyID;
    }

    private void UpdateReportSetting()
    {
        UpdateHierarchyFiltering();

        // ReviewedType
        ReviewedType = RiskSessionManager.MCF_RetrievalsChargebacksReferer.ReviewedType;
        uxHiddenWorkedType.Value = ((int)ReviewedType).ToString();

        // Sorting
        if (RiskSessionManager.MCF_RetrievalsChargebacksReferer != null
            && !string.IsNullOrEmpty(RiskSessionManager.MCF_RetrievalsChargebacksReferer.SortExpression)
            && RiskSessionManager.MCF_RetrievalsChargebacksReferer.SortOrder.ToLower() != "none")
        {
            string sortExpression = string.Format("{0} {1}",
                RiskSessionManager.MCF_RetrievalsChargebacksReferer.SortExpression,
                RiskSessionManager.MCF_RetrievalsChargebacksReferer.SortOrder == GridSortOrder.Descending.ToString()
                    ? GeneralFuncsLib.DESC : GeneralFuncsLib.ASC);
            uxReportGrid.MasterTableView.SortExpressions.AddSortExpression(sortExpression);
            uxReportGrid.AS_SortExpression = sortExpression;
        }

        // Keep Page Index
        uxReportGrid.MasterTableView.CurrentPageIndex = RiskSessionManager.MCF_RetrievalsChargebacksReferer.PageIndex;
    }

    private void SetRetrievalsChargebackReferer(bool resetPageSizeAndIndex)
    {
        if (RiskSessionManager.MCF_RetrievalsChargebacksReferer == null || resetPageSizeAndIndex)
        {
            RiskSessionManager.MCF_RetrievalsChargebacksReferer = new MCF_RetrievalsChargebacks();
        }

        RiskSessionManager.MCF_RetrievalsChargebacksReferer.HierarchyMode
            = ReportFilter.CurrentValue.HierarchyMode;
        RiskSessionManager.MCF_RetrievalsChargebacksReferer.HierarchyValue
            = ReportFilter.CurrentValue.Value;
        RiskSessionManager.MCF_RetrievalsChargebacksReferer.ReviewedType = ReviewedType;
        if (!resetPageSizeAndIndex)
        {
            RiskSessionManager.MCF_RetrievalsChargebacksReferer.PageSize
                = uxReportGrid.MasterTableView.PageSize;
            RiskSessionManager.MCF_RetrievalsChargebacksReferer.PageIndex
                = uxReportGrid.MasterTableView.CurrentPageIndex;
        }
    }

    #endregion Private Methods

    #endregion Methods

    protected void uxReportFiltering_Filtering(object sender, EventArgs e)
    {
        RiskSessionManager.MCF_RetrievalsChargebacksReferer.HierarchyMode
                    = ReportFilter.CurrentValue.HierarchyMode;
        RiskSessionManager.MCF_RetrievalsChargebacksReferer.HierarchyValue
            = ReportFilter.CurrentValue.Value;
    }

    private void ShowRDRColumns()
    {
        if (GeneralFuncsLib.GetDataOfExtendedSetting("SHOW_RDR").ToLower().Equals("true"))
        {
            var rdrColumnNames = GeneralFuntionBusiness.GetRDRColumnNames("rm_MCF_RetCb");
            foreach (var columnName in rdrColumnNames)
            {
                var column = uxReportGrid.Columns.FindByUniqueName(columnName);
                if (column != null)
                {
                    column.Visible = true;
                }
            }
        }
    }
}
