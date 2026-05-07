using System;
using System.Data;
using System.ComponentModel;
using System.Collections;
using System.Linq;

namespace AS.Security.WS.Entities
{
    //Permission Entity
    [Serializable]
    public class Permission : IComparable<Permission>
    {
        public Permission() { }

        #region Properties
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is int</value>
        public int PermissionId { get; set; }
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is int</value>
        public int SystemId { get; set; }
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is nvarchar</value>
        public string PermissionCode { get; set; }
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is nvarchar</value>
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public string Group { get; set; }
        public string Type { get; set; }
        public string GroupFuncName { get; set; }
        public int NodeOrder { get; set; }
        public string PermissionCodesRequire { get; set; }
        #endregion

        #region Sort
        public int CompareTo(Permission p)
        {
            return this.PermissionId.CompareTo(p.PermissionId);
        }
        public override bool Equals(object obj)
        {
            var other = obj as Permission;
            if (object.ReferenceEquals(other, null))
            {
                return false;
            }
            return this.CompareTo(other) == 0;
        }
        public override int GetHashCode() 
        {
            return this.PermissionId.GetHashCode();
        }
        public static bool operator ==(Permission left, Permission right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }
            return left.Equals(right);
        }
        public static bool operator >(Permission left, Permission right)
        {
            return left.CompareTo(right) > 0;
        }
        public static bool operator >=(Permission left, Permission right)
        {
            return left.CompareTo(right) >= 0;
        }
        public static bool operator <(Permission left, Permission right)
        {
            return left.CompareTo(right) < 0;
        }
        public static bool operator <=(Permission left, Permission right)
        {
            return left.CompareTo(right) <= 0;
        }
        public static bool operator !=(Permission left, Permission right)
        {
            return !(left == right);
        }
        #endregion
    }//End Class

    public class SortPermissionByNodeOrder : IComparer
    {
        int IComparer.Compare(object x, object y)
        {

            if (((Permission)x).NodeOrder > ((Permission)y).NodeOrder)
                return 1;
            if (((Permission)x).NodeOrder < ((Permission)y).NodeOrder)
                return 0;
            else
                return -1;

        }

    }

    //Permission Entity Collections
    [Serializable]
    public class PermissionCollection : CollectionBase
    {

        public Permission this[int index]
        {
            get { return ((Permission)List[index]); }
            set { List[index] = value; }
        }

        public int Add(Permission value)
        {
            return (List.Add(value));
        }

        public int IndexOf(Permission value)
        {
            return (List.IndexOf(value));
        }

        public void Insert(int index, Permission value)
        {
            List.Insert(index, value);
        }

        public void Remove(Permission value)
        {
            List.Remove(value);
        }

        public bool Contains(Permission value)
        {
            // If value is not of type Permission, this will return false.
            return (List.Contains(value));
        }
        public void Sort()
        { 
            InnerList.Sort(new SortPermissionByNodeOrder());  
        }

    }
}
