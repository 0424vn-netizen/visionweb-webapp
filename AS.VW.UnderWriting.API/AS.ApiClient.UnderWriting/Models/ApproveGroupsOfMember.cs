using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.ApiClient.UnderWriting.Models
{
    public class ApproveGroupsOfMember
    {
        public string Id { get; set; }
        public string ApproverGroupName { get; set; }
        public bool IsAssigned { get; set; }
    }
}
