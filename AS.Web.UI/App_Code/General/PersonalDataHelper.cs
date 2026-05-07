using AS.Common.DBManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

/// <summary>
/// Personal configuration data
/// </summary>
public static class PersonalDataHelper<T>
{
    #region User configs

    public static T RiskReport_ConfigurationData
    {
        get
        {
            return PersonalDataHelper.GetJSONConfig<T>(UserConfigNames.CONFIG_RISK_TRANS_VOLUME_ANALYSIS);
        }
        set
        {
            PersonalDataHelper.SaveConfigAsJSON(UserConfigNames.CONFIG_RISK_TRANS_VOLUME_ANALYSIS, value);
        }
    }

    #endregion
}

/// <summary>
/// Personal configuration data
/// </summary>
public static class PersonalDataHelper
{
    private const string SPA_GET_CONFIG_USER_DATA = "spa_Config_GetDataByUser";
    private const string SPA_SAVE_CONFIG_USER_DATA = "spa_Config_SaveDataByUser";

    /// <summary>
    /// Get list of UI settings to transfer to Client
    /// </summary>
    /// <param name="settings"></param>
    /// <returns></returns>
    public static Dictionary<string, string> GetUserUISettings(string[] settings = null)
    {
        var uiSettings = settings != null ? settings : new string[] { 
            UserConfigNames.CONFIG_USER_PROFILE_EXPAND_COLLAPSE_FILTERS, 
            UserConfigNames.CONFIG_USER_PROFILE_EXPAND_COLLAPSE_GRAPHS,
            UserConfigNames.CONFIG_USER_PROFILE_RISK_DETECTION_QUEUE_VIEW
        };

        Dictionary<string, string> result = new Dictionary<string, string>();
        foreach (var setting in uiSettings)
        {
            result.Add(setting, GetJSONConfig<string>(setting));
        }
        return result;
    }

    /// <summary>
    /// Get the setting object by config name
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="configName"></param>
    /// <returns></returns>
    public static T GetJSONConfig<T>(string configName)
    {
        try
        {
            var userData = GetConfigurationData(configName);
            return new JavaScriptSerializer().Deserialize<T>(userData);
        }
        catch
        {
            return default(T);
        }
    }

    /// <summary>
    /// Save user config data
    /// </summary>
    /// <param name="configName"></param>
    /// <param name="userConfig"></param>
    public static void SaveConfigAsJSON(string configName, object userConfig)
    {
        var userConfigInString = userConfig == null ? string.Empty : new JavaScriptSerializer().Serialize(userConfig);
        InsertConfigurationData(configName, userConfigInString);
    }

    /// <summary>
    /// Get user config data from database
    /// </summary>
    /// <param name="configName"></param>
    /// <returns></returns>
    private static string GetConfigurationData(string configName)
    {
        string key = SPA_GET_CONFIG_USER_DATA + configName
            + SessionManager.CurrentUser.UserID + SessionManager.CurrentUser.ASClient.ToString()
            + SessionManager.CurrentUserType.ToString();

        if (HttpRuntime.Cache[key] == null)
        {
            var parameters = new FilterParameterCollection();
            parameters.AddLoggedInUserReportingParams(false);
            parameters.Add(new FilterParameter("@ConfigName", configName, DbType.String));
            var dt = WebServices.SecurityServices.GetReports(SPA_GET_CONFIG_USER_DATA, parameters);
            if (dt.Rows.Count > 0)
            {
                string userData = dt.Rows[0]["UserData"].ToString();
                HttpRuntime.Cache.Insert(key, userData);
            }
            else
            {
                HttpRuntime.Cache.Insert(key, string.Empty);
            }
        }
        return HttpRuntime.Cache[key].ToString();
    }

    /// <summary>
    /// Save user config data to database
    /// </summary>
    /// <param name="configName"></param>
    /// <param name="userData"></param>
    /// <returns></returns>
    private static DataTable InsertConfigurationData(string configName, string userData)
    {
        string key = SPA_GET_CONFIG_USER_DATA + configName +
            SessionManager.CurrentUser.UserID + SessionManager.CurrentUser.ASClient.ToString() + SessionManager.CurrentUserType.ToString();
        if (HttpRuntime.Cache[key] != null)
            HttpRuntime.Cache.Remove(key);

        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(false);
        parameters.Add(new FilterParameter("@ConfigName", configName, DbType.String));
        parameters.Add(new FilterParameter("@UserData", userData, DbType.String));
        return WebServices.SecurityServices.GetReports(SPA_SAVE_CONFIG_USER_DATA, parameters);
    }
}

public class UserConfigNames
{
    public const string CONFIG_RISK_TRANS_VOLUME_ANALYSIS = "TransactionVolumeAnalysis";
    public const string CONFIG_USER_PROFILE_EXPAND_COLLAPSE_FILTERS = "FiltersDefaultView"; 
    public const string CONFIG_USER_PROFILE_EXPAND_COLLAPSE_GRAPHS = "GraphsDefaultView";
    public const string CONFIG_USER_PROFILE_RISK_DETECTION_QUEUE_VIEW = "RiskDetectionQueueDefaultView";
    public const string CONFIG_USER_PROFILE_SHOW_ENVIRONMENT_INDICATOR = "ShowEnvironmentIndicator";
    public const string CONFIG_RISK_DETECTIONQUEUE_CUSTOMIZE_COLUMNS = "Detectionqueue_Customize_Columns";
    //39251 – VW - Add Default Landing Page On Update My Profile Page
    public const string CONFIG_USER_PROFILE_DEFAULT_LANDING_PAGE = "DefaultLandingPage";
    //  44758 - VW CMS Case Type Default Preference    
    public const string CONFIG_USER_PROFILE_CASE_TYPE_DEFAULT = "CaseTypeDefault";
    //44894 - VW- Merchant Note Default Preferences via User Mgmt Settings
    public const string CONFIG_MERCHANT_PROFILE_SOURCE_DEFAULT_SETTING = "MerchantProfileSourceDefaultSetting";
    public const string CONFIG_RISK_REPORT_SOURCE_DEFAULT_SETTING = "RiskReportSourceDefaultSetting";
    public const string CONFIG_MERCHANT_PROFILE_CH_STATUS_DEFAULT_SETTING = "MerchantProfileCHStatusDefaultSetting";
    public const string CONFIG_MERCHANT_PROFILE_CH_TYPE_DEFAULT_SETTING = "MerchantProfileCHTypeDefaultSetting";
    public const string CONFIG_MERCHANT_PROFILE_CH_PRIORITY_DEFAULT_SETTING = "MerchantProfileCHPriorityDefaultSetting";
    public const string CONFIG_RISK_REPORT_CH_STATUS_DEFAULT_SETTING = "RiskReportCHStatusDefaultSetting";
    public const string CONFIG_RISK_REPORT_CH_TYPE_DEFAULT_SETTING = "RiskReportCHTypeDefaultSetting";
    public const string CONFIG_RISK_REPORT_CH_PRIORITY_DEFAULT_SETTING = "RiskReportCHPriorityDefaultSetting";
}