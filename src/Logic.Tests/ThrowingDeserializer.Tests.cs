using Logic.Kafka;

namespace Logic.Tests;

public class ThrowingDeserializerTests
{
    [Fact]
    public void ThrowingDeserializerCanBeCreated()
    {
        // Act
        var exception = Record.Exception(() => _ = ThrowingDeserializer.Instance);

        // Assert
        exception.Should().BeNull();
    }
}
