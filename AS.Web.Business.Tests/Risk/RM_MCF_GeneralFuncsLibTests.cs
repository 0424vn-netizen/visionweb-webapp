using AS.Controls.Grid;
using AS.Web.Business.General;
using AS.Web.Business.Risk;
using System.Drawing;

namespace AS.Web.Business.Tests.Risk
{
    [TestClass]
    public class RM_MCF_GeneralFuncsLibTests
    {
        #region GetASFormat

        [TestMethod]
        public void GetASFormat_ShouldReturnCorrectFormatType_ForValidInput()
        {
            Assert.AreEqual(FormatType.Integer, RM_MCF_GeneralFuncsLib.GetASFormat("integer"));
            Assert.AreEqual(FormatType.Percentage, RM_MCF_GeneralFuncsLib.GetASFormat("percentage"));
            Assert.AreEqual(FormatType.Currency, RM_MCF_GeneralFuncsLib.GetASFormat("currency"));
            Assert.AreEqual(FormatType.Date, RM_MCF_GeneralFuncsLib.GetASFormat("date"));
            Assert.AreEqual(FormatType.DateAndTime, RM_MCF_GeneralFuncsLib.GetASFormat("dateandtime"));
            Assert.AreEqual(FormatType.Time, RM_MCF_GeneralFuncsLib.GetASFormat("time"));
            Assert.AreEqual(FormatType.Percentage0Digits, RM_MCF_GeneralFuncsLib.GetASFormat("percentage0digits"));
            Assert.AreEqual(FormatType.Percentage4Digits, RM_MCF_GeneralFuncsLib.GetASFormat("percentage4digits"));
            Assert.AreEqual(FormatType.Currency4Digits, RM_MCF_GeneralFuncsLib.GetASFormat("currency4digits"));
            Assert.AreEqual(FormatType.Number, RM_MCF_GeneralFuncsLib.GetASFormat("number"));
            Assert.AreEqual(FormatType.Number1Digit, RM_MCF_GeneralFuncsLib.GetASFormat("number1digit"));
            Assert.AreEqual(FormatType.Number4Digits, RM_MCF_GeneralFuncsLib.GetASFormat("mumber4digits"));
            Assert.AreEqual(FormatType.Phone, RM_MCF_GeneralFuncsLib.GetASFormat("phone"));
        }

        [TestMethod]
        public void GetASFormat_ShouldReturnDynamicString_ForUnknownInput()
        {
            Assert.AreEqual(FormatType.DynamicString, RM_MCF_GeneralFuncsLib.GetASFormat("unknown"));
        }

        [TestMethod]
        public void GetASFormat_ShouldReturnDynamicString_ForEmptyInput()
        {
            Assert.AreEqual(FormatType.DynamicString, RM_MCF_GeneralFuncsLib.GetASFormat(""));
        }

        [TestMethod]
        public void GetASFormat_ShouldReturnDynamicString_ForNullInput()
        {
            Assert.AreEqual(FormatType.DynamicString, RM_MCF_GeneralFuncsLib.GetASFormat(null));
        }

        [TestMethod]
        public void GetASFormat_ShouldHandleCaseInsensitivity()
        {
            Assert.AreEqual(FormatType.Integer, RM_MCF_GeneralFuncsLib.GetASFormat("INTEGER"));
            Assert.AreEqual(FormatType.Percentage, RM_MCF_GeneralFuncsLib.GetASFormat("PERCENTAGE"));
            Assert.AreEqual(FormatType.Currency, RM_MCF_GeneralFuncsLib.GetASFormat("CURRENCY"));
            Assert.AreEqual(FormatType.Date, RM_MCF_GeneralFuncsLib.GetASFormat("DATE"));
        }

        #endregion

        #region GetVertical

        [TestMethod]
        public void GetVertical_ShouldReturnLeft_WhenFormatTypeIsStaticString()
        {
            FormatType format = FormatType.StaticString;

            string result = RM_MCF_GeneralFuncsLib.GetVertical(format);

            Assert.AreEqual("left", result);
        }

        [TestMethod]
        public void GetVertical_ShouldReturnLeft_WhenFormatTypeIsDynamicString()
        {
            FormatType format = FormatType.DynamicString;

            string result = RM_MCF_GeneralFuncsLib.GetVertical(format);

            Assert.AreEqual("left", result);
        }

        [TestMethod]
        public void GetVertical_ShouldReturnLeft_WhenFormatTypeIsNone()
        {
            FormatType format = FormatType.None;

            string result = RM_MCF_GeneralFuncsLib.GetVertical(format);

            Assert.AreEqual("left", result);
        }

        [TestMethod]
        public void GetVertical_ShouldReturnRight_WhenFormatTypeIsCurrency()
        {
            FormatType format = FormatType.Currency;

            string result = RM_MCF_GeneralFuncsLib.GetVertical(format);

            Assert.AreEqual("right", result);
        }

