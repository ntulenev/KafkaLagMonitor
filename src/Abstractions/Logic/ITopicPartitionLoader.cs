using Confluent.Kafka;

namespace Abstractions.Logic;

/// <summary>
/// Partitions loader.
/// </summary>
public interface ITopicPartitionLoader
{
    /// <summary>
    /// Loads Kafka partitions.
    /// </summary>
    /// <param name="timeout">Loads timeout.</param>
    /// <returns>Partitions collection.</returns>
    public IReadOnlyCollection<TopicPartition> LoadPartitions(TimeSpan timeout);
}
