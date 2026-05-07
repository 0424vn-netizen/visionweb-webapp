using System.Runtime.Serialization;

namespace AS.VW.Api.Model.Report
{
    /// <summary>
    /// Rolled-up from DepositDetail by Merchant and ReportDate.    
    /// </summary>
    [DataContract]
    public class DepositSummary
    {
        
        [DataMember(Order = 1)]
        public string ReportDate { get; set; }

        [DataMember(Order = 2)]
        public string DepositDate { get; set; }

        [DataMember(Order = 3)]
        public string RoutingNumber { get; set; }

        [DataMember(Order = 4)]
        public string DDANumber { get; set; }

        [DataMember(Order = 5)]
        public int? DepositCount { get; set; }

        [DataMember(Order = 6)]
        public decimal? DepositAmount { get; set; }

        [DataMember(Order = 7)]
        public int? DebitCount { get; set; }

        [DataMember(Order = 8)]
        public decimal? DebitAmount { get; set; }
         
        [DataMember(Order = 9)]
        public decimal? NetDepositAmount { get; set; }
    }
}
