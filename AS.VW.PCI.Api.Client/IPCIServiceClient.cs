using AS.VW.PCI.Api.Client.Models.Common;
using AS.VW.PCI.Api.Client.Models.Requests;
using AS.VW.PCI.Api.Client.Models.Responses;
using System.Collections.Generic;

namespace VW.PCI.Api.Client
{
    public interface IPCIServiceClient
    {
        IApiResponse<AuthTokenResponse> Authenticate(AuthTokenRequest request);

        IApiResponse<CreateUserResponse> CreateUser(int applicationId, CreateUserRequest request);

        IApiResponse<UpdateUserResponse> UpdateUser(int applicationId, UpdateUserRequest request);

        IApiResponse<GetMasterMerchantResponse> GetMasterMerchant(int applicationId, GetMasterMerchantRequest request);

        IApiResponse<List<HierarchyIDItem>> GetHierarchyID(int applicationId, GetHierarchyIDRequest request);

        IApiResponse<UpdSecRoleByUserIDResponse> UpdSecRoleByUserID(int applicationId, UpdSecRoleByUserIDRequest request);

        IApiResponse<UpdateOptInOutResponse> UpdateOptInOut(int applicationId, UpdateOptInOutRequest request);

        IApiResponse<List<HierarchyForAOItem>> GetAllHierarchyForAO(int applicationId, GetAllHierarchyForAORequest request);

        IApiResponse<GetUsersResponse> GetUsers(int applicationId, GetUsersRequest request);
    }
}
