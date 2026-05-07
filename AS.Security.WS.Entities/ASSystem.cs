using System;
using System.Data;
using System.ComponentModel;
using System.Collections;

namespace AS.Security.WS.Entities
{
    public class ASSystem
    {
        #region Constructors

        public ASSystem() { }

        #endregion

        #region Properties

        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is int</value>
        public int SystemId {get; set; }
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is int</value>
        public int ASClient { get; set; }
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is varchar</value>
        public string SystemShortDescription { get; set; }
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is varchar</value>
        public string SystemLongDescription { get; set; }
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is varchar</value>
        public string SystemHelp { get; set; }
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is datetime</value>
        public DateTime DateCreated { get; set; }
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is varchar</value>
        public string CreatedBy { get; set; }
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is datetime</value>
        public DateTime DateUpdated { get; set; }
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is varchar</value>
        public string UpdatedBy { get; set; }

        public string ActvStatus { get; set; }

        #endregion
    }//End Class


    //System Entity Collections
    [Serializable]
    public class ASSystemCollection : CollectionBase
    {

        public ASSystem this[int index]
        {
            get { return ((ASSystem)List[index]); }
            set { List[index] = value; }
        }

        public int Add(ASSystem value)
        {
            return (List.Add(value));
        }

        public int IndexOf(ASSystem value)
        {
            return (List.IndexOf(value));
        }

        public void Insert(int index, ASSystem value)
        {
            List.Insert(index, value);
        }

        public void Remove(ASSystem value)
        {
            List.Remove(value);
        }

        public bool Contains(ASSystem value)
        {
            // If value is not of type System, this will return false.
            return (List.Contains(value));
        }

    }
}
