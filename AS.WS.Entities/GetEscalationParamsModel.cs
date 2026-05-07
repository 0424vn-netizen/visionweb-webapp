using System;

namespace AS.WS.Entities
{
    public class GetEscalationParamsModel
    {
        public string AssignedToList { get; set; }
        public string ResolutionList { get; set; }
        public string StatusList { get; set; }
        public string OpenClosedCode { get; set; }
        public DateTime OpenClosedFromDate { get; set; }
        public DateTime OpenClosedToDate { get; set; }
        public string KeyType { get; set; }
        public string KeyValue { get; set; }
        public string FollowUpCode { get; set; }
        public DateTime? FollowUpFromDate { get; set; }
        public DateTime? FollowUpToDate { get; set; }
        public string Order { get; set; }
    }
}
