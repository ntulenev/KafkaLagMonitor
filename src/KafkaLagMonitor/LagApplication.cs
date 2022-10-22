using Microsoft.Extensions.Options;

using Abstractions.Export;
using Abstractions.Logic;
using KafkaLagMonitor.Configuration;
using Models;

namespace KafkaLagMonitor
{
    /// <summary>
    /// Main app. Shows lags for groups from config.
    /// </summary>
    public class LagApplication
    {
        /// <summary>
        /// Creates <see cref="LagApplication"/>.
        /// </summary>
        /// <param name="options">Configuration.</param>
        /// <param name="loader">Kafla lag loader.</param>
        /// <param name="exporter">Kafka lag exporter.</param>
        /// <exception cref="ArgumentNullException">Throws if some params not set.</exception>
        /// <exception cref="ArgumentException">Throws if some params if not correct.</exception>
        public LagApplication(IOptions<LagApplicationConfiguration> options,
                              ILagLoader loader,
                              IExporter exporter
                              )
        {
            ArgumentNullException.ThrowIfNull(options);

            _loader = loader ?? throw new ArgumentNullException(nameof(loader));
            _exporter = exporter ?? throw new ArgumentNullException(nameof(exporter));

            if (options.Value is null)
            {
                throw new ArgumentException("Options value is not set", nameof(options));
            }

            _timeout = options.Value.Timeout;
            _groups = options.Value.GetGroups();
        }

        /// <summary>
        /// Runs loading data from Kafka.
        /// </summary>
        public void Run()
        {
            foreach (var group in _groups)
            {
                Console.WriteLine(group.Value);

                var lags = _loader.LoadOffsetsLags(group, _timeout);
                _exporter.Export(lags);
            }
        }

        private readonly ILagLoader _loader;
        private readonly IExporter _exporter;
        private readonly IEnumerable<GroupId> _groups;
        private readonly TimeSpan _timeout;
    }
}
