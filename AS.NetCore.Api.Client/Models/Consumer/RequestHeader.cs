using System;

namespace AS.NetCore.Api.Client.Models
{
    public class RequestHeader : ApiHeader
    {
        ///// <summary>
        ///// Authentication token
        ///// </summary>
        //[JsonProperty(Required = Required.Always)]
        public string SecretToken { get; set; }
    }
}
