using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Core.Common.Utilities;
using AS.Security.WS.Entities;
using AS.Web.SharedSession;
using AS.Web.UI.Controls;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Xml;
using Telerik.Web.UI;
using CardTypes = WebSiteEnums.CardTypes;

/// <summary>
/// Summary description for GeneralFuncsLib
/// </summary>
public static partial class GeneralFuncsLib
{
    #region Constants

    public const string DETECTION_QUEUE_AS_VALUE = "DQ";
    public const string WORK_QUEUE_AS_VALUE = "WQ";
    public const string DISTINCT_DETECTION_QUEUE_AS_VALUE = "DT";
    public const string AGGREGATE_QUEUE_AS_VALUE = "AQ";
    public const string SUBSITE_AS_VALUE = "SS";

    public const string NA_VALUE = "N/A";
    public const string CSS_DISPLAY_NONE = "none";
    public const string NBSP = "&nbsp;";

    public const string DESC = "DESC";
    public const string ASC = "ASC";

    public const string HIERARCHY_USERS_TEXT = "Hierarchy Users";
    public const string FILE_NAME_REPLACE_CHANGE = "\\/:*?\"<>|`!~@#$%^&*(),;'?{}[]";

    #endregion Constants

    #region ReportFilter

    public static bool CheckExistObject(this DataTable table, string ColumnName, string Value)
    {
        foreach (DataRow row in table.Rows)
        {
            if (row[ColumnName].ToString() == Value)
            {
                return true;
            }
        }
        return false;
    }

    public static DataRow FindObject(this DataTable table, string ColumnName, string Value)
    {
        foreach (DataRow row in table.Rows)
        {
            if (row[ColumnName] != DBNull.Value && row[ColumnName].ToString().ToLower() == Value.ToLower())
            {
                return row;
            }
        }
        return null;
    }

