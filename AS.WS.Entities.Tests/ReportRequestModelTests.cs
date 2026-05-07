using AS.Common.DBManager;

namespace AS.WS.Entities.Tests
{
    [TestClass]
    public class ReportRequestModelTests
    {
        [TestMethod]
        public void Constructor_InitializeProperties()
        {
            // Arrange
            var originalParams = new FilterParameterCollection();
            var encryptedColumns = new List<string>();
            var hashColumns = new List<HashColEntity>();
            var decryptColumns = new List<string>();            

            var model = new ReportRequestModel
            {
                Params = originalParams,
                AsClientId = 123,
                IsExport = true,
                EncryptedColumnRequest = encryptedColumns,
                IsHash = false,
                HashColumns = hashColumns,
                DecryptColumns = decryptColumns,
                HashColumnsConfig = "Config1",
                HashClientsConfig = "Config2"
            };

            // Act & Assert
            Assert.AreEqual(originalParams, model.Params);
            Assert.AreEqual(123, model.AsClientId);
            Assert.IsTrue(model.IsExport);
            Assert.AreEqual(encryptedColumns, model.EncryptedColumnRequest);
            Assert.IsFalse(model.IsHash);
            Assert.AreEqual(hashColumns, model.HashColumns);
            Assert.AreEqual(decryptColumns, model.DecryptColumns);
            Assert.AreEqual("Config1", model.HashColumnsConfig);
            Assert.AreEqual("Config2", model.HashClientsConfig);
        }

        [TestMethod]
        public void Clone_ReturnReportRequestModel()
        {
            // Arrange
            var originalParams = new FilterParameterCollection();
            var encryptedColumns = new List<string>();
            var hashColumns = new List<HashColEntity>();
            var decryptColumns = new List<string>();

            var originalModel = new ReportRequestModel
            {
                Params = originalParams,
                AsClientId = 123,
                IsExport = true,
                EncryptedColumnRequest = encryptedColumns,
                IsHash = false,
                HashColumns = hashColumns,
                DecryptColumns = decryptColumns,
                HashColumnsConfig = "Config1",
                HashClientsConfig = "Config2"
            };

            // Act
            var clonedModel = (ReportRequestModel)originalModel.Clone();

            // Assert
            Assert.AreNotSame(originalModel, clonedModel);
            Assert.AreEqual(originalModel.Params, clonedModel.Params);
            Assert.AreEqual(originalModel.AsClientId, clonedModel.AsClientId);
            Assert.AreEqual(originalModel.IsExport, clonedModel.IsExport);
            Assert.AreEqual(originalModel.EncryptedColumnRequest, clonedModel.EncryptedColumnRequest);
            Assert.AreEqual(originalModel.IsHash, clonedModel.IsHash);
            Assert.AreEqual(originalModel.HashColumns, clonedModel.HashColumns);
            Assert.AreEqual(originalModel.DecryptColumns, clonedModel.DecryptColumns);
            Assert.AreEqual(originalModel.HashColumnsConfig, clonedModel.HashColumnsConfig);
            Assert.AreEqual(originalModel.HashClientsConfig, clonedModel.HashClientsConfig);
        }
    }
}
