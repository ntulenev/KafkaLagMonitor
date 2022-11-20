using Confluent.Kafka;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using Models;

namespace Logic.Tests
{
    public class OffsetsLagsLoaderTest
    {
        [Fact]
        public void OffsetsLagsLoaderCanBeCreated()
        {
            // Arrange
            Func<GroupId, IConsumer<byte[], byte[]>> func = _ => null!;
            var logger = Mock.Of<ILogger<OffsetsLagsLoader>>(MockBehavior.Strict);

            // Act
            var exception = Record.Exception(() => new OffsetsLagsLoader(func, logger));

            // Assert
            exception.Should().BeNull();
        }

        [Fact]
        public void OffsetsLagsLoaderCantBeCreatedWuthNullDelegate()
        {
            // Arrange
            var logger = Mock.Of<ILogger<OffsetsLagsLoader>>(MockBehavior.Strict);

            // Act
            var exception = Record.Exception(() => new OffsetsLagsLoader(null!, logger));

            // Assert
            exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void OffsetsLagsLoaderCantBeCreatedWuthNullLogger()
        {
            // Arrange
            Func<GroupId, IConsumer<byte[], byte[]>> func = _ => null!;

            // Act
            var exception = Record.Exception(() => new OffsetsLagsLoader(func, null!));

            // Assert
            exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void OffsetsLagsLoaderCantLoadForNullPartitions()
        {
            // Arrange
            Func<GroupId, IConsumer<byte[], byte[]>> func = _ => null!;
            var logger = Mock.Of<ILogger<OffsetsLagsLoader>>(MockBehavior.Strict);
            var loader = new OffsetsLagsLoader(func, logger);
            var groupId = new GroupId("Test");
            var timeout = TimeSpan.FromSeconds(1);

            // Act
            var exception = Record.Exception(() => loader.LoadOffsetsLags(null!, groupId, timeout));

            // Assert
            exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void OffsetsLagsLoaderCanLoadData()
        {
            // Arrange
            var groupId = new GroupId("Test");
            var partitions = Enumerable.Empty<TopicPartition>();
            var timeout = TimeSpan.FromSeconds(1);
            var consumer = new Mock<IConsumer<byte[], byte[]>>(MockBehavior.Strict);

            var testOffset1 = new TopicPartitionOffset("A", new Partition(1), new Offset(0));
            var testOffset2 = new TopicPartitionOffset("A", new Partition(2), new Offset(1));
            var testOffset3 = new TopicPartitionOffset("A", new Partition(3), Offset.End);
            var testOffset4 = new TopicPartitionOffset("B", new Partition(1), new Offset(0));

            var offsets = new List<TopicPartitionOffset>()
            {
                testOffset1,testOffset2,testOffset3,testOffset4
            };
            var testWatermark = new WatermarkOffsets(0, 1);
            consumer.Setup(x => x.Committed(partitions, timeout)).Returns(offsets);
            consumer.Setup(x => x.Dispose());
            consumer.Setup(x => x.QueryWatermarkOffsets(It.IsAny<TopicPartition>(), timeout)).Returns(testWatermark);
            Func<GroupId, IConsumer<byte[], byte[]>> func = _ => consumer.Object;
            var logger = NullLogger<OffsetsLagsLoader>.Instance;
            var loader = new OffsetsLagsLoader(func, logger);


            // Act
            var result = loader.LoadOffsetsLags(partitions, groupId, timeout);

            // Assert
            result.Group.Should().Be(groupId);
            var lags = result.Lags.ToList();
            lags.Count.Should().Be(4);
            lags[0].Topic.Should().Be(testOffset1.Topic);
            lags[0].PartitionId.Should().Be(testOffset1.Partition.Value);
            lags[1].Topic.Should().Be(testOffset2.Topic);
            lags[1].PartitionId.Should().Be(testOffset2.Partition.Value);
            lags[2].Topic.Should().Be(testOffset3.Topic);
            lags[2].PartitionId.Should().Be(testOffset3.Partition.Value);
            lags[3].Topic.Should().Be(testOffset4.Topic);
            lags[3].PartitionId.Should().Be(testOffset4.Partition.Value);
        }
    }
}
