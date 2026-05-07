using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AS.VW.Api.Model.Security;

namespace AS.VW.Api.Business.Repository
{
    public interface ISecurityRepository
    {
        AuthorizationResult ValidateApiToken(ApiAuthToken token, bool autoLoadUser);
        AuthorizationResult ValidateApiTokenAndLoadUser(ApiAuthToken token);        
        User GetUser(User loggedInUser);
        void InsertServiceRequestLog(ServiceRequestLog log);
        string GetUserPassword(ApiAuthToken token);
        void AddRequestHeader(string key, string value);
        DataTable GetValidateUser(ApiAuthToken token);
    }
}
