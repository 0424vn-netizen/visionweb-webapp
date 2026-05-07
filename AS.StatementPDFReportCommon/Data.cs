using AS.Common.DBManager;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System;
using System.Data;
using System.Data.Common;

namespace AS.StatementPDFReportCommon
{
    public class Data
    {
        private ASSqlDatabase _Database = null;

        public ASSqlDatabase Database
        {
            get
            {
                if (_Database == null)
                {
                    _Database = DatabaseManager.CreateDatabase("DBCONN");
                }
                return _Database;
            }
        }

        #region Command Methods
        protected DataSet GetDataSet(SqlDatabase dbmanager, string spName, FilterParameterCollection _paramaters)
        {
            try
            {
                DbCommand command = dbmanager.GetStoredProcCommand(spName);


                foreach (FilterParameter param in _paramaters)
                {
                    dbmanager.AddInParameter(command, param.ParameterName, (DbType)Enum.ToObject(typeof(DbType), param.ParameterType), param.ParameterValue);
                }

                DataSet _dtsResult = dbmanager.ExecuteDataSet(command);

                return _dtsResult;
            }
            catch (Exception ex)
            {
                AS.Common.Logger.LoggerManager.Error(spName + " " + _paramaters.ToStringWithParamInfo() + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        protected object GetValue(SqlDatabase dbmanager, string spName, FilterParameterCollection _paramaters)
        {
            try
            {
                DbCommand command = dbmanager.GetStoredProcCommand(spName);


                foreach (FilterParameter param in _paramaters)
                {
                    dbmanager.AddInParameter(command, param.ParameterName, (DbType)Enum.ToObject(typeof(DbType), param.ParameterType), param.ParameterValue);
                }

                return dbmanager.ExecuteScalar(command);
            }
            catch (Exception ex)
            {
                AS.Common.Logger.LoggerManager.Error(spName + " " + _paramaters.ToStringWithParamInfo() + Environment.NewLine + ex.ToString());
                throw;
            }
        }


        protected IDataReader GetDataReader(SqlDatabase dbmanager, string spName, FilterParameterCollection _paramaters)
        {
            try
            {
                DbCommand command = dbmanager.GetStoredProcCommand(spName);


                foreach (FilterParameter param in _paramaters)
                {
                    dbmanager.AddInParameter(command, param.ParameterName, (DbType)Enum.ToObject(typeof(DbType), param.ParameterType), param.ParameterValue);
                }

                var _dtsResult = dbmanager.ExecuteReader(command);

                return _dtsResult;
            }
            catch (Exception ex)
            {
                AS.Common.Logger.LoggerManager.Error(spName + " " + _paramaters.ToStringWithParamInfo() + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        protected DataSet ExecuteQueryCommand(SqlDatabase dbmanager, string spName, FilterParameterCollection _paramaters, out FilterParameterCollection OutputParams)
        {
            OutputParams = new FilterParameterCollection();
            try
            {
                DbCommand command = dbmanager.GetStoredProcCommand(spName);

                foreach (FilterParameter param in _paramaters)
                {
                    if (param.IsOutParameter) dbmanager.AddOutParameter(command, param.ParameterName, (DbType)Enum.ToObject(typeof(DbType), param.ParameterType), 100);
                    else dbmanager.AddInParameter(command, param.ParameterName, (DbType)Enum.ToObject(typeof(DbType), param.ParameterType), param.ParameterValue);
                }
                DataSet ds = dbmanager.ExecuteDataSet(command);
                FilterParameter OutParam = null;
                foreach (FilterParameter param in _paramaters)
                {
                    if (!param.IsOutParameter) continue;

                    // get output values and set to parameter value
                    OutParam = param.Clone();
                    OutParam.ParameterValue = dbmanager.GetParameterValue(command, param.ParameterName);
                    OutputParams.Add(OutParam);
                }
                return ds;
            }
            catch (Exception ex)
            {
                AS.Common.Logger.LoggerManager.Error(spName + " " + _paramaters.ToStringWithParamInfo() + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        protected int ExecuteNonQueryCommand(SqlDatabase dbmanager, string spName, FilterParameterCollection _paramaters, out FilterParameterCollection OutputParams)
        {
            OutputParams = new FilterParameterCollection();
            try
            {
                DbCommand command = dbmanager.GetStoredProcCommand(spName);

                foreach (FilterParameter param in _paramaters)
                {
                    if (param.IsOutParameter) dbmanager.AddOutParameter(command, param.ParameterName, (DbType)Enum.ToObject(typeof(DbType), param.ParameterType), 100);
                    else dbmanager.AddInParameter(command, param.ParameterName, (DbType)Enum.ToObject(typeof(DbType), param.ParameterType), param.ParameterValue);
                }
                int AffectedRows = dbmanager.ExecuteNonQuery(command);
                FilterParameter OutParam = null;
                foreach (FilterParameter param in _paramaters)
                {
                    if (!param.IsOutParameter) continue;

                    // get output values and set to parameter value
                    OutParam = param.Clone();
                    OutParam.ParameterValue = dbmanager.GetParameterValue(command, param.ParameterName);
                    OutputParams.Add(OutParam);
                }
                return AffectedRows;
            }
            catch (Exception ex)
            {
                AS.Common.Logger.LoggerManager.Error(spName + " " + _paramaters.ToStringWithParamInfo() + Environment.NewLine + ex.ToString());
                throw;
            }
        }
        #endregion

        public DataSet GetDataSetReports(string spName, FilterParameterCollection _paramaters)
        {
            DataSet _dts = GetDataSet(Database, spName, _paramaters);
            if (_dts != null && _dts.Tables.Count > 0) return _dts;
            return null;
        }

        public DataTable GetReports(string spName, FilterParameterCollection _paramaters)
        {
            DataSet _dts = GetDataSet(Database, spName, _paramaters);
            if (_dts != null && _dts.Tables.Count > 0) return _dts.Tables[0];
            return null;
        }

        public object GetValue(string spName, FilterParameterCollection _paramaters)
        {
            return GetValue(Database, spName, _paramaters);
        }

        public void ExecuteNonQueryCommand(string spName, FilterParameterCollection _paramaters)
        {
            ExecuteNonQueryCommand(Database, spName, _paramaters, out _paramaters);
        }
    }
}
