using AS.Core.Common.Log;
using RestSharp;
using System;
using System.Net;

namespace AS.ApiClient.UnderWriting.BaseClient
{
    public class BaseApiClient : RestClient
    {
        public BaseApiClient(string baseUrl)
        {
            BaseUrl = new Uri(baseUrl);
        }

        public override IRestResponse<T> Execute<T>(IRestRequest request)
        {
            BeforeRequest(request);
            IRestResponse<T> response = default;
            try
            {
                var response2 = base.Execute<T>(request);
                return response2;
            }
            catch (Exception ex)
            {
                Logger.Error("ERROR Under Writing Api", ex);
            }           

            AfterRequest(response);
            return response;
        }

        public virtual T Get<T>(IRestRequest request) where T : new()
        {
            var response = Execute<T>(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return response.Data;
            }
            return default(T);
        }
        protected virtual IRestRequest CreateRequest(string resource, Method method)
        {
            IRestRequest request = new RestRequest(resource, method);
            CustomRequest(request);

            return request; 
        }

        protected virtual void CustomRequest(IRestRequest request) { }
        protected virtual void BeforeRequest(IRestRequest request) { }
        protected virtual void AfterRequest(IRestResponse response) { }
    }
}
