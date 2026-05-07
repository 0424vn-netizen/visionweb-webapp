using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AS.VW.Api.Model.Security;

namespace AS.VW.Api.Business.Repository
{
    public class AuthenticatedRepositoryBase : IAuthenticatedRepository
    {
        public User PrincipalUser
        {
            get
            {
                if (this.ServiceAdapter == null)
                {
                    return null;
                }

                return ServiceAdapter.GetUser();
            }
        }
        public int ClientId { get; set; }
        public IServiceAdapter ServiceAdapter { get; set; }


        public virtual void SetClientId(string clientId)
        {
            
        }
    }
}
