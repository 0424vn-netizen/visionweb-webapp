namespace AS.Web.Business.RiskReport.Models
{
    public class GetRiskReportNoteRequest
    {
        public string UserMode { get; set; }
        public string MerchantNumber { get; set; }
        public string NotesSourceList { get; set; }
        public string RoleList { get; set; }
        public string UserList { get; set; }
        public bool IsHasMerchantProfile { get; set; }
        public bool IsHasRiskReport { get; set; }
        public bool IsHasOpenNewCase { get; set; }
        public bool IsHasOpenRiskCase { get; set; }
        public bool IsHasShadowUnderwriting { get; set; }
        public string MerchantNoteSort { get; set; }
        public bool HasMultiLanguageFeature { get; set; }
        public int CurrentLanguage { get; set; }
        public bool IsExporting { get; set; }        
    }
}
