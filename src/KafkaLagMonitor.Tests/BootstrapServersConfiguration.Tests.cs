using KafkaLagMonitor.Configuration;

namespace KafkaLagMonitor.Tests;

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
