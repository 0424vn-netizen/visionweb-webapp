using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Api.Model.Report
{
    [DataContract]
    public class AuthorizationLogSummary
    {
        [DataMember(Order=1)]
        public string EntityId { get; set; }

        [DataMember(Order = 2)]
        public long? TransactionCount { get; set; }

        [DataMember(Order = 3)]
        public decimal? SaleAmount { get; set; }

        [DataMember(Order = 4)]
        public decimal? ReturnAmount { get; set; }

        [DataMember(Order = 5)]
        public decimal? NetAmount { get; set; }

        [DataMember(Order = 6)]
        public int? AuthorizationCount { get; set; }

        [DataMember(Order = 7)]        
        public decimal? AuthorizationAmount { get; set; }

    }
}
