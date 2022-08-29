using Confluent.Kafka;

using Models;

namespace Abstractions.Logic
{
    public interface IOffsetsLagsLoader
    {
        public IReadOnlyCollection<PartitionLag> LoadOffsetsLags(IEnumerable<TopicPartition> partitions, GroupId groupId, TimeSpan timeout);
    }
}
