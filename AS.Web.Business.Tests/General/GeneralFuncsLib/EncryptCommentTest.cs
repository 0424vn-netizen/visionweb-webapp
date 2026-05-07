namespace AS.Web.Business.Tests.General.GeneralFuncsLib
{
    [TestClass]
    public class EncryptCommentTest
    {
        [TestMethod]
        [DataRow(null, null, null, DisplayName = "All param is null")]
        [DataRow(null, "123456789123,123456789456", null, DisplayName = "Param comment is null")]
        [DataRow("Edit risk note 123456789123", null, "Edit risk note 123456789123", DisplayName = "Param hdCardDetected is null")]
        [DataRow("Edit risk note 123456789123", "123456789123,123456789456", "Edit risk note 123456xxxxxx9123", DisplayName = "Result data is success")]
        public void EncryptComment_ToTest(string comment, string hdCardDetected, string? expected)
        {
            var result = Business.General.GeneralFuncsLib.EncryptComment(comment, hdCardDetected);            
            Assert.AreEqual(expected, result);
        }
    }
}
