namespace AS.Web.Business.Tests.Risk
{
    [TestClass]
    public class ToDecimalTests
    {
        [TestMethod]
        public void ToDecimal_ReturnIsNull()
        {
            var result = Business.Risk.RM_MCF_GeneralFuncsLib.ToDecimal(null);
            Assert.IsTrue(result == 0);
        }

        [TestMethod]
        public void ToDecimal_ReturnEmptyAndDefaultValue()
        {
            var result = Business.Risk.RM_MCF_GeneralFuncsLib.ToDecimal("", 0m);
            Assert.IsTrue(result == 0);
        }
        [TestMethod]
        public void ToDecimal_InputNotNumber_Return0()
        {
            object val = "abc";
            decimal defaultVal = 0m;
            var result = Business.Risk.RM_MCF_GeneralFuncsLib.ToDecimal(val, defaultVal);
            Assert.IsTrue(result == 0);
        }
        [TestMethod]
        public void ToDecimal_ReturnDefault()
        {
            object val = "45.89";
            var result = Business.Risk.RM_MCF_GeneralFuncsLib.ToDecimal(val);
            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public void ToDecimal_ReturnSuccess()
        {
            string val = "45.89";
            decimal defaultVal = 0m;
            var result = Business.Risk.RM_MCF_GeneralFuncsLib.ToDecimal(val, defaultVal);
            Assert.IsTrue(result > 0);
        }
    }
}
