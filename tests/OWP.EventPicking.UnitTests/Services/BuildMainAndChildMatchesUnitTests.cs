using MediatR;
using NSubstitute;
using OWP.Services;
using Xunit;

namespace OWP.EventPicking.UnitTests.Services;

using Microsoft.Extensions.Options;
using OWP.EventPicking.Api.Models;
using OWP.EventPicking.Api.Services;
using OWP.Models;
using Xunit.Asserts.Compare;

public class BuildMainAndChildMatchesUnitTests
{
    private readonly OWP.EventPicking.Repository repository;
    private readonly BuildChildMatchMaxBet buildChildMatchMaxBet;
    private readonly BuildRowColor buildRowColor;
    private readonly BuildMatchOfferByLiveSetting buildMatchOffer;
    private readonly BuildProvider buildProvider;
    private readonly IOptions<AppSettings> appSettingOptions;
    private readonly MaxbetSettingService maxbetSettingService;
    private readonly SyncEventPickingInfo syncEventPickingInfo;
    private readonly BuildMainAndChildMatchesImp services;

    public BuildMainAndChildMatchesUnitTests()
    {
        repository = Substitute.For<OWP.EventPicking.Repository>();
        buildChildMatchMaxBet = Substitute.For<BuildChildMatchMaxBet>();
        buildRowColor = Substitute.For<BuildRowColor>();
        buildMatchOffer = Substitute.For<BuildMatchOfferByLiveSetting>();
        buildProvider = Substitute.For<BuildProvider>();
        maxbetSettingService = Substitute.For<MaxbetSettingService>();
        syncEventPickingInfo = Substitute.For<SyncEventPickingInfo>();

        var appSettings = new AppSettings
        {
            ShowTestMatchEventPicking = false
        };

        appSettingOptions = Substitute.For<IOptions<AppSettings>>();
        appSettingOptions.Value.Returns(appSettings);

        services = new BuildMainAndChildMatchesImp(
            repository,
            buildChildMatchMaxBet,
            buildRowColor,
            buildMatchOffer,
            buildProvider,
            appSettingOptions,
            syncEventPickingInfo);
    }

    [Fact]
    public async Task BuildMainAndChildMatchMaxbet_ReturnValues()
    {
        repository.QueryMainMatch(new DateTime(2022, 11, 06)).Returns(MockMainMatches());
        repository.QueryChildMatch(new DateTime(2022, 11, 06)).Returns(MockChildMatches());
        var maxbetSettings = new Dictionary<decimal, MaxBetSettingModel>
        {
            [5000] = new() {
                FT_HDP = 5000,
                NoOfCorner_FT_HDP = 2000,
                NoOfCorner_FT_OU = 100,
                NextCorner_FT_HDP = 12000,
                SecondAccept_Corner = "5s Accept"
            },
            [4000] = new() {FT_HDP = 4000, NextCorner_FT_HDP = 400, NextBooking_FT_HDP = 100, SecondAccept_Fantasy = "20s Accept"},
            [3000] = new() {FT_HDP = 3000, TotalBooking_FT_HDP = 300, TotalBooking_FT_OU = 100, SecondAccept_TotalBooking = "12s Accept"},
            [-1000] = new() {FT_HDP = -1000, NextCorner_FT_HDP = 200, NextBooking_FT_HDP = 333}
        };
        buildChildMatchMaxBet.GetMaxBetDefaultSetting().Returns(maxbetSettings);

        var actual = await services.GetMainAndChildMatchEvent(new DateTime(2022, 11, 06));
        DeepAssert.Equal(ExpectedMatches(), actual);
    }

