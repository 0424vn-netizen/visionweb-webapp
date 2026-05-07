using AS.Common.DBManager;
using AS.Security.WS.Entities;
using AS.Web.Business.RiskReport.Models;
using Moq;
using System.Data;

namespace AS.Web.Business.Tests.RiskReportNoteBussinessTest
{
    [TestClass]
    public class GetDetailRiskReportNoteTest : RiskReportNoteBussinessBaseTest
    {
        [TestMethod]
        public void GetDetailRiskReportNote_ReturnDataWithParamIsNull()
        {
            GetDetailRiskReportNoteRequest? request = null;
            User? user = null;
            var result = _riskReportNoteBussiness?.GetDetailRiskReportNote(request, user);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetDetailRiskReportNote_ReturnDataWithModelRequestIsNull()
        {
            GetDetailRiskReportNoteRequest? request = null;
            var result = _riskReportNoteBussiness?.GetDetailRiskReportNote(request, _currentUser);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetDetailRiskReportNote_ReturnDataWithUserIsNull()
        {
            var request = new GetDetailRiskReportNoteRequest()
            {
                UserMode = _userMode,
                MerchantNoteID = "7976",
                MerchantNumber = "408100003502"
            };
            User? user = null;
            var result = _riskReportNoteBussiness?.GetDetailRiskReportNote(request, user);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetDetailRiskReportNote_ReturnSuccess()
        {
            var request = new GetDetailRiskReportNoteRequest()
            {
                UserMode = _userMode,
                MerchantNoteID = "7972",
                MerchantNumber = "408100003502"
            };
            // Mock the expected DataTable
            DataTable mockDataTable = new DataTable();
            mockDataTable.Columns.Add("ASClientID");
            mockDataTable.Columns.Add("NoteSourceId");
            mockDataTable.Columns.Add("NotesSourceDesc");
            mockDataTable.Columns.Add("MerchantNoteID");
            mockDataTable.Columns.Add("Comment");
            mockDataTable.Columns.Add("UserRoleID");
            DataRow row = mockDataTable.NewRow();
            row["ASClientID"] = "200";
            row["NoteSourceId"] = "2";
            row["NotesSourceDesc"] = "Risk Report";
            row["MerchantNoteID"] = "7972";
            row["Comment"] = "Test Risk Note";
            row["UserRoleID"] = "20001";
            mockDataTable.Rows.Add(row);            
            _reportServicesMock?.Setup(service => service.GetReports("spa_RM_MCF_GetDetail_RiskReport_MerchantNote", It.IsAny<FilterParameterCollection>())).Returns(mockDataTable);
            var result = _riskReportNoteBussiness?.GetDetailRiskReportNote(request, _currentUser);
            Assert.IsTrue(result?.Rows?.Count > 0);
        }
    }
}
