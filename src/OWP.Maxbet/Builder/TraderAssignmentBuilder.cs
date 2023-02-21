using OWP.Enums;
using OWP.Maxbet.Helpers;
using OWP.Maxbet.Models;
using OWP.Services;

namespace OWP.Maxbet.Builder;

public interface TraderAssignmentBuilder
{
    Task<TraderAssignmentSender> BuildTraderAssignmentSender(int owpMatchId);
}

public class TraderAssignmentBuilderImp : TraderAssignmentBuilder
{
    private readonly MaxbetRepository syncMaxbetRepository;
    private readonly LogServiceWrapper logServiceWrapper;

    public TraderAssignmentBuilderImp(MaxbetRepository syncMaxbetRepository, LogServiceWrapper logServiceWrapper)
    {
        this.syncMaxbetRepository = syncMaxbetRepository;
        this.logServiceWrapper = logServiceWrapper;
    }

    public async Task<TraderAssignmentSender> BuildTraderAssignmentSender(int owpMatchId)
    {
        var traderAssignments = await syncMaxbetRepository.GetTradersByOwpMatchId(owpMatchId);
        await logServiceWrapper.Log("====BuildTraderAssignmentSender======", traderAssignments);

        var handicap = MaxbetHelper.FindTraderAssignmentByBettype(traderAssignments, (int)EnumBettype.Handicap) ?? new TraderAssignment();
        var overUnder = MaxbetHelper.FindTraderAssignmentByBettype(traderAssignments, (int)EnumBettype.OverUnder) ?? new TraderAssignment();
        var firstHalfHandicap = MaxbetHelper.FindTraderAssignmentByBettype(traderAssignments, (int)EnumBettype.FirstHalfHandicap) ?? new TraderAssignment();
        var firstHalfOverUnder = MaxbetHelper.FindTraderAssignmentByBettype(traderAssignments, (int)EnumBettype.FirstHalfOverUnder) ?? new TraderAssignment();
        return new TraderAssignmentSender
        {
            Handicap = handicap.UserId,
            OverUnder = overUnder.UserId,
            FirstHalfHandicap = firstHalfHandicap.UserId,
            FirstHalfOverUnder = firstHalfOverUnder.UserId
        };
    }
}