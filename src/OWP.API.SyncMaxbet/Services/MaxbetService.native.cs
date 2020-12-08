using OWP.API.SyncMaxbet.Builder;
using OWP.API.SyncMaxbet.Models;
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

        private readonly TraderAssignmentBuilder traderAssignmentBuilder;

        private readonly SpreadBuilder spreadBuilder;

        private readonly MaxbetBuilder maxbetBuilder;

        public MaxbetServiceImp(
            SyncMaxbetReposiory syncMaxbetReposiory,
            TraderAssignmentBuilder traderAssignmentBuilder,
            SpreadBuilder spreadBuilder,
            MaxbetBuilder maxbetBuilder)
        {
            this.syncMaxbetReposiory = syncMaxbetReposiory;
            this.traderAssignmentBuilder = traderAssignmentBuilder;
            this.spreadBuilder = spreadBuilder;
            this.maxbetBuilder = maxbetBuilder;
        }
        public async Task<MaxbetInfo> GetMaxbetInfo(int adminsiteId)
        {
            var owpMatchId = await syncMaxbetReposiory.GetMatchIdByAdminSiteId(adminsiteId);

            var traderAssignmentsSender = await traderAssignmentBuilder.BuildTraderAssignmentSender(owpMatchId);

            var spread = await spreadBuilder.BuildSpread(owpMatchId);

            var maxbet = await maxbetBuilder.BuildMaxbet(owpMatchId);

            return new MaxbetInfo
            {
                OwpMatchId = owpMatchId,
                TraderAssignments = traderAssignmentsSender,
                Spread = spread,
                Maxbet = maxbet
            };
        }
    }
}
