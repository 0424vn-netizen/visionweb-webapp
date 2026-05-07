using System;
using System.Web.Services;
using AS.WS.Business;
using AS.WS.Entities;
using AS.VW.Api.Model.Report;
using AS.VW.Api.Model.Security;
using AS.VW.Api.Model.Filter;
using AS.VW.Api.Model.Dto;
using System.Data;

/// <summary>
/// Summary description for fdc_ApiServicces
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]

public class ApiServices : BaseService
{

    private readonly ApiBusiness _apiBusiness;
    private readonly ApiBusiness _authenBusiness;

    public ApiServices()
        : base()
    {
        // Get client id from request header
        _apiBusiness = new ApiBusiness(GeneralFuncsLib.GetConnStringSettings(GetHeader(), "MS_DBCONN"), AS.Common.DBManager.DatabaseName.MS);
        _authenBusiness = new ApiBusiness(GeneralFuncsLib.GetConnStringSettings(GetHeader(), "SEC_DBCONN"), AS.Common.DBManager.DatabaseName.SEC);
    }

    private string GetHeader()
    {
        string[] strs = RequestHeaders["ClientId"].Split(',');
        if (strs != null && strs.Length > 0)
            return strs[0];
        else
            return RequestHeaders["ClientId"];
    }

    [WebMethod]
    public AuthorizationResult ValidateApiToken(ApiAuthToken token, bool isLoadUser)
    {
        AuthorizationResult result = null;
        result = _authenBusiness.ValidateApiToken(token, isLoadUser);
        return result;
    }

    [WebMethod]
    public string GetUserPassword(ApiAuthToken token)
    {
        return _authenBusiness.GetUserPassword(token);
    }

    [WebMethod]
    public DataTable GetValidateUser(ApiAuthToken token)
    {
        return _authenBusiness.GetValidateUser(token);
    }

    [WebMethod]
    public long InsertLogPMAPI(Guid requestID, string processingStep, string message, string data1, string data2, string data3, string data4, string data5, string xmlMessage, string xmlErrorResponse, DateTime requestDTS)
    {
        var logPmApiModel = new LogPmApiModel()
        {
            RequestId = requestID,
            ProcessingStep = processingStep,
            Message = message,
            Data1 = data1,
            Data2 = data2,
            Data3 = data3,
            Data4 = data4,
            Data5 = data5,
            XmlMessage = xmlMessage,
            XmlErrorResponse = xmlErrorResponse,
            RequestDts = requestDTS,
        };
        return _apiBusiness.InsertLogPMAPI(logPmApiModel);
    }

    #region Batches

    [WebMethod]
    public ReportResponse<BatchHierarchySummary> GetBatchSummaryByEntity(User requestedUser, GenericReportFilter parameters)
    {
        return _apiBusiness.GetBatchSummaryByEntity(requestedUser, parameters);
    }

    [WebMethod]
    public ReportResponse<BatchSummary> GetBatchSummaryByMerchant(User requestedUser, MerchantSummaryFilter parameters)
    {
        return _apiBusiness.GetBatchSummaryByMerchant(requestedUser, parameters);
    }

    [WebMethod]
    public ReportResponse<TransactionDetailBatch> GetBatchDetail(User requestedUser, BatchDetailFilter parameters)
    {
        return _apiBusiness.GetBatchDetail(requestedUser, parameters);
    }

    [WebMethod]
    public ReportResponse<CardSummary> GetCardSummary(User requestedUser, CardSummaryFilter parameters)
    {
        return _apiBusiness.GetCardSummary(requestedUser, parameters);
    }

    #endregion

    #region Transactions

    [WebMethod]
    public ReportResponse<TransactionDetail> GetTransactions(User requestedUser, TransactionSearchFilter parameters)
    {
        return _apiBusiness.GetTransactions(requestedUser, parameters);
    }

    [WebMethod]
    public Voucher GetVoucher(User requestedUser, VoucherFilter parameters)
    {
        return _apiBusiness.GetVoucher(requestedUser, parameters);
    }

    #endregion

