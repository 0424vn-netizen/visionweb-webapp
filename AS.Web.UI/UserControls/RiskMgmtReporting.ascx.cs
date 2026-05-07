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

public partial class UserControls_RiskMgmtReporting : System.Web.UI.UserControl
{
    #region CONSTANTS
    private const string ID = "ID";
    private const string CATEGORY= "Category";
    private const string CATEGORY_CODE = "CategoryCode";
    private const string CATEGORY_DESCRIPTION = "CategoryDescription";
    private const string SUB_CATEGORY_CODE = "SubCategoryCode";
    private const string SUB_CATEGORY_DESCRIPTION = "SubCategoryDescription";
    private const string SPA_NAME = "SPAName";
    private const string DATE_CRITERIA = "DateCriteria";
    private const string MERCHANT_CRITERIA = "MerchantCriteria";
    private const string MERCHANT_NUMBER = "MERCHANTNUMBER";
    private const string DATE_OPTION = "DATEOPTION";
    #endregion

    #region FIELDS
    private MgmtReportEntities _MgmtReportEntities;    

    protected DataTable Category
    {
        get {
            if (ViewState[CATEGORY] != null)
                return (DataTable)ViewState[CATEGORY];
            else
            {
                var parameters = new FilterParameterCollection();
                parameters.AddLoggedInUserReportingParams();
                DataTable dt = WebServices.RiskServices.GetReports("spa_rm_GetExtractSubCategory", parameters);
                ViewState[CATEGORY] = dt;
                return dt;
            }
        }        
    }
    #endregion    
    
