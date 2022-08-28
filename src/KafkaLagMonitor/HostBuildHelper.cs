using Microsoft.Extensions.Hosting;

namespace KafkaLagMonitor
{
    public static class HostBuildHelper
    {
        /// <summary>
        /// Creates default host for app.
        /// </summary>
        public static IHost CreateHost()
        {
            var builder = new HostBuilder()
                   .ConfigureAppConfiguration((hostingContext, config) =>
                   {
                       config.RegisterApplicationSettings();
                   })
                   .ConfigureServices((hostContext, services) =>
                   {
                       services.AddKafka(hostContext);
                       services.AddAppServices(hostContext);
                       services.AddLogging(hostContext);
                   });

            return builder.Build();
        }
    }
}
