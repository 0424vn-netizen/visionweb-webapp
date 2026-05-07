namespace AS.Web.Business.Tests.General.GeneralFuncsLib
{
    [TestClass]
    public class SplitToArrayTests
    {
        [TestMethod]
        public void SplitToArray_ReturnIsNull()
        {
            string inputData = null;
            char separator = ',';
            var result = Business.General.GeneralFuncsLib.SplitToArray(inputData, separator);
            Assert.IsTrue(!result.Any());
        }
        [TestMethod]
        public void SplitToArray_ReturnIsEmpty()
        {
            string inputData = "";
            char separator = ',';
            var result = Business.General.GeneralFuncsLib.SplitToArray(inputData, separator);
            Assert.IsTrue(!result.Any());
        }

        [TestMethod]
        public void SplitToArray_ReturnSeparatorNoMatched()
        {
            string inputData = "P196,P197";
            char separator = ';';
            var result = Business.General.GeneralFuncsLib.SplitToArray(inputData, separator);
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public void SplitToArray_ReturnSuccess()
        {
            string inputData = "P196,P197";
            char separator = ',';
            var result = Business.General.GeneralFuncsLib.SplitToArray(inputData, separator);
            Assert.IsTrue(result.Any());
        }
    }
}
