using NSubstitute;
using OWP.AutoAssignment.Models.PublicApi;
using OWP.AutoAssignment.Services.PublicApi;
using Xunit;

namespace OWP.AutoAssignment.UnitTests.Services.PublicApi;

using Xunit.Asserts.Compare;

public class SabaMatchServiceUnitTests
{
    private readonly Repository repository;

    public SabaMatchServiceUnitTests()
    {
        repository = Substitute.For<Repository>();
    }

    [Fact]
    public async Task BuildSabaMatch_Always_ReturnListSaBaMatch()
    {
        var matchQueries = MockListMatchQueries();
        repository.GetSabaMatchCommonInfoByIds(Arg.Compat.Any<List<int>>())
            .Returns(MockListSabaMatchInfo());
        var expected = GetExpected();
        var service = new SabaMatchServiceImp(repository);

        var actual = await service.BuildSabaMatchInfo(matchQueries);

        DeepAssert.Equal(expected, actual);
    }

    [Fact]
    public async Task GetSabaMaxbet_Always_ReturnValues()
    {
        var expected = new List<MaxBetOfFTHDP>
        {
            new() { InfoSiteMatchId = 1, MaxBetFTHDP = 1 }
        };

        repository.GetMaxMaxbetFTHDP(Arg.Compat.Any<int[]>())
            .Returns(expected);
        var service = new SabaMatchServiceImp(repository);

        var act = await service.GetSabaMaxbet(new[] { 1 });

        DeepAssert.Equal(expected, act);
    }

    public List<SabaMatchCommonInfo> MockListSabaMatchInfo()
        =>
        [
            new SabaMatchCommonInfo
            {
                InfositeId = 1,
                KOTime = new DateTime(2021, 11, 30)
            },

            new SabaMatchCommonInfo
            {
                InfositeId = 2,
                KOTime = new DateTime(2021, 11, 29)
            },

            new SabaMatchCommonInfo
            {
                InfositeId = 3,
                KOTime = new DateTime(2021, 11, 28)
            }
        ];

    public List<MatchQueried> MockListMatchQueries()
        =>
        [
            new MatchQueried
            {
                InfositeMatchId = 1,
                SabaType = 1
            },

            new MatchQueried
            {
                InfositeMatchId = 2,
                SabaType = 2
            },

            new MatchQueried
            {
                InfositeMatchId = 3,
                SabaType = 3
            }
        ];

    public List<SabaMatchSender> GetExpected()
        =>
        [
            new SabaMatchSender
            {
                MatchId = 1,
                SabaType = 1,
                KOTime = "2021-11-30T00:00:00"
            },

            new SabaMatchSender
            {
                MatchId = 2,
                SabaType = 2,
                KOTime = "2021-11-29T00:00:00"
            },

            new SabaMatchSender
            {
                MatchId = 3,
                SabaType = 3,
                KOTime = "2021-11-28T00:00:00"
            }
        ];
}