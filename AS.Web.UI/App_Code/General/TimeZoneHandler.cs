using AS.Common.DBManager;
using AS.Web.SharedSession;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

/// <summary>
/// TimeZone functions
/// </summary>

public static class TimeZoneHandler
{
    public static string CurrentSettingKey()
    {
        DataTable dt = GetSettingTimeZone();
        if (dt.HasData())
            return dt.Rows[0]["TimeZoneID"].ToString();
        return string.Empty;
    }

    public static bool IsAutomaticTimeZone()
    {
        DataTable dt = GetSettingTimeZone();
        if(dt.HasData())
            return dt.Rows[0]["IsAutomaticTimezone"].ToBoolean();
        return true;
    }

    //Get list timezone bind to dropdownlist
    public static DataTable GetTimeZones()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.Add("@ASClient", SessionManager.CurrentClient, DbType.Int32);
        parameters.Add("@SiteID", SessionManager.CurrentUser.SiteID, DbType.AnsiString);
        parameters.Add("@UserID", SessionManager.CurrentUser.UserID, DbType.AnsiString);
        parameters.Add("@UserMode", SessionManager.CurrentUser.UserSecRole, DbType.AnsiString);
        parameters.Add("@EntityNumber", SessionManager.CurrentUser.EntityID, DbType.AnsiString);
        parameters.Add("@EntityTypeId", SessionManager.CurrentUser.EntityType, DbType.AnsiString);
        parameters.AddLanguageID();
        return WebServices.SecurityServices.GetReports("spa_CS_GetListTimeZone", parameters);
    }

    public static DataTable GetSettingTimeZone()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.Add("@ASClientID", SessionManager.CurrentClient, DbType.Int32);
        parameters.Add("@SiteID", SessionManager.CurrentUser.SiteID, DbType.AnsiString);
        parameters.Add("@UserID", SessionManager.CurrentUser.UserID, DbType.AnsiString);
        parameters.Add("@UserMode", SessionManager.CurrentUser.UserSecRole, DbType.AnsiString);
        return WebServices.SecurityServices.GetReports("spa_Config_GetTZByUser", parameters);
    }

    public static int SaveTimeZone(bool? isAutomaticTz = null, string tzkey = "", string tzVal = "", int daylightSavingTime = 0)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        FilterParameterCollection parametersOut = new FilterParameterCollection();
        parameters.Add("@ASClientID", SessionManager.CurrentClient, DbType.Int32);
        parameters.Add("@SiteID", SessionManager.CurrentUser.SiteID, DbType.AnsiString);
        parameters.Add("@UserID", SessionManager.CurrentUser.UserID, DbType.AnsiString);
        parameters.Add("@UserMode", SessionManager.CurrentUser.UserSecRole, DbType.AnsiString);
        parameters.Add("@UserRecId", SessionManager.CurrentUser.RecId, DbType.Guid);
        parameters.Add("@IsAutomaticTimezone", isAutomaticTz, DbType.Boolean);
        parameters.Add("@TimezoneID", tzkey, DbType.AnsiString);
        parameters.Add("@TimezoneValue", tzVal, DbType.AnsiString);
        parameters.Add("@DaylightSavingTime", daylightSavingTime, DbType.Int16);
        int result = 1;
        try
        {            
            WebServices.SecurityServices.GetReports("spa_Config_UpdateTZByUser", parameters);
        }
        catch (Exception ex)
        {
            AS.Common.Logger.LoggerManager.Error("Update time zone error: \n" + ex.ToString());
            result = 0;
        }
        // need to review
        GetCurrentDate();
        return result;
    }

    public static void GetCurrentDate()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.Add("@ASClientID", SessionManager.CurrentClient, DbType.Int32);
        parameters.Add("@SiteID", SessionManager.CurrentUser.SiteID, DbType.AnsiString);
        parameters.Add("@UserID", SessionManager.CurrentUser.UserID, DbType.AnsiString);
        parameters.Add("@UserMode", SessionManager.CurrentUser.UserSecRole, DbType.AnsiString);
        DataTable dt = WebServices.SecurityServices.GetReports("spa_Timezone_GetCurrentDateByUser", parameters);
        if (dt.HasData())
        {
            DateTime date;
            DateTime.TryParse(dt.Rows[0][0].ToString(), out date);
            SharedSessionManager.CurrentDateByUser = date;
        }
    }
}