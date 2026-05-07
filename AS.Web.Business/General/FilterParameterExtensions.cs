using AS.Common.DBManager;
using AS.Security.WS.Entities;
using System.Data;

namespace AS.Web.Business.General
{
    public static class FilterParameterExtensions
    {
        public static FilterParameterCollection AddParamCurrentInUser(this FilterParameterCollection source, User currentUser, string userMode)
        {
            return AddParamCurrentInUser(source, currentUser, userMode, currentUser.SiteID);
        }
        public static FilterParameterCollection AddParamCurrentInUser(this FilterParameterCollection source, User currentUser, string userMode, int siteID)
        {
            source.Add(new FilterParameter("@UserMode", userMode, DbType.AnsiString));
            source.Add(new FilterParameter("@UserID", currentUser.UserID, DbType.AnsiString));
            source.Add(new FilterParameter("@ASClient", currentUser.ASClient, DbType.Int32));
            if (siteID >= 0)
                source.Add(new FilterParameter("@SiteID", siteID, DbType.Int32));

            return source;
        }

        public static FilterParameterCollection AddParamLanguageId(this FilterParameterCollection source, bool hasMultiLanguageFeature, int currentLanguage)
        {
            if (!hasMultiLanguageFeature)
            {
                return source;
            }
            source.Add(new FilterParameter("@LanguageID", currentLanguage, DbType.Int32));
            return source;
        }

        public static FilterParameterCollection AddParamExport(this FilterParameterCollection source, bool isExporting)
        {
            if (!isExporting)
            {
                return source;
            }
            source.Add(new FilterParameter("@IsExport", isExporting, DbType.Boolean));
            return source;
        }
    }
}
