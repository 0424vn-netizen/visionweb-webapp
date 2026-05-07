using FluentValidation;
using FluentValidation.Results;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Api.Validation
{
    [Serializable]
    public class CustomValidationException : ArgumentException
    {
        public List<CustomValidationFailure> Errors { get; private set; }

        public string ErrorCode { get; private set; }

        protected CustomValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            //
        }
        public CustomValidationException(IEnumerable<ValidationFailure> errors)
        {
            Errors = new List<CustomValidationFailure>();
            foreach (var failure in errors)
            {
                string failureErrorMessage = failure.ErrorMessage;
                int indexOfSeparator = failure.ErrorMessage.IndexOf(ResourceApi.Validator_Separator);

                string failureErrorCode = ResourceApi.Validator_EC_Default;
                if (indexOfSeparator > 0)
                {
                    failureErrorCode = failureErrorMessage.Substring(0, indexOfSeparator);
                    failureErrorMessage = failureErrorMessage.Substring(indexOfSeparator+1);
                }
                CustomValidationFailure cusFailure = null;
                if (failure.AttemptedValue != null)
                {
                    cusFailure = new CustomValidationFailure(failure.PropertyName, failureErrorMessage, failure.AttemptedValue, failureErrorCode);
                }
                else
                {
                    cusFailure = new CustomValidationFailure(failure.PropertyName, failureErrorMessage, failureErrorCode);
                }
                Errors.Add(cusFailure);
            }
            if (Errors.Count > 0)
            {
                ErrorCode = Errors[0].ErrorCode;
            }
            
        }

        public override string Message
        {
            get
            {
                var arr = Errors.Select(x => "\r\n --" + x.ErrorMessage).ToArray();
                return "Validation failed: "+ string.Join("",arr);
            }
        }
    }
}
