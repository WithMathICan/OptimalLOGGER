namespace LoggerWithInternalLogger.Logger {
    /// <summary>
    /// A logger that writes log messages to the console.
    /// </summary>
    internal class ConsoleLogger : LoggerBase {
        /// <summary>
        /// Logs a message to the console with the specified log level.
        /// </summary>
        /// <param name="level">The severity level of the log message.</param>
        /// <param name="message">The message to log.</param>
        public override void Log(LogLevel level, string message) {
            Console.WriteLine(FormatMessage(level, message));
        }
    }
}
