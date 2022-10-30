using Confluent.Kafka;

using FluentAssertions;

using Microsoft.Extensions.Logging;

using Moq;

namespace Logic.Tests
{
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
    }
}
