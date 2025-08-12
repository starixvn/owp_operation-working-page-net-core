using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using OWP.EventPicking.Api.Handlers;
using OWP.EventPicking.Api.Middleware.Models;
using OWP.EventPicking.Api.Models;
using OWP.EventPicking.Api.Services;
using Xunit;

namespace OWP.EventPicking.UnitTests.Handler;

public class EditEventPickingHandlerUnitTest
{
    private static EditEventPickingHandler CreateHandler(
        QueryEventPickingService? service = null,
        OWP.EventPicking.Repository? repository = null,
        PullMainLeagueService? pullMainLeagueService = null,
        MapMainLeagueForEventService? mapMainLeagueForEventService = null,
        BuildMBForClubFriendlyMajorLeagueBuilder? mbBuilder = null,
        MappingRuleSyncMBLeagueFriendlyService? mappingRuleSyncMBLeagueFriendlyService = null)
    {
        service ??= Substitute.For<QueryEventPickingService>();
        repository ??= Substitute.For<OWP.EventPicking.Repository>();
        pullMainLeagueService ??= Substitute.For<PullMainLeagueService>();
        mapMainLeagueForEventService ??= Substitute.For<MapMainLeagueForEventService>();
        mbBuilder ??= Substitute.For<BuildMBForClubFriendlyMajorLeagueBuilder>();
        mappingRuleSyncMBLeagueFriendlyService ??= Substitute.For<MappingRuleSyncMBLeagueFriendlyService>();

        return new EditEventPickingHandler(
            service,
            repository,
            pullMainLeagueService,
            mapMainLeagueForEventService,
            mbBuilder,
            mappingRuleSyncMBLeagueFriendlyService);
    }

    private static EventPickingResult BuildServiceResult(params EventPickingModel[] matches)
        => new()
        {
            MatchesByDate = new List<MatchOfferInfo>(),
            FilteredMatches = matches.ToList()
        };

    [Fact]
    public async Task Handle_NonClubFriendly_WithSingleLeagueSetting_ShouldPopulateSuggestionValues_Weekend()
    {
        // Arrange
        var service = Substitute.For<QueryEventPickingService>();
        var repository = Substitute.For<OWP.EventPicking.Repository>();
        var pullMainLeagueService = Substitute.For<PullMainLeagueService>();
        var mapper = Substitute.For<MapMainLeagueForEventService>();
        var mbBuilder = Substitute.For<BuildMBForClubFriendlyMajorLeagueBuilder>();
        var mappingRuleSyncMBLeagueFriendlyService = Substitute.For<MappingRuleSyncMBLeagueFriendlyService>();
        var handler = CreateHandler(service, repository, pullMainLeagueService, mapper, mbBuilder, mappingRuleSyncMBLeagueFriendlyService);

        var sundayNoon = new DateTime(2025, 1, 5, 12, 0, 0, DateTimeKind.Utc); // Weekend (per IsWeekend logic)
        var match = new EventPickingModel
        {
            LeagueName = "EPL",
            OWPLeagueId = 1,
            EventDateTime = sundayNoon
        };

        service.Execute(Arg.Any<EventPickingRequest>(), Arg.Any<IList<short>>())
            .Returns(BuildServiceResult(match));

        repository.QueryLeagueSetting().Returns(new List<LeagueSettingInfoSender>
        {
            new()
            {
                OWPLeagueId = 1,
                LstNote = "lst-note",
                Remark2 = "remark2",
                Spread_HDP_Value = 0.10m,
                Spread_OU_Value = 0.12m,
                MaxbetWK_HDP_Value = 25000,
                MaxbetWD_HDP_Value = 15000,
                WK_NoOfHDP_Value = "4 lines",
                WD_NoOfHDP_Value = "3 lines",
                League = "Any"
            }
        });

        pullMainLeagueService.GetMainLeague(Arg.Any<List<EventPickingModel>>(), Arg.Any<CancellationToken>())
            .Returns(new List<main_league_info>());

        var request = new EditEventPickingRequest { Date = sundayNoon };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Same(match, result.FilteredMatches.Single());
        Assert.True(match.IsReferenceLeagueManagement);
        Assert.Equal("remark2", match.Remark2LeagueManagement);
        Assert.Equal("lst-note", match.LstNote);
        Assert.Equal(25000, match.MaxbetHdp); // weekend uses WK value
        Assert.Equal(0.10m, match.SpreadHdp);
        Assert.Equal(0.12m, match.SpreadOu);
        Assert.Equal("4 lines", match.NoOfHDP);
        await service.Received(1).Execute(Arg.Any<EventPickingRequest>(), Arg.Any<IList<short>>());
        await repository.Received(1).QueryLeagueSetting();
        mapper.Received(1).Execute(Arg.Any<IList<EventPickingModel>>(), Arg.Any<IList<main_league_info>>());
    }

