using System;
using System.Collections.Generic;
using System.Data;
using AS.WS.Data;
using AS.VW.Api.Model.Report;
using AS.VW.Api.Model.Security;
using AS.VW.Api.Model.Filter;
using AS.VW.Api.Model.Dto;
using AS.Common.DBManager;
using System.Xml.Linq;
using AS.Common.DataProtection;
using AS.WS.Entities;
using AS.VW.Common;

namespace AS.WS.Business
{
    public class ApiBusiness
    {
        private readonly IReportingDao _ApiDao = null;

        private readonly Dictionary<string, string> _mapEntityId = new Dictionary<string, string>()
        {
            {"EntityId","Entity"}
        };

        private readonly Dictionary<string, string> _mapCardNumber = new Dictionary<string, string>()
        {
            {"CardNumber","AccountNumber"}
        };

        public ApiBusiness(string connString, DatabaseName dbName)
        {
            // Default is Merchant API
            switch (dbName)
            {
                case DatabaseName.SEC:
                    break;
                case DatabaseName.CS:
                    break;
                case DatabaseName.MS:
                    break;
                default:
                    _ApiDao = new MsReportingDao(connString);
                    break;

            }

        }

        #region Utils
        private string GetSortExpression()
        {
            string columnDefault = "Entity ASC";


            return columnDefault;
        }

        private ReportResponse<T> SetPaging<T>(ReportResponse<T> result, IPagingFilter filters = null)
        {
            if (filters != null && result.Paging != null)
            {
                result.Paging.CurrentPageIndex = filters.CurrentPageIndex;
                result.Paging.PageSize = filters.PageSize;
            }
            else if (result.Paging != null && result.Result != null)
            {
                result.Paging.NumberOfRecords = result.Result.Count;
            }
            return result;
        }
        #endregion

        #region Log
        public long InsertLogPMAPI(LogPmApiModel logPmApiModel)
        {
            XElement xElement = XElement.Parse(logPmApiModel.XmlMessage);
            XElement xElementError = XElement.Parse(logPmApiModel.XmlErrorResponse);

            FilterParameterCollection parameters = new FilterParameterCollection();
            FilterParameterCollection outParameters;
            parameters.Add("@LogID", 0, DbType.Int64, true)
                    .Add("@XMLMessage", xElement.ToString(), DbType.Xml)
                    .Add("@XMLErrorResponse", xElementError.ToString(), DbType.Xml)
                    .Add("@RequestDTS", logPmApiModel.RequestDts, DbType.DateTime)
                    .Add("@RequestID", logPmApiModel.RequestId, DbType.Guid)
                    .Add("@ProcessingStep", logPmApiModel.ProcessingStep, DbType.String)
                    .Add("@Message", logPmApiModel.Message, DbType.AnsiString)
                    .Add("@Data1", logPmApiModel.Data1, DbType.String)
                    .Add("@Data2", logPmApiModel.Data2, DbType.String)
                    .Add("@Data3", logPmApiModel.Data3, DbType.String)
                    .Add("@Data4", logPmApiModel.Data4, DbType.String)
                    .Add("@Data5", logPmApiModel.Data5, DbType.String);

            _ApiDao.ExecuteNonQueryCommand("spa_APIReporting_Insert_Log", parameters, out outParameters);

            if (outParameters[0].ParameterValue.IsNotNullData())
            {
                return Convert.ToInt64(outParameters[0].ParameterValue);
            }

            return 0;
        }
        #endregion

        #region Security

