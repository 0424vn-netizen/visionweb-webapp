using System.Runtime.Serialization;

namespace AS.VW.Api.Model.Report
{
    /// <summary>
    /// Rolled-up from DepositDetail by Merchant/Hierarchy
    /// </summary>
    [DataContract]
    public class DepositHierarchySummary
    {
        [DataMember(Order = 1)]
        public string EntityId { get; set; }

        [DataMember(Order = 2)]
        public int? DepositCount { get; set; }

        [DataMember(Order = 3)]
        public decimal? DepositAmount { get; set; }

        [DataMember(Order = 4)]
        public int? DebitCount { get; set; }

        [DataMember(Order = 5)]
        public decimal? DebitAmount { get; set; }
          
        [DataMember(Order = 6)]
        public decimal? NetDeposit { get; set; }

        
    }
}
