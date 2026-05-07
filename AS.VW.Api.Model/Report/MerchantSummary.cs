using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Api.Model.Report
{
    [DataContract]
    public class MerchantSummary
    {
        [DataMember(Order = 1)]
        public string MerchantNumber { get; set; }

        [DataMember(Order = 2)]
        public string MerchantName { get; set; }

        [DataMember(Order = 4)]
        public string CorporateName { get; set; }

        [DataMember(Order = 5)]
        public string SysPrinAgent { get; set; }

        [DataMember(Order = 6)]
        public string SalesAgent { get; set; }

        [DataMember(Order = 7)]
        public string HeadquarterMerchantNumber { get; set; }

        [DataMember(Order = 8)]
        public string ChainCode { get; set; }

        [DataMember(Order = 9)]
        public string Bank { get; set; }

        [DataMember(Order = 10)]
        public string Agent { get; set; }

        [DataMember(Order = 11)]
        public string Corp { get; set; }

        [DataMember(Order = 12)]
        public string NorthChain { get; set; }

        [DataMember(Order = 13)]
        public string NorthSalesAgent { get; set; }

        [DataMember(Order = 14)]
        public string MasterChain { get; set; }

        [DataMember(Order = 15)]
        public string SalesmanNo { get; set; }

        [DataMember(Order = 16)]
        public string MemphisChain { get; set; }

        [DataMember(Order = 17)]
        public string MasterSalesAgent { get; set; }

        [DataMember(Order = 18)]
        public string MCCSIC { get; set; }

        [DataMember(Order = 19)]
        public string Description { get; set; }

        [DataMember(Order = 20)]
        public string MerchantStatus { get; set; }

        [DataMember(Order = 21)]
        public string Status { get; set; }

        [DataMember(Order = 22)]
        public DateTime? LastBatchActivity { get; set; }

        [DataMember(Order = 23)]
        public string Address { get; set; }

        [DataMember(Order = 24)]
        public string Phone { get; set; }

        [DataMember(Order = 25)]
        public string Email { get; set; }

    }
}
