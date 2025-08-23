using LoggerWithInternalLogger.Utils;

namespace LoggerWithInternalLogger.Logger {
    /// <summary>
    /// Logger implementation that writes log messages to a file using an <see cref="IFileWriter"/>.
    /// </summary>
    internal class FileLogger(IFileWriter fileWriter) : LoggerBase {
        private readonly IFileWriter _fileWriter = fileWriter;

        /// <summary>
        /// Logs a message with the specified <see cref="LogLevel"/> to the file.
        /// </summary>
        /// <param name="level">The severity level of the log message.</param>
        /// <param name="message">The message to log.</param>
        public override void Log(LogLevel level, string message) {
            string logMessage = FormatMessage(level, message);
            _fileWriter.AppendLine(logMessage);
        }
    }
}
