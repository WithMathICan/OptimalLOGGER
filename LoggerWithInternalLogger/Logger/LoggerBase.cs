using System.Globalization;
using System.Text;

namespace LoggerWithInternalLogger.Logger {
    /// <summary>
    /// Provides a base implementation for loggers, including message formatting and disposal.
    /// </summary>
    internal abstract class LoggerBase : ILogger {
        /// <summary>
        /// Logs a message with the specified log level.
        /// </summary>
        /// <param name="level">The severity level of the log message.</param>
        /// <param name="message">The message to log.</param>
        public abstract void Log(LogLevel level, string message);

        /// <summary>
        /// Releases any resources used by the logger.
        /// </summary>
        public virtual void Dispose() { }

        /// <summary>
        /// Formats a log message with the log level, current date and time, and the message content.
        /// </summary>
        /// <param name="level">The severity level of the log message.</param>
        /// <param name="message">The message to format.</param>
        /// <returns>A formatted log message string.</returns>
        protected virtual string FormatMessage(LogLevel level, string message) {
            string logLevel = LogLevelFactory.GetString(level);
            string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            return string.Concat(logLevel, "   ", date, "   ", message);
        }

        protected virtual void AppendFormattedLineToStringBuilder(LogEntry logEntry, StringBuilder sb) {
            sb.Append(LogLevelFactory.GetString(logEntry.Level))
                .Append("  ")
                .AppendFormat("{0:yyyy-MM-dd HH:mm:ss}", logEntry.Date)
                .Append("  ")
                .AppendLine(logEntry.Message);
        }
    }
}
