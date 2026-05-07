using System;
using System.Data;
using System.ComponentModel;
using System.Collections;

namespace AS.Security.WS.Entities
{
    //SecHierarchy Entity
    [Serializable]
    public class Hierarchy
    {
        #region Constructors

        public Hierarchy() { }

        #endregion

        #region Properties

        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is int</value>
        public int HierarchyID { get; set; }
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is int</value>
        public int SystemId { get; set; }
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is varchar</value>
        public string HierarchyName { get; set; } = string.Empty;
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is int</value>
        public int HierarchyParent { get; set; }
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is varchar</value>
        public string HierarchyDescription { get; set; } = string.Empty;
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is varchar</value>
        public string ActvStatus { get; set; }

        public Guid CreatedBy { get; set; }

        public int ClientId { get; set; }

        public string HierarchyCode { get; set; } = string.Empty;

        public string UserRoleType { get; set; } = string.Empty;

        public string HierarchyLevel { get; set; } = string.Empty;

        public int UserCount { get; set; }

        public bool IsPredefined { get; set; }

        public DateTime CreatedDate { get; set; }
        #endregion
    }//End Class


    //SecHierarchy Entity Collections
    [Serializable]
    public class HierarchyCollection : CollectionBase
    {

        public Hierarchy this[int index]
        {
            get { return ((Hierarchy)List[index]); }
            set { List[index] = value; }
        }

        public int Add(Hierarchy value)
        {
            return (List.Add(value));
        }

        public int IndexOf(Hierarchy value)
        {
            return (List.IndexOf(value));
        }

        public void Insert(int index, Hierarchy value)
        {
            List.Insert(index, value);
        }

        public void Remove(Hierarchy value)
        {
            List.Remove(value);
        }

        public bool Contains(Hierarchy value)
        {
            // If value is not of type SecHierarchy, this will return false.
            return (List.Contains(value));
        }

        public DataTable ToDataTable()
        {
            System.Data.DataTable table = new System.Data.DataTable();
            table.Columns.Add(new System.Data.DataColumn("HierarchyID", typeof(Int32)));

            table.Columns.Add(new System.Data.DataColumn("SystemId", typeof(Int32)));

            table.Columns.Add(new System.Data.DataColumn("HierarchyName", typeof(String)));

            table.Columns.Add(new System.Data.DataColumn("HierarchyParent", typeof(Int32)));

            table.Columns.Add(new System.Data.DataColumn("HierarchyDescription", typeof(String)));

            table.Columns.Add(new System.Data.DataColumn("ActvStatus", typeof(String)));

            table.Columns.Add(new System.Data.DataColumn("CreatedBy", typeof(Guid)));

            table.Columns.Add(new System.Data.DataColumn("ClientId", typeof(Int32)));

            table.Columns.Add(new System.Data.DataColumn("HierarchyCode", typeof(String)));

            table.Columns.Add(new System.Data.DataColumn("UserRoleType", typeof(String)));

            table.Columns.Add(new System.Data.DataColumn("HierarchyLevel", typeof(String)));

            table.Columns.Add(new System.Data.DataColumn("UserCount", typeof(Int32)));

            table.Columns.Add(new System.Data.DataColumn("IsPredefined", typeof(bool)));

            table.Columns.Add(new System.Data.DataColumn("CreatedDate", typeof(DateTime)));

            System.Data.DataRow row;
            for (int i = 0; i < this.Count; i++)
            {
                row = table.NewRow();

                row["HierarchyID"] = this[i].HierarchyID;

                row["SystemId"] = this[i].SystemId;

                row["HierarchyName"] = this[i].HierarchyName;

                row["HierarchyParent"] = this[i].HierarchyParent;

                row["HierarchyDescription"] = this[i].HierarchyDescription;

                row["ActvStatus"] = this[i].ActvStatus;

                row["CreatedBy"] = this[i].CreatedBy;

                row["ClientId"] = this[i].ClientId;

                row["HierarchyCode"] = this[i].HierarchyCode;

                row["UserRoleType"] = this[i].UserRoleType;

                row["HierarchyLevel"] = this[i].HierarchyLevel;

                row["UserCount"] = this[i].UserCount;

                row["IsPredefined"] = this[i].IsPredefined;

                row["CreatedDate"] = this[i].CreatedDate;

                table.Rows.Add(row);

            }
            return table;
        }
    }
}
