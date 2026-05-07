using AS.VW.Api.Model.Security;
using AS.VW.Api.Business.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Dispatcher;
using System.Web;

namespace AS.VW.Api.Business.WCF.Utilities
{
    public class SecurityParameterInspector : IParameterInspector
    {
        private readonly ISecurityRepository _SecurityRepository;

        public SecurityParameterInspector(ISecurityRepository securityRepository)
        {
            _SecurityRepository = securityRepository;
        }

        public void AfterCall(string operationName, object[] outputs, object returnValue, object correlationState)
        {
 	        throw new NotImplementedException();
        }

        public object BeforeCall(string operationName, object[] inputs)
        {
            foreach (object input in inputs.Where(x => x is ApiAuthToken))
            {
                _SecurityRepository.ValidateApiToken((ApiAuthToken)input, true);
            }
            return null;
        }
    }
}