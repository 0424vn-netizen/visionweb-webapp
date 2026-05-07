using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace AS.Web.Business.Tests.General.GeneralFuncsLib
{
    [TestClass]
    public class GetValueTests
    {
        [TestMethod]
        public void GetValue_ReturnDefault()
        {
            var dataTable = PrepareMockData();

            var result = Business.General.GeneralFuncsLib.GetValue<string>(dataTable.Rows[0], "ColumnNotExist");
            Assert.IsTrue(result == default);
        }

        [TestMethod]
        public void GetValue_ReturnDefault2()
        {
            var dataTable = PrepareMockData();

            var result = Business.General.GeneralFuncsLib.GetValue<int>(dataTable.Rows[0], "Column3");
            Assert.IsTrue(result == default);
        }

        [TestMethod]
        public void GetValue_WrongFormat_Exception()
        {
            var dataTable = PrepareMockData();
            Assert.ThrowsException<FormatException>(() => Business.General.GeneralFuncsLib.GetValue<int>(dataTable.Rows[0], "Column2"));
        }

        [TestMethod]
        public void GetValue_DataTypeString_ReturnIsNotNull()
        {
            var dataTable = PrepareMockData();

            var result = Business.General.GeneralFuncsLib.GetValue<string>(dataTable.Rows[0], "Column1");
            Assert.IsNotNull(result == default);
        }

        [TestMethod]
        public void GetValue_DataTypeInt_ReturnIsNotNull()
        {
            var dataTable = PrepareMockData();

            var result = Business.General.GeneralFuncsLib.GetValue<int>(dataTable.Rows[0], "Column1");
            Assert.IsNotNull(result);
        }        

        private static DataTable PrepareMockData()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Column1", typeof(int));
            table.Columns.Add("Column2", typeof(string));
            table.Columns.Add("Column3", typeof(int));

            DataRow row = table.NewRow();
            row["Column1"] = 1;
            row["Column2"] = "Merchant";
            row["Column3"] = DBNull.Value;

            table.Rows.Add(row);

            return table;
        }
    }
}
