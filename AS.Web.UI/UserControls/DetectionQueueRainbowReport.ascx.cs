using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using AS.Utilities;
using AS.Controls.Exporter;
using AS.Controls.Grid;
using AS.Common;
using AS.Common.DBManager;
using System.Collections.Generic;
using System.Drawing;
using AS.Web.Business;
using AS.Controls.Pages;
using System.Globalization;
using AS.Common.Formater;

public partial class UserControls_DetectionQueueRainbowReport : GlobalUserControl
{
    enum DataBindAction
    {
        BindRainbowGrid
    }

    public event AfterGridSortHandler AfterGridSort;
    public delegate void AfterGridSortHandler(string colSort);

    private const string SESSION_FILTERING_OPTIONS = "DetectionQueueFilteringOptions";
    private readonly string[] UNCHANGE_COLUMNS = { "CheckBoxColumn", "RQCheckbox", "MerchantName", "CardView", "MerchantNumber" };
    private const string MERCHANT_NUMBER = "MerchantNumber";
    private const string RECORD_ID = "RecordID";
    private const string DESCEND = "Descending";
    private const string ASCEND = "Ascending";
    private const string NONE = "None";
    private const int ROW_HEIGHT = 30;
    private string _merchantIntruderQuery = string.Empty;
    private bool _isSorting = false;

    private string MerchantIntruderQuery
    {
        get
        {
            if (_merchantIntruderQuery.IsNullOrEmpty())
            {
                _merchantIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(
                    uxReportGrid.ID, new string[] { MERCHANT_NUMBER });
            }
            return _merchantIntruderQuery;
        }
    }

    private bool IsWQ
    {
        get
        {
            return RiskSessionManager.DetectionQueue.AssignmentType == WebSiteEnums.AssignmentType.WorkQueue;
        }
    }

    public void Rebind(bool keepPageIndex, bool isFirstTimeLoading)
    {
        if (!keepPageIndex)
        {
            uxReportGrid.MasterTableView.CurrentPageIndex = 0;
            if (RiskSessionManager.DetectionQueue != null)
            {
                RiskSessionManager.DetectionQueue.SortExpression = "";
            }
        }
        else
        {
            if (isFirstTimeLoading)
            {
                if (RiskSessionManager.DetectionQueue != null)
                {
                    uxReportGrid.MasterTableView.CurrentPageIndex = RiskSessionManager.DetectionQueue.PageIndex;
                }
            }
        }
        uxReportGrid.Rebind();
    }

    protected void uxReportGrid_Init(object sender, EventArgs e)
    {
        if (RiskSessionManager.DetectionQueue != null)
            this.uxReportGrid.PageSize = RiskSessionManager.DetectionQueue.PageSize;
    }

    private GridSortOrder ConvertValueToSortOrder(string value)
    {
        switch (value.ToLower())
        {
            case "descending":
                return GridSortOrder.Descending;
            case "ascending":
                return GridSortOrder.Ascending;
            default:
                return GridSortOrder.None;
        }
    }

    private void SortHandle(string fieldName)
    {
        if (fieldName == RiskSessionManager.DetectionQueue.SortExpression)
        {
            switch (RiskSessionManager.DetectionQueue.SortOrder)
            {
                case DESCEND:
                    RiskSessionManager.DetectionQueue.SortOrder = ASCEND;
                    break;
                case ASCEND:
                    RiskSessionManager.DetectionQueue.SortOrder = NONE;
                    break;
                default:
                    RiskSessionManager.DetectionQueue.SortOrder = DESCEND;
                    break;
            }
        }
        else
        {
            RiskSessionManager.DetectionQueue.SortExpression = fieldName;
            RiskSessionManager.DetectionQueue.SortOrder = DESCEND;
        }
    }

