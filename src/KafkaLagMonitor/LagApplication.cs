using Abstractions.Export;
using Abstractions.Logic;
using KafkaLagMonitor.Configuration;
using Microsoft.Extensions.Options;
using Models;

namespace KafkaLagMonitor
{
    public class LagApplication
    {
        public LagApplication(IOptions<LagApplicationConfiguration> options,
                              ILagLoader loader,
                              IExporter exporter
                              )
        {
            ArgumentNullException.ThrowIfNull(options);

            _loader = loader ?? throw new ArgumentNullException(nameof(loader));
            _exporter = exporter ?? throw new ArgumentNullException(nameof(exporter));
            _options = options.Value ?? throw new ArgumentException("Options value is not set", nameof(options));
        }

        public void Run()
        {
            foreach (var groupName in _options.Groups)
            {
                var group = new GroupId(groupName);

                Console.WriteLine(group.Value);

                var lags = _loader.LoadOffsetsLags(group, _options.Timeout);
                _exporter.Export(lags);
            }
        }

        private readonly ILagLoader _loader;
        private readonly IExporter _exporter;
        private readonly LagApplicationConfiguration _options;
    }
}
