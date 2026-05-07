using AS.ApiClient.UnderWriting.BaseClient;
using AS.ApiClient.UnderWriting.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using AS.Core.Common.Log;

namespace AS.ApiClient.UnderWriting 
{
    public class UnderWritingClient : BaseApiClient
    {
        static UnderWritingClient()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return Settings.ServerCertificateValidation; };
        }
        public UnderWritingClient()
           : base(Settings.ApiUrl)
        {
            FollowRedirects = true;            
        }
        protected override void CustomRequest(IRestRequest request)
        {
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", Settings.ApiToken);           
        }

        private string GetEndpointApi(string method)
        {
            var configs = Settings.GetApiConfigFile();
            if (configs != null)
            {
                var apiEndpoint = configs.FirstOrDefault(x => x.Code.Equals(method, StringComparison.OrdinalIgnoreCase));
                if (apiEndpoint != null)
                {
                    return apiEndpoint.Name;
                }
            }
            return string.Empty;
        }

        public List<ApproveGroup> GetApproveGroup(GetApproveGroupRequest requestData, string method)
        {
            var endpointUrl = GetEndpointApi(method);
            var request = CreateRequest(endpointUrl, Method.POST);            
            request.AddJsonBody(requestData);
            var response = Execute<BaseResponseData<ApproveGroupData>>(request);

            if (response != null && response.Data != null && response.Data.Data != null)
            {
                return response.Data.Data.ApproveGroups;
            }

            return new List<ApproveGroup>();
        }

        public bool UpdateApproverGroupMember(UpdateApproveGroupRequest requestData, string method)
        {
            var endpointUrl = GetEndpointApi(method);
            var request = CreateRequest(endpointUrl, Method.POST);
            request.AddJsonBody(requestData);
            var response = Execute<BaseResponseData<BaseResponse>>(request);
            if (response != null && response.Data != null && response.Data.Data != null)
                return true;
            return false;
        }

        public List<ApproveGroupsOfMember> GetApproveGroupsOfMember(GetApproveGroupRequest requestData)
        {
            var allGroup = GetApproveGroup(requestData, "GetApproverGroups");
            var groupsOfMember = GetApproveGroup(requestData, "GetApproverGroupsByUser");

            if (allGroup != null && allGroup.Any())
            {
                var datas = new List<ApproveGroupsOfMember>();
                foreach (var item in allGroup)
                {
                    datas.Add(new ApproveGroupsOfMember()
                    {
                        Id = item.Id,
                        ApproverGroupName = item.ApproverGroupName,
                        IsAssigned = groupsOfMember.FirstOrDefault(x => x.Id == item.Id) != null
                    });
                }

                return datas;
            }

            return new List<ApproveGroupsOfMember>();
        }
        public bool CreateUpdateDocumentType(DocumentTypeRequest requestData, string method)
        {
            var endpointUrl = GetEndpointApi(method);
            var request = CreateRequest(endpointUrl, Method.POST);
            request.AddJsonBody(requestData);
            var response = Execute<BaseResponseData<BaseResponse>>(request);
            if (response != null && response.Data != null && response.Data.Data != null)
                return true;
            return false;
        }

        public void UpdateFullNameForApproverGroup(UpdateFullNameForAPRequest requestData, string method)
        {
            var endpointUrl = GetEndpointApi(method);
            var request = CreateRequest(endpointUrl, Method.POST);
            request.AddJsonBody(requestData);
            Execute<BaseResponseData<BaseResponse>>(request);
        }

        protected override void BeforeRequest(IRestRequest request)
        {
            Logger.Info(JsonConvert.SerializeObject(request));
        }
        protected override void AfterRequest(IRestResponse response)
        {
            Logger.Info(JsonConvert.SerializeObject(response));
        }
    }
}
