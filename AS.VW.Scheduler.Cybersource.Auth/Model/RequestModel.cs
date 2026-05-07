using Newtonsoft.Json;

namespace AS.VW.Scheduler.Cybersource.Auth.Model
{
    public class RequestModel
    {
        [JsonProperty("query")]
        public string Query { get; set; }

        [JsonProperty("offset")]
        public int Offset { get; set; }

        [JsonProperty("limit")]
        public int Limit { get; set; }

        [JsonProperty("sort")]
        public string Sort { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }    
}
