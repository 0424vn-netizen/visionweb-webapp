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
    public interface IBatchService
    {
        [OperationContract]
        [ApiPermission(PermissionCode = WebApiSettings.PERMISSION_BATCH)]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
           , BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetBatchSummaryByEntity")]
        ReportResponse<BatchHierarchySummary> GetBatchSummaryByEntity(GenericReportFilter req);

        [OperationContract]
        [ApiPermission(PermissionCode = WebApiSettings.PERMISSION_BATCH)]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
           , BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetBatchSummaryByMerchant")]
        ReportResponse<BatchSummary> GetBatchSummaryByMerchant(MerchantSummaryFilter req);

        [OperationContract]
        [ApiPermission(PermissionCode = WebApiSettings.PERMISSION_BATCH)]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
           , BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetBatchDetail")]
        ReportResponse<TransactionDetailBatch> GetBatchDetail(BatchDetailFilter req);

        [OperationContract]
        [ApiPermission(PermissionCode = WebApiSettings.PERMISSION_BATCH)]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
           , BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetCardSummary")]
        ReportResponse<CardSummary> GetCardSummary(CardSummaryFilter req);
    }
}