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