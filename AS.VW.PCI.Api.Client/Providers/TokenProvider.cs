using AS.VW.Api.RestClient;
using AS.VW.PCI.Api.Client.Models.Requests;
using AS.VW.PCI.Api.Client.Models.Responses;
using Newtonsoft.Json;
using System.Threading.Tasks;
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

        public Task<AuthTokenResponse> GetToken(AuthTokenRequest request)
        {
            LogDebug("GetToken:: Start function");

            var token = GetAuthenApi(request).Result;

            LogDebug($"GetToken:: End function. {JsonConvert.SerializeObject(token)}");
            return Task.FromResult(token);
        }

        private Task<AuthTokenResponse> GetAuthenApi(AuthTokenRequest request)
        {
            LogDebug("GetAuthenApi::Start function.");

            // call API Authen        
            var authenticateRs = ServiceClient.Authenticate(request);

            var result = new AuthTokenResponse
            {
                AccessToken = authenticateRs.Data.AccessToken,
                ExpireMinutes = authenticateRs.Data.ExpireMinutes,
                TokenType = authenticateRs.Data.TokenType,
            };

            LogDebug("GetAuthenApi::Start function.");

            return Task.FromResult(result);
            
        }
        
        private void LogDebug(string messsage)
        {
            Logger.Debug($"TokenProvider::{messsage}");
        }
    }
}
