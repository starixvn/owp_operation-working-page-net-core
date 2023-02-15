namespace OWP.AutoAssignment.UnitTests.Services.InternalApi;

using AutoAssignment.Services.InternalApi;
using Models.InternalApi;
using NSubstitute;
using Xunit;
using Xunit.Asserts.Compare;

public class LeagueSettingBuilderServiceUnitTests
{
    private readonly Repository repository;

    public LeagueSettingBuilderServiceUnitTests()
    {
        repository = Substitute.For<Repository>();
    }

    [Fact]
    public async Task BuildLeagueSettingInfo_CompareWith_Model()
    {
        var requestId = "abc-xyz";
        var service = new LeagueSettingBuilderServiceImp(repository);
        var models = new List<ExportMatchModel>
        {
            new()
            {
                InfoSiteMatchId = 100,
                Bettype = 0
            },
            new()
            {
                InfoSiteMatchId = 101,
                Bettype = -3
            }
        };
        var autoAssignment = MockAssignmentResultModels();

        repository.GetLeagueSettingInfo(Arg.Compat.Any<IList<int>>(), Arg.Compat.Any<string>()).Returns(MockLeagueSettingInfos());

        await service.Build(models, autoAssignment, requestId);

        var expected = ExpectedModel();

        DeepAssert.Equal(expected, models);
    }

    public List<ExportMatchModel> ExpectedModel()
        => new()
        {
            new ExportMatchModel
            {
                InfoSiteMatchId = 100,
                Bettype = 0,
                MaxBetHDP = "1K",
                SpreadHDP = "100000",
                SpreadOU = "200000",
                MaxBetOU = "2K",
                MaxBet1HHDP = "1K",
                Spread1HHDP = "100000",
                Spread1HOU = "100000",
                MaxBet1HOU = "1K",
                Spread1X2 = "100000",
                MaxBet1X2 = "1K",
                MaxBet1H1X2 = "1K"
            },
            new ExportMatchModel
            {
                InfoSiteMatchId = 101,
                Bettype = -3,
                MaxBetHDP = "10K",
                MaxBetOU = "10K"
            }
        };

    public List<AutoAssignmentResultModel> MockAssignmentResultModels()
        => new()
        {
            new AutoAssignmentResultModel
            {
                Bettype = 0,
                InfoSiteMatchId = 100,
                RequestId = "abc-xyz"
            },
            new AutoAssignmentResultModel
            {
                Bettype = -3,
                InfoSiteMatchId = 101,
                RequestId = "abc-xyz"
            }
        };

    public List<LeagueSettingInfo> MockLeagueSettingInfos()
        => new()
        {
            new LeagueSettingInfo
            {
                MatchId = 100,
                MaxBetHDP = 1000,
                SpreadHDP = 1000,
                SpreadOU = 2000,
                MaxBetOU = 2000,
                MaxBet1HHDP = 1000,
                Spread1HHDP = 1000,
                Spread1HOU = 1000,
                MaxBet1HOU = 1000,
                Spread1X2 = 1000,
                MaxBet1X2 = 1000,
                MaxBet1H1X2 = 1000
            },
            new LeagueSettingInfo
            {
                MatchId = 101,
                MaxBetHDP = 1000,
                SpreadHDP = 1000,
                SpreadOU = 2000,
                MaxBetOU = 2000,
                MaxBet1HHDP = 1000,
                Spread1HHDP = 1000,
                Spread1HOU = 1000,
                MaxBet1HOU = 1000,
                Spread1X2 = 1000,
                MaxBet1X2 = 1000,
                MaxbetNoOfCorner = 10000,
                MaxBet1H1X2 = 1000
            }
        };
}