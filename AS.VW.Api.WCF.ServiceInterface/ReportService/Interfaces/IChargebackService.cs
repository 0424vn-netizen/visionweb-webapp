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
    public interface IChargebackService
    {
        [OperationContract]
        [ApiPermission(PermissionCode = WebApiSettings.PERMISSION_CHARGEBACK)]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
           , BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetChargebacksSummary")]
        ReportResponse<ChargebackHierarchySummary> GetChargebacksSummary(GenericReportFilter req);

        [OperationContract]
        [ApiPermission(PermissionCode = WebApiSettings.PERMISSION_CHARGEBACK)]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
           , BodyStyle = WebMessageBodyStyle.WrappedRequest, UriTemplate = "/GetChargebacksDetail")]
        ReportResponse<ChargebackDetail> GetChargebacksDetail(MerchantSummaryFilter req);
    }
}
