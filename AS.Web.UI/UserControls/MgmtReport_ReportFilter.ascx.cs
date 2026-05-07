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

public partial class UserControls_MgmtReport_ReportFilter : GlobalUserControl
{
    public event EventHandler SubmitFiltering;
    #region Enums

    enum DataBindAction
    {
        BindAgentList,
        BindGroupList,
        BindAssignmentList,
        BindParameterList,
    }

    enum PostBackAction
    {
        DoFilterAction,
        DoRefreshAction,
    }

    #endregion

    #region Properties
    public bool IsAgentSearch
    {
        get
        {
            return RiskSessionManager.RiskMgmtReportFilter.IsAgentSearch;                
        }
        set 
        {
            RiskSessionManager.RiskMgmtReportFilter.IsAgentSearch = value;           
        }
    }
    public bool IsGroupSearch
    {
        get
        {
             return RiskSessionManager.RiskMgmtReportFilter.IsGroupSearch;                
        }
        set 
        {
            RiskSessionManager.RiskMgmtReportFilter.IsGroupSearch = value;          
        }       
    }
    public bool VisibleUserGroupFilter
    {
        set
        {
            uxUserGroupPanel.Visible = value;
        }
    }
    public bool VisibleAssignmentFilter
    {
        set
        {
            uxAssignmentPanel.Visible = value;
        }
    }
    public bool VisibleParameterFilter
    {
        set
        {
            uxParameterPanel.Visible = value;
        }
    }
    public bool VisibleMerchantFilter
    {
        set
        {
            uxMerchantPanel.Visible = value;
        }
    }

    public string AgentFilterValue
    {
        get
        {
            return ViewState["AgentFilterValue"] == null ? string.Empty : ViewState["AgentFilterValue"].ToString();
        }
        set
        {
            ViewState["AgentFilterValue"] = value;
        }
    }
    public string GroupFilterValue
    {
        get
        {
            return ViewState["GroupFilterValue"] == null ? string.Empty : ViewState["GroupFilterValue"].ToString();
        }
        set
        {
            ViewState["GroupFilterValue"] = value;
        }
    }
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
    public bool HasMerchantFilter
    {
        get
        {
            return !(uxMerchantNumber.Text == string.Empty && uxMerchantName.Text == string.Empty);
        }
    }
    public string HeaderText
    {
        get
        {
            string result = string.Empty;
            if (IsAgentSearch)
            {
                result = (AgentFilterValue == string.Empty ? GetLocalResourceObject("MgmtReport_ReportFilterCS_Text_AllAgent").ToString() : GetLocalResourceObject("MgmtReport_ReportFilterCS_Text_MultiAgent").ToString());
            }
            else if (IsGroupSearch)
            {
                result = (GroupFilterValue == string.Empty ? GetLocalResourceObject("MgmtReport_ReportFilterCS_Text_AllGroup").ToString() : GetLocalResourceObject("MgmtReport_ReportFilterCS_Text_MultiGroup").ToString());
            }
            else
            {
                if (uxAssignmentPanel.Visible)
                {
                    result += (AssignmentFilterValue == string.Empty ? GetLocalResourceObject("MgmtReport_ReportFilterCS_Text_AllAssgn").ToString() : GetLocalResourceObject("MgmtReport_ReportFilterCS_Text_MultiAssgn").ToString());
                }
                if (uxParameterPanel.Visible)
                {
                    string temp = (ParameterFilterValue == string.Empty ? GetLocalResourceObject("MgmtReport_ReportFilterCS_Text_AllParam").ToString() : GetLocalResourceObject("MgmtReport_ReportFilterCS_Text_MultiParam").ToString());
                    result += ", " + temp;
                }
                if (uxMerchantPanel.Visible)
                {
                    string temp = (!HasMerchantFilter ? GetLocalResourceObject("MgmtReport_ReportFilterCS_Text_AllMerchants").ToString() : GetLocalResourceObject("MgmtReport_ReportFilterCS_Text_MultiMerchants").ToString());
                    result += ", " + temp;
                }
            }
            return result + " " + GetDateFilterText();
        }
    }
    public bool IsMonthlyDefault { get; set; }
    public bool IsWeeklyDefault { get; set; }

