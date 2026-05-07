using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.WS.Entities.Tests
{
    [TestClass]
    public class ServiceRequestModelTests
    {
        [TestMethod]
        public void Constructor_InitializeProperties()
        {
            // Act
            var model = new ServiceRequestModel();

            // Assert
            Assert.IsNull(model.Params);
            Assert.AreEqual(0, model.AsClientId);
            Assert.IsFalse(model.IsExport);
            Assert.IsFalse(model.IsHash);
            Assert.IsNotNull(model.HashColumns);
            Assert.AreEqual(0, model.HashColumns.Count);
            Assert.IsNotNull(model.DecryptColumns);
            Assert.AreEqual(0, model.DecryptColumns.Count);
            Assert.IsNull(model.HashColumnsConfig);
            Assert.IsNull(model.HashClientsConfig);
        }
    }
}
