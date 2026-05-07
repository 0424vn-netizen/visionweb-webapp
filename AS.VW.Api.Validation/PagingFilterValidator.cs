using AS.VW.Api.Model.Filter;
using FluentValidation;

namespace AS.VW.Api.Validation
{
    public class PagingFilterValidator : CompositeValidator<IPagingFilter>
    {
        public const string PAGING_RULES = "PagingRules";
        public PagingFilterValidator()
        {
            RuleSet(PAGING_RULES, () =>
            {
                RuleFor(x => x.CurrentPageIndex).GreaterThanOrEqualTo(0);
                RuleFor(x => x.PageSize).GreaterThan(0);
                
            });
        }
    }
}
