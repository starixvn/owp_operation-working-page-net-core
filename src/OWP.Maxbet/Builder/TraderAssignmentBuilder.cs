using System.Threading.Tasks;
using OWP.Maxbet.Models;
using OWP.Enums;
using OWP.Helpers;

namespace OWP.Maxbet.Builder
{
    public interface TraderAssignmentBuilder
    {
        Task<TraderAssignmentSender> BuildTraderAssignmentSender(int owpMatchId);
    }

    public class TraderAssignmentBuilderImp : TraderAssignmentBuilder
    {
        private readonly MaxbetReposiory syncMaxbetReposiory;

        public TraderAssignmentBuilderImp(MaxbetReposiory syncMaxbetReposiory)
        {
            this.syncMaxbetReposiory = syncMaxbetReposiory;
        }

        public async Task<TraderAssignmentSender> BuildTraderAssignmentSender(int owpMatchId)
        {
            var traderAssignments = await syncMaxbetReposiory.GetTradersByOwpMatchId(owpMatchId);

            var handicap = MaxbetHelper.FindTraderAssinmentByBettype(traderAssignments, (int)EnumBettype.Handicap) ?? new TraderAssignment();
            var overunder = MaxbetHelper.FindTraderAssinmentByBettype(traderAssignments, (int)EnumBettype.OverUnder) ?? new TraderAssignment();
            var firstHalfHandicap = MaxbetHelper.FindTraderAssinmentByBettype(traderAssignments, (int)EnumBettype.FirstHalfHandicap) ?? new TraderAssignment();
            var firstHalfOverUnder = MaxbetHelper.FindTraderAssinmentByBettype(traderAssignments, (int)EnumBettype.FirstHalfOverUnder) ?? new TraderAssignment();
            return new TraderAssignmentSender
            {
                Handicap = handicap.UserId,
                OverUnder = overunder.UserId,
                FirstHalfHandicap = firstHalfHandicap.UserId,
                FirstHalfOverUnder = firstHalfOverUnder.UserId,
            };
        }
    }
}