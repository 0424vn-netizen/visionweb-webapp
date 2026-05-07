using AS.VW.Api.Model.Dto;
using AS.VW.Api.Model.Filter;
using AS.VW.Api.Model.Report;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace AS.VW.Api.WCF.ServiceInterface
{
    [ServiceContract]
    //[XmlSerializerFormat]
    //[AS.WCF.SecuredService]
    [ASWcfService]
    public interface IReturnService
    {
        [OperationContract]
        [ApiPermission(PermissionCode = WebApiSettings.PERMISSION_RETURN)]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetReturnSummaryByEntity")]
        ReportResponse<ReturnHierarchySummary> GetReturnSummaryByEntity(GenericReportFilter req);

        [OperationContract]
        [ApiPermission(PermissionCode = WebApiSettings.PERMISSION_RETURN)]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetReturnSummaryByMerchant")]
        ReportResponse<ReturnSummary> GetReturnSummaryByMerchant(MerchantSummaryFilter req);
    }
}
