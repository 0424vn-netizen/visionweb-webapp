using AS.WS.Mobile.Domain.Models;
using System.Linq;
using System.Collections.Generic;
using AS.Common.DBManager;
using System.Data;
using AS.WS.Mobile.Domain;
using System;
using AS.VW.Common;

namespace AS.WS.MobileBusiness
{
    public partial class MobileService
    {
        private static readonly string SP_GET_STMT_BY_MONTHS = "spa_api_GetStatementByMonths";
        private static readonly string SP_GET_STMT_BY_REPORT_DATE = "spa_api_GetStatementByReportDate";
        private static readonly string SP_GET_STMT_DETAIL = "spa_api_GetStatementDetail";
        private static readonly string SP_GET_STMT_TOTAL = "spa_api_stmnt_GetStatementTotal";

        public Statements GetStatementByMonths(MobileParameters ps)
        {
            DataTable data = null;
            string spa = SP_GET_STMT_BY_MONTHS;
            FilterParameterCollection pr = new FilterParameterCollection();
            pr.AddCommonParams(ps);
            pr.AddHierarchyFilterValueParam(ps.HierarchyFilterMode, ps.HierarchyFilterValue);
            pr.AddPagingParam(ps.PageNo, ps.PageSize, ps.IsPaging);

            DataSet ds = _mobileDAO.GetReportsAsDataSet(spa, pr);
            if (ds != null && ds.Tables.Count >= 1)
                data = ds.Tables[0];

            if (data == null)
                return null;

            var result = new Statements()
            {
                Items = data.To<Statement>().ToList()
            };

            return result;
        }

        public ChainStatement GetChainStatementByMonths(MobileParameters ps)
        {
            var data = GetChainStatementByMonths(ps, "spa_api_GetHierarchyStatement");
            if (data == null)
            {
                return null;
            }

            var result = new ChainStatement()
            {
                Items = data.To<ChainStatement>().ToList()
            };

            return result;
        }

        public DataTable GetChainStatementByMonths(MobileParameters ps, string spa)
        {
            FilterParameterCollection pr = new FilterParameterCollection();

            pr.AddCommonParams(ps);
            pr.AddHierarchyFilterValueParam(ps.HierarchyFilterMode, ps.HierarchyFilterValue);
            pr.AddPagingParam(ps.PageNo, ps.PageSize, ps.IsPaging);
            DataSet ds = _mobileDAO.GetReportsAsDataSet(spa, pr);
            if (ds != null && ds.Tables.Count >= 1)
                return ds.Tables[0];
            return null;
        }

        /// <summary>
        /// Get Statement details: SettlementSummary, SurchargeSummary, OtherFeesSummary
        /// </summary>
        /// <param name="ps">The parameter</param>
        /// <returns>Statement Detail</returns>
        public StatementDetail GetStatementByReportDate(MobileParameters ps)
        {
            var pr = new FilterParameterCollection();
            pr.AddCommonParams(ps);
            pr.AddHierarchyFilterValueParam(ps.HierarchyFilterMode, ps.HierarchyFilterValue);
            pr.Add(new FilterParameter()
            {
                ParameterName = "@ReportDate",
                ParameterType = DbType.DateTime,
                ParameterValue = ps.ReportDate
            });
            DataSet ds = _mobileDAO.GetReportsAsDataSet(SP_GET_STMT_BY_REPORT_DATE, pr);

            if (ds == null)
            {
                return null;
            }

            var statementDetail = new StatementDetail();
            if (ds.Tables.Count == 3)
            {
                var data = new DataTable[] { ds.Tables[0], ds.Tables[1], ds.Tables[2] };

                // Mapping SettlementSummary, SurchargeSummary, OtherFeesSummary
                if (data[0] != null && data[0].To<SettlementSummary>().Any())
                {
                    statementDetail.SettlementSummary = data[0].Rows[0].To<SettlementSummary>();
                }
                if (data[1] != null && data[1].To<SurchargeSummary>().Any())
                {
                    statementDetail.SurchargeSummary = data[1].Rows[0].To<SurchargeSummary>();
                }
                if (data[2] != null && data[2].To<OtherFeesSummary>().Any())
                {
                    statementDetail.OtherFeesSummary = data[2].Rows[0].To<OtherFeesSummary>();
                    statementDetail.WRFCFeesSummary = data[2].Rows[0].To<WrfcFeesSummary>();
                }
            }
            return statementDetail;
        }