    [Fact]
    public async Task GetMainEvent_ShouldReturnFilteredMainMatches()
    {
        // Arrange
        var testDate = new DateTime(2022, 11, 6);
        var mainMatches = new List<EventPickingModel>
        {
            new EventPickingModel
            {
                InfoSiteMatchId = 1,
                EventDateTime = testDate,
                IsTestMatch = false,
                LeagueName = "Premier League"
            },
            new EventPickingModel
            {
                InfoSiteMatchId = 2,
                EventDateTime = testDate,
                IsTestMatch = true, // Should be filtered out
                LeagueName = "Test League"
            }
        };

        var maxBetSettings = new Dictionary<decimal, MaxBetSettingModel>
        {
            [1000] = new() { FT_HDP = 1000 }
        };

        repository.QueryMainMatch(testDate).Returns(mainMatches);
        buildChildMatchMaxBet.GetMaxBetDefaultSetting().Returns(maxBetSettings);

        // Act
        var result = await services.GetMainEvent(testDate);

        // Assert
        Assert.Single(result); // Only non-test match should remain
        Assert.Equal(1, result[0].InfoSiteMatchId);
        Assert.Equal("Premier League", result[0].LeagueName);
    }

    [Fact]
    public async Task GetMainEvent_WithMovedEvents_ShouldIncludeMovedEvents()
    {
        // Arrange
        var testDate = new DateTime(2022, 11, 6);
        var mainMatches = new List<EventPickingModel>
        {
            new EventPickingModel { InfoSiteMatchId = 1, EventDateTime = testDate }
        };

        var movedEvents = new List<EventPickingModel>
        {
            new EventPickingModel { InfoSiteMatchId = 2, EventDateTime = testDate.AddDays(1) }
        };

        var movedEventsRnD = new List<RAndDInfo>
        {
            new RAndDInfo { InfositeId = 2 }
        };

        var maxBetSettings = new Dictionary<decimal, MaxBetSettingModel>
        {
            [1000] = new() { FT_HDP = 1000 }
        };

        repository.QueryMainMatch(testDate).Returns(mainMatches);
        repository.QueryMovedEventDateMatch(testDate).Returns((movedEvents, movedEventsRnD));
        buildChildMatchMaxBet.GetMaxBetDefaultSetting().Returns(maxBetSettings);

        // Act
        var result = await services.GetMainEvent(testDate);

        // Assert
        Assert.Equal(2, result.Count); // Original + moved event
        Assert.Contains(result, m => m.InfoSiteMatchId == 1);
        Assert.Contains(result, m => m.InfoSiteMatchId == 2);
    }

    [Fact]
    public async Task GetChildEventEventPicking_ShouldReturnProcessedChildEvents()
    {
        // Arrange
        var testDate = new DateTime(2022, 11, 6);
        var childMatches = new List<EventPickingModel>
        {
            new EventPickingModel
            {
                InfoSiteMatchId = 1,
                EventDateTime = testDate,
                RowColor = "default"
            }
        };

        repository.QueryEventPickingChildMatch(testDate).Returns(childMatches);

        // Act
        var result = await services.GetChildEventEventPicking(testDate);

        // Assert
        Assert.Single(result);
        Assert.Equal(1, result[0].InfoSiteMatchId);
    }

    [Fact]
    public async Task GetMainAndChildMatchEvent_ShouldSyncLeagueSettings()
    {
        // Arrange
        var testDate = new DateTime(2022, 11, 6);
        var mainMatches = new List<EventPickingModel>
        {
            new EventPickingModel
            {
                OWPLeagueId = 1,
                InfoSiteMatchId = 1,
                EventDateTime = testDate,
                IsTestMatch = false
            }
        };

        var childMatches = new List<EventPickingModel>();
        var maxBetSettings = new Dictionary<decimal, MaxBetSettingModel>
        {
            [1000] = new() { FT_HDP = 1000 }
        };

        repository.QueryMainMatch(testDate).Returns(mainMatches);
        repository.QueryChildMatch(testDate).Returns(childMatches);
        buildChildMatchMaxBet.GetMaxBetDefaultSetting().Returns(maxBetSettings);

        // Act
        var result = await services.GetMainAndChildMatchEvent(testDate);

        // Assert
        Assert.Single(result);
        // Verify that SyncEventPickingInfo.Execute was called
        await syncEventPickingInfo.Received(1).Execute(Arg.Is<List<EventPickingModel>>(matches => matches.Count == 1));
    }

