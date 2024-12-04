using KafkaLagMonitor.Configuration;

namespace KafkaLagMonitor.Tests
{
    public class LagApplicationConfigurationTests
    {
        [Fact]
        public void LagApplicationConfigurationCanBeCreated()
        {
            // Act
            var exception = Record.Exception(() => new LagApplicationConfiguration());

            // Assert
            exception.Should().BeNull();
        }

        [Fact]
        public void LagApplicationConfigurationReturnsCorrectGroups()
        {
            // Arrange
            var config = new LagApplicationConfiguration()
            {
                Groups =
                [
                      "A","B"
                ]
            };

            // Act
            var groups = config.GetGroups();

            // Assert
            groups.Should().HaveCount(2);
            var items = groups.ToArray();
            items[0].Value.Should().Be("A");
            items[1].Value.Should().Be("B");
        }
    }
}
