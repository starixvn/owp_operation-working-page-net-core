using NSubstitute;
using OWP.AutoAssignment.Models.PublicApi;
using OWP.AutoAssignment.Services.PublicApi;
using System.Globalization;
using Xunit;

namespace OWP.AutoAssignment.UnitTests.Services.PublicApi;

using Xunit.Asserts.Compare;

public class SchedulingMatchBuilderServiceUnitTests
{
    private readonly Repository repository;

    private readonly SabaMatchService sabaMatchService;

    private readonly RealMatchService realMatchService;

    public SchedulingMatchBuilderServiceUnitTests()
    {
        repository = Substitute.For<Repository>();
        sabaMatchService = Substitute.For<SabaMatchService>();
        realMatchService = Substitute.For<RealMatchService>();
    }

    [Fact]
    public async Task GetMatchesInfo_EventDate_ReturnMatchInfo()
    {
        // Arrange
        MockRepository();
        var KOTine = DateTime.Parse("2021-12-08").ToString(CultureInfo.InvariantCulture);

        var sabaMatchInfo = new List<SabaMatchSender>
        {
            new()
            {
                SabaType=1,
                MatchId=9999,
                MaxBet=12000,
                KOTime= KOTine
            }
        };

        var realMatchInfo = new List<RealMatchSender>
        {
            new()
            {
                MatchId=1234,
                MaxBetMainMatch=12000,
                KOTime= KOTine
            }
        };

        sabaMatchService.BuildSabaMatchInfo(Arg.Compat.Any<IList<MatchQueried>>()).ReturnsForAnyArgs(_ => sabaMatchInfo);
        realMatchService.BuildRealMatchInfo(Arg.Compat.Any<IList<MatchQueried>>()).ReturnsForAnyArgs(_ => realMatchInfo);

        // Act
        var service = new SchedulingMatchBuilderServiceImp(repository, sabaMatchService, realMatchService);
        var actual = await service.GetMatchesInfo(DateTime.Parse("2021-12-8"));

        var expected = new SchedulingMatch
        {
            Real =
            [
                new RealMatchSender
                {
                    KOTime = KOTine,
                    MatchId = 1234,
                    MaxBetMainMatch = 12000
                }
            ],
            Saba =
            [
                new SabaMatchSender
                {
                    KOTime = KOTine,
                    MatchId = 9999,
                    SabaType = 1,
                    MaxBet = 12000
                }
            ]
        };

        // Assert
        DeepAssert.Equal(expected, actual);
    }

    private void MockRepository()
    {
        var matchQueried = new List<MatchQueried>
        {
            new()
            {
                InfositeMatchId=9999,
                SabaType=1
            },
            new()
            {
                InfositeMatchId=1234
            }
        };

        repository.GetMatchByDate(Arg.Compat.Any<DateTime>()).ReturnsForAnyArgs(_ => matchQueried);
    }
}