namespace AS.Web.Business.Tests.Risk
{
    [TestClass]
    public class IsNullOrEmptyTests
    {
        [TestMethod]
        public void IsNullOrEmpty_ReturnTrue()
        {
            var result = Business.Risk.RM_MCF_GeneralFuncsLib.IsNullOrEmpty(null);
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void IsNullOrEmpty_ReturnFalse()
        {
            string val = "45.89";
            var result = Business.Risk.RM_MCF_GeneralFuncsLib.IsNullOrEmpty(val);
            Assert.IsTrue(!result);
        }
    }
}
