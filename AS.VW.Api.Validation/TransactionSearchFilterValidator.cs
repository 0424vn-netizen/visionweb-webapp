using AS.VW.Api.Model.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Resources;

namespace AS.VW.Api.Validation
{
    public sealed class TransactionSearchFilterValidator: ReportFilterValidator<TransactionSearchFilter>
    {
        public const string OTHERPARAMS_FORMATDATA_RULES = "OtherParamsFormatDataRules";
        public const string TRANSACTIONS_AMOUNT_RULES = "TransactionAmountRules";
        public const string BUSINESS_RULES = "BusinessRules";
        public const string ALL_AND_EXTEND_RULES = ReportFilterNoViewLevelValidator.HIERARCHYFILTER_ALLOW_EMPTY + "," + OTHERPARAMS_FORMATDATA_RULES 
            + "," + TRANSACTIONS_AMOUNT_RULES + "," + BUSINESS_RULES;
       
        public TransactionSearchFilterValidator()
        {
           
            RuleSet(OTHERPARAMS_FORMATDATA_RULES, () =>
            {
                RuleFor(x => x.First6CardNumber).First6CardNumber();
                RuleFor(x => x.Last4CardNumber).Last4CardNumber();
                RuleFor(x => x.AuthorizationNumber).AuthorizationNumber();
            });

            RuleSet(TRANSACTIONS_AMOUNT_RULES, () =>
            {
                RuleFor(x => x.TransactionOperatorValidate).NotNull2();
                RuleFor(x => x.TransactionAmountFrom).NotNull2().When((x) => (x.TransactionOperatorValidate == TransactionOperator.Between && x.TransactionAmountTo.HasValue));
                RuleFor(x => x.TransactionAmountTo).NotNull2().When((x) => x.TransactionOperatorValidate == TransactionOperator.Between && x.TransactionAmountFrom.HasValue);
                RuleFor(x => x.TransactionAmountFrom.Value).LessThanOrEqualTo(y => y.TransactionAmountTo.Value)
                    .When((filter) => 
                        filter.TransactionAmountFrom.HasValue 
                        && filter.TransactionAmountTo.HasValue 
                        && filter.TransactionOperatorValidate == TransactionOperator.Between)
                    .WithErrorCodeAndMessage(ResourceApi.Validator_EC_LessThanOrEqualTo,
                    string.Format(ResourceApi.GReportFilter_FromValueGreaterToValue, "'AmountFrom'", "'AmountTo'"));
            });
            RuleSet(BUSINESS_RULES, () =>
            {
                RuleFor(x => x.HierarchyFilterValue).Must((filter, hierarchyValue) => (hierarchyValue.IsNotNullAndNotEmpty() ||
                                                                                      filter.Last4CardNumber.IsNotNullAndNotEmpty() ||
                                                                                      filter.First6CardNumber.IsNotNullAndNotEmpty() ||
                                                                                      filter.AuthorizationNumber.IsNotNullAndNotEmpty() ||
                                                                                      (filter.TransactionOperatorValidate == TransactionOperator.EqualTo && filter.TransactionAmountFrom.HasValue)))
                                                    .When(x => x.ToDateValidate - x.FromDateValidate <= new TimeSpan(720, 0, 0, 0, 0))
                                                    .WithErrorCodeAndMessage(ResourceApi.Validator_EC_Predicate, ResourceApi.TransactionSearch_MSG_ConditionLTOEQ90Days);

                RuleFor(x => x.HierarchyFilterValue).Must((filter, hierarchyValue) => (hierarchyValue.IsNotNullAndNotEmpty() ||
                                                                                      filter.Last4CardNumber.IsNotNullAndNotEmpty() ||
                                                                                      filter.First6CardNumber.IsNotNullAndNotEmpty() ||
                                                                                      filter.AuthorizationNumber.IsNotNullAndNotEmpty()))
                                                    .When(x => x.ToDateValidate - x.FromDateValidate > new TimeSpan(720, 0, 0, 0, 0))
                                                    .WithErrorCodeAndMessage(ResourceApi.Validator_EC_Predicate, ResourceApi.TransactionSearch_MSG_ConditionGT90Days);
            });
        }
    }
}
