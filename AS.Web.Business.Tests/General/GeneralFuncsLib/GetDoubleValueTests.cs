namespace AS.Web.Business.Tests.General.GeneralFuncsLib
{
    [TestClass]
    public class GetDoubleValueTests
    {
        [TestMethod]
        public void GetDoubleValue_ReturnIsNull()
        {
            string parameterValue = "";
            int parameterPrecision = 0;
            bool isDecimal = false;
            var result = Business.General.GeneralFuncsLib.GetDoubleValue(parameterValue, parameterPrecision, isDecimal);
            Assert.IsTrue(string.IsNullOrEmpty(result));
        }
        [TestMethod]
        public void GetDoubleValue_ReturnNoPrecision()
        {
            string parameterValue = "0.01";
            int parameterPrecision = -1;
            bool isDecimal = false;
            var result = Business.General.GeneralFuncsLib.GetDoubleValue(parameterValue, parameterPrecision, isDecimal);
            Assert.IsTrue(!string.IsNullOrEmpty(result));
        }

        [TestMethod]
        public void GetDoubleValue_ReturnNoDecimal()
        {
            string parameterValue = "0.015";
            int parameterPrecision = 2;
            bool isDecimal = false;
            var result = Business.General.GeneralFuncsLib.GetDoubleValue(parameterValue, parameterPrecision, isDecimal);
            Assert.IsTrue(!string.IsNullOrEmpty(result));
        }

        [TestMethod]
        public void Success_Tests()
        {
            string parameterValue = "0.1";
            int parameterPrecision = 2;
            bool isDecimal = true;
            var result = Business.General.GeneralFuncsLib.GetDoubleValue(parameterValue, parameterPrecision, isDecimal);
            Assert.IsTrue(!string.IsNullOrEmpty(result));
        }
    }
}
