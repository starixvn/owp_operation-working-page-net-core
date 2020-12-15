using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanex.Data.Repository;
using OWP.Maxbet.Criteria;
using OWP.Maxbet.Models;

namespace OWP.Maxbet
{
    public interface MaxbetReposiory
    {
        public Task<int> GetMatchIdByAdminSiteId(int adminSiteId);

        public Task<List<TraderAssignment>> GetTradersByOwpMatchId(int owpMatchId);

        public Task<ValueTuple<IEnumerable<SpreadModel>, IEnumerable<LeagueGroup>>> GetSpreadByOwpmatchId(int owpMatchId);

        public Task<IEnumerable<MaxbetModel>> GetMaxbetByOwpMatchId(int matchId);
    }

    public class MaxbetReposioryImp : MaxbetReposiory
    {
        private readonly IDynamicRepository dynamicRepository;

        public MaxbetReposioryImp(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<ValueTuple<IEnumerable<SpreadModel>, IEnumerable<LeagueGroup>>> GetSpreadByOwpmatchId(int owpMatchId)
        {
            var criteria = new SpreadGetCriteria
            {
                OwpMatchId = owpMatchId
            };
            var (firstResultSet, secondResultSet) = await dynamicRepository.FetchMultipleAsync<SpreadModel, LeagueGroup>(criteria);
            var valueTupple = new ValueTuple<IEnumerable<SpreadModel>, IEnumerable<LeagueGroup>>
            {
                Item1 = firstResultSet,
                Item2 = secondResultSet
            };
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