using AS.WS.Data;
using System.Data;
using System.Linq;
using AS.Common.DBManager;
using AS.WS.Mobile.Domain.Contracts;
using AS.WS.Mobile.Domain.Models;
using System;
using AS.VW.Common;

namespace AS.WS.MobileBusiness
{
    public partial class MobileService : IMobileService
    {
        private readonly MobileDao _mobileDAO;

        #region SpNames
        private static readonly string SP_GET_CHART_VOLUME = "spa_api_GetDashBoardYOY_VolumeChart";
        private static readonly string SP_GET_MONTHLY_VOLUME = "spa_api_GetDashboardYTDVolumeTransChart";
        private static readonly string SP_GET_CARD_VOLUME = "spa_api_GetDashboardCard";
        private static readonly string SP_GET_VOLUME_ANALYSIS = "spa_api_GetDashboardSale";

        private static readonly string SP_GET_BATCH_BY_REPORT_DATE = "spa_api_GetBatchByReportDate";
        private static readonly string SP_GET_BATCH_SUM_BY_DAYS = "spa_api_GetBatchSumByDays";
        private static readonly string SP_GET_BATCH_BY_BATCHNUMBER = "spa_api_GetBatchDetail";

        private static readonly string SP_GET_CHARGEBACK_BY_DAYS = "spa_api_GetChargebackSumByDays";
        private static readonly string SP_GET_CHARGEBACK_BY_REPORT_DATE = "spa_api_GetChargebackByReportDate";
        private static readonly string SP_GET_CHARGEBACK_DETAIL = "spa_api_GetChargebackDetail";

        private static readonly string SP_GET_MERCHANT_LIST = "spa_api_GetMerchantList";

        #endregion

        public MobileService(string connString)
        {
            _mobileDAO = new MobileDao(connString);
        }

        #region Common

        private DataTable GetReportByLastDays(MobileParameters ps, string spa)
        {
            FilterParameterCollection pr = new FilterParameterCollection();
            pr.AddCommonParams(ps);
            pr.AddHierarchyFilterValueParam(ps.HierarchyFilterMode, ps.HierarchyFilterValue);
            pr.AddPagingParam(ps.PageNo, ps.PageSize, ps.IsPaging);
            pr.Add(new FilterParameter()
            {
                ParameterName = "@BeginDate",
                ParameterType = DbType.DateTime,
                ParameterValue = ps.BeginDate
            });
            pr.Add(new FilterParameter()
            {
                ParameterName = "@EndDate",
                ParameterType = DbType.DateTime,
                ParameterValue = ps.EndDate
            });
            DataSet ds = _mobileDAO.GetReportsAsDataSet(spa, pr);
            if (ds != null && ds.Tables.Count >= 1)
                return ds.Tables[0];
            return null;
        }

        private DataTable GetReportByMonth(MobileParameters ps, string spa)
        {
            FilterParameterCollection pr = new FilterParameterCollection();
            pr.AddCommonParams_New(ps);
            DataSet ds = _mobileDAO.GetReportsAsDataSet(spa, pr);
            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];
            return null;
        }

