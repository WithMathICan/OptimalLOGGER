namespace LoggerWithInternalLogger.Logger {
    /// <summary>
    /// A class for safely acquiring and releasing a mutex with using support.
    /// </summary>
    internal class MutexLock : IDisposable {
        private readonly Mutex _mutex;
        private bool _acquired;
        private bool _disposed;
        private readonly ILogger _logger;

        public bool IsAcquired => _acquired;

        public MutexLock(Mutex mutex, TimeSpan timeout, ILogger logger) {
            if (mutex == null) throw new ArgumentNullException(nameof(mutex));
            _mutex = mutex;
            _logger = logger;

            try {
                _acquired = _mutex.WaitOne(timeout);
            } catch (AbandonedMutexException ex) {
                _logger.Log(LogLevel.ERROR, $"Mutex was abandoned: {ex.Message}. Recovering and proceeding.");
                _acquired = true;
            }
        }

        public void Dispose() {
            if (!_disposed && _acquired) {
                try {
                    _mutex.ReleaseMutex();
                } catch (Exception ex) {
                    _logger.Log(LogLevel.ERROR, $"Failed to release mutex: {ex.Message}");
                }
                _acquired = false;
                _disposed = true;
            }
        }
    }
}
