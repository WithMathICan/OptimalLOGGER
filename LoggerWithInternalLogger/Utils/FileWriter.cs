using LoggerWithInternalLogger.Logger;
using System.Text;
using System.Threading;

namespace LoggerWithInternalLogger.Utils {
    internal class FileWriter(string filePath, ILogger logger) : FileWriterBase(filePath, logger) {

        /// <summary>
        /// Writes the contents of the provided <see cref="StringBuilder"/> to the file in a thread-safe manner.
        /// Acquires a mutex before writing to ensure exclusive access.
        /// Logs an error if the mutex cannot be acquired or if an exception occurs during writing.
        /// </summary>
        /// <param name="sb">The <see cref="StringBuilder"/> containing the text to write to the file.</param>
        public override void Write(StringBuilder sb) {
            using var mutexLock = new MutexLock(_mutex, TimeSpan.FromSeconds(5), _internalLogger);
            if (!mutexLock.IsAcquired) {
                InternalLog(LogLevel.ERROR, $"Unable to acquire mutex to write to {FilePath}.");
                return;
            }
            try {
                using var stream = new FileStream(FilePath, FileMode.Append);
                using var streamWriter = new StreamWriter(stream, Encoding.UTF8);
                streamWriter.Write(sb);
            } catch (Exception ex) {
               InternalLog(LogLevel.ERROR, $"Logger failed: {ex}");
            }
        }

        /// <summary>
        /// Appends a line of text to the file in a thread-safe manner.
        /// Acquires a mutex before writing to ensure exclusive access.
        /// Logs an error if the mutex cannot be acquired or if an exception occurs during writing.
        /// </summary>
        /// <param name="text">The text to append as a new line to the file.</param>
        public override void AppendLine(string text) {
            using var mutexLock = new MutexLock(_mutex, TimeSpan.FromSeconds(5), _internalLogger);
            if (!mutexLock.IsAcquired) {
                InternalLog(LogLevel.ERROR, $"Unable to acquire mutex to write to {FilePath}. Using fallback.");
                return;
            }
            try {
                File.AppendAllText(FilePath, text + Environment.NewLine);
            } catch (Exception ex) {
                InternalLog(LogLevel.ERROR, $"Unhandled error when saving log to file: {ex.Message}.");
            }
        }
    }
}
