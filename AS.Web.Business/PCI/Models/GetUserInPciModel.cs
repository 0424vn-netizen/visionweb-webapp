using System;
using System.Data;

namespace AS.Web.Business.PCI.Models
{
    public class GetUserInPciInfo
    {
        public int AsClientId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public DataTable UserInfo { get; set; }
    }
}
