namespace AS.Web.Business.Tests.Risk
{
    [TestClass]
    public class IsNullDataTests
    {
        [TestMethod]
        public void IsNullData_ReturnTrue()
        {
            var result = Business.Risk.RM_MCF_GeneralFuncsLib.IsNullData(null);
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void IsNullData_ReturnFalse()
        {
            string val = "45.89";
            var result = Business.Risk.RM_MCF_GeneralFuncsLib.IsNullData(val);
            Assert.IsTrue(!result);
        }
    }
}
