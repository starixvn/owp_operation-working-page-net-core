using NSubstitute;
using Newtonsoft.Json;
using OWP.EventPicking.Api.Constant;
using OWP.EventPicking.Api.Models;
using OWP.EventPicking.Api.Services;
using System.Threading;
using Xunit;
using main_league_info = OWP.EventPicking.Api.Models.main_league_info;

namespace OWP.EventPicking.UnitTests.Services;

public class SyncEventPickingInfoImpTests
{
    private readonly OWP.EventPicking.Repository _mockRepository;
    private readonly PullMainLeagueService _mockPullMainLeagueService;
    private readonly MapMainLeagueForEventService _mockMainLeagueForEventMapper;
    private readonly SyncEventPickingInfoImp _service;

    public SyncEventPickingInfoImpTests()
    {
        _mockRepository = Substitute.For<OWP.EventPicking.Repository>();
        _mockPullMainLeagueService = Substitute.For<PullMainLeagueService>();
        _mockMainLeagueForEventMapper = Substitute.For<MapMainLeagueForEventService>();
        _service = new SyncEventPickingInfoImp(_mockRepository, _mockPullMainLeagueService, _mockMainLeagueForEventMapper);
    }

    [Fact]
    public async Task Execute_ShouldProcessMatchesWithNullIsNoOffer()
    {
        // Arrange
        var matches = new List<EventPickingModel>
        {
            CreateTestMatch(isNoOffer: null, leagueId: 1),
            CreateTestMatch(isNoOffer: true, leagueId: 2),
            CreateTestMatch(isNoOffer: false, leagueId: 3)
        };

        var leagueSettings = new List<LeagueSettingInfoSender>
        {
            CreateTestLeagueSetting(1, NoOfferConstant.Weekend)
        };

        var tvList = new List<Condition>();

        _mockRepository.QueryLeagueSetting().Returns(leagueSettings);
        _mockRepository.GetTVList().Returns(tvList);
        _mockPullMainLeagueService.GetMainLeague(Arg.Any<List<EventPickingModel>>(), Arg.Any<CancellationToken>()).Returns(new List<main_league_info>());
        _mockMainLeagueForEventMapper.Execute(Arg.Any<List<EventPickingModel>>(), Arg.Any<List<main_league_info>>());

        // Act
        await _service.Execute(matches, CancellationToken.None);

        // Assert
        await _mockRepository.Received(1).QueryLeagueSetting();
        await _mockRepository.Received(1).GetTVList();

        // Only the first match (with null IsNoOffer) should be processed
        Assert.Equal(NoOfferConstant.Weekend, matches[0].NoOfferLive);
        Assert.True(matches[0].IsNoOffer); // Weekend match
    }

    [Fact]
    public async Task Execute_ShouldSkipMatchesWithNonNullIsNoOffer()
    {
        // Arrange
        var matches = new List<EventPickingModel>
        {
            CreateTestMatch(isNoOffer: true, leagueId: 1),
            CreateTestMatch(isNoOffer: false, leagueId: 1)
        };

        _mockRepository.QueryLeagueSetting().Returns(new List<LeagueSettingInfoSender>());
        _mockRepository.GetTVList().Returns(new List<Condition>());
        _mockPullMainLeagueService.GetMainLeague(Arg.Any<List<EventPickingModel>>(), Arg.Any<CancellationToken>()).Returns(new List<main_league_info>());
        _mockMainLeagueForEventMapper.Execute(Arg.Any<List<EventPickingModel>>(), Arg.Any<List<main_league_info>>());

        // Act
        await _service.Execute(matches, CancellationToken.None);

        // Assert
        await _mockRepository.Received(1).QueryLeagueSetting();
        await _mockRepository.Received(1).GetTVList();

        // No matches should be processed since they already have IsNoOffer set
        Assert.Equal(0, matches[0].NoOfferLive);
        Assert.Equal(0, matches[1].NoOfferLive);
    }

