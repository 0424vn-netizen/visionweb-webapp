using AS.VW.Api.RestClient;
using AS.VW.PCI.Api.Client.Models.Requests;
using AS.VW.PCI.Api.Client.Models.Responses;
using AS.VW.PCI.Api.Client.Settings;
using System;
using VW.PCI.Api.Client;

namespace AS.VW.PCI.Api.Client.Providers
{
    public class TokenProvider
    {
        protected readonly ILogger Logger;
        protected readonly IPCIServiceClient ServiceClient;

        public TokenProvider(ILogger logger, IPCIServiceClient serviceClient)
        {
            Logger = logger;
            ServiceClient = serviceClient;
        }

        public AuthTokenResponse GetToken(int applicationId)
        {
            LogDebug("GetToken::Start function");

            var config = PCIClientConfigProvider.Instance.GetConfig(applicationId);
            var authRequest = new AuthTokenRequest
            {
                ApplicationId = config.ApplicationId,
                ApplicationName = config.ApplicationName,
                ApplicationCode = config.ApplicationCode
            };

            var token = GetAuthenApi(authRequest);

            LogDebug($"GetToken::End function. TokenType={token.TokenType}, ExpireMinutes={token.ExpireMinutes}");
            return token;
        }

        private AuthTokenResponse GetAuthenApi(AuthTokenRequest request)
        {
            LogDebug("GetAuthenApi::Start function.");

            var authenticateRs = ServiceClient.Authenticate(request);

            if (authenticateRs?.Data == null)
                throw new InvalidOperationException(
                    $"Authentication failed for ApplicationId={request.ApplicationId}: empty response.");

            LogDebug("GetAuthenApi::End function.");

            return new AuthTokenResponse
            {
                AccessToken = authenticateRs.Data.AccessToken,
                ExpireMinutes = authenticateRs.Data.ExpireMinutes,
                TokenType = authenticateRs.Data.TokenType
            };
        }

        private void LogDebug(string message)
        {
            Logger.Debug($"TokenProvider::{message}");
        }
    }
}
