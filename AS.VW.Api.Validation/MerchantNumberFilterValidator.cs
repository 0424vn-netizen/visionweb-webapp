using AS.VW.Api.Model.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentValidation;

namespace AS.VW.Api.Validation
{
    public class MerchantNumberFilterValidator<T> : CompositeValidator<T> where T : IMerchantNumberFilter
    {
        public const string FORMAT_RULES = "FormatRules";
        public const string REQUIRE_RULE = "RequireRule";
        public const string ALL_RULES = REQUIRE_RULE + "," + FORMAT_RULES;

        public MerchantNumberFilterValidator()
        {
            RuleSet(REQUIRE_RULE, () =>
            {
                RuleFor(x => x.MerchantNumber).NotEmpty2();
            });

            RuleSet(FORMAT_RULES, () =>
            {
                RuleFor(x => x.MerchantNumber).LengthWhenNotEmpty(0, 16).OnlyAllowNumbers();
            });

        }
    }
    public class MerchantNumberFilterValidator : MerchantNumberFilterValidator<IMerchantNumberFilter> { }

    public class MerchantInformationFilterValidator : MerchantNumberFilterValidator<MerchantInformationFilter> { }

    public class MerchantInformationFilterPagingValidator : MerchantNumberFilterValidator<MerchantInformationPagingFilter> { }
}
