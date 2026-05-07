using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Api.Model.Filter
{
    [DataContract]
    public class VendorAuthorizationDetailFilter : MerchantSummaryFilter
    {
        [DataMember]
        public string VendorId { get; set; }
    }
}
