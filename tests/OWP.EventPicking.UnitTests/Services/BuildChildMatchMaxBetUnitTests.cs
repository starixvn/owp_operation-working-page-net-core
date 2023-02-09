namespace OWP.EventPicking.UnitTests.Services;

using EventPicking.Services;
using MaxbetSetting.Models;
using Models;
using Xunit;
using Xunit.Asserts.Compare;

public class BuildChildMatchMaxBetUnitTests
{
    private readonly BuildChildMatchMaxBetImp services;

    public BuildChildMatchMaxBetUnitTests()
    {
        services = new BuildChildMatchMaxBetImp();
    }

    [Fact]
    public void BuildChildMatchMaxbet()
    {
        var childMatches = MockChildMatches();
        var mainMatches = MockMainMatches();
        var maxbetSettings = new Dictionary<decimal, MaxBetSettingModel>
        {
            { 5000, new MaxBetSettingModel {
                FT_HDP = 5000,
                NoOfCorner_FT_HDP = 2000,
                NoOfCorner_FT_OU = 100,
                NextCorner_FT_HDP = 12000
            } },
            { 4000, new() {FT_HDP = 4000, NextCorner_FT_HDP = 400, NextBooking_FT_HDP = 100} },
            { 3000, new() { FT_HDP = 3000, TotalBooking_FT_HDP = 300, TotalBooking_FT_OU = 100} },
            { -1000, new() { FT_HDP = -1000, NextCorner_FT_HDP = 200, NextBooking_FT_HDP = 333 }}
        };

        services.Build(mainMatches, childMatches, maxbetSettings);

        DeepAssert.Equal(MockExpectedMatches().First().MaxBetHDP, childMatches.First().MaxBetHDP);
    }

    public List<EventPickingModel> MockChildMatches()
        => new()
        {
            new()
            {
                InfoSiteMatchId = 3333,
                MinorBettypeId = -3
            },
            new()
            {
                InfoSiteMatchId = 3333,
                MinorBettypeId = -4
            },
            new()
            {
                InfoSiteMatchId = 4444,
                MinorBettypeId = -6
            },
            new()
            {
                InfoSiteMatchId = 5555,
                MinorBettypeId = -7
            },
            new()
            {
                InfoSiteMatchId = 4444,
                MinorBettypeId = -99
            }
        };

    public List<EventPickingModel> MockMainMatches()
        => new()
        {
            new() { MaxBetHDP = null },
            new()
            {
                MaxBetHDPNumber = 5000,
                InfoSiteMatchId = 3333
            },
            new()
            {
                MaxBetHDPNumber = 3000,
                InfoSiteMatchId = 4444
            },
            new()
            {
                MaxBetHDPNumber = 100,
                InfoSiteMatchId = 5555
            }
        };

    public List<EventPickingModel> MockExpectedMatches()
        => new()
        {
            new()
            {
                InfoSiteMatchId = 3333,
                MinorBettypeId = -3,
                MaxBetHDP = "2K",
                MaxBetOU = "0.1K"
            },
            new()
            {
                InfoSiteMatchId = 3333,
                MinorBettypeId = -4,
                MaxBetHDP = "12K"
            },
            new()
            {
                InfoSiteMatchId = 4444,
                MinorBettypeId = -6,
                MaxBetHDP = "0.3K",
                MaxBetOU = "0.1K"
            },
            new()
            {
                InfoSiteMatchId = 5555,
                MinorBettypeId = -7,
                MaxBetHDP = "0.333K"
            },
            new()
            {
                InfoSiteMatchId = 4444,
                MinorBettypeId = -99,
                MaxBetHDP = ""
            }
        };
}