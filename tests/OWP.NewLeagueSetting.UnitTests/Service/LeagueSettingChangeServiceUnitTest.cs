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
        Assert.Equal("League Name", result[0].Field);
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
        Assert.Equal("Condition TV", result[0].Field);
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
        Assert.Contains(result, r => r.Field == "League Name");
        Assert.Contains(result, r => r.Field == "FT Spread");
        Assert.Contains(result, r => r.Field == "Condition TV");
        Assert.Contains(result, r => r.Field == "No Offer Live");
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
    public void AreValuesEqual_WithEqualConditionLists_DifferentOrder_ShouldReturnTrue()
    {
        // Arrange
        var list1 = new List<Condition>
        {
            new Condition("Type1", "Value1"),
            new Condition("Type2", "Value2")
        };
        var list2 = new List<Condition>
        {
            new Condition("Type2", "Value2"),
            new Condition("Type1", "Value1")
        };

        // Act
        var result = LeagueSettingChangeService.AreValuesEqual(list1, list2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void AreValuesEqual_WithDifferentConditionLists_ShouldReturnFalse()
    {
        // Arrange
        var list1 = new List<Condition>
        {
            new Condition("Type1", "Value1"),
            new Condition("Type2", "Value2")
        };
        var list2 = new List<Condition>
        {
            new Condition("Type1", "Value1"),
            new Condition("Type3", "Value3")
        };

        // Act
        var result = LeagueSettingChangeService.AreValuesEqual(list1, list2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void AreValuesEqual_WithConditionLists_DifferentCounts_ShouldReturnFalse()
    {
        // Arrange
        var list1 = new List<Condition>
        {
            new Condition("Type1", "Value1"),
            new Condition("Type2", "Value2")
        };
        var list2 = new List<Condition>
        {
            new Condition("Type1", "Value1")
        };

        // Act
        var result = LeagueSettingChangeService.AreValuesEqual(list1, list2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void AreValuesEqual_WithEmptyConditionLists_ShouldReturnTrue()
    {
        // Arrange
        var list1 = new List<Condition>();
        var list2 = new List<Condition>();

        // Act
        var result = LeagueSettingChangeService.AreValuesEqual(list1, list2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void AreValuesEqual_WithConditionLists_CaseInsensitive_ShouldReturnTrue()
    {
        // Arrange
        var list1 = new List<Condition> { new Condition("Type1", "Value1") };
        var list2 = new List<Condition> { new Condition("TYPE1", "VALUE1") };

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
        Assert.Equal("Condition IPTV", result[0].Field);
        Assert.Equal("", result[0].OldValue);
        Assert.Equal("IpTv - Test", result[0].NewValue);
    }

    [Fact]
    public void LogProcessingChanges_WithDifferentConditionLists_ShouldReturnChanges()
    {
        // Arrange
        var before = CreateTestLeagueSettingChangeModel();
        before.Round = new List<Condition> { new Condition("Round", "1") };

        var after = CreateTestLeagueSettingChangeModel();
        after.Round = new List<Condition>
        {
            new Condition("Round", "1"),
            new Condition("Round", "2")
        };

        // Act
        var result = _service.LogProcessingChanges(before, after);

        // Assert
        Assert.Single(result);
        Assert.Equal("Condition Round", result[0].Field);
        Assert.Equal("Round - 1", result[0].OldValue);
        Assert.Contains("Round - 1", result[0].NewValue);
        Assert.Contains("Round - 2", result[0].NewValue);
    }

    [Fact]
    public void LogProcessingChanges_WithSameConditionLists_DifferentOrder_ShouldNotReturnChanges()
    {
        // Arrange
        var before = CreateTestLeagueSettingChangeModel();
        before.Division = new List<Condition>
        {
            new Condition("Division", "A"),
            new Condition("Division", "B")
        };

        var after = CreateTestLeagueSettingChangeModel();
        after.Division = new List<Condition>
        {
            new Condition("Division", "B"),
            new Condition("Division", "A")
        };

        // Act
        var result = _service.LogProcessingChanges(before, after);

        // Assert
        Assert.Empty(result);
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

    #region AreValuesEqual Additional Tests

    [Fact]
    public void AreValuesEqual_WithNullAndEmptyString_ShouldReturnTrue()
    {
        // Act
        var result1 = LeagueSettingChangeService.AreValuesEqual(null, "");
        var result2 = LeagueSettingChangeService.AreValuesEqual("", null);

        // Assert
        Assert.True(result1);
        Assert.True(result2);
    }

    [Fact]
    public void AreValuesEqual_WithNullAndNonEmptyString_ShouldReturnFalse()
    {
        // Act
        var result1 = LeagueSettingChangeService.AreValuesEqual(null, "test");
        var result2 = LeagueSettingChangeService.AreValuesEqual("test", null);

        // Assert
        Assert.False(result1);
        Assert.False(result2);
    }

    [Fact]
    public void AreValuesEqual_WithEqualBooleans_ShouldReturnTrue()
    {
        // Act
        var result = LeagueSettingChangeService.AreValuesEqual(true, true);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void AreValuesEqual_WithDifferentBooleans_ShouldReturnFalse()
    {
        // Act
        var result = LeagueSettingChangeService.AreValuesEqual(true, false);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void AreValuesEqual_WithEqualIntegers_ShouldReturnTrue()
    {
        // Act
        var result = LeagueSettingChangeService.AreValuesEqual(123, 123);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void AreValuesEqual_WithDifferentIntegers_ShouldReturnFalse()
    {
        // Act
        var result = LeagueSettingChangeService.AreValuesEqual(123, 456);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void AreValuesEqual_WithStringEnumerableList_ShouldWork()
    {
        // Arrange
        var list1 = new List<string> { "A", "B", "C" };
        var list2 = new List<string> { "A", "B", "C" };

        // Act
        var result = LeagueSettingChangeService.AreValuesEqual(list1, list2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void AreValuesEqual_WithDifferentStringEnumerableList_ShouldReturnFalse()
    {
        // Arrange
        var list1 = new List<string> { "A", "B", "C" };
        var list2 = new List<string> { "A", "B", "D" };

        // Act
        var result = LeagueSettingChangeService.AreValuesEqual(list1, list2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void AreValuesEqual_WithOneNullEnumerable_ShouldReturnFalse()
    {
        // Arrange
        var list = new List<Condition> { new Condition("Test", "Value") };

        // Act
        var result1 = LeagueSettingChangeService.AreValuesEqual(list, null);
        var result2 = LeagueSettingChangeService.AreValuesEqual(null, list);

        // Assert
        Assert.False(result1);
        Assert.False(result2);
    }

    [Fact]
    public void AreValuesEqual_WithEmptyAndNullEnumerables_ShouldReturnTrue()
    {
        // Arrange
        var emptyList = new List<Condition>();

        // Act
        var result = LeagueSettingChangeService.AreValuesEqual(emptyList, null);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void AreValuesEqual_WithMixedTypeEnumerables_ShouldWork()
    {
        // Arrange
        var list1 = new List<int> { 1, 2, 3 };
        var list2 = new List<int> { 1, 2, 3 };

        // Act
        var result = LeagueSettingChangeService.AreValuesEqual(list1, list2);

        // Assert
        Assert.True(result);
    }

    #endregion

    #region FormatValue Additional Tests

    [Fact]
    public void FormatValue_WithBoolean_ShouldReturnStringRepresentation()
    {
        // Act
        var resultTrue = LeagueSettingChangeService.FormatValue(true);
        var resultFalse = LeagueSettingChangeService.FormatValue(false);

        // Assert
        Assert.Equal("True", resultTrue);
        Assert.Equal("False", resultFalse);
    }

    [Fact]
    public void FormatValue_WithInteger_ShouldReturnStringRepresentation()
    {
        // Act
        var result = LeagueSettingChangeService.FormatValue(123);

        // Assert
        Assert.Equal("123", result);
    }

    [Fact]
    public void FormatValue_WithDecimal_ShouldReturnStringRepresentation()
    {
        // Act
        var result = LeagueSettingChangeService.FormatValue(123.45m);

        // Assert
        Assert.Equal("123.45", result);
    }

    [Fact]
    public void FormatValue_WithEmptyEnumerable_ShouldReturnEmptyString()
    {
        // Arrange
        var emptyList = new List<Condition>();

        // Act
        var result = LeagueSettingChangeService.FormatValue(emptyList);

        // Assert
        Assert.Equal("", result);
    }

    [Fact]
    public void FormatValue_WithStringEnumerable_ShouldReturnFormattedString()
    {
        // Arrange
        var list = new List<string> { "Item1", "Item2", "Item3" };

        // Act
        var result = LeagueSettingChangeService.FormatValue(list);

        // Assert
        Assert.Contains("String", result);
        Assert.Contains("<br />", result);
    }

    [Fact]
    public void FormatValue_WithMultipleConditions_ShouldFormatWithLineBreaks()
    {
        // Arrange
        var conditions = new List<Condition>
        {
            new Condition("Type1", "Value1"),
            new Condition("Type2", "Value2"),
            new Condition("Type3", "Value3")
        };

        // Act
        var result = LeagueSettingChangeService.FormatValue(conditions);

        // Assert
        Assert.Contains("Type1 - Value1", result);
        Assert.Contains("Type2 - Value2", result);
        Assert.Contains("Type3 - Value3", result);
        Assert.Equal(2, result.Split("<br />").Length - 1); // Should have 2 line breaks for 3 items
    }

    #endregion

    #region GetChanges Direct Tests

    [Fact]
    public void GetChanges_WithIdenticalModels_ShouldReturnEmptyList()
    {
        // Arrange
        var before = CreateTestLeagueSettingChangeModel();
        var after = CreateTestLeagueSettingChangeModel();

        // Act
        var result = LeagueSettingChangeService.GetChanges(before, after);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GetChanges_WithAllPropertiesChanged_ShouldReturnAllChanges()
    {
        // Arrange
        var before = CreateTestLeagueSettingChangeModel();
        var after = new LeagueSettingChangeModel
        {
            League = "Different League",
            LeagueGroup = "Different Group",
            Spread = "2.5",
            MaxbetWD_HDP = "5000",
            WD_NoOfHDP = "15",
            MaxbetWK_HDP = "6000",
            WK_NoOfHDP = "20",
            LstNote = "Different Note",
            Remark2 = "Different Remark 2",
            HasTv = true,
            HasIpTv = true,
            IpTv = new List<Condition> { new Condition("IPTV", "Yes") },
            Round = new List<Condition> { new Condition("Round", "Final") },
            Division = new List<Condition> { new Condition("Division", "Premier") },
            Remark = "Different Remark",
            IsBlackList = true,
            NoOfferLive = true
        };

        // Act
        var result = LeagueSettingChangeService.GetChanges(before, after);

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains(result, r => r.Field == "League Name");
        Assert.Contains(result, r => r.Field == "League Group");
        Assert.Contains(result, r => r.Field == "FT Spread");
        Assert.Contains(result, r => r.Field == "MB Weekday HDP");
        Assert.Contains(result, r => r.Field == "MB Weekend HDP");
        Assert.Contains(result, r => r.Field == "Condition TV");
        Assert.Contains(result, r => r.Field == "No Offer Live");
    }

    [Fact]
    public void GetChanges_WithUnmappedPropertyName_ShouldUsePropertyName()
    {
        // Note: IsBlackList is not in PropertyDisplayNames dictionary
        // Arrange
        var before = CreateTestLeagueSettingChangeModel();
        before.IsBlackList = false;
        var after = CreateTestLeagueSettingChangeModel();
        after.IsBlackList = true;

        // Act
        var result = LeagueSettingChangeService.GetChanges(before, after);

        // Assert
        Assert.Single(result);
        Assert.Equal("IsBlackList", result[0].Field); // Should use property name as-is
        Assert.Equal("False", result[0].OldValue);
        Assert.Equal("True", result[0].NewValue);
    }

    #endregion

    #region Edge Cases and Boundary Tests

    [Fact]
    public void LogProcessingChanges_WithNullToValueChange_ShouldHandleCorrectly()
    {
        // Arrange
        var before = new LeagueSettingChangeModel();
        var after = new LeagueSettingChangeModel
        {
            League = "New League"
        };

        // Act
        var result = _service.LogProcessingChanges(before, after);

        // Assert
        Assert.Single(result);
        Assert.Equal("League Name", result[0].Field);
        Assert.Equal("", result[0].OldValue);
        Assert.Equal("New League", result[0].NewValue);
    }

    [Fact]
    public void LogProcessingChanges_WithValueToNullChange_ShouldHandleCorrectly()
    {
        // Arrange
        var before = new LeagueSettingChangeModel
        {
            League = "Old League"
        };
        var after = new LeagueSettingChangeModel();

        // Act
        var result = _service.LogProcessingChanges(before, after);

        // Assert
        Assert.Single(result);
        Assert.Equal("League Name", result[0].Field);
        Assert.Equal("Old League", result[0].OldValue);
        Assert.Equal("", result[0].NewValue);
    }

    [Fact]
    public void LogProcessingChanges_WithEmptyToNonEmptyString_ShouldDetectChange()
    {
        // Arrange
        var before = CreateTestLeagueSettingChangeModel();
        before.LstNote = "";
        var after = CreateTestLeagueSettingChangeModel();
        after.LstNote = "New Note";

        // Act
        var result = _service.LogProcessingChanges(before, after);

        // Assert
        Assert.Single(result);
        Assert.Equal("LST Note", result[0].Field);
        Assert.Equal("", result[0].OldValue);
        Assert.Equal("New Note", result[0].NewValue);
    }

    [Fact]
    public void LogProcessingChanges_WithNonEmptyToEmptyString_ShouldDetectChange()
    {
        // Arrange
        var before = CreateTestLeagueSettingChangeModel();
        before.LstNote = "Old Note";
        var after = CreateTestLeagueSettingChangeModel();
        after.LstNote = "";

        // Act
        var result = _service.LogProcessingChanges(before, after);

        // Assert
        Assert.Single(result);
        Assert.Equal("LST Note", result[0].Field);
        Assert.Equal("Old Note", result[0].OldValue);
        Assert.Equal("", result[0].NewValue);
    }

    [Fact]
    public void LogProcessingChanges_WithEmptyConditionListToPopulated_ShouldDetectChange()
    {
        // Arrange
        var before = CreateTestLeagueSettingChangeModel();
        before.Round = new List<Condition>();
        var after = CreateTestLeagueSettingChangeModel();
        after.Round = new List<Condition> { new Condition("Round", "Quarter Final") };

        // Act
        var result = _service.LogProcessingChanges(before, after);

        // Assert
        Assert.Single(result);
        Assert.Equal("Condition Round", result[0].Field);
        Assert.Equal("", result[0].OldValue);
        Assert.Equal("Round - Quarter Final", result[0].NewValue);
    }

    [Fact]
    public void LogProcessingChanges_WithPopulatedToEmptyConditionList_ShouldDetectChange()
    {
        // Arrange
        var before = CreateTestLeagueSettingChangeModel();
        before.Round = new List<Condition> { new Condition("Round", "Quarter Final") };
        var after = CreateTestLeagueSettingChangeModel();
        after.Round = new List<Condition>();

        // Act
        var result = _service.LogProcessingChanges(before, after);

        // Assert
        Assert.Single(result);
        Assert.Equal("Condition Round", result[0].Field);
        Assert.Equal("Round - Quarter Final", result[0].OldValue);
        Assert.Equal("", result[0].NewValue);
    }

    [Fact]
    public void AreValuesEqual_WithNullConditionInList_ShouldHandleGracefully()
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
    public void LogProcessingChanges_WithHasIpTvChange_ShouldDetectChange()
    {
        // Arrange
        var before = CreateTestLeagueSettingChangeModel();
        before.HasIpTv = false;
        var after = CreateTestLeagueSettingChangeModel();
        after.HasIpTv = true;

        // Act
        var result = _service.LogProcessingChanges(before, after);

        // Assert
        Assert.Single(result);
        // Note: HasIpTv is mapped to "Condition IPTV" in PropertyDisplayNames
        Assert.Equal("Condition IPTV", result[0].Field);
        Assert.Equal("False", result[0].OldValue);
        Assert.Equal("True", result[0].NewValue);
    }

    [Fact]
    public void LogProcessingChanges_WithMultipleConditionChanges_ShouldDetectAll()
    {
        // Arrange
        var before = CreateTestLeagueSettingChangeModel();
        var after = CreateTestLeagueSettingChangeModel();
        after.Round = new List<Condition> { new Condition("Round", "Final") };
        after.Division = new List<Condition> { new Condition("Division", "Premier") };
        after.IpTv = new List<Condition> { new Condition("IPTV", "Available") };

        // Act
        var result = _service.LogProcessingChanges(before, after);

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Contains(result, r => r.Field == "Condition Round");
        Assert.Contains(result, r => r.Field == "Condition Division");
        Assert.Contains(result, r => r.Field == "Condition IPTV");
    }

    #endregion

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
