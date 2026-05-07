using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using AS.Security.WS.Entities;
using AS.Common.DBManager;

namespace AS.Security.WS.Data
{
    public class AsSystemDao : BaseSecDao
    {
        public AsSystemDao(string connString) : base(connString) { }

        public ASSystem BindSecSystems(IDataReader reader)
        {
            ASSystem item = new ASSystem();
            if (!reader.IsDBNull(reader.GetOrdinal("SystemId"))) item.SystemId = (int)reader["SystemId"];
            if (!reader.IsDBNull(reader.GetOrdinal("ASClient"))) item.ASClient = (int)reader["ASClient"];
            if (!reader.IsDBNull(reader.GetOrdinal("SystemShortDescription"))) item.SystemShortDescription = (string)reader["SystemShortDescription"];
            if (!reader.IsDBNull(reader.GetOrdinal("SystemLongDescription"))) item.SystemLongDescription = (string)reader["SystemLongDescription"];
            if (!reader.IsDBNull(reader.GetOrdinal("SystemHelp"))) item.SystemHelp = (string)reader["SystemHelp"];
            if (!reader.IsDBNull(reader.GetOrdinal("DateCreated"))) item.DateCreated = (DateTime)reader["DateCreated"];
            if (!reader.IsDBNull(reader.GetOrdinal("CreatedBy"))) item.CreatedBy = (string)reader["CreatedBy"];
            if (!reader.IsDBNull(reader.GetOrdinal("DateUpdated"))) item.DateUpdated = (DateTime)reader["DateUpdated"];
            if (!reader.IsDBNull(reader.GetOrdinal("UpdatedBy"))) item.UpdatedBy = (string)reader["UpdatedBy"];
            if (!reader.IsDBNull(reader.GetOrdinal("ActvStatus"))) item.ActvStatus = (string)reader["ActvStatus"];
            return item;
        }

        public ASSystemCollection GetAllSystems()
        {
            
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_GetAllSystems");
            ASSystemCollection secSystemsCollection = new ASSystemCollection();
            using (IDataReader reader = base.ExecuteReader(command))
            {
                while (reader.Read())
                {
                    ASSystem secSystems = BindSecSystems(reader);
                    secSystemsCollection.Add(secSystems);
                }
                reader.Close();
            }
            return secSystemsCollection;
        }

        public ASSystemCollection GetSystemsForUser(int clientId, string userName)
        {
            
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_GetSystemsForUser");
            ASSystemCollection secSystemsCollection = new ASSystemCollection();
            SecurityDatabase.AddInParameter(command, "@ASClient", DbType.Int32, clientId);
            SecurityDatabase.AddInParameter(command, "@UserName", DbType.AnsiString, userName);

            using (IDataReader reader = base.ExecuteReader(command))
            {
                while (reader.Read())
                {
                    ASSystem secSystems = BindSecSystems(reader);
                    secSystemsCollection.Add(secSystems);
                }
                reader.Close();
            }
            return secSystemsCollection;
        }
    }
}
