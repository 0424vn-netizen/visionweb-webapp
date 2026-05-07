using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections.Generic;
using System.Linq;

using AS.Common;
using AS.Common.Logger;
using AS.Common.DBManager;
using AS.Security.WS.Entities;

/// <summary>
/// Summary description for FilterParameterExtension
/// </summary>
public static class FilterParameterExtension
{


    public static FilterParameterCollection AddLoggedInUserReportingParams(this FilterParameterCollection source)
    {
        return AddLoggedInUserReportingParams(source, false);
    }
    public static FilterParameterCollection AddLoggedInUserReportingParams(this FilterParameterCollection source, bool withoutSiteID)
    {
        if (!withoutSiteID)
            return AddLoggedInUserParams(source, SessionManager.CurrentUser.SiteID);
        else
            return AddLoggedInUserParams(source, -1);

    }
    public static FilterParameterCollection AddLoggedInUserRiskParams(this FilterParameterCollection source)
    {
        return AddLoggedInUserParams(source, SessionManager.CurrentRiskSiteID);
    }
    public static FilterParameterCollection AddLoggedInUserPrimaryUserID(this FilterParameterCollection source)
    {
        source.Add(new AS.Common.DBManager.FilterParameter("@PrimaryUserID", SessionManager.CurrentUser.EntityID, DbType.AnsiString));
        return source;
    }

    public static FilterParameterCollection AddCaseLoggedInUser(this FilterParameterCollection source)
    {
        source.AddCaseLoggedInUserNoSite();
        source.Add(new FilterParameter("@SiteID", SessionManager.CurrentUser.SiteID, DbType.Int32));
        return source;
    }

    public static FilterParameterCollection AddCaseLoggedInUserNoSite(this FilterParameterCollection source)
    {
        source.Add(new AS.Common.DBManager.FilterParameter("@UserID", SessionManager.CurrentUser.RecId, DbType.Guid));
        source.Add(new AS.Common.DBManager.FilterParameter("@ASClientID", SessionManager.CurrentClient, DbType.Int32));
        if (!string.IsNullOrEmpty(SessionManager.CurrentUser.EntityID))
        {
            source.Add(new FilterParameter("@EntityTypeID", SessionManager.CurrentUser.EntityType, DbType.Int32));
            source.Add(new FilterParameter("@EntityNumber", SessionManager.CurrentUser.EntityID, DbType.String));
        }
        return source;
    }

    public static FilterParameterCollection AddMgmtLoggedInUser(this FilterParameterCollection source)
    {
        source.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        if (!string.IsNullOrEmpty(SessionManager.CurrentUser.EntityID))
        {
            source.Add(new FilterParameter("@EntityTypeID", SessionManager.CurrentUser.EntityType, DbType.Int32));
            source.Add(new FilterParameter("@EntityNumber", SessionManager.CurrentUser.EntityID, DbType.String));
        }
        return source;
    }

    public static FilterParameterCollection AddLanguageID(this FilterParameterCollection source)
    {
        if (GeneralFuncsLib.HasMultiLanguageFeature)
        {
            return AddLanguageID(source, SessionManager.CurrentLanguage);
        }
        return source;
    }

    public static FilterParameterCollection AddLanguageID(this FilterParameterCollection source, int currentLanguage)
    {
        source.Add(new AS.Common.DBManager.FilterParameter("@LanguageID", currentLanguage, DbType.Int32));
        return source;
    }


    public static FilterParameterCollection AddLoggedInUserParams(this FilterParameterCollection source, int siteID)
    {
        return AddLoggedInUserParams(source, SessionManager.CurrentUser, siteID);
    }

    public static FilterParameterCollection AddLoggedInUserParams(this FilterParameterCollection source, User currentUser, int siteID)
    {
        string userMode = GeneralFuncsLib.GetUserMode();
        return AddLoggedInUserParams(source, currentUser, siteID, userMode);
    }

