namespace Models.Tests;

public class GroupIdTests
{
    [Fact]
    public void GroupIdCanBeCreated()
    {
        // Arrange
        var name = "test";

        // Act
        var groupId = new GroupId(name);

        // Assert
        groupId.Value.Should().Be(name);
    }

    [Theory]
    [InlineData((string)null!)]
    [InlineData("")]
    public void GroupIdCantBeCreatedWithNullOrEmpty(string data)
    {
        // Arrange

        // Act
        var exception = Record.Exception(() => new GroupId(data));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact]
    public void GroupIdCantBeCreatedWithNameOfSpaces()
    {
        // Arrange
        var name = "     ";

        // Act
        var exception = Record.Exception(() => new GroupId(name));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentException>();
    }
}