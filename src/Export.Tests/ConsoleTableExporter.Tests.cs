namespace Export.Tests;

public class ConsoleTableExporterTests
{
    [Fact]
    public void ConsoleTableExporterCanBeCreated()
    {
        // Arrange

        // Act
        var exception = Record.Exception(() => new ConsoleTableExporter());

        // Assert
        exception.Should().BeNull();
    }

    [Fact]
    public void ConsoleTableExporterCanExportData()
    {
        // Arrange
        var exporter = new ConsoleTableExporter();
        var exportModel = new Models.GroupLagResult(new Models.GroupId("1"), new[]
        {
            new Models.PartitionLag(new Confluent.Kafka.TopicPartitionOffset(
                new Confluent.Kafka.TopicPartition("a",new Confluent.Kafka.Partition(1)),Confluent.Kafka.Offset.Beginning))
        });

        // Act
        var exception = Record.Exception(() => exporter.Export(exportModel));

        // Assert
        exception.Should().BeNull();
    }

    [Fact]
    public void ConsoleTableExporterCantExportNullData()
    {
        // Arrange
        var exporter = new ConsoleTableExporter();

        // Act
        var exception = Record.Exception(() => exporter.Export(null!));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

}
