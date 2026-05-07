using AS.Common.DBManager;
using AS.Controls.Pages;
using AS.Security.WS.Entities;
using Newtonsoft.Json;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for MerchantProfileHelper
/// </summary>
public class MerchantProfileHelper
{
    #region Constants

    public const string MERCHANT_STATUS_DEACTIVATED = "Deactivated";
    public const string MERCHANT_STATUS_CLOSED = "Closed";
    public const string OPTED_OUT_STATUS = "Opted Out";
    public const string OPTED_OUT_VALUE = "0";
    public const string OPTED_IN_STATUS = "Opted In";
    public const string OPTED_IN_VALUE = "1";
    private const string FORMAT_USERID_TEMPLATE = "<tr class='borderBottom'><td>{0}</td><td>&nbsp;{1}</td><td colspan='4'></td></tr>";

    public static string SPA_UPDATE_RELATIONSHIP_MANAGER = "spa_UpdateRelationshipManager";

    #endregion Constants

    #region Methods

    public static string BuildURLForSiteAccessInMIF(SecurePage page,
        string merchantNumber, string optedIn, string mifEmail)
    {
        return string.Format(
            "return ShowPopupModal('MerchantProfileModal.aspx?{0}', 'auto')",
            page.BuildSecureQueryString(
                string.Format("merchant={0}&m={1}&e={2}",
                              merchantNumber, optedIn, mifEmail)));
    }

    public static bool CheckPermissionAddEditChain(object siteAccess, SecurePage page)
    {
        bool hasPermissionAddEditChain = false;
        bool hideAddEdit = GeneralFuncsLib.HideSessionAddEditChain();

        if (!hideAddEdit && page.IsUserWithPermission("AddEditChain")
            && SiteAccessIsOptInOut(siteAccess))
        {
            hasPermissionAddEditChain = true;
        }
        return hasPermissionAddEditChain;
    }
    public static bool CheckPermissionAddEditAccessChain(object siteAccess, SecurePage page)
    {
        bool hasPermissionAddEditChain = false;
        bool hideAddEdit = GeneralFuncsLib.HideSessionAddEditChain();

        if (!hideAddEdit && page.IsUserWithPermission("AddEditAccessChain")
            && SiteAccessIsOptInOut(siteAccess))
        {
            hasPermissionAddEditChain = true;
        }
        return hasPermissionAddEditChain;
    }

    public static bool SiteAccessIsOptInOut(object siteAccess)
    {
        return (String.Compare(siteAccess.ToString(), "opted in", true) == 0
            || String.Compare(siteAccess.ToString(), "opted out", true) == 0
            || String.Compare(siteAccess.ToString(), "Incluido", true) == 0
        || String.Compare(siteAccess.ToString(), "Excluido", true) == 0);
    }

    public static string SetOptInStatus(object optIn, string merchantStatus)
    {
        string status = string.Empty;
        if (!optIn.ToString().IsNullOrEmpty())
        {
            if (optIn.ToString().Equals(OPTED_IN_VALUE))
            {
                status = OPTED_OUT_STATUS;
            }
            else if (optIn.ToString().Equals(OPTED_OUT_VALUE))
            {
                status = OPTED_IN_STATUS;
            }
            else if (merchantStatus == MERCHANT_STATUS_DEACTIVATED
                || merchantStatus == MERCHANT_STATUS_CLOSED)
            {
                status = merchantStatus;
            }
        }
        return status;
    }

    public static string GetRoutingNumberWithoutDecrypt(string full, string partial)
    {
        return (SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.CS
            || SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.AS)
            ? full : partial;
    }

    public static string GetRoutingNumberByDecrypt(object full, object partial)
    {
        //if (SessionManager.CurrentUserType != WebSiteEnums.UserHierarchyMode.Merchant)
        if (SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.CS || SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.AS)
        {
            return WebServices.CsReportServices.DecryptText((string)full, SessionManager.CurrentUser.ASClient);
        }
        return (partial is DBNull ? string.Empty : (string)partial);
    }

