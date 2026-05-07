using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Api.Validation
{
    public class CustomValidationFailure : ValidationFailure
    {
        public string ErrorCode {get;private set;}
        public CustomValidationFailure(string propertyName, string error,string errorCode):base(propertyName,error)
        {
            ErrorCode = errorCode;
        }

        public CustomValidationFailure(string propertyName, string error, object attemptedValue, string errorCode)
            :base(propertyName,error,attemptedValue)
        {
            ErrorCode = errorCode;
        }
    }
}
