namespace AS.Web.Business.RiskReport.Models
{
    public class CalculateForwardDeliveryRequest
    {
        public string UserMode { get; set; }
        public string MerchantNumber { get; set; }
        public int CreditTimeliness { get; set; }
        public int NDX { get; set; }
        public int NDXPercent { get; set; }
        public Shared.Enums.EnumFWDAction EnumFWDAction { get; set; }
    }
}
