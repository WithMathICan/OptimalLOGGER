using Logger.Utils;

namespace Logger.Logger {
    internal class LoggerSimple(IFileWriter fileWriter) : IAppLogger {
        private readonly IFileWriter _fileWriter = fileWriter;

        public void Log(LogLevel level, string message) {
            string logLevel = LogLevelFactory.GetString(level);
            string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string logMessage = string.Concat(logLevel, "   ", date, "   ", message, Environment.NewLine);
            _fileWriter.AppendText(logMessage);
        }
    }
}
