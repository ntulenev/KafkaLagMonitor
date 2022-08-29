using Confluent.Kafka;

using Microsoft.Extensions.Logging;

using Abstractions.Logic;
using Models;

namespace Logic
{
    public class OffsetsLagsLoader : IOffsetsLagsLoader
    {
        public OffsetsLagsLoader(Func<GroupId, IConsumer<byte[], byte[]>> metadataConsumerFactory,
                                 ILogger<OffsetsLagsLoader> logger)
        {
            _metadataConsumerFactory = metadataConsumerFactory ?? throw new ArgumentNullException(nameof(metadataConsumerFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IReadOnlyCollection<PartitionLag> LoadOffsetsLags(IEnumerable<TopicPartition> partitions, GroupId groupId, TimeSpan timeout)
        {
            ArgumentNullException.ThrowIfNull(partitions);
            ArgumentNullException.ThrowIfNull(groupId);

            _logger.LogDebug("Loading commited offsets.");

            using var metadataConsumer = _metadataConsumerFactory(groupId);

            var committedOffsets = metadataConsumer.Committed(partitions, timeout);

            _logger.LogDebug("Commited offsets loaded. Count {count}", committedOffsets.Count);

            _logger.LogDebug("Loading topics with offsets.");
            var topicsWithFoundOffsets = committedOffsets.GroupBy(t => t.Topic)
                                                         .Where(t => t.Any(s => !s.Offset.IsSpecial))
                                                         .SelectMany(t => t).ToList();

            _logger.LogDebug("Topics with offsets loaded. Count {count}", topicsWithFoundOffsets.Count);


            _logger.LogDebug("Loading lags.");

            var lags = topicsWithFoundOffsets.Select(tpo =>
            {
                if (tpo.Offset.IsSpecial)
                    return new PartitionLag(tpo.Topic, tpo.Partition.Value, tpo.Offset);

                var watermark = metadataConsumer.QueryWatermarkOffsets(tpo.TopicPartition, timeout);

                return new PartitionLag(tpo.Topic, tpo.Partition.Value, watermark.High - tpo.Offset);
            }).ToList();

            _logger.LogDebug("Lags loaded. Count {count}", lags.Count);

            return lags;
        }

        private readonly Func<GroupId, IConsumer<byte[], byte[]>> _metadataConsumerFactory;
        private readonly ILogger<OffsetsLagsLoader> _logger;
    }
}
