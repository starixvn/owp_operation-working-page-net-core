using OWP.API.SyncMaxbet.Models;
using OWP.Enums;
using OWP.Helpers;
using System.Threading.Tasks;

namespace OWP.API.SyncMaxbet.Builder
{
    public interface TraderAssignmentBuilder
    {
        Task<TraderAssignmentSender> BuildTraderAssignmentSender(int owpMatchId);
    }

    public class TraderAssignmentBuilderImp : TraderAssignmentBuilder
    {
        private readonly SyncMaxbetReposiory syncMaxbetReposiory;

        public TraderAssignmentBuilderImp(SyncMaxbetReposiory syncMaxbetReposiory)
        {
            this.syncMaxbetReposiory = syncMaxbetReposiory;
        }

        public async Task<TraderAssignmentSender> BuildTraderAssignmentSender(int owpMatchId)
        {
            var traderAssignments = await syncMaxbetReposiory.GetTradersByOwpMatchId(owpMatchId);

            var handicap = MaxbetHelper.FindTraderAssinmentByBettype(traderAssignments, (int)Bettype.Handicap) ?? new TraderAssignment();
            var overunder = MaxbetHelper.FindTraderAssinmentByBettype(traderAssignments, (int)Bettype.OverUnder) ?? new TraderAssignment();
            var firstHalfHandicap = MaxbetHelper.FindTraderAssinmentByBettype(traderAssignments, (int)Bettype.FirstHalfHandicap) ?? new TraderAssignment();
            var firstHalfOverUnder = MaxbetHelper.FindTraderAssinmentByBettype(traderAssignments, (int)Bettype.FirstHalfOverUnder) ?? new TraderAssignment();
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
