using Confluent.Kafka;

using Models;
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

    [Fact]
    public void LadLoaderCantLoadForNullGroupId()
    {
        // Arrange
        var offsetLoader = Mock.Of<IOffsetsLagsLoader>(MockBehavior.Strict);
        var topicLoader = Mock.Of<ITopicPartitionLoader>(MockBehavior.Strict);
        var loader = new LagLoader(offsetLoader, topicLoader);
        var timeSpan = TimeSpan.FromSeconds(1);

        // Act
        var exception = Record.Exception(() => loader.LoadOffsetsLags(null!, timeSpan));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact]
    public void LadLoaderCanLoadData()
    {
        // Arrange
        var timeSpan = TimeSpan.FromSeconds(1);
        var group = new GroupId("data");
        var topOffset = 20;
        var lowOffset = 5;
        var topicName = "123";
        var partitionId = 42;
        var partition = new Confluent.Kafka.TopicPartition(topicName, new Confluent.Kafka.Partition(partitionId));
        var offset = new Confluent.Kafka.TopicPartitionOffset(partition, lowOffset);
        var wm = new Confluent.Kafka.WatermarkOffsets(new Confluent.Kafka.Offset(1), new Confluent.Kafka.Offset(topOffset));
        var lags = new[]
        {
            new PartitionLag(offset, wm)
        };
        var expectedResult = new GroupLagResult(group, lags);
        var partitions = new[]
        {
            new TopicPartition("a",new Partition(1))
        };
        var topicLoader = new Mock<ITopicPartitionLoader>(MockBehavior.Strict);
        topicLoader.Setup(x => x.LoadPartitions(timeSpan)).Returns(partitions);
        var offsetLoader = new Mock<IOffsetsLagsLoader>(MockBehavior.Strict);
        offsetLoader.Setup(x => x.LoadOffsetsLags(partitions, group, timeSpan)).Returns(expectedResult);
        var loader = new LagLoader(offsetLoader.Object, topicLoader.Object);

        // Act
        var result = loader.LoadOffsetsLags(group, timeSpan);

        // Assert
        result.Should().Be(expectedResult);
    }
}
