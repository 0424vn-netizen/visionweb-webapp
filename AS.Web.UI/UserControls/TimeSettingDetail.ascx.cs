using AS.Common.DBManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_TimeSetting : GlobalUserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindREFTimeZones();
            LoadSettings();
        }
        else
        {
            LoadEndingAt();
        }
    }

    protected void uxSubmit_Click(object sender, EventArgs e)
    {
        SaveSettings();
    }

    private void BindREFTimeZones()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@LanguageID", SessionManager.CurrentLanguage, DbType.Int16));
        uxTimeZone.DataSource = WebServices.SecurityServices.GetReports("spa_CS_GetListTimeZone", parameters);
        uxTimeZone.DataValueField = "TimeZoneID";
        uxTimeZone.DataTextField = "TimeZoneName";
        uxTimeZone.DataBind();
    }

    private void LoadSettings()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        DataTable tb = WebServices.CsReportServices.GetReports("spa_CS_GetTimerSetting", parameters);
        if (tb.Rows.Count > 0)
        {
            uxNormalDayWorkingHours.Value = (int)tb.Rows[0]["WeekHoursPerDay"];
            if (tb.Rows[0]["WeekStarting"] != null)
                uxNormalDayStartTime.SelectedTime = (TimeSpan)tb.Rows[0]["WeekStarting"];
            uxWeekendDayWorkingHours.Value = (int)tb.Rows[0]["WeekendHoursPerDay"];
            if (tb.Rows[0]["WeekendStarting"] != null)
                uxWeekendDayStartTime.SelectedTime = (TimeSpan)tb.Rows[0]["WeekendStarting"];
            if (tb.Rows[0]["TimeZoneID"] != null)
                uxTimeZone.SelectedValue = tb.Rows[0]["TimeZoneID"].ToASString();
            if (tb.Rows[0]["WeekEnding"] != null)
                uxWeekEndingAt.SelectedTime = (TimeSpan)tb.Rows[0]["WeekEnding"];
            if (tb.Rows[0]["WeekendEnding"] != null)
                uxWeekendEndingAt.SelectedTime = (TimeSpan)tb.Rows[0]["WeekendEnding"];
            uxIsExcludeFederalHolidays.Checked = tb.Rows[0]["IsExcludeFederalHoliday"].ToBoolean();
        }
        else
        {
            ResetSettings();
        }
    }

    private void ResetSettings()
    {
        uxNormalDayWorkingHours.Value = 8;
        uxWeekendDayWorkingHours.Value = 8;
        uxNormalDayStartTime.SelectedTime = new TimeSpan(8, 0, 0);
        uxWeekendDayStartTime.SelectedTime = new TimeSpan(8, 0, 0);
        uxIsExcludeFederalHolidays.Checked = true;
    }

    private void SaveSettings()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@WeekHoursPerDay", uxNormalDayWorkingHours.Value.ToInt(), DbType.Int32));
        parameters.Add(new FilterParameter("@WeekStarting", uxNormalDayStartTime.SelectedTime.ToString(), DbType.String));
        parameters.Add(new FilterParameter("@WeekendHoursPerDay", uxWeekendDayWorkingHours.Value.ToInt(), DbType.Int32));
        parameters.Add(new FilterParameter("@WeekendStarting", uxWeekendDayStartTime.SelectedTime.ToString(), DbType.String));
        parameters.Add(new FilterParameter("@TimeZoneID", uxTimeZone.SelectedValue, DbType.String));
        parameters.Add(new FilterParameter("@IsExcludeFederalHoliday", uxIsExcludeFederalHolidays.Checked, DbType.Boolean));
        parameters.Add(new FilterParameter("@Result", 0, DbType.Boolean, true));
        WebServices.CsReportServices.ExecuteNonQueryCommand("spa_CS_AddEditTimerSetting", parameters, out parameters);
    }

    private void LoadEndingAt()
    {
        // Reset working hours if value not valid.
        if (uxNormalDayWorkingHours.Value == null || uxWeekendDayWorkingHours.Value == null
            || (int)uxNormalDayWorkingHours.Value > 24 || (int)uxNormalDayWorkingHours.Value < 0
            || (int)uxWeekendDayWorkingHours.Value > 24 || (int)uxWeekendDayWorkingHours.Value < 0)
        {
            BindREFTimeZones();
            LoadSettings();
            return;
        }

        var normalDayStartTime = uxNormalDayStartTime.SelectedTime;
        TimeSpan weekEndingAt = new TimeSpan((int)uxNormalDayWorkingHours.Value + normalDayStartTime.Value.Hours, normalDayStartTime.Value.Minutes, normalDayStartTime.Value.Seconds);
        uxWeekEndingAt.SelectedTime = weekEndingAt;

        var weekendDayStartTime = uxWeekendDayStartTime.SelectedTime;
        TimeSpan weekendEndingAt = new TimeSpan((int)uxWeekendDayWorkingHours.Value + weekendDayStartTime.Value.Hours, weekendDayStartTime.Value.Minutes, weekendDayStartTime.Value.Seconds);
        uxWeekendEndingAt.SelectedTime = weekendEndingAt;
    }

}