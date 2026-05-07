using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Text;

namespace AS.Security.WS.Data
{
    public class LogStopWatch : Stopwatch
    {
        private readonly int _LogSlownessInSeconds = 10 * 1000;//10s
        public LogStopWatch()
        {
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["LogSlownessInSeconds"]))
                _LogSlownessInSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["LogSlownessInSeconds"]) * 1000;
        }

        public void WriteLog(long duration, string connectionString, string spName, DbParameterCollection paramList)
        {
            if (duration >= _LogSlownessInSeconds)
            {
                AS.Common.Logger.LoggerManager.Warn(string.Format("Duration: {0}s {2}Connection: {1} {2}exec {3} {4}", duration * 1.0 / 1000, connectionString, Environment.NewLine, spName, ToString(paramList)));
            }
        }

        public string ToString(DbParameterCollection paramList)
        {
            if (paramList == null)
                return string.Empty;
            StringBuilder result = new StringBuilder();
            foreach (DbParameter p in paramList)
            {
                var dataType = (DbType)Enum.ToObject(typeof(DbType), p.DbType);
                if (dataType == DbType.AnsiString
                    || dataType == DbType.AnsiStringFixedLength
                    || dataType == DbType.String
                    || dataType == DbType.StringFixedLength
                    || dataType == DbType.Date
                    || dataType == DbType.DateTime
                    || dataType == DbType.DateTime2)
                    result.Append(string.Format("{0}='{1}', ", p.ParameterName, p.Value));
                else
                    result.Append(string.Format("{0}={1}, ", p.ParameterName, p.Value));
            }
            return result.ToString().Trim().TrimEnd(',');
        }
    }
}

