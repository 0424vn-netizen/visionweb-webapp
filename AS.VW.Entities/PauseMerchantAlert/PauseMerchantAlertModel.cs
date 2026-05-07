using Newtonsoft.Json;
using System;

namespace AS.VW.Entities.PauseMerchantAlert
{
    public class PauseMerchantAlertModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        [JsonProperty("MerchantID")]
        public string MerchantId { get; set; }

        [JsonProperty("RowGUID")]
        public string RowGuid { get; set; }
    }
    
}
