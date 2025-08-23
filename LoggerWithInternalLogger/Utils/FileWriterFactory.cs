using System.Collections.Concurrent;

namespace LoggerWithInternalLogger.Utils {
    /// <summary>
    /// Provides a factory for creating and managing <see cref="IFileWriter"/> instances for different file paths.
    /// Ensures that only one writer exists per normalized file path.
    /// </summary>
    internal class FileWriterFactory : IFileWriterFactory {
        private readonly ConcurrentDictionary<string, FileWriter> _writers = new();
        private bool _disposed;

        /// <summary>
        /// Gets an <see cref="IFileWriter"/> for the specified file path.
        /// If a writer for the normalized path already exists, it is returned; otherwise, a new one is created.
        /// </summary>
        /// <param name="filePath">The path of the file to write to.</param>
        /// <returns>An <see cref="IFileWriter"/> instance for the specified file path.</returns>
        /// <exception cref="ObjectDisposedException">Thrown if the factory has been disposed.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="filePath"/> is null or whitespace.</exception>
        public IFileWriter GetWriter(string filePath) {
            ObjectDisposedException.ThrowIf(_disposed, typeof(FileWriterFactory));
            ArgumentException.ThrowIfNullOrWhiteSpace(filePath, nameof(filePath));
            // Normalize path to avoid duplicates (e.g., "log.txt" vs "./log.txt")
            string normalizedPath = Path.GetFullPath(filePath);
            return _writers.GetOrAdd(normalizedPath, _ => new FileWriter(normalizedPath));
        }

        /// <summary>
        /// Disposes the factory and all managed <see cref="IFileWriter"/> instances.
        /// </summary>
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