    public static bool IsMerchantMode(string HierarchyMode)
    {
        foreach (DataRow row in SessionManager.AllHierarchyFilter.Rows)
        {
            if (row["HierarchyMode"].ToString() == HierarchyMode)
            {
                if (row["IsMerchant"] != DBNull.Value && (bool)row["IsMerchant"])
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static bool IsMerchantModeToSetTooltip(ReportFilter ReportFilter)
    {
        return GeneralFuncsLib.GetNextHierachyInfo(ReportFilter.CurrentValue.HierarchyMode).IsMerchant;
    }
    public static string GetGridDrilldownColumnName(string hierarchyMode, string value)
    {
        foreach (DataRow row in SessionManager.HierarchyFilterDrillDown.Rows)
        {
            if (row["CurrentHierarchyMode"].ToString() == hierarchyMode)
            {
                if (string.IsNullOrEmpty(value) || value == SessionManager.AllHierarchyFilter.FindObject("HierarchyMode", hierarchyMode)["HierarchyPrefix"].ToString())
                {
                    return row["CurrentHierarchyGridName"].ToString();
                }
                return row["NextHierarchyGridName"].ToString();
            }
        }

        return string.Empty;
    }

    public static string GetGridDrilldownHierarchyMode(string hierarchyMode, string value)
    {
        foreach (DataRow row in SessionManager.HierarchyFilterDrillDown.Rows)
        {
            if (row["CurrentHierarchyMode"].ToString() == hierarchyMode)
            {
                if (string.IsNullOrEmpty(value) || (value == SessionManager.AllHierarchyFilter.FindObject("HierarchyMode", hierarchyMode)["HierarchyPrefix"].ToString() && !string.IsNullOrEmpty(SessionManager.AllHierarchyFilter.FindObject("HierarchyMode", hierarchyMode)["HierarchyPrefix"].ToString())))
                {
                    return row["CurrentHierarchyMode"].ToString();
                }
                return row["NextHierarchyMode"].ToString();
            }
        }

        return string.Empty;
    }


    public static HierarchyDetail GetNextHierachyInfo(string hierarchyMode)
    {
        HierarchyDetail info = null;

        DataRow row = SessionManager.HierarchyFilterDrillDown.FindObject("CurrentHierarchyMode", hierarchyMode);
        row = SessionManager.AllHierarchyFilter.FindObject("HierarchyID", row["NextHierarchyID"].ToString());
        if (row != null)
        {
            info = new HierarchyDetail()
            {
                HierarchyID = row["HierarchyID"].ToString(),
                HierarchyMode = row["HierarchyMode"].ToString(),
                HierarchyName = row["HierarchyName"].ToString(),
                IsMerchant = (row["IsMerchant"] is DBNull) ? false : (bool)row["IsMerchant"]
            };

        }
        return info;
    }

    public static string GetFullGridTitleName(ReportFilter ReportFilter)
    {
        string HierarchyModeValue = ReportFilter.CurrentValue.HierarchyMode;
        string HierarchyValue = ReportFilter.CurrentValue.Value;

        string GridTitleName = string.Empty;
        string HierarchyName = GetGridTitleName(ReportFilter);

        if (HierarchyModeValue == "LAST6MERCHNUMBER")
        {
            HierarchyName = Resources.GeneralFuncsLib.GeneralFuncsLib_HierarchyName_Last6ofMid;
        }
        if (string.IsNullOrEmpty(HierarchyValue) || HierarchyValue == GetHierarchyInfo(HierarchyModeValue).Prefix)
        {
            if (SessionManager.HierarchyFilterExtend != null)
            {
                DataTable tdExtend = SessionManager.HierarchyFilterExtend;
                var _temp = tdExtend.Select("IsEnableInclusion = 1 and HierarchyMode= '" + HierarchyModeValue + "' and Url='" + HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath + "'");
                if (_temp != null && _temp.Count() > 0)
                {
                    return _temp.FirstOrDefault()["HierarchyName"].ToString();
                }
            }

            GridTitleName = GetAllTitleName(ReportFilter);
        }
        else
        {
            if (IsMerchantMode(HierarchyModeValue))
            {
                GridTitleName = HierarchyValue + ": " + GeneralFuncsLib.GetMerchantName(HierarchyValue);
            }
            else
            {
                //MERCHANTNUMBER, MERCHANTNAME, EMAIL
                if (HierarchyModeValue == "MERCHANTNAME" || HierarchyModeValue == "EMAIL")
                {
                    if (HierarchyModeValue == "MERCHANTNAME")
                    {
                        HierarchyName = Resources.GeneralFuncsLib.GeneralFuncsLib_HierarchyName_MerchantName;
                    }

                    if (!HierarchyValue.Contains('*'))
                    {
                        HierarchyValue = "*" + HierarchyValue + "*";
                    }
                }
                GridTitleName = HierarchyName + " " + HierarchyValue;
            }
        }


        return GridTitleName;
    }

    public static string GetGridTitleName(ReportFilter ReportFilter)
    {
        string HierarchyModeValue = ReportFilter.CurrentValue.HierarchyMode;

        foreach (DataRow row in SessionManager.HierarchyFilterDrillDown.Rows)
        {
            if (row["CurrentHierarchyMode"].ToString() == HierarchyModeValue)
            {

                return row["CurrentHierarchyGridName"].ToString();
            }
        }

        return string.Empty;
    }

    public static string GetAllTitleName(ReportFilter ReportFilter)
    {
        string HierarchyModeValue = ReportFilter.CurrentValue.HierarchyMode;

        foreach (DataRow row in SessionManager.HierarchyFilterDrillDown.Rows)
        {
            if (row["CurrentHierarchyMode"].ToString() == HierarchyModeValue)
            {

                return row["CurrentAllHierarchyText"].ToString();
            }
        }

        return string.Empty;
    }

    public static string GetDateFilterText(ReportFilter ReportFilter)
    {
        string dateText = string.Empty;
        string beginDateText = string.Empty;
        string endDateText = string.Empty;
        switch (ReportFilter.CurrentValue.DateOption)
        {
            case AS.Web.UI.Controls.DateOptionMode.Daily:
                {
                    beginDateText = ReportFilter.CurrentValue.DateOptionValue.From.ToGenericDateString();
                    dateText = string.Format("{0}", beginDateText);
                }
                break;
            case AS.Web.UI.Controls.DateOptionMode.Monthly:
                {
                    beginDateText = ReportFilter.CurrentValue.DateOptionValue.From.GetFirstDayOfMonth().ToGenericDateString();
                    if (ReportFilter.CurrentValue.DateOptionValue.From.Year == DateTime.Today.Year && ReportFilter.CurrentValue.DateOptionValue.From.Month == DateTime.Today.Month)
                        endDateText = DateTime.Today.ToGenericDateString();
                    else
                        endDateText = ReportFilter.CurrentValue.DateOptionValue.From.GetLastDayOfMonth().ToGenericDateString();
                }
                dateText = string.Format("{0} - {1}", beginDateText, endDateText);
                break;
            case AS.Web.UI.Controls.DateOptionMode.DateRange:
                {
                    beginDateText = ReportFilter.CurrentValue.DateOptionValue.From.ToGenericDateString();
                    endDateText = ReportFilter.CurrentValue.DateOptionValue.To.ToGenericDateString();
                }
                dateText = string.Format("{0} - {1}", beginDateText, endDateText);
                break;
            case AS.Web.UI.Controls.DateOptionMode.TrailingTwelveMonths:
                {
                    beginDateText = ReportFilter.CurrentValue.DateOptionValue.From.ToGenericDateString();
                    endDateText = ReportFilter.CurrentValue.DateOptionValue.From.Date.AddMonths(-12).ToGenericDateString();
                }
                dateText = string.Format("{0} - {1}", beginDateText, endDateText);
                break;

            case AS.Web.UI.Controls.DateOptionMode.Yearly:
                {
                    beginDateText = ReportFilter.CurrentValue.DateOptionValue.From.Year.ToString();
                }
                dateText = string.Format("{0}", beginDateText);
                break;


        }

        return "(" + dateText + ")";
    }

    public static string GetValueForActivityFunc(bool isMerchant, ReportFilter ReportFilter)
    {
        string hierachyMode = ReportFilter.CurrentValue.HierarchyMode;
        string hierachyValue = ReportFilter.CurrentValue.Value;
        string value = string.Empty;
        if (isMerchant)
        {
            value = "View " + hierachyValue;
        }
        else
        {
            if (string.IsNullOrEmpty(hierachyValue))
            {
                if (GeneralFuncsLib.IsMerchantMode(ReportFilter.CurrentValue.HierarchyMode))
                    value = "Search all Merchants";
                else
                    value = "Search all " + hierachyMode + "s";
            }
            else
            {
                value = string.Format("Search by {0} {1}", hierachyMode, hierachyValue);
            }
        }

        return value;
    }

    public static string GetDateTextForActivityFunc(bool isMerchant, ReportFilter ReportFilter)
    {
        string hierachyMode = ReportFilter.CurrentValue.HierarchyMode;
        string hierachyValue = ReportFilter.CurrentValue.Value;

        string dateText = string.Empty;
        string beginDateText = string.Empty;
        string endDateText = string.Empty;

        switch (ReportFilter.CurrentValue.DateOption)
        {
            case AS.Web.UI.Controls.DateOptionMode.Daily:
                {
                    beginDateText = ReportFilter.CurrentValue.DateOptionValue.From.ToGenericDateString();
                    dateText = string.Format("on {0}", beginDateText);
                }
                break;
            case AS.Web.UI.Controls.DateOptionMode.Monthly:
                {
                    beginDateText = ReportFilter.CurrentValue.DateOptionValue.From.GetFirstDayOfMonth().ToGenericDateString();
                    if (ReportFilter.CurrentValue.DateOptionValue.From.Year == DateTime.Today.Year && ReportFilter.CurrentValue.DateOptionValue.From.Month == DateTime.Today.Month)
                        endDateText = DateTime.Today.ToGenericDateString();
                    else
                        endDateText = ReportFilter.CurrentValue.DateOptionValue.From.GetLastDayOfMonth().ToGenericDateString();
                }
                dateText = string.Format("from {0} to {1}", beginDateText, endDateText);
                break;
            case AS.Web.UI.Controls.DateOptionMode.DateRange:
                {
                    beginDateText = ReportFilter.CurrentValue.DateOptionValue.From.ToGenericDateString();
                    endDateText = ReportFilter.CurrentValue.DateOptionValue.To.ToGenericDateString();
                }
                dateText = string.Format("from {0} to {1}", beginDateText, endDateText);
                break;
        }

        return dateText;
    }

    public static HierarchyDetail GetMerchantHierarchyInfo()
    {
        HierarchyDetail info = null;
        if (SessionManager.MerchantHierarchyFilter == null)
        {


            DataRow row = SessionManager.AllHierarchyFilter.FindObject("IsMerchant", "true");

            if (row != null)
            {
                info = new HierarchyDetail()
                {
                    HierarchyID = row["HierarchyID"].ToString(),
                    HierarchyMode = row["HierarchyMode"].ToString(),
                    HierarchyName = row["HierarchyName"].ToString(),
                    UserMode = row["UserMode"].ToString(),
                    Prefix = row["HierarchyPrefix"].ToString(),
                    IsMerchant = true,
                    ShowPrefix = (row["ShowPrefix"] is DBNull) ? false : (bool)row["ShowPrefix"],
                    ChildOf = (row["ChildOf"] is DBNull) ? "" : row["ChildOf"].ToString()
                };

            }
            SessionManager.MerchantHierarchyFilter = info;
        }
        else
        {
            info = SessionManager.MerchantHierarchyFilter;
        }
        return info;
    }
    public static HierarchyDetail GetHierarchyInfo(int entityType)
    {
        HierarchyDetail info = null;

        DataRow row = SessionManager.AllHierarchyFilter.FindObject("EntityType", entityType.ToString());

        if (row != null)
        {
            info = new HierarchyDetail()
            {
                HierarchyID = row["HierarchyID"].ToString(),
                HierarchyMode = row["HierarchyMode"].ToString(),
                HierarchyName = row["HierarchyName"].ToString(),
                UserMode = row["UserMode"].ToString(),
                Prefix = row["HierarchyPrefix"].ToString(),
                IsMerchant = (row["IsMerchant"] is DBNull) ? false : (bool)row["IsMerchant"],
                ShowPrefix = (row["ShowPrefix"] is DBNull) ? false : (bool)row["ShowPrefix"],
                ChildOf = (row["ChildOf"] is DBNull) ? "" : row["ChildOf"].ToString()
            };
        }

        return info;
    }
    public static HierarchyDetail GetHierarchyInfo(string hierarchyMode)
    {
        HierarchyDetail info = null;

        DataRow row = SessionManager.AllHierarchyFilter.FindObject("HierarchyMode", hierarchyMode);

        if (row != null)
        {
            info = new HierarchyDetail()
            {
                HierarchyID = row["HierarchyID"].ToString(),
                HierarchyMode = row["HierarchyMode"].ToString(),
                HierarchyName = row["HierarchyName"].ToString(),
                UserMode = row["UserMode"].ToString(),
                Prefix = row["HierarchyPrefix"].ToString(),
                IsMerchant = (row["IsMerchant"] is DBNull) ? false : (bool)row["IsMerchant"],
                ShowPrefix = (row["ShowPrefix"] is DBNull) ? false : (bool)row["ShowPrefix"],
                ChildOf = (row["ChildOf"] is DBNull) ? "" : row["ChildOf"].ToString()
            };
        }

        return info;
    }


    #endregion

    #region Risk Hierarchy Filter

    public static string GetRiskHierarchyFilterText(string HierarchyFilterMode)
    {
        foreach (DataRow row in SessionManager.RiskHierarchyFilter.Rows)
        {
            if (row["HierarchyFilterMode"].ToString() == HierarchyFilterMode)
            {
                return row["HierarchyFilterText"].ToString();
            }
        }
        return string.Empty;
    }
    #endregion

    #region Message Hierarchy Filter

    public static string GetMessageHierarchyFilterText(string HierarchyFilterMode)
    {
        foreach (DataRow row in SessionManager.MessageHierarchyFilter.Rows)
        {
            if (row["HierarchyFilterMode"].ToString() == HierarchyFilterMode)
            {
                return row["DisplayedText"].ToString();
            }
        }
        return string.Empty;
    }
    #endregion

    #region Risk Parameter Assignment

    public static string GetDouble4Precision(string doubleStr)
    {
        if (string.IsNullOrEmpty(doubleStr))
            return doubleStr;

        return double.Parse(doubleStr).ToString("#,#0");
    }

    public static string FormatPrecision(string doubleStr, int precision)
    {
        if (string.IsNullOrEmpty(doubleStr))
            return doubleStr;
        if (precision == 2)
            return double.Parse(doubleStr).ToString("#,#0.00");
        else if (precision == 4)
            return double.Parse(doubleStr).ToString("#,#0.0000");
        else
            return double.Parse(doubleStr).ToString("#,#0"); ;
    }

    public static string FormatParameterDataType(string dataType, string dataValue)
    {
        return FormatParameterDataType(dataType, dataValue, 0);
    }
    public static string FormatParameterDataType(string dataType, string dataValue, int precision)
    {
        return FormatParameterDataType(dataType, dataValue, precision, false);
    }
    public static string FormatParameterDataType(string dataType, string dataValue, int precision, bool isHtmlMode, bool isEdit = false)
    {
        string displayedValue;
        string value;

        if (dataValue.IsNullOrEmpty())
        {
            if (!isEdit)
            {
                displayedValue = string.Empty;
            }
            else
            {
                displayedValue = string.Empty;
            }
        }
        else
        {
            value = precision > 0 ? FormatPrecision(dataValue, precision) : GeneralFuncsLib.GetDouble4Precision(dataValue);
            if (dataType == SessionManager.CurrencySymbol || dataType == "%")
            {
                string formatString = "{1}{0}";
                if (dataType == "%")
                    formatString = "{0}{1}";

                if (decimal.Parse(dataValue) >= 0)
                {
                    displayedValue = string.Format(formatString, value, dataType);
                }
                else
                {
                    if (isHtmlMode)
                        displayedValue = string.Format("<span style=\"color:red;\">(" + formatString + ")</span>", value.Replace("-", string.Empty), dataType);
                    else
                        displayedValue = string.Format("(" + formatString + ")", value.Replace("-", string.Empty), dataType);
                }
            }
            else if (dataType == "days")
                displayedValue = string.Format("{0} {1}", value, "day(s)");

            else
                displayedValue = string.Format("{0}", value);
        }

        return displayedValue;
    }

    public static string FormatParameterExport(string dataType, string dataValue, int precision)
    {
        return FormatParameterExport(dataType, dataValue, precision, false);
    }
    public static string FormatParameterExport(string dataType, string dataValue, int precision, bool isEdit)
    {
        string displayedValue;
        string value;

        if (dataValue.IsNullOrEmpty())
        {
            if (!isEdit)
            {
                displayedValue = string.Empty;
            }
            else
            {
                displayedValue = string.Empty;
            }
        }
        else
        {
            value = precision > 0 ? FormatPrecision(dataValue, precision) : GeneralFuncsLib.GetDouble4Precision(dataValue);
            if (dataType == SessionManager.CurrencySymbol)
            {
                if (decimal.Parse(dataValue) >= 0)
                {
                    displayedValue = string.Format("{1}{0}", value, dataType);
                }
                else
                {
                    displayedValue = string.Format("-{1}{0}", value.Replace("-", string.Empty), dataType);
                }
            }
            else if (dataType == "%")
            {
                displayedValue = string.Format("{0}{1}", value, dataType);
            }
            else if (dataType == "days")
                displayedValue = string.Format("{0} {1}", value, "day(s)");

            else
                displayedValue = string.Format("{0}", value);
        }

        return displayedValue;
    }

    public static string FormatLowHighThreshold(string ThresholdLow, string ThresholdHigh, string ThresholdType)
    {
        return FormatLowHighThreshold(ThresholdLow, ThresholdHigh, ThresholdType, false);
    }

    public static string FormatLowHighThreshold(string ThresholdLow, string ThresholdHigh, string ThresholdType, bool isEdit, int precision = 0)
    {
        string displayedValue;

        var low = FormatParameterDataType(ThresholdType, ThresholdLow, precision, false, isEdit);
        var high = FormatParameterDataType(ThresholdType, ThresholdHigh, precision, false, isEdit);

        if (low.IsNullOrEmpty() || high.IsNullOrEmpty())
        {
            displayedValue = string.Empty;
        }
        else
        {
            var ThresholdLowFormat = FormatParameterDataType(ThresholdType, ThresholdLow, precision, false, isEdit);
            var ThresholdHighFormat = FormatParameterDataType(ThresholdType, ThresholdHigh, precision, false, isEdit);
            if (string.IsNullOrEmpty(ThresholdLowFormat) || string.IsNullOrEmpty(ThresholdHighFormat))
            {
                displayedValue = string.Empty;
            }
            else
            {
                displayedValue = ThresholdLowFormat + " - " + ThresholdHighFormat;
            }

        }
        return displayedValue;
    }

    #endregion

    public static string BaseUrl
    {
        get
        {
            return HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + HttpContext.Current.Request.ApplicationPath;
        }
    }
    public static bool IsNullOrEmpty(this string value)
    {
        return string.IsNullOrEmpty(value);
    }
    public static DataTable CreateDummyData(string[] columns, int rowCount)
    {
        DataTable tb = new DataTable("tb");
        DataColumn col = null;
        DataRow row = null;
        foreach (string colName in columns)
        {
            if (colName.IndexOf(":") > 0)
            {
                string[] colNameDetail = colName.Split(':');
                switch (colNameDetail[1].ToLower())
                {
                    case "int":
                        col = new DataColumn(colNameDetail[0], typeof(int));
                        tb.Columns.Add(col);
                        break;
                    case "datetime":
                        col = new DataColumn(colNameDetail[0], typeof(DateTime));
                        tb.Columns.Add(col);
                        break;
                    case "string":
                        col = new DataColumn(colNameDetail[0], typeof(string));
                        tb.Columns.Add(col);
                        break;
                    case "decimal":
                        col = new DataColumn(colNameDetail[0], typeof(decimal));
                        tb.Columns.Add(col);
                        break;
                    case "bool":
                        col = new DataColumn(colNameDetail[0], typeof(bool));
                        tb.Columns.Add(col);
                        break;
                }
            }
            else
            {
                col = new DataColumn(colName);
                tb.Columns.Add(col);
            }
        }

        for (int i = 1; i <= rowCount; i++)
        {
            row = tb.NewRow();
            foreach (string colName in columns)
            {

                if (colName.IndexOf(":") > 0)
                {
                    string[] colNameDetail = colName.Split(':');
                    switch (colNameDetail[1].ToLower())
                    {
                        case "int":
                            row[colNameDetail[0]] = i;
                            break;
                        case "datetime":
                            row[colNameDetail[0]] = DateTime.Now;
                            break;
                        case "string":
                            row[colNameDetail[0]] = colNameDetail[0] + " " + i;
                            break;
                        case "decimal":
                            row[colNameDetail[0]] = 123.456f;
                            break;
                        case "bool":
                            row[colNameDetail[0]] = i % 2 == 0;
                            break;
                    }
                }
                else
                {
                    row[colName] = colName + " " + i;
                }
            }
            tb.Rows.Add(row);
        }
        return tb;
    }
    public static bool SaveLoginUserData(string username, SecurePage page)
    {
        User user = WebServices.SecurityServices.GetUser(SessionManager.CurrentClient, username);

        SessionManager.CurrentUser = user;
        SessionManager.IsLoggedIn = true;
        // TK 41446 
        SharedSessionManager.UserEntityId = user.EntityID;
        SharedSessionManager.UserEntityType = user.EntityType;

        PermissionCollection permissions = new PermissionCollection();
        if (SessionManager.SingleSignOnMenuMod)
            permissions = WebServices.SecurityServices.GetPermissionsForUserSSO(SessionManager.CurrentClient, user.UserID, SessionManager.SSOSecurityLevel);
        else
            permissions = WebServices.SecurityServices.GetPermissionsForUser(SessionManager.CurrentClient, user.UserID);

        //43745: Exclude Permission: Login from Landing page
        permissions = ExcludeAccessPermission(permissions);

        string per_codes = ",";
        foreach (Permission permission in permissions)
        {
            per_codes += permission.PermissionCode + ",";
        }
        SessionManager.CurrentUserPermissions = per_codes;

        //Check user PCI active
        if (page.IsUserWithPermission("SiteAccessPCIAdmin") || page.IsUserWithPermission("HierarchySiteAccessPCIAdmin") || page.IsUserWithPermission("MerchantSiteAccessPCIAdmin"))
        {
            string userID = username;
            if (GeneralFuncsLib.GetDataOfExtendedSetting("AllowChainAccessToPCIWithOldestMerchant") == "true" && SessionManager.CurrentUser.EntityType == 11)
            {
                FilterParameterCollection paras = new FilterParameterCollection();
                paras.AddLoggedInUserReportingParams();
                paras.AddLoggedInUserPrimaryUserID();
                DataTable dtUserChain = WebServices.SecurityServices.GetReports("spa_SEC_GetUserChain", paras);
                if (dtUserChain != null && dtUserChain.Rows.Count > 0)
                {
                    userID = dtUserChain.Rows[0]["MerchantNumber"].ToString();
                }
                else
                {
                    userID = username;
                }
            }
            if (SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.Merchant)
            {
                userID = SessionManager.CurrentUser.EntityID;
            }
            var getUsersResponse = PCIServiceClient.Instance.GetUsers(WebSiteSettings.PCIApplicationId, new AS.VW.PCI.Api.Client.Models.Requests.GetUsersRequest
            {
                ASClient = SessionManager.CurrentClient,
                UserName = userID
            });
            var pciUser = getUsersResponse?.Data;
            string isUserActivePCI = pciUser == null ? "0" : pciUser.ActiveStatus.ToString();

            bool isHierachyPrimaryUser = (SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.Hierarchy && SessionManager.CurrentUser.UserSecRole.Contains("PRI"));

            bool isNotValidPCIUser = isUserActivePCI == "0"
                           || (isUserActivePCI == "-1" && (SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.Merchant || isHierachyPrimaryUser));

            if (isNotValidPCIUser)
            {
                SessionManager.CurrentUserPermissions = SessionManager.CurrentUserPermissions.Replace(",SiteAccessPCIAdmin,", ",");
                SessionManager.CurrentUserPermissions = SessionManager.CurrentUserPermissions.Replace(",HierarchySiteAccessPCIAdmin,", ",");
                SessionManager.CurrentUserPermissions = SessionManager.CurrentUserPermissions.Replace(",MerchantSiteAccessPCIAdmin,", ",");
            }
        }
        //End of check PCI user active

        SessionManager.CurrentUserRoles = WebServices.SecurityServices.GetHierarchysOfUser(SessionManager.CurrentClient, SessionManager.CurrentUser.UserID, 0);
        if (SessionManager.CurrentUserRoles == null || SessionManager.CurrentUserRoles.Count == 0)
        {
            return false;
        }
        foreach (Hierarchy item in SessionManager.CurrentUserRoles)
        {
            if (item.SystemId == WebSiteSettings.DefaultSystem)
            {
                SessionManager.CurrentHierarchyId = item.HierarchyID;
                break;
            }
        }
        //get user type        
        string userType = SessionManager.CurrentUserRoles[0].HierarchyCode.ToUpper();
        userType = userType.Split('_')[0];
        switch (userType)
        {
            case "SYSADMIN":
                SessionManager.CurrentUserType = WebSiteEnums.UserHierarchyMode.AS;
                SessionManager.CurrentSystem = WebSiteConstants.AS_SYSTEM_CS;
                break;
            case "CSUSERS":

                if (user.UserType == 1)
                {
                    SessionManager.CurrentUserType = WebSiteEnums.UserHierarchyMode.AS;
                }
                else
                {
                    SessionManager.CurrentUserType = WebSiteEnums.UserHierarchyMode.CS;
                }
                SessionManager.CurrentSystem = WebSiteConstants.AS_SYSTEM_CS;
                break;
            case "SUSERS":
                SessionManager.CurrentUserType = WebSiteEnums.UserHierarchyMode.Site;
                SessionManager.CurrentSystem = WebSiteConstants.AS_SYSTEM_MS;
                break;
            case "HUSERS":
                SessionManager.CurrentUserType = WebSiteEnums.UserHierarchyMode.Hierarchy;
                SessionManager.CurrentSystem = WebSiteConstants.AS_SYSTEM_MS;
                break;
            case "MUSERS":
                SessionManager.CurrentUserType = WebSiteEnums.UserHierarchyMode.Merchant;
                SessionManager.CurrentSystem = WebSiteConstants.AS_SYSTEM_MS;
                break;
            case "HQUSERS":
                SessionManager.CurrentUserType = WebSiteEnums.UserHierarchyMode.Headquarter;
                SessionManager.CurrentSystem = WebSiteConstants.AS_SYSTEM_MS;
                break;
            default:
                SessionManager.CurrentUserType = WebSiteEnums.UserHierarchyMode.CS;
                SessionManager.CurrentSystem = WebSiteConstants.AS_SYSTEM_CS;
                break;
        }
        switch (WebSiteSettings.WebSiteType.ToLower())
        {
            case "cs":
                if (SessionManager.CurrentUserType != WebSiteEnums.UserHierarchyMode.AS && SessionManager.CurrentUserType != WebSiteEnums.UserHierarchyMode.CS)
                {
                    return false;
                }
                break;
            case "ms":
                if (SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.AS || SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.CS)
                {
                    return false;
                }
                break;
            default:
                break;
        }
        //Get Theme
        ASThemeCollection themes = WebServices.SecurityServices.GetASThemsByUser(SessionManager.CurrentUser.ASClient, SessionManager.CurrentUser.UserID, 0);
        if (themes.Count > 0)
        {
            SessionManager.CurrentUserTheme = themes[0];
        }
        else
        {
            SessionManager.CurrentUserTheme = new ASTheme(1, "Default", "", "", DateTime.Now, "1");
        }

        string userMode = GetUserMode();
        SetAutheticatedData(user.ASClient + "|" + user.RecId.ToString() + "|" + userMode, "web_user");
        //check using for market data
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.Add(new FilterParameter("@ASClient", user.ASClient, DbType.Int32));
        DataTable result = WebServices.SecurityServices.GetReports("spa_SEC_GetClientsInfo", parameters);
        if (result != null && result.Rows.Count > 0)
        {
            RiskSessionManager.IsUsingMarketData = bool.Parse(result.Rows[0]["IsUsingMarketData"].ToString());
            // Check Client has IsAutoCheckWork
            RiskSessionManager.IsAutoCheckWork = bool.Parse(result.Rows[0]["IsAutoCheckWork"].ToString());
        }

        LoadClientInfo();
        //49947 - WRFC - Merchant has an error when trying to add a new user
        bool hasSyn1099K = CheckPermissionSyn1099K(SessionManager.CurrentSystem == WebSiteConstants.AS_SYSTEM_MS, SessionManager.CurrentUser);
        if (!hasSyn1099K)
        {
            SessionManager.CurrentUserPermissions = SessionManager.CurrentUserPermissions.Replace(string.Format("{0},", WebSiteConstants.SEC_PERMISSION_SITE_JUMP_1099K), "");
        }

        //get user view
        SessionManager.CurrentUserViewMode |= page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_CC) ? (int)WebSiteEnums.SecViewMode.CC : 0;
        SessionManager.CurrentUserViewMode |= page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_DDA) ? (int)WebSiteEnums.SecViewMode.DDA : 0;
        SessionManager.CurrentUserViewMode |= page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_TAX_ID) ? (int)WebSiteEnums.SecViewMode.TAX : 0;

        //save login url host request
        SessionManager.CurrentUrl = page.Request.Url.Host;
        return true;
    }

