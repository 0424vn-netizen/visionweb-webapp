using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.WS.Entities.Tests
{
    [TestClass]
    public class BarometerColumnTests
    {
        [TestMethod]
        public void Constructor_InitializeProperties()
        {
            // Act
            var column = new BarometerColumn();

            // Assert
            Assert.IsNull(column.Name);
            Assert.IsNull(column.BgColor);
            Assert.IsNull(column.Color);
        }
    }
}
