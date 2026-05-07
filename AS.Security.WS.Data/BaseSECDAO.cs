using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using AS.Common.DBManager;

namespace AS.Security.WS.Data
{
    public class BaseSecDao
    {
        private readonly ASSqlDatabase _SecurityDatabase = null;
        public BaseSecDao(string connString)
        {
            _SecurityDatabase = DatabaseManager.Create(connString);
        }

        protected ASSqlDatabase SecurityDatabase
        {
            get
            {                
                return _SecurityDatabase;
            }
        }

        private readonly LogStopWatch _StopWatch = new LogStopWatch();


        public virtual DataSet ExecuteDataSet(DbCommand command)
        {
            try
            {
                _StopWatch.Start();
                DataSet result = SecurityDatabase.ExecuteDataSet(command);
                _StopWatch.Stop();
                _StopWatch.WriteLog(_StopWatch.ElapsedMilliseconds, SecurityDatabase.ConnectionString, command.CommandText, command.Parameters);
                return result;
            }
            catch (Exception ex)
            {
                LogException(ex, command);
                throw;
            }
        }

        public virtual IDataReader ExecuteReader(DbCommand command)
        {
            try
            {
                _StopWatch.Start();
                IDataReader result = SecurityDatabase.ExecuteReader(command);
                _StopWatch.Stop();
                _StopWatch.WriteLog(_StopWatch.ElapsedMilliseconds, SecurityDatabase.ConnectionString, command.CommandText, command.Parameters);
                return result;
            }
            catch (Exception ex)
            {
                LogException(ex, command);
                throw;
            }
        }

        public virtual int ExecuteNonQuery(DbCommand command)
        {
            try
            {
                _StopWatch.Start();
                int result = SecurityDatabase.ExecuteNonQuery(command);
                _StopWatch.Stop();
                _StopWatch.WriteLog(_StopWatch.ElapsedMilliseconds, SecurityDatabase.ConnectionString, command.CommandText, command.Parameters);
                return result;
            }
            catch (Exception ex)
            {
                LogException(ex, command);
                throw;
            }
        }

        public virtual object ExecuteScalar(DbCommand command)
        {
            try
            {
                _StopWatch.Start();
                object result = SecurityDatabase.ExecuteScalar(command);
                _StopWatch.Stop();
                _StopWatch.WriteLog(_StopWatch.ElapsedMilliseconds, SecurityDatabase.ConnectionString, command.CommandText, command.Parameters);
                return result;
            }
            catch (Exception ex)
            {
                LogException(ex, command);
                throw;
            }
        }

        private void LogException(Exception ex, DbCommand command)
        {
            _StopWatch.Stop();
            _StopWatch.WriteLog(_StopWatch.ElapsedMilliseconds, SecurityDatabase.ConnectionString, command.CommandText, command.Parameters);
            AS.Common.Logger.LoggerManager.Error(command.CommandText + " " + _StopWatch.ToString(command.Parameters) + Environment.NewLine + ex.ToString());
        }
    }
}