        [TestMethod]
        public void GetVertical_ShouldReturnRight_WhenFormatTypeIsCurrency4Digits()
        {
            FormatType format = FormatType.Currency4Digits;

            string result = RM_MCF_GeneralFuncsLib.GetVertical(format);

            Assert.AreEqual("right", result);
        }

        [TestMethod]
        public void GetVertical_ShouldReturnRight_WhenFormatTypeIsInteger()
        {
            FormatType format = FormatType.Integer;

            string result = RM_MCF_GeneralFuncsLib.GetVertical(format);

            Assert.AreEqual("right", result);
        }

        [TestMethod]
        public void GetVertical_ShouldReturnRight_WhenFormatTypeIsNumber1Digit()
        {
            FormatType format = FormatType.Number1Digit;

            string result = RM_MCF_GeneralFuncsLib.GetVertical(format);

            Assert.AreEqual("right", result);
        }

        [TestMethod]
        public void GetVertical_ShouldReturnRight_WhenFormatTypeIsPercentage()
        {
            FormatType format = FormatType.Percentage;

            string result = RM_MCF_GeneralFuncsLib.GetVertical(format);

            Assert.AreEqual("right", result);
        }

        [TestMethod]
        public void GetVertical_ShouldReturnRight_WhenFormatTypeIsPercentage0Digits()
        {
            FormatType format = FormatType.Percentage0Digits;

            string result = RM_MCF_GeneralFuncsLib.GetVertical(format);

            Assert.AreEqual("right", result);
        }

        [TestMethod]
        public void GetVertical_ShouldReturnRight_WhenFormatTypeIsPercentage4Digits()
        {
            FormatType format = FormatType.Percentage4Digits;

            string result = RM_MCF_GeneralFuncsLib.GetVertical(format);

            Assert.AreEqual("right", result);
        }

        [TestMethod]
        public void GetVertical_ShouldReturnLeft_WhenFormatTypeIsUnknown()
        {
            FormatType format = (FormatType)999;

            string result = RM_MCF_GeneralFuncsLib.GetVertical(format);

            Assert.AreEqual("center", result);
        }

        #endregion

        #region FormatBorderText

        [TestMethod]
        public void FormatBorderText_ShouldReturnEmpty_WhenValueIsNull()
        {
            string value = null;
            Color color = Color.Red;

            string result = RM_MCF_GeneralFuncsLib.FormatBorderText(value, color);

            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void FormatBorderText_ShouldReturnEmpty_WhenValueIsEmpty()
        {
            string value = string.Empty;
            Color color = Color.Red;

            string result = RM_MCF_GeneralFuncsLib.FormatBorderText(value, color);

            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void FormatBorderText_ShouldReturnEmpty_WhenValueIsNonBreakingSpace()
        {
            string value = "&nbsp;";
            Color color = Color.Red;

            string result = RM_MCF_GeneralFuncsLib.FormatBorderText(value, color);

            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void FormatBorderText_ShouldReturnSpanWithoutStyle_WhenColorIsWhite()
        {
            string value = "Test Text";
            Color color = Color.White;

            string result = RM_MCF_GeneralFuncsLib.FormatBorderText(value, color);

            Assert.AreEqual("<span style='border-bottom: 2px solid White'>Test Text</span>", result);
        }

        [TestMethod]
        public void FormatBorderText_ShouldReturnSpanWithoutStyle_WhenCheckWhiteColorIsTrueAndColorIsWhite()
        {
            string value = "Test Text";
            Color color = Color.White;

            string result = RM_MCF_GeneralFuncsLib.FormatBorderText(value, color, true);

            Assert.AreEqual("<span>Test Text</span>", result);
        }

        [TestMethod]
        public void FormatBorderText_ShouldReturnSpanWithStyle_WhenColorIsNotWhite()
        {
            string value = "Test Text";
            Color color = Color.Red;

            string result = RM_MCF_GeneralFuncsLib.FormatBorderText(value, color);

            Assert.AreEqual("<span style='border-bottom: 2px solid Red'>Test Text</span>", result);
        }

        [TestMethod]
        public void FormatBorderText_ShouldReturnSpanWithStyle_WhenColorIsNotWhiteAndCheckWhiteColorIsTrue()
        {
            string value = "Test Text";
            Color color = Color.Red;

            string result = RM_MCF_GeneralFuncsLib.FormatBorderText(value, color, true);

            Assert.AreEqual("<span style='border-bottom: 2px solid Red'>Test Text</span>", result);
        }

        [TestMethod]
        public void FormatBorderText_ShouldReturnSpanWithoutStyle_WhenColorIsWhiteAndCheckWhiteColorIsTrue()
        {
            string value = "Test Text";
            Color color = Color.White;

            string result = RM_MCF_GeneralFuncsLib.FormatBorderText(value, color, true);

            Assert.AreEqual("<span>Test Text</span>", result);
        }

        #endregion
    }
}
