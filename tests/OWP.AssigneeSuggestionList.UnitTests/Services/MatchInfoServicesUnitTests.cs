using NSubstitute;
using OWP.AssigneeSuggestionList.Handlers;
using OWP.AssigneeSuggestionList.Models;
using OWP.AssigneeSuggestionList.Services;
using Xunit;

namespace OWP.AssigneeSuggestionList.UnitTests.Services;
public class MatchInfoServicesUnitTests
{
    private readonly Repository repository;

    public MatchInfoServicesUnitTests()
    {
        repository = Substitute.For<Repository>();
    }

    [Fact]
    public async Task GetMatchInfo_ForSabaMatch_ReturnExpect()
    {
        repository.QueryMatchInfo(Arg.Any<int>())
            .Returns(new MatchInfo
            {
                OwpMatchId = 1,
                IsSaba = true,
                KOTime = new DateTime(2023, 4, 19, 14, 0, 0, 0),
                LeagueName = "saba  abc"
            });

        var service = new MatchInfoServicesImp(repository);

        var actual = await service.GetMatchInfo(new QueryTraderSuggestionRequest());

        Assert.Equal(1, actual?.OwpMatchId);
        Assert.Equal(new DateTime(2023, 4, 19, 15, 0, 0, 0), actual?.EndTime);
        Assert.Equal(false, actual?.IsSingleMatch);
    }

    [Fact]
    public async Task GetMatchInfo_ForMainMatch_ReturnExpect()
    {
        repository.QueryMatchInfo(Arg.Any<int>())
            .Returns(new MatchInfo
            {
                OwpMatchId = 1,
                IsSaba = false,
                KOTime = new DateTime(2023, 4, 19, 14, 0, 0, 0),
                MaxbetHDP = 13000,
                IsMainMatch = true,
            });

        var service = new MatchInfoServicesImp(repository);

        var actual = await service.GetMatchInfo(new QueryTraderSuggestionRequest());

        Assert.Equal(1, actual?.OwpMatchId);
        Assert.Equal(new DateTime(2023, 4, 19, 16, 0, 0, 0), actual?.EndTime);
        Assert.Equal(true, actual?.IsSingleMatch);
    }

    [Fact]
    public async Task GetMatchInfo_ForCornerMatch_ReturnExpect()
    {
        repository.QueryMatchInfo(Arg.Any<int>())
            .Returns(new MatchInfo
            {
                OwpMatchId = 1,
                IsSaba = false,
                KOTime = new DateTime(2023, 4, 19, 14, 0, 0, 0),
                LeagueName = "FUTSAL ",
                MaxbetHDP = 11000,
                IsMainMatch = false,
                ParentMatchId = 12,

            });
        repository.QueryParentMatchInfo(Arg.Any<List<int>>()).Returns(new List<ParentMatchInfo>
            { new() { OwpMatchId = 12, Remark = "ET" } });
        var service = new MatchInfoServicesImp(repository);

        var actual = await service.GetMatchInfo(new QueryTraderSuggestionRequest() { BetTypeId = -3 });

        Assert.Equal(new DateTime(2023, 4, 19, 17, 0, 0, 0), actual?.EndTime);
        Assert.Equal(true, actual?.IsSingleMatch);
    }
}
