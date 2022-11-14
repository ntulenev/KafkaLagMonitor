using Microsoft.Extensions.Options;

using Abstractions.Export;
using Abstractions.Logic;
using KafkaLagMonitor.Configuration;

namespace KafkaLagMonitor.Tests;

public class LagApplicationTests
{
    [Fact]
    public void LagApplicationCantBeCreatedWithoutExporter()
    {
        //Arrange
        var options = new Mock<IOptions<LagApplicationConfiguration>>(MockBehavior.Strict);
        var loader = new Mock<ILagLoader>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() => new LagApplication(options.Object, loader.Object, null!));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact]
    public void LagApplicationCantBeCreatedWithoutLoader()
    {
        //Arrange
        var options = new Mock<IOptions<LagApplicationConfiguration>>(MockBehavior.Strict);
        var exporter = new Mock<IExporter>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() => new LagApplication(options.Object, null!, exporter.Object));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact]
    public void LagApplicationCantBeCreatedWithoutOptions()
    {
        //Arrange
        var loader = new Mock<ILagLoader>(MockBehavior.Strict);
        var exporter = new Mock<IExporter>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() => new LagApplication(null!, loader.Object, exporter.Object));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }
}
