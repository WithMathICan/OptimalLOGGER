using Logger.Logger;
using Logger.Services;
using Logger.Utils;

/// <summary>
/// Main application class coordinating services and logging
/// </summary>
internal class Application {
    private readonly IAppLogger _appLogger;
    private readonly List<IService> _services;
    private readonly SimpleFileWriter _appLogWriter;

    /// <summary>
    /// Initializes a new instance of the application
    /// </summary>
    /// <param name="config">Application configuration settings</param>
    internal Application(AppConfig config) {
        _appLogWriter = new SimpleFileWriter(config.AppLogFileName, config.MaxNumberOfRetriesToWriteToFile);
        _appLogger = new LoggerSimple(_appLogWriter);
        _services = [
            new HighLoadService(_appLogger),
            new LoadService(_appLogger),
            new LoadServiceWithError(_appLogger),
        ];
    }

    /// <summary>
    /// Executes a collection of actions in parallel
    /// </summary>
    /// <param name="actions">Collection of actions to run</param>
    private static void RunTasks(IEnumerable<Action> actions) {
        var tasks = actions.Select(a => Task.Run(a));
        Task.WaitAll([.. tasks]);
    }

    /// <summary>
    /// Starts all services in parallel
    /// </summary>
    internal void Start() {
        RunTasks(_services.Select<IService, Action>(s => s.Start));
    }

    /// <summary>
    /// Shuts down all services and disposes resources
    /// </summary>
    internal void Shutdown() {
        RunTasks(_services.Select<IService, Action>(s => s.Stop));
        _appLogWriter.Dispose();
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
//    //private readonly object _queueLock = new object;
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



