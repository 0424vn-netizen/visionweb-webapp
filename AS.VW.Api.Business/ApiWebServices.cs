using AS.Common.DataProtection;
using AS.Common.WebUI;
using AS.VW.Api.Business.vw_ApiServicesProxy;
using AS.VW.Api.Model.Dto;
using Microsoft.Web.Services3.Security.Tokens;
using System.Configuration;
using System.Data;
using ApiAuthToken = AS.VW.Api.Model.Security.ApiAuthToken;
using ApiAuthTokenWs = AS.VW.Api.Business.vw_ApiServicesProxy.ApiAuthToken;
using AuthorizationResult = AS.VW.Api.Model.Security.AuthorizationResult;
using BatchHierarchySummary = AS.VW.Api.Model.Report.BatchHierarchySummary;
using fdc_ApiServicesProxyNS = AS.VW.Api.Business.vw_ApiServicesProxy;
using FilterNS = AS.VW.Api.Model.Filter;
using GenericReportFilter = AS.VW.Api.Model.Filter.GenericReportFilter;
using GenericReportFilterWs = AS.VW.Api.Business.vw_ApiServicesProxy.GenericReportFilter;
using Pagination = AS.VW.Api.Model.Pagination;
using ReportNS = AS.VW.Api.Model.Report;
using User = AS.VW.Api.Model.Security.User;
using UserWs = AS.VW.Api.Business.vw_ApiServicesProxy.User;
namespace AS.VW.Api.Business
{
    public class ApiWebServices : AS.Common.WebUI.ASReportBusiness, ICryptor
    {
        private ApiServices _apiWebService = null;

        public ApiWebServices()
        {
            string url = ConfigurationManager.AppSettings["ApiWebServiceUrl"];
            string token1 = ConfigurationManager.AppSettings["ApiWebServiceToken1"];
            string token2 = ConfigurationManager.AppSettings["ApiWebServiceToken2"];
            InitializeApiWebService(url, token1, token2);
        }

        public ApiWebServices(string url, string token1, string token2)
        {
            InitializeApiWebService(url, token1, token2);
        }

        public void InitializeApiWebService(string url, string token1, string token2)
        {
            _apiWebService = new ApiServices();

            string userName = Cryptophy.DecryptText(token1);
            string passWord = Cryptophy.DecryptText(token2);
            UsernameToken token = new UsernameToken(userName, passWord, PasswordOption.SendPlainText);
            _apiWebService.Url = url;
            _apiWebService.SetClientCredential(token);
            _apiWebService.SetPolicy("ClientPolicy");
            try
            {
                _apiWebService.Timeout = int.Parse(ConfigurationManager.AppSettings["ServiceTimeout"]) * 60000;
            }
            catch
            {
                _apiWebService.Timeout = 5 * 60000;
            }

            this.ReportWS = _apiWebService;

        }

        public void AddRequestHeader(string name, string value)
        {
            // just add one time
            _apiWebService.AddRequestHeader(name, value);
        }              

        #region Security
        public AuthorizationResult ValidateApiToken(ApiAuthToken token, bool autoLoadUser)
        {
            AuthorizationResult authenticatedResult = null;
            authenticatedResult = _apiWebService.ValidateApiToken(token.ConvertTo<ApiAuthTokenWs>(), autoLoadUser).ConvertTo<AuthorizationResult>();

            return authenticatedResult;
        }

        public string GetUserPassword(ApiAuthToken token)
        {
            return _apiWebService.GetUserPassword(token.ConvertTo<ApiAuthTokenWs>());
        }

        public DataTable GetValidateUser(ApiAuthToken token)
        {
            return _apiWebService.GetValidateUser(token.ConvertTo<ApiAuthTokenWs>());
        }


        #region GetUser
        public DataTable GetUser(string userName, string password)
        {
            return new DataTable();
        }
        #endregion

        #region LogUserPass
        public void LogUserPass(string user, string pass, string status, string reason)
        {
            //_PCIAPIServices.LogUserPass(user, pass, status, reason);
        }
        #endregion

        #endregion

