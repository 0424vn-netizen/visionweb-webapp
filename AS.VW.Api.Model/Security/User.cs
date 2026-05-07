using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Api.Model.Security
{
    public class User
    {
        public int AsClientId { get; set; }
        public int SiteId { get; set; }
        public Guid UserRecId { get; set; }
        public string UserId { get; set; }
        public int EntityTypeId { get; set; }
        public string EntityId { get; set; }
        public string UserFullName { get; set; }
    }
}