    public static string GetRoutingNumberFromFullCard(string full)
    {
        if (SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.Merchant)
        {
            if (full.Length > 10)
            {
                string part2 = full.Substring(full.Length - 4, 4);
                full = "xxxx" + part2;
            }
        }
        return full;
    }

    public static string BindAddress(object add1, object add2, object add3, object city, object state, object zip)
    {
        add1 = ProcessNullValue(add1);
        add2 = ProcessNullValue(add2);
        add3 = ProcessNullValue(add3);
        city = ProcessNullValue(city);
        state = ProcessNullValue(state);
        zip = ProcessNullValue(zip);
        string address1 = string.IsNullOrEmpty((string)add1) ? string.Empty : (add1 + "<br />");
        string address2 = string.IsNullOrEmpty((string)add2) ? string.Empty : (add2 + "<br />");
        if (!string.IsNullOrEmpty((string)add3))
        {
            return address1 + address2 + add3;
        }
        else
        {
            string cityAddress = string.IsNullOrEmpty((string)city) ? string.Empty : (city + ", ");
            return address1 + address2 + cityAddress + state + " " + zip;
        }
    }

    public static object ProcessNullValue(object obj)
    {
        return obj.GetType() == typeof(DBNull) ? null : obj;
    }

    public static string FormatDate(object date)
    {
        date = MerchantProfileHelper.ProcessNullValue(date);
        return (date == null || date.ToString().IsNullOrEmpty())
            ? string.Empty : Convert.ToDateTime(date).ToString(WebSiteConstants.DATE_FORMAT);
    }

    public static string FormatCurrency(object moneyValue)
    {
        return moneyValue == DBNull.Value ? string.Empty : GeneralFuncsLib.FormatCurrency(moneyValue);
    }

    public static object TranslateStatusText(object obj)
    {
        if (String.Compare(obj.ToString(), "opted in", true) == 0
            || String.Compare(obj.ToString(), "Incluido", true) == 0)
        {
            obj = "opted in";
        }
        else
        {
            obj = "opted out";
        }
        return obj;
    }

    public static string SetStatusForSiteAccess(object obj)
    {
        obj = MerchantProfileHelper.ProcessNullValue(obj);
        string _tempStr = string.Empty;
        if (String.Compare((string)obj, "opted in", true) == 0 || String.Compare((string)obj, "Incluido", true) == 0)
        {
            _tempStr = Resources.Template.MIF_MerchantProfile_OptOut;
        }
        else if (String.Compare((string)obj, "opted out", true) == 0 || String.Compare((string)obj, "Excluido", true) == 0)
        {
            _tempStr = Resources.Template.MIF_MerchantProfile_OptIn;
        }
        return _tempStr;
    }

