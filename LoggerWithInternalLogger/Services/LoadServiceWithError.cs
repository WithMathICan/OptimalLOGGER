using LoggerWithInternalLogger.Logger;

namespace LoggerWithInternalLogger.Services {
    internal class LoadServiceWithError : IService {
        public LoadServiceWithError() {
            Application.Log(LogLevel.DEBUG, "LoadServiceWithError initialized");
        }
        public void Start() {
            Application.Log(LogLevel.ERROR, "Error during LoadServiceWithError starting");
        }
        public void Stop() {
            Application.Log(LogLevel.WARN, "LoadServiceWithError stoped");
        }
    }
}