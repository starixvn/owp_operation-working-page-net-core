using OWP.API.SyncMaxbet.Enums;
using OWP.API.SyncMaxbet.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OWP.API.SyncMaxbet.Services
{
    public interface MaxbetService
    {
        public Task<MaxbetInfo> GetMaxbetInfo(int adminsiteId);
    }

    public class MaxbetServiceImp : MaxbetService
    {
        private readonly SyncMaxbetReposiory syncMaxbetReposiory;
        public MaxbetServiceImp(SyncMaxbetReposiory syncMaxbetReposiory)
        {
            this.syncMaxbetReposiory = syncMaxbetReposiory;
        }
        public async Task<MaxbetInfo> GetMaxbetInfo(int adminsiteId)
        {
            var owpMatchId = await syncMaxbetReposiory.GetMatchIdByAdminSiteId(adminsiteId);
            var traderAssignments = await syncMaxbetReposiory.GetTradersByOwpMatchId(owpMatchId);
            var traderAssignmentsSender = BuildTraderAssignmentSender(traderAssignments);
            return new MaxbetInfo
            {
                MatchID = adminsiteId,
                TraderAssignments = traderAssignmentsSender
            };
        }

        public TraderAssignmentSender BuildTraderAssignmentSender(List<TraderAssignment> traderAssignments)
        {
            var handicap = FindTraderAssinmentByBettype(traderAssignments, (int)Bettype.Handicap);
            var overunder = FindTraderAssinmentByBettype(traderAssignments, (int)Bettype.OverUnder);
            var firstHalfHandicap = FindTraderAssinmentByBettype(traderAssignments, (int)Bettype.FirstHalfHandicap);
            var firstHalfOverUnder = FindTraderAssinmentByBettype(traderAssignments, (int)Bettype.FirstHalfOverUnder);
            return new TraderAssignmentSender
            {
                Handicap = handicap.UserId,
                OverUnder = overunder.UserId,
                FirstHalfHandicap = firstHalfHandicap.UserId,
                FirstHalfOverUnder = firstHalfOverUnder.UserId,
            };
        }

        private T FindTraderAssinmentByBettype<T>(List<T> assignment, int bettypeId) where T : TraderAssignment
        => assignment.FirstOrDefault(a => a.BettypeId == bettypeId);
    }
}
