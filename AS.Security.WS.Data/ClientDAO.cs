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
    public class ClientDao : BaseSecDao
    {
        public ClientDao(string connString) : base(connString) { }

        public Client BindASClient(IDataReader reader)
        {
            Client item = new Client();
            if (!reader.IsDBNull(reader.GetOrdinal("ASClient"))) item.ASClient = (int)reader["ASClient"];
            if (!reader.IsDBNull(reader.GetOrdinal("ClientAbbreviation"))) item.ClientAbbreviation = (string)reader["ClientAbbreviation"];
            if (!reader.IsDBNull(reader.GetOrdinal("ClientName"))) item.ClientName = (string)reader["ClientName"];
            if (!reader.IsDBNull(reader.GetOrdinal("ActvStatus"))) item.ActvStatus = (string)reader["ActvStatus"];
            if (!reader.IsDBNull(reader.GetOrdinal("DateCreated"))) item.DateCreated = (DateTime)reader["DateCreated"];
            if (!reader.IsDBNull(reader.GetOrdinal("CreatedBy"))) item.CreatedBy = (string)reader["CreatedBy"];
            if (!reader.IsDBNull(reader.GetOrdinal("DateUpdated"))) item.DateUpdated = (DateTime)reader["DateUpdated"];
            if (!reader.IsDBNull(reader.GetOrdinal("UpdatedBy"))) item.UpdatedBy = (string)reader["UpdatedBy"];
            return item;
        }
    }
}