        #region Batches
        public ReportResponse<BatchHierarchySummary> GetBatchSummaryByEntity(User requestedUser, GenericReportFilter parameters)
        {
            ReportResponse<BatchHierarchySummary> result = new ReportResponse<BatchHierarchySummary>();

            ReportResponseOfBatchHierarchySummary wsResult = _apiWebService.GetBatchSummaryByEntity(requestedUser.ConvertTo<UserWs>(), parameters.ConvertTo<GenericReportFilterWs>());

            result.Result = wsResult.Result.ConvertListTo<BatchHierarchySummary>();
            result.Total = wsResult.Total.ConvertTo<BatchHierarchySummary>();
            result.Paging = wsResult.Paging.ConvertTo<Pagination>();

            return result;
        }

        public ReportResponse<ReportNS.BatchSummary> GetBatchSummaryByMerchant(User requestedUser, FilterNS.MerchantSummaryFilter parameters)
        {
            ReportResponseOfBatchSummary wsResult = _apiWebService.GetBatchSummaryByMerchant(requestedUser.ConvertTo<UserWs>(), parameters.ConvertTo<fdc_ApiServicesProxyNS.MerchantSummaryFilter>());

            ReportResponse<ReportNS.BatchSummary> result = new ReportResponse<ReportNS.BatchSummary>();
            result.Result = wsResult.Result.ConvertListTo<ReportNS.BatchSummary>();
            result.Total = wsResult.Total.ConvertTo<ReportNS.BatchSummary>();
            result.Paging = wsResult.Paging.ConvertTo<Pagination>();

            return result;
        }

        public ReportResponse<ReportNS.TransactionDetailBatch> GetBatchDetail(User requestedUser, FilterNS.BatchDetailFilter parameters)
        {

            ReportResponseOfTransactionDetailBatch wsResult = _apiWebService.GetBatchDetail(requestedUser.ConvertTo<UserWs>(), parameters.ConvertTo<fdc_ApiServicesProxyNS.BatchDetailFilter>());

            ReportResponse<ReportNS.TransactionDetailBatch> result = new ReportResponse<ReportNS.TransactionDetailBatch>();
            result.Result = wsResult.Result.ConvertListTo<ReportNS.TransactionDetailBatch>();
            result.Total = wsResult.Total.ConvertTo<ReportNS.TransactionDetailBatch>();
            result.Paging = wsResult.Paging.ConvertTo<Pagination>();

            return result;
        }

        public ReportResponse<ReportNS.CardSummary> GetCardSummary(User requestedUser, FilterNS.CardSummaryFilter parameters)
        {

            ReportResponseOfCardSummary wsResult = _apiWebService.GetCardSummary(requestedUser.ConvertTo<UserWs>(), parameters.ConvertTo<fdc_ApiServicesProxyNS.CardSummaryFilter>());

            ReportResponse<ReportNS.CardSummary> result = new ReportResponse<ReportNS.CardSummary>();
            result.Result = wsResult.Result.ConvertListTo<ReportNS.CardSummary>();
            result.Total = wsResult.Total.ConvertTo<ReportNS.CardSummary>();
            result.Paging = wsResult.Paging.ConvertTo<Pagination>();

            return result;
        }
        #endregion

        #region Payments
        public ReportResponse<ReportNS.DepositHierarchySummary> GetPaymentSummaryByEntity(User requestedUser, GenericReportFilter parameters)
        {

            ReportResponse<ReportNS.DepositHierarchySummary> result = new ReportResponse<ReportNS.DepositHierarchySummary>();

            ReportResponseOfDepositHierarchySummary wsResult = _apiWebService.GetPaymentSummaryByEntity(requestedUser.ConvertTo<UserWs>(), parameters.ConvertTo<fdc_ApiServicesProxyNS.GenericReportFilter>());

            result.Result = wsResult.Result.ConvertListTo<ReportNS.DepositHierarchySummary>();
            result.Total = wsResult.Total.ConvertTo<ReportNS.DepositHierarchySummary>();
            result.Paging = wsResult.Paging.ConvertTo<Pagination>();

            return result;
        }

        public ReportResponse<ReportNS.DepositSummary> GetPaymentSummaryByMerchant(User requestedUser, FilterNS.MerchantSummaryFilter parameters)
        {

            ReportResponse<ReportNS.DepositSummary> result = new ReportResponse<ReportNS.DepositSummary>();

            ReportResponseOfDepositSummary wsResult = _apiWebService.GetPaymentSummaryByMerchant(requestedUser.ConvertTo<UserWs>(), parameters.ConvertTo<fdc_ApiServicesProxyNS.MerchantSummaryFilter>());

            result.Result = wsResult.Result.ConvertListTo<ReportNS.DepositSummary>();
            result.Total = wsResult.Total.ConvertTo<ReportNS.DepositSummary>();
            result.Paging = wsResult.Paging.ConvertTo<Pagination>();

            return result;
        }