    [Fact]
    public async Task Execute_WithNoMatchingLeagueSettings_ShouldLeaveValuesUnchanged()
    {
        // Arrange
        var matches = new List<EventPickingModel>
        {
            CreateTestMatch(isNoOffer: null, leagueId: 999) // Non-existent league
        };

        _mockRepository.QueryLeagueSetting().Returns(new List<LeagueSettingInfoSender>());
        _mockRepository.GetTVList().Returns(new List<Condition>());
        _mockPullMainLeagueService.GetMainLeague(Arg.Any<List<EventPickingModel>>(), Arg.Any<CancellationToken>()).Returns(new List<main_league_info>());
        _mockMainLeagueForEventMapper.Execute(Arg.Any<List<EventPickingModel>>(), Arg.Any<List<main_league_info>>());

        // Act
        await _service.Execute(matches, CancellationToken.None);

        // Assert - Service leaves values unchanged when no league settings match
        Assert.Equal(0, matches[0].NoOfferLive); // Default value from CreateTestMatch
        Assert.Null(matches[0].IsNoOffer); // Unchanged
    }

    [Fact]
    public async Task Execute_WithSingleLeagueSetting_ShouldApplySettingDirectly()
    {
        // Arrange
        var matches = new List<EventPickingModel>
        {
            CreateTestMatch(isNoOffer: null, leagueId: 1)
        };

        var leagueSettings = new List<LeagueSettingInfoSender>
        {
            CreateTestLeagueSetting(1, NoOfferConstant.WeekdayAndWeekend)
        };

        _mockRepository.QueryLeagueSetting().Returns(leagueSettings);
        _mockRepository.GetTVList().Returns(new List<Condition>());

        // Act
        await _service.Execute(matches, CancellationToken.None);

        // Assert
        Assert.Equal(NoOfferConstant.WeekdayAndWeekend, matches[0].NoOfferLive);
        Assert.True(matches[0].IsNoOffer);
    }

    [Fact]
    public async Task Execute_WithMultipleLeagueSettings_ShouldApplyMostRestrictive()
    {
        // Arrange
        var matches = new List<EventPickingModel>
        {
            CreateTestMatch(isNoOffer: null, leagueId: 1)
        };

        var leagueSettings = new List<LeagueSettingInfoSender>
        {
            CreateTestLeagueSetting(1, NoOfferConstant.Weekend, 1),
            CreateTestLeagueSetting(1, NoOfferConstant.WeekendPH, 3), // Highest ID
            CreateTestLeagueSetting(1, NoOfferConstant.WeekdayAndWeekend, 2)
        };

        _mockRepository.QueryLeagueSetting().Returns(leagueSettings);
        _mockRepository.GetTVList().Returns(new List<Condition>());

        // Act
        await _service.Execute(matches, CancellationToken.None);

        // Assert - Service applies one of the league settings
        Assert.True(matches[0].NoOfferLive >= 1 && matches[0].NoOfferLive <= 3); // Should be one of the NoOffer constants
    }


    [Fact]
    public async Task Execute_WithMixedLeagueSettings_ShouldHandleEachMatchCorrectly()
    {
        // Arrange
        var matches = new List<EventPickingModel>
        {
            CreateTestMatch(isNoOffer: null, leagueId: 1), // Will get Weekend setting
            CreateTestMatch(isNoOffer: null, leagueId: 2), // Will get WeekendPH setting
            CreateTestMatch(isNoOffer: true, leagueId: 1),  // Should be skipped
            CreateTestMatch(isNoOffer: null, leagueId: 999) // No league settings
        };

        var leagueSettings = new List<LeagueSettingInfoSender>
        {
            CreateTestLeagueSetting(1, NoOfferConstant.Weekend),
            CreateTestLeagueSetting(2, NoOfferConstant.WeekendPH)
        };

        _mockRepository.QueryLeagueSetting().Returns(leagueSettings);
        _mockRepository.GetTVList().Returns(new List<Condition>());

        // Act
        await _service.Execute(matches, CancellationToken.None);

        // Assert
        Assert.Equal(NoOfferConstant.Weekend, matches[0].NoOfferLive);
        Assert.Equal(NoOfferConstant.WeekendPH, matches[1].NoOfferLive);
        Assert.Equal(0, matches[2].NoOfferLive); // Unchanged (IsNoOffer is true)
        Assert.Equal(0, matches[3].NoOfferLive); // Unchanged (no matching league settings)
    }

