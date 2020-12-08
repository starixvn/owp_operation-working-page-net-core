using Fanex.Data.Repository;
using OWP.API.SyncMaxbet.Criterias;
using OWP.API.SyncMaxbet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OWP.API.SyncMaxbet
{
    public interface SyncMaxbetReposiory
    {
        public Task<List<TraderAssignment>> GetTradersByOwpMatchId(int owpMatchId);

        public Task<ValueTuple<IEnumerable<SpreadModel>, IEnumerable<int>>> GetSpreadByOwpmatchId(int owpMatchId);

        Task<IEnumerable<Maxbet>> GetMaxbetByOwpMatchId(int matchId);
    }

    public class SyncMaxbetReposioryImp : SyncMaxbetReposiory
    {
        private readonly IDynamicRepository dynamicRepository;

        public SyncMaxbetReposioryImp(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<ValueTuple<IEnumerable<SpreadModel>, IEnumerable<int>>> GetSpreadByOwpmatchId(int owpMatchId)
        {
            var criteria = new SpreadGetCriteria
            {
                OwpMatchId = owpMatchId
            };
            return await dynamicRepository.FetchMultipleAsync<SpreadModel, int>(criteria);
        }

        public async Task<List<TraderAssignment>> GetTradersByOwpMatchId(int owpMatchId)
        {
            var criteria = new TradersGetCriteria
            {
                OwpMatchId = owpMatchId
            };
            return (await dynamicRepository.FetchAsync<TraderAssignment>(criteria)).ToList();
        }

        public async Task<IEnumerable<Maxbet>> GetMaxbetByOwpMatchId(int matchId)
                => await dynamicRepository.FetchAsync<Maxbet>(new MaxbetGetCriteria { OWPMatchId = matchId });
    }
}