    #region Returns
    [WebMethod]
    public ReportResponse<ReturnHierarchySummary> GetReturnSummaryByEntity(User requestedUser, GenericReportFilter parameters)
    {
        return _apiBusiness.GetReturnSummaryByEntity(requestedUser, parameters);
    }

    [WebMethod]
    public ReportResponse<ReturnSummary> GetReturnSummaryByMerchant(User requestedUser, MerchantSummaryFilter parameters)
    {
        return _apiBusiness.GetReturnSummaryByMerchant(requestedUser, parameters);
    }

    #endregion

    #region Authorization
    [WebMethod]
    public ReportResponse<AuthorizationLogSummary> GetAuthorizationLogSummary(User requestedUser, GenericReportFilter parameters)
    {
        return _apiBusiness.GetAuthorizationLogSummary(requestedUser, parameters, string.Empty);
    }

    [WebMethod]
    public ReportResponse<AuthorizationDetail> GetAuthorizationLogDetail(User requestedUser, MerchantSummaryFilter parameters)
    {
        return _apiBusiness.GetAuthorizationLogDetail(requestedUser, parameters);
    }

    #endregion

    #region Payments

    [WebMethod]
    public ReportResponse<DepositHierarchySummary> GetPaymentSummaryByEntity(User requestedUser, GenericReportFilter parameters)
    {
        return _apiBusiness.GetPaymentSummaryByEntity(requestedUser, parameters);
    }

    [WebMethod]
    public ReportResponse<DepositSummary> GetPaymentSummaryByMerchant(User requestedUser, MerchantSummaryFilter parameters)
    {
        ReportResponse<DepositSummary> result = _apiBusiness.GetPaymentSummaryByMerchant(requestedUser, parameters);
        return result;
    }

    [WebMethod]
    public ReportResponse<DepositDetail> GetPaymentDetail(User requestedUser, MerchantSummaryFilter parameters)
    {
        return _apiBusiness.GetPaymentDetail(requestedUser, parameters);
    }

    #endregion

    #region Retrievals

    [WebMethod]
    public ReportResponse<RetrievalHierarchySummary> GetRetrievalSummaryByEntity(User requestedUser, GenericReportFilter parameters)
    {
        return _apiBusiness.GetRetrievalSummaryByEntity(requestedUser, parameters);
    }

    [WebMethod]
    public ReportResponse<RetrievalDetail> GetRetrievalDetail(User requestedUser, MerchantSummaryFilter parameters)
    {
        return _apiBusiness.GetRetrievalDetail(requestedUser, parameters);
    }

    #endregion

    #region Chargebacks

    [WebMethod]
    public ReportResponse<ChargebackHierarchySummary> GetChargebackSummaryByEntity(User requestedUser, GenericReportFilter parameters)
    {
        return _apiBusiness.GetChargebackSummaryByEntity(requestedUser, parameters);
    }

    [WebMethod]
    public ReportResponse<ChargebackDetail> GetChargebackDetail(User requestedUser, MerchantSummaryFilter parameters)
    {
        return _apiBusiness.GetChargebackDetail(requestedUser, parameters);
    }

    #endregion

    #region VoidsRejects

    [WebMethod]
    public ReportResponse<VoidRejectHierarchySummary> GetVoidsRejectSummaryByEntity(User requestedUser, GenericReportFilter parameters)
    {
        return _apiBusiness.GetVoidsRejectSummaryByEntity(requestedUser, parameters);
    }

    [WebMethod]
    public ReportResponse<VoidRejectDetail> GetVoidsRejectDetail(User requestedUser, MerchantSummaryFilter parameters)
    {
        return _apiBusiness.GetVoidsRejectDetail(requestedUser, parameters);
    }

    #endregion

    #region MerchantInfomation

    [WebMethod]
    public ReportResponse<MerchantSummary> GetMerchantList(User requestedUser, HierarchyPagingFilter filter)
    {
        return _apiBusiness.GetMerchantList(requestedUser, filter);
    }

    [WebMethod]
    public ReportResponse<MerchantComment> GetMerchantComments(User requestedUser, MerchantInformationPagingFilter filter)
    {
        return _apiBusiness.GetMerchantComments(requestedUser, filter);
    }

    #endregion
}
