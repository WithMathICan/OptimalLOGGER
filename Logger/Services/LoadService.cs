using Logger.Logger;

namespace Logger.Services {
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
}
