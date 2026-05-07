using AS.Common.DBManager;
using AS.Common.Logger;
using System;
using System.Data;
using System.Text;
using System.Web;

namespace AS.Security.WS.Entities.Utility
{
    public static class LogHepler
    {
        public static void WriteLogException(string method, string spName, FilterParameterCollection paramList, Exception ex)
        {
            LoggerManager.Error($"{method} Exception - {spName} {paramList.ParamsToString()} {Environment.NewLine}{ex.Message}");
        }

        public static void WriteLogInfo(string method, string message, string spName, FilterParameterCollection paramList = null)
        {
            LoggerManager.Info($"{method}: {message} {spName} {paramList.ParamsToString()}");
        }

        public static void WriteLogWarn(string method, string message, string spName, FilterParameterCollection paramList = null)
        {
            LoggerManager.Warn($"{method}: {message} {spName} {paramList.ParamsToString()}");
        }

        private static string ParamsToString(this FilterParameterCollection paramList)
        {
            if (paramList == null)
            {
                return string.Empty;
            }

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