        public StatementDetail GetStatementByStatementId(MobileParameters ps)
        {
            var data = GetStatementDetail(ps, SP_GET_STMT_DETAIL);
            if (data == null || data.Length < 1)
            {
                return null;
            }

            var statementDetail = new StatementDetail();
            if (data.Count() == 1 && data[0].Rows.Count > 0)
            {
                if (ps.StatementDetailType == (int)StatementDetailType.Plan)
                    statementDetail.PlanSummary = data[0].Rows[0].To<PlanSummary>();

                if (ps.StatementDetailType == (int)StatementDetailType.Deposit)
                {
                    statementDetail.DepositSummary = data[0].Rows[0].To<DepositSummary>();
                    statementDetail.DepositWRFCSummary = data[0].Rows[0].To<DepositWrfcSummary>();
                }
                if (ps.StatementDetailType == (int)StatementDetailType.Adjustment)
                    statementDetail.AdjustmentWRFCSummary = data[0].Rows[0].To<AdjustmentWrfcSummary>();

                if (ps.StatementDetailType == (int)StatementDetailType.Chargeback)
                    statementDetail.ChargebackSummary = data[0].Rows[0].To<ChargebackSummary>();

                if (ps.StatementDetailType == (int)StatementDetailType.Card)
                    statementDetail.CardSummary = data[0].Rows[0].To<CardSummary>();

                if (ps.StatementDetailType == (int)StatementDetailType.More)
                    statementDetail.MoreSummary = data[0].Rows[0].To<MoreSummary>();
            }
            return statementDetail;
        }

        public SettlementDetailsSummary GetSettlementDetailsByStatementId(MobileParameters ps)
        {
            var data = GetStatementDetail(ps, SP_GET_STMT_DETAIL);
            if (data == null || data.Length < 1)
            {
                return null;
            }

            var settlementDetailsSummary = new SettlementDetailsSummary();
            //TK: 42609 - Update Settlement/Discount and Other Fees Tabs on VW Mobile
            if (data.Count() == 2 && data[0].Rows.Count > 0 && data[1].Rows.Count > 0
                && ps.StatementDetailType == (int)StatementDetailType.SettlementDiscount)
            {
                settlementDetailsSummary.Items = data[0].To<SettlementDetails>().ToList();
                settlementDetailsSummary.TotalItems = data[1].Rows[0]["TotalItems"].IsNullData() ? 0 : long.Parse(data[1].Rows[0]["TotalItems"].ToString());
                settlementDetailsSummary.TotalAmount = data[1].Rows[0]["TotalAmount"].IsNullData() ? 0 : decimal.Parse(data[1].Rows[0]["TotalAmount"].ToString());
            }
            return settlementDetailsSummary;
        }

        public SurchargeDetailsSummary GetSurchargeDetailsByStatementId(MobileParameters ps)
        {
            var data = GetStatementDetail(ps, SP_GET_STMT_DETAIL);
            if (data == null || data.Length < 1)
            {
                return null;
            }
            var surchargeDetailsSummary = new SurchargeDetailsSummary();
            if (data.Count() == 2 && data[0].Rows.Count > 0 && data[1].Rows.Count > 0
                && ps.StatementDetailType == (int)StatementDetailType.Surcharge)
            {
                surchargeDetailsSummary.Items = data[0].To<SurchargeDetails>().ToList();
                surchargeDetailsSummary.TotalItems = data[1].Rows[0]["TotalItems"].IsNullData() ? 0 : long.Parse(data[1].Rows[0]["TotalItems"].ToString());
                surchargeDetailsSummary.TotalAmount = data[1].Rows[0]["TotalAmount"].IsNullData() ? 0 : decimal.Parse(data[1].Rows[0]["TotalAmount"].ToString());
            }
            return surchargeDetailsSummary;
        }

        public OtherFeesDetailsSummary GetOtherFeesDetailsByStatementId(MobileParameters ps)
        {
            var data = GetStatementDetail(ps, SP_GET_STMT_DETAIL);
            if (data == null || data.Length < 1)
            {
                return null;
            }

            var otherFeesDetailsSummary = new OtherFeesDetailsSummary();
            //TK: 42609 - Update Settlement/Discount and Other Fees Tabs on VW Mobile
            if (data.Count() == 2 && data[0].Rows.Count > 0 && data[1].Rows.Count > 0)
            {
                if (ps.StatementDetailType == (int)StatementDetailType.OtherFees)
                    otherFeesDetailsSummary.Items = data[0].To<OtherFeesDetails>().ToList();
                otherFeesDetailsSummary.TotalAmount = data[1].Rows[0]["TotalAmount"].IsNullData() ? 0 : decimal.Parse(data[1].Rows[0]["TotalAmount"].ToString());
            }
            return otherFeesDetailsSummary;
        }


        public StatementDetail GetStatementTotalByStatementId(MobileParameters ps)
        {
            var data = GetStatementDetail(ps, SP_GET_STMT_TOTAL);
            if (data == null || data.Length < 1)
            {
                return null;
            }

            var statementDetail = new StatementDetail();
            if (data.Count() == 1 && data[0].Rows.Count > 0 && ps.StatementDetailType == (int)StatementDetailType.More)
            {
                statementDetail.MoreWRFCSummary = data[0].Rows[0].To<MoreWrfcSummary>();
            }
            return statementDetail;
        }

