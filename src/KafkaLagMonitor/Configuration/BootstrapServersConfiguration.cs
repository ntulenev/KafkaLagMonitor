using Confluent.Kafka;

namespace KafkaLagMonitor.Configuration;

/// <summary>
/// Bootstrap servers configuration.
/// </summary>
#pragma warning disable CA1515 // Consider making public types internal
public class BootstrapServersConfiguration
#pragma warning restore CA1515 // Consider making public types internal
{
    /// <summary>
    /// List of bootstrap servers.
    /// </summary>
#pragma warning disable CA1002 // Do not expose generic lists
    public List<string> BootstrapServers { get; init; } = default!;
#pragma warning restore CA1002 // Do not expose generic lists

    /// <summary>
    /// Kafka user name, is any.
    /// </summary>
    public string? Username { get; init; }

    /// <summary>
    /// Kafka password name, is any.
    /// </summary>
    public string? Password { get; init; }

    /// <summary>
    /// Kafka security protocol.
    /// </summary>
    public SecurityProtocol SecurityProtocol { get; init; } = SecurityProtocol.Plaintext;

    /// <summary>
    /// Kafka security protocol mechanism.
    /// </summary>
    public SaslMechanism? SASLMechanism { get; init; }
}
