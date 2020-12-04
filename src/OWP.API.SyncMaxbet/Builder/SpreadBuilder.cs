using OWP.API.SyncMaxbet.Enums;
using OWP.API.SyncMaxbet.Models;
using OWP.Helpers;
using System.Linq;
using System.Threading.Tasks;

namespace OWP.API.SyncMaxbet.Builder
{
    public interface SpreadBuilder
    {
        Task<MaxbetSpread> BuildSpread(int owpMatchId);
    }

    public class SpreadBuilderImp : SpreadBuilder
    {
        private readonly SyncMaxbetReposiory syncMaxbetReposiory;

        public SpreadBuilderImp(SyncMaxbetReposiory syncMaxbetReposiory)
        {
            this.syncMaxbetReposiory = syncMaxbetReposiory;
        }

        public async Task<MaxbetSpread> BuildSpread(int owpMatchId)
        {
            var spread = await syncMaxbetReposiory.GetSpreadByOwpmatchId(owpMatchId);
            if (!spread.Any())
            {
                return new MaxbetSpread();
            }
            // TODO
            return new MaxbetSpread();
        }
    }
}
