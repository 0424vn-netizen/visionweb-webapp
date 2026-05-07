using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Text;
using AS.Common;
using AS.Web.UI.Controls;
using System.Data;
using AS.Common.DBManager;
using System.Text.RegularExpressions;
using AS.Controls.Pages;

public partial class UserControls_rm_MCF_MerchantAlertHistory_ReportFilter : GlobalUserControl
{
    private const string CATEGORY = "Category";
    private const string CATEGORY_CODE = "CategoryCode";
    private const string SUB_CATEGORY_CODE = "SubCategoryCode";
    private const string SPA_NAME = "SPAName";
    private const string DATE_CRITERIA = "DateCriteria";
    private const string MERCHANT_CRITERIA = "MerchantCriteria";
    private const string LIMIT_REPORT_DAY_MERCHANT_ALERT_HISTORY = "LIMIT_REPORT_DAY_MERCHANT_ALERT_HISTORY";

    public event EventHandler SubmitFiltering;
    public delegate void EventHandler(object sender, MerchantAlertReportEntities merchantAlertReportEntities);
    #region Enums

    enum DataBindAction
    {
        BindAgentList,
        BindGroupList,
        BindAssignmentList,
        BindParameterList,
        BindUserNameList,
        BindDispositionList
    }

    enum PostBackAction
    {
        DoFilterAction,
        DoRefreshAction,
    }

    #endregion

    #region Field
    private HierarchyFilterValue reportFilter = null;
    #endregion

    #region Properties

    public string AssignmentFilterValue
    {
        get
        {
            return ViewState["AssignmentFilterValue"] == null ? string.Empty : ViewState["AssignmentFilterValue"].ToString();
        }
        set
        {
            ViewState["AssignmentFilterValue"] = value;
        }
    }
    public string ParameterFilterValue
    {
        get
        {
            return ViewState["ParameterFilterValue"] == null ? string.Empty : ViewState["ParameterFilterValue"].ToString();
        }
        set
        {
            ViewState["ParameterFilterValue"] = value;
        }
    }
    public string MerchantNumber
    {
        get
        {
            return ViewState["MerchantNumber"] == null ? string.Empty : ViewState["MerchantNumber"].ToString();
        }
        set
        {
            ViewState["MerchantNumber"] = value;
        }
    }
    public string MerchantName
    {
        get
        {
            return ViewState["MerchantName"] == null ? string.Empty : ViewState["MerchantName"].ToString();
        }
        set
        {
            ViewState["MerchantName"] = value;
        }
    }
    public string UserNameFilterValue
    {
        get
        {
            return ViewState["UserNameFilterValue"] == null ? string.Empty : ViewState["UserNameFilterValue"].ToString();
        }
        set
        {
            ViewState["UserNameFilterValue"] = value;
        }
    }
    public string DispositionFilterValue
    {
        get
        {
            return ViewState["DispositionFilterValue"] == null ? string.Empty : ViewState["DispositionFilterValue"].ToString();
        }
        set
        {
            ViewState["DispositionFilterValue"] = value;
        }
    }
    public bool HasMerchantFilter
    {
        get
        {
            return !(uxMerchantNumber.Text == string.Empty && uxMerchantName.Text == string.Empty);
        }
    }

    public int LimitReportDayMerchanAlertHistory
    {
        get
        {
            int result = 0;
            if (GeneralFuncsLib.GetDataOfExtendedSetting(LIMIT_REPORT_DAY_MERCHANT_ALERT_HISTORY, "0") != null)
            {
                Int32.TryParse(GeneralFuncsLib.GetDataOfExtendedSetting(LIMIT_REPORT_DAY_MERCHANT_ALERT_HISTORY, "0"), out result);
            }
            return result;
        }
    }

    protected DataTable Category
    {
        get
        {
            if (ViewState[CATEGORY] != null)
                return (DataTable)ViewState[CATEGORY];
            else
            {
                var parameters = new FilterParameterCollection();
                parameters.AddLoggedInUserReportingParams();
                parameters.Add(new FilterParameter("@PageType", "MerchantAlertWorkedPage", DbType.String));
                DataTable dt = WebServices.RiskServices.GetReports("spa_RM_MCF_GetExtractSubCategory", parameters);
                ViewState[CATEGORY] = dt;
                return dt;
            }
        }
    }
    private MerchantAlertReportEntities _MerchantAlertReportEntities;

    #endregion
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            OnDataBindControls(DataBindAction.BindAssignmentList);
            OnDataBindControls(DataBindAction.BindParameterList);
            OnDataBindControls(DataBindAction.BindUserNameList);
            OnDataBindControls(DataBindAction.BindDispositionList);

