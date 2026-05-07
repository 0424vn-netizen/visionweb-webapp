using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;

using Telerik.Web.UI;

using AS.Controls;
using AS.Security.WS.Entities;
using AS.Common.DBManager;
using System.Collections.Generic;
using System.Text;
using AS.Web.UI.Controls;
using System.IO;
using System.Security.Cryptography;
using As.VisionWeb.Web.Entity;
using Newtonsoft.Json;
using AS.Security.WS.Entities.Utility;

/// <summary>
/// Summary description for SessionManager
/// </summary>
public static partial class SessionManager
{

    public static int CurrentSystem
    {
        get
        {
            if (HttpContext.Current.Session["CurrentSystem"] != null)
            {
                return (int)HttpContext.Current.Session["CurrentSystem"];
            }
            else
            {
                return 0;
            }

        }
        set
        {
            HttpContext.Current.Session["CurrentSystem"] = value;
        }
    }



    public static int CurrentClient
    {
        get
        {
            if (HttpContext.Current.Session["CurrentClient"] != null)
            {
                return (int)HttpContext.Current.Session["CurrentClient"];
            }
            else
            {
                return 0;
            }

        }
        set
        {
            HttpContext.Current.Session["CurrentClient"] = value;
        }
    }

    public static int CurrentLanguage
    {
        get
        {
            if (HttpContext.Current.Session["CurrentLanguageID"] != null)
            {
                return (int)HttpContext.Current.Session["CurrentLanguageID"];                
            }
            else
            {
                // default is English
                return 1;
            }

        }
        set
        {
            HttpContext.Current.Session["CurrentLanguageID"] = value;
        }
    }

    //Sprint 6 - 46652 - AW Multi-currency Transaction Display 
    public static string CurrencyFortmat
    {
        get
        {
            if (HttpContext.Current.Session["CurrencyFortmat"] != null)
            {
                return HttpContext.Current.Session["CurrencyFortmat"].ToString();
            }
            else
            {
                return "en-US";
            }

        }
        set
        {
            HttpContext.Current.Session["CurrencyFortmat"] = value;
        }
    }
    
    public static string CurrencySymbol
    {
        get
        {
            if (HttpContext.Current.Session["CurrencySymbol"] != null)
            {
                return HttpContext.Current.Session["CurrencySymbol"].ToString();
            }
            else
            {
                return "$";
            }

        }
        set
        {
            HttpContext.Current.Session["CurrencySymbol"] = value;
        }
    }

    // For Risk Site ID that user selects on the dropdown 
    public static int CurrentRiskSiteID
    {
        get
        {
            if (HttpContext.Current.Session["CurrentRiskSiteID"] != null)
            {
                return (int)HttpContext.Current.Session["CurrentRiskSiteID"];
            }
            else
            {
                return 0;
            }

        }
        set
        {
            HttpContext.Current.Session["CurrentRiskSiteID"] = value;
        }
    }


    public static Boolean ShowUnreadMessages
    {
        get
        {
            if (HttpContext.Current.Session["ShowUnreadMessages"] != null)
            {
                return (bool)HttpContext.Current.Session["ShowUnreadMessages"];
            }
            else
            {
                return false;
            }

        }
        set
        {
            HttpContext.Current.Session["ShowUnreadMessages"] = value;
        }

    }

    #region Report Filter

    public static DataTable HierarchyFilter
    {
        get
        {
            if (HttpContext.Current.Session["HierarchyFilterList"] != null)
            {
                return (DataTable)HttpContext.Current.Session["HierarchyFilterList"];
            }
            else
            {
                return null;
            }
        }
        set
        {
            HttpContext.Current.Session["HierarchyFilterList"] = value;
        }
    }

    public static DataTable HierarchyFilterExtend
    {
        get
        {
            if (HttpContext.Current.Session["HierarchyFilterExtend"] != null)
            {
                return (DataTable)HttpContext.Current.Session["HierarchyFilterExtend"];
            }
            else
            {
                return null;
            }
        }
        set
        {
            HttpContext.Current.Session["HierarchyFilterExtend"] = value;
        }
    }

    public static DataTable AllHierarchyFilter
    {
        get
        {
            if (HttpContext.Current.Session["AllHierarchyFilterList"] == null)
            {
                FilterParameterCollection parameters = new FilterParameterCollection();
                parameters.Add(new AS.Common.DBManager.FilterParameter("@UserMode", "CSUSER", DbType.AnsiString));
                parameters.Add(new AS.Common.DBManager.FilterParameter("@UserID", SessionManager.CurrentUser.UserID, DbType.AnsiString));
                parameters.Add(new AS.Common.DBManager.FilterParameter("@ASClient", SessionManager.CurrentUser.ASClient, DbType.Int32));
                parameters.AddLanguageID();

                var hierarchyFilters = WebServices.RiskServices.GetReports("spa_GetAllHierarchyFilters", parameters);
                if (hierarchyFilters == null || hierarchyFilters.Rows.Count == 0)
                {
                    LogHepler.WriteLogWarn("Get AllHierarchyFilter", "AllHierarchyFilterList is null", "spa_GetAllHierarchyFilters", parameters);
                }

                HttpContext.Current.Session["AllHierarchyFilterList"] = hierarchyFilters;
            }
            return (DataTable)HttpContext.Current.Session["AllHierarchyFilterList"];
        }
        set
        {
            if (value == null || value.Rows.Count == 0)
            {
                LogHepler.WriteLogWarn("Set AllHierarchyFilter", "AllHierarchyFilterList is null", string.Empty);
            }

            HttpContext.Current.Session["AllHierarchyFilterList"] = value;
        }
    }
    public static HierarchyDetail MerchantHierarchyFilter
    {
        get
        {
            if (HttpContext.Current.Session["MerchantHierarchyFilter"] != null)
            {
                return (HierarchyDetail)HttpContext.Current.Session["MerchantHierarchyFilter"];
            }
            else
            {
                return null;
            }
        }
        set
        {
            HttpContext.Current.Session["MerchantHierarchyFilter"] = value;
        }
    }
    public static DataTable HierarchyFilterForMerchantProfile
    {
        get
        {
            if (HttpContext.Current.Session["HierarchyFilterForMerchantProfileList"] != null)
            {
                return (DataTable)HttpContext.Current.Session["HierarchyFilterForMerchantProfileList"];
            }
            else
            {
                return null;
            }
        }
        set
        {
            HttpContext.Current.Session["HierarchyFilterForMerchantProfileList"] = value;
        }
    }