    protected void uxReportGrid_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (optCard.Checked)
        {
            if (e.CommandName.StartsWith("Sort_"))
            {
                _isSorting = true;
                var fieldName = e.CommandName.Split('_')[1];
                SortHandle(fieldName);
                uxReportGrid.Rebind();
            }
        }
    }

    protected void uxReportGrid_SortCommand(object sender, GridSortCommandEventArgs e)
    {

        //GridSortExpression sortExpr = new GridSortExpression();
        //switch (e.NewSortOrder)
        //{
        //    case GridSortOrder.None:
        //        sortExpr.FieldName = e.SortExpression;
        //        sortExpr.SortOrder = GridSortOrder.None;

        //        e.Item.OwnerTableView.SortExpressions.AddSortExpression(sortExpr);
        //        break;
        //    case GridSortOrder.Descending:
        //        sortExpr.FieldName = e.SortExpression;
        //        sortExpr.SortOrder = GridSortOrder.Descending;

        //        e.Item.OwnerTableView.SortExpressions.AddSortExpression(sortExpr);
        //        break;
        //    case GridSortOrder.Ascending:
        //        sortExpr.FieldName = e.SortExpression;
        //        sortExpr.SortOrder = GridSortOrder.Ascending;
        //        e.Item.OwnerTableView.SortExpressions.AddSortExpression(sortExpr);
        //        break;

        //}
        //RiskSessionManager.DetectionQueue.SortExpression = sortExpr.FieldName;
        //RiskSessionManager.DetectionQueue.SortOrder = e.NewSortOrder.ToString();

    }

    protected string BuildSortExpression(GridSortExpression e)
    {
        string col = e.FieldName;
        string order = e.SortOrder.ToString();
        order = order == "Descending" ? "DESC" :
                order == "None" ? "" : "ASC";
        return order == "" ? "" : col + " " + order;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        #region Do NOT edit
        uxReportGrid.IsIntruder = true;
        uxReportGrid.IntruderSourceName = GeneralFuncsLib.GetRequestFileName() + uxReportGrid.ID;

        GenerateAllColumnsReportGrid();
        SetDisplayColumns();
        if (!IsPostBack)
        {
            LoadReportViewMode();
            if (Session[SESSION_FILTERING_OPTIONS] != null)
            {
                string[] parts = ((string)(Session[SESSION_FILTERING_OPTIONS])).Split(';');

                if (parts.Length == 2)
                {
                    //uxReportGrid.MasterTableView.CurrentPageIndex = 0;
                    uxReportGrid.AS_SortExpression = parts[1];
                }
            }

            uxReportGrid.Rebind();
        }
        #endregion
    }

    protected override void OnPreRender(EventArgs e)
    {
        //Hide/Show columns by selecting view mode
        SwitchView();
        //SetDisplayColumns();
        base.OnPreRender(e);

    }

    protected void uxExport_OnNeedExportConfig(object sender, ExportConfig exportConfig)
    {
        //Comment out for fixing bug #34814
        //exportConfig.AllowHtmlEncoded = true;
        exportConfig.ReportHeader = RiskSessionManager.DetectionQueue.Header;
        exportConfig.FileName = GeneralFuncsLib.FormatFileName(RiskSessionManager.DetectionQueue.Header);
        if (uxReportGrid.Columns.FindByUniqueNameSafe("CheckBoxColumn") != null)
            uxReportGrid.Columns.FindByUniqueNameSafe("CheckBoxColumn").Visible = false;
        if (uxReportGrid.Columns.FindByUniqueNameSafe("MerchantNumber") != null)
            uxReportGrid.Columns.FindByUniqueNameSafe("MerchantNumber").Visible = true;
        if (uxReportGrid.Columns.FindByUniqueNameSafe("RQCheckbox") != null)
            uxReportGrid.Columns.FindByUniqueNameSafe("RQCheckbox").Visible = false;
        if (uxReportGrid.Columns.FindByUniqueNameSafe("CardView") != null)
            uxReportGrid.Columns.FindByUniqueNameSafe("CardView").Visible = false;

        List<GridColumn> listCols = new List<GridColumn>();
        foreach (GridColumn col in uxReportGrid.MasterTableView.Columns)
        {
            if (col.Display && col.Visible)
            {
                listCols.Add(col);
            }
        }
        uxReportGrid.MasterTableView.Columns.Clear();
        uxExporterTop.Formatter = new Dictionary<string, Func<object, string>>();
        listCols = listCols.OrderBy(c => c.OrderIndex).ToList();
        foreach (GridColumn col in listCols)
        {
            if (col != null)
            {
                uxReportGrid.MasterTableView.Columns.Add(col);
                if (col.UniqueName.Equals("VolumePercent"))
                {
                    uxExporterTop.Formatter.Add("VolumePercent", new Func<object, string>(MyConvert));
                }

                if (col.UniqueName.Equals("ContractualVolume"))
                {
                    uxExporterTop.Formatter.Add("ContractualVolume", new Func<object, string>(MyConvertPercent));
                }
                if (col.UniqueName.Equals("AverageTicketPercent"))
                {
                    uxExporterTop.Formatter.Add("AverageTicketPercent", new Func<object, string>(MyConvert));
                }
                if (col.UniqueName.Equals("AuthorizationPercent"))
                {
                    uxExporterTop.Formatter.Add("AuthorizationPercent", new Func<object, string>(MyConvert));
                }
                if (col.UniqueName.Equals("DeclinedAuthorizationPercent"))
                {
                    uxExporterTop.Formatter.Add("DeclinedAuthorizationPercent", new Func<object, string>(MyConvert));
                }
                if (col.UniqueName.Equals("KeyPercent"))
                {
                    uxExporterTop.Formatter.Add("KeyPercent", new Func<object, string>(MyConvert));
                }
                if (col.UniqueName.Equals("EvenDollarTransactionPercent"))
                {
                    uxExporterTop.Formatter.Add("EvenDollarTransactionPercent", new Func<object, string>(MyConvert));
                }
                if (col.UniqueName.Equals("DuplicateDollarTransactionPercent"))
                {
                    uxExporterTop.Formatter.Add("DuplicateDollarTransactionPercent", new Func<object, string>(MyConvert));
                }
                if (col.UniqueName.Equals("ReturnPercent"))
                {
                    uxExporterTop.Formatter.Add("ReturnPercent", new Func<object, string>(MyConvert));
                }

            }
        }

    }

    protected void uxReportGrid_DataSourceReady(object sender, EventArgs e)
    {
        if (RiskSessionManager.DetectionQueue != null)
        {
            RiskSessionManager.DetectionQueue.PageSize = uxReportGrid.PageSize;
            RiskSessionManager.DetectionQueue.PageIndex = uxReportGrid.MasterTableView.CurrentPageIndex;
        }
        //use for requeque
        if (GeneralFuncsLib.HasQueuingMechanismFeature)
        {
            RiskSessionManager.DetectionQueueTemporaryRequeueInfo.TotalAlertMerchant = uxReportGrid.MasterTableView.VirtualItemCount;
        }
        if (uxReportGrid.AS_DataSource.Rows.Count == 0 || uxReportGrid.AS_DataSource == null)
        {
            chkHeaderCV.Visible = false;
        }
    }

    private string MyConvert(object obj)
    {
        return Decimal.Parse(((int)obj).ToString()).ToString("#,#0") + "%";
    }

    private string MyConvertEmptyDash(object obj)
    {
        if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
            return Decimal.Parse(((int)obj).ToString()).ToString("#,#0") + "%";
        else
            return WebSiteConstants.HTML_EM_DASH_ENCODE;
    }

    private string MyConvertPercent(object obj)
    {
        if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
            return string.Format("{0:N}%", (Decimal.Parse(obj.ToString())));
        else
            return WebSiteConstants.HTML_EM_DASH_ENCODE;
    }

    public void BuildCustomViewLink()
    {
        uxExporterTop.BuildCustomViewLink();
        //if (RiskSessionManager.DetectionQueue != null && RiskSessionManager.DetectionQueue.AssignmentID > 0)
        //{
        //    uxCustomizeColumnLink.Visible = true;
        //    string encodeURL = string.Format("rm_DetectionManageCustomViewsModal.aspx?" + this.Page.BuildSecureQueryString("AssignmentID=" + RiskSessionManager.DetectionQueue.AssignmentID));
        //    uxCustomizeColumnLink.OnClientClick = "return doOpenNewPopup('" + encodeURL + "');";
        //}
        //else
        //{
        //    uxCustomizeColumnLink.Visible = false;
        //}
    }
    protected override void OnDataBindControls(Enum type, object sender)
    {
        if (Page.IsIntruderDetected) return;

        switch ((DataBindAction)type)
        {
            case DataBindAction.BindRainbowGrid:
                {
                    //for requeue feature
                    if (RiskSessionManager.DetectionQueue == null ||
                        (RiskSessionManager.DetectionQueue != null && RiskSessionManager.DetectionQueue.AssignmentID < 1))
                    {
                        uxReportGrid.DataSource = new DataTable();
                        return;
                    }

                    uxExporterTop.GridHeader = VeraCodeSolution.ValidateResponseData(RiskSessionManager.DetectionQueue.Header);
                    uxExporterTop.GridTitle = VeraCodeSolution.ValidateResponseData(RiskSessionManager.DetectionQueue.Header.Replace(":", ""));
                    uxExporterTop.GridSubTitle = VeraCodeSolution.ValidateResponseData(RiskSessionManager.DetectionQueue.Header.Replace(Resources.LanguageResource.RiskEntities_Header_Assignment + " ", string.Empty));


                    if (RiskSessionManager.DetectionQueue != null && !string.IsNullOrEmpty(RiskSessionManager.DetectionQueue.SortExpression) && RiskSessionManager.DetectionQueue.SortOrder.ToLower() != "none")
                    {
                        uxReportGrid.AS_SortExpression = RiskSessionManager.DetectionQueue.SortExpression + " " + (RiskSessionManager.DetectionQueue.SortOrder == "Descending" ? "DESC" : "ASC");
                    }
                    else
                    {
                        uxReportGrid.AS_SortExpression = RiskSessionManager.DetectionQueue.OrderBy;
                    }

                    if (AfterGridSort != null)
                        AfterGridSort(uxReportGrid.AS_SortExpression);

                    Session[SESSION_FILTERING_OPTIONS] = string.Format("{0};{1}",
                       uxReportGrid.MasterTableView.CurrentPageIndex, uxReportGrid.AS_SortExpression);

                    string spName = "spa_rm_cs_GetRainbowReport";
                    FilterParameterCollection parameterList = new FilterParameterCollection();
                    parameterList.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parameterList.Add(new FilterParameter("@AssignmentID", RiskSessionManager.DetectionQueue.AssignmentID, DbType.Int32));
                    parameterList.Add(new FilterParameter("@ReportDate", RiskSessionManager.DetectionQueue.ReportDate, DbType.Date));
                    parameterList.Add(new FilterParameter("@Mode", (int)RiskSessionManager.DetectionQueue.MerchantWorkedType, DbType.Int32));
                    parameterList.Add(new FilterParameter("@LanguageID", SessionManager.CurrentLanguage, DbType.Int32));
                    uxReportGrid.DataSourceInvoker = new ASFuncInvoker(WebServices.RiskServices, "GetReports", new object[] { spName, ReportServices.ConvertToFilterParamWSArray(parameterList) });

                    uxReportGrid.VisibleGrid(true);
                }
                break;
        }
    }

    protected void uxReportGrid_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.BindRainbowGrid, uxReportGrid);
    }

    private Color GetActiveDayColor(string activeDayFlag)
    {
        if (activeDayFlag == "1")
            return Color.Red;
        else if (activeDayFlag == "2")
            return Color.Brown;
        else if (activeDayFlag == "3")
            return Color.Blue;
        else if (activeDayFlag == "4")
            return Color.Green;
        else
            return Color.Black;
    }

    protected void ChangeView(object sender, EventArgs e)
    {
        uxReportGrid.Rebind();
    }

    private bool HasRQColumn
    {
        get
        {
            return GeneralFuncsLib.HasQueuingMechanismFeature
                                && RiskSessionManager.DetectionQueue != null
                                && RiskSessionManager.DetectionQueue.ReportDate == DateTime.Today;
        }
    }

    private string[] Default_Invisible_Columns = new string[] { "MerchantNumber" };
    private void SwitchView()
    {
        foreach (GridColumn col in uxReportGrid.MasterTableView.Columns)
        {
            col.Visible = optGrid.Checked && !Default_Invisible_Columns.Contains(col.UniqueName);
            if (col.UniqueName.Equals("CardView", StringComparison.InvariantCultureIgnoreCase))
            {
                col.Visible = !optGrid.Checked;
            }
            else if (col.UniqueName.Equals("RQCheckbox", StringComparison.InvariantCultureIgnoreCase))
            {
                col.Visible = HasRQColumn && !optCard.Checked;
            }
        }

        chkHeaderCV.Visible = optCard.Checked && HasRQColumn;
        uxExporterTop.ShowHideCustomViewLink(!optCard.Checked);
        if (optCard.Checked)
        {
            uxReportGrid.CssClass += " block-card-view ";
            ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript("setTabActive('0');");
        }
        else
        {
            uxReportGrid.CssClass = uxReportGrid.CssClass.Replace("block-card-view", "").Trim();
            ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript("setTabActive('1');");
        }
    }

    /// <summary>
    /// Get user setting for default view mode
    /// </summary>
    private void LoadReportViewMode()
    {
        var userRiskView = PersonalDataHelper.GetJSONConfig<string>(UserConfigNames.CONFIG_USER_PROFILE_RISK_DETECTION_QUEUE_VIEW);
        optGrid.Checked = string.IsNullOrEmpty(userRiskView) || userRiskView == "Grid";
        optCard.Checked = userRiskView == "Card";
    }

    protected void uxReportGrid_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridHeaderItem)
        {
            if (optGrid.Checked)
            {
                GridHeaderItem headerItem = e.Item as GridHeaderItem;
                if (headerItem["RQCheckbox"].Visible)
                {
                    RequeueAllGridView(headerItem);
                }
            }
            else
            {
                if (optCard.Checked && HasRQColumn)
                {
                    RequeueAllCardView();
                }
            }
        }
        else if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            var rowItem = (e.Item.DataItem as DataRowView).Row;

            if (optGrid.Checked)
            {
                //Custom for GridView
                uxReportGrid_ItemDataBound_GridView(dataItem, rowItem);
            }
            else
            {
                //Custom for CardView
                uxReportGrid_ItemDataBound_CardView(dataItem, rowItem, e.Item.ItemIndex);
            }
        }
    }

    private void uxReportGrid_ItemDataBound_GridView(GridDataItem dataItem, DataRow rowItem)
    {
        var merchantNumber = rowItem["MerchantNumber"];

        if (dataItem["RQCheckbox"].Visible)
        {
            HtmlInputCheckBox chkRequeueSingleMerchant = dataItem["RQCheckbox"].FindControl("chkItem") as HtmlInputCheckBox;
            if (chkRequeueSingleMerchant != null)
            {
                //this merchant was requeued before, gray out the column
                if (bool.Parse(rowItem["IsRequeued"].ToString()))
                {
                    dataItem["RQCheckbox"].BackColor = Color.FromName("#BBB");
                }

                //process check state from spa
                chkRequeueSingleMerchant.Disabled = !GeneralFuncsLib.ReadOnlyRiskManagementEnableStatus((SecurePage)Page);
                if (RiskSessionManager.DetectionQueueTemporaryRequeueInfo.RequeuedMerchantList != null)
                {
                    chkRequeueSingleMerchant.Checked = RiskSessionManager.DetectionQueueTemporaryRequeueInfo.RequeuedMerchantList.Contains(merchantNumber.ToString());
                }
                if (RiskSessionManager.DetectionQueueTemporaryRequeueInfo.IsRequeueAll)
                {
                    chkRequeueSingleMerchant.Checked = true;
                    chkRequeueSingleMerchant.Disabled = true;
                }

                chkRequeueSingleMerchant.Value = merchantNumber.ToString();
                chkRequeueSingleMerchant.Attributes.Add("onclick", string.Format("RequeueSingleMerchant(this," + IsWQ.ToString().ToLower() + ")"));
            }
        }
        //checkbox column
        Control ctrl = dataItem["CheckBoxColumn"].FindControl("cBox");

        if (ctrl != null)
        {
            HtmlInputCheckBox chkBox = ctrl as HtmlInputCheckBox;
            chkBox.Checked = rowItem["Worked"].ToString().Trim() == "1";
            chkBox.Value = rowItem["MerchantNumber"].ToString();
            chkBox.Attributes.Add("onclick", string.Format("ChangeMerchantWorked(this,{0})", rowItem["TodayVolume"]));
            chkBox.Disabled = !GeneralFuncsLib.ReadOnlyRiskManagementEnableStatus((SecurePage)Page);
        }

        //merchant name
        var merchantName = dataItem["MerchantName"];

        string url = String.Empty;
        string urlText = rowItem["MerchantName"].ToString();
        //RiskGeneral.BuildMerchantNameWithFontTag(rowItem["MerchantName"].ToString(),GeneralFuncsLib.NvlString(rowItem["ActiveDayFlag"]));

        if (GeneralFuncsLib.HasRiskRptPermission((SecurePage)Page))
        {
            url = RiskGeneral.BuildMerchantHyperlinkInRisk((SecurePage)Page,
                rowItem[MERCHANT_NUMBER], true, MerchantIntruderQuery, urlText);
            url = string.Format("<a class=\"link\" href=\"#\" style=\"cursor:pointer\" onclick=\"MerchantNumberClick('{0}','{1}',this); return false;\">", rowItem["MerchantNumber"], rowItem["TodayVolume"]) + urlText + "</a>";
        }

        merchantName.Text = VeraCodeSolution.GetOutputHtmlString(url);
        var merchantNameColor = Color.Transparent;
        if (rowItem["Worked"].ToString() == "1")
        {
            merchantNameColor = Color.LightPink;

        }
        merchantName.Text = GeneralFuncsLib.FormatBorderText(merchantName.Text, merchantNameColor);

        if (rowItem["Watch"].Equals(1))
        {
            merchantName.Text = VeraCodeSolution.DoVeraCode(merchantName.Text + "(<span style=\"color:red\">*</span>)");

            merchantName.ToolTip = VeraCodeSolution.DoVeraCode(
                "(M)" + rowItem["MerchantNumber"].ToString().Trim() +
                (rowItem["ApprovalDate"] == DBNull.Value ? "" : " - " + ((DateTime)rowItem["ApprovalDate"]).ToString("MM/dd/yyyy")) +
                (rowItem["MDDescription"] == DBNull.Value ? "" : " - " + rowItem["MDDescription"])
                );
        }
        else if (rowItem["MultiWatch"].Equals(true))
        {
            merchantName.Text = VeraCodeSolution.DoVeraCode(merchantName.Text + "(<span style=\"color:red\">*</span>)");

            merchantName.ToolTip = VeraCodeSolution.DoVeraCode(
                 rowItem["MerchantNumber"].ToString().Trim() +
                (rowItem["ApprovalDate"] == DBNull.Value ? "" : " - " + ((DateTime)rowItem["ApprovalDate"]).ToString("MM/dd/yyyy")) +
                (rowItem["MDDescription"] == DBNull.Value ? "" : " - " + rowItem["MDDescription"])
                );
        }
        else
        {
            merchantName.ToolTip = VeraCodeSolution.DoVeraCode(
                 rowItem["MerchantNumber"].ToString().Trim() +
                (rowItem["ApprovalDate"] == DBNull.Value ? "" : " - " + ((DateTime)rowItem["ApprovalDate"]).ToString("MM/dd/yyyy")) +
                (rowItem["MDDescription"] == DBNull.Value ? "" : " - " + rowItem["MDDescription"])
                );
        }

        string assignmentTypeQueryString = RiskSessionManager.DetectionQueue.AssignmentType != WebSiteEnums.AssignmentType.DetectionQueue ? "&AssignmentType=" + (int)RiskSessionManager.DetectionQueue.AssignmentType : "";
        string queryStringImage = this.Page.BuildSecureQueryString("MerchantNumber=" + rowItem[MERCHANT_NUMBER] + "&ReportDate=" + RiskSessionManager.DetectionQueue.ReportDate + "&AssignmentID=" + RiskSessionManager.DetectionQueue.AssignmentID + assignmentTypeQueryString);
        string urlImage = "rm_DQReasonModal.aspx?" + queryStringImage;
        urlImage = "<a class=\"image-link\" href=\"#\" style=\"cursor:pointer\" onclick=\"OpenInstanceWindow('" + urlImage + "','DQWindow'); return false;\"><img src='" + ResolveUrl("~/") + "res/img/information.png' border='0' alt='" + GetLocalResourceObject("DetectionQueueRainbowReport_ascx_PV").ToString() + "' /></a>";
        merchantName.Text = VeraCodeSolution.DoVeraCode(urlImage + " " + merchantName.Text);


        //profile
        var profile = dataItem["ProfileDescription"];
        string profiletext = rowItem["ProfileDescription"].ToString();
        profile.ToolTip = VeraCodeSolution.DoVeraCode(profiletext);

        //riskscore
        var riskScore = dataItem["RiskScore"];
        Color riskScoreColor = rowItem["RiskScoreColor"].ToString().ToColor();

        if (!GeneralFuncsLib.NvlString(rowItem["RiskScore"]).Equals("0") && rowItem["RiskScore"] != DBNull.Value
            && !rowItem["RiskScore"].ToString().IsNullOrEmpty())
        {
            string queryString = this.Page.BuildSecureQueryString("MerchantNumber=" + rowItem[MERCHANT_NUMBER] + "&ReportDate=" + RiskSessionManager.DetectionQueue.ReportDate);
            string urlRiskScoreDetail = "rm_RiskScoreDetailModal.aspx?" + queryString;
            url = "<a class=\"link\" href=\"#\" style=\"cursor:pointer\" onclick=\"ShowPopupModal('" + urlRiskScoreDetail + "','auto'); return false;\">";

            riskScore.Text = VeraCodeSolution.GetOutputHtmlString(url + dataItem["RiskScore"].Text + "</a>");
        }
        else
        {
            riskScore.Text = VeraCodeSolution.DoVeraCode(rowItem["RiskScore"].ToString().Trim() != string.Empty ?
                dataItem["RiskScore"].Text : "0");
        }
        riskScore.Text = GeneralFuncsLib.FormatBorderText(riskScore.Text, riskScoreColor);

        //volume percent
        var volumePercent = dataItem["VolumePercent"];
        volumePercent.ToolTip = VeraCodeSolution.DoVeraCode(string.Format("{0:C}", rowItem["TodayVolume"]).ToCurrencySymbol());
        Color volPercentColor = rowItem["VolumeColor"].ToString().ToColor();
        string rptDate = RiskSessionManager.DetectionQueue.ReportDate.ToShortDateString();
        if (!GeneralFuncsLib.NvlString(rowItem["VolumePercent"]).Equals("0") && GeneralFuncsLib.NvlString(rowItem["VolumePercent"]).Length > 0)
        {
            string queryString1 = this.Page.BuildSecureQueryString(string.Format("MerchantNumber={0}&ReportDate={1}",
            rowItem["MerchantNumber"], rowItem["ReportDate"]));
            string url1 = string.Format("<a class=\"link\" href=\"#\" style=\"cursor:pointer\" onclick=\"OpenInstanceWindow('rm_TransactionDetailsModal.aspx?{0}','DQWindow'); return false;\">", queryString1);
            decimal VolumePercent = 0;
            decimal.TryParse(GeneralFuncsLib.NvlString(rowItem["VolumePercent"]), out VolumePercent);

            dataItem["VolumePercent"].Text = VeraCodeSolution.DoVeraCode(url1 + VolumePercent.ToString("#,#0") + "%" + "</a>");
        }
        else
        {
            volumePercent.Text = VeraCodeSolution.DoVeraCode(rowItem["VolumePercent"].ToString().Trim() != string.Empty ?
                 (decimal.Parse(rowItem["VolumePercent"].ToString()).ToString("#,#0") + "%") : string.Empty);
        }
        volumePercent.Text = GeneralFuncsLib.FormatBorderText(volumePercent.Text, volPercentColor);

        // 40965

        var contractualVolume = dataItem["ContractualVolume"];
        Color contractualVolumeColor = rowItem["CVColor"].ToString().ToColor();
        if (rowItem["ContractualVolume"] != DBNull.Value)
        {
            contractualVolume.ToolTip = VeraCodeSolution.DoVeraCode(string.Format("{0:C}", rowItem["ContractualDailyVolume"]).ToCurrencySymbol());
        }
        else
        {
            contractualVolume.ToolTip = GetLocalResourceObject("ContractualValuenotBeenProvided_Resource").ToString();
        }
        contractualVolume.Text = VeraCodeSolution.DoVeraCode(rowItem["ContractualVolume"] != DBNull.Value ?
                        string.Format("{0:N}%", (decimal.Parse(rowItem["ContractualVolume"].ToString()))) : WebSiteConstants.HTML_EM_DASH_ENCODE);
        contractualVolume.Text = GeneralFuncsLib.FormatBorderText(contractualVolume.Text, contractualVolumeColor);


        //average ticket
        var avgTicket = dataItem["AverageTicketPercent"];
        Color avgTicketColor = rowItem["AvgTktColor"].ToString().ToColor();
        avgTicket.ToolTip = VeraCodeSolution.DoVeraCode(string.Format("{0:C}", rowItem["AverageTicketVolume"]).ToCurrencySymbol());
        avgTicket.Text = VeraCodeSolution.DoVeraCode(rowItem["AverageTicketPercent"].ToString() != string.Empty ?
            (decimal.Parse(rowItem["AverageTicketPercent"].ToString()).ToString("#,#0") + "%") : string.Empty);
        avgTicket.Text = GeneralFuncsLib.FormatBorderText(avgTicket.Text, avgTicketColor);

        //auth percent
        var authPercent = dataItem["AuthorizationPercent"];
        Color authPercentColor = rowItem["AuthColor"].ToString().ToColor();
        authPercent.ToolTip = VeraCodeSolution.DoVeraCode(string.Format("{0:C} ({1})", rowItem["TodayAuthorizationVolume"], rowItem["TodayAuthorizationCount"]).ToCurrencySymbol());
        if (!GeneralFuncsLib.NvlString(rowItem["AuthorizationPercent"]).Equals("0") && GeneralFuncsLib.NvlString(rowItem["AuthorizationPercent"]).Length > 0)
        {
            string queryString1 = this.Page.BuildSecureQueryString(string.Format("MerchantNumber={0}&ReportDate={1}",
            rowItem["MerchantNumber"], rowItem["ReportDate"]));
            string url1 = string.Format("<a class=\"link\" href=\"#\" style=\"cursor:pointer\" onclick=\"OpenInstanceWindow('NewRiskReport_TransactionHistoryModal.aspx?{0}','DQWindow'); return false;\">", queryString1);
            decimal authPerc = 0;
            decimal.TryParse(GeneralFuncsLib.NvlString(rowItem["AuthorizationPercent"]), out authPerc);

            dataItem["AuthorizationPercent"].Text = VeraCodeSolution.DoVeraCode(url1 + authPerc.ToString("#,#0") + "%" + "</a>");
        }
        else
        {
            authPercent.Text = VeraCodeSolution.DoVeraCode(rowItem["AuthorizationPercent"].ToString().Trim() != string.Empty ?
           (decimal.Parse(rowItem["AuthorizationPercent"].ToString()).ToString("#,#0") + "%") : string.Empty);
        }
        authPercent.Text = GeneralFuncsLib.FormatBorderText(authPercent.Text, authPercentColor);

        //decline percent
        var decPercent = dataItem["DeclinedAuthorizationPercent"];
        Color decPercentColor = rowItem["DeclAuthPctColor"].ToString().ToColor();
        decPercent.ToolTip = VeraCodeSolution.DoVeraCode(string.Format("{0:C}", rowItem["TodayDeclinedAuthorizationVolume"]).ToCurrencySymbol());
        decPercent.Text = VeraCodeSolution.DoVeraCode(rowItem["DeclinedAuthorizationPercent"].ToString().Trim() != string.Empty ?
            VeraCodeSolution.DoVeraCode((decimal.Parse(rowItem["DeclinedAuthorizationPercent"].ToString())).ToString("#,#0") + "%") : string.Empty);
        decPercent.Text = GeneralFuncsLib.FormatBorderText(decPercent.Text, decPercentColor);

        //# repeat auth
        var rptAuth = dataItem["RepeatAuthorizationCount"];
        Color rptAuthColor = rowItem["RptAuthColor"].ToString().ToColor();
        rptAuth.Text = GeneralFuncsLib.FormatBorderText(rptAuth.Text, rptAuthColor, true);

        //FC (foreign card)
        var fc = dataItem["TodayForeignCardCount"];
        Color fcColor = rowItem["FCColor"].ToString().ToColor();
        fc.ToolTip = VeraCodeSolution.DoVeraCode(string.Format("{0:C}", rowItem["TodayForeignCardVolume"]).ToCurrencySymbol());
        fc.Text = GeneralFuncsLib.FormatBorderText(fc.Text, fcColor, true);

        //key percent
        var keyPercent = dataItem["KeyPercent"];
        Color keyPercentColor = rowItem["KeyColor"].ToString().ToColor();
        keyPercent.ToolTip = VeraCodeSolution.DoVeraCode(string.Format("{0} ({1})", GeneralFuncsLib.FormatCurrencyTooltip(rowItem["TodayKeyVolume"], SessionManager.CurrencyFortmat), rowItem["TodayKeyCount"]).ToCurrencySymbol());
        keyPercent.Text = VeraCodeSolution.DoVeraCode(rowItem["KeyPercent"].ToString().Trim() != string.Empty ?
            VeraCodeSolution.DoVeraCode((decimal.Parse(rowItem["KeyPercent"].ToString())).ToString("#,#0") + "%") : string.Empty);
        keyPercent.Text = GeneralFuncsLib.FormatBorderText(keyPercent.Text, keyPercentColor, true);

        //even percent
        var evenPercent = dataItem["EvenDollarTransactionPercent"];
        Color evenPercentColor = rowItem["EvenColor"].ToString().ToColor();
        evenPercent.Text = VeraCodeSolution.DoVeraCode(rowItem["EvenDollarTransactionPercent"].ToString() != string.Empty ?
            VeraCodeSolution.DoVeraCode((decimal.Parse(rowItem["EvenDollarTransactionPercent"].ToString())).ToString("#,#0") + "%") : string.Empty);
        evenPercent.Text = GeneralFuncsLib.FormatBorderText(evenPercent.Text, evenPercentColor, true);

        //duplicate percent
        var dupPercent = dataItem["DuplicateDollarTransactionPercent"];
        Color dupPercentColor = rowItem["DupColor"].ToString().ToColor();
        dupPercent.ToolTip = VeraCodeSolution.DoVeraCode(rowItem["DuplicateDollarTransactionCount"].ToString().ToCurrencySymbol());
        dupPercent.Text = VeraCodeSolution.DoVeraCode(rowItem["DuplicateDollarTransactionPercent"].ToString() != string.Empty ?
            VeraCodeSolution.DoVeraCode((decimal.Parse(rowItem["DuplicateDollarTransactionPercent"].ToString())).ToString("#,#0") + "%") : string.Empty);
        dupPercent.Text = GeneralFuncsLib.FormatBorderText(dupPercent.Text, dupPercentColor);

        //duplicate bin
        var dupBin = dataItem["DuplicateBin"];
        Color dupBinColor = rowItem["SixDupBinColor"].ToString().ToColor();
        dupBin.ToolTip = VeraCodeSolution.DoVeraCode(string.Format("{0:C}", rowItem["DuplicateBin6TransactionAmount"]).ToCurrencySymbol());
        dupBin.Text = GeneralFuncsLib.FormatBorderText(dupBin.Text, dupBinColor);


        //# negative
        var negative = dataItem["NegativeBatchCount"];
        Color negColor = rowItem["NegColor"].ToString().ToColor();
        negative.Text = GeneralFuncsLib.FormatBorderText(negative.Text, negColor);

        //# zero
        var zero = dataItem["ZeroBatchCount"];
        Color zeroColor = rowItem["ZeroColor"].ToString().ToColor();
        zero.Text = GeneralFuncsLib.FormatBorderText(zero.Text, zeroColor);

        //today volume
        var todayVolumeCtrl = dataItem["TodayVolume"];
        Color todayColor = rowItem["VolumeColor"].ToString().ToColor();
        todayVolumeCtrl.Text = GeneralFuncsLib.FormatBorderText(todayVolumeCtrl.Text, todayColor);

        //RTVL
        var rtvl = dataItem["TodayFirstTimeRetrievalVolume"];
        Color rtvlColor = rowItem["RTVLColor"].ToString().ToColor();
        rtvl.ToolTip = VeraCodeSolution.DoVeraCode(rowItem["TodayFirstTimeRetrievalCount"].ToString().ToCurrencySymbol());
        rtvl.Text = GeneralFuncsLib.FormatBorderText(rtvl.Text, rtvlColor);

        //CB
        var cb = dataItem["TodayChargebackVolume"];
        Color cbColor = rowItem["CBColor"].ToString().ToColor();
        cb.ToolTip = VeraCodeSolution.DoVeraCode(rowItem["TodayChargebackCount"].ToString().ToCurrencySymbol());
        cb.Text = GeneralFuncsLib.FormatBorderText(cb.Text, cbColor);

        //Rtn Percent
        var rtnPercent = dataItem["ReturnPercent"];
        Color rtnPercentColor = rowItem["RtnColor"].ToString().ToColor();
        rtnPercent.ToolTip = VeraCodeSolution.DoVeraCode(GeneralFuncsLib.FormatCurrencyTooltip(rowItem["TodayReturnAmount"], SessionManager.CurrencyFortmat).ToCurrencySymbol());
        rtnPercent.Text = VeraCodeSolution.DoVeraCode(rowItem["ReturnPercent"].ToString().Trim() != string.Empty ?
            VeraCodeSolution.DoVeraCode((decimal.Parse(rowItem["ReturnPercent"].ToString())).ToString("#,#0") + "%") : string.Empty);
        rtnPercent.Text = GeneralFuncsLib.FormatBorderText(rtnPercent.Text, rtnPercentColor);

        //Max ticket $
        var maxTkt = dataItem["TodayHighestTransactionAmount"];
        Color maxTktColor = rowItem["MaxTktColor"].ToString().ToColor();
        maxTkt.Text = GeneralFuncsLib.FormatBorderText(maxTkt.Text, maxTktColor, true);

        //# Tkts
        var tkt = dataItem["TodayTransactionCount"];
        Color tktColor = rowItem["TktsColor"].ToString().ToColor();
        tkt.Text = GeneralFuncsLib.FormatBorderText(tkt.Text, tktColor, true);

        //# batch
        var batch = dataItem["TodayBatchCount"];
        Color batchColor = rowItem["BatchColor"].ToString().ToColor();
        batch.Text = GeneralFuncsLib.FormatBorderText(batch.Text, batchColor, true);

        //SIC
        var sic = dataItem["SICCode"];
        sic.ToolTip = VeraCodeSolution.DoVeraCode(rowItem["SICDescription"].ToString().Replace("&nbsp;", string.Empty));
        //*/
        //SC
        var sc = dataItem["SingleCardTransToday"];
        Color scColor = rowItem["DupBinColor"].ToString().ToColor();
        sc.ToolTip = VeraCodeSolution.DoVeraCode(string.Format("{0:C}", rowItem["VolumeOfSimilarCardTransaction"]).ToCurrencySymbol());
        sc.Text = GeneralFuncsLib.FormatBorderText(sc.Text, scColor, true);
        //PV

        var pv = dataItem["RulesViolated"];
        pv.ToolTip = VeraCodeSolution.DoVeraCode(rowItem["RulesViolated"].ToString().ToCurrencySymbol());
    }

    private void uxReportGrid_ItemDataBound_CardView(GridDataItem dataItem, DataRow rowItem, int dataIndex)
    {
        var merchantNumber = rowItem["MerchantNumber"];
        var cardViewColumnCtr = dataItem["CardView"];


        //46652 - AW - Multi-Currency Transaction Display
        var lbtSort_TodayHighestTransactionAmount = cardViewColumnCtr.FindControl("lbtSort_TodayHighestTransactionAmount") as LinkButton;
        lbtSort_TodayHighestTransactionAmount.Text = lbtSort_TodayHighestTransactionAmount.Text.ToCurrencySymbol();
        var lbtSort_TodayFirstTimeRetrievalVolume = cardViewColumnCtr.FindControl("lbtSort_TodayFirstTimeRetrievalVolume") as LinkButton;
        lbtSort_TodayFirstTimeRetrievalVolume.Text = lbtSort_TodayFirstTimeRetrievalVolume.Text.ToCurrencySymbol();

        //process requeued merchant
        var colRQColumn = cardViewColumnCtr.FindControl("colRQColumn") as HtmlGenericControl;
        if (colRQColumn != null)
        {
            if (HasRQColumn)
            {
                colRQColumn.Visible = true;
                ResizedMerchantNameColumn(cardViewColumnCtr, true);

                HtmlInputCheckBox chkRequeueSingleMerchant = colRQColumn.FindControl("chkItemCV") as HtmlInputCheckBox;
                if (chkRequeueSingleMerchant != null)
                {
                    //this merchant was requeued before, gray out the column
                    if (bool.Parse(rowItem["IsRequeued"].ToString()))
                    {
                        if (colRQColumn.FindControl("bgChkItemCV") != null)
                        {
                            ((HtmlGenericControl)colRQColumn.FindControl("bgChkItemCV")).Attributes["class"] += "isRequeue";
                        }
                    }

                    //process check state from spa
                    chkRequeueSingleMerchant.Disabled = !GeneralFuncsLib.ReadOnlyRiskManagementEnableStatus((SecurePage)Page);
                    if (RiskSessionManager.DetectionQueueTemporaryRequeueInfo.RequeuedMerchantList != null)
                    {
                        chkRequeueSingleMerchant.Checked = RiskSessionManager.DetectionQueueTemporaryRequeueInfo.RequeuedMerchantList.Contains(merchantNumber.ToString());
                    }
                    if (RiskSessionManager.DetectionQueueTemporaryRequeueInfo.IsRequeueAll)
                    {
                        chkRequeueSingleMerchant.Checked = true;
                        chkRequeueSingleMerchant.Disabled = true;
                    }

                    chkRequeueSingleMerchant.Value = merchantNumber.ToString();
                    chkRequeueSingleMerchant.Attributes.Add("onclick", string.Format("RequeueSingleMerchant(this," + IsWQ.ToString().ToLower() + ")"));
                    chkRequeueSingleMerchant.Visible = true;
                }
            }
            else
            {
                colRQColumn.Visible = false;
                ResizedMerchantNameColumn(cardViewColumnCtr, false);
            }
        }

        Control ctrlCard = cardViewColumnCtr.FindControl("cBoxCV");
        if (ctrlCard != null)
        {
            HtmlInputCheckBox chkBox = ctrlCard as HtmlInputCheckBox;
            chkBox.Checked = rowItem["Worked"].ToString().Trim() == "1";
            chkBox.Value = rowItem["MerchantNumber"].ToString();
            chkBox.Attributes.Add("onclick", string.Format("ChangeMerchantWorked(this,{0})", rowItem["TodayVolume"]));
            chkBox.Disabled = !GeneralFuncsLib.ReadOnlyRiskManagementEnableStatus((SecurePage)Page);
        }

        //merchant name
        var merchantName = cardViewColumnCtr.FindControl("merchantName") as HtmlGenericControl;

        string url = String.Empty;
        string urlText = rowItem["MerchantName"].ToString();

        var merchantNameColor = Color.Transparent.Name;
        if (rowItem["Worked"].ToString() == "1")
        {
            cardViewColumnCtr.CssClass += "row-checked";
            merchantNameColor = Color.LightPink.Name;
        }

        if (GeneralFuncsLib.HasRiskRptPermission((SecurePage)Page))
        {
            url = RiskGeneral.BuildMerchantHyperlinkInRisk((SecurePage)Page,
                rowItem[MERCHANT_NUMBER], true, MerchantIntruderQuery, urlText);
            url = string.Format("<a class=\"link\" href=\"#\" style=\"cursor:pointer; border-bottom: 2px {2} solid;\" onclick=\"MerchantNumberClick('{0}','{1}',this); return false;\">", rowItem["MerchantNumber"], rowItem["TodayVolume"], merchantNameColor) + urlText + "</a>";
        }
        AddHtml(merchantName, VeraCodeSolution.DoVeraCode(url));

        if (rowItem["Watch"].Equals(1))
        {
            AddHtml(merchantName, VeraCodeSolution.DoVeraCode(merchantName.InnerHtml + "(<span style=\"color:red\">*</span>)"));
            AddTooltip(merchantName, VeraCodeSolution.DoVeraCode(
                "(M)" + rowItem["MerchantNumber"].ToString().Trim() +
                (rowItem["ApprovalDate"] == DBNull.Value ? "" : " - " + ((DateTime)rowItem["ApprovalDate"]).ToString("MM/dd/yyyy")) +
                (rowItem["MDDescription"] == DBNull.Value ? "" : " - " + rowItem["MDDescription"])
                ));

        }
        else if (rowItem["MultiWatch"].Equals(true))
        {
            AddHtml(merchantName, VeraCodeSolution.DoVeraCode(merchantName.InnerHtml + "(<span style=\"color:red\">*</span>)"));
            AddTooltip(merchantName, VeraCodeSolution.DoVeraCode(
                 rowItem["MerchantNumber"].ToString().Trim() +
                (rowItem["ApprovalDate"] == DBNull.Value ? "" : " - " + ((DateTime)rowItem["ApprovalDate"]).ToString("MM/dd/yyyy")) +
                (rowItem["MDDescription"] == DBNull.Value ? "" : " - " + rowItem["MDDescription"])
                ));
        }
        else
        {
            AddTooltip(merchantName, VeraCodeSolution.DoVeraCode(
                 rowItem["MerchantNumber"].ToString().Trim() +
                (rowItem["ApprovalDate"] == DBNull.Value ? "" : " - " + ((DateTime)rowItem["ApprovalDate"]).ToString("MM/dd/yyyy")) +
                (rowItem["MDDescription"] == DBNull.Value ? "" : " - " + rowItem["MDDescription"])
                ));
        }

        string assignmentTypeQueryString = RiskSessionManager.DetectionQueue.AssignmentType != WebSiteEnums.AssignmentType.DetectionQueue ? "&AssignmentType=" + (int)RiskSessionManager.DetectionQueue.AssignmentType : "";
        string queryStringImage = this.Page.BuildSecureQueryString("MerchantNumber=" + rowItem[MERCHANT_NUMBER] + "&ReportDate=" + RiskSessionManager.DetectionQueue.ReportDate + "&AssignmentID=" + RiskSessionManager.DetectionQueue.AssignmentID + assignmentTypeQueryString);
        string urlImage = "rm_DQReasonModal.aspx?" + queryStringImage;
        urlImage = "<a class=\"image-link\" href=\"#\" style=\"cursor:pointer\" onclick=\"OpenInstanceWindow('" + urlImage + "','DQWindow'); return false;\"><img src='" + ResolveUrl("~/") + "res/img/information.png' border='0' alt='" + GetLocalResourceObject("DetectionQueueRainbowReport_ascx_PV").ToString() + "' /></a>";
        AddHtml(merchantName, VeraCodeSolution.DoVeraCode(merchantName.InnerHtml + " " + urlImage));


        //riskscore
        var riskScore = cardViewColumnCtr.FindControl("riskScore") as HtmlGenericControl;
        Color riskScoreColor = rowItem["RiskScoreColor"].ToString().ToColor();
        if (!GeneralFuncsLib.NvlString(rowItem["RiskScore"]).Equals("0") && rowItem["RiskScore"] != DBNull.Value
            && !rowItem["RiskScore"].ToString().IsNullOrEmpty())
        {
            string queryString = this.Page.BuildSecureQueryString("MerchantNumber=" + rowItem[MERCHANT_NUMBER] + "&ReportDate=" + RiskSessionManager.DetectionQueue.ReportDate);
            string urlRiskScoreDetail = "rm_RiskScoreDetailModal.aspx?" + queryString;
            url = string.Format("<a class=\"link\" href=\"#\" style=\"cursor:pointer;\" onclick=\"ShowPopupModal('{0}','auto'); return false;\">",
                urlRiskScoreDetail);
            AddHtml(riskScore, VeraCodeSolution.GetOutputHtmlString(url + (Convert.ToDecimal(rowItem["RiskScore"]).ToString("#,##0")) + "</a>"));
        }
        else
        {
            AddText(riskScore, VeraCodeSolution.DoVeraCode(rowItem["RiskScore"].ToString().Trim() != string.Empty ?
                rowItem["RiskScore"].ToString() : "0"));
        }
        riskScore.InnerHtml = GeneralFuncsLib.FormatBorderText(riskScore.InnerHtml, riskScoreColor);

        //volume percent
        var volumePercent = cardViewColumnCtr.FindControl("volumePercent") as HtmlGenericControl;
        Color volPercentColor = rowItem["VolumeColor"].ToString().ToColor();
        AddTooltip(volumePercent, VeraCodeSolution.DoVeraCode(string.Format("{0:C}", rowItem["TodayVolume"]).ToCurrencySymbol()));
        string rptDate = RiskSessionManager.DetectionQueue.ReportDate.ToShortDateString();
        if (!GeneralFuncsLib.NvlString(rowItem["VolumePercent"]).Equals("0") && GeneralFuncsLib.NvlString(rowItem["VolumePercent"]).Length > 0)
        {
            string queryString1 = this.Page.BuildSecureQueryString(string.Format("MerchantNumber={0}&ReportDate={1}",
            rowItem["MerchantNumber"], rowItem["ReportDate"]));
            string url1 = string.Format("<a class=\"link\" href=\"#\" style=\"cursor:pointer;\" onclick=\"OpenInstanceWindow('rm_TransactionDetailsModal.aspx?{0}','DQWindow'); return false;\">", queryString1);
            AddHtml(volumePercent, VeraCodeSolution.DoVeraCode(url1 + (decimal.Parse(rowItem["VolumePercent"].ToString())).ToString("#,#0") + "%") + "</a>");
        }
        else
        {
            AddText(volumePercent, VeraCodeSolution.DoVeraCode(rowItem["VolumePercent"].ToString().Trim() != string.Empty ?
            VeraCodeSolution.DoVeraCode((decimal.Parse(rowItem["VolumePercent"].ToString())).ToString("#,#0") + "%") : string.Empty));
        }
        volumePercent.InnerHtml = GeneralFuncsLib.FormatBorderText(volumePercent.InnerHtml, volPercentColor);

        //

        //40965 Contractual Volume
        var contractualVolume = cardViewColumnCtr.FindControl("contractualVolume") as HtmlGenericControl;
        Color contractualVolumeColor = rowItem["CVColor"].ToString().ToColor();
        AddTooltip(contractualVolume, VeraCodeSolution.DoVeraCode(string.Format("{0:C}", rowItem["ContractualDailyVolume"]).ToCurrencySymbol()));
        AddText(contractualVolume, VeraCodeSolution.DoVeraCode(rowItem["ContractualVolume"].ToString() != string.Empty ?
                        string.Format("{0:N}%", (decimal.Parse(rowItem["ContractualVolume"].ToString()))) : WebSiteConstants.HTML_EM_DASH_ENCODE));

        contractualVolume.InnerHtml = GeneralFuncsLib.FormatBorderText(contractualVolume.InnerHtml, contractualVolumeColor);

        //auth percent
        var authPercent = cardViewColumnCtr.FindControl("authPercent") as HtmlGenericControl;
        Color authPercentColor = rowItem["AuthColor"].ToString().ToColor();
        AddTooltip(authPercent, VeraCodeSolution.DoVeraCode(string.Format("{0:C} ({1})", rowItem["TodayAuthorizationVolume"], rowItem["TodayAuthorizationCount"]).ToCurrencySymbol()));
        if (!GeneralFuncsLib.NvlString(rowItem["AuthorizationPercent"]).Equals("0") && GeneralFuncsLib.NvlString(rowItem["AuthorizationPercent"]).Length > 0)
        {
            string queryString1 = this.Page.BuildSecureQueryString(string.Format("MerchantNumber={0}&ReportDate={1}",
            rowItem["MerchantNumber"], rowItem["ReportDate"]));
            string url1 = string.Format("<a class=\"link\" href=\"#\" style=\"cursor:pointer;\" onclick=\"OpenInstanceWindow('NewRiskReport_TransactionHistoryModal.aspx?{0}','DQWindow'); return false;\">", queryString1, authPercentColor.Name.ToString());
            decimal authPerc = 0;
            decimal.TryParse(GeneralFuncsLib.NvlString(rowItem["AuthorizationPercent"]), out authPerc);
            AddHtml(authPercent, VeraCodeSolution.DoVeraCode(url1 + authPerc.ToString("#,#0") + "%" + "</a>"));
        }
        else
        {
            AddText(authPercent, VeraCodeSolution.DoVeraCode(rowItem["AuthorizationPercent"].ToString().Trim() != string.Empty ?
                (decimal.Parse(rowItem["AuthorizationPercent"].ToString()).ToString("#,#0") + "%") : string.Empty));
        }
        authPercent.InnerHtml = GeneralFuncsLib.FormatBorderText(authPercent.InnerHtml, authPercentColor);

        //profile
        var profileDes = cardViewColumnCtr.FindControl("profileDes") as HtmlGenericControl;
        AddTooltip(profileDes, VeraCodeSolution.DoVeraCode(rowItem["ProfileDescription"].ToSafeString()));

        //average ticket
        var avgTicket = cardViewColumnCtr.FindControl("avgTicket") as HtmlGenericControl;
        Color avgTicketColor = rowItem["AvgTktColor"].ToString().ToColor();
        AddTooltip(avgTicket, VeraCodeSolution.DoVeraCode(string.Format("{0:C}", rowItem["AverageTicketVolume"]).ToCurrencySymbol()));
        AddText(avgTicket, VeraCodeSolution.DoVeraCode(rowItem["AverageTicketPercent"].ToString() != string.Empty ? (decimal.Parse(rowItem["AverageTicketPercent"].ToString()).ToString("#,#0") + "%") : WebSiteConstants.HTML_EM_DASH_ENCODE));
        avgTicket.InnerHtml = GeneralFuncsLib.FormatBorderText(avgTicket.InnerHtml, avgTicketColor);

        //decline percent
        var decPercent = cardViewColumnCtr.FindControl("decPercent") as HtmlGenericControl;
        Color decPercentColor = rowItem["DeclAuthPctColor"].ToString().ToColor();
        AddTooltip(decPercent, VeraCodeSolution.DoVeraCode(string.Format("{0:C}", rowItem["TodayDeclinedAuthorizationVolume"]).ToCurrencySymbol()));
        AddText(decPercent, VeraCodeSolution.DoVeraCode(rowItem["DeclinedAuthorizationPercent"].ToString().Trim() != string.Empty ?
            VeraCodeSolution.DoVeraCode((decimal.Parse(rowItem["DeclinedAuthorizationPercent"].ToString())).ToString("#,#0") + "%") : string.Empty));
        decPercent.InnerHtml = GeneralFuncsLib.FormatBorderText(decPercent.InnerHtml, decPercentColor);

        //# repeat auth
        var rptAuth = cardViewColumnCtr.FindControl("rptAuth") as HtmlGenericControl;
        Color rptAuthColor = rowItem["RptAuthColor"].ToString().ToColor();
        rptAuth.InnerHtml = GeneralFuncsLib.FormatBorderText(rptAuth.InnerHtml, rptAuthColor, true);

        //FC (foreign card)
        var fCardCout = cardViewColumnCtr.FindControl("fCardCout") as HtmlGenericControl;
        Color fCardCoutColor = rowItem["FCColor"].ToString().ToColor();
        AddTooltip(fCardCout, VeraCodeSolution.DoVeraCode(string.Format("{0:C}", rowItem["TodayForeignCardVolume"]).ToCurrencySymbol()));
        fCardCout.InnerHtml = GeneralFuncsLib.FormatBorderText(fCardCout.InnerHtml, fCardCoutColor, true);

        //key percent
        var keyPercent = cardViewColumnCtr.FindControl("keypercent") as HtmlGenericControl;
        Color keyPercentColor = rowItem["KeyColor"].ToString().ToColor();
        AddTooltip(keyPercent, VeraCodeSolution.DoVeraCode(string.Format("{0} ({1})", GeneralFuncsLib.FormatCurrencyTooltip(rowItem["TodayKeyVolume"], SessionManager.CurrencyFortmat), rowItem["TodayKeyCount"]).ToCurrencySymbol()));
        AddText(keyPercent, VeraCodeSolution.DoVeraCode(rowItem["KeyPercent"].ToString().Trim() != string.Empty ? VeraCodeSolution.DoVeraCode((decimal.Parse(rowItem["KeyPercent"].ToString())).ToString("#,#0") + "%") : string.Empty));
        keyPercent.InnerHtml = GeneralFuncsLib.FormatBorderText(keyPercent.InnerHtml, keyPercentColor, true);

        //even percent
        var evenPercent = cardViewColumnCtr.FindControl("evenPercent") as HtmlGenericControl;
        Color evenPercentColor = rowItem["EvenColor"].ToString().ToColor();
        AddText(evenPercent, VeraCodeSolution.DoVeraCode(rowItem["EvenDollarTransactionPercent"].ToString() != string.Empty ?
            VeraCodeSolution.DoVeraCode((decimal.Parse(rowItem["EvenDollarTransactionPercent"].ToString())).ToString("#,#0") + "%") : string.Empty));
        evenPercent.InnerHtml = GeneralFuncsLib.FormatBorderText(evenPercent.InnerHtml, evenPercentColor, true);

        //duplicate percent
        var dupPercent = cardViewColumnCtr.FindControl("dupPercent") as HtmlGenericControl;
        Color dupPercentColor = rowItem["DupColor"].ToString().ToColor();
        AddTooltip(dupPercent, VeraCodeSolution.DoVeraCode(rowItem["DuplicateDollarTransactionCount"].ToString().ToCurrencySymbol()));
        AddText(dupPercent, VeraCodeSolution.DoVeraCode(rowItem["DuplicateDollarTransactionPercent"].ToString() != string.Empty ?
            VeraCodeSolution.DoVeraCode((decimal.Parse(rowItem["DuplicateDollarTransactionPercent"].ToString())).ToString("#,#0") + "%") : string.Empty));
        dupPercent.InnerHtml = GeneralFuncsLib.FormatBorderText(dupPercent.InnerHtml, dupPercentColor);

        //duplicate bin
        var dupBin = cardViewColumnCtr.FindControl("dupBin") as HtmlGenericControl;
        Color dupBinColor = rowItem["SixDupBinColor"].ToString().ToColor();
        //AddTooltip(dupBin, VeraCodeSolution.DoVeraCode(SessionManager.CurrencySymbol + Convert.ToDecimal(rowItem["DuplicateBin6TransactionAmount"]).ToString("#,##0.#0")));
        AddTooltip(dupBin, VeraCodeSolution.DoVeraCode(GeneralFuncsLib.FormatCurrencyTooltip(rowItem["DuplicateBin6TransactionAmount"], SessionManager.CurrencyFortmat)));
        AddText(dupBin, VeraCodeSolution.DoVeraCode(Convert.ToDecimal(rowItem["DuplicateBin"]).ToString("#,##0")));
        dupBin.InnerHtml = GeneralFuncsLib.FormatBorderText(dupBin.InnerHtml, dupBinColor);

        //# negative
        var negativeBC = cardViewColumnCtr.FindControl("negativeBC") as HtmlGenericControl;
        Color negColor = rowItem["NegColor"].ToString().ToColor();
        negativeBC.InnerHtml = GeneralFuncsLib.FormatBorderText(negativeBC.InnerHtml, negColor);

        //# zero
        var zeroBC = cardViewColumnCtr.FindControl("zeroBC") as HtmlGenericControl;
        Color zeroColor = rowItem["ZeroColor"].ToString().ToColor();
        zeroBC.InnerHtml = GeneralFuncsLib.FormatBorderText(zeroBC.InnerHtml, zeroColor);

        //today volume
        var todayVolumeCtrl = cardViewColumnCtr.FindControl("todayVolume") as HtmlGenericControl;
        Color todayVolumeColor = rowItem["VolumeColor"].ToString().ToColor();
        AddText(todayVolumeCtrl, VeraCodeSolution.DoVeraCode(GeneralFuncsLib.FormatCurrency(rowItem["TodayVolume"].ToString())));
        todayVolumeCtrl.InnerHtml = GeneralFuncsLib.FormatBorderText(todayVolumeCtrl.InnerHtml, todayVolumeColor);

        ////RTVL
        var todayRV = cardViewColumnCtr.FindControl("todayRV") as HtmlGenericControl;
        Color rtvlColor = rowItem["RTVLColor"].ToString().ToColor();
        AddTooltip(todayRV, VeraCodeSolution.DoVeraCode(rowItem["TodayFirstTimeRetrievalCount"].ToString().ToCurrencySymbol()));
        AddText(todayRV, VeraCodeSolution.DoVeraCode(rowItem["TodayFirstTimeRetrievalVolume"].ToString() != string.Empty ?
            VeraCodeSolution.DoVeraCode(GeneralFuncsLib.FormatCurrency(rowItem["TodayFirstTimeRetrievalVolume"].ToString(), "C4")) : string.Empty));
        todayRV.InnerHtml = GeneralFuncsLib.FormatBorderText(todayRV.InnerHtml, rtvlColor);

        ////CB
        var todayCB = cardViewColumnCtr.FindControl("todayCB") as HtmlGenericControl;
        Color cbColor = rowItem["CBColor"].ToString().ToColor();
        AddTooltip(todayCB, VeraCodeSolution.DoVeraCode(rowItem["TodayChargebackCount"].ToString().ToCurrencySymbol()));
        AddText(todayCB, VeraCodeSolution.DoVeraCode(rowItem["TodayChargebackVolume"].ToString() != string.Empty ?
            VeraCodeSolution.DoVeraCode(GeneralFuncsLib.FormatCurrency(rowItem["TodayChargebackVolume"].ToString(), "C4")) : string.Empty));
        todayCB.InnerHtml = GeneralFuncsLib.FormatBorderText(todayCB.InnerHtml, cbColor);

        ////Rtn Percent
        var rtnPercent = cardViewColumnCtr.FindControl("rtnPercent") as HtmlGenericControl;
        Color rtnPercentColor = rowItem["RtnColor"].ToString().ToColor();
        AddTooltip(rtnPercent, VeraCodeSolution.DoVeraCode(GeneralFuncsLib.FormatCurrencyTooltip(rowItem["TodayReturnAmount"], SessionManager.CurrencyFortmat)).ToCurrencySymbol());
        AddText(rtnPercent, VeraCodeSolution.DoVeraCode(rowItem["ReturnPercent"].ToString() != string.Empty ?
            VeraCodeSolution.DoVeraCode((decimal.Parse(rowItem["ReturnPercent"].ToString())).ToString("#,#0") + "%") : string.Empty));
        rtnPercent.InnerHtml = GeneralFuncsLib.FormatBorderText(rtnPercent.InnerHtml, rtnPercentColor);

        ////Max ticket $
        var maxTkt = cardViewColumnCtr.FindControl("maxTkt") as HtmlGenericControl;
        Color maxTktColor = rowItem["MaxTktColor"].ToString().ToColor();
        AddText(maxTkt, VeraCodeSolution.DoVeraCode(GeneralFuncsLib.FormatCurrency(rowItem["TodayHighestTransactionAmount"].ToString())));
        maxTkt.InnerHtml = GeneralFuncsLib.FormatBorderText(maxTkt.InnerHtml, maxTktColor);

        ////# Tkts
        var todayTC = cardViewColumnCtr.FindControl("todayTC") as HtmlGenericControl;
        Color tktColor = rowItem["TktsColor"].ToString().ToColor();
        AddText(todayTC, VeraCodeSolution.DoVeraCode(Convert.ToDecimal(rowItem["TodayTransactionCount"]).ToString("#,##0")));
        todayTC.InnerHtml = GeneralFuncsLib.FormatBorderText(todayTC.InnerHtml, tktColor, true);

        ////# batch
        var todayBC = cardViewColumnCtr.FindControl("todayBC") as HtmlGenericControl;
        Color batchColor = rowItem["BatchColor"].ToString().ToColor();
        todayBC.InnerHtml = GeneralFuncsLib.FormatBorderText(todayBC.InnerHtml, batchColor, true);

        //SIC
        var sicCode = cardViewColumnCtr.FindControl("sicCode") as HtmlGenericControl;
        AddTooltip(sicCode, VeraCodeSolution.DoVeraCode(rowItem["SICDescription"].ToString().Replace("&nbsp;", string.Empty)).ToCurrencySymbol());

        ////SC
        var scTransToday = cardViewColumnCtr.FindControl("scTransToday") as HtmlGenericControl;
        Color scColor = rowItem["DupBinColor"].ToString().ToColor();
        AddTooltip(scTransToday, VeraCodeSolution.DoVeraCode(string.Format("{0:C}", rowItem["VolumeOfSimilarCardTransaction"]).ToCurrencySymbol()));
        scTransToday.InnerHtml = GeneralFuncsLib.FormatBorderText(scTransToday.InnerHtml, scColor);

        //just for first row
        if (dataIndex == 0 && _isSorting)
        {
            AddSortIcon(cardViewColumnCtr);
            _isSorting = false;
        }
    }

    /// <summary>
    /// Adjust ccs class when show/hide RQ column
    /// </summary>
    /// <param name="ctrl"></param>
    /// <param name="isRQVisible"></param>
    private void ResizedMerchantNameColumn(TableCell ctrl, bool isRQVisible = false)
    {
        var col = ctrl.FindControl("colMerchantName") as HtmlGenericControl;

        if (col == null) return;

        if (isRQVisible)
        {
            RemoveClass(col, "col-xs-10");
            AddClass(col, "col-xs-8");
        }
        else
        {
            RemoveClass(col, "col-xs-8");
            AddClass(col, "col-xs-10");
        }
    }

    private void RequeueAllGridView(GridHeaderItem headerItem)
    {
        HtmlInputCheckBox chkRequeueAllMerchant = headerItem["RQCheckbox"].FindControl("chkHeader") as HtmlInputCheckBox;
        if (chkRequeueAllMerchant != null)
        {
            //have records

            if (uxReportGrid.MasterTableView.VirtualItemCount > 0)
            {
                chkRequeueAllMerchant.Disabled = false;
                //process check state from spa
                chkRequeueAllMerchant.Attributes.Add("onclick", string.Format("RequeueAllMerchants(this, " + IsWQ.ToString().ToLower() + ")"));
                if ((RiskSessionManager.DetectionQueueTemporaryRequeueInfo.IsRequeueAll
                    && RiskSessionManager.DetectionQueueTemporaryRequeueInfo.RequeuedMerchantList.IsEmpty())
                    || (RiskSessionManager.DetectionQueueTemporaryRequeueInfo.TotalAlertMerchant == RiskSessionManager.DetectionQueueTemporaryRequeueInfo.RequeuedMerchantList.Count()
                    && RiskSessionManager.DetectionQueueTemporaryRequeueInfo.TotalAlertMerchant > 0)
                    )
                {
                    chkRequeueAllMerchant.Checked = true;
                }
                else
                {
                    chkRequeueAllMerchant.Checked = false;
                }
            }
            chkRequeueAllMerchant.Disabled = !GeneralFuncsLib.ReadOnlyRiskManagementEnableStatus((SecurePage)Page);
        }
    }

    private void RequeueAllCardView()
    {
        if (uxReportGrid.MasterTableView.VirtualItemCount > 0)
        {
            //process check state from spa
            chkHeaderCV.Attributes.Add("onclick", string.Format("RequeueAllMerchants(this," + IsWQ.ToString().ToLower() + ")"));
            if ((RiskSessionManager.DetectionQueueTemporaryRequeueInfo.IsRequeueAll
                && RiskSessionManager.DetectionQueueTemporaryRequeueInfo.RequeuedMerchantList.IsEmpty())
                || (RiskSessionManager.DetectionQueueTemporaryRequeueInfo.TotalAlertMerchant == RiskSessionManager.DetectionQueueTemporaryRequeueInfo.RequeuedMerchantList.Count()
                && RiskSessionManager.DetectionQueueTemporaryRequeueInfo.TotalAlertMerchant > 0)
                )
            {
                chkHeaderCV.Checked = true;
            }
            else
            {
                chkHeaderCV.Checked = false;
            }
        }
    }

    private void AddClass(HtmlGenericControl ctrl, string className)
    {
        var current = ctrl.Attributes["class"];
        if (current == null)
        {
            ctrl.Attributes["class"] = className;
        }
        else
        {
            if (current.Contains(className))
            {
                current = current.Replace(className, "").Trim();
            }
            ctrl.Attributes["class"] = (current + " " + className).Trim();
        }
    }

    private void RemoveClass(HtmlGenericControl ctrl, string className)
    {
        var current = ctrl.Attributes["class"];
        if (current != null && current.Contains(className))
        {
            current = current.Replace(className, "").Trim();
        }
        ctrl.Attributes["class"] = current;
    }

    private void AddColor(HtmlGenericControl ctrl, string color)
    {
        ctrl.Style.Add("color", color);
    }

    private void AddBackColor(HtmlGenericControl ctrl, string color)
    {
        ctrl.Style.Add("background-color", color);
    }

    private void AddTooltip(HtmlGenericControl ctrl, string tooltip)
    {
        ctrl.Attributes["title"] = tooltip;
    }

    private void AddText(HtmlGenericControl ctrl, string text)
    {
        ctrl.InnerText = text;
    }

    private void AddHtml(HtmlGenericControl ctrl, string html)
    {
        ctrl.InnerHtml = html;
    }

    private void AddSortIcon(TableCell ctrl)
    {
        if (RiskSessionManager.DetectionQueue == null || RiskSessionManager.DetectionQueue.SortExpression == null) return;

        var sortingField = RiskSessionManager.DetectionQueue.SortExpression;
        var sortingOrder = RiskSessionManager.DetectionQueue.SortOrder;

        var ctrlId = "lbtSort_" + sortingField;
        var tempBtn = ctrl.FindControl(ctrlId);
        if (tempBtn != null)
        {
            var linkButtonCtrl = tempBtn as LinkButton;
            switch (RiskSessionManager.DetectionQueue.SortOrder)
            {
                case DESCEND:
                    linkButtonCtrl.CssClass += " rgSortQueueDesc";
                    break;
                case ASCEND:
                    linkButtonCtrl.CssClass += " rgSortQueueDesc";
                    break;
                default:
                    linkButtonCtrl.CssClass.Replace("rgSortQueueDesc", "")
                                           .Replace("rgSortQueueDesc", "");
                    break;
            }
        }
    }

    public void SetDisplayColumns()
    {
        var viewData = PersonalDataHelper.GetJSONConfig<List<ColumnDisplayedConfigurationItem>>(UserConfigNames.CONFIG_RISK_DETECTIONQUEUE_CUSTOMIZE_COLUMNS);

        if (viewData != null)
        {
            List<string> listDisplayedColumns = viewData.Select(c => c.ColumnName).ToList();
            foreach (GridColumn col in uxReportGrid.MasterTableView.Columns)
            {
                if (col.UniqueName == WebSiteConstants.GRID_COLUMN_TEMP)
                {
                    col.Display = true;
                    col.OrderIndex = 0;
                }
                else if (UNCHANGE_COLUMNS.Any(c => c.Equals(col.UniqueName)))
                {
                    col.Display = true;
                    col.OrderIndex = Array.IndexOf(UNCHANGE_COLUMNS, col.UniqueName);
                }
                else
                {
                    if (listDisplayedColumns.Any(c => c.Equals(col.UniqueName)))
                    {
                        ColumnDisplayedConfigurationItem item = viewData.Where(s => s.ColumnName.Equals(col.UniqueName)).FirstOrDefault();
                        if (item != null)
                        {
                            col.OrderIndex = item.OrderIndex;
                        }
                        col.Display = true;
                    }
                    else
                    {
                        col.Display = false;
                    }
                }
            }
        }

        //46652 - AW - Multi-Currency Transaction Display
        uxReportGrid.Columns.FindByUniqueName("TodayFirstTimeRetrievalVolume").HeaderText =
            uxReportGrid.Columns.FindByUniqueName("TodayFirstTimeRetrievalVolume").HeaderText.ToCurrencySymbol();
        uxReportGrid.Columns.FindByUniqueName("TodayHighestTransactionAmount").HeaderText =
            uxReportGrid.Columns.FindByUniqueName("TodayHighestTransactionAmount").HeaderText.ToCurrencySymbol();
    }

    private void GenerateAllColumnsReportGrid()
    {
        var listColumns = new List<ColumnDisplayedConfigurationItem>();
        var colGrid = uxReportGrid.MasterTableView.Columns;
        foreach (GridColumn col in colGrid)
        {
            if (!UNCHANGE_COLUMNS.Any(c => c.Equals(col.UniqueName)) && col.UniqueName != WebSiteConstants.GRID_COLUMN_TEMP)
            {
                listColumns.Add(new ColumnDisplayedConfigurationItem
                {
                    ColumnName = col.UniqueName,
                    ColumnText = GetLocalResourceObject(col.UniqueName + ".HeaderText").ToString().Replace("$", SessionManager.CurrencySymbol),
                    IsDisplayed = false,
                    OrderIndex = col.OrderIndex
                });
            }
        }
        RiskSessionManager.RiskReportDetectionQueueColumns = listColumns;

    }
    protected void btnRebind_Click(object sender, EventArgs e)
    {
        SetDisplayColumns();
        uxReportGrid.Rebind();
    }
    protected void uxReportGrid_PreRender(object sender, EventArgs e)
    {
        SetDisplayColumns();
    }

}
