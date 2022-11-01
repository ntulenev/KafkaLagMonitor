using Abstractions.Logic;

namespace Logic.Tests;

public class LagLoaderTests
{
    [Fact]
    public void LagLoaderCanBeCreated()
    {
        // Arrange
        var offsetLoader = Mock.Of<IOffsetsLagsLoader>(MockBehavior.Strict);
        var topicLoader = Mock.Of<ITopicPartitionLoader>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() => new LagLoader(offsetLoader, topicLoader));

        // Assert
        exception.Should().BeNull();
    }

    [Fact]
    public void LagLoaderCantBeCreatedWithNullOffsetLoader()
    {
        // Arrange
        var topicLoader = Mock.Of<ITopicPartitionLoader>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() => new LagLoader(null!, topicLoader));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact]
    public void LagLoaderCantBeCreatedWithNullTopicLoader()
    {
        // Arrange
        var offsetLoader = Mock.Of<IOffsetsLagsLoader>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() => new LagLoader(offsetLoader, null!));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }
}
