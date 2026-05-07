using AS.VW.Api.Model.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentValidation;

namespace AS.VW.Api.Validation
{
    public sealed class BatchDetailFilterValidator : DetailFilterValidator<BatchDetailFilter>
    {
        public const string BATCH_NUMBER_RULES = "BatchNumberRules";

        public BatchDetailFilterValidator()
        {
            RuleSet(BATCH_NUMBER_RULES, () =>
            {
                RuleFor(x => x.BatchNumber).NotEmpty2().BatchNumber();
            });
        }
    }
}
