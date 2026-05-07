using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Api.WCF.ServiceInterface
{
    public class GlobalExceptionHandler : IErrorHandler
    {
        public bool HandleError(System.Exception error)
        {
            ServiceLogger.Log.Error(error.Message);
            return true;
        }

        public void ProvideFault(System.Exception error, System.ServiceModel.Channels.MessageVersion version, ref System.ServiceModel.Channels.Message fault)
        {
            // Method intentionally left empty.
        }
    }
}
