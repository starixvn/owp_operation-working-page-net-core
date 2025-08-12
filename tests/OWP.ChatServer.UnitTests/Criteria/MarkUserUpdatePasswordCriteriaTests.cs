using Fanex.Data.Repository;
using OWP.ChatServer.Criteria;
using Xunit;

namespace OWP.ChatServer.UnitTests.Criteria;

public class MarkUserUpdatePasswordCriteriaTests
{
    [Fact]
    public void GetSettingKey_ReturnsExpectedKey()
    {
        // Arrange
        var criteria = new MarkUserUpdatePasswordCriteria();

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("OWP_MarkUserUpdatePassword", result);
    }

    [Fact]
    public void IsValid_AlwaysReturnsTrue()
    {
        // Arrange
        var criteria = new MarkUserUpdatePasswordCriteria();

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void UserId_CanBeSetAndRetrieved()
    {
        // Arrange
        var criteria = new MarkUserUpdatePasswordCriteria();
        var expectedUserId = 123;

        // Act
        criteria.UserId = expectedUserId;

        // Assert
        Assert.Equal(expectedUserId, criteria.UserId);
    }

    [Fact]
    public void ImplementsINonQueryCommand()
    {
        // Arrange
        var criteria = new MarkUserUpdatePasswordCriteria();

        // Act & Assert
        Assert.IsAssignableFrom<INonQueryCommand>(criteria);
    }
}
