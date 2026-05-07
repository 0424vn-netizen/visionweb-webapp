using AS.Common.DBManager;
using AS.WS.Mobile.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AS.WS.MobileBusiness
{
    public static class Extensions
    {
        #region FilterParameter Extensions

        public static FilterParameterCollection AddASClientParam(this FilterParameterCollection parameters, int clientId)
        {
            parameters.Add(new FilterParameter()
            {
                ParameterName = "@ASClient",
                ParameterType = DbType.Int32,
                ParameterValue = clientId
            });

            return parameters;
        }

        public static FilterParameterCollection AddMerchantNumberParam(this FilterParameterCollection parameters, string merchantNumber)
        {
            parameters.Add(new FilterParameter()
            {
                ParameterName = "@MerchantNumber",
                ParameterType = DbType.String,
                ParameterValue = merchantNumber
            });

            return parameters;
        }

        public static FilterParameterCollection AddReportDateParam(this FilterParameterCollection parameters, DateTime reportDate)
        {
            parameters.Add(new FilterParameter()
            {
                ParameterName = "@ReportDate",
                ParameterType = DbType.DateTime,
                ParameterValue = reportDate
            });

            return parameters;
        }

        public static FilterParameterCollection AddIsPagingParameter(this FilterParameterCollection parameters, bool isPaging)
        {
            parameters.Add(new FilterParameter()
            {
                ParameterName = "@IsPaging",
                ParameterType = DbType.Boolean,
                ParameterValue = isPaging
            });

            return parameters;
        }

        public static FilterParameterCollection AddPageIndexParameters(this FilterParameterCollection parameters, int pageIndex)
        {
            parameters.Add(new FilterParameter()
            {
                ParameterName = "@PageNo",
                ParameterType = DbType.Int32,
                ParameterValue = pageIndex
            });

            return parameters;
        }

        public static FilterParameterCollection AddPageSizeParameters(this FilterParameterCollection parameters, int pageSize)
        {
            parameters.Add(new FilterParameter()
            {
                ParameterName = "@PageSize",
                ParameterType = DbType.Int32,
                ParameterValue = pageSize
            });

            return parameters;
        }

        public static FilterParameterCollection AddOrderParameter(this FilterParameterCollection parameters, string sortExp)
        {
            parameters.Add(new FilterParameter()
            {
                ParameterName = "@stOrder",
                ParameterType = DbType.AnsiString,
                ParameterValue = sortExp
            });

            return parameters;
        }

        public static FilterParameterCollection AddWhenNotNullOrEmpty(this FilterParameterCollection parameters,
                                                                       string parameterName,
                                                                       string parameterValue,
                                                                       DbType parameterType = DbType.AnsiString)
        {
            if (!string.IsNullOrEmpty(parameterValue))
            {
                parameters.Add(new FilterParameter()
                {
                    ParameterName = parameterName,
                    ParameterType = parameterType,
                    ParameterValue = parameterValue
                });
            }
            return parameters;
        }

        public static FilterParameterCollection AddDDSClientIDParam(this FilterParameterCollection parameters, int clientId)
        {
            parameters.Add(new FilterParameter()
            {
                ParameterName = "@ASClientID",
                ParameterType = DbType.Int32,
                ParameterValue = clientId
            });

            return parameters;
        }

        public static FilterParameterCollection AddDDSClientParam(this FilterParameterCollection parameters, int clientId)
        {
            var paramName = "@ASClient";
            parameters.Add(new FilterParameter()
            {
                ParameterName = paramName,
                ParameterType = DbType.Int32,
                ParameterValue = clientId
            });

            return parameters;
        }

        public static FilterParameterCollection AddUserIdParam(this FilterParameterCollection parameters, string userId)
        {
            parameters.Add(new FilterParameter()
            {
                ParameterName = "@UserID",
                ParameterType = DbType.String,
                ParameterValue = userId
            });

            return parameters;
        }

        public static FilterParameterCollection AddUserModeParam(this FilterParameterCollection parameters, string userMode)
        {
            parameters.Add(new FilterParameter()
            {
                ParameterName = "@UserMode",
                ParameterType = DbType.String,
                ParameterValue = userMode
            });

            return parameters;
        }

        public static FilterParameterCollection AddSiteIDParam(this FilterParameterCollection parameters, int siteId)
        {
            parameters.Add(new FilterParameter()
            {
                ParameterName = "@SiteID",
                ParameterType = DbType.Int32,
                ParameterValue = siteId
            });

            return parameters;
        }

        public static FilterParameterCollection AddCommonParams(this FilterParameterCollection parameters,
                                                                        int asClientId,
                                                                        string userId, string userMode)
        {
            parameters.AddDDSClientParam(asClientId);
            parameters.AddUserIdParam(userId);
            parameters.AddUserModeParam(userMode);
            return parameters;
        }

        public static FilterParameterCollection AddCommonParams(this FilterParameterCollection parameters,
                                                                        MobileParameters mbPrs)
        {
            parameters.AddDDSClientIDParam(mbPrs.ASClientID)
                      .AddUserIdParam(mbPrs.UserID)
                      .AddUserModeParam(mbPrs.UserMode)
                      .AddSiteIDParam(mbPrs.SiteID);

            return parameters;
        }

        public static FilterParameterCollection AddCommonParams_New(this FilterParameterCollection parameters,
                                                                        MobileParameters mbPrs)
        {
            parameters.AddDDSClientParam(mbPrs.ASClientID)
                      .AddUserIdParam(mbPrs.UserID)
                      .AddUserModeParam(mbPrs.UserMode)
                      .AddSiteIDParam(mbPrs.SiteID);

            return parameters;
        }

        public static FilterParameterCollection AddCommonParamsNoneSiteID(this FilterParameterCollection parameters,
                                                                        MobileParameters mbPrs)
        {
            parameters.AddDDSClientParam(mbPrs.ASClientID)
                      .AddUserIdParam(mbPrs.UserID)
                      .AddUserModeParam(mbPrs.UserMode);

            return parameters;
        }

        public static FilterParameterCollection AddHierarchyFilterModeParam(this FilterParameterCollection parameters, string hierarchyFilterMode)
        {
            parameters.Add(new FilterParameter()
            {
                ParameterName = "@HierarchyFilterMode",
                ParameterType = DbType.String,
                ParameterValue = hierarchyFilterMode
            });

            return parameters;
        }

        public static FilterParameterCollection AddHierarchyFilterValueParam(this FilterParameterCollection parameters, string hierarchyFilterValue)
        {
            parameters.Add(new FilterParameter()
            {
                ParameterName = "@HierarchyFilterValue",
                ParameterType = DbType.String,
                ParameterValue = hierarchyFilterValue
            });

            return parameters;
        }

        public static FilterParameterCollection AddHierarchyFilterValueParam(this FilterParameterCollection parameters, string hierarchyFilterMode
                                                                                    , string hierarchyFilterValue)
        {
            parameters.AddHierarchyFilterModeParam(hierarchyFilterMode);
            parameters.AddHierarchyFilterValueParam(hierarchyFilterValue);
            return parameters;
        }

        public static FilterParameterCollection AddDashboardModeParam(this FilterParameterCollection parameters, string mode, string modeValue)
        {
            parameters.Add(new FilterParameter()
            {
                ParameterName = mode,
                ParameterType = DbType.String,
                ParameterValue = modeValue
            });

            return parameters;
        }

        public static FilterParameterCollection AddPageNoParam(this FilterParameterCollection parameters, int pageNo)
        {
            parameters.Add(new FilterParameter()
            {
                ParameterName = "@PageNo",
                ParameterType = DbType.Int32,
                ParameterValue = pageNo
            });

            return parameters;
        }

        public static FilterParameterCollection AddPageSizeParam(this FilterParameterCollection parameters, int pageSize)
        {
            var paramName = "@PageSize";
            parameters.Add(new FilterParameter()
            {
                ParameterName = paramName,
                ParameterType = DbType.Int32,
                ParameterValue = pageSize
            });

            return parameters;
        }

        public static FilterParameterCollection AddIsPagingParam(this FilterParameterCollection parameters, bool IsPaging)
        {
            parameters.Add(new FilterParameter()
            {
                ParameterName = "@IsPaging",
                ParameterType = DbType.Boolean,
                ParameterValue = IsPaging
            });

            return parameters;
        }

        public static FilterParameterCollection AddPagingParam(this FilterParameterCollection parameters, int pageNo
                                                                                    , int pageSize, bool IsPaging)
        {
            parameters.AddPageNoParam(pageNo);
            parameters.AddPageSizeParam(pageSize);
            parameters.AddIsPagingParam(IsPaging);
            return parameters;
        }

        #endregion
    }
}
