using AS.Common.DBManager;
using AS.Controls.Telerik;
using AS.Web.UI.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControls_ServiceTestFilter : System.Web.UI.UserControl
{
    #region CONSTANTS
    private const string MERCHANT_NUMBER = "MERCHANTNUMBER";
    #endregion
    
    #region EVENTS
    public event SearchHandler Search;
    public delegate void SearchHandler(object sender);
    private HierarchyFilterValue _reportFitler = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (SessionManager.CurrentReportFilter != null && GeneralFuncsLib.IsMerchantMode(SessionManager.CurrentReportFilter.HierarchyMode))
            {
                uxSearchValue.Text = SessionManager.CurrentReportFilter.Value;
            }
            GetReportFilter();
            SetReportFilter();
        }       
    }

    protected void uxSearch_Click(object sender, EventArgs e)
    {
        if (!ValidateData())        
            return;
        
        if (Search != null)
        {
            SetReportFilter();
            Search(this);
        }
    }
    
    #endregion

    #region HELPER METHODS

    private void GetReportFilter()
    {
        _reportFitler = SessionManager.CurrentReportFilter;
        if (_reportFitler == null)
        {
            _reportFitler = new HierarchyFilterValue();
            _reportFitler.DateOption = DateOptionMode.DateRange;
            _reportFitler.DateOptionValue.To = DateTime.Now;
            if (GeneralFuncsLib.HasExtendedSetting("FILTERING_OPTIONS_DATERANGE"))
            {
                this._reportFitler.DateOptionValue.From = DateTime.Now.AddDays(-90);
            }
            else
            {
                this._reportFitler.DateOptionValue.From = DateTime.Now.GetFirstDayOfMonth();
            }
        }
        if (_reportFitler != null)
        {
            switch (_reportFitler.DateOption)
            {
                case DateOptionMode.Daily:
                    uxDaily.Checked = true;
                    uxDate.SelectedDate = _reportFitler.DateOptionValue.From;
                    break;
                case DateOptionMode.Monthly:
                    uxMonthly.Checked = true;
                    uxDate.SelectedDate = _reportFitler.DateOptionValue.From;
                    break;
                case DateOptionMode.DateRange:
                    uxDateRange.Checked = true;
                    uxDateRangeFrom.SelectedDate = _reportFitler.DateOptionValue.From;
                    uxDateRangeTo.SelectedDate = _reportFitler.DateOptionValue.To;
                    break;
                default:
                    break;
            }
        }
    }

    private void SetReportFilter()
    {
        //Get report filter form session
        if (SessionManager.CurrentReportFilter != null)
            this._reportFitler = SessionManager.CurrentReportFilter;
        else
            this._reportFitler = new HierarchyFilterValue();

        if (uxDaily.Checked)
        {
            _reportFitler.DateOption = DateOptionMode.Daily;
            _reportFitler.DateOptionValue.From = _reportFitler.DateOptionValue.To = (DateTime)uxDate.SelectedDate;
        }
        else if (uxMonthly.Checked)
        {
            _reportFitler.DateOption = DateOptionMode.Monthly;
            _reportFitler.DateOptionValue.From = _reportFitler.DateOptionValue.To = (DateTime)uxDate.SelectedDate;
        }
        else if (uxDateRange.Checked)
        {
            _reportFitler.DateOption = DateOptionMode.DateRange;
            _reportFitler.DateOptionValue.From = (DateTime)uxDateRangeFrom.SelectedDate;
            _reportFitler.DateOptionValue.To = (DateTime)uxDateRangeTo.SelectedDate;
        }

        _reportFitler.Value = uxSearchValue.Text;
        _reportFitler.HierarchyMode = MERCHANT_NUMBER;
        SessionManager.CurrentReportFilter = _reportFitler;
    }

    #endregion

    #region VALIDATE METHODS
    private bool ValidateData()
    {
        if (haveDate.Value == "1")
        {
            if (!StartIsDate() && !StartDateIsGreaterThanToDay() &&
           !FromIsDate() && !FromDateIsGreaterThanToDay() &&
           !EndIsDate() && !EndDateIsGreaterThanToDay() && !ToDateIsGreaterThanFromDate())
                return false;

        }
        if (haveMerchant.Value == "1")
        {
            if (!IsValidMerchantNumber())
                return false;
        }
        return true;
    }    

    private bool StartIsDate()
    {
        if (!this.uxDateRange.Checked)
        {
            return (uxDate.SelectedDate != null && uxDate.SelectedDate.HasValue);
        }
        return true;
    }

    private bool StartDateIsGreaterThanToDay()
    {
        if (!this.uxDateRange.Checked)
        {
            if (DateTime.Compare(Convert.ToDateTime(uxDate.SelectedDate), DateTime.Now) > 0)
            {
                return false;
            }
        }
        return true;
    }

    private bool FromIsDate()
    {
        if (this.uxDateRange.Checked)
        {
            return (uxDateRangeFrom.SelectedDate != null && uxDateRangeFrom.SelectedDate.HasValue);
        }
        return true;
    }

    private bool FromDateIsGreaterThanToDay()
    {
        if (this.uxDateRange.Checked)
        {
            if (DateTime.Compare(Convert.ToDateTime(uxDateRangeFrom.SelectedDate), DateTime.Now) > 0)
            {
                return false;
            }
        }
        return true;
    }

    private bool EndIsDate()
    {
        if (this.uxDateRange.Checked)
        {
            return (uxDateRangeTo.SelectedDate != null && uxDateRangeTo.SelectedDate.HasValue);
        }
        return true;
    }

    private bool EndDateIsGreaterThanToDay()
    {
        if (this.uxDateRange.Checked)
        {
            if (DateTime.Compare(Convert.ToDateTime(uxDateRangeTo.SelectedDate), DateTime.Now) > 0)
            {
                return false;
            }
        }
        return true;
    }

    private bool ToDateIsGreaterThanFromDate()
    {
        if (this.uxDateRange.Checked)
        {
            if (DateTime.Compare(Convert.ToDateTime(uxDateRangeFrom.SelectedDate), Convert.ToDateTime(uxDateRangeTo.SelectedDate)) > 0)
            {
                return false;
            }
        }
        return true;
    }

    private bool IsValidMerchantNumber()
    {
        if (uxRCBMerchant.SelectedValue.CompareTo(MERCHANT_NUMBER) == 0)
        {
            Regex reg = new Regex(@"^[0-9]{0,16}$");
            if (!reg.IsMatch(uxSearchValue.Text.Trim()))
                return false;            
        }
        return true;
    }
    #endregion
}