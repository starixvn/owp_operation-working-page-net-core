using MediatR;
using NSubstitute;
using OWP.MaxbetSetting.Handlers;
using OWP.MaxbetSetting.Models;
using Xunit;

namespace OWP.EventPicking.UnitTests.Services;

using OWP.EventPicking.Api.Models;
using OWP.EventPicking.Api.Services;
using Xunit.Asserts.Compare;

public class BuildMainAndChildMatchesUnitTests
{
    private readonly Repository repository;
    private readonly IMediator mediator;
    private readonly BuildMainAndChildMatchesImp services;

    public BuildMainAndChildMatchesUnitTests()
    {
        repository = Substitute.For<Repository>();
        var buildChildMatchMaxBet = Substitute.For<BuildChildMatchMaxBet>();
        var buildRowColor = Substitute.For<BuildRowColor>();
        mediator = Substitute.For<IMediator>();
        var buildMatchOffer = Substitute.For<BuildMatchOfferByLiveSetting>();
        var providerBuilder = Substitute.For<BuildProvider>();

        services = new BuildMainAndChildMatchesImp(
            repository,
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
                RowColor = "white"
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
                IsTestMatch = true
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
                MatchNo = "0001"
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