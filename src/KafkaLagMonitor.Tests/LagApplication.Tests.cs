using Microsoft.Extensions.Options;

using Abstractions.Export;
using Abstractions.Logic;
using KafkaLagMonitor.Configuration;
using Models;

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

    [Fact]
    public void LagApplicationLoadsAndExportAllGroupsOnRun()
    {
        //Arrange
        var timeout = TimeSpan.FromSeconds(5);
        var group1 = "test1";
        var group2 = "test2";
        var topOffset1 = 20;
        var lowOffset1 = 5;
        var topicName1 = "123";
        var partitionId1 = 42;
        var partition1 = new Confluent.Kafka.TopicPartition(topicName1, new Confluent.Kafka.Partition(partitionId1));
        var offset1 = new Confluent.Kafka.TopicPartitionOffset(partition1, lowOffset1);
        var wm1 = new Confluent.Kafka.WatermarkOffsets(new Confluent.Kafka.Offset(1), new Confluent.Kafka.Offset(topOffset1));
        PartitionLag lag1 = new PartitionLag(offset1, wm1);
        PartitionLag[] lags1 = new[] { lag1 };
        var topOffset2 = 21;
        var lowOffset2 = 6;
        var topicName2 = "456";
        var partitionId2 = 43;
        var partition2 = new Confluent.Kafka.TopicPartition(topicName2, new Confluent.Kafka.Partition(partitionId2));
        var offset2 = new Confluent.Kafka.TopicPartitionOffset(partition2, lowOffset2);
        var wm2 = new Confluent.Kafka.WatermarkOffsets(new Confluent.Kafka.Offset(1), new Confluent.Kafka.Offset(topOffset2));
        PartitionLag lag2 = new PartitionLag(offset1, wm2);
        PartitionLag[] lags2 = new[] { lag2 };
        var lagResult1 = new GroupLagResult(new GroupId(group1), lags1);
        var lagResult2 = new GroupLagResult(new Models.GroupId(group2), lags2);
        var loader = new Mock<ILagLoader>(MockBehavior.Strict);
        loader.Setup(x => x.LoadOffsetsLags(new Models.GroupId(group1), timeout)).Returns(lagResult1);
        loader.Setup(x => x.LoadOffsetsLags(new Models.GroupId(group2), timeout)).Returns(lagResult2);
        var exporter = new Mock<IExporter>(MockBehavior.Strict);
        int export1 = 0;
        int export2 = 0;
        exporter.Setup(x => x.Export(lagResult1)).Callback(() => export1++);
        exporter.Setup(x => x.Export(lagResult2)).Callback(() => export2++);
        var options = new Mock<IOptions<LagApplicationConfiguration>>(MockBehavior.Strict);
        options.Setup(x => x.Value).Returns(new LagApplicationConfiguration
        {
            Timeout = timeout,
            Groups = new()
              {
                  group1,group2
              }
        });
        var app = new LagApplication(options.Object, loader.Object, exporter.Object);

        // Act
        app.Run();

        // Assert
        export1.Should().Be(1);
        export2.Should().Be(1);
    }
}
