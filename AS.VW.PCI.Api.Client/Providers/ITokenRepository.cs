using AS.VW.PCI.Api.Client.Common;
using System;

namespace AS.VW.PCI.Api.Client.Providers
{
    public interface ITokenRepository
    {
        TokenCacheEntry GetToken(int applicationId);

        void SaveToken(int applicationId, string accessToken, string tokenType, DateTime expireAt);
    }
}
