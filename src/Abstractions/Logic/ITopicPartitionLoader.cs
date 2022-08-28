using Confluent.Kafka;

namespace Abstractions.Logic
{
    public interface ITopicPartitionLoader
    {
        public IReadOnlyCollection<TopicPartition> LoadPartitions(TimeSpan timeout);
    }
}