        public ReportResponse<ReportNS.DepositDetail> GetPaymentDetail(User requestedUser, FilterNS.MerchantSummaryFilter parameters)
        {            
            ReportResponse<ReportNS.DepositDetail> result = new ReportResponse<ReportNS.DepositDetail>();

            ReportResponseOfDepositDetail wsResult = _apiWebService.GetPaymentDetail(requestedUser.ConvertTo<UserWs>(), parameters.ConvertTo<fdc_ApiServicesProxyNS.MerchantSummaryFilter>());

            result.Result = wsResult.Result.ConvertListTo<ReportNS.DepositDetail>();
            result.Total = wsResult.Total.ConvertTo<ReportNS.DepositDetail>();
            result.Paging = wsResult.Paging.ConvertTo<Pagination>();

            return result;
        }
        #endregion

        #region Returns
        public ReportResponse<ReportNS.ReturnHierarchySummary> GetReturnSummaryByEntity(User requestedUser, GenericReportFilter parameters)
        {

            ReportResponseOfReturnHierarchySummary wsResult = _apiWebService.GetReturnSummaryByEntity(requestedUser.ConvertTo<UserWs>(), parameters.ConvertTo<fdc_ApiServicesProxyNS.GenericReportFilter>());

            ReportResponse<ReportNS.ReturnHierarchySummary> result = new ReportResponse<ReportNS.ReturnHierarchySummary>();
            result.Result = wsResult.Result.ConvertListTo<ReportNS.ReturnHierarchySummary>();
            result.Total = wsResult.Total.ConvertTo<ReportNS.ReturnHierarchySummary>();
            result.Paging = wsResult.Paging.ConvertTo<Pagination>();

            return result;
        }

        public ReportResponse<ReportNS.ReturnSummary> GetReturnSummaryByMerchant(User requestedUser, FilterNS.MerchantSummaryFilter parameters)
        {

            ReportResponseOfReturnSummary wsResult = _apiWebService.GetReturnSummaryByMerchant(requestedUser.ConvertTo<UserWs>(), parameters.ConvertTo<fdc_ApiServicesProxyNS.MerchantSummaryFilter>());

            ReportResponse<ReportNS.ReturnSummary> result = new ReportResponse<ReportNS.ReturnSummary>();
            result.Result = wsResult.Result.ConvertListTo<ReportNS.ReturnSummary>();
            result.Total = wsResult.Total.ConvertTo<ReportNS.ReturnSummary>();
            result.Paging = wsResult.Paging.ConvertTo<Pagination>();

            return result;
        }
        #endregion

        #region AuthorizationLog

        public ReportResponse<ReportNS.AuthorizationLogSummary> GetAuthorizationLogSummary(User requestedUser, GenericReportFilter parameters)
        {
            
            ReportResponseOfAuthorizationLogSummary wsResult = _apiWebService.GetAuthorizationLogSummary(requestedUser.ConvertTo<UserWs>(), parameters.ConvertTo<fdc_ApiServicesProxyNS.GenericReportFilter>());

            ReportResponse<ReportNS.AuthorizationLogSummary> result = new ReportResponse<ReportNS.AuthorizationLogSummary>();
            result.Result = wsResult.Result.ConvertListTo<ReportNS.AuthorizationLogSummary>();
            result.Total = wsResult.Total.ConvertTo<ReportNS.AuthorizationLogSummary>();
            result.Paging = wsResult.Paging.ConvertTo<Pagination>();

            return result;
        }

        public ReportResponse<ReportNS.AuthorizationDetail> GetAuthorizationLogDetail(User requestedUser, FilterNS.MerchantSummaryFilter parameters)
        {

            ReportResponseOfAuthorizationDetail wsResult = _apiWebService.GetAuthorizationLogDetail(requestedUser.ConvertTo<UserWs>(), parameters.ConvertTo<fdc_ApiServicesProxyNS.MerchantSummaryFilter>());

            ReportResponse<ReportNS.AuthorizationDetail> result = new ReportResponse<ReportNS.AuthorizationDetail>();
            result.Result = wsResult.Result.ConvertListTo<ReportNS.AuthorizationDetail>();
            result.Total = wsResult.Total.ConvertTo<ReportNS.AuthorizationDetail>();
            result.Paging = wsResult.Paging.ConvertTo<Pagination>();

            return result;
        }

