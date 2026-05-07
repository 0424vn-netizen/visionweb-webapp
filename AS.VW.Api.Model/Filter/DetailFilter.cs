using AS.VW.Api.Model.Security;
using System;
using System.Runtime.Serialization;

namespace AS.VW.Api.Model.Filter
{
    [DataContract]
    public class DetailFilter: BaseFilter, IMerchantNumberFilter
    {
        [DataMember]
        public string MerchantNumber { get; set; }

        [DataMember]
        public string ReportDate { get; set; }

        //[DataMember]
        public DateTime ReportDateValidate
        {
            get
            {
                if (string.IsNullOrEmpty(ReportDate))
                    return DateTime.Today.AddDays(-1);
                return DateTime.Parse(ReportDate);
            }
            set { ReportDate = value.ToString(); }
        }
    }
}
