using LoggerWithInternalLogger.Logger;

namespace LoggerWithInternalLogger.Services {
    internal class LoadService : IService {
        public LoadService() {
            Application.Log(LogLevel.DEBUG, "LoadService initialized");
        }
        public void Start() {
            Application.Log(LogLevel.INFO, "LoadService started");
        }
        public void Stop() {
            Application.Log(LogLevel.WARN, "LoadService stoped");
        }
    }
}