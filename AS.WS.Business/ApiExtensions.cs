using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using AS.Common.DBManager;
using AS.VW.Api.Model;
using AS.VW.Api.Model.Dto;
using AS.VW.Api.Model.Filter;
using AS.VW.Api.Model.Security;
using AS.VW.Common;

namespace AS.WS.Business
{
    public static class ApiExtensions
    {

        #region FilterParameterExtensions

        public static FilterParameterCollection AddASClientParam(this FilterParameterCollection parameters, int clientId)
        {
            parameters.Add(new FilterParameter()
            {
                ParameterName = "@ASClientID",
                ParameterType = DbType.Int32,
                ParameterValue = clientId
            });

            return parameters;
        }

        public static FilterParameterCollection AddSiteIdParam(this FilterParameterCollection parameters, int siteId)
        {
            parameters.Add(new FilterParameter()
            {
                ParameterName = "@SiteId",
                ParameterType = DbType.Int32,
                ParameterValue = siteId
            });

            return parameters;
        }

        public static FilterParameterCollection AddUserIdParam(this FilterParameterCollection parameters, string userId)
        {
            parameters.Add(new FilterParameter()
            {
                ParameterName = "@UserId",
                ParameterType = DbType.String,
                ParameterValue = userId
            });

            return parameters;
        }

        public static FilterParameterCollection AddUserIDModeParam(this FilterParameterCollection parameters, string userIdMode = "CLIENT")
        {
            parameters.Add(new FilterParameter()
            {
                ParameterName = "@UserMode",
                ParameterType = DbType.String,
                ParameterValue = userIdMode
            });

            return parameters;
        }

        public static FilterParameterCollection AddDateFilterModeParam(this FilterParameterCollection parameters, DateFilterType dateFilter)
        {
            parameters.Add(new FilterParameter()
            {
                ParameterName = "@DateFilterMode",
                ParameterType = DbType.Int32,
                ParameterValue = dateFilter
            });

            return parameters;
        }

        public static FilterParameterCollection AddFromDateParam(this FilterParameterCollection parameters, DateTime fromDate)
        {
            parameters.Add(new FilterParameter()
            {
                ParameterName = "@BeginDate",
                ParameterType = DbType.DateTime,
                ParameterValue = fromDate
            });

            return parameters;
        }

        public static FilterParameterCollection AddToDateParam(this FilterParameterCollection parameters, DateTime toDate)
        {
            parameters.Add(new FilterParameter()
            {
                ParameterName = "@EndDate",
                ParameterType = DbType.DateTime,
                ParameterValue = toDate
            });

            return parameters;
        }

        public static FilterParameterCollection AddHierarchyFilterModeParam(this FilterParameterCollection parameters, string filterMode)
        {
            parameters.Add(new FilterParameter()
            {
                ParameterName = "@HierarchyFilterMode",
                ParameterType = DbType.String,
                ParameterValue = filterMode
            });

            return parameters;
        }

