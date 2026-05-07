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
    public class CardSummaryFilter : GenericReportFilterNoViewLevel
    {
        [DataMember]
        public string BatchNumber { get; set; }
    }
}
