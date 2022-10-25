using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Tests
{
    public class PartitionLagTests
    {
        [Fact]
        public void PartitionLagCanBeCreatedWithSpecialOffset()
        {
            // Arrange
            var topicName = "123";
            var partitionId = 42;
            var partition = new Confluent.Kafka.TopicPartition(topicName, new Confluent.Kafka.Partition(partitionId));
            var offset = new Confluent.Kafka.TopicPartitionOffset(partition, Confluent.Kafka.Offset.Beginning);

            // Act
            var lag = new PartitionLag(offset);

            // Assert
            lag.Topic.Should().Be(topicName);
            lag.PartitionId.Should().Be(partitionId);
            lag.Lag.Should().Be(Confluent.Kafka.Offset.Beginning);
        }

        [Fact]
        public void PartitionLagCantBeCreatedWithNonSpecialOffsetAndNoWatermark()
        {
            // Arrange
            var topicName = "123";
            var partitionId = 42;
            var partition = new Confluent.Kafka.TopicPartition(topicName, new Confluent.Kafka.Partition(partitionId));
            var offset = new Confluent.Kafka.TopicPartitionOffset(partition, new Confluent.Kafka.Offset(1));

            // Act
            var exception = Record.Exception(() => new PartitionLag(offset));

            // Assert
            exception.Should().NotBeNull().And.BeOfType<InvalidOperationException>();
        }

        [Fact]
        public void PartitionLagCanBeCreatedWithoutSpecialOffsetAndWatermark()
        {
            // Arrange
            var topOffset = 20;
            var lowOffset = 5;
            var topicName = "123";
            var partitionId = 42;
            var partition = new Confluent.Kafka.TopicPartition(topicName, new Confluent.Kafka.Partition(partitionId));
            var offset = new Confluent.Kafka.TopicPartitionOffset(partition, lowOffset);
            var wm = new Confluent.Kafka.WatermarkOffsets(new Confluent.Kafka.Offset(1), new Confluent.Kafka.Offset(topOffset));

            // Act
            var lag = new PartitionLag(offset, wm);

            // Assert
            lag.Topic.Should().Be(topicName);
            lag.PartitionId.Should().Be(partitionId);
            lag.Lag.Should().Be(15);
        }


        [Fact]
        public void PartitionLagCantBeCreatedWithSpecialOffsetAndWatermark()
        {
            // Arrange
            var topOffset = 20;
            var topicName = "123";
            var partitionId = 42;
            var partition = new Confluent.Kafka.TopicPartition(topicName, new Confluent.Kafka.Partition(partitionId));
            var offset = new Confluent.Kafka.TopicPartitionOffset(partition, Confluent.Kafka.Offset.Beginning);
            var wm = new Confluent.Kafka.WatermarkOffsets(new Confluent.Kafka.Offset(1), new Confluent.Kafka.Offset(topOffset));

            // Act
            var exception = Record.Exception(() => new PartitionLag(offset, wm));

            // Assert
            exception.Should().NotBeNull().And.BeOfType<InvalidOperationException>();
        }
    }
}
