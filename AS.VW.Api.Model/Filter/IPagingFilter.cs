using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Api.Model.Filter
{
    public interface IPagingFilter
    {
        int PageSize {get;set;}

        int CurrentPageIndex { get; set; }
    }
}
