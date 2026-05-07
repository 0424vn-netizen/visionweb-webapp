using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Api.Model.Report
{
    [DataContract]
    public class VoidRejectHierarchySummary
    {
        [DataMember(Order = 1)]
        public string EntityType { get; set; }

        [DataMember(Order = 01)]
        public string PlatformId { get; set; }

        [DataMember(Order = 2)]
        public string EntityId { get; set; }

        [DataMember(Order = 3)]
        public string EntityName { get; set; }

        [DataMember(Order = 4)]
        public long? TransactionCount { get; set; }

        [DataMember(Order = 5)]
        public decimal? SaleAmount { get; set; }

        [DataMember(Order = 6)]
        public decimal? ReturnAmount { get; set; }

        [DataMember(Order = 7)]
        public decimal? NetAmount { get; set; }

        [DataMember(Order = 8)]
        public int? VoidRejectCount { get; set; }

        [DataMember(Order = 9)]
        public decimal? VoidRejectAmount { get; set; }
    }
}
