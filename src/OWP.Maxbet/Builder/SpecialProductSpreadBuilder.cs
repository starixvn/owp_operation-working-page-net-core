using OWP.Maxbet.Models;
using System.Linq;
using System.Threading.Tasks;

namespace OWP.Maxbet.Builder
{
    public interface SpecialProductSpreadBuilder
    {
        Task<MaxbetSpread> BuildSpreadXMins(int owpMatchId);
    }

    public class SpecialProductSpreadBuilderImp : SpecialProductSpreadBuilder
    {
        private readonly MaxbetReposiory syncMaxbetReposiory;

        public SpecialProductSpreadBuilderImp(MaxbetReposiory syncMaxbetReposiory)
        {
            this.syncMaxbetReposiory = syncMaxbetReposiory;
        }

        public async Task<MaxbetSpread> BuildSpreadXMins(int owpMatchId)
        {
            var spread = await syncMaxbetReposiory.GetSpreadXMins(owpMatchId);
            var spreadHdp = spread?.FirstOrDefault();
            return new()
            {
                Handicap = spreadHdp,
                OverUnder = spreadHdp
            };
        }
    }
}