using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Api.Model.Filter
{
    [DataContract]
    public class DetailPagingFilter : DetailFilter,IPagingFilter
    {
        [DataMember]
        public int PageSize { get; set; }

        [DataMember]
        public int CurrentPageIndex { get; set; }
    }
}
