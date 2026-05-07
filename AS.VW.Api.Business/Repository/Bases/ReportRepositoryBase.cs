using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AS.VW.Api.Model.Security;

namespace AS.VW.Api.Business.Repository
{
    public class ReportRepositoryBase : AuthenticatedRepositoryBase
    {
        public ApiWebServices _apiWebService;

        public ReportRepositoryBase()
        {
            _apiWebService = new ApiWebServices();
            //To Do check AsClientId from where
            //_apiWebService.AddRequestHeader("ClientId", ClientID.ToString());
        }

        public override void SetClientId(string clientId)
        {
            _apiWebService.AddRequestHeader("ClientId", clientId);
        }
    }
}
