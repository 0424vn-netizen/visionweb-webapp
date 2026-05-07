using AS.Common.DBManager;
using AS.Security.WS.Entities;
using AS.Web.Business.RiskReport.Models;
using Moq;
using System.Data;

namespace AS.Web.Business.Tests.ForwardDeliveryBussinessTest
{
    [TestClass]
    public class GetForwardDeliveryTest : ForwardDeliveryBussinessBaseTest
    {
        [TestMethod]
        public void GetForwardDelivery_ReturnDataWithParamIsNull()
        {
            GetForwardDeliveryRequest? request = null;
            User? user = null;
            var result = _forwardDeliveryBussiness?.GetForwardDelivery(request, user);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetForwardDelivery_ReturnDataWithModelRequestIsNull()
        {
            GetForwardDeliveryRequest? request = null;
            var result = _forwardDeliveryBussiness?.GetForwardDelivery(request, _currentUser);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetForwardDelivery_ReturnDataWithUserIsNull()
        {
            var request = new GetForwardDeliveryRequest()
            {
                UserMode = _userMode,
                MerchantNumber = "408100003502"
            };
            User? user = null;
            var result = _forwardDeliveryBussiness?.GetForwardDelivery(request, user);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetForwardDelivery_ReturnSuccess()
        {
            var request = new GetForwardDeliveryRequest()
            {
                UserMode = _userMode,
                MerchantNumber = "408100003502"
            };
            // Mock the expected DataTable
            DataTable mockDataTable = new DataTable();
            mockDataTable.Columns.Add("ASClientID");
            mockDataTable.Columns.Add("MerchantNumber");
            mockDataTable.Columns.Add("CreditTimeliness");
            mockDataTable.Columns.Add("NDX");
            mockDataTable.Columns.Add("NDXPercent");
            mockDataTable.Columns.Add("NDXDays");
            mockDataTable.Columns.Add("AdvDepositPercentage");
            mockDataTable.Columns.Add("LastNDXUpdatedBy");
            mockDataTable.Columns.Add("LastNDXPerUpdatedBy");

            DataRow row = mockDataTable.NewRow();
            row["ASClientID"] = "200";
            row["MerchantNumber"] = "408100003502";
            row["CreditTimeliness"] = "Risk Report";
            row["NDX"] = "7972";
            row["NDXPercent"] = "Test Risk Note";
            row["AdvDepositPercentage"] = "20001";
            row["LastNDXUpdatedBy"] = "20001";
            row["LastNDXPerUpdatedBy"] = "20001";
            mockDataTable.Rows.Add(row);
            _reportServicesMock?.Setup(service => service.GetReports("spa_RM_MCF_RiskReport_GetForwardDelivery", It.IsAny<FilterParameterCollection>())).Returns(mockDataTable);
            var result = _forwardDeliveryBussiness?.GetForwardDelivery(request, _currentUser);
            Assert.IsTrue(result?.Rows?.Count > 0);
        }
    }
}
