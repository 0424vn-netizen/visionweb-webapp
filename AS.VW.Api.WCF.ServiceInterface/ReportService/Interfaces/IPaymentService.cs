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
    public interface IPaymentService
    {
        [OperationContract]
        [ApiPermission(PermissionCode = WebApiSettings.PERMISSION_PAYMENT)]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
           , BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetPaymentSummaryByEntity")]
        ReportResponse<DepositHierarchySummary> GetPaymentSummaryByEntity(GenericReportFilter req);

        [OperationContract]
        [ApiPermission(PermissionCode = WebApiSettings.PERMISSION_PAYMENT)]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
           , BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetPaymentSummaryByMerchant")]
        ReportResponse<DepositSummary> GetPaymentSummaryByMerchant(MerchantSummaryFilter req);

        [OperationContract]
        [ApiPermission(PermissionCode = WebApiSettings.PERMISSION_PAYMENT)]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetPaymentDetail")]
        ReportResponse<DepositDetail> GetPaymentDetail(MerchantSummaryFilter req);

    }
}
