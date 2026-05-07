namespace AS.Web.Business.Tests.Risk
{
    [TestClass]
    public class FormatCurrencyTextTests
    {
        [TestMethod]
        public void FormatCurrencyText_InputIsNull_ReturnDefaultFormnat()
        {
            var result = Business.Risk.RM_MCF_GeneralFuncsLib.FormatCurrencyText(null);
            Assert.IsTrue(!string.IsNullOrEmpty(result));
        }

        [TestMethod]
        public void FormatCurrencyText_InputVal_ReturnSuccess()
        {
            object val = "45.89";
            var result = Business.Risk.RM_MCF_GeneralFuncsLib.FormatCurrencyText(val);
            Assert.IsTrue(!string.IsNullOrEmpty(result));
        }
        [TestMethod]
        public void FormatCurrencyText_InputFormatProviderEmpty_ReturnSuccess()
        {
            object val = "45.89";
            string formatProvider = "";
            var result = Business.Risk.RM_MCF_GeneralFuncsLib.FormatCurrencyText(val, formatProvider);
            Assert.IsTrue(!string.IsNullOrEmpty(result));
        }
        [TestMethod]
        public void FormatCurrencyText_InputFormatEmpty_ReturnSuccess()
        {
            object val = "45.89";
            string formatProvider = "";
            string format = "";
            var result = Business.Risk.RM_MCF_GeneralFuncsLib.FormatCurrencyText(val, formatProvider, format);
            Assert.IsTrue(!string.IsNullOrEmpty(result));
        }
        [TestMethod]
        public void FormatCurrencyText_InputDefaultEmpty_ReturnSuccess()
        {
            object val = "45.89";
            string formatProvider = "";
            string format = "";
            string defaultVal = "";
            var result = Business.Risk.RM_MCF_GeneralFuncsLib.FormatCurrencyText(val, formatProvider, format, defaultVal);
            Assert.IsTrue(!string.IsNullOrEmpty(result));
        }

        [TestMethod]
        public void CheckExistsInSplitToArray_ReturnSuccess()
        {
            object val = "45.89";
            string formatProvider = "en-US";
            string format = "C";
            string defaultVal = "-";
            var result = Business.Risk.RM_MCF_GeneralFuncsLib.FormatCurrencyText(val, formatProvider, format, defaultVal);
            Assert.IsTrue(!string.IsNullOrEmpty(result));
        }

    }


}