        public AuthorizationResult ValidateApiToken(ApiAuthToken token, bool isLoadUser)
        {
            AuthorizationResult result = null;

            FilterParameterCollection parameters = new FilterParameterCollection();
            FilterParameterCollection outParameters;
            parameters.Add(new FilterParameter("@LoginResult", 0, DbType.Int32, true));
            parameters.Add(new FilterParameter("@AsClientId", token.AsClientId, DbType.Int32));
            parameters.Add(new FilterParameter("@UserID", token.ApiClientId, DbType.String));
            parameters.Add(new FilterParameter("@ApiKey", token.ApiKey, DbType.String));
            parameters.Add(new FilterParameter("@RemoteIp", token.RemoteIp, DbType.String));
            parameters.Add(new FilterParameter("@IsLoadUser", isLoadUser, DbType.Boolean));
            parameters.Add(new FilterParameter("@ApiVersion", token.ApiVersion, DbType.String));

            DataSet dsResult = _ApiDao.ExecuteQueryCommand("spa_APIReporting_ValidateToken", parameters, out outParameters);

            if (dsResult != null)
            {
                result = new AuthorizationResult() { Result = LoginActions.Fail };

                int loginResult = (int)outParameters.FindFilterParameterByName("@LoginResult", false).ParameterValue;

                if (loginResult == 1 && dsResult.Tables[0].Rows.Count > 0)
                {
                    result.AuthorizedUser = dsResult.Tables[0].Rows[0].To<User>();

                    result.Permissions = new List<string>();

                    foreach (DataRow row in dsResult.Tables[1].Rows)
                    {
                        result.Permissions.Add(row["PermissionCode"].ToString());
                    }

                    result.Roles = new List<string>();

                    foreach (DataRow row in dsResult.Tables[2].Rows)
                    {
                        result.Roles.Add(row["HierarchyCode"].ToString());
                    }

                    result.Result = LoginActions.Success;
                }
            }

            return result;
        }

        public string GetUserPassword(ApiAuthToken token)
        {
            var parameters = new FilterParameterCollection();
            FilterParameterCollection outParameters;
            parameters.Add("@Password", string.Empty, DbType.AnsiString, true)
                      .Add("@ASClientID", token.AsClientId, DbType.Int32)
                      .Add("@SiteID", 1000, DbType.Int32)
                      .Add("@UserID", token.ApiClientId, DbType.AnsiString)
                      .Add("@PlatformId", 0, DbType.Int32)
                      .Add("@ModuleName", "PM", DbType.AnsiString)
                      .Add("@UserMode", "CLIENT", DbType.AnsiString);

            _ApiDao.ExecuteNonQueryCommand("spa_APIReporting_GeUserPassword", parameters, out outParameters);

            if (outParameters[0].ParameterValue.IsNotNullData())
            {
                return Cryptophy.DecryptText(outParameters[0].ParameterValue.ToString());
            }

            return string.Empty;
        }

        public DataTable GetValidateUser(ApiAuthToken token)
        {
            FilterParameterCollection param = new FilterParameterCollection();
            param.Add("@ASClientID", token.AsClientId, DbType.Int32);
            param.Add("@ModuleName", "PM", DbType.AnsiString);
            param.Add(new FilterParameter("@UserID", token.ApiClientId, DbType.AnsiString));
            param.Add(new FilterParameter("@UserPassword", token.ApiKey, DbType.AnsiString));
            return (_ApiDao.ExecuteQueryCommand("spa_APIReporting_CheckUser", param, out param)).Tables[0];
        }

        #endregion

        #region Batches

        public ReportResponse<BatchHierarchySummary> GetBatchSummaryByEntity(User requestedUser, GenericReportFilter filter)
        {
            FilterParameterCollection spaParameters = new FilterParameterCollection();
            spaParameters = spaParameters.AddReportFilterParameters(filter)
                                         .AddLoggedInUser(requestedUser)
                                         .AddOrderParameter(GetSortExpression());

            DataSet dsResult = _ApiDao.GetReportsAsDataSet("spa_APIReporting_GetBatchSummary", spaParameters);
            return SetPaging(dsResult.ToReportResponse<BatchHierarchySummary>(true, mapNames: _mapEntityId).AddEntityType(filter, filter.ViewLevel.ToString()), filter);

        }

        public ReportResponse<BatchSummary> GetBatchSummaryByMerchant(User requestedUser, MerchantSummaryFilter filters)
        {
            FilterParameterCollection spaParameters = new FilterParameterCollection();
            spaParameters = spaParameters.AddReportFilterParameters(filters)
                                         .AddLoggedInUser(requestedUser)
                                         .AddOrderParameter(" ReportDate DESC ");

            DataSet dsResult = _ApiDao.GetReportsAsDataSet("spa_APIReporting_GetBatchSummaryByMerchant", spaParameters);
            return SetPaging(dsResult.ToReportResponse<BatchSummary>(true), filters);
        }


