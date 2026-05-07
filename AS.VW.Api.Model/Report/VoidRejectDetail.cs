using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Api.Model.Report
{
    [DataContract]
    public class VoidRejectDetail
    {
        [DataMember(Order = 1)]
        public DateTime? ReportDate { get; set; }

        [DataMember(Order = 2)]
        public string BatchNumber { get; set; }

        [DataMember(Order = 3)]
        public DateTime? TransactionDate { get; set; }

        [DataMember(Order = 4)]
        public string TransactionTime { get; set; }

        [DataMember(Order = 5)]
        public string TransactionCode { get; set; }

        [DataMember(Order = 6)]
        public string TransactionDescription { get; set; }

        [DataMember(Order = 7)]
        public string Keyed { get; set; }

        [DataMember(Order = 8)]
        public string EntryModeDescription { get; set; }

        [DataMember(Order = 9)]
        public string CardType { get; set; }

        [DataMember(Order = 10)]
        public string CardDescription { get; set; }

        [DataMember(Order = 11)]
        public string CardNumber { get; set; }

        [DataMember(Order = 12)]
        public string AuthorizationNumber { get; set; }

        [DataMember(Order = 13)]
        public string ReasonCode { get; set; }

        [DataMember(Order = 14)]
        public string ReasonCodeDescription { get; set; }

        [DataMember(Order = 15)]
        public decimal? TransactionAmount { get; set; }
    }
}
