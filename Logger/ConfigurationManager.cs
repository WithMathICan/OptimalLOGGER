using Microsoft.Extensions.Configuration;

namespace Logger {
    internal record class AppConfig(int QueueCapacity, string AppLogFileName);

    internal static class ConfigurationManager {
        internal static AppConfig GetAppConfig() {
            string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.{env}.json", optional: false, reloadOnChange: true)
                .Build();

            int queueCapacity = configuration.GetValue<int>("LoggingSettings:QueueCapacity");
            string appLogFileName = configuration.GetValue<string>("LoggingSettings:AppLogFileName") ?? "app.log";
            return new AppConfig(queueCapacity, appLogFileName);
        }
    }
}