    public static bool SetVisibleSiteAccessLink(object obj, SecurePage page)
    {
        if (!GeneralFuncsLib.HasMSProductEnvironment())
            return false;

        return GeneralFuncsLib.HasOptInOutPermission(page)
            && MerchantProfileHelper.SiteAccessIsOptInOut(obj);
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

    public static bool CheckDisplayHierachyMS(string hierarchy)
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

    public static string FormatSIC(object sic, object sicDesc)
    {
        sic = ProcessNullValue(sic);
        return sic == null ? sicDesc.ToString()
            : string.Format("{0} - {1}", sic.ToString(), sicDesc.ToString());
    }

    public static string CheckPermisson(object obj, string permissionCode, SecurePage page)
    {
        if (obj == null || obj.ToString().IsNullOrEmpty())
        {
            return string.Empty;
        }

        string tempStr = obj.ToString();
        if (SessionManager.CurrentUserViewMode > 0)
        {
            if (page.IsUserWithPermission(permissionCode)
                || page.IsUserWithPermission("MS" + permissionCode))
            {
                tempStr = WebServices.CsReportServices.DecryptText(tempStr, SessionManager.CurrentUser.ASClient);
            }
        }

        return tempStr;
    }

    public static string BuildSubmitReportFilterJavascriptCall(string hierarchy, string hierarchyValue)
    {
        return string.Format("return rf_SubmitReportFilterValues('{0}', '{1}', '{2}');",
            GeneralFuncsLib.GetHierarchyInfo(hierarchy).HierarchyID,
            hierarchy,
            hierarchyValue);
    }

    public static void BuildMIFDetailHierarchyLink(DataTable dtMerch, RepeaterItem item, string uxLink, string valueLink, string modeLink)
    {
        BuildHierarchyLink(dtMerch, item, uxLink, valueLink, modeLink, string.Empty);
    }

    public static void BuildMIFDetailHierarchyLink(DataTable dtMerch, RepeaterItem item, string uxLink, string valueLink, string modeLink, string uxMDash)
    {
        BuildHierarchyLink(dtMerch, item, uxLink, valueLink, modeLink, uxMDash);
    }

    public static void BuildHierarchyLink(DataTable dtMerch, RepeaterItem item, string uxLink, string valueLink, string modeLink, string uxMDash)
    {
        HtmlAnchor attr = (HtmlAnchor)item.FindControl(uxLink);
        string value = dtMerch.Rows[0][valueLink].ToString();
        if (attr != null && CheckHierarchy(modeLink))
        {
            if (!string.IsNullOrEmpty(value))
            {
                attr.Visible = true;
                attr.Attributes.Add(
                    "onclick",
                    MerchantProfileHelper.BuildSubmitReportFilterJavascriptCall(modeLink, value));
            }
            else
            {
                attr.Visible = false;
                if (!string.IsNullOrEmpty(uxMDash))
                {
                    Literal eDash = (Literal)item.FindControl(uxMDash);
                    eDash.Visible = true;
                }
            }
        }
    }

    public static bool HasSiteAccessPermission(SecurePage page)
    {
        if (page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_JSACCESS))
        {
            return true;
        }
        else {
            return false;
        }
    }

    public static void SiteAccess(SecurePage page, string userID)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();        
        parameters.Add(new FilterParameter("@UserHierarchyCode", SessionManager.CurrentUserRoles[0].HierarchyCode, DbType.String));
        parameters.Add(new FilterParameter("@UserSiteJump", userID, DbType.String));
        parameters.Add(new FilterParameter("@UserEntityType", SessionManager.CurrentUser.EntityType, DbType.Int32));
        parameters.Add(new FilterParameter("@SystemId", SessionManager.CurrentSystem, DbType.Int32));

        DataTable dt = WebServices.SecurityServices.GetReports("spa_SEC_CheckUserSiteJump", parameters);
        if (dt != null && dt.Rows.Count > 0)
        {
            switch (Int32.Parse(dt.Rows[0][0].ToString()))
            {
                case 0: // Success
                    break;
                case 1: // Invalid user                                       
                case 2:  // Entity not belongs to the current user
                    ShowMessage(page, Resources.Template.Message_InvalidUserAccount);
                    return;
                case 3:  // Merchant is opted out
                    ShowMessage(page, Resources.Template.Message_MerchantIsOptedOut);
                    return;
                case 4:  // User is opted out
                    ShowMessage(page, Resources.Template.Message_UserIsOptedOut);
                    return;
            }
        }
        User user = WebServices.SecurityServices.GetUser(SessionManager.CurrentClient, userID);        

