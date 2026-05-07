using FluentValidation.Validators;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AS.VW.Api.Validation.PropertyValidators
{
    public class MatchesWhenNotEmptyPropertyValidator: PropertyValidator,IRegularExpressionValidator
    {
        public string Expression {get; private set;}

        public MatchesWhenNotEmptyPropertyValidator(string expression)
            : base(ResourceApi.Validator_EC_InvalidFormat + ResourceApi.Validator_Separator+ ResourceApi.Validator_MSG_InvalidFormat )
        {
            Expression = expression;
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            string oSource = context.PropertyValue as string;

            if (string.IsNullOrEmpty(oSource))
            {
                return true;
            }
            Regex reg = new Regex(Expression, RegexOptions.None, TimeSpan.FromMilliseconds(1000));
            return  reg.IsMatch(oSource);
        }
    }
}
