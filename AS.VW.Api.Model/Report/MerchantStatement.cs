using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Api.Model.Report
{
    [DataContract]
    public class MerchantStatement
    {
        [DataMember(Order = 1)]
        public string MerchantNumber { get; set; }


        [DataMember(Order = 2)]
        public string StatementId { get; set; }

        [DataMember(Order = 3)]
        public string StatementData { get; set; }
    }
}
