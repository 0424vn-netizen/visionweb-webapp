using System.Runtime.Serialization;

namespace AS.VW.Api.Model.Report
{
    [DataContract]
    public class TransactionDetailBatch
    {
        [DataMember(Order = 1)]
        public string TransactionDate { get; set; }

        [DataMember(Order = 2)]
        public string TransactionTime { get; set; }

        [DataMember(Order = 3)]
        public string TransactionCode { get; set; }

        [DataMember(Order = 4)]
        public string TerminalNumber { get; set; }

        [DataMember(Order = 5)]
        public string FileSource { get; set; }

        [DataMember(Order = 6)]
        public string Keyed { get; set; }

        [DataMember(Order = 7)]
        public string EMV { get; set; }

        [DataMember(Order = 8)]
        public string CardType { get; set; }

        [DataMember(Order = 9)]
        public string CardNumber { get; set; }

        [DataMember(Order = 10)]
        public decimal? TransactionAmount { get; set; }

    }
}