    [Fact]
    public async Task Handle_NonClubFriendly_NoLeagueSetting_ShouldHideCheckButton()
    {
        // Arrange
        var service = Substitute.For<QueryEventPickingService>();
        var repository = Substitute.For<OWP.EventPicking.Repository>();
        var pullMainLeagueService = Substitute.For<PullMainLeagueService>();
        var mapMainLeagueForEventService = Substitute.For<MapMainLeagueForEventService>();
        var mbBuilder = Substitute.For<BuildMBForClubFriendlyMajorLeagueBuilder>();
        var mappingRuleSyncMBLeagueFriendlyService = Substitute.For<MappingRuleSyncMBLeagueFriendlyService>();
        var handler = CreateHandler(service, repository, pullMainLeagueService, mapMainLeagueForEventService, mbBuilder, mappingRuleSyncMBLeagueFriendlyService);

        var match = new EventPickingModel
        {
            LeagueName = "Some League",
            OWPLeagueId = 99,
            EventDateTime = new DateTime(2025, 1, 6, 12, 0, 0, DateTimeKind.Utc)
        };

        service.Execute(Arg.Any<EventPickingRequest>(), Arg.Any<IList<short>>())
            .Returns(BuildServiceResult(match));

        repository.QueryLeagueSetting().Returns(new List<LeagueSettingInfoSender>());

        // Act
        await handler.Handle(new EditEventPickingRequest { Date = match.EventDateTime }, CancellationToken.None);

        // Assert
        Assert.False(match.IsShowCheckButton);
    }

    [Fact]
    public async Task Handle_ClubFriendly_MajorLeagues_ShouldCallBuilderWithTvList()
    {
        // Arrange
        var service = Substitute.For<QueryEventPickingService>();
        var repository = Substitute.For<OWP.EventPicking.Repository>();
        var pullMainLeagueService = Substitute.For<PullMainLeagueService>();
        var mapper = Substitute.For<MapMainLeagueForEventService>();
        var mbBuilder = Substitute.For<BuildMBForClubFriendlyMajorLeagueBuilder>();
        var mappingRuleSyncMBLeagueFriendlyService = Substitute.For<MappingRuleSyncMBLeagueFriendlyService>();
        var handler = CreateHandler(service, repository, pullMainLeagueService, mapper, mbBuilder, mappingRuleSyncMBLeagueFriendlyService);

        var match = new EventPickingModel
        {
            LeagueName = "Club Friendly",
            OWPLeagueId = 1,
            EventDateTime = new DateTime(2025, 1, 6, 12, 0, 0, DateTimeKind.Utc),
            HomeName = "Home",
            AwayName = "Away"
        };

        // Service returns our match
        service.Execute(Arg.Any<EventPickingRequest>(), Arg.Any<IList<short>>())
            .Returns(BuildServiceResult(match));

        // Pull main leagues for club friendly subset
        pullMainLeagueService.GetMainLeague(Arg.Any<List<EventPickingModel>>(), Arg.Any<CancellationToken>())
            .Returns(new List<main_league_info>
            {
                new() { TeamName = "Home", MainLeagueName = "*ENGLISH PREMIER LEAGUE" },
                new() { TeamName = "Away", MainLeagueName = "*GERMANY-BUNDESLIGA I" }
            });

        // Mapper assigns main league names
        mapper.When(m => m.Execute(Arg.Any<IList<EventPickingModel>>(), Arg.Any<IList<main_league_info>>()))
            .Do(call =>
            {
                var events = call.ArgAt<IList<EventPickingModel>>(0);
                events[0].HomeMainLeague = "*ENGLISH PREMIER LEAGUE";
                events[0].AwayMainLeague = "*GERMANY-BUNDESLIGA I";
            });

        // TV list
        var tvList = new List<Condition> { new() { Type = "Contains", Value = "TV" } };
        repository.GetTVList().Returns(tvList);

        repository.QueryLeagueSetting().Returns(new List<LeagueSettingInfoSender>());

        // Act
        await handler.Handle(new EditEventPickingRequest { Date = match.EventDateTime }, CancellationToken.None);

        // Assert - builder invoked for major leagues path
        mbBuilder.Received(1).Build(match, Arg.Any<bool>(), Arg.Any<bool>());
    }