    public static PermissionCollection ExcludeAccessPermission(PermissionCollection permissions)
    {
        if (!string.IsNullOrEmpty(SessionManager.ExcludeAccessPermission))
        {
            string[] excludeAccessPermissions = SessionManager.ExcludeAccessPermission.Split(',');
            foreach (string perAccess in excludeAccessPermissions)
            {
                if (string.IsNullOrEmpty(perAccess)) continue;
                foreach (Permission permission in permissions)
                {
                    if (permission.PermissionCode.Equals(perAccess))
                    {
                        permissions.Remove(permission);
                        break;
                    }
                }
            }
        }

        return permissions;
    }
    public static void LoadClientInfo()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.Add(new FilterParameter("@ASClient", SessionManager.CurrentClient, System.Data.DbType.Int32));
        parameters.AddLanguageID();
        DataTable clientInfo = WebServices.SecurityServices.GetReports("spa_GetContactUsInfo", parameters);
        if (clientInfo != null && clientInfo.Rows.Count > 0)
        {
            SessionManager.ClientInfo = new ClientInfo();
            SessionManager.ClientInfo.ClientName = clientInfo.Rows[0]["RefTblCol1"].ToString();
            SessionManager.ClientInfo.Address1 = clientInfo.Rows[0]["RefTblCol2"].ToString();
            SessionManager.ClientInfo.Address2 = clientInfo.Rows[0]["RefTblCol3"].ToString();
            SessionManager.ClientInfo.Zip = clientInfo.Rows[0]["RefTblCol4"].ToString();
            SessionManager.ClientInfo.Phone = clientInfo.Rows[0]["RefTblCol5"].ToString();
            SessionManager.ClientInfo.Fax = clientInfo.Rows[0]["RefTblCol6"].ToString();
            SessionManager.ClientInfo.ClientEmail = clientInfo.Rows[0]["RefTblCol7"].ToString();
            SessionManager.ClientInfo.ContactEmail = clientInfo.Rows[0]["RefTblCol8"].ToString();
            SessionManager.ClientInfo.NoReplyEmail = clientInfo.Rows[0]["RefTblCol9"].ToString();
            SessionManager.ClientInfo.FinancialIntitutions = clientInfo.Rows[0]["FinancialIntitutions"].ToString();
            SessionManager.ClientInfo.DirectMerchants = clientInfo.Rows[0]["DirectMerchants"].ToString();
        }
    }

    public static int SaveUserActivity(string merchantNumber, string activity)
    {
        return SaveUserActivity(merchantNumber, activity, false);
    }

    public static int SaveUserActivity(string merchantNumber, string activity, bool isMCFRisk)
    {
        string spaName = isMCFRisk ? "spa_RM_MCF_SaveUserActivity" : "spa_rm_cs_SaveUserActivity";
        FilterParameterCollection _paramsIn = new FilterParameterCollection();
        FilterParameterCollection _paramsOut = new FilterParameterCollection();
        _paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        _paramsIn.Add(new FilterParameter("@MerchantNumber", merchantNumber, DbType.AnsiString));
        _paramsIn.Add(new FilterParameter("@Activity", activity, DbType.AnsiString));

        int res = WebServices.CsReportServices.ExecuteNonQueryCommand(spaName, _paramsIn, out _paramsOut);
        return res;
    }

    static void SetAutheticatedData(string name, string roles)
    {
        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, name, DateTime.Now, DateTime.Now.AddMinutes(45), false, roles);
        string hash = FormsAuthentication.Encrypt(ticket);
        HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hash);
        cookie.HttpOnly = true;
        cookie.Secure = FormsAuthentication.RequireSSL;
        HttpContext.Current.Response.Cookies.Add(cookie);
    }

    public static void SetCookie(string key, string value)
    {
        value = AS.Common.DataProtection.Cryptophy.EncryptText(value);
        HttpCookie cookie = new HttpCookie(key, value);
        cookie.HttpOnly = true;
        cookie.Secure = true;
        HttpContext.Current.Response.Cookies.Add(cookie);
    }

    public static string GetCookie(string key)
    {
        if (HttpContext.Current.Request.Cookies.AllKeys.Contains(key))
        {
            string value = HttpContext.Current.Request.Cookies[key].Value;
            return AS.Common.DataProtection.Cryptophy.DecryptText(value);
        }
        return null;
    }
    /// <summary>
    /// Get File Name for Exporting
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static string FormatFileName(string fileName)
    {
        fileName = fileName.Replace(" ", "");//Remove space
        for (int index = 0; index < FILE_NAME_REPLACE_CHANGE.Length; index++)
        {
            fileName = fileName.Replace(FILE_NAME_REPLACE_CHANGE[index], '_');
        }
        return fileName;

    }

    public static string BuildIntruderInfoForQueryStr(string sourceName, string[] keyNames)
    {
        string Temp = string.Empty;
        Temp += "&" + WebSiteConstants.INTRUDER_SOURCE_PARAM_NAME + "=" + sourceName;
        Temp += "&" + WebSiteConstants.INTRUDER_KEY_PARAM_NAME + "=";
        foreach (string key in keyNames)
        {
            Temp += key + WebSiteConstants.INTRUDER_KEY_SEPERATOR;
        }
        if (Temp.EndsWith(WebSiteConstants.INTRUDER_KEY_SEPERATOR)) Temp = Temp.TrimEnd(WebSiteConstants.INTRUDER_KEY_SEPERATOR.ToCharArray());

        return Temp;
    }
    public static string GetRequestFileName()
    {
        return HttpContext.Current.Request.Url.Segments.Length > 0 ? HttpContext.Current.Request.Url.Segments[HttpContext.Current.Request.Url.Segments.Length - 1] : string.Empty;
    }
    public static string GetLegalFileName(this string fileName)
    {
        string specChar = "\\/:*?\"<>|";//  \/:*?"<>|
        fileName = fileName.Replace(" ", "");//Remove space
        for (int index = 0; index < specChar.Length; index++)
        {
            fileName = fileName.Replace(specChar[index], '_');
        }
        return fileName;
    }
    public static VisionWebClient GetClientInfo(int asClientId)
    {
        VisionWebClient client = null;
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.Add("@ASClient", asClientId, DbType.Int32, false);
        DataTable clientInfo = WebServices.SecurityServices.GetReports("spa_SEC_GetClient", parameters);
        if (clientInfo.Rows.Count > 0)
        {
            client = new VisionWebClient();
            DataRow clientRow = clientInfo.Rows[0];
            if (clientRow["ASClient"] != DBNull.Value)
                client.ASClient = Convert.ToInt32(clientRow["ASClient"]);
            if (clientRow["ClientAbbreviation"] != DBNull.Value)
                client.ClientAbbreviation = clientRow["ClientAbbreviation"].ToString();
            if (clientRow["ClientName"] != DBNull.Value)
                client.ClientName = clientRow["ClientName"].ToString();
            if (clientRow["ActvStatus"] != DBNull.Value)
                client.ActvStatus = clientRow["ActvStatus"].ToString();
            if (clientRow["DateCreated"] != DBNull.Value)
                client.DateCreated = Convert.ToDateTime(clientRow["DateCreated"]);
            if (clientRow["CreatedBy"] != DBNull.Value)
                client.CreatedBy = clientRow["CreatedBy"].ToString();
            if (clientRow["DateUpdated"] != DBNull.Value)
                client.DateUpdated = Convert.ToDateTime(clientRow["DateUpdated"]);
            if (clientRow["UpdatedBy"] != DBNull.Value)
                client.UpdatedBy = clientRow["UpdatedBy"].ToString();
            if (clientRow["MarketData"] != DBNull.Value)
                client.UpdatedBy = clientRow["MarketData"].ToString();
            if (clientRow["IsAutoCheckWork"] != DBNull.Value)
                client.IsAutoCheckWork = (bool)clientRow["IsAutoCheckWork"];
            if (clientRow["TransHist30"] != DBNull.Value)
                client.TransHist30 = (bool)clientRow["TransHist30"];
        }
        return client;
    }

    #region Risk Management

    public static bool ReadOnlyRiskManagementEnableStatus(SecurePage page)
    {
        return !page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_READONLY_RISK_MGMT_MS);
    }

    //get queueing feature permission
    public static bool HasQueuingMechanismFeature
    {
        get
        {
            var hasQueuingMechanism =
                GeneralFuncsLib.GetClientExtendedSetting("HasQueuingMechanism");
            return hasQueuingMechanism.Data != null
                && hasQueuingMechanism.Data.ToLower().Equals("true");
        }
    }

    public static bool IsDistinctAssignment()
    {
        //check if the client has "Exclude from DQ Distinct" or not
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.Add(new FilterParameter("@ASClient", SessionManager.CurrentClient, DbType.Int32));
        DataTable result = WebServices.SecurityServices.GetReports("spa_SEC_GetClientsInfo", parameters);

        if (result.Rows.Count > 0)
        {
            return bool.Parse(result.Rows[0]["IsDistinctAssignment"].ToString());
        }
        else
        {
            return false;
        }
    }


    public static string NvlString(object val)
    {
        return (val == null || val == DBNull.Value ? string.Empty : val.ToString());
    }

    public static string FormatCurrency(object val)
    {
        return FormatNumber(val, "{0:C}");
    }

    public static string FormatCurrency(object val, string format = "C")
    {
        return val.ToDecimal().ToString(format, CultureInfo.CreateSpecificCulture(SessionManager.CurrencyFortmat));
    }

    public static string FormatDate(object val)
    {
        return (val == null || NvlString(val).Length == 0 ? string.Empty : string.Format("{0:MM/dd/yyyy}", val));
    }
    public static string FormatMonthDay(object date)
    {
        return (date == null || NvlString(date).Length == 0 ? string.Empty : string.Format("{0:MM/dd}", date));
    }
    public static string FormatPercent(object val)
    {
        return (val == null || val == DBNull.Value || string.IsNullOrEmpty(val.ToString())) ? string.Empty : decimal.Parse(val.ToString()).ToString("0.00") + "%";
    }

    public static string FormatCurrency2(this object value)
    {
        return value.IsNullOrEmpty() ? string.Empty : AS.Common.Formater.FormatData.FormatCurrency(value, SessionManager.CurrencyFortmat);
    }
    public static string FormatPhone(this object value)
    {
        return value.IsNullOrEmpty() ? string.Empty : AS.Common.Formater.FormatData.FormatPhoneNumber(value.ToString());
    }
    public static string FormatString(object value)
    {
        return value.IsNullOrEmpty() ? "&nbsp;" : HttpUtility.HtmlEncode(value.ToString()).Replace("\r", "").Replace("\n", "<br/>");
    }

    public static bool IsNullData(this object value)
    {
        return value == null || value == DBNull.Value;
    }

    public static bool IsNotNullData(this object value)
    {
        return !value.IsNullData();
    }

    public static bool IsNullOrEmpty(this object value)
    {
        return value.IsNullData() || value.ToString().IsNullOrEmpty();
    }

    //public static bool IsNullOrEmpty(this string value)
    //{
    //    return string.IsNullOrEmpty(value);
    //}

    public static string ConvertIntToPercent(object val)
    {
        return Decimal.Parse(val.ToString()).ToString("#,#0") + "%";
    }

    public static string FormatInteger(object val)
    {
        return (val == null || val == DBNull.Value || string.IsNullOrEmpty(val.ToString())) ? string.Empty : decimal.Parse(val.ToString()).ToString("#,#0");
    }

    public static string GetPageUrlFileName()
    {
        return HttpContext.Current.Request.Url.Segments.Length > 0 ? HttpContext.Current.Request.Url.Segments[HttpContext.Current.Request.Url.Segments.Length - 1] : string.Empty;
    }

    public static string StripHtml(string val)
    {
        return Regex.Replace(val, @"<(.|\n)*?>", string.Empty);
    }
    public static bool IsSortAscending(string sortOrder)
    {
        return (string.Compare(WebSiteConstants.SORT_ASC, sortOrder) == 0);
    }

    public static int GetIntegerValue(string intStr)
    {
        int i;
        if (int.TryParse(intStr, out i))
            return i;
        return int.MinValue;
    }

    public static int? GetIntegerValue(object val)
    {
        string integerStr = NvlString(val);
        int result;
        if (!integerStr.IsNullOrEmpty() && int.TryParse(integerStr, out result))
        {
            return result;
        }
        return null;
    }

    public static decimal GetDecimalValue(string decStr)
    {
        decimal d;
        if (decimal.TryParse(decStr, out d))
            return d;
        return decimal.MinValue;
    }

    public static DataTable GetSelectedRadList(DataTable list, Telerik.Web.UI.RadListBox radList, string valueKey)
    {
        if (list == null)
            return null;

        DataTable selectedList = list.Clone();

        foreach (RadListBoxItem item in radList.Items)
        {
            DataRow[] selected = list.Select(string.Format("{0} = '{1}'", valueKey, item.Value));

            if (selected.Length > 0)
            {
                DataRow newRow = selectedList.NewRow();
                DataRow row = selected[0];

                for (int i = 0; i < selectedList.Columns.Count; i++)
                {
                    newRow[i] = row[i];
                }

                selectedList.Rows.Add(newRow);
            }
        }

        return selectedList;
    }

    public static DataTable ToSortedDataTable(DataTable list, string sortExpression)
    {
        list.DefaultView.Sort = sortExpression;
        return list.DefaultView.ToTable();
    }

    public static string GetFileName(string fileName)
    {
        fileName = fileName.Replace(" ", "");//Remove space
        for (int index = 0; index < FILE_NAME_REPLACE_CHANGE.Length; index++)
        {
            fileName = fileName.Replace(FILE_NAME_REPLACE_CHANGE[index], '_');
        }
        return fileName;

    }

    public static string FormatWholeNumber(object val)
    {
        return FormatNumber(val, "{0:#,##0}");
    }
    public static string FormatNumber(object val, string format)
    {
        return (val == null || NvlString(val).Length == 0 ? string.Format(format, 0) : string.Format(format, val));
    }

    public static string FormatPercentage(int topValue, int bottomValue, string format)
    {
        return FormatPercentage(topValue * 1.0, bottomValue * 1.0, format);
    }

    public static string FormatPercentage(double topValue, double bottomValue, string format)
    {
        return (bottomValue == 0 ? string.Format(format, 0) : string.Format(format, (topValue / bottomValue) * 100));
    }

    public static DateTime GetDateTimeValue(string dtStr)
    {
        DateTime dt = DateTime.MinValue;
        if (DateTime.TryParse(dtStr, out dt))
            return dt;
        return DateTime.MinValue;
    }

    public static string BuildAssignmentTypeAbbr(object assignmentTypeCell)
    {
        string type = NA_VALUE;
        var assignmentType = GeneralFuncsLib.NvlString(assignmentTypeCell);
        if (string.IsNullOrEmpty(assignmentType)
            || assignmentType.Equals(((int)WebSiteEnums.AssignmentType.DetectionQueue).ToString()))
        {
            type = DETECTION_QUEUE_AS_VALUE;
        }
        else if (assignmentType.Equals(((int)WebSiteEnums.AssignmentType.WorkQueue).ToString()))
        {
            type = WORK_QUEUE_AS_VALUE;
        }
        else if (assignmentType.Equals(((int)WebSiteEnums.AssignmentType.AggregateQueue).ToString()))
        {
            type = AGGREGATE_QUEUE_AS_VALUE;
        }
        else if (assignmentType.Equals(((int)WebSiteEnums.AssignmentType.Subsite).ToString()))
        {
            type = SUBSITE_AS_VALUE;
        }
        else
        {
            type = DISTINCT_DETECTION_QUEUE_AS_VALUE;
        }
        return type;
    }

    public static bool IsFieldStartDateOfRiskMgnt(string currentType)
    {
        var lstShow = new List<string>
        {
            ((int)WebSiteEnums.AssignmentType.DetectionQueue).ToString()
        };
        return lstShow.Contains(currentType);
    }    

    #endregion

    #region Reporting

    public static DateTime GetFirstDayOfMonth(this DateTime date)
    {
        DateTime dt = date;
        dt = dt.AddDays(-(dt.Day) + 1);
        return dt;
    }

    public static DateTime GetLastDayOfMonth(this DateTime date)
    {
        DateTime dt = date;
        dt = dt.AddMonths(1);
        dt = dt.AddDays(-(dt.Day));
        return dt;
    }

    public static string GetMerchantName(string merchantNumber)
    {
        return GetMerchantName(merchantNumber, false);
    }

    public static string GetMerchantName(string merchantNumber, bool isNotInMif)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();
        parameters.Add(new FilterParameter("@MerchantNumber", merchantNumber, DbType.String));
        parameters.Add(new FilterParameter("@IsNotInMif", isNotInMif, DbType.Boolean));

        DataTable userInfo = WebServices.CsReportServices.GetReports("spa_cs_GetMerchantName", parameters);

        if (userInfo != null && userInfo.Rows.Count > 0)
        {
            return userInfo.Rows[0]["MerchantName"].ToString();
        }
        else
            return string.Empty;
    }

    public static bool CheckOnOffModule(string moduleCode)
    {
        string prefixModule = "OnOffModule_" + moduleCode;
        string strModulData = GeneralFuncsLib.GetClientExtendedSetting("FORCE_TURNOFF_MODULE").Data;
        if (string.IsNullOrEmpty(strModulData) == true ? false : strModulData.Contains(moduleCode))
        {
            HttpContext.Current.Session[prefixModule] = "False";
        }
        else
        {
            if (HttpContext.Current.Session[prefixModule] == null)
            {
                FilterParameterCollection parameters = new FilterParameterCollection();
                parameters.Add(new FilterParameter("@ASClient", SessionManager.CurrentUser.ASClient, DbType.Int32));
                parameters.Add(new FilterParameter("@ModuleCode", moduleCode, DbType.String));
                DataTable td = WebServices.CsReportServices.GetReports("spa_SEC_CheckModuleByClient", parameters);

                if (td != null && td.Rows.Count > 0)
                {
                    HttpContext.Current.Session[prefixModule] = td.Rows[0]["STATUS"].ToString();
                }
                else
                    HttpContext.Current.Session[prefixModule] = "False";
            }
        }

        return HttpContext.Current.Session[prefixModule].ToString().Equals("True", StringComparison.OrdinalIgnoreCase);
    }


    public static string GetRemoteIpAddress
    {
        get
        {
            string clientIPAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(clientIPAddress)) clientIPAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            return clientIPAddress;
        }
    }

    public static bool HasIPForFullCard(this SecurePage page)
    {
        string sourceIPs = "," + WebSiteSettings.SourceIP.Trim(',') + ",";
        if (sourceIPs.IndexOf("," + GetRemoteIpAddress + ",") >= 0)
        {
            if (page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_CC_MS))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
            return false;
    }

    public static bool CheckCSViewFullCard(this SecurePage page)
    {
        if ((SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.AS
            || SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.CS)
            && page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_CC))
        {
            return true;
        }
        return false;
    }

    public static string BuildRiskUrlForFullCard(SecurePage page, string reportType,
        string recordId, string issueBank, string reportDate, object partialCardNumber,
        object accountNumber, string merchantNr, string text, int index, bool isModal)
    {
        return BuildUrlForFullCard(page, reportType, recordId, issueBank, reportDate,
            partialCardNumber, accountNumber, merchantNr, text, index, isModal, true);
    }

    public static string BuildUrlForFullCard(SecurePage page, string reportType,
        string recordId, string issueBank, string reportDate, object partialCardNumber,
        object accountNumber, string merchantNr, string text, int index, bool isModal)
    {
        return BuildUrlForFullCard(page, reportType, recordId, issueBank, reportDate,
            partialCardNumber, accountNumber, merchantNr, text, index, isModal, false);
    }

    public static string BuildUrlForNotInMifFullCard(SecurePage page, string reportType,
        string recordId, string issueBank, string reportDate, object partialCardNumber,
        object accountNumber, string merchantNr, string text, int index, bool isModal, bool isNotInMif = false)
    {
        string queryString = page.BuildSecureQueryString(
            string.Format("rt={0}&cn={1}&issue={2}&reportdate={3}&idx={4}",
                          reportType,
                          page.Server.UrlEncode(recordId),
                          issueBank,
                          reportDate,
                          index));

        string urlFullCardDetail = string.Format(page.ResolveUrl("~") + "FullCC.aspx?{0}", queryString);

        string img = string.Format("<img src='{0}res/img/information.png' border=\"0\"/>", page.ResolveUrl("~"));

        string urlBlueDot = string.Empty;
        if (isModal)
        {
            urlBlueDot = string.Format(
                "<a href=\"#\" style=\"cursor:pointer\" onclick=\" return parent.ShowPopupModalChild({0}, '{1}','auto');\">{2}</a>&nbsp; &nbsp;",
                index, urlFullCardDetail, img);
        }
        else
        {
            urlBlueDot = string.Format(
               "<a href=\"#\" style=\"cursor:pointer\" onclick=\" return ShowPopupModal('{0}','auto');\">{1}</a>&nbsp; &nbsp;",
               urlFullCardDetail, img);
        }

        return VeraCodeSolution.GetOutputHtmlString(urlBlueDot
            + BuildUrlForNotInMifCardNumber(page, partialCardNumber, accountNumber, merchantNr, text, isNotInMif));
    }

    public static string BuildUrlForFullCard(SecurePage page, string reportType,
        string recordId, string issueBank, string reportDate, object partialCardNumber,
        object accountNumber, string merchantNr, string text, int index, bool isModal, bool isRisk = false)
    {
        string queryString = page.BuildSecureQueryString(
            string.Format("rt={0}&cn={1}&issue={2}&reportdate={3}&idx={4}",
                          reportType,
                          page.Server.UrlEncode(recordId),
                          issueBank,
                          reportDate,
                          index));

        string urlFullCardDetail = string.Format(page.ResolveUrl("~") + "FullCC.aspx?{0}", queryString);

        string img = string.Format("<img src='{0}res/img/information.png' border=\"0\"/>", page.ResolveUrl("~"));

        string urlBlueDot = string.Empty;
        if (isModal)
        {
            urlBlueDot = string.Format(
                "<a href=\"#\" style=\"cursor:pointer\" onclick=\" return parent.ShowPopupModalChild({0}, '{1}','auto');\">{2}</a>&nbsp; &nbsp;",
                index, urlFullCardDetail, img);
        }
        else
        {
            urlBlueDot = string.Format(
               "<a href=\"#\" style=\"cursor:pointer\" onclick=\" return ShowPopupModal('{0}','auto');\">{1}</a>&nbsp; &nbsp;",
               urlFullCardDetail, img);
        }

        return VeraCodeSolution.GetOutputHtmlString(urlBlueDot
            + BuildUrlForCardNumber(page, partialCardNumber, accountNumber, merchantNr, text, isRisk));
    }

    public static string BuildRiskUrlForCardNumber(SecurePage page, object partialCardNumber,
        object accountNumber, string merchantNr, string text)
    {
        return BuildUrlForCardNumber(page, partialCardNumber, accountNumber,
                                     merchantNr, text, true);
    }
    public static string BuildUrlForNotInMifCardNumber(SecurePage page, object partialCardNumber,
        object accountNumber, string merchantNr, string text, bool isNotInMif)
    {
        string queryString = string.Format("cn={0}&cnf={1}&merch={2}", partialCardNumber, accountNumber, merchantNr);
        if (isNotInMif)
        {
            queryString += "&isNotInMif=" + isNotInMif;
        }
        queryString = page.BuildSecureQueryString(queryString);

        string urlCardDetail = string.Format(page.ResolveUrl("~") + "CardHistoryModal.aspx?{0}", queryString);

        return VeraCodeSolution.GetOutputHtmlString(string.Format(
            "<a href=\"javascript:void(0)\" onclick=\"openPopupWindow('{0}','CardHistoryWindow'); return false;\">{1}</a>",
            urlCardDetail,
            text));
    }

    public static string BuildUrlForCardNumber(SecurePage page, object partialCardNumber,
        object accountNumber, string merchantNr, string text, bool isRisk = false)
    {
        string queryString = string.Format("cn={0}&cnf={1}&merch={2}", partialCardNumber, accountNumber, merchantNr);
        if (isRisk)
        {
            queryString += "&isRisk=1";
        }
        queryString = page.BuildSecureQueryString(queryString);

        string urlCardDetail = string.Format(page.ResolveUrl("~") + "CardHistoryModal.aspx?{0}", queryString);

        return VeraCodeSolution.GetOutputHtmlString(string.Format(
            "<a href=\"javascript:void(0)\" onclick=\"openPopupWindow('{0}','CardHistoryWindow'); return false;\">{1}</a>",
            urlCardDetail,
            text));
    }

    /// <summary>
    /// Auth Url with AuthNumber, Merchant & Auth Intruder Query
    /// </summary>
    public static string BuildAuthUrlPopupModalChild(SecurePage page, object authCell, string merchantNr,
        string authIntruderQuery, string text, bool isModal)
    {
        string url = string.Empty;
        if (!authCell.ToString().IsNullOrEmpty())
        {
            string queryString = page.BuildSecureQueryString(
                string.Format(
                    "AuthNumber={0}&merch={1}{2}",
                    authCell,
                    merchantNr,
                    authIntruderQuery)
            );
            url = BuildAuthUrlPopupModalChild(page, queryString, text, isModal);
        }
        return url;
    }

    /// <summary>
    /// Auth Url with AuthNumber, Merchant, Index & Auth Intruder Query
    /// </summary>
    public static string BuildAuthUrlPopupModalChild(SecurePage page, object authCell, string merchantNr,
       int index, string authIntruderQuery, string text, bool isModal)
    {
        return BuildAuthUrlPopupModalChild(page, authCell, merchantNr, index, authIntruderQuery, text, isModal, false);
    }

    public static string BuildAuthUrlPopupModalChild(SecurePage page, object authCell, string merchantNr,
       int index, string authIntruderQuery, string text, bool isModal, bool isNotInMif = false)
    {
        string url = string.Empty;
        if (!authCell.ToString().IsNullOrEmpty())
        {
            string queryString = page.BuildSecureQueryString(
                string.Format(
                    "AuthNumber={0}&merch={1}&idx={2}&isNotInMif={3}{4}",
                    authCell,
                    merchantNr,
                    index,
                    isNotInMif,
                    authIntruderQuery)
            );
            url = BuildAuthUrlPopupModalChild(page, queryString, text, isModal);
        }
        return url;
    }

    private static string BuildAuthUrlPopupModalChild(SecurePage page, string queryString, string text, bool isModal)
    {
        string urlAuthDetail = string.Format("{0}AuthorizationDetailsModal.aspx?{1}", page.ResolveUrl("~"), queryString);
        string urlAuth = isModal ?
            "<a href=\"#\" style=\"cursor:pointer\" onclick=\" return parent.ShowPopupModalChild(1,'{0}','auto');\">{1}</a>"
            : "<a href=\"#\" style=\"cursor:pointer\" onclick=\" return ShowPopupModalChild(1,'{0}','auto');\">{1}</a>";
        return VeraCodeSolution.GetOutputHtmlString(
            string.Format(urlAuth, urlAuthDetail, text));
    }

    /// <summary>
    /// Build Auth Url with AuthNr & AuthIntruderQuery params
    /// </summary>
    public static string BuildAuthUrlPopupModal(SecurePage page, string text,
        object authCell, string authIntruderQuery)
    {
        if (authCell.ToString().IsNullOrEmpty())
            return string.Empty;

        string queryString = string.Format("AuthNumber={0}{1}", authCell, authIntruderQuery);
        return BuildAuthUrlPopupModal(page, queryString, text);
    }

    /// <summary>
    ///  Build Auth Url with Merch, AuthNr & AuthIntruderQuery params
    /// </summary>
    public static string BuildAuthUrlPopupModal(SecurePage page, string text,
        object authCell, string authIntruderQuery, string merchantNr)
    {
        if (authCell.ToString().IsNullOrEmpty())
            return string.Empty;

        string queryString = string.Format("merch={0}&AuthNumber={1}{2}", merchantNr, authCell, authIntruderQuery);
        return BuildAuthUrlPopupModal(page, queryString, text);
    }

    /// <summary>
    ///  Build Auth Url with Merch, AuthNr, Index & AuthIntruderQuery params
    /// </summary>
    public static string BuildAuthUrlPopupModal(SecurePage page, string text,
        object authCell, string authIntruderQuery, string merchantNr, int index)
    {
        if (authCell.ToString().IsNullOrEmpty())
            return string.Empty;

        string queryString = string.Format("merch={0}&AuthNumber={1}&idx={2}{3}",
            merchantNr, authCell, index, authIntruderQuery);
        return BuildAuthUrlPopupModal(page, queryString, text);
    }

    /// <summary>
    ///  Build Auth Url for not in mif with Merch, AuthNr, Index & AuthIntruderQuery params
    /// </summary>
    public static string BuildAuthUrlPopupModal(SecurePage page, string text,
        object authCell, string authIntruderQuery, string merchantNr, int index, bool isNotInMif)
    {
        if (authCell.ToString().IsNullOrEmpty())
            return string.Empty;

        string queryString = string.Format("merch={0}&AuthNumber={1}&idx={2}{3}&isNotInMif={4}",
            merchantNr, authCell, index, authIntruderQuery, isNotInMif);
        return BuildAuthUrlPopupModal(page, queryString, text);
    }

    /// <summary>
    /// Build Auth Url with AuthNr, AuthIntruderQuery,
    /// HierarchyFilterValue (MerchantNr, BeginDate, EndDate & DateRange) params
    /// </summary>
    public static string BuildAuthUrlPopupModal(SecurePage page, object authCell,
        string authIntruderQuery, HierarchyFilterValue filter, string text)
    {
        if (authCell.ToString().IsNullOrEmpty())
            return string.Empty;

        string queryString = string.Format(
            "merch={0}&AuthNumber={1}{2}&BeginDate={3}&EndDate={4}&DateRange={5}",
             filter.Value,
             authCell,
             authIntruderQuery,
             filter.DateOptionValue.From.ToString(),
             filter.DateOptionValue.To.ToString(),
             (int)filter.DateOption);
        return BuildAuthUrlPopupModal(page, queryString, text);
    }

    private static string BuildAuthUrlPopupModal(SecurePage page, string queryString, string text)
    {
        string urlAuthDetail = page.ResolveUrl("~") + "AuthorizationDetailsModal.aspx?"
            + page.BuildSecureQueryString(queryString);
        string urlAuth = "<a href=\"#\" style=\"cursor:pointer\" onclick=\"return ShowPopupModal('{0}','auto');\">{1}</a>";
        return VeraCodeSolution.GetOutputHtmlString(string.Format(urlAuth, urlAuthDetail, text));
    }

    public static string BuildRskAuthUrlPopupModal(SecurePage page,
        object authCell, string merchantNr, string merchantName,
        object transactionDate, int index, string text)
    {
        return BuildRskAuthUrl(page, authCell, merchantNr, merchantName,
            transactionDate, index, text, false);
    }

    public static string BuildRskAuthUrlPopupModalChild(SecurePage page,
        object authCell, string merchantNr, string merchantName,
        object transactionDate, int index, string text, bool isModal)
    {
        return BuildRskAuthUrl(page, authCell, merchantNr, merchantName,
           transactionDate, index, text, true, isModal, false);
    }

    public static string BuildRskAuthUrlPopupModalChild(SecurePage page,
        object authCell, string merchantNr, string merchantName,
        object transactionDate, int index, string text, bool isModal, bool isMCFRisk)
    {
        return BuildRskAuthUrl(page, authCell, merchantNr, merchantName,
           transactionDate, index, text, true, isModal, isMCFRisk);
    }

    public static string BuildRskAuthUrl(SecurePage page, object authCell,
        string merchantNr, string merchantName, object transactionDate,
        int index, string text, bool isPopupModalChild, bool isModal = false, bool isMCFRisk = false)
    {
        string pageUrl = isMCFRisk ? "risk_MCF/rm_MCF_AuthorizationDetailsModal.aspx?" : "Risk/rm_AuthorizationDetailsModal.aspx?";
        string param = page.BuildSecureQueryString(
            string.Format("AuthNumber={0}&merch={1}&merchname={2}&transactiondate={3}&idx={4}",
                authCell,
                merchantNr,
                HttpUtility.UrlEncode(merchantName),
                transactionDate,
                index));

        string urlAuth = string.Format("{0}{1}{2}",
            page.ResolveUrl("~"), pageUrl, param);

        string url = isPopupModalChild ?
            string.Format("<a href=\"#\" style=\"cursor:pointer\" onclick=\" return {0}ShowPopupModalChild({1},'{2}', 'auto');\">{3}</a>",
                          isModal ? "parent." : string.Empty, index, urlAuth, text)
            : string.Format("<a href=\"#\" style=\"cursor:pointer\" onclick=\" return ShowPopupModal('{0}','auto');\">{1}</a>",
                            urlAuth, text);
        return VeraCodeSolution.GetOutputHtmlString(url);
    }

    public static string BuildRskAuthFunc(SecurePage page, object authCell,
      string merchantNr, string merchantName, object transactionDate,
      int index, string text, bool isPopupModalChild, bool isModal = false, bool isMCFRisk = false)
    {
        string pageUrl = isMCFRisk ? "risk_MCF/rm_MCF_AuthorizationDetailsModal.aspx?" : "Risk/rm_AuthorizationDetailsModal.aspx?";
        string param = page.BuildSecureQueryString(
            string.Format("AuthNumber={0}&merch={1}&merchname={2}&transactiondate={3}&idx={4}",
                authCell,
                merchantNr,
                HttpUtility.UrlEncode(merchantName),
                transactionDate,
                index));

        string urlAuth = string.Format("{0}{1}{2}",
            page.ResolveUrl("~"), pageUrl, param);

        string url = isPopupModalChild ?
            string.Format("<{0}ShowPopupModalChild({1},'{2}', 'auto');",
                          isModal ? "parent." : string.Empty, index, urlAuth, text)
            : string.Format("ShowPopupModal('{0}','auto');",
                            urlAuth, text);
        return VeraCodeSolution.GetOutputHtmlString(url);
    }

    public static string BuildVoucherUrl(SecurePage page, object transactionIdCell,
        string merchantNr, int index, DateTime reportDate, string voucherIntruder,
        bool isParentInRisk, string text)
    {
        string queryStringVoucher = page.BuildSecureQueryString(
            string.Format("TransactionID={0}&merch={1}&idx={2}&ReportDate={3}&IsParentInRisk={4}{5}",
            transactionIdCell, merchantNr, index, reportDate, isParentInRisk, voucherIntruder));

        string queryStringVoucherDetail = page.ResolveUrl("~") + "VoucherModal.aspx?" + queryStringVoucher;

        string urlVoucher = string.Format(
            "<a href=\"#\" style=\"cursor:pointer\" onclick=\" return parent.ShowPopupModalChild({0},'{1}','auto');\">{2}</a>",
            index,
            queryStringVoucherDetail,
            text);

        return VeraCodeSolution.GetOutputHtmlString(urlVoucher);
    }

    public static string BuildVoucherUrl(SecurePage page, object transactionIdCell,
       string merchantNr, int index, DateTime reportDate, string voucherIntruder,
       bool isParentInRisk, string text, bool isNotInMif = false)
    {
        string queryStringVoucher = page.BuildSecureQueryString(
            string.Format("TransactionID={0}&merch={1}&idx={2}&ReportDate={3}&IsParentInRisk={4}{5}&isNotInMif={6}",
            transactionIdCell, merchantNr, index, reportDate, isParentInRisk, voucherIntruder, isNotInMif));

        string queryStringVoucherDetail = page.ResolveUrl("~") + "VoucherModal.aspx?" + queryStringVoucher;

        string urlVoucher = string.Format(
            "<a href=\"#\" style=\"cursor:pointer\" onclick=\" return parent.ShowPopupModalChild({0},'{1}','auto');\">{2}</a>",
            index,
            queryStringVoucherDetail,
            text);

        return VeraCodeSolution.GetOutputHtmlString(urlVoucher);
    }

    public static string BuildLastBatchHistoryLink(SecurePage page, string date, string merchantNumber, string text)
    {
        string url = BuildBatchHistoryUrl(page, date, merchantNumber);
        string link = string.Format("<a class='link' href='{0}'>{1}</a>", url, text);
        return VeraCodeSolution.GetOutputHtmlString(link);
    }

    public static string BuildBatchHistoryUrl(SecurePage page, string date, string merchantNumber)
    {
        string url = page.ResolveUrl("~") + "BatchHistory.aspx?" + page.BuildSecureQueryString(
            string.Format("date={0}&merchantnumber={1}", date, merchantNumber));
        return url;
    }



    public static bool SiteAccessIsOptInOut(object siteAccess)
    {
        return (String.Compare(siteAccess.ToString(), "opted in", true) == 0
            || String.Compare(siteAccess.ToString(), "opted out", true) == 0
            || String.Compare(siteAccess.ToString(), "Incluido", true) == 0
            || String.Compare(siteAccess.ToString(), "Excluido", true) == 0);
    }

    public static bool CheckHierarchy(string hierarchy)
    {
        DataTable hrc = SessionManager.HierarchyFilter;
        if (hrc != null && hrc.Rows.Count > 0)
        {
            for (int i = 0; i < hrc.Rows.Count; i++)
            {
                if (hrc.Rows[i]["HierarchyMode"].ToString().ToUpper() == hierarchy.ToUpper())
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static string BuildAuthUrl(SecurePage page,
        object authCell, string authIntruderQuery,
        AS.Controls.Grid.ASGrid currentGrid, bool parentIsRisk,
        HierarchyFilterValue hierarchyFilterValue)
    {
        string url = string.Empty;
        if (!authCell.ToString().IsNullOrEmpty())
        {
            string queryString = page.BuildSecureQueryString(
                string.Format(
                    "MerchantNumber={0}&AuthNumber={1}{2}&BeginDate={3}&EndDate={4}&DateRange={5}",
                    hierarchyFilterValue.Value,
                    authCell,
                    authIntruderQuery,
                    hierarchyFilterValue.DateOptionValue.From.ToString(),
                    hierarchyFilterValue.DateOptionValue.To.ToString(),
                    (int)hierarchyFilterValue.DateOption)
            );

            string urlAuthDetail = parentIsRisk ? "../AuthorizationDetailsModal.aspx?" + queryString
                : "AuthorizationDetailsModal.aspx?" + queryString;

            string urlAuth =
                "<a class=\"link\" href=\"#\" style=\"cursor:pointer\" onclick=\" return ShowPopupModalChild(1,'{0}');\">{1}</a>";
            url = VeraCodeSolution.DoVeraCode(
                string.Format(urlAuth, urlAuthDetail, authCell.ToString()));
        }
        return url;
    }

    public static string BuildURLForSiteAccessInMIF(SecurePage page,
       string merchantNumber, string optedIn, string mifEmail)
    {
        return string.Format(
            "return ShowPopupModal('MerchantProfileModal.aspx?{0}', 'auto')",
            page.BuildSecureQueryString(
                string.Format("merchant={0}&m={1}&e={2}",
                              merchantNumber, optedIn, mifEmail)));
    }

    public static DataTable GetStatementCustomTheme(string merchantNumber)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.Add(new FilterParameter("@ASClient", SessionManager.CurrentClient, System.Data.DbType.Int32));
        parameters.Add(new FilterParameter("@MerchantNumber", merchantNumber, System.Data.DbType.AnsiString));
        if (SessionManager.CurrentSystem == WebSiteConstants.AS_SYSTEM_MS)
        {
            parameters.Add(new FilterParameter("@EntityType", SessionManager.CurrentUser.EntityType, System.Data.DbType.Int32));
            parameters.Add(new FilterParameter("@EntityID", SessionManager.CurrentUser.EntityID, System.Data.DbType.AnsiString));
        }
        DataTable clientInfo = WebServices.MsReportServices.GetReports("spa_stmt_GetStatementCustomThemeInfo", parameters);

        return clientInfo;
    }

    #endregion

    /// <summary>
    /// Get the aspx file name of current URL
    /// </summary>
    /// <returns></returns>

    #region Validation Functions
    public static bool IsValidWithRegularExpression(string expression, string inputVal)
    {
        System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(expression);
        return reg.IsMatch(inputVal) && reg.Matches(inputVal)[0].Length == inputVal.Length;
    }
    public static bool IsValidWithAllowCharaters(string validChars, string inputVal)
    {
        string validHexValue = "";
        for (var i = 0; i < validChars.Length; i++)
        {
            validHexValue += "\\x" + ((int)validChars[i]).ToString("X").ToUpper();
        }
        System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[" + validHexValue + "]");
        return reg.IsMatch(inputVal) && reg.Matches(inputVal).Count == inputVal.Length;
    }
    public static bool IsValidWithNotAllowCharaters(string invalidChars, string inputVal)
    {
        string validHexValue = "";
        for (var i = 0; i < invalidChars.Length; i++)
        {
            validHexValue += "\\x" + ((int)invalidChars[i]).ToString("X").ToUpper();
        }
        System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[" + validHexValue + "]");
        return !reg.IsMatch(inputVal);
    }

    #endregion

    public static bool IsIFrameSupported()
    {
        if (SessionManager.IsLoggedIn && GeneralFuncsLib.HasExtendedSetting("PORTAL_IFRAME_ENABLED") && SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.Merchant && SessionManager.JumpSource == "Outside")
        {
            return true;
        }
        return false;
    }

    public static int GetWeekNumber(DateTime date)
    {
        CultureInfo ci = CultureInfo.CurrentCulture;
        return ci.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
    }
    public static void GetStartAndEndOfWeek(DateTime dayInWeek, out DateTime startOfWeek, out DateTime endOfWeek)
    {

        CultureInfo ci = CultureInfo.CreateSpecificCulture("en-US");
        ci.DateTimeFormat.FirstDayOfWeek = DayOfWeek.Monday;
        ci.DateTimeFormat.CalendarWeekRule = CalendarWeekRule.FirstFourDayWeek;

        int week = ci.Calendar.GetWeekOfYear(dayInWeek, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        int year = dayInWeek.Year;
        if (dayInWeek.Month == 1 && week > 52)
        {
            year = year - 1;
        }
        // find the first week.
        CalendarWeekRule cwr = ci.DateTimeFormat.CalendarWeekRule;
        DayOfWeek firstDayOfWeek = ci.DateTimeFormat.FirstDayOfWeek;
        DateTime firstdayofyear = new DateTime(year, 1, 1);
        int offset = 0;
        if (firstdayofyear.DayOfWeek != firstDayOfWeek)
        {
            // find first first day.
            if (cwr == CalendarWeekRule.FirstFourDayWeek)
            {
                DateTime firstFullWeekStart = firstdayofyear;
                while (firstFullWeekStart.DayOfWeek != firstDayOfWeek)
                    firstFullWeekStart = firstFullWeekStart.AddDays(1);
                if (firstFullWeekStart.Subtract(firstdayofyear).Days >= 4)
                    offset = -1;
            }
            if (cwr == CalendarWeekRule.FirstDay)
                offset = -1;
        }
        startOfWeek = firstdayofyear.AddDays(7 * (week + offset));
        while (startOfWeek != firstdayofyear && startOfWeek.DayOfWeek != firstDayOfWeek)
            startOfWeek = startOfWeek.AddDays(-1);
        endOfWeek = startOfWeek;
        do
        {
            endOfWeek = endOfWeek.AddDays(1);
        } while (endOfWeek.AddDays(1).DayOfWeek != firstDayOfWeek); //endOfWeek < new DateTime(year + 1, 1, 1).AddDays(-1) &&

        int currentWeek = ci.Calendar.GetWeekOfYear(DateTime.Today, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        if (week == currentWeek && endOfWeek.Year == DateTime.Today.Year)      //Dorian Turner TK31325: Weekly data will not be returned as yearly data
        {
            endOfWeek = DateTime.Today;
        }
    }
    public static void GetRealDateRange(DateOptionMode dateFilterMode, ref DateTime fromDate, ref DateTime toDate)
    {
        switch (dateFilterMode)
        {
            case DateOptionMode.Daily:
                toDate = fromDate;
                break;
            case DateOptionMode.Weekly:
                GetStartAndEndOfWeek(fromDate, out fromDate, out toDate);
                break;
            case DateOptionMode.Monthly:
                {
                    fromDate = fromDate.GetFirstDayOfMonth();
                    if (fromDate.Month == DateTime.Now.Month && fromDate.Year == DateTime.Now.Year)
                        toDate = DateTime.Today;
                    else
                        toDate = fromDate.GetLastDayOfMonth();
                }
                break;
            case DateOptionMode.DateRange:
                break;
        }
    }

    public static int LatestContaintNumberic(string text)
    {
        Regex regexword = new Regex("[a-zA-Z]");
        Regex regexspecial = new Regex("[^a-zA-Z0-9]");
        return regexspecial.Replace(regexword.Replace(text.Trim(), string.Empty), string.Empty).Length;

    }
    public static int LatestContaintLowerCase(string text)
    {
        Regex regexword = new Regex("[A-Z0-9]");
        Regex regexspecial = new Regex("[^a-zA-Z0-9]");
        return regexspecial.Replace(regexword.Replace(text.Trim(), string.Empty), string.Empty).Length;

    }
    public static int LatestContaintUpperCase(string text)
    {
        Regex regexword = new Regex("[a-z0-9]");
        Regex regexspecial = new Regex("[^a-zA-Z0-9]");
        return regexspecial.Replace(regexword.Replace(text.Trim(), string.Empty), string.Empty).Length;

    }

    public static int LatestContaintSpecialChar(string text)
    {
        Regex regexword = new Regex("[a-zA-Z0-9]");
        return regexword.Replace(text.Trim(), string.Empty).Length;


    }

    public static void UpdateUserLogs(string action)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.Add(new FilterParameter("@ASClient", SessionManager.CurrentClient, DbType.Int32));
        parameters.Add(new FilterParameter("@SiteID", SessionManager.CurrentUserSiteId, DbType.Int32));
        parameters.Add(new FilterParameter("@UserID", SessionManager.CurrentUser.RecId, DbType.Guid));
        parameters.Add(new FilterParameter("@LogSessionID", SessionManager.UniqueSessionID, DbType.String));
        parameters.Add(new FilterParameter("@ActionType", action, DbType.String));
        WebServices.SecurityServices.GetReports("spa_SEC_UpdateUserActionLog", parameters);

    }

    public static void UpdateUserLogs(string sessionId, string action)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.Add(new FilterParameter("@ASClient", SessionManager.CurrentClient, DbType.Int32));
        parameters.Add(new FilterParameter("@SiteID", SessionManager.CurrentUserSiteId, DbType.Int32));
        parameters.Add(new FilterParameter("@UserID", SessionManager.CurrentUser.RecId, DbType.Guid));
        // parameters.Add(new FilterParameter("@LogSessionID", SessionManager.UniqueSessionID, DbType.String));
        parameters.Add(new FilterParameter("@AspSessionID", sessionId, DbType.String));
        parameters.Add(new FilterParameter("@ActionType", action, DbType.String));
        WebServices.SecurityServices.GetReports("spa_SEC_UpdateUserActionLog", parameters);

    }

    public static void LoadHierarchyConfigurations()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        //get all hierarchy filter
        if (SessionManager.AllHierarchyFilter == null)
        {
            parameters.Clear();
            parameters.AddLoggedInUserReportingParams(true);
            parameters.AddLanguageID();
            SessionManager.AllHierarchyFilter = WebServices.RiskServices.GetReports("spa_GetAllHierarchyFilters", parameters);
        }

        //get full hierarchy Filter of login user
        if (SessionManager.HierarchyFilter == null)
        {
            parameters.Clear();
            parameters.AddLoggedInUserReportingParams(true);
            parameters.AddLanguageID();
            SessionManager.HierarchyFilter = WebServices.RiskServices.GetReports("spa_GetHierarchyFilter", parameters);
        }

        //get hierarchy filter for merchant profile
        if (SessionManager.HierarchyFilterForMerchantProfile == null)
        {
            parameters.Clear();
            parameters.AddLoggedInUserReportingParams(true);
            parameters.Add(new FilterParameter("@PageMode", "MerchantProfile", DbType.String));
            parameters.AddLanguageID();
            SessionManager.HierarchyFilterForMerchantProfile = WebServices.RiskServices.GetReports("spa_GetHierarchyFilter", parameters);
        }

        if (SessionManager.HierarchyFilterDrillDown == null)
        {
            parameters.Clear();
            parameters.AddLoggedInUserReportingParams(true);
            parameters.AddLanguageID();
            SessionManager.HierarchyFilterDrillDown = WebServices.RiskServices.GetReports("spa_GetHierarchyFilterLevel", parameters);
        }
    }

    #region Check Permissions

    public static bool HasRiskRptPermission(SecurePage page)
    {
        return page.IsUserWithPermission(WebSiteConstants.VIEW_RISK_REPORT)
            || page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RSK_RPT)
            || page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RSK_RPT_MS);
    }




    public static bool HasCMPermission(SecurePage page)
    {
        return page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_CM_MY_CASES)
            || page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_CM_OPEN_CASE)
            || page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_CM_SEARCH_CASE);
    }

    public static bool HasMSUserManagementFeature
    {
        get
        {
            var hasQueuingMechanism =
                GeneralFuncsLib.GetClientExtendedSetting("HasMSUserManagement");
            return hasQueuingMechanism.Data != null
                && hasQueuingMechanism.Data.ToLower().Equals("true");
        }
    }

    #endregion Check Permissions

    public static void GetDefaultDaysOfRiskReport()
    {
        VisionWebClient client = GetClientInfo(SessionManager.CurrentClient);
        if (client != null && client.TransHist30)
        {
            RiskSessionManager.TransDateRangeModal = 30;
            RiskSessionManager.BatchDateRangeModal = 30;
            RiskSessionManager.ACHReturnDateRangeModal = 30;
        }
    }

    public static string GetDefaultDaysOfRiskTransactionHistory()
    {
        VisionWebClient client = GetClientInfo(SessionManager.CurrentClient);
        if (client != null && client.TransHist30)
        {
            RiskSessionManager.TransDateRangeModal = 30;
        }
        return RiskSessionManager.TransDateRangeModal.ToString();
    }

    public static string GetDefaultDaysOfRiskBatchHistory()
    {
        VisionWebClient client = GetClientInfo(SessionManager.CurrentClient);
        if (client != null && client.TransHist30)
        {
            RiskSessionManager.BatchDateRangeModal = 30;
        }
        return RiskSessionManager.BatchDateRangeModal.ToString();
    }

    public static string GetDefaultDaysOfACHReturnDetail()
    {
        VisionWebClient client = GetClientInfo(SessionManager.CurrentClient);
        if (client != null && client.TransHist30)
        {
            RiskSessionManager.ACHReturnDateRangeModal = 30;
        }
        return RiskSessionManager.ACHReturnDateRangeModal.ToString();
    }

    public static void ResetSessionLanguage()
    {
        SessionManager.CurrentMenuItems = null;
        SessionManager.AllHierarchyFilter = null;
        SessionManager.AllHierarchyFilter = null;
        SessionManager.HierarchyFilter = null;
        SessionManager.HierarchyFilterForMerchantProfile = null;
        SessionManager.HierarchyFilterDrillDown = null;
        SessionManager.PwdValidationRule = null;
    }

    public static void ReloadSession()
    {
        LoadClientInfo();
    }

    public static string GetCulture()
    {
        if (SessionManager.CurrentLanguage == (int)WebSiteEnums.LanguageCode.English)
        {
            return WebSiteConstants.USCulture;
        }
        else if (SessionManager.CurrentLanguage == (int)WebSiteEnums.LanguageCode.Spanish)
        {
            return WebSiteConstants.SpanishCulture;
        }

        // default is English
        return WebSiteConstants.USCulture;
    }

    public static bool HasMultiLanguageFeature
    {
        get
        {
            if (ConfigurationManager.AppSettings.HasKeys() && ConfigurationManager.AppSettings["MultiLanguageFeature"] != null)
            {
                bool hasMultiLanguageFeature = false;
                hasMultiLanguageFeature = ConfigurationManager.AppSettings["MultiLanguageFeature"].ToLower().Equals("true") ? true : false;
                if (hasMultiLanguageFeature && GeneralFuncsLib.GetDataOfExtendedSetting("MultiLanguage").ToLower().Equals("true"))
                {
                    return true;
                }
            }
            return false;
        }
    }

    public static bool IsSSOUser(int clientId, string userId)
    {
        bool result = false;
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.Add(new FilterParameter("@ASClient", clientId, System.Data.DbType.Int32));
        parameters.Add(new AS.Common.DBManager.FilterParameter("@UserId", userId, DbType.AnsiString));
        FilterParameterCollection outParam = new FilterParameterCollection();
        parameters.Add(new AS.Common.DBManager.FilterParameter("@ReturnValue", false, DbType.Boolean, true));
        DataTable data = WebServices.SecurityServices.GetReports("spa_SEC_IsSSOUser", parameters);
        // Need to review
        if (data.HasData())
        {

            bool.TryParse(data.Rows[0][0].ToString(), out result);
        }
        return result;
    }

    public static int ValidateSSOUser(int clientId, string userId)
    {
        int result = 0;
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.Add(new FilterParameter("@ASClient", clientId, System.Data.DbType.Int32));
        parameters.Add(new AS.Common.DBManager.FilterParameter("@UserId", userId, DbType.AnsiString));
        FilterParameterCollection outParam = new FilterParameterCollection();
        parameters.Add(new AS.Common.DBManager.FilterParameter("@UserStatus", 0, DbType.Int32, true));
        WebServices.CsReportServices.ExecuteNonQueryCommand("spa_SEC_ValidateSSOUser", parameters, out outParam);
        int.TryParse(outParam[0].ParameterValue.ToString(), out result);
        return result;
    }

    public static bool DisableResetPasswordForInactiveUser(string entityType, string entityTypeIDs)
    {
        bool result = false;
        if (!string.IsNullOrEmpty(entityTypeIDs))
        {
            entityTypeIDs = string.Format(",{0},", entityTypeIDs);
            if (entityTypeIDs.Contains(string.Format(",{0},", entityType)))
            {
                result = true;
            }
        }
        return result;
    }

    public static FilterParameterCollection GetMasterMerchant(out bool isShowMsg)
    {
        isShowMsg = false;
        FilterParameterCollection paramUser = new FilterParameterCollection();
        paramUser.Add(new FilterParameter("@ASClient", SessionManager.CurrentUser.ASClient, DbType.Int32));
        if (GeneralFuncsLib.GetDataOfExtendedSetting("AllowChainAccessToPCIWithOldestMerchant") == "true" && SessionManager.CurrentUser.EntityType == 11)
        {
            FilterParameterCollection paramMasterMerchant = new FilterParameterCollection();
            paramMasterMerchant.Add(new FilterParameter("@ASClient", SessionManager.CurrentUser.ASClient, DbType.Int32));
            paramMasterMerchant.AddLoggedInUserPrimaryUserID();
            DataTable dtMasterMerchant = PciWebServices.PciReportServices.GetReports("spa_MULTI_PCI_GetMasterMerchant", paramMasterMerchant);
            if (dtMasterMerchant != null && dtMasterMerchant.Rows.Count > 0)
            {
                string merchantNumber = dtMasterMerchant.Rows[0]["MerchantNumber"].ToString();
                bool isActiveMerchant = dtMasterMerchant.Rows[0]["Status"].ToBoolean();
                if (isActiveMerchant && !merchantNumber.IsNullOrEmpty())
                    paramUser.Add(new FilterParameter("@UserName", merchantNumber, DbType.AnsiString));
                else
                    isShowMsg = true;
            }
            else
            {
                FilterParameterCollection parameters = new FilterParameterCollection();
                parameters.AddLoggedInUserReportingParams();
                parameters.AddLoggedInUserPrimaryUserID();
                DataTable dtUserChain = WebServices.SecurityServices.GetReports("spa_SEC_GetUserChain", parameters);
                if (dtUserChain != null && dtUserChain.Rows.Count > 0)
                    paramUser.Add(new FilterParameter("@UserName", dtUserChain.Rows[0]["MerchantNumber"].ToString(), DbType.AnsiString));
                else
                {
                    paramUser.Add(new FilterParameter("@UserName", string.Empty, DbType.AnsiString));
                    isShowMsg = true;
                }
            }
        }
        else
        {
            if (SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.Merchant)
                paramUser.Add(new FilterParameter("@UserName", SessionManager.CurrentUser.EntityID, DbType.AnsiString));
            else
            {
                var username = SessionManager.CurrentUser.OriginalUserID;
                if (GeneralFuncsLib.GetDataOfExtendedSetting("PCI_Site_Jump_MappingPrimaryUser") == "true" && GetEntityTypeMapping())
                {
                    FilterParameterCollection _param = new FilterParameterCollection();
                    _param.Add(new FilterParameter("@ASClient", SessionManager.CurrentUser.ASClient, DbType.Int32));
                    _param.Add(new FilterParameter("@UserID", SessionManager.CurrentUser.UserID, DbType.AnsiString));
                    DataTable dtNewUserId = WebServices.SecurityServices.GetReports("spa_cs_GetSpecial_OriginalUserID", _param);
                    if (dtNewUserId != null && dtNewUserId.Rows.Count > 0)
                    {
                        username = dtNewUserId.Rows[0]["OriginalUserID"].ToString();
                    }
                }
                paramUser.Add(new FilterParameter("@UserName", username, DbType.AnsiString));

            }
        }
        return paramUser;
    }

    public static bool GetEntityTypeMapping()
    {
        var entityTypes = GeneralFuncsLib.GetDataOfExtendedSetting("EntityType_MappingPrimaryUser");
        if (string.IsNullOrEmpty(entityTypes))
        {
            return false;
        }
        var _entityTypes = entityTypes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

        if (_entityTypes.Any())
        {
            return _entityTypes.Exists(x => x.Equals(SessionManager.CurrentUser.EntityType.ToString(), StringComparison.OrdinalIgnoreCase));
        }
        return false;
    }

    //Replace [ , % ^ _ by [[] [,] [%] [^] [_] for MerchantName when filter
    public static string ReplaceSpecialCharacter(string input)
    {
        return input.Replace("[", "[[]").Replace("%", "[%]").Replace(",", "[,]").Replace("_", "[_]");
    }

    /// <summary>
    /// mode: ALL --> Get All, BYUSER --> Get selected organization
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="recId"></param>
    /// <returns></returns>
    public static DataTable GetOrganizations(string mode, Guid? recId = null)
    {
        string spaName = "spa_SEC_GetOrganizationList";
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.Add("@ASClientID", SessionManager.CurrentClient, DbType.Int32);
        parameters.Add("@SiteID", SessionManager.CurrentUser.SiteID, DbType.Int32);
        parameters.Add("@LoginUserID", SessionManager.CurrentUser.RecId, DbType.Guid);
        parameters.Add("@Mode", mode, DbType.AnsiString);
        parameters.Add("@UserRecID", recId, DbType.Guid);

        return WebServices.SecurityServices.GetReports(spaName, parameters);
    }

    public static DataTable GetListOrganizationsView(string mode, string salesRepCode, Guid? recId = null)
    {
        string spaName = "spa_SEC_GetOrganizationList";
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.Add("@ASClientID", SessionManager.CurrentClient, DbType.Int32);
        parameters.Add("@SiteID", SessionManager.CurrentUser.SiteID, DbType.Int32);
        parameters.Add("@LoginUserID", SessionManager.CurrentUser.RecId, DbType.Guid);
        parameters.Add("@Mode", mode, DbType.AnsiString);
        parameters.Add("@UserRecID", recId, DbType.Guid);
        parameters.Add("@SalesRepCode", salesRepCode, DbType.AnsiString);

        return WebServices.SecurityServices.GetReports(spaName, parameters);
    }


    public static DataTable GetDefaultLandingPage(int hierarchyID)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.Add(new FilterParameter("@ASClient", SessionManager.CurrentClient, DbType.Int32));
        parameters.Add(new FilterParameter("@HierarchyID", hierarchyID, DbType.Int32));
        return WebServices.SecurityServices.GetReports("spa_SEC_GetDefaultLandingPage", parameters);
    }

    //36801 – MCPS – VW Supplemental MIF – FE
    public static DataTable GetRiskCreditScore(int primaryID, WebSiteEnums.AssignmentFilterModes option, WebSiteEnums.ParamFilterMode mode)
    {
        return GetRiskCreditScore(primaryID, option, mode, false);
    }

    public static DataTable GetRiskCreditScore(int primaryID, WebSiteEnums.AssignmentFilterModes option, WebSiteEnums.ParamFilterMode mode, bool isMCFRisk)
    {
        FilterParameterCollection paramsIn = new FilterParameterCollection();
        paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramsIn.AddLanguageID();
        paramsIn.Add(new FilterParameter("@PrimaryID", primaryID, DbType.Int32));
        paramsIn.Add(new FilterParameter("@Option", option, DbType.Int32));
        paramsIn.Add(new FilterParameter("@Mode", mode, DbType.Int32));
        string spa = isMCFRisk ? "spa_RM_MCF_GetFilterCreditScore" : "spa_rm_cs_GetFilterCreditScore";
        return WebServices.RiskServices.GetReports(spa, paramsIn);
    }

    public static DataTable GetCreditScoreByFilter(int primaryID, WebSiteEnums.ParamFilterMode mode)
    {
        return GetCreditScoreByFilter(primaryID, mode, false);
    }

    public static DataTable GetCreditScoreByFilter(int primaryID, WebSiteEnums.ParamFilterMode mode, bool isMCFRisk)
    {
        FilterParameterCollection paramsIn = new FilterParameterCollection();
        paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramsIn.Add(new FilterParameter("@AssignmentID", primaryID, DbType.Int32));
        paramsIn.Add(new FilterParameter("@FilterMode", mode, DbType.Int32));
        string spa = isMCFRisk ? "spa_RM_MCF_GetCreditScoreByFilter" : "spa_rm_cs_GetCreditScoreByFilter";
        return WebServices.RiskServices.GetReports(spa, paramsIn);
    }

    public static bool EnableSupplementalFeature(WebSiteEnums.SUPPLEMENTAL_FEATURE feature)
    {
        string features = GeneralFuncsLib.GetDataOfExtendedSetting("ENABLED_SUPPLEMENTAL");
        if (string.IsNullOrEmpty(features))
            return false;
        return features.Contains(feature.ToString());
    }

    public static bool AssignmentFilterExtend(WebSiteEnums.Filter_Extend feature)
    {
        string features = GeneralFuncsLib.GetDataOfExtendedSetting("Assignment_Filter_Extend");
        return !string.IsNullOrWhiteSpace(features) && (features ?? "").Split(',').Any(s => s.Trim().Equals(feature.ToString().Trim(), StringComparison.OrdinalIgnoreCase));
    }

    public static bool RiskReportExtend(WebSiteEnums.Filter_Extend feature)
    {
        string features = GeneralFuncsLib.GetDataOfExtendedSetting("RiskReport_Extend");
        return !string.IsNullOrWhiteSpace(features) && (features ?? "").Split(',').Any(s => s.Trim().Equals(feature.ToString().Trim(), StringComparison.OrdinalIgnoreCase));
    }

    //End

    public static string GetPartialTaxId(string value)
    {
        if (value.IsNullOrEmpty() || value.Length < 5)
        {
            return string.Empty;
        }
        return value.Substring(value.Length - 4).PadLeft(8, 'x');
    }

    /// <summary>
    /// Gets the notification permissions of source application.
    /// </summary>
    /// <param name="sourceAppId">The source application id.</param>
    /// <returns></returns>
    public static IList<string> GetNotificationPermissionsOfSourceApp(int? sourceAppId = null)
    {
        var permissions = new List<string>();
        var configFile = HttpContext.Current.Server.MapPath(WebSiteConstants.NOTIFICATION_SETTING_CONFIG_PATH);
        if (!File.Exists(configFile))
        {
            return permissions;
        }

        var document = new XmlDocument();
        document.Load(configFile);

        var nodes = !document.HasChildNodes ? null : document.SelectNodes("//Configuration/Permissions/SourceApp");
        if (nodes == null || nodes.Count == 0)
        {
            return permissions;
        }

        foreach (XmlNode node in nodes)
        {
            if (node.Attributes == null || node.Attributes.Count == 0 ||
                sourceAppId.HasValue && !node.Attributes["Id"].Value.EqualTo(sourceAppId.Value.ToString()) ||
                node.Attributes["Permissions"] == null || string.IsNullOrWhiteSpace(node.Attributes["Permissions"].Value))
            {
                continue;
            }

            permissions.AddRange(node.Attributes["Permissions"].Value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
        }

        return permissions;
    }


    public static DataTable CheckMerchantIsWorked(DateTime reportDate, string merchantNumber = "")
    {
        return CheckMerchantIsWorked(reportDate, false, merchantNumber, string.Empty);
    }

    //40706 - Check merchant worked
    public static DataTable CheckMerchantIsWorked(DateTime reportDate, bool isMCFRisk, string merchantNumber = "", string parentCycleID = "")
    {
        string spa = isMCFRisk ? "spa_Risk_MCF_Check_MerchantWorked" : "spa_Risk_Check_MerchantWorked";
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@MerchantNumber", string.IsNullOrEmpty(merchantNumber) ? RiskSessionManager.currentMerchantNumber : merchantNumber, DbType.String)); ;
        parameters.Add(new FilterParameter("@ReportDate", reportDate, DbType.DateTime));
        if (!string.IsNullOrEmpty(parentCycleID))
        {
            parameters.Add(new FilterParameter("@ParentCycleID", int.Parse(parentCycleID), DbType.Int32));
        }
        return WebServices.RiskServices.GetReports(spa, parameters);
    }

    public static string GetMessageWorked(DataTable tbWorked, string messageTpl)
    {
        DateTime workedDate = new DateTime();
        DateTime.TryParse(tbWorked.Rows[0]["WorkedDate"].ToString(), out workedDate);
        return string.Format(messageTpl, tbWorked.Rows[0]["UserID"].ToString(), workedDate.ToString("yyyy/MM/dd hh:mm:ss tt"));
    }
    //End

    //44648 - VW - Implement Change Log to review permission changes made to user roles
    public static string GetChangeLogLastestInfo(int hierarchyId)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@HierarchyID", hierarchyId, DbType.Int32));

        DataTable dt = WebServices.SecurityServices.GetReports("spa_SEC_GetChangeLogLastedForHierarchy", parameters);

        string lastestInfo = string.Empty;
        if (dt.HasData())
        {
            DateTime logTime = new DateTime();
            DateTime.TryParse(dt.Rows[0]["ChangedDateTime"].ToString(), out logTime);

            string changeDateTime = logTime.ToString("MM/dd/yyyy - hh:mm tt");
            lastestInfo = string.Format(Resources.GeneralFuncsLib.ChangeLogInfo, changeDateTime, dt.Rows[0]["ChangedBy"].ToString());
        }
        return lastestInfo;
    }
    //End


    //44894 - VW- Merchant Note Default Preferences via User Mgmt Settings
    public static string GenerateRgba(string hexCode, string opacity)
    {
        Color color = ColorTranslator.FromHtml(hexCode);
        int r = Convert.ToInt16(color.R);
        int g = Convert.ToInt16(color.G);
        int b = Convert.ToInt16(color.B);
        return string.Format("rgba({0},{1},{2},{3}) !important;", r, g, b, opacity);
    }
    //End

    //Sprint 8 - 46716 - Add Geographic Data to CC Transactions 
    public static bool IsAddGeographicData()
    {
        return GetDataOfExtendedSetting("ADD_GEOGRAPHIC_DATA").ToUpper().Contains(WebSiteSettings.WebSiteType.ToUpper());
    }

    public static bool IsEnableMerchantLink(SecurePage page)
    {
        return page.IsUserWithPermission("MerchProfile")
            && IsAddGeographicData();
    }

    public static void ShowHideColumnGeographicData(ASGrid grid)
    {
        grid.Columns.FindByUniqueName("Owner").Visible = IsAddGeographicData();
        grid.Columns.FindByUniqueName("Contact").Visible = IsAddGeographicData();
        grid.Columns.FindByUniqueName("City").Visible = IsAddGeographicData();
        grid.Columns.FindByUniqueName("State").Visible = IsAddGeographicData();
        grid.Columns.FindByUniqueName("Zip").Visible = IsAddGeographicData();
        grid.Columns.FindByUniqueName("Phone").Visible = IsAddGeographicData();
    }
    //End

    public static bool HasAssignmentSubsite()
    {
        string hasAssignmentSubsite = GetDataOfExtendedSetting("HasAssignmentSubsite");

        return !string.IsNullOrEmpty(hasAssignmentSubsite) && hasAssignmentSubsite.Equals("true", StringComparison.OrdinalIgnoreCase);
    }

    public static List<string> GetAdditionAccessFunctionsConfiguration()
    {
        string accessFunctions = GeneralFuncsLib.GetDataOfExtendedSetting("AdditionAccessFunctions");
        if (string.IsNullOrEmpty(accessFunctions))
            return new List<string>();

        var configs = accessFunctions.Split('|').ToList();
        var datas = new List<string>();

        foreach (var item in configs)
        {
            var itemConfig = item.Split(':').ToList();
            if (itemConfig.Count > 1)
            {
                datas.Add(item);
            }
        }

        return datas;
    }

    public static List<Permission> GetAdditionAccessFunctions()
    {
        var accessFunctions = GetAdditionAccessFunctionsConfiguration();
        var permissions = new List<Permission>();

        if (accessFunctions.Any())
        {
            var currentUser = SessionManager.CurrentUser;
            var currentSystem = SessionManager.CurrentSystem;
            var currentLanguage = SessionManager.CurrentLanguage;

            foreach (var item in accessFunctions)
            {
                var additionPermission = item.Split(':')[0];
                var group = item.Split(':')[1];

                if (HasUserPermission(additionPermission))
                {
                    var additionAccessFunction = WebServices.SecurityServices.GetPermissionsByUserGroupType(
                    currentUser.ASClient,
                    currentUser.UserID,
                    currentSystem,
                    group,
                    WebSiteConstants.PERMISSION_TYPE_ACCESS_FUNC,
                    currentLanguage);

                    if (additionAccessFunction != null && additionAccessFunction.Count > 0)
                    {
                        foreach (Permission itemPermission in additionAccessFunction)
                        {
                            permissions.Add(itemPermission);
                        }
                    }
                }
            }
        }

        return permissions;
    }
    public static bool HasShadowUnderwriting()
    {
        string hasShadowUnderwriting = GetDataOfExtendedSetting("Has_Shadow_Underwriting");

        return !string.IsNullOrEmpty(hasShadowUnderwriting) && hasShadowUnderwriting.Equals("true", StringComparison.OrdinalIgnoreCase);
    }

    public static bool IsOpenRecurringSystemMessage()
    {
        bool isShow = GetDataOfExtendedSetting("ShowRecurringSystemMessage") == "true";
        bool isMerchant = SessionManager.CurrentUser.EntityType == 12;
        bool isOpen = SessionManager.IsOpenRecurringSystemMessage && isMerchant && isShow;
        SetCookie("is_open_recurring_system_message", isOpen.ToString());
        return isOpen;
    }

    public static DataTable GetAcquirers(CardTypes cardTypeEnum, string acquirerId)
    {
        string cardType = cardTypeEnum == CardTypes.Visa ? "VI" : "MC";
        acquirerId = acquirerId.Replace("%", "[%]").Replace(",", "[,]").Replace("^", "[^]").Replace("_", "[_]");
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(false);
        parameters.Add(new FilterParameter("@AcquirerID", acquirerId, DbType.String));
        parameters.Add(new FilterParameter("@CardTypeCode", cardType, DbType.String));
        DataTable dt = WebServices.RiskServices.GetReports("spa_RM_RCF_GetAcquirerList", parameters);
        return dt;
    }
}

