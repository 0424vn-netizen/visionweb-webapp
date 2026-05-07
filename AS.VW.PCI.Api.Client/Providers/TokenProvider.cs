using AS.VW.Api.RestClient;
using AS.VW.PCI.Api.Client.Models.Requests;
using AS.VW.PCI.Api.Client.Models.Responses;
using AS.VW.PCI.Api.Client.Settings;
using System;
using System.Collections.Concurrent;
using VW.PCI.Api.Client;

namespace AS.VW.PCI.Api.Client.Providers
{
    public class TokenProvider
    {
        protected readonly ILogger Logger;
        protected readonly IPCIServiceClient ServiceClient;
        private readonly ITokenRepository _repository;

        // One lock object per applicationId to avoid blocking unrelated clients
        private static readonly ConcurrentDictionary<int, object> _lockMap =
            new ConcurrentDictionary<int, object>();

        public TokenProvider(ILogger logger, IPCIServiceClient serviceClient, ITokenRepository repository)
        {
            Logger = logger;
            ServiceClient = serviceClient;
            _repository = repository;
        }

        public AuthTokenResponse GetToken(int applicationId)
        {
            LogDebug($"GetToken::Start. ApplicationId={applicationId}");

            // Fast path — check DB without acquiring lock
            var cached = _repository.GetToken(applicationId);
            if (cached != null && cached.IsValid())
            {
                LogDebug("GetToken::Cache hit.");
                return ToResponse(cached);
            }

            // Slow path — one lock per applicationId (double-checked)
            var lockObj = _lockMap.GetOrAdd(applicationId, _ => new object());
            lock (lockObj)
            {
                cached = _repository.GetToken(applicationId);
                if (cached != null && cached.IsValid())
                {
                    LogDebug("GetToken::Cache hit after lock.");
                    return ToResponse(cached);
                }

                LogDebug("GetToken::Token missing or expired — fetching from API.");
                var token = FetchAndSave(applicationId);
                LogDebug("GetToken::End.");
                return token;
            }
        }

        private AuthTokenResponse FetchAndSave(int applicationId)
        {
            var config = PCIClientConfigProvider.Instance.GetConfig(applicationId);
            var authRequest = new AuthTokenRequest
            {
                ApplicationId   = config.ApplicationId,
                ApplicationName = config.ApplicationName,
                ApplicationCode = config.ApplicationCode
            };

            var authenticateRs = ServiceClient.Authenticate(authRequest);

            if (authenticateRs?.Data == null)
                throw new InvalidOperationException(
                    $"Authentication failed for ApplicationId={applicationId}: empty response.");

            // Subtract 1 minute as safety buffer before actual expiry
            var expireAt = DateTime.UtcNow.AddMinutes(authenticateRs.Data.ExpireMinutes - 1);
            _repository.SaveToken(applicationId, authenticateRs.Data.AccessToken, authenticateRs.Data.TokenType, expireAt);

            LogDebug($"FetchAndSave::Token saved. ExpireAt={expireAt:O} UTC.");

            return new AuthTokenResponse
            {
                AccessToken    = authenticateRs.Data.AccessToken,
                TokenType      = authenticateRs.Data.TokenType,
                ExpireMinutes  = authenticateRs.Data.ExpireMinutes
            };
        }

        private AuthTokenResponse ToResponse(Common.TokenCacheEntry cached) =>
            new AuthTokenResponse { AccessToken = cached.AccessToken, TokenType = cached.TokenType };

        private void LogDebug(string message) => Logger.Debug($"TokenProvider::{message}");
    }
}
