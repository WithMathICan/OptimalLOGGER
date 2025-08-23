using LoggerWithInternalLogger.Logger;

namespace LoggerWithInternalLogger.Services {
    internal class HighLoadService : IService {
        private readonly ILogger _logger;
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
}