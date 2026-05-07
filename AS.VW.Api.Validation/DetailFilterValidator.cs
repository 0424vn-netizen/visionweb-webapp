using AS.VW.Api.Model.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace AS.VW.Api.Validation
{
    public class DetailFilterValidator<T> : CompositeValidator<T> where T : DetailFilter
    {
        public const string REPORTDATE_RULES = "ReportDateRules";
        public const string DEFAULT_RULES = MerchantNumberFilterValidator.ALL_RULES + "," + REPORTDATE_RULES;

        public DetailFilterValidator()
        {
            RegisterValidator(new MerchantNumberFilterValidator());
            RuleSet(REPORTDATE_RULES, () => {
                RuleFor(x => x.ReportDateValidate).DateRules();
            });
        }
    }

    public class DetailFilterValidator : DetailFilterValidator<DetailFilter>
    { 
        
    }

    public class DetailFilterPagingValidator : DetailFilterValidator<DetailPagingFilter>
    {

    }
}
