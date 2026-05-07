using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Api.Validation
{
    public abstract class CompositeValidator<T> : AbstractValidator<T>
    {
        private readonly List<IValidator> _childValidators = new List<IValidator>();

        protected void RegisterValidator<TBase>(IValidator<TBase> validator)
        {
            if (validator.CanValidateInstancesOfType(typeof(T)))
            {
                _childValidators.Add(validator);
            }
            else {
                throw new NotSupportedException(
                    string.Format("Type {0} is not a base-class or interface implemented  by {1}",typeof(TBase).Name,typeof(T).Name));
            }
        }

        public override ValidationResult Validate(ValidationContext<T> context)
        {
            IList<ValidationFailure> mainErrors = base.Validate(context).Errors;
            IEnumerable<ValidationFailure> errorsFromChildValidators = _childValidators.SelectMany(x => x.Validate(context).Errors);
            IEnumerable<ValidationFailure> combinedErrors = mainErrors.Concat(errorsFromChildValidators);
            return new ValidationResult(combinedErrors);
        }
    }
}
