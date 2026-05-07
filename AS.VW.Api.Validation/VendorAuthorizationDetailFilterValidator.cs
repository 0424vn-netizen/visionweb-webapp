using AS.VW.Api.Model.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Api.Validation
{
    public sealed class VendorAuthorizationDetailFilterValidator: MerchantSummaryFilterValidator<VendorAuthorizationDetailFilter>
    {
        public const string VENDOR_ID_FORMAT_RULES = "VendorIdFormatRules";

        public VendorAuthorizationDetailFilterValidator()
        {
            RuleSet(VENDOR_ID_FORMAT_RULES, () => {
                RuleFor(x => x.VendorId).VendorId();
            });
        }
    }
}
