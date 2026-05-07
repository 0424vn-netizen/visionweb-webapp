using AS.VW.Api.Model.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace AS.VW.Api.Validation
{
    public sealed class VoucherFilterValidator : DetailFilterValidator<VoucherFilter>
    {
        public const string TRANSACTION_ID_RULES = "TransactionIdRules";

        public VoucherFilterValidator()
        {
            RuleSet(TRANSACTION_ID_RULES, () =>
            {
                RuleFor(x => x.TransactionId).NotNull2().NotEmpty2();
            });
        }
    }
}