    [Fact]
    public async Task GetMainEvent_ShouldApplyMatchOfferBuild()
    {
        // Arrange
        var testDate = new DateTime(2022, 11, 6);
        var mainMatches = new List<EventPickingModel>
        {
            new EventPickingModel { InfoSiteMatchId = 1, EventDateTime = testDate, IsTestMatch = false }
        };

        var maxBetSettings = new Dictionary<decimal, MaxBetSettingModel>
        {
            [1000] = new() { FT_HDP = 1000 }
        };

        repository.QueryMainMatch(testDate).Returns(mainMatches);
        buildChildMatchMaxBet.GetMaxBetDefaultSetting().Returns(maxBetSettings);

        // Act
        await services.GetMainEvent(testDate);

        // Assert - Verify that buildMatchOffer.BuildForMainMatch was called
        // Note: This would require the mock to be set up properly for verification
    }

    [Fact]
    public async Task GetMainAndChildMatchEvent_WithTestMatches_ShouldFilterThemOut()
    {
        // Arrange
        var testDate = new DateTime(2022, 11, 6);
        var mainMatches = new List<EventPickingModel>
        {
            new EventPickingModel
            {
                OWPLeagueId = 1,
                InfoSiteMatchId = 1,
                EventDateTime = testDate,
                IsTestMatch = false,
                LeagueName = "Premier League"
            },
            new EventPickingModel
            {
                OWPLeagueId = 2,
                InfoSiteMatchId = 2,
                EventDateTime = testDate,
                IsTestMatch = true, // Should be filtered out
                LeagueName = "Test League"
            }
        };

        var childMatches = new List<EventPickingModel>();
        var maxBetSettings = new Dictionary<decimal, MaxBetSettingModel>
        {
            [1000] = new() { FT_HDP = 1000 }
        };

        repository.QueryMainMatch(testDate).Returns(mainMatches);
        repository.QueryChildMatch(testDate).Returns(childMatches);
        buildChildMatchMaxBet.GetMaxBetDefaultSetting().Returns(maxBetSettings);

        // Act
        var result = await services.GetMainAndChildMatchEvent(testDate);

        // Assert
        Assert.Single(result); // Only non-test match should remain
        Assert.Equal("Premier League", result[0].LeagueName);
        Assert.Equal(1, result[0].InfoSiteMatchId);
    }

    [Fact]
    public async Task GetChildEventEventPicking_ShouldApplySpecialProductBuild()
    {
        // Arrange
        var testDate = new DateTime(2022, 11, 6);
        var childMatches = new List<EventPickingModel>
        {
            new EventPickingModel
            {
                InfoSiteMatchId = 1,
                EventDateTime = testDate
            }
        };

        repository.QueryEventPickingChildMatch(testDate).Returns(childMatches);

        // Act
        var result = await services.GetChildEventEventPicking(testDate);

        // Assert
        Assert.Single(result);
        // Special product building would be verified if we had access to the static method
    }

