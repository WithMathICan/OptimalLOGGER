namespace LoggerWithInternalLogger.Logger {
    /// <summary>
    /// Represents the available log levels.
    /// </summary>
    internal enum LogLevel {
        /// <summary>
        /// Debug level for detailed diagnostic information.
        /// </summary>
        DEBUG,
        /// <summary>
        /// Info level for informational messages.
        /// </summary>
        INFO,
        /// <summary>
        /// Warn level for warning messages.
        /// </summary>
        WARN,
        /// <summary>
        /// Error level for error messages.
        /// </summary>
        ERROR
    }
    /// <summary>
    /// Provides string representations for <see cref="LogLevel"/> values.
    /// </summary>
    internal static class LogLevelFactory {
        /// <summary>
        /// Array of string representations for each <see cref="LogLevel"/>.
        /// </summary>
        private static readonly string[] LogLevels;

        /// <summary>
        /// Initializes the <see cref="LogLevelFactory"/> class and sets up the string representations for log levels.
        /// </summary>
        static LogLevelFactory() {
            LogLevels = new string[Enum.GetNames(typeof(LogLevel)).Length];

            LogLevels[(int)LogLevel.DEBUG] = "DEBG";
            LogLevels[(int)LogLevel.INFO] = "INFO";
            LogLevels[(int)LogLevel.WARN] = "WARN";
            LogLevels[(int)LogLevel.ERROR] = "ERRR";
        }

        /// <summary>
        /// Gets the string representation for the specified <see cref="LogLevel"/>.
        /// </summary>
        /// <param name="level">The log level.</param>
        /// <returns>The string representation of the log level.</returns>
        internal static string GetString(LogLevel level) => LogLevels[(int)level];
    }
}
