using System;
using System.Data;
using System.Data.Common;
using AS.Security.WS.Entities;
using System.Collections.Generic;

namespace AS.Security.WS.Data
{
    public class SsoPermissionXlatDao : BaseSecDao
    {
        ///Description for BindSSOPermissionXlat
        ///Author:
        ///Date: 

        public SsoPermissionXlatDao(string connString) : base(connString) { }

        public SsoPermissionXlat BindSSOPermissionXlat(IDataReader reader)
        {
            SsoPermissionXlat item = new SsoPermissionXlat();

            if (!reader.IsDBNull(reader.GetOrdinal("SecurityLevelId"))) item.SecurityLevelId = (int)reader["SecurityLevelId"];
            if (!reader.IsDBNull(reader.GetOrdinal("SecurityLevelName"))) item.SecurityLevelName = (string)reader["SecurityLevelName"];
            if (!reader.IsDBNull(reader.GetOrdinal("PermissionId"))) item.PermissionId = (int)reader["PermissionId"];
            if (!reader.IsDBNull(reader.GetOrdinal("IsExcluded"))) item.IsExcluded = (bool)reader["IsExcluded"];
            if (!reader.IsDBNull(reader.GetOrdinal("ClientId"))) item.ClientId = (int)reader["ClientId"];
            if (!reader.IsDBNull(reader.GetOrdinal("PermissionCode"))) item.PermissionCode = (string)reader["PermissionCode"];
            
            
            return item;
        }

        public SsoPermissionXlatCollection GetSSOPermissionXlat(int PartnerID, int ClientID, int PermissionLevel)
        {
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_GetSSOPermissionXlat");
            SecurityDatabase.AddInParameter(command, "@PartnerID", DbType.Int16, PartnerID);
            SecurityDatabase.AddInParameter(command, "@ClientID", DbType.Int16, ClientID);
            SecurityDatabase.AddInParameter(command, "@SecurityLevelID", DbType.Int16, PermissionLevel);
            SsoPermissionXlatCollection ssoPermissionXlatCollection = new SsoPermissionXlatCollection();
            using (IDataReader reader = base.ExecuteReader(command))
            {
                while (reader.Read())
                {
                    SsoPermissionXlat ssoPermissionXlat = BindSSOPermissionXlat(reader);
                    ssoPermissionXlatCollection.Add(ssoPermissionXlat);
                }
                reader.Close();
            }
            return ssoPermissionXlatCollection;
        }
            

    }
}
