using OWP.OSLeagueSetting.Helper;
using OWP.OSLeagueSetting.Models;
using Xunit;

namespace OWP.OSLeagueSetting.UnitTests.Helper;

using Xunit.Asserts.Compare;

public class OSLeagueSettingHelperUnitTests
{
    [Fact]
    public void CalculateSummaryInfo_Always_ListSummaryDetail()
    {
        var expected = GetExpectedSummaryInfos();

        var actual = OSLeagueSettingHelper.CalculateSummaryInfo(GetLeagueSettingInfosRaw()).ToList();

        DeepAssert.Equal(expected, actual);
    }

    [Fact]
    public void CalculateSummaryRemark_Always_ListSummaryDetail()
    {
        var expected = GetExpectedSummaryRemark();

        var actual = OSLeagueSettingHelper.CalculateSummaryRemark(GetLeagueSettingInfosRaw()).ToList();

        DeepAssert.Equal(expected, actual);
    }

    private List<SummaryDetail> GetExpectedSummaryInfos()
        =>
        [
            new SummaryDetail { Text = "1 line/2 line", Style = "offer-lines" },
            new SummaryDetail { Text = "1 Bet Types", Style = "bet-type" },
            new SummaryDetail { Text = "Manual Update Score", Style = "update-score" }
        ];

    private List<SummaryDetail> GetExpectedSummaryRemark()
        =>
        [
            new SummaryDetail { Text = "abc/cde", Style = "week-match" },
            new SummaryDetail { Text = "00:03 - 00:04", Style = "time-range" },
            new SummaryDetail { Text = "IPTV Only", Style = "Iptv-only" }
        ];

    private IList<LeagueSettingInfo> GetLeagueSettingInfosRaw()
        => new List<LeagueSettingInfo>
        {
            new()
            {
                LeagueSettingId = 1,
                OSLeagueName = "C1",
                BetType = "HDP",
                KOFromTime = new TimeSpan(0,3,20),
                KOToTime = new TimeSpan(0,4,20),
                OfferLines = "1 line",
                IsManualUpdateScore = true,
                WeekMatch = "abc",
                IsIptvOnly = true
            },
            new()
            {
                LeagueSettingId = 1,
                OSLeagueName = "C1",
                KOFromTime = new TimeSpan(0,3,20),
                KOToTime = new TimeSpan(0,4,20),
                OfferLines = "2 line",
                WeekMatch = "cde"
            }
        };

    [Fact]
    public void CalculateSummaryExportFirst_WithValues_ReturnsFormattedString()
    {
        // Arrange
        var leagueSettingInfo = new LeagueSettingInfo
        {
            MaxBetHDP = 100,
            MaxBetOU = 200
        };

        // Act
        var result = OSLeagueSettingHelper.CalculateSummaryExportFirst(leagueSettingInfo);

        // Assert
        Assert.Equal("{HDP 100}{OU 200}", result);
    }

    [Fact]
    public void CalculateSummaryExportFirst_WithoutValues_ReturnsEmptyString()
    {
        // Arrange
        var leagueSettingInfo = new LeagueSettingInfo();

        // Act
        var result = OSLeagueSettingHelper.CalculateSummaryExportFirst(leagueSettingInfo);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void CalculateSummaryExportSecond_WithValues_ReturnsFormattedString()
    {
        // Arrange
        var offerLines = "OfferLines1/OfferLines2";
        var bettypes = "BetType1/BetType2";
        var betDelays = "BetDelay1/BetDelay2";

        // Act
        var result = OSLeagueSettingHelper.CalculateSummaryExportSecond(offerLines, bettypes, betDelays);

        // Assert
        Assert.Equal("(OfferLines1/OfferLines2)(BetType1/BetType2)(BetDelay1/BetDelay2)", result);
    }

    [Fact]
    public void CalculateSummaryExportSecond_WithoutValues_ReturnsEmptyString()
    {
        // Arrange
        var offerLines = "";
        var bettypes = "";
        var betDelays = "";

        // Act
        var result = OSLeagueSettingHelper.CalculateSummaryExportSecond(offerLines, bettypes, betDelays);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void CalculateRemarkExport_WithValues_ReturnsFormattedString()
    {
        // Arrange
        var leagueSettingInfo = new LeagueSettingInfo
        {
            IsManualUpdateScore = true,
            WeekMatch = "Week1",
            Provider = "Provider1",
            KOFromTime = new TimeSpan(14, 0, 0),
            KOToTime = new TimeSpan(16, 0, 0),
            IsIptvOnly = true,
            Remark = "Remark1"
        };
        var leagueSettingInfos = new List<LeagueSettingInfo> { leagueSettingInfo };

        // Act
        var result = OSLeagueSettingHelper.CalculateRemarkExport(leagueSettingInfos);

        // Assert
        Assert.Equal("(Manual Update Score)(End Of Preseason before Week1 Week Match)(Provider1)(14:00 - 16:00)(IPTV Only)(Remark1)", result);
    }

    [Fact]
    public void CalculateRemarkExport_WithoutValues_ReturnsEmptyString()
    {
        // Arrange
        var leagueSettingInfo = new LeagueSettingInfo();
        var leagueSettingInfos = new List<LeagueSettingInfo> { leagueSettingInfo };

        // Act
        var result = OSLeagueSettingHelper.CalculateRemarkExport(leagueSettingInfos);

        // Assert
        Assert.Equal(string.Empty, result);
    }
}