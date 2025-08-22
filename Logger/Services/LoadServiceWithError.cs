using Logger.Logger;

namespace Logger.Services {

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
