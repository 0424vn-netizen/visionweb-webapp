using System;

namespace AS.VW.PCI.Api.Client.Common
{
    public class TokenCacheEntry
    {
        public string AccessToken { get; set; }

        public string TokenType { get; set; }

        public DateTime ExpireAt { get; set; }

        public bool IsValid() => DateTime.UtcNow < ExpireAt;
    }
}
