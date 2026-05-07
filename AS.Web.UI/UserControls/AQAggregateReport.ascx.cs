using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
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
using System.Linq;
using AS.Common.Formater;

public partial class UserControls_AQAggregateReport : GlobalUserControl
{
    enum DataBindAction
    {
        BindAQAggregateGrid
    }
    #region Constants

    private const string SPA_GET_AQ_AGGREGATE_AS = "spa_RM_AutoQueue_Get_BarometerReport";

    // Grid columns name: because we use the datafield name for both properties:
    // DataField & UniqueName, use the same constant. If they're different, please
    // add new constant for it.
    private const string MERCHANT_NR_COL = "MerchantNumber";
    private const string MERCHANT_NAME_COL = "MerchantName";
    private const string WORKED_COL = "Worked";
    private const string WATCH_COL = "Watch";
    private const string MULTI_WATCH_COL = "MultiWatch";
    private const string KEY_PERCENT_COL = "KeyedPercent";
    private const string EXCESS_ABOVE_KEY_PERCENT_COL = "ExcessAboveKeyedPercent";
    private const string P4_VOL_PERECENT_COL = "P4VolumeIncreasePercent";
    private const string ACTIVE_DAY_FLAG = "ActiveDayFlag";
    private const string APPROVAL_DATE = "ApprovalDate";
    private const string MD_DESCRIPTION = "MDDescription";
    private const string PROFILE_COL = "ProfileType";
    private const string PROFILE_DESC_COL = "ProfileDescription";
    private const string SIC_CODE_COL = "SICCode";
    private const string SIC_DESC_COL = "SICDescription";
    private const string RV_COL = "RulesViolatedToday";
    private const string RQ_CHECK_BOX_COL = "RQCheckbox";

    private readonly string[] UNCHANGE_COLUMNS = { "CheckBoxColumn", "RQCheckbox", "MerchantName", "CardView", "MerchantNumber" };
    private const string RECORD_ID = "RecordID";
    private const string DESCEND = "Descending";
    private const string ASCEND = "Ascending";
    private const string NONE = "None";
    private const int ROW_HEIGHT = 30;
    private bool _isSorting = false;

    #endregion Constants

    #region Fields

    private string _merchantIntruderQuery = string.Empty;

    #endregion Fields

    #region Properties

    private string MerchantIntruderQuery
    {
        get
        {
            if (_merchantIntruderQuery.IsNullOrEmpty())
            {
                _merchantIntruderQuery =
                    GeneralFuncsLib.BuildIntruderInfoForQueryStr(
                        uxReportAggregateGrid.ID,
                        new string[] { MERCHANT_NR_COL });
            }
            return _merchantIntruderQuery;
        }
    }

    private bool HasRQColumn
    {
        get
        {
            return GeneralFuncsLib.HasQueuingMechanismFeature
                                && RiskSessionManager.DetectionQueue != null
                                && RiskSessionManager.DetectionQueue.AssignmentType != WebSiteEnums.AssignmentType.WorkQueue
                                && RiskSessionManager.DetectionQueue.ReportDate == DateTime.Today;
        }
    }

    #endregion Properties

    #region Methods

