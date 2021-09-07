using OWP.Maxbet.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OWP.Maxbet.Builder
{
    public interface SpecialProductMaxbetBuilder
    {
        Task<MaxbetSpread> BuildMaxbet(int owpMatchId);

        Task<MaxbetSpread> BuildMaxbetXMins(int owpMatchId);
    }

    public class SpecialProductMaxbetBuilderImp : SpecialProductMaxbetBuilder
    {
        private readonly MaxbetReposiory syncMaxbetReposiory;

        public SpecialProductMaxbetBuilderImp(MaxbetReposiory syncMaxbetReposiory)
        {
            this.syncMaxbetReposiory = syncMaxbetReposiory;
        }

        public async Task<MaxbetSpread> BuildMaxbet(int owpMatchId)
        {
            var maxbet = await syncMaxbetReposiory.GetMaxbetByOwpMatchId(owpMatchId);
            return !maxbet.Any()
                ? new()
                : MaxbetBuilderImp.BuildMaxbetSender(maxbet.ToList());
        }

        public async Task<MaxbetSpread> BuildMaxbetXMins(int owpMatchId)
        {
            var maxbet = await syncMaxbetReposiory.GetMaxbetXMins(owpMatchId);
            var maxbetHdp = maxbet?.FirstOrDefault();
            return new()
            {
                Handicap = maxbetHdp,
                OverUnder = maxbetHdp
            };
        }
    }
}
