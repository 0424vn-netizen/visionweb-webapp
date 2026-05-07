using System.Runtime.Serialization;

namespace AS.VW.Api.Model.Report
{
    [DataContract]
    public class BatchHierarchySummary
    {
        [DataMember(Order = 01)]
        public string EntityId { get; set; }

        [DataMember(Order = 02)]
        public string KeyedPercent { get; set; }

        [DataMember(Order = 03)]
        public string AvgTrans { get; set; }

        [DataMember(Order = 04)]
        public string TransactionCount { get; set; }

        [DataMember(Order = 05)]
        public string BankCardSale { get; set; }

        [DataMember(Order = 06)]
        public decimal? BankCardReturn { get; set; }

        [DataMember(Order = 07)]
        public decimal? BankCardNet { get; set; }

        [DataMember(Order = 08)]
        public decimal? NonBankCardSale { get; set; }

        [DataMember(Order = 09)]
        public decimal? NonBankCardReturn { get; set; }

        [DataMember(Order = 10)]
        public decimal? NonBankCardNet { get; set; }

        [DataMember(Order = 11)]
        public decimal? TotalBankCardSale { get; set; }

        [DataMember(Order = 10)]
        public decimal? TotalBankCardReturn { get; set; }

        [DataMember(Order = 11)]
        public decimal? TotalBankCardNet { get; set; }
      
    }
}
