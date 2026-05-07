using AS.Security.WS.Entities;
using AS.Web.Business.RiskReport;
using Moq;

namespace AS.Web.Business.Tests.RiskReportNoteBussinessTest
{
    public class RiskReportNoteBussinessBaseTest
    {
        protected Mock<IReportServices>? _reportServicesMock;
        protected IRiskReportNoteBussiness? _riskReportNoteBussiness;
        protected User? _currentUser;
        protected string _userMode = "CSUSER";

        [TestInitialize]
        public void Setup()
        {
            _reportServicesMock = new Mock<IReportServices>();
            _riskReportNoteBussiness = new RiskReportNoteBussiness(_reportServicesMock.Object);
            _currentUser = new User
            {
                UserID = "asadmin",
                ASClient = 200,
                SiteID = 1001
            };
        }
    }
}
