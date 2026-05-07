using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.WS.Entities.Tests
{
    [TestClass]
    public class BarometerConfigTests
    {
        [TestMethod]
        public void Constructor_InitializeProperties()
        {
            // Act
            var config = new BarometerConfig();

            // Assert
            Assert.IsNull(config.Configs);
        }
    }
}
