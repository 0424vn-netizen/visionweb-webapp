using AS.VW.Api.Model.Filter;
using Resources;

namespace AS.VW.Api.Validation
{
    public class CardSummaryFilterValidator : ReportFilterValidator<CardSummaryFilter>
    {
        public const string BATCH_NUMBER_RULES = "BatchNumberRules";
        public const string HIERARCHY_FILTER_MODE_NOT_ACCEPT = "HierarchyFilterMode";

        public CardSummaryFilterValidator()
        {
            RuleSet(BATCH_NUMBER_RULES, () =>
            {
                RuleFor(x => x.BatchNumber).BatchNumber();
            });

            RuleSet(HIERARCHY_FILTER_MODE_NOT_ACCEPT, () =>
            {
                RuleFor(x => x.HierarchyFilterMode).NotEmpty2();

                RuleFor(x => x.HierarchyFilterMode).Must2(x => x.ToUpper() != HierarchyFilterMode.MerchantName.ToString().ToUpper()).WithErrorCodeAndMessage(ResourceApi.Validator_EC_InvalidCharacter, ResourceApi.Validator_Card_Sum_Hier_Mode);

                RuleFor(x => x.HierarchyFilterMode).Must2(x => x.ToUpper() != HierarchyFilterMode.Last6MerchantNumber.ToString().ToUpper()).WithErrorCodeAndMessage(ResourceApi.Validator_EC_InvalidCharacter, ResourceApi.Validator_Card_Sum_Hier_Mode);

                RuleFor(x => x.HierarchyFilterMode).Must2(x => x.ToUpper() != HierarchyFilterMode.LAST6MERCHNUMBER.ToString().ToUpper()).WithErrorCodeAndMessage(ResourceApi.Validator_EC_InvalidCharacter, ResourceApi.Validator_Card_Sum_Hier_Mode);
            });
        }
    }

}
