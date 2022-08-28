using Abstractions.Export;
using Abstractions.Logic;

namespace KafkaLagMonitor
{
    public class LagApplication
    {
        public LagApplication
            (
             IOffsetsLagsLoader offsetLoader,
             ITopicPartitionLoader topicLoader,
             IExporter exporter
            )
        {
            _offsetLoader = offsetLoader ?? throw new ArgumentNullException(nameof(offsetLoader));
            _topicLoader = topicLoader ?? throw new ArgumentNullException(nameof(topicLoader));
            _exporter = exporter ?? throw new ArgumentNullException(nameof(exporter));
        }

        public void Run()
        {
            //TODO Add to options
            var timeout = TimeSpan.FromSeconds(20);

            var partitions = _topicLoader.LoadPartitions(timeout);
            var lags = _offsetLoader.LoadOffsetsLags(partitions, timeout);
            _exporter.Export(lags);
        }

        private readonly IOffsetsLagsLoader _offsetLoader;
        private readonly ITopicPartitionLoader _topicLoader;
        private readonly IExporter _exporter;
    }
}
