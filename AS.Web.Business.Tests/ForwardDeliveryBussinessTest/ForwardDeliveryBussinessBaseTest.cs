using AS.Security.WS.Entities;
using AS.Web.Business.RiskReport;
using Moq;

namespace AS.Web.Business.Tests.ForwardDeliveryBussinessTest
{
    public class ForwardDeliveryBussinessBaseTest
    {
        protected Mock<IReportServices>? _reportServicesMock;
        protected IForwardDeliveryBussiness? _forwardDeliveryBussiness;
        protected User? _currentUser;
        protected string _userMode = "CSUSER";

        [TestInitialize]
        public void Setup()
        {
            _reportServicesMock = new Mock<IReportServices>();
            _forwardDeliveryBussiness = new ForwardDeliveryBussiness(_reportServicesMock.Object);
            _currentUser = new User
            {
                UserID = "asadmin",
                ASClient = 200,
                SiteID = 1001
            };
        }
    }
}
