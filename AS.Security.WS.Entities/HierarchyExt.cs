using System;
using System.Data;
using System.ComponentModel;
using System.Collections;

namespace AS.Security.WS.Entities
{
    [Serializable]
    public class HierarchyAccess
    {
        public HierarchyAccess() { }

        #region Property
        public Int32 RecId { get; set; }

        public String Login { get; set; }

        public String ClientName { get; set; }

        public Int32 HierarchyId { get; set; }

        public String EntityNumber { get; set; }

        public String Type { get; set; }

        public String UserIDCreate { get; set; }

        public String Sys { get; set; }

        public String Prin { get; set; }

        public String Agent { get; set; }

        public DateTime CreateDate { get; set; }

        public string DisplayText { get; set; } = "ALL FIs";

        #endregion

    }
    [Serializable]
    public class HierarchyAccessCollection : CollectionBase
    {

        public HierarchyAccess this[int index]
        {
            get { return ((HierarchyAccess)List[index]); }
            set { List[index] = value; }
        }

        public int Add(HierarchyAccess value)
        {
            return (List.Add(value));
        }

        public int IndexOf(HierarchyAccess value)
        {
            return (List.IndexOf(value));
        }

        public void Insert(int index, HierarchyAccess value)
        {
            List.Insert(index, value);
        }

        public void Remove(HierarchyAccess value)
        {
            List.Remove(value);
        }

        public bool Contains(HierarchyAccess value)
        {
            // If value is not of type SecHierarchyAccess, this will return false.
            return (List.Contains(value));
        }

        public System.Data.DataTable ToDataTable()
        {
            System.Data.DataTable table = new System.Data.DataTable();
            table.Columns.Add(new System.Data.DataColumn("RecId", typeof(Int32)));

            table.Columns.Add(new System.Data.DataColumn("Login", typeof(String)));

            table.Columns.Add(new System.Data.DataColumn("ClientName", typeof(String)));

            table.Columns.Add(new System.Data.DataColumn("HierarchyId", typeof(Int32)));

            table.Columns.Add(new System.Data.DataColumn("EntityNumber", typeof(String)));

            table.Columns.Add(new System.Data.DataColumn("Type", typeof(String)));

            table.Columns.Add(new System.Data.DataColumn("UserIDCreate", typeof(String)));

            table.Columns.Add(new System.Data.DataColumn("CreateDate", typeof(DateTime)));

            System.Data.DataRow row;
            for (int i = 0; i < this.Count; i++)
            {
                row = table.NewRow();

                row["RecId"] = this[i].RecId;

                row["Login"] = this[i].Login;

                row["ClientName"] = this[i].ClientName;

                row["HierarchyId"] = this[i].HierarchyId;


                row["EntityNumber"] = this[i].EntityNumber;

                row["Type"] = this[i].Type;

                row["UserIDCreate"] = this[i].UserIDCreate;

                row["CreateDate"] = this[i].CreateDate;


                table.Rows.Add(row);

            }
            return table;
        }

    }
}

