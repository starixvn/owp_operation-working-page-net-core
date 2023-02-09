using NSubstitute;
using OWP.Enums;
using OWP.Maxbet.Builder;
using OWP.Maxbet.Models;
using OWP.Services;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.Maxbet.UnitTests.Builder;

public class TraderAssignmentBuilderUnitTests
{
    private readonly MaxbetRepository syncMaxbetRepository;

    private readonly TraderAssignmentBuilderImp traderAssignment;

    private readonly LogServiceWrapper logServiceWrapper;

    public TraderAssignmentBuilderUnitTests()
    {
        logServiceWrapper = Substitute.For<LogServiceWrapper>();
        syncMaxbetRepository = Substitute.For<MaxbetRepository>();
        traderAssignment = new TraderAssignmentBuilderImp(syncMaxbetRepository, logServiceWrapper);
    }

    [Fact]
    public async Task BuildTraderAssignmentSender_OwpmatchId_TraderAssignment_Has_Full_Bettype_ReturnTraderInfoFullBettype()
    {
        var matchId = 123;
        List<TraderAssignment> traders = new List<TraderAssignment>
        {
            new TraderAssignment
            {
                BettypeId= (int)EnumBettype.Handicap,
                UserId=111
            },
            new TraderAssignment
            {
                BettypeId= (int)EnumBettype.OverUnder,
                UserId=112
            },
            new TraderAssignment
            {
                BettypeId= (int)EnumBettype.FirstHalfHandicap,
                UserId=113
            },
            new TraderAssignment
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
    public async Task BuildTraderAssignmentSender_OwpmatchId_TraderAssignment_NotFound_Bettype_ReturnTraderDefault()
    {
        var matchId = 123;

        syncMaxbetRepository
            .GetTradersByOwpMatchId(matchId)
            .ReturnsForAnyArgs(Task.FromResult(new List<TraderAssignment>()));

        var result = await traderAssignment.BuildTraderAssignmentSender(matchId);

        var expected = new TraderAssignmentSender();

        DeepAssert.Equal(result, expected);
    }
}