using Confluent.Kafka;

using Microsoft.Extensions.Logging;

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
        public void OffsetsLagsLoaderCantLoadForNullGroup()
        {
            // Arrange
            Func<GroupId, IConsumer<byte[], byte[]>> func = _ => null!;
            var logger = Mock.Of<ILogger<OffsetsLagsLoader>>(MockBehavior.Strict);
            var loader = new OffsetsLagsLoader(func, logger);
            var partitions = Enumerable.Empty<TopicPartition>();
            var timeout = TimeSpan.FromSeconds(1);

            // Act
            var exception = Record.Exception(() => loader.LoadOffsetsLags(partitions, null!, timeout));

            // Assert
            exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
        }
    }
}
