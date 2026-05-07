using System.Runtime.Serialization;

namespace AS.VW.Api.Model.Report
{
    [DataContract]
    public class BatchSummary
    {
        [DataMember(Order = 1)]
        public string ReportDate {get; set;}

        [DataMember(Order = 2)]
        public string TerminalNumber {get; set;}

        [DataMember(Order = 3)]
        public string FileSource {get; set;}

        [DataMember(Order = 4)]
        public string BatchNumber {get; set;}

        [DataMember(Order = 5)]
        public decimal? KeyedPercent {get; set;}

        [DataMember(Order = 6)]
        public decimal? AvgTrans {get; set;}

        [DataMember(Order = 07)]
        public long? TransactionCount { get; set; }

        [DataMember(Order = 8)]
        public decimal? BankCardSale { get; set; }

        [DataMember(Order = 10)]
        public decimal? BankCardReturn { get; set; }

        [DataMember(Order = 11)]
        public decimal? BankCardNet { get; set; }

        [DataMember(Order = 12)]
        public decimal? NonBankCardSale { get; set; }

        [DataMember(Order = 13)]
        public decimal? NonBankCardReturn { get; set; }

        [DataMember(Order = 14)]
        public decimal? NonBankCardNet { get; set; }

        [DataMember(Order = 15)]
        public decimal? TotalBankCardSale { get; set; }

        [DataMember(Order = 16)]
        public decimal? TotalBankCardReturn { get; set; }

        [DataMember(Order = 17)]
        public decimal? TotalBankCardNet { get; set; }
    }
}
