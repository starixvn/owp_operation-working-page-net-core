using Fanex.Data.Repository;
using OWP.API.SyncMaxbet.Criterias;
using OWP.API.SyncMaxbet.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OWP.API.SyncMaxbet
{
    public interface SyncMaxbetReposiory
    {
        public Task<int> GetMatchIdByAdminSiteId(int adminSiteId);

        public Task<List<TraderAssignment>> GetTradersByOwpMatchId(int owpMatchId);
    }

    public class SyncMaxbetReposioryImp : SyncMaxbetReposiory
    {
        private readonly IDynamicRepository dynamicRepository;

        public SyncMaxbetReposioryImp(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<int> GetMatchIdByAdminSiteId(int adminSiteId)
        {
            var criteria = new MatchIdGetCriteria
            {
                AdminsiteId = adminSiteId
            };

            int matchId = (await dynamicRepository.FetchAsync<int>(criteria)).FirstOrDefault();
            return matchId;
        }

        public async Task<List<TraderAssignment>> GetTradersByOwpMatchId(int owpMatchId)
        {
            var criteria = new TradersGetCriteria
            {
                OwpMatchId = owpMatchId
            };
            return (await dynamicRepository.FetchAsync<TraderAssignment>(criteria)).ToList();
        }
    }
}
