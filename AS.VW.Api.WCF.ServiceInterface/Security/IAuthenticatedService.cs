using AS.VW.Api.Model.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Api.WCF.ServiceInterface
{
    public interface IAuthenticatedService
    {       
        User LoggedInUser { get; set; }

        ApiAuthToken AuthToken { get; set; } 

        ServiceRequestLog LogTracking { get; set; }
    }
}
