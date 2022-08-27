using Confluent.Kafka;
using ConsoleTables;

var hosts = new[]
{
   "test"
};

string groupId = "commands";

Console.WriteLine($"Lag for groupId = {groupId}");

using var adminClient = new AdminClientBuilder(new AdminClientConfig
{
    BootstrapServers = string.Join(",", hosts)
}).Build();

// step 1.
var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(20));
var topicMetadata = metadata.Topics.SelectMany(t => t.Partitions.Select(p => new TopicPartition(t.Topic, p.PartitionId))).ToArray();

// step 2.
using var metadataConsumer = new ConsumerBuilder<byte[], byte[]>(new ConsumerConfig
{
    EnableAutoCommit = false,
    AllowAutoCreateTopics = false,
    EnableAutoOffsetStore = false,
    AutoOffsetReset = AutoOffsetReset.Error,
    GroupId = groupId,
    BootstrapServers = string.Join(",", hosts)
})
    .SetValueDeserializer(ThrowingDeserializer.Instance)
    .SetKeyDeserializer(ThrowingDeserializer.Instance)
    .SetPartitionsAssignedHandler((Func<IConsumer<byte[], byte[]>, List<TopicPartition>, IEnumerable<TopicPartitionOffset>>)((_, _) => throw new NotSupportedException("Misused.")))
    .Build();

// step 4.
var committedOffsets = metadataConsumer.Committed(topicMetadata, TimeSpan.FromSeconds(10));

// step 5.
var topicsWithFoundOffsets = committedOffsets.GroupBy(t => t.Topic).Where(t => t.Any(s => !s.Offset.IsSpecial)).SelectMany(t => t);

// step 6.
var lag = topicsWithFoundOffsets.Select<TopicPartitionOffset, (TopicPartition Partition, long Lag)>(tpo =>
 {
     if (tpo.Offset.IsSpecial)
         return (tpo.TopicPartition, tpo.Offset);

     var watermark = metadataConsumer.QueryWatermarkOffsets(tpo.TopicPartition, TimeSpan.FromSeconds(10));
     return (tpo.TopicPartition, watermark.High - tpo.Offset);
 });

var table = new ConsoleTable("Topic", "Partition", "Lag");
foreach (var item in lag)
{
    table.AddRow(item.Partition.Topic, item.Partition.Partition.Value, item.Lag);
}
table.Write();

public class ThrowingDeserializer : IDeserializer<byte[]>
{
    private ThrowingDeserializer() { }

    public static ThrowingDeserializer Instance = new ThrowingDeserializer();

    public byte[] Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        throw new NotSupportedException();
    }
}