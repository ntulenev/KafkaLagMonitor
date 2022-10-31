using Confluent.Kafka;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Logic.Tests;

public class TopicPartitionLoaderTests
{
    [Fact]
    public void TopicPartitionLoaderCanBeCreated()
    {
        // Arrange
        var client = Mock.Of<IAdminClient>(MockBehavior.Strict);
        var logger = Mock.Of<ILogger<TopicPartitionLoader>>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() => new TopicPartitionLoader(client, logger));

        // Assert
        exception.Should().BeNull();
    }

    [Fact]
    public void TopicPartitionLoaderCantBeCreatedWithNullClient()
    {
        // Arrange
        var logger = Mock.Of<ILogger<TopicPartitionLoader>>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() => new TopicPartitionLoader(null!, logger));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact]
    public void TopicPartitionLoaderCantBeCreatedWithNullLogger()
    {
        // Arrange
        var client = Mock.Of<IAdminClient>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() => new TopicPartitionLoader(client, null!));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact]
    public void TopicPartitionLoaderCanLoadPartitions()
    {
        // Arrange
        var topicName = "A";
        var partId = 42;
        var expected = new[]
        {
            new TopicPartition(topicName,new Partition(partId))
        };
        var timespan = TimeSpan.FromSeconds(1);
        var metaData = new Metadata(new List<BrokerMetadata>(), new List<TopicMetadata>()
        {
            new TopicMetadata(topicName,new List<PartitionMetadata>()
            {
                new PartitionMetadata(partId,9,new[]{ 0 }, new[] { 0},null!)
            }, null!)
        }, 1, "broker");
        var clientMock = new Mock<IAdminClient>(MockBehavior.Strict);
        clientMock.Setup(x => x.GetMetadata(timespan)).Returns(metaData);
        var logger = NullLogger<TopicPartitionLoader>.Instance;
        var loader = new TopicPartitionLoader(clientMock.Object, logger);

        // Act
        var result = loader.LoadPartitions(timespan);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}
