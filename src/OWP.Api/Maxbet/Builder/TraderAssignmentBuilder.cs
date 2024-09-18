using OWP.Api.Maxbet.Helpers;
using OWP.Api.Maxbet.Models;
using OWP.Enums;

namespace OWP.Api.Maxbet.Builder;

public interface TraderAssignmentBuilder
{
    Task<TraderAssignmentSender> BuildTraderAssignmentSender(int owpMatchId);
}

public class TraderAssignmentBuilderImp : TraderAssignmentBuilder
{
    private readonly MaxbetRepository syncMaxbetRepository;

    public TraderAssignmentBuilderImp(MaxbetRepository syncMaxbetRepository)
    {
        this.syncMaxbetRepository = syncMaxbetRepository;
    }

    public async Task<TraderAssignmentSender> BuildTraderAssignmentSender(int owpMatchId)
    {
        var traderAssignments = await syncMaxbetRepository.GetTradersByOwpMatchId(owpMatchId);

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