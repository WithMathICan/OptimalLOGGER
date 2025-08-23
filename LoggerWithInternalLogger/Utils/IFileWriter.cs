using System.Text;

namespace LoggerWithInternalLogger.Utils {
    internal interface IFileWriter : IDisposable {
        void Write(StringBuilder sb);
        void AppendLine(string text);
    }
}