    #endregion 
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        if (uxOption.Visible)
        {
            if (!IsAgentSearch && !IsGroupSearch)
            {
                IsAgentSearch = true;
            }
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        
        uxDaily.MaxDate = uxWeekly.MaxDate = uxMonthly.MaxDate = uxDateRangeFrom.MaxDate = uxDateRangeTo.MaxDate = DateTime.Today;
        
        if (!IsPostBack)
        {
            if (uxUserGroupPanel.Visible)
            { 
                OnDataBindControls(DataBindAction.BindAgentList);
                OnDataBindControls(DataBindAction.BindGroupList);
            }
            if (uxAssignmentPanel.Visible)
            {
                OnDataBindControls(DataBindAction.BindAssignmentList);
            }
            if (uxParameterPanel.Visible)
            {
                OnDataBindControls(DataBindAction.BindParameterList);
            }
            // Reset default for first load
            if (!RiskSessionManager.RiskMgmtReportFilter.KeepSession)
            {
                if (IsWeeklyDefault)
                    RiskSessionManager.RiskMgmtReportFilter.DateType = DateOptionMode.Weekly;
                else if (IsMonthlyDefault)
                    RiskSessionManager.RiskMgmtReportFilter.DateType = DateOptionMode.Monthly;
                else
                    RiskSessionManager.RiskMgmtReportFilter.DateType = DateOptionMode.Daily;
                
                RiskSessionManager.RiskMgmtReportFilter.FromDate = RiskSessionManager.RiskMgmtReportFilter.ToDate = DateTime.Today;
            }          

            GetReportFilter();
            SetFilterParams();
        }
        if (ViewState["FirstSubmit"] == null)
        {
            ViewState["FirstSubmit"] = 1;

            uxOption.Items.FindItemByValue("optAgent", true).Selected = IsAgentSearch;
            uxOption.Items.FindItemByValue("optGroup", true).Selected = IsGroupSearch;


            switch (RiskSessionManager.RiskMgmtReportFilter.DateType)
            {
                case DateOptionMode.Daily:
                    optDaily.Checked = true;
                    break;
                case DateOptionMode.Monthly:
                    optMonthly.Checked = true;
                    break;
                case DateOptionMode.Weekly:
                    optWeekly.Checked = true;
                    break;

            }
            MgmtReportFilter reportFilterValue = RiskSessionManager.RiskMgmtReportFilter;

            if (optDaily.Checked)
            {
                reportFilterValue.DateType = DateOptionMode.Daily;
                reportFilterValue.FromDate = reportFilterValue.ToDate = reportFilterValue.FromDate;
                uxDaily.SelectedDate = reportFilterValue.FromDate == null ? DateTime.Now : reportFilterValue.FromDate;
            }
            else if (optWeekly.Checked)
            {
                reportFilterValue.DateType = DateOptionMode.Weekly;
                reportFilterValue.FromDate = reportFilterValue.ToDate = reportFilterValue.FromDate; //
                uxWeekly.SelectedDate = reportFilterValue.FromDate == null ? DateTime.Now : reportFilterValue.FromDate;
            }
            else if (optMonthly.Checked)
            {
                reportFilterValue.DateType = DateOptionMode.Monthly;
                reportFilterValue.FromDate = reportFilterValue.ToDate = reportFilterValue.FromDate; // 
                uxMonthly.SelectedDate = reportFilterValue.FromDate == null ? DateTime.Now : reportFilterValue.FromDate;
            }

        }

    }


    protected override void OnPostBackActions(Enum type, object sender)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.DoFilterAction:
                SetFilterParams();
                if (SubmitFiltering != null)
                {
                    SubmitFiltering(sender, null);
                }
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
            case DataBindAction.BindAgentList:
                {
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    uxAgentList.DataValueField = "UserID";
                    uxAgentList.DataTextField = "UserText";
                    uxAgentList.DataSource = WebServices.RiskServices.GetReports("spa_rm_cs_GetRiskUsers_MgmtReport", parameters);
                    uxAgentList.DataBind();
                }
                break;
            case DataBindAction.BindGroupList:
                {
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parameters.Add(new FilterParameter("@IsActive", 1, DbType.Int32));
                    parameters.Add(new FilterParameter("@SortOrder", "GroupName ASC", DbType.AnsiString));
                    uxGroupList.DataTextField = "GroupName";
                    uxGroupList.DataValueField = "GroupID";
                    uxGroupList.DataSource = WebServices.RiskServices.GetReports("spa_rm_cs_GetAllGroups", parameters);
                    uxGroupList.DataBind();
                }
                break;
            case DataBindAction.BindAssignmentList:
                {
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parameters.Add(new FilterParameter("@IsNotExpire", true, DbType.Boolean)); 
                    this.uxAssignmentList.DataTextField = "AssignmentName";
                    this.uxAssignmentList.DataValueField = "AssignmentID";
                    this.uxAssignmentList.DataSource = WebServices.RiskServices.GetReports("spa_rm_cs_GetAssignmentList", parameters);
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
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        SetReportFilterStatus();
    }
    private DataTable GetOriginalAssignmentParameters(int ActivityStatus)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.AddLanguageID();
        parameters.Add(new FilterParameter("@ActivityStatus", ActivityStatus, DbType.Int32));
        if (RiskSessionManager.IsUsingMarketData)
            return WebServices.RiskServices.GetReports("spa_rm_cs_GetParametersList_MarketData", parameters);
        return WebServices.RiskServices.GetReports("spa_rm_cs_GetParametersList", parameters);
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

