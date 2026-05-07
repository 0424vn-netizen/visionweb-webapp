using AS.VW.Api.Model.Dto;
using AS.VW.Api.Model.Filter;
using AS.VW.Api.Model.Report;
using AS.VW.Api.Business.Repository;
using AS.VW.Api.Validation;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace AS.VW.Api.WCF.ServiceInterface
{    
    
    [ServiceContract]
    //[XmlSerializerFormat]
    //[AS.WCF.SecuredService]
    [ASWcfService]
    public interface IAuthorizationService 
    {
        [OperationContract]
        [ApiPermission(PermissionCode = WebApiSettings.PERMISSION_AUTHOR)]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
           , BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAuthorizationLogSummary")]
        ReportResponse<AuthorizationLogSummary> GetAuthorizationLogSummary(GenericReportFilter req);

        [OperationContract]
        [ApiPermission(PermissionCode = WebApiSettings.PERMISSION_AUTHOR)]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
           , BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAuthorizationLogDetail")]
        ReportResponse<AuthorizationDetail> GetAuthorizationLogDetail(MerchantSummaryFilter req);

    }
}
