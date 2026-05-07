using System.Runtime.Serialization;
using System.Security;

namespace AS.VW.Api.Model.Report
{
    [DataContract]
    public class TransactionDetail
    {
        [DataMember(Order = 1)]
        public string EntityID { get; set; }

        [DataMember(Order = 2)]
        public string MerchantName { get; set; }

        [DataMember(Order = 3)]
        public string ReportDate { get; set; }

        [DataMember(Order = 4)]
        public string BatchNumber { get; set; }

        [DataMember(Order = 5)]
        public string TransactionDate { get; set; }

        [DataMember(Order = 6)]
        public string TransactionTime { get; set; }

        [DataMember(Order = 7)]
        public string TransactionCode { get; set; }

        [DataMember(Order = 8)]
        public string Matched { get; set; }

        [DataMember(Order = 9)]
        public string TerminalNumber { get; set; }

        [DataMember(Order = 10)]
        public string FileSource { get; set; }

        [DataMember(Order = 11)]
        public string Keyed { get; set; }

        [DataMember(Order = 12)]
        public string EMV { get; set; }

        [DataMember(Order = 13)]
        public string CardType { get; set; }

        [DataMember(Order = 14)]
        public string CardNumber { get; set; }

        private string expirationDate; 
        [DataMember(Order = 15)]
        public string ExpirationDate { 
            get{
                return expirationDate;
            }
            set {
                expirationDate = (value != null ? SecurityElement.Escape(value) : string.Empty);
            } 
        }

        [DataMember(Order = 16)]
        public string AuthorizationNumber { get; set; }

        [DataMember(Order = 17)]
        public string TransactionAmount { get; set; } 
    }
}
