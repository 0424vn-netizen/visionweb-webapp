using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Api.Model.Filter
{
    public interface IHierarchyFilter
    {
        string HierarchyFilterMode { get; set; }

        string HierarchyFilterValue { get; set; }
    }
}
