using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AS.Web.UI.Controls;
using AS.Common.DBManager;
using System.Data;
using Telerik.Web.UI;

public partial class UserControls_rm_MCF_IncomeFiltering : GlobalUserControl
{
    protected DateTime DefaultFromDate
    {
        get {
            return ((ReportPage)Page).ReportFilter.DefaultDateRange.From;
        }
    }
    public IncomeFilterOptions IncomeFilterOption
    {       
        get
        {
            return SessionManager.IncomeFilterOption;
        }
    }

    public void SetVisibleProfile(bool visible)
    {        
        uxPanelProfile.Visible = visible;
    }

    protected void uxReportFilter_ReportFilterAction(object sender, AS.Web.UI.Controls.ReportFilterEventArgs e)
    {
        if (e.ActionType == AS.Web.UI.Controls.ReportFilterEventType.Submit)
        { 
            IncomeFilterOptions op = SessionManager.IncomeFilterOption;
            if (op == null)
            {
                op = new IncomeFilterOptions();              
            }
            if (uxradIEYearToDate.Checked)
            {
                op.DateFilterMode = (int)DateOptionMode.Yearly;
                op.FromDate = new DateTime(DateTime.Now.Year, 1, 1);
                op.ToDate = DateTime.Now;
            }
            if (uxradTwelveMonth.Checked)
            {
                op.DateFilterMode = (int)DateOptionMode.TrailingTwelveMonths;
                op.FromDate = DateTime.Now.AddMonths(-12);
                op.ToDate = DateTime.Now;
            }
            if (uxradDateRange.Checked)
            {
                op.FromDate = uxIEDateFrom.SelectedDate;
                op.ToDate = uxIEDateTo.SelectedDate;
                op.DateFilterMode = (int)DateOptionMode.DateRange;               
            }
            
            op.NetProfit = uxNetProfit.SelectedValue;
            if (op.NetProfit.ToUpper() != "ALL")
            {
                int FromNetProfit = 0;
                int.TryParse(uxNetProfitFrom.Text, out FromNetProfit);
                op.FromNetProfit = FromNetProfit;
                int ToNetProfit = 0;
                int.TryParse(uxNetProfitTo.Text, out ToNetProfit);
                op.ToNetProfit = ToNetProfit;
            }
            else
            {
                op.FromNetProfit = 0;
                op.ToNetProfit = 0;

            }
            int profileId = 0;
            int.TryParse(uxProfile.SelectedValue, out profileId);
            op.ProfileID = profileId;

            SessionManager.IncomeFilterOption = op;

            if (uxNetProfit.SelectedValue == "GREATERTHAN" || uxNetProfit.SelectedValue == "LESSTHAN" || uxNetProfit.SelectedValue == "BETWEEN")
            {
                pnlProfitFrom.Attributes.CssStyle.Add("display", "inline-block");
                uxNetProfitFrom.Text = SessionManager.IncomeFilterOption.FromNetProfit == 0 ? string.Empty : SessionManager.IncomeFilterOption.FromNetProfit.ToString();
            }
            else
            {
                pnlProfitFrom.Attributes.CssStyle.Add("display", "none");
                pnlProfitTo.Attributes.CssStyle.Add("display", "none");

            }
            if (uxNetProfit.SelectedValue == "BETWEEN")
            {
                pnlProfitTo.Attributes.CssStyle.Add("display", "inline-block");

                uxNetProfitFrom.Text = (SessionManager.IncomeFilterOption.FromNetProfit == 0 && SessionManager.IncomeFilterOption.ToNetProfit == 0) ? string.Empty : SessionManager.IncomeFilterOption.FromNetProfit.ToString();
                uxNetProfitTo.Text = SessionManager.IncomeFilterOption.ToNetProfit == 0 ? string.Empty : SessionManager.IncomeFilterOption.ToNetProfit.ToString();
            }
             
            if (uxradIEYearToDate.Checked)
            {
                pnlDateRangecontrolsfrom.Attributes.CssStyle.Add("display", "none");
                pnlDateRangecontrolsto.Attributes.CssStyle.Add("display", "none");
            }
            else if (uxradTwelveMonth.Checked)
            {
                pnlDateRangecontrolsfrom.Attributes.CssStyle.Add("display", "none");
                pnlDateRangecontrolsto.Attributes.CssStyle.Add("display", "none");
            }
            else
            {
                pnlDateRangecontrolsfrom.Attributes.CssStyle.Add("display", "inline-block");
                pnlDateRangecontrolsto.Attributes.CssStyle.Add("display", "inline-block");
            }


        } 
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!IsPostBack)
        {
            IncomeFilterOptions oldFilter = SessionManager.IncomeFilterOption;  
            // First Time Access
            if (oldFilter == null)
            {
                oldFilter = new IncomeFilterOptions();
                oldFilter.DateFilterMode = (int)DateOptionMode.DateRange;
                oldFilter.FromDate = ((ReportPage)Page).ReportFilter.DefaultDateRange.From;
                oldFilter.ToDate = ((ReportPage)Page).ReportFilter.DefaultDateRange.To;
                oldFilter.NetProfit = uxNetProfit.Items[0].Value;
                SessionManager.IncomeFilterOption = oldFilter;
            }

            uxIEDateFrom.SelectedDate = oldFilter.FromDate;
            uxIEDateTo.SelectedDate = oldFilter.ToDate;
            switch (oldFilter.DateFilterMode)
            { 
                case (int)DateOptionMode.DateRange:
                    uxradDateRange.Checked = true;
                    uxradIEYearToDate.Checked = false;
                    uxradTwelveMonth.Checked = false;
                    break;
                case (int)DateOptionMode.Yearly:
                    uxradDateRange.Checked = false;
                    uxradIEYearToDate.Checked = true;
                    uxradTwelveMonth.Checked = false;
                    break;
                case (int)DateOptionMode.TrailingTwelveMonths:
                    uxradDateRange.Checked = false;
                    uxradIEYearToDate.Checked = false;
                    uxradTwelveMonth.Checked = true;
                    break;
            }
            uxNetProfit.SelectedValue = oldFilter.NetProfit; 
            

            if (uxNetProfit.SelectedValue == "GREATERTHAN" || uxNetProfit.SelectedValue == "LESSTHAN" || uxNetProfit.SelectedValue == "BETWEEN")
            {
              
                uxNetProfitFrom.Text = SessionManager.IncomeFilterOption.FromNetProfit == 0 ? string.Empty : SessionManager.IncomeFilterOption.FromNetProfit.ToString();
            }
             
            if (uxNetProfit.SelectedValue == "BETWEEN")
            { 
                uxNetProfitFrom.Text = (SessionManager.IncomeFilterOption.FromNetProfit == 0 && SessionManager.IncomeFilterOption.ToNetProfit == 0) ? string.Empty : SessionManager.IncomeFilterOption.FromNetProfit.ToString();
                uxNetProfitTo.Text = SessionManager.IncomeFilterOption.ToNetProfit == 0 ? string.Empty : SessionManager.IncomeFilterOption.ToNetProfit.ToString();
            }

            FilterParameterCollection parameters = new FilterParameterCollection();
            parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
            parameters.Add(new FilterParameter("@IsSendToIEReport", true, DbType.Boolean));
            DataTable table = WebServices.RiskServices.GetReports("spa_RM_MCF_GetAllProfiles", parameters);
            uxProfile.DataTextField = "ProfileName";
            uxProfile.DataValueField = "ProfileType";
            uxProfile.DataSource = table;            
            uxProfile.DataBind();
            uxProfile.Items.Insert(0, new RadComboBoxItem("[" + GetLocalResourceObject("IncomeFilteringCS_Text_SelectOne").ToString() + "]", null));
            
        }
        if (uxNetProfit.SelectedValue == "GREATERTHAN" || uxNetProfit.SelectedValue == "LESSTHAN" || uxNetProfit.SelectedValue == "BETWEEN")
        {
            pnlProfitFrom.Attributes.CssStyle.Add("display", "inline-block");
            pnlProfitTo.Attributes.CssStyle.Add("display", "none"); 
        }
        else
        {
            pnlProfitFrom.Attributes.CssStyle.Add("display", "none");
            pnlProfitTo.Attributes.CssStyle.Add("display", "none");

        }
        if (uxNetProfit.SelectedValue == "BETWEEN")
        {
            pnlProfitTo.Attributes.CssStyle.Add("display", "inline-block"); 
        }
        if (uxradIEYearToDate.Checked)
        {
            pnlDateRangecontrolsfrom.Attributes.CssStyle.Add("display", "none");
            pnlDateRangecontrolsto.Attributes.CssStyle.Add("display", "none");
        }
        else if (uxradTwelveMonth.Checked)
        {
            pnlDateRangecontrolsfrom.Attributes.CssStyle.Add("display", "none");
            pnlDateRangecontrolsto.Attributes.CssStyle.Add("display", "none");
        }
        else
        {
            pnlDateRangecontrolsfrom.Attributes.CssStyle.Add("display", "inline-block");
            pnlDateRangecontrolsto.Attributes.CssStyle.Add("display", "inline-block");
        }
    }   

}
