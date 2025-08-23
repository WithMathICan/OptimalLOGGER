using System.Collections.Concurrent;

namespace LoggerWithInternalLogger.Utils {
    internal class FileWriterFactory : IFileWriterFactory {
        private readonly ConcurrentDictionary<string, FileWriter> _writers = new();
        private bool _disposed;

        public IFileWriter GetWriter(string filePath) {
            ObjectDisposedException.ThrowIf(_disposed, typeof(FileWriterFactory));
            ArgumentException.ThrowIfNullOrWhiteSpace(filePath, nameof(filePath));
            // Normalize path to avoid duplicates (e.g., "log.txt" vs "./log.txt")
            string normalizedPath = Path.GetFullPath(filePath);
            return _writers.GetOrAdd(normalizedPath, _ => new FileWriter(normalizedPath));
        }

        public void Dispose() {
            if (!_disposed) {
                foreach (var writer in _writers.Values) {
                    writer.Dispose();
                }
                _writers.Clear();
                _disposed = true;
            }
        }
    }
}
