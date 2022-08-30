using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Confluent.Kafka;

using Serilog;

using Abstractions.Export;
using Abstractions.Logic;
using Export;
using Logic;
using Logic.Kafka;
using Models;
using KafkaLagMonitor.Configuration;

namespace KafkaLagMonitor
{
    public static class StartupHelper
    {

        public static void RegisterConfigs(this IServiceCollection services, HostBuilderContext hostContext)
        {
            services.Configure<LagApplicationConfiguration>(hostContext.Configuration.GetSection(nameof(LagApplicationConfiguration)));
        }

        public static void RegisterApplicationSettings(this IConfigurationBuilder builder)
        {
            builder.AddJsonFile("appsettings.json", optional: true);
        }

        public static void AddLogging(this IServiceCollection services, HostBuilderContext hostContext)
        {
            var logger = new LoggerConfiguration()
                             .ReadFrom.Configuration(hostContext.Configuration)
                             .CreateLogger();

            services.AddLogging(x =>
            {
                x.SetMinimumLevel(LogLevel.Information);
                x.AddSerilog(logger: logger, dispose: true);
            });
        }

        public static void AddAppServices(this IServiceCollection services, HostBuilderContext hostContext)
        {
            services.AddSingleton<IExporter, ConsoleTableExporter>();
            services.AddSingleton<IOffsetsLagsLoader, OffsetsLagsLoader>();
            services.AddSingleton<ITopicPartitionLoader, TopicPartitionLoader>();
            services.AddSingleton<ILagLoader, LagLoader>();
            services.AddSingleton<LagApplication>();
        }

        public static void AddKafka(this IServiceCollection services, HostBuilderContext hostContext)
        {
            //TODO Move to config

            var hosts = new[]
            {
                "localhost:9092"
            };

            services.AddSingleton<Func<GroupId, IConsumer<byte[], byte[]>>>(sp =>
            {
                return (groupId) =>
                {
                    return new ConsumerBuilder<byte[], byte[]>(new ConsumerConfig
                    {
                        EnableAutoCommit = false,
                        AllowAutoCreateTopics = false,
                        EnableAutoOffsetStore = false,
                        AutoOffsetReset = AutoOffsetReset.Error,
                        GroupId = groupId.Value,
                        BootstrapServers = string.Join(",", hosts)
                    })
                    .SetValueDeserializer(ThrowingDeserializer.Instance)
                    .SetKeyDeserializer(ThrowingDeserializer.Instance)
                    .SetPartitionsAssignedHandler((_, _) => throw new NotSupportedException("Misused."))
                    .Build();
                };
            });

            services.AddSingleton(sp =>
            {
                return new AdminClientBuilder(new AdminClientConfig
                {
                    BootstrapServers = string.Join(",", hosts)
                }).Build();
            });
        }
    }
}
