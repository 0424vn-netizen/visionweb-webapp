using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Api.Model.Filter
{
    public interface IDateFilter
    {
        DateTime FromDateValidate { get; set; }

        DateTime ToDateValidate { get; set; }
    }
}