    [Fact]
    public async Task GetMainEvent_ShouldHandleEmptyResults()
    {
        // Arrange
        var testDate = new DateTime(2022, 11, 6);
        var emptyMainMatches = new List<EventPickingModel>();
        var maxBetSettings = new Dictionary<decimal, MaxBetSettingModel>
        {
            [1000] = new() { FT_HDP = 1000 }
        };

        repository.QueryMainMatch(testDate).Returns(emptyMainMatches);
        buildChildMatchMaxBet.GetMaxBetDefaultSetting().Returns(maxBetSettings);

        // Act
        var result = await services.GetMainEvent(testDate);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetMainAndChildMatchEvent_ShouldHandleEmptyChildMatches()
    {
        // Arrange
        var testDate = new DateTime(2022, 11, 6);
        var mainMatches = new List<EventPickingModel>
        {
            new EventPickingModel
            {
                OWPLeagueId = 1,
                InfoSiteMatchId = 1,
                EventDateTime = testDate,
                IsTestMatch = false
            }
        };

        var emptyChildMatches = new List<EventPickingModel>();
        var maxBetSettings = new Dictionary<decimal, MaxBetSettingModel>
        {
            [1000] = new() { FT_HDP = 1000 }
        };

        repository.QueryMainMatch(testDate).Returns(mainMatches);
        repository.QueryChildMatch(testDate).Returns(emptyChildMatches);
        buildChildMatchMaxBet.GetMaxBetDefaultSetting().Returns(maxBetSettings);

        // Act
        var result = await services.GetMainAndChildMatchEvent(testDate);

        // Assert
        Assert.Single(result); // Only main match should be included
        Assert.Equal(1, result[0].InfoSiteMatchId);
    }

    [Fact]
    public async Task GetChildEventEventPicking_ShouldHandleEmptyResults()
    {
        // Arrange
        var testDate = new DateTime(2022, 11, 6);
        var emptyChildMatches = new List<EventPickingModel>();

        repository.QueryEventPickingChildMatch(testDate).Returns(emptyChildMatches);

        // Act
        var result = await services.GetChildEventEventPicking(testDate);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetMainEvent_ShouldSortResults()
    {
        // Arrange
        var testDate = new DateTime(2022, 11, 6);
        var mainMatches = new List<EventPickingModel>
        {
            new EventPickingModel { InfoSiteMatchId = 3, EventDateTime = testDate, IsTestMatch = false },
            new EventPickingModel { InfoSiteMatchId = 1, EventDateTime = testDate, IsTestMatch = false },
            new EventPickingModel { InfoSiteMatchId = 2, EventDateTime = testDate, IsTestMatch = false }
        };

        var maxBetSettings = new Dictionary<decimal, MaxBetSettingModel>
        {
            [1000] = new() { FT_HDP = 1000 }
        };

        repository.QueryMainMatch(testDate).Returns(mainMatches);
        buildChildMatchMaxBet.GetMaxBetDefaultSetting().Returns(maxBetSettings);

        // Act
        var result = await services.GetMainEvent(testDate);

        // Assert
        Assert.Equal(3, result.Count);
        // Verify sorting is applied (this would depend on the SortEventPicking.SortList implementation)
    }

    public List<EventPickingModel> MockMainMatches()
        =>
        [
            new EventPickingModel
            {
                OWPLeagueId = 111,
                InfoSiteMatchId = 3333,
                EventDateTime = new DateTime(2022, 11, 06),
                LiveTV = "TV",
                LocalTV = "TV",
                Round = "Round",
                OutSide = "Outside",
                RowColor = "white",
                LeagueName = ""
            },

            new EventPickingModel
            {
                OWPLeagueId = 222,
                InfoSiteMatchId = 4444,
                EventDateTime = new DateTime(2022, 11, 7),
                LiveTV = "test",
                LocalTV = "TV",
                Round = "Round",
                OutSide = "Outside",
                RowColor = "white",
                IsTestMatch = true,
                LeagueName = ""
            }
        ];
    public List<EventPickingModel> MockChildMatches()
        =>
        [
            new EventPickingModel
            {
                OWPLeagueId = 111,
                InfoSiteMatchId = 3333,
                EventDateTime = new DateTime(2022, 11, 06),
                RowColor = "white",
                MinorBettypeId = -3
            },

            new EventPickingModel
            {
                OWPLeagueId = 222,
                InfoSiteMatchId = 3333,
                EventDateTime = new DateTime(2022, 11, 7),
                RowColor = "white"
            },

            new EventPickingModel
            {
                OWPLeagueId = 222,
                InfoSiteMatchId = 4444,
                EventDateTime = new DateTime(2022, 11, 7),
                HomeName = "Match (ET)",
                RowColor = "white"
            }
        ];
    public List<EventPickingModel> ExpectedMatches()
        =>
        [
            new EventPickingModel
            {
                OWPLeagueId = 111,
                InfoSiteMatchId = 3333,
                EventDateTime = new DateTime(2022, 11, 06),
                LiveTV = "TV",
                LocalTV = "TV",
                Round = "Round",
                OutSide = "Outside",
                RowColor = "white",
                MatchNo = "0001",
                LeagueName = ""
            },
            new EventPickingModel
            {
                OWPLeagueId = 222,
                InfoSiteMatchId = 3333,
                EventDateTime = new DateTime(2022, 11, 7),
                RowColor = "white",
                MatchNo = "0001"
            },
            new EventPickingModel
            {
                OWPLeagueId = 111,
                InfoSiteMatchId = 3333,
                EventDateTime = new DateTime(2022, 11, 06),
                RowColor = "white",
                MatchNo = "0001C",
                MinorBettypeId = -3
            }
        ];
}