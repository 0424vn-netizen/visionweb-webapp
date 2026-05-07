using System;

namespace AS.Security.WS.Entities
{
	public class ValidateUserModel
	{
		public int ClientId { get; set; }
		public string UserName { get; set; }
		public string BarePassword { get; set; }
		public string RemoteIp { get; set; }
		public string HostIp { get; set; }
		public string BrowserType { get; set; }
		public string LogSessionID { get; set; }
		public string Sessionid { get; set; }
	}
}