    public static DataTable HierarchyFilterDrillDown
    {
        get
        {
            if (HttpContext.Current.Session["HierarchyFilterLevel"] != null)
            {
                return (DataTable)HttpContext.Current.Session["HierarchyFilterLevel"];
            }
            else
            {


                return null;
            }
        }
        set
        {
            HttpContext.Current.Session["HierarchyFilterLevel"] = value;
        }
    }

    public static HierarchyFilterValue FromHierarchyDrilldown
    {
        get
        {
            if (HttpContext.Current.Session["FromHierarchyDrilldown"] != null)
            {
                return (HierarchyFilterValue)HttpContext.Current.Session["FromHierarchyDrilldown"];
            }
            else
            {


                return null;
            }
        }
        set
        {
            HttpContext.Current.Session["FromHierarchyDrilldown"] = value;
        }
    }

    #endregion

    #region Risk Hierarchy Filter

    public static DataTable RiskHierarchyFilter
    {
        get
        {
            if (HttpContext.Current.Session["RiskHierarchyFilter"] != null)
            {
                return (DataTable)HttpContext.Current.Session["RiskHierarchyFilter"];
            }
            else
            {
                return null;
            }
        }
        set
        {
            HttpContext.Current.Session["RiskHierarchyFilter"] = value;
        }
    }

    #endregion

    #region Message Hierarchy Filter

    public static DataTable MessageHierarchyFilter
    {
        get
        {
            if (HttpContext.Current.Session["MessageHierarchyFilter"] != null)
            {
                return (DataTable)HttpContext.Current.Session["MessageHierarchyFilter"];
            }
            else
            {
                return null;
            }
        }
        set
        {
            HttpContext.Current.Session["MessageHierarchyFilter"] = value;
        }
    }

    #endregion



    #region Risk Multiwatch
    public static DataTable RiskHierarchyMultiwatchGird
    {
        get
        {
            if (HttpContext.Current.Session["RiskHierarchyMultiwatchGird"] != null)
            {
                return (DataTable)HttpContext.Current.Session["RiskHierarchyMultiwatchGird"];
            }
            else
            {
                return null;
            }
        }
        set
        {
            HttpContext.Current.Session["RiskHierarchyMultiwatchGird"] = value;
        }
    }
    #endregion


    #region Risk Manage Assignment
    public static StringBuilder DataCSV
    {
        get
        {
            if (HttpContext.Current.Session["DataCSV"] != null)
            {
                return (StringBuilder)HttpContext.Current.Session["DataCSV"];
            }
            else
            {
                return null;
            }
        }
        set
        {
            HttpContext.Current.Session["DataCSV"] = value;
        }
    }

    public static List<DataTable> AssignmentGroupTables
    {
        get
        {
            if (HttpContext.Current.Session["AssignmentGroupTables"] != null)
            {
                return (List<DataTable>)HttpContext.Current.Session["AssignmentGroupTables"];
            }
            else
            {
                return null;
            }
        }
        set
        {
            HttpContext.Current.Session["AssignmentGroupTables"] = value;
        }
    }
    public static List<string> HeaderList
    {
        get
        {
            if (HttpContext.Current.Session["HeaderList"] != null)
            {
                return (List<string>)HttpContext.Current.Session["HeaderList"];
            }
            else
            {
                return null;
            }
        }
        set
        {
            HttpContext.Current.Session["HeaderList"] = value;
        }
    }

    public static string AssignementFilteringOptions
    {
        get
        {
            if (HttpContext.Current.Session["AssignementFilteringOptions"] != null)
            {
                return (string)HttpContext.Current.Session["AssignementFilteringOptions"];
            }
            else
            {
                return string.Empty;
            }
        }
        set
        {
            HttpContext.Current.Session["AssignementFilteringOptions"] = value;
        }
    }

    public static string UserGroupSort
    {
        get
        {
            if (HttpContext.Current.Session["UserGroupSort"] != null)
            {
                return (string)HttpContext.Current.Session["UserGroupSort"];
            }
            else
            {
                return string.Empty;
            }
        }
        set
        {
            HttpContext.Current.Session["UserGroupSort"] = value;
        }
    }


