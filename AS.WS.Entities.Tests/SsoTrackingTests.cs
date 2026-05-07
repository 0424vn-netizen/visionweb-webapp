using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.WS.Entities.Tests
{
    [TestClass] 
    public class SsoTrackingTests
    {
        [TestMethod]
        public void Constructor_InitializeProperties()
        {
            // Act
            var ssoTracking = new SsoTracking();

            // Assert
            Assert.AreEqual(0, ssoTracking.ClientId);
            Assert.IsNull(ssoTracking.UserId);
            Assert.IsFalse(ssoTracking.Status);
            Assert.IsNull(ssoTracking.Message);
            Assert.IsNull(ssoTracking.ClientIp);
            Assert.IsNull(ssoTracking.HostIp);
            Assert.IsNull(ssoTracking.SessionId);
            Assert.IsNull(ssoTracking.AppId);
        }
    }
}
