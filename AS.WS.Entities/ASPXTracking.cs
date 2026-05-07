using System;

namespace AS.WS.Entities
{
    public class SsoTracking
    {
        public int ClientId { get; set; }
        public string UserId { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
        public string ClientIp { get; set; }
        public string HostIp { get; set; }
        public string SessionId { get; set; }
        public string AppId { get; set; }
    }
}
