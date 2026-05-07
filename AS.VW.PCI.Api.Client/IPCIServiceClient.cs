using AS.VW.PCI.Api.Client.Models.Common;
using AS.VW.PCI.Api.Client.Models.Requests;
using AS.VW.PCI.Api.Client.Models.Responses;

namespace VW.PCI.Api.Client
{
    public interface IPCIServiceClient
    {
        IApiResponse<AuthTokenResponse> Authenticate(AuthTokenRequest request);

        //PCIApiResponse<CreateUserResponse> CreateUser(CreateUserRequest request);

        //PCIApiResponse<UpdateUserResponse> UpdateUser(UpdateUserRequest request);

        //PCIApiResponse<GetMasterMerchantResponse> GetMasterMerchant(GetMasterMerchantRequest request);

        //PCIApiResponse<GetHierarchyIDResponse> GetHierarchyID(GetHierarchyIDRequest request);

        //PCIApiResponse<UpdSecRoleByUserIDResponse> UpdSecRoleByUserID(UpdSecRoleByUserIDRequest request);

        //PCIApiResponse<UpdateOptInOutResponse> UpdateOptInOut(UpdateOptInOutRequest request);

        //PCIApiResponse<GetAllHierarchyForAOResponse> GetAllHierarchyForAO(GetAllHierarchyForAORequest request);

        IApiResponse<GetUsersResponse> GetUsers(GetUsersRequest request);
    }
}