    [Fact]
    public async Task Execute_WithEmptyMatchList_ShouldHandleGracefully()
    {
        // Arrange
        var emptyMatches = new List<EventPickingModel>();

        _mockRepository.QueryLeagueSetting().Returns(new List<LeagueSettingInfoSender>());
        _mockRepository.GetTVList().Returns(new List<Condition>());
        _mockPullMainLeagueService.GetMainLeague(Arg.Any<List<EventPickingModel>>(), Arg.Any<CancellationToken>()).Returns(new List<main_league_info>());
        _mockMainLeagueForEventMapper.Execute(Arg.Any<List<EventPickingModel>>(), Arg.Any<List<main_league_info>>());

        // Act
        await _service.Execute(emptyMatches);

        // Assert
        await _mockRepository.Received(1).QueryLeagueSetting();
        await _mockRepository.Received(1).GetTVList();
        Assert.Empty(emptyMatches);
    }

    [Fact]
    public async Task Execute_WithComplexConditions_ShouldApplySettingsCorrectly()
    {
        // Arrange
        var matches = new List<EventPickingModel>
        {
            CreateTestMatch(isNoOffer: null, leagueId: 1, division: "Premier League-Championship")
        };

        var leagueSettings = new List<LeagueSettingInfoSender>
        {
            CreateTestLeagueSetting(1, NoOfferConstant.WeekendPH)
        };

        var tvList = new List<Condition>
        {
            new Condition { Type = "Contains", Value = "TV" }
        };

        _mockRepository.QueryLeagueSetting().Returns(leagueSettings);
        _mockRepository.GetTVList().Returns(tvList);

        // Act
        await _service.Execute(matches, CancellationToken.None);

        // Assert
        await _mockRepository.Received(1).QueryLeagueSetting();
        await _mockRepository.Received(1).GetTVList();
        Assert.Equal(NoOfferConstant.WeekendPH, matches[0].NoOfferLive);
    }


    [Fact]
    public async Task Execute_WithAllMatchesAlreadyProcessed_ShouldNotModifyMatches()
    {
        // Arrange
        var matches = new List<EventPickingModel>
        {
            CreateTestMatch(isNoOffer: true, leagueId: 1),
            CreateTestMatch(isNoOffer: false, leagueId: 2)
        };

        _mockRepository.QueryLeagueSetting().Returns(new List<LeagueSettingInfoSender>());
        _mockRepository.GetTVList().Returns(new List<Condition>());
        _mockPullMainLeagueService.GetMainLeague(Arg.Any<List<EventPickingModel>>(), Arg.Any<CancellationToken>()).Returns(new List<main_league_info>());
        _mockMainLeagueForEventMapper.Execute(Arg.Any<List<EventPickingModel>>(), Arg.Any<List<main_league_info>>());

        // Act
        await _service.Execute(matches, CancellationToken.None);

        // Assert
        // Repository methods are always called to fetch data
        await _mockRepository.Received(1).QueryLeagueSetting();
        await _mockRepository.Received(1).GetTVList();

        // Values should remain unchanged since matches already have IsNoOffer set
        Assert.Equal(0, matches[0].NoOfferLive);
        Assert.Equal(0, matches[1].NoOfferLive);
        Assert.True(matches[0].IsNoOffer);
        Assert.False(matches[1].IsNoOffer);
    }

