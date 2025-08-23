using LoggerWithInternalLogger.Logger;

namespace LoggerWithInternalLogger.Services {
    internal class LoadServiceWithError : IService {
        private readonly ILogger _logger;
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
}