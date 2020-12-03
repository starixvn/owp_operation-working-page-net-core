using Fanex.Data.Repository;
using OWP.API.SyncMaxbet.Criterias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWP.API.SyncMaxbet
{


    public interface SyncMaxbetReposiory
    {
        public Task<int> GetMatchIdByAdminSiteId(int adminSiteId);
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
    }
}