        public ReportResponse<TransactionDetailBatch> GetBatchDetail(User requestedUser, BatchDetailFilter filter)
        {
            FilterParameterCollection spaParameters = new FilterParameterCollection();
            spaParameters = spaParameters.AddReportFilterParameters(filter)
                                         .Add("@BatchNumber", filter.BatchNumber, DbType.AnsiString)
                                         .AddLoggedInUser(requestedUser)
                                         .AddOrderParameter("TransactionDate Desc , TransactionTime DESC");

            DataSet dsResult = _ApiDao.GetReportsAsDataSet("spa_APIReporting_GetBatchDetail", spaParameters);
            return SetPaging(dsResult.ToReportResponse<TransactionDetailBatch>(true, mapNames: _mapCardNumber), filter);

        }

        public ReportResponse<CardSummary> GetCardSummary(User requestedUser, CardSummaryFilter filter)
        {
            FilterParameterCollection spaParameters = new FilterParameterCollection();
            spaParameters = spaParameters.AddDateFilterParam(filter)
                                         .AddPagingFilterParam(filter)
                                         .Add("@BatchNumber", filter.BatchNumber, DbType.AnsiString)
                                         .Add("@HierarchyFilterMode", filter.HierarchyFilterMode.ToString(), DbType.AnsiString)
                                         .AddLoggedInUser(requestedUser);
            if (!string.IsNullOrEmpty(filter.HierarchyFilterValue))
            {
                spaParameters.AddHierarchyFilterParam(filter);
            }

            DataSet dsResult = _ApiDao.GetReportsAsDataSet("spa_APIReporting_GetCardSummary", spaParameters);
            return SetPaging(dsResult.ToReportResponse<CardSummary>(true), filter);
        }

        #endregion

        #region Payments

        public ReportResponse<DepositHierarchySummary> GetPaymentSummaryByEntity(User requestedUser, GenericReportFilter filter)
        {
            FilterParameterCollection spaParameters = new FilterParameterCollection();
            spaParameters = spaParameters.AddReportFilterParameters(filter)
                                         .AddLoggedInUser(requestedUser)
                                         .AddOrderParameter(GetSortExpression());

            DataSet dsResult = _ApiDao.GetReportsAsDataSet("spa_APIReporting_GetPaymentSummary", spaParameters);
            return SetPaging(dsResult.ToReportResponse<DepositHierarchySummary>(true, mapNames: _mapEntityId).AddEntityType(filter, filter.ViewLevel.ToString()), filter);
        }

        public ReportResponse<DepositSummary> GetPaymentSummaryByMerchant(User requestedUser, MerchantSummaryFilter filter)
        {
            FilterParameterCollection spaParameters = new FilterParameterCollection();
            spaParameters = spaParameters.AddReportFilterParameters(filter)
                                         .AddLoggedInUser(requestedUser)
                                         .AddOrderParameter(" ReportDate DESC, DepositDate DESC ");

            DataSet dsResult = _ApiDao.GetReportsAsDataSet("spa_APIReporting_GetPaymentSummaryByMerchant", spaParameters);
            return SetPaging(dsResult.ToReportResponse<DepositSummary>(true), filter);
        }

        public ReportResponse<DepositDetail> GetPaymentDetail(User requestedUser, MerchantSummaryFilter filter)
        {
            FilterParameterCollection spaParameters = new FilterParameterCollection();
            spaParameters = spaParameters.AddReportFilterParameters(filter)
                                         .AddLoggedInUser(requestedUser)
                                         .AddOrderParameter(" ReportDate DESC, DepositDate DESC ");

            DataSet dsResult = _ApiDao.GetReportsAsDataSet("spa_APIReporting_GetPaymentDetail", spaParameters);
            return SetPaging(dsResult.ToReportResponse<DepositDetail>(true), filter);
        }

        #endregion

        #region Transactions

