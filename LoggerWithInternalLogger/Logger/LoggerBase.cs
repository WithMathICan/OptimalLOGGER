namespace LoggerWithInternalLogger.Logger {
    internal abstract class LoggerBase : ILogger {
        public abstract void Log(LogLevel level, string message);

        public virtual void Dispose() {}

        protected virtual string FormatMessage(LogLevel level, string message) {
            string logLevel = LogLevelFactory.GetString(level);
            string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            return string.Concat(logLevel, "   ", date, "   ", message);
        }
    }
}
