using NSubstitute;
using OWP.EventPicking.Models;
using OWP.EventPicking.Services;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.EventPicking.UnitTests.Services;

public class BuildMainMatchMaxBetUnitTests
{
    private readonly Repository repository;
    private readonly BuildMainMatchMaxBetImp services;

    public BuildMainMatchMaxBetUnitTests()
    {
        repository = Substitute.For<Repository>();
        services = new BuildMainMatchMaxBetImp(repository);
    }

    [Fact]
    public async Task BuildMainMatchMaxbet_MapCorrectMaxBet()
    {
        var leagueSetting = new Dictionary<int, List<LeagueSetting>>
        {
            { 111, new List<LeagueSetting>{
                new(){
                    LeagueName = "LeagueA",
                    HasTV = true,
                    MaxbetWK_HDP = 2000,
                    MaxbetWK_NoOfHDP ="No",
                    MaxbetWK_MatchOffer = "OfferWK1",
                    LSTNote="LSTNote",
                    Spread_HDP = 0.02m,
                    MaxbetWK_OU = 100,
                    Spread_OU = 0.01m
                }
            } },
            { 222, new List<LeagueSetting> { new() {
                LeagueName = "LeagueB",
                HasTV = true,
                MaxbetWD_MatchOffer = "OfferWD",
                LSTNote = "LST note",
                MaxbetWD_HDP = 1000,
                Spread_HDP = 0.01m,
                MaxbetWD_OU = 2222,
                Spread_OU = 0.03m,
                MaxbetWD_NoOfHDP = "No 1"
                }
            } },
            { 333, new List<LeagueSetting> { new() { LeagueName = "LeagueB", HasTV = false } } },
        };
        var tvList = new List<Condition>
        {
            new() { Type = "Contains", Value="TV" },
            new() { Type = "Contains", Value="Local" },
        };
        repository.GetLeagueSettings().Returns(leagueSetting);
        repository.GetTVList().Returns(tvList);

        var mainMatches = MockMainMatches();

        await services.BuildMainMatchMaxbet(mainMatches);

        DeepAssert.Equal(MockExpectedMatches().First().MaxBetHDP, mainMatches.First().MaxBetHDP);
    }

    public List<EventPickingModel> MockMainMatches()
        => new List<EventPickingModel>
        {
            new()
            {
                OWPLeagueId = 111,
                InfoSiteMatchId = 3333,
                EventDateTime = new DateTime(2022, 11, 06),
                LiveTV = "TV",
                LocalTV= "TV",
                Round = "Round",
                OutSide = "Outside"
            },
            new()
            {
                OWPLeagueId = 222,
                InfoSiteMatchId = 4444,
                EventDateTime = new DateTime(2022, 11, 10),
                LiveTV = "test",
                LocalTV= "TV",
                Round = "Round",
                OutSide = "Outside"
            },
            new()
            {
                OWPLeagueId = 333,
                InfoSiteMatchId = 5555,
                EventDateTime = new DateTime(2022, 11, 06),
                LiveTV = "",
                LocalTV= "",
                Round = "Round",
                OutSide = "Outside"
            },
            new()
            {
                OWPLeagueId = 444,
                InfoSiteMatchId = 5555,
                EventDateTime = new DateTime(2022, 11, 06),
                LiveTV = "",
                LocalTV= "",
                Round = "",
                OutSide = ""
            }
        };

    public List<EventPickingModel> MockExpectedMatches()
        => new List<EventPickingModel>
        {
            new()
            {
                OWPLeagueId = 111,
                MaxBetHDPNumber = 2000,
                InfoSiteMatchId = 3333,
                EventDateTime = new DateTime(2022, 11, 06),
                LiveTV = "TV",
                LocalTV= "TV",
                Round = "Round",
                OutSide = "Outside",
                MaxBetHDP = "2K",
                NoHDP ="No",
                MatchOffer = "OfferWK1",
                LstNote="LSTNote",
                SpreadHDP = "2%",
                MaxBetOU = "0.1K",
                SpreadOU = "1%"
            },
            new()
            {
                OWPLeagueId = 222,
                MaxBetHDPNumber = 1000,
                InfoSiteMatchId = 4444,
                EventDateTime = new DateTime(2022, 11, 10),
                LiveTV = "test",
                LocalTV= "TV",
                Round = "Round",
                OutSide = "Outside",
                MatchOffer = "OfferWD",
                LstNote = "LST note",
                MaxBetHDP = "1K",
                SpreadHDP = "1%",
                MaxBetOU = "2.222K",
                SpreadOU = "3%",
                NoHDP = "No 1"
            },
            new()
            {
                OWPLeagueId = 333,
                InfoSiteMatchId = 5555,
                EventDateTime = new DateTime(2022, 11, 06),
                LiveTV = "",
                LocalTV= "",
                Round = "Round",
                OutSide = "Outside",
                LstNote = "",
                MaxBetHDP = "",
                MaxBetOU = "",
                SpreadHDP = "",
                SpreadOU = ""
            },
            new()
            {
                OWPLeagueId = 444,
                InfoSiteMatchId = 5555,
                EventDateTime = new DateTime(2022, 11, 06),
                LiveTV = "",
                LocalTV= "",
                Round = "",
                OutSide = "",
            }
        };
}