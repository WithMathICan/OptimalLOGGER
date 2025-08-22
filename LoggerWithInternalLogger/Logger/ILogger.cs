namespace LoggerWithInternalLogger.Logger {
    internal interface ILogger : IDisposable {
        void Log(LogLevel level, string message);
    }
}
