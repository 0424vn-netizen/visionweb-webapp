using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Scheduler.Cybersource.Auth.Model
{
    public class AuthorizationData
    {
        public string OriginalFileName { get; set; }
        public List<AuthViewModel> Auths { get; set; }
    }
    public class AuthViewModel
    {
        public string Id { get; set; }
        
        public string ReportDate { get; set; }
        public string ReportTime { get; set; }

        [Required]
        public string MerchantId { get; set; }
        public string TerminalId { get; set; }
        public string BatchNumber { get; set; }

        [Required]
        public string CardType { get; set; }
        public string CardMethod { get; set; }
        public string CardMethodDescription { get; set; }

        [JsonIgnore]
        public string AccountNumber { get; set; }

        [Required]
        public string EncryptedAccountNumber { get; set; }
        public string HashedAccountNumber { get; set; }

        [Required]
        public string BinNumber { get; set; }    
        public string Bin8Number { get; set; }
        public string HashedBin8Number { get; set; }

        [Required]
        public string MSAccountNumber { get; set; }        

        [Required]
        public string ExpirationDate { get; set; }

        [Required]
        public string TransactionCode { get; set; }

        [Required]
        public string TransactionDate { get; set; }

        [Required]
        public string TransactionTime { get; set; }

        [Required]
        public decimal? TotalAmount { get; set; }

        [Required]
        public string AuthorizationNumber { get; set; }

        [Required]
        public string ResponseCode { get; set; }

        [Required]
        public string EntryMode { get; set; }
        public string Currency { get; set; }      

        [Required]
        public string CVV2ResponseCode { get; set; }

        [Required]
        public string AVSCode { get; set; }
        public string AVSCodeRaw { get; set; }
        public string TransactionID { get; set; }
        public string ReferenceNumber { get; set; }
        public ApplicationInformation ApplicationInformation { get; set; }
        public bool IsValid { get; set; }
    }    

    public class SaveDataModel
    {
        public string OriginalFileName { get; set; }
        public string JsonData { get; set; }
        public string TransactionDate { get; set; }
        public int TransactionCount { get; set; }
        public bool IsValid { get; set; }
    }

}
