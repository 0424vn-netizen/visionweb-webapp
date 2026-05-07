using AS.Common.DBManager;
using AS.Security.WS.Entities;
using AS.Web.Business.RiskReport.Models;
using Moq;
using System.Data;

namespace AS.Web.Business.Tests.ForwardDeliveryBussinessTest
{
    [TestClass]
    public class CalculateForwardDeliveryTest : ForwardDeliveryBussinessBaseTest
    {
        [TestMethod]
        public void CalculateForwardDelivery_ReturnDataWithParamIsNull()
        {
            CalculateForwardDeliveryRequest? request = null;
            User? user = null;
            var result = _forwardDeliveryBussiness?.CalculateForwardDelivery(request, user);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CalculateForwardDelivery_ReturnDataWithModelRequestIsNull()
        {
            CalculateForwardDeliveryRequest? request = null;
            var result = _forwardDeliveryBussiness?.CalculateForwardDelivery(request, _currentUser);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CalculateForwardDelivery_ReturnDataWithUserIsNull()
        {
            var request = new CalculateForwardDeliveryRequest()
            {
                UserMode = _userMode,
                MerchantNumber = "408100003502",
                CreditTimeliness = 100,
                NDX = 45,
                NDXPercent = 80,
                EnumFWDAction = Shared.Enums.EnumFWDAction.Calc
            };
            User? user = null;
            var result = _forwardDeliveryBussiness?.CalculateForwardDelivery(request, user);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CalculateForwardDelivery_ReturnSuccess()
        {
            var request = new CalculateForwardDeliveryRequest()
            {
                UserMode = _userMode,
                MerchantNumber = "408100003502",
                CreditTimeliness = 100,
                NDX = 45,
                NDXPercent = 80,
                EnumFWDAction = Shared.Enums.EnumFWDAction.Calc
            };
            var mockDataTable = new DataTable();
            _reportServicesMock?.Setup(service => service.GetReports("spa_RM_MCF_RiskReport_CalculateForwardDelivery", It.IsAny<FilterParameterCollection>())).Returns(mockDataTable);
            var result = _forwardDeliveryBussiness?.CalculateForwardDelivery(request, _currentUser);
            Assert.IsTrue(result);
        }
    }
}
