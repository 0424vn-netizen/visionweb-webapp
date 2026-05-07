using AS.VW.Api.Model.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Api.Business
{
    public interface IServiceAdapter
    {
        User GetUser();
    }
}
