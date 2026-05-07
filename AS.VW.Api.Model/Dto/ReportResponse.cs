using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace AS.VW.Api.Model.Dto
{
    [DataContract]
    public class ReportResponse<T>
    {
        [DataMember(Order = 01)]
        public List<T> Result { get; set; }

        [DataMember(Order = 02)]
        public T Total { get; set; }

        [DataMember(Order = 03)]
        public Pagination Paging { get; set; }

        [DataMember(Order = 04)]
        public ErrorModel Validation { get; set; }
    }

    public class ErrorModel
    {
        public List<string> Messages { get; set; }

        public string Status { get; set; }
    }
}
