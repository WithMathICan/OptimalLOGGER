using LoggerWithInternalLogger.Utils;
using System.Collections.Concurrent;
using System.Text;

namespace LoggerWithInternalLogger.Logger {
    /// <summary>
    /// Logger implementation that buffers log messages and writes them to a file in batches.
    /// </summary>
    internal class FileLoggerWithBuffer : LoggerBase {
        private readonly IFileWriter _fileWriter;
        private readonly int _batchSize;
        private readonly AutoResetEvent _signal;
        private readonly Thread _worker;
        private readonly ConcurrentQueue<string> _queue = new();
        private readonly CancellationTokenSource _cts = new();
        private static readonly ThreadLocal<StringBuilder> _stringBuilder = new(() => new StringBuilder(256), trackAllValues: false);

        /// <summary>
        /// Initializes a new instance of the <see cref="FileLoggerWithBuffer"/> class.
        /// </summary>
        /// <param name="batchSize">The number of log messages to buffer before writing to file.</param>
        /// <param name="fileWriter">The file writer used to write log messages.</param>
        public FileLoggerWithBuffer(int batchSize, IFileWriter fileWriter) {
            _fileWriter = fileWriter;
            _batchSize = batchSize;
            _signal = new AutoResetEvent(false);
            _worker = new Thread(WorkerLoop) { IsBackground = true };
            _worker.Start();
        }

        /// <summary>
        /// The background worker loop that processes the log queue and writes batches to file.
        /// </summary>
        private void WorkerLoop() {
            while (!_cts.IsCancellationRequested) {
                _signal.WaitOne(TimeSpan.FromSeconds(5));
                while (_queue.Count > _batchSize) Flush();
            }
        }

        /// <summary>
        /// Flushes a batch of log messages from the queue to the file.
        /// </summary>
        private void Flush() {
            if (_queue.IsEmpty) return;
            var sb = _stringBuilder.Value ?? new StringBuilder(256);
            sb.Clear();
            int count = 0;
            while (_queue.TryDequeue(out var log)) {
                sb.AppendLine(log);
                count++;
                if (count >= _batchSize) break;
            }
            if (count != 0) _fileWriter.Write(sb);
        }

        /// <summary>
        /// Enqueues a log message and signals the worker if the batch size is exceeded.
        /// </summary>
        /// <param name="level">The log level.</param>
        /// <param name="message">The log message.</param>
        public override void Log(LogLevel level, string message) {
            string logMessage = FormatMessage(level, message);
            _queue.Enqueue(logMessage);
            if (_queue.Count > _batchSize) _signal.Set();
        }

        /// <summary>
        /// Disposes the logger, ensuring all buffered messages are written to file.
        /// </summary>
        public override void Dispose() {
            _cts.Cancel();
            _signal.Set();
            _worker.Join();
            Flush();
        }
    }
}
