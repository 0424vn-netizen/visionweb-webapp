using AS.NetCore.Api.Client.Models;

namespace AS.NetCore.Api.Client.BaseClient
{
    public class BaseApiClient 
    {
        protected readonly ApiConsumer _apiConsumer;
        protected IdentityData _currentUserData;

        protected BaseApiClient()
        {
            AppSetting appSetting = new AppSetting();
            _apiConsumer = new ApiConsumer(appSetting);
        }

        protected virtual dynamic PrepareRequest(RequestWrapper request)
        {
            var parameters = request.RequestData.ToDynamic();
            BindHeader(parameters);
            return parameters;
        }

        protected virtual void BindHeader(dynamic p)
        {
            if (_currentUserData != null)
            {
                p.header = _currentUserData.ApiHeader;
            }
        }

        protected T Execute<T>(string endpointKey, DataModel requestData, string method = "POST") where T : DataModel, new()
        {
            var request = new RequestWrapper()
            {
                EndpointKey = endpointKey,
                RequestData = requestData,
                Method = method
            };

            var parameters = PrepareRequest(request);
            return _apiConsumer.Execute<T>(endpointKey, method, parameters);
        }
    }
}
