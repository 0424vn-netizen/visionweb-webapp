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
    public class GenericReportFilterNoViewLevel : BaseFilter, IHierarchyFilter, IDateFilter, IPagingFilter
    {
        [DataMember]
        public string FromDate { get; set; }

        [DataMember]
        public string ToDate { get; set; }

        //[DataMember]
        public DateTime FromDateValidate
        {
            get
            {
                if (string.IsNullOrEmpty(FromDate))
                    return DateTime.Now;
                return DateTime.Parse(FromDate);
            }
            set { FromDate = value.ToString(); }
        }

        //[DataMember]
        public DateTime ToDateValidate
        {
            get
            {
                if (string.IsNullOrEmpty(FromDate))
                    return DateTime.Now;
                return DateTime.Parse(ToDate);
            }
            set { ToDate = value.ToString(); }
        }

        [DataMember]
        public string HierarchyFilterMode { get; set; }

        [DataMember]
        public string HierarchyFilterValue { get; set; }

        [DataMember]
        public int PageSize { get; set; }

        [DataMember]
        public int CurrentPageIndex { get; set; }
    }
}
