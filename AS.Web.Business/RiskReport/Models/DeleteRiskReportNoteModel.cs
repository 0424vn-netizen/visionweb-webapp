namespace AS.Web.Business.RiskReport.Models
{
    public class DeleteRiskReportNoteRequest
    {
        public string UserMode { get; set; }
        public string MerchantNoteID { get; set; }
        public string MerchantNumber { get; set; }
    }
}
