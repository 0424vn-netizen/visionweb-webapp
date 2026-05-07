namespace AS.WS.Entities.Tests
{
    [TestClass]
    public class LogPmApiModelTests
    {
        [TestMethod]
        public void Constructor_InitializeProperties()
        {
            // Arrange
            var requestId = Guid.NewGuid();
            var processingStep = "Step1";
            var message = "Test message";
            var data1 = "Data1";
            var data2 = "Data2";
            var data3 = "Data3";
            var data4 = "Data4";
            var data5 = "Data5";
            var xmlMessage = "<xml>Message</xml>";
            var xmlErrorResponse = "<xml>Error</xml>";
            var requestDts = DateTime.Now;

            // Act
            var model = new LogPmApiModel
            {
                RequestId = requestId,
                ProcessingStep = processingStep,
                Message = message,
                Data1 = data1,
                Data2 = data2,
                Data3 = data3,
                Data4 = data4,
                Data5 = data5,
                XmlMessage = xmlMessage,
                XmlErrorResponse = xmlErrorResponse,
                RequestDts = requestDts
            };

            // Assert
            Assert.AreEqual(requestId, model.RequestId);
            Assert.AreEqual(processingStep, model.ProcessingStep);
            Assert.AreEqual(message, model.Message);
            Assert.AreEqual(data1, model.Data1);
            Assert.AreEqual(data2, model.Data2);
            Assert.AreEqual(data3, model.Data3);
            Assert.AreEqual(data4, model.Data4);
            Assert.AreEqual(data5, model.Data5);
            Assert.AreEqual(xmlMessage, model.XmlMessage);
            Assert.AreEqual(xmlErrorResponse, model.XmlErrorResponse);
            Assert.AreEqual(requestDts, model.RequestDts);
        }
    }
}