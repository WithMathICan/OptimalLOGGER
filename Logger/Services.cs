namespace Logger {
    internal interface IService {
        void Start();
        void Stop();
    }
    internal class HighLoadService : IService {
        IAppLogger _logger;
        public HighLoadService(IAppLogger logger) {
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
        IAppLogger _logger;
        public LoadService(IAppLogger logger) {
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
        IAppLogger _logger;
        public LoadServiceWithError(IAppLogger logger) {
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
}
