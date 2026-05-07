using System.Runtime.Serialization;

namespace AS.VW.Api.Model.Report
{
    [DataContract]
    public class ChargebackDetail
    {
        [DataMember(Order = 1)]
        public string ReportDate { get; set; }
             
        [DataMember(Order = 2)]
        public string TransactionDate { get; set; }

        [DataMember(Order = 3)]
        public string CardType { get; set; }

        [DataMember(Order = 4)]
        public string CardNumber { get; set; }

        [DataMember(Order = 5)]
        public string ReasonCode { get; set; }

        [DataMember(Order = 6)]
        public string ReasonText { get; set; } 

        [DataMember(Order = 7)]
        public string ReferenceNumber { get; set; }

        [DataMember(Order = 8)]
        public decimal? FirstChargebackAmount { get; set; }        
    }
}