        public static FilterParameterCollection AddHierarchyFilterValueParam(this FilterParameterCollection parameters, string filterValue)
        {
            parameters.Add(new FilterParameter()
            {
                ParameterName = "@HierarchyFilterValue",
                ParameterType = DbType.String,
                ParameterValue = filterValue
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

        public static FilterParameterCollection AddReportDateParamForOldSpa(this FilterParameterCollection parameters, DateTime reportDate)
        {
            parameters.AddFromDateParam(reportDate)
                .AddToDateParam(reportDate)
                .AddDateFilterModeParam(DateFilterType.Daily);

            return parameters;
        }

        public static FilterParameterCollection AddDataViewLevelParameters(this FilterParameterCollection parameters, DataViewLevel dataView)
        {
            parameters.Add(new FilterParameter()
            {
                ParameterName = "@IsMerchantList",
                ParameterType = DbType.Boolean,
                ParameterValue = dataView == DataViewLevel.Merchant
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

        public static FilterParameterCollection AddIsPagingTotalParameter(this FilterParameterCollection parameters, bool isCountTotal)
        {
            parameters.Add(new FilterParameter()
            {
                ParameterName = "@IsCountPageTotal",
                ParameterType = DbType.Boolean,
                ParameterValue = isCountTotal
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

        public static FilterParameterCollection AddOutTotalRecordParameter(this FilterParameterCollection parameters)
        {
            parameters.Add(new FilterParameter()
            {
                ParameterName = "@TotalRecord",
                ParameterType = DbType.Int32,
                IsOutParameter = true
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

        #endregion

        #region ReportRequestParameter Extensions
        public static FilterParameterCollection AddHierarchyFilterParam(this FilterParameterCollection parameters, IHierarchyFilter filter)
        {
            filter.HierarchyFilterValue = filter.HierarchyFilterMode.ToUpper().Equals("MERCHANTNAME")
                ? '%' + filter.HierarchyFilterValue.Replace("[", "[[]").Replace("%", "[%]").Replace(",", "[,]").Replace("_", "[_]") + '%'
                : filter.HierarchyFilterValue;

            parameters.AddHierarchyFilterModeParam(filter.HierarchyFilterMode);
            parameters.AddHierarchyFilterValueParam(filter.HierarchyFilterValue);

            return parameters;
        }

        public static FilterParameterCollection AddDateFilterParam(this FilterParameterCollection parameters, IDateFilter filter)
        {
            parameters.AddFromDateParam(filter.FromDateValidate);
            parameters.AddToDateParam(filter.ToDateValidate);
            parameters.AddDateFilterModeParam(DateFilterType.DateRange);
            return parameters;
        }

        public static FilterParameterCollection AddPagingFilterParam(this FilterParameterCollection parameters, IPagingFilter filter)
        {
            parameters.AddIsPagingParameter(true);
            parameters.AddPageIndexParameters(filter.CurrentPageIndex);
            parameters.AddPageSizeParameters(filter.PageSize);
            return parameters;
        }

        public static FilterParameterCollection AddReportFilterNoViewLevelParameters(this FilterParameterCollection parameters, GenericReportFilterNoViewLevel filter)
        {
            parameters.AddHierarchyFilterParam(filter)
                .AddDateFilterParam(filter)
                .AddPagingFilterParam(filter);

            return parameters;
        }

        public static FilterParameterCollection AddReportFilterParameters(this FilterParameterCollection parameters, GenericReportFilter filter)
        {
            parameters.AddReportFilterNoViewLevelParameters(filter)
                .AddDataViewLevelParameters(filter.ViewLevelValidate);

            return parameters;
        }

        public static FilterParameterCollection AddReportFilterParameters(this FilterParameterCollection parameters, MerchantSummaryFilter filter)
        {
            parameters.AddHierarchyFilterModeParam(HierarchyFilterMode.MerchantNumber.ToString())
                .AddHierarchyFilterValueParam(filter.MerchantNumber)
                .AddDateFilterParam(filter)
                .AddPagingFilterParam(filter);

            return parameters;
        }

        public static FilterParameterCollection AddReportFilterParameters(this FilterParameterCollection parameters, DetailPagingFilter filter)
        {
            parameters.AddReportFilterParametersForDetailFilter(filter)
                .AddPagingFilterParam(filter);
            return parameters;
        }

        public static FilterParameterCollection AddReportFilterParametersForDetailFilter(this FilterParameterCollection parameters, DetailFilter filter)
        {
            parameters.AddReportDateParam(filter.ReportDateValidate)
                .AddMerchantNumberParam(filter.MerchantNumber);
            return parameters;
        }

        public static FilterParameterCollection AddLoggedInUser(this FilterParameterCollection parameters, User user)
        {
            parameters.AddASClientParam(user.AsClientId);
            parameters.AddSiteIdParam(user.SiteId);
            parameters.AddUserIdParam(user.UserId);
            parameters.AddUserIDModeParam();

            return parameters;
        }

        public static FilterParameterCollection RemoveByName(this FilterParameterCollection parameters, string paramName)
        {
            FilterParameter parameter = parameters.FindFilterParameterByName(paramName, false);
            if (parameter != null)
                parameters.Remove(parameter);
            return parameters;
        }

        #endregion

        #region Data Converter     
        public static ReportResponse<T> ToReportResponse<T>(this DataSet dsSource, bool includeTotalRow = false, Dictionary<string, string> mapNames = null)
        {
            ReportResponse<T> ret = new ReportResponse<T>();

            if (dsSource.Tables.Count > 0)
            {
                // first table will be the data records
                int totalRows = 0;
                ret.Result = dsSource.Tables[0].To<T>(ref totalRows, mapNames).ToList<T>();
                //fill paging
                ret.Paging = new Pagination() { NumberOfRecords = totalRows };
                if (totalRows > 0 && includeTotalRow)
                    ret.Total = dsSource.Tables[0].Rows[0].ToSum<T>();
            }

            if (dsSource.Tables.Count > 1 && dsSource.Tables[1].Rows.Count > 0)
            {
                ret.Total = dsSource.Tables[1].Rows[0].To<T>();
            }

            return ret;
        }

        public static ReportResponse<T> AddEntityType<T>(this ReportResponse<T> source, IHierarchyFilter hierarchyFilter, string viewLevel)
        {
            return source;
        }

        #endregion

    }
}
