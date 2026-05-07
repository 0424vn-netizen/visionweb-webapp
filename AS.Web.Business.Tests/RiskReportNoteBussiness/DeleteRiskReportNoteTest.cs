using AS.Common.DBManager;
using AS.Security.WS.Entities;
using AS.Web.Business.RiskReport.Models;
using Moq;
using System.Data;

namespace AS.Web.Business.Tests.RiskReportNoteBussinessTest
{
    [TestClass]
    public class DeleteRiskReportNoteTest : RiskReportNoteBussinessBaseTest
    {
        [TestMethod]
        public void DeleteRiskReportNote_ReturnDataWithParamIsNull()
        {
            DeleteRiskReportNoteRequest? request = null;
            User? user = null;
            var result = _riskReportNoteBussiness?.DeleteRiskReportNote(request, user);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void DeleteRiskReportNote_ReturnDataWithModelRequestIsNull()
        {
            DeleteRiskReportNoteRequest? request = null;           
            var result = _riskReportNoteBussiness?.DeleteRiskReportNote(request, _currentUser);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void DeleteRiskReportNote_ReturnDataWithUserIsNull()
        {
            var request = new DeleteRiskReportNoteRequest()
            {
                UserMode = _userMode,
                MerchantNoteID = "7976",
                MerchantNumber = "408100003502"
            };
            User? user = null;           
            var result = _riskReportNoteBussiness?.DeleteRiskReportNote(request, user);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void DeleteRiskReportNote_ReturnSuccess()
        {
            var request = new DeleteRiskReportNoteRequest()
            {
                UserMode = _userMode,
                MerchantNoteID = "7976",
                MerchantNumber = "408100003502"
            };            
            var mockDataTable = new DataTable();
            _reportServicesMock?.Setup(service => service.GetReports("spa_MerchantNotes_UpdateNote", It.IsAny<FilterParameterCollection>())).Returns(mockDataTable);
            var result = _riskReportNoteBussiness?.DeleteRiskReportNote(request, _currentUser);
            Assert.IsTrue(result);
        }
    }
}
