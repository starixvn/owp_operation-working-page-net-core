using OWP.Maxbet.Models;
using OWP.Services;
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

        private readonly LogServiceWrapper logServiceWrapper;

        public SpecialProductTraderBuilderImp(MaxbetReposiory maxbetReposiory, LogServiceWrapper logServiceWrapper)
        {
            this.maxbetReposiory = maxbetReposiory;
            this.logServiceWrapper = logServiceWrapper;
        }

        public async Task<TraderAssignmentSender> BuildTraderXMins(int specialProductId)
        {
            var traders = await maxbetReposiory.GetTraderXMins(specialProductId);
            await logServiceWrapper.Log("=====BuildTraderXMins======", traders);
            var traderHdp = traders?.FirstOrDefault();
            return new TraderAssignmentSender
            {
                Handicap = traderHdp,
                OverUnder = traderHdp
            };
        }
    }
}