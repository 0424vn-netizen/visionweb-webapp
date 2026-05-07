using AS.Common.DBManager;
using AS.WS.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.WS.Data
{
   public  class LogDao: ReportingDao
    {
        public LogDao(string connString)
            : base(connString)
        {
        }

        protected override ASSqlDatabase CreateDatabaseManager(string connectionString)
        {
            return DatabaseManager.Create(connectionString);
        }

       
    }
}
