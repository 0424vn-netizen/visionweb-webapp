using AS.Common.DBManager;
using AS.Controls.Pages;
using AS.Security.WS.Entities.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;
using Telerik.Web.UI;
using Unit = System.Web.UI.WebControls.Unit;

[PagePermission("RskQueue,MSRskQueue")]
public partial class rm_MCF_DetectionQueue : ReportPage
{
    #region Constants

    public const string ASSIGNMENT_TYPE = "AssignmentType";
    private const string SPA_GET_WQ_ASSIGNMENT = "spa_RM_MCF_wq_GetWorkQueueAssignmentList";

    #endregion Constants

    enum DataBindAction
    {
        BindAssignmentList,
        BindOrderBy,
        BindAssignmentGrid,
    }

    enum PostBackAction
    {
        ChangeAllOrMyAssignmentEvent,
        SearchDetectionQueueEvent,
    }

    public class ItemOrder
    {
        public string DataValue { get; set; }
        public string DataText { get; set; }
    }

    #region Variables

    /// <summary>
    /// All Assignment => ALL
    /// My Assignment => USER
    /// </summary>
    private WebSiteEnums.AssignmentFilterTypes _AssignmentOption
    {
        get
        {
            if (RiskSessionManager.DetectionQueue != null & !IsPostBack)
            {
                optAllMerchantAssignments.Checked = RiskSessionManager.DetectionQueue.AssignmentOption == WebSiteEnums.AssignmentFilterTypes.All;
                optMyAssignments.Checked = !optAllMerchantAssignments.Checked;
            }
            if (optAllMerchantAssignments.Checked)
                return WebSiteEnums.AssignmentFilterTypes.All;
            else
                return WebSiteEnums.AssignmentFilterTypes.User;
        }
        set
        {
            optAllMerchantAssignments.Checked = (string.Compare(value.ToString(), WebSiteEnums.AssignmentFilterTypes.All.ToString()) == 0);
            optMyAssignments.Checked = !optAllMerchantAssignments.Checked;
        }
    }

    /// <summary>
    /// If users are 'DDSAdmin', 'NPCAdmin', 'RiskAdmin' then SHOW Assignment List
    /// Else HIDE assignment list
    /// </summary>
    private void ShowAllOrMyAssignments()
    {
        cidAssignmentOption.Visible = IsUserWithPermission("RskAllAss") | IsUserWithPermission("MSRskAllAss");
    }

    /// <summary>
    /// Gets the assignment ID.
    /// </summary>
    /// <value>The assignment ID.</value>
    private int _AssignmentID
    {
        get
        {
            if (this.optMyAssignments.Checked && string.IsNullOrEmpty(this.uxAssignmentList.SelectedValue))
            {
                LogHepler.WriteLogWarn("DetectionQueue _AssignmentID is invalid", "uxAssignmentList does not selected", string.Empty);
                return -1;
            }

            int assignmentID = -1;
            int.TryParse(this.uxAssignmentList.SelectedValue, out assignmentID);
            RiskSessionManager.MCF_DQ_CurrentAssignmentID = assignmentID;

            if (assignmentID <= 0)
            {
                LogHepler.WriteLogWarn("DetectionQueue _AssignmentID is invalid", "uxAssignmentList selected value is " + this.uxAssignmentList.SelectedValue, string.Empty);
            }

            return assignmentID;
        }
        set
        {
            RadComboBoxItem selectedItem = this.uxAssignmentList.FindItemByValue(value.ToString());
            if (selectedItem != null)
                this.uxAssignmentList.FindItemByValue(value.ToString()).Selected = true;
            else if (this.uxAssignmentList.Items.Count > 0)
                this.uxAssignmentList.Items[0].Selected = true;
        }
    }

    public WebSiteEnums.AssignmentType _AssignmentType
    {
        get
        {
            if (this.optMyAssignments.Checked && string.IsNullOrEmpty(this.uxAssignmentList.SelectedValue))
            {
                return WebSiteEnums.AssignmentType.DetectionQueue;
            }
            int assignmentType = 0;
            if (this.uxAssignmentList.SelectedItem != null)
                int.TryParse(this.uxAssignmentList.SelectedItem.Attributes[ASSIGNMENT_TYPE].ToString(), out assignmentType);
            return (WebSiteEnums.AssignmentType)assignmentType;
        }

    }
    /// <summary>
    /// Get Assignment Name
    /// </summary>
    private string _AssignmentName
    {
        get
        {
            return (uxAssignmentList.SelectedItem == null ? uxAssignmentList.Text : uxAssignmentList.SelectedItem.Text);
        }
    }

