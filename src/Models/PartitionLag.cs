using Confluent.Kafka;

namespace Models;

public class PartitionLag
{
    public PartitionLag(TopicPartitionOffset tpo)
    {
        ArgumentNullException.ThrowIfNull(tpo);

        if (!tpo.Offset.IsSpecial)
        {
            throw new InvalidOperationException("This offset is not special and requred watermark offset data.");
        }

        Topic = tpo.Topic;
        PartitionId = tpo.Partition.Value;
        Lag = tpo.Offset;
    }

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

    public string Topic { get; }

    public int PartitionId { get; }

    public long Lag { get; }
}
