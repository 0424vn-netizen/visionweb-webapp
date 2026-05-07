using System;

namespace AS.VW.Scheduler.Cybersource.Auth.Model
{
    public class AuthLogModel
    {
        public string OriginalFileName { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime ReportDate { get; set; }
        public int TransactionCount { get; set; }
        public bool IsValid { get; set; }
    }
}