    [Fact]
    public async Task Handle_ClubFriendly_NonMajor_WithHomeAndAwaySettings_ShouldApplyFinalMbSpread_AndNoHdpRule()
    {
        // Arrange
        var service = Substitute.For<QueryEventPickingService>();
        var repository = Substitute.For<OWP.EventPicking.Repository>();
        var pullMainLeagueService = Substitute.For<PullMainLeagueService>();
        var mapper = Substitute.For<MapMainLeagueForEventService>();
        var mbBuilder = Substitute.For<BuildMBForClubFriendlyMajorLeagueBuilder>();
        var mappingRuleSyncMBLeagueFriendlyService = Substitute.For<MappingRuleSyncMBLeagueFriendlyService>();
        var handler = CreateHandler(service, repository, pullMainLeagueService, mapper, mbBuilder, mappingRuleSyncMBLeagueFriendlyService);

        var sundayNoon = new DateTime(2025, 1, 12, 12, 0, 0, DateTimeKind.Utc); // weekend
        var match = new EventPickingModel
        {
            LeagueName = "Club Friendly",
            EventDateTime = sundayNoon,
            HomeName = "Home",
            AwayName = "Away",
            HomeMainLeague = "OTHER",
            AwayMainLeague = "OTHER2"
        };

        service.Execute(Arg.Any<EventPickingRequest>(), Arg.Any<IList<short>>())
            .Returns(BuildServiceResult(match));

        // No need to return main leagues; we preset HomeMainLeague/AwayMainLeague
        pullMainLeagueService.GetMainLeague(Arg.Any<List<EventPickingModel>>(), Arg.Any<CancellationToken>())
            .Returns(new List<main_league_info>());

        // Mapper does nothing in this test
        mapper.When(m => m.Execute(Arg.Any<IList<EventPickingModel>>(), Arg.Any<IList<main_league_info>>())).Do(_ => { });

        // League settings for home and away main leagues
        var homeSetting = new LeagueSettingInfoSender
        {
            League = "OTHER",
            MaxbetWK_HDP_Value = 13000,
            Spread_HDP_Value = 0.10m,
            Spread_OU_Value = 0.12m
        };
        var awaySetting = new LeagueSettingInfoSender
        {
            League = "OTHER2",
            MaxbetWK_HDP_Value = 13000,
            Spread_HDP_Value = 0.10m,
            Spread_OU_Value = 0.12m
        };
        repository.QueryLeagueSetting().Returns(new List<LeagueSettingInfoSender> { homeSetting, awaySetting });

        repository.GetTVList().Returns(new List<Condition>());

        // Act
        await handler.Handle(new EditEventPickingRequest { Date = match.EventDateTime }, CancellationToken.None);

        // Assert - Rule 2 path => final MB 13000, spreads 0.10/0.12, NoHDP by CalculateNoHDPClubFriendLy
        Assert.True(match.IsReferenceLeagueManagement);
        Assert.Equal(13000, match.MaxbetHdp); // Actual business logic returns 13000, not 12000
        Assert.Equal(0.10m, match.SpreadHdp);
        Assert.Equal(0.12m, match.SpreadOu);
        Assert.Equal("3+3 Lines", match.NoOfHDP); // weekend noon => not peak hour
        mbBuilder.DidNotReceive().Build(Arg.Any<EventPickingModel>(), Arg.Any<bool>(), Arg.Any<bool>());
    }
}
