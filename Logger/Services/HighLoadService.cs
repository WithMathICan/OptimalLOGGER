using Logger.Logger;

namespace Logger.Services {
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
}
