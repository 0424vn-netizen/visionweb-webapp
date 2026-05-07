using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Api.Model.Report
{
    [DataContract]
    public class Voucher
    {
        [DataMember(Order = 1)]
        public string CardType { get; set; }

        [DataMember(Order = 2)]
        public string FileSource { get; set; }

        [DataMember(Order = 3)]
        public string MerchantNumber { get; set; }

        [DataMember(Order = 4)]
        public DateTime? TransactionDate { get; set; }

        [DataMember(Order = 5)]
        public string CardNumber { get; set; }

        [DataMember(Order = 7)]
        public string BatchNumber { get; set; }

        [DataMember(Order = 8)]
        public string AuthorizationNumber { get; set; }

        [DataMember(Order = 9)]
        public decimal? TransactionAmount { get; set; }

        [DataMember(Order = 10)]
        public string MerchantName { get; set; }

        [DataMember(Order = 11)]
        public string Address1 { get; set; }

        [DataMember(Order = 12)]
        public string Address2 { get; set; }
    }
}