        #endregion

        #region Transactions
        public ReportResponse<ReportNS.TransactionDetail> GetTransactions(User requestedUser, FilterNS.TransactionSearchFilter parameters)
        {

            ReportResponseOfTransactionDetail wsResult = _apiWebService.GetTransactions(requestedUser.ConvertTo<UserWs>(), parameters.ConvertTo<fdc_ApiServicesProxyNS.TransactionSearchFilter>());

            ReportResponse<ReportNS.TransactionDetail> result = new ReportResponse<ReportNS.TransactionDetail>();
            result.Result = wsResult.Result.ConvertListTo<ReportNS.TransactionDetail>();
            result.Total = wsResult.Total.ConvertTo<ReportNS.TransactionDetail>();
            result.Paging = wsResult.Paging.ConvertTo<Pagination>();

            return result;
        }

        public ReportNS.Voucher GetVoucher(User requestedUser, FilterNS.VoucherFilter parameters)
        {
            fdc_ApiServicesProxyNS.Voucher wsResult = _apiWebService.GetVoucher(requestedUser.ConvertTo<UserWs>(), parameters.ConvertTo<fdc_ApiServicesProxyNS.VoucherFilter>());

            return wsResult.ConvertTo<ReportNS.Voucher>();
        }
        #endregion

        #region VoidsRejects
        public ReportResponse<ReportNS.VoidRejectHierarchySummary> GetVoidRejectSummaryByEntity(User requestedUser, GenericReportFilter parameters)
        {
            ReportResponseOfVoidRejectHierarchySummary wsResult = _apiWebService.GetVoidsRejectSummaryByEntity(requestedUser.ConvertTo<UserWs>(), parameters.ConvertTo<fdc_ApiServicesProxyNS.GenericReportFilter>());

            ReportResponse<ReportNS.VoidRejectHierarchySummary> result = new ReportResponse<ReportNS.VoidRejectHierarchySummary>();
            result.Result = wsResult.Result.ConvertListTo<ReportNS.VoidRejectHierarchySummary>();
            result.Total = wsResult.Total.ConvertTo<ReportNS.VoidRejectHierarchySummary>();
            result.Paging = wsResult.Paging.ConvertTo<Pagination>();

            return result;
        }

        public ReportResponse<ReportNS.VoidRejectDetail> GetVoidsRejectDetail(User requestedUser, FilterNS.MerchantSummaryFilter parameters)
        {
            ReportResponseOfVoidRejectDetail wsResult = _apiWebService.GetVoidsRejectDetail(requestedUser.ConvertTo<UserWs>(), parameters.ConvertTo<fdc_ApiServicesProxyNS.MerchantSummaryFilter>());

            ReportResponse<ReportNS.VoidRejectDetail> result = new ReportResponse<ReportNS.VoidRejectDetail>();
            result.Result = wsResult.Result.ConvertListTo<ReportNS.VoidRejectDetail>();
            result.Total = wsResult.Total.ConvertTo<ReportNS.VoidRejectDetail>();
            result.Paging = wsResult.Paging.ConvertTo<Pagination>();

            return result;
        }
        #endregion

        #region Retrievals
        public ReportResponse<ReportNS.RetrievalHierarchySummary> GetRetrievalSummaryByEntity(User requestedUser, GenericReportFilter parameters)
        {

            ReportResponseOfRetrievalHierarchySummary wsResult = _apiWebService.GetRetrievalSummaryByEntity(requestedUser.ConvertTo<UserWs>(), parameters.ConvertTo<fdc_ApiServicesProxyNS.GenericReportFilter>());

            ReportResponse<ReportNS.RetrievalHierarchySummary> result = new ReportResponse<ReportNS.RetrievalHierarchySummary>();
            result.Result = wsResult.Result.ConvertListTo<ReportNS.RetrievalHierarchySummary>();
            result.Total = wsResult.Total.ConvertTo<ReportNS.RetrievalHierarchySummary>();
            result.Paging = wsResult.Paging.ConvertTo<Pagination>();

            return result;
        }

