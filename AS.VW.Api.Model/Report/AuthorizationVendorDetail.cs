using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Api.Model.Report
{
    [DataContract]
    public class AuthorizationVendorDetail
    {
        [DataMember(Order = 1)]
        public string Entity { get; set; }

        [DataMember(Order = 2)]
        public string VendorID { get; set; }

        [DataMember(Order = 3)]
        public int? RecordID { get; set; }

        [DataMember(Order = 4)]
        public DateTime? ReportDate { get; set; }

        [DataMember(Order = 5)]
        public string CardType { get; set; }

        [DataMember(Order = 6)]
        public string DebitOrCredit { get; set; }

        [DataMember(Order = 7)]
        public int? TotalMerchantOccurrence { get; set; }

        [DataMember(Order = 8)]
        public int? AuthorizationUnderLimit { get; set; }

        [DataMember(Order = 9)]
        public int? AuthorizationOverLimit { get; set; }

        [DataMember(Order = 10)]
        public int? TotalAuthorization { get; set; }

        [DataMember(Order = 11)]
        public int? TotalMTDAuthorization { get; set; }

        [DataMember(Order = 12)]
        public string ErrorDescription { get; set; }

        [DataMember(Order = 13)]
        public int? TotalMerchant { get; set; }

        [DataMember(Order = 14)]
        public int? TotalMerchantAccepted { get; set; }

        [DataMember(Order = 15)]
        public int? TotalAuthorizationAccepted { get; set; }

        [DataMember(Order = 16)]
        public int? TotalUnrecoverableRejectAuths { get; set; }

        [DataMember(Order = 17)]
        public int? TotalRecoverableRejectAuths { get; set; }

        [DataMember(Order = 18)]
        public int? TotalMTDAuthorizationAccepted { get; set; }

        [DataMember(Order = 19)]
        public int? TotalMTDUnrecoverableRejectAuths { get; set; }

        [DataMember(Order = 20)]
        public int? TotalMTDRecoverableRejectAuths { get; set; }

        [DataMember(Order = 21)]
        public string LastTapeProcessed { get; set; }

        [DataMember(Order = 22)]
        public string ShortDescription { get; set; }

        [DataMember(Order = 23)]
        public string LongDescription { get; set; }
    }
}
