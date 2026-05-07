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
    public interface IRetrievalService
    {
        [OperationContract]
        [ApiPermission(PermissionCode = WebApiSettings.PERMISSION_RETRIEVAL)]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetRetrievalsSummary")]
        ReportResponse<RetrievalHierarchySummary> GetRetrievalsSummary(GenericReportFilter req);

        [OperationContract]
        [ApiPermission(PermissionCode = WebApiSettings.PERMISSION_RETRIEVAL)]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetRetrievalsDetail")]
        ReportResponse<RetrievalDetail> GetRetrievalsDetail(MerchantSummaryFilter req);
    }
}
