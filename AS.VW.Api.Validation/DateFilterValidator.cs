using AS.VW.Api.Model.Filter;
using FluentValidation;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Api.Validation
{
    public class DateFilterValidator: CompositeValidator<IDateFilter>
    {
        public const string DATE_RULES = "DateRules";
        public DateFilterValidator()
        {
            RuleSet(DATE_RULES, () =>
            {
                RuleFor(x => x.FromDateValidate).DateRules();
                RuleFor(x => x.ToDateValidate).DateRules();
             
                RuleFor(x => x.FromDateValidate).LessThanOrEqualTo(filter => filter.ToDateValidate)
                    .WithErrorCodeAndMessage(ResourceApi.Validator_EC_LessThanOrEqualTo, string.Format(ResourceApi.GReportFilter_FromValueGreaterToValue, "'FromDate'", "'ToDate'")); 
                // max 90 days
                RuleFor(x => x.ToDateValidate).LessThanOrEqualTo(filter => filter.FromDateValidate.AddDays(720))
                    .WithErrorCodeAndMessage(ResourceApi.Validator_EC_LessThanOrEqualTo, ResourceApi.Validator_ninetydays); 
            });
        }
    }
}
