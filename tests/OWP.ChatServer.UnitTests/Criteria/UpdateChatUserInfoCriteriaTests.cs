using Fanex.Data.Repository;
using OWP.ChatServer.Criteria;
using Xunit;

namespace OWP.ChatServer.UnitTests.Criteria;

public class UpdateChatUserInfoCriteriaTests
{
    [Fact]
    public void GetSettingKey_ReturnsExpectedKey()
    {
        // Arrange
        var criteria = new UpdateChatUserInfoCriteria();

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("OWP_UpdateChatUserId", result);
    }

    [Fact]
    public void IsValid_AlwaysReturnsTrue()
    {
        // Arrange
        var criteria = new UpdateChatUserInfoCriteria();

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void UserJson_CanBeSetAndRetrieved()
    {
        // Arrange
        var criteria = new UpdateChatUserInfoCriteria();
        var expectedUserJson = "[{\"UserId\":123,\"ChatUserId\":\"chat123\"}]";

        // Act
        criteria.UserJson = expectedUserJson;

        // Assert
        Assert.Equal(expectedUserJson, criteria.UserJson);
    }

    [Fact]
    public void UserJson_CanHandleEmptyString()
    {
        // Arrange
        var criteria = new UpdateChatUserInfoCriteria();
        var expectedUserJson = string.Empty;

        // Act
        criteria.UserJson = expectedUserJson;

        // Assert
        Assert.Equal(expectedUserJson, criteria.UserJson);
    }

    [Fact]
    public void UserJson_CanHandleNull()
    {
        // Arrange
        var criteria = new UpdateChatUserInfoCriteria();

        // Act
        criteria.UserJson = null!;

        // Assert
        Assert.Null(criteria.UserJson);
    }

    [Fact]
    public void ImplementsINonQueryCommand()
    {
        // Arrange
        var criteria = new UpdateChatUserInfoCriteria();

        // Act & Assert
        Assert.IsAssignableFrom<INonQueryCommand>(criteria);
    }
}