    #region Protected Methods

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadReportViewMode();
        }
        uxReportAggregateGrid.IsIntruder = true;
        uxReportAggregateGrid.IntruderSourceName = GeneralFuncsLib.GetRequestFileName() + uxReportAggregateGrid.ID;
    }

    protected void ChangeView(object sender, EventArgs e)
    {
        RiskSessionManager.DetectionQueue.AggregateSortExpression = string.Empty;
        RiskSessionManager.DetectionQueue.AggregateSortOrder = string.Empty;
        uxReportAggregateGrid.Rebind();
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        if (Page.IsIntruderDetected) return;

        //for requeue feature
        var assignmentType = RiskSessionManager.DetectionQueue.AssignmentType;
        if (GeneralFuncsLib.HasQueuingMechanismFeature
            && assignmentType != WebSiteEnums.AssignmentType.WorkQueue
            && RiskSessionManager.DetectionQueue.ReportDate == DateTime.Today)
        {
            uxReportAggregateGrid.Columns.FindByUniqueName(RQ_CHECK_BOX_COL).Visible = true;
        }
        //

        if (RiskSessionManager.DetectionQueue == null
            || (RiskSessionManager.DetectionQueue != null
                && RiskSessionManager.DetectionQueue.AssignmentID < 1))
        {
            uxReportAggregateGrid.DataSource = new DataTable();
            return;
        }

        uxExporterAggregateTop.GridHeader = VeraCodeSolution.ValidateResponseData(
            RiskSessionManager.DetectionQueue.Header);
        uxExporterAggregateTop.GridTitle = VeraCodeSolution.ValidateResponseData(GetLocalResourceObject("GridTitle.Title").ToString());
        uxExporterAggregateTop.GridSubTitle = VeraCodeSolution.ValidateResponseData(RiskSessionManager.DetectionQueue.Header.Replace("Assignment: ", string.Empty));

        //Sort
        if (RiskSessionManager.DetectionQueue != null && !string.IsNullOrEmpty(RiskSessionManager.DetectionQueue.AggregateSortExpression) && RiskSessionManager.DetectionQueue.AggregateSortOrder.ToLower() != "none")
        {
            uxReportAggregateGrid.AS_SortExpression = RiskSessionManager.DetectionQueue.AggregateSortExpression + " " + (RiskSessionManager.DetectionQueue.AggregateSortOrder == "Descending" ? "DESC" : "ASC");
        }
        else
        {
            uxReportAggregateGrid.AS_SortExpression = string.Empty;
        }

        FilterParameterCollection parameterList = new FilterParameterCollection();
        parameterList.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameterList.Add(new FilterParameter(
            "@ReportDate",
            RiskSessionManager.DetectionQueue.ReportDate,
            DbType.Date));
        parameterList.Add(new FilterParameter(
            "@Mode",
            (int)RiskSessionManager.DetectionQueue.MerchantWorkedType,
            DbType.Int32));
        parameterList.Add(new FilterParameter("@AssignmentID", RiskSessionManager.DetectionQueue.AssignmentID, DbType.Int64));
        uxReportAggregateGrid.DataSourceInvoker = new ASFuncInvoker(
            WebServices.RiskServices, "GetReports",
            new object[] {
                            SPA_GET_AQ_AGGREGATE_AS,
                            ReportServices.ConvertToFilterParamWSArray(parameterList) });

        uxReportAggregateGrid.VisibleGrid(true);
    }

    protected void uxExport_OnNeedExportConfig(object sender, ExportConfig exportConfig)
    {
        // Top
        uxExporterAggregateTop.Formatter = new Dictionary<string, Func<object, string>>();
        uxExporterAggregateTop.Formatter.Add(KEY_PERCENT_COL,
            new Func<object, string>(GeneralFuncsLib.ConvertIntToPercent));
        uxExporterAggregateTop.Formatter.Add(EXCESS_ABOVE_KEY_PERCENT_COL,
            new Func<object, string>(GeneralFuncsLib.ConvertIntToPercent));
        uxExporterAggregateTop.Formatter.Add(P4_VOL_PERECENT_COL,
            new Func<object, string>(GeneralFuncsLib.ConvertIntToPercent));

        exportConfig.AllowHtmlEncoded = true;
        exportConfig.FileName = GeneralFuncsLib.FormatFileName(RiskSessionManager.DetectionQueue.Header);
        uxReportAggregateGrid.Columns.FindByUniqueName(RQ_CHECK_BOX_COL).Visible = false;
        uxReportAggregateGrid.Columns.FindByUniqueName("CheckBoxColumn").Visible = false;
        uxReportAggregateGrid.Columns.FindByUniqueNameSafe("RQCheckbox").Visible = false;
        uxReportAggregateGrid.Columns.FindByUniqueNameSafe("CardView").Visible = false;
    }

    protected void uxReportGrid_DataSourceReady(object sender, EventArgs e)
    {
        //use for requeque
        if (GeneralFuncsLib.HasQueuingMechanismFeature)
        {
            RiskSessionManager.DetectionQueueTemporaryRequeueInfo.TotalAlertMerchant
                = uxReportAggregateGrid.MasterTableView.VirtualItemCount;
        }

        if (uxReportAggregateGrid.AS_DataSource.Rows.Count == 0 || uxReportAggregateGrid.AS_DataSource == null)
        {
            chkHeaderCV.Visible = false;
        }
    }

    private void SwitchView()
    {
        foreach (GridColumn col in uxReportAggregateGrid.MasterTableView.Columns)
        {
            col.Visible = optGrid.Checked;
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
        uxExporterAggregateTop.ShowHideCustomViewLink(false);
        if (optCard.Checked)
        {
            uxReportAggregateGrid.CssClass += " block-card-view ";
            ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript("setTabActive1('0');");
        }
        else
        {
            uxReportAggregateGrid.CssClass = uxReportAggregateGrid.CssClass.Replace("block-card-view", "").Trim();
            ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript("setTabActive1('1');");
        }
    }

    protected void uxReportGrid_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        if (!IsPostBack)
        {
            LoadReportViewMode();
        }
        OnDataBindControls(DataBindAction.BindAQAggregateGrid, uxReportAggregateGrid);
    }

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
                uxReportGrid_ItemDataBound_GridView(sender, e);
            }
            else
            {
                //Custom for CardView
                uxReportGrid_ItemDataBound_CardView(dataItem, rowItem, e.Item.ItemIndex);
            }
        }
    }

    private void RequeueAllGridView(GridHeaderItem headerItem)
    {
        HtmlInputCheckBox chkRequeueAllMerchant = headerItem["RQCheckbox"].FindControl("chkHeader") as HtmlInputCheckBox;
        if (chkRequeueAllMerchant != null)
        {
            //have records

            if (uxReportAggregateGrid.MasterTableView.VirtualItemCount > 0)
            {
                chkRequeueAllMerchant.Disabled = false;
                //process check state from spa
                chkRequeueAllMerchant.Attributes.Add("onclick", string.Format("RequeueAllMerchants(this)"));
                if ((RiskSessionManager.DetectionQueueTemporaryRequeueInfo.IsRequeueAll
                    && RiskSessionManager.DetectionQueueTemporaryRequeueInfo.RequeuedMerchantList.IsEmpty())
                    || (RiskSessionManager.DetectionQueueTemporaryRequeueInfo.TotalAlertMerchant == RiskSessionManager.DetectionQueueTemporaryRequeueInfo.RequeuedMerchantList.Count
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
        if (uxReportAggregateGrid.MasterTableView.VirtualItemCount > 0)
        {
            //process check state from spa
            chkHeaderCV.Attributes.Add("onclick", string.Format("RequeueAllMerchants(this)"));
            if ((RiskSessionManager.DetectionQueueTemporaryRequeueInfo.IsRequeueAll
                && RiskSessionManager.DetectionQueueTemporaryRequeueInfo.RequeuedMerchantList.IsEmpty())
                || (RiskSessionManager.DetectionQueueTemporaryRequeueInfo.TotalAlertMerchant == RiskSessionManager.DetectionQueueTemporaryRequeueInfo.RequeuedMerchantList.Count
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

    protected override void OnPreRender(EventArgs e)
    {
        //Hide/Show columns by selecting view mode
        SwitchView();
        base.OnPreRender(e);

    }

    protected void uxReportAggregateGrid_PreRender(object sender, EventArgs e)
    {
        uxReportAggregateGrid.ClientSettings.Scrolling.AllowScroll = true;
        uxReportAggregateGrid.ClientSettings.Scrolling.UseStaticHeaders = true;
        // If ReportDate = Today, we will freeze 3 columns: RQ, Merchant ID, Merchant Name
        // else,  we will freeze 2 columns: Merchant ID, Merchant Name
        uxReportAggregateGrid.ClientSettings.Scrolling.FrozenColumnsCount =
            RiskSessionManager.DetectionQueue.ReportDate == DateTime.Today ? 3 : 2;
    }

    #endregion Protected Methods

    private string FormatMerchantName(DataRow rowItem, TableCell merchantNameCell)
    {
        // Format hyperlink
        string url = rowItem[MERCHANT_NAME_COL].ToString();
        if (Page.IsUserWithPermission(WebSiteConstants.VIEW_RISK_REPORT)
            || Page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RSK_RPT)
            || Page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RSK_RPT_MS))
        {
            url = string.Format(
                "<a class=\"link\" href=\"#\" style=\"cursor:pointer\" onclick=\"openPopupWindow('{0}','RiskReport'); return false;\">{1}</a>",
                "rm_RiskReport.aspx?" + Page.BuildSecureQueryString(
                    string.Format("merchantnumber={0}&IsPopup={1}{2}",
                        GeneralFuncsLib.NvlString(rowItem[MERCHANT_NR_COL]),
                        true,
                        MerchantIntruderQuery)),
                url);
        }
        merchantNameCell.Text = VeraCodeSolution.GetOutputHtmlString(url);

        // Build query string
        string assignmentTypeQueryString =
            RiskSessionManager.DetectionQueue.AssignmentType != WebSiteEnums.AssignmentType.DetectionQueue
            ? "&AssignmentType=" + (int)RiskSessionManager.DetectionQueue.AssignmentType : "";

        string queryStringImage = this.Page.BuildSecureQueryString(
            string.Format("MerchantNumber={0}&ReportDate={1}&AssignmentID={2}{3}",
            rowItem[MERCHANT_NR_COL],
            RiskSessionManager.DetectionQueue.ReportDate,
            RiskSessionManager.DetectionQueue.AssignmentID,
            assignmentTypeQueryString));
        string urlImage = "rm_DQReasonModal.aspx?" + queryStringImage;
        urlImage = "<a class=\"image-link\" href=\"#\" style=\"cursor:pointer\" onclick=\"OpenInstanceWindow('" + urlImage + "','DQWindow'); return false;\"><img src='../res/images/information.gif' border='0' alt='Parameter Violations' /></a>";

        merchantNameCell.Text = VeraCodeSolution.DoVeraCode(urlImage + " " + merchantNameCell.Text);
        return url;
    }

    public void Rebind(bool keepPageIndex, bool isFirstTimeLoading)
    {
        uxReportAggregateGrid.Rebind();
    }

    public void uxReportGrid_ItemDataBound_GridView(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridHeaderItem)
        {
            GridHeaderItem headerItem = e.Item as GridHeaderItem;
            if (headerItem[RQ_CHECK_BOX_COL].Visible)
            {
                HtmlInputCheckBox chkRequeueAllMerchant = headerItem[RQ_CHECK_BOX_COL].FindControl("chkHeader") as HtmlInputCheckBox;
                if (chkRequeueAllMerchant != null)
                {
                    //have records
                    if (uxReportAggregateGrid.MasterTableView.VirtualItemCount > 0)
                    {
                        chkRequeueAllMerchant.Disabled = false;
                        //process check state from spa
                        chkRequeueAllMerchant.Attributes.Add("onclick", string.Format("RequeueAllMerchants(this)"));
                        if ((RiskSessionManager.DetectionQueueTemporaryRequeueInfo.IsRequeueAll
                            && RiskSessionManager.DetectionQueueTemporaryRequeueInfo.RequeuedMerchantList.IsEmpty())
                            || (RiskSessionManager.DetectionQueueTemporaryRequeueInfo.TotalAlertMerchant
                                == RiskSessionManager.DetectionQueueTemporaryRequeueInfo.RequeuedMerchantList.Count
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
        }
        else if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            var rowItem = (e.Item.DataItem as DataRowView).Row;

            var MerchantNumber = rowItem["MerchantNumber"];
            //Wk checkbox column
            Control ctrl = dataItem["CheckBoxColumn"].FindControl("cBox");
            if (ctrl != null)
            {
                HtmlInputCheckBox chkBox = ctrl as HtmlInputCheckBox;
                chkBox.Checked = rowItem["IsWorked"].ToString() == "1";
                chkBox.Value = rowItem["MerchantNumber"].ToString();
                chkBox.Attributes.Add("onclick", string.Format("ChangeAQMerchantWorked(this,{0})", rowItem["TodayTotalVolume"]));
                chkBox.Disabled = !GeneralFuncsLib.ReadOnlyRiskManagementEnableStatus((SecurePage)Page);
            }

            if (rowItem["IsWorked"].ToString() == "1")
                dataItem["MerchantName"].BackColor = Color.LightPink;
            //Re-queue checkbox column
            //process requeued merchant
            if (dataItem[RQ_CHECK_BOX_COL].Visible)
            {
                HtmlInputCheckBox chkRequeueSingleMerchant = dataItem[RQ_CHECK_BOX_COL].FindControl("chkItem") as HtmlInputCheckBox;
                if (chkRequeueSingleMerchant != null)
                {
                    //this merchant was requeued before, gray out the column
                    if (Convert.ToBoolean(rowItem["IsRequeued"]))
                    {
                        dataItem[RQ_CHECK_BOX_COL].BackColor = Color.FromName("#BBB");
                    }

                    //process check state from spa
                    chkRequeueSingleMerchant.Disabled = !GeneralFuncsLib.ReadOnlyRiskManagementEnableStatus((SecurePage)Page);
                    if (RiskSessionManager.DetectionQueueTemporaryRequeueInfo.RequeuedMerchantList != null)
                    {
                        chkRequeueSingleMerchant.Checked = RiskSessionManager.DetectionQueueTemporaryRequeueInfo.RequeuedMerchantList.Contains(MerchantNumber.ToString());
                    }
                    if (RiskSessionManager.DetectionQueueTemporaryRequeueInfo.IsRequeueAll)
                    {
                        chkRequeueSingleMerchant.Checked = true;
                        chkRequeueSingleMerchant.Disabled = true;
                    }

                    chkRequeueSingleMerchant.Value = MerchantNumber.ToString();
                    chkRequeueSingleMerchant.Attributes.Add("onclick", string.Format("RequeueSingleMerchant(this)"));
                }
            }

            //merchant name
            var merchantName = dataItem[MERCHANT_NAME_COL];
            string url = FormatMerchantName(rowItem, merchantName);

            //profile
            string profiletext = rowItem[PROFILE_DESC_COL].ToString();
            dataItem[PROFILE_DESC_COL].ToolTip = VeraCodeSolution.DoVeraCode(profiletext);

            //SIC
            dataItem[SIC_CODE_COL].ToolTip = VeraCodeSolution.DoVeraCode(
                rowItem[SIC_DESC_COL].ToString().Replace("&nbsp;", string.Empty));

            string RV = rowItem[RV_COL].ToString();
            dataItem[RV_COL].ToolTip = VeraCodeSolution.DoVeraCode(RV);

            //if (RV.IndexOf(',', RV.IndexOf(',') + 1) > 0)
            //{
            //    RV = RV.Substring(0, RV.IndexOf(',', RV.IndexOf(',') + 1) + 1) +
            //                        (RV.IsNullOrEmpty() ? "" : "...");
            //}
            //dataItem[RV_COL].Text = VeraCodeSolution.DoVeraCode(RV);
        }
    }

    private void uxReportGrid_ItemDataBound_CardView(GridDataItem dataItem, DataRow rowItem, int dataIndex)
    {
        var merchantNumber = rowItem["MerchantNumber"];
        var cardViewColumnCtr = dataItem["CardView"];

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
                    chkRequeueSingleMerchant.Attributes.Add("onclick", string.Format("RequeueSingleMerchant(this)"));
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
            chkBox.Checked = rowItem["IsWorked"].ToString().Trim() == "1";
            chkBox.Value = rowItem["MerchantNumber"].ToString();
            chkBox.Attributes.Add("onclick", string.Format("ChangeAQMerchantWorked(this,{0})", rowItem["TodayTotalVolume"]));
            chkBox.Disabled = !GeneralFuncsLib.ReadOnlyRiskManagementEnableStatus((SecurePage)Page);
        }

        //merchant name
        //merchant name
        var merchantName = cardViewColumnCtr.FindControl("merchantName") as HtmlGenericControl;

        string url = String.Empty;
        string urlText = rowItem["MerchantName"].ToString();

        var merchantNameColor = Color.Transparent.Name;
        if (rowItem["IsWorked"].ToString() == "1")
        {
            cardViewColumnCtr.CssClass += "row-checked";
            merchantNameColor = Color.LightPink.Name;
        }

        if (GeneralFuncsLib.HasRiskRptPermission((SecurePage)Page))
        {
            url = RiskGeneral.BuildMerchantHyperlinkInRisk((SecurePage)Page,
                rowItem[MERCHANT_NR_COL], true, MerchantIntruderQuery, urlText);
            url = string.Format("<a class=\"link\" href=\"#\" style=\"cursor:pointer; border-bottom: 2px {2} solid;\" onclick=\"MerchantNumberClick('{0}','{1}',this); return false;\">", rowItem["MerchantNumber"], rowItem["TodayTotalVolume"], merchantNameColor) + urlText + "</a>";
        }
        AddHtml(merchantName, VeraCodeSolution.DoVeraCode(url));

        string assignmentTypeQueryString = RiskSessionManager.DetectionQueue.AssignmentType != WebSiteEnums.AssignmentType.DetectionQueue ? "&AssignmentType=" + (int)RiskSessionManager.DetectionQueue.AssignmentType : "";
        string queryStringImage = this.Page.BuildSecureQueryString("MerchantNumber=" + rowItem[MERCHANT_NR_COL] + "&ReportDate=" + RiskSessionManager.DetectionQueue.ReportDate + "&AssignmentID=" + RiskSessionManager.DetectionQueue.AssignmentID + assignmentTypeQueryString);
        string urlImage = "rm_DQReasonModal.aspx?" + queryStringImage;
        urlImage = "<a class=\"image-link\" href=\"#\" style=\"cursor:pointer\" onclick=\"OpenInstanceWindow('" + urlImage + "','DQWindow'); return false;\"><img src='" + ResolveUrl("~/") + "res/img/information.png' border='0' alt='" + GetLocalResourceObject("DetectionQueueRainbowReport_ascx_PV").ToString() + "' /></a>";
        AddHtml(merchantName, VeraCodeSolution.DoVeraCode(merchantName.InnerHtml + " " + urlImage));

        //SIC
        var sicCode = cardViewColumnCtr.FindControl("sicCode") as HtmlGenericControl;
        AddTooltip(sicCode, VeraCodeSolution.DoVeraCode(rowItem["SICDescription"].ToString().Replace("&nbsp;", string.Empty)));
        //just for first row
        if (dataIndex == 0 && _isSorting)
        {
            AddSortIcon(cardViewColumnCtr);
            _isSorting = false;
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

    private void AddSortIcon(TableCell ctrl)
    {
        if (RiskSessionManager.DetectionQueue == null || RiskSessionManager.DetectionQueue.AggregateSortExpression == null) return;

        var sortingField = RiskSessionManager.DetectionQueue.AggregateSortExpression;
        var sortingOrder = RiskSessionManager.DetectionQueue.AggregateSortOrder;

        var ctrlId = "lbtSort_" + sortingField;
        var tempBtn = ctrl.FindControl(ctrlId);
        if (tempBtn != null)
        {
            var linkButtonCtrl = tempBtn as LinkButton;
            switch (RiskSessionManager.DetectionQueue.AggregateSortOrder)
            {
                case DESCEND:
                    linkButtonCtrl.CssClass += " rgSortQueueDesc";
                    break;
                case ASCEND:
                    linkButtonCtrl.CssClass += " rgSortQueueAsc";
                    break;
                default:
                    linkButtonCtrl.CssClass.Replace("rgSortQueueAsc", "")
                                           .Replace("rgSortQueueDesc", "");
                    break;
            }
        }
    }

    #endregion Methods

    private void SortHandle(string fieldName)
    {
        if (fieldName == RiskSessionManager.DetectionQueue.AggregateSortExpression)
        {
            switch (RiskSessionManager.DetectionQueue.AggregateSortOrder)
            {
                case DESCEND:
                    RiskSessionManager.DetectionQueue.AggregateSortOrder = ASCEND;
                    break;
                case ASCEND:
                    RiskSessionManager.DetectionQueue.AggregateSortOrder = NONE;
                    break;
                default:
                    RiskSessionManager.DetectionQueue.AggregateSortOrder = DESCEND;
                    break;
            }
        }
        else
        {
            RiskSessionManager.DetectionQueue.AggregateSortExpression = fieldName;
            RiskSessionManager.DetectionQueue.AggregateSortOrder = DESCEND;
        }
    }

    protected void uxReportAggregateGrid_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (optCard.Checked)
        {
            if (e.CommandName.StartsWith("Sort_"))
            {
                _isSorting = true;
                var fieldName = e.CommandName.Split('_')[1];
                SortHandle(fieldName);
                uxReportAggregateGrid.Rebind();
            }
        }
    }
    protected void uxReportAggregateGrid_SortCommand(object sender, GridSortCommandEventArgs e)
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
        //RiskSessionManager.DetectionQueue.AggregateSortExpression = sortExpr.FieldName;
        //RiskSessionManager.DetectionQueue.AggregateSortOrder = e.NewSortOrder.ToString();
    }

    protected string BuildSortExpression(GridSortExpression e)
    {
        string col = e.FieldName;
        string order = e.SortOrder.ToString();
        order = order == "Descending" ? "DESC" :
                order == "None" ? "" : "ASC";
        return order == "" ? "" : col + " " + order;
    }

    protected string FormatCurrency(object data)
    {
        if (data == DBNull.Value)
            return string.Empty;
        else return FormatData.FormatCurrency(data, SessionManager.CurrencyFortmat);
    }
    protected string FormatCurrency(object data, int count)
    {
        if (data == DBNull.Value)
            return string.Empty;
        else return FormatData.FormatCurrency(data, count, SessionManager.CurrencyFortmat);
    }

    protected static string FormatPercent(object val)
    {
        return (val == null || val == DBNull.Value || string.IsNullOrEmpty(val.ToString())) ? string.Empty : decimal.Parse(val.ToString()).ToString("0.00") + "%";
    }
    protected static string FormatInteger(object val)
    {
        return (val == null || val == DBNull.Value || string.IsNullOrEmpty(val.ToString())) ? string.Empty : decimal.Parse(val.ToString()).ToString("#,#0");
    }
}
