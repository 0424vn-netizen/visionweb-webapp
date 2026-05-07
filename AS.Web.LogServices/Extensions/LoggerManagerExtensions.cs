using AS.Common.Logger;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AS.Web.LogServices.Extensions
{
    public static class LoggerManagerExtensions
    {
        public static void InfoRaw(string message)
        {
            try
            {                
                var loggerField = typeof(LoggerManager).GetField("asLogger",
                    BindingFlags.NonPublic | BindingFlags.Static);

                if (loggerField == null)
                {
                    LoggerManager.Error("MetricsLogger find LoggerManager not found");
                    return;
                }

                var logger = loggerField.GetValue(null);
                if (logger == null)
                {
                    LoggerManager.Error("MetricsLogger get LoggerManager is null");
                    return;
                }
                
                var propLogger = logger.GetType().GetProperty("Logger");
                var innerLogger = propLogger?.GetValue(logger, null);
                if (innerLogger == null)
                {
                    LoggerManager.Error("MetricsLogger find Inner logger is null");
                    return;
                }
                
                var logMethod = innerLogger.GetType().GetMethod("Log",
                    new[] { typeof(Type), typeof(log4net.Core.Level), typeof(object), typeof(Exception) });

                if (logMethod != null)
                {
                    logMethod.Invoke(innerLogger, new object[]
                    {
                        typeof(LoggerManagerExtensions),
                        log4net.Core.Level.Info,
                        message,
                        null
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Error($"MetricsLogger find LoggerManager] Error: {ex.Message}");                
            }
        }
    }
}
