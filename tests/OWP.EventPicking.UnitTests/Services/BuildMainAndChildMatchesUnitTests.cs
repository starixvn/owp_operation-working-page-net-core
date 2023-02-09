namespace OWP.EventPicking.UnitTests.Services;

using EventPicking.Services;
using MaxbetSetting.Handlers;
using MaxbetSetting.Models;
using MediatR;
using Models;
using NSubstitute;
using Xunit;
using Xunit.Asserts.Compare;

public class BuildMainAndChildMatchesUnitTests
{
    private readonly Repository repository;
    private readonly BuildMainMatchMaxBet buildMainMatchMaxBet;
    private readonly BuildChildMatchMaxBet buildChildMatchMaxBet;
    private readonly BuildRowColor buildRowColor;
    private readonly IMediator mediator;
    private readonly BuildMatchOfferByLiveSetting buildMatchOffer;
    private readonly BuildMainAndChildMatchesImp services;
    private readonly BuildProvider providerBuilder;

    public BuildMainAndChildMatchesUnitTests()
    {
        repository = Substitute.For<Repository>();
        buildMainMatchMaxBet = Substitute.For<BuildMainMatchMaxBet>();
        buildChildMatchMaxBet = Substitute.For<BuildChildMatchMaxBet>();
        buildRowColor = Substitute.For<BuildRowColor>();
        mediator = Substitute.For<IMediator>();
        buildMatchOffer = Substitute.For<BuildMatchOfferByLiveSetting>();
        providerBuilder = Substitute.For<BuildProvider>();
        services = new BuildMainAndChildMatchesImp(
            repository,
            buildMainMatchMaxBet,
            buildChildMatchMaxBet,
            buildRowColor,
            buildMatchOffer,
            providerBuilder,
            mediator);
    }

    [Fact]
    public async Task BuildMainAndChildMatchMaxbet_ReturnValues()
    {
        repository.QueryMainMatch(new DateTime(2022, 11, 06)).Returns(MockMainMatches());
        repository.QueryChildMatch(new DateTime(2022, 11, 06)).Returns(MockChildMatches());
        var maxbetSettings = new List<MaxBetSettingModel>
        {
            new() {
                FT_HDP = 5000,
                NoOfCorner_FT_HDP = 2000,
                NoOfCorner_FT_OU = 100,
                NextCorner_FT_HDP = 12000,
                SecondAccept_Corner = "5s Accept"
            },
            new() {FT_HDP = 4000, NextCorner_FT_HDP = 400, NextBooking_FT_HDP = 100, SecondAccept_Fantasy = "20s Accept"},
            new() {FT_HDP = 3000, TotalBooking_FT_HDP = 300, TotalBooking_FT_OU = 100, SecondAccept_TotalBooking = "12s Accept"},
            new() {FT_HDP = -1000, NextCorner_FT_HDP = 200, NextBooking_FT_HDP = 333}
        };
        mediator.Send(Arg.Any<QueryMaxbetSettingRequest>()).Returns(maxbetSettings);

        var actual = await services.GetMainAndChildMatchEvent(new DateTime(2022, 11, 06));
        DeepAssert.Equal(ExpectedMatches(), actual);
    }

    public List<EventPickingModel> MockMainMatches()
        => new()
        {
            new()
            {
                OWPLeagueId = 111,
                InfoSiteMatchId = 3333,
                EventDateTime = new DateTime(2022, 11, 06),
                LiveTV = "TV",
                LocalTV= "TV",
                Round = "Round",
                OutSide = "Outside",
                RowColor = "white"
            },
            new()
            {
                OWPLeagueId = 222,
                InfoSiteMatchId = 4444,
                EventDateTime = new DateTime(2022, 11, 7),
                LiveTV = "test",
                LocalTV= "TV",
                Round = "Round",
                OutSide = "Outside",
                RowColor = "white",
                IsTestMatch = true
            }
        };
    public List<EventPickingModel> MockChildMatches()
        => new()
        {
            new()
            {
                OWPLeagueId = 111,
                InfoSiteMatchId = 3333,
                EventDateTime = new DateTime(2022, 11, 06),
                RowColor = "white"
            },
            new()
            {
                OWPLeagueId = 222,
                InfoSiteMatchId = 3333,
                EventDateTime = new DateTime(2022, 11, 7),
                RowColor = "white"
            },
            new()
            {
                OWPLeagueId = 222,
                InfoSiteMatchId = 4444,
                EventDateTime = new DateTime(2022, 11, 7),
                HomeName = "Match (ET)",
                RowColor = "white"
            }
        };
    public List<EventPickingModel> ExpectedMatches()
        => new()
        {
            new()
            {
                OWPLeagueId = 111,
                InfoSiteMatchId = 3333,
                EventDateTime = new DateTime(2022, 11, 06),
                LiveTV = "TV",
                LocalTV= "TV",
                Round = "Round",
                OutSide = "Outside",
                RowColor = "white"
            },
            new()
            {
                OWPLeagueId = 111,
                InfoSiteMatchId = 3333,
                EventDateTime = new DateTime(2022, 11, 06),
                RowColor = "white"
            },
            new()
            {
                OWPLeagueId = 222,
                InfoSiteMatchId = 3333,
                EventDateTime = new DateTime(2022, 11, 7),
                RowColor = "white"
            }

        };
}