    private string GetDateFilterText()
    {
        string dateText = string.Empty;
        string beginDateText = string.Empty;
        string endDateText = string.Empty;
        DateTime beginDate = RiskSessionManager.RiskMgmtReportFilter.FromDate;
        if (optDaily.Checked)
        {
            RiskSessionManager.RiskMgmtReportFilter.DateType = DateOptionMode.Daily;
            beginDate =  uxDaily.SelectedDate.Value;
        }
        else if (optWeekly.Checked)
        {
            RiskSessionManager.RiskMgmtReportFilter.DateType = DateOptionMode.Weekly;
            beginDate = uxWeekly.SelectedDate.Value;
        }
        else if (optMonthly.Checked)
        {
            RiskSessionManager.RiskMgmtReportFilter.DateType = DateOptionMode.Monthly;
            beginDate = uxMonthly.SelectedDate.Value;
        } 

        DateTime endDate = RiskSessionManager.RiskMgmtReportFilter.ToDate;
        GeneralFuncsLib.GetRealDateRange(RiskSessionManager.RiskMgmtReportFilter.DateType, ref beginDate, ref endDate);
        dateText = (RiskSessionManager.RiskMgmtReportFilter.DateType == DateOptionMode.Daily ? beginDate.ToGenericDateString() : string.Format("{0} - {1}", beginDate.ToGenericDateString(), endDate.ToGenericDateString()));
        return "(" + dateText + ")";
    }

    private void GetReportFilter()
    {
        switch (RiskSessionManager.RiskMgmtReportFilter.DateType)
        {
            case DateOptionMode.Daily:
                optDaily.Checked = true;
                optWeekly.Checked = optMonthly.Checked = optDateRange.Checked = false;
                uxDaily.SelectedDate = RiskSessionManager.RiskMgmtReportFilter.FromDate;
                break;
            case DateOptionMode.Weekly:
                optWeekly.Checked = true;
                optDaily.Checked = optMonthly.Checked = optDateRange.Checked = false;
                uxWeekly.SelectedDate = RiskSessionManager.RiskMgmtReportFilter.FromDate;
                break;
            case DateOptionMode.Monthly:
                optMonthly.Checked = true;
                optDaily.Checked = optWeekly.Checked = optDateRange.Checked = false;
                uxMonthly.SelectedDate = RiskSessionManager.RiskMgmtReportFilter.FromDate;
                break;
            case DateOptionMode.DateRange:
                optDateRange.Checked = true;
                optDaily.Checked = optWeekly.Checked = optMonthly.Checked = false;
                uxDateRangeFrom.SelectedDate = RiskSessionManager.RiskMgmtReportFilter.FromDate;
                uxDateRangeTo.SelectedDate = RiskSessionManager.RiskMgmtReportFilter.ToDate;
                break;
        }       

        if (RiskSessionManager.RiskMgmtReportFilter.KeepSession)
        {
            if (RiskSessionManager.RiskMgmtReportFilter.IsAgentSearch)
            {
                uxGroupPanel.Style.Add(HtmlTextWriterStyle.Display, "none");
                uxAgentPanel.Style.Add(HtmlTextWriterStyle.Display, "");
                uxOption.Items.FindItemByValue("optAgent", true).Selected = true;
                uxAgentPanel.Visible = true;               
            }
            else if (RiskSessionManager.RiskMgmtReportFilter.IsGroupSearch)
            {
                uxGroupPanel.Style.Add(HtmlTextWriterStyle.Display, "");
                uxAgentPanel.Style.Add(HtmlTextWriterStyle.Display, "none");
                uxOption.Items.FindItemByValue("optGroup", true).Selected = true;              
                uxUserGroupPanel.Visible = true;              
            }

            SetFilterItems(uxAgentList, RiskSessionManager.RiskMgmtReportFilter.AgentFilterValue);
            SetFilterItems(uxGroupList, RiskSessionManager.RiskMgmtReportFilter.GroupFilterValue);
            SetFilterItems(uxAssignmentList, RiskSessionManager.RiskMgmtReportFilter.AssignmentFilterValue);
            SetFilterItems(uxParameterList, RiskSessionManager.RiskMgmtReportFilter.ParameterFilterValue);
            uxMerchantName.Text = RiskSessionManager.RiskMgmtReportFilter.MerchantName;
            uxMerchantNumber.Text = RiskSessionManager.RiskMgmtReportFilter.MerchantNumber;
        }
        else
        {
            if (IsAgentSearch)
            {
                uxGroupPanel.Style.Add(HtmlTextWriterStyle.Display, "none");
                uxAgentPanel.Style.Add(HtmlTextWriterStyle.Display, "");
            }
            else if (IsGroupSearch)
            {
                uxGroupPanel.Style.Add(HtmlTextWriterStyle.Display, "");
                uxAgentPanel.Style.Add(HtmlTextWriterStyle.Display, "none");
            }
        }

    }

