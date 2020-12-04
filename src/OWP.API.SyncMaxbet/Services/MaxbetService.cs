using OWP.API.SyncMaxbet.Builder;
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
        private readonly TraderAssignmentBuilder traderAssignmentBuilder;
        public MaxbetServiceImp(
            SyncMaxbetReposiory syncMaxbetReposiory,
            TraderAssignmentBuilder traderAssignmentBuilder)
        {
            this.syncMaxbetReposiory = syncMaxbetReposiory;
            this.traderAssignmentBuilder = traderAssignmentBuilder;
        }
        public async Task<MaxbetInfo> GetMaxbetInfo(int adminsiteId)
        {
            var owpMatchId = await syncMaxbetReposiory.GetMatchIdByAdminSiteId(adminsiteId);
          
            var traderAssignmentsSender = await traderAssignmentBuilder.BuildTraderAssignmentSender(owpMatchId);
            
            
            return new MaxbetInfo
            {
                AdminsiteId = adminsiteId,
                TraderAssignments = traderAssignmentsSender
            };
        }

        

        public MaxbetSpread BuildSpread(List<SpreadModel> traderAssignments)
        {

            return new MaxbetSpread();
        }

    }
}
