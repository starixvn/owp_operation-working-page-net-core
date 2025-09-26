using System.Collections.Generic;
using OWP.NewLeagueSetting.Models;
using OWP.NewLeagueSetting.Service;
using Xunit;

namespace OWP.NewLeagueSetting.UnitTests.Service;

public class LeagueSettingChangeServiceUnitTest
{
    private readonly LeagueSettingChangeService _service;

    public LeagueSettingChangeServiceUnitTest()
    {
        _service = new LeagueSettingChangeService();
    }

    [Fact]
    public void LogProcessingChanges_WithIdenticalModels_ShouldReturnEmptyList()
    {
        // Arrange
        var before = CreateTestLeagueSettingChangeModel();
        var after = CreateTestLeagueSettingChangeModel();

        // Act
        var result = _service.LogProcessingChanges(before, after);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void LogProcessingChanges_WithDifferentLeagueNames_ShouldReturnOneChange()
    {
        // Arrange
        var before = CreateTestLeagueSettingChangeModel();
        var after = CreateTestLeagueSettingChangeModel();
        after.League = "Different League";

        // Act
        var result = _service.LogProcessingChanges(before, after);

        // Assert
        Assert.Single(result);
        Assert.Equal("League name", result[0].Field);
        Assert.Equal("Test League", result[0].OldValue);
        Assert.Equal("Different League", result[0].NewValue);
    }

    [Fact]
    public void LogProcessingChanges_WithDifferentSpread_ShouldReturnOneChange()
    {
        // Arrange
        var before = CreateTestLeagueSettingChangeModel();
        var after = CreateTestLeagueSettingChangeModel();
        after.Spread = "1.5";

        // Act
        var result = _service.LogProcessingChanges(before, after);

        // Assert
        Assert.Single(result);
        Assert.Equal("FT Spread", result[0].Field);
        Assert.Equal("1.0", result[0].OldValue);
        Assert.Equal("1.5", result[0].NewValue);
    }

    [Fact]
    public void LogProcessingChanges_WithDifferentHasTv_ShouldReturnOneChange()
    {
        // Arrange
        var before = CreateTestLeagueSettingChangeModel();
        var after = CreateTestLeagueSettingChangeModel();
        after.HasTv = true;

        // Act
        var result = _service.LogProcessingChanges(before, after);

        // Assert
        Assert.Single(result);
        Assert.Equal("Condition HasTv", result[0].Field);
        Assert.Equal("False", result[0].OldValue);
        Assert.Equal("True", result[0].NewValue);
    }

    [Fact]
    public void LogProcessingChanges_WithDifferentConditions_ShouldReturnChanges()
    {
        // Arrange
        var before = CreateTestLeagueSettingChangeModel();
        var after = CreateTestLeagueSettingChangeModel();
        after.Round = new List<Condition> { new Condition("Test", "Value") };

        // Act
        var result = _service.LogProcessingChanges(before, after);

        // Assert
        Assert.Single(result);
        Assert.Equal("Condition Round", result[0].Field);
        Assert.Equal("", result[0].OldValue);
        Assert.Equal("Test - Value", result[0].NewValue);
    }

    [Fact]
    public void LogProcessingChanges_WithMultipleChanges_ShouldReturnAllChanges()
    {
        // Arrange
        var before = CreateTestLeagueSettingChangeModel();
        var after = CreateTestLeagueSettingChangeModel();
        after.League = "Different League";
        after.Spread = "2.0";
        after.HasTv = true;
        after.NoOfferLive = true;

        // Act
        var result = _service.LogProcessingChanges(before, after);

        // Assert
        Assert.Equal(4, result.Count);
        Assert.Contains(result, r => r.Field == "League name");
        Assert.Contains(result, r => r.Field == "FT Spread");
        Assert.Contains(result, r => r.Field == "Condition HasTv");
        Assert.Contains(result, r => r.Field == "No OfferLive");
    }

    [Fact]
    public void LogProcessingChanges_WithNullValues_ShouldHandleCorrectly()
    {
        // Arrange
        var before = new LeagueSettingChangeModel();
        var after = CreateTestLeagueSettingChangeModel();

        // Act
        var result = _service.LogProcessingChanges(before, after);

        // Assert
        Assert.NotEmpty(result);
        Assert.All(result, r => Assert.Equal("", r.OldValue));
    }

    [Fact]
    public void AreValuesEqual_WithNullValues_ShouldReturnTrue()
    {
        // Act
        var result = LeagueSettingChangeService.AreValuesEqual(null, null);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void AreValuesEqual_WithOneNullValue_ShouldReturnFalse()
    {
        // Act
        var result = LeagueSettingChangeService.AreValuesEqual(null, "test");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void AreValuesEqual_WithEqualStrings_ShouldReturnTrue()
    {
        // Act
        var result = LeagueSettingChangeService.AreValuesEqual("test", "test");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void AreValuesEqual_WithDifferentStrings_ShouldReturnFalse()
    {
        // Act
        var result = LeagueSettingChangeService.AreValuesEqual("test1", "test2");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void AreValuesEqual_WithEmptyStrings_ShouldReturnTrue()
    {
        // Act
        var result = LeagueSettingChangeService.AreValuesEqual("", "");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void AreValuesEqual_WithEqualEnumerables_ShouldReturnTrue()
    {
        // Arrange
        var list1 = new List<Condition> { new Condition("Test", "Value") };
        var list2 = new List<Condition> { new Condition("Test", "Value") };

        // Act
        var result = LeagueSettingChangeService.AreValuesEqual(list1, list2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void AreValuesEqual_WithDifferentEnumerables_ShouldReturnFalse()
    {
        // Arrange
        var list1 = new List<Condition> { new Condition("Test", "Value1") };
        var list2 = new List<Condition> { new Condition("Test", "Value2") };

        // Act
        var result = LeagueSettingChangeService.AreValuesEqual(list1, list2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void FormatValue_WithNullValue_ShouldReturnEmptyString()
    {
        // Act
        var result = LeagueSettingChangeService.FormatValue(null);

        // Assert
        Assert.Equal("", result);
    }

    [Fact]
    public void FormatValue_WithEmptyString_ShouldReturnEmptyString()
    {
        // Act
        var result = LeagueSettingChangeService.FormatValue("");

        // Assert
        Assert.Equal("", result);
    }

    [Fact]
    public void FormatValue_WithString_ShouldReturnString()
    {
        // Act
        var result = LeagueSettingChangeService.FormatValue("test");

        // Assert
        Assert.Equal("test", result);
    }

    [Fact]
    public void FormatValue_WithEnumerables_ShouldReturnFormattedString()
    {
        // Arrange
        var conditions = new List<Condition> 
        { 
            new Condition("Test", "Value1"),
            new Condition("Test2", "Value2")
        };

        // Act
        var result = LeagueSettingChangeService.FormatValue(conditions);

        // Assert
        Assert.Contains("Test - Value1", result);
        Assert.Contains("Test2 - Value2", result);
    }

    [Fact]
    public void LogProcessingChanges_WithDifferentMaxbetValues_ShouldReturnChanges()
    {
        // Arrange
        var before = CreateTestLeagueSettingChangeModel();
        var after = CreateTestLeagueSettingChangeModel();
        after.MaxbetWD_HDP = "2000";
        after.MaxbetWK_HDP = "3000";

        // Act
        var result = _service.LogProcessingChanges(before, after);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, r => r.Field == "MB Weekday HDP");
        Assert.Contains(result, r => r.Field == "MB Weekend HDP");
    }

    [Fact]
    public void LogProcessingChanges_WithDifferentRemarkValues_ShouldReturnChanges()
    {
        // Arrange
        var before = CreateTestLeagueSettingChangeModel();
        var after = CreateTestLeagueSettingChangeModel();
        after.Remark = "New Remark";
        after.Remark2 = "New Remark 2";

        // Act
        var result = _service.LogProcessingChanges(before, after);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, r => r.Field == "Remark");
        Assert.Contains(result, r => r.Field == "Remark 2");
    }

    [Fact]
    public void LogProcessingChanges_WithDifferentIpTvConditions_ShouldReturnChanges()
    {
        // Arrange
        var before = CreateTestLeagueSettingChangeModel();
        var after = CreateTestLeagueSettingChangeModel();
        after.IpTv = new List<Condition> { new Condition("IpTv", "Test") };

        // Act
        var result = _service.LogProcessingChanges(before, after);

        // Assert
        Assert.Single(result);
        Assert.Equal("Condition IpTv", result[0].Field);
        Assert.Equal("", result[0].OldValue);
        Assert.Equal("IpTv - Test", result[0].NewValue);
    }

    [Fact]
    public void LogProcessingChanges_WithDifferentDivisionConditions_ShouldReturnChanges()
    {
        // Arrange
        var before = CreateTestLeagueSettingChangeModel();
        var after = CreateTestLeagueSettingChangeModel();
        after.Division = new List<Condition> { new Condition("Division", "A") };

        // Act
        var result = _service.LogProcessingChanges(before, after);

        // Assert
        Assert.Single(result);
        Assert.Equal("Condition Division", result[0].Field);
        Assert.Equal("", result[0].OldValue);
        Assert.Equal("Division - A", result[0].NewValue);
    }

    private static LeagueSettingChangeModel CreateTestLeagueSettingChangeModel()
    {
        return new LeagueSettingChangeModel
        {
            League = "Test League",
            LeagueGroup = "Test Group",
            Spread = "1.0",
            MaxbetWD_HDP = "1000",
            WD_NoOfHDP = "5",
            MaxbetWK_HDP = "2000",
            WK_NoOfHDP = "10",
            LstNote = "Test Note",
            Remark2 = "Test Remark 2",
            Comment = "Test Comment",
            HasTv = false,
            HasIpTv = false,
            IpTv = new List<Condition>(),
            Round = new List<Condition>(),
            Division = new List<Condition>(),
            Remark = "Test Remark",
            IsBlackList = false,
            NoOfferLive = false
        };
    }
}