        private DataTable GetReportByChart(MobileParameters ps, string spa)
        {
            FilterParameterCollection pr = new FilterParameterCollection();
            pr.AddCommonParams_New(ps);
            pr.AddDashboardModeParam("@Mode", ps.Mode);
            DataSet ds = _mobileDAO.GetReportsAsDataSet(spa, pr);
            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];
            return null;
        }

        private DataTable GetReportByReportDate(MobileParameters ps, string spa)
        {
            FilterParameterCollection pr = new FilterParameterCollection();
            pr.AddCommonParams(ps);
            pr.AddPagingParam(ps.PageNo, ps.PageSize, ps.IsPaging);
            pr.Add(new FilterParameter()
            {
                ParameterName = "@ReportDate",
                ParameterType = DbType.DateTime,
                ParameterValue = ps.ReportDate
            });
            pr.Add(new FilterParameter()
            {
                ParameterName = "@MerchantNumber",
                ParameterType = DbType.String,
                ParameterValue = ps.Mid
            });

            DataSet ds = _mobileDAO.GetReportsAsDataSet(spa, pr);
            if (ds != null && ds.Tables.Count >= 1)
                return ds.Tables[0];
            return null;
        }

        private DataTable[] GetBatchDetail(MobileParameters ps, string spa)
        {
            FilterParameterCollection pr = new FilterParameterCollection();
            pr.AddCommonParams(ps);
            pr.AddPagingParam(ps.PageNo, ps.PageSize, ps.IsPaging);
            pr.Add(new FilterParameter()
            {
                ParameterName = "@ReportDate",
                ParameterType = DbType.DateTime,
                ParameterValue = ps.ReportDate
            });
            pr.Add(new FilterParameter()
            {
                ParameterName = "@MerchantNumber",
                ParameterType = DbType.String,
                ParameterValue = ps.Mid
            });
            pr.Add(new FilterParameter()
            {
                ParameterName = "@BatchNumber",
                ParameterType = DbType.String,
                ParameterValue = ps.BatchNumber
            });
            pr.Add(new FilterParameter()
            {
                ParameterName = "@RecordID",
                ParameterType = DbType.Int64,
                ParameterValue = ps.RecordId
            });
            DataSet ds = _mobileDAO.GetReportsAsDataSet(spa, pr);
            if (ds != null && ds.Tables.Count >= 2)
                return new DataTable[] { ds.Tables[0], ds.Tables[1] };

            return new DataTable[0];
        }

        private DataTable[] GetReportByReportDate2Table(MobileParameters ps, string spa)
        {
            FilterParameterCollection pr = new FilterParameterCollection();
            pr.AddCommonParams(ps);
            pr.AddPagingParam(ps.PageNo, ps.PageSize, ps.IsPaging);
            pr.Add(new FilterParameter()
            {
                ParameterName = "@ReportDate",
                ParameterType = DbType.DateTime,
                ParameterValue = ps.ReportDate
            });
            pr.Add(new FilterParameter()
            {
                ParameterName = "@MerchantNumber",
                ParameterType = DbType.String,
                ParameterValue = ps.Mid
            });

            DataSet ds = _mobileDAO.GetReportsAsDataSet(spa, pr);
            if (ds != null && ds.Tables.Count >= 2)
                return new DataTable[] { ds.Tables[0], ds.Tables[1] };

            return new DataTable[0];
        }

        private DataTable GetRecordDetailsByRecId(MobileParameters ps, string spa)
        {
            FilterParameterCollection pr = new FilterParameterCollection();
            pr.AddCommonParams(ps);
            pr.Add(new FilterParameter()
            {
                ParameterName = "@ReportDate",
                ParameterType = DbType.DateTime,
                ParameterValue = ps.ReportDate
            });
            pr.Add(new FilterParameter()
            {
                ParameterName = "@MerchantNumber",
                ParameterType = DbType.String,
                ParameterValue = ps.Mid
            });
            pr.Add(new FilterParameter()
            {
                ParameterName = "@RecordID",
                ParameterType = DbType.Int64,
                ParameterValue = ps.RecordId
            });

            DataSet ds = _mobileDAO.GetReportsAsDataSet(spa, pr);
            if (ds != null && ds.Tables.Count >= 1)
                return ds.Tables[0];
            return null;
        }

        public Merchants GetMerchantList(MobileParameters parameters)
        {
            FilterParameterCollection pr = new FilterParameterCollection();
            pr.AddCommonParams(parameters);
            pr.AddHierarchyFilterValueParam(parameters.HierarchyFilterMode, parameters.HierarchyFilterValue);
            pr.AddPagingParam(parameters.PageNo, parameters.PageSize, parameters.IsPaging);

            DataSet ds = _mobileDAO.GetReportsAsDataSet(SP_GET_MERCHANT_LIST, pr);
            if (ds == null || ds.Tables.Count == 0)
                return null;

            return new Merchants()
            {
                Items = ds.Tables[0].To<Merchant>().ToList()
            };
        }

        public DataTable GetAuditUserChanges(MobileParameters ps)
        {
            FilterParameterCollection _parames = new FilterParameterCollection();
            _parames.AddCommonParams_New(ps);
            _parames.Add(new FilterParameter("@ChangedByRecID", ps.RecId, DbType.Guid));
            _parames.Add(new FilterParameter("@ChangedUserID", ps.UserID, DbType.AnsiString));

            DataSet ds = _mobileDAO.GetReportsAsDataSet("spa_GetAuditUserChangesOfUser", _parames);
            if (ds == null || ds.Tables.Count == 0)
                return null;
            return ds.Tables[0];
        }

        public string GetParentEntityNumber(MobileParameters ps)
        {
            FilterParameterCollection _parames = new FilterParameterCollection();
            _parames.AddCommonParamsNoneSiteID(ps);
            _parames.Add("@ParentEntityType", ps.EntityTypeID, DbType.Int32);
            DataSet ds = _mobileDAO.GetReportsAsDataSet("spa_GetParentEntityNumber", _parames);
            if (ds == null || ds.Tables.Count == 0)
                return null;
            DataTable data = ds.Tables[0];
            if (data != null && data.Rows.Count > 0)
            {
                if (data.Rows[0]["EntityNumber"] == null || data.Rows[0]["EntityNumber"] == DBNull.Value) return string.Empty;
                return data.Rows[0]["EntityNumber"].ToString();
            }
            return string.Empty;
        }
        #endregion
    }
}
