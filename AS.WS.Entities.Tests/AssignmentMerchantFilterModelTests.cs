using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.WS.Entities.Tests
{
    [TestClass] 
    public class AssignmentMerchantFilterModelTests
    {
        [TestMethod]
        public void Constructor_InitializeProperties()
        {
            // Act
            var model = new AssignmentMerchantFilterModel();

            // Assert
            Assert.AreEqual(0, model.AssignmentId);
            Assert.AreEqual(0, model.IsMerchantsOnWatch);
            Assert.IsNull(model.IsAllMerchants);
            Assert.IsNull(model.ReportDate);
            Assert.IsNull(model.MerchantNumber);
            Assert.IsNull(model.Mode);
        }
    }
}
