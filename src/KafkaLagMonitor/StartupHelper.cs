using System.Diagnostics;

using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Confluent.Kafka;

using Serilog;

using Abstractions.Export;
using Abstractions.Logic;
using KafkaLagMonitor.Configuration.Validation;
using KafkaLagMonitor.Configuration;
using Export;
using Logic;
using Logic.Kafka;
using Models;

namespace KafkaLagMonitor;

/// <summary>
/// Config and DI helpers
/// </summary>
public static class StartupHelper
{
    /// <summary>
    /// Register configuration DI.
    /// </summary>
    public static void RegisterConfigs(this IServiceCollection services, HostBuilderContext hostContext)
    {
        services.AddSingleton<IValidateOptions<LagApplicationConfiguration>, LagApplicationConfigurationValidator>();
        services.AddSingleton<IValidateOptions<BootstrapServersConfiguration>, BootstrapServersConfigurationValidator>();
        services.Configure<LagApplicationConfiguration>(hostContext.Configuration.GetSection(nameof(LagApplicationConfiguration)));
    }

    /// <summary>
    /// Register config files.
    /// </summary>
    /// <param name="builder"></param>
    public static void RegisterApplicationSettings(this IConfigurationBuilder builder)
    {
        builder.AddJsonFile("appsettings.json", optional: true);
    }

    /// <summary>
    /// Registers logging.
    /// </summary>
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

    /// <summary>
    /// Registers logic.
    /// </summary>
    public static void AddAppServices(this IServiceCollection services, HostBuilderContext hostContext)
    {
        services.AddSingleton<IExporter, ConsoleTableExporter>();
        services.AddSingleton<IOffsetsLagsLoader, OffsetsLagsLoader>();
        services.AddSingleton<ITopicPartitionLoader, TopicPartitionLoader>();
        services.AddSingleton<ILagLoader, LagLoader>();
        services.AddSingleton<LagApplication>();
    }

    /// <summary>
    /// Gets configuration for Kafka servers.
    /// </summary>
    public static BootstrapServersConfiguration GetBootstrapConfig(this IServiceProvider sp, IConfiguration configuration)
    {
        var section = configuration.GetSection(nameof(BootstrapServersConfiguration));
        var config = section.Get<BootstrapServersConfiguration>();

        Debug.Assert(config is not null);

        var validator = sp.GetRequiredService<IValidateOptions<BootstrapServersConfiguration>>();

        // Crutch to use IValidateOptions in manual generation logic.
        var validationResult = validator.Validate(string.Empty, config);
        if (validationResult.Failed)
        {
            throw new OptionsValidationException
                (string.Empty, typeof(BootstrapServersConfiguration), new[] { validationResult.FailureMessage });
        }

        return config;
    }

    /// <summary>
    /// Registers Kafka.
    /// </summary>
    public static void AddKafka(this IServiceCollection services, HostBuilderContext hostContext)
    {
        services.AddSingleton<Func<GroupId, IConsumer<byte[], byte[]>>>(sp =>
        {
            var config = sp.GetBootstrapConfig(hostContext.Configuration);
            var servers = string.Join(",", config.BootstrapServers);

            return (groupId) =>
            {
                return new ConsumerBuilder<byte[], byte[]>(new ConsumerConfig
                {
                    EnableAutoCommit = false,
                    AllowAutoCreateTopics = false,
                    EnableAutoOffsetStore = false,
                    AutoOffsetReset = AutoOffsetReset.Error,
                    GroupId = groupId.Value,
                    BootstrapServers = servers,
                    SecurityProtocol = config.SecurityProtocol,
                    SaslMechanism = config.SASLMechanism,
                    SaslUsername = config.Username,
                    SaslPassword = config.Password
                })
                .SetValueDeserializer(ThrowingDeserializer.Instance)
                .SetKeyDeserializer(ThrowingDeserializer.Instance)
                .SetPartitionsAssignedHandler((_, _) => throw new NotSupportedException("Misused."))
                .Build();
            };
        });

        services.AddSingleton(sp =>
        {
            var config = sp.GetBootstrapConfig(hostContext.Configuration);
            var servers = string.Join(",", config.BootstrapServers);

            return new AdminClientBuilder(new AdminClientConfig
            {
                BootstrapServers = servers,
                SecurityProtocol = config.SecurityProtocol,
                SaslMechanism = config.SASLMechanism,
                SaslUsername = config.Username,
                SaslPassword = config.Password
            }).Build();
        });
    }
}
