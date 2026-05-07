using AS.VW.Api.Validation.PropertyValidators;
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
    public static class ValidatorExtensions
    {
        public static IRuleBuilderOptions<T, TProperty> WithErrorCodeAndMessage<T, TProperty>(this IRuleBuilderOptions<T, TProperty> ruleBuilder, string errorCode, string message)
        {
            return ruleBuilder.WithMessage(errorCode + ResourceApi.Validator_Separator + message);
        }

        public static IRuleBuilderOptions<T, TProperty> NotEmpty2<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder)
        {
            return ruleBuilder.NotEmpty().WithErrorCodeAndMessage(ResourceApi.Validator_EC_NotEmpty , FluentValidation.Resources.Messages.notempty_error);
        }

        public static IRuleBuilderOptions<T, TProperty> NotNull2<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder)
        {
            return ruleBuilder.NotNull().WithErrorCodeAndMessage(ResourceApi.Validator_EC_NotNull, FluentValidation.Resources.Messages.notnull_error);
        }

        public static IRuleBuilderOptions<T, TProperty> Must2<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, Func<TProperty, bool> predicate) { 
            return ruleBuilder.Must(predicate).WithErrorCodeAndMessage(ResourceApi.Validator_EC_Predicate,FluentValidation.Resources.Messages.predicate_error);
        }

        public static IRuleBuilderOptions<T, string> ExceptCharacters<T>(this IRuleBuilder<T, string> ruleBuilder, string characters)
        {
            return ruleBuilder.SetValidator(new ExceptCharactersPropertyValidator(characters));
        }

        public static IRuleBuilderOptions<T, string> MatchesWhenNotEmpty<T>(this IRuleBuilder<T, string> ruleBuilder, string expression)
        {
            return ruleBuilder.SetValidator(new MatchesWhenNotEmptyPropertyValidator(expression));
        }

        public static IRuleBuilderOptions<T, string> OnlyAllowNumbers<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.MatchesWhenNotEmpty(ResourceApi.Regexp_Numeric)
                .WithErrorCodeAndMessage(ResourceApi.Validator_EC_OnlyAllowNumbers,ResourceApi.Validator_MSG_OnlyAllowNumbers);
        }

        public static IRuleBuilderOptions<T, string> LengthWhenNotEmpty<T>(this IRuleBuilder<T, string> ruleBuilder, int exactLength)
        {
            return ruleBuilder.SetValidator(new ExactLengthWhenNotEmptyPropertyValidator(exactLength));
        }

        public static IRuleBuilderOptions<T, string> LengthWhenNotEmpty<T>(this IRuleBuilder<T, string> ruleBuilder, int min, int max)
        {
            return ruleBuilder.SetValidator(new LengthWhenNotEmptyPropertyValidator(min, max));
        }

        public static IRuleBuilderOptions<T, string> AuthorizationNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.LengthWhenNotEmpty(0, 16).MatchesWhenNotEmpty(ResourceApi.Regexp_AlphaCharacters);
        }

        public static IRuleBuilderOptions<T, string> First6CardNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.LengthWhenNotEmpty(6).OnlyAllowNumbers();
        }

        public static IRuleBuilderOptions<T, string> Last4CardNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.LengthWhenNotEmpty(4).OnlyAllowNumbers();
        }

        public static IRuleBuilderOptions<T, string> Last6MerchantNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.LengthWhenNotEmpty(6).OnlyAllowNumbers();
        }

        public static IRuleBuilderOptions<T, string> BatchNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.LengthWhenNotEmpty(0, 20).MatchesWhenNotEmpty(Resources.ResourceApi.Regexp_AlphaCharacters);
        }

        public static IRuleBuilderOptions<T, string> VendorId<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.LengthWhenNotEmpty(0, 10).ExceptCharacters(Resources.ResourceApi.Regexp_SpecialCharacters);
        }

        public static IRuleBuilderOptions<T, DateTime> DateRules<T>(this IRuleBuilder<T, DateTime> ruleBuilder)
        {
            return ruleBuilder.NotNull2()
                .GreaterThan(DateTime.MinValue).WithErrorCodeAndMessage(ResourceApi.Validator_EC_GreaterThan, ResourceApi.Validator_MSG_GreaterThanMinDate)
                .LessThanOrEqualTo(DateTime.Now).WithErrorCodeAndMessage(ResourceApi.Validator_EC_LessThanOrEqualTo, ResourceApi.Validator_MSG_LessOrEqualToday);
        }

        public static IRuleBuilderOptions<T, string> ExceptPattern<T>(this IRuleBuilder<T, string> ruleBuilder, string expression)
        {
            return ruleBuilder.SetValidator(new ExceptPatternPropertyValidator(expression));
        }
    }
}
