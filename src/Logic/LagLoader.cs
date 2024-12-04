using Abstractions.Logic;
using Models;

namespace Logic;

/// <summary>
/// Loader for for Kafka lags info.
/// </summary>
public class LagLoader : ILagLoader
{
    /// <summary>
    /// Creates <see cref="=LagLoader"/>.
    /// </summary>
    /// <param name="offsetLoader">Offset loader.</param>
    /// <param name="topicLoader">Topic loader.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public LagLoader(IOffsetsLagsLoader offsetLoader,
                     ITopicPartitionLoader topicLoader
                    )
    {
        _offsetLoader = offsetLoader ?? throw new ArgumentNullException(nameof(offsetLoader));
        _topicLoader = topicLoader ?? throw new ArgumentNullException(nameof(topicLoader));
    }

    /// <inheritdoc/>
    public GroupLagResult LoadOffsetsLags(GroupId groupId, TimeSpan timeout)
    {
        var partitions = _topicLoader.LoadPartitions(timeout);
        return _offsetLoader.LoadOffsetsLags(partitions, groupId, timeout);
    }

    private readonly IOffsetsLagsLoader _offsetLoader;
    private readonly ITopicPartitionLoader _topicLoader;
}
