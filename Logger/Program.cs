using Logger.Utils;

internal class Program {
    private static void Main(string[] args) {
        AppConfig config = ConfigurationManager.GetAppConfig();
        Application application = new(config);
        application.Start();
        Thread.Sleep(500);
        application.Shutdown();
    }
}



//internal class Producer {
//    private readonly BlockingCollection<LogEntry> _queue;
//    internal Producer(BlockingCollection<LogEntry> queue) {
//        _queue = queue;
//    }

//    internal void AddLog(LogLevel logLevel, string message) {
//        if (!_queue.TryAdd(new LogEntry(logLevel, DateTime.Now, message), 100)) {
//            Console.WriteLine("[ERROR] It is impossible to add log to the queue");
//        }
//    }
//}

//internal class Consumer {
//    private readonly BlockingCollection<LogEntry> _queue;

//    internal Consumer(BlockingCollection<LogEntry> queue) {
//        _queue = queue;
//    }

//    internal void ConsumeLogs() {

//    }
//}


//internal class LogQueueWrapper {
//    private readonly BlockingCollection<LogEntry> _logQueue;
//    private readonly ConcurrentQueue<LogEntry> _queue;
//    //private Mutex logFileMutex = new Mutex(false, "Global\\ApplicationLog");
//    //private readonly object _queueLock = new object();
//    private readonly SemaphoreSlim _slim;

//    public LogQueueWrapper(int capacity) {
//        //_logQueue = new BlockingCollection<LogEntry>(capacity);
//        _queue = new();
//        _slim = new(0, capacity);
//    }

//    public void AddLogEntry(LogLevel level, string message) {
//        _logQueue.Add(new LogEntry(level, DateTime.Now, message));
//        _slim.Release();
//    }

//    public LogEntry? TakeLogEntry() {
//        _slim.Wait();
//        if (!_queue.TryDequeue(out LogEntry? result)) {
//            _slim.Release();
//            throw new Exception("Error when taking log entry");
//        }
//        return result;
//    }


//}
