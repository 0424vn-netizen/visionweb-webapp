using AS.NetCore.Api.Client.BaseClient;
using AS.NetCore.Api.Client.Models;

namespace AS.NetCore.Api.Client
{
    public class FapsApiClient : BaseApiClient
    {
        public FapsApiClient() { }

        public bool UpdateChainNumber(BaseRequestData<UpdateChainNumberRequest> request)
        {
            var response = Execute<BaseResponseData<UpdateChainNumberResponse>>("UpdateChainNumber", request, "POST");
            return response != null && response.Data != null && response.Data.IsSuccess;
        }
    }
}
