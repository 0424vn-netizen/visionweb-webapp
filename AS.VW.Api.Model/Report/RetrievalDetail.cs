using System.Runtime.Serialization;

namespace AS.VW.Api.Model.Report
{
    [DataContract]
    public class RetrievalDetail
    {
        [DataMember(Order = 1)]
        public string ReportDate { get; set; }

        [DataMember(Order = 2)]
        public string TransactionDate { get; set; }

        [DataMember(Order = 3)]
        public string DateReceived { get; set; }

        [DataMember(Order = 4)]
        public string DueDate { get; set; }

        [DataMember(Order = 5)]
        public string CardType { get; set; }

        [DataMember(Order = 6)]
        public string CardNumber { get; set; }

        [DataMember(Order = 7)]
        public string ReferenceNumber { get; set; }

        [DataMember(Order = 8)]
        public string ReasonCode { get; set; }

        [DataMember(Order = 9)]
        public decimal? TransactionAmount { get; set; }
    }
}
