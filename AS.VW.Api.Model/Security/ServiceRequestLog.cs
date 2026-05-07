using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Api.Model.Security
{
    public class ServiceRequestLog
    {
        public int LogRecNo { get; set; }
        public DateTime LogDts { get; set; }
        public DateTime LogWebServerDts { get; set; }
        public DateTime LogRequestStartDts { get; set; }
        public string LogId1 { get; set; }
        public string LogId2 { get; set; }
        public int LogRecordCount { get; set; }
        public int LogElapsedTime { get; set; }
        public int LogMenuId { get; set; }
        public int LogSubMenuId { get; set; }
        public string LogSessionId { get; set; }
        public int LogSessionCount { get; set; }
        public int LogLoggingMode { get; set; }
        public int LogSpecialId { get; set; }
        public int LogClientId { get; set; }
        public int LogSystemId { get; set; }
        public string LogFullName { get; set; }
        public string LogWebSiteName { get; set; }
        public string LogData1 { get; set; }
        public string LogData2 { get; set; }
        public string LogData3 { get; set; }
        public string LogData4 { get; set; }
        public string LogData5 { get; set; }
        public string LogData6 { get; set; }
        public string LogData7 { get; set; }
        public string LogData8 { get; set; }
        public string LogData9 { get; set; }
        public string LogData10 { get; set; }
        public string LogTxt1 { get; set; }
        public string LogTxt2 { get; set; }
        public string LogClientIpAddress { get; set; }
        public string LogHostIpAddress { get; set; }
        public string LogBrowserType { get; set; }
    }
}
