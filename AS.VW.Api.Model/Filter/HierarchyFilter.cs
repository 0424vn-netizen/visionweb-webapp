using AS.VW.Api.Model.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Api.Model.Filter
{
    [DataContract]
    public class HierarchyFilter : BaseFilter, IHierarchyFilter
    {
        [DataMember]
        public string HierarchyFilterMode { get; set; }
        
        [DataMember]
        public string HierarchyFilterValue { get; set; }
    }
}
