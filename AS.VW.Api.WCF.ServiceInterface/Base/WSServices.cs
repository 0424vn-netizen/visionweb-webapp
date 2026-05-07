using AS.VW.Api.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Api.WCF.ServiceInterface.Base
{
    
    public class WSServices
    {

        public static ApiWebServices ApiServices
        {
            get
            {
                ApiWebServices apiServices = new ApiWebServices();
                apiServices.AddRequestHeader("ClientId", SessionManager.CurrentClient.ToString());
                return apiServices;
            }
        }
    }
   
}
