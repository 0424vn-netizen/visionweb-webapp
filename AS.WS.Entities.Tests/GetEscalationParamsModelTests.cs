using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.WS.Entities.Tests
{
    [TestClass]
    public class GetEscalationParamsModelTests
    {
        [TestMethod]
        public void Constructor_InitializeProperties()
        {
            // Act
            var model = new GetEscalationParamsModel();

            // Assert
            Assert.IsNull(model.AssignedToList);
            Assert.IsNull(model.ResolutionList);
            Assert.IsNull(model.StatusList);
            Assert.IsNull(model.OpenClosedCode);
            Assert.AreEqual(default(DateTime), model.OpenClosedFromDate);
            Assert.AreEqual(default(DateTime), model.OpenClosedToDate);
            Assert.IsNull(model.KeyType);
            Assert.IsNull(model.KeyValue);
            Assert.IsNull(model.FollowUpCode);
            Assert.IsNull(model.FollowUpFromDate);
            Assert.IsNull(model.FollowUpToDate);
            Assert.IsNull(model.Order);
        }
    }
}
