using AS.VW.Api.RestClient;
using System;

namespace AS.VW.PCI.Api.Client.Common
{
    /// <summary>
    /// The logger
    /// </summary>
    /// <seealso cref="ILogger" />
    public sealed class VWLogger : ILogger
    {
        /// <summary>
        /// The lazy instance
        /// </summary>
        private static readonly Lazy<VWLogger> _lazyInstance = new Lazy<VWLogger>(() => new VWLogger());

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static VWLogger Instance
        {
            get { return _lazyInstance.Value; }
        }

        /// <summary>
        /// Log message as level: DEBUG.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Debug(object message)
        {
            AS.Common.Logger.LoggerManager.Debug(message);
        }

        /// <summary>
        /// Log message as level: DEBUG.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void Debug(object message, Exception exception)
        {
            AS.Common.Logger.LoggerManager.Debug(message, exception);
        }

        /// <summary>
        /// Log message as level: ERROR.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Error(object message)
        {
            AS.Common.Logger.LoggerManager.Error(message);
        }

        /// <summary>
        /// Log message as level: ERROR.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void Error(object message, Exception exception)
        {
            AS.Common.Logger.LoggerManager.Error(message, exception);
        }

        /// <summary>
        /// Log message as level: FATAL.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Fatal(object message)
        {
            AS.Common.Logger.LoggerManager.Fatal(message);
        }

        /// <summary>
        /// Log message as level: FATAL.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void Fatal(object message, Exception exception)
        {
            AS.Common.Logger.LoggerManager.Fatal(message, exception);
        }

        /// <summary>
        /// Log message as level: INFO.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Info(object message)
        {
            AS.Common.Logger.LoggerManager.Info(message);
        }

        /// <summary>
        /// Log message as level: INFO.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void Info(object message, Exception exception)
        {
            AS.Common.Logger.LoggerManager.Info(message, exception);
        }

        /// <summary>
        /// Log message as level: WARN.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Warn(object message)
        {
            AS.Common.Logger.LoggerManager.Warn(message);
        }

        /// <summary>
        /// Log message as level: WARN.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void Warn(object message, Exception exception)
        {
            AS.Common.Logger.LoggerManager.Warn(message, exception);
        }
    }
}
