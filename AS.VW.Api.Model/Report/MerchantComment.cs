using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Api.Model.Report
{
    [DataContract]
    public class MerchantComment
    {
        [DataMember(Order = 1)]
        public string MerchantNumber { get; set; }

        [DataMember(Order = 2)]
        public DateTime? CommentedDate { get; set; }

        [DataMember(Order = 3)]
        public string PostedBy { get; set; }

        [DataMember(Order = 4)]
        public string CommentText { get; set; }
    }
}
