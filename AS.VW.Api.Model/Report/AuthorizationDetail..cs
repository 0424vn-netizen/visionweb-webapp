using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Api.Model.Report
{
    [DataContract]
    public class AuthorizationDetail
    {
            
        [DataMember(Order = 1)]
        public string ReportDate { get; set; }

        [DataMember(Order = 2)]
        public string TransactionDate { get; set; }

        [DataMember(Order = 3)]
        public string TransactionTime { get; set; }

        [DataMember(Order = 4)]
        public string TransactionCode { get; set; }

        [DataMember(Order = 5)]
        public string Keyed { get; set; }

        [DataMember(Order = 6)]
        public string EMV { get; set; }

        [DataMember(Order = 7)]
        public string CardType { get; set; }

        [DataMember(Order = 8)]
        public string CardNumber { get; set; }

        [DataMember(Order = 9)]
        public string ExpirationDate { get; set; }

        [DataMember(Order = 10)]
        public string AuthorizationNumber { get; set; }

        [DataMember(Order = 11)]
        public decimal? AuthorizationAmount { get; set; }

        [DataMember(Order = 12)]
        public decimal? TransactionAmount { get; set; }

        [DataMember(Order = 13)]
        public string ApprovedDeclined { get; set; }

        [DataMember(Order = 14)]
        public string ResponseCode { get; set; }

        [DataMember(Order = 15)]
        public string AVS { get; set; }

        [DataMember(Order = 16)]
        public string CVV { get; set; }

        [DataMember(Order = 17)]
        public string AuthorizationSource { get; set; }

        [DataMember(Order = 18)]
        public string CustomerID { get; set; }

        [DataMember(Order = 19)]
        public string MOTO { get; set; }

    }
}
