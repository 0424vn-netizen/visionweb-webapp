using System;
using System.Collections.Generic;

namespace AS.WS.Entities
{
    public class AssignmentMerchantFilterModel
    {
        public int AssignmentId { get; set; }
        public int IsMerchantsOnWatch { get; set; }
        public bool? IsAllMerchants { get; set; }
        public DateTime? ReportDate { get; set; }
        public string MerchantNumber { get; set; }
        public int? Mode { get; set; }
    }

    public class ExportRiskReportRequest
    {
        public string ReportTitle { get; set; }
        public string ReportType { get; set; }
        public string ExportType { get; set; }
        public string FilePath { get; set; }
        public string TemplatePath { get; set; }
        public Dictionary<string, string> Resources { get; set; }
        public string CurrencyFormat { get; set; } = "es-US";
        public bool IsNRTRisk { get; set; }
    }
}
