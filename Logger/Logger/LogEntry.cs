namespace Logger.Logger {
    internal record class LogEntry(LogLevel level, DateTime timeStamp, string message) {
        internal string Level = LogLevelFactory.GetString(level);
        internal DateTime TimeStamp = timeStamp;
        internal string Message = message;
    }
}