    #endregion


    #region system requires
    public static AS.Controls.IPageStateCollection RefState
    {
        get
        {
            if (HttpContext.Current.Session["PageState"] == null)
            {
                HttpContext.Current.Session["PageState"] = new AS.Controls.PageStateCollection();
            }
            return (AS.Controls.IPageStateCollection)HttpContext.Current.Session["PageState"];

        }
    }
    public static int ExportWSThreshold
    {
        get
        {
            if (HttpContext.Current.Session["ASGRID_ExportThresholdOfRows"] == null)
            {
                int Temp = 0;
                int.TryParse(WebSiteSettings.ExportThresholdOfRows, out Temp);
                HttpContext.Current.Session["ASGRID_ExportThresholdOfRows"] = Temp;
            }
            return (int)HttpContext.Current.Session["ASGRID_ExportThresholdOfRows"];
        }
    }
    #endregion

    #region security requires
    public static bool IsLoggedIn
    {
        get
        {
            if (HttpContext.Current.Session["IsLoggedIn"] != null)
            {
                return (bool)HttpContext.Current.Session["IsLoggedIn"];
            }
            else
            {
                return false;
            }

        }
        set
        {
            HttpContext.Current.Session["IsLoggedIn"] = value;
        }
    }
    public static bool PasswordExpired
    {
        get
        {
            if (HttpContext.Current.Session["PasswordExpired"] != null)
            {
                return (bool)HttpContext.Current.Session["PasswordExpired"];
            }
            else
            {
                return false;
            }

        }
        set
        {
            HttpContext.Current.Session["PasswordExpired"] = value;
        }
    }

    public static bool PasswordExpiredNearly
    {
        get
        {
            if (HttpContext.Current.Session["PasswordExpiredNearly"] != null)
            {
                return (bool)HttpContext.Current.Session["PasswordExpiredNearly"];
            }
            else
            {
                return false;
            }

        }
        set
        {
            HttpContext.Current.Session["PasswordExpiredNearly"] = value;
        }
    }

    public static string PasswordTemporary
    {
        get
        {
            if (HttpContext.Current.Session["PasswordTemporary"] != null)
            {
                return (string)HttpContext.Current.Session["PasswordTemporary"];
            }
            else
            {
                return string.Empty;
            }

        }
        set
        {
            HttpContext.Current.Session["PasswordTemporary"] = value;
        }
    }

    public static int DayRemainingPasswordExpired
    {
        get
        {
            if (HttpContext.Current.Session["DayRemainingPasswordExpired"] != null)
            {
                return (int)HttpContext.Current.Session["DayRemainingPasswordExpired"];
            }
            else
            {
                return 0;
            }

        }
        set
        {
            HttpContext.Current.Session["DayRemainingPasswordExpired"] = value;
        }
    }

    public static string IncludeExcludeItem
    {
        get
        {
            if (HttpContext.Current.Session["IncludeExcludeItem"] != null)
            {
                return HttpContext.Current.Session["IncludeExcludeItem"].ToString();
            }
            return string.Empty;

        }
        set
        {
            HttpContext.Current.Session["IncludeExcludeItem"] = value;
        }
    }
    public static bool IsIncludeItem
    {
        get
        {
            if (HttpContext.Current.Session["IsIncludeItem"] != null)
            {
                return bool.Parse(HttpContext.Current.Session["IsIncludeItem"].ToString());
            }
            return true;
        }
        set
        {
            HttpContext.Current.Session["IsIncludeItem"] = value;
        }
    }


    public static User CurrentUser
    {
        get
        {
            if (HttpContext.Current.Session["ASLogUser"] != null)
            {
                return (User)HttpContext.Current.Session["ASLogUser"];
            }
            else
            {
                HttpContext.Current.Session.Abandon();
                HttpContext.Current.Session.Clear();
                FormsAuthentication.SignOut();
                if (LoginUrl == "[jumpsite]") HttpContext.Current.Response.Redirect("~/gen_ms_Login.aspx", true);
                else
                    HttpContext.Current.Response.Redirect(LoginUrl, true);
                return null;
            }

        }
        set
        {
            HttpContext.Current.Session["ASLogUser"] = value;

            if (value != null)
            {
                var shareUser = new UserInfo()
                {
                    ASClient = value.ASClient,
                    RecId = value.RecId,
                    SiteID = value.SiteID,
                    UserID = value.UserID,
                    UserNameFull = value.UserNameFull
                };
                ShareUserInfo = JsonConvert.SerializeObject(shareUser);
            }            
        }
    }
    public static User LandingCurrentUser
    {
        get
        {
            return (User)HttpContext.Current.Session["ASLandingLogUser"];
        }
        set
        {
            HttpContext.Current.Session["ASLandingLogUser"] = value;
        }
    }

    public static string SiteJumper
    {
        get
        {
            if (HttpContext.Current.Session["SiteJumper"] != null)
            {
                return (string)HttpContext.Current.Session["SiteJumper"];
            }
            else
            {
                return null;
            }

        }
        set
        {
            HttpContext.Current.Session["SiteJumper"] = value;
        }
    }
    public static string JumpFrom
    {
        get
        {
            if (HttpContext.Current.Session["JumpFrom"] != null)
            {
                return (string)HttpContext.Current.Session["JumpFrom"];
            }
            else
            {
                return null;
            }

        }
        set
        {
            HttpContext.Current.Session["JumpFrom"] = value;
        }
    }
    public static User ResetPasswordUser
    {
        get
        {
            if (HttpContext.Current.Session["ResetPasswordUser"] != null)
            {
                return (User)HttpContext.Current.Session["ResetPasswordUser"];
            }
            else
            {
                return null;
            }

        }
        set
        {
            HttpContext.Current.Session["ResetPasswordUser"] = value;
        }
    }


