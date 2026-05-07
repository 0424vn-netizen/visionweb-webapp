using Newtonsoft.Json;
using System;

namespace AS.ApiClient.UnderWriting.Models
{
    public class DocumentTypeRequest
    {
        [JsonProperty("header")]
        public Header Header { get; set; }

        [JsonProperty("data")]
        public DocumentType Data { get; set; }
    }

    public class DocumentType
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("documentTypeName")]
        public string DocumentTypeName { get; set; }

        [JsonProperty("recordStatus")]
        public int RecordStatus { get; set; }

        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("updatedBy")]
        public string UpdatedBy { get; set; }

        [JsonProperty("updatedDate")]
        public DateTime UpdatedDate { get; set; }
    }
}