        string excludeHierachies = GeneralFuncsLib.GetDataOfExtendedSetting("DisableSiteJumpByEntityTypeIDs");
        if (!string.IsNullOrEmpty(excludeHierachies))
        {
            excludeHierachies = string.Format(",{0},", excludeHierachies);
            if (excludeHierachies.Contains(string.Format(",{0},", user.EntityType)))
            {
                ShowMessage(page, Resources.Template.Message_InvalidUserAccount);
                return;
            }
        }        
        if ((SessionManager.CurrentSystem == WebSiteConstants.AS_SYSTEM_CS && user.SiteID == 0) 
           ||(SessionManager.CurrentClient == WebSiteConstants.TOTAL_CLIENT && GeneralFuncsLib.IsSSOUser(SessionManager.CurrentClient, user.UserID) == true))
        {
            ShowMessage(page, Resources.Template.Message_InvalidUserAccount);
            return;
        }        
        string key = WebServices.SecurityServices.CreateJumpSiteTicket(user.RecId, page.Request.UserHostAddress, 2, SessionManager.CurrentUser.RecId);
        string url = WebSiteSettings.MsGate;
        url = string.Format(url + "?u={0}&k={1}&j={2}&c={3}&f={4}&lan={5}", HttpUtility.UrlEncode(WebServices.SecurityServices.EncryptText(user.UserID)),
            HttpUtility.UrlEncode(key),
            HttpUtility.UrlEncode(WebServices.SecurityServices.EncryptText(SessionManager.CurrentUser.RecId.ToString())),
            HttpUtility.UrlEncode(AS.Common.DataProtection.Cryptophy.EncryptText(user.ASClient.ToString())),
            HttpUtility.UrlEncode(WebServices.SecurityServices.EncryptText("CS")),
            SessionManager.CurrentLanguage
            );

