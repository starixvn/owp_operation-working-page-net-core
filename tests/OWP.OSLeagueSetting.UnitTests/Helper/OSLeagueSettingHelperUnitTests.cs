namespace OWP.OSLeagueSetting.UnitTests.Helper;

using Models;
using OSLeagueSetting.Helper;
using Xunit;
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
        => new()
        {
            new SummaryDetail { Text = "1 line/2 line", Style = "offer-lines" },
            new SummaryDetail { Text = "1 Bet Types", Style = "bet-type" },
            new SummaryDetail { Text = "Manual Update Score", Style = "update-score" }
        };

    private List<SummaryDetail> GetExpectedSummaryRemark()
        => new()
        {
            new SummaryDetail { Text = "abc/cde", Style = "week-match" },
            new SummaryDetail { Text = "00:03 - 00:04", Style = "time-range" },
            new SummaryDetail { Text = "IPTV Only", Style = "Iptv-only" }
        };

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
}