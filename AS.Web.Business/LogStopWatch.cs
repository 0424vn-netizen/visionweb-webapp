using AS.Common.DBManager;
using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Text;

namespace AS.Web.Business
{
    public class LogStopWatch : Stopwatch
    {
        private readonly int _LogSlownessInSeconds = 10 * 1000;//10s
        public LogStopWatch()
        {
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogSlownessInSeconds"]))
                _LogSlownessInSeconds = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LogSlownessInSeconds"]);
        }

        public void WriteLog(long duration, string clientId, string spName, FilterParameterCollection paramList)
        {
            if (duration >= _LogSlownessInSeconds)
            {
                AS.Common.Logger.LoggerManager.Info(string.Format("Total={0}s; ClientId={1}; exec {2} {3}", duration * 1.0 / 1000, clientId, spName, ToString(paramList)));
            }
        }

        private string ToString(FilterParameterCollection paramList)
        {
            StringBuilder result = new StringBuilder();
            foreach (FilterParameter p in paramList)
            {
                var dataType = (DbType)Enum.ToObject(typeof(DbType), p.ParameterType);
                if (dataType == DbType.AnsiString
                    || dataType == DbType.AnsiStringFixedLength
                    || dataType == DbType.String
                    || dataType == DbType.StringFixedLength
                    || dataType == DbType.Date
                    || dataType == DbType.DateTime
                    || dataType == DbType.DateTime2)
                    result.Append(string.Format("{0}='{1}', ", p.ParameterName, p.ParameterValue));
                else
                    result.Append(string.Format("{0}={1}, ", p.ParameterName, p.ParameterValue));
            }
            return result.ToString().Trim().TrimEnd(',');
        }
    }
}

