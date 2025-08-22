namespace LoggerWithInternalLogger.Logger {
    internal class ConsoleLogger : ILogger {
        private string FormatMessage(LogLevel level, string message) {
            string logLevel = LogLevelFactory.GetString(level);
            string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            return string.Concat(logLevel, "   ", date, "   ", message, Environment.NewLine);
        }

        public void Log(LogLevel level, string message) {
            Console.WriteLine(FormatMessage(level, message));
        }

        public void Dispose() {
            Console.WriteLine("ConsoleLogger is Disposed");
        }
    }
}
