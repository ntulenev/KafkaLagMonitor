using KafkaLagMonitor.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaLagMonitor.Tests
{
    public class BootstrapServersConfigurationTests
    {
        [Fact]
        public void BootstrapServersConfigurationCanBeCreated()
        {
            // Act
            var exception = Record.Exception(() => new BootstrapServersConfiguration());

            // Assert
            exception.Should().BeNull();
        }
    }
}
