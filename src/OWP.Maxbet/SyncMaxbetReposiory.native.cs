using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanex.Data.Repository;
using OWP.Maxbet.Criterias;
using OWP.Maxbet.Models;

namespace OWP.Maxbet
{
    public interface SyncMaxbetReposiory
    {
        public Task<int> GetMatchIdByAdminSiteId(int adminSiteId);

        public Task<List<TraderAssignment>> GetTradersByOwpMatchId(int owpMatchId);

        public Task<ValueTuple<IEnumerable<SpreadModel>, IEnumerable<int?>>> GetSpreadByOwpmatchId(int owpMatchId);

        public Task<IEnumerable<MaxbetModel>> GetMaxbetByOwpMatchId(int matchId);
    }

    public class SyncMaxbetReposioryImp : SyncMaxbetReposiory
    {
        private readonly IDynamicRepository dynamicRepository;

        public SyncMaxbetReposioryImp(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<ValueTuple<IEnumerable<SpreadModel>, IEnumerable<int?>>> GetSpreadByOwpmatchId(int owpMatchId)
        {
            var criteria = new SpreadGetCriteria
            {
                OwpMatchId = owpMatchId
            };
            var result= await dynamicRepository.FetchMultipleAsync<SpreadModel, int?>(criteria);
            var valueTupple = new ValueTuple<IEnumerable<SpreadModel>, IEnumerable<int?>>();
            valueTupple.Item1 = result.firstResultSet;
            valueTupple.Item2 = result.secondResultSet;
            return valueTupple;
        }

        public async Task<List<TraderAssignment>> GetTradersByOwpMatchId(int owpMatchId)
        {
            var criteria = new TradersGetCriteria
            {
                OwpMatchId = owpMatchId
            };
            return (await dynamicRepository.FetchAsync<TraderAssignment>(criteria)).ToList();
        }

        public async Task<int> GetMatchIdByAdminSiteId(int adminSiteId)
        {
            var criteria = new MatchIdGetCriteria
            {
                AdminsiteId = adminSiteId
            };

            var matchId = (await dynamicRepository.FetchAsync<int>(criteria)).FirstOrDefault();
            return matchId;
        }

        public async Task<IEnumerable<MaxbetModel>> GetMaxbetByOwpMatchId(int matchId)
                => await dynamicRepository.FetchAsync<MaxbetModel>(new MaxbetGetCriteria { OWPMatchId = matchId });
    }
}