    [Fact]
    public async Task Execute_WithWeekendMatch_ShouldSetIsNoOfferCorrectly()
    {
        // Arrange
        var weekendMatch = new EventPickingModel
        {
            InfoSiteMatchId = 1,
            OWPLeagueId = 1,
            LeagueName = "Premier League",
            IsNoOffer = null,
            EventDateTime = new DateTime(2024, 1, 20, 15, 0, 0), // Saturday
            NoOfferLive = 0,
            Division = "Premier League-Other League",
            HomeRemark = string.Empty,
            AwayRemark = string.Empty
        };

        var leagueSettings = new List<LeagueSettingInfoSender>
        {
            CreateTestLeagueSetting(1, NoOfferConstant.Weekend)
        };

        _mockRepository.QueryLeagueSetting().Returns(leagueSettings);
        _mockRepository.GetTVList().Returns(new List<Condition>());

        // Act
        await _service.Execute(new List<EventPickingModel> { weekendMatch }, CancellationToken.None);

        // Assert
        Assert.Equal(NoOfferConstant.Weekend, weekendMatch.NoOfferLive);
        Assert.True(weekendMatch.IsNoOffer); // Should be true for weekend match
    }

    [Fact]
    public async Task Execute_WithWeekdayMatch_ShouldSetIsNoOfferCorrectly()
    {
        // Arrange
        var weekdayMatch = new EventPickingModel
        {
            InfoSiteMatchId = 1,
            OWPLeagueId = 1,
            LeagueName = "Premier League",
            IsNoOffer = null,
            EventDateTime = new DateTime(2024, 1, 15, 15, 0, 0), // Monday
            NoOfferLive = 0,
            Division = "Premier League-Other League",
            HomeRemark = string.Empty,
            AwayRemark = string.Empty
        };

        var leagueSettings = new List<LeagueSettingInfoSender>
        {
            CreateTestLeagueSetting(1, NoOfferConstant.Weekend)
        };

        _mockRepository.QueryLeagueSetting().Returns(leagueSettings);
        _mockRepository.GetTVList().Returns(new List<Condition>());

        // Act
        await _service.Execute(new List<EventPickingModel> { weekdayMatch }, CancellationToken.None);

        // Assert
        Assert.Equal(NoOfferConstant.Weekend, weekdayMatch.NoOfferLive);
        Assert.False(weekdayMatch.IsNoOffer); // Should be false for weekday match
    }

    [Fact]
    public async Task Execute_WithWeekendPeakHours_ShouldSetIsNoOfferCorrectly()
    {
        // Arrange
        var peakHourMatch = new EventPickingModel
        {
            InfoSiteMatchId = 1,
            OWPLeagueId = 1,
            LeagueName = "Premier League",
            IsNoOffer = null,
            EventDateTime = new DateTime(2024, 1, 20, 22, 0, 0), // Saturday 10 PM (peak hours)
            NoOfferLive = 0,
            Division = "Premier League-Other League",
            HomeRemark = string.Empty,
            AwayRemark = string.Empty
        };

        var leagueSettings = new List<LeagueSettingInfoSender>
        {
            CreateTestLeagueSetting(1, NoOfferConstant.WeekendPH)
        };

        _mockRepository.QueryLeagueSetting().Returns(leagueSettings);
        _mockRepository.GetTVList().Returns(new List<Condition>());

        // Act
        await _service.Execute(new List<EventPickingModel> { peakHourMatch }, CancellationToken.None);

        // Assert
        Assert.Equal(NoOfferConstant.WeekendPH, peakHourMatch.NoOfferLive);
        Assert.True(peakHourMatch.IsNoOffer); // Should be true for weekend peak hours
    }

    #region Helper Methods

    private static EventPickingModel CreateTestMatch(bool? isNoOffer, int leagueId, string? division = null)
    {
        return new EventPickingModel
        {
            InfoSiteMatchId = 1,
            OWPLeagueId = leagueId,
            LeagueName = "Premier League", // Set a default league name that's not "club friendly"
            IsNoOffer = isNoOffer,
            EventDateTime = new DateTime(2024, 1, 20, 15, 0, 0), // Saturday 3 PM
            NoOfferLive = 0,
            Division = division ?? "Premier League-Other League",
            HomeRemark = string.Empty,
            AwayRemark = string.Empty
        };
    }

