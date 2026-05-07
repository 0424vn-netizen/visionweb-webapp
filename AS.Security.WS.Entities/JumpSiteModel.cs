using System;

namespace AS.Security.WS.Entities
{
	public class JumpSiteModel
	{
		public Guid UserId { get; set; }
		public Guid Jumper { get; set; }
		public string TempPassword { get; set; }
		public string RemoteIp { get; set; }
		public int DesSysId { get; set; }
		public int ClientId { get; set; }
		public string HostIp { get; set; }
		public string Browser { get; set; }
	}
}
