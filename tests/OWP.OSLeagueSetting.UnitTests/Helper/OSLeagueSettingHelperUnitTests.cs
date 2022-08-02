using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using OWP.OSLeagueSetting.Handlers;
using OWP.OSLeagueSetting.Helper;
using OWP.OSLeagueSetting.Models;
using OWP.OSLeagueSetting.Services;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.OSLeagueSetting.UnitTests.Helper;

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
            new() { Text = "1 line/2 line", Style = "offer-lines" },
            new() { Text = "1 Bet Types", Style = "bet-type" },
            new() { Text = "Manual Update Score", Style = "update-score" },
        };

    private List<SummaryDetail> GetExpectedSummaryRemark()
        => new()
        {
            new() { Text = "abc/cde", Style = "week-match" },
            new() { Text = "00:03 - 00:04", Style = "time-range" },
            new() { Text = "IPTV Only", Style = "Iptv-only" }
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
                },
        };
}