public static class Formatter
{
    /// <summary>
    /// Return Display Date String (Format MM/dd/yyyy)
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static string ToGenericDateString(this DateTime dt)
    {
        return dt.ToString(WebSiteConstants.DATE_FORMAT);
    }

    public static object FormatDataToMDash(object val)
    {
        return (val == null || string.IsNullOrEmpty(val.ToString()) ? WebSiteConstants.HTML_EM_DASH : val);
    }

    public static string ToMMddyyyy(DateTime? dateTime)
    {
        if (dateTime.HasValue)
        {
            return dateTime.Value.ToGenericDateString();
        }
        return string.Empty;
    }

    public static string ToMMddyyyyhhmmnsstt(string dateTime)
    {
        DateTime dt;
        if (DateTime.TryParse(dateTime, out dt))
        {
            return dt.ToString("MM/dd/yyyy hh:mm:ss tt");
        }
        return string.Empty;
    }
}

public static class RiskExportAs
{
    #region MgmtExporter
    public static void DownloadExportedFile(string reportTitle, string reportFileName, string reportType, string exportType, FilterParameterCollection parameters)
    {
        DownloadExportedFile(reportTitle, reportFileName, reportType, exportType, parameters, false);
    }

    public static void DownloadExportedFile(string reportTitle, string reportFileName, string reportType, string exportType, FilterParameterCollection parameters, bool isMCFRisk)
    {
        string strExtension = "";
        if (exportType == AS.Controls.UserControls.UxExport.ExportType.Excel.ToString().ToLower())
            strExtension = ".xls";
        else if (exportType == AS.Controls.UserControls.UxExport.ExportType.CSV.ToString().ToLower())
            strExtension = ".csv";
        string fileName = GeneralFuncsLib.GetFileName(reportFileName) + strExtension;

        var url = string.Format("{0}?reportType={1}&exportType={2}&culture={3}&currencyFormat={4}&IsMgmt=1&isMCFRisk={5}",
            ConfigurationSettings.AppSettings["ExportWSDownloadUrl"],
            AS.Common.DataProtection.Cryptophy.EncryptText(reportType),
            AS.Common.DataProtection.Cryptophy.EncryptText(exportType),
            AS.Common.DataProtection.Cryptophy.EncryptText(GeneralFuncsLib.GetCurrentCulture()),
            AS.Common.DataProtection.Cryptophy.EncryptText(SessionManager.CurrencyFortmat),
            isMCFRisk);

        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";
        request.Headers.Add("ClientId", SessionManager.CurrentClient.ToString());
        string postData = GetPostData(reportTitle, parameters);
        byte[] byteArray = Encoding.UTF8.GetBytes(postData);
        request.ContentLength = byteArray.Length;

        Stream dataStream = request.GetRequestStream();
        dataStream.Write(byteArray, 0, byteArray.Length);
        dataStream.Close();

        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + VeraCodeSolution.RemoveCRLF(fileName));
        if (exportType == AS.Controls.UserControls.UxExport.ExportType.Excel.ToString().ToLower())
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
        else if (exportType == AS.Controls.UserControls.UxExport.ExportType.CSV.ToString().ToLower())
            HttpContext.Current.Response.ContentType = "text/csv";
        else
            HttpContext.Current.Response.Close();

