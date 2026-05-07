using Ninject.Modules;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AS.VW.Api.Business.Repository;
using AS.VW.Api.Business.Repository.WebService;
namespace AS.VW.Api.WCF.ServiceInterface
{
    public class WcfServiceModule : NinjectModule
    {
        private static WcfServiceModule _Instance = null;
        public static WcfServiceModule Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new WcfServiceModule();
                }
                return _Instance;
            }
        }

        private WcfServiceModule() { }
      
        public T Resolve<T>()
        {
            return this.Kernel.Get<T>();
        }

        public override void Load()
        {
            //Bind all repositories
            Bind<ISecurityRepository>().To<WsSecurityRepository>();
            Bind<IAuthorizationRepository>().To<WsAuthorizationRepository>();
            Bind<IPaymentRepository>().To<WsPaymentRepository>();
            Bind<IRetrievalRepository>().To<WsRetrievalRepository>();
            Bind<IChargebackRepository>().To<WsChargebackRepository>();
            Bind<IBatchRepository>().To<WsBatchRepository>();
            Bind<IReturnRepository>().To<WsReturnRepository>();
            Bind<IVoidsRejectRepository>().To<WsVoidsRejectRepository>();
            Bind<ITransactionRepository>().To<WsTransactionRepository>();
            Bind<IMerchantInformationRepository>().To<WsMerchantInfomationRepository>(); 
        }
    }
}