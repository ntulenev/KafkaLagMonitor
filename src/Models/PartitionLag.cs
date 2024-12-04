using Confluent.Kafka;
namespace Models;

/// <summary>
/// Represents the lag information for a specific partition of a topic.
/// </summary>
public class PartitionLag
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PartitionLag"/> class with a special offset.
    /// </summary>
    /// <param name="tpo">The topic partition offset information.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="tpo"/> is null.</exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the offset is not special and watermark offset data is required.
    /// </exception>
    public PartitionLag(TopicPartitionOffset tpo)
    {
        ArgumentNullException.ThrowIfNull(tpo);

        if (!tpo.Offset.IsSpecial)
        {
            throw new InvalidOperationException("This offset is not special and required watermark offset data.");
        }

        Topic = tpo.Topic;
        PartitionId = tpo.Partition.Value;
        Lag = tpo.Offset;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PartitionLag"/> class with a specified watermark offset.
    /// </summary>
    /// <param name="tpo">The topic partition offset information.</param>
    /// <param name="watermark">The watermark offset data for the partition.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="tpo"/> or <paramref name="watermark"/> is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when a special offset is used with watermark offset data.</exception>
    public PartitionLag(TopicPartitionOffset tpo, WatermarkOffsets watermark)
    {
        ArgumentNullException.ThrowIfNull(tpo);
        ArgumentNullException.ThrowIfNull(watermark);

        if (tpo.Offset.IsSpecial)
        {
            throw new InvalidOperationException("Special offset cant be used with watermark offset data.");
        }

        Topic = tpo.Topic;
        PartitionId = tpo.Partition.Value;
        Lag = watermark.High - tpo.Offset;
    }

    /// <summary>
    /// Gets the name of the topic associated with the partition.
    /// </summary>
    public string Topic { get; }

    /// <summary>
    /// Gets the identifier of the partition.
    /// </summary>
    public int PartitionId { get; }

    /// <summary>
    /// Gets the lag value for the partition.
    /// </summary>
    public long Lag { get; }
}
