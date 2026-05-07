using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AS.Common.DBManager;
using AS.VW.Entities;
using AS.VW.Common;

namespace AS.VW.Repository
{
    public class WsStatementRepository: IStatementRepository
    {
        public List<DownLoadItem> GetPendingDownloadItems()
        {
            FilterParameterCollection param = new FilterParameterCollection();

            DataTable dataProcess = WebServices.ReportServices.GetReports("spa_Statement_GetPendingDownloadItem", param);

            return dataProcess.ConvertDataTable<DownLoadItem>();
        }

        public void UpdateStatusDownloadItems(long? docId, long? recordId, StatementStatus status)
        {
            FilterParameterCollection paramIn = new FilterParameterCollection();
            FilterParameterCollection paramOut;
            paramIn.Add(new FilterParameter("@DocID", docId, DbType.Int64));
            paramIn.Add(new FilterParameter("@RecordID", recordId, DbType.Int64));
            paramIn.Add(new FilterParameter("@Status", (int)status, DbType.Int32));

            WebServices.ReportServices.ExecuteNonQueryCommand("spa_Statement_UpdateStatusDownloadItem", paramIn, out paramOut);
        }

        public List<StatementItem> GetStatementListByDownLoadItemId(long downLoadItemId)
        {
            FilterParameterCollection paramIn = new FilterParameterCollection();
            paramIn.Add(new FilterParameter("@StatementID", downLoadItemId, DbType.Int64));

            DataTable dataProcess = WebServices.ReportServices.GetReports("spa_Statement_GetDownloadItemDetail", paramIn);

            return dataProcess.ConvertDataTable<StatementItem>();
        }

        public DataTable GetStatementDetail(string spaName, int asClientId, int siteId, string userId, string userMode, DateTime dateTime, string merchantNumber)
        {
            var paramIn = new FilterParameterCollection();
            paramIn.Add(new FilterParameter("@ASClient", asClientId, DbType.Int32));
            paramIn.Add(new FilterParameter("@UserID", userId, DbType.String));
            paramIn.Add(new FilterParameter("@UserMode", userMode, DbType.String));
            paramIn.Add(new FilterParameter("@SiteID", siteId, DbType.Int32));
            paramIn.Add(new FilterParameter("@DateFilterMode", 2, DbType.Int32));
            paramIn.Add(new FilterParameter("@BeginDate", dateTime, DbType.DateTime));
            paramIn.Add(new FilterParameter("@EndDate", dateTime, DbType.DateTime));
            paramIn.Add(new FilterParameter("@MerchantNumber", merchantNumber, DbType.String));

            return WebServices.ReportServices.GetReports(spaName, paramIn);
        }
    }
}
