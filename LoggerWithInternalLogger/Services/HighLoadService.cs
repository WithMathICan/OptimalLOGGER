using LoggerWithInternalLogger.Logger;

namespace LoggerWithInternalLogger.Services {
    internal class HighLoadService : IService {
        public HighLoadService() {
            Application.Log(LogLevel.DEBUG, "HighLoadService initialized");
        }
        public void Start() {
            Application.Log(LogLevel.INFO, "HighLoadService started");
        }
        public void Stop() {
            Application.Log(LogLevel.WARN, "HighLoadService stoped");
        }
    }
}