using Newtonsoft.Json;
using OWP.NewLeagueSetting.Helper;
using OWP.NewLeagueSetting.Models;
using Xunit;

namespace OWP.NewLeagueSetting.UnitTests.Helper;

public class StandardizedLeagueInfoUnitTest
{
    [Fact]
    public void Execute_ConvertsExcelInfoToLeagueSettingInfo()
    {
        // Arrange
        var excelInfos = new List<LeagueSettingExcelInfo>
        {
            new LeagueSettingExcelInfo
            {
                LeagueGroup = "Group1",
                League = "League1",
                LstNote = "LstNote1",
                Remark2 = "Remark2",
                FTSpread = "10-20",
                WD_HDP = "5",
                WK_HDP = "3",
                WD_NO_HDP = "2",
                WK_NO_HDP = "1",
                LeagueGroupNote = "GroupNote",
                LeagueNote = "LeagueNote",
                LocalTvNote = "TvNote",
                RoundNote = "RoundNote",
                LeagueDivisionNote = "DivisionNote",
                RemarkNote = "RemarkNote",
                FTSpreadNote = "SpreadNote",
                WD_HDPNote = "WD_HDPNote",
                WD_NO_HDPNote = "WD_NO_HDPNote",
                WK_HDPNote = "WK_HDPNote",
                WK_NO_HDPNote = "WK_NO_HDPNote",
                LstNoteNote = "LstNoteNote",
                Remark2Note = "Remark2Note"
            }
        };

        // Act
        var result = StandardizedLeagueInfo.Execute(excelInfos);

        // Assert
        Assert.Single(result);
        var leagueInfo = result[0];
        Assert.Equal("Group1", leagueInfo.LeagueGroup);
        Assert.Equal("League1", leagueInfo.League);
        Assert.Equal("LstNote1", leagueInfo.LstNote);
        Assert.Equal("Remark2", leagueInfo.Remark2);
        Assert.Equal("10-20", leagueInfo.Spread_Note);
    }

    [Fact]
    public void BuildCommonInfo_SetsPropertiesCorrectly()
    {
        // Arrange
        var leagueInfo = new LeagueSettingInfo();
        var excelInfo = new LeagueSettingExcelInfo
        {
            LeagueGroup = "Group1",
            League = "League1",
            LstNote = "LstNote1",
            Remark2 = "Remark2"
        };

        // Act
        StandardizedLeagueInfo.BuildCommonInfo(leagueInfo, excelInfo);

        // Assert
        Assert.Equal("Group1", leagueInfo.LeagueGroup);
        Assert.Equal("League1", leagueInfo.League);
        Assert.Equal("LstNote1", leagueInfo.LstNote);
        Assert.Equal("Remark2", leagueInfo.Remark2);
    }

    [Fact]
    public void BuildCondition_SetsConditionCorrectly()
    {
        // Arrange
        var leagueInfo = new LeagueSettingInfo();
        var excelInfo = new LeagueSettingExcelInfo
        {
            LocalTv = "TV/Local",
            Round = "*Round1*/*Round2*",
            LeagueDivision = "*Division*",
            Remark = "Remark"
        };

        // Act
        StandardizedLeagueInfo.BuildCondition(leagueInfo, excelInfo);

        // Assert
        var condition = JsonConvert.DeserializeObject<LeagueSettingCondition>(leagueInfo.FullConditionDescription);
        Assert.True(condition.HasTv);
        Assert.Contains(new Condition("Contains", "Local"), condition.IpTv);
        Assert.Contains(new Condition("Contains", "Round1"), condition.Round);
        Assert.Contains(new Condition("Contains", "Round2"), condition.Round);
        Assert.Contains(new Condition("Contains", "Division"), condition.Division);
        Assert.Equal("Remark", condition.Remark);
    }

    private const decimal Epsilon = 0.00001m; // Tolerance for decimal comparisons

    [Theory]
    [InlineData("10-20", 0.1, 0.2, null)]   // Valid percentages
    [InlineData("50-30", 0.5, 0.3, null)]   // Valid percentages
    [InlineData("10%-20%", 0.1, 0.2, null)] // Valid percentages with '%'
    public void AnalyzeSpreadFT_ShouldReturnExpectedResults(string spreadSource, decimal? expectedHdp, decimal? expectedOu, string expectedNote)
    {
        // Act
        var result = StandardizedLeagueInfo.AnalyzeSpreadFT(spreadSource);

        // Assert
        Assert.Equal(expectedNote, result.spreadSource);

        Assert.True(AreApproximatelyEqual(expectedHdp, result.spreadHdp));
        Assert.True(AreApproximatelyEqual(expectedOu, result.spreadOu));
    }

    private static bool AreApproximatelyEqual(decimal? expected, decimal? actual)
    {
        if (expected.HasValue && actual.HasValue)
        {
            return Math.Abs(expected.Value - actual.Value) < Epsilon;
        }
        return !expected.HasValue && !actual.HasValue;
    }

    [Fact]
    public void AnalyzeSpreadFT()
    {
        // Act
        var result = StandardizedLeagueInfo.AnalyzeSpreadFT("10%-20%");

        // Assert
        Assert.Equal(null, result.spreadSource);

        Assert.True(AreApproximatelyEqual(0.1m, result.spreadHdp));
        Assert.True(AreApproximatelyEqual(0.2m, result.spreadOu));
    }
}
