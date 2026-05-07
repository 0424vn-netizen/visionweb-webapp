using FluentValidation.Results;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AS.VW.Api.Validation
{
    public static class GeneralExtentions
    {
        public static bool IsNotNullAndNotEmpty(this string str)
        {
            return !string.IsNullOrEmpty(str);
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool HasInvalidCharacters(this string str, string characters)
        {
            if (str.IsNotNullAndNotEmpty() && characters.IsNotNullAndNotEmpty())
            {
                foreach (var c in characters)
                {
                    if (str.IndexOf(c) >= 0)
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Converts the validation result to an error result which will be serialized by ServiceStack in a clean and human-readable way.
        /// </summary>
        /// <param name="result">The validation result</param>
        /// <returns></returns>
        public static List<object> ToErrorResult(this ValidationResult result)
        {
            var validationResult = new List<object>();
            foreach (var error in result.Errors)
                validationResult.Add(new
                {
                    PropertyName = error.PropertyName,
                    ErrorMessage = error.ErrorMessage,
                    AttemptedValue = error.AttemptedValue
                });

            return validationResult;
        }
    }
}
