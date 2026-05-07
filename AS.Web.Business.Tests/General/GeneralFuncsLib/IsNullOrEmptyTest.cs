using AS.Web.Business.General;

namespace AS.Web.Business.Tests.General.GeneralFuncsLib
{

    [TestClass]
    public class IsNullOrEmptyTest
    {
        [TestMethod]
        public void IsNullOrEmpty_ReturnTrue()
        {
            string? template = null;
            var isNull = template.IsNullOrEmpty();
            Assert.IsTrue(isNull);
        }
        [TestMethod]
        public void IsNullOrEmpty_ReturnFalse()
        {
            string? template = "Test data";
            var isNull = template.IsNullOrEmpty();
            Assert.IsFalse(isNull);
        }
    }
}
