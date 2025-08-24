namespace LoggerWithInternalLogger.Logger {
    internal record class LogEntry(LogLevel Level, DateTime Date, string Message);
}