        public DepositItemSummary GetDepositItemByStatementId(MobileParameters ps)
        {
            var data = GetStatementDetail(ps, SP_GET_STMT_DETAIL);
            if (data == null || data.Length < 1)
            {
                return null;
            }

            var result = new DepositItemSummary();
            if (data.Count() == 1 && data[0].Rows.Count > 0 && ps.StatementDetailType == (int)StatementDetailType.DepositItemSummary)
            {
                    result.Items = data[0].To<DepositItem>().ToList();
            }
            return result;
        }

        public MonthlyMessages GetMonthlyMessages(MobileParameters ps)
        {
            var data = GetStatementDetail(ps, SP_GET_STMT_DETAIL);
            if (data == null || data.Length < 1)
            {
                return null;
            }

            var monthlyMsg = new MonthlyMessages();
            if (data.Count() == 1 && data[0].Rows.Count > 0 && ps.StatementDetailType == (int)StatementDetailType.MonthlyMessages)
            {
                monthlyMsg.Messages = data[0].To<MessagesItem>().ToList();
            }

            return monthlyMsg;
        }

        private DataTable[] GetStatementDetail(MobileParameters ps, string spa)
        {
            FilterParameterCollection pr = new FilterParameterCollection();
            pr.AddCommonParams(ps);
            pr.AddHierarchyFilterValueParam(ps.HierarchyFilterMode, ps.HierarchyFilterValue);
            pr.Add(new FilterParameter()
            {
                ParameterName = "@ReportDate",
                ParameterType = DbType.DateTime,
                ParameterValue = ps.ReportDate
            });
            pr.Add(new FilterParameter()
            {
                ParameterName = "@StatementType",
                ParameterType = DbType.Int32,
                ParameterValue = ps.StatementDetailType
            });
            DataSet ds = _mobileDAO.GetReportsAsDataSet(spa, pr);
            if (ds != null && ds.Tables.Count == 1)
                return new DataTable[] { ds.Tables[0] };
            if (ds != null && ds.Tables.Count == 2)
                return new DataTable[] { ds.Tables[0], ds.Tables[1] };
            return new DataTable[0];
        }

        public DataTable CheckMerchantIsConvertion(MobileParameters ps, string merchantNumber)
        {
            string spaName = "spa_api_CheckMerchantIsConvertion";
            FilterParameterCollection paras = new FilterParameterCollection();
            paras.Add(new FilterParameter("@MerchantNumber", merchantNumber, DbType.AnsiString));
            paras.Add(new FilterParameter("@ASClient", ps.ASClientID, DbType.Int32));
            paras.Add(new FilterParameter("@UserID", ps.UserID, DbType.AnsiString));
            paras.Add(new FilterParameter("@UserMode", ps.UserMode, DbType.AnsiString));
            paras.Add(new FilterParameter("@SiteID", ps.SiteID, DbType.Int32));

            return _mobileDAO.GetReports(spaName, paras);
        }

        public bool CheckMerchantBelongToUser(MobileParameters ps, string merchantNumber)
        {
            string spaName = "spa_CheckMerchantBelongtoUser";
            FilterParameterCollection paras = new FilterParameterCollection();
            paras.Add(new FilterParameter("@MerchantNumber", merchantNumber, DbType.AnsiString));
            paras.Add(new FilterParameter("@ASClient", ps.ASClientID, DbType.Int32));
            paras.Add(new FilterParameter("@UserID", ps.UserID, DbType.AnsiString));
            paras.Add(new FilterParameter("@UserMode", ps.UserMode, DbType.AnsiString));
            paras.Add(new FilterParameter("@SiteID", ps.SiteID, DbType.Int32));

            var dt = _mobileDAO.GetReports(spaName, paras);
            if (dt != null && dt.Rows.Count > 0 && (bool)dt.Rows[0]["IsBelongTo"])
                return true;
            return false;
        }

        public DataTable UpdateStatementTracking(MobileParameters ps)
        {
            FilterParameterCollection parameterList = new FilterParameterCollection();
            parameterList.AddCommonParams_New(ps);
            parameterList.Add(new FilterParameter("@UserRecID", ps.RecId, DbType.Guid));
            parameterList.Add(new FilterParameter("@EntityTypeID", ps.EntityTypeID, DbType.Int32));
            parameterList.Add(new FilterParameter("@EntityNumber", ps.EntityNumber, DbType.String));
            parameterList.Add(new FilterParameter("@DocID", ps.DocId, DbType.String));
            parameterList.Add(new FilterParameter("@ReportDate", ps.ReportDate, DbType.DateTime));
            parameterList.Add(new FilterParameter("@FileName", ps.StatementName, DbType.String));
            parameterList.Add(new FilterParameter("@FromSource", "Mobile", DbType.String));
            parameterList.Add(new FilterParameter("@HierarchyNumber", ps.HierarchyStatementValue, DbType.String));
            parameterList.Add(new FilterParameter("@HierarchyMode", ps.HierarchyStatementMode, DbType.String));

            return _mobileDAO.GetReports("spa_Statement_TrackingLog", parameterList);
        }
    }
}