    public static string CurrentUserPermissions
    {
        get
        {
            if (HttpContext.Current.Session["UserPermissions"] != null)
            {
                return (string)HttpContext.Current.Session["UserPermissions"];
            }
            else
            {
                return "";
            }

        }
        set
        {

            HttpContext.Current.Session["UserPermissions"] = value;
        }
    }



    public static ASTheme CurrentUserTheme
    {
        get
        {
            if (HttpContext.Current.Session["CurrentUserTheme"] != null)
            {

                return (ASTheme)HttpContext.Current.Session["CurrentUserTheme"];
            }
            else
            {
                return null;
            }

        }
        set
        {
            HttpContext.Current.Session["CurrentUserTheme"] = value;
            HttpContext.Current.Session["Theme"] = value.ThemeName;
        }
    }

    public static string CurrentUserThemeName
    {
        get
        {
            if (CurrentUserTheme == null)
            {
                HttpContext.Current.Session["Theme"] = "Default";
            }
            else if (HttpContext.Current.Session["Theme"] == null)
            {
                HttpContext.Current.Session["Theme"] = CurrentUserTheme.ThemeName;
            }
            return (string)HttpContext.Current.Session["Theme"];
        }
    }

    public static int CurrentHierarchyId
    {
        get
        {
            if (HttpContext.Current.Session["CurrentHierarchyId"] != null)
            {
                return (int)HttpContext.Current.Session["CurrentHierarchyId"];
            }
            else
            {
                return 0;
            }

        }
        set
        {
            HttpContext.Current.Session["CurrentHierarchyId"] = value;
        }
    }

    public static HierarchyCollection CurrentUserRoles
    {
        get
        {
            if (HttpContext.Current.Session["CurrentUserRoles"] != null)
            {
                return (HierarchyCollection)HttpContext.Current.Session["CurrentUserRoles"];
            }
            else
            {
                return null;
            }

        }
        set
        {
            HttpContext.Current.Session["CurrentUserRoles"] = value;
        }
    }

    public static WebSiteEnums.UserHierarchyMode CurrentUserType
    {
        get
        {
            return HttpContext.Current.Session["CurrentUserType"] != null ? (WebSiteEnums.UserHierarchyMode)HttpContext.Current.Session["CurrentUserType"] : WebSiteEnums.UserHierarchyMode.Unknown;
        }
        set
        {
            HttpContext.Current.Session["CurrentUserType"] = value;
        }
    }

    public static int CurrentUserSiteId
    {
        get
        {
            if (HttpContext.Current.Session["CurrentUserSiteId"] != null)
            {
                return (int)HttpContext.Current.Session["CurrentUserSiteId"];
            }
            else
            {
                return -1;
            }

        }
        set
        {
            HttpContext.Current.Session["CurrentUserSiteId"] = value;
        }
    }

    public static bool ForceChangePassword
    {
        get
        {
            if (HttpContext.Current.Session["ForceChangePassword"] != null)
            {
                return (bool)HttpContext.Current.Session["ForceChangePassword"];
            }
            else
            {
                return false;
            }

        }
        set
        {
            HttpContext.Current.Session["ForceChangePassword"] = value;
        }
    }
    public static int ForgetPasswordAttempt
    {
        get
        {
            if (HttpContext.Current.Session["ForgetPasswordAttempt"] != null)
            {
                return (int)HttpContext.Current.Session["ForgetPasswordAttempt"];
            }
            else
            {
                return 0;
            }

        }
        set
        {
            HttpContext.Current.Session["ForgetPasswordAttempt"] = value;
        }
    }

    public static User ForgetPasswordUser
    {
        get
        {
            if (HttpContext.Current.Session["ForgetPasswordUser"] != null)
            {
                return (User)HttpContext.Current.Session["ForgetPasswordUser"];
            }
            else
            {
                return null;
            }

        }
        set
        {
            HttpContext.Current.Session["ForgetPasswordUser"] = value;
        }
    }

    static string LoginUrl
    {
        get
        {
            HttpCookie url_cokkie = HttpContext.Current.Request.Cookies["login_url"];
            if (url_cokkie != null)
            {
                return url_cokkie.Value;
            }
            else
            {
                return FormsAuthentication.LoginUrl;
            }
        }
        set
        {
            HttpContext.Current.Response.Cookies.Add(new HttpCookie("login_url", value));
        }
    }

    public static DataTable CurrentMenuItems
    {
        get
        {
            if (HttpContext.Current.Session["CurrentMenuItems"] != null) return (DataTable)HttpContext.Current.Session["CurrentMenuItems"];
            else return null;
        }
        set
        {
            HttpContext.Current.Session["CurrentMenuItems"] = value;
        }
    }

