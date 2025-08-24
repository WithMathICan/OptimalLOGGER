using LoggerWithInternalLogger.Logger;
using LoggerWithInternalLogger.Services;
using LoggerWithInternalLogger.Utils;

namespace LoggerWithInternalLogger {
    internal class Application {
        private readonly List<IService> _services;
        private static readonly ILogger _appLogger;
        private static readonly FileWriterFactory _fileWriterFactory;
        private static readonly ConsoleLogger _internalLogger;

        internal static void Log(LogLevel level, string text) {
            _appLogger.Log(level, text);
        }

        private static void RunTasks(IEnumerable<Action> actions) {
            var tasks = actions.Select(a => Task.Run(a));
            Task.WaitAll([.. tasks]);
        }

        static Application() {
            _internalLogger = new();
            _fileWriterFactory = new FileWriterFactory(_internalLogger);
            _appLogger = new FileLoggerWithBuffer(5, _fileWriterFactory.GetWriter("app.log"));
            AppDomain.CurrentDomain.ProcessExit += (sender, args) => _appLogger.Dispose();
            AppDomain.CurrentDomain.ProcessExit += (sender, args) => _fileWriterFactory.Dispose();
            AppDomain.CurrentDomain.ProcessExit += (sender, args) => _internalLogger.Dispose();
        }

        public Application() {
            
            _services = [
                new HighLoadService(),
                new LoadService(),
                new LoadServiceWithError(),
            ];
        }

        internal void Start() {
            RunTasks(_services.Select<IService, Action>(s => s.Start));
        }

        internal void Shutdown() {
            RunTasks(_services.Select<IService, Action>(s => s.Stop));
        }
    }
}