using LoggerWithInternalLogger.Logger;

namespace LoggerWithInternalLogger.Utils {
    /// <summary>
    /// A class for safely acquiring and releasing a mutex with using support.
    /// </summary>
    internal class MutexLock : IDisposable {
        private readonly Mutex _mutex;
        private bool _acquired;
        private bool _disposed;

        public bool IsAcquired => _acquired;

        public MutexLock(Mutex mutex, TimeSpan timeout) {
            ArgumentNullException.ThrowIfNull(mutex);
            _mutex = mutex;
            try {
                _acquired = _mutex.WaitOne(timeout);
            } catch (AbandonedMutexException ex) {
                Application.Log(LogLevel.ERROR, $"Mutex was abandoned: {ex.Message}. Recovering and proceeding.");
                _acquired = true;
            }
        }

        public void Dispose() {
            if (!_disposed && _acquired) {
                try {
                    _mutex.ReleaseMutex();
                } catch (Exception ex) {
                    Application.Log(LogLevel.ERROR, $"Failed to release mutex: {ex.Message}");
                }
                _acquired = false;
                _disposed = true;
            }
        }
    }
}
