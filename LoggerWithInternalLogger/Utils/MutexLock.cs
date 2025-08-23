using LoggerWithInternalLogger.Logger;

namespace LoggerWithInternalLogger.Utils {
    /// <summary>
    /// A class for safely acquiring and releasing a mutex with using support.
    /// </summary>
    internal class MutexLock : IDisposable {
        private readonly Mutex _mutex;
        private bool _acquired;
        private bool _disposed;

        /// <summary>
        /// Gets a value indicating whether the mutex was successfully acquired.
        /// </summary>
        public bool IsAcquired => _acquired;

        /// <summary>
        /// Initializes a new instance of the <see cref="MutexLock"/> class and attempts to acquire the specified mutex within the given timeout.
        /// </summary>
        /// <param name="mutex">The <see cref="Mutex"/> to acquire.</param>
        /// <param name="timeout">The maximum amount of time to wait for the mutex.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="mutex"/> is null.</exception>
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

        /// <summary>
        /// Releases the mutex if it was acquired and disposes of the <see cref="MutexLock"/> instance.
        /// </summary>
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
