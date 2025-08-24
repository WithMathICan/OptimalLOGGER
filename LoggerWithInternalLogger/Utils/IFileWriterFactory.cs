using LoggerWithInternalLogger.Logger;

namespace LoggerWithInternalLogger.Utils {
    internal interface IFileWriterFactory : IDisposable {
        IFileWriter GetWriter(string filePath);
    }
}