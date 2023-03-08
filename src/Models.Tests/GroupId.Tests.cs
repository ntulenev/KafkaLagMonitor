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

    [Fact]
    public void GroupIdCantBeCreatedWithNull()
    {
        // Arrange

        // Act
        var exception = Record.Exception(() => new GroupId(null!));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact]
    public void GroupIdCantBeCreatedWithEmpty()
    {
        // Arrange

        // Act
        var exception = Record.Exception(() => new GroupId(""));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentException>();
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