        Stream responseStream = response.GetResponseStream();
        int length = 0;
        const int MAX_BYTE = 4000;
        byte[] buffer = new byte[MAX_BYTE];
        do
        {
            if (HttpContext.Current.Response.IsClientConnected)
            {
                length = responseStream.Read(buffer, 0, MAX_BYTE);
                HttpContext.Current.Response.OutputStream.Write(buffer, 0, length);
                HttpContext.Current.Response.Flush();
                buffer = new byte[MAX_BYTE];
            }
            else
            {
                length = -1;
            }
        }
        while (length > 0);
        responseStream.Close();
        HttpContext.Current.Response.End();
    }

    private static string GetPostData(string reportTitle, FilterParameterCollection parameters)
    {
        reportTitle = Convert.ToBase64String(Encoding.UTF8.GetBytes(reportTitle));
        string strPostData = "ReportTitle=" + HttpUtility.UrlEncode(reportTitle) + "&"; //Report title
        foreach (FilterParameter p in parameters)
        {
            string strKey = p.ParameterName.ToString().Replace("@", "");
            string strValue = HttpUtility.UrlEncode(AS.Common.DataProtection.Cryptophy.EncryptText(p.ParameterValue.ToString()));
            string strType = p.ParameterType.ToString();
            strPostData += strKey + "=" + strValue + "[[" + strType + "]]&";
        }
        return strPostData.Substring(0, strPostData.Length - 1);
    }
    #endregion
}