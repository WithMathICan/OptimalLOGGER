namespace Logger {

    internal interface IAppLogger {
        void Log(LogLevel level, string message);
    }

    internal record class LogEntry(LogLevel level, DateTime timeStamp, string message) {
        internal string Level = LogLevelFactory.GetString(level);
        internal DateTime TimeStamp = timeStamp;
        internal string Message = message;
    }

    internal enum LogLevel {
        DEBUG, INFO, WARN, ERROR
    }

    internal static class LogLevelFactory {
        private static readonly string[] LogLevels;

        static LogLevelFactory() {
            LogLevels = new string[Enum.GetNames(typeof(LogLevel)).Length];

            LogLevels[(int)LogLevel.DEBUG] = string.Intern("DEBG");
            LogLevels[(int)LogLevel.INFO] = string.Intern("INFO");
            LogLevels[(int)LogLevel.WARN] = string.Intern("WARN");
            LogLevels[(int)LogLevel.ERROR] = string.Intern("ERRR");
        }

        internal static string GetString(LogLevel level) => LogLevels[(int)level];
    }
}
