using KafkaLagMonitor.Configuration;
using KafkaLagMonitor.Configuration.Validation;

namespace KafkaLagMonitor.Tests
{
    public class BootstrapServersConfigurationValidatorTests
    {
        [Fact]
        public void BootstrapServersConfigurationValidatorSucceedOnCorrectData()
        {
            // Arrange
            var validator = new BootstrapServersConfigurationValidator();

            // Act
            var result = validator.Validate(string.Empty, new BootstrapServersConfiguration
            {
                BootstrapServers = new[]
                 {
                     "123"
                 }.ToList()
            });

            // Assert
            result.Succeeded.Should().BeTrue();
        }

        [Fact]
        public void BootstrapServersConfigurationValidatorFailsOnNullServers()
        {
            // Arrange
            var validator = new BootstrapServersConfigurationValidator();

            // Act
            var result = validator.Validate(string.Empty, new BootstrapServersConfiguration
            {
                BootstrapServers = null!
            });

            // Assert
            result.Succeeded.Should().BeFalse();
        }

        [Fact]
        public void BootstrapServersConfigurationValidatorFailsOnNoServers()
        {
            // Arrange
            var validator = new BootstrapServersConfigurationValidator();

            // Act
            var result = validator.Validate(string.Empty, new BootstrapServersConfiguration
            {
                BootstrapServers = new List<string>()
            });

            // Assert
            result.Succeeded.Should().BeFalse();
        }


        [Fact]
        public void BootstrapServersConfigurationValidatorFailsOnEmptyStringServers()
        {
            // Arrange
            var validator = new BootstrapServersConfigurationValidator();

            // Act
            var result = validator.Validate(string.Empty, new BootstrapServersConfiguration
            {
                BootstrapServers = new List<string>()
                {
                    ""
                }
            });

            // Assert
            result.Succeeded.Should().BeFalse();
        }

        [Fact]
        public void BootstrapServersConfigurationValidatorFailsOnSpaceStringServers()
        {
            // Arrange
            var validator = new BootstrapServersConfigurationValidator();

            // Act
            var result = validator.Validate(string.Empty, new BootstrapServersConfiguration
            {
                BootstrapServers = new List<string>()
                {
                    "   "
                }
            });

            // Assert
            result.Succeeded.Should().BeFalse();
        }
    }
}
