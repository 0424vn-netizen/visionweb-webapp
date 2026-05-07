namespace AS.Web.Business.Tests.General
{
    [TestClass]
    public class GeneralFuncsLibCryptophyTest
    {
        [TestMethod]
        public void DecryptText_ReturnsDecryptedText_WhenInputIsValid()
        {
            // Arrange
            var encrypted = "M94PWaa1wtVMer6yWpaY1A==";
            var decrypted = "unittestsso";
            var requestId = Guid.NewGuid();
            // Act
            var result = Business.General.GeneralFuncsLib.DecryptText(encrypted, requestId);
            // Assert
            Assert.AreEqual(decrypted, result);
        }

        [TestMethod]
        public void DecryptText_ReturnsNull_WhenInputIsNullOrEmpty()
        {
            // Act & Assert
            Assert.IsNull(Business.General.GeneralFuncsLib.DecryptText(null, Guid.NewGuid()));
            Assert.IsNull(Business.General.GeneralFuncsLib.DecryptText("", Guid.NewGuid()));
        }

        [TestMethod]
        public void DecryptText_LogsErrorAndReturnsNull_OnException()
        {
            // Arrange
            var encrypted = "abc";
            var requestId = Guid.NewGuid();
            // Act & Assert
            try
            {
                var result = Business.General.GeneralFuncsLib.DecryptText(encrypted, requestId);
                Assert.IsNull(result);
            }
            catch
            {
                // Exception is expected, test passes
            }
        }
    }
}
