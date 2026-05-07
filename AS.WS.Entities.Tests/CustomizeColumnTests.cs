using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.WS.Entities.Tests
{
    [TestClass]
    public class CustomizeColumnTests
    {
        [TestMethod]
        public void Constructor_InitializeProperties()
        {
            // Act
            var column = new CustomizeColumn();

            // Assert
            Assert.IsNull(column.Key);
            Assert.IsNull(column.ASFormat);
            Assert.IsNull(column.CustomFormat);
            Assert.IsNull(column.ReSourceKey);
            Assert.IsFalse(column.IsDefault);
            Assert.AreEqual(0, column.Width);
            Assert.AreEqual(0, column.OrderNo);
            Assert.IsNull(column.DefaultValue);
        }
    }
}
