﻿namespace KafkaLagMonitor.Configuration
{
    /// <summary>
    /// Lag application configuration.
    /// </summary>
    public class LagApplicationConfiguration
    {
        /// <summary>
        /// List of groups where we need to check the offsets.
        /// </summary>
        public List<string> Groups { get; set; } = null!;

        /// <summary>
        /// Offsets request timeout.
        /// </summary>
        public TimeSpan Timeout { get; set; }
    }
}
