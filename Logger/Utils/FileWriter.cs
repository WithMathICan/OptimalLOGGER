using Logger.Logger;

namespace Logger.Utils {
    internal abstract class FileWriterBase : IDisposable {
        protected string _filePath;
        protected Mutex _mutex;
        protected readonly string _reserveFilePath;
        protected readonly int _maxRetries;
        protected readonly TimeSpan _retryDelay;

        internal FileWriterBase(string logFileName, int maxRetries) {
            _filePath = logFileName;
            _mutex = new Mutex(false, $"Global\\{SanitizeMutexName(_filePath)}");
            _reserveFilePath = AddPostfixToTheFileName(logFileName, "---reserved");
            _maxRetries = maxRetries > 1 ? maxRetries : 2;
            _retryDelay = TimeSpan.FromMilliseconds(500);
        }
        private static string SanitizeMutexName(string filePath) {
            return Path.GetFullPath(filePath).Replace("\\", "_").Replace(":", "_").Replace("/", "_");
        }
        protected static string AddPostfixToTheFileName(string originalPath, string postfix) {
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(originalPath);
            string extension = Path.GetExtension(originalPath);
            string? directory = Path.GetDirectoryName(originalPath);

            string newFileName = $"{fileNameWithoutExtension}{postfix}{extension}";

            if (directory == null) {
                return newFileName;
            } else {
                return Path.Combine(directory, newFileName);
            }
        }

        public virtual void Dispose() {
            _mutex?.Dispose();
        }
    }

    internal interface IFileWriter {
        void AppendText(string text);
        Task AppendTextWithRetryAsync(string text);
    }

    internal class SimpleFileWriter(string logFileName, int maxRetries) : FileWriterBase(logFileName, maxRetries), IFileWriter {
        public void AppendText(string text) {
            Console.WriteLine(text);
            _mutex.WaitOne();
            try {
                File.AppendAllText(_filePath, text);
            } catch (IOException) {
                WriteToFallback(text);
            } catch (Exception ex) {
                WriteToFallback(text);
                Console.WriteLine($"[Meta-Log-ERROR] Unhandled error: {ex.Message}. Use fallback.");
                throw;
            } finally { _mutex.ReleaseMutex(); }
        }

        public async Task AppendTextWithRetryAsync(string text) {
            if (string.IsNullOrWhiteSpace(text)) throw new ArgumentException("Empty text should not be added.", nameof(text));
            using var mutexLock = new MutexLock(_mutex, TimeSpan.FromSeconds(5));
            if (!mutexLock.IsAcquired) {
                Console.WriteLine($"[Meta-Log] Unable to acquire mutex to write to {_filePath}. Using fallback.");
                await WriteToFallbackAsync(text);
                return;
            }

            bool success = false;
            for (int attempt = 1; attempt <= _maxRetries; ++attempt) {
                try {
                    await File.AppendAllTextAsync(_filePath, text);
                    success = true;
                    break;
                } catch (IOException ex) {
                    Console.WriteLine($"[Meta-Log-ERROR] Error write to {_filePath} (retry {attempt}/{_maxRetries}): {ex.Message}");
                    if (attempt == _maxRetries) {
                        Console.WriteLine($"[Meta-Log-ERROR] Retry max acceded. Use fallback.");
                        await WriteToFallbackAsync(text);
                    } else {
                        await Task.Delay(_retryDelay); // Backoff
                    }
                } catch (Exception ex) {
                    Console.WriteLine($"[Meta-Log-ERROR] Unhandled error: {ex.Message}. Use fallback.");
                    await WriteToFallbackAsync(text);
                }
            }
            if (success) {
                Console.WriteLine($"[Meta-Log] Success in append tex to {_filePath}.");
            }
        }

        private async Task WriteToFallbackAsync(string text) {
            try {
                await File.AppendAllTextAsync(_reserveFilePath, text);
                Console.WriteLine($"[Meta-Log-INFO] Text appended to the reserve file {_reserveFilePath}.");
            } catch (Exception ex) {
                Console.WriteLine($"[Meta-Log-ERROR] Error even in writing to fallback: {ex.Message}. Lost text: {text}");
            }
        }

        private void WriteToFallback(string text) {
            try {
                File.AppendAllText(_reserveFilePath, text);
                Console.WriteLine($"[Meta-Log-INFO] Text appended to the reserve file {_reserveFilePath}.");
            } catch (Exception ex) {
                Console.WriteLine($"[Meta-Log-ERROR] Error even in writing to fallback: {ex.Message}. Lost text: {text}");
            }
        }
    }
}
