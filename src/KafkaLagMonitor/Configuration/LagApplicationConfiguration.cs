namespace KafkaLagMonitor.Configuration
{
    public class LagApplicationConfiguration
    {
        public List<string> Groups { get; set; } = null!;

        public TimeSpan Timeout { get; set; }
    }
}