    public static FilterParameterCollection AddLoggedInUserParams(this FilterParameterCollection source, User currentUser, string userMode)
    {
        return AddLoggedInUserParams(source, currentUser, currentUser.SiteID, userMode);
    }

    public static FilterParameterCollection AddLoggedInUserParams(this FilterParameterCollection source, User currentUser, int siteID, string userMode)
    {
        source.Add(new AS.Common.DBManager.FilterParameter("@UserMode", userMode, DbType.AnsiString));
        source.Add(new AS.Common.DBManager.FilterParameter("@UserID", currentUser.UserID, DbType.AnsiString));
        source.Add(new AS.Common.DBManager.FilterParameter("@ASClient", currentUser.ASClient, DbType.Int32));
        if (siteID >= 0)
            source.Add(new AS.Common.DBManager.FilterParameter("@SiteID", siteID, DbType.Int32));

        return source;
    }

    public static FilterParameterCollection AddLoggedInUserParamsWithRecId(this FilterParameterCollection source)
    {

        string userMode = "";
        switch (SessionManager.CurrentUserType)
        {
            case WebSiteEnums.UserHierarchyMode.CS:
            case WebSiteEnums.UserHierarchyMode.AS:
                userMode = "CSUSER";
                break;
            case WebSiteEnums.UserHierarchyMode.Site:
                userMode = "CLIENT";
                break;
            case WebSiteEnums.UserHierarchyMode.Hierarchy:
            case WebSiteEnums.UserHierarchyMode.Merchant:
            case WebSiteEnums.UserHierarchyMode.Headquarter:
                userMode = GeneralFuncsLib.GetHierarchyInfo(SessionManager.CurrentUser.EntityType).UserMode;
                break;
        }
        source.Add(new AS.Common.DBManager.FilterParameter("@UserMode", userMode, DbType.AnsiString));
        source.Add(new AS.Common.DBManager.FilterParameter("@UserID", SessionManager.CurrentUser.UserID, DbType.AnsiString));
        source.Add(new AS.Common.DBManager.FilterParameter("@ASClient", SessionManager.CurrentUser.ASClient, DbType.Int32));
        source.Add(new AS.Common.DBManager.FilterParameter("@SiteID", SessionManager.CurrentUser.SiteID, DbType.Int32));
        source.Add(new AS.Common.DBManager.FilterParameter("@UserRecId", SessionManager.CurrentUser.RecId, DbType.Guid));

        return source;
    }

    public static FilterParameterCollection AddHierarchyFilterParamsForORION(this FilterParameterCollection source, ReportPage page)
    {
        string hierarchyValue = "";
        if (!page.ReportFilter.CurrentValue.Value.IsNullOrEmpty())
        {
            if (!GeneralFuncsLib.GetHierarchyInfo(page.ReportFilter.CurrentValue.HierarchyMode).ShowPrefix)
            {
                hierarchyValue = GeneralFuncsLib.GetHierarchyInfo(page.ReportFilter.CurrentValue.HierarchyMode).Prefix + page.ReportFilter.CurrentValue.Value;
            }
            else
            {
                if (page.ReportFilter.CurrentValue.Value.ToLower() != GeneralFuncsLib.GetHierarchyInfo(page.ReportFilter.CurrentValue.HierarchyMode).Prefix.ToLower())
                {
                    hierarchyValue = page.ReportFilter.CurrentValue.Value;
                }
                else
                {
                    hierarchyValue = string.Empty;
                }
            }
            
            if (page.ReportFilter.CurrentValue.HierarchyMode.Equals(HierarchyMode.MERCHANT_NAME, StringComparison.OrdinalIgnoreCase)
                || page.ReportFilter.CurrentValue.HierarchyMode.Equals(HierarchyMode.USERID, StringComparison.OrdinalIgnoreCase))
                hierarchyValue = GeneralFuncsLib.ReplaceSpecialCharacter(hierarchyValue);
            else
                hierarchyValue = hierarchyValue.Replace("*", "%");

        }

        if (SessionManager.FromHierarchyDrilldown != null && SessionManager.FromHierarchyDrilldown.HierarchyMode == "POR" && page.ReportFilter.CurrentValue.HierarchyMode == "GROUP")
        {
            source.Insert(0, new AS.Common.DBManager.FilterParameter("@HierarchyFilterMode", SessionManager.FromHierarchyDrilldown.HierarchyMode + (char)241 + page.ReportFilter.CurrentValue.HierarchyMode, DbType.AnsiString));
            source.Add(new AS.Common.DBManager.FilterParameter("@HierarchyFilterValue", SessionManager.FromHierarchyDrilldown.Value + (char)241 + hierarchyValue, DbType.AnsiString));
        }
        else
        {
            source.Insert(0, new AS.Common.DBManager.FilterParameter("@HierarchyFilterMode", page.ReportFilter.CurrentValue.HierarchyMode, DbType.AnsiString));
            source.Add(new AS.Common.DBManager.FilterParameter("@HierarchyFilterValue", hierarchyValue, DbType.AnsiString));
        }

        return source;
    }

