using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Api.Model.Filter
{
    public interface IDataViewLevelFilter
    {
        DataViewLevel ViewLevelValidate {get;set;}
    }
}
