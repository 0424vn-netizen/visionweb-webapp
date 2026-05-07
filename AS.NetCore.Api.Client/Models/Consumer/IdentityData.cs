using System;
using System.Collections.Generic;

namespace AS.NetCore.Api.Client.Models
{
    public class IdentityData
    {
        public string AgentId { get; set; }

        public string AgentName { get; set; }

        public string Email { get; set; }
        public string UserName { get; set; }

        public string WsToken { get; set; }

        public string WsRefreshToken { get; set; }

        public string Source { get; set; } //SsoSource

        public string SessionWorkingId { get; set; }

        public dynamic ApiHeader { get; set; }

        public string[] Permissions { get; set; }

        public Dictionary<string, string> AdditionalData { get; set; }

    }
}
