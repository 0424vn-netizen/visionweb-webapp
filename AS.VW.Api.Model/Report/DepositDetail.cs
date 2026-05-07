using System.Runtime.Serialization;

namespace AS.VW.Api.Model.Report
{
    [DataContract]
    public class DepositDetail
    {
        [DataMember(Order = 1)]
        public string ReportDate { get; set; }

        [DataMember(Order = 2)]
        public string DepositDate { get; set; }

        [DataMember(Order = 3)]
        public string TransactionCode { get; set; }
         
        [DataMember(Order = 4)]
        public string TransactionType { get; set; }

        [DataMember(Order = 5)]
        public string RoutingNumber { get; set; }
         
        [DataMember(Order = 6)]
        public string DDANumber { get; set; }

        [DataMember(Order = 7)]
        public string DepositTraceNumber { get; set; }

        [DataMember(Order = 8)]
        public decimal? DepositAmount { get; set; }
         
    }
}
