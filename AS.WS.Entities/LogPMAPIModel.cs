using System;

namespace AS.WS.Entities
{
    public class LogPmApiModel
    {
        public Guid RequestId { get; set; }
        public string ProcessingStep { get; set; }
        public string Message { get; set; }
        public string Data1 { get; set; }
        public string Data2 { get; set; }
        public string Data3 { get; set; }
        public string Data4 { get; set; }
        public string Data5 { get; set; }
        public string XmlMessage { get; set; }
        public string XmlErrorResponse { get; set; }
        public DateTime RequestDts { get; set; }
    }
}
