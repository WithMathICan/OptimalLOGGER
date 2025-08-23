using LoggerWithInternalLogger.Logger;
using LoggerWithInternalLogger.Services;
using LoggerWithInternalLogger.Utils;

namespace LoggerWithInternalLogger {
    internal class Application {
        private readonly List<IService> _services;
        private readonly ILogger _appLogger;
        private static readonly FileWriterFactory _fileWriterFactory;
        private static readonly ConsoleLogger _internalLogger = new();

        internal static void Log(LogLevel level, string text) {
            _internalLogger.Log(level, text);
        }

        private static void RunTasks(IEnumerable<Action> actions) {
            var tasks = actions.Select(a => Task.Run(a));
            Task.WaitAll([.. tasks]);
        }

        static Application() {
            _fileWriterFactory = new FileWriterFactory();
            AppDomain.CurrentDomain.ProcessExit += (sender, args) => _fileWriterFactory.Dispose();
        }

        public Application() {
            _appLogger = new FileLoggerWithBuffer(5, _fileWriterFactory.GetWriter("app.log"));
            _services = [
                new HighLoadService(_appLogger),
                new LoadService(_appLogger),
                new LoadServiceWithError(_appLogger),
            ];
        }

        internal void Start() {
            RunTasks(_services.Select<IService, Action>(s => s.Start));
        }

        internal void Shutdown() {
            RunTasks(_services.Select<IService, Action>(s => s.Stop));
            _appLogger.Dispose();
            _fileWriterFactory.Dispose();
            _internalLogger.Dispose();
        }
    }
}