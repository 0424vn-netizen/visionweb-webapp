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

public partial class UserControls_rm_MCF_RiskWorkLog_Filter : GlobalUserControl
{
    public event EventHandler SubmitFiltering;
    #region Enums
    enum DataBindAction
    {
        BindAssignmentName,
        BindDQView,
        BindUser,
    }
    enum PostBackAction
    {
        DoFilterAction
    }
    #endregion
    public string MerchantNameFilterValue
    {
        get
        {
            return ViewState["MerchantNameFilterValue"] == null ? string.Empty : ViewState["MerchantNameFilterValue"].ToString();
        }
        set
        {
            ViewState["MerchantNameFilterValue"] = value;
        }
    }

    public string MerchantIDFilterValue
    {
        get
        {
            return ViewState["MerchantIDFilterValue"] == null ? string.Empty : ViewState["MerchantIDFilterValue"].ToString();
        }
        set
        {
            ViewState["MerchantIDFilterValue"] = value;
        }
    }

    public string UserFilterValue
    {
        get
        {
            return ViewState["UserFilterValue"] == null ? string.Empty : ViewState["UserFilterValue"].ToString();
        }
        set
        {
            ViewState["UserFilterValue"] = value;
        }
    }

    public string DQViewFilterValue
    {
        get
        {
            return ViewState["DQViewFilterValue"] == null ? string.Empty : ViewState["DQViewFilterValue"].ToString();
        }
        set
        {
            ViewState["DQViewFilterValue"] = value;
        }
    }

    public string AssignmentListFilterValue
    {
        get
        {
            return ViewState["AssignmentListFilterValue"] == null ? string.Empty : ViewState["AssignmentListFilterValue"].ToString();
        }
        set
        {
            ViewState["AssignmentListFilterValue"] = value;
        }
    }
    
    public HierarchyFilterValue ReportFilterValue
    {
        get
        {
            return ViewState["ReportFilterValue"] == null ? new HierarchyFilterValue() : ViewState["ReportFilterValue"] as HierarchyFilterValue;
        }
        set
        {
            ViewState["ReportFilterValue"] = value;
        }
    }
    
