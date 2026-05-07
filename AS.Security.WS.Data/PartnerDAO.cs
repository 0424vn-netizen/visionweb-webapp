using System;
using System.Data;
using System.Data.Common;
using AS.Security.WS.Entities;

namespace AS.Security.WS.Data
{
    public class PartnerDao : BaseSecDao
    {
        ///Description for BindPartner
        ///Author:
        ///Date: 

        public PartnerDao(string connString) : base(connString) { }

        public Partner BindPartner(IDataReader reader)
        {
            Partner item = new Partner();

            if (!reader.IsDBNull(reader.GetOrdinal("PartnerName"))) item.PartnerName = (string)reader["PartnerName"];
            if (!reader.IsDBNull(reader.GetOrdinal("PartnerID"))) item.PartnerID = (int)reader["PartnerID"];
            if (!reader.IsDBNull(reader.GetOrdinal("ProductionEndpointURL"))) item.ProductionEndpointURL = (string)reader["ProductionEndpointURL"];
            if (!reader.IsDBNull(reader.GetOrdinal("SandboxTestURL"))) item.SandboxTestURL = (string)reader["SandboxTestURL"];
            if (!reader.IsDBNull(reader.GetOrdinal("AperiaContactEmail"))) item.AperiaContactEmail = (string)reader["AperiaContactEmail"];
            if (!reader.IsDBNull(reader.GetOrdinal("ResponseSigned"))) item.ResponseSigned = (bool)reader["ResponseSigned"];
            if (!reader.IsDBNull(reader.GetOrdinal("ResponseSignedCertPath"))) item.ResponseSignedCertPath = (string)reader["ResponseSignedCertPath"];
            if (!reader.IsDBNull(reader.GetOrdinal("AssertionSigned"))) item.AssertionSigned = (bool)reader["AssertionSigned"];
            if (!reader.IsDBNull(reader.GetOrdinal("AssertionSignedCertPath"))) item.AssertionSignedCertPath = (string)reader["AssertionSignedCertPath"];
            if (!reader.IsDBNull(reader.GetOrdinal("AssertionEncrypted"))) item.AssertionEncrypted = (bool)reader["AssertionEncrypted"];
            if (!reader.IsDBNull(reader.GetOrdinal("AperiaCertPath"))) item.AperiaCertPath = (string)reader["AperiaCertPath"];
            if (!reader.IsDBNull(reader.GetOrdinal("AperiaCertPassword"))) item.AperiaCertPassword = (string)reader["AperiaCertPassword"];
            if (!reader.IsDBNull(reader.GetOrdinal("UserTypeID"))) item.UserTypeID = (int)reader["UserTypeID"];
            if (!reader.IsDBNull(reader.GetOrdinal("PermissionLevelIsPassed"))) item.PermissionLevelIsPassed = (bool)reader["PermissionLevelIsPassed"];
            if (!reader.IsDBNull(reader.GetOrdinal("ASClientID"))) item.ASClientID = (int)reader["ASClientID"];

            return item;
        }

        public PartnerCollection GetPartners(string PartnerName)
        {

            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_GetPartner");
            SecurityDatabase.AddInParameter(command, "@PartnerName", DbType.String, PartnerName);
            
            PartnerCollection PartnerCollection = new PartnerCollection();
            using (IDataReader reader = base.ExecuteReader(command))
            {
                while (reader.Read())
                {
                    try
                    {
                        Partner _Partner = BindPartner(reader);
                   
                        PartnerCollection.Add(_Partner);

                    }
                    catch (Exception ex)
                    {
                        AS.Common.Logger.LoggerManager.Error(string.Format("SAML: Error while reading out of the Partner Config table; PartnerName={0}; Error=\n", PartnerName), ex);
                    }
                }
                reader.Close();
            }
            return PartnerCollection;
        }

    }
}
