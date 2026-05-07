using AS.Web.Business.Shared.Enums;
using System.Collections.Generic;

namespace AS.Web.Business.Shared.Models
{
    public class IsAllowClientModel
    {
        public int CurrentClientId { get; set; }
        public List<EnumClient> EnumClients { get; set; }
        public bool IsAll {  get; set; }

    }
}
