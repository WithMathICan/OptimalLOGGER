namespace LoggerWithInternalLogger.Logger {
    internal class ConsoleLogger : LoggerBase {
        public override void Log(LogLevel level, string message) {
            Console.WriteLine(FormatMessage(level, message));
        }
    }
}
