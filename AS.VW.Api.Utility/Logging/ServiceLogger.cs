using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Api.Utility.Logging
{
    public static class ServiceLogger
    {
        public static readonly ILog Log = LogManager.GetLogger(typeof(ServiceLogger));
    }
}
