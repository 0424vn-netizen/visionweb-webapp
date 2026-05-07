using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Api.WCF.ServiceInterface
{
    public class BaseService 
    {
        public string CurrentApiVersion
        {
            get
            {
                return "1.0";
            }
        }
    }
}
