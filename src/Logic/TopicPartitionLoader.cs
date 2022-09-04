using Microsoft.Extensions.Logging;

using Confluent.Kafka;

using Abstractions.Logic;

namespace Logic
{
    public class TopicPartitionLoader : ITopicPartitionLoader
    {
        public TopicPartitionLoader(IAdminClient adminClient,
                                    ILogger<TopicPartitionLoader> logger)
        {
            _adminClient = adminClient ?? throw new ArgumentNullException(nameof(adminClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
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
}