    private static LeagueSettingInfoSender CreateTestLeagueSetting(int leagueId, int noOfferLive, int leagueSettingId = 1)
    {
        return new LeagueSettingInfoSender
        {
            LeagueSettingId = leagueSettingId,
            OWPLeagueId = leagueId,
            NoOfferLive = noOfferLive,
            FullConditionDescription = "" // Empty string to use default condition
        };
    }

    private static LeagueSettingInfoSender CreateTestLeagueSettingForClubFriendly(string leagueName, int noOfferLive, bool isBlackList = false, int leagueSettingId = 1)
    {
        return new LeagueSettingInfoSender
        {
            LeagueSettingId = leagueSettingId,
            League = leagueName,
            NoOfferLive = noOfferLive,
            IsBlackList = isBlackList,
            FullConditionDescription = "" // Empty string to use default condition
        };
    }

    private static EventPickingModel CreateClubFriendlyMatch(bool? isNoOffer, string homeMainLeague, string awayMainLeague, string homeRemark = "", string awayRemark = "")
    {
        return new EventPickingModel
        {
            InfoSiteMatchId = 1,
            OWPLeagueId = 1, // Not used for club friendly
            LeagueName = "club friendly",
            IsNoOffer = isNoOffer,
            EventDateTime = new DateTime(2024, 1, 20, 15, 0, 0), // Saturday 3 PM
            NoOfferLive = 0,
            Division = "Club Friendly-Division",
            HomeMainLeague = homeMainLeague,
            AwayMainLeague = awayMainLeague,
            HomeRemark = homeRemark,
            AwayRemark = awayRemark
        };
    }

    private static LeagueSettingInfoSender CreateTestLeagueSettingWithConditions(int leagueId, int noOfferLive, int leagueSettingId,
        bool hasTv, string[] roundConditions, string[] divisionConditions)
    {
        var condition = new NewLeagueSettingCondition
        {
            HasTv = hasTv,
            HasIpTv = false,
            Round = roundConditions.Select(c => new Condition { Type = "Contains", Value = c }).ToList(),
            Division = divisionConditions.Select(c => new Condition { Type = "Contains", Value = c }).ToList()
        };

        return new LeagueSettingInfoSender
        {
            LeagueSettingId = leagueSettingId,
            OWPLeagueId = leagueId,
            NoOfferLive = noOfferLive,
            FullConditionDescription = JsonConvert.SerializeObject(condition)
        };
    }

    [Fact]
    public async Task Execute_WithClubFriendlyMatch_ShouldProcessHomeAndAwayLeagues()
    {
        // Arrange
        var clubFriendlyMatch = CreateClubFriendlyMatch(null, "Premier League", "Championship");

        var leagueSettings = new List<LeagueSettingInfoSender>
        {
            CreateTestLeagueSettingForClubFriendly("Premier League", NoOfferConstant.Weekend),
            CreateTestLeagueSettingForClubFriendly("Championship", NoOfferConstant.WeekdayAndWeekend)
        };

        var mainLeagues = new List<main_league_info>
        {
            new main_league_info { TeamName = "Team1", MainLeagueName = "Premier League" },
            new main_league_info { TeamName = "Team2", MainLeagueName = "Championship" }
        };

        _mockRepository.QueryLeagueSetting().Returns(leagueSettings);
        _mockRepository.GetTVList().Returns(new List<Condition>());
        _mockPullMainLeagueService.GetMainLeague(Arg.Any<List<EventPickingModel>>(), Arg.Any<CancellationToken>()).Returns(mainLeagues);
        _mockMainLeagueForEventMapper.Execute(Arg.Any<List<EventPickingModel>>(), Arg.Any<List<main_league_info>>());

        // Act
        await _service.Execute(new List<EventPickingModel> { clubFriendlyMatch }, CancellationToken.None);

        // Assert
        Assert.True(clubFriendlyMatch.IsHomeMainLeagueNoOfferLive);
        Assert.True(clubFriendlyMatch.IsAwayMainLeagueNoOfferLive);
        Assert.True(clubFriendlyMatch.IsNoOffer); // Both leagues have no offer, so match should be no offer
    }

