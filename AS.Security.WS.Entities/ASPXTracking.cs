using System;

namespace AS.Security.WS.Entities
{
    public class AspxTracking: BaseTracking, AS.Common.WebUI.ILogObject
    {}

	public class BaseTracking
	{
		public string LogBrowserType { get; set; }
		public string LogTxt2 { get; set; }
		public string LogTxt1 { get; set; }
		public int LogSystemId { get; set; }
		public int LogSubMenuId { get; set; }
		public int LogSpecialId { get; set; }
		public string LogSessionId { get; set; }
		public int LogSessionCnt { get; set; }
		public int LogRecordCount { get; set; }
		public int LogRecNo { get; set; }
		public int LogMenuId { get; set; }
		public int LogLoggingMode { get; set; }
		public string LogId2 { get; set; }
		public string LogId1 { get; set; }
		public string LogHostIPAddr { get; set; }
		public string LogFullName { get; set; }
		public int LogElapsedTime { get; set; }
		public DateTime LogDts { get; set; }
		public string LogData9 { get; set; }
		public string LogData8 { get; set; }
		public string LogData7 { get; set; }
		public string LogData6 { get; set; }
		public string LogData5 { get; set; }
		public string LogData4 { get; set; }
		public string LogData3 { get; set; }
		public string LogData2 { get; set; }
		public string LogData10 { get; set; }
		public string LogData1 { get; set; }
		public string LogClientIPAddr { get; set; }
		public int LogClientId { get; set; }
		public DateTime LogWebServerDts { get; set; }
		public string LogWebSiteName { get; set; }
	}

}