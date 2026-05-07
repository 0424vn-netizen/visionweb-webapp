using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.WS.Entities.Tests
{

    [TestClass]
    public class HashDataModelTests
    {
        [TestMethod]
        public void Constructor_InitializeProperties()
        {
            // Act
            var model = new HashDataModel();
            model.HashColumns = new List<HashColEntity>();
            model.DecryptColumns = new List<string>();

            // Assert
            Assert.IsNotNull(model.HashColumns);
            Assert.IsNotNull(model.DecryptColumns);
            Assert.AreEqual(0, model.HashColumns.Count);
            Assert.AreEqual(0, model.DecryptColumns.Count);
        }
    }

}
