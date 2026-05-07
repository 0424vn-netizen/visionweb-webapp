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
    public class AppConfigDao : BaseSecDao
    {
        public AppConfigDao(string connString) : base(connString) { }

        public static AppConfig CreateAppConfig(System.Data.IDataReader reader)
        {
            List<String> columnsName = new List<String>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                columnsName.Add(reader.GetName(i));
            }

            AppConfig item = new AppConfig();
            if (columnsName.IndexOf("AppCode") >= 0 && !reader.IsDBNull(reader.GetOrdinal("AppCode"))) 
                item.AppCode = (String)reader["AppCode"];
            if (columnsName.IndexOf("KeyName") >= 0 && !reader.IsDBNull(reader.GetOrdinal("KeyName")))
                item.KeyName = (String)reader["KeyName"];
            if (columnsName.IndexOf("KeyValue") >= 0 && !reader.IsDBNull(reader.GetOrdinal("KeyValue")))
                item.KeyValue = (String)reader["KeyValue"];
            if (columnsName.IndexOf("SortSeq") >= 0 && !reader.IsDBNull(reader.GetOrdinal("SortSeq")))
                item.SortSeq = (int)reader["SortSeq"];

            return item;

        }
        

        public AppConfigCollection GetAppConfigByAppCode(string appCode)
        {

            AppConfigCollection retValue = new AppConfigCollection();
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_REF_GetAppConfig", appCode);

            using (IDataReader reader = base.ExecuteReader(command))
            {
                while (reader.Read())
                {
                    AppConfig item = CreateAppConfig(reader);
                    retValue.Add(item);
                }
                reader.Close();
            }
            return retValue;
        }
    }
}
