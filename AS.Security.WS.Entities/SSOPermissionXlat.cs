using System;
using System.Collections;

namespace AS.Security.WS.Entities
{
    // SSOPermissionXlat Entity
    [Serializable]
    public class SsoPermissionXlat
    {
        #region Constructors

        public SsoPermissionXlat() { }

        public SsoPermissionXlat(

            int securityLevelId,
            string securityLevelName,
            int permissionId,
            bool isExcluded,
            int clientId,
            int partnerId,
            string permissionCode
            )
        {
            SecurityLevelId = securityLevelId;
            SecurityLevelName = securityLevelName;
            PermissionId = permissionId;    // VisionWeb permission to be modified during translation
            IsExcluded = isExcluded;
            ClientId = clientId;
            PartnerId = partnerId;
            PermissionCode = permissionCode;
        }

        #endregion

        #region Properties


        public int SecurityLevelId { get; set; }

        public string SecurityLevelName { get; set; } = string.Empty;

        public int PermissionId { get; set; }

        public bool IsExcluded { get; set; }

        public int ClientId { get; set; }

        public int PartnerId { get; set; }

        public string PermissionCode { get; set; }

        #endregion
    }


    //Permission Entity Collections
    [Serializable]
    public class SsoPermissionXlatCollection : CollectionBase
    {

        public SsoPermissionXlat this[int index]
        {
            get { return ((SsoPermissionXlat)List[index]); }
            set { List[index] = value; }
        }

        public int Add(SsoPermissionXlat value)
        {
            return (List.Add(value));
        }

        public int IndexOf(SsoPermissionXlat value)
        {
            return (List.IndexOf(value));
        }

        public void Insert(int index, SsoPermissionXlat value)
        {
            List.Insert(index, value);
        }

        public void Remove(SsoPermissionXlat value)
        {
            List.Remove(value);
        }

        public bool Contains(SsoPermissionXlat value)
        {
            // If value is not of type SSOPermissionXlat, this will return false.
            return (List.Contains(value));
        }

    }
   



}