    public static FilterParameterCollection AddHierarchyFilterParamsWithoutDate(this FilterParameterCollection source, ReportPage page)
    {
        source.Insert(0, new AS.Common.DBManager.FilterParameter("@HierarchyFilterMode", page.ReportFilter.CurrentValue.HierarchyMode, DbType.AnsiString));
        string hierarchyValue = "";
        if (!page.ReportFilter.CurrentValue.Value.IsNullOrEmpty())
        {
            if (!GeneralFuncsLib.GetHierarchyInfo(page.ReportFilter.CurrentValue.HierarchyMode).ShowPrefix)
            {
                hierarchyValue = GeneralFuncsLib.GetHierarchyInfo(page.ReportFilter.CurrentValue.HierarchyMode).Prefix + page.ReportFilter.CurrentValue.Value;
            }
            else
            {
                if (page.ReportFilter.CurrentValue.Value.ToLower() != GeneralFuncsLib.GetHierarchyInfo(page.ReportFilter.CurrentValue.HierarchyMode).Prefix.ToLower())
                {
                    hierarchyValue = page.ReportFilter.CurrentValue.Value;
                }
                else
                {
                    hierarchyValue = string.Empty;
                }

            }
            
            if ((page.ReportFilter.CurrentValue.HierarchyMode == HierarchyMode.MERCHANT_NAME) ||
                (page.ReportFilter.CurrentValue.HierarchyMode == HierarchyMode.CAYAN_CORPNAME) ||
                page.ReportFilter.CurrentValue.HierarchyMode.Equals(HierarchyMode.USERID, StringComparison.OrdinalIgnoreCase))
            {
                hierarchyValue = GeneralFuncsLib.ReplaceSpecialCharacter(hierarchyValue);
            }
            else
            {
                hierarchyValue = hierarchyValue.Replace("*", "%");
            }

            switch (page.ReportFilter.CurrentValue.HierarchyMode)
            {
                case HierarchyMode.TAXID:
                    source.AddEncryptedInputParams("HierarchyFilterValue");
                    break;
                default:
                    break;
            }

        }
        source.Add(new AS.Common.DBManager.FilterParameter("@HierarchyFilterValue", hierarchyValue, DbType.AnsiString));
        return source;
    }
    public static FilterParameterCollection AddHierarchyFilterParams(this FilterParameterCollection source, ReportPage page)
    {
        if (SessionManager.CurrentUser.ASClient == 29) // Orion
        {
            source = AddHierarchyFilterParamsForORION(source, page);
        }
        else
        {
            source = AddHierarchyFilterParamsWithoutDate(source, page);
        }
        source.Add(new AS.Common.DBManager.FilterParameter("@DateFilterMode", (int)page.ReportFilter.CurrentValue.DateOption, DbType.Int32));
        source.Add(new AS.Common.DBManager.FilterParameter("@BeginDate", page.ReportFilter.CurrentValue.DateOptionValue.From, DbType.DateTime));
        source.Add(new AS.Common.DBManager.FilterParameter("@EndDate", (page.ReportFilter.CurrentValue.DateOption == AS.Web.UI.Controls.DateOptionMode.Daily || page.ReportFilter.CurrentValue.DateOption == AS.Web.UI.Controls.DateOptionMode.Monthly) ? page.ReportFilter.CurrentValue.DateOptionValue.From : page.ReportFilter.CurrentValue.DateOptionValue.To, DbType.DateTime));
        return source;
    }
    public static FilterParameterCollection AddDateFilterParams(this FilterParameterCollection source, ReportPage page)
    {
        source.Add(new AS.Common.DBManager.FilterParameter("@DateFilterMode", (int)page.ReportFilter.CurrentValue.DateOption, DbType.Int32));
        source.Add(new AS.Common.DBManager.FilterParameter("@BeginDate", page.ReportFilter.CurrentValue.DateOptionValue.From, DbType.DateTime));
        source.Add(new AS.Common.DBManager.FilterParameter("@EndDate", (page.ReportFilter.CurrentValue.DateOption == AS.Web.UI.Controls.DateOptionMode.Daily || page.ReportFilter.CurrentValue.DateOption == AS.Web.UI.Controls.DateOptionMode.Monthly) ? page.ReportFilter.CurrentValue.DateOptionValue.From : page.ReportFilter.CurrentValue.DateOptionValue.To, DbType.DateTime));
        return source;
    }
    public static FilterParameterCollection AddDecryptDataParams(this FilterParameterCollection source, string encryptedColumn, bool isExporting)
    {

        source.Add(new FilterParameter("@EncryptedColumn", encryptedColumn, System.Data.DbType.String));
        source.Add(new FilterParameter("@IsExporting", isExporting, System.Data.DbType.Boolean));

        return source;
    }

