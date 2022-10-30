using Confluent.Kafka;
using Models;

namespace Abstractions.Logic;

/// <summary>
/// Creates <see cref="GroupLagResult"/> for specific group and partitions.
/// </summary>
public interface IOffsetsLagsLoader
{
    /// <summary>
    /// Creates <see cref="GroupLagResult"/> for specific group and partitions.
    /// </summary>
    /// <param name="partitions">Partitions.</param>
    /// <param name="groupId">Group.</param>
    /// <param name="timeout">Load timeout.</param>
    /// <returns>Lags for group.</returns>
    public GroupLagResult LoadOffsetsLags(IEnumerable<TopicPartition> partitions, GroupId groupId, TimeSpan timeout);
}
