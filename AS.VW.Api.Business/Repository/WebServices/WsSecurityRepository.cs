using AS.VW.Api.Model.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Api.Business.Repository.WebService
{
    public class WsSecurityRepository : ISecurityRepository
    {        
        private ApiWebServices _authWebService = null;
                
        public WsSecurityRepository()
        {
            _authWebService = new ApiWebServices();
        }

        public AuthorizationResult ValidateApiToken(ApiAuthToken token, bool autoLoadUser)
        {
            return _authWebService.ValidateApiToken(token, autoLoadUser);
        }

        public AuthorizationResult ValidateApiTokenAndLoadUser(ApiAuthToken token)
        {
            return ValidateApiToken(token, true);
        }

        public User GetUser(User loggedInUser)
        {
            throw new NotImplementedException();
        }

        public DataTable GetValidateUser(ApiAuthToken token)
        {
            return _authWebService.GetValidateUser(token);
        }

        public void InsertServiceRequestLog(ServiceRequestLog log)
        {
            //throw new NotImplementedException();
        }

        public string GetUserPassword(ApiAuthToken token)
        {
            return _authWebService.GetUserPassword(token);
        }


        public void AddRequestHeader(string key, string value)
        {
            _authWebService.AddRequestHeader(key, value);
        }
    }
}
