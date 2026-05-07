using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AS.VW.Api.Model.Security;

namespace AS.VW.Api.Business.Repository
{
    public interface IAuthenticatedRepository
    {
        User PrincipalUser { get; }
        IServiceAdapter ServiceAdapter { get; set; }        
        void SetClientId(string clientId);
    }
}