    /// <summary>
    /// Get, Set the date time.
    /// </summary>
    /// <value>The date time.</value>
    public DateTime _ReportDate
    {
        get
        {
            return this.uxReportDate.SelectedDate != null && this.uxReportDate.SelectedDate.HasValue ?
                this.uxReportDate.SelectedDate.Value : DateTime.Now;
        }
        set
        {
            uxReportDate.SelectedDate = value;
        }
    }

    /// <summary>
    /// Get, Set Order By
    /// </summary>
    private string _OrderBy
    {
        get
        {
            string orderBy = string.Empty;
            if (uxOrderBy1.SelectedValue != string.Empty)
            {
                orderBy = string.Format("[{0}]", uxOrderBy1.SelectedValue);
                if (optAsc1.Checked)
                    orderBy += " ASC";
                else
                    orderBy += " DESC";
            }
            if (uxOrderBy2.SelectedValue != string.Empty)
            {
                if (!string.IsNullOrEmpty(orderBy))
                    orderBy += ", " + string.Format("[{0}]", uxOrderBy2.SelectedValue);
                else orderBy = uxOrderBy2.SelectedValue;
                if (optAsc2.Checked)
                    orderBy += " ASC";
                else
                    orderBy += " DESC";
            }
            return orderBy;
        }
    }

