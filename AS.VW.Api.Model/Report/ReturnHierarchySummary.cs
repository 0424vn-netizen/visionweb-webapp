using System.Runtime.Serialization;

namespace AS.VW.Api.Model.Report
{
    [DataContract]
    public class ReturnHierarchySummary
    {
        [DataMember(Order = 1)]
        public string EntityId { get; set; }

        [DataMember(Order = 2)]
        public long? SaleCount { get; set; }

        [DataMember(Order = 3)]
        public decimal? SaleAmount { get; set; }

        [DataMember(Order = 4)]
        public long? ReturnsCount { get; set; }  

        [DataMember(Order = 5)]
        public decimal? FullReturn { get; set; }

        [DataMember(Order = 6)]
        public decimal? PartialReturn { get; set; }

        [DataMember(Order = 7)]
        public string NoMatch  { get; set; }

        [DataMember(Order = 9)]
        public decimal? NetAmount { get; set; }
    }
}
