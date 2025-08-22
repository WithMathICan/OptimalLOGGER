using LoggerWithInternalLogger.Logger;

namespace LoggerWithInternalLogger {
    internal class Program {
        private static void Main(string[] args) {
            Application app = new();
            app.Start();
            Thread.Sleep(500);
            app.Shutdown();
        }
    }

    internal interface IService {
        void Start();
        void Stop();
    }

    internal class HighLoadService : IService {
        ILogger _logger;
        public HighLoadService(ILogger logger) {
            _logger = logger;
            _logger.Log(LogLevel.DEBUG, "HighLoadService initialized");
        }

        public void Start() {
            _logger.Log(LogLevel.INFO, "HighLoadService started");
        }

        public void Stop() {
            _logger.Log(LogLevel.WARN, "HighLoadService stoped");
        }
    }

    internal class LoadService : IService {
        ILogger _logger;
        public LoadService(ILogger logger) {
            _logger = logger;
            _logger.Log(LogLevel.DEBUG, "LoadService initialized");
        }

        public void Start() {
            _logger.Log(LogLevel.INFO, "LoadService started");
        }

        public void Stop() {
            _logger.Log(LogLevel.WARN, "LoadService stoped");
        }
    }

    internal class LoadServiceWithError : IService {
        ILogger _logger;
        public LoadServiceWithError(ILogger logger) {
            _logger = logger;
            _logger.Log(LogLevel.DEBUG, "LoadServiceWithError initialized");
        }

        public void Start() {
            _logger.Log(LogLevel.ERROR, "Error during LoadServiceWithError starting");
        }

        public void Stop() {
            _logger.Log(LogLevel.WARN, "LoadServiceWithError stoped");
        }
    }

    internal class Application {
        private readonly List<IService> _services;
        private readonly ILogger _appLogger;
        private readonly ILogger _reserveLogger;
        private static void RunTasks(IEnumerable<Action> actions) {
            var tasks = actions.Select(a => Task.Run(a));
            Task.WaitAll([.. tasks]);
        }

        public Application() {
            _reserveLogger = new ConsoleLogger();
            _appLogger = new FileLogger("app.log", _reserveLogger);
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
            _reserveLogger.Dispose();
            _appLogger.Dispose();
        }
    }
}