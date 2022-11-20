namespace Models.Tests;

public class GroupLagResultTests
{
    [Fact]
    public void GroupLagResultCanBeCreated()
    {
        // Arrange
        var groupId = new GroupId("1");
        var lags = Enumerable.Empty<PartitionLag>();

        // Act
        var result = new GroupLagResult(groupId, lags);

        // Assert
        result.Group.Should().Be(groupId);
        (result.Lags == lags).Should().BeTrue();
    }

    [Fact]
    public void GroupLagResultCantBeCreatedWithNullLags()
    {
        // Arrange
        var groupId = new GroupId("1");

        // Act
        var exception = Record.Exception(() => new GroupLagResult(groupId, null!));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }
}
