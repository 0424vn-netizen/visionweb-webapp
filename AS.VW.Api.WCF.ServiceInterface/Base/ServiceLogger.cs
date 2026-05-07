using AS.Common.Logger;
using log4net;
using log4net.Config;

namespace AS.VW.Api.WCF.ServiceInterface
{
    public static class ServiceLogger
    {
        public static ILog Log { get; private set; }
        static ServiceLogger()
        {
            Log = LoggerManager.GetLogger(typeof(ServiceLogger));
        }
    }
}
