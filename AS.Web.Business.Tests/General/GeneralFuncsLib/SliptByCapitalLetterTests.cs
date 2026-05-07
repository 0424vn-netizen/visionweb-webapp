namespace AS.Web.Business.Tests.General.GeneralFuncsLib
{
    [TestClass]
    public class SliptByCapitalLetterTests
    {
        [TestMethod]
        public void SliptByCapitalLetterT_ReturnIsNull()
        {
            string inputData = "";
            var result = Business.General.GeneralFuncsLib.SliptByCapitalLetter(inputData);
            Assert.IsTrue(string.IsNullOrEmpty(result));
        }

        [TestMethod]
        public void SliptByCapitalLetterT_ReturnAlLOfToLower()
        {
            string inputData = "merchantlinkedtoriskclosedmid";
            var result = Business.General.GeneralFuncsLib.SliptByCapitalLetter(inputData);
            Assert.IsTrue(!string.IsNullOrEmpty(result));
        }

        [TestMethod]
        public void SliptByCapitalLetterT_ReturnSuccess()
        {
            string inputData = "MerchantLinkedToRiskClosedMID";
            var result = Business.General.GeneralFuncsLib.SliptByCapitalLetter(inputData);
            Assert.IsTrue(!string.IsNullOrEmpty(result));
        }
    }
}