            SessionManager.CurrentAlertHistoryReportFilter = null;
            GetReportFilter();
            SetFilterParams();
        }
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.DoFilterAction:
                SetFilterParams();
                // Validate value filter
                if (!ValidateFilter()) return;

                if (SubmitFiltering != null)
                {
                    SubmitFiltering(sender, _MerchantAlertReportEntities);
                }
                GetReportFilter();
                break;
            case PostBackAction.DoRefreshAction:
                string[] list = hddMerchantList.Value.Split(',');
                string merchantList = uxMerchantNumber.Text;
                foreach (var merchant in list)
                {
                    if ((',' + merchantList.Replace(" ", "") + ',').IndexOf(',' + merchant + ',') < 0)
                        merchantList += merchant + ',';
                }
                uxMerchantNumber.Text = VeraCodeSolution.DoVeraCode(merchantList.Trim(','));
                break;
            default:
                break;
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindAssignmentList:
                {
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parameters.Add(new FilterParameter("@IsNotExpire", true, DbType.Boolean));
                    this.uxAssignmentList.DataTextField = "AssignmentName";
                    this.uxAssignmentList.DataValueField = "AssignmentID";
                    this.uxAssignmentList.DataSource = WebServices.RiskServices.GetReports("spa_RM_MCF_GetAssignmentList", parameters);
                    this.uxAssignmentList.DataBind();
                }
                break;
            case DataBindAction.BindParameterList:
                uxParameterList.DataTextField = "DataText";
                uxParameterList.DataValueField = "DataValue";
                DataTable data = GetOriginalAssignmentParameters(-1);
                DataTable dt = new DataTable();
                dt.Columns.Add("DataValue");
                dt.Columns.Add("DataText");
                foreach (DataRow row in data.Rows)
                {
                    DataRow r = dt.NewRow();
                    r[0] = row["ParameterKey"];
                    r[1] = (row["ParameterKey"] == DBNull.Value ? string.Empty : row["ParameterKey"].ToString()) + (row["ParameterName"] == DBNull.Value ? string.Empty : " - " + row["ParameterName"].ToString());
                    dt.Rows.Add(r);
                }

                uxParameterList.DataSource = dt;
                uxParameterList.DataBind();
                break;
            case DataBindAction.BindUserNameList:
                {
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    parameters.Add(new FilterParameter("@UserName", "", DbType.String));
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    this.uxUserNameList.DataTextField = "UserNameFull";
                    this.uxUserNameList.DataValueField = "UserID";
                    this.uxUserNameList.DataSource = WebServices.RiskServices.GetReports("spa_RM_MCF_Reporting_GetUsers", parameters);
                    this.uxUserNameList.DataBind();
                }
                break;
            case DataBindAction.BindDispositionList:
                {
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    this.uxDispositionList.DataTextField = "DispositionName";
                    this.uxDispositionList.DataValueField = "DispositionID";
                    this.uxDispositionList.DataSource = WebServices.RiskServices.GetReports("spa_RM_MCF_Reporting_GetDisposition", parameters);
                    this.uxDispositionList.DataBind();
                }
                break;
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript("checkAndRemoveSpecialCharacters();");
    }
    private DataTable GetOriginalAssignmentParameters(int ActivityStatus)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.AddLanguageID();
        parameters.Add(new FilterParameter("@ActivityStatus", ActivityStatus, DbType.Int32));
        if (RiskSessionManager.IsUsingMarketData)
            return WebServices.RiskServices.GetReports("spa_RM_MCF_GetParametersList_MarketData", parameters);
        return WebServices.RiskServices.GetReports("spa_RM_MCF_GetParametersList", parameters);
    }

    protected void uxSearch_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.DoFilterAction);
    }
    protected void uxRefresh_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.DoRefreshAction);
    }


    #region Helpers

    private string HandleString(ListItemCollection items)
    {
        StringBuilder result = new StringBuilder();
        string delim = "";
        foreach (ListItem item in items)
        {
            result.Append(delim); delim = ",";
            result.Append(item.Value);
        }
        return result.ToString();
    }

    private void SetFilterItems(AS.Controls.Chooser.MultiChooser ctrl, string filterValue)
    {
        if (filterValue != null)
        {
            string[] list = filterValue.Split(',');
            foreach (string value in list)
            {
                ListItem item = ctrl.Items.FindByValue(value);
                if (item != null)
                    item.Selected = true;
            }
        }
    }

    private void GetReportFilter()
    {
        reportFilter = SessionManager.CurrentAlertHistoryReportFilter;
        if (reportFilter == null)
        {
            reportFilter = new HierarchyFilterValue();
            // Set default value for date from and date to on filter
            var currentDt = DateTime.Now;
            // Assign default value to session variable for the first load without search event click
            reportFilter.DateOption = DateOptionMode.Daily;
            reportFilter.DateOptionValue.From = DateTime.Now;
        }
        switch (reportFilter.DateOption)
        {
            case DateOptionMode.Daily:
                optDaily.Checked = true;
                optDateRange.Checked = optMonthly.Checked = false;
                uxDate.SelectedDate = reportFilter.DateOptionValue.From;
                break;
            case DateOptionMode.Monthly:
                optMonthly.Checked = true;
                optDaily.Checked = optDateRange.Checked = false;
                uxDate.SelectedDate = reportFilter.DateOptionValue.From; ;
                break;
            case DateOptionMode.DateRange:
                optDateRange.Checked = true;
                optDaily.Checked = optMonthly.Checked = false;
                uxFromDate.SelectedDate = reportFilter.DateOptionValue.From; ;
                uxEndDate.SelectedDate = reportFilter.DateOptionValue.To;
                hhdDateFrom.Value = uxFromDate.SelectedDate.ToString();
                hhdDateTo.Value = uxEndDate.SelectedDate.ToString();
                break;
        }
    }

    private void SetFilterParams()
    {
        if (optAllMerchant.Checked)
        {
            uxMerchantNumber.Text = string.Empty;
            uxMerchantName.Text = string.Empty;
        }

        MerchantNumber = uxMerchantPanel.Visible ? uxMerchantNumber.Text.Trim().Replace(" ", string.Empty) : string.Empty;
        MerchantName = uxMerchantPanel.Visible ? uxMerchantName.Text.Trim() : string.Empty;
        ParameterFilterValue = uxParameterList.Visible ? HandleString(uxParameterList.SelectedItems) : string.Empty;
        AssignmentFilterValue = uxAssignmentPanel.Visible ? HandleString(uxAssignmentList.SelectedItems) : string.Empty;
        UserNameFilterValue = uxUserName.Visible ? HandleString(uxUserNameList.SelectedItems) : string.Empty;
        DispositionFilterValue = uxDisposition.Visible ? HandleString(uxDispositionList.SelectedItems) : string.Empty;        

        reportFilter = new HierarchyFilterValue();
        if (optDaily.Checked)
        {
            reportFilter.DateOption = DateOptionMode.Daily;
            reportFilter.DateOptionValue.From = reportFilter.DateOptionValue.To = uxDate.SelectedDate.Value;
        }
        else if (optDateRange.Checked)
        {
            reportFilter.DateOption = DateOptionMode.DateRange;
            reportFilter.DateOptionValue.From = uxFromDate.SelectedDate.Value;
            reportFilter.DateOptionValue.To = uxEndDate.SelectedDate.Value;

        }
        else if (optMonthly.Checked)
        {
            reportFilter.DateOption = DateOptionMode.Monthly;
            reportFilter.DateOptionValue.From = reportFilter.DateOptionValue.To = uxDate.SelectedDate.Value;
        }
        
        SessionManager.CurrentAlertHistoryReportFilter = reportFilter;
        _MerchantAlertReportEntities = new MerchantAlertReportEntities();
        DataTable tb = Category;
        if (tb != null && tb.Rows.Count > 0)
        {
            _MerchantAlertReportEntities.CategoryCode = (int)tb.Rows[0][CATEGORY_CODE];
            _MerchantAlertReportEntities.SubCategoryCode = (int)tb.Rows[0][SUB_CATEGORY_CODE];
            _MerchantAlertReportEntities.SPAName = tb.Rows[0][SPA_NAME].ToString();
            _MerchantAlertReportEntities.HasMerchantFilter = tb.Rows[0][MERCHANT_CRITERIA].ToString().Equals("Y") ? true : false;
            _MerchantAlertReportEntities.HasDateRangeFilter = tb.Rows[0][DATE_CRITERIA].ToString().Equals("Y") ? true : false;
        }
    }

    private bool ValidateFilter()
    {
        if (!string.IsNullOrEmpty(MerchantNumber))
        {
            var reg = new Regex(@"^[0-9\-, *]*$");
            Match match = reg.Match(MerchantNumber);
            if (match.Length == 0)
            {
                ShowMessageBox(string.Format(Resources.MessageManager.ReportFilter_V7, GetLocalResourceObject("LiteralResource1.Text").ToString()));
                return false;
            }
        }

        return true;
    }

    private void ShowMessageBox(string message, bool reload = false)
    {
        if (reload)
            message = String.Format("alert('{0}');  window.location.href = window.location.href; ", message).ToString();
        else
            message = String.Format("alert('{0}');", message).ToString();
        Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowAlerMessage", message, true);
    }
    #endregion
}
