using AS.Common.DBManager;
using AS.Security.WS.Entities;
using AS.Web.Business.RiskReport.Models;
using Moq;
using System.Data;

namespace AS.Web.Business.Tests.RiskReportNoteBussinessTest
{
    [TestClass]
    public class GetRiskReportNotesTest : RiskReportNoteBussinessBaseTest
    {
        [TestMethod]
        public void GetRiskReportNotes_ReturnDataWithParamIsNull()
        {
            GetRiskReportNoteRequest? request = null;
            User? user = null;
            var result = _riskReportNoteBussiness?.GetRiskReportNotes(request, user);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetRiskReportNotes_ReturnDataWithModelRequestIsNull()
        {
            GetRiskReportNoteRequest? request = null;
            var result = _riskReportNoteBussiness?.GetRiskReportNotes(request, _currentUser);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetRiskReportNotes_ReturnDataWithUserIsNull()
        {
            var request = new GetRiskReportNoteRequest()
            {
                UserMode = _userMode,
                MerchantNumber = "408100003502"
            };
            User? user = null;
            var result = _riskReportNoteBussiness?.GetRiskReportNotes(request, user);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetRiskReportNotes_ReturnWithExportExcelSuccess()
        {
            var request = new GetRiskReportNoteRequest()
            {
                UserMode = _userMode,
                MerchantNumber = "408100003502",
                IsHasRiskReport = true,
                IsExporting = true
            };

            DataTable mockDataTable = new DataTable();
            mockDataTable.Columns.Add("ASClientID");
            mockDataTable.Columns.Add("NoteSourceId");
            mockDataTable.Columns.Add("NotesSourceDesc");
            mockDataTable.Columns.Add("MerchantNoteID");
            mockDataTable.Columns.Add("CaseID");
            mockDataTable.Columns.Add("CaseTitle");
            mockDataTable.Columns.Add("CaseNumber");
            mockDataTable.Columns.Add("CaseNumberTitle");
            mockDataTable.Columns.Add("Comment");
            mockDataTable.Columns.Add("UserRoleID");
            mockDataTable.Columns.Add("HierarchyName");
            mockDataTable.Columns.Add("CreatedByFullName");
            mockDataTable.Columns.Add("CreatedDate");
            mockDataTable.Columns.Add("UpdatedByFullName");
            mockDataTable.Columns.Add("UpdatedDate");
            mockDataTable.Columns.Add("DeletedByFullName");
            mockDataTable.Columns.Add("DeletedDate");
            mockDataTable.Columns.Add("IsDeleted");
            mockDataTable.Columns.Add("TotalRows");
            DataRow row = mockDataTable.NewRow();
            row["ASClientID"] = "200";
            row["NoteSourceId"] = "2";
            row["NotesSourceDesc"] = "Risk Report";
            row["MerchantNoteID"] = "7972";
            row["CaseID"] = null;
            row["CaseTitle"] = null;
            row["CaseNumber"] = null;
            row["CaseNumberTitle"] = null;
            row["Comment"] = "Test Risk Note";
            row["UserRoleID"] = "20001";
            row["HierarchyName"] = "AS Users";
            row["CreatedByFullName"] = "Esquire Bank";
            row["CreatedDate"] = "2025-05-16 11:32:30.637";
            row["UpdatedByFullName"] = "Esquire Bank";
            row["UpdatedDate"] = "2025-05-16 11:32:30.637";
            row["DeletedByFullName"] = null;
            row["DeletedDate"] = null;
            row["IsDeleted"] = "0";
            row["TotalRows"] = "1";
            mockDataTable.Rows.Add(row);
            _reportServicesMock?.Setup(service => service.GetReports("spa_RM_MCF_Get_RiskReport_MerchantNote", It.IsAny<FilterParameterCollection>())).Returns(mockDataTable);
            var result = _riskReportNoteBussiness?.GetRiskReportNotes(request, _currentUser);
            Assert.IsTrue(result != null);
        }


        [TestMethod]
        public void GetRiskReportNotes_ReturnSuccess()
        {
            var request = new GetRiskReportNoteRequest()
            {
                UserMode = _userMode,
                MerchantNumber = "408100003502",
                IsHasRiskReport = true,
                IsExporting = false
            };

            DataTable mockDataTable = new DataTable();
            mockDataTable.Columns.Add("ASClientID");
            mockDataTable.Columns.Add("NoteSourceId");
            mockDataTable.Columns.Add("NotesSourceDesc");
            mockDataTable.Columns.Add("MerchantNoteID");
            mockDataTable.Columns.Add("CaseID");
            mockDataTable.Columns.Add("CaseTitle");
            mockDataTable.Columns.Add("CaseNumber");
            mockDataTable.Columns.Add("CaseNumberTitle");
            mockDataTable.Columns.Add("Comment");
            mockDataTable.Columns.Add("UserRoleID");
            mockDataTable.Columns.Add("HierarchyName");
            mockDataTable.Columns.Add("CreatedByFullName");
            mockDataTable.Columns.Add("CreatedDate");
            mockDataTable.Columns.Add("UpdatedByFullName");
            mockDataTable.Columns.Add("UpdatedDate");
            mockDataTable.Columns.Add("DeletedByFullName");
            mockDataTable.Columns.Add("DeletedDate");
            mockDataTable.Columns.Add("IsDeleted");
            mockDataTable.Columns.Add("TotalRows");
            DataRow row = mockDataTable.NewRow();
            row["ASClientID"] = "200";
            row["NoteSourceId"] = "2";
            row["NotesSourceDesc"] = "Risk Report";
            row["MerchantNoteID"] = "7972";
            row["CaseID"] = null;
            row["CaseTitle"] = null;
            row["CaseNumber"] = null;
            row["CaseNumberTitle"] = null;
            row["Comment"] = "Test Risk Note";
            row["UserRoleID"] = "20001";
            row["HierarchyName"] = "AS Users";
            row["CreatedByFullName"] = "Esquire Bank";
            row["CreatedDate"] = "2025-05-16 11:32:30.637";
            row["UpdatedByFullName"] = "Esquire Bank";
            row["UpdatedDate"] = "2025-05-16 11:32:30.637";
            row["DeletedByFullName"] = null;
            row["DeletedDate"] = null;
            row["IsDeleted"] = "0";
            row["TotalRows"] = "23";
            mockDataTable.Rows.Add(row);
            DataRow rowDel = mockDataTable.NewRow();
            rowDel["ASClientID"] = "200";
            rowDel["NoteSourceId"] = "2";
            rowDel["NotesSourceDesc"] = "Risk Report";
            rowDel["MerchantNoteID"] = "7973";
            rowDel["CaseID"] = null;
            rowDel["CaseTitle"] = null;
            rowDel["CaseNumber"] = null;
            rowDel["CaseNumberTitle"] = null;
            rowDel["Comment"] = "Test Risk Note 2";
            rowDel["UserRoleID"] = "20001";
            rowDel["HierarchyName"] = "AS Users";
            rowDel["CreatedByFullName"] = "Esquire Bank";
            rowDel["CreatedDate"] = "2025-05-16 11:32:30.637";
            rowDel["UpdatedByFullName"] = "Esquire Bank";
            rowDel["UpdatedDate"] = "2025-05-16 11:32:30.637";
            rowDel["DeletedByFullName"] = "Esquire Bank";
            rowDel["DeletedDate"] = "2025-05-16 15:10:30.550";
            rowDel["IsDeleted"] = "1";
            rowDel["TotalRows"] = "2";
            mockDataTable.Rows.Add(rowDel);
            _reportServicesMock?.Setup(service => service.GetReports("spa_RM_MCF_Get_RiskReport_MerchantNote", It.IsAny<FilterParameterCollection>())).Returns(mockDataTable);
            var result = _riskReportNoteBussiness?.GetRiskReportNotes(request, _currentUser);
            Assert.IsTrue(result != null);
        }
    }
}
