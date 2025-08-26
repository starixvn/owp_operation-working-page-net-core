using OWP.NewLeagueSetting.Criteria;
using Xunit;

namespace OWP.NewLeagueSetting.UnitTests.Criteria;

public class RemoveSettingCriteriaUnitTest
{
    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Arrange
        var criteria = new RemoveSettingCriteria();

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("OWP_NewLeagueSetting_RemoveLeagueSetting", result);
    }

    [Fact]
    public void IsValid_WithPositiveLeagueSettingIdAndActorId_ShouldReturnTrue()
    {
        // Arrange
        var criteria = new RemoveSettingCriteria
        {
            LeagueSettingId = 123,
            ActorId = 456
        };

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValid_WithZeroLeagueSettingId_ShouldReturnFalse()
    {
        // Arrange
        var criteria = new RemoveSettingCriteria
        {
            LeagueSettingId = 0,
            ActorId = 123
        };

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_WithNegativeLeagueSettingId_ShouldReturnFalse()
    {
        // Arrange
        var criteria = new RemoveSettingCriteria
        {
            LeagueSettingId = -1,
            ActorId = 123
        };

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_WithLargePositiveLeagueSettingId_ShouldReturnTrue()
    {
        // Arrange
        var criteria = new RemoveSettingCriteria
        {
            LeagueSettingId = int.MaxValue,
            ActorId = 123
        };

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Properties_ShouldBeAccessibleAndSettable()
    {
        // Arrange
        var criteria = new RemoveSettingCriteria();

        // Act & Assert
        criteria.LeagueSettingId = 789;
        Assert.Equal(789, criteria.LeagueSettingId);

        criteria.ActorId = 123;
        Assert.Equal(123, criteria.ActorId);
    }

    [Fact]
    public void Inheritance_ShouldExtendNonQueryCommand()
    {
        // Arrange & Act
        var criteria = new RemoveSettingCriteria();

        // Assert
        Assert.IsAssignableFrom<Fanex.Data.Repository.NonQueryCommand>(criteria);
    }

    [Fact]
    public void DefaultValues_ShouldBeSetCorrectly()
    {
        // Arrange
        var criteria = new RemoveSettingCriteria();

        // Assert
        Assert.Equal(0, criteria.LeagueSettingId);
        Assert.Equal(0, criteria.ActorId);
    }

    [Fact]
    public void Constructor_WithParameters_ShouldSetValues()
    {
        // Arrange
        var leagueSettingId = 456;
        var actorId = 789;

        // Act
        var criteria = new RemoveSettingCriteria
        {
            LeagueSettingId = leagueSettingId,
            ActorId = actorId
        };

        // Assert
        Assert.Equal(leagueSettingId, criteria.LeagueSettingId);
        Assert.Equal(actorId, criteria.ActorId);
    }

    [Fact]
    public void IsValid_WithEdgeCases_ShouldHandleCorrectly()
    {
        // Arrange & Act
        var criteria1 = new RemoveSettingCriteria { LeagueSettingId = 1, ActorId = 123 };
        var criteria2 = new RemoveSettingCriteria { LeagueSettingId = 999999, ActorId = 456 };
        var criteria3 = new RemoveSettingCriteria { LeagueSettingId = -999999, ActorId = 789 };

        // Assert
        Assert.True(criteria1.IsValid());
        Assert.True(criteria2.IsValid());
        Assert.False(criteria3.IsValid());
    }
}
