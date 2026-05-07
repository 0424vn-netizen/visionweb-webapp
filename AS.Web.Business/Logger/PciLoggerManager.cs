using AS.Common.DBManager;
using log4net;
using System;
using System.Data;
using System.Text;

namespace AS.Web.Business.Logger
{
    public static class PciLoggerManager
    {
        #region ctor
        private const string PCI_LOGGER = "PciLogger";
        private static ILog pciLogger = InitLogger();
        private static ILog InitLogger()
        {
            log4net.Config.XmlConfigurator.Configure();
            return LogManager.GetLogger(PCI_LOGGER);
        }
        // Internal setter for testing
        public static void SetLogger(ILog logger)
        {
            pciLogger = logger;
        }
        #endregion
        public static void Info(object message) => pciLogger.Info(message);
        public static void Debug(object message) => pciLogger.Debug(message);
        public static void Error(object message) => pciLogger.Error(message);
        public static void Error(object message, Exception ex) => pciLogger.Error(message + Environment.NewLine, ex);
        public static void Warn(object message) => pciLogger.Warn(message);
        public static void Fatal(object message) => pciLogger.Fatal(message);
        public static void Info(string message, string method, string spName, FilterParameterCollection paramList = null) => Info($"{method}: {message} {spName} {paramList.ParamsToString()}");
        public static void Debug(string message, string method, string spName, FilterParameterCollection paramList = null) => Debug($"{method}: {message} {spName} {paramList.ParamsToString()}");
        public static void Error(string message, string method, string spName, FilterParameterCollection paramList = null) => Error($"{method}: {message} {spName} {paramList.ParamsToString()}");
        public static void Warn(string message, string method, string spName, FilterParameterCollection paramList = null) => Warn($"{method}: {message} {spName} {paramList.ParamsToString()}");
        public static void Fatal(string message, string method, string spName, FilterParameterCollection paramList = null) => Fatal($"{method}: {message} {spName} {paramList.ParamsToString()}");
        public static void Info(string message, string method, string spName, FilterParameterCollection paramList, Exception ex) => Info($"{method}: {message} - Exception - {spName} {paramList.ParamsToString()} {Environment.NewLine}{ex}");
        public static void Debug(string message, string method, string spName, FilterParameterCollection paramList, Exception ex) => Debug($"{method}: {message} - Exception - {spName} {paramList.ParamsToString()} {Environment.NewLine}{ex}");
        public static void Error(string message, string method, string spName, FilterParameterCollection paramList, Exception ex) => Error($"{method}: {message} - Exception - {spName} {paramList.ParamsToString()} {Environment.NewLine}{ex}");
        public static void Warn(string message, string method, string spName, FilterParameterCollection paramList, Exception ex) => Warn($"{method}: {message} - Exception - {spName} {paramList.ParamsToString()} {Environment.NewLine}{ex}");
        public static void Fatal(string message, string method, string spName, FilterParameterCollection paramList, Exception ex) => Fatal($"{method}: {message} - Exception - {spName} {paramList.ParamsToString()} {Environment.NewLine}{ex}");

        #region private
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
        #endregion
    }
}
