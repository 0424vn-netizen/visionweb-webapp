using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.ApiClient.UnderWriting.Models
{
    public class ApproveGroup
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("approverGroupName")]
        public string ApproverGroupName { get; set; }
    }
    public class ApproveGroupData
    {
        [JsonProperty("approveGroups")]
        public List<ApproveGroup> ApproveGroups { get; set; }
    }

    public class UpdateApproveGroupData
    {
        [JsonProperty("approverGroupIds")]
        public List<int> ApproverGroupIds { get; set; }

        [JsonProperty("memberId")]
        public string MemberId { get; set; }

        [JsonProperty("memberFullname")]
        public string MemberFullname { get; set; }
    }

    public class UpdateApproveGroupRequest
    {
        [JsonProperty("header")]
        public Header Header { get; set; }

        [JsonProperty("data")]
        public UpdateApproveGroupData Data { get; set; }
    }

    public class GetApproveGroupRequest
    {
        [JsonProperty("header")]
        public Header Header { get; set; }

        [JsonProperty("data")]
        public RequestFilterBody Data { get; set; }
    }

    public class RequestFilterBody
    {
        public string SearchValue { get; set; }
        
        public JsonFilter[] Filters { get; set; }
    }

    public class JsonFilter
    {
        public string AttributeName { get; set; }
        public string Value { get; set; }
        public string SearchType { get; set; }

        public JsonFilter()
        {
            SearchType = "IN";
        }
    }
}
