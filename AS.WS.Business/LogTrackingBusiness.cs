using AS.Common.DBManager;
using AS.Common.WebUI;
using AS.WS.Data;
using AS.WS.Entities;
using System;
using System.Data;
using System.Threading;

namespace AS.WS.Business
{
    public class LogTrackingBusiness
    {
        private readonly IReportingDao _LogDao = null;
        private readonly bool IsAsync = System.Configuration.ConfigurationManager.AppSettings["InsertSSOAsync"] == "true";
        public LogTrackingBusiness(string connString)
        {
            // Default is Merchant API
            _LogDao = new LogDao(connString);
        }

       public int InsertASPXTrackingLog(ILogObject trackingLog)
       {
           try
           {
               FilterParameterCollection parameters = new FilterParameterCollection();
               parameters.Add(new FilterParameter("@LogRecNo", 0, DbType.Int32));
               parameters.Add(new FilterParameter("@LogWebServerDts", trackingLog.LogWebServerDts, DbType.DateTime));
               parameters.Add(new FilterParameter("@LogId1", trackingLog.LogId1, DbType.String));
               parameters.Add(new FilterParameter("@LogId2", trackingLog.LogId2, DbType.String));
               parameters.Add(new FilterParameter("@LogRecordCount", trackingLog.LogRecordCount, DbType.Int32));
               parameters.Add(new FilterParameter("@LogElapsedTime", trackingLog.LogElapsedTime, DbType.Int32));
               parameters.Add(new FilterParameter("@LogMenuId", trackingLog.LogMenuId, DbType.Int32));
               parameters.Add(new FilterParameter("@LogSubMenuId", trackingLog.LogSubMenuId, DbType.Int32));
               parameters.Add(new FilterParameter("@LogSessionId", trackingLog.LogSessionId, DbType.String));
               parameters.Add(new FilterParameter("@LogSessionCnt", trackingLog.LogSessionCnt, DbType.Int32));
               parameters.Add(new FilterParameter("@LogLoggingMode", trackingLog.LogLoggingMode, DbType.Int32));
               parameters.Add(new FilterParameter("@LogSpecialId", trackingLog.LogSpecialId, DbType.Int32));
               parameters.Add(new FilterParameter("@LogClientId", trackingLog.LogClientId, DbType.Int32));
               parameters.Add(new FilterParameter("@LogSystemId", trackingLog.LogSystemId, DbType.Int32));
               parameters.Add(new FilterParameter("@LogFullName", trackingLog.LogFullName, DbType.String));
               parameters.Add(new FilterParameter("@LogWebSiteName", trackingLog.LogWebSiteName, DbType.String));
               parameters.Add(new FilterParameter("@LogData1", trackingLog.LogData1, DbType.String));
               parameters.Add(new FilterParameter("@LogData2", trackingLog.LogData2, DbType.String));
               parameters.Add(new FilterParameter("@LogData3", trackingLog.LogData3, DbType.String));
               parameters.Add(new FilterParameter("@LogData4", trackingLog.LogData4, DbType.String));
               parameters.Add(new FilterParameter("@LogData5", trackingLog.LogData5, DbType.String));
               parameters.Add(new FilterParameter("@LogData6", trackingLog.LogData6, DbType.String));
               parameters.Add(new FilterParameter("@LogData7", trackingLog.LogData7, DbType.String));
               parameters.Add(new FilterParameter("@LogData8", trackingLog.LogData8, DbType.String));
               parameters.Add(new FilterParameter("@LogData9", trackingLog.LogData9, DbType.String));
               parameters.Add(new FilterParameter("@LogData10", trackingLog.LogData10, DbType.String));
               parameters.Add(new FilterParameter("@LogTxt1", trackingLog.LogTxt1, DbType.String));
               parameters.Add(new FilterParameter("@LogTxt2", trackingLog.LogTxt2, DbType.String));
               parameters.Add(new FilterParameter("@LogClientIPAddr", trackingLog.LogClientIPAddr, DbType.String));
               parameters.Add(new FilterParameter("@LogHostIPAddr", trackingLog.LogHostIPAddr, DbType.String));
               parameters.Add(new FilterParameter("@LogBrowserType", trackingLog.LogBrowserType, DbType.String));
               DataSet result = _LogDao.GetReportsAsDataSet("spa_LOG_InsASPXTracking", parameters);
               
               int validCode = 0;

               if (result != null && result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
               {
                   validCode = int.Parse(result.Tables[0].Rows[0][0].ToString());
               }

               return validCode;
           }
           catch (Exception ex)
           {
               Common.Logger.LoggerManager.Error("InsASPXTracking: " + ex.Message);
               return 0;
           }
       }

        public void InsertSSOTrackingLog(SsoTracking tracking)
        {
            try
            {
                string spaName = "spa_LOG_Insert_SSO_Tracking";
                FilterParameterCollection parameters = new FilterParameterCollection();
                FilterParameterCollection outParameters;
                parameters.Add(new FilterParameter("@AsClientID", tracking.ClientId, DbType.Int32));
                parameters.Add(new FilterParameter("@UserId", tracking.UserId, DbType.String));
                parameters.Add(new FilterParameter("@Status", tracking.Status, DbType.Boolean));
                parameters.Add(new FilterParameter("@Message", tracking.Message, DbType.String));
                parameters.Add(new FilterParameter("@ClientIp", tracking.ClientIp, DbType.String));
                parameters.Add(new FilterParameter("@HostIp", tracking.HostIp, DbType.String));
                parameters.Add(new FilterParameter("@LoginSessionID", tracking.SessionId, DbType.String));
                parameters.Add(new FilterParameter("@AspSessionID", tracking.AppId, DbType.String));
                parameters.Add(new FilterParameter("@RequestedDate", DateTime.Now, DbType.String));

                if (IsAsync)
                {
                    StartThreadMethod(() =>
                    {
                        _LogDao.ExecuteNonQueryCommand(spaName, parameters, out outParameters);
                    });
                }
                else
                    _LogDao.ExecuteNonQueryCommand(spaName, parameters, out outParameters);
            }
            catch (Exception ex)
            {
                Common.Logger.LoggerManager.Error("InsertSSOTrackingLog: " + ex.Message);              
            }
        }

        private void StartThreadMethod(Action action)
        {
            var task = new Thread(() => SafeExecute(action, HandleExceptionInThread));
            task.Start();
        }

        /// <summary>
        /// Log errors in new threads
        /// </summary>
        /// <param name="exs"></param>
        private void HandleExceptionInThread(Exception ex)
        {
            Common.Logger.LoggerManager.Error(ex.ToString());
        }

        /// <summary>
        /// Try/cacth exceptions in threads
        /// </summary>
        /// <param name="action"></param>
        /// <param name="exHandler"></param>
        private void SafeExecute(Action action, Action<Exception> exHandler)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                exHandler(ex);
            }
        }
    }
}
