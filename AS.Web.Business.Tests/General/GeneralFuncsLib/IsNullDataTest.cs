using AS.Web.Business.General;

namespace AS.Web.Business.Tests.General.GeneralFuncsLib
{

    [TestClass]
    public class IsNullDataTest
    {
        [TestMethod]
        public void IsNullData_ReturnTrue()
        {
            string? template = null;
           var isNull =  template.IsNullData();
            Assert.IsTrue(isNull);
        }
        [TestMethod]
        public void IsNullData_ReturnFalse()
        {
            string? template = "Test data";
            var isNull = template.IsNullData();
            Assert.IsFalse(isNull);
        }
    }
}
