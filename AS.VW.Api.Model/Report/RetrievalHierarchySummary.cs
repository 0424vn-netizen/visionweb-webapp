using System.Runtime.Serialization;

namespace AS.VW.Api.Model.Report
{
    [DataContract]
    public class RetrievalHierarchySummary
    {
        [DataMember(Order = 1)]
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
        public int? RetrievalCount { get; set; }

        [DataMember(Order = 7)]
        public decimal? RetrievalAmount { get; set; }

    }
}