        public ReportResponse<ReportNS.RetrievalDetail> GetRetrievalDetail(User requestedUser, FilterNS.MerchantSummaryFilter parameters)
        {

            ReportResponseOfRetrievalDetail wsResult = _apiWebService.GetRetrievalDetail(requestedUser.ConvertTo<UserWs>(), parameters.ConvertTo<fdc_ApiServicesProxyNS.MerchantSummaryFilter>());

            ReportResponse<ReportNS.RetrievalDetail> result = new ReportResponse<ReportNS.RetrievalDetail>();
            result.Result = wsResult.Result.ConvertListTo<ReportNS.RetrievalDetail>();
            result.Total = wsResult.Total.ConvertTo<ReportNS.RetrievalDetail>();
            result.Paging = wsResult.Paging.ConvertTo<Pagination>();

            return result;
        }
        #endregion

        #region Chargebacks
        public ReportResponse<ReportNS.ChargebackHierarchySummary> GetChargebackSummaryByEntity(User requestedUser, GenericReportFilter parameters)
        {

            ReportResponseOfChargebackHierarchySummary wsResult = _apiWebService.GetChargebackSummaryByEntity(requestedUser.ConvertTo<UserWs>(), parameters.ConvertTo<fdc_ApiServicesProxyNS.GenericReportFilter>());

            ReportResponse<ReportNS.ChargebackHierarchySummary> result = new ReportResponse<ReportNS.ChargebackHierarchySummary>();
            result.Result = wsResult.Result.ConvertListTo<ReportNS.ChargebackHierarchySummary>();
            result.Total = wsResult.Total.ConvertTo<ReportNS.ChargebackHierarchySummary>();
            result.Paging = wsResult.Paging.ConvertTo<Pagination>();

            return result;
        }

        public ReportResponse<ReportNS.ChargebackDetail> GetChargebackDetail(User requestedUser, FilterNS.MerchantSummaryFilter parameters)
        {

            ReportResponseOfChargebackDetail wsResult = _apiWebService.GetChargebackDetail(requestedUser.ConvertTo<UserWs>(), parameters.ConvertTo<fdc_ApiServicesProxyNS.MerchantSummaryFilter>());

            ReportResponse<ReportNS.ChargebackDetail> result = new ReportResponse<ReportNS.ChargebackDetail>();
            result.Result = wsResult.Result.ConvertListTo<ReportNS.ChargebackDetail>();
            result.Total = wsResult.Total.ConvertTo<ReportNS.ChargebackDetail>();
            result.Paging = wsResult.Paging.ConvertTo<Pagination>();

            return result;
        }
        #endregion

        #region MerchantInfomation

        public ReportResponse<ReportNS.MerchantSummary> GetMerchantList(User requestedUser, FilterNS.HierarchyPagingFilter parameters)
        {
            ReportResponseOfMerchantSummary wsResult = _apiWebService.GetMerchantList(requestedUser.ConvertTo<UserWs>(), parameters.ConvertTo<fdc_ApiServicesProxyNS.HierarchyPagingFilter>());

            ReportResponse<ReportNS.MerchantSummary> result = new ReportResponse<ReportNS.MerchantSummary>();
            result.Result = wsResult.Result.ConvertListTo<ReportNS.MerchantSummary>();
            result.Total = wsResult.Total.ConvertTo<ReportNS.MerchantSummary>();
            result.Paging = wsResult.Paging.ConvertTo<Pagination>();

            return result;
        }



        public ReportResponse<ReportNS.MerchantComment> GetMerchantComments(User requestedUser, FilterNS.MerchantInformationPagingFilter parameters)
        {
            ReportResponseOfMerchantComment wsResult = _apiWebService.GetMerchantComments(requestedUser.ConvertTo<UserWs>(), parameters.ConvertTo<fdc_ApiServicesProxyNS.MerchantInformationPagingFilter>());

            ReportResponse<ReportNS.MerchantComment> result = new ReportResponse<ReportNS.MerchantComment>();
            result.Result = wsResult.Result.ConvertListTo<ReportNS.MerchantComment>();
            result.Total = wsResult.Total.ConvertTo<ReportNS.MerchantComment>();
            result.Paging = wsResult.Paging.ConvertTo<Pagination>();

            return result;
        }




        #endregion


        public string DecryptText(string str, string key)
        {
            throw new System.NotImplementedException();
        }

        public string DecryptText(string str)
        {
            throw new System.NotImplementedException();
        }

        public string EncryptText(string str, string key)
        {
            throw new System.NotImplementedException();
        }

        public string EncryptText(string str)
        {
            throw new System.NotImplementedException();
        }
    }
}
