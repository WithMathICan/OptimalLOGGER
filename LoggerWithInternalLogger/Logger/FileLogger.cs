namespace LoggerWithInternalLogger.Logger {
    internal class FileLogger : ILogger {
        private readonly string _filePath;
        private readonly Mutex _mutex;
        private readonly ILogger _internalLogger;

        public FileLogger(string filePath, ILogger reservedLogger) {
            _filePath = filePath;
            _internalLogger = reservedLogger;
            _mutex = new Mutex(false, $"Global\\{SanitizeMutexName(_filePath)}");
        }

        private static string SanitizeMutexName(string filePath) {
            return Path.GetFullPath(filePath).Replace("\\", "_").Replace(":", "_").Replace("/", "_");
        }

        private static string FormatMessage(LogLevel level, string message) {
            string logLevel = LogLevelFactory.GetString(level);
            string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            return string.Concat(logLevel, "   ", date, "   ", message, Environment.NewLine);
        }

        public void Log(LogLevel level, string message) {
            string logMessage = FormatMessage(level, message);
            using var mutexLock = new MutexLock(_mutex, TimeSpan.FromSeconds(3), _internalLogger);
            if (!mutexLock.IsAcquired) {
                _internalLogger.Log(LogLevel.ERROR, $"Unable to acquire mutex to write to {_filePath}. Using fallback.");
                return;
            }
            try {
                File.AppendAllText(_filePath, logMessage);
            } catch(Exception ex) {
                _internalLogger.Log(LogLevel.ERROR, $"Unhandled error when saving log to file: {ex.Message}.");
            }
        }

        public void Dispose() {
            _mutex?.Dispose();
        }
    }
}
