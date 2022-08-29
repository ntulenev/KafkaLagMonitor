using Abstractions.Export;
using Abstractions.Logic;
using Models;

namespace KafkaLagMonitor
{
    public class LagApplication
    {
        public LagApplication(ILagLoader loader,
                              IExporter exporter
                              )
        {
            _loader = loader ?? throw new ArgumentNullException(nameof(loader));
            _exporter = exporter ?? throw new ArgumentNullException(nameof(exporter));
        }

        public void Run()
        {
            //TODO Add to options
            var timeout = TimeSpan.FromSeconds(20);
            var group = new GroupId("commands");

            var lags = _loader.LoadOffsetsLags(group, timeout);
            _exporter.Export(lags);
        }

        private readonly ILagLoader _loader;
        private readonly IExporter _exporter;
    }
}
