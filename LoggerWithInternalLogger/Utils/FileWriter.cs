using LoggerWithInternalLogger.Logger;
using System.Text;
using System.Threading;

namespace LoggerWithInternalLogger.Utils {
    internal class FileWriter : FileWriterBase {
        public FileWriter(string filePath) : base(filePath) {}

        public override void Write(StringBuilder sb) {
            using var mutexLock = new MutexLock(_mutex, TimeSpan.FromSeconds(5));
            if (!mutexLock.IsAcquired) {
                Application.Log(LogLevel.ERROR, $"Unable to acquire mutex to write to {FilePath}.");
                return;
            }
            try {
                using var stream = new FileStream(FilePath, FileMode.Append);
                using var streamWriter = new StreamWriter(stream, Encoding.UTF8);
                streamWriter.Write(sb);
            } catch (Exception ex) {
                Application.Log(LogLevel.ERROR, $"Logger failed: {ex}");
            }
        }

        public override void AppendLine(string text) {
            using var mutexLock = new MutexLock(_mutex, TimeSpan.FromSeconds(5));
            if (!mutexLock.IsAcquired) {
                Application.Log(LogLevel.ERROR, $"Unable to acquire mutex to write to {FilePath}. Using fallback.");
                return;
            }
            try {
                File.AppendAllText(FilePath, text + Environment.NewLine);
            } catch (Exception ex) {
                Application.Log(LogLevel.ERROR, $"Unhandled error when saving log to file: {ex.Message}.");
            }
        }
    }
}
