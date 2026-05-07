using System;
using System.Data;
using System.ComponentModel;
using System.Collections;

namespace AS.Security.WS.Entities
{
    [Serializable]
    public class SecMenuItem
    {
        #region Constructors

        public SecMenuItem() { }

        public SecMenuItem(
            int siteMapId,
            int systemId,
            string url,
            string title,
            string description,
            string permissionCodes,
            int parent)
        {
            SiteMapId = siteMapId;
            SystemId = systemId;
            Url = url;
            Title = title;
            Description = description;
            Permissions = permissionCodes;
            Parent = parent;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is int</value>
        public int SiteMapId { get; set; }
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is int</value>
        public int SystemId { get; set; }
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is varchar</value>
        public string Url { get; set; }
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is varchar</value>
        public string Title { get; set; }
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is varchar</value>
        public string Description { get; set; }
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is varchar</value>
        public string Permissions { get; set; }
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is int</value>
        public int Parent { get; set; }
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is int</value>
        public int NodeOrder { get; set; }
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is varchar</value>
        public string DisplayType { get; set; }

        #endregion
    }//End Class


    //SECMenuItem Entity Collections
    [Serializable]
    public class SecMenuItemCollection : CollectionBase
    {

        public SecMenuItem this[int index]
        {
            get { return ((SecMenuItem)List[index]); }
            set { List[index] = value; }
        }

        public int Add(SecMenuItem value)
        {
            return (List.Add(value));
        }

        public void Add(SecMenuItemCollection values)
        {
            for (int i = 0; i<values.Count; i++)
            {
                List.Add(values[i]);
            }
            
        }

        public int IndexOf(SecMenuItem value)
        {
            return (List.IndexOf(value));
        }

        public void Insert(int index, SecMenuItem value)
        {
            List.Insert(index, value);
        }

        public void Remove(SecMenuItem value)
        {
            List.Remove(value);
        }

        public bool Contains(SecMenuItem value)
        {
            // If value is not of type SECMenuItem, this will return false.
            return (List.Contains(value));
        }

        public System.Data.DataTable ToDataTable()
        {
            System.Data.DataTable table = new System.Data.DataTable();
            table.Columns.Add(new System.Data.DataColumn("Title", typeof(string)));
            table.Columns.Add(new System.Data.DataColumn("SiteMapId", typeof(int)));
            table.Columns.Add(new System.Data.DataColumn("Parent", typeof(int)));
            table.Columns.Add(new System.Data.DataColumn("Permissions", typeof(string)));
            System.Data.DataRow row;
            for (int i = 0; i < this.Count; i++)
            {
                row = table.NewRow();
                row["Title"] = this[i].Title;
                row["SiteMapId"] = this[i].SiteMapId;
                object o = DBNull.Value;
                if (this[i].Parent != 0) o = this[i].Parent;
                row["Parent"] = o;
                row["Permissions"] = this[i].Permissions;
                table.Rows.Add(row);

            }
            return table;
        }

    }
}
