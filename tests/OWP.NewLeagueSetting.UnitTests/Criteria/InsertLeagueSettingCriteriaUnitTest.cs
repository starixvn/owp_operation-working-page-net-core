using OWP.NewLeagueSetting.Criteria;
using Xunit;

namespace OWP.NewLeagueSetting.UnitTests.Criteria;

public class InsertLeagueSettingCriteriaUnitTest
{
    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Arrange
        var criteria = new InsertLeagueSettingCriteria();

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("OWP_NewLeagueSetting_InsertLeagueSetting", result);
    }

    [Fact]
    public void IsValid_WithLeagueName_ShouldReturnTrue()
    {
        // Arrange
        var criteria = new InsertLeagueSettingCriteria
        {
            League = "Test League"
        };

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValid_WithNullLeagueName_ShouldReturnFalse()
    {
        // Arrange
        var criteria = new InsertLeagueSettingCriteria
        {
            League = null
        };

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_WithEmptyLeagueName_ShouldReturnTrue()
    {
        // Arrange
        var criteria = new InsertLeagueSettingCriteria
        {
            League = ""
        };

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValid_WithWhitespaceLeagueName_ShouldReturnTrue()
    {
        // Arrange
        var criteria = new InsertLeagueSettingCriteria
        {
            League = "   "
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
        var criteria = new InsertLeagueSettingCriteria();

        // Act & Assert
        criteria.LeagueSettingId = 789;
        Assert.Equal(789, criteria.LeagueSettingId);

        criteria.ActorId = 123;
        Assert.Equal(123, criteria.ActorId);

        criteria.LeagueGroupId = 999;
        Assert.Equal(999, criteria.LeagueGroupId);

        criteria.Spread_HDP_Value = 0.05m;
        Assert.Equal(0.05m, criteria.Spread_HDP_Value);

        criteria.Spread_OU_Value = 0.10m;
        Assert.Equal(0.10m, criteria.Spread_OU_Value);

        criteria.Spread_Note = "Test Spread Note";
        Assert.Equal("Test Spread Note", criteria.Spread_Note);

        criteria.MaxbetWD_HDP_Value = 1000m;
        Assert.Equal(1000m, criteria.MaxbetWD_HDP_Value);

        criteria.MaxbetWD_HDP_Note = "Test Maxbet WD Note";
        Assert.Equal("Test Maxbet WD Note", criteria.MaxbetWD_HDP_Note);

        criteria.WD_NoOfHDP_Value = "5";
        Assert.Equal("5", criteria.WD_NoOfHDP_Value);

        criteria.WD_NoOfHDP_Note = "Test WD NoOfHDP Note";
        Assert.Equal("Test WD NoOfHDP Note", criteria.WD_NoOfHDP_Note);

        criteria.MaxbetWK_HDP_Value = 2000m;
        Assert.Equal(2000m, criteria.MaxbetWK_HDP_Value);

        criteria.MaxbetWK_HDP_Note = "Test Maxbet WK Note";
        Assert.Equal("Test Maxbet WK Note", criteria.MaxbetWK_HDP_Note);

        criteria.WK_NoOfHDP_Value = "10";
        Assert.Equal("10", criteria.WK_NoOfHDP_Value);

        criteria.WK_NoOfHDP_Note = "Test WK NoOfHDP Note";
        Assert.Equal("Test WK NoOfHDP Note", criteria.WK_NoOfHDP_Note);

        criteria.LstNote = "Test Lst Note";
        Assert.Equal("Test Lst Note", criteria.LstNote);

        criteria.Remark2 = "Test Remark2";
        Assert.Equal("Test Remark2", criteria.Remark2);

        criteria.Comment = "Test Comment";
        Assert.Equal("Test Comment", criteria.Comment);

        criteria.FullConditionDescription = "Test Full Condition Description";
        Assert.Equal("Test Full Condition Description", criteria.FullConditionDescription);

        criteria.NoOfferLive = 1;
        Assert.Equal(1, criteria.NoOfferLive);

        criteria.League = "Test League";
        Assert.Equal("Test League", criteria.League);
    }

    [Fact]
    public void Inheritance_ShouldExtendInsertOrUpdateSettingCriteria()
    {
        // Arrange & Act
        var criteria = new InsertLeagueSettingCriteria();

        // Assert
        Assert.IsAssignableFrom<InsertOrUpdateSettingCriteria>(criteria);
    }

    [Fact]
    public void DefaultValues_ShouldBeSetCorrectly()
    {
        // Arrange
        var criteria = new InsertLeagueSettingCriteria();

        // Assert
        Assert.Equal(0, criteria.LeagueSettingId);
        Assert.Equal(0, criteria.ActorId);
        Assert.Null(criteria.LeagueGroupId);
        Assert.Null(criteria.Spread_HDP_Value);
        Assert.Null(criteria.Spread_OU_Value);
        Assert.Null(criteria.Spread_Note);
        Assert.Null(criteria.MaxbetWD_HDP_Value);
        Assert.Null(criteria.MaxbetWD_HDP_Note);
        Assert.Null(criteria.WD_NoOfHDP_Value);
        Assert.Null(criteria.WD_NoOfHDP_Note);
        Assert.Null(criteria.MaxbetWK_HDP_Value);
        Assert.Null(criteria.MaxbetWK_HDP_Note);
        Assert.Null(criteria.WK_NoOfHDP_Value);
        Assert.Null(criteria.WK_NoOfHDP_Note);
        Assert.Null(criteria.LstNote);
        Assert.Null(criteria.Remark2);
        Assert.Null(criteria.Comment);
        Assert.Null(criteria.FullConditionDescription);
        Assert.Equal(0, criteria.NoOfferLive);
        Assert.Null(criteria.League);
    }
}
