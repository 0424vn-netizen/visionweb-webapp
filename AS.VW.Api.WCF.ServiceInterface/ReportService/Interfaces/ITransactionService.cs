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
    public interface ITransactionService
    {
        [OperationContract]
        [ApiPermission(PermissionCode = WebApiSettings.PERMISSION_TRANSACTION)]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json
            , BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetTransactions")]
        ReportResponse<TransactionDetail> GetTransactions(TransactionSearchFilter req);
    }
}
