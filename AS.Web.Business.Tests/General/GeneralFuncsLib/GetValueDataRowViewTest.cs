using System.Data;

namespace AS.Web.Business.Tests.General.GeneralFuncsLib
{
    [TestClass]
    public class GetValueDataRowViewTest
    {

        [TestMethod]
        public void GetValueDataRowView_ReturnNull()
        {
            var result = Business.General.GeneralFuncsLib.GetValueDataRowView(null, null);
            Assert.AreEqual(result, string.Empty);
        }
        [TestMethod]
        public void GetValueDataRowView_ReturnNotNull()
        {
            // Mock the expected DataTable
            DataTable mockDataTable = new DataTable();
            mockDataTable.Columns.Add("AssignmentName");
            mockDataTable.Columns.Add("StartDate");
            mockDataTable.Columns.Add("ExpirationDate");
            DataRow row = mockDataTable.NewRow();
            row["AssignmentName"] = "TestStartDate";
            row["StartDate"] = "08/05/2025";
            row["ExpirationDate"] = "08/31/2025";
            mockDataTable.Rows.Add(row);
            DataView dataView = new(mockDataTable);
            int intTrue = 0;
            foreach (DataRowView rowView in dataView)
            {
                var result = Business.General.GeneralFuncsLib.GetValueDataRowView(rowView, "StartDate");
                intTrue += !string.IsNullOrEmpty(result) ? 1 : 0;
            }

            Assert.AreEqual(intTrue, dataView.Count);
        }
    }
}
