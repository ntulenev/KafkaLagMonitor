using Microsoft.Extensions.Logging;

using Confluent.Kafka;

using Abstractions.Logic;

namespace Logic;

/// <summary>
/// Kafka partitions loader.
/// </summary>
public class TopicPartitionLoader : ITopicPartitionLoader
{
    /// <summary>
    /// Creates <see cref="TopicPartitionLoader"/>.
    /// </summary>
    /// <param name="adminClient">Kafka admin client.</param>
    /// <param name="logger">Logger.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public TopicPartitionLoader(IAdminClient adminClient,
                                ILogger<TopicPartitionLoader> logger)
    {
        _adminClient = adminClient ?? throw new ArgumentNullException(nameof(adminClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public IReadOnlyCollection<TopicPartition> LoadPartitions(TimeSpan timeout)
    {
        _logger.LogDebug("Starting load partitions");

        var metadata = _adminClient.GetMetadata(timeout);
        var partitions = metadata.Topics
                                    .SelectMany(t => t.Partitions.Select(p => new TopicPartition(t.Topic, p.PartitionId)))
                                    .ToList();

        _logger.LogDebug("Partitions has beed loaded. Count - {count}", partitions.Count);

        return partitions;
    }

    private readonly IAdminClient _adminClient;
    private readonly ILogger<TopicPartitionLoader> _logger;
}
