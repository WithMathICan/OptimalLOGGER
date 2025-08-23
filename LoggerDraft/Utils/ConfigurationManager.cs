using Microsoft.Extensions.Configuration;

namespace Logger.Utils {
    internal record class AppConfig(int QueueCapacity, string AppLogFileName, int MaxNumberOfRetriesToWriteToFile);

    internal static class ConfigurationManager {
        internal static AppConfig GetAppConfig() {
            string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.{env}.json", optional: false, reloadOnChange: true)
                .Build();

            int queueCapacity = configuration.GetValue<int>("LoggingSettings:QueueCapacity");
            int retriesNumber = configuration.GetValue<int>("LoggingSettings:MaxNumberOfRetriesToWriteToFile");
            string appLogFileName = configuration.GetValue<string>("LoggingSettings:AppLogFileName") ?? "app.log";
            return new AppConfig(queueCapacity, appLogFileName, retriesNumber);
        }
    }
}
