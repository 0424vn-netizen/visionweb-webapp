using AS.Web.Business.Shared.Enums;

namespace AS.Web.Business.Tests.General.GeneralFuncsLib
{
    [TestClass]
    public class GetDateToNowTests
    {
        [TestMethod]
        public void GetDateToNow_ReturnDefault()
        {
            var dateNow = DateTime.Now.Date;
            var result = Business.General.GeneralFuncsLib.GetDateToNow();
            Assert.AreEqual(result.Date, dateNow);
        }

        [TestMethod]
        public void GetDateToNow_ReturnDefault2()
        {
            var dateUTC = DateTime.UtcNow.Date;
            var result = Business.General.GeneralFuncsLib.GetDateToNow(EnumTimeZone.Utc);
            Assert.AreEqual(result.Date, dateUTC);
        }
    }
}
