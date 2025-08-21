namespace Logger {

    internal interface IAppLogger {
        void Log(LogLevel level, string message);
    }

    internal abstract class LoggerBase {
        protected string _logFileName;
        internal LoggerBase(string logFileName) {
            _logFileName = logFileName;
        }
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

            LogLevels[(int)LogLevel.DEBUG] = string.Intern("DEBUG");
            LogLevels[(int)LogLevel.INFO] = string.Intern("INFO");
            LogLevels[(int)LogLevel.WARN] = string.Intern("WARN");
            LogLevels[(int)LogLevel.ERROR] = string.Intern("ERROR");
        }

        internal static string GetString(LogLevel level) => LogLevels[(int)level];
    }
}
