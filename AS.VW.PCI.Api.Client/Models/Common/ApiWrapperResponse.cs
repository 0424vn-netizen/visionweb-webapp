using Newtonsoft.Json;

namespace AS.VW.PCI.Api.Client.Models.Common
{
    public class ApiWrapperResponse<T> 
    { 
        [JsonProperty("data")] 
        public T Data { get; set; } 
        
        [JsonProperty("errorMessages")] 
        public object ErrorMessages { get; set; } 
    }
}
