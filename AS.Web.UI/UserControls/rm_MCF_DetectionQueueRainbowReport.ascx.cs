using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Exporter;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Security.WS.Entities.Utility;
using AS.Web.Business;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControls_rm_MCF_DetectionQueueRainbowReport : GlobalUserControl
{
    #region ---- Variable & Enum ----
    enum DataBindAction
    {
        BindRainbowGrid,
    }

    public event AfterGridSortHandler AfterGridSort;
    public delegate void AfterGridSortHandler(string colSort);

    private const string SESSION_FILTERING_OPTIONS = "DetectionQueueFilteringOptions";
    private const string MERCHANT_NUMBER = "MerchantNumber";
    private const string RECORD_ID = "RecordID";
    private const string WORKMERCHANT_ID = "WorkingMerchantID";
    private const string CYCLE_ID = "CycleID";
    private const string ORIGINALCYCLE_ID = "ParentCycleID";
    private const string NONE = "None";
    private const int ROW_HEIGHT = 30;
    private const string DESCEND = "Descending";
    private const string ASCEND = "Ascending";
    private const string COL_ISDEFAULT = "IsDefault";
    private string _merchantIntruderQuery = string.Empty;
    private bool _isSorting = false;
    private Dictionary<string, RiskCustomizeColumn> _dicRiskCustomizeColumn = new Dictionary<string, RiskCustomizeColumn>();
    private Dictionary<string, string> _dicAttrToolTip = new Dictionary<string, string>();
    private const string TOOLTIP_GRID_TEXT1_FORMAT = "<span class='tooltip-text1'>{0}</span>";
    private const string TOOLTIP_GRID_TEXT2_FORMAT = "<span class='tooltip-text2'>{0}</span>";
    private DataTable _dispositionTable = new DataTable();
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
            return _AssignmentType == WebSiteEnums.AssignmentType.WorkQueue;
        }
    }

    private bool IsAQ
    {
        get
        {
            return _AssignmentType == WebSiteEnums.AssignmentType.AggregateQueue;
        }
    }

    private bool IsDistinctQueue
    {
        get
        {
            return _AssignmentType == WebSiteEnums.AssignmentType.DetectionQueueDistinct;
        }
    }

    private bool HasRQColumn
    {
        get
        {
            return RiskSessionManager.MCF_BarometerReport != null
                    && _ReportDate == DateTime.Today
                    && GeneralFuncsLib.HasQueuingMechanismFeature;
        }
    }

    private string[] Default_Invisible_Columns = new string[] { "MerchantNumber", "CurrentStatusDesc", "IsRequeuedDesc" };


    /// <summary>
    /// Gets custom view id.
    /// </summary>
    /// <value>Gets custom view id.</value>
    private int _CustomViewId
    {
        get
        {
            var session = RiskSessionManager.RiskMCFDQRainbowReport;
            if (session != null && session.ContainsKey(_AssignmentID))
            {
                return Convert.ToInt32(session[_AssignmentID]);
            }
            return -1;
        }
    }

    private int _AssignmentID
    {
        get
        {
            int assignmentID = 0;
            if (!string.IsNullOrEmpty(Page.SecureQueryString["AssignmentID"]))
            {
                assignmentID = Convert.ToInt32(Page.SecureQueryString["AssignmentID"]);
            }
            else
            {
                assignmentID = -1;
            }

            if (assignmentID <= 0)
            {
                LogHepler.WriteLogWarn("DetectionQueueRainbowReport_SecureQueryString",
                    "_AssignmentID is invalid: " + assignmentID + " - SecureQueryString: " + Page.SecureQueryString.ToASString(), string.Empty);
            }

            return assignmentID;
        }
    }

    private string _AssignmentName
    {
        get
        {

            if (!string.IsNullOrEmpty(Page.SecureQueryString["AssignmentName"]))
            {
                return Convert.ToString(Page.SecureQueryString["AssignmentName"]);
            }
            else
            {
                return string.Empty;
            }
        }
    }

    private WebSiteEnums.AssignmentType _AssignmentType
    {
        get
        {
            if (!string.IsNullOrEmpty(Page.SecureQueryString["AssignmentType"]))
            {
                return (WebSiteEnums.AssignmentType)Enum.Parse(typeof(WebSiteEnums.AssignmentType), Page.SecureQueryString["AssignmentType"]);
            }
            else
            {
                return WebSiteEnums.AssignmentType.All;
            }
        }
    }

    private DateTime _ReportDate
    {
        get
        {

            if (!string.IsNullOrEmpty(Page.SecureQueryString["ReportDate"]))
            {
                return Convert.ToDateTime(Page.SecureQueryString["ReportDate"]);
            }
            else
            {
                return DateTime.Now;
            }
        }
    }

    private string _Header { get { return string.Format("{0} ({1})", _AssignmentName, _ReportDate.ToString(WebSiteConstants.DATE_FORMAT)); } }

    private string _ApplyFilterId
    {
        get
        {

            if (!string.IsNullOrEmpty(Page.SecureQueryString["ApplyFilterId"]))
            {
                return Convert.ToString(Page.SecureQueryString["ApplyFilterId"]);
            }
            else
            {
                return string.Empty;
            }
        }
    }

    private WebSiteEnums.MCF_MerchantWorkingStatus _FilterWorkingStatus
    {
        get
        {
            if (!string.IsNullOrEmpty(filterWorkingStatus.Value))
            {
                return (WebSiteEnums.MCF_MerchantWorkingStatus)Convert.ToInt32(filterWorkingStatus.Value);
            }
            else
            {
                return WebSiteEnums.MCF_MerchantWorkingStatus.All;
            }
        }
    }
    private string _OrderByColumnName
    {
        get
        {
            if (!string.IsNullOrEmpty(Page.SecureQueryString["OrderBy"]))
            {
                return Page.SecureQueryString["OrderBy"];
            }
            else
            {
                return string.Empty;
            }
        }
    }

    private string orderByColumnName
    {
        get
        {
            var sessionType = RiskSessionManager.MCF_BarometerReport != null ? RiskSessionManager.MCF_BarometerReport.SortOrder : null;
            var sessionSortField = RiskSessionManager.MCF_BarometerReport != null ? RiskSessionManager.MCF_BarometerReport.SortExpression : null;
            GridSortExpression sortExpr = new GridSortExpression();

            if (!(string.IsNullOrEmpty(sessionType) && string.IsNullOrEmpty(sessionSortField)))
            {
                sortExpr.SortOrder = (GridSortOrder)Enum.Parse(typeof(GridSortOrder), sessionType);
                sortExpr.FieldName = !string.IsNullOrEmpty(sessionSortField) ? string.Format("[{0}]", sessionSortField) : string.Empty;
                return sortExpr.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
    }

    private static DataTable _dispositionList
    {
        get
        {
            return RiskSessionManager.Risk_MCF_DispositionList;
        }
        set
        {
            RiskSessionManager.Risk_MCF_DispositionList = value;
        }
    }

    #endregion ---- Variable & Enum ----

    #region ---- Private Methods -----

    private void SortHandle(string fieldName)
    {
        GridSortExpression sortExpr = new GridSortExpression();
        if (fieldName == RiskSessionManager.MCF_BarometerReport.SortExpression)
        {
            switch (RiskSessionManager.MCF_BarometerReport.SortOrder)
            {
                case DESCEND:
                    RiskSessionManager.MCF_BarometerReport.SortOrder = ASCEND;
                    sortExpr.FieldName = fieldName;
                    sortExpr.SortOrder = GridSortOrder.Ascending;
                    break;
                case ASCEND:
                    RiskSessionManager.MCF_BarometerReport.SortOrder = NONE;
                    sortExpr.FieldName = string.Empty;
                    sortExpr.SortOrder = GridSortOrder.None;
                    break;
                default:
                    RiskSessionManager.MCF_BarometerReport.SortOrder = DESCEND;
                    sortExpr.FieldName = fieldName;
                    sortExpr.SortOrder = GridSortOrder.Descending;
                    break;
            }
        }
        else
        {
            sortExpr.FieldName = fieldName;
            sortExpr.SortOrder = GridSortOrder.Descending;
        }

        RiskSessionManager.MCF_BarometerReport.SortExpression = sortExpr.FieldName;
        RiskSessionManager.MCF_BarometerReport.SortOrder = sortExpr.SortOrder.ToString();
    }

    private void SwitchView()
    {
        var isGrid = optGrid.Checked;
        //default value Grid if no Check
        if (!optGrid.Checked && !optCard.Checked)
            isGrid = true;

        foreach (GridColumn col in uxReportGrid.MasterTableView.Columns)
        {
            col.Visible = isGrid && !Default_Invisible_Columns.Contains(col.UniqueName);
            if (col.UniqueName.Equals("CardView", StringComparison.InvariantCultureIgnoreCase))
            {
                col.Visible = !isGrid;
            }
            else if (col.UniqueName.Equals("RQCheckbox", StringComparison.InvariantCultureIgnoreCase))
            {
                col.Visible = HasRQColumn && !optCard.Checked;
            }
            else if (col.UniqueName.Equals("CheckBoxColumn", StringComparison.InvariantCultureIgnoreCase))
            {
                col.Visible = !IsDistinctQueue && !optCard.Checked;
            }
        }

        chkHeaderCV.Visible = optCard.Checked && HasRQColumn;
        if (optCard.Checked)
        {
            uxReportGrid.CssClass += " block-card-view ";
            ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript("setTabActive('0');");
        }
        else
        {
            uxReportGrid.CssClass = uxReportGrid.CssClass.Replace("block-card-view", string.Empty).Trim();
            ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript("setTabActive('1');");
        }
    }

    private void ItemDataBound_GridView(GridDataItem dataItem, DataRow rowItem)
    {
        var url = string.Empty;
        var merchantNumber = rowItem["MerchantNumber"];
        var currentStatus = (WebSiteEnums.WorkStatus)Enum.Parse(typeof(WebSiteEnums.WorkStatus), rowItem["CurrentStatus"].ToString());

        if (dataItem["RQCheckbox"].Visible)
        {
            HtmlInputCheckBox chkRequeueSingleMerchant = dataItem["RQCheckbox"].FindControl("chkItem") as HtmlInputCheckBox;
            if (chkRequeueSingleMerchant != null)
            {
                //this merchant was requeued before, gray out the column
                if (bool.Parse(rowItem["IsRequeued"].ToString()))
                {
                    dataItem["RQCheckbox"].BackColor = Color.FromName("#e8e8e8");
                }
                if (currentStatus != WebSiteEnums.WorkStatus.WorkInProgressByOther)
                {
                    //process check state from spa
                    chkRequeueSingleMerchant.Disabled = !GeneralFuncsLib.ReadOnlyRiskManagementEnableStatus(this.Page);
                    if (RiskSessionManager.MCF_DQTemporaryBarometerReport.RequeuedMerchantList != null)
                    {
                        chkRequeueSingleMerchant.Checked = RiskSessionManager.MCF_DQTemporaryBarometerReport.RequeuedMerchantList.Contains(merchantNumber.ToString());
                    }
                    if (RiskSessionManager.MCF_DQTemporaryBarometerReport.IsRequeueAll)
                    {

                        chkRequeueSingleMerchant.Checked = true;
                        chkRequeueSingleMerchant.Disabled = true;

                    }

                    chkRequeueSingleMerchant.Value = merchantNumber.ToString();
                    chkRequeueSingleMerchant.Attributes.Add("onclick", string.Format("RequeueSingleMerchant(this," + IsWQ.ToString().ToLower()
                        + "," + rowItem[CYCLE_ID].ToString() + "," + rowItem[ORIGINALCYCLE_ID].ToString() + ")"));
                }
                else
                {
                    chkRequeueSingleMerchant.Attributes.Add("rqwip", "other");
                    chkRequeueSingleMerchant.Attributes.Add("disabled", "disabled");
                }
            }
        }
        // Generate Work popover
        if (!string.IsNullOrEmpty(rowItem["WorkingMerchantID"].ToString()))
        {
            var workStateID = string.IsNullOrEmpty(rowItem["WorkStateID"].ToString()) ?
                0 : int.Parse(rowItem["WorkStateID"].ToString());
            _dicAttrToolTip = new Dictionary<string, string>();
            _dicAttrToolTip.Add(CYCLE_ID, rowItem[CYCLE_ID].ToString());
            _dicAttrToolTip.Add(ORIGINALCYCLE_ID, rowItem[ORIGINALCYCLE_ID].ToString());
            _dicAttrToolTip.Add(MERCHANT_NUMBER, rowItem[MERCHANT_NUMBER].ToString());
            _dicAttrToolTip.Add("AssignmentID", _AssignmentID.ToString());
            _dicAttrToolTip.Add("ReportDate", _ReportDate.ToString());
            _dicAttrToolTip.Add("WorkStateID", workStateID.ToString());
            _dicAttrToolTip.Add("WorkingStatusMessage", rowItem["WorkingStatusMessage"].ToString());
            _dicAttrToolTip.Add("WorkingMerchantID", rowItem["WorkingMerchantID"].ToString());
            _dicAttrToolTip.Add("CurrentStatus", currentStatus.ToString());

            Control ctrl = dataItem["CheckBoxColumn"].FindControl("workItem");
            AS.Controls.Global.ASMCFWorkContent lblWork = ctrl as AS.Controls.Global.ASMCFWorkContent;
            lblWork.DicAttrToolTip = _dicAttrToolTip;
            lblWork.DispositionTable = _dispositionTable;
            lblWork.DispositionChecked = rowItem["DispositionIDs"].ToString();
        }

        //merchant name
        var merchantName = dataItem["MerchantName"];

        string urlText = rowItem["MerchantName"].ToString();

        if (GeneralFuncsLib.HasRiskRptPermission(this.Page))
        {
            url = RiskGeneral.BuildMerchantHyperlinkInRisk(this.Page,
                rowItem[MERCHANT_NUMBER], true, MerchantIntruderQuery, urlText, true);
            url = string.Format("<a class=\"link\" href=\"#\" style=\"cursor:pointer\" onclick=\"MerchantNumberClick('{0}','{1}',\'{2}\','{3}',this); return false;\">", rowItem["MerchantNumber"], rowItem["TodayVolume"], _ReportDate.ToString(), _AssignmentID) + urlText + "</a>";
        }

        merchantName.Text = VeraCodeSolution.GetOutputHtmlString(url);
        var merchantNameColor = Color.Transparent;
        if (int.Parse(rowItem["WorkStateID"].ToString()) == (int)WebSiteEnums.WorkStatus.Worked)
        {
            merchantNameColor = Color.LightPink;
        }
        merchantName.Text = RM_MCF_GeneralFuncsLib.FormatBorderText(merchantName.Text, merchantNameColor);

        if (rowItem["Watch"].Equals(1))
        {
            merchantName.Text = VeraCodeSolution.DoVeraCode(merchantName.Text + "(<span style=\"color:red\">*</span>)");

            merchantName.ToolTip = VeraCodeSolution.DoVeraCode(
                "(M)" + rowItem["MerchantNumber"].ToString().Trim() +
                (rowItem["ApprovalDate"] == DBNull.Value ? string.Empty : " - " + ((DateTime)rowItem["ApprovalDate"]).ToString(WebSiteConstants.DATE_FORMAT))
                );
        }
        else if (rowItem["MultiWatch"].Equals(true))
        {
            merchantName.Text = VeraCodeSolution.DoVeraCode(merchantName.Text + "(<span style=\"color:red\">*</span>)");

            merchantName.ToolTip = VeraCodeSolution.DoVeraCode(
                 rowItem["MerchantNumber"].ToString().Trim() +
                (rowItem["ApprovalDate"] == DBNull.Value ? string.Empty : " - " + ((DateTime)rowItem["ApprovalDate"]).ToString(WebSiteConstants.DATE_FORMAT))
                );
        }
        else
        {
            merchantName.ToolTip = VeraCodeSolution.DoVeraCode(
                 rowItem["MerchantNumber"].ToString().Trim() +
                (rowItem["ApprovalDate"] == DBNull.Value ? string.Empty : " - " + ((DateTime)rowItem["ApprovalDate"]).ToString(WebSiteConstants.DATE_FORMAT))
                );
        }

        string assignmentTypeQueryString = _AssignmentType != WebSiteEnums.AssignmentType.DetectionQueue ? "&AssignmentType=" + (int)_AssignmentType : string.Empty;
        string queryStringImage = this.Page.BuildSecureQueryString("MerchantNumber=" + rowItem[MERCHANT_NUMBER] + "&ReportDate=" + _ReportDate + "&AssignmentID=" + _AssignmentID + assignmentTypeQueryString);
        string urlImage = "rm_MCF_DQReasonModal.aspx?" + queryStringImage;
        urlImage = "<a class=\"image-link\" href=\"#\" style=\"cursor:pointer\" onclick=\"openPopupWindowOnMenu(this,'" + urlImage + "','DQMCFWindow2'); return false;\"><img src='" + ResolveUrl("~/") + "res/img/information.png' border='0' alt='" + RM_MCF_GeneralFuncsLib.GetResourceValue("DetectionQueueRainbowReport_ascx_PV").ToString() + "' /></a>";
        merchantName.Text = VeraCodeSolution.DoVeraCode(urlImage + " " + merchantName.Text);

        RM_MCF_GeneralFuncsLib.ItemDataBound_GridView(dataItem, rowItem, this.Page, _dicRiskCustomizeColumn);
    }

    private void ItemDataBound_CardView(GridDataItem dataItem, DataRow rowItem, int dataIndex)
    {
        string url = String.Empty;
        var merchantNumber = rowItem["MerchantNumber"];
        var cardViewColumnCtr = dataItem["CardView"];
        var listColumnCustomizes = new List<string>();
        var currentStatus = (WebSiteEnums.WorkStatus)Enum.Parse(typeof(WebSiteEnums.WorkStatus), rowItem["CurrentStatus"].ToString());

        var colWorkStatus = cardViewColumnCtr.FindControl("colWorkStatus") as HtmlGenericControl;
        if (colWorkStatus != null)
        {
            colWorkStatus.Visible = !IsDistinctQueue;
        }

        //process requeued merchant
        var colRQColumn = cardViewColumnCtr.FindControl("colRQColumn") as HtmlGenericControl;
        if (colRQColumn != null)
        {
            if (HasRQColumn)
            {
                colRQColumn.Visible = true;
                //ResizedMerchantNameColumn(cardViewColumnCtr, true);

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
                    if (currentStatus != WebSiteEnums.WorkStatus.WorkInProgressByOther)
                    {
                        //process check state from spa
                        chkRequeueSingleMerchant.Disabled = !GeneralFuncsLib.ReadOnlyRiskManagementEnableStatus(this.Page);
                        if (RiskSessionManager.MCF_DQTemporaryBarometerReport.RequeuedMerchantList != null)
                        {
                            chkRequeueSingleMerchant.Checked = RiskSessionManager.MCF_DQTemporaryBarometerReport.RequeuedMerchantList.Contains(merchantNumber.ToString());
                        }
                        if (RiskSessionManager.MCF_DQTemporaryBarometerReport.IsRequeueAll)
                        {
                            chkRequeueSingleMerchant.Checked = true;
                            chkRequeueSingleMerchant.Disabled = true;
                        }

                        chkRequeueSingleMerchant.Value = merchantNumber.ToString();
                        chkRequeueSingleMerchant.Attributes.Add("onclick", string.Format("RequeueSingleMerchant(this," + IsWQ.ToString().ToLower()
                        + "," + rowItem[CYCLE_ID].ToString() + "," + rowItem[ORIGINALCYCLE_ID].ToString() + ")"));
                        chkRequeueSingleMerchant.Visible = true;
                    }
                    else
                    {
                        chkRequeueSingleMerchant.Attributes.Add("rqwip", "other");
                        chkRequeueSingleMerchant.Attributes.Add("disabled", "disabled");
                    }
                }
            }
            else
            {
                colRQColumn.Visible = false;
            }
        }

        if (!string.IsNullOrEmpty(rowItem["WorkingMerchantID"].ToString()))
        {
            var workStateID = string.IsNullOrEmpty(rowItem["WorkStateID"].ToString()) ?
               0 : int.Parse(rowItem["WorkStateID"].ToString());
            _dicAttrToolTip = new Dictionary<string, string>();
            _dicAttrToolTip.Add(CYCLE_ID, rowItem[CYCLE_ID].ToString());
            _dicAttrToolTip.Add(ORIGINALCYCLE_ID, rowItem[ORIGINALCYCLE_ID].ToString());
            _dicAttrToolTip.Add(MERCHANT_NUMBER, rowItem[MERCHANT_NUMBER].ToString());
            _dicAttrToolTip.Add("AssignmentID", _AssignmentID.ToString());
            _dicAttrToolTip.Add("ReportDate", _ReportDate.ToString());
            _dicAttrToolTip.Add("WorkStateID", workStateID.ToString());
            _dicAttrToolTip.Add("WorkingStatusMessage", rowItem["WorkingStatusMessage"].ToString());
            _dicAttrToolTip.Add("WorkingMerchantID", rowItem["WorkingMerchantID"].ToString());
            _dicAttrToolTip.Add("CurrentStatus", currentStatus.ToString());
            _dicAttrToolTip.Add("isCardView", "true");

            Control ctrlCard = cardViewColumnCtr.FindControl("workItemCV");
            AS.Controls.Global.ASMCFWorkContent lblWorkCard = ctrlCard as AS.Controls.Global.ASMCFWorkContent;
            lblWorkCard.DicAttrToolTip = _dicAttrToolTip;
            lblWorkCard.CssClass = "item-cv";
            lblWorkCard.DispositionTable = _dispositionTable;
            lblWorkCard.DispositionChecked = rowItem["DispositionIDs"].ToString();
        }

        //merchant name
        var merchantName = cardViewColumnCtr.FindControl("merchantName") as HtmlGenericControl;
        string urlText = rowItem["MerchantName"].ToString();
        var merchantNameColor = Color.Transparent.Name;
        if (int.Parse(rowItem["WorkStateID"].ToString()) == (int)WebSiteEnums.WorkStatus.Worked)
        {
            cardViewColumnCtr.CssClass += "row-checked";
            merchantNameColor = Color.LightPink.Name;
        }

        if (GeneralFuncsLib.HasRiskRptPermission(this.Page))
        {
            url = RiskGeneral.BuildMerchantHyperlinkInRisk(this.Page,
                rowItem[MERCHANT_NUMBER], true, MerchantIntruderQuery, urlText, true);
            url = string.Format("<a class=\"link\" href=\"#\" style=\"cursor:pointer; border-bottom: 2px {6} solid;\" onclick=\"MerchantNumberClick('{0}','{1}',\'{2}\','{3}',this); return false;\">", rowItem["MerchantNumber"], rowItem["TodayVolume"], _ReportDate.ToString(), _AssignmentID, rowItem["MerchantNumber"], rowItem["TodayVolume"], merchantNameColor) + urlText + "</a>";
        }
        RM_MCF_GeneralFuncsLib.AddHtml(merchantName, VeraCodeSolution.DoVeraCode(url));

        if (rowItem["Watch"].Equals(1))
        {
            RM_MCF_GeneralFuncsLib.AddHtml(merchantName, VeraCodeSolution.DoVeraCode(merchantName.InnerHtml + "(<span style=\"color:red\">*</span>)"));
            RM_MCF_GeneralFuncsLib.AddTooltip(merchantName, VeraCodeSolution.DoVeraCode(
                "(M)" + rowItem["MerchantNumber"].ToString().Trim() +
                (rowItem["ApprovalDate"] == DBNull.Value ? string.Empty : " - " + ((DateTime)rowItem["ApprovalDate"]).ToString(WebSiteConstants.DATE_FORMAT))
                ));
        }
        else if (rowItem["MultiWatch"].Equals(true))
        {
            RM_MCF_GeneralFuncsLib.AddHtml(merchantName, VeraCodeSolution.DoVeraCode(merchantName.InnerHtml + "(<span style=\"color:red\">*</span>)"));
            RM_MCF_GeneralFuncsLib.AddTooltip(merchantName, VeraCodeSolution.DoVeraCode(
                 rowItem["MerchantNumber"].ToString().Trim() +
                (rowItem["ApprovalDate"] == DBNull.Value ? string.Empty : " - " + ((DateTime)rowItem["ApprovalDate"]).ToString(WebSiteConstants.DATE_FORMAT))
                ));
        }
        else
        {
            RM_MCF_GeneralFuncsLib.AddTooltip(merchantName, VeraCodeSolution.DoVeraCode(
                 rowItem["MerchantNumber"].ToString().Trim() +
                (rowItem["ApprovalDate"] == DBNull.Value ? string.Empty : " - " + ((DateTime)rowItem["ApprovalDate"]).ToString(WebSiteConstants.DATE_FORMAT))
                ));
        }

        string assignmentTypeQueryString = _AssignmentType != WebSiteEnums.AssignmentType.DetectionQueue ? "&AssignmentType=" + (int)_AssignmentType : string.Empty;
        string queryStringImage = this.Page.BuildSecureQueryString(string.Format("MerchantNumber={0}&ReportDate={1}&AssignmentID={2}", rowItem[MERCHANT_NUMBER], _ReportDate, _AssignmentID, assignmentTypeQueryString));
        string urlImage = "rm_MCF_DQReasonModal.aspx?" + queryStringImage;
        urlImage = "<a class=\"image-link\" href=\"#\" style=\"cursor:pointer\" onclick=\"openPopupWindowOnMenu(this,'" + urlImage + "','DQMCFWindow2'); return false;\"><img src='" + ResolveUrl("~/") + "res/img/information.png' border='0' alt='" + RM_MCF_GeneralFuncsLib.GetResourceValue("DetectionQueueRainbowReport_ascx_PV").ToString() + "' /></a>";
        RM_MCF_GeneralFuncsLib.AddHtml(merchantName, VeraCodeSolution.DoVeraCode(merchantName.InnerHtml + " " + urlImage));

        //just for first row
        if (dataIndex == 0 && _isSorting)
        {
            AddSortIcon(cardViewColumnCtr);
            _isSorting = false;
        }

    }

    private void RequeueAllGridView(bool isGridView, GridHeaderItem headerItem)
    {
        HtmlInputCheckBox chkRequeueAllMerchant = null;
        if (isGridView)
            chkRequeueAllMerchant = headerItem["RQCheckbox"].FindControl("chkHeader") as HtmlInputCheckBox;
        bool isRequeueHeader = chkRequeueAllMerchant != null;

        bool isRequeueAll = RiskSessionManager.MCF_DQTemporaryBarometerReport.IsRequeueAll;
        string requeueAll = string.Format("RequeueAllMerchants(this, " + IsWQ.ToString().ToLower() + "," + (int)_FilterWorkingStatus + ")");
        if (isRequeueHeader)
        {
            chkRequeueAllMerchant.Disabled = !GeneralFuncsLib.ReadOnlyRiskManagementEnableStatus((SecurePage)Page);
            chkRequeueAllMerchant.Attributes.Add("onclick", requeueAll);
        }

        chkHeaderCV.Enabled = GeneralFuncsLib.ReadOnlyRiskManagementEnableStatus((SecurePage)Page);
        chkHeaderCV.Attributes.Add("onclick", requeueAll);
        if (isRequeueAll)
        {
            if (isRequeueHeader)
                chkRequeueAllMerchant.Checked = true;
            chkHeaderCV.Checked = true;
        }
        else
        {
            if (isRequeueHeader)
                chkRequeueAllMerchant.Checked = false;
            chkHeaderCV.Checked = false;
        }

        var isDisabled = RiskSessionManager.MCF_DQTemporaryBarometerReport.TotalAllMerchants == 0;
        if ((IsWQ || IsAQ) && !isDisabled && RiskSessionManager.MCF_DQTemporaryBarometerReport.TotalMerchantsOtherFilter == 0)
        {
            int totalAllMerchant = RiskSessionManager.MCF_DQTemporaryBarometerReport.TotalAllMerchants;
            int requeueMerchant = RiskSessionManager.MCF_DQTemporaryBarometerReport.TotalRequeueMerchants;
            if (requeueMerchant != 0)
            {
                if (requeueMerchant == totalAllMerchant)
                {
                    isDisabled = true;
                    RiskSessionManager.MCF_DQTemporaryBarometerReport.TotalAllMerchants = 0;
                    RiskSessionManager.MCF_DQTemporaryBarometerReport.TotalRequeueMerchants = 0;
                }
                else
                {
                    RiskSessionManager.MCF_DQTemporaryBarometerReport.TotalAllMerchants = totalAllMerchant - requeueMerchant;
                }
            }
        }

        if (isDisabled)
        {
            if (isRequeueHeader)
                chkRequeueAllMerchant.Disabled = true;
            chkHeaderCV.Enabled = false;
        }
    }

    private void InitGrid()
    {
        _dicRiskCustomizeColumn = RM_MCF_GeneralFuncsLib.GenGridAssignmentCustomizeColumns(uxReportGrid, _CustomViewId);
    }

    private void AddSortIcon(TableCell ctrl)
    {
        if (RiskSessionManager.MCF_BarometerReport == null || RiskSessionManager.MCF_BarometerReport.SortExpression == null) return;

        var sortingField = RiskSessionManager.MCF_BarometerReport.SortExpression;
        var sortingOrder = RiskSessionManager.MCF_BarometerReport.SortOrder;

        var ctrlId = "lbtSort_" + sortingField;
        var tempBtn = ctrl.FindControl(ctrlId);
        if (tempBtn != null)
        {
            var linkButtonCtrl = tempBtn as LinkButton;
            switch (RiskSessionManager.MCF_BarometerReport.SortOrder)
            {
                case DESCEND:
                    linkButtonCtrl.CssClass += " rgSortQueueDesc";
                    break;
                case ASCEND:
                    linkButtonCtrl.CssClass += " rgSortQueueAsc";
                    break;
                default:
                    linkButtonCtrl.CssClass.Replace("rgSortQueueDesc", string.Empty)
                                           .Replace("rgSortQueueAsc", string.Empty);
                    break;
            }
        }
    }

    private void AddSortIcon(LinkButton ctrl)
    {
        if (RiskSessionManager.MCF_BarometerReport == null || RiskSessionManager.MCF_BarometerReport.SortExpression == null) return;

        var sortingField = RiskSessionManager.MCF_BarometerReport.SortExpression;
        var sortingOrder = RiskSessionManager.MCF_BarometerReport.SortOrder;

        if (ctrl != null)
        {
            switch (RiskSessionManager.MCF_BarometerReport.SortOrder)
            {
                case DESCEND:
                    ctrl.CssClass += " rgSortQueueDesc";
                    break;
                case ASCEND:
                    ctrl.CssClass += " rgSortQueueAsc";
                    break;
                default:
                    ctrl.CssClass.Replace("rgSortQueueDesc", string.Empty)
                                           .Replace("rgSortQueueAsc", string.Empty);
                    break;
            }
        }
    }

    private void UpdateToolTip(string elementID, UpdatePanel panel)
    {
        HtmlGenericControl title = new HtmlGenericControl("<h6></h6>");
        title.InnerText = RM_MCF_GeneralFuncsLib.GetResourceValue(string.Format("{0}_Text", elementID));
        title.Attributes.Add("class", "rtTitle");
        panel.ContentTemplateContainer.Controls.Add(title);
        HtmlGenericControl text = new HtmlGenericControl("<span></span>");
        text.InnerText = RM_MCF_GeneralFuncsLib.GetResourceValue(string.Format("{0}_ToolTip", elementID));
        panel.ContentTemplateContainer.Controls.Add(text);
    }

    private void SetInfoForAssignmentList()
    {
        grdAssignment.AssignmentID = _AssignmentID;
        grdAssignment.ReportDate = _ReportDate;
        grdAssignment.ApplyFilterId = _ApplyFilterId;
    }

    private void CreateTemporaryWorkQueueSession()
    {
        RM_MCF_GeneralFuncsLib.CreateTemporaryWorkQueueSession(_ReportDate, _AssignmentID, _ApplyFilterId, WebSiteEnums.PAGE_CODE.DQ);
    }

    private void SetInfo()
    {
        SetInfoForAssignmentList();
        optWorked.Visible = optNotWorked.Visible = optWIPByMe.Visible = optWIPByOrthers.Visible = !IsDistinctQueue;
        uxPlaceHolderColorLegend.Visible = !IsDistinctQueue;
        optRequeued.Visible = GeneralFuncsLib.HasQueuingMechanismFeature;

        btnAddWorkQueue.OnClientClick = RM_MCF_GeneralFuncsLib.BuildUrlRequeue(true, _ReportDate, _AssignmentID, _FilterWorkingStatus, _ApplyFilterId, WebSiteEnums.PAGE_CODE.DQ);
        btnRemoveWorkQueue.OnClientClick = RM_MCF_GeneralFuncsLib.BuildUrlRequeue(false, _ReportDate, _AssignmentID, _FilterWorkingStatus, _ApplyFilterId, WebSiteEnums.PAGE_CODE.DQ);
    }

    private void SetBarometerReportInfo()
    {
        RiskSessionManager.MCF_BarometerReport = new DetectionQueue();
    }

    private DataTable GetAllDispostion()
    {
        FilterParameterCollection parameterList = new FilterParameterCollection();
        parameterList.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        return WebServices.RiskServices.GetReports("spa_RM_MCF_GetAllDispositionForReport", parameterList);
    }

    #endregion ---- Private Methods -----

    #region ---- Events Handle -----
    protected void uxReportGrid_Init(object sender, EventArgs e)
    {
        if (RiskSessionManager.MCF_BarometerReport != null)
            this.uxReportGrid.PageSize = RiskSessionManager.MCF_BarometerReport.PageSize;
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
        GridTableView tableView = e.Item.OwnerTableView;
        if (tableView.SortExpressions == null || tableView.SortExpressions.Count == 0)
        {
            RiskSessionManager.MCF_BarometerReport.SortExpression = string.Empty;
            RiskSessionManager.MCF_BarometerReport.SortOrder = GridSortOrder.None.ToString();
        }
        else
        {
            var currentSort = tableView.SortExpressions[0];
            RiskSessionManager.MCF_BarometerReport.SortExpression = currentSort.FieldName;
            RiskSessionManager.MCF_BarometerReport.SortOrder = currentSort.SortOrder.ToString();
        }
    }

    protected override void OnInit(EventArgs e)
    {
        InitGrid();
        base.OnInit(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        uxReportGrid.IsIntruder = true;
        uxReportGrid.IntruderSourceName = GeneralFuncsLib.GetRequestFileName() + uxReportGrid.ID;
        if (!IsPostBack)
        {
            SetBarometerReportInfo();
            optGrid.Checked = true;
            var key = SESSION_FILTERING_OPTIONS + "_" + HttpContext.Current.Session.SessionID;
            if (Cache[key] != null)
            {
                string[] parts = ((string)Cache[key]).Split(';');

                if (parts.Length == 2)
                {
                    uxReportGrid.AS_SortExpression = parts[1];
                }
            }
            CreateTemporaryWorkQueueSession();
            RiskSessionManager.MCF_DQTemporaryBarometerReport.TotalRequeueMerchants = 0;
        }
        SetInfo();
    }

    protected override void OnPreRender(EventArgs e)
    {
        //Hide/Show columns by selecting view mode
        SwitchView();

        uxExporterTop.VisibleExport = !(uxReportGrid.GetDataSource() == null || uxReportGrid.GetDataSource().Rows.Count == 0);

        base.OnPreRender(e);
    }

    protected void uxExport_OnNeedExportConfig(object sender, ExportConfig exportConfig)
    {
        bool isExportFullView = hddExportOption.Value == "true";
        exportConfig.ReportHeader = _Header;
        exportConfig.FileName = GeneralFuncsLib.FormatFileName(_Header);

        //call function gen column
        RM_MCF_GeneralFuncsLib.NeedExportGridConfig(uxReportGrid, isExportFullView, HasRQColumn, IsDistinctQueue);

        if (uxReportGrid.Columns.FindByUniqueNameSafe("CheckBoxColumn") != null)
            uxReportGrid.Columns.FindByUniqueNameSafe("CheckBoxColumn").Visible = false;
        if (uxReportGrid.Columns.FindByUniqueNameSafe("MerchantNumber") != null)
            uxReportGrid.Columns.FindByUniqueNameSafe("MerchantNumber").Visible = true;
        if (uxReportGrid.Columns.FindByUniqueNameSafe("RQCheckbox") != null)
            uxReportGrid.Columns.FindByUniqueNameSafe("RQCheckbox").Visible = false;
        if (uxReportGrid.Columns.FindByUniqueNameSafe("CardView") != null)
            uxReportGrid.Columns.FindByUniqueNameSafe("CardView").Visible = false;
        if (uxReportGrid.Columns.FindByUniqueNameSafe("Work") != null)
            uxReportGrid.Columns.FindByUniqueNameSafe("Work").Visible = false;
        if (uxReportGrid.Columns.FindByUniqueNameSafe("Requeued") != null)
            uxReportGrid.Columns.FindByUniqueNameSafe("Requeued").Visible = false;
        if (uxReportGrid.Columns.FindByUniqueNameSafe("CurrentStatusDesc") != null)
            uxReportGrid.Columns.FindByUniqueNameSafe("CurrentStatusDesc").Visible = !IsDistinctQueue;
        if (uxReportGrid.Columns.FindByUniqueNameSafe("IsRequeuedDesc") != null)
            uxReportGrid.Columns.FindByUniqueNameSafe("IsRequeuedDesc").Visible = HasRQColumn;
    }

    protected void uxReportGrid_DataSourceReady(object sender, EventArgs e)
    {
        if (RiskSessionManager.MCF_BarometerReport != null)
        {
            RiskSessionManager.MCF_BarometerReport.PageSize = uxReportGrid.PageSize;
            RiskSessionManager.MCF_BarometerReport.PageIndex = uxReportGrid.MasterTableView.CurrentPageIndex;
        }

        //use for requeque
        RiskSessionManager.MCF_DQTemporaryBarometerReport.TotalAlertMerchant = uxReportGrid.MasterTableView.VirtualItemCount;
        if (_FilterWorkingStatus == WebSiteEnums.MCF_MerchantWorkingStatus.All)
        {
            // Get fisrt time when load page
            RiskSessionManager.MCF_DQTemporaryBarometerReport.TotalAllMerchants = uxReportGrid.MasterTableView.VirtualItemCount;
        }
        else
        {
            RiskSessionManager.MCF_DQTemporaryBarometerReport.TotalMerchantsOtherFilter = uxReportGrid.MasterTableView.VirtualItemCount;
        }
    }

    protected void uxRpt_Grid_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            var dataobject = (RiskCustomizeColumn)e.Item.DataItem.GetType().GetProperty("Value").GetValue(e.Item.DataItem, null);
            var rowItem = (DataRow)e.Item.DataItem.GetType().GetProperty("rowItem").GetValue(e.Item.DataItem, null);
            var itemIndex = (int)e.Item.DataItem.GetType().GetProperty("ItemIndex").GetValue(e.Item.DataItem, null);

            if (dataobject != null)
            {
                var btn = e.Item.FindControl("lbtSort_Column") as LinkButton;
                var tgId = Guid.NewGuid().ToString();
                if (btn != null)
                {
                    btn.Text = string.Format("<span class='text-gray-light' id={0}>{1}</span>", tgId, RM_MCF_GeneralFuncsLib.GetResourceValue(string.Format("{0}_Text", dataobject.ReSourceKey)));
                    RM_MCF_GeneralFuncsLib.GenerateTooltipHeaderColumn(uxReportGrid, tgId, RM_MCF_GeneralFuncsLib.GetResourceValue(string.Format("{0}_Tooltip", dataobject.ReSourceKey)));
                    if (itemIndex == 0 && !string.IsNullOrEmpty(RiskSessionManager.MCF_BarometerReport.SortExpression)
                        && RiskSessionManager.MCF_BarometerReport.SortExpression.Equals(dataobject.Key))
                    {
                        AddSortIcon(btn);
                    }
                }
            }
            var control = e.Item.FindControl("column") as HtmlGenericControl;
            if (rowItem != null && control != null)
            {
                RM_MCF_GeneralFuncsLib.ItemRptDataBound_CardView(dataobject, rowItem, control, this.Page);
            }
        }

    }
    protected void uxRpt_Grid_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (optCard.Checked)
        {
            _isSorting = true;
            var fieldName = e.CommandArgument.ToString();
            SortHandle(fieldName);
            uxReportGrid.Rebind();
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        if (Page.IsIntruderDetected) return;

        switch ((DataBindAction)type)
        {
            case DataBindAction.BindRainbowGrid:
                {
                    //for requeue feature
                    if (_AssignmentID < 1)
                    {
                        uxReportGrid.DataSource = new DataTable();
                        return;
                    }

                    uxExporterTop.GridHeader = VeraCodeSolution.ValidateResponseData(_Header);
                    uxExporterTop.GridTitle = VeraCodeSolution.ValidateResponseData(_Header.Replace(":", string.Empty));
                    uxExporterTop.GridSubTitle = VeraCodeSolution.ValidateResponseData(_Header.Replace(Resources.LanguageResource.RiskEntities_Header_Assignment + " ", string.Empty));

                    if (AfterGridSort != null)
                        AfterGridSort(uxReportGrid.AS_SortExpression);

                    var key = SESSION_FILTERING_OPTIONS + "_" + HttpContext.Current.Session.SessionID;
                    Cache[key] = string.Format("{0};{1}",
                       uxReportGrid.MasterTableView.CurrentPageIndex, uxReportGrid.AS_SortExpression);

                    string spName = "spa_RM_MCF_Get_BarometerReport";
                    string spa = spName;
                    FilterParameterCollection parameterList = new FilterParameterCollection();
                    parameterList.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parameterList.Add(new FilterParameter("@AssignmentID", _AssignmentID, DbType.Int32));
                    parameterList.Add(new FilterParameter("@ReportDate", _ReportDate, DbType.Date));
                    parameterList.Add(new FilterParameter("@Mode", (int)_FilterWorkingStatus, DbType.Int32));
                    parameterList.Add(new FilterParameter("@LanguageID", SessionManager.CurrentLanguage, DbType.Int32));
                    parameterList.Add(new FilterParameter("@ApplyFilterId", _ApplyFilterId, DbType.String));

                    if (optCard.Checked)
                    {
                        parameterList.Add(new FilterParameter("@stOrder", !string.IsNullOrEmpty(orderByColumnName) ? orderByColumnName : _OrderByColumnName, DbType.String));
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(orderByColumnName))
                        {
                            parameterList.Add(new FilterParameter("@stOrder", _OrderByColumnName, DbType.String));
                        }
                    }

                    uxReportGrid.DataSourceInvoker = new ASFuncInvoker(WebServices.RiskServices, "GetReports", new object[] { spa, ReportServices.ConvertToFilterParamWSArray(parameterList) });
                    uxReportGrid.VisibleGrid(true);

                    //Get disposition list
                    if (_dispositionList != null)
                    {
                        _dispositionTable = _dispositionList;
                    }
                    else
                    {
                        _dispositionTable = GetAllDispostion();
                        _dispositionList = _dispositionTable;
                    }

                }
                break;
        }
    }

    protected void uxReportGrid_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.BindRainbowGrid, uxReportGrid);
    }

    protected void uxReportGrid_ItemDataBound(object sender, GridItemEventArgs e)
    {
        var count = 0;

        var checkBoxColumn = uxReportGrid.MasterTableView.Columns.FindByUniqueName("CheckBoxColumn");
        string headerTextCheckBoxColumn = GetLocalResourceObject("CheckBoxColumn.HeaderText").ToString();
        string checkBoxColumnId = GetLocalResourceObject("CheckBoxColumn" + ".HeaderText").ToString();
        if (!checkBoxColumn.HeaderText.Contains(string.Format("id='{0}'", checkBoxColumnId)))
        {
            string checkBoxColumnToolTip = GetLocalResourceObject("CheckBoxColumn" + ".HeaderTooltip").ToString();
            RM_MCF_GeneralFuncsLib.GenerateTooltipHeaderDefaulColumn(checkBoxColumn, headerTextCheckBoxColumn, uxReportGrid, checkBoxColumnId, checkBoxColumnToolTip);
        }

        var workedColumn = uxReportGrid.MasterTableView.Columns.FindByUniqueName("Worked");
        string headerTextWorkedColumn = GetLocalResourceObject("Worked.HeaderText").ToString();
        string workedColumnId = GetLocalResourceObject("Worked" + ".HeaderText").ToString();
        if (!workedColumn.HeaderText.Contains(string.Format("id='{0}'", workedColumnId)))
        {
            string workedColumnToolTip = RM_MCF_GeneralFuncsLib.GetResourceValue(string.Format("{0}_Tooltip", "Worked"));
            RM_MCF_GeneralFuncsLib.GenerateTooltipHeaderDefaulColumn(workedColumn, headerTextWorkedColumn, uxReportGrid, workedColumnId, workedColumnToolTip);
        }

        var parametersWorkedColumn = uxReportGrid.MasterTableView.Columns.FindByUniqueName("ParametersWorked");
        string headerTextParametersWorked = GetLocalResourceObject("ParametersWorked.HeaderText").ToString();
        string parametersWorkedId = GetLocalResourceObject("ParametersWorked" + ".HeaderText").ToString();
        if (!parametersWorkedColumn.HeaderText.Contains(string.Format("id='{0}'", parametersWorkedId)))
        {
            string parametersWorkedToolTip = RM_MCF_GeneralFuncsLib.GetResourceValue(string.Format("{0}_Tooltip", "ParametersWorked"));
            RM_MCF_GeneralFuncsLib.GenerateTooltipHeaderDefaulColumn(parametersWorkedColumn, headerTextParametersWorked, uxReportGrid, parametersWorkedId, parametersWorkedToolTip);
        }

        var merchantNameColumn = uxReportGrid.MasterTableView.Columns.FindByUniqueName("MerchantName");
        string headerTextMerchantName = GetLocalResourceObject("MerchantName.HeaderText").ToString();
        string merchantNameId = GetLocalResourceObject("MerchantName" + ".HeaderText").ToString();
        if (!merchantNameColumn.HeaderText.Contains(string.Format("id='{0}'", merchantNameId)))
        {
            string merchantNameToolTip = RM_MCF_GeneralFuncsLib.GetResourceValue(string.Format("{0}_Tooltip", "MerchantName"));
            RM_MCF_GeneralFuncsLib.GenerateTooltipHeaderDefaulColumn(merchantNameColumn, headerTextMerchantName, uxReportGrid, merchantNameId, merchantNameToolTip);
        }

        var RQCheckboxColumn = uxReportGrid.MasterTableView.Columns.FindByUniqueName("RQCheckbox");
        string headerTextRQCheckbox = GetLocalResourceObject("RQCheckbox.HeaderText").ToString();
        string RQCheckboxId = GetLocalResourceObject("RQCheckbox" + ".HeaderText").ToString();
        if (!RQCheckboxColumn.HeaderText.Contains(string.Format("id='{0}'", RQCheckboxId)))
        {
            string RQCheckboxToolTip = RM_MCF_GeneralFuncsLib.GetResourceValue(string.Format("{0}_Tooltip", "Requeued"));
            RM_MCF_GeneralFuncsLib.GenerateTooltipHeaderDefaulColumn(RQCheckboxColumn, headerTextRQCheckbox, uxReportGrid, RQCheckboxId, RQCheckboxToolTip);
        }

        if (e.Item is GridHeaderItem)
        {
            for (int i = 0; i < _dicRiskCustomizeColumn.Count; i++)
            {
                string targetControlId = _dicRiskCustomizeColumn.Keys.ElementAt(i);
                string toolTipContent = RM_MCF_GeneralFuncsLib.GetResourceValue(string.Format("{0}_Tooltip",
                    _dicRiskCustomizeColumn.Keys.ElementAt(i).ToString()));
                RM_MCF_GeneralFuncsLib.GenerateTooltipHeaderColumn(uxReportGrid, targetControlId, toolTipContent);
            }

            count++;

            if (optGrid.Checked)
            {

                GridHeaderItem headerItem = e.Item as GridHeaderItem;
                if (headerItem["RQCheckbox"].Visible)
                {
                    RequeueAllGridView(true, headerItem);
                }

            }
            else
            {
                if (optCard.Checked && HasRQColumn)
                {
                    RequeueAllGridView(false, null);
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
                ItemDataBound_GridView(dataItem, rowItem);
            }
            else
            {
                var data = _dicRiskCustomizeColumn.Select(x => new { x.Key, x.Value, rowItem = rowItem, ItemIndex = e.Item.ItemIndex }).ToList();
                var uxRpt_Grid = e.Item.FindControl("uxRpt_Grid") as Repeater;
                if (uxRpt_Grid != null)
                {
                    uxRpt_Grid.DataSource = data;
                    uxRpt_Grid.DataBind();
                }

                LinkButton lbtSort_Worked = e.Item.FindControl("lbtSort_Worked") as LinkButton;
                if (lbtSort_Worked != null)
                {
                    RM_MCF_GeneralFuncsLib.GenerateTooltipHeaderColumn(uxReportGrid, lbtSort_Worked.ClientID,
                       RM_MCF_GeneralFuncsLib.GetResourceValue(string.Format("{0}_Tooltip", "Worked")));
                }
                LinkButton lbtSort_ParametersWorked = e.Item.FindControl("lbtSort_ParametersWorked") as LinkButton;
                if (lbtSort_ParametersWorked != null)
                {
                    RM_MCF_GeneralFuncsLib.GenerateTooltipHeaderColumn(uxReportGrid, lbtSort_ParametersWorked.ClientID,
                         RM_MCF_GeneralFuncsLib.GetResourceValue(string.Format("{0}_Tooltip", "ParametersWorked")));
                }

                LinkButton lbtSort_MerchantName = e.Item.FindControl("lbtSort_MerchantName") as LinkButton;
                if (lbtSort_MerchantName != null)
                {
                    RM_MCF_GeneralFuncsLib.GenerateTooltipHeaderColumn(uxReportGrid, lbtSort_MerchantName.ClientID,
                        RM_MCF_GeneralFuncsLib.GetResourceValue(string.Format("{0}_Tooltip", "MerchantName")));
                }
                if (HasRQColumn)
                {
                    Label lbl_colRQColumn = e.Item.FindControl("lbl_colRQColumn") as Label;
                    RM_MCF_GeneralFuncsLib.GenerateTooltipHeaderColumn(uxReportGrid, lbl_colRQColumn.ClientID,
                           RM_MCF_GeneralFuncsLib.GetResourceValue(string.Format("{0}_Tooltip", "Requeued")));
                }
                ItemDataBound_CardView(dataItem, rowItem, e.Item.ItemIndex);
            }
        }
    }

    protected void uxReportGrid_PreRender(object sender, EventArgs e)
    {
        int frozenCol = HasRQColumn ? 5 : 4;
        frozenCol = IsDistinctQueue ? frozenCol - 1 : frozenCol;
        uxReportGrid.ClientSettings.Scrolling.FrozenColumnsCount = frozenCol;
    }

    protected void ChangeView(object sender, EventArgs e)
    {
        uxReportGrid.Rebind();
    }

    protected void uxRTM_Column_AjaxUpdate(object sender, ToolTipUpdateEventArgs e)
    {
        UpdateToolTip(e.Value, e.UpdatePanel);
    }

    protected void uxOpenWarning_Click(object sender, EventArgs e)
    {
        var message = Page.BuildSecureQueryString("Message=" + hddMessage.Value);
        ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript(string.Format(@"openWarning('" + message + "');"));
    }

    protected void btnFilterWorkingStatus_Click(object sender, EventArgs e)
    {
        uxReportGrid.MasterTableView.CurrentPageIndex = 0;
        uxReportGrid.Rebind();
    }

    protected void btnRefreshRainbow_Click(object sender, EventArgs e)
    {
        uxReportGrid.Rebind();
        SetInfoForAssignmentList();
        grdAssignment.Rebind();
    }

    protected void btnRebindReportGrid_Click(object sender, EventArgs e)
    {
        CreateTemporaryWorkQueueSession();
        uxReportGrid.Rebind();
    }

    protected void uxCustomView_SelectedIndexChange(object sender)
    {
        uxReportGrid.Rebind();
    }

    protected void uxRefreshBtn_Click(object sender, EventArgs e)
    {
        grdAssignment.AssignmentID = _AssignmentID;
        grdAssignment.ReportDate = _ReportDate;
        grdAssignment.ApplyFilterId = _ApplyFilterId;
        grdAssignment.Rebind();
    }
    #endregion ---- Events Handle -----
}