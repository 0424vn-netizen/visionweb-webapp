using AS.Common.DBManager;
using AS.Security.WS.Entities;
using AS.Web.Business.RiskReport.Models;
using Moq;
using System.Data;

namespace AS.Web.Business.Tests.RiskReportNoteBussinessTest
{
    [TestClass]
    public class UpdateRiskReportNoteTest : RiskReportNoteBussinessBaseTest
    {
        [TestMethod]
        public void UpdateRiskReportNote_ReturnDataWithParamIsNull()
        {
            UpdateRiskReportNoteRequest? request = null;
            User? user = null;
            var result = _riskReportNoteBussiness?.UpdateRiskReportNote(request, user);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void UpdateRiskReportNote_ReturnDataWithModelRequestIsNull()
        {
            UpdateRiskReportNoteRequest? request = null;
            var result = _riskReportNoteBussiness?.UpdateRiskReportNote(request, _currentUser);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void UpdateRiskReportNote_ReturnDataWithUserIsNull()
        {
            var request = new UpdateRiskReportNoteRequest()
            {
                UserMode = _userMode,
                MerchantNoteID = "7976",
                MerchantNumber = "408100003502"
            };
            User? user = null;
            var result = _riskReportNoteBussiness?.UpdateRiskReportNote(request, user);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void UpdateRiskReportNote_ReturnSuccess()
        {
            var request = new UpdateRiskReportNoteRequest()
            {
                UserMode = _userMode,
                MerchantNoteID = "7972",
                MerchantNumber = "408100003502",
                Comment = "Test Update Risk Note",
                CommentPlainText = "Test Update Risk Note"
            };
            var mockDataTable = new DataTable();
            _reportServicesMock?.Setup(service => service.GetReports("spa_MerchantNotes_UpdateNote", It.IsAny<FilterParameterCollection>())).Returns(mockDataTable);
            var result = _riskReportNoteBussiness?.UpdateRiskReportNote(request, _currentUser);
            Assert.IsTrue(result);
        }
    }
}
