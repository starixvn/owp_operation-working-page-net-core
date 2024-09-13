using NSubstitute;
using OWP.Api.Maxbet;
using OWP.Api.Maxbet.Builder;
using OWP.Api.Maxbet.Models;
using OWP.Enums;
using Xunit;

namespace OWP.Maxbet.UnitTests.Builder;

using Xunit.Asserts.Compare;

public class TraderAssignmentBuilderUnitTests
{
    private readonly MaxbetRepository syncMaxbetRepository;

    private readonly TraderAssignmentBuilderImp traderAssignment;

    public TraderAssignmentBuilderUnitTests()
    {
        syncMaxbetRepository = Substitute.For<MaxbetRepository>();
        traderAssignment = new TraderAssignmentBuilderImp(syncMaxbetRepository);
    }

    [Fact]
    public async Task BuildTraderAssignmentSender_OwpMatchId_TraderAssignment_Has_Full_Bettype_ReturnTraderInfoFullBettype()
    {
        const int matchId = 123;
        var traders = new List<TraderAssignment>
        {
            new()
            {
                BettypeId= (int)EnumBettype.Handicap,
                UserId=111
            },
            new()
            {
                BettypeId= (int)EnumBettype.OverUnder,
                UserId=112
            },
            new()
            {
                BettypeId= (int)EnumBettype.FirstHalfHandicap,
                UserId=113
            },
            new()
            {
                BettypeId= (int)EnumBettype.FirstHalfOverUnder,
                UserId=114
            }
        };

        syncMaxbetRepository
            .GetTradersByOwpMatchId(matchId)
            .ReturnsForAnyArgs(Task.FromResult(traders));

        var result = await traderAssignment.BuildTraderAssignmentSender(matchId);

        var expected = new TraderAssignmentSender
        {
            Handicap = 111,
            OverUnder = 112,
            FirstHalfHandicap = 113,
            FirstHalfOverUnder = 114
        };

        DeepAssert.Equal(result, expected);
    }

    [Fact]
    public async Task BuildTraderAssignmentSender_OwpMatchId_TraderAssignment_NotFound_Bettype_ReturnTraderDefault()
    {
        const int matchId = 123;

        syncMaxbetRepository
            .GetTradersByOwpMatchId(matchId)
            .ReturnsForAnyArgs(Task.FromResult(new List<TraderAssignment>()));

        var result = await traderAssignment.BuildTraderAssignmentSender(matchId);

        var expected = new TraderAssignmentSender();

        DeepAssert.Equal(result, expected);
    }
}