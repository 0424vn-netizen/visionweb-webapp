using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AS.VW.Api.Model.Report;
using AS.VW.Api.Model.Filter;
using AS.VW.Api.Model.Security;
using FluentValidation;
using FluentValidation.Results;
using AS.VW.Api.Model.Dto;

namespace AS.VW.Api.Validation
{
    public static class ValidatorFilterExtensions
    {
        /// <summary>
        /// Validate ReportFilter with selected RuleSets. Throw exception with error(s) if any
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="ruleSets"></param>
        public static ValidationResult ValidateFilter<T,M>(this M filter,params string[] ruleSets)
            where T : CompositeValidator<M>, new()
            where M : BaseFilter                                                                                          
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }

            var validator = new T() { CascadeMode = CascadeMode.Continue };
            var ruleSetNames =string.Join(",",ruleSets);
            var results = validator.Validate(filter,ruleSet : ruleSetNames);

            return results;
        }

        #region SetDefaultValue
        public static void SetDefaultValueForDateFilter(this IDateFilter filter)
        {
            if (filter.FromDateValidate.IsNull() && filter.ToDateValidate.IsNull())
            {
                filter.FromDateValidate = filter.ToDateValidate = DateTime.Today.AddDays(-1);
            }
            else if (!filter.FromDateValidate.IsNull() && filter.ToDateValidate.IsNull())
            {
                filter.ToDateValidate = filter.FromDateValidate;
            }
            else if (filter.FromDateValidate.IsNull() && !filter.ToDateValidate.IsNull())
            {
                filter.FromDateValidate = filter.ToDateValidate;
            }

        }

        public static void SetDefaultValueForPagingFilter(this IPagingFilter filter)
        {
            if (filter.CurrentPageIndex <= 0)
                filter.CurrentPageIndex = 1;
            if (filter.PageSize <= 0)
                filter.PageSize = 100;
        }

        public static void SetDefaultValueForMerchantNumberFilter(this IMerchantNumberFilter filter)
        {
            if (filter.MerchantNumber.IsNullOrEmpty())
            {
                filter.MerchantNumber = string.Empty;
            }
        }

        #endregion

        #region SetDefaultAndValidate
        //Generic ReportFilter
        public static ValidationResult SetDefaultValuesAndValidateForReportFilterNoViewLevel<T, F>(this F filter, params string[] ruleSets)
            where T : CompositeValidator<F>, new()
            where F : GenericReportFilterNoViewLevel
        {
            filter.SetDefaultValueForDateFilter();
            filter.SetDefaultValueForPagingFilter();

            return filter.ValidateFilter<T, F>(ruleSets);
        }

        public static ValidationResult SetDefaultValuesAndValidate<T,F>(this F filter,params string[] ruleSets)
            where T : CompositeValidator<F>,new()
            where F : GenericReportFilter
        {
            return filter.SetDefaultValuesAndValidateForReportFilterNoViewLevel<T, F>(ruleSets);
        }

        public static ValidationResult SetDefaultValuesAndValidate(this GenericReportFilter filter, params string[] ruleSets)
        {
            return filter.SetDefaultValuesAndValidate<ReportFilterValidator, GenericReportFilter>(ruleSets);
        }

        public static ValidationResult SetDefaultValuesAndValidateForMerchantFilter<T,F>(this F filter, params string[] ruleSets)
            where T : CompositeValidator<F>, new()
            where F : MerchantSummaryFilter
        {
            filter.SetDefaultValueForMerchantNumberFilter();
            filter.SetDefaultValueForDateFilter();
            filter.SetDefaultValueForPagingFilter();
            return filter.ValidateFilter<T,F>(ruleSets);
        }

        public static ValidationResult SetDefaultValuesAndValidateForMerchantFilter(this MerchantSummaryFilter filter, params string[] ruleSets)
        {
            return filter.SetDefaultValuesAndValidateForMerchantFilter<MerchantSummaryFilterValidator, MerchantSummaryFilter>(ruleSets);
        }

        public static ValidationResult SetDefaultValuesAndValidateForDetailFilter<T,F>(this F filter, params string[] ruleSets)
            where T : CompositeValidator<F>, new()
            where F : DetailFilter
        {
            filter.SetDefaultValueForMerchantNumberFilter();
            if (filter.ReportDateValidate.IsNull())
            {
                filter.ReportDateValidate = DateTime.Today.AddDays(-1);
            }

            return filter.ValidateFilter<T,F>(ruleSets);
        }

        public static void SetDefaultValuesAndValidateForDetailFilter(this DetailFilter filter, params string[] ruleSets)
        {
            filter.SetDefaultValuesAndValidateForDetailFilter<DetailFilterValidator, DetailFilter>(ruleSets);
        }

        public static ValidationResult SetDefaultValuesAndValidateForDetailFilterPaging<T,F>(this F filter, params string[] ruleSets)
            where T : CompositeValidator<F>, new()
            where F : DetailPagingFilter
        {
            filter.SetDefaultValueForPagingFilter();
            return filter.SetDefaultValuesAndValidateForDetailFilter<T,F>(ruleSets);
        }

        public static void SetDefaultValuesAndValidateForDetailFilterPaging(this DetailPagingFilter filter, params string[] ruleSets)
        {
            filter.SetDefaultValueForPagingFilter();
            filter.SetDefaultValuesAndValidateForDetailFilter<DetailFilterPagingValidator, DetailPagingFilter>(ruleSets);
        }
        public static void SetDefaultValuesAndValidateForMerchantInformationFilter<T,F>(this F filter, params string[] ruleSets)
            where T : CompositeValidator<F>, new()
            where F : MerchantInformationFilter
        {
            filter.SetDefaultValueForMerchantNumberFilter();
            filter.ValidateFilter<T, F>(ruleSets);
        }

        public static void SetDefaultValuesAndValidateForMerchantInformationFilter(this MerchantInformationFilter filter, params string[] ruleSets)
        {
            filter.SetDefaultValuesAndValidateForMerchantInformationFilter<MerchantInformationFilterValidator, MerchantInformationFilter>(ruleSets);
        }

        public static void SetDefaultValuesAndValidateForMerchantInformationPagingFilter(this MerchantInformationPagingFilter filter, params string[] ruleSets)
        {
            filter.SetDefaultValueForPagingFilter();
            filter.SetDefaultValuesAndValidateForMerchantInformationFilter(ruleSets);
        }        
        #endregion

        public static bool IsNull(this DateTime date)
        {
            return date == DateTime.MinValue;
        }  

        public static ErrorModel ProcessError(ValidationResult result)
        {
            var item = new CustomValidationException(result.Errors);
            ErrorModel error = new ErrorModel() { Status = "FAILED" };
            error.Messages = item.Errors.Select(x => "---" + x.ErrorMessage).ToList();
            return error;
        }
    }
}
