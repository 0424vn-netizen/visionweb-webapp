using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace AS.VW.Api.Model
{
    [DataContract]
    public class Pagination
    {
        [DataMember(Order = 01)]
        public int CurrentPageIndex { get; set; } 
       
        [DataMember(Order = 02)]
        public int PageSize { get; set; } 
   
        [DataMember(Order = 03)]
        public int NumberOfRecords { get; set; }
    }
}
