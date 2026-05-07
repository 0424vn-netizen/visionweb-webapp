using AS.Common.DataProtection;
using AS.Common.WebUI;
using Microsoft.Web.Services3.Security.Tokens;
using System;

namespace AS.Web.LogServices
{
    public class LogService : ASReportBusiness
    {
        private readonly TrackingLogService.TrackingLogServices _Service = null;
        public int ClientId { get; set; }

        public void AddRequestHeader(string name, string value)
        {
            _Service.AddRequestHeader(name, value);
        }

         public LogService()
        {
            string userName = Cryptophy.DecryptText(System.Configuration.ConfigurationManager.AppSettings["SEC_WS_Token1"]);
            string passWord = Cryptophy.DecryptText(System.Configuration.ConfigurationManager.AppSettings["SEC_WS_Token2"]);
            UsernameToken token = new UsernameToken(userName, passWord, PasswordOption.SendPlainText);
           
            _Service = new TrackingLogService.TrackingLogServices();            
            _Service.Url = System.Configuration.ConfigurationManager.AppSettings["LOG_WS_URL"]; 
            _Service.SetClientCredential(token);
            _Service.SetPolicy("ClientPolicy");
            this.ReportWS = _Service;
        }

         public int InsertASPXTrackingLog(ILogObject trackingLog)
         {
             return _Service.InsertASPXTrackingLog(trackingLog.LogWebServerDts, trackingLog.LogId1, trackingLog.LogId2, trackingLog.LogRecordCount, trackingLog.LogElapsedTime, 
                 trackingLog.LogMenuId, trackingLog.LogSubMenuId, trackingLog.LogSessionId, trackingLog.LogSessionCnt, trackingLog.LogLoggingMode, trackingLog.LogSpecialId,
                 trackingLog.LogClientId, trackingLog.LogSystemId, trackingLog.LogFullName, trackingLog.LogWebSiteName, trackingLog.LogData1, trackingLog.LogData2, 
                 trackingLog.LogData3, trackingLog.LogData4, trackingLog.LogData5, trackingLog.LogData6, trackingLog.LogData7, trackingLog.LogData8, trackingLog.LogData9,
                 trackingLog.LogData10, trackingLog.LogTxt1, trackingLog.LogTxt2, trackingLog.LogClientIPAddr, trackingLog.LogHostIPAddr, trackingLog.LogBrowserType);
         }

        public void InsertSSOTrackingLog(TrackingLogService.SsoTracking tracking)
        {
            _Service.InsertSSOTrackingLog(tracking);
        }
    }
}
