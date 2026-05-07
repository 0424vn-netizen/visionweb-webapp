using AS.VW.Api.Model.Filter;
using FluentValidation;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AS.VW.Api.Validation
{
    public class HierarchyFilterValidator<T> : CompositeValidator<T> where T: IHierarchyFilter
    {
        public const string FORMAT_RULES = "FormatRules";
        public const string REQUIRE = "Require";
        public const string REQUIRE_ALL= "RequireAll";
        public const string ALL_RULES = REQUIRE + "," + FORMAT_RULES;

        public HierarchyFilterValidator()
        {
            RuleSet(REQUIRE_ALL, () =>
            {
                RuleFor(x => x.HierarchyFilterValue).NotEmpty2();
            });

            RuleSet(FORMAT_RULES, () =>
            {
              //Filter Common               
           
                RuleFor(x => x.HierarchyFilterValue).OnlyAllowNumbers()
                   .When(x => x.HierarchyFilterMode.ToLower() == HierarchyFilterMode.MerchantNumber.ToString().ToLower());

                RuleFor(x => x.HierarchyFilterValue).LengthWhenNotEmpty(0, 16)
                    .When(x => x.HierarchyFilterMode.ToLower() == HierarchyFilterMode.MerchantNumber.ToString().ToLower());

                RuleFor(x => x.HierarchyFilterValue).ExceptCharacters("<")
                   .When(x => x.HierarchyFilterMode.ToLower() == HierarchyFilterMode.MerchantName.ToString().ToLower());

                RuleFor(x => x.HierarchyFilterValue).ExceptCharacters(">")
                   .When(x => x.HierarchyFilterMode.ToLower() == HierarchyFilterMode.MerchantName.ToString().ToLower());

                RuleFor(x => x.HierarchyFilterValue).ExceptPattern("&#")
                  .When(x => x.HierarchyFilterMode.ToLower() == HierarchyFilterMode.MerchantName.ToString().ToLower());
            });
        }
    }

    public class HierarchyFilterValidator : HierarchyFilterValidator<HierarchyFilter>
    {
    }
    public class HierarchyPagingFilterValidator : HierarchyFilterValidator<HierarchyPagingFilter>
    {
    }
}