        public ReportResponse<TransactionDetail> GetTransactions(User requestedUser, TransactionSearchFilter filter)
        {
            string betweenOrPlus = " BETWEEN {0} AND {1} ";
            string transAmountExp = null;
            if (filter.TransactionAmountFrom.HasValue)
            {
                switch (filter.TransactionOperatorValidate)
                {
                    case TransactionOperator.Between:
                        if (filter.TransactionAmountTo.HasValue)
                        {
                            transAmountExp = string.Format(betweenOrPlus, filter.TransactionAmountFrom, filter.TransactionAmountTo);
                        }
                        break;
                    case TransactionOperator.EqualTo:
                        transAmountExp = string.Format(" = {0} ", filter.TransactionAmountFrom);
                        break;
                    case TransactionOperator.GreaterThan:
                        transAmountExp = string.Format(" > {0} ", filter.TransactionAmountFrom);
                        break;
                    case TransactionOperator.LessThan:
                        transAmountExp = string.Format(" < {0} ", filter.TransactionAmountFrom);
                        break;
                    case TransactionOperator.PlusMinus5:
                        transAmountExp = string.Format(betweenOrPlus, filter.TransactionAmountFrom - 5, filter.TransactionAmountFrom + 5);
                        break;
                }
            }
            if (!string.IsNullOrEmpty(transAmountExp))
                transAmountExp = " AND td.TransactionAmount" + transAmountExp;

            FilterParameterCollection spaParameters = new FilterParameterCollection();
            spaParameters.AddPagingFilterParam(filter)
                         .AddDateFilterParam(filter)
                         .AddLoggedInUser(requestedUser)
                         .AddWhenNotNullOrEmpty("@TransAmount", transAmountExp)
                         .AddWhenNotNullOrEmpty("@First6CardNumber", filter.First6CardNumber)
                         .AddWhenNotNullOrEmpty("@Last4CardNumber", filter.Last4CardNumber)
                         .AddWhenNotNullOrEmpty("@AuthNumber", filter.AuthorizationNumber)
                         .Add("@HierarchyFilterMode", filter.HierarchyFilterMode.ToString(), DbType.AnsiString)
                         .AddOrderParameter("TransactionDate DESC,ReportDate DESC");
            if (!string.IsNullOrEmpty(filter.HierarchyFilterValue))
            {
                spaParameters.AddHierarchyFilterParam(filter);
            }
            DataSet dsResult = _ApiDao.GetReportsAsDataSet("spa_APIReporting_GetTransactions", spaParameters);
            return SetPaging(dsResult.ToReportResponse<TransactionDetail>(true, _mapCardNumber), filter);
        }

