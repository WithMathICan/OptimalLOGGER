using LoggerWithInternalLogger.Logger;

namespace LoggerWithInternalLogger.Services {
    internal class LoadService : IService {
        private readonly ILogger _logger;
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
}