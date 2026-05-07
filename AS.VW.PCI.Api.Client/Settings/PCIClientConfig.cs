using Newtonsoft.Json;

namespace AS.VW.PCI.Api.Client.Settings
{
    public class PCIClientConfig
    {
        [JsonProperty("applicationId")]
        public int ApplicationId { get; set; }

        [JsonProperty("applicationName")]
        public string ApplicationName { get; set; }

        [JsonProperty("applicationCode")]
        public string ApplicationCode { get; set; }
    }
}