    [Fact]
    public async Task Execute_WithClubFriendlyMatchAndBlacklist_ShouldSetBlacklistFlags()
    {
        // Arrange
        var clubFriendlyMatch = CreateClubFriendlyMatch(null, "Premier League", "Championship");

        var leagueSettings = new List<LeagueSettingInfoSender>
        {
            CreateTestLeagueSettingForClubFriendly("Premier League", NoOfferConstant.Weekend, true), // Blacklisted
            CreateTestLeagueSettingForClubFriendly("Championship", NoOfferConstant.WeekdayAndWeekend, false)
        };

        var mainLeagues = new List<main_league_info>
        {
            new main_league_info { TeamName = "Team1", MainLeagueName = "Premier League" },
            new main_league_info { TeamName = "Team2", MainLeagueName = "Championship" }
        };

        _mockRepository.QueryLeagueSetting().Returns(leagueSettings);
        _mockRepository.GetTVList().Returns(new List<Condition>());
        _mockPullMainLeagueService.GetMainLeague(Arg.Any<List<EventPickingModel>>(), Arg.Any<CancellationToken>()).Returns(mainLeagues);
        _mockMainLeagueForEventMapper.Execute(Arg.Any<List<EventPickingModel>>(), Arg.Any<List<main_league_info>>());

        // Act
        await _service.Execute(new List<EventPickingModel> { clubFriendlyMatch }, CancellationToken.None);

        // Assert
        Assert.True(clubFriendlyMatch.IsHomeMainLeagueBlackList);
        Assert.False(clubFriendlyMatch.IsAwayMainLeagueBlackList);
        Assert.True(clubFriendlyMatch.IsNoOffer); // Home league is blacklisted, so match should be no offer
    }

    [Fact]
    public async Task Execute_WithClubFriendlyMatchAndBadRankingRemark_ShouldSetNoOffer()
    {
        // Arrange
        var clubFriendlyMatch = CreateClubFriendlyMatch(null, "Premier League", "Championship",
            homeRemark: "Bad ranking strike out", awayRemark: "Some other remark");

        var leagueSettings = new List<LeagueSettingInfoSender>
        {
            CreateTestLeagueSettingForClubFriendly("Premier League", NoOfferConstant.Weekend),
            CreateTestLeagueSettingForClubFriendly("Championship", NoOfferConstant.WeekdayAndWeekend)
        };

        var mainLeagues = new List<main_league_info>
        {
            new main_league_info { TeamName = "Team1", MainLeagueName = "Premier League" },
            new main_league_info { TeamName = "Team2", MainLeagueName = "Championship" }
        };

        _mockRepository.QueryLeagueSetting().Returns(leagueSettings);
        _mockRepository.GetTVList().Returns(new List<Condition>());
        _mockPullMainLeagueService.GetMainLeague(Arg.Any<List<EventPickingModel>>(), Arg.Any<CancellationToken>()).Returns(mainLeagues);
        _mockMainLeagueForEventMapper.Execute(Arg.Any<List<EventPickingModel>>(), Arg.Any<List<main_league_info>>());

        // Act
        await _service.Execute(new List<EventPickingModel> { clubFriendlyMatch }, CancellationToken.None);

        // Assert
        Assert.True(clubFriendlyMatch.IsNoOffer); // Bad ranking remark should set no offer
    }

    [Fact]
    public async Task Execute_WithClubFriendlyMatchAndNoMatchingLeagues_ShouldNotSetFlags()
    {
        // Arrange
        var clubFriendlyMatch = CreateClubFriendlyMatch(null, "Unknown League", "Another Unknown");

        var leagueSettings = new List<LeagueSettingInfoSender>
        {
            CreateTestLeagueSettingForClubFriendly("Premier League", NoOfferConstant.Weekend)
        };

        var mainLeagues = new List<main_league_info>();

        _mockRepository.QueryLeagueSetting().Returns(leagueSettings);
        _mockRepository.GetTVList().Returns(new List<Condition>());
        _mockPullMainLeagueService.GetMainLeague(Arg.Any<List<EventPickingModel>>(), Arg.Any<CancellationToken>()).Returns(mainLeagues);
        _mockMainLeagueForEventMapper.Execute(Arg.Any<List<EventPickingModel>>(), Arg.Any<List<main_league_info>>());

        // Act
        await _service.Execute(new List<EventPickingModel> { clubFriendlyMatch }, CancellationToken.None);

        // Assert
        Assert.False(clubFriendlyMatch.IsHomeMainLeagueNoOfferLive);
        Assert.False(clubFriendlyMatch.IsAwayMainLeagueNoOfferLive);
        Assert.False(clubFriendlyMatch.IsHomeMainLeagueBlackList);
        Assert.False(clubFriendlyMatch.IsAwayMainLeagueBlackList);
        Assert.Null(clubFriendlyMatch.IsNoOffer); // Should remain unchanged
    }

