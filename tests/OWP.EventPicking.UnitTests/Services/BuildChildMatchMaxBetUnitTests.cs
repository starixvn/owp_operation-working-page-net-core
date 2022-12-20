using MediatR;
using NSubstitute;
using OWP.EventPicking.Models;
using OWP.EventPicking.Services;
using OWP.MaxbetSetting.Handlers;
using OWP.MaxbetSetting.Models;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.EventPicking.UnitTests.Services;

public class BuildChildMatchMaxBetUnitTests
{
    private readonly IMediator mediator;
    private readonly BuildChildMatchMaxBetImp services;

    public BuildChildMatchMaxBetUnitTests()
    {
        mediator = Substitute.For<IMediator>();
        services = new BuildChildMatchMaxBetImp(mediator);
    }

    [Fact]
    public async Task BuildChildMatchMaxbet()
    {
        var childMatches = MockChildMatches();
        var mainMatches = MockMainMatches();
        var maxbetSettings = new List<MaxBetSettingModel>
        {
            new() {
                FT_HDP = 5000,
                NoOfCorner_FT_HDP = 2000,
                NoOfCorner_FT_OU = 100,
                NextCorner_FT_HDP = 12000
            },
            new() {FT_HDP = 4000, NextCorner_FT_HDP = 400, NextBooking_FT_HDP = 100},
            new() {FT_HDP = 3000, TotalBooking_FT_HDP = 300, TotalBooking_FT_OU = 100},
            new() {FT_HDP = -1000, NextCorner_FT_HDP = 200, NextBooking_FT_HDP = 333}
        };
        mediator.Send(Arg.Any<QueryMaxbetSettingRequest>()).Returns(maxbetSettings);

        await services.BuildChildMatchMaxbet(mainMatches, childMatches);

        DeepAssert.Equal(MockExpectedMatches().First().MaxBetHDP, childMatches.First().MaxBetHDP);
    }

    public List<EventPickingModel> MockChildMatches()
        => new List<EventPickingModel>
        {
            new()
            {
                InfoSiteMatchId = 3333,
                MinorBettypeId = -3,
            },
            new()
            {
                InfoSiteMatchId = 3333,
                MinorBettypeId = -4,
            },
            new()
            {
                InfoSiteMatchId = 4444,
                MinorBettypeId = -6,
            },
            new()
            {
                InfoSiteMatchId = 5555,
                MinorBettypeId = -7,
            },
            new()
            {
                InfoSiteMatchId = 4444,
                MinorBettypeId = -99,
            }
        };

    public List<EventPickingModel> MockMainMatches()
        => new List<EventPickingModel>
        {
            new() { MaxBetHDP = null },
            new()
            {
                MaxBetHDPNumber = 5000,
                InfoSiteMatchId = 3333,
            },
            new()
            {
                MaxBetHDPNumber = 3000,
                InfoSiteMatchId = 4444,
            },
            new()
            {
                MaxBetHDPNumber = 100,
                InfoSiteMatchId = 5555,
            }
        };

    public List<EventPickingModel> MockExpectedMatches()
        => new List<EventPickingModel>
        {
            new()
            {
                InfoSiteMatchId = 3333,
                MinorBettypeId = -3,
                MaxBetHDP = "2K",
                MaxBetOU = "0.1K",
            },
            new()
            {
                InfoSiteMatchId = 3333,
                MinorBettypeId = -4,
                MaxBetHDP = "12K",
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
                MaxBetHDP = "0.333K",
            },
            new()
            {
                InfoSiteMatchId = 4444,
                MinorBettypeId = -99,
                MaxBetHDP = ""
            }
        };
}