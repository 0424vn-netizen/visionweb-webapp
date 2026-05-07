using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace AS.VW.Api.Model.Security
{    
    
    public class ApiAuthToken
    {        
        public int AsClientId { get; set; }
   
        public string ApiClientId { get; set; }

        public string ApiKey { get; set; }
        
        public string RemoteIp { get; set; }
        
        public string ApiVersion { get; set; }
    }
}
