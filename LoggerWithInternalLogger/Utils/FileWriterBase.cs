using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerWithInternalLogger.Utils {

    internal abstract class FileWriterBase : IFileWriter {
        private readonly string _filePath;
        protected readonly Mutex _mutex;
        protected virtual string FilePath => _filePath;

        public FileWriterBase(string filePath) {
            _filePath = filePath;
            _mutex = new Mutex(false, $"Global\\{SanitizeMutexName(_filePath)}");
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

        public abstract void Write(StringBuilder sb);

        public abstract void AppendLine(string text);
    }
}