        ((BaseMasterPage)page.Master).AjaxAddResponseScript(string.Format("window.open('{0}');", url));        
    }
    public static string CreateSiteJumpLink(string userID)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();
        parameters.Add(new FilterParameter("@UserHierarchyCode", SessionManager.CurrentUserRoles[0].HierarchyCode, DbType.String));
        parameters.Add(new FilterParameter("@UserSiteJump", userID, DbType.String));
        parameters.Add(new FilterParameter("@UserEntityType", SessionManager.CurrentUser.EntityType, DbType.Int32));
        parameters.Add(new FilterParameter("@SystemId", SessionManager.CurrentSystem, DbType.Int32));

        DataTable dt = WebServices.SecurityServices.GetReports("spa_SEC_CheckUserSiteJump", parameters);
        if (dt != null && dt.Rows.Count > 0)
        {
            switch (Int32.Parse(dt.Rows[0][0].ToString()))
            {
                case 0: // Success
                    break;
                case 1: // Invalid user                                       
                case 2:  // Entity not belongs to the current user
                    return string.Empty;
                case 3:  // Merchant is opted out
                    return string.Empty;
                case 4:  // User is opted out
                    return string.Empty;
            }
        }
        User user = WebServices.SecurityServices.GetUser(SessionManager.CurrentClient, userID);

        string excludeHierachies = GeneralFuncsLib.GetDataOfExtendedSetting("DisableSiteJumpByEntityTypeIDs");
        if (!string.IsNullOrEmpty(excludeHierachies))
        {
            excludeHierachies = string.Format(",{0},", excludeHierachies);
            if (excludeHierachies.Contains(string.Format(",{0},", user.EntityType)))
            {
                return string.Empty;
            }
        }
        if ((SessionManager.CurrentSystem == WebSiteConstants.AS_SYSTEM_CS && user.SiteID == 0)
           || (SessionManager.CurrentClient == WebSiteConstants.TOTAL_CLIENT && GeneralFuncsLib.IsSSOUser(SessionManager.CurrentClient, user.UserID) == true))
        {
            return string.Empty;
        }
        string key = WebServices.SecurityServices.CreateJumpSiteTicket(user.RecId, HttpContext.Current.Request.UserHostAddress, 2, SessionManager.CurrentUser.RecId);
        string url = WebSiteSettings.MsGate;
        url = string.Format(url + "?u={0}&k={1}&j={2}&c={3}&f={4}&lan={5}", HttpUtility.UrlEncode(WebServices.SecurityServices.EncryptText(user.UserID)),
            HttpUtility.UrlEncode(key),
            HttpUtility.UrlEncode(WebServices.SecurityServices.EncryptText(SessionManager.CurrentUser.RecId.ToString())),
            HttpUtility.UrlEncode(AS.Common.DataProtection.Cryptophy.EncryptText(user.ASClient.ToString())),
            HttpUtility.UrlEncode(WebServices.SecurityServices.EncryptText("CS")),
            SessionManager.CurrentLanguage
            );
     
        return url;
    }

    public static void FormatExportUserInfo(StringBuilder strBuilder, DataTable _datasource)
    {
        if (GeneralFuncsLib.IsUserSignOn())
        {
            strBuilder.Replace("class=\"borderBottom\"", string.Empty);
            strBuilder.Replace("[USERID_INFO]", string.Format(FORMAT_USERID_TEMPLATE, Resources.Template.UserID, _datasource.Rows[0]["UserID"]));            
        }
        else
        {
            strBuilder.Replace("[USERID_INFO]", string.Empty);
        }
    }

    private static void ShowMessage(SecurePage page, String message)
    {
        page.ClientScript.RegisterClientScriptBlock(page.GetType(), "message", string.Format("alert('{0}')", message), true);
    }

    public static MerchantProfileConfig GetMerchantProfileConfigs(string processor)
    {
        var asClientID = SessionManager.CurrentUser.ASClient;
        var configs = new MerchantProfileConfig();
        processor = string.IsNullOrEmpty(processor) ? "default" : processor;
        var filePath = string.Format("~/App_Data/MerchantProfileTemplate/{0}/{1}/merchant-config.json", asClientID, processor);
        var physicalFilePath = HttpContext.Current.Server.MapPath(filePath);

        if (File.Exists(physicalFilePath) == false)
        {
            return null;
        }

        var key = physicalFilePath.Replace(@"\", "_").Replace(" ", "_");

        if (HttpRuntime.Cache[key] == null)
        {
            var fileCotent = File.ReadAllText(physicalFilePath);
            configs = JsonConvert.DeserializeObject<MerchantProfileConfig>(fileCotent);
            HttpRuntime.Cache.Insert(key, fileCotent, new System.Web.Caching.CacheDependency(physicalFilePath));
        }
        else
        {
            var content = HttpRuntime.Cache[key].ToString();
            configs = JsonConvert.DeserializeObject<MerchantProfileConfig>(content);            
        }

        configs.Processor = processor;

        return configs;
    }
    public static MerchantProfileClientConfig GetClientConfig()
    {
        var asClientID = SessionManager.CurrentUser.ASClient;
        var configs = new MerchantProfileClientConfig();
        var filePath = string.Format("~/App_Data/MerchantProfileTemplate/{0}/client-config.json", asClientID);
        var physicalFilePath = HttpContext.Current.Server.MapPath(filePath);

        if (File.Exists(physicalFilePath) == false)
        {
            return null;
        }

        var key = physicalFilePath.Replace(@"\", "_").Replace(" ", "_");

        if (HttpRuntime.Cache[key] == null)
        {
            var fileCotent = File.ReadAllText(physicalFilePath);
            configs = JsonConvert.DeserializeObject<MerchantProfileClientConfig>(fileCotent);
            HttpRuntime.Cache.Insert(key, fileCotent, new System.Web.Caching.CacheDependency(physicalFilePath));
        }
        else
        {
            var content = HttpRuntime.Cache[key].ToString();
            configs = JsonConvert.DeserializeObject<MerchantProfileClientConfig>(content);
        }

        return configs;
    }

    public static string GetMerchantProcessor(string merchantNumber)
    {
        var currentUser = SessionManager.CurrentUser;
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.Add(new FilterParameter("@SiteID", currentUser.SiteID, DbType.Int32));
        parameters.Add(new FilterParameter("@ASClient", currentUser.ASClient, DbType.Int32));
        parameters.Add(new FilterParameter("@MerchantNumber", merchantNumber, DbType.String));
        DataTable table = WebServices.CsReportServices.GetReports("spa_cs_GetBEProcessor", parameters);
        
        if (table != null && table.Rows.Count > 0)
        {
            return table.Rows[0][0].ToString();            
        }

        return string.Empty;
    }

    public static string GetFolderPath(string processor)
    {
        var asClientID = SessionManager.CurrentUser.ASClient;
        var filePath = string.Format("~/App_Data/MerchantProfileTemplate/{0}/{1}/", asClientID, processor);
        var physicalFolderPath = HttpContext.Current.Server.MapPath(filePath);
        return physicalFolderPath;
    }
    #endregion Methods
}