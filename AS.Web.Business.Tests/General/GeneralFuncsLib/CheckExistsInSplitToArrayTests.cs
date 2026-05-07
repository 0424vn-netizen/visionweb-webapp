namespace AS.Web.Business.Tests.General.GeneralFuncsLib
{
    [TestClass]
    public class CheckExistsInSplitToArrayTests
    {
        [TestMethod]
        public void CheckExistsInSplitToArray_ReturnIsNull()
        {
            string inputData = "";
            char separator = ',';
            string key = "";
            var result = Business.General.GeneralFuncsLib.CheckExistsInSplitToArray(inputData, separator, key);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void CheckExistsInSplitToArray_ReturnNoExists()
        {
            string inputData = "P196,P197";
            char separator = ',';
            string key = "P28";
            var result = Business.General.GeneralFuncsLib.CheckExistsInSplitToArray(inputData, separator, key);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void CheckExistsInSplitToArray_ReturnSuccess()
        {
            string inputData = "P196,P197";
            char separator = ',';
            string key = "P196";
            var result = Business.General.GeneralFuncsLib.CheckExistsInSplitToArray(inputData, separator, key);
            Assert.IsTrue(result);
        }
    }
}