    #region EVENTS
    public event SearchHandler Search;
    public delegate void SearchHandler(object sender, MgmtReportEntities mgmtReportEntites);

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            uxDaily.Checked = true;
            uxDate.SelectedDate = DateTime.Now;
            BindReportType();
            GetReportFilter();
        }       
    }

    protected void uxSearch_Click(object sender, EventArgs e)
    {
        if (!ValidateData())        
            return;
        
        if (Search != null)
        {
            SetReportFilter();
            Search(this, _MgmtReportEntities);
        }
    }
    
    #endregion

    #region HELPER METHODS
    private void BindReportType()
    {
        //var distinct = (from row in dt.AsEnumerable() select row["CategoryCode"]).Distinct();
        DataTable distinct = Category.DefaultView.ToTable(CATEGORY, true, CATEGORY_CODE, CATEGORY_DESCRIPTION);
        
        foreach (DataRow dtRow in distinct.Rows){
            int categoryCode = (int)dtRow[CATEGORY_CODE];
            string categoryName = dtRow[CATEGORY_DESCRIPTION].ToString();
            uxRCBReportType.Items.Add(new ASRadComboBoxItem() { Text = categoryName, IsSeparator = true });

            DataTable tb = (from row in Category.AsEnumerable() where row.Field<int>(CATEGORY_CODE) == categoryCode select row).CopyToDataTable();
            foreach (DataRow subRow in tb.Rows)
            {               
                uxRCBReportType.Items.Add(new ASRadComboBoxItem() { Text = subRow[SUB_CATEGORY_DESCRIPTION].ToString(), Value = subRow[ID].ToString(), CssClass = "rcbPrimary" });                
            }
        }

    }

    private void GetReportFilter()
    {
        HierarchyFilterValue reportFilter = SessionManager.CurrentReportFilter;
        if (reportFilter != null)
        {
            switch (reportFilter.DateOption)
            {
                case DateOptionMode.Daily:
                    uxDaily.Checked = true;
                    uxDate.SelectedDate = reportFilter.DateOptionValue.From;
                    break;
                case DateOptionMode.Monthly:
                    uxMonthly.Checked = true;
                    uxDate.SelectedDate = reportFilter.DateOptionValue.From;
                    break;
                case DateOptionMode.DateRange:
                    uxDateRange.Checked = true;
                    uxDateRangeFrom.SelectedDate = reportFilter.DateOptionValue.From;
                    uxDateRangeTo.SelectedDate = reportFilter.DateOptionValue.To;
                    break;
                default:
                    break;
            }
        }

        int id = 0;
        if (Int32.TryParse(uxRCBReportType.SelectedValue, out id))
        {
            DataTable tb = (from row in Category.AsEnumerable() where row.Field<Int64>(ID) == id select row).CopyToDataTable();
            if (tb != null && tb.Rows.Count > 0)
            {
                // ToDo: Work for multiple date option
                var dateOption = tb.Rows[0][DATE_OPTION].ToString();
                if (!string.IsNullOrEmpty(dateOption))
                {
                    if (dateOption.ToInt() == (int)WebSiteEnums.ExtractDateRange.Mothly)
                    {
                        uxDaily.Visible = false;
                        uxDateRange.Visible = false;
                    }
                }
            }
        }
    }

    private void SetReportFilter()
    {
        
        if (haveDate.Value == "1")
        {
            HierarchyFilterValue reportFilter = SessionManager.CurrentReportFilter;
            if (reportFilter == null)
                reportFilter = new HierarchyFilterValue();
            if (uxDaily.Checked)
            {
                reportFilter.DateOption = DateOptionMode.Daily;
                reportFilter.DateOptionValue.From = reportFilter.DateOptionValue.To = (DateTime)uxDate.SelectedDate;
            }
            else if (uxMonthly.Checked)
            {
                reportFilter.DateOption = DateOptionMode.Monthly;
                reportFilter.DateOptionValue.From = reportFilter.DateOptionValue.To = (DateTime)uxDate.SelectedDate;
            }
            else if (uxDateRange.Checked)
            {
                reportFilter.DateOption = DateOptionMode.DateRange;
                reportFilter.DateOptionValue.From = (DateTime)uxDateRangeFrom.SelectedDate;
                reportFilter.DateOptionValue.To = (DateTime)uxDateRangeTo.SelectedDate;
            }
            SessionManager.CurrentReportFilter = reportFilter;
        }

        _MgmtReportEntities = new MgmtReportEntities();
        if(haveMerchant.Value == "1")
        {
            if (uxRCBMerchant.SelectedValue.CompareTo(MERCHANT_NUMBER) == 0)
            {
                _MgmtReportEntities.MerchantType = 2;
                _MgmtReportEntities.SearchValue = uxSearchValue.Text;
            }
            else
            {
                _MgmtReportEntities.MerchantType = 1;
                _MgmtReportEntities.SearchValue = string.Empty;
            }
        }

        int id = 0;
        if (Int32.TryParse(uxRCBReportType.SelectedValue, out id))
        {
            DataTable tb = (from row in Category.AsEnumerable() where row.Field<Int64>(ID) == id select row).CopyToDataTable();
            if (tb != null && tb.Rows.Count > 0)
            {
                _MgmtReportEntities.CategoryCode = (int)tb.Rows[0][CATEGORY_CODE];
                _MgmtReportEntities.SubCategoryCode = (int)tb.Rows[0][SUB_CATEGORY_CODE];
                _MgmtReportEntities.SPAName = tb.Rows[0][SPA_NAME].ToString();
                _MgmtReportEntities.HasMerchantFilter = tb.Rows[0][MERCHANT_CRITERIA].ToString().Equals("Y") ? true : false;
                _MgmtReportEntities.HasDateRangeFilter = tb.Rows[0][DATE_CRITERIA].ToString().Equals("Y") ? true : false;
            }
        }
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
            Regex reg = new Regex(@"^[-a-zA-Z0-9]{0,16}$");
            if (!reg.IsMatch(uxSearchValue.Text.Trim()))
                return false;            
        }
        return true;
    }
    #endregion

    //42924 – VW - Filter Collapse Issue
    #region Web Methods
    [WebMethod(EnableSession = true)]
    public string[] GetCriteria(string reportType)
    {
        int id = 0;
        if (Int32.TryParse(reportType, out id))
        {
            DataTable tb = (from row in Category.AsEnumerable() where row.Field<Int64>(ID) == id select row).CopyToDataTable();
            if (tb != null && tb.Rows.Count > 0)
            {
                int isMerchant = tb.Rows[0][MERCHANT_CRITERIA].ToString().Equals("Y") ? 1 : 0;
                int isDate = tb.Rows[0][DATE_CRITERIA].ToString().Equals("Y") ? 1 : 0;
                string optionDate = tb.Rows[0][DATE_OPTION].ToString();

                return new string[] { isMerchant.ToString(), isDate.ToString(), optionDate };
            }
        }

        return null;
    }
    #endregion
}