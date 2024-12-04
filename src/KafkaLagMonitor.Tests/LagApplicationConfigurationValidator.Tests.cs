using KafkaLagMonitor.Configuration.Validation;
using KafkaLagMonitor.Configuration;

namespace KafkaLagMonitor.Tests;

public class LagApplicationConfigurationValidatorTests
{
    [Fact]
    public void LagApplicationConfigurationValidatorSucceedOnCorrectData()
    {
        // Arrange
        var validator = new LagApplicationConfigurationValidator();

        // Act
        var result = validator.Validate(string.Empty, new LagApplicationConfiguration
        {
            Groups = ["a"],
            Timeout = TimeSpan.FromSeconds(10)
        });

        // Assert
        result.Succeeded.Should().BeTrue();
    }

    [Fact]
    public void LagApplicationConfigurationValidatorFailsOnBadTimestamp()
    {
        // Arrange
        var validator = new LagApplicationConfigurationValidator();

        // Act
        var result = validator.Validate(string.Empty, new LagApplicationConfiguration
        {
            Groups = ["a"],
            Timeout = TimeSpan.Zero
        });

        // Assert
        result.Succeeded.Should().BeFalse();
    }

    [Fact]
    public void LagApplicationConfigurationValidatorFailsOnNullGroups()
    {
        // Arrange
        var validator = new LagApplicationConfigurationValidator();

        // Act
        var result = validator.Validate(string.Empty, new LagApplicationConfiguration
        {
            Groups = null!,
            Timeout = TimeSpan.FromSeconds(10)
        });

        // Assert
        result.Succeeded.Should().BeFalse();
    }

    [Fact]
    public void LagApplicationConfigurationValidatorFailsOnEmptyGroups()
    {
        // Arrange
        var validator = new LagApplicationConfigurationValidator();

        // Act
        var result = validator.Validate(string.Empty, new LagApplicationConfiguration
        {
            Groups = [],
            Timeout = TimeSpan.FromSeconds(10)
        });

        // Assert
        result.Succeeded.Should().BeFalse();
    }

    [Fact]
    public void LagApplicationConfigurationValidatorFailsOnEmptyGroup()
    {
        // Arrange
        var validator = new LagApplicationConfigurationValidator();

        // Act
        var result = validator.Validate(string.Empty, new LagApplicationConfiguration
        {
            Groups =
            [
                "A", string.Empty
            ],
            Timeout = TimeSpan.FromSeconds(10)
        });

        // Assert
        result.Succeeded.Should().BeFalse();
    }

    [Fact]
    public void LagApplicationConfigurationValidatorFailsOnOnlyWhitespaceGroup()
    {
        // Arrange
        var validator = new LagApplicationConfigurationValidator();

        // Act
        var result = validator.Validate(string.Empty, new LagApplicationConfiguration
        {
            Groups =
            [
                "A", "    "
            ],
            Timeout = TimeSpan.FromSeconds(10)
        });

        // Assert
        result.Succeeded.Should().BeFalse();
    }
}
