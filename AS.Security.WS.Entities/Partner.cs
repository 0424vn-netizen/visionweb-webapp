using System;
using System.Collections;

namespace AS.Security.WS.Entities
{
    //Partner Entity
    [Serializable]
    public class Partner
    {
        #region Constructors

        public Partner() { }

        #endregion

        #region Properties

        public int ASClientID { get; set; }

        public string PartnerName { get; set; }

        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is varchar(50)</value>
        public int PartnerID { get; set; }

        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is varchar(50)</value>
        public string ProductionEndpointURL { get; set; }

        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is varchar(50)</value>
        public string SandboxTestURL { get; set; }

        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is varchar(50)</value>
        public string AperiaContactEmail { get; set; }

        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is bit</value>
        public bool ResponseSigned { get; set; }

        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is varchar(50)</value>
        public string ResponseSignedCertPath { get; set; }

        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is bit</value>
        public bool AssertionSigned { get; set; }

        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is varchar(50)</value>
        public string AssertionSignedCertPath { get; set; }


        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is bit</value>
        public bool AssertionEncrypted { get; set; }

        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is varchar(50)</value>
        public string AperiaCertPath { get; set; }


        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is varchar(50)</value>
        public string AperiaCertPassword { get; set; }

        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is varchar(50)</value>
        public int UserTypeID { get; set; }

        public bool PermissionLevelIsPassed { get; set; }

        #endregion
    }

    //Partner Entity Collections
    [Serializable]
    public class PartnerCollection : CollectionBase
    {

        public Partner this[int index]
        {
            get { return ((Partner)List[index]); }
            set { List[index] = value; }
        }

        public int Add(Partner value)
        {
            return (List.Add(value));
        }

        public int IndexOf(Partner value)
        {
            return (List.IndexOf(value));
        }

        public void Insert(int index, Partner value)
        {
            List.Insert(index, value);
        }

        public void Remove(Partner value)
        {
            List.Remove(value);
        }

        public bool Contains(Partner value)
        {
            // If value is not of type Partner, this will return false.
            return (List.Contains(value));
        }

    }



}



