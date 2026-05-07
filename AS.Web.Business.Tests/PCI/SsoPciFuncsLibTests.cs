using AS.Web.Business.PCI;
using AS.Web.Business.PCI.Models;
using ComponentSpace.SAML2.Protocols;

namespace AS.Web.Business.Tests.PCI
{
    [TestClass]
    public class SsoPciFuncsLibTests
    {
        [TestMethod]
        public void PciLogInfo_NullRequest_LogsInfo()
        {
            try
            {
                // Act
                SsoPciFuncsLib.PciLogInfo(null);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Exception thrown: {ex.Message}");
            }
        }

        [TestMethod]
        public void PciLogInfo_ValidRequest_LogsInfo()
        {
            try
            {
                // Arrange
                var request = new PciLogInfoRequest
                {
                    RequestId = Guid.NewGuid(),
                    PciSsoUrl = "https://test.url",
                    Message = "Can not access PCI, because has an error exception",
                    ClientId = "200",
                    AsClientId = "200",
                    CurrentUserId = "assadmin"
                };
                // Act
                SsoPciFuncsLib.PciLogInfo(request);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Exception thrown: {ex.Message}");
            }
        }

        [TestMethod]
        public void GetSsoRequest_NullUrlOrSaml_ReturnsNull()
        {
            // Arrange
            SAMLResponse? saml = null;
            string url = "https://test.url";
            // Act
            var result1 = SsoPciFuncsLib.GetSsoRequest(null, saml);
            var result2 = SsoPciFuncsLib.GetSsoRequest(url, null);

            // Assert
            Assert.IsNull(result1);
            Assert.IsNull(result2);
        }       
    }
}