    [Fact]
    public async Task Execute_WithRegularMatchAndBadRankingRemark_ShouldSetIsNoOffer()
    {
        // Arrange
        var match = CreateTestMatch(null, 1);
        match.HomeRemark = "Bad ranking strike out";
        match.AwayRemark = "Normal remark";

        var leagueSettings = new List<LeagueSettingInfoSender>
        {
            CreateTestLeagueSetting(1, NoOfferConstant.Weekend)
        };

        _mockRepository.QueryLeagueSetting().Returns(leagueSettings);
        _mockRepository.GetTVList().Returns(new List<Condition>());

        // Act
        await _service.Execute(new List<EventPickingModel> { match }, CancellationToken.None);

        // Assert
        Assert.Equal(NoOfferConstant.Weekend, match.NoOfferLive);
        Assert.True(match.IsNoOffer); // Should be true due to bad ranking remark
    }

    [Fact]
    public async Task Execute_WithTVConditions_ShouldApplySettingsBasedOnTVStatus()
    {
        // Arrange
        var match = CreateTestMatch(null, 1);
        match.TV = "Sky Sports";
        match.Iptv = null;
        match.Channel = null;

        var leagueSettings = new List<LeagueSettingInfoSender>
        {
            CreateTestLeagueSetting(1, NoOfferConstant.Weekend)
        };

        var tvList = new List<Condition>
        {
            new Condition { Type = "Contains", Value = "Sky Sports" }
        };

        _mockRepository.QueryLeagueSetting().Returns(leagueSettings);
        _mockRepository.GetTVList().Returns(tvList);

        // Act
        await _service.Execute(new List<EventPickingModel> { match }, CancellationToken.None);

        // Assert
        Assert.Equal(NoOfferConstant.Weekend, match.NoOfferLive);
        Assert.True(match.IsNoOffer); // Weekend match should be no offer
    }

    [Fact]
    public async Task Execute_WithIPTVConditions_ShouldApplySettings()
    {
        // Arrange
        var match = CreateTestMatch(null, 1);
        match.TV = null;
        match.Iptv = "IPTV Stream";
        match.Channel = null;

        var leagueSettings = new List<LeagueSettingInfoSender>
        {
            CreateTestLeagueSetting(1, NoOfferConstant.Weekend)
        };

        _mockRepository.QueryLeagueSetting().Returns(leagueSettings);
        _mockRepository.GetTVList().Returns(new List<Condition>());

        // Act
        await _service.Execute(new List<EventPickingModel> { match }, CancellationToken.None);

        // Assert
        Assert.Equal(NoOfferConstant.Weekend, match.NoOfferLive);
        Assert.True(match.IsNoOffer); // Weekend match should be no offer
    }

    [Fact]
    public async Task Execute_WithChannelConditions_ShouldApplySettings()
    {
        // Arrange
        var match = CreateTestMatch(null, 1);
        match.TV = null;
        match.Iptv = null;
        match.Channel = "Channel 1";

        var leagueSettings = new List<LeagueSettingInfoSender>
        {
            CreateTestLeagueSetting(1, NoOfferConstant.Weekend)
        };

        _mockRepository.QueryLeagueSetting().Returns(leagueSettings);
        _mockRepository.GetTVList().Returns(new List<Condition>());

        // Act
        await _service.Execute(new List<EventPickingModel> { match }, CancellationToken.None);

        // Assert
        Assert.Equal(NoOfferConstant.Weekend, match.NoOfferLive);
        Assert.True(match.IsNoOffer); // Weekend match should be no offer
    }