    public static FilterParameterCollection AddEncryptedInputParams(this FilterParameterCollection source, string encryptedParams)
    {
        source.Add(new FilterParameter("@EncryptedParams", encryptedParams, System.Data.DbType.String));
        return source;
    }

    public static FilterParameterCollection AddDecryptDataParams(this FilterParameterCollection source, string encryptedColumn)
    {
        return AddDecryptDataParams(source, encryptedColumn, false);
    }
     

    public static FilterParameterCollection AddUserSessionID(this FilterParameterCollection source)
    {
        string sessionID = HttpContext.Current.Session.SessionID;
        source.Insert(0, new AS.Common.DBManager.FilterParameter("@SessionID", sessionID, DbType.String));
        return source;
    }

    public static FilterParameterCollection AddLoggedInUserParamsForCM(this FilterParameterCollection source, int siteID)
    {
        source.Add(new AS.Common.DBManager.FilterParameter("@UserID", SessionManager.CurrentUser.RecId, DbType.Guid));
        source.Add(new AS.Common.DBManager.FilterParameter("@ASClientID", SessionManager.CurrentUser.ASClient, DbType.Int32));
        if (!string.IsNullOrEmpty(SessionManager.CurrentUser.EntityID))
        {
            source.Add(new AS.Common.DBManager.FilterParameter("@EntityTypeID", SessionManager.CurrentUser.EntityType, DbType.Int32));
            source.Add(new AS.Common.DBManager.FilterParameter("@EntityNumber", SessionManager.CurrentUser.EntityID, DbType.String));
        }
        if (siteID > 0)
        {
            source.Add(new AS.Common.DBManager.FilterParameter("@SiteID", siteID, DbType.Int32));
        }
        return source;
    }

    public static FilterParameterCollection AddEntityTypeParam(this FilterParameterCollection source)
    {
        if (!string.IsNullOrEmpty(SessionManager.CurrentUser.EntityID))
        {
            source.Add(new AS.Common.DBManager.FilterParameter("@EntityTypeID", SessionManager.CurrentUser.EntityType, DbType.Int32));
            source.Add(new AS.Common.DBManager.FilterParameter("@EntityNumber", SessionManager.CurrentUser.EntityID, DbType.String));
        }
        return source;
    }
}
