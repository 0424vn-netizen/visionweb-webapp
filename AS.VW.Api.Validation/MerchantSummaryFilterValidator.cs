using AS.VW.Api.Model.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace AS.VW.Api.Validation
{
    public class MerchantSummaryFilterValidator<T> : CompositeValidator<T> where T : MerchantSummaryFilter
    {
        public const string ALL = MerchantNumberFilterValidator.ALL_RULES + "," + DateFilterValidator.DATE_RULES;

        public MerchantSummaryFilterValidator()
        {
            RegisterValidator(new MerchantNumberFilterValidator());
            RegisterValidator(new DateFilterValidator());
        }
    }

    public class MerchantSummaryFilterValidator : MerchantSummaryFilterValidator<MerchantSummaryFilter>
    { 
        
    }
}
