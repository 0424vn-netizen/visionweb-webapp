using Newtonsoft.Json;

namespace AS.NetCore.Api.Client.Models
{
    public class UpdateChainNumberRequest : RequestBody
    {
        public bool IsCreate { get; set; }
        public string MerchantNumber { get; set; }

        public string ChainNumber { get; set; }
    }
}
