using System;

namespace AS.VW.Scheduler.G2ML
{
    public class ExtractReportLogModel
    {
        public string SpaName { get; set; }
        public string ASClient { get; set; }
        public DateTime ReportDate { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FilePath { get; set; }
        public string Status { get; set; }
        public int NumberOfMatches { get; set; }
    }
}
