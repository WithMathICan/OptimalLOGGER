using LoggerWithInternalLogger.Utils;

namespace LoggerWithInternalLogger.Logger {
    internal class FileLogger(IFileWriter fileWriter) : LoggerBase {
        private readonly IFileWriter _fileWriter = fileWriter;

        public override void Log(LogLevel level, string message) {
            string logMessage = FormatMessage(level, message);
            _fileWriter.AppendLine(logMessage);
        }
    }
}
