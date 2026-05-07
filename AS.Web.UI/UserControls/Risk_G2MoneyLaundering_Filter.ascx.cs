using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AS.Web.UI.Controls;


public partial class UserControls_Risk_G2MoneyLaundering_Filter : GlobalUserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) {
            var reportFilter = SessionManager.CurrentReportFilter;
            if (reportFilter == null)
            {
                reportFilter = new HierarchyFilterValue();
                // Set default value for date from and date to on filter
                var currentDt = DateTime.Now;
                uxDateRangeFrom.SelectedDate = new DateTime(currentDt.Year, currentDt.Month, 1);
                uxDateRangeTo.SelectedDate = currentDt;
                // Assign default value to session variable for the first load without search event click
                reportFilter.DateOptionValue.From = new DateTime(currentDt.Year, currentDt.Month, 1);
                reportFilter.DateOptionValue.To = currentDt;
                SessionManager.CurrentReportFilter = reportFilter;
            }
            else
            {
                GetReportFilter();
            }
        }
    }

    private MgmtReportEntities _MgmtReportEntities;
    public event SearchHandler Search;
    public delegate void SearchHandler();

    protected void uxSearch_Click(object sender, EventArgs e)
    {
        if (!ValidateData())
            return;
        if (Search != null)
        {
            SetReportFilter();
            Search();
            GetReportFilter();
        }
    }

    protected void SetReportFilter()
    {
        HierarchyFilterValue reportFilter = SessionManager.CurrentReportFilter ?? new HierarchyFilterValue();
        reportFilter.DateOptionValue.From = (DateTime)uxDateRangeFrom.SelectedDate;
        reportFilter.DateOptionValue.To = (DateTime)uxDateRangeTo.SelectedDate;
        SessionManager.CurrentReportFilter = reportFilter;
    }

    private void GetReportFilter()
    {
        HierarchyFilterValue reportFilter = SessionManager.CurrentReportFilter;
        if (reportFilter != null)
        {
            uxDateRangeFrom.SelectedDate = reportFilter.DateOptionValue.From;
            uxDateRangeTo.SelectedDate = reportFilter.DateOptionValue.To;
        }
    }

    #region Validation

    private bool ValidateData()
    {
        if (!FromIsDate() && !FromDateIsGreaterThanToDay() &&
                !EndIsDate() && !EndDateIsGreaterThanToDay() && !ToDateIsGreaterThanFromDate())
            return false;
        return true;
    }    

    private bool FromIsDate()
    {
        return (uxDateRangeFrom.SelectedDate != null && uxDateRangeFrom.SelectedDate.HasValue);
    }

    private bool FromDateIsGreaterThanToDay()
    {
        if (DateTime.Compare(Convert.ToDateTime(uxDateRangeFrom.SelectedDate), DateTime.Now) > 0)
            return false;
        return true;
    }

    private bool EndIsDate()
    {
        return (uxDateRangeTo.SelectedDate != null && uxDateRangeTo.SelectedDate.HasValue);
    }

    private bool EndDateIsGreaterThanToDay()
    {
        if (DateTime.Compare(Convert.ToDateTime(uxDateRangeTo.SelectedDate), DateTime.Now) > 0)
            return false;
        return true;
    }

    private bool ToDateIsGreaterThanFromDate()
    {
        if (DateTime.Compare(Convert.ToDateTime(uxDateRangeFrom.SelectedDate), Convert.ToDateTime(uxDateRangeTo.SelectedDate)) > 0)
            return false;
        return true;
    }

    #endregion
}