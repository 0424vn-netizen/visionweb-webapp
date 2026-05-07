using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using AS.Security.WS.Entities;
using AS.Common.DBManager;

namespace AS.Security.WS.Data
{
    public class LogDao : BaseSecDao
    {
        public LogDao(string connString) : base(connString) { }

        public void Insert(Intruders item)
        {
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_LOG_InsIntruders", item.LogRecNo, item.LogWebServerDts, item.LogId1, item.LogId2, item.LogRecordCount, item.LogElapsedTime, item.LogMenuId, item.LogSubMenuId, item.LogSessionId, item.LogSessionCnt, item.LogLoggingMode, item.LogSpecialId, item.LogClientId, item.LogSystemId, item.LogFullName, item.LogWebSiteName, item.LogData1, item.LogData2, item.LogData3, item.LogData4, item.LogData5, item.LogData6, item.LogData7, item.LogData8, item.LogData9, item.LogData10, item.LogTxt1, item.LogTxt2, item.LogClientIPAddr, item.LogHostIPAddr, item.LogBrowserType);
            base.ExecuteNonQuery(command);
            command.Dispose();
        }

        public int Insert(AspxTracking item)
        {
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_LOG_InsASPXTracking", item.LogRecNo, item.LogWebServerDts, item.LogId1, item.LogId2, item.LogRecordCount, item.LogElapsedTime, item.LogMenuId, item.LogSubMenuId, item.LogSessionId, item.LogSessionCnt, item.LogLoggingMode, item.LogSpecialId, item.LogClientId, item.LogSystemId, item.LogFullName, item.LogWebSiteName, item.LogData1, item.LogData2, item.LogData3, item.LogData4, item.LogData5, item.LogData6, item.LogData7, item.LogData8, item.LogData9, item.LogData10, item.LogTxt1, item.LogTxt2, item.LogClientIPAddr, item.LogHostIPAddr, item.LogBrowserType);
            DataSet result = base.ExecuteDataSet(command);
            int validCode = 0;

            if (result != null && result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
            {
                validCode = int.Parse(result.Tables[0].Rows[0][0].ToString());
            }
            command.Dispose();

            return validCode;
        }

        

        
    }
}
