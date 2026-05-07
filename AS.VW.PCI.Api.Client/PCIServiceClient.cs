using AS.VW.Api.RestClient;
using AS.VW.Api.RestClient.Models;
using AS.VW.PCI.Api.Client.Common;
using AS.VW.PCI.Api.Client.Models.Common;
using AS.VW.PCI.Api.Client.Models.Requests;
using AS.VW.PCI.Api.Client.Models.Responses;
using AS.VW.PCI.Api.Client.Providers;
using AS.VW.PCI.Api.Client.Settings;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace VW.PCI.Api.Client
{
    public class PCIServiceClient : VWRestClient, IPCIServiceClient
    {
        private const string ApiSource = "pci";
        private const string ApiSettingFile = "pciSettings.xml";

        private TokenProvider tokenProvider { get; set; }

        [ThreadStatic]
        private static int _currentApplicationId;

        private static ITokenRepository _tokenRepository;

        private static readonly Lazy<PCIServiceClient> _lazyInstance = new Lazy<PCIServiceClient>(() => new PCIServiceClient());

        public static PCIServiceClient Instance => _lazyInstance.Value;

        /// <summary>
        /// Call once at application startup before first use.
        /// </summary>
        public static void Configure(ITokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository ?? throw new ArgumentNullException(nameof(tokenRepository));
        }

        public PCIServiceClient()
            : base(VWLogger.Instance, ApiLoggingService.Instance, ApiSource, ApiSettingFile, PCIClientSettings.Instance)
        {
            if (_tokenRepository == null)
                throw new InvalidOperationException(
                    "PCIServiceClient has not been configured. Call PCIServiceClient.Configure(ITokenRepository) at application startup.");

            tokenProvider = new TokenProvider(base.Logger, this, _tokenRepository);
        }

        //public static void Configure(ILogger logger, ILoggingService loggingService)
        //{
        //    _sharedLogger = logger ?? throw new ArgumentNullException(nameof(logger));
        //    _sharedLoggingService = loggingService ?? throw new ArgumentNullException(nameof(loggingService));
        //}

        //public static IPCIServiceClient ForUser(AuthTokenRequest credentials)
        //{
        //    if (_sharedLogger == null)
        //    {
        //        throw new InvalidOperationException("PCIServiceClient has not been configured. Call PCIServiceClient.Configure() at application startup.");
        //    }

        //    if (credentials == null) throw new ArgumentNullException(nameof(credentials));

        //    return new PCIServiceClient(_sharedLogger, _sharedLoggingService, credentials);
        //}

        //private static readonly ConcurrentDictionary<string, PCITokenInfo> _tokenCache = new ConcurrentDictionary<string, PCITokenInfo>();

        //private static readonly object _lock = new object();

        //private readonly AuthTokenRequest _credentials;

        //private PCIServiceClient(ILogger logger, ILoggingService loggingService, AuthTokenRequest credentials)
        //    : base(logger, loggingService, ApiSource, ApiSettingFile, new RestClientSettings())
        //{
        //    _credentials = credentials;
        //}

        protected override void InterceptRequest(string trackingId, ApiSetting apiSetting, IRestRequest request)
        {
            if (apiSetting.Name == "auth/token") return;

            var token = tokenProvider.GetToken(_currentApplicationId);
            request.AddHeader("Authorization", $"Bearer {token.AccessToken}");
        }

        protected override void InterceptResponse(string trackingId, ApiSetting apiSetting, IRestRequest request, IRestResponse response, out bool shouldRetryPrevRequest)
        {
            shouldRetryPrevRequest = false;
        }

        //private string GetValidToken()
        //{
        //    var key = _credentials.ApplicationId.ToString();

        //    if (_tokenCache.TryGetValue(key, out var tokenInfo) && tokenInfo.IsValid())
        //    {
        //        return tokenInfo.AccessToken;
        //    }

        //    lock (_lock)
        //    {
        //        if (_tokenCache.TryGetValue(key, out tokenInfo) && tokenInfo.IsValid())
        //        {
        //            return tokenInfo.AccessToken;
        //        }

        //        Logger.Debug($"PCIServiceClient: Fetching new token for ApplicationId={key}...");

        //        var apiSetting = this.GetApiSetting("auth/token");
        //        var apiResponse = this.Post<AuthTokenRequest, PCIApiResponse<AuthTokenResponse>>(apiSetting, body: _credentials);
        //        var response = apiResponse?.Data;

        //        if (response == null || string.IsNullOrWhiteSpace(response.AccessToken))
        //        {
        //            throw new InvalidOperationException($"PCIServiceClient: Failed to retrieve access token for ApplicationId={key}.");
        //        }
                    
        //        var newToken = new PCITokenInfo
        //        {
        //            AccessToken = response.AccessToken,
        //            TokenType = response.TokenType,
        //            ExpireAt = DateTime.UtcNow.AddMinutes(response.ExpireMinutes - 1)
        //        };

        //        _tokenCache[key] = newToken;
        //        Logger.Debug($"PCIServiceClient: Token saved for ApplicationId={key}. Expires at {newToken.ExpireAt:O} UTC.");

        //        return newToken.AccessToken;
        //    }
        //}

        //public PCIApiResponse<CreateUserResponse> CreateUser(CreateUserRequest request)
        //    => Execute<CreateUserRequest, CreateUserResponse>("user/CreateUser", request);

        //public PCIApiResponse<UpdateUserResponse> UpdateUser(UpdateUserRequest request)
        //    => Execute<UpdateUserRequest, UpdateUserResponse>("user/UpdateUser", request);

        //public PCIApiResponse<GetMasterMerchantResponse> GetMasterMerchant(GetMasterMerchantRequest request)
        //    => Execute<GetMasterMerchantRequest, GetMasterMerchantResponse>("user/GetMasterMerchant", request);

        //public PCIApiResponse<GetHierarchyIDResponse> GetHierarchyID(GetHierarchyIDRequest request)
        //    => Execute<GetHierarchyIDRequest, GetHierarchyIDResponse>("user/GetHierarchyID", request);

        //public PCIApiResponse<UpdSecRoleByUserIDResponse> UpdSecRoleByUserID(UpdSecRoleByUserIDRequest request)
        //    => Execute<UpdSecRoleByUserIDRequest, UpdSecRoleByUserIDResponse>("user/UpdSecRoleByUserID", request);

        //public PCIApiResponse<UpdateOptInOutResponse> UpdateOptInOut(UpdateOptInOutRequest request)
        //    => Execute<UpdateOptInOutRequest, UpdateOptInOutResponse>("user/UpdateOptInOut", request);

        //public PCIApiResponse<GetAllHierarchyForAOResponse> GetAllHierarchyForAO(GetAllHierarchyForAORequest request)
        //    => Execute<GetAllHierarchyForAORequest, GetAllHierarchyForAOResponse>("user/GetAllHierarchyForAO", request);

        //public PCIApiResponse<GetUsersResponse> GetUsers(GetUsersRequest request)
        //    => Execute<GetUsersRequest, GetUsersResponse>("user/GetUsers", request);

        private IApiResponse<TResult> Execute<TBody, TResult>(
            int applicationId, string path, TBody body,
            IDictionary<string, string> headers = null)
            where TBody : class, new()
            where TResult : class, new()
        {
            _currentApplicationId = applicationId;
            try
            {
                return TryPost<TBody, TResult>(Utils.GetTrackingId(), GetApiSetting(path), body, null, headers);
            }
            finally
            {
                _currentApplicationId = 0;
            }
        }

        private PCIErrorResponse TryDeserializeError(string content)
        {
            try { return JsonConvert.DeserializeObject<PCIErrorResponse>(content); }
            catch { return null; }
        }

        public IApiResponse<AuthTokenResponse> Authenticate(AuthTokenRequest request)
        {
            var apiSetting = this.GetApiSetting("auth/token");
            var trackingId = Utils.GetTrackingId();

            var result = TryPost<AuthTokenRequest, AuthTokenResponse>(trackingId, apiSetting, request, null, null);

            return result;
        }

        private ApiResponse<TResult> TryPost<T, TResult>(string trackingId, ApiSetting apiSetting, T body, IDictionary<string, string> parameters, IDictionary<string, string> headers)
            where T : class, new()
            where TResult : class, new()
        {
            try
            {
                var response = this.Post<T, ApiWrapperResponse<TResult>>(apiSetting, body, parameters, headers, DataFormat.Json, trackingId);
                var unwrapped = response?.Data;
                return CreateApiResponse(trackingId, unwrapped);
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return ErrorResponse<TResult>(trackingId, exception);
            }
        }

        private ApiResponse<TResult> ErrorResponse<TResult>(string trackingId, Exception exception)
        {
            var responseStatusCode = HttpStatusCode.InternalServerError;
            var messageContent = "An error occurred.";

            if (exception is ApiException)
            {
                var apiException = exception as ApiException;
                responseStatusCode = apiException.StatusCode;
                messageContent = apiException.Message;
            }

            var message = new ApiMessage
            {
                Code = responseStatusCode.ToString(),
                MessageCode = (int)responseStatusCode,
                Description = messageContent
            };

            Logger.Debug($"ErrorResponse:: ATG Service client : response status code {responseStatusCode}");

            return new ApiResponse<TResult>
            {
                Data = default(TResult),
                Messages = new List<ApiMessage> { message },
                Status = ApiResponseStatus.Error,
                StatusCode = responseStatusCode,
                TrackingId = trackingId
            };
        }

        private ApiResponse<TResult> CreateApiResponse<TResult>(string trackingId, TResult response)
        {
            if (response == null)
            {
                Logger.Debug("ATG Service client : response is null, status code is 403");
                return new ApiResponse<TResult>
                {
                    Status = ApiResponseStatus.Error,
                    StatusCode = HttpStatusCode.Forbidden,
                    TrackingId = trackingId,
                    Messages = new List<ApiMessage>()
                };
            }

            return new ApiResponse<TResult>
            {
                Status = ApiResponseStatus.Success,
                StatusCode = HttpStatusCode.OK,
                Data = response,
                TrackingId = trackingId,
                Messages = null
            };
        }

        public IApiResponse<CreateUserResponse> CreateUser(int applicationId, CreateUserRequest request)
        {
            Logger.Debug($"CreateUser::Start. ApplicationId={applicationId}, Request={JsonConvert.SerializeObject(request)}");
            var result = Execute<CreateUserRequest, CreateUserResponse>(applicationId, "user/CreateUser", request);
            Logger.Debug($"CreateUser::End. Response={JsonConvert.SerializeObject(result.Data)}");
            return result;
        }

        public IApiResponse<UpdateUserResponse> UpdateUser(int applicationId, UpdateUserRequest request)
        {
            Logger.Debug($"UpdateUser::Start. ApplicationId={applicationId}, Request={JsonConvert.SerializeObject(request)}");
            var result = Execute<UpdateUserRequest, UpdateUserResponse>(applicationId, "user/UpdateUser", request);
            Logger.Debug($"UpdateUser::End. Response={JsonConvert.SerializeObject(result.Data)}");
            return result;
        }

        public IApiResponse<GetMasterMerchantResponse> GetMasterMerchant(int applicationId, GetMasterMerchantRequest request)
        {
            Logger.Debug($"GetMasterMerchant::Start. ApplicationId={applicationId}, Request={JsonConvert.SerializeObject(request)}");
            var result = Execute<GetMasterMerchantRequest, GetMasterMerchantResponse>(applicationId, "user/GetMasterMerchant", request);
            Logger.Debug($"GetMasterMerchant::End. Response={JsonConvert.SerializeObject(result.Data)}");
            return result;
        }

        public IApiResponse<List<HierarchyIDItem>> GetHierarchyID(int applicationId, GetHierarchyIDRequest request)
        {
            Logger.Debug($"GetHierarchyID::Start. ApplicationId={applicationId}, Request={JsonConvert.SerializeObject(request)}");
            var result = Execute<GetHierarchyIDRequest, List<HierarchyIDItem>>(applicationId, "user/GetHierarchyID", request);
            Logger.Debug($"GetHierarchyID::End. Response={JsonConvert.SerializeObject(result.Data)}");
            return result;
        }

        public IApiResponse<UpdSecRoleByUserIDResponse> UpdSecRoleByUserID(int applicationId, UpdSecRoleByUserIDRequest request)
        {
            Logger.Debug($"UpdSecRoleByUserID::Start. ApplicationId={applicationId}, Request={JsonConvert.SerializeObject(request)}");
            var result = Execute<UpdSecRoleByUserIDRequest, UpdSecRoleByUserIDResponse>(applicationId, "user/UpdSecRoleByUserID", request);
            Logger.Debug($"UpdSecRoleByUserID::End. Response={JsonConvert.SerializeObject(result.Data)}");
            return result;
        }

        public IApiResponse<UpdateOptInOutResponse> UpdateOptInOut(int applicationId, UpdateOptInOutRequest request)
        {
            Logger.Debug($"UpdateOptInOut::Start. ApplicationId={applicationId}, Request={JsonConvert.SerializeObject(request)}");
            var result = Execute<UpdateOptInOutRequest, UpdateOptInOutResponse>(applicationId, "user/UpdateOptInOut", request);
            Logger.Debug($"UpdateOptInOut::End. Response={JsonConvert.SerializeObject(result.Data)}");
            return result;
        }

        public IApiResponse<List<HierarchyForAOItem>> GetAllHierarchyForAO(int applicationId, GetAllHierarchyForAORequest request)
        {
            Logger.Debug($"GetAllHierarchyForAO::Start. ApplicationId={applicationId}, Request={JsonConvert.SerializeObject(request)}");
            var result = Execute<GetAllHierarchyForAORequest, List<HierarchyForAOItem>>(applicationId, "user/GetAllHierarchyForAO", request);
            Logger.Debug($"GetAllHierarchyForAO::End. Response={JsonConvert.SerializeObject(result.Data)}");
            return result;
        }

        public IApiResponse<GetUsersResponse> GetUsers(int applicationId, GetUsersRequest request)
        {
            Logger.Debug($"GetUsers::Start. ApplicationId={applicationId}, Request={JsonConvert.SerializeObject(request)}");
            var result = Execute<GetUsersRequest, GetUsersResponse>(applicationId, "user/GetUsers", request);
            Logger.Debug($"GetUsers::End. Response={JsonConvert.SerializeObject(result.Data)}");
            return result;
        }
    }
}
