using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using AS.Common.DBManager;
using Telerik.Web.UI;
using System.IO;
using System.Text.RegularExpressions;
using AS.Common;
using System.Configuration;
using System.Text;
using AS.Controls.Pages;
using System.Web.Services;
using Newtonsoft.Json;
using System.Threading;
using AS.Security.WS.Entities;

[PagePermission("RskQueue,MSRskQueue")]
public partial class rm_MCF_DQNextQReportPopup : ReportPage
{
    #region ---- Variable & Enum ---
    #region Enums
    enum DataBindAction
    {
        BindGrid
    }

    #endregion

    #region Properties
    private const string MERCHANT_NAME = "MerchantName";
    private const string MERCHANT_NUMBER = "MerchantNumber";
    private const string VIEW_RSK_MERCHANT_WORKED = ",RskMerchantWorked,";
    private const string VIEW_MRSK_MERCHANT_WORKED = ",MSRskMerchantWorked,";
    private const string RECORD_ID = "RecordID";
    private const string WORKMERCHANT_ID = "WorkingMerchantID";
    private const string CYCLE_ID = "CycleID";
    private const string ORIGINALCYCLE_ID = "ParentCycleID";
    private Dictionary<string, string> _dicAttrToolTip = new Dictionary<string, string>();
    private static User _currentUser = new User();
    private static string _userMode = string.Empty;
    private static int _clientId;
    public static int LENGHT_OF_QUEUE
    {
        get
        {
            int len = 2;
            int.TryParse(ConfigurationManager.AppSettings["NextQueue_BufferMerchants"], out len);
            if (len < 2)
                return 2;
            else
                return len;
        }
    }

    protected int MerchantCount
    {
        get
        {
            var count = 0;
            if (SecureQueryString != null && SecureQueryString["MerchantCount"] != null)
                int.TryParse(SecureQueryString["MerchantCount"], out count);
            return count;
        }
    }

    protected bool isFromMerchantWorked
    {
        get
        {
            var isFrom = false;
            if (SecureQueryString != null && SecureQueryString["isFromMerchantWorked"] != null)
                bool.TryParse(SecureQueryString["isFromMerchantWorked"], out isFrom);
            return isFrom;
        }
    }

    protected DateTime ReportDate
    {
        get
        {
            var date = DateTime.Now;
            if (SecureQueryString != null && SecureQueryString["reportDate"] != null)
                DateTime.TryParse(SecureQueryString["reportDate"], out date);
            return date;
        }
    }

    protected int AssignmentId
    {
        get
        {
            var assignmentId = 0;
            if (SecureQueryString != null && SecureQueryString["assignmentId"] != null)
                int.TryParse(SecureQueryString["assignmentId"], out assignmentId);
            return assignmentId;
        }
    }

    protected string MerNumberFromMerWorked
    {
        get
        {
            if (!string.IsNullOrEmpty(SecureQueryString["MerchantNumber"]))
                return SecureQueryString["MerchantNumber"].ToString();
            return string.Empty;
        }
    }

    protected string MerchantNumber
    {
        get
        {
            return RiskSessionManager.MCF_CurrentNextQueue.MerchantNumber;
        }
    }

    protected string AssignmentName
    {
        get
        {
            if (SecureQueryString != null && SecureQueryString["assignmentName"] != null)
                return SecureQueryString["assignmentName"].ToString();
            else
                return string.Empty;
        }
    }