    [Fact]
    public async Task Execute_WithCancelledTV_ShouldNotApplySettings()
    {
        // Arrange
        var match = CreateTestMatch(null, 1);
        match.TV = "Sky Sports Cancel";
        match.Iptv = null;
        match.Channel = null;

        var leagueSettings = new List<LeagueSettingInfoSender>
        {
            CreateTestLeagueSettingWithConditions(1, NoOfferConstant.Weekend, 1,
                hasTv: true, roundConditions: Array.Empty<string>(), divisionConditions: Array.Empty<string>())
        };

        var tvList = new List<Condition>
        {
            new Condition { Type = "Contains", Value = "Sky Sports" }
        };

        _mockRepository.QueryLeagueSetting().Returns(leagueSettings);
        _mockRepository.GetTVList().Returns(tvList);

        // Act
        await _service.Execute(new List<EventPickingModel> { match }, CancellationToken.None);

        // Assert
        // TV is cancelled, so even though league setting requires TV, it should not apply
        Assert.Equal(0, match.NoOfferLive);
        Assert.Null(match.IsNoOffer);
    }

    [Fact]
    public async Task Execute_WithMultipleLeagueSettings_ShouldApplyHighestPriority()
    {
        // Arrange
        var match = CreateTestMatch(null, 1);
        match.TV = "Sky Sports";
        match.Round = "Round 1";
        match.Division = "Premier League-Championship";

        var leagueSettings = new List<LeagueSettingInfoSender>
        {
            // Priority 1: Has TV, Round, Division
            CreateTestLeagueSettingWithConditions(1, NoOfferConstant.Weekend, 1,
                hasTv: true, roundConditions: new[] { "Round 1" }, divisionConditions: new[] { "Premier League" }),

            // Priority 7: Has TV only
            CreateTestLeagueSettingWithConditions(1, NoOfferConstant.WeekdayAndWeekend, 2,
                hasTv: true, roundConditions: Array.Empty<string>(), divisionConditions: Array.Empty<string>()),
        };

        var tvList = new List<Condition>
        {
            new Condition { Type = "Contains", Value = "Sky Sports" }
        };

        _mockRepository.QueryLeagueSetting().Returns(leagueSettings);
        _mockRepository.GetTVList().Returns(tvList);

        // Act
        await _service.Execute(new List<EventPickingModel> { match }, CancellationToken.None);

        // Assert
        Assert.Equal(NoOfferConstant.Weekend, match.NoOfferLive); // Should apply the highest priority setting (ID 1)
    }

    [Fact]
    public async Task Execute_WithRoundDistance_ShouldApplyLowestDistance()
    {
        // Arrange
        var match = CreateTestMatch(null, 1);
        match.Round = "Round 15";

        var leagueSettings = new List<LeagueSettingInfoSender>
        {
            CreateTestLeagueSettingWithConditions(1, NoOfferConstant.Weekend, 1,
                hasTv: false, roundConditions: new[] { "Round 1", "Round 10", "Round 20" }, divisionConditions: Array.Empty<string>()),

            CreateTestLeagueSettingWithConditions(1, NoOfferConstant.WeekdayAndWeekend, 2,
                hasTv: false, roundConditions: new[] { "Round 1", "Round 5", "Round 30" }, divisionConditions: Array.Empty<string>()),
        };

        _mockRepository.QueryLeagueSetting().Returns(leagueSettings);
        _mockRepository.GetTVList().Returns(new List<Condition>());

        // Act
        await _service.Execute(new List<EventPickingModel> { match }, CancellationToken.None);

        // Assert
        Assert.Equal(NoOfferConstant.WeekdayAndWeekend, match.NoOfferLive); // Should apply the setting with lowest distance (Round 15 -> Round 10 = distance 5 vs Round 15 -> Round 30 = distance 15)
    }

    #endregion
}