        public Voucher GetVoucher(User requestedUser, VoucherFilter filter)
        {
            FilterParameterCollection spaParameters = new FilterParameterCollection();
            spaParameters.AddHierarchyFilterModeParam(HierarchyFilterMode.MerchantNumber.ToString())
                         .AddHierarchyFilterValueParam(filter.MerchantNumber)
                         .AddReportDateParam(filter.ReportDateValidate)
                         .Add("@TransactionID", filter.TransactionId.ToString(), DbType.AnsiString)
                         .AddLoggedInUser(requestedUser);

            DataSet dsResult = _ApiDao.GetReportsAsDataSet("spa_64_api_GetVoucher", spaParameters);
            Voucher voucher = null;
            if (dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
                voucher = dsResult.Tables[0].Rows[0].To<Voucher>(_mapCardNumber);
            return voucher;
        }

        #endregion

        #region RetrievalsChargebacks
        public ReportResponse<ChargebackHierarchySummary> GetChargebackSummaryByEntity(User requestedUser, GenericReportFilter filter)
        {
            FilterParameterCollection spaParameters = new FilterParameterCollection();
            spaParameters = spaParameters.AddReportFilterParameters(filter)
                .AddLoggedInUser(requestedUser)
                .AddOrderParameter(GetSortExpression());

            DataSet dsResult = _ApiDao.GetReportsAsDataSet("spa_APIReporting_GetChargebacksSummary", spaParameters);
            return SetPaging(dsResult.ToReportResponse<ChargebackHierarchySummary>(true, _mapEntityId).AddEntityType(filter, filter.ViewLevel.ToString()), filter);
        }

        public ReportResponse<RetrievalHierarchySummary> GetRetrievalSummaryByEntity(User requestedUser, GenericReportFilter filter)
        {
            FilterParameterCollection spaParameters = new FilterParameterCollection();
            spaParameters = spaParameters.AddReportFilterParameters(filter)
                .AddLoggedInUser(requestedUser)
                .AddOrderParameter(GetSortExpression());

            DataSet dsResult = _ApiDao.GetReportsAsDataSet("spa_APIReporting_GetRetrievalsSummary", spaParameters);
            return SetPaging(dsResult.ToReportResponse<RetrievalHierarchySummary>(true, _mapEntityId).AddEntityType(filter, filter.ViewLevel.ToString()), filter);
        }

        public ReportResponse<RetrievalDetail> GetRetrievalDetail(User requestedUser, MerchantSummaryFilter filter)
        {
            FilterParameterCollection spaParameters = new FilterParameterCollection();
            spaParameters = spaParameters.AddReportFilterParameters(filter)
                .AddLoggedInUser(requestedUser);

            var maps = new Dictionary<string, string>() { 
                {"CardTypeDescription", "CardTypeDesc"},
                {"ReasonCodeDescription", "ReasonCodeDesc"}
            };

            DataSet dsResult = _ApiDao.GetReportsAsDataSet("spa_APIReporting_GetRetrievalsDetail", spaParameters);
            return SetPaging(dsResult.ToReportResponse<RetrievalDetail>(true, mapNames: maps), filter);
        }

        public ReportResponse<ChargebackDetail> GetChargebackDetail(User requestedUser, MerchantSummaryFilter filter)
        {
            FilterParameterCollection spaParameters = new FilterParameterCollection();
            spaParameters = spaParameters.AddReportFilterParameters(filter)
                                         .AddLoggedInUser(requestedUser);

            var maps = new Dictionary<string, string>() { 
                {"CardTypeDescription", "CardTypeDesc"},
                {"ReasonCodeDescription", "ReasonCodeDesc"},
                {"CBTypeDescription", "CBTypeDesc"},
                {"CBSequenceNumber", "CBSeqNo"}
            };

            DataSet dsResult = _ApiDao.GetReportsAsDataSet("spa_APIReporting_GetChargebacksDetail", spaParameters);
            return SetPaging(dsResult.ToReportResponse<ChargebackDetail>(true, mapNames: maps), filter);
        }

        #endregion

        #region Returns

        public ReportResponse<ReturnHierarchySummary> GetReturnSummaryByEntity(User requestedUser, GenericReportFilter filter)
        {
            FilterParameterCollection spaParameters = new FilterParameterCollection();
            spaParameters = spaParameters.AddReportFilterParameters(filter)
                                         .AddLoggedInUser(requestedUser)
                                         .AddOrderParameter(GetSortExpression());

            DataSet dsResult = _ApiDao.GetReportsAsDataSet("spa_APIReporting_GetReturnSummary", spaParameters);
            return SetPaging(dsResult.ToReportResponse<ReturnHierarchySummary>(true, _mapEntityId).AddEntityType(filter, filter.ViewLevel.ToString()), filter);
        }

        public ReportResponse<ReturnSummary> GetReturnSummaryByMerchant(User requestedUser, MerchantSummaryFilter filter)
        {
            FilterParameterCollection spaParameters = new FilterParameterCollection();
            spaParameters = spaParameters.AddReportFilterParameters(filter)
                                         .AddLoggedInUser(requestedUser)
                                         .AddOrderParameter(" ReportDate DESC , TransactionDate DESC , TransactionTime DESC ");

            DataSet dsResult = _ApiDao.GetReportsAsDataSet("spa_APIReporting_GetReturnSummaryByMerchant", spaParameters);
            return SetPaging(dsResult.ToReportResponse<ReturnSummary>(true, _mapCardNumber), filter);
        }

        #endregion

        #region VoidsRejects

        public ReportResponse<VoidRejectHierarchySummary> GetVoidsRejectSummaryByEntity(User requestedUser, GenericReportFilter filter)
        {
            FilterParameterCollection spaParameters = new FilterParameterCollection();
            spaParameters = spaParameters.AddReportFilterParameters(filter)
                                         .AddLoggedInUser(requestedUser)
                                         .AddOrderParameter(GetSortExpression());

            DataSet dsResult = _ApiDao.GetReportsAsDataSet("spa_64_api_GetVoidRejectSummary", spaParameters);
            return SetPaging(dsResult.ToReportResponse<VoidRejectHierarchySummary>(true, _mapEntityId).AddEntityType(filter, filter.ViewLevel.ToString()), filter);
        }

        public ReportResponse<VoidRejectDetail> GetVoidsRejectDetail(User requestedUser, MerchantSummaryFilter filter)
        {
            FilterParameterCollection spaParameters = new FilterParameterCollection();
            spaParameters = spaParameters.AddReportFilterParameters(filter)
                                         .AddLoggedInUser(requestedUser)
                                         .AddOrderParameter(" TransactionDate DESC, TransactionTime DESC ");

            DataSet dsResult = _ApiDao.GetReportsAsDataSet("spa_64_api_GetVoidRejectDetail", spaParameters);
            return SetPaging(dsResult.ToReportResponse<VoidRejectDetail>(true, _mapCardNumber), filter);
        }

        #endregion

        #region AuthorizationLog

        public ReportResponse<AuthorizationLogSummary> GetAuthorizationLogSummary(User requestedUser, GenericReportFilter filter, string reportType)
        {
            FilterParameterCollection spaParameters = new FilterParameterCollection();
            spaParameters = spaParameters.AddReportFilterParameters(filter)
                                         .AddLoggedInUser(requestedUser)
                                         .AddOrderParameter(GetSortExpression());


            DataSet dsResult = null;
            if (string.IsNullOrEmpty(reportType))
                dsResult = _ApiDao.GetReportsAsDataSet("spa_APIReporting_GetAuthorizationLogSummary", spaParameters);
            else
            {
                spaParameters.Add("@ReportType", reportType, DbType.AnsiString);
                dsResult = _ApiDao.GetReportsAsDataSet("spa_APIReporting_GetAuthorizationLogSummary", spaParameters);
            }
            return SetPaging(dsResult.ToReportResponse<AuthorizationLogSummary>(true, mapNames: _mapEntityId).AddEntityType(filter, filter.ViewLevel.ToString()), filter);
        }

        public ReportResponse<AuthorizationDetail> GetAuthorizationLogDetail(User requestedUser, MerchantSummaryFilter filter)
        {
            FilterParameterCollection spaParameters = new FilterParameterCollection();
            spaParameters = spaParameters.AddReportFilterParameters(filter)
                                         .AddLoggedInUser(requestedUser)
                                         .AddOrderParameter("ReportDate DESC, TransactionDate DESC");

            DataSet dsResult = _ApiDao.GetReportsAsDataSet("spa_APIReporting_GetAuthorizationDetail", spaParameters);
            return SetPaging(dsResult.ToReportResponse<AuthorizationDetail>(true, _mapCardNumber), filter);
        }

        #endregion

        #region MerchantInfomation

        public ReportResponse<MerchantSummary> GetMerchantList(User requestedUser, HierarchyPagingFilter filter)
        {
            FilterParameterCollection spaParameters = new FilterParameterCollection();
            spaParameters = spaParameters.AddPagingFilterParam(filter)
                                         .AddHierarchyFilterParam(filter)
                                         .AddLoggedInUser(requestedUser)
                                         .AddOrderParameter("Entity ASC");

            var maps = new Dictionary<string, string>() { 
                {"MerchantNumber","Entity"},
                {"MerchantName","EntityName"},
                {"HeadquarterMerchantNumber","Headquarter"},
                {"Bank","NBank"},
                {"Agent","NAgent"},
                {"Corp","NCorp"},
                {"NorthSalesAgent","NSalesAgent"},
                {"NorthChain","Chain"},
                {"MemphisChain","ChainID"}
            };

            DataSet dsResult = _ApiDao.GetReportsAsDataSet("spa_64_api_GetMerchantList", spaParameters);
            return SetPaging(dsResult.ToReportResponse<MerchantSummary>(mapNames: maps), filter);
        }


        public ReportResponse<MerchantComment> GetMerchantComments(User requestedUser, MerchantInformationPagingFilter filter)
        {
            var spaParameters = new FilterParameterCollection();
            spaParameters.AddMerchantNumberParam(filter.MerchantNumber)
                         .AddLoggedInUser(requestedUser)
                         .AddPagingFilterParam(filter)
                         .AddOrderParameter("CommentedDate DESC");

            DataSet dsResult = _ApiDao.GetReportsAsDataSet("spa_64_api_GetMerchantComments", spaParameters);
            return SetPaging(dsResult.ToReportResponse<MerchantComment>(), filter);
        }

        #endregion

    }
}