    public void uxMerchantName_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        if (e.Text.Length > 2)
        {
            string merchantName = e.Text.Replace("%", "[%]").Replace(",", "[,]").Replace("^", "[^]").Replace("_", "[_]");
            FilterParameterCollection parameters = new FilterParameterCollection();
            parameters.AddLoggedInUserReportingParams(false);
            parameters.Add(new FilterParameter("@MerchantName", merchantName, DbType.String));
            DataTable list = WebServices.RiskServices.GetReports("spa_RM_MCF_GetMerchantList", parameters);
            uxMerchantName.DataSource = list;
            uxMerchantName.DataBind();
        }
        else
        {
            uxMerchantName.Items.Clear();
        }
    }

    public void uxUserList_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        if (e.Text.Length > 2)
        {
            string userName = e.Text.Replace("%", "[%]").Replace(",", "[,]").Replace("^", "[^]").Replace("_", "[_]");
            FilterParameterCollection parameters = new FilterParameterCollection();
            parameters.Add(new FilterParameter("@UserName", userName, DbType.String));
            parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
            DataTable list = WebServices.RiskServices.GetReports("spa_RM_MCF_Reporting_GetUsers", parameters);
            uxUser.DataSource = list;
            this.uxUser.DataTextField = "UserNameFull";
            this.uxUser.DataValueField = "UserID";

            uxUser.DataBind();
        }
        else
        {
            uxUser.Items.Clear();
        }
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetFilterParams();
            GetFilter();
            OnDataBindControls(DataBindAction.BindAssignmentName, uxAssignmentName);
            OnDataBindControls(DataBindAction.BindDQView, uxDQView);
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

            default:
                break;
        }
    }

    private void SetFilterParams()
    {
        MerchantNameFilterValue = uxMerchantName.Text.Trim().Replace(" ", string.Empty);
        MerchantIDFilterValue = uxMerchantID.Text.Trim();
        UserFilterValue = uxUser.SelectedValue;
        DQViewFilterValue = uxDQView.SelectedValue;
        AssignmentListFilterValue = uxAssignmentName.SelectedValue;
        
        if (uxDaily.Checked)
        {
            ReportFilterValue.DateOption = DateOptionMode.Daily;
            ReportFilterValue.DateOptionValue.From = ReportFilterValue.DateOptionValue.To = uxDate.SelectedDate.Value;
        }
        else if (uxRange.Checked)
        {
            ReportFilterValue.DateOption = DateOptionMode.DateRange;
            ReportFilterValue.DateOptionValue.From = uxFromDate.SelectedDate.Value;
            ReportFilterValue.DateOptionValue.To = uxEndDate.SelectedDate.Value;
        }
    }

    private void GetFilterParams()
    {
        MerchantNameFilterValue = uxMerchantName.Text.Trim().Replace(" ", string.Empty);
        MerchantIDFilterValue = uxMerchantID.Text.Trim();
        UserFilterValue = uxUser.SelectedValue;
        DQViewFilterValue = uxDQView.SelectedValue;
        AssignmentListFilterValue = uxAssignmentName.SelectedValue;

        var currentDt = DateTime.Now;
        ReportFilterValue = new HierarchyFilterValue();
        ReportFilterValue.DateOption = DateOptionMode.Daily;
        ReportFilterValue.DateOptionValue.From = ReportFilterValue.DateOptionValue.To = ReportFilterValue.DateOptionValue.From = currentDt;
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();

        switch ((DataBindAction)type)
        {
            case DataBindAction.BindAssignmentName:
                {
                    RadComboBox cbx = (RadComboBox)sender;

                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parameters.Add(new FilterParameter("@FilterType", "All", DbType.AnsiString));
                    parameters.Add(new FilterParameter("@FilterValue", string.Empty, DbType.AnsiString));
                    parameters.Add(new FilterParameter("@IsReskinSite", 1, DbType.Int32));
                    parameters.Add(new FilterParameter("@AssignmentTypeExcludeList", "2", DbType.AnsiString));
                    DataTable info = WebServices.RiskServices.GetReports("spa_RM_MCF_GetInvestigatedAssignmentsByUserID", parameters);

                    cbx.DataSource = info;
                    cbx.DataTextField = "AssignmentName";
                    cbx.DataValueField = "AssignmentID";
                    cbx.DataBind();
                    cbx.Items.Insert(0, GetLocalResourceObject("lblAllResouce").ToString());
                }
                break;
            case DataBindAction.BindDQView:
                {
                    RadComboBox cbx = (RadComboBox)sender;

                    parameters.AddLanguageID();
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parameters.Add(new FilterParameter("@LookupClass", "View", DbType.AnsiString));
                    DataTable info = WebServices.RiskServices.GetReports("spa_RM_Get_Lookups", parameters);

                    cbx.DataSource = info;
                    cbx.DataTextField = "Description";
                    cbx.DataValueField = "Code";
                    cbx.DataBind();
                }
                break;
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
    }

    protected void uxSearch_Click(object sender, EventArgs e)
    {

        OnPostBackActions(PostBackAction.DoFilterAction);

    }

    private void GetFilter()
    {
        if (ReportFilterValue == null)
        {
            this.ReportFilterValue = new HierarchyFilterValue();
            this.ReportFilterValue.DateOption = DateOptionMode.Daily;
            this.ReportFilterValue.DateOptionValue.To = !uxEndDate.SelectedDate.IsNullOrEmpty() ? uxEndDate.SelectedDate.Value : DateTime.Now;
            this.ReportFilterValue.DateOptionValue.From = !uxFromDate.SelectedDate.IsNullOrEmpty() ? uxFromDate.SelectedDate.Value : DateTime.Now.GetFirstDayOfMonth();

        }
        switch (ReportFilterValue.DateOption)
        {
            case DateOptionMode.Daily:
                this.uxDaily.Checked = true;
                this.uxDate.SelectedDate = this.ReportFilterValue.DateOptionValue.From;
                break;
            case DateOptionMode.DateRange:
                this.uxRange.Checked = true;
                this.uxFromDate.SelectedDate = this.ReportFilterValue.DateOptionValue.From;
                this.uxEndDate.SelectedDate = this.ReportFilterValue.DateOptionValue.To;
                break;
        }
    }
}
