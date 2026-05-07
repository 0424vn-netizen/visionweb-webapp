using System;
using System.Data;
using System.ComponentModel;
using System.Collections;

namespace AS.Security.WS.Entities
{
    //Client Entity
    [Serializable]
    public class Client
    {
        #region Constructors

        public Client() { }

        #endregion

        #region Properties

        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is int</value>
        public int ASClient { get; set; }
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is varchar</value>
        public string ClientAbbreviation { get; set; }
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is varchar</value>
        public string ClientName { get; set; }
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is varchar</value>
        public string ActvStatus { get; set; }
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

        #endregion
    }

    //Client Entity Collections
    [Serializable]
    public class ClientCollection : CollectionBase
    {

        public Client this[int index]
        {
            get { return ((Client)List[index]); }
            set { List[index] = value; }
        }

        public int Add(Client value)
        {
            return (List.Add(value));
        }

        public int IndexOf(Client value)
        {
            return (List.IndexOf(value));
        }

        public void Insert(int index, Client value)
        {
            List.Insert(index, value);
        }

        public void Remove(Client value)
        {
            List.Remove(value);
        }

        public bool Contains(Client value)
        {
            // If value is not of type Client, this will return false.
            return (List.Contains(value));
        }

    }
}
