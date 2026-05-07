using AS.VW.Api.Model.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Api.Validation
{
    public sealed class VendorAuthorizationFilterValidator: ReportFilterValidator<VendorAuthorizationFilter>
    {
        public const string VENDOR_ID_FORMAT_RULES = "VendorIdFormatRules";

        public VendorAuthorizationFilterValidator()
        {
            RuleSet(VENDOR_ID_FORMAT_RULES, () => {
                RuleFor(x => x.VendorId).VendorId();
            });
        }
    }
}
