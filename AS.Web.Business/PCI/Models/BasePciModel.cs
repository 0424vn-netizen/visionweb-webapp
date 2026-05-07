using AS.Web.Business.Shared.Constants;
using System;

namespace AS.Web.Business.PCI.Models
{
    public class BasePciModel
    {
        public string ClientId { get; set; }
        private readonly string _clientName = PciConstants.SSO_CLIENT_NAME;
        public string ClientName { get => _clientName; }
        public string AsClientId { get; set; }
        public string CurrentUserId { get; set; }
        public Guid RequestId { get; set; }
    }
}
