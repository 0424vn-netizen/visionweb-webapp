using System.Runtime.Serialization;

namespace AS.VW.Api.Model.Report
{
    [DataContract]
    public class CardSummary
    {
        [DataMember(Order = 1)]
        public string CardType { get; set; }

        [DataMember(Order = 2)]
        public string TransactionCode { get; set; }

        [DataMember(Order = 3)]
        public long? TransactionCount { get; set; }

        [DataMember(Order = 4)]
        public decimal? TransactionAmount { get; set; }

    }
}
