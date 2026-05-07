using AS.VW.Entities;
using System;
using System.Collections.Generic;
using System.Data;

namespace AS.VW.Repository
{
    public interface IStatementRepository
    {
        List<DownLoadItem> GetPendingDownloadItems();
        void UpdateStatusDownloadItems(long? docId, long? recordId, StatementStatus status);
        List<StatementItem> GetStatementListByDownLoadItemId(long downLoadItemId);
        DataTable GetStatementDetail(string spaName, int asClientId, int siteId, string userId, string userMode,
                                    DateTime dateTime, string merchantNumber);
    }
}
