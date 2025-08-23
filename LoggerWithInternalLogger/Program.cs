namespace LoggerWithInternalLogger {
    internal class Program {
        private static void Main(string[] args) {
            Application app = new();
            app.Start();
            Thread.Sleep(500);
            app.Shutdown();
        }
    }
}