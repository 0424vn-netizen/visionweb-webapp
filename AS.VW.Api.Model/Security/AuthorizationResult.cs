using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AS.VW.Api.Model.Security;

namespace AS.VW.Api.Model.Security
{
    public class AuthorizationResult
    {
        public LoginActions Result { get; set; }
        public User AuthorizedUser { get; set; }
        public List<string> Permissions { get; set; }
        public List<string> Roles { get; set; }
    }
}
