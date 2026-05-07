using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.ApiClient.UnderWriting.Models
{
    public class RequestHeader
    {
        [JsonProperty("header")]
        public Header Header { get; set; }
    }

    public class Header
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("asclientId")]
        public int AsclientId { get; set; }

        [JsonProperty("languageId")]
        public string LanguageId { get; set; }
    }

    public class BaseResponse
    {
        [JsonProperty("isSuccess")]
        public bool IsSuccess { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
