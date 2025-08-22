namespace Logger.Utils {
    internal abstract class FileWriterBase {
        protected string _logFileName;
        protected Mutex _fileMutex;
        internal FileWriterBase(string logFileName) {
            _logFileName = logFileName;
            _fileMutex = new Mutex(false, $"/Global/{_logFileName}");
        }
    }

    internal interface IFileWriter {
        void AppendLine(string line);
    }

    internal class SimpleFileWriter(string logFileName) : FileWriterBase(logFileName), IFileWriter {
        public void AppendLine(string line) {
            Console.WriteLine(line);
            _fileMutex.WaitOne();
            try {
                File.AppendAllLines(_logFileName, [line]);
            } catch (IOException) {
                File.AppendAllLines($"reserved-{_logFileName}", [line]);
            } finally { _fileMutex.ReleaseMutex(); }
        }
    }
}
