namespace AS.Web.Business.RiskReport.Models
{
    public class UpdateRiskReportNoteRequest
    {
        public string UserMode { get; set; }
        public string MerchantNoteID { get; set; }
        public string MerchantNumber { get; set; }
        public string Comment { get; set; }
        public string CommentPlainText { get; set; }
        public string HdCardDetected { get; set; }
    }
}
