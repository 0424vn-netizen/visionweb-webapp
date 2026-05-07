using AS.Web.Business.Shared.Constants;

namespace AS.Web.Business.PCI.Models
{
    public class GenerateSamlContentRequest : BasePciModel
    {
        private readonly string _assertionKeyUserName = PciConstants.SSO_ASSERTION_KEY_USERNAME;
        public string AssertionKeyUserName { get => _assertionKeyUserName; }
        private readonly string _assertionKeyAsClientId = PciConstants.SSO_ASSERTION_KEY_ASCLIENTID;
        public string AssertionKeyAsClientId { get => _assertionKeyAsClientId; }
        private readonly string _assertionSubject = PciConstants.SSO_ASSERTION_SUBJECT;
        public string AssertionSubject { get => _assertionSubject; }
        public string PciCertificatePass { get; set; }
        public string FullPathPciCertificateUrl { get; set; }
    }
}
