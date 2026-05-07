using AS.Security.WS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Security.WS.Mobile
{
    public class MobileUser : User
    {
        public bool HasMobileAccess { get; set; }
        public bool IsChainHQ { get; set; }
        public string UserMode { get; set; }
        public string[] Permissions { get; set; }
    }
}
