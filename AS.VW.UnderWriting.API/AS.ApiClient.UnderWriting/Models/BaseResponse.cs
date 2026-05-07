using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.ApiClient.UnderWriting.Models
{
    public class BaseResponseData<T>
    {
        [JsonProperty("header")]
        public RequestHeader Header { get; set; }

        [JsonProperty("meta")]
        public ResponseMetaData Meta { get; set; } 

        [JsonProperty("data")]
        public T Data { get; set; }

        [JsonProperty("messages")]
        public ResponseMessage Messages { get; set; }
    }

    public class ResponseMetaData
    {
        [JsonProperty("messages")]
        public List<Message> Messages { get; set; }

        [JsonProperty("timeStamp")]
        public DateTime TimeStamp { get; set; }
    }
    public class Message
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("fieldName")]
        public string FieldName { get; set; }
    }
    public class ResponseMessage
    {
        [JsonProperty("field")]
        public string Field { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }
    }
}