    private void SetReportFilterStatus()
    {
        MgmtReportFilter reportFilterValue = RiskSessionManager.RiskMgmtReportFilter;
        HideAllDatePicker();
        if (reportFilterValue.DateType == DateOptionMode.Daily)
        {
            pnlDaily.Style.Add("display", "");            
        }
        else if (reportFilterValue.DateType == DateOptionMode.Weekly)
        {
            pnlWeekly.Style.Add("display", "");         
        }
        else if (reportFilterValue.DateType == DateOptionMode.Monthly)
        {
            pnlMonthly.Style.Add("display", "");         
        }
        else
        {
            pnlDateRange.Style.Add("display", "");         
        }

        if (IsAgentSearch)
        {
            uxGroupPanel.Style.Add(HtmlTextWriterStyle.Display, "none");
            uxAgentPanel.Style.Add(HtmlTextWriterStyle.Display, "");          
        }
        else if (IsGroupSearch)
        {
            uxGroupPanel.Style.Add(HtmlTextWriterStyle.Display, "");
            uxAgentPanel.Style.Add(HtmlTextWriterStyle.Display, "none");
        }       
    }

    private void HideAllDatePicker()
    {
        pnlDaily.Style.Add("display", "none");
        pnlWeekly.Style.Add("display", "none");
        pnlMonthly.Style.Add("display", "none");
        pnlDateRange.Style.Add("display", "none");
    }

    private void SetFilterParams()
    {

        IsAgentSearch = uxUserGroupPanel.Visible && uxOption.SelectedValue.Equals("optAgent", StringComparison.OrdinalIgnoreCase);
        IsGroupSearch = uxUserGroupPanel.Visible && uxOption.SelectedValue.Equals("optGroup", StringComparison.OrdinalIgnoreCase);
         
        MerchantNumber = uxMerchantPanel.Visible ? uxMerchantNumber.Text.Trim().Replace(" ", string.Empty) : string.Empty;
        MerchantName = uxMerchantPanel.Visible ? uxMerchantName.Text.Trim() : string.Empty;
        AgentFilterValue = IsAgentSearch ? HandleString(uxAgentList.SelectedItems) : string.Empty;
        GroupFilterValue = IsGroupSearch ? HandleString(uxGroupList.SelectedItems) : string.Empty;
        ParameterFilterValue = uxParameterList.Visible ? HandleString(uxParameterList.SelectedItems) : string.Empty;
        AssignmentFilterValue = uxAssignmentPanel.Visible ? HandleString(uxAssignmentList.SelectedItems) : string.Empty;
       
        MgmtReportFilter reportFilterValue = RiskSessionManager.RiskMgmtReportFilter;

        if (optDaily.Checked)
        {
            reportFilterValue.DateType = DateOptionMode.Daily;
            reportFilterValue.FromDate = reportFilterValue.ToDate = uxDaily.SelectedDate.Value;
        }
        else if (optWeekly.Checked)
        {
            reportFilterValue.DateType = DateOptionMode.Weekly;
            reportFilterValue.FromDate = reportFilterValue.ToDate = uxWeekly.SelectedDate.Value;
        }
        else if (optMonthly.Checked)
        {
            reportFilterValue.DateType = DateOptionMode.Monthly;
            reportFilterValue.FromDate = reportFilterValue.ToDate = uxMonthly.SelectedDate.Value;
        }
        if (IsAgentSearch)
        {
            reportFilterValue.IsAgentSearch = true;
            reportFilterValue.IsGroupSearch = false;
            reportFilterValue.AgentFilterValue = AgentFilterValue;
        }
        else if (IsGroupSearch)
        {           
            reportFilterValue.IsGroupSearch = true;
            reportFilterValue.IsAgentSearch = false;
            reportFilterValue.GroupFilterValue = GroupFilterValue;
        }
        reportFilterValue.AssignmentFilterValue = AssignmentFilterValue;
        reportFilterValue.ParameterFilterValue = ParameterFilterValue;
        reportFilterValue.MerchantName = MerchantName;
        reportFilterValue.MerchantNumber = MerchantNumber;

        RiskSessionManager.RiskMgmtReportFilter = reportFilterValue;
    }

    #endregion
}
