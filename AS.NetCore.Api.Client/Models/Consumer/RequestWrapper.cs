namespace AS.NetCore.Api.Client.Models
{
    public class RequestWrapper
    {
        public string EndpointKey { get; set; }
        public DataModel RequestData { get; set; }
        public string Method { get; set; }
        public int CacheTime { get; set; }
    }
}
