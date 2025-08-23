using System.Collections.Concurrent;
using System.Text;

namespace LoggerWithInternalLogger.Logger {
    internal class FileLoggerWithBuffer : ILogger {
        private readonly string _filePath;
        private readonly Mutex _mutex;
        private readonly ILogger _internalLogger;
        private readonly int _batchSize;
        private readonly AutoResetEvent _signal;
        private readonly Thread _worker;
        private readonly ConcurrentQueue<string> _queue = new();
        private readonly CancellationTokenSource _cts = new();

        public FileLoggerWithBuffer(string filePath, ILogger reservedLogger, int batchSize) {
            _filePath = filePath;
            _internalLogger = reservedLogger;
            _mutex = new Mutex(false, $"Global\\{SanitizeMutexName(_filePath)}");
            _batchSize = batchSize;
            _signal = new AutoResetEvent(false);
            _worker = new Thread(WorkerLoop) { IsBackground = true };
            _worker.Start();
        }

        private void WorkerLoop() {
            while (!_cts.IsCancellationRequested) {
                _signal.WaitOne(TimeSpan.FromSeconds(5));
                Flush();
            }
        }

        private void Flush() {
            if (_queue.IsEmpty) return;
            var sb = new StringBuilder();
            int count = 0;
            while (_queue.TryDequeue(out var log)) {
                sb.Append(log);
                count++;
                if (count >= _batchSize) break;
            }
            if (count == 0) return;

            using var mutexLock = new MutexLock(_mutex, TimeSpan.FromSeconds(5), _internalLogger);
            if (!mutexLock.IsAcquired) {
                _internalLogger.Log(LogLevel.ERROR, $"Unable to acquire mutex to write to {_filePath}.");
                return;
            }
            try {
                using var stream = new FileStream(_filePath, FileMode.Append);
                using var streamWriter = new StreamWriter(stream, Encoding.UTF8);
                streamWriter.Write(sb);
            } catch (Exception ex) {
                _internalLogger.Log(LogLevel.ERROR, $"Logger failed: {ex}");
            }
        }

        private static string SanitizeMutexName(string filePath) {
            return Path.GetFullPath(filePath).Replace("\\", "_").Replace(":", "_").Replace("/", "_");
        }

        private static string FormatMessage(LogLevel level, string message) {
            string logLevel = LogLevelFactory.GetString(level);
            string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            return string.Concat(logLevel, "   ", date, "   ", message, Environment.NewLine);
        }

        public void Log(LogLevel level, string message) {
            string logMessage = FormatMessage(level, message);
            _queue.Enqueue(logMessage);
            if (_queue.Count >= _batchSize) _signal.Set();
        }

        public void Dispose() {
            _cts.Cancel();
            _signal.Set();
            _worker.Join();
            Flush();

            _internalLogger?.Dispose();
            _mutex?.Dispose();
        }
    }
}
