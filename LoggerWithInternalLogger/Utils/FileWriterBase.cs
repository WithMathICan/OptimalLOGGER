using LoggerWithInternalLogger.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerWithInternalLogger.Utils {

    /// <summary>
    /// Provides a base implementation for file writers with mutex-based synchronization.
    /// </summary>
    internal abstract class FileWriterBase : IFileWriter {
        /// <summary>
        /// The file path associated with this writer.
        /// </summary>
        private readonly string _filePath;

        /// <summary>
        /// The mutex used for synchronizing file access.
        /// </summary>
        protected readonly Mutex _mutex;

        /// <summary>
        /// Gets the file path associated with this writer.
        /// </summary>
        protected virtual string FilePath => _filePath;

        protected ILogger _internalLogger;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileWriterBase"/> class.
        /// </summary>
        /// <param name="filePath">The file path to write to.</param>
        public FileWriterBase(string filePath, ILogger logger) {
            _filePath = filePath;
            _mutex = new Mutex(false, $"Global\\{SanitizeMutexName(_filePath)}");
            _internalLogger = logger;
        }

        protected void InternalLog(LogLevel level, string message) {
            _internalLogger.Log(level, message);
        }

        /// <summary>
        /// Sanitizes the file path to create a valid mutex name.
        /// </summary>
        /// <param name="filePath">The file path to sanitize.</param>
        /// <returns>A sanitized string suitable for use as a mutex name.</returns>
        private static string SanitizeMutexName(string filePath) {
            return Path.GetFullPath(filePath).Replace("\\", "_").Replace(":", "_").Replace("/", "_");
        }

        /// <summary>
        /// Adds a postfix to the file name before the extension.
        /// </summary>
        /// <param name="originalPath">The original file path.</param>
        /// <param name="postfix">The postfix to add to the file name.</param>
        /// <returns>The new file path with the postfix added to the file name.</returns>
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

        /// <summary>
        /// Releases all resources used by the <see cref="FileWriterBase"/>.
        /// </summary>
        public virtual void Dispose() {
            _mutex?.Dispose();
        }

        /// <summary>
        /// Writes the contents of the specified <see cref="StringBuilder"/> to the file.
        /// </summary>
        /// <param name="sb">The <see cref="StringBuilder"/> containing the text to write.</param>
        public abstract void Write(StringBuilder sb);

        /// <summary>
        /// Appends a line of text to the file.
        /// </summary>
        /// <param name="text">The text to append as a line.</param>
        public abstract void AppendLine(string text);
    }
}
