using AS.VW.Api.Model.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Resources;

namespace AS.VW.Api.Validation
{
    public class DataViewLevelValidator : CompositeValidator<IDataViewLevelFilter>
    {
        public const string DATAVIEWLEVEL_FORMAT_RULES = "DataViewLevelRules";

        public DataViewLevelValidator()
        {
            RuleSet(DATAVIEWLEVEL_FORMAT_RULES, () =>
            {
                RuleFor(x => x.ViewLevelValidate).Must((ViewLevel) => Enum.IsDefined(typeof(DataViewLevel), ViewLevel)).WithErrorCodeAndMessage(ResourceApi.Validator_EC_InvalidFormat,
                    ResourceApi.Validator_MSG_InvalidFormat);
            });
        }
    }
}