    public static void GetHeaderMenu()
    {
        HttpContext.Current.Items["InHeaderLoop"] = true;
        TextWriter writer = new StringWriter();
        HttpContext.Current.Server.Execute("~/header.aspx", writer);
        string html = writer.ToString();
        html = System.Text.RegularExpressions.Regex.Replace(html, @"action="".*?""", @"action=""" + VirtualPathUtility.ToAbsolute("~/header.aspx") + @"""");
        HttpContext.Current.Session["HeaderMenu"] = html;
        //add checksum 
        string hash = "";
        using (MD5 md5 = MD5.Create())
        {
            hash = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(html)));
        }
        HttpContext.Current.Session["HeaderMenuCheckSum"] = hash;
        
    }

    public static PasswordValidationRule PwdValidationRule
    {
        get
        {
            if (HttpContext.Current.Session["PwdValidationRule"] != null)
            {
                return (PasswordValidationRule)HttpContext.Current.Session["PwdValidationRule"];
            }
            else
            {
                return null;
            }
        }
        set
        {
            HttpContext.Current.Session["PwdValidationRule"] = value;
        }
    }
    #endregion



    #region MPS Session Variables

    public static string CurrentMerchantNumber
    {
        get
        {
            if (HttpContext.Current.Session["CurrentMerchantNumber"] != null)
            {
                return HttpContext.Current.Session["CurrentMerchantNumber"].ToString();
            }
            else
            {
                return null;
            }
        }
        set
        {
            HttpContext.Current.Session["CurrentMerchantNumber"] = value;
        }
    }

    // Share Merchant Number with Sponsor Underwriting Shadow module
    public static string ShareMerchantNumber
    {
        get
        {
            if (HttpContext.Current.Session["VW_ShareMerchantNumber"] != null)
            {
                return HttpContext.Current.Session["VW_ShareMerchantNumber"].ToString();
            }

            return string.Empty;
        }
        set
        {
            HttpContext.Current.Session["VW_ShareMerchantNumber"] = value;
        }
    }

    public static string ShareUserInfo
    {
        get
        {
            if (HttpContext.Current.Session["VW_ShareUserInfo"] != null)
            {
                return HttpContext.Current.Session["VW_ShareUserInfo"].ToString();
            }

            return string.Empty;
        }
        set
        {
            HttpContext.Current.Session["VW_ShareUserInfo"] = value;
        }
    }



    #endregion

    #region Session for merchant profile

    public static int CurrentUserViewMode
    {
        get
        {
            if (HttpContext.Current.Session["CurrentUserViewMode"] != null)
            {
                return (int)HttpContext.Current.Session["CurrentUserViewMode"];
            }
            else
            {
                return 0;
            }

        }
        set
        {
            HttpContext.Current.Session["CurrentUserViewMode"] = value;
        }
    }

    #endregion


    #region Case Management
    public static string CurrentTicketComment
    {
        get
        {
            if (HttpContext.Current.Session["CM_CurrentTicketComment"] != null)
            {
                return (string)HttpContext.Current.Session["CM_CurrentTicketComment"];
            }
            else
            {
                return "";
            }

        }
        set
        {
            HttpContext.Current.Session["CM_CurrentTicketComment"] = value;
        }
    }

    public static bool CM_IsExistStatement
    {
        get
        {
            if (HttpContext.Current.Session["CM_IsExistStatement"] != null)
            {
                return (bool)HttpContext.Current.Session["CM_IsExistStatement"];
            }
            else
            {
                return false;
            }

        }
        set
        {
            HttpContext.Current.Session["CM_IsExistStatement"] = value;
        }
    }
    #endregion

    #region Client Info
    public static ClientInfo ClientInfo
    {
        get
        {
            if (HttpContext.Current.Session["ClientInfo"] != null)
            {
                return (ClientInfo)HttpContext.Current.Session["ClientInfo"];
            }
            else
            {
                return null;
            }
        }
        set
        {
            HttpContext.Current.Session["ClientInfo"] = value;
        }
    }
    public static DataTable Processors
    {
        get
        {
            if (HttpContext.Current.Session["Processors"] == null)
            {
                FilterParameterCollection parameters = new FilterParameterCollection();
                HttpContext.Current.Session["Processors"] = WebServices.CsReportServices.GetReports("spa_GetClients", parameters);
            }
            return HttpContext.Current.Session["Processors"] as DataTable;
        }
    }
    #endregion

    #region JumpSite Info

    public static string JumpSource
    {
        get
        {
            if (HttpContext.Current.Session["JumpSource"] != null)
            {
                return HttpContext.Current.Session["JumpSource"].ToString();
            }
            else
            {
                return null;
            }
        }
        set
        {
            HttpContext.Current.Session["JumpSource"] = value;
        }
    }

    #endregion

    public static string UserRoleType
    {
        get
        {
            if (HttpContext.Current.Session["UserRoleType"] != null)
            {
                return HttpContext.Current.Session["UserRoleType"].ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        set
        {
            HttpContext.Current.Session["UserRoleType"] = value;
        }
    }

    public static string ClientFrameInfo
    {
        get
        {
            if (HttpContext.Current.Session["ClientFrameInfo"] != null)
            {
                return HttpContext.Current.Session["ClientFrameInfo"].ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        set
        {
            HttpContext.Current.Session["ClientFrameInfo"] = value;
        }
    }

    public static bool UsePrimaryPrefix
    {
        get
        {
            if (HttpContext.Current.Session["UsePrimaryPrefix"] != null)
            {
                return (bool)HttpContext.Current.Session["UsePrimaryPrefix"];
            }
            else
            {
                return false;
            }
        }
        set
        {
            HttpContext.Current.Session["UsePrimaryPrefix"] = value;
        }
    }

    public static string HierarchyName
    {
        get
        {
            if (HttpContext.Current.Session["HierarchyName"] != null)
            {
                return HttpContext.Current.Session["HierarchyName"].ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        set
        {
            HttpContext.Current.Session["HierarchyName"] = value;
        }
    }

    public static HierarchyFilterValue CurrentReportFilter
    {
        get
        {
            return (HierarchyFilterValue)HttpContext.Current.Session["SavedReportFilterValue"];
        }
        set
        {
            HttpContext.Current.Session["SavedReportFilterValue"] = value;
        }
    }

    public static HierarchyFilterValue CurrentCardSearchReportFilter
    {
        get
        {
            return (HierarchyFilterValue)HttpContext.Current.Session["CurrentCardSearchReportFilter"];
        }
        set
        {
            HttpContext.Current.Session["CurrentCardSearchReportFilter"] = value;
        }
    }

    public static Guid CaseSiteJumpTicket
    {
        get
        {
            if (HttpContext.Current.Session["CaseSiteJumpTicket"] != null)
            {
                return new Guid(HttpContext.Current.Session["CaseSiteJumpTicket"].ToString());
            }
            else
            {
                return Guid.Empty;
            }

        }
        set
        {
            HttpContext.Current.Session["CaseSiteJumpTicket"] = value;
        }
    }
    public static string CaseSiteJumpPassword
    {
        get
        {
            if (HttpContext.Current.Session["CaseSiteJumpPassword"] != null)
            {
                return HttpContext.Current.Session["CaseSiteJumpPassword"].ToString();
            }
            else
            {
                return string.Empty;
            }

        }
        set
        {
            HttpContext.Current.Session["CaseSiteJumpPassword"] = value;
        }
    }
    public static string ClientContactInforKey
    {
        get
        {
            if (HttpContext.Current.Session["ClientContactInforKey"] != null)
            {
                return HttpContext.Current.Session["ClientContactInforKey"].ToString();
            }
            else
            {
                return "ClientInformation";
            }

        }
        set
        {
            HttpContext.Current.Session["ClientContactInforKey"] = value;
        }
    }

    public static IncomeFilterOptions IncomeFilterOption
    {
        get
        {
            if (HttpContext.Current.Session["IncomeFilterOption"] != null)
            {
                return (IncomeFilterOptions)HttpContext.Current.Session["IncomeFilterOption"];
            }
            else
            {
                return null;
            }
        }
        set
        {
            HttpContext.Current.Session["IncomeFilterOption"] = value;
        }

    }

    public static UserAccessFilterOptions UserAccessFilterOption
    {
        get
        {
            if (HttpContext.Current.Session["UserAccessFilterOption"] != null)
            {
                return (UserAccessFilterOptions)HttpContext.Current.Session["UserAccessFilterOption"];
            }
            else
            {
                return null;
            }
        }
        set
        {
            HttpContext.Current.Session["UserAccessFilterOption"] = value;
        }

    }


    public static ManageUserFilterOptions ManageUserFilterOption
    {
        get
        {
            if (HttpContext.Current.Session["ManageUserFilterOption"] != null)
            {
                return (ManageUserFilterOptions)HttpContext.Current.Session["ManageUserFilterOption"];
            }
            else
            {
                return null;
            }
        }
        set
        {
            HttpContext.Current.Session["UserAccessFilterOption"] = value;
        }
    }

    public static string UniqueSessionID
    {
        get
        {
            if (HttpContext.Current.Session["UniqueSessionID"] != null)
                return HttpContext.Current.Session["UniqueSessionID"].ToString();
            else
                return string.Empty;
        }
        set
        {
            HttpContext.Current.Session["UniqueSessionID"] = value;
        }
    }

    public static string ClientSettingFilePostfix
    {
        get
        {
            if (HttpContext.Current.Session["ClientSettingFilePostfix"] != null)
            {
                return HttpContext.Current.Session["ClientSettingFilePostfix"].ToString();
            }
            else
            {
                return null;
            }
        }
        set
        {
            HttpContext.Current.Session["ClientSettingFilePostfix"] = value;
        }
    }

    public static Guid SocialMediaSiteJumpTicket
    {
        get
        {
            if (HttpContext.Current.Session["SocialMediaSiteJumpTicket"] != null)
            {
                return new Guid(HttpContext.Current.Session["SocialMediaSiteJumpTicket"].ToString());
            }
            else
            {
                return Guid.Empty;
            }

        }
        set
        {
            HttpContext.Current.Session["SocialMediaSiteJumpTicket"] = value;
        }
    }
    public static string SocialMediaSiteJumpPassword
    {
        get
        {
            if (HttpContext.Current.Session["SocialMediaSiteJumpPassword"] != null)
            {
                return HttpContext.Current.Session["SocialMediaSiteJumpPassword"].ToString();
            }
            else
            {
                return string.Empty;
            }

        }
        set
        {
            HttpContext.Current.Session["SocialMediaSiteJumpPassword"] = value;
        }
    }

    public static bool SingleSignOnMenuMod
    {
        get
        {
            if (HttpContext.Current.Session["SingleSignOnMenuMod"] != null)
            {
                return (bool)HttpContext.Current.Session["SingleSignOnMenuMod"];
            }
            else
            {
                return false;
            }
        }
        set
        {
            HttpContext.Current.Session["SingleSignOnMenuMod"] = value;
        }
    }
    public static int SSOSecurityLevel
    {
        get
        {
            if (HttpContext.Current.Session["SSOSecurityLevel"] != null)
            {
                return (int)HttpContext.Current.Session["SSOSecurityLevel"];
            }
            else
            {
                return -1;
            }
        }
        set
        {
            HttpContext.Current.Session["SSOSecurityLevel"] = value;
        }
    }
    public static bool OpenStatementDetailPopup
    {
        get
        {
            if (HttpContext.Current.Session["OpenStatementDetailPopup"] != null)
            {
                return (bool)HttpContext.Current.Session["OpenStatementDetailPopup"];
            }
            else
            {
                return false;
            }
        }
        set
        {
            HttpContext.Current.Session["OpenStatementDetailPopup"] = value;
        }
    }

    //Use when export multi section
    public static List<int> SelectedFunctions
    {
        get
        {
            if (HttpContext.Current.Session["SelectedFunction"] != null)
                return (List<int>)HttpContext.Current.Session["SelectedFunction"];
            return new List<int>();
        }
        set
        {
            HttpContext.Current.Session.Remove("SelectedFunction");
            HttpContext.Current.Session["SelectedFunction"] = value;
        }
    }

    public static string ClientTimeZone 
    {
        get
        {
            if (HttpContext.Current.Session["ClientTimezone"] != null)
                return HttpContext.Current.Session["ClientTimezone"].ToString();
            else
                return string.Empty;
        }
        set
        {
            HttpContext.Current.Session["ClientTimezone"] = value;
        }
    }

    public static string VWLoginUrl
    {
        get
        {
            if (HttpContext.Current.Session["LoginUrl"] != null)
            {
                return (string)HttpContext.Current.Session["LoginUrl"];
            }
            else
            {
                return null;
            }

        }
        set
        {
            HttpContext.Current.Session["LoginUrl"] = value;
        }
    }

    public static DateTime? LastLoginDTS
    {
        get
        {
            if (HttpContext.Current.Session["LastLoginDTS"] != null)
            {
                return (DateTime)HttpContext.Current.Session["LastLoginDTS"];
            }
            else
            {
                return null;
            }

        }
        set
        {
            HttpContext.Current.Session["LastLoginDTS"] = value;
        }
    }

    #region Boarding Permission

    public static PermissionCollection SelectedGroupPermission
    {
        get
        {
            if (HttpContext.Current.Session["SelectedGroupPermission"].IsNotNullData())
                return HttpContext.Current.Session["SelectedGroupPermission"] as PermissionCollection;
            else
                return new PermissionCollection();
        }
        set
        {
            HttpContext.Current.Session["SelectedGroupPermission"] = value;
        }
    }
    //Contain permission of Role
    public static PermissionCollection RoleGroupPermissions
    {
        get 
        {
            if (HttpContext.Current.Session["RoleGroupPermission"].IsNotNullData())
                return HttpContext.Current.Session["RoleGroupPermission"] as PermissionCollection;
            else
                return new PermissionCollection();
        }
        set 
        {
            HttpContext.Current.Session["RoleGroupPermission"] = value;
        }
    }

    // Contains permission of User
    public static PermissionCollection UserGroupPermissions
    {
        get
        {
            if (HttpContext.Current.Session["UserGroupPermissions"].IsNotNullData())
                return HttpContext.Current.Session["UserGroupPermissions"] as PermissionCollection;
            else
                return new PermissionCollection();
        }
        set
        {
            HttpContext.Current.Session["UserGroupPermissions"] = value;
        }
    }

    public static string Organizations
    {
        get
        {
            if (HttpContext.Current.Session["Organizations"].IsNotNullData())
                return HttpContext.Current.Session["Organizations"] as string;
            return string.Empty;
        }
        set
        {
            HttpContext.Current.Session["Organizations"] = value;
        }
    }
    #endregion

    //Sprint 14 - 43358 - VW Risk CR-Design Change #2 to 39919
    public static string MerchantNoteSort
    {
        get
        {
            if (!HttpContext.Current.Session["MerchantNoteSort"].IsNullOrEmpty())
                return HttpContext.Current.Session["MerchantNoteSort"].ToString();
            return string.Empty;
        }
        set
        {
            HttpContext.Current.Session["MerchantNoteSort"] = value;
        }
    }

    public static string CurrentSortColumn
    {
        get
        {
            if (!HttpContext.Current.Session["CurrentSortColumn"].IsNullOrEmpty())
                return HttpContext.Current.Session["CurrentSortColumn"].ToString();
            return string.Empty;
        }
        set
        {
            HttpContext.Current.Session["CurrentSortColumn"] = value;
        }
    }

    //45627 - [CAYAN_Aperia] - Display Legal Address for TSYS portfolio
    public static string ExcludeAccessPermission
    {
        get
        {
            if (!HttpContext.Current.Session["ExcludeAccessPermission"].IsNullOrEmpty())
                return HttpContext.Current.Session["ExcludeAccessPermission"].ToString();
            return string.Empty;
        }
        set
        {
            HttpContext.Current.Session["ExcludeAccessPermission"] = value;
        }
    }

    public static string CaseHistorySort
    {
        get
        {
            if (HttpContext.Current.Session["CaseHistorySort"] != null)
                return HttpContext.Current.Session["CaseHistorySort"].ToString();
            return string.Empty;
        }
        set
        {
            HttpContext.Current.Session["CaseHistorySort"] = value;
        }
    }

    public static string CurrentSortColumnCase
    {
        get
        {
            if (HttpContext.Current.Session["CurrentSortColumnCase"] != null)
                return HttpContext.Current.Session["CurrentSortColumnCase"].ToString();
            return string.Empty;
        }
        set
        {
            HttpContext.Current.Session["CurrentSortColumnCase"] = value;
        }
    }

    public static bool IsAddNewCase
    {
        get
        {
            if (HttpContext.Current.Session["IsAddNewCase"] != null)
                return HttpContext.Current.Session["IsAddNewCase"].ToBoolean();
            return false;
        }
        set
        {
            HttpContext.Current.Session["IsAddNewCase"] = value;
        }
    }

    //45535 - FIS - Activation Report
    public static string FirstDateFilterType
    {
        get
        {
            if (HttpContext.Current.Session["FirstDateFilterType"] != null)
                return HttpContext.Current.Session["FirstDateFilterType"].ToString();
            return string.Empty;
        }
        set
        {
            HttpContext.Current.Session["FirstDateFilterType"] = value;
        }
    }
    public static string SecondDateFilterType
    {
        get
        {
            if (HttpContext.Current.Session["SecondDateFilterType"] != null)
                return HttpContext.Current.Session["SecondDateFilterType"].ToString();
            return string.Empty;
        }
        set
        {
            HttpContext.Current.Session["SecondDateFilterType"] = value;
        }
    }

    public static HierarchyFilterValue CurrentAlertHistoryReportFilter
    {
        get
        {
            return (HierarchyFilterValue)HttpContext.Current.Session["CurrentAlertHistoryReportFilter"];
        }
        set
        {
            HttpContext.Current.Session["CurrentAlertHistoryReportFilter"] = value;
        }
    }

    public static string DefaultLangdingPage
    {
        get
        {
            if (HttpContext.Current.Session["DefaultLangdingPage"] != null)
                return HttpContext.Current.Session["DefaultLangdingPage"].ToString();
            else
                return null;
        }
        set
        {
            HttpContext.Current.Session["DefaultLangdingPage"] = value;
        }
    }

    public static DataTable DocumentType
    {
        get
        {
            if (!HttpContext.Current.Session["DocumentType"].IsNullOrEmpty())
                return HttpContext.Current.Session["DocumentType"] as DataTable;
            return null;
        }
        set
        {
            HttpContext.Current.Session["DocumentType"] = value;
        }
    }

    public static bool IsSiteJump
    {
        get
        {
            if (!HttpContext.Current.Session["IsSiteJump"].IsNullOrEmpty())
                return HttpContext.Current.Session["IsSiteJump"].ToBoolean();
            return false;
        }
        set
        {
            HttpContext.Current.Session["IsSiteJump"] = value;
        }
    }
    public static string CurrentUrl
    {
        get
        {
            if (!HttpContext.Current.Session["VW_CurrentUrl"].IsNullOrEmpty())
                return HttpContext.Current.Session["VW_CurrentUrl"].ToString();
            return string.Empty;
        }
        set
        {
            HttpContext.Current.Session["VW_CurrentUrl"] = value;
        }
    }

    public static bool IsOpenRecurringSystemMessage
    {
        get
        {
            if (HttpContext.Current.Session["IsOpenRecurringSystemMessage"] != null)
            {
                return (bool)HttpContext.Current.Session["IsOpenRecurringSystemMessage"];
            }
            else
            {
                return true;
            }

        }
        set
        {
            HttpContext.Current.Session["IsOpenRecurringSystemMessage"] = value;
        }

    }

    public static string BrandFraudReportFilterPaymentEntitieType
    {
        get
        {
            if (!HttpContext.Current.Session["BrandFraudReportFilterPaymentEntitieType"].IsNullOrEmpty())
                return HttpContext.Current.Session["BrandFraudReportFilterPaymentEntitieType"].ToString();
            return string.Empty;
        }
        set
        {
            HttpContext.Current.Session["BrandFraudReportFilterPaymentEntitieType"] = value;
        }
    }
}

[Serializable]
public class IncomeFilterOptions
{
    public int DateFilterMode { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string NetProfit { get; set; }
    public double? FromNetProfit { get; set; }
    public double? ToNetProfit { get; set; }
    public int ProfileID { get; set; }
    public int FilteringMode { get; set; }
}
[Serializable]
public class UserAccessFilterOptions
{
    public int DateFilterMode { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string SearchType { get; set; }
    public string SearchValue { get; set; }
    public string FilterValue { get; set; }
    public int HierarchyID { get; set; }
    public int FilterMode { get; set; }
}

[Serializable]
public class ManageUserFilterOptions
{
    public string SearchType { get; set; }
    // use in case Filtering by other searchTypes
    public string SearchValue { get; set; }
    // use in case Filtering by UserRole
    public int RoleId { get; set; }
    // use in case Filtering by UserType
    public int FilterMode { get; set; }
}