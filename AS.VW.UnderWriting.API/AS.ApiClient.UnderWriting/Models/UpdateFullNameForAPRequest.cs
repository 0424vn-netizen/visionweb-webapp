using Newtonsoft.Json;

namespace AS.ApiClient.UnderWriting.Models
{
    public class UpdateFullNameForAPRequest
    {
        [JsonProperty("header")]
        public Header Header { get; set; }

        [JsonProperty("data")]
        public MemberItem Data { get; set; }
    }

    public class MemberItem
    {
        [JsonProperty("memberId")]
        public string MemberId { get; set; }

        [JsonProperty("memberFullname")]
        public string MemberFullname { get; set; }
    }
}
