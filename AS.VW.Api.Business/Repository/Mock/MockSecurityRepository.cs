using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AS.VW.Api.Model.Security;

namespace AS.VW.Api.Business.Repository.Mock
{
    public class MockSecurityRepository : ISecurityRepository
    {
        public AuthorizationResult ValidateApiToken(ApiAuthToken token, bool autoLoadUser)
        {
            if (token != null)
            {
                return new AuthorizationResult()
                {
                    Result = LoginActions.Success,
                    AuthorizedUser = new User()
                    {
                        AsClientId = 64,
                        SiteId = 1225,
                        UserId = "merit75201",
                        EntityId = "merit75201",
                        EntityTypeId = 1,
                        UserFullName = "My Test Client"
                    }
                };
            }
            else
            {
                return new AuthorizationResult()
                {
                    Result = LoginActions.Fail
                };
            }
        }

        public AuthorizationResult ValidateApiTokenAndLoadUser(ApiAuthToken token)
        {
            return ValidateApiToken(token, true);
        }

        public User GetUser(User loggedInUser)
        {
            return new User()
            {
                AsClientId = 64,
                SiteId = 1225,
                UserId = "merit75201",
                EntityId = "merit75201",
                EntityTypeId = 1
            };
        }

        public DataTable GetValidateUser(ApiAuthToken token)
        {
            return new DataTable();
        }

        public void InsertServiceRequestLog(ServiceRequestLog log)
        {
            // call web service here
        }


        public string GetUserPassword(ApiAuthToken token)
        {
            throw new NotImplementedException();
        }


        public void AddRequestHeader(string key, string value)
        {
            throw new NotImplementedException();
        }
    }
}