    private string _ApplyFilterId
    {
        get
        {
            return hddApplyFilterId.Value;
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

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        IsBindDataOnLoad = true;
        if (!IsPostBack)
        {
            //check permission to select all assignment
            ShowAllOrMyAssignments();
            OnDataBindControls(DataBindAction.BindAssignmentList, uxAssignmentList);
            OnDataBindControls(DataBindAction.BindOrderBy);
            GetDetectionQueue();
        }

        // 38605: Advanced Filter
        uxAdvancedFilter.FilterPage = FilterPageEnums.DetectionQueue;
        if (IsPostBack)
        {
            var eventtarget = this.Request.Form["__EVENTTARGET"];
            if (!(!string.IsNullOrEmpty(eventtarget) && (eventtarget.EndsWith(btnRefresh.ID)
                || eventtarget.EndsWith(uxbtn_NextQueue.ID)
                || eventtarget.EndsWith(btnChangeAllOrMy.ID)
                || eventtarget.EndsWith("btnViewAssignment")
                || eventtarget.EndsWith(btnRefreshAssignmentGrid.ID)
                )))
            {
                hddApplyFilterId.Value = string.Empty;
                uxAdvancedFilter.IsBindData = true;
            }
            _dispositionList = GetAllDispostion();
        }
        // End - 38605
    }

    /// <summary>
    /// Search detection queue
    /// </summary>
    private void SearchDetectionQueue(bool keepRainBowPageIndex, bool isFirstTimeLoading)
    {
        SetDetectionQueue(isFirstTimeLoading);

        // Grid AsssignmentList
        BindGridAssignmentList();

    }

    private DataTable GetAllDispostion()
    {
        FilterParameterCollection parameterList = new FilterParameterCollection();
        parameterList.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        return WebServices.RiskServices.GetReports("spa_RM_MCF_GetAllDispositionForReport", parameterList);
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        if (this.IsIntruderDetected) return;
        FilterParameterCollection parameters = new FilterParameterCollection();

        switch ((DataBindAction)type)
        {
            case DataBindAction.BindAssignmentList:
                {
                    RadComboBox cbx = (RadComboBox)sender;

                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parameters.Add(new FilterParameter("@FilterType", this._AssignmentOption.ToString(), DbType.AnsiString));
                    parameters.Add(new FilterParameter("@FilterValue", string.Empty, DbType.AnsiString));
                    parameters.Add(new FilterParameter("@IsReskinSite", 1, DbType.Int32));
                    DataTable info = WebServices.RiskServices.GetReports("spa_RM_MCF_GetInvestigatedAssignmentsByUserID", parameters);

                    cbx.DataSource = info;
                    cbx.DataTextField = "AssignmentName";
                    cbx.DataValueField = "AssignmentID";
                    cbx.DataBind();
                }
                break;
            case DataBindAction.BindOrderBy:
                {
                    string path = "~/App_Data/Risk/DetectionQueueOrderColumn.xml";

                    XmlDocument doc = new XmlDocument();
                    doc.Load(MapPath(path));
                    XmlNodeList list = doc.SelectNodes("//Items/Item");
                    List<ItemOrder> colList = new List<ItemOrder>();
                    string itemSelected = string.Empty;
                    foreach (XmlNode node in list)
                    {
                        string value = node.Attributes["Value"].Value;
                        string text = RM_MCF_GeneralFuncsLib.GetResourceValue(value + "_Text");
                        colList.Add(new ItemOrder { DataText = text, DataValue = value });
                        if (node.Attributes["Selected"] != null)
                        {
                            itemSelected = value;
                        }
                    }

                    colList = colList.OrderBy(x => x.DataText).ToList();

                    uxOrderBy1.DataSource = colList;
                    uxOrderBy1.DataTextField = "DataText";
                    uxOrderBy1.DataValueField = "DataValue";
                    uxOrderBy1.DataBind();

                    uxOrderBy2.DataSource = colList;
                    uxOrderBy2.DataTextField = "DataText";
                    uxOrderBy2.DataValueField = "DataValue";
                    uxOrderBy2.DataBind();


                    RadComboBoxItem emptyItem1 = new RadComboBoxItem(string.Empty, string.Empty);
                    emptyItem1.Height = Unit.Pixel(13);
                    uxOrderBy1.Items.Insert(0, emptyItem1);
                    uxOrderBy1.SelectedValue = itemSelected;

                    RadComboBoxItem emptyItem2 = new RadComboBoxItem(string.Empty, string.Empty);
                    emptyItem2.Height = Unit.Pixel(13);
                    emptyItem2.Selected = true;
                    uxOrderBy2.Items.Insert(0, emptyItem2);
                }
                break;
            case DataBindAction.BindAssignmentGrid:
                {
                    grdAssignment.AssignmentID = _AssignmentID;
                    grdAssignment.ReportDate = _ReportDate;
                    grdAssignment.ApplyFilterId = _ApplyFilterId;
                    grdAssignment.NoDataMessage = Resources.LanguageResource.AS_ReportFilterJS_Msg_Date_24Month;
                    grdAssignment.Rebind();
                }
                break;
        }
    }

    protected override void OnPostBackActions(Enum type, object param)
    {
        if (this.IsIntruderDetected) return;

        switch ((PostBackAction)type)
        {
            case PostBackAction.ChangeAllOrMyAssignmentEvent:
                {
                    OnDataBindControls(DataBindAction.BindAssignmentList, param);
                }
                break;
            case PostBackAction.SearchDetectionQueueEvent:
                {
                    //search detection queue
                    SearchDetectionQueue(false, false);
                }
                break;
        }
    }

    protected void uxAssignmentList_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
    {
        DataRowView dataView = e.Item.DataItem as DataRowView;
        e.Item.Attributes.Add("AssignmentType", dataView["AssignmentType"].ToString());
    }

    /// <summary>
    /// Raise event change between options: All Assignments & My Assignments
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnChangeAllOrMy_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.ChangeAllOrMyAssignmentEvent, uxAssignmentList);
    }

    /// <summary>
    /// Raise event for submitting detection queue
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void uxSearchDetectionQueue_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.SearchDetectionQueueEvent);
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        BindGridAssignmentList();
        BindUrlForButton();
    }

    #region Private Method

    /// <summary>
    /// Get Detection Queue Filter from session
    /// </summary>
    private void GetDetectionQueue()
    {
        uxReportDate.SelectedDate = uxReportDate.MaxDate = DateTime.Today;
        if (RiskSessionManager.DetectionQueue != null)
        {
            //Set Assignment Type: All Assignment or My Assignment
            _AssignmentOption = RiskSessionManager.DetectionQueue.AssignmentOption;
            //Set AssignmentID for dropdownlist
            _AssignmentID = RiskSessionManager.DetectionQueue.AssignmentID;
            //Set report date
            _ReportDate = RiskSessionManager.DetectionQueue.ReportDate;

            //Search detection queue
            BindOrderByFromSession();
            SearchDetectionQueue(true, true);
        }
    }

    /// <summary>
    /// Set detection queue filter to session
    /// </summary>
    private void SetDetectionQueue(bool isFirstTimeLoading)
    {
        advFilter.Visible = true;
        pnlgroupButton.Visible = true;

        BindUrlForButton();

        switch (_AssignmentType)
        {
            case WebSiteEnums.AssignmentType.DetectionQueue:
            case WebSiteEnums.AssignmentType.WorkQueue:
            case WebSiteEnums.AssignmentType.Subsite:
                uxbtn_BarometerReport.Visible = uxbtn_SecurityReport.Visible = true;
                uxbtn_NextQueue.Visible = GeneralFuncsLib.GetDataOfExtendedSetting("NEXTQUEUE_REPORT") == "true";
                break;
            case WebSiteEnums.AssignmentType.DetectionQueueDistinct:
            case WebSiteEnums.AssignmentType.AggregateQueue:
                uxbtn_BarometerReport.Visible = true;
                uxbtn_NextQueue.Visible = false;
                uxbtn_SecurityReport.Visible = false;
                break;
        }
    }

    private void BindGridAssignmentList()
    {
        grdAssignment.Visible = true;
        OnDataBindControls(DataBindAction.BindAssignmentGrid);
    }

    private void ShowMessageBox(string message)
    {
        message = String.Format("alert('{0}');", message).ToString();

        ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript(message);
        // Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowAlerMessage", message, true);
    }

    private void BindOrderByFromSession()
    {
        string[] arrOrderBy = RiskSessionManager.DetectionQueue.OrderBy.Split(',');
        string orderBy1 = arrOrderBy[0];
        if (orderBy1.Contains("ASC"))
        {
            optAsc1.Checked = true;
        }
        else
        {
            optDesc1.Checked = true;
        }
        orderBy1 = orderBy1.Replace("DESC", "");
        orderBy1 = orderBy1.Replace("ASC", "");
        orderBy1 = orderBy1.Trim();
        uxOrderBy1.SelectedValue = orderBy1;

        if (arrOrderBy.Length > 1)
        {
            string orderBy2 = arrOrderBy[1];
            if (orderBy2.Contains("ASC"))
            {
                optAsc2.Checked = true;
            }
            else
            {
                optDesc2.Checked = true;
            }
            orderBy2 = orderBy2.Replace("DESC", "");
            orderBy2 = orderBy2.Replace("ASC", "");
            orderBy2 = orderBy2.Trim();
            uxOrderBy2.SelectedValue = orderBy2;
        }
        else
        {
            optDesc2.Checked = true;
        }
    }

    private void BindUrlForButton()
    {
        var url = string.Format("AssignmentID={0}&AssignmentType={1}&AssignmentName={2}&ReportDate={3}&OrderBy={4}&ApplyFilterId={5}",
                                   _AssignmentID, _AssignmentType, HttpUtility.UrlEncode(_AssignmentName), _ReportDate, _OrderBy, _ApplyFilterId);

        uxbtn_BarometerReport.OnClientClick = "openPopupWindow('" + string.Format("rm_MCF_DQBarometerReportPopup.aspx?{0}",
            this.BuildSecureQueryString(url)) + "','DQMCFWindow'); return false;";

        uxbtn_SecurityReport.OnClientClick = "openPopupWindow('" + string.Format("rm_MCF_DQSecurityReportPopup.aspx?{0}",
            this.BuildSecureQueryString(url)) + "','DQMCFWindow3'); return false;";
    }

    #endregion
    protected void uxbtn_NextQueue_Click(object sender, EventArgs e)
    {
        // Comment out for passed 

        if (_ReportDate != DateTime.Today)
        {
            ShowMessageBox(GetLocalResourceObject("rm_DetectionQueue_aspx_cs_MessageBox").ToString());
            return;
        }

        NextQueueService.DoClearWebLoading(true);
        var nextQueueCount = NextQueueService.DoGetNextQueueDetail(true, false, true, _AssignmentID, _ReportDate.ToString()
            , _OrderBy, _ApplyFilterId, string.Empty).ToInt();
        if (nextQueueCount == 0)
        {
            int notWorkedMerchantCount = NextQueueService.CheckNotWorkedMerchantCount(true, _AssignmentID, _ReportDate);
            if (notWorkedMerchantCount > 0)
            {
                ShowMessageBox(GetLocalResourceObject("CheckNotWorkedMerchantCountGreaterThanZero.Text").ToString());
            }
            else if (notWorkedMerchantCount < 0)
            {
                ShowMessageBox(string.Format(GetLocalResourceObject("CheckNotWorkedMerchantCountLessThanZero.Text").ToString(), _AssignmentName));
            }
            else
            {
                ShowMessageBox(string.Format(GetLocalResourceObject("CheckNotWorkedMerchantCountEqualThanZero.Text").ToString(), _AssignmentName));
            }
        }
        else
        {
            string result = NextQueueService.DoGetNextQueueDetail(false, true, true, _AssignmentID, _ReportDate.ToString(), _OrderBy, _ApplyFilterId, string.Empty);
            //  [CLEARENT_Aperia] FW: Error message in Next Queue - Ticket # 46475
            // loop try to get next merchant --
            if (result.Equals("error", StringComparison.OrdinalIgnoreCase))
            {
                for (int i = 0; i < WebSiteSettings.NextQueue_ErrorTryGetLimited; i++)
                {
                    result = NextQueueService.DoGetNextQueueDetail(false, true, true, _AssignmentID, _ReportDate.ToString(), _OrderBy, _ApplyFilterId, string.Empty);
                    if (!result.Equals("error", StringComparison.OrdinalIgnoreCase))
                    {
                        break;
                    }
                }
            }
            if (!result.Equals("error", StringComparison.OrdinalIgnoreCase))
            {
                RiskSessionManager.MCF_CurrentNextQueue = RiskSessionManager.MCF_RiskNextQueue_Cache.Get(0, true);
                // Check has detail data  => show popup
                if (RiskSessionManager.MCF_CurrentNextQueue.Data != null)
                {
                    var url = string.Format("AssignmentID={0}&AssignmentType={1}&AssignmentName={2}&ReportDate={3}&OrderBy={4}&ApplyFilterId={5}&MerchantCount={6}",
                           _AssignmentID, _AssignmentType, HttpUtility.UrlEncode(_AssignmentName), _ReportDate, _OrderBy, _ApplyFilterId, nextQueueCount);

                    string encodeURL = string.Format("rm_MCF_DQNextQReportPopup.aspx?{0}", this.BuildSecureQueryString(url));

                    ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript("openNextQueue('" + encodeURL + "');");

                }
                else
                {
                    ShowMessageBox(string.Format(GetLocalResourceObject("CheckNotWorkedMerchantCountGreaterThanZero.Text").ToString(), _AssignmentName));
                }
            }
            else
            {
                ShowMessageBox(string.Format(GetLocalResourceObject("CheckNotWorkedMerchantCountGreaterThanZero.Text").ToString(), _AssignmentName));
            }
        }
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string CheckDisposition(string Id, string reportDate)
    {
        return RM_MCF_GeneralFuncsLib.CheckDisposition(Id, reportDate);
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string UpdateDisposition(string dispositionList, string workStateID, string feWorkStateID, string cycleID, string ParentCycleID, string reportDate,
        string assignmentID, string merchantNumber, string viewCode)
    {
        return RM_MCF_GeneralFuncsLib.UpdateDisposition(dispositionList, workStateID, feWorkStateID, cycleID,
            ParentCycleID, reportDate, assignmentID, merchantNumber, viewCode);
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string GetAssignmentsForDetectionQueue(string assignmentID, string reportDate, string applyFilterId)
    {
        return RM_MCF_GeneralFuncsLib.GetAssignmentsForDetectionQueue(assignmentID, reportDate, applyFilterId);
    }

    protected void btnRefreshAssignmentGrid_Click(object sender, EventArgs e)
    {
        OnDataBindControls(DataBindAction.BindAssignmentGrid);
    }
}
