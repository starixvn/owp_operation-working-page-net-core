using NSubstitute;
using OWP.EventPicking.Models;
using OWP.EventPicking.Services;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.EventPicking.UnitTests.Services;

public class BuildMainAndChildMatchesUnitTests
{
    private readonly Repository repository;
    private readonly BuildMainMatchMaxBet buildMainMatchMaxBet;
    private readonly BuildChildMatchMaxBet buildChildMatchMaxBet;
    private readonly BuildRowColor buildRowColor;
    private readonly BuildMainAndChildMatchesImp services;
    public BuildMainAndChildMatchesUnitTests()
    {
        repository = Substitute.For<Repository>();
        buildMainMatchMaxBet = Substitute.For<BuildMainMatchMaxBet>();
        buildChildMatchMaxBet = Substitute.For<BuildChildMatchMaxBet>();
        buildRowColor = Substitute.For<BuildRowColor>();
        services = new BuildMainAndChildMatchesImp(repository, buildMainMatchMaxBet, buildChildMatchMaxBet, buildRowColor);
    }

    [Fact]
    public async Task BuildMainAndChildMatchMaxbet_ReturnValues()
    {
        repository.QueryMainMatch(new DateTime(2022, 11, 06)).Returns(MockMainMatches());
        repository.QueryChildMatch(new DateTime(2022, 11, 06)).Returns(MockChildMatches());

        var actual = await services.GetMainAndChildMatchEvent(new DateTime(2022, 11, 06));
        DeepAssert.Equal(ExpectedMatches(), actual);
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
        => new List<EventPickingModel>
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