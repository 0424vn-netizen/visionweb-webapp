using AS.Common.DBManager;
using AS.Security.WS.Entities;
using AS.Web.Business;
using AS.Web.Business.PauseMerchantAlert.Impl;
using AS.Web.Business.PauseMerchantAlert.Models;
using Moq;
using System.Data;
using System.Diagnostics.CodeAnalysis;

[assembly: ExcludeFromCodeCoverage]
namespace AS.Web.Business.Tests
{
    [TestClass]
    public class PauseMerchantAlertBusinessTests
    {
        private Mock<IReportServices> _reportServicesMock;
        private PauseMerchantAlertBusiness _pauseMerchantAlertBusiness;
        private const string userMode = "userMode";
        private User _currentUser;

        [TestInitialize]
        public void Setup()
        {
            _reportServicesMock = new Mock<IReportServices>();
            _pauseMerchantAlertBusiness = new PauseMerchantAlertBusiness(_reportServicesMock.Object);
            _currentUser = new User
            {
                UserID = "testUser",
                ASClient = 123,
                SiteID = 456
            };
        }

        [TestMethod]
        public void GetPauseMerchantAlertFilters_ReturnsExpectedDataTable()
        {
            // Arrange
            string assignmentId = "789";
            var expectedDataTable = new DataTable();
            expectedDataTable.Columns.Add("Column1", typeof(string));
            expectedDataTable.Rows.Add("Value1");

            _reportServicesMock.Setup(r => r.GetReports("spa_RM_MCF_Get_Assignment_PauseDateRange", It.IsAny<FilterParameterCollection>())).Returns(expectedDataTable);

            // Act
            DataTable result = _pauseMerchantAlertBusiness.GetPauseMerchantAlertFilters(userMode, _currentUser, assignmentId);

            // Assert
            Assert.AreEqual(expectedDataTable, result);
            _reportServicesMock.Verify(r => r.GetReports("spa_RM_MCF_Get_Assignment_PauseDateRange", It.IsAny<FilterParameterCollection>()), Times.Once);
        }

        [TestMethod]
        public void GetPauseMerchantAlertFilters_ReportServiceReturnsNull_ReturnsNull()
        {
            // Arrange            
            string assignmentId = "123";

            _reportServicesMock.Setup(r => r.GetReports("spa_RM_MCF_Get_Assignment_PauseDateRange", It.IsAny<FilterParameterCollection>())).Returns((DataTable)null);

            // Act
            DataTable result = _pauseMerchantAlertBusiness.GetPauseMerchantAlertFilters(userMode, _currentUser, assignmentId);

            // Assert
            Assert.IsNull(result);
        }


        [TestMethod]
        public void DeleteAssignmentPauseDateRange_ValidInput_ReturnsExpectedResult()
        {
            // Arrange            
            string assignmentId = "789";
            string rowGuid = "rowGuid";

            var outputParams = new FilterParameterCollection();
            _reportServicesMock.Setup(rs => rs.ExecuteNonQueryCommand(It.IsAny<string>(), It.IsAny<FilterParameterCollection>(), out outputParams)).Returns(1);

            // Act
            int result = _pauseMerchantAlertBusiness.DeleteAssignmentPauseDateRange(userMode, _currentUser, assignmentId, rowGuid);

            // Assert
            Assert.AreEqual(1, result); // assuming 1 is the expected result
            _reportServicesMock.Verify(service => service.ExecuteNonQueryCommand("spa_RM_MCF_Delete_Assignment_PauseDateRange", It.IsAny<FilterParameterCollection>(), out outputParams), Times.Once);
        }

