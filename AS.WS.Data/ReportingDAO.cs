using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

using AS.WS.Entities;
using AS.Common.DBManager;

namespace AS.WS.Data
{
    public class CsReportingDao : ReportingDao
    {
        public CsReportingDao(string connString) 
            : base (connString)
        {
        }
        protected override ASSqlDatabase CreateDatabaseManager(string connectionString)
        {
            return DatabaseManager.Create(connectionString);
        }
    }
    public class MsReportingDao : ReportingDao
    {
        public MsReportingDao(string connString)
            : base(connString)
        {
        }
        protected override ASSqlDatabase CreateDatabaseManager(string connectionString)
        {
            return DatabaseManager.Create(connectionString);
        }
    }

    public class RiskDao : ReportingDao
    {
        public RiskDao(string connString)
            : base(connString)
        {
        }
        protected override ASSqlDatabase CreateDatabaseManager(string connectionString)
        {
            return DatabaseManager.Create(connectionString);
        }
    }

    public class MobileDao : ReportingDao
    {
        public MobileDao(string connString)
            : base(connString)
        {
        }
        protected override ASSqlDatabase CreateDatabaseManager(string connectionString)
        {
            return DatabaseManager.Create(connectionString);
        }
    }

    public abstract class ReportingDao : BaseDAO, IReportingDao
    {
        private readonly ASSqlDatabase _database = null;
        protected abstract ASSqlDatabase CreateDatabaseManager(string connectionString);

        protected ReportingDao(string connectionString)
        {
            _database = DatabaseManager.Create(connectionString);
        }       

        public DataTable GetReports(string spName, FilterParameterCollection _parameters)
        {
            DataSet _dts = base.GetDataSet(_database, spName, _parameters);
            if (_dts != null && _dts.Tables.Count > 0) 
                return _dts.Tables[0];
            return null;
        }

        public IDataReader GetReportsAsDataReader(string spName, FilterParameterCollection _parameters)
        {
            IDataReader reader = base.GetDataReader(_database, spName, _parameters);
            if (reader != null)
                return reader;
            return null;
        }      

        public int ExecuteNonQueryCommand(string spName, FilterParameterCollection _parameters, out FilterParameterCollection OutputParams)
        {
            return base.ExecuteNonQueryCommand(_database, spName, _parameters, out OutputParams);
        }

        public DataSet ExecuteQueryCommand(string spName, FilterParameterCollection _parameters, out FilterParameterCollection OutputParams)
        {
            return base.ExecuteQueryCommand(_database, spName, _parameters, out OutputParams);
        }

        public DataSet GetReportsAsDataSet(string spName, FilterParameterCollection _parameters)
        {
            return base.GetDataSet(_database, spName, _parameters);
        }

        public DataSet GetSecurityReportForDetectionQueue(string spaName, FilterParameterCollection _parameters)
        {
            DbCommand command = _database.GetStoredProcCommand(spaName);
            foreach (FilterParameter param in _parameters)
            {
                _database.AddInParameter(command, param.ParameterName, (DbType)Enum.ToObject(typeof(DbType), param.ParameterType), param.ParameterValue);
            }
            DataSet _dtsResult = _database.ExecuteDataSet(command);
            return _dtsResult;
        }
    }
}
