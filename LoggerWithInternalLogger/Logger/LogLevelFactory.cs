namespace LoggerWithInternalLogger.Logger {
    internal enum LogLevel {
        DEBUG, INFO, WARN, ERROR
    }
    internal static class LogLevelFactory {
        private static readonly string[] LogLevels;

        static LogLevelFactory() {
            LogLevels = new string[Enum.GetNames(typeof(LogLevel)).Length];

            LogLevels[(int)LogLevel.DEBUG] = "DEBG";
            LogLevels[(int)LogLevel.INFO]  = "INFO";
            LogLevels[(int)LogLevel.WARN]  = "WARN";
            LogLevels[(int)LogLevel.ERROR] = "ERRR";
        }

        internal static string GetString(LogLevel level) => LogLevels[(int)level];
    }
}
