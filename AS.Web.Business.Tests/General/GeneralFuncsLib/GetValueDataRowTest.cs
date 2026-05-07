using System.Data;

namespace AS.Web.Business.Tests.General.GeneralFuncsLib
{
    [TestClass]
    public class GetValueDataRowTest
    {

        [TestMethod]
        public void GetValueDataRow_ReturnNull()
        {
            var result = Business.General.GeneralFuncsLib.GetValueDataRow(null, null);
            Assert.AreEqual(result, string.Empty);
        }
        [TestMethod]
        public void GetValueDataRow_ReturnNotNull()
        {
            // Mock the expected DataTable
            DataTable mockDataTable = new DataTable();
            mockDataTable.Columns.Add("DNX");
            mockDataTable.Columns.Add("NDXPercent");
            mockDataTable.Columns.Add("LastNDXUpdatedBy");
            mockDataTable.Columns.Add("LastNDXPerUpdatedBy");
            DataRow row = mockDataTable.NewRow();
            row["DNX"] = "200";
            row["NDXPercent"] = "50";
            row["LastNDXUpdatedBy"] = "System";
            mockDataTable.Rows.Add(row);
            var result = Business.General.GeneralFuncsLib.GetValueDataRow(mockDataTable.Rows[0], "DNX");
            Assert.IsNotNull(result);
        }


        [TestMethod]
        public void GetValueDataRow_DefaultValue_ReturnNull()
        {
            var result = Business.General.GeneralFuncsLib.GetValueDataRow(null, null, null);
            Assert.AreEqual(result, string.Empty);
        }

        [TestMethod]
        public void GetValueDataRow_DefaultValue_ReturnDefaultValue()
        {
            // Mock the expected DataTable
            DataTable mockDataTable = new DataTable();
            mockDataTable.Columns.Add("DNX");
            mockDataTable.Columns.Add("NDXPercent");
            mockDataTable.Columns.Add("LastNDXUpdatedBy");
            mockDataTable.Columns.Add("LastNDXPerUpdatedBy");
            DataRow row = mockDataTable.NewRow();
            row["DNX"] = "200";
            row["NDXPercent"] = "50";
            row["LastNDXUpdatedBy"] = "System";
            row["LastNDXPerUpdatedBy"] = "";
            mockDataTable.Rows.Add(row);
            string defaultVal = "-";
            var result = Business.General.GeneralFuncsLib.GetValueDataRow(mockDataTable.Rows[0], "LastNDXPerUpdatedBy", defaultVal);
            Assert.AreEqual(result, defaultVal);
        }

        [TestMethod]
        public void GetValueDataRow_DefaultValue_ReturnNotNull()
        {
            // Mock the expected DataTable
            DataTable mockDataTable = new DataTable();
            mockDataTable.Columns.Add("DNX");
            mockDataTable.Columns.Add("NDXPercent");
            mockDataTable.Columns.Add("LastNDXUpdatedBy");
            mockDataTable.Columns.Add("LastNDXPerUpdatedBy");
            DataRow row = mockDataTable.NewRow();
            row["DNX"] = "200";
            row["NDXPercent"] = "50";
            row["LastNDXUpdatedBy"] = "System";
            mockDataTable.Rows.Add(row);
            var result = Business.General.GeneralFuncsLib.GetValueDataRow(mockDataTable.Rows[0], "DNX", "-");
            Assert.IsNotNull(result);
        }


    }
}
