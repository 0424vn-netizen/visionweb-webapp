using AS.VW.Api.WCF.ServiceInterface;
using Ninject;
using Ninject.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace AS.VW.Api.Business.WCF
{
    public class Global : NinjectHttpApplication
    {
        protected override IKernel CreateKernel()
        {
            return new StandardKernel(WcfServiceModule.Instance);
        }

        public override void Init()
        {
            base.Init();
            AS.VW.Api.Business.Extensions.InitAutoMapper();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception error = Server.GetLastError();
            if (error.InnerException != null) 
                error = error.InnerException;
            ServiceLogger.Log.Error(error);
        }
    }
}