        [TestMethod]
        public void GetAllMerchants_ValidInput_ReturnsExpectedDataTable()
        {
            // Arrange
            string merchantIDOrmerchantName = "Test";

            // Mock the expected DataTable
            DataTable mockDataTable = new DataTable();
            mockDataTable.Columns.Add("MerchantID");
            mockDataTable.Columns.Add("MerchantName");
            DataRow row = mockDataTable.NewRow();
            row["MerchantID"] = "123";
            row["MerchantName"] = "Test Merchant";
            mockDataTable.Rows.Add(row);

            _reportServicesMock.Setup(service => service.GetReports("spa_RM_MCF_GetMerchantList", It.IsAny<FilterParameterCollection>())).Returns(mockDataTable);

            // Act
            DataTable result = _pauseMerchantAlertBusiness.GetAllMerchants(merchantIDOrmerchantName, userMode, _currentUser);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Rows.Count);
            Assert.AreEqual("123", result.Rows[0]["MerchantID"]);
            Assert.AreEqual("Test Merchant", result.Rows[0]["MerchantName"]);

            _reportServicesMock.Verify(service => service.GetReports("spa_RM_MCF_GetMerchantList", It.IsAny<FilterParameterCollection>()), Times.Once);
        }

        [TestMethod]
        public void GetAllMerchants_EmptyMerchantName_ReturnsDataTable()
        {
            // Arrange
            string merchantIDOrmerchantName = null;

            // Mock the expected DataTable
            DataTable mockDataTable = new DataTable();
            mockDataTable.Columns.Add("MerchantID");
            mockDataTable.Columns.Add("MerchantName");
            DataRow row = mockDataTable.NewRow();
            row["MerchantID"] = "123";
            row["MerchantName"] = "Test Merchant";
            mockDataTable.Rows.Add(row);

            _reportServicesMock.Setup(service => service.GetReports("spa_RM_MCF_GetMerchantList", It.IsAny<FilterParameterCollection>())).Returns(mockDataTable);

            // Act
            DataTable result = _pauseMerchantAlertBusiness.GetAllMerchants(merchantIDOrmerchantName, userMode, _currentUser);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Rows.Count);
            Assert.AreEqual("123", result.Rows[0]["MerchantID"]);
            Assert.AreEqual("Test Merchant", result.Rows[0]["MerchantName"]);
        }

        [TestMethod]
        public void SaveAssignmentPauseDateRange_ValidInput_ReturnsExpectedResult()
        {
            // Arrange            
            string assignmentId = "1";
            string data = "data";

            var outputParams = new FilterParameterCollection();
            _reportServicesMock.Setup(service => service.ExecuteNonQueryCommand("spa_RM_MCF_Save_Assignment_PauseDateRange", It.IsAny<FilterParameterCollection>(), out outputParams)).Returns(1);

            // Act
            int result = _pauseMerchantAlertBusiness.SaveAssignmentPauseDateRange(userMode, _currentUser, assignmentId, data);

            // Assert
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void GetMerchantFilters_KeywordIsValid_ReturnData()
        {
            // Arrange
            string customfilterstring = @"{""take"":31,""skip"":0,""page"":1,""pageSize"":31,""filter"":{""logic"":""and"",""filters"":[{""value"":""shop"",""field"":""DataText"",""operator"":""contains"",""ignoreCase"":true}]},""selectedDataKeys"":[]}";

            // Mock the expected DataTable
            DataTable mockDataTable = new DataTable();
            mockDataTable.Columns.Add("DataKey");
            mockDataTable.Columns.Add("DataText");
            mockDataTable.Columns.Add("TotalRows", typeof(int));
            DataRow row = mockDataTable.NewRow();
            row["DataKey"] = "1";
            row["DataText"] = "shop";
            row["TotalRows"] = 1;
            mockDataTable.Rows.Add(row);

            _reportServicesMock.Setup(service => service.GetReports("spa_RM_MCF_GetMerchantList", It.IsAny<FilterParameterCollection>())).Returns(mockDataTable);

            // Act
            var result = _pauseMerchantAlertBusiness.GetMerchantFilters(userMode, _currentUser, customfilterstring);

            // Assert
            Assert.IsTrue(result.Data.Length > 0);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void GetMerchantFilters_KeywordIsValid_ReturnNoData()
        {
            // Arrange
            string customfilterstring = @"{""take"":31,""skip"":0,""page"":1,""pageSize"":31,""filter"":{""logic"":""and"",""filters"":[{""value"":""test"",""field"":""DataText"",""operator"":""contains"",""ignoreCase"":true}]},""selectedDataKeys"":[]}";

            // Mock the expected DataTable
            DataTable mockDataTable = new DataTable();
            mockDataTable.Columns.Add("DataKey");
            mockDataTable.Columns.Add("DataText");

            _reportServicesMock.Setup(service => service.GetReports("spa_RM_MCF_GetMerchantList", It.IsAny<FilterParameterCollection>())).Returns(mockDataTable);

            // Act
            var result = _pauseMerchantAlertBusiness.GetMerchantFilters(userMode, _currentUser, customfilterstring);

            // Assert
            Assert.IsTrue(result.Data.Length == 0);
            Assert.IsTrue(result.Count == 0);
        }

        [TestMethod]
        public void GetMerchantFilters_SelectedDataKeys_ReturnData()
        {
            // Arrange
            string customfilterstring = @"{""take"":31,""skip"":0,""page"":1,""pageSize"":31,""filter"":{""logic"":""and"",""filters"":[{""value"":""shop"",""field"":""DataText"",""operator"":""contains"",""ignoreCase"":true}]},""selectedDataKeys"":[""2""]}";

            // Mock the expected DataTable
            DataTable mockDataTable = new DataTable();
            mockDataTable.Columns.Add("DataKey");
            mockDataTable.Columns.Add("DataText");
            mockDataTable.Columns.Add("TotalRows", typeof(int));
            DataRow row1 = mockDataTable.NewRow();
            row1["DataKey"] = "1";
            row1["DataText"] = "shop 1";
            row1["TotalRows"] = 2;
            mockDataTable.Rows.Add(row1);

            DataRow row2 = mockDataTable.NewRow();
            row2["DataKey"] = "2";
            row2["DataText"] = "shop 2";
            row2["TotalRows"] = 2;
            mockDataTable.Rows.Add(row2);

            _reportServicesMock.Setup(service => service.GetReports("spa_RM_MCF_GetMerchantList", It.IsAny<FilterParameterCollection>())).Returns(mockDataTable);

            // Act
            var result = _pauseMerchantAlertBusiness.GetMerchantFilters(userMode, _currentUser, customfilterstring);

            // Assert
            Assert.IsTrue(result.Data.Length > 0);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void GetMerchantFilters_KeywordIsEmpty_ReturnNoData()
        {
            // Arrange
            string customfilterstring = @"{""take"":31,""skip"":0,""page"":1,""pageSize"":31,""selectedDataKeys"":[]}";

            // Act
            var result = _pauseMerchantAlertBusiness.GetMerchantFilters(userMode, _currentUser, customfilterstring);

            // Assert
            Assert.IsTrue(result.Data.Length == 0);
            Assert.IsTrue(result.Count == 0);
        }

        [TestMethod]
        public void GetMerchantFilters_PauseMerchantAlertResponse_OnException()
        {
            // Arrange
            string customfilterstring = @"{""take"":31,""skip"":0,""page"":1,""pageSize"":31,""filter"":{""logic"":""and"",""filters"":[{""value"":""shop"",""field"":""DataText"",""operator"":""contains"",""ignoreCase"":true}]},""selectedDataKeys"":[]}";

            _reportServicesMock.Setup(service => service.GetReports("spa_RM_MCF_GetMerchantList", It.IsAny<FilterParameterCollection>())).Throws(new Exception("test exception"));

            // Act
            var result = _pauseMerchantAlertBusiness.GetMerchantFilters(userMode, _currentUser, customfilterstring);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("test exception", result.Errors);
        }

        [TestMethod]
        public void GetMerchantFilters_Constructor_ReturnPauseMerchantAlertResponse()
        {
            //Act
            var response = new PauseMerchantAlertResponse();

            //Assert
            Assert.IsNotNull(response);
        }
    }
}