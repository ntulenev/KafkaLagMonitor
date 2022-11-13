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
            Groups = new List<string> { "a" },
            Timeout = TimeSpan.FromSeconds(10)
        });

        // Assert
        result.Succeeded.Should().BeTrue();
    }

    [Fact]
    public void LagApplicationConfigurationValidatorFaildsOnBadTimestamp()
    {
        // Arrange
        var validator = new LagApplicationConfigurationValidator();

        // Act
        var result = validator.Validate(string.Empty, new LagApplicationConfiguration
        {
            Groups = new List<string> { "a" },
            Timeout = TimeSpan.Zero
        });

        // Assert
        result.Succeeded.Should().BeFalse();
    }

    [Fact]
    public void LagApplicationConfigurationValidatorFaildsOnNullGroups()
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
    public void LagApplicationConfigurationValidatorFaildsOnEmptyGroups()
    {
        // Arrange
        var validator = new LagApplicationConfigurationValidator();

        // Act
        var result = validator.Validate(string.Empty, new LagApplicationConfiguration
        {
            Groups = new List<string>(),
            Timeout = TimeSpan.FromSeconds(10)
        });

        // Assert
        result.Succeeded.Should().BeFalse();
    }

    [Fact]
    public void LagApplicationConfigurationValidatorFaildsOnEmptyGroup()
    {
        // Arrange
        var validator = new LagApplicationConfigurationValidator();

        // Act
        var result = validator.Validate(string.Empty, new LagApplicationConfiguration
        {
            Groups = new List<string>()
            {
                "A", string.Empty
            },
            Timeout = TimeSpan.FromSeconds(10)
        });

        // Assert
        result.Succeeded.Should().BeFalse();
    }

    [Fact]
    public void LagApplicationConfigurationValidatorFaildsOnOnlyWhitespaceGroup()
    {
        // Arrange
        var validator = new LagApplicationConfigurationValidator();

        // Act
        var result = validator.Validate(string.Empty, new LagApplicationConfiguration
        {
            Groups = new List<string>()
            {
                "A", "    "
            },
            Timeout = TimeSpan.FromSeconds(10)
        });

        // Assert
        result.Succeeded.Should().BeFalse();
    }
}
