namespace Logger.Utils {
    /// <summary>
    /// A class for safely acquiring and releasing a mutex with using support.
    /// </summary>
    internal class MutexLock : IDisposable {
        private readonly Mutex _mutex;
        private bool _acquired;
        private bool _disposed;

        public bool IsAcquired => _acquired;

        public MutexLock(Mutex mutex, TimeSpan timeout) {
            if (mutex == null) throw new ArgumentNullException(nameof(mutex));
            _mutex = mutex;

            try {
                _acquired = _mutex.WaitOne(timeout);
            } catch (AbandonedMutexException ex) {
                Console.WriteLine($"[Meta-Log-ERROR {DateTime.Now:yyyy-MM-dd HH:mm:ss}] Mutex was abandoned: {ex.Message}. Recovering and proceeding.");
                _acquired = true;
            }
        }

        public void Dispose() {
            if (!_disposed && _acquired) {
                try {
                    _mutex.ReleaseMutex();
                } catch (Exception ex) {
                    Console.WriteLine($"[Meta-Log-ERROR {DateTime.Now:yyyy-MM-dd HH:mm:ss}] Failed to release mutex: {ex.Message}");
                }
                _acquired = false;
                _disposed = true;
            }
        }
    }
}
