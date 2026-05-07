namespace AS.Web.Business.Tests.General
{
    [TestClass]
    public class GeneralFuncsLibTests
    {
        #region NvlString

        [TestMethod]
        public void NvlString_ShouldReturnEmptyString_WhenInputIsNull()
        {
            object input = null;

            string result = Business.General.GeneralFuncsLib.NvlString(input);

            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void NvlString_ShouldReturnEmptyString_WhenInputIsDBNull()
        {
            object input = DBNull.Value;

            string result = Business.General.GeneralFuncsLib.NvlString(input);

            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void NvlString_ShouldReturnString_WhenInputIsNonNullValue()
        {
            object input = "Test String";

            string result = Business.General.GeneralFuncsLib.NvlString(input);

            Assert.AreEqual("Test String", result);
        }

        [TestMethod]
        public void NvlString_ShouldReturnString_WhenInputIsEmptyString()
        {
            object input = string.Empty;

            string result = Business.General.GeneralFuncsLib.NvlString(input);

            Assert.AreEqual(string.Empty, result);
        }

        #endregion

        #region GetRDRColumnNames

        [TestMethod]
        public void GetRDRColumnNames_ShouldReturnCorrectColumns_WhenPageIsChargebacksDetail()
        {
            string page = "ChargebacksDetail";

            string[] result = Business.General.GeneralFuncsLib.GetRDRColumnNames(page);

            CollectionAssert.AreEqual(new string[] { "FirstChargebackRDRAmount", "PostChargebackRDRAmount" }, result);
        }

        [TestMethod]
        public void GetRDRColumnNames_ShouldReturnCorrectColumns_WhenPageIsRmMCFRetCb()
        {
            string page = "rm_MCF_RetCb";

            string[] result = Business.General.GeneralFuncsLib.GetRDRColumnNames(page);

            CollectionAssert.AreEqual(new string[]
            {
                "FirstChargebackRDRCount", "FirstChargebackRDRAmount", "PostChargebackRDRCount", "PostChargebackRDRAmount",
                "ThirtyDaysFirstChargebackRDRCount", "ThirtyDaysFirstChargebackRDRAmount", "ThirtyDaysPostChargebackRDRCount", "ThirtyDaysPostChargebackRDRAmount",
                "NinetyDaysFirstChargebackRDRCount", "NinetyDaysFirstChargebackRDRAmount", "NinetyDaysPostChargebackRDRCount", "NinetyDaysPostChargebackRDRAmount"
            }, result);
        }

        [TestMethod]
        public void GetRDRColumnNames_ShouldReturnDefaultColumns_WhenPageIsOther()
        {
            string page = "OtherPage";

            string[] result = Business.General.GeneralFuncsLib.GetRDRColumnNames(page);

            CollectionAssert.AreEqual(new string[]
            {
                "FirstChargebackRDRCount", "FirstChargebackRDRAmount", "PostChargebackRDRCount", "PostChargebackRDRAmount"
            }, result);
        }

        #endregion
    }
}
