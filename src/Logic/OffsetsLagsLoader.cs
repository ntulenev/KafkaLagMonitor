using Confluent.Kafka;

using Microsoft.Extensions.Logging;

using Abstractions.Logic;
using Models;

namespace Logic
{
    /// <summary>
    /// Loads partitions offsets.
    /// </summary>
    public class OffsetsLagsLoader : IOffsetsLagsLoader
    {
        /// <summary>
        /// Creates <see cref="OffsetsLagsLoader"/>.
        /// </summary>
        /// <param name="metadataConsumerFactory">Kafka consumer factory.</param>
        /// <param name="logger">Lpgger.</param>
        /// <exception cref="ArgumentNullException">If factory or logger is null.</exception>
        public OffsetsLagsLoader(Func<GroupId, IConsumer<byte[], byte[]>> metadataConsumerFactory,
                                 ILogger<OffsetsLagsLoader> logger)
        {
            _metadataConsumerFactory = metadataConsumerFactory ?? throw new ArgumentNullException(nameof(metadataConsumerFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public GroupLagResult LoadOffsetsLags(IEnumerable<TopicPartition> partitions, GroupId groupId, TimeSpan timeout)
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
                {
                    return new PartitionLag(tpo);
                }

                var watermark = metadataConsumer.QueryWatermarkOffsets(tpo.TopicPartition, timeout);

                return new PartitionLag(tpo, watermark);

            }).ToList();

            _logger.LogDebug("Lags loaded. Count {count}", lags.Count);

            return new(groupId, lags);
        }

        private readonly Func<GroupId, IConsumer<byte[], byte[]>> _metadataConsumerFactory;
        private readonly ILogger<OffsetsLagsLoader> _logger;
    }
}