    private static int RequeueSessionID
    {
        get
        {
            return RiskSessionManager.MCF_DQTemporaryNextQueue.RequeueSessionID;
        }
        set { RiskSessionManager.MCF_DQTemporaryNextQueue.RequeueSessionID = value; }
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

    protected string ApplyFilterId
    {
        get
        {

            if (!string.IsNullOrEmpty(SecureQueryString["ApplyFilterId"]))
            {
                return Convert.ToString(SecureQueryString["ApplyFilterId"]);
            }
            else
            {
                return string.Empty;
            }
        }
    }

    protected string OrderBy
    {
        get
        {
            if (SecureQueryString != null && SecureQueryString["OrderBy"] != null)
                return SecureQueryString["OrderBy"].ToString();
            else
                return string.Empty;
        }
    }

    private string _Header { get { return string.Format("{0} ({1})", AssignmentName, ReportDate.ToString(WebSiteConstants.DATE_FORMAT)); } }

    #endregion

    #region Methods
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PageTitle1.ReportTitle = GetLocalResourceObject("PageResource1.Title").ToString();
            string userPermission = SessionManager.CurrentUserPermissions;

            if (userPermission.Contains(VIEW_RSK_MERCHANT_WORKED) || userPermission.Contains(VIEW_MRSK_MERCHANT_WORKED))
            {
                uxMerchantWorkedReport.Visible = true;
            }

            // Merchant from Merchant Worked Report page
            if (isFromMerchantWorked)
            {
                if (AssignmentId == 0)
                {
                    IsIntruderDetected = true;
                    IntruderLog.LogData4 += "&assignmentId=NULL";
                    RaiseIntruderEvent(IntruderType.PostData);
                    return;
                }
                else
                {
                    RiskSessionManager.MCF_CurrentNextQueue = NextQueueService.GetCurrentMerchantWorked(true, MerNumberFromMerWorked, AssignmentId, ReportDate);
                    if (ReportDate.Date == DateTime.Now.Date && (RiskSessionManager.MCF_RiskNextQueue_Cache == null || RiskSessionManager.MCF_RiskNextQueue_Cache.Count == 0))
                    {
                        NextQueueService.DoClearWebLoading(true);
                        var nextQueueCount = NextQueueService.DoGetNextQueueDetail(true, false, true, AssignmentId, ReportDate.ToString(), OrderBy, string.Empty).ToInt();
                        if (nextQueueCount != 0)
                        {
                            //RiskSessionManager.IsRMMCFNextQueueEnd = false;
                            NextQueueItemCollection lst = RiskSessionManager.MCF_RiskNextQueue_Cache;
                            lst.Add(RiskSessionManager.MCF_CurrentNextQueue);
                            RiskSessionManager.MCF_RiskNextQueue_Cache = lst;
                            NextQueueService.DoGetNextQueueDetail(false, false, true, AssignmentId, ReportDate.ToString(), OrderBy, ApplyFilterId, MerchantNumber);
                        }
                    }
                    else if ((RiskSessionManager.MCF_RiskNextQueue_Cache != null && RiskSessionManager.MCF_RiskNextQueue_Cache.Count > 0))
                    {
                        uxNext.Visible = true;
                    }

                }

                DisplayCurrentMerchantDetails();
            }
            else
            {
                DisplayNextMerchant();
            }

            SetInfoForAssignmentList();
            GenDispositionPopver();
            SetInfo();
        }
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
        PageTitle1.ReportTitle = GetLocalResourceObject("PageResource1.Title").ToString();
    }
    protected void uxNext_Click(object sender, EventArgs e)
    {
        uxNextQReport.VisibleGrid();

        //40706 - Check merchant is worked
        bool isWorked = true;

        for (int i = 0; RiskSessionManager.MCF_RiskNextQueue_Cache.Count > 0; i++)
        {
            var currentRMMCFNextQueue = RiskSessionManager.MCF_RiskNextQueue_Cache.Get(0, true);
            if (currentRMMCFNextQueue != null && currentRMMCFNextQueue.Barometer.HasData())
            {
                var parentCycleID = currentRMMCFNextQueue.Barometer.Rows[0]["ParentCycleID"];
                DataTable dt = GeneralFuncsLib.CheckMerchantIsWorked(ReportDate, true, currentRMMCFNextQueue.MerchantNumber, parentCycleID.ToString());
                if (!dt.HasData())
                {
                    DataTable currentBarometer = RiskSessionManager.MCF_CurrentNextQueue.Barometer;
                    RiskSessionManager.MCF_CurrentNextQueue = currentRMMCFNextQueue;
                    isWorked = false;
                    // get disposition and change status from wk to wip
                    GenDispositionPopver();

                    break;
                }
            }
        }

        if (RiskSessionManager.MCF_RiskNextQueue_Cache.Count <= 0 && isWorked)
        {
            //Take a chance to check if there are any merchants to continue working
            var result = NextQueueService.DoGetNextQueueDetail(false, true, true, AssignmentId, ReportDate.ToString(), OrderBy, ApplyFilterId, MerchantNumber);

            if (result == "end")
            {
                DisplayCurrentMerchantDetails();

                int notWorkedMerchantCount = NextQueueService.CheckNotWorkedMerchantCount(true, AssignmentId, ReportDate);

                // 42809
                if (notWorkedMerchantCount > 0)
                {
                    ShowMessageBox(GetLocalResourceObject("CheckNotWorkedMerchantCountGreaterThanZero.Text").ToString(), true);
                }
                else if (notWorkedMerchantCount < 0)
                {
                    ShowMessageBox(string.Format(GetLocalResourceObject("CheckNotWorkedMerchantCountLessThanZero.Text").ToString(), AssignmentName), true);
                }
                else
                {
                    ShowMessageBox(string.Format(GetLocalResourceObject("CheckNotWorkedMerchantCountEqualThanZero.Text").ToString(), AssignmentName), true);
                }
                return;
            }
            else if (result.Equals("error", StringComparison.OrdinalIgnoreCase))
            {
                //  [CLEARENT_Aperia] FW: Error message in Next Queue - Ticket # 46475
                // loop try to get next merchant --
                for (int i = 0; i < WebSiteSettings.NextQueue_ErrorTryGetLimited; i++)
                {
                    result = NextQueueService.DoGetNextQueueDetail(false, false, true, AssignmentId, ReportDate.ToString(), OrderBy, ApplyFilterId, MerchantNumber);
                    if (!result.Equals("error", StringComparison.OrdinalIgnoreCase))
                    {
                        break;
                    }
                }
                if (result.Equals("error", StringComparison.OrdinalIgnoreCase))
                {
                    ShowMessageBox(string.Format(GetLocalResourceObject("CheckNotWorkedMerchantCountEqualThanZero.Text").ToString(), AssignmentName));
                }
            }
            else
            {
                RiskSessionManager.MCF_CurrentNextQueue = RiskSessionManager.MCF_RiskNextQueue_Cache.Get(0, true);
            }
        }

        if (RiskSessionManager.MCF_CurrentNextQueue.Data != null)
        {
            DisplayNextMerchant();
        }
        else
        {
            string msg = string.Format(GetLocalResourceObject("CheckNotWorkedMerchantCountGreaterThanZero.Text").ToString(), AssignmentName);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "closemodal", "showNextQueueNodata(\"" + msg + "\"); ", true);
        }
    }

    private void ShowMessageBox(string message, bool closeWindow = false)
    {
        if (!closeWindow)
            message = String.Format("setTimeout(\"alert('{0}')\",500);", message).ToString();
        else
        {
            message = String.Format("setTimeout(\"alert('{0}');window.close(); \",500);", message).ToString();
            NextQueueService.DoClearWebLoading(true);
        }

        Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowAlerMessage", message, true);
    }

    #endregion
    #endregion ---- Variable & Enum ---

    #region ---- Private Methods ----

    void DisplayNextMerchant()
    {
        DisplayCurrentMerchantDetails();
        CreateTemporaryWorkQueueSession();
    }

    private void UpdateStateWork(DataTable dt, string workState, string currentWorkState, FilterParameterCollection parameters)
    {
        AS.Web.Business.ReportServices service = WebServices.RiskServices;
        string cycleid = dt.Rows[0]["CycleID"].ToString();
        string parentCycleID = dt.Rows[0]["ParentCycleID"].ToString();
        string merchantNumber = dt.Rows[0]["MerchantNumber"].ToString();
        string assignmentID = AssignmentId.ToString();
        string reportDate = ReportDate.ToString();

        service.AddRequestHeader("ClientId", _clientId.ToString());
        parameters.Add(new FilterParameter("@MerchantNumber", merchantNumber, DbType.String));
        parameters.Add(new FilterParameter("@ViewCode", "NQ", DbType.String));
        parameters.Add(new FilterParameter("@ReportDate", Convert.ToDateTime(reportDate), DbType.Date));
        parameters.Add(new FilterParameter("@AssignmentID", assignmentID, DbType.Int32));
        parameters.Add(new FilterParameter("@CycleID", int.Parse(cycleid), DbType.Int32));
        parameters.Add(new FilterParameter("@ParentCycleID", int.Parse(parentCycleID), DbType.Int32));
        parameters.Add(new FilterParameter("@WorkStateID", int.Parse(workState), DbType.Int32));
        parameters.Add(new FilterParameter("@FEWorkStateID", int.Parse(currentWorkState), DbType.Int32));
        service.GetReports("spa_RM_MCF_UpdateMerchantWorked", parameters);
    }

    private static void UpdateRequeuedMerchant(string applyFilteredId)
    {
        if (RiskSessionManager.MCF_CurrentNextQueue != null && RiskSessionManager.MCF_CurrentNextQueue.Barometer.HasData())
        {
            DataTable currentBarometer = RiskSessionManager.MCF_CurrentNextQueue.Barometer;
            string cycleID = currentBarometer.Rows[0]["CycleID"].ToString();
            string parentCycleID = currentBarometer.Rows[0]["ParentCycleID"].ToString();
            string merchantNumber = currentBarometer.Rows[0]["MerchantNumber"].ToString();

            RM_MCF_GeneralFuncsLib.UpdateRequeuedMerchant(RequeueSessionID, "true", merchantNumber, cycleID, parentCycleID, WebSiteEnums.PAGE_CODE.NQ, applyFilteredId);
        }
    }

    private void CreateTemporaryWorkQueueSession()
    {
        RM_MCF_GeneralFuncsLib.CreateTemporaryWorkQueueSession(ReportDate, AssignmentId, ApplyFilterId, WebSiteEnums.PAGE_CODE.NQ);
    }

    private void SetInfoForAssignmentList()
    {
        //grdAssignment.AssignmentID = AssignmentId;
        //grdAssignment.ReportDate = ReportDate;
        //grdAssignment.ApplyFilterId = ApplyFilterId;
    }

    private DataTable GetDipositionDiaLog(string reportDate)
    {
        var parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(false);
        parameters.Add(new FilterParameter("@ReportDate", Convert.ToDateTime(reportDate), DbType.Date));
        return WebServices.RiskServices.GetReports("spa_RM_MCF_GetDispositionForReport", parameters);
    }

    private void GenDispositionPopver()
    {
        var data = RiskSessionManager.MCF_CurrentNextQueue.Barometer;
        UpdateStateWorkAsync();

        if (data.HasData())
        {
            _dicAttrToolTip.Add(CYCLE_ID, data.Rows[0][CYCLE_ID].ToString());
            _dicAttrToolTip.Add(ORIGINALCYCLE_ID, data.Rows[0][ORIGINALCYCLE_ID].ToString());
            _dicAttrToolTip.Add(MERCHANT_NUMBER, data.Rows[0][MERCHANT_NUMBER].ToString());
            _dicAttrToolTip.Add("TodayVolume", data.Rows[0]["todayVolume"].ToString());
            _dicAttrToolTip.Add("FEWorkState", data.Rows[0]["WorkStateID"].ToString());
            _dicAttrToolTip.Add("data-state", data.Rows[0]["WorkStateID"].ToString());
            _dicAttrToolTip.Add("DispositionIDs", data.Rows[0]["DispositionIDs"].ToString());
        }

        _dicAttrToolTip.Add("AssignmentID", AssignmentId.ToString());
        _dicAttrToolTip.Add("ReportDate", ReportDate.ToString());
        _dicAttrToolTip.Add("WorkStateID", "0");
        _dicAttrToolTip.Add("onclick", "setDispositionAndNext(this);");

        foreach (var item in _dicAttrToolTip)
        {
            uxNext.Attributes.Add(item.Key, item.Value);
        }

        GenPoverDisposition();
    }

    private void UpdateStateWorkAsync()
    {
        if (RiskSessionManager.MCF_CurrentNextQueue != null && RiskSessionManager.MCF_CurrentNextQueue.Barometer != null)
        {
            DataTable data = RiskSessionManager.MCF_CurrentNextQueue.Barometer;
            if (data.Rows.Count > 0 && data.Columns.Contains("WorkStateID") && data.Rows[0]["WorkStateID"] != DBNull.Value)
            {
                int workStateId = int.Parse(data.Rows[0]["WorkStateID"].ToString());
                if (workStateId != 2)
                {
                    _currentUser = SessionManager.CurrentUser;
                    _userMode = GeneralFuncsLib.GetUserMode();
                    _clientId = SessionManager.CurrentClient;
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserParams(_currentUser, _userMode);
                    Thread thread = new Thread(() =>
                    {
                        UpdateStateWork(data, ((int)WebSiteEnums.WorkStatus.WorkInProgress).ToString(), ((int)WebSiteEnums.WorkStatus.Work).ToString(), parameters);
                    });

                    thread.Start();
                }
            }
        }
    }

    private void SetInfo()
    {
        btnAddWorkQueue.OnClientClick = RM_MCF_GeneralFuncsLib.BuildUrlRequeue(true, ReportDate, AssignmentId, WebSiteEnums.MCF_MerchantWorkingStatus.All, ApplyFilterId, WebSiteEnums.PAGE_CODE.NQ);
    }

    private void GenPoverDisposition()
    {
        DataTable dataDisposition = new DataTable();
        if (_dispositionList != null)
        {
            dataDisposition = _dispositionList.AsEnumerable().Where(x => x.Field<bool>("IsActive") == true).CopyToDataTable();
            if (!dataDisposition.Columns.Contains("Checked"))
            {
                dataDisposition.Columns.Add("Checked", typeof(Boolean));
            }

            List<string> dipositionCheckedList = _dicAttrToolTip["DispositionIDs"].ToString().Split(',').ToList();
            foreach (DataRow item in dataDisposition.Rows)
            {
                item["Checked"] = dipositionCheckedList.Any(x => x.Trim().Equals(item["DispositionID"].ToString().Trim()));
            }
        }
        else
        {
            dataDisposition = GetDipositionDiaLog(ReportDate.ToString());
            _dispositionList = dataDisposition;
        }
        uxDispositionPop.Text = RM_MCF_GeneralFuncsLib.GetDataWorkPopover("Disposition", int.Parse(_dicAttrToolTip["WorkStateID"].ToString()),
                dataDisposition, uxNext.ClientID, _dicAttrToolTip["MerchantNumber"].ToString(), true);
    }
    #endregion ---- Private Methods ----

    #region ---- Public Methods ----
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static void GetWorkQueueAssignment(string applyFilteredId)
    {
        UpdateRequeuedMerchant(applyFilteredId);
    }

    #region Web Methods

    /// <summary>
    /// Update user activity status
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public static void KeepSessionActive()
    {
        NextQueueService.KeepSessionActive(true);
    }

    [WebMethod(EnableSession = true)]
    public static string[] GetNQTransactions(int pageIndex)
    {
        UserControls_rm_MCF_DQNextQReport us = new UserControls_rm_MCF_DQNextQReport();
        return us.GetNQTransactions(pageIndex, WebSiteSettings.DefaultRiskPageSize);
    }

    [WebMethod(EnableSession = true)]
    public static string[] GetNQChargebacks(int pageIndex)
    {
        UserControls_rm_MCF_DQNextQReport us = new UserControls_rm_MCF_DQNextQReport();
        return us.GetNQChargebacks(pageIndex, WebSiteSettings.DefaultRiskPageSize);
    }

    #endregion
    #endregion ---- Public Methods ----

    #region ---- Event Handles ----

    protected void DisplayCurrentMerchantDetails()
    {
        uxPageTitle.ReportTitle = _Header;
        uxPageTitle.HasShowHierarchy = false;
        uxNextQReport.ReportDate = ReportDate;
        if (RiskSessionManager.MCF_CurrentNextQueue != null && RiskSessionManager.MCF_CurrentNextQueue.MerchantInfo.HasData())
        {
            lblMerchantName.Text = RiskSessionManager.MCF_CurrentNextQueue.MerchantInfo.Rows[0]["MerchantName"].ToString();
        }
        uxNextQReport.GetFlatReportInfo();
        CreateTemporaryWorkQueueSession();
    }


    protected bool CheckMerchantIsWorked(string merchantNumber)
    {
        FilterParameterCollection paras = new FilterParameterCollection();
        paras.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paras.Add(new FilterParameter("@ReportDate", ReportDate, DbType.DateTime));
        paras.Add(new FilterParameter("@MerchantNumber", merchantNumber, DbType.String));
        paras.Add(new FilterParameter("@IsWorked", false, DbType.Boolean, true));
        FilterParameterCollection parasOut = new FilterParameterCollection();
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_CheckMerchantWorked", paras, out parasOut);
        FilterParameter isWorked = parasOut.FindFilterParameterByName("@IsWorked", true);
        if (Convert.ToBoolean(isWorked.ParameterValue))
        {
            return true;
        }
        return false;
    }

    protected void ImageButtonExcel_Click(object sender, EventArgs e)
    {
        this.IsNoCache = false;
        uxNextQReport.IsExport = true;
        uxNextQReport.PrepareForExport();
    }

    protected void btnClickMerchantNumber_Click(object sender, EventArgs e)
    {
        string riskReportIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(this.ID, new string[] { "MerchantNumber" });
        string queryString = this.BuildSecureQueryString(string.Format("{0}={1}{2}",
            "MerchantNumber", MerchantNumber, riskReportIntruderQuery));
        string url = "rm_MCF_RiskReport.aspx?" + queryString;

        RadAjaxManager ajax = RadAjaxManager.GetCurrent(this.Page);
        ajax.ResponseScripts.Add("ReloadParent('" + url + "');");
    }

    protected void uxOpenWarning_Click(object sender, EventArgs e)
    {
        var message = BuildSecureQueryString("Message=" + hddMessage.Value);
        ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript(string.Format(@"openWarning('" + message + "');"));
    }

    protected void uxRefreshBtn_Click(object sender, EventArgs e)
    {
        SetInfoForAssignmentList();
        //grdAssignment.Rebind();
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static void SetDispositionAndNextAsync(string dispositionList, string workStateID, string feWorkStateID, string cycleID, string ParentCycleID, string reportDate,
       string assignmentID, string merchantNumber, string viewCode)
    {
        _currentUser = SessionManager.CurrentUser;
        _userMode = GeneralFuncsLib.GetUserMode();
        _clientId = SessionManager.CurrentClient;
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(_currentUser, _userMode);
        AspxTracking requestTracking = new AspxTracking()
        {
            LogWebServerDts = DateTime.Now,
            LogData1 = "risk_MCF\\rm_MCF_DQNextQReportPopup.aspx\\UpdateDisposition",
            LogData2 = merchantNumber,
            LogData3 = assignmentID,
            LogData4 = viewCode
        };

        Thread thread = new Thread(() =>
        {
            SetDispositionAndNext(dispositionList, workStateID, feWorkStateID, cycleID, ParentCycleID, reportDate, assignmentID, merchantNumber, viewCode, parameters);
        });
        thread.Start();

        while (thread.IsAlive)
        {
            Thread.Sleep(10);
        }
        GeneralFuncsLib.WriteRequestLog(requestTracking);

    }

    public static void SetDispositionAndNext(string dispositionList, string workStateID, string feWorkStateID, string cycleID, string ParentCycleID, string reportDate,
       string assignmentID, string merchantNumber, string viewCode, FilterParameterCollection parameters)
    {

        AS.Web.Business.ReportServices service = WebServices.RiskServices;
        service.AddRequestHeader("ClientId", _clientId.ToString());
        parameters.Add(new FilterParameter("@MerchantNumber", merchantNumber, DbType.String));
        parameters.Add(new FilterParameter("@ViewCode", viewCode, DbType.String));
        if (!string.IsNullOrEmpty(dispositionList))
        {
            parameters.Add(new FilterParameter("@DispositionList", dispositionList, DbType.String));
        }
        parameters.Add(new FilterParameter("@ReportDate", Convert.ToDateTime(reportDate), DbType.Date));
        parameters.Add(new FilterParameter("@AssignmentID", assignmentID, DbType.Int32));
        parameters.Add(new FilterParameter("@CycleID", int.Parse(cycleID), DbType.Int32));
        parameters.Add(new FilterParameter("@ParentCycleID", int.Parse(ParentCycleID), DbType.Int32));
        parameters.Add(new FilterParameter("@WorkStateID", int.Parse(workStateID), DbType.Int32));
        parameters.Add(new FilterParameter("@FEWorkStateID", int.Parse(feWorkStateID), DbType.Int32));
        service.GetReports("spa_RM_MCF_UpdateMerchantWorked", parameters);
    }


    #endregion ---- Event Handles ----
}
