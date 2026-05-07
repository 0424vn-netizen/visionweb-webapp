using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.WS.Entities.Tests
{
    [TestClass]
    public class HashColEntityTests
    {
        [TestMethod]
        public void Constructor_SingleParameter_InitializeProperties()
        {
            // Arrange
            string columnName = "TestColumn";

            // Act
            var entity = new HashColEntity(columnName);

            // Assert
            Assert.AreEqual(columnName, entity.ColumnName);
            Assert.IsNull(entity.HashValue);
            Assert.IsNull(entity.RawData);
        }

        [TestMethod]
        public void Constructor_TwoParameters_InitializeProperties()
        {
            // Arrange
            string columnName = "TestColumn";
            string hashValue = "TestValue";

            // Act
            var entity = new HashColEntity(columnName, hashValue);

            // Assert
            Assert.AreEqual(columnName, entity.ColumnName);
            Assert.AreEqual(hashValue, entity.HashValue);
            Assert.IsNull(entity.RawData);
        }

        [TestMethod]
        public void AddHashValue_InputValue_AppendValueToHashValue()
        {
            // Arrange
            var entity = new HashColEntity("TestColumn", "InitialValue");

            // Act
            entity.AddHashValue("NewValue");

            // Assert
            Assert.AreEqual("InitialValue,NewValue", entity.HashValue);
        }

        [TestMethod]
        public void AddHashValue_EmptyValue_NotChangeHashValue()
        {
            // Arrange
            var entity = new HashColEntity("TestColumn", "InitialValue");

            // Act
            entity.AddHashValue("");

            // Assert
            Assert.AreEqual("InitialValue", entity.HashValue);
        }

        [TestMethod]
        public void TrimHash_RemoveFirstComma()
        {
            // Arrange
            var entity = new HashColEntity("TestColumn", ",Value1,Value2");

            // Act
            entity.TrimHash();

            // Assert
            Assert.AreEqual("Value1,Value2", entity.HashValue);
        }

        [TestMethod]
        public void TrimHash_EmptyHashValue_NotChangeHashValue()
        {
            // Arrange
            var entity = new HashColEntity("TestColumn", "");

            // Act
            entity.TrimHash();

            // Assert
            Assert.AreEqual("", entity.HashValue);
        }

        [TestMethod]
        public void TrimHash_NullHashValue_NotChangeHashValue()
        {
            // Arrange
            var entity = new HashColEntity("TestColumn");

            // Act
            entity.TrimHash();

            // Assert
            Assert.IsNull(entity.HashValue);
        }
    }
}
