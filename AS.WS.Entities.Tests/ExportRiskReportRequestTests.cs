using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.WS.Entities.Tests
{
    [TestClass]
    public class ExportRiskReportRequestTests
    {
        [TestMethod]
        public void Constructor_InitializeProperties()
        {
            // Act
            var request = new ExportRiskReportRequest()
            {
                Resources = new Dictionary<string, string>()
            };            

            // Assert
            Assert.IsNull(request.ReportTitle);
            Assert.IsNull(request.ReportType);
            Assert.IsNull(request.ExportType);
            Assert.IsNull(request.FilePath);
            Assert.IsNull(request.TemplatePath);
            Assert.IsNotNull(request.Resources);
            Assert.AreEqual("es-US", request.CurrencyFormat);
            Assert.IsFalse(request.IsNRTRisk);
        }
    }
}
