using System;

namespace AS.Web.Business.PCI.Models
{
    public class PciLogInfoRequest : BasePciModel
    {
        public Guid RequestId { get; set; }
        public string PciSsoUrl { get; set; }
        public string Message { get; set; }
    }
}
