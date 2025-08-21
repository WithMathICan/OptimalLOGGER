namespace Logger {
    internal class LoggerSimple(IFileWriter fileWriter) : IAppLogger {
        private IFileWriter _fileWriter = fileWriter;

        public void Log(LogLevel level, string message) {
            string logMessage = string.Concat(LogLevelFactory.GetString(level), "   ", DateTime.Now, "   ", message);
            _fileWriter.AppendLine(logMessage);
        }
    }
}
