using AS.VW.Api.Model.Security;
using AS.VW.Api.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Api.WCF.ServiceInterface
{
    public class WcfServiceAdapter: IServiceAdapter
    {
        private readonly IAuthenticatedService _AuthenService;

        public WcfServiceAdapter(IAuthenticatedService authenService)
        {
            _AuthenService = authenService;
        }

        public User GetUser()
        {
            return _AuthenService.LoggedInUser;
        }
    }
}
