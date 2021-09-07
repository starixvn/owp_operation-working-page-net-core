using OWP.Maxbet.Models;
using System.Linq;
using System.Threading.Tasks;

namespace OWP.Maxbet.Builder
{
    public interface SpecialProductTraderBuilder
    {
        public Task<TraderAssignmentSender> BuildTraderXMins(int specialProductId);
    }

    public class SpecialProductTraderBuilderImp : SpecialProductTraderBuilder
    {
        private readonly MaxbetReposiory maxbetReposiory;

        public SpecialProductTraderBuilderImp(MaxbetReposiory maxbetReposiory)
        {
            this.maxbetReposiory = maxbetReposiory;
        }

        public async Task<TraderAssignmentSender> BuildTraderXMins(int specialProductId)
        {
            var traders = await maxbetReposiory.GetTraderXMins(specialProductId);
            var traderHdp = traders?.FirstOrDefault();
            return new TraderAssignmentSender
            {
                Handicap = traderHdp,
                OverUnder = traderHdp
            };
        }
    }
}
