using NSubstitute;
using OWP.AutoAssignment.Models.PublicApi;
using OWP.AutoAssignment.Services.PublicApi;

using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.AutoAssignment.UnitTests.Services.PublicApi;

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
        var KOTine = DateTime.Parse("2021-12-08").ToString();

        var sabaMatchInfo = new List<SabaMatchSender>
        {
            new SabaMatchSender
            {
                SabaType=1,
                MatchId=9999,
                MaxBet=12000,
                KOTime= KOTine
            }
        };

        var realMatchInfo = new List<RealMatchSender>
        {
            new RealMatchSender
            {
                MatchId=1234,
                MaxBetMainMatch=12000,
                KOTime= KOTine
            }
        };

        sabaMatchService.BuildSabaMatchInfo(Arg.Compat.Any<IList<MatchQueried>>()).ReturnsForAnyArgs(r => sabaMatchInfo);
        realMatchService.BuildRealMatchInfo(Arg.Compat.Any<IList<MatchQueried>>()).ReturnsForAnyArgs(r => realMatchInfo);

        // Act
        var service = new SchedulingMatchBuilderServiceImp(repository, sabaMatchService, realMatchService);
        var actual = await service.GetMatchesInfo(DateTime.Parse("2021-12-8"));

        var expected = new SchedulingMatch
        {
            Real = new List<RealMatchSender>
            {
                new RealMatchSender
                {
                    KOTime= KOTine,
                    MatchId=1234,
                    MaxBetMainMatch=12000
                }
            },
            Saba = new List<SabaMatchSender>
            {
                new SabaMatchSender
                {
                    KOTime= KOTine,
                    MatchId=9999,
                    SabaType=1,
                    MaxBet=12000
                }
            }
        };

        // Assert
        DeepAssert.Equal(expected, actual);
    }

    private void MockRepository()
    {
        var matchQueried = new List<MatchQueried>
        {
            new MatchQueried
            {
                InfositeMatchId=9999,
                SabaType=1
            },
            new MatchQueried
            {
                InfositeMatchId=1234,
            }
        };

        repository.GetMatchByDate(Arg.Compat.Any<DateTime>()).ReturnsForAnyArgs(r => matchQueried);
    }
}