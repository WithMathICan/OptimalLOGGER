using Logger.Logger;

namespace Logger.Services {
    /// <summary>
    /// Service representing a standard load operation
    /// </summary>
    internal class LoadService : IService {
        IAppLogger _logger;

        /// <summary>
        /// Initializes a new instance of LoadService
        /// </summary>
        /// <param name="logger">Logger instance for service operations</param>
        public LoadService(IAppLogger logger) {
            _logger = logger;
            _logger.Log(LogLevel.DEBUG, "LoadService initialized");
        }

        /// <summary>
        /// Starts the load service operation
        /// </summary>
        public void Start() {
            _logger.Log(LogLevel.INFO, "LoadService started");
        }

        /// <summary>
        /// Stops the load service operation
        /// </summary>
        public void Stop() {
            _logger.Log(LogLevel.WARN, "LoadService stoped");
        }
    }
}
