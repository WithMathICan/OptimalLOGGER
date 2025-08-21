namespace Logger {
    internal class LoggerSimple : LoggerBase, IAppLogger {
        internal LoggerSimple(string logFileName) : base(logFileName) { }
        private Mutex _logFileMutex = new Mutex(false, "Global\\AppLogger");

        public void Log(LogLevel level, string message) {
            string logMessage = string.Concat(LogLevelFactory.GetString(level), ":\t ", DateTime.Now, "\t ", message);
            _logFileMutex.WaitOne();
            try {
                File.AppendAllLines(_logFileName, [logMessage]);
            } finally { _logFileMutex.ReleaseMutex(); }
            Console.WriteLine(logMessage);
        }
    }
}
