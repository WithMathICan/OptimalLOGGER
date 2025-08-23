using LoggerWithInternalLogger.Utils;
using System.Collections.Concurrent;
using System.Text;

namespace LoggerWithInternalLogger.Logger {
    internal class FileLoggerWithBuffer : LoggerBase {
        private readonly IFileWriter _fileWriter;
        private readonly int _batchSize;
        private readonly AutoResetEvent _signal;
        private readonly Thread _worker;
        private readonly ConcurrentQueue<string> _queue = new();
        private readonly CancellationTokenSource _cts = new();

        public FileLoggerWithBuffer(int batchSize, IFileWriter fileWriter) {
            _fileWriter = fileWriter;
            _batchSize = batchSize;
            _signal = new AutoResetEvent(false);
            _worker = new Thread(WorkerLoop) { IsBackground = true };
            _worker.Start();
        }

        private void WorkerLoop() {
            while (!_cts.IsCancellationRequested) {
                _signal.WaitOne(TimeSpan.FromSeconds(5));
                while(_queue.Count > _batchSize) Flush();
            }
        }

        private void Flush() {
            if (_queue.IsEmpty) return;
            var sb = new StringBuilder();
            int count = 0;
            while (_queue.TryDequeue(out var log)) {
                sb.AppendLine(log);
                count++;
                if (count >= _batchSize) break;
            }
            if (count != 0) _fileWriter.Write(sb);
        }

        public override void Log(LogLevel level, string message) {
            string logMessage = FormatMessage(level, message);
            _queue.Enqueue(logMessage);
            if (_queue.Count > _batchSize) _signal.Set();
        }

        public override void Dispose() {
            _cts.Cancel();
            _signal.Set();
            _worker.Join();
            Flush();
        }
    }
}
