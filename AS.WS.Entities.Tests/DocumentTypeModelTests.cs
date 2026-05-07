using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.WS.Entities.Tests
{
    [TestClass]
    public class DocumentTypeModelTests
    {
        [TestMethod]
        public void Constructor_InitializeProperties()
        {
            // Act
            var model = new DocumentTypeModel();

            // Assert
            Assert.AreEqual(0, model.Id);
            Assert.IsNull(model.Name);
            Assert.IsNull(model.Description);
            Assert.IsFalse(model.IsActive);
            Assert.AreEqual(0, model.SourceId);
            Assert.IsNull(model.SourceIdList);
            Assert.IsFalse(model.IsSyncData);
            Assert.IsNull(model.CreatedBy);
        }
    }
}
