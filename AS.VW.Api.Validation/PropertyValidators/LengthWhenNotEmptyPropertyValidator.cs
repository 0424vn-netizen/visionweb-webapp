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
    public class LengthWhenNotEmptyPropertyValidator: PropertyValidator,ILengthValidator
    {
        public int Max
        {
            get;
            private set;
        }

        public int Min
        {
            get;
            private set;
        }
       
        public LengthWhenNotEmptyPropertyValidator(int min, int max)
            : this(min,max,ResourceApi.Validator_MSG_Length)
        {
        }

        public LengthWhenNotEmptyPropertyValidator(int min, int max, string errorMessager)
            : base(ResourceApi.Validator_EC_Length+ ResourceApi.Validator_Separator+errorMessager)
        {
            Min = min;
            Max = max;
            if (max < min)
                throw new ArgumentOutOfRangeException("max", "Max should be larger than min.");
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            string oSource = context.PropertyValue as string;

            if (string.IsNullOrEmpty(oSource))
            {
                return true;
            }
            int length = oSource.Length;
            if (length < Min || length > Max)
            {
                context.MessageFormatter
                    .AppendArgument("MinLength", Min)
                    .AppendArgument("MaxLength", Max)
                    .AppendArgument("TotalLength", length);
                return false;
            }

            return true;
        }
    }
    public class ExactLengthWhenNotEmptyPropertyValidator : LengthWhenNotEmptyPropertyValidator
    {
        public ExactLengthWhenNotEmptyPropertyValidator(int exactLength):
            base(exactLength,exactLength,Resources.ResourceApi.Validator_MSG_ExactLength) 
        { }
    }


}
