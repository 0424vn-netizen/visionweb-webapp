using FluentValidation.Validators;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Api.Validation.PropertyValidators
{
    public class ExceptCharactersPropertyValidator : PropertyValidator
    {
        public string Characters
        {
            get;
            private set;
        }

        public ExceptCharactersPropertyValidator(string characters)
            : base(ResourceApi.Validator_EC_InvalidCharacter+ResourceApi.Validator_Separator+ResourceApi.Validator_MSG_InvalidCharacter)
        {
            Characters = characters;
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            string oSource = context.PropertyValue as string;

            return !oSource.HasInvalidCharacters(Characters);
        }     

